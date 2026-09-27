// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 20 — The Network (Plan 146 batch-4 / ORPHAN-SEAL A.83)
// Subsystem    : Informant network tradecraft ledger (stateful owner over the
//                static tradecraft engine). One authority for informant field
//                ops; the counter-intelligence system keeps shelter-side
//                vetting/detention and is not touched.
// Determinism  : rolls hash (worldSeed, informantId, day) via StableHash —
//                replayable; no System.Random.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Espionage
{
    [Serializable]
    public sealed class InformantNetworkState
    {
        public int schemaVersion = 1;
        public int lastProcessedDay = -1;
        public int totalIntelPoints = 0;
        public List<InformantRecord> informants = new List<InformantRecord>();
    }

    /// <summary>
    /// Stateful owner for the shelter's recruited informant network: field
    /// tradecraft (dead drops, briefings, radio bursts, couriers), counter-
    /// intelligence sweeps against compromised assets, and interrogation
    /// doctrine for captives. Mutates only informant records owned here.
    /// </summary>
    public sealed class InformantNetworkSystem
    {
        public const string SystemId = "informant_network";

        private readonly InformantNetworkState _state = new InformantNetworkState();

        public InformantNetworkState State => _state;

        public InformantNetworkSystem() { }

        public InformantNetworkSystem(InformantNetworkState? state)
        {
            if (state != null) _state = state;
        }

        // ------------------------------------------------------------- roster

        public InformantRecord? GetInformant(string informantId) =>
            _state.informants.Find(i => string.Equals(i.InformantId, informantId, StringComparison.Ordinal));

        public IReadOnlyList<InformantRecord> Active => _state.informants.Where(i => !i.IsCompromised).ToList();

        public InformantRecord Recruit(
            string informantId, string targetFactionId, InformantArchetype archetype,
            TradecraftMethod method, int loyaltyPermille = 700, int suspicionPermille = 100,
            int yieldPermille = 500)
        {
            var existing = GetInformant(informantId);
            if (existing != null) return existing; // idempotent recruit

            var record = new InformantRecord
            {
                InformantId = informantId,
                TargetFactionId = targetFactionId,
                Archetype = archetype,
                ActiveMethod = method,
                LoyaltyPermille = loyaltyPermille,
                SuspicionPermille = suspicionPermille,
                IntelligenceYieldPermille = yieldPermille
            };
            _state.informants.Add(record);
            return record;
        }

        public bool Retire(string informantId)
        {
            var record = GetInformant(informantId);
            if (record == null) return false;
            return _state.informants.Remove(record);
        }

        // ---------------------------------------------------------- operations

        /// <summary>Runs one field tradecraft op; mutates the informant record.</summary>
        public TradecraftOperationResult RunOperation(string informantId, int day, int worldSeed)
        {
            var informant = GetInformant(informantId);
            if (informant == null) return new TradecraftOperationResult(false, 0, 0, false, false);
            var result = InformantNetworkTradecraftEngine.ExecuteTradecraftOperation(informant, day, worldSeed);
            if (result.Success) _state.totalIntelPoints += result.IntelPointsDelivered;
            _state.lastProcessedDay = day;
            return result;
        }

        /// <summary>
        /// Daily drift: suspicion eases slightly for laid-low assets, loyalty
        /// decays for unpaid mercenary brokers. Ordinal order, deterministic.
        /// </summary>
        public void TickDay(int day)
        {
            foreach (var informant in _state.informants.OrderBy(i => i.InformantId, StringComparer.Ordinal))
            {
                if (informant.IsCompromised) continue;
                int relief = informant.ActiveMethod switch
                {
                    TradecraftMethod.DeadDrop => 5,
                    TradecraftMethod.CutoutCourier => 4,
                    TradecraftMethod.RadioBurstTransmission => 3,
                    _ => 2
                };
                informant.SuspicionPermille = Math.Max(0, informant.SuspicionPermille - relief);
                if (informant.Archetype == InformantArchetype.MercenaryBroker)
                    informant.LoyaltyPermille = Math.Max(0, informant.LoyaltyPermille - 2);
            }
            _state.lastProcessedDay = day;
        }

        // -------------------------------------------------------------- sweeps

        public bool RunCounterIntelSweep(string informantId, int shelterRatingPermille, int day, int worldSeed)
        {
            var informant = GetInformant(informantId);
            if (informant == null) return false;
            return InformantNetworkTradecraftEngine.DetectCompromisedAsset(informant, shelterRatingPermille, day, worldSeed);
        }

        // ------------------------------------------------------- interrogation

        public InterrogationOutcome InterrogateCaptive(string informantId, bool humaneProtocolsEnforced)
        {
            var informant = GetInformant(informantId);
            if (informant == null)
                return new InterrogationOutcome(false, 0, 0, false);
            return InformantNetworkTradecraftEngine.EvaluateInterrogation(informant, humaneProtocolsEnforced);
        }

        // -------------------------------------------------------------- custody

        public InformantNetworkState CaptureState() => Clone(_state);

        public void RestoreState(InformantNetworkState? state)
        {
            if (state == null) return; // legacy save tolerance
            _state.schemaVersion = state.schemaVersion;
            _state.lastProcessedDay = state.lastProcessedDay;
            _state.totalIntelPoints = state.totalIntelPoints;
            _state.informants = state.informants?.Select(r => r?.Clone() ?? new InformantRecord()).ToList()
                                ?? new List<InformantRecord>();
        }

        private static InformantNetworkState Clone(InformantNetworkState src) => new InformantNetworkState
        {
            schemaVersion = src.schemaVersion,
            lastProcessedDay = src.lastProcessedDay,
            totalIntelPoints = src.totalIntelPoints,
            informants = src.informants.Select(r => r.Clone()).ToList()
        };
    }
}
