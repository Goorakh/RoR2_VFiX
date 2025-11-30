using RoR2;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    public sealed class LightScaleFromParentRestarter : MonoBehaviour, IEffectRestarter
    {
        LightScaleFromParent _lightScaleFromParent;

        Light _light;
        float _originalRange;

        void Awake()
        {
            _lightScaleFromParent = GetComponent<LightScaleFromParent>();

            if (TryGetComponent(out Light light))
            {
                _light = light;
                _originalRange = light.range;
                Log.Debug($"Recorded original light range of {_originalRange} for {Util.GetGameObjectHierarchyName(gameObject)}");
            }
            else
            {
                Log.Warning($"No light reference, light range will not be restored: {Util.GetGameObjectHierarchyName(gameObject)}");
            }
        }

        void IEffectRestarter.Restart()
        {
            if (_light)
            {
                _light.range = _originalRange;
                Log.Debug($"Restored light range of {_light.range} for {Util.GetGameObjectHierarchyName(gameObject)}");
            }
            else
            {
                Log.Warning($"No light reference, light range will not be restored: {Util.GetGameObjectHierarchyName(gameObject)}");
            }

            if (_lightScaleFromParent)
            {
                _lightScaleFromParent.Start();
            }

            Log.Debug($"Scaled light range of {_light.range} for: {Util.GetGameObjectHierarchyName(gameObject)}");
        }
    }
}
