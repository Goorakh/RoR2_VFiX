using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class RTPCControllerRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public RTPCController RTPCController;

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

            if (!RTPCController)
            {
                RTPCController = GetComponent<RTPCController>();
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
            if (RTPCController)
            {
                RTPCController.fixedAge = 0f;
                RTPCController.Start();
            }
        }
    }
}
