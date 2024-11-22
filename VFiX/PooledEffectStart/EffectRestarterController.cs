using System;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class EffectRestarterController : MonoBehaviour
    {
        Rigidbody[] _rigidbodies;

        bool _hasStarted;
        bool _shouldReset;

        public event Action OnReset;

        void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        }

        void Start()
        {
            _hasStarted = true;
        }

        void OnEnable()
        {
            if (_hasStarted)
            {
                _shouldReset = true;
            }
        }

        void Update()
        {
            if (_shouldReset)
            {
                _shouldReset = false;

                foreach (Rigidbody rigidbody in _rigidbodies)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }

                OnReset?.Invoke();
            }
        }
    }
}
