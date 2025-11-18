using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class ApplyTorqueOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        ApplyTorqueOnStart _applyTorqueOnStart;

        void Awake()
        {
            _applyTorqueOnStart = GetComponent<ApplyTorqueOnStart>();
        }

        void IEffectRestarter.Restart()
        {
            if (_applyTorqueOnStart)
            {
                _applyTorqueOnStart.Start();
            }
        }
    }
}
