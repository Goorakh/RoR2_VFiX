using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.MeteorStrikePredictionEffect
{
    static class MeteorStrikePredictionEffectFix
    {
        public static void Init()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Meteor/MeteorStrikePredictionEffect.prefab").CallOnSuccess(meteorStrikePredictionEffect =>
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
