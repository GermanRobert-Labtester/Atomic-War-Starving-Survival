using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Evolving-world activation (task 122) ────────────────────────
        // The world trio (location evolution, wildlife migration, landmark
        // degradation) is ticked by EvolvingWorldDayOwner; this partial owns
        // the influence plumbing the rest of the host reads every day.

        /// <summary>Trapping density multiplier for the home sector, refreshed daily.</summary>
        private float _homeTrappingDensity = 1f;

        /// <summary>Regional danger multiplier factory installed on the expedition engine.</summary>
        private Func<string, float> _expeditionDangerComposer = null!;

        /// <summary>Ashfall mm per weather kind — the ash the sky actually drops.</summary>
        internal static float AshfallMmFor(WeatherKind kind) => kind switch
        {
            WeatherKind.Ashfall => 6f,
            WeatherKind.FalloutStorm => 10f,
            WeatherKind.BlackRain => 14f,
            WeatherKind.BlackSnow => 4f,
            WeatherKind.BloodRain => 8f,
            WeatherKind.GlassStorm => 5f,
            WeatherKind.RadHail => 3f,
            _ => 0f
        };

        /// <summary>
        /// One-time influence wiring: composes the expedition encounter
        /// multiplier (warlord ground × wildlife pressure × location threats)
        /// and refreshes the trapping density. Safe to call every tick.
        /// </summary>
        private void SetupEvolvingWorldInfluence()
        {
            SetupWorld();

            _expeditionDangerComposer ??= ComposeExpeditionDangerMultiplier();
            SetupExpeditions();
            _expeditions.SetEncounterChanceMultiplier(_expeditionDangerComposer);

            RefreshTrappingDensity();
        }

        /// <summary>
        /// Wildlife pressure on the home sector feeds the snare lines: empty
        /// ground halves the authored catch rate, a booming sector lifts it.
        /// </summary>
        private void RefreshTrappingDensity()
        {
            SetupWorld();
            SetupWildlifeTrappingIfBound();
            string sector = _world.ShelterSectorId;
            int pop = string.IsNullOrEmpty(sector) ? 0 : _world.Wildlife.GetSectorPackPopulation(sector);
            _homeTrappingDensity = Math.Clamp(0.5f + pop * 0.1f, 0.4f, 1.5f);
            if (_wildlifeTrapping != null)
                _wildlifeTrapping.WildlifeDensityMultiplier = _homeTrappingDensity;
        }

        private void SetupWildlifeTrappingIfBound()
        {
            // The trapping session is constructed by its own surface; if it is
            // not up yet there is nothing to feed today — the next daily tick
            // refreshes it after the panel wires.
        }

        /// <summary>
        /// Single composer for the expedition engine's one encounter-chance
        /// slot: warlord travel danger × regional wildlife desperation ×
        /// live location threats. Installed here and from
        /// WireWarlordExpeditionDanger so both wirings converge on one truth.
        /// </summary>
        private Func<string, float> ComposeExpeditionDangerMultiplier()
        {
            return locationId =>
            {
                float mult = 1f;

                // Warlord-controlled/contested ground (existing Core danger).
                var w = _yearOfAsh?.Warlord;
                if (w != null)
                {
                    float mod = w.TravelDangerModifier(locationId);
                    if (mod > 0f) mult *= 1f + mod;
                }

                // Regional wildlife pressure: desperate or rabid-rich country
                // is louder on the road, a booming one barely noticed.
                if (_world != null)
                {
                    float ratio = _world.Wildlife.GetGlobalPopulationRatio();
                    if (ratio < 0.4f) mult *= 1.15f;
                    else if (ratio > 1.2f) mult *= 0.95f;
                    foreach (var p in _world.Wildlife.State.packs)
                    {
                        if (p != null && p.isRabid) { mult *= 1.05f; break; }
                    }

                    // Live location threats and heavy contamination.
                    var rec = _world.LocationEvolution?.TryGetRecord(locationId);
                    if (rec != null)
                    {
                        mult *= 1f + Math.Min(0.45f, rec.activeThreats.Count * 0.15f);
                        if (rec.contaminationLevel > 0.6f) mult *= 1.1f;
                    }
                }

                return mult;
            };
        }

        }
}
