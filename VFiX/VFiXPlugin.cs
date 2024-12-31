using BepInEx;
using System.Diagnostics;
using VFiX.ChestZipper;
using VFiX.GrandparentPreSpawnEffect;
using VFiX.Highlight;
using VFiX.LightningStrikeImpact;
using VFiX.MeteorStrikePredictionEffect;
using VFiX.PooledEffectCleanup;
using VFiX.PooledEffectParent;

namespace VFiX
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    //[BepInDependency(RiskOfOptions.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class VFiXPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "VFiX";
        public const string PluginVersion = "1.0.1";

        static VFiXPlugin _instance;
        internal static VFiXPlugin Instance => _instance;

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            SingletonHelper.Assign(ref _instance, this);

            Log.Init(Logger);

            PooledEffectParentFix.Init();
            PooledEffectCleanupSoundFix.Init();
            EffectPoolPreventInUsePoolClearFix.Init();
            LightningStrikeImpactFix.Init();
            ChestZipperFix.Init();
            MeteorStrikePredictionEffectFix.Init();
            HighlightFix.Init();
            GrandparentPreSpawnEffectFix.Init();

            stopwatch.Stop();
            Log.Message_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalMilliseconds:F0}ms");
        }

        void OnDestroy()
        {
            SingletonHelper.Unassign(ref _instance, this);
        }
    }
}
