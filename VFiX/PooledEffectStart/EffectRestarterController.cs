using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class EffectRestarterController : MonoBehaviour
    {
        EffectComponent _effectComponent;

        Rigidbody[] _rigidbodies = [];
        IEffectRestarter[] _restarters = [];

        Vector3 _originalScale = Vector3.one;

        void Awake()
        {
            _effectComponent = GetComponent<EffectComponent>();
            if (_effectComponent)
            {
                _effectComponent.OnEffectComponentReset += onEffectComponentReset;
            }

            _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
            _restarters = GetComponentsInChildren<IEffectRestarter>(true);

            _originalScale = transform.localScale;
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
            // Reset is also called on Start, but we only care about when an effect is reset from the pool
            if (gameObject.activeSelf)
                return;

            if (!hasEffectData || !_effectComponent.applyScale)
            {
                transform.localScale = _originalScale;
            }

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
