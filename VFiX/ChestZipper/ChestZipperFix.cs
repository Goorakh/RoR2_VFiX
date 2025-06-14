using HG;
using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.ChestZipper
{
    static class ChestZipperFix
    {
        public static void Init()
        {
            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_Base_Chest1.ChestUnzip_prefab)).CallOnSuccess(chestUnzip =>
            {
                chestUnzip.EnsureComponent<ChestZipperController>();

                Transform sparks = chestUnzip.transform.Find("Sparks (1)");
                if (sparks)
                {
                    sparks.gameObject.SetActive(true);
                }
            });

            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_Base_GoldChest.GoldChestUnzip_prefab)).CallOnSuccess(goldChestUnzip =>
            {
                goldChestUnzip.EnsureComponent<ChestZipperController>();

                Transform sparks = goldChestUnzip.transform.Find("Sparks (1)");
                if (sparks)
                {
                    sparks.gameObject.SetActive(true);
                }
            });

            static void fixChestZipperReferences(string chestPrefabAssetGuid, string[] zipperPaths)
            {
                AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(chestPrefabAssetGuid)).CallOnSuccess(chestPrefab =>
                {
                    if (!chestPrefab.TryGetComponent(out ModelLocator modelLocator))
                    {
                        Log.Error($"{chestPrefab} is missing model locator");
                        return;
                    }

                    Transform modelTransform = modelLocator.modelTransform;
                    if (!modelTransform)
                    {
                        Log.Error($"{chestPrefab} is missing model transform");
                        return;
                    }

                    if (!modelTransform.TryGetComponent(out ChildLocator childLocator))
                    {
                        Log.Error($"{modelTransform} is missing child locator");
                        return;
                    }

                    bool tryAssignZipperReference(int childIndex, string modelPath)
                    {
                        ref ChildLocator.NameTransformPair transformPair = ref childLocator.transformPairs[childIndex];
                        if (transformPair.transform)
                            return false;

                        Transform newReference = modelTransform.Find(modelPath);
                        if (!newReference)
                        {
                            Log.Warning($"Failed to find {modelPath} relative to {modelTransform} ({chestPrefab})");
                            return false;
                        }

                        transformPair.transform = newReference;
                        return true;
                    }

                    int numAddedZipperReferences = 0;

                    for (int i = 0; i < zipperPaths.Length; i++)
                    {
                        int childIndex = childLocator.FindChildIndex($"Zipper{i + 1}");
                        if (childIndex != -1)
                        {
                            if (tryAssignZipperReference(childIndex, zipperPaths[i]))
                            {
                                numAddedZipperReferences++;
                            }
                        }
                    }

                    if (numAddedZipperReferences == 0)
                    {
                        Log.Error($"Failed to fix zipper references for {chestPrefab}");
                    }
                    else
                    {
                        Log.Debug($"Fixed {numAddedZipperReferences} zipper references for {chestPrefab}");
                    }
                });
            }

            string[] categoryChestZipperPaths = ["CategoryChestArmature/zipper1", "CategoryChestArmature/zipper1.001"];
            fixChestZipperReferences(RoR2_Base_CategoryChest.CategoryChestDamage_prefab, categoryChestZipperPaths);
            fixChestZipperReferences(RoR2_Base_CategoryChest.CategoryChestHealing_prefab, categoryChestZipperPaths);
            fixChestZipperReferences(RoR2_Base_CategoryChest.CategoryChestUtility_prefab, categoryChestZipperPaths);

            AssetAsyncReferenceManager<RuntimeAnimatorController>.LoadAsset(new AssetReferenceT<RuntimeAnimatorController>(RoR2_Base_CategoryChest.animCategoryChest_controller)).CallOnSuccess(categoryChestAnimatorController =>
            {
                AnimationClip[] animationClips = categoryChestAnimatorController.animationClips;
                for (int i = 0; i < animationClips.Length; i++)
                {
                    if (animationClips[i].name != "CategoryChestArmature|Open")
                        continue;

                    AnimationEvent[] events = animationClips[i].events;
                    bool anyChanged = false;

                    foreach (AnimationEvent animationEvent in events)
                    {
                        if (animationEvent.functionName == nameof(AnimationEvents.CreatePrefab) &&
                            animationEvent.stringParameter.Contains("Zipper", StringComparison.OrdinalIgnoreCase))
                        {
                            // Fix 'parent' flag for chest zipper
                            if (animationEvent.intParameter == 0)
                            {
                                animationEvent.intParameter = 1;
                                anyChanged = true;
                            }
                        }
                    }

                    if (anyChanged)
                    {
                        animationClips[i].events = events;
                    }
                }
            });

            On.RoR2.ChestBehavior.Awake += ChestBehavior_Awake;
            On.EntityStates.Barrel.Opened.OnEnter += Opened_OnEnter;
        }

        static void ChestBehavior_Awake(On.RoR2.ChestBehavior.orig_Awake orig, ChestBehavior self)
        {
            orig(self);
            self.gameObject.EnsureComponent<ChestZipperTracker>();
        }

        static void Opened_OnEnter(On.EntityStates.Barrel.Opened.orig_OnEnter orig, EntityStates.Barrel.Opened self)
        {
            orig(self);

            ChestZipperTracker chestZipperTracker = self.GetComponent<ChestZipperTracker>();
            if (chestZipperTracker)
            {
                chestZipperTracker.DestroyAllTrackedZippers();
            }
        }
    }
}
