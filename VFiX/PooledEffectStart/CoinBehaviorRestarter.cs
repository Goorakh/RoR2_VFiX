using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class CoinBehaviorRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public CoinBehavior CoinBehavior;

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

            if (!CoinBehavior)
            {
                CoinBehavior = GetComponent<CoinBehavior>();
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
            if (CoinBehavior)
            {
                foreach (CoinBehavior.CoinTier tier in CoinBehavior.coinTiers)
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

                CoinBehavior.Start();
            }
        }
    }
}
