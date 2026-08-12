using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Environment
{
    public struct OzoneScourgeWarningEvent
    {
        public string Message;
        public float AmbientUV;

        public OzoneScourgeWarningEvent(string message, float ambientUV)
        {
            Message = message;
            AmbientUV = ambientUV;
        }
    }

    /// <summary>
    /// Expansion IV — Chapter 38.1 The Ozone Scourge.
    /// Modifies Weather_FalseSpring and Weather_SilentSpring. When the ash breaks,
    /// AmbientUV spikes to lethal levels (acting like a microwave).
    /// </summary>
    public class OzoneScourgeSystem
    {
        public const float StandardUVIndex = 1.0f;
        public const float FalseSpringUVIndex = 14.5f;
        public const float SilentSpringUVIndex = 18.0f;
        public const float UVBlisteringHealthDamage = 15f;
        public const float UVBlisteringMoraleDamage = 25f;

        public const string Affliction_SnowBlindness = "affliction_snow_blindness";
        public const string Affliction_UV_Blistering = "affliction_uv_blistering";
        public const string Affliction_CornealBurn = "affliction_corneal_burn";

        public const string Item_WeldersGlass = "item_welders_glass";
        public const string Item_LeadVisor = "item_lead_visor";
        public const string Item_AshGhillie = "item_ash_ghillie";

        private readonly WeatherSystem _weatherSystem;
        private NeedsSystem _needsSystem;

        public event Action<Survivor, string> OnUVAfflictionApplied;
        public event Action<OzoneScourgeWarningEvent> OnOzoneScourgeWarningEventBus;

        public OzoneScourgeSystem(WeatherSystem weatherSystem)
        {
            _weatherSystem = weatherSystem;
        }

        public void BindNeedsSystem(NeedsSystem needsSystem)
        {
            _needsSystem = needsSystem;
        }

        public float GetAmbientUV()
        {
            if (_weatherSystem == null) return StandardUVIndex;
            return GetAmbientUVForWeather(_weatherSystem.Current);
        }

        public static float GetAmbientUVForWeather(WeatherKind kind)
        {
            switch (kind)
            {
                case WeatherKind.FalseSpring:
                    return FalseSpringUVIndex;
                case WeatherKind.SilentSpring:
                    return SilentSpringUVIndex;
                default:
                    return StandardUVIndex;
            }
        }

        public bool IsOzoneScourgeActive()
        {
            if (_weatherSystem == null) return false;
            return _weatherSystem.Current == WeatherKind.FalseSpring || _weatherSystem.Current == WeatherKind.SilentSpring;
        }

        /// <summary>
        /// Looking at surface camera feeds without item_welders_glass causes Affliction_SnowBlindness / CornealBurn.
        /// </summary>
        public bool InspectCameraFeed(bool hasWeldersGlassFilter, Survivor observer)
        {
            if (!IsOzoneScourgeActive()) return true;

            if (!hasWeldersGlassFilter && observer != null && observer.IsAlive)
            {
                if (!observer.HasTrait("trait_sun_blindness"))
                {
                    observer.Traits.Add(Affliction_SnowBlindness);
                    if (_needsSystem != null)
                    {
                        _needsSystem.Modify(observer, NeedKind.Morale, -10f);
                    }
                    OnUVAfflictionApplied?.Invoke(observer, Affliction_SnowBlindness);
                }

                OnOzoneScourgeWarningEventBus?.Invoke(new OzoneScourgeWarningEvent("OPTIC NERVE DEGRADATION DETECTED: Unfiltered UV Feed Exposure", GetAmbientUV()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stepping outside without item_lead_visor and item_ash_ghillie during Ozone Scourge applies Affliction_UV_Blistering.
        /// </summary>
        public void EvaluateExpeditionSurfaceExposure(Survivor survivor, bool hasLeadVisor, bool hasAshGhillie)
        {
            if (survivor == null || !survivor.IsAlive || !IsOzoneScourgeActive()) return;

            if (!hasLeadVisor || !hasAshGhillie)
            {
                if (!survivor.HasTrait(Affliction_UV_Blistering))
                {
                    survivor.Traits.Add(Affliction_UV_Blistering);
                }

                if (!hasLeadVisor && !survivor.HasTrait(Affliction_CornealBurn))
                {
                    survivor.Traits.Add(Affliction_CornealBurn);
                }

                if (_needsSystem != null)
                {
                    _needsSystem.Modify(survivor, NeedKind.Health, -UVBlisteringHealthDamage);
                    _needsSystem.Modify(survivor, NeedKind.Morale, -UVBlisteringMoraleDamage);
                }

                OnUVAfflictionApplied?.Invoke(survivor, Affliction_UV_Blistering);
            }
        }
    }
}
