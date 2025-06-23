using RoR2;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;

namespace VFiX.LightningStrikeImpact
{
    static class LightningStrikeImpactFix
    {
        public static void Init()
        {
            AssetLoadUtils.TemporaryPreload<GameObject>(RoR2_Base_Lightning.LightningStrikeImpact_prefab, lightningStrikeImpactPrefab =>
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

            AssetLoadUtils.TemporaryPreload<GameObject>(RoR2_Base_LightningStrikeOnHit.SimpleLightningStrikeImpact_prefab, simpleLightningStrikeImpactPrefab =>
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
