using RoR2;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;

namespace VFiX.GrandparentPreSpawnEffect
{
    static class GrandparentPreSpawnEffectFix
    {
        public static void Init()
        {
            AssetLoadUtils.TemporaryPreload<GameObject>(RoR2_Base_Grandparent.GrandparentPreSpawnEffect_prefab, grandparentPreSpawnEffect =>
            {
                if (grandparentPreSpawnEffect.TryGetComponent(out VFXAttributes vfxAttributes))
                {
                    vfxAttributes.DoNotPool = true;
                }
            });
        }
    }
}
