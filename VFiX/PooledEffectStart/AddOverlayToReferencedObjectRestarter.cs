using RoR2.DispatachableEffects;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class AddOverlayToReferencedObjectRestarter : MonoBehaviour, IEffectRestarter
    {
        AddOverlayToReferencedObject _addOverlayToReferencedObject;

        void Awake()
        {
            _addOverlayToReferencedObject = GetComponent<AddOverlayToReferencedObject>();
        }

        void IEffectRestarter.Restart()
        {
            if (_addOverlayToReferencedObject)
            {
                _addOverlayToReferencedObject.Start();
            }
        }
    }
}
