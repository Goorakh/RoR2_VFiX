using BepInEx;
using System.Diagnostics;
using VFiX.GrandparentPreSpawnEffect;
using VFiX.MeteorStrikePredictionEffect;
using VFiX.PooledEffectCleanup;

namespace VFiX
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    public sealed class VFiXPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "VFiX";
        public const string PluginVersion = "1.1.2";

        static VFiXPlugin _instance;
        internal static VFiXPlugin Instance => _instance;

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            SingletonHelper.Assign(ref _instance, this);

            Log.Init(Logger);

            EffectPoolPreventInUsePoolClearFix.Init();
            GrandparentPreSpawnEffectFix.Init();
            MeteorStrikePredictionEffectFix.Init();
            PooledEffectCleanupSoundFix.Init();

            stopwatch.Stop();
            Log.Message_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalMilliseconds:F0}ms");
        }

        void OnDestroy()
        {
            SingletonHelper.Unassign(ref _instance, this);
        }
    }
}
