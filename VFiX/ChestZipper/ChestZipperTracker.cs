using System.Collections.Generic;
using UnityEngine;

namespace VFiX.ChestZipper
{
    public class ChestZipperTracker : MonoBehaviour
    {
        readonly List<ChestZipperController> _ownedZipperControllers = [];

        public void DestroyAllTrackedZippers()
        {
            Log.Debug($"Destroying {_ownedZipperControllers.Count} zipper(s) for {name}");

            for (int i = _ownedZipperControllers.Count - 1; i >= 0; i--)
            {
                ChestZipperController zipperController = _ownedZipperControllers[i];
                if (zipperController)
                {
                    zipperController.DestroyOrReturnToPool();
                }
            }

            _ownedZipperControllers.Clear();
        }

        public void TrackZipperInstance(ChestZipperController zipperController)
        {
            _ownedZipperControllers.Add(zipperController);
        }

        public void RemoveZipperInstance(ChestZipperController zipperController)
        {
            _ownedZipperControllers.Remove(zipperController);
        }
    }
}
