using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ShelterFireHazardSystem.
    /// Manages fire detection, propagation, suppression, smoke, and CO hazards
    /// within shelter zones. Adapts Core authority for Godot UI and simulation wiring.
    /// </summary>
    public sealed class ShelterFireHostSession : HostSessionBase
    {
        public ShelterFireHazardSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public ShelterFireHostSession(ShelterFireHazardSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnFireIgnited += (incId, zoneId) =>
            {
                LastEvent = $"[Fire] Ignition in zone {zoneId} (Incident {incId})";
                RaiseStateChanged();
            };

            System.OnAlarmRaised += incId =>
            {
                LastEvent = $"[Fire] ALARM RAISED for incident {incId}";
                RaiseStateChanged();
            };

            System.OnIncidentSuppressed += incId =>
            {
                LastEvent = $"[Fire] Incident {incId} suppressed";
                RaiseStateChanged();
            };

            System.OnIncidentResolved += incId =>
            {
                LastEvent = $"[Fire] Incident {incId} resolved";
                RaiseStateChanged();
            };

            System.OnStateChanged += _ => RaiseStateChanged();
        }

        /// <summary>
        /// Resolves the primary active (unresolved) incident ID, or the first incident if any exists.
        /// Returns empty string if no incidents are registered.
        /// </summary>
        public string ResolveActiveIncidentId()
        {
            foreach (var kvp in System.Incidents)
            {
                if (!kvp.Value.isResolved)
                    return kvp.Key;
            }
            foreach (var kvp in System.Incidents)
            {
                return kvp.Key;
            }
            return string.Empty;
        }

        /// <summary>
        /// Advances every unresolved incident once for a campaign-day tick.
        /// Incident IDs are sorted so adding a new incident cannot change the
        /// RNG draw order of existing incidents.
        /// </summary>
        public int TickDay(int day, ISeededRng rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            var incidentIds = new List<string>();
            foreach (var pair in System.Incidents)
            {
                if (pair.Value != null && !pair.Value.isResolved)
                    incidentIds.Add(pair.Key);
            }
            incidentIds.Sort(StringComparer.Ordinal);

            for (int i = 0; i < incidentIds.Count; i++)
                System.Tick(incidentIds[i], rng);

            if (incidentIds.Count > 0)
                LastEvent = $"Fire incidents advanced for campaign day {day}: {incidentIds.Count} active.";
            return incidentIds.Count;
        }
    }
}
