using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFiX.MeteorStrikePredictionEffect
{
    static class MeteorStrikePredictionEffectFix
    {
        public static void Init()
        {
            AssetAsyncReferenceManager<GameObject>.LoadAsset(new AssetReferenceT<GameObject>(RoR2_Base_Meteor.MeteorStrikePredictionEffect_prefab)).CallOnSuccess(meteorStrikePredictionEffect =>
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
