using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class SetRandomRotationRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public SetRandomRotation SetRandomRotation;

        Vector3 _originalEulerAngles;

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

            if (!SetRandomRotation)
            {
                SetRandomRotation = GetComponent<SetRandomRotation>();
            }

            _originalEulerAngles = transform.localEulerAngles;
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
            if (SetRandomRotation)
            {
                transform.localEulerAngles = _originalEulerAngles;
                SetRandomRotation.Start();
            }
        }
    }
}
