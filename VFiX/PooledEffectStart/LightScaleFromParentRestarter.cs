using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class LightScaleFromParentRestarter : MonoBehaviour, IEffectRestarter
    {
        LightScaleFromParent _lightScaleFromParent;

        Light _light;
        float _originalRange;

        void Awake()
        {
            _lightScaleFromParent = GetComponent<LightScaleFromParent>();

            if (TryGetComponent(out _light))
            {
                _originalRange = _light.range;
            }
        }

        void IEffectRestarter.Restart()
        {
            if (_light)
            {
                _light.range = _originalRange;
            }

            if (_lightScaleFromParent)
            {
                _lightScaleFromParent.Start();
            }
        }
    }
}
