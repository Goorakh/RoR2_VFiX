using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.ScorchlingBomb
{
    static class ScorchlingBombFix
    {
        public static void Init()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Scorchling/LavaBombHeatOrbGhost.prefab").CallOnSuccess(lavaBombHeatOrbGhost =>
            {
                foreach (AnimateShaderAlpha animateShaderAlpha in lavaBombHeatOrbGhost.GetComponentsInChildren<AnimateShaderAlpha>(true))
                {
                    animateShaderAlpha.continueExistingAfterTimeMaxIsReached = true;
                }
            });
        }
    }
}
