using System;
using UnityEngine;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// RAD-001 — the per-survivor exposure hook RadiationSystem ticks against.
    ///
    /// RadiationSystem was constructed with no hook at all, so every tick resolved a
    /// null context: zone 0, no gear, no shelter query. Nothing but scripted
    /// <c>Expose(...)</c> calls could dose anyone, which turned off ambient radiation —
    /// the game's signature hazard — both indoors and out.
    /// </summary>
    public partial class GameBootstrap
    {
        /// <summary>
        /// Reused across survivors and ticks. RadiationSystem consumes the context
        /// immediately inside its own loop and never retains it, so one instance is
        /// enough and keeps the hot path allocation-free.
        /// </summary>
        private readonly ExposureContext _exposureScratch = new ExposureContext();

        private Func<float, float> _shelterRadQuery;

        /// <summary>
        /// Dose rate outside the hatch right now. Baseline is clean — a bunker sited on
        /// safe ground — with fallout storms and black rain flooding the surface.
        /// </summary>
        private float GetOutdoorRadsPerHour()
        {
            return WeatherSystem != null ? Mathf.Max(0f, WeatherSystem.OutdoorRadModifier) : 0f;
        }

        /// <summary>
        /// Build the exposure context for one survivor: where they are standing, what
        /// they are wearing, and — when they are inside — the shelter query that applies
        /// shielding, filtration and the bunker's own contamination.
        /// </summary>
        private ExposureContext BuildExposureContext(Survivor survivor)
        {
            if (survivor == null) return null;

            var context = _exposureScratch;
            context.ShelterShielding = 0f;
            Inventory?.FillWornGear(context.WornGear);

            float outdoor = GetOutdoorRadsPerHour();

            var expedition = FindActiveExpedition(survivor);
            if (expedition != null)
            {
                // Out in the open: the node's own dose plus whatever the sky is doing.
                // No shelter query — there is nothing overhead but weather.
                context.ZoneRadLevel = Mathf.Max(0f, expedition.TrueRadPerHour) + outdoor;
                context.ShelterRadQuery = null;
                return context;
            }

            // Inside: the exterior dose goes through Shelter.GetInteriorRadsPerHour,
            // which applies overworld/ceiling/module shielding and then adds the
            // contamination the bunker is carrying on its own floor.
            context.ZoneRadLevel = outdoor;
            if (_shelterRadQuery == null)
                _shelterRadQuery = zone => Shelter != null ? Shelter.GetInteriorRadsPerHour(zone) : zone;
            context.ShelterRadQuery = _shelterRadQuery;
            return context;
        }

        private ExpeditionState FindActiveExpedition(Survivor survivor)
        {
            var active = ExpeditionSystem?.ActiveExpeditions;
            if (survivor == null || active == null) return null;
            for (int i = 0; i < active.Count; i++)
            {
                var exp = active[i];
                if (exp?.Survivor != null && exp.Survivor.Id == survivor.Id) return exp;
            }
            return null;
        }
    }
}
