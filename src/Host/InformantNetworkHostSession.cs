// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : InformantNetworkHostSession (Plan 146 batch-4 / A.83)
// Core         : Ashfall.Core.Espionage.InformantNetworkSystem (ledger owner)
//                + InformantNetworkTradecraftEngine (resolution authority)
// Save         : own checksummed section `informant_network`
// Consumer     : Main.FactionBranch (day owner + readout), probe selftest
// ============================================================================

using System;
using Ashfall.Core.Espionage;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class InformantNetworkSaveStore
    {
        public const string FileName = "informant_network_save.json";
        public const string SectionName = "informant_network";

        private static readonly SaveStore<InformantNetworkState> s_store =
            SaveStoreHub.Checksummed<InformantNetworkState>(FileName, nameof(InformantNetworkSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(InformantNetworkState state) => s_store.TrySave(state);
        public static InformantNetworkState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(InformantNetworkState state) => s_store.CaptureBare(state);
    }

    /// <summary>
    /// Thin host layer over the informant-network ledger owner. Raises
    /// StateChanged on every mutation so bound panels refresh truthfully.
    /// </summary>
    public sealed class InformantNetworkHostSession : HostSessionBase
    {
        public InformantNetworkSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public InformantNetworkHostSession(InformantNetworkSystem? system = null)
        {
            System = system ?? new InformantNetworkSystem();
        }

        public static InformantNetworkHostSession Create()
        {
            var session = new InformantNetworkHostSession();
            if (InformantNetworkSaveStore.TryLoad() is { } loaded)
            {
                session.System.RestoreState(loaded);
                session.LastEvent = "Informant network restored from save.";
            }
            return session;
        }

        public InformantRecord Recruit(
            string informantId, string targetFactionId, InformantArchetype archetype,
            TradecraftMethod method, int loyaltyPermille = 700, int suspicionPermille = 100,
            int yieldPermille = 500)
        {
            var record = System.Recruit(informantId, targetFactionId, archetype, method, loyaltyPermille, suspicionPermille, yieldPermille);
            LastEvent = $"Recruited {informantId} ({archetype}) against {targetFactionId}.";
            MarkDirty();
            return record;
        }

        public TradecraftOperationResult RunOperation(string informantId, int day, int worldSeed)
        {
            var result = System.RunOperation(informantId, day, worldSeed);
            LastEvent = result.Success
                ? $"Dead-drop run for {informantId} delivered {result.IntelPointsDelivered} intel."
                : result.InterceptedByEnemy
                    ? $"Operation for {informantId} intercepted — suspicion +{result.SuspicionDeltaPermille}."
                    : $"Operation for {informantId} aborted.";
            MarkDirty();
            return result;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            LastEvent = $"Informant network settled into day {day}.";
            MarkDirty();
        }

        public bool RunCounterIntelSweep(string informantId, int shelterRatingPermille, int day, int worldSeed)
        {
            bool detected = System.RunCounterIntelSweep(informantId, shelterRatingPermille, day, worldSeed);
            LastEvent = detected
                ? $"Counter-intel sweep burned {informantId}."
                : $"Sweep on {informantId} came back clean.";
            MarkDirty();
            return detected;
        }

        public InterrogationOutcome InterrogateCaptive(string informantId, bool humaneProtocolsEnforced)
        {
            var outcome = System.InterrogateCaptive(informantId, humaneProtocolsEnforced);
            LastEvent = humaneProtocolsEnforced
                ? (outcome.ReliableIntelligenceObtained ? "Humane interrogation produced reliable intelligence." : "Humane interrogation stalled.")
                : "Coercive interrogation — fabricated intelligence warning.";
            MarkDirty();
            return outcome;
        }

        public string Readout()
        {
            int total = System.State.informants.Count;
            int active = 0;
            foreach (var informant in System.State.informants)
                if (!informant.IsCompromised) active++;
            int burned = total - active;
            return $"Informants active {active} (burned {burned}) · intel banked {System.State.totalIntelPoints}";
        }

    }
}
