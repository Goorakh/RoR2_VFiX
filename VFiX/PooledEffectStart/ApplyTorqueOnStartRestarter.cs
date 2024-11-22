using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class ApplyTorqueOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public ApplyTorqueOnStart ApplyTorqueOnStart;

        void Awake()
        {
            if (!_restarter)
            {
                _restarter = GetComponentInParent<EffectRestarterController>();
            }

            if (_restarter)
            {
                _restarter.OnReset += reset;
            }

            if (!ApplyTorqueOnStart)
            {
                ApplyTorqueOnStart = GetComponent<ApplyTorqueOnStart>();
            }
        }

        void OnDestroy()
        {
            if (_restarter)
            {
                _restarter.OnReset -= reset;
            }
        }

        void reset()
        {
            if (ApplyTorqueOnStart)
            {
                ApplyTorqueOnStart.Start();
            }
        }
    }
}
