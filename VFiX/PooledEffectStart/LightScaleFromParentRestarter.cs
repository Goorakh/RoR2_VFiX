using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class LightScaleFromParentRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public LightScaleFromParent LightScaleFromParent;

        Light _light;
        float _originalRange;

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

            if (!LightScaleFromParent)
            {
                LightScaleFromParent = GetComponent<LightScaleFromParent>();
            }

            if (TryGetComponent(out _light))
            {
                _originalRange = _light.range;
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
            if (_light)
            {
                _light.range = _originalRange;
            }

            if (LightScaleFromParent)
            {
                LightScaleFromParent.Start();
            }
        }
    }
}
