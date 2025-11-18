using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class ParticleSystemRandomColorRestarter : MonoBehaviour, IEffectRestarter
    {
        ParticleSystemRandomColor _particleSystemRandomColor;

        void Awake()
        {
            _particleSystemRandomColor = GetComponent<ParticleSystemRandomColor>();
        }

        void IEffectRestarter.Restart()
        {
            if (_particleSystemRandomColor)
            {
                _particleSystemRandomColor.Awake();
            }
        }
    }
}
