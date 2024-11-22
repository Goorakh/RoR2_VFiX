using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class ParticleSystemRandomColorRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public ParticleSystemRandomColor ParticleSystemRandomColor;

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

            if (!ParticleSystemRandomColor)
            {
                ParticleSystemRandomColor = GetComponent<ParticleSystemRandomColor>();
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
            if (ParticleSystemRandomColor)
            {
                ParticleSystemRandomColor.Awake();
            }
        }
    }
}
