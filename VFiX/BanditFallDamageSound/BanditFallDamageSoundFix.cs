using RoR2;
using UnityEngine;

namespace VFiX.BanditFallDamageSound
{
    static class BanditFallDamageSoundFix
    {
        public static void Init()
        {
            AssetLoadUtils.TemporaryPreload<GameObject>(RoR2BepInExPack.GameAssetPaths.RoR2_Base_Bandit2.Bandit2Body_prefab, banditBody =>
            {
                if (banditBody.TryGetComponent(out SfxLocator sfxLocator))
                {
                    if (string.IsNullOrEmpty(sfxLocator.fallDamageSound))
                    {
                        sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
                    }
                }
                else
                {
                    Log.Error($"{banditBody} is missing SfxLocator component");
                }
            });
        }
    }
}
