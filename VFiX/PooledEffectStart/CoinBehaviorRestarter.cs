using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class CoinBehaviorRestarter : MonoBehaviour, IEffectRestarter
    {
        CoinBehavior _coinBehavior;

        void Awake()
        {
            _coinBehavior = GetComponent<CoinBehavior>();
        }

        void IEffectRestarter.Restart()
        {
            if (_coinBehavior)
            {
                foreach (CoinBehavior.CoinTier tier in _coinBehavior.coinTiers)
                {
                    ParticleSystem particleSystem = tier.particleSystem;
                    if (particleSystem)
                    {
                        ParticleSystem.EmissionModule emission = particleSystem.emission;
                        emission.enabled = false;

                        particleSystem.Stop();
                        particleSystem.Clear();

                        particleSystem.gameObject.SetActive(false);
                    }
                }

                _coinBehavior.Start();
            }
        }
    }
}
