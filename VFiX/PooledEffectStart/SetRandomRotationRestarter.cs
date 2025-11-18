using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class SetRandomRotationRestarter : MonoBehaviour, IEffectRestarter
    {
        SetRandomRotation _setRandomRotation;

        Vector3 _originalEulerAngles;

        void Awake()
        {
            _setRandomRotation = GetComponent<SetRandomRotation>();
            _originalEulerAngles = transform.localEulerAngles;
        }

        void IEffectRestarter.Restart()
        {
            if (_setRandomRotation)
            {
                transform.localEulerAngles = _originalEulerAngles;
                _setRandomRotation.Start();
            }
        }
    }
}
