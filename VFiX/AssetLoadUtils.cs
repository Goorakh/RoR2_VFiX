using RoR2.ContentManagement;
using System;
using UnityEngine.AddressableAssets;

namespace VFiX
{
    public static class AssetLoadUtils
    {
        public static void TemporaryPreload<T>(string assetGuid, Action<T> onLoaded) where T : UnityEngine.Object
        {
            if (string.IsNullOrWhiteSpace(assetGuid))
                throw new ArgumentException($"'{nameof(assetGuid)}' cannot be null or whitespace.", nameof(assetGuid));

            TemporaryPreload(new AssetReferenceT<T>(assetGuid), onLoaded);
        }

        public static void TemporaryPreload<T>(AssetReferenceT<T> assetReference, Action<T> onLoaded) where T : UnityEngine.Object
        {
            if (assetReference is null)
                throw new ArgumentNullException(nameof(assetReference));

            if (onLoaded is null)
                throw new ArgumentNullException(nameof(onLoaded));

            AssetAsyncReferenceManager<T>.LoadAsset(assetReference, AsyncReferenceHandleUnloadType.Preload).Completed += handle =>
            {
                T asset = handle.Result;
                if (asset)
                {
                    onLoaded(asset);
                }

                AssetAsyncReferenceManager<T>.UnloadAsset(assetReference);
            };
        }
    }
}
