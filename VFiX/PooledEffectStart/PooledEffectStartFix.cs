using HG;
using RoR2;
using RoR2.DispatachableEffects;
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

                logUnhandledStartComponents(effectDef);

                GameObject prefab = effectDef.prefab;

                bool tryAddRestarters<TComponent, TRestarter>() where TComponent : Component where TRestarter : MonoBehaviour, IEffectRestarter
                {
                    bool addedAnyRestarter = false;
                    foreach (TComponent component in prefab.GetComponentsInChildren<TComponent>(true))
                    {
                        TRestarter restarter = component.gameObject.AddComponent<TRestarter>();
                        addedAnyRestarter = true;
                    }

                    return addedAnyRestarter;
                }

                bool addedAnyRestarter = false;
                addedAnyRestarter |= tryAddRestarters<LightScaleFromParent, LightScaleFromParentRestarter>();
                addedAnyRestarter |= tryAddRestarters<ImpactEffect, ImpactEffectRestarter>();
                addedAnyRestarter |= tryAddRestarters<ParticleSystemColorFromEffectData, ParticleSystemColorFromEffectDataRestarter>();
                addedAnyRestarter |= tryAddRestarters<CoinBehavior, CoinBehaviorRestarter>();
                addedAnyRestarter |= tryAddRestarters<VelocityRandomOnStart, VelocityRandomOnStartRestarter>();
                addedAnyRestarter |= tryAddRestarters<ParticleSystemRandomColor, ParticleSystemRandomColorRestarter>();
                addedAnyRestarter |= tryAddRestarters<ApplyTorqueOnStart, ApplyTorqueOnStartRestarter>();
                addedAnyRestarter |= tryAddRestarters<ApplyForceOnStart, ApplyForceOnStartRestarter>();
                addedAnyRestarter |= tryAddRestarters<ExplodeRigidbodiesOnStart, ExplodeRigidbodiesOnStartRestarter>();
                addedAnyRestarter |= tryAddRestarters<SetRandomRotation, SetRandomRotationRestarter>();
                addedAnyRestarter |= tryAddRestarters<AddOverlayToReferencedObject, AddOverlayToReferencedObjectRestarter>();
                addedAnyRestarter |= tryAddRestarters<TemporaryOverlay, TemporaryOverlayRestarter>();
                addedAnyRestarter |= tryAddRestarters<EffectPassArgsToLocalToken, EffectPassArgsToLocalTokenRestarter>();

                if (addedAnyRestarter)
                {
                    prefab.EnsureComponent<EffectRestarterController>();
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

                if (type == typeof(EffectComponent) || type == typeof(DestroyOnTimer) || type == typeof(DestroyOnParticleEnd) || type == typeof(AnimateShaderAlpha) || type == typeof(LightScaleFromParent) || type == typeof(RTPCController) || type == typeof(TeamFilter) || type == typeof(ScaleParticleSystemDuration) || type == typeof(RotateObject) || type == typeof(AlignToNormal) || type == typeof(OmniEffect) || type == typeof(ImpactEffect) || type == typeof(ParticleSystemColorFromEffectData) || type == typeof(LaserPointer) || type == typeof(CoinBehavior) || type == typeof(VelocityRandomOnStart) || type == typeof(ParticleSystemRandomColor) || type == typeof(RigidbodyStickOnImpact) || type == typeof(ApplyTorqueOnStart) || type == typeof(ApplyForceOnStart) || type == typeof(ExplodeRigidbodiesOnStart) || type == typeof(MaintainRotation) || type == typeof(RotateItem) || type == typeof(SetRandomRotation) || type == typeof(Tracer) || type == typeof(BeamPointsFromTransforms) || type == typeof(LineBetweenTransforms) || type == typeof(MultiPointBezierCurveLine) || type == typeof(AddOverlayToReferencedObject) || type == typeof(TemporaryOverlay) || type == typeof(RendererColorFromEffectData) || type == typeof(EffectRetimer) || type == typeof(EffectPassArgsToLocalToken) || type == typeof(RotateIfMoving) || type == typeof(PickRandomObjectOnAwake) || type == typeof(PlatformToggle))
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
