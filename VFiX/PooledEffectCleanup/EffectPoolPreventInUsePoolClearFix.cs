using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VFiX.PooledEffectCleanup
{
    // Remove >30 pool count check that bypasses the "in-use" check for some reason,
    // which causes effects to be removed while active
    static class EffectPoolPreventInUsePoolClearFix
    {
        public static void Init()
        {
            IL.RoR2.EffectManager.UnloadOutdatedPools += EffectManager_UnloadOutdatedPools;
        }

        static void EffectManager_UnloadOutdatedPools(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            PropertyInfo effectPrefabMapCount = typeof(Dictionary<GameObject, EffectPool>).GetProperty("Count");
            if (effectPrefabMapCount == null)
            {
                Log.Error("Failed to find Dictionary Count property");
                return;
            }

            if (c.TryGotoNext(MoveType.After,
                              x => x.MatchCallOrCallvirt(effectPrefabMapCount.GetMethod),
                              x => x.MatchLdcI4(out _)))
            {
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, int.MaxValue);
            }
        }
    }
}
