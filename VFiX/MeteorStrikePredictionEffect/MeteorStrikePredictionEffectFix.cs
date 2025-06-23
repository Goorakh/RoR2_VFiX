using RoR2;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;

namespace VFiX.MeteorStrikePredictionEffect
{
    static class MeteorStrikePredictionEffectFix
    {
        public static void Init()
        {
            AssetLoadUtils.TemporaryPreload<GameObject>(RoR2_Base_Meteor.MeteorStrikePredictionEffect_prefab, meteorStrikePredictionEffect =>
            {
                Transform groundSlamIndicator = meteorStrikePredictionEffect.transform.Find("GroundSlamIndicator");
                if (!groundSlamIndicator)
                {
                    Log.Error("Failed to find GroundSlamIndicator");
                    return;
                }

                if (!groundSlamIndicator.TryGetComponent(out AnimateShaderAlpha animateShaderAlpha))
                {
                    Log.Error($"{groundSlamIndicator.name} is missing AnimateShaderAlpha component");
                    return;
                }

                animateShaderAlpha.continueExistingAfterTimeMaxIsReached = true;
            });
        }
    }
}
