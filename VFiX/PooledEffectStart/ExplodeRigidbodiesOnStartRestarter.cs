using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class ExplodeRigidbodiesOnStartRestarter : MonoBehaviour, IEffectRestarter
    {
        ExplodeRigidbodiesOnStart _explodeRigidbodiesOnStart;

        void Awake()
        {
            _explodeRigidbodiesOnStart = GetComponent<ExplodeRigidbodiesOnStart>();
        }

        void IEffectRestarter.Restart()
        {
            if (_explodeRigidbodiesOnStart)
            {
                _explodeRigidbodiesOnStart.Start();
            }
        }
    }
}
