using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class VelocityRandomOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        VelocityRandomOnStart _velocityRandomOnStart;

        void Awake()
        {
            _velocityRandomOnStart = GetComponent<VelocityRandomOnStart>();
        }

        void IEffectRestarter.Restart()
        {
            if (_velocityRandomOnStart)
            {
                _velocityRandomOnStart.Start();
            }
        }
    }
}
