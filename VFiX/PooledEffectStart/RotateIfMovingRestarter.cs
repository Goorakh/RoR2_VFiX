using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class RotateIfMovingRestarter : MonoBehaviour, IEffectRestarter
    {
        RotateIfMoving _rotateIfMoving;

        void Awake()
        {
            _rotateIfMoving = GetComponent<RotateIfMoving>();
        }

        void IEffectRestarter.Restart()
        {
            if (_rotateIfMoving)
            {
                _rotateIfMoving.Start();
            }
        }
    }
}
