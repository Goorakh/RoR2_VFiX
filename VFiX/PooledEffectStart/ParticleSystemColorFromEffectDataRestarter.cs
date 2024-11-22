using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class ParticleSystemColorFromEffectDataRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public ParticleSystemColorFromEffectData ParticleSystemColorFromEffectData;

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

            if (!ParticleSystemColorFromEffectData)
            {
                ParticleSystemColorFromEffectData = GetComponent<ParticleSystemColorFromEffectData>();
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
            if (ParticleSystemColorFromEffectData)
            {
                foreach (ParticleSystem particleSystem in ParticleSystemColorFromEffectData.particleSystems)
                {
                    particleSystem.Stop();
                    particleSystem.Clear();
                }

                ParticleSystemColorFromEffectData.Start();
            }
        }
    }
}
