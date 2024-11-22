using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class ImpactEffectRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public ImpactEffect ImpactEffect;

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

            if (!ImpactEffect)
            {
                ImpactEffect = GetComponent<ImpactEffect>();
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
            if (ImpactEffect)
            {
                foreach (ParticleSystem particleSystem in ImpactEffect.particleSystems)
                {
                    particleSystem.Stop();
                    particleSystem.Clear();
                }

                ImpactEffect.Start();
            }
        }
    }
}
