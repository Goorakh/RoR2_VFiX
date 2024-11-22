using System;
using System.Diagnostics;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace VFiX
{
    public static class AddressableExtensions
    {
        public static void CallOnSuccess<T>(this AsyncOperationHandle<T> handle, Action<T> onSuccess)
        {
#if DEBUG
            StackTrace stackTrace = new StackTrace();
#endif

            handle.Completed += handle =>
            {
                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    string log = $"Failed to load asset {handle.LocationName}";
#if DEBUG
                    log += $" {stackTrace}";
#endif

                    Log.Error(log);
                    return;
                }

                onSuccess(handle.Result);
            };
        }
    }
}
