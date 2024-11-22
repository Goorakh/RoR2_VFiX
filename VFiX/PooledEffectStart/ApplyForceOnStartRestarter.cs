using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class ApplyForceOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public ApplyForceOnStart ApplyForceOnStart;

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

            if (!ApplyForceOnStart)
            {
                ApplyForceOnStart = GetComponent<ApplyForceOnStart>();
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
            if (ApplyForceOnStart)
            {
                ApplyForceOnStart.Start();
            }
        }
    }
}
