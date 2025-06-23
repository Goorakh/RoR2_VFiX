using RoR2;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;

namespace VFiX.ScorchlingBomb
{
    static class ScorchlingBombFix
    {
        public static void Init()
        {
            AssetLoadUtils.TemporaryPreload<GameObject>(RoR2_DLC2_Scorchling.LavaBombHeatOrbGhost_prefab, lavaBombHeatOrbGhost =>
            {
                foreach (AnimateShaderAlpha animateShaderAlpha in lavaBombHeatOrbGhost.GetComponentsInChildren<AnimateShaderAlpha>(true))
                {
                    animateShaderAlpha.continueExistingAfterTimeMaxIsReached = true;
                }
            });
        }
    }
}
