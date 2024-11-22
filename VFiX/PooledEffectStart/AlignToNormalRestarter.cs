using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public class AlignToNormalRestarter : MonoBehaviour, IEffectRestarter
    {
        [SerializeField]
        EffectRestarterController _restarter;

        EffectRestarterController IEffectRestarter.RestarterController
        {
            get => _restarter;
            set => _restarter = value;
        }

        public AlignToNormal AlignToNormal;

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

            if (!AlignToNormal)
            {
                AlignToNormal = GetComponent<AlignToNormal>();
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
            if (AlignToNormal)
            {
                AlignToNormal.Start();
            }
        }
    }
}
