using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.ScorchlingBomb
{
    static class ScorchlingBombFix
    {
        public static void Init()
        {
            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_DLC2_Scorchling.LavaBombHeatOrbGhost_prefab)).CallOnSuccess(lavaBombHeatOrbGhost =>
            {
                foreach (AnimateShaderAlpha animateShaderAlpha in lavaBombHeatOrbGhost.GetComponentsInChildren<AnimateShaderAlpha>(true))
                {
                    animateShaderAlpha.continueExistingAfterTimeMaxIsReached = true;
                }
            });
        }
    }
}
