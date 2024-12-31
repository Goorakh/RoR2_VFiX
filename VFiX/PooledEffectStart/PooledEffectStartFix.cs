using RoR2;
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace VFiX.PooledEffectStart
{
    static class PooledEffectStartFix
    {
        [SystemInitializer(typeof(EffectCatalog))]
        static void Init()
        {
            for (int i = 0; i < EffectCatalog.effectCount; i++)
            {
                EffectDef effectDef = EffectCatalog.GetEffectDef((EffectIndex)i);
                if (effectDef == null || !effectDef.prefab)
                    continue;

                if (effectDef.prefabVfxAttributes && effectDef.prefabVfxAttributes.DoNotPool)
                {
                    Log.Debug($"Skipping unpooled effect {effectDef.prefabName}");
                    continue;
                }

                //logUnhandledStartComponents(effectDef);

                GameObject prefab = effectDef.prefab;

                EffectRestarterController restarterController = null;

                EffectRestarterController getOrAddRestarterController()
                {
                    if (!restarterController)
                    {
                        restarterController = prefab.AddComponent<EffectRestarterController>();
                    }

                    return restarterController;
                }

                void tryAddRestarters<TComponent, TRestarter>(string componentFieldName = null) where TComponent : Component where TRestarter : MonoBehaviour, IEffectRestarter
                {
                    FieldInfo componentField = null;
                    if (!string.IsNullOrEmpty(componentFieldName))
                    {
                        componentField = typeof(TRestarter).GetField(componentFieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    }

                    foreach (TComponent component in prefab.GetComponentsInChildren<TComponent>(true))
                    {
                        TRestarter restarter = component.gameObject.AddComponent<TRestarter>();
                        restarter.RestarterController = getOrAddRestarterController();

                        if (componentField != null)
                        {
                            componentField.SetValue(restarter, component);
                        }
                    }
                }

                tryAddRestarters<LightScaleFromParent, LightScaleFromParentRestarter>(nameof(LightScaleFromParentRestarter.LightScaleFromParent));
                tryAddRestarters<RTPCController, RTPCControllerRestarter>(nameof(RTPCControllerRestarter.RTPCController));
                tryAddRestarters<AlignToNormal, AlignToNormalRestarter>(nameof(AlignToNormalRestarter.AlignToNormal));
                tryAddRestarters<ImpactEffect, ImpactEffectRestarter>(nameof(ImpactEffectRestarter.ImpactEffect));
                tryAddRestarters<ParticleSystemColorFromEffectData, ParticleSystemColorFromEffectDataRestarter>(nameof(ParticleSystemColorFromEffectDataRestarter.ParticleSystemColorFromEffectData));
                tryAddRestarters<CoinBehavior, CoinBehaviorRestarter>(nameof(CoinBehaviorRestarter.CoinBehavior));
                tryAddRestarters<VelocityRandomOnStart, VelocityRandomOnStartRestarter>(nameof(VelocityRandomOnStartRestarter.VelocityRandomOnStart));
                tryAddRestarters<ParticleSystemRandomColor, ParticleSystemRandomColorRestarter>(nameof(ParticleSystemRandomColorRestarter.ParticleSystemRandomColor));
                tryAddRestarters<ApplyTorqueOnStart, ApplyTorqueOnStartRestarter>(nameof(ApplyTorqueOnStartRestarter.ApplyTorqueOnStart));
                tryAddRestarters<ApplyForceOnStart, ApplyForceOnStartRestarter>(nameof(ApplyForceOnStartRestarter.ApplyForceOnStart));
                tryAddRestarters<ExplodeRigidbodiesOnStart, ExplodeRigidbodiesOnStartRestarter>(nameof(ExplodeRigidbodiesOnStartRestarter.ExplodeRigidbodiesOnStart));
                tryAddRestarters<SetRandomRotation, SetRandomRotationRestarter>(nameof(SetRandomRotationRestarter.SetRandomRotation));

                if (restarterController)
                {
                    Log.Debug($"Added effect restarter controller(s) to {effectDef.prefabName}");
                }
            }
        }

        [Conditional("DEBUG")]
        static void logUnhandledStartComponents(EffectDef effectDef)
        {
            MonoBehaviour[] components = effectDef.prefab.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                Type type = component.GetType();

                if (type == typeof(EffectComponent) || type == typeof(DestroyOnTimer) || type == typeof(DestroyOnParticleEnd) || type == typeof(AnimateShaderAlpha) || type == typeof(LightScaleFromParent) || type == typeof(RTPCController) || type == typeof(TeamFilter) || type == typeof(ScaleParticleSystemDuration) || type == typeof(RotateObject) || type == typeof(AlignToNormal) || type == typeof(OmniEffect) || type == typeof(ImpactEffect) || type == typeof(ParticleSystemColorFromEffectData) || type == typeof(LaserPointer) || type == typeof(CoinBehavior) || type == typeof(VelocityRandomOnStart) || type == typeof(ParticleSystemRandomColor) || type == typeof(RigidbodyStickOnImpact) || type == typeof(ApplyTorqueOnStart) || type == typeof(ApplyForceOnStart) || type == typeof(ExplodeRigidbodiesOnStart) || type == typeof(MaintainRotation) || type == typeof(RotateItem) || type == typeof(SetRandomRotation) || type == typeof(Tracer) || type == typeof(BeamPointsFromTransforms))
                {
                    continue;
                }

                const BindingFlags METHOD_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                bool hasInit = type.GetMethod("Awake", METHOD_FLAGS) != null || type.GetMethod("Start", METHOD_FLAGS) != null;
                bool hasEnable = type.GetMethod("OnEnable", METHOD_FLAGS) != null;

                if (hasInit && !hasEnable)
                {
                    string log = $"Init-only component {type.FullName} on {effectDef.prefabName}";

                    if (component.transform != effectDef.prefab.transform)
                    {
                        log += $", at {Util.BuildPrefabTransformPath(effectDef.prefab.transform, component.transform, false, true)}";
                    }

                    Log.Info(log);
                }
            }
        }
    }
}
