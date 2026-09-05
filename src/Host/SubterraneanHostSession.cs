// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Subterranean;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host session over <see cref="SubterraneanSystem"/> (Flagship XI —
    /// Plan 156) plus the expedition bridge: underground zones register as
    /// canonical expedition destinations (no second roster — the crew, time and
    /// inventory stay ExpeditionSystem's), and each underground expedition day
    /// feeds oxygen/collapse/flood context back into the network.
    /// </summary>
    public sealed class SubterraneanHostSession : HostSessionBase
    {
        public SubterraneanSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        private readonly Dictionary<string, ExpeditionDefinition> _undergroundDefs =
            new Dictionary<string, ExpeditionDefinition>(StringComparer.Ordinal);

        // Bridge ports (wired by Main).
        private Func<IReadOnlyDictionary<string, ExpeditionState>>? _activeExpeditions;
        private Action<string, float>? _applyMoraleDelta;
        private Func<string, bool>? _hasClaustrophobia;

        public SubterraneanHostSession(SubterraneanSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnCaveIn += (nodeId, day) =>
            {
                LastEvent = $"A section of {NodeName(nodeId)} has come down.";
                RaiseStateChanged();
            };
            System.OnNodeFlooded += (nodeId, day) =>
            {
                LastEvent = $"Water has taken {NodeName(nodeId)}.";
                RaiseStateChanged();
            };
            System.OnForcedRetreat += (survivorId, nodeId, day) =>
            {
                LastEvent = $"{survivorId} cannot breathe down there any longer — turning back.";
                RaiseStateChanged();
            };
            System.OnNodeDiscovered += nodeId =>
            {
                LastEvent = $"Found a way down: {NodeName(nodeId)}.";
                RaiseStateChanged();
            };
        }

        private string NodeName(string nodeId) =>
            System.Zones.TryGetValue(nodeId, out var zone) ? zone.display_name : nodeId;

        /// <summary>Wires the expedition bridge ports (called by Main after setup).</summary>
        public void ConfigureBridge(
            Func<IReadOnlyDictionary<string, ExpeditionState>> activeExpeditions,
            Action<string, float> applyMoraleDelta,
            Func<string, bool> hasClaustrophobia)
        {
            _activeExpeditions = activeExpeditions;
            _applyMoraleDelta = applyMoraleDelta;
            _hasClaustrophobia = hasClaustrophobia;
        }

        // --------------------------------------------------------------- bridge

        /// <summary>
        /// Registers every catalog zone as a canonical expedition destination
        /// (156.6): distance by depth tier, danger and encounter risk from the
        /// zone's abstract risk, loot through the zone's scavenging table.
        /// Idempotent.
        /// </summary>
        public void RegisterUndergroundExpeditions()
        {
            foreach (var zone in System.Zones.Values.OrderBy(z => z.id, StringComparer.Ordinal))
            {
                if (_undergroundDefs.ContainsKey(zone.id)) continue;
                var def = new ExpeditionDefinition
                {
                    id = zone.id,
                    displayName = zone.display_name,
                    distanceTicks = 3 + zone.depth_tier * 2,
                    dangerLevel = Math.Clamp((int)MathF.Round(zone.base_structural_risk * 4f) + 1, 1, 5),
                    encounterChancePerTick = 0.08f + zone.base_structural_risk * 0.2f,
                    scavenging_table_id = zone.scavenging_table_id,
                    requiresDiscovery = true
                };
                _undergroundDefs[zone.id] = def;
                ExpeditionDefinitionRegistry.Register(def);
            }
        }

        /// <summary>Surface anchor reached → reveal its tier-1 nodes (and one step down if already known).</summary>
        public void OnSurfaceLocationReached(string surfaceLocationId)
        {
            foreach (var nodeId in System.DiscoverFromAnchor(surfaceLocationId))
            {
                LastEvent = $"Found a way down: {NodeName(nodeId)}.";
                RaiseStateChanged();
            }
        }

        /// <summary>
        /// One day of bridge work, run from the subterranean day owner AFTER the
        /// expeditions tick (156.6/156.7/156.14/156.15): environmental hazards for
        /// every underground expedition, claustrophobia morale through the
        /// canonical needs authority, and forced retreats through the engine.
        /// </summary>
        public void TickDay(int day, Action<string>? requestRetreat)
        {
            if (_activeExpeditions == null) return;

            var underground = new List<(string survivorId, string nodeId)>();
            foreach (var pair in _activeExpeditions().OrderBy(p => p.Key, StringComparer.Ordinal))
            {
                var state = pair.Value;
                if (state == null || string.IsNullOrEmpty(state.locationId)) continue;
                if (!System.Zones.ContainsKey(state.locationId)) continue;
                var phase = (ExpeditionPhase)state.phase;
                if (phase == ExpeditionPhase.Completed || phase == ExpeditionPhase.Failed) continue;
                underground.Add((pair.Key, state.locationId));
            }
            if (underground.Count == 0) return;

            // Claustrophobia: canonical morale modifier; contagion spreads the rest.
            if (_applyMoraleDelta != null)
            {
                foreach (var (survivorId, nodeId) in underground.OrderBy(o => o.survivorId, StringComparer.Ordinal))
                {
                    float delta = System.UndergroundMoraleDelta(nodeId, _hasClaustrophobia?.Invoke(survivorId) ?? false);
                    if (delta < 0f) _applyMoraleDelta(survivorId, delta);
                }
            }

            string? retreating = null;
            System.OnForcedRetreat += OnForcedRetreatHandler;
            void OnForcedRetreatHandler(string survivorId, string nodeId, int d)
            {
                retreating ??= survivorId; // one forced retreat per day, ordinal-first
            }

            try
            {
                System.ApplyUndergroundDay(day, underground);
            }
            finally
            {
                System.OnForcedRetreat -= OnForcedRetreatHandler;
            }

            if (retreating != null)
            {
                requestRetreat?.Invoke(retreating);
                LastEvent = $"{retreating} surfaces early — the air down there had run thin.";
                RaiseStateChanged();
            }
        }

        // ------------------------------------------------------------- commands

        public string ShoreNode(string nodeId)
        {
            var error = System.TryShoreNode(nodeId);
            RaiseStateChanged();
            return error == null
                ? $"Timber and steel set into {NodeName(nodeId)}. It will hold a while longer."
                : error switch
                {
                    "missing_materials" => "Not enough materials: 4 scrap wood, 2 steel rebar (plus 2 scrap metal for a second set).",
                    "shoring_maxed" => $"{NodeName(nodeId)} is as shored as timber can make it.",
                    _ => "That passage is not on any map we hold."
                };
        }

        public string InstallVentilation(string nodeId)
        {
            var error = System.TryInstallVentilation(nodeId);
            RaiseStateChanged();
            return error == null
                ? $"A pump line now breathes for {NodeName(nodeId)}."
                : error switch
                {
                    "missing_materials" => "Not enough materials: 3 scrap metal, 1 battery.",
                    "already_installed" => $"{NodeName(nodeId)} already has a pump line.",
                    _ => "That passage is not on any map we hold."
                };
        }

        public string ClearBlockage(string nodeId)
        {
            var error = System.TryClearBlockage(nodeId);
            RaiseStateChanged();
            return error == null
                ? $"{NodeName(nodeId)} is passable again."
                : error switch
                {
                    "missing_materials" => "Not enough materials: 2 scrap wood.",
                    "structure_unsound" => $"No — {NodeName(nodeId)} would come down on whoever dug.",
                    "not_blocked" => $"{NodeName(nodeId)} is already open.",
                    _ => "That passage is not on any map we hold."
                };
        }

        // ------------------------------------------------------------ day/save

        public override void Save()
        {
            SubterraneanSaveStore.TrySave(SubterraneanSaveCodec.ToSaveState(System.CaptureState()));
        }

        public SubterraneanSaveState CaptureSave() =>
            SubterraneanSaveCodec.ToSaveState(System.CaptureState());

        public void RestoreSave(SubterraneanSaveState save)
        {
            if (save == null) return;
            System.RestoreState(SubterraneanSaveCodec.FromSaveState(save));
            LastEvent = "The under-map is as we left it.";
            RaiseStateChanged();
        }
    }
}
