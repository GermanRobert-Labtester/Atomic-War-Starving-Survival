// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Settlements;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 58 / The Continuation — host session for the authored outpost and
    /// secondary-holdfast network.
    /// <para>
    /// Custody boundary (signed premise, recorded in
    /// <c>docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md</c> §2):
    /// <list type="bullet">\n    /// <item><see cref=\"OutpostSettlementSystem\"/> is the single authority for\n    /// authored outposts and secondary positions.</item>\n    /// <item><c>WaystationSystem</c> (save section <c>waystation</c>) remains the\n    /// holdfast S2 waystation authority — separate, not merged.</item>\n    /// <item><c>ColonySystem</c> (section <c>colony</c>) remains the player-founded\n    /// colony authority — separate, not merged.</item>\n    /// <item><c>SettlementCatalog</c> remains the world settlement catalog — a\n    /// read-only graph/geography reference for an outpost's parent node.</item>\n    /// </list>\n    /// No second population, food, inventory, or settlement ledger is created
    /// here: the session draws garrison from the canonical roster and rations
    /// from the canonical inventory through caller-supplied providers, and it
    /// stores only outpost state (establishment, condition, garrison ids, supply
    /// reserve, starvation/overrun flags).\n    /// </para>\n    /// </summary>
    public sealed class OutpostSettlementHostSession : HostSessionBase
    {
        /// <summary>Authored outpost catalog in the data authority.</summary>
        public const string CatalogFile = "outposts.json";

        private readonly List<string> _rawJsonLines = new List<string>();

        public OutpostSettlementSystem System { get; }

        /// <summary>True when the authored catalog loaded through the Core parser.</summary>
        public bool CatalogLoaded { get; private set; }

        /// <summary>Constructor for a live, bound outpost session.</summary>
        public OutpostSettlementHostSession(OutpostSettlementSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        /// <summary>
        /// Load the authored outpost catalog. A missing or malformed file is a
        /// hard failure: an outpost that is not authored is not invented here.
        /// </summary>
        public static OutpostSettlementHostSession Load(string dataDirectory, IFileIO files)
        {
            string path = Path.Combine(dataDirectory, CatalogFile);
            if (files == null || !files.FileExists(path))
                throw new InvalidOperationException(
                    $"OutpostSettlementHostSession: {CatalogFile} does not exist at '{path}'.");

            var system = OutpostSettlementSystem.FromJson(files.ReadAllText(path));
            if (system.GetAllDefinitions().Count == 0)
                throw new InvalidOperationException(
                    $"OutpostSettlementHostSession: {CatalogFile} defines no outposts.");

            return new OutpostSettlementHostSession(system);
        }

        /// <summary>
        /// A census of the current network. Read-only: no state is mutated, so
        /// this is safe to call from a UI projection at any time.
        /// </summary>
        public OutpostCensus ReadCensus()
        {
            int established = 0, overrun = 0, starving = 0, garrisoned = 0, rations = 0;
            foreach (var inst in System.GetAllInstances())
            {
                if (inst == null) continue;
                if (inst.IsEstablished) established++;
                if (inst.IsOverrun) overrun++;
                if (inst.IsStarving) starving++;
                garrisoned += inst.GarrisonSurvivorIds?.Count ?? 0;
                rations += inst.RationReserve;
            }

            return new OutpostCensus(
                System.GetAllDefinitions().Count,
                established,
                overrun,
                starving,
                garrisoned,
                rations);
        }

        /// <summary>Establish an outpost, consuming build cost through the provider.</summary>
        public bool Establish(string outpostId, Func<string, int, bool>? costConsumer = null)
            => System.EstablishOutpost(outpostId, costConsumer);

        /// <summary>Assign a roster survivor to an outpost garrison.</summary>
        public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null)
            => System.AssignGarrison(outpostId, survivorId, fitnessCheck);

        /// <summary>Relieve a garrison survivor back to central holdfast duty.</summary>
        public bool RelieveGarrison(string outpostId, string survivorId)
            => System.RelieveGarrison(outpostId, survivorId);

        /// <summary>Deliver rations to an outpost reserve.</summary>
        public bool Supply(string outpostId, int rationsDelivered)
            => System.SupplyOutpost(outpostId, rationsDelivered);

        /// <summary>Abandon an established outpost.</summary>
        public bool Abandon(string outpostId) => System.AbandonOutpost(outpostId ?? string.Empty);

        /// <summary>Advance the daily outpost lifecycle.</summary>
        public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null)
        {
            System.TickDay(centralRationSupplyProvider);
            RaiseStateChanged();
        }

        /// <summary>Resolve hostile pressure against one outpost.</summary>
        public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng)
            => System.SimulateRisk(outpostId ?? string.Empty, dangerRating, rng);

        /// <summary>Capture the outpost section (definitions are never persisted).</summary>
        public OutpostSettlementState CaptureState() => System.CaptureState();

        /// <summary>Restore the outpost section over the live instances.</summary>
        public bool RestoreState(OutpostSettlementState? state) => System.RestoreState(state);

        /// <summary>Append one diegetic line for the host report (bounded).</summary>
        public void AppendReportLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            _rawJsonLines.Add(line);
            RaiseStateChanged();
        }

        public IReadOnlyList<string> ReportLines => _rawJsonLines;

        public string Describe() => ReadCensus().Describe();
    }

    /// <summary>Read-model census of the outpost network. No mutable state.</summary>
    public readonly struct OutpostCensus
    {
        public readonly int Authored;
        public readonly int Established;
        public readonly int Overrun;
        public readonly int Starving;
        public readonly int Garrisoned;
        public readonly int RationReserve;

        public OutpostCensus(int authored, int established, int overrun, int starving,
            int garrisoned, int rationReserve)
        {
            Authored = authored;
            Established = established;
            Overrun = overrun;
            Starving = starving;
            Garrisoned = garrisoned;
            RationReserve = rationReserve;
        }

        public string Describe()
            => $"outposts: {Established}/{Authored} established, {Garrisoned} garrisoned, "
                + $"{RationReserve} ration(s) in reserve, {Overrun} overrun, {Starving} starving";
    }

    public static class OutpostSettlementSaveStore
    {
        public const string FileName = "outpost_settlement_save.json";
        public const string SectionName = "outpost_settlement";

        private static readonly SaveStore<OutpostSettlementState> s_store =
            SaveStoreHub.Checksummed<OutpostSettlementState>(FileName, nameof(OutpostSettlementSaveStore));

        public static bool TrySave(OutpostSettlementState state) => s_store.TrySave(state);
        public static OutpostSettlementState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(OutpostSettlementState state) => s_store.CapturePersisted(state);
        public static OutpostSettlementState? TryRestore(string json) => s_store.RestoreEnvelope(json);
        public static OutpostSettlementState? TryRestoreBare(string json) => s_store.RestoreBare(json);
    }
}
