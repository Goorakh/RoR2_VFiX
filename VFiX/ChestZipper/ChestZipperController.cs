using RoR2;
using System.Collections;
using UnityEngine;

namespace VFiX.ChestZipper
{
    public class ChestZipperController : MonoBehaviour
    {
        EffectManagerHelper _effectManagerHelper;

        ChestZipperTracker _ownerZipperTracker;

        void Start()
        {
            _effectManagerHelper = GetComponent<EffectManagerHelper>();
        }

        void OnEnable()
        {
            StartCoroutine(waitThenFindOwner());
        }

        void OnDisable()
        {
            if (_ownerZipperTracker)
            {
                _ownerZipperTracker.RemoveZipperInstance(this);
            }
        }

        IEnumerator waitThenFindOwner()
        {
            yield return new WaitForFixedUpdate();

            findOwner();
        }

        void findOwner()
        {
            GameObject chestOwner = null;

            EntityLocator entityLocator = GetComponentInParent<EntityLocator>();
            if (entityLocator)
            {
                chestOwner = entityLocator.entity;
            }

            if (chestOwner && chestOwner.TryGetComponent(out _ownerZipperTracker))
            {
                Log.Debug($"{Util.GetGameObjectHierarchyName(gameObject)}: Found chest owner: {Util.GetGameObjectHierarchyName(chestOwner)}");

                _ownerZipperTracker.TrackZipperInstance(this);
            }
            else
            {
                Log.Warning($"{Util.GetGameObjectHierarchyName(gameObject)} failed to find owner, returning to pool");
                DestroyOrReturnToPool();
            }
        }

        public void DestroyOrReturnToPool()
        {
            if (_effectManagerHelper)
            {
                _effectManagerHelper.ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
