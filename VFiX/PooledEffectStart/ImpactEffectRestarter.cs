using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class ImpactEffectRestarter : MonoBehaviour, IEffectRestarter
    {
        ImpactEffect _impactEffect;

        void Awake()
        {
            _impactEffect = GetComponent<ImpactEffect>();
        }

        void IEffectRestarter.Restart()
        {
            if (_impactEffect)
            {
                foreach (ParticleSystem particleSystem in _impactEffect.particleSystems)
                {
                    particleSystem.Stop();
                    particleSystem.Clear();
                }

                _impactEffect.Start();
            }
        }
    }
}
