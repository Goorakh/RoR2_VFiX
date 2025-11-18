using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class ParticleSystemColorFromEffectDataRestarter : MonoBehaviour, IEffectRestarter
    {
        ParticleSystemColorFromEffectData _particleSystemColorFromEffectData;

        void Awake()
        {
            _particleSystemColorFromEffectData = GetComponent<ParticleSystemColorFromEffectData>();
        }

        void IEffectRestarter.Restart()
        {
            if (_particleSystemColorFromEffectData)
            {
                foreach (ParticleSystem particleSystem in _particleSystemColorFromEffectData.particleSystems)
                {
                    particleSystem.Stop();
                    particleSystem.Clear();
                }

                _particleSystemColorFromEffectData.Start();
            }
        }
    }
}
