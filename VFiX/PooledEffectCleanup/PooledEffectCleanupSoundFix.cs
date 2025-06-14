using RoR2;

namespace VFiX.PooledEffectCleanup
{
    static class PooledEffectCleanupSoundFix
    {
        public static void Init()
        {
            On.RoR2.EffectPool.PreDestroyObject += EffectPool_PreDestroyObject;
        }

        static void EffectPool_PreDestroyObject(On.RoR2.EffectPool.orig_PreDestroyObject orig, EffectPool self, EffectManagerHelper inKillObject)
        {
            orig(self, inKillObject);

            // Prevent OnDestroy events from triggering when clearing pools
            if (inKillObject && inKillObject.TryGetComponent(out AkTriggerHandler akTriggerHandler))
            {
                akTriggerHandler.triggerList.Clear();
            }
        }
    }
}
