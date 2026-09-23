// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Records;
using Ashfall.Core.Save;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// One canonical owner's contribution to a retention pass, plus what it did.
    /// </summary>
    public sealed class RetentionOwnerResult
    {
        public readonly string CollectionKey;
        public readonly int CountBefore;
        public readonly int CountAfter;
        public readonly int Pruned;

        public RetentionOwnerResult(string collectionKey, int countBefore, int countAfter, int pruned)
        {
            CollectionKey = collectionKey ?? string.Empty;
            CountBefore = countBefore;
            CountAfter = countAfter;
            Pruned = pruned;
        }
    }

    /// <summary>
    /// Plan 55 / Task 55A host session — binds the authored
    /// <c>retention_policies.json</c> table to the Core retention authority and
    /// applies that authority to the canonical owners that hold unbounded
    /// campaign logs.
    /// <para>
    /// Authority boundary (deliberate): this session owns NO log. Every
    /// collection stays with its existing owner — the kitchen serving log with
    /// <see cref="KitchenNutritionSystem"/>, the enacted-decree history with
    /// <see cref="YearOfAsh.FactionWarSystem"/>, the machine log with
    /// <see cref="Verdict.MachineLogSystem"/>, and the per-survivor dose reading
    /// history with <see cref="DoseLedgerSystem"/>. The session only asks the
    /// authority how much each owner may keep and asks the owner to apply it.
    /// There is no mirrored or second copy of any collection.
    /// </para>
    /// </summary>
    public sealed class RetentionHostSession : HostSessionBase
    {
        /// <summary>Authored policy table in the data authority.</summary>
        public const string PolicyFile = RetentionPolicyCatalogLoader.FileName;

        private readonly List<RetentionOwnerResult> _lastResults = new List<RetentionOwnerResult>();

        public RetentionPolicyCatalog Catalog { get; }
        public Ashfall.Core.KitchenNutritionSystem? Kitchen { get; set; }
        public Ashfall.Core.YearOfAsh.FactionWarSystem? FactionWar { get; set; }
        public Ashfall.Core.Verdict.MachineLogSystem? MachineLog { get; set; }
        public Ashfall.Core.DoseLedgerSystem? DoseLedger { get; set; }

        /// <summary>True when the authored table loaded and was overlaid.</summary>
        public bool UsingAuthoredPolicies { get; private set; }

        /// <summary>Overlay error, when the authored table was rejected.</summary>
        public string? LoadError { get; private set; }

        /// <summary>Total retention passes applied through this session.</summary>
        public int Passes { get; private set; }

        public IReadOnlyList<RetentionOwnerResult> LastResults => _lastResults;

        public RetentionHostSession(RetentionPolicyCatalog? catalog = null)
        {
            Catalog = catalog ?? new RetentionPolicyCatalog();
        }

        /// <summary>
        /// Load the authored policy table and overlay it on the built-in
        /// defaults. A missing or invalid table is a soft failure: the built-in
        /// defaults still bound every collection, so the campaign never runs
        /// unbounded because an authored file was incomplete.
        /// </summary>
        public void LoadAuthoredPolicies(string dataDirectory, IFileIO files)
        {
            var loaded = RetentionPolicyCatalogLoader.Load(dataDirectory, files);
            if (loaded.HasErrors)
            {
                UsingAuthoredPolicies = false;
                LoadError = string.Join("; ", loaded.Errors);
                return;
            }

            Catalog.ApplyOverlay(loaded.Policies);
            UsingAuthoredPolicies = true;
            LoadError = null;
            RaiseStateChanged();
        }

        /// <summary>
        /// Bind the canonical owners whose collections retention governs.
        /// Every parameter is nullable: an owner that is not constructed in a
        /// given campaign simply contributes nothing.
        /// </summary>
        public void BindOwners(
            Ashfall.Core.KitchenNutritionSystem? kitchen,
            Ashfall.Core.YearOfAsh.FactionWarSystem? factionWar,
            Ashfall.Core.Verdict.MachineLogSystem? machineLog,
            Ashfall.Core.DoseLedgerSystem? doseLedger)
        {
            Kitchen = kitchen;
            FactionWar = factionWar;
            MachineLog = machineLog;
            DoseLedger = doseLedger;
            RaiseStateChanged();
        }

        /// <summary>
        /// Apply retention across every bound owner. Owners that hold nothing
        /// (or whose collection is an iron-rule obligation) contribute a
        /// zero-pruned row so the audit report is a full picture.
        /// </summary>
        public RetentionAuditReport ApplyRetention()
        {
            _lastResults.Clear();
            var report = new RetentionAuditReport();

            ApplyOwner("kitchen_serving_log",
                Kitchen == null ? 0 : Kitchen.State.servingLog.Count,
                () => Kitchen?.ApplyRetention(Catalog) ?? 0);
            ApplyOwner("faction_war_decrees",
                FactionWar == null ? 0 : FactionWar.State.enactedDecrees.Count,
                () => FactionWar?.ApplyRetention(Catalog) ?? 0);
            ApplyOwner("machine_log",
                MachineLog == null ? 0 : MachineLog.Entries.Count,
                () => MachineLog?.ApplyRetention(Catalog) ?? 0);
            ApplyOwner("dose_ledger",
                DoseLedger == null ? 0 : CountDoseReadings(),
                () => DoseLedger?.ApplyRetention(Catalog) ?? 0);

            foreach (var result in _lastResults)
            {
                report.TotalCollectionsAudited++;
                if (result.Pruned > 0)
                {
                    report.TotalEntriesPruned += result.Pruned;
                    if (!report.PrunedCollectionKeys.Exists(
                        k => string.Equals(k, result.CollectionKey, StringComparison.OrdinalIgnoreCase)))
                    {
                        report.PrunedCollectionKeys.Add(result.CollectionKey);
                    }
                }
            }

            foreach (var policy in Catalog.ActivePolicies)
            {
                if (policy.IsProtectedObligation
                    && !report.ProtectedCollectionKeys.Exists(
                        k => string.Equals(k, policy.CollectionKey, StringComparison.OrdinalIgnoreCase)))
                {
                    report.ProtectedCollectionKeys.Add(policy.CollectionKey);
                    report.TotalProtectedCollectionsPreserved++;
                }
            }

            Passes++;
            RaiseStateChanged();
            return report;
        }

        private void ApplyOwner(string key, int countBefore, Func<int> apply)
        {
            int pruned = apply();
            int after = Math.Max(0, countBefore - pruned);
            _lastResults.Add(new RetentionOwnerResult(key, countBefore, after, pruned));
        }

        private int CountDoseReadings()
        {
            if (DoseLedger == null) return 0;
            int total = 0;
            foreach (var entry in DoseLedger.State.entries)
            {
                if (entry?.readingsHistory == null) continue;
                total += entry.readingsHistory.Count;
            }
            return total;
        }

        public string Describe()
        {
            string source = UsingAuthoredPolicies ? "authored overlay" : "built-in defaults";
            return $"retention: {Catalog.ActivePolicies.Count} polic(ies) from {source}; "
                + $"{Passes} pass(es) applied";
        }
    }

    /// <summary>Strict JSON projection of the retention audit section.</summary>
    public sealed class RetentionAuditState
    {
        public int schema_version { get; set; } = 1;
        public int passes { get; set; }
        public bool using_authored_policies { get; set; }
        public int total_collections_audited { get; set; }
        public int total_entries_pruned { get; set; }
        public int total_protected_collections_preserved { get; set; }
        public List<string> pruned_collection_keys { get; set; } = new List<string>();
        public List<string> protected_collection_keys { get; set; } = new List<string>();
    }

    public static class RetentionSaveStore
    {
        public const string FileName = "retention_save.json";
        public const string SectionName = "retention";

        private static readonly SaveStore<RetentionAuditState> s_store =
            SaveStoreHub.Checksummed<RetentionAuditState>(FileName, nameof(RetentionSaveStore));

        public static bool TrySave(RetentionAuditState state) => s_store.TrySave(state);
        public static RetentionAuditState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RetentionAuditState state) => s_store.CapturePersisted(state);
        public static RetentionAuditState? TryRestore(string json) => s_store.RestoreEnvelope(json);
        public static RetentionAuditState? TryRestoreBare(string json) => s_store.RestoreBare(json);

        /// <summary>
        /// Project the live audit report for persistence. Only the audit is
        /// persisted — never the log contents, which stay in their own sections.
        /// </summary>
        public static RetentionAuditState From(RetentionAuditReport report, int passes, bool usingAuthoredPolicies)
            => new RetentionAuditState
            {
                schema_version = 1,
                passes = passes,
                using_authored_policies = usingAuthoredPolicies,
                total_collections_audited = report.TotalCollectionsAudited,
                total_entries_pruned = report.TotalEntriesPruned,
                total_protected_collections_preserved = report.TotalProtectedCollectionsPreserved,
                pruned_collection_keys = report.PrunedCollectionKeys ?? new List<string>(),
                protected_collection_keys = report.ProtectedCollectionKeys ?? new List<string>()
            };
    }
}
