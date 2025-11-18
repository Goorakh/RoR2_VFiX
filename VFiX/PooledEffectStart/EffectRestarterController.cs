using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class EffectRestarterController : MonoBehaviour
    {
        EffectComponent _effectComponent;

        Rigidbody[] _rigidbodies = [];
        IEffectRestarter[] _restarters = [];

        void Awake()
        {
            _effectComponent = GetComponent<EffectComponent>();
            if (_effectComponent)
            {
                _effectComponent.OnEffectComponentReset += onEffectComponentReset;
            }

            _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
            _restarters = GetComponentsInChildren<IEffectRestarter>(true);
        }

        void OnDestroy()
        {
            if (_effectComponent)
            {
                _effectComponent.OnEffectComponentReset -= onEffectComponentReset;
            }
        }

        void onEffectComponentReset(bool hasEffectData)
        {
            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                if (rigidbody)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
            }

            foreach (IEffectRestarter restarter in _restarters)
            {
                if (restarter == null || (restarter is UnityEngine.Object restarterObject && !restarterObject))
                    continue;

                restarter.Restart();
            }
        }
    }
}
