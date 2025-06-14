using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.GrandparentPreSpawnEffect
{
    static class GrandparentPreSpawnEffectFix
    {
        public static void Init()
        {
            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_Base_Grandparent.GrandparentPreSpawnEffect_prefab)).CallOnSuccess(grandparentPreSpawnEffect =>
            {
                if (grandparentPreSpawnEffect.TryGetComponent(out VFXAttributes vfxAttributes))
                {
                    vfxAttributes.DoNotPool = true;
                }
            });
        }
    }
}
