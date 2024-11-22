using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.LightningStrikeImpact
{
    static class LightningStrikeImpactFix
    {
        public static void Init()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lightning/LightningStrikeImpact.prefab").CallOnSuccess(lightningStrikeImpactPrefab =>
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

            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LightningStrikeOnHit/SimpleLightningStrikeImpact.prefab").CallOnSuccess(simpleLightningStrikeImpactPrefab =>
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
