using HG.Coroutines;
using RoR2;
using RoR2.ContentManagement;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace VFiX.DitherFix
{
    static class ModelDitherFix
    {
        static readonly int _FadeOn = Shader.PropertyToID("_FadeOn");

        [SystemInitializer(typeof(SkinCatalog), typeof(BodyCatalog), typeof(ItemDisplayRuleSet))]
        static void Init()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            HashSet<string> loadedAssets = [];
            HashSet<Material> handledMaterials = [];
            int numFixedMaterials = 0;

            ParallelCoroutine loadCoroutine = new ParallelCoroutine();

            for (SkinIndex skinIndex = 0; (int)skinIndex < SkinCatalog.skinCount; skinIndex++)
            {
                SkinDef skinDef = SkinCatalog.GetSkinDef(skinIndex);

                if (skinDef)
                {
                    (AssetReferenceT<SkinDefParams> skinParamsReference, SkinDefParams skinParams) = skinDef.GetSkinParams();
                    if (skinParams)
                    {
                        loadCoroutine.Add(handleSkinParamsAsync(skinParams));
                    }
                    else if (skinParamsReference != null && skinParamsReference.RuntimeKeyIsValid())
                    {
                        if (loadedAssets.Add(skinParamsReference.RuntimeKey.ToString()))
                        {
                            loadCoroutine.Add(loadSkinParamsAsync(skinParamsReference));

                            IEnumerator loadSkinParamsAsync(AssetReferenceT<SkinDefParams> skinParamsReference)
                            {
                                AsyncOperationHandle<SkinDefParams> skinParamsLoad = AssetAsyncReferenceManager<SkinDefParams>.LoadAsset(skinParamsReference);
                                yield return skinParamsLoad;

                                if (skinParamsLoad.Status == AsyncOperationStatus.Succeeded && skinParamsLoad.Result)
                                {
                                    yield return handleSkinParamsAsync(skinParamsLoad.Result);
                                }

                                AssetAsyncReferenceManager<SkinDefParams>.UnloadAsset(skinParamsReference);
                            }
                        }
                    }
                }

                IEnumerator handleSkinParamsAsync(SkinDefParams skinDefParams)
                {
                    return handleRendererInfosAsync(skinDefParams.rendererInfos);
                }
            }

            foreach (ItemDisplayRuleSet itemDisplayRuleSet in ItemDisplayRuleSet.instancesList)
            {
                foreach (ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup in itemDisplayRuleSet.keyAssetRuleGroups)
                {
                    if (!keyAssetRuleGroup.displayRuleGroup.isEmpty)
                    {
                        foreach (ItemDisplayRule rule in keyAssetRuleGroup.displayRuleGroup.rules)
                        {
                            if (rule.followerPrefab)
                            {
                                loadCoroutine.Add(handleDisplayPrefabAsync(rule.followerPrefab));
                            }
                            else if (rule.followerPrefabAddress != null && rule.followerPrefabAddress.RuntimeKeyIsValid())
                            {
                                if (loadedAssets.Add(rule.followerPrefabAddress.RuntimeKey.ToString()))
                                {
                                    loadCoroutine.Add(loadDisplayPrefabAsync(rule.followerPrefabAddress));

                                    IEnumerator loadDisplayPrefabAsync(AssetReferenceT<GameObject> displayPrefabReference)
                                    {
                                        AsyncOperationHandle<GameObject> displayPrefabLoad = AssetAsyncReferenceManager<GameObject>.LoadAsset(displayPrefabReference);
                                        yield return displayPrefabLoad;

                                        if (displayPrefabLoad.Status == AsyncOperationStatus.Succeeded && displayPrefabLoad.Result)
                                        {
                                            yield return handleDisplayPrefabAsync(displayPrefabLoad.Result);
                                        }

                                        AssetAsyncReferenceManager<GameObject>.UnloadAsset(displayPrefabReference);
                                    }
                                }
                            }

                            IEnumerator handleDisplayPrefabAsync(GameObject displayPrefab)
                            {
                                CharacterModel.RendererInfo[] rendererInfos = [];
                                if (displayPrefab.TryGetComponent(out ItemDisplay itemDisplay) &&
                                    itemDisplay.rendererInfos != null && itemDisplay.rendererInfos.Length > 0)
                                {
                                    rendererInfos = itemDisplay.rendererInfos;
                                }

                                if (rendererInfos.Length > 0)
                                {
                                    yield return handleRendererInfosAsync(itemDisplay.rendererInfos);
                                }
                            }
                        }
                    }
                }
            }

            IEnumerator handleRendererInfosAsync(IEnumerable<CharacterModel.RendererInfo> rendererInfos)
            {
                ParallelCoroutine loadMaterialsCoroutine = new ParallelCoroutine();

                foreach (CharacterModel.RendererInfo rendererInfo in rendererInfos)
                {
                    if (rendererInfo.defaultMaterial)
                    {
                        handleMaterial(rendererInfo.defaultMaterial);
                    }
                    else if (rendererInfo.defaultMaterialAddress != null && rendererInfo.defaultMaterialAddress.RuntimeKeyIsValid())
                    {
                        if (loadedAssets.Add(rendererInfo.defaultMaterialAddress.RuntimeKey.ToString()))
                        {
                            loadMaterialsCoroutine.Add(loadMaterialAsync(rendererInfo.defaultMaterialAddress));
                        }
                    }
                }

                return loadMaterialsCoroutine;
            }

            IEnumerator loadMaterialAsync(AssetReferenceT<Material> materialReference)
            {
                AsyncOperationHandle<Material> materialLoad = AssetAsyncReferenceManager<Material>.LoadAsset(materialReference);
                yield return materialLoad;

                if (materialLoad.Status == AsyncOperationStatus.Succeeded && materialLoad.Result)
                {
                    handleMaterial(materialLoad.Result);
                }

                AssetAsyncReferenceManager<Material>.UnloadAsset(materialReference);
            }

            void handleMaterial(Material material)
            {
                if (!material)
                    return;

                Shader shader = material.shader;
                if (!shader)
                    return;

                if (!handledMaterials.Add(material))
                    return;

                bool fixedDither = false;
                if (shader.keywordSpace.FindKeyword("DITHER").isValid)
                {
                    if (!material.IsKeywordEnabled("DITHER"))
                    {
                        material.EnableKeyword("DITHER");
                        fixedDither = true;

                        Log.Debug($"Enabled dither keyword for {material.name}");
                    }
                }

                if (material.HasInt(_FadeOn))
                {
                    if (material.GetInt(_FadeOn) == 0)
                    {
                        material.SetInt(_FadeOn, 1);
                        fixedDither = true;

                        Log.Debug($"Enabled dither setting for {material.name}");
                    }
                }

                if (fixedDither)
                {
                    numFixedMaterials++;
                }
            }

            IEnumerator runLoadCoroutine()
            {
                yield return loadCoroutine;

                Log.Info($"Fixed dithering for {numFixedMaterials}/{handledMaterials.Count} material(s) ({stopwatch.Elapsed.TotalMilliseconds:0}ms)");
                stopwatch.Stop();

                handledMaterials.Clear();
                loadedAssets.Clear();
            }

            RoR2Application.instance.StartCoroutine(runLoadCoroutine());
        }
    }
}
