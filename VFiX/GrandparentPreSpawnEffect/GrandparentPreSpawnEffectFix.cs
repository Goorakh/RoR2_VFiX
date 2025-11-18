using RoR2;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;

namespace VFiX.GrandparentPreSpawnEffect
{
    static class GrandparentPreSpawnEffectFix
    {
        public static void Init()
        {
            AssetLoadUtils.LoadTempAssetAsync<GameObject>(RoR2_Base_Grandparent.GrandparentPreSpawnEffect_prefab).CallOnSuccess(grandparentPreSpawnEffect =>
            {
                if (grandparentPreSpawnEffect.TryGetComponent(out VFXAttributes vfxAttributes))
                {
                    vfxAttributes.DoNotPool = true;
                }
            });
        }
    }
}
