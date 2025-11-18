using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class EffectPassArgsToLocalTokenRestarter : MonoBehaviour, IEffectRestarter
    {
        EffectPassArgsToLocalToken _effectPassArgsToLocalToken;

        void Awake()
        {
            _effectPassArgsToLocalToken = GetComponent<EffectPassArgsToLocalToken>();
        }

        void IEffectRestarter.Restart()
        {
            if (_effectPassArgsToLocalToken)
            {
                _effectPassArgsToLocalToken.Start();
            }
        }
    }
}
