using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectParent
{
    static class PooledEffectParentFix
    {
        public static void Init()
        {
            On.RoR2.EffectManager.GetPooledEffect_GameObject_Vector3_Quaternion_Transform += EffectManager_GetPooledEffect_GameObject_Vector3_Quaternion_Transform;
            On.RoR2.EffectManagerHelper.OnReturnToPool += EffectManagerHelper_OnReturnToPool;
        }

        static EffectManagerHelper EffectManager_GetPooledEffect_GameObject_Vector3_Quaternion_Transform(On.RoR2.EffectManager.orig_GetPooledEffect_GameObject_Vector3_Quaternion_Transform orig, GameObject inPrefab, Vector3 inPosition, Quaternion inRotation, Transform inTransform)
        {
            EffectManagerHelper pooledEffect = orig(inPrefab, inPosition, inRotation, inTransform);

            if (pooledEffect && inTransform)
            {
                pooledEffect.transform.SetParent(inTransform, true);
            }

            return pooledEffect;
        }

        static void EffectManagerHelper_OnReturnToPool(On.RoR2.EffectManagerHelper.orig_OnReturnToPool orig, EffectManagerHelper self)
        {
            orig(self);

            self.transform.SetParent(null, true);
        }
    }
}
