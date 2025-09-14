using BepInEx;
using System.Diagnostics;
using VFiX.BanditFallDamageSound;
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
    public class VFiXPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "VFiX";
        public const string PluginVersion = "1.0.6";

        static VFiXPlugin _instance;
        internal static VFiXPlugin Instance => _instance;

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            SingletonHelper.Assign(ref _instance, this);

            Log.Init(Logger);

            BanditFallDamageSoundFix.Init();
            ChestZipperFix.Init();
            EffectPoolPreventInUsePoolClearFix.Init();
            GrandparentPreSpawnEffectFix.Init();
            HighlightFix.Init();
            LightningStrikeImpactFix.Init();
            MeteorStrikePredictionEffectFix.Init();
            PooledEffectCleanupSoundFix.Init();
            PooledEffectParentFix.Init();

            stopwatch.Stop();
            Log.Message_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalMilliseconds:F0}ms");
        }

        void OnDestroy()
        {
            SingletonHelper.Unassign(ref _instance, this);
        }
    }
}
