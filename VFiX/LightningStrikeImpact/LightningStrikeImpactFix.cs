using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.LightningStrikeImpact
{
    static class LightningStrikeImpactFix
    {
        public static void Init()
        {
            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_Base_Lightning.LightningStrikeImpact_prefab)).CallOnSuccess(lightningStrikeImpactPrefab =>
            {
                if (!lightningStrikeImpactPrefab.TryGetComponent(out EffectComponent effectComponent))
                {
                    Log.Error("Lightning strike impact is missing EffectComponent");
                    return;
                }

                if (string.IsNullOrEmpty(effectComponent.soundName))
                {
                    effectComponent.soundName = "Play_item_use_lighningArm";
                }
            });

            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_Base_LightningStrikeOnHit.SimpleLightningStrikeImpact_prefab)).CallOnSuccess(simpleLightningStrikeImpactPrefab =>
            {
                if (!simpleLightningStrikeImpactPrefab.TryGetComponent(out EffectComponent effectComponent))
                {
                    Log.Error("Simple lightning strike impact is missing EffectComponent");
                    return;
                }

                if (string.IsNullOrEmpty(effectComponent.soundName))
                {
                    effectComponent.soundName = "Play_item_use_lighningArm";
                }
            });
        }
    }
}
