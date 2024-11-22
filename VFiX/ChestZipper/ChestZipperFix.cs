using RoR2;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.ChestZipper
{
    static class ChestZipperFix
    {
        public static void Init()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Chest1/ChestUnzip.prefab").CallOnSuccess(chestUnzip =>
            {
                chestUnzip.AddComponent<ChestZipperController>();

                Transform sparks = chestUnzip.transform.Find("Sparks (1)");
                if (sparks)
                {
                    sparks.gameObject.SetActive(true);
                }
            });

            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GoldChest/GoldChestUnzip.prefab").CallOnSuccess(goldChestUnzip =>
            {
                goldChestUnzip.AddComponent<ChestZipperController>();

                Transform sparks = goldChestUnzip.transform.Find("Sparks (1)");
                if (sparks)
                {
                    sparks.gameObject.SetActive(true);
                }
            });

            static void fixChestZipperReferences(string assetPath, string[] zipperPaths)
            {
                Addressables.LoadAssetAsync<GameObject>(assetPath).CallOnSuccess(prefab =>
                {
                    if (!prefab.TryGetComponent(out ModelLocator modelLocator))
                    {
                        Log.Error($"{prefab} is missing model locator");
                        return;
                    }

                    Transform modelTransform = modelLocator.modelTransform;
                    if (!modelTransform)
                    {
                        Log.Error($"{prefab} is missing model transform");
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
                            Log.Warning($"Failed to find {modelPath} relative to {modelTransform} ({prefab})");
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
                        Log.Error($"Failed to fix zipper references for {prefab}");
                    }
                    else
                    {
                        Log.Debug($"Fixed {numAddedZipperReferences} zipper references for {prefab}");
                    }
                });
            }

            string[] categoryChestZipperPaths = ["CategoryChestArmature/zipper1", "CategoryChestArmature/zipper1.001"];
            fixChestZipperReferences("RoR2/Base/CategoryChest/CategoryChestDamage.prefab", categoryChestZipperPaths);
            fixChestZipperReferences("RoR2/Base/CategoryChest/CategoryChestHealing.prefab", categoryChestZipperPaths);
            fixChestZipperReferences("RoR2/Base/CategoryChest/CategoryChestUtility.prefab", categoryChestZipperPaths);

            Addressables.LoadAssetAsync<RuntimeAnimatorController>("RoR2/Base/CategoryChest/animCategoryChest.controller").CallOnSuccess(categoryChestAnimatorController =>
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
            self.gameObject.AddComponent<ChestZipperTracker>();
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
