using RoR2;
using UnityEngine;

namespace VFiX.ChestZipper
{
    public class ChestZipperController : MonoBehaviour
    {
        EffectManagerHelper _effectManagerHelper;

        ChestZipperTracker _ownerZipperTracker;
        bool _isTracked = false;

        float _untrackedAge = 0f;

        void Start()
        {
            _effectManagerHelper = GetComponent<EffectManagerHelper>();
        }

        void OnEnable()
        {
            _isTracked = false;
            _untrackedAge = 0f;
        }

        void OnDisable()
        {
            if (_isTracked && _ownerZipperTracker)
            {
                _ownerZipperTracker.RemoveZipperInstance(this);
            }
        }

        void FixedUpdate()
        {
            if (!_isTracked)
            {
                _untrackedAge += Time.fixedDeltaTime;
                if (_untrackedAge > 10f)
                {
                    Log.Warning($"{name}: Timed out, returning to pool");
                    DestroyOrReturnToPool();
                    return;
                }

                GameObject chestOwner = null;

                EntityLocator entityLocator = GetComponentInParent<EntityLocator>();
                if (entityLocator)
                {
                    chestOwner = entityLocator.entity;
                }

                if (chestOwner && chestOwner.TryGetComponent(out _ownerZipperTracker))
                {
                    Log.Debug($"Found chest owner: {chestOwner}");

                    _ownerZipperTracker.TrackZipperInstance(this);
                    _isTracked = true;
                }
            }
        }

        public void DestroyOrReturnToPool()
        {
            if (_effectManagerHelper)
            {
                _effectManagerHelper.ReturnToPool();
                return;
            }

            Destroy(gameObject);
        }
    }
}
