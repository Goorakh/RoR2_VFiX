using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.GrandparentPreSpawnEffect
{
    static class GrandparentPreSpawnEffectFix
    {
        public static void Init()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandparentPreSpawnEffect.prefab").CallOnSuccess(grandparentPreSpawnEffect =>
            {
                if (grandparentPreSpawnEffect.TryGetComponent(out VFXAttributes vfxAttributes))
                {
                    vfxAttributes.DoNotPool = true;
                }
            });
        }
    }
}
