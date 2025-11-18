using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class ApplyForceOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        ApplyForceOnStart _applyForceOnStart;

        void Awake()
        {
            _applyForceOnStart = GetComponent<ApplyForceOnStart>();
        }

        void IEffectRestarter.Restart()
        {
            if (_applyForceOnStart)
            {
                _applyForceOnStart.Start();
            }
        }
    }
}
