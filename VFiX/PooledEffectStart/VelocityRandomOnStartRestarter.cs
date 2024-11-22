using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class VelocityRandomOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public VelocityRandomOnStart VelocityRandomOnStart;

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

            if (!VelocityRandomOnStart)
            {
                VelocityRandomOnStart = GetComponent<VelocityRandomOnStart>();
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
            if (VelocityRandomOnStart)
            {
                VelocityRandomOnStart.Start();
            }
        }
    }
}
