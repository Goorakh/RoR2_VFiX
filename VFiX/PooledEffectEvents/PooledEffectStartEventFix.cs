using HG;
using RoR2;
using RoR2BepInExPack.GameAssetPathsBetter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VFiX.PooledEffectEvents
{
    static class PooledEffectStartEventFix
    {
        [SystemInitializer]
        static void Init()
        {
            static void convertStartEventsToEnable(string assetGuid)
            {
                AssetLoadUtils.LoadTempAssetAsync<GameObject>(assetGuid).CallOnSuccess(transferStartEventsToEnable);
            }

            convertStartEventsToEnable(RoR2_Base_Captain.CaptainAirstrikeGhost1_prefab);
        }

        static void transferStartEventsToEnable(GameObject obj)
        {
            if (obj.TryGetComponent(out StartEvent startEvent) && startEvent.action != null)
            {
                List<BaseInvokableCall> calls = [];
                List<PersistentCall> persistentCalls = [];

                if (startEvent.action.m_Calls?.m_ExecutingCalls != null)
                {
                    foreach (BaseInvokableCall call in startEvent.action.m_Calls.m_ExecutingCalls)
                    {
                        calls.Add(call);
                    }
                }

                if (startEvent.action.m_PersistentCalls != null)
                {
                    foreach (PersistentCall persistentCall in startEvent.action.m_PersistentCalls.GetListeners())
                    {
                        persistentCalls.Add(persistentCall);
                    }
                }

                if (calls.Count > 0 || persistentCalls.Count > 0)
                {
                    OnEnableEvent onEnableEvent = obj.EnsureComponent<OnEnableEvent>();
                    onEnableEvent.action ??= new UnityEvent();

                    foreach (BaseInvokableCall call in calls)
                    {
                        onEnableEvent.action.AddCall(call);
                    }

                    foreach (PersistentCall persistentCall in persistentCalls)
                    {
                        onEnableEvent.action.m_PersistentCalls.AddListener(persistentCall);
                    }

                    startEvent.enabled = false;

                    Log.Debug($"Transferred {calls.Count + persistentCalls.Count} StartEvent calls to OnEnableEvent on {Util.GetGameObjectHierarchyName(obj)}");
                }
            }
        }
    }
}
