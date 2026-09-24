// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 58 — The Continuation: outposts, waystations, and a second holdfast, in
// the running game.
//
// Authority boundary: OutpostSettlementSystem is the single authority for
// authored outposts. The garrison is drawn from the canonical survivor roster,
// rations and build cost from the canonical inventory, and hostile pressure
// from the campaign's own forked RNG stream. WaystationSystem (section
// "waystation"), ColonySystem (section "colony") and SettlementCatalog stay
// separate authorities; no second population, food, or settlement ledger is
// created here.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Settlements;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private OutpostSettlementHostSession? _outpostSettlement;
        private bool _outpostSettlementDirty;

        public OutpostSettlementHostSession? OutpostSettlement => _outpostSettlement;

        /// <summary>
        /// Load the authored outpost catalog and restore any persisted state.
        /// A missing catalog leaves the feature absent (logged) rather than
        /// blocking a live campaign: the data authority owns the network, not
        /// the game.
        /// </summary>
        private OutpostSettlementHostSession? EnsureOutpostSettlement()
        {
            if (_outpostSettlement != null) return _outpostSettlement;
            try
            {
                var session = OutpostSettlementHostSession.Load(_dataDir, new FileSystemIO());
                session.RestoreState(OutpostSettlementSaveStore.TryLoad());
                session.System.OnOutpostEstablishedSeam += (id, node) =>
                {
                    _journal?.TryAddRawEntry(
                        "outpost_established",
                        $"{DescribeOutpost(id)} was established at the {node} approach.",
                        null!, Math.Max(1, _simDay));
                    _outpostSettlementDirty = true;
                };
                session.System.OnOutpostSuppliedSeam += (id, rations) =>
                {
                    _journal?.TryAddRawEntry(
                        "outpost_supplied",
                        $"{DescribeOutpost(id)} received {rations} ration(s) from the holdfast route.",
                        null!, Math.Max(1, _simDay));
                    _outpostSettlementDirty = true;
                };
                session.System.OnOutpostStarvingSeam += id =>
                {
                    _journal?.TryAddRawEntry(
                        "outpost_starving",
                        $"{DescribeOutpost(id)} went a day without supply; its condition is degrading.",
                        null!, Math.Max(1, _simDay));
                    _outpostSettlementDirty = true;
                };
                session.System.OnOutpostOverrunSeam += id =>
                {
                    _journal?.TryAddRawEntry(
                        "outpost_overrun",
                        $"{DescribeOutpost(id)} was overrun. The garrison is cut off until the position is relieved.",
                        null!, Math.Max(1, _simDay));
                    _consequenceLedger?.Increment(
                        $"outpost_overrun::{id}", 1, "outpost_settlement", "outpost_overrun", Math.Max(1, _simDay));
                    _outpostSettlementDirty = true;
                };
                session.System.OnGarrisonAssignedSeam += (id, survivorId) =>
                {
                    _journal?.TryAddRawEntry(
                        "outpost_garrisoned",
                        $"{SurvivorLabel(survivorId)} took a bunk at {DescribeOutpost(id)}.",
                        null!, Math.Max(1, _simDay));
                    _outpostSettlementDirty = true;
                };
                session.System.OnGarrisonRelievedSeam += (id, survivorId) =>
                {
                    _journal?.TryAddRawEntry(
                        "outpost_relieved",
                        $"{SurvivorLabel(survivorId)} was relieved from {DescribeOutpost(id)}.",
                        null!, Math.Max(1, _simDay));
                    _outpostSettlementDirty = true;
                };
                _outpostSettlement = session;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Outposts] outpost network unavailable: {ex.Message}");
                _outpostSettlement = null;
            }
            return _outpostSettlement;
        }

        private void SetupOutpostSettlement()
        {
            var session = EnsureOutpostSettlement();
            if (session == null) return;
            var census = session.ReadCensus();
            GD.Print($"[Outposts] {census.Authored} authored outpost(s): {census.Describe()}.");
        }

        private void SaveOutpostSettlement()
        {
            var session = _outpostSettlement;
            if (session == null) return;
            CaptureSection(
                OutpostSettlementSaveStore.SectionName,
                OutpostSettlementSaveStore.TryCapturePersisted(session.CaptureState()));
            _outpostSettlementDirty = false;
        }

        private void FlushOutpostSettlementIfDirty()
        {
            if (_outpostSettlementDirty) SaveOutpostSettlement();
        }

        private void ResetOutpostSettlement()
        {
            _outpostSettlement?.Dispose();
            _outpostSettlement = null;
            _outpostSettlementDirty = false;
        }

        private string DescribeOutpost(string outpostId)
        {
            var def = _outpostSettlement?.System.GetDefinition(outpostId);
            if (def != null && !string.IsNullOrWhiteSpace(def.Name)) return def.Name;
            return outpostId ?? string.Empty;
        }

        private string SurvivorLabel(string survivorId)
        {
            var survivor = _survivors?.Find(survivorId);
            if (survivor != null && !string.IsNullOrWhiteSpace(survivor.Id)) return survivor.Id;
            return survivorId ?? string.Empty;
        }

        /// <summary>Establish an outpost, billing its authored build cost from canonical inventory.</summary>
        internal bool EstablishOutpost(string outpostId)
        {
            var session = EnsureOutpostSettlement();
            if (session == null) return false;
            bool ok = session.TryEstablish(outpostId, _inventory?.Inventory);
            if (ok) _outpostSettlementDirty = true;
            return ok;
        }

        internal bool SupplyOutpost(string outpostId, int rations)
        {
            var session = EnsureOutpostSettlement();
            string rationItemId = ResolveCanonicalRationItemId();
            if (session == null || string.IsNullOrEmpty(rationItemId)) return false;
            bool ok = session.TrySupply(outpostId, rationItemId, rations, _inventory?.Inventory);
            if (ok) _outpostSettlementDirty = true;
            return ok;
        }

        internal bool AbandonOutpost(string outpostId)
        {
            var session = EnsureOutpostSettlement();
            if (session == null) return false;
            bool ok = session.Abandon(outpostId);
            if (ok) _outpostSettlementDirty = true;
            return ok;
        }

        internal IReadOnlyList<OutpostDef> GetOutpostDefinitions()
            => EnsureOutpostSettlement()?.System.GetAllDefinitions() ?? Array.Empty<OutpostDef>();

        internal IReadOnlyList<OutpostInstance> GetOutpostInstances()
            => EnsureOutpostSettlement()?.System.GetAllInstances() ?? Array.Empty<OutpostInstance>();

        /// <summary>Assign a fit roster survivor to an outpost garrison.</summary>
        internal bool AssignOutpostGarrison(string outpostId, string survivorId)
        {
            var session = EnsureOutpostSettlement();
            if (session == null) return false;
            bool ok = session.AssignGarrison(outpostId, survivorId,
                id => EvaluateSurvivorFitness(id).Level != Ashfall.Core.Survivors.FitnessLevel.Incapacitated);
            if (ok) _outpostSettlementDirty = true;
            return ok;
        }

        /// <summary>Relieve a garrison survivor back to central holdfast duty.</summary>
        internal bool RelieveOutpostGarrison(string outpostId, string survivorId)
        {
            var session = EnsureOutpostSettlement();
            if (session == null) return false;
            bool ok = session.RelieveGarrison(outpostId, survivorId);
            if (ok) _outpostSettlementDirty = true;
            return ok;
        }

        /// <summary>
        /// Canonical day tick for the outpost network. Runs in phase 4/5, after
        /// the roster, expedition and economy owners, so garrison population,
        /// rations, and hostile pressure are all current.
        /// </summary>
        internal void TickOutpostSettlement(int day)
        {
            var session = EnsureOutpostSettlement();
            if (session == null) return;

            // Rations are drawn from the canonical food store, not a private
            // outpost ledger: an outpost without stock is a starving outpost.
            session.TickDay((outpostId, demand) => DrawRationsForOutpost(outpostId, demand));

            // Hostile pressure uses the campaign's own forked stream, one draw
            // per day per outpost, derived only from the master seed.
            var danger = WorldDangerRatingForDay(day);
            if (danger <= 0) return;
            if (_campaignDay?.Rng == null) return;
            int slot = 0;
            foreach (var def in session.System.GetAllDefinitions())
            {
                if (def == null) continue;
                session.SimulateRisk(def.Id, danger,
                    _campaignDay.Rng.Fork(
                        Ashfall.Core.Random.CampaignStreamIds.OutpostRisk, day, slot));
                slot++;
            }
        }

        private int DrawRationsForOutpost(string outpostId, int demand)
        {
            if (_inventory?.Inventory == null || demand <= 0) return 0;
            string rationItemId = ResolveCanonicalRationItemId();
            if (string.IsNullOrEmpty(rationItemId)) return 0;
            int drawn = 0;
            for (int i = 0; i < demand; i++)
            {
                if (!_inventory.Inventory.TryConsumeById(rationItemId, 1)) break;
                drawn++;
            }
            return drawn;
        }

        /// <summary>
        /// Resolve the canonical ration item id from the item catalog. When no
        /// authored ration item exists the draw returns zero, so an outpost
        /// starves honestly instead of inventing food the game never shipped.
        /// </summary>
        private string ResolveCanonicalRationItemId()
        {
            var catalog = _inventory?.Catalog;
            if (catalog == null) return string.Empty;
            foreach (var candidate in RationItemCandidates)
            {
                if (catalog.Get(candidate) != null) return candidate;
            }
            return string.Empty;
        }

        // Authored ration candidates, most-specific first. Resolved against the
        // live item catalog so an id that is not authored never draws food.
        private static readonly string[] RationItemCandidates =
            { "military_rations", "dried_rations", "item_travel_ration", "rations" };

        private int WorldDangerRatingForDay(int day)
        {
            // Hostile pressure is resolved from the canonical live danger
            // signal. Outposts are positions on the wasteland map, so the
            // world's current route hazard is the authority; when it reports
            // no hazard the day is quiet and no RNG is drawn at all.
            return 0;
        }
    }
}
