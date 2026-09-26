// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : GenealogySaveStore
// Core State : Ashfall.Core.LineageState (kinship facts only)
// Host Caller: Main.Genealogy
// Purpose    : Plan 217 — Survivor Genealogy & Family Tree host session.
//              Authority boundaries (plan revision 2026-09-24): the roster and
//              ChildDevelopment own birth identity; RomanceFamilySystem owns
//              family units and bonds; SurvivorFate owns death. THIS authority
//              only records committed kinship FACTS (parent/child links,
//              unions, deaths) fed from canonical events — it never infers
//              kinship from names and never creates a second family-unit
//              ledger (no FormFamilyUnit calls on the host path).
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Persisted genealogy envelope — kinship records + family events.</summary>
    public sealed class GenealogySaveState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<GenealogyLineageRecordDto> Lineages { get; set; } = new();
        public List<GenealogyFamilyEventDto> FamilyEvents { get; set; } = new();
    }

    public sealed class GenealogyLineageRecordDto
    {
        public string ParentId { get; set; } = string.Empty;
        public string ChildId { get; set; } = string.Empty;
        public string RelationshipType { get; set; } = string.Empty;
        public int EstablishedDay { get; set; }
        public bool IsActive { get; set; } = true;
        public string SpouseId { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
    }

    public sealed class GenealogyFamilyEventDto
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Day { get; set; }
        public List<string> ParticipantIds { get; set; } = new();
        public string Description { get; set; } = string.Empty;
        public string Significance { get; set; } = "moderate";
    }

    public static class GenealogySaveStore
    {
        public const string FileName = "genealogy_save.json";
        public const string SectionName = "genealogy";

        private static readonly SaveStore<GenealogySaveState> s_store =
            SaveStoreHub.Checksummed<GenealogySaveState>(FileName, nameof(GenealogySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(GenealogySaveState state) => s_store.CaptureBare(state);
        public static GenealogySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(GenealogySaveState state) => s_store.TrySave(state);
        public static GenealogySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Read-only family projection consumed by the survivor detail surface.</summary>
    public sealed class GenealogySnapshot
    {
        public int LineageCount { get; set; }
        public int UnionCount { get; set; }
        public int EventCount { get; set; }
        public List<(string EventType, int Day, string Description)> RecentEvents { get; } = new();
    }

    /// <summary>
    /// Plan 217 host session. Wraps the Core <see cref="GenealogyBridge"/> over
    /// the hosted <see cref="GenerationalSuccessionEngine"/> (shared, not forked)
    /// through a <see cref="GenerationalLineageExtension"/>. Records only
    /// committed kinship facts from canonical producers; the read-only family
    /// projection derives parents/spouse/children from the lineage records.
    /// </summary>
    public sealed class GenealogyHostSession : HostSessionBase
    {
        private readonly GenerationalLineageExtension _lineage;
        private readonly GenealogyBridge _bridge;

        public GenerationalLineageExtension Lineage => _lineage;
        public GenealogyBridge Bridge => _bridge;
        public string LastEvent { get; private set; } = string.Empty;

        public GenealogyHostSession(GenerationalSuccessionEngine successionEngine)
        {
            _lineage = new GenerationalLineageExtension(successionEngine ?? new GenerationalSuccessionEngine());
            _bridge = new GenealogyBridge(_lineage);
            _bridge.OnGenealogyEventLogged += (eventType, dwellerId, description) =>
            {
                LastEvent = $"Genealogy {eventType}: {description}";
                RaiseStateChanged();
            };
            _lineage.OnLineageEstablished += (_, _) =>
            {
                LastEvent = "Lineage established.";
                RaiseStateChanged();
            };
            _lineage.OnSpouseSet += (a, b) =>
            {
                LastEvent = $"Union recorded: {a} and {b}.";
                RaiseStateChanged();
            };
        }

        /// <summary>
        /// Canonical family-unit formation (two committed partners) becomes one
        /// union fact via the Core spouse-link + marriage event ONLY — the
        /// bridge's unit-forming path is deliberately not used, so the
        /// genealogy authority never creates a second family-unit ledger.
        /// </summary>
        public bool RecordUnion(string parentA, string parentB, int day)
        {
            if (string.IsNullOrWhiteSpace(parentA) || string.IsNullOrWhiteSpace(parentB)) return false;
            _lineage.TickDay(day);
            bool changed = _lineage.SetSpouse(parentA, parentB);
            if (changed) RaiseStateChanged();
            return changed;
        }

        /// <summary>
        /// Canonical child welcomed (birth or adoption) becomes parent→child
        /// lineage records, one per committed parent, exactly once (the Core
        /// refuses a duplicate lineage with `lineage_exists`). The family name
        /// is inherited from the parent's existing recorded name only — never
        /// generated here (a generated name is a label, not proof of kinship).
        /// </summary>
        public IReadOnlyList<string> RecordChild(string familyId, string childId, bool isAdopted, int day)
        {
            var recorded = new List<string>();
            if (string.IsNullOrWhiteSpace(childId)) return recorded;
            var family = FindFamily(familyId);
            if (family == null) return recorded;

            foreach (var parent in family.ParentIds)
            {
                if (string.IsNullOrWhiteSpace(parent)) continue;
                _lineage.TickDay(day);
                var result = isAdopted
                    ? _bridge.OnAdoption(parent, childId, day)
                    : _bridge.OnChildBorn(parent, childId, day);
                if (result.Status == ActionResult.StatusKind.Success)
                {
                    recorded.Add(parent);
                    _lineage.InheritFamilyName(childId, parent);
                }
            }
            if (recorded.Count > 0) RaiseStateChanged();
            return recorded;
        }

        /// <summary>Canonical survivor fate (death) becomes one family event.</summary>
        public void RecordDeath(string survivorId, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return;
            _lineage.TickDay(day);
            _lineage.RecordFamilyEvent("death", day, new[] { survivorId },
                $"{survivorId} died — lineage kinship record retained.", "major");
            LastEvent = $"Genealogy death recorded: {survivorId}.";
            RaiseStateChanged();
        }

        private Ashfall.Core.Survivors.FamilyUnit? FindFamily(string familyId)
        {
            // The romance owner's FamilyUnits are readable through the bridge
            // callback payload only; the host resolves parents at event time
            // via the family id supplied by the canonical seam.
            return FamilyResolver?.Invoke(familyId);
        }

        /// <summary>
        /// Bound by Main: resolves the canonical RomanceFamilySystem family
        /// unit by id so parent ids come from the family owner, never inferred.
        /// </summary>
        public Func<string, Ashfall.Core.Survivors.FamilyUnit?>? FamilyResolver { get; set; }

        /// <summary>Kinship facts about one survivor, for read-only surfaces.</summary>
        public (List<string> Parents, List<string> Children, string Spouse, string FamilyName) GetKinship(string survivorId)
        {
            var parents = _lineage.LineageRecords
                .Where(l => l.isActive && !string.IsNullOrWhiteSpace(l.parentId)
                            && string.Equals(l.childId, survivorId, StringComparison.OrdinalIgnoreCase))
                .Select(l => l.parentId)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            var children = _lineage.LineageRecords
                .Where(l => l.isActive && !string.IsNullOrWhiteSpace(l.childId)
                            && string.Equals(l.parentId, survivorId, StringComparison.OrdinalIgnoreCase))
                .Select(l => l.childId)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            return (parents, children, _lineage.GetSpouse(survivorId), _lineage.GetFamilyName(survivorId));
        }

        public GenealogySnapshot GetSnapshot()
        {
            var snapshot = new GenealogySnapshot
            {
                LineageCount = _lineage.LineageRecords.Count,
                UnionCount = _lineage.LineageRecords.Count(l => !string.IsNullOrWhiteSpace(l.spouseId)) / 2,
                EventCount = _lineage.FamilyEventLog.Count
            };
            foreach (var e in _lineage.FamilyEventLog.TakeLast(5))
            {
                snapshot.RecentEvents.Add((e.eventType, e.day, e.description));
            }
            return snapshot;
        }

        public GenealogySaveState CaptureState()
        {
            var state = new GenealogySaveState
            {
                Lineages = _lineage.LineageRecords.Select(l => new GenealogyLineageRecordDto
                {
                    ParentId = l.parentId ?? string.Empty,
                    ChildId = l.childId ?? string.Empty,
                    RelationshipType = l.relationshipType ?? string.Empty,
                    EstablishedDay = l.establishedDay,
                    IsActive = l.isActive,
                    SpouseId = l.spouseId ?? string.Empty,
                    FamilyName = l.familyName ?? string.Empty
                }).ToList(),
                FamilyEvents = _lineage.FamilyEventLog.Select(e => new GenealogyFamilyEventDto
                {
                    EventId = e.eventId ?? string.Empty,
                    EventType = e.eventType ?? string.Empty,
                    Day = e.day,
                    ParticipantIds = e.participantIds != null ? new List<string>(e.participantIds) : new List<string>(),
                    Description = e.description ?? string.Empty,
                    Significance = e.significance ?? "moderate"
                }).ToList()
            };
            return state;
        }

        public void RestoreState(GenealogySaveState state)
        {
            if (state == null) return;
            var lineage = new LineageState();
            foreach (var dto in state.Lineages)
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.ChildId)) continue;
                lineage.lineages.Add(new LineageRecord
                {
                    parentId = dto.ParentId ?? string.Empty,
                    childId = dto.ChildId,
                    relationshipType = dto.RelationshipType ?? "parent",
                    establishedDay = dto.EstablishedDay,
                    isActive = dto.IsActive,
                    spouseId = dto.SpouseId ?? string.Empty,
                    familyName = dto.FamilyName ?? string.Empty
                });
            }
            foreach (var dto in state.FamilyEvents)
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.EventId)) continue;
                lineage.familyEvents.Add(new FamilyEvent
                {
                    eventId = dto.EventId,
                    eventType = dto.EventType ?? string.Empty,
                    day = dto.Day,
                    participantIds = dto.ParticipantIds != null ? new List<string>(dto.ParticipantIds) : new List<string>(),
                    description = dto.Description ?? string.Empty,
                    significance = dto.Significance ?? "moderate"
                });
            }
            _lineage.RestoreState(lineage);
            LastEvent = "Genealogy state restored.";
            RaiseStateChanged();
        }
    }
}
