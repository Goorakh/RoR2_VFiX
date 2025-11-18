using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class TemporaryOverlayRestarter : MonoBehaviour, IEffectRestarter
    {
        TemporaryOverlay _temporaryOverlay;

        void Awake()
        {
            _temporaryOverlay = GetComponent<TemporaryOverlay>();
        }

        void IEffectRestarter.Restart()
        {
            if (_temporaryOverlay)
            {
                if (_temporaryOverlay.playOnAwake)
                {
                    _temporaryOverlay.StartOverlay();
                }
            }
        }
    }
}
