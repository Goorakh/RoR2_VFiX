using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class ExplodeRigidbodiesOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public ExplodeRigidbodiesOnStart ExplodeRigidbodiesOnStart;

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

            if (!ExplodeRigidbodiesOnStart)
            {
                ExplodeRigidbodiesOnStart = GetComponent<ExplodeRigidbodiesOnStart>();
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
            if (ExplodeRigidbodiesOnStart)
            {
                ExplodeRigidbodiesOnStart.Start();
            }
        }
    }
}
