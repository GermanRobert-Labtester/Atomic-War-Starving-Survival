// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS8618
using Ashfall.Core.Legacy;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class LineageRecord
    {
        public string parentId = string.Empty;
        public string childId = string.Empty;
        public string relationshipType = string.Empty; // "parent", "adopted", "mentor"
        public int establishedDay;
        public bool isActive = true;
        public List<string> inheritedTraitIds = new List<string>();
        public string spouseId = string.Empty;
        public string familyName = string.Empty;
    }

    [Serializable]
    public sealed class FamilyUnit
    {
        public string unitId = string.Empty;
        public string familyName = string.Empty;
        public int foundingDay;
        public List<string> memberIds = new List<string>();
        public string patriarchId = string.Empty;
        public string matriarchId = string.Empty;
        public string status = "active"; // active, dissolved, extinct
        public int currentGeneration = 1;
    }

    [Serializable]
    public sealed class FamilyEvent
    {
        public string eventId = string.Empty;
        public string eventType = string.Empty; // birth, death, marriage, divorce, adoption, reunion, schism, milestone, succession
        public int day;
        public List<string> participantIds = new List<string>();
        public string description = string.Empty;
        public string significance = "moderate"; // minor, moderate, major
    }

    [Serializable]
    public sealed class LineageState
    {
        public string systemId = GenerationalLineageExtension.SystemId;
        public List<LineageRecord> lineages = new List<LineageRecord>();
        public List<FamilyUnit> familyUnits = new List<FamilyUnit>();
        public List<FamilyEvent> familyEvents = new List<FamilyEvent>();
    }

    /// <summary>
    /// Plan 217 — Survivor Genealogy & Family Tree System.
    /// Extends GenerationalSuccessionEngine with parent/child lineage, siblings, spouses,
    /// family units, kinship affinity calculations, and genealogical family events.
    /// </summary>
    public sealed class GenerationalLineageExtension
    {
        public const string SystemId = "generational_lineage";
        private LineageState _state = new LineageState();
        private readonly GenerationalSuccessionEngine _engine;
        private readonly ILog _log;
        private int _currentDay;
        private int _nextEventSeq = 1;

        public LineageState State => _state;
        public event Action<string, string>? OnLineageEstablished;
        public event Action<string, string>? OnSuccessionPerformed;
        public event Action? OnLineageChanged;
        public event Action<FamilyUnit>? OnFamilyUnitCreated;
        public event Action<FamilyEvent>? OnFamilyEvent;
        public event Action<string, string>? OnSpouseSet;

        public GenerationalLineageExtension(GenerationalSuccessionEngine engine, ILog? log = null)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult EstablishLineage(string parentId, string childId, string relationshipType)
        {
            if (_state.lineages.Exists(l => l.parentId == parentId && l.childId == childId))
                return ActionResult.Blocked("lineage_exists", "lineage.already_exists");

            _engine.RegisterDweller(childId, 0, generation: _engine.GetRecord(parentId)?.generationIndex + 1 ?? 1);
            _engine.FormMentorship(parentId, childId, string.Empty);

            var parentRecord = _state.lineages.Find(l => l.childId == parentId);
            string inheritedFamilyName = parentRecord?.familyName ?? string.Empty;

            var record = new LineageRecord
            {
                parentId = parentId,
                childId = childId,
                relationshipType = relationshipType ?? "parent",
                establishedDay = _currentDay,
                familyName = inheritedFamilyName
            };
            _state.lineages.Add(record);

            RecordFamilyEvent("birth", _currentDay, new[] { parentId, childId }, $"{childId} born to {parentId}.", "major");

            _log.Info($"[Lineage] {relationshipType}: {parentId} -> {childId}");
            OnLineageEstablished?.Invoke(parentId, childId);
            OnLineageChanged?.Invoke();
            return ActionResult.Success("lineage.established",
                new Dictionary<string, double> { { "generation", _engine.GetRecord(childId)?.generationIndex ?? 1 } });
        }

        public ActionResult PerformSuccession(string retireeId, string successorId)
        {
            var record = _engine.GetRecord(retireeId);
            if (record == null) return ActionResult.Failed("unknown_dweller", "lineage.unknown_dweller");
            var succRecord = _engine.GetRecord(successorId);
            if (succRecord == null) return ActionResult.Failed("unknown_successor", "lineage.unknown_successor");

            record.isRetired = true;
            succRecord.generationIndex = record.generationIndex + 1;

            RecordFamilyEvent("succession", _currentDay, new[] { retireeId, successorId }, $"Succession passed from {retireeId} to {successorId}.", "major");

            _log.Info($"[Lineage] succession: {retireeId} -> {successorId} (gen {succRecord.generationIndex})");
            OnSuccessionPerformed?.Invoke(retireeId, successorId);
            OnLineageChanged?.Invoke();
            return ActionResult.Success("lineage.succession",
                new Dictionary<string, double> { { "generation", succRecord.generationIndex } });
        }

        public bool SetSpouse(string dwellerA, string dwellerB)
        {
            if (string.IsNullOrWhiteSpace(dwellerA) || string.IsNullOrWhiteSpace(dwellerB)) return false;
            if (string.Equals(dwellerA, dwellerB, StringComparison.OrdinalIgnoreCase)) return false;

            var recA = _state.lineages.FirstOrDefault(l => l.childId == dwellerA) ?? new LineageRecord { childId = dwellerA };
            var recB = _state.lineages.FirstOrDefault(l => l.childId == dwellerB) ?? new LineageRecord { childId = dwellerB };

            if (!_state.lineages.Contains(recA)) _state.lineages.Add(recA);
            if (!_state.lineages.Contains(recB)) _state.lineages.Add(recB);

            recA.spouseId = dwellerB;
            recB.spouseId = dwellerA;

            RecordFamilyEvent("marriage", _currentDay, new[] { dwellerA, dwellerB }, $"Union formed between {dwellerA} and {dwellerB}.", "major");
            OnSpouseSet?.Invoke(dwellerA, dwellerB);
            OnLineageChanged?.Invoke();
            return true;
        }

        public string GetSpouse(string dwellerId)
        {
            var rec = _state.lineages.FirstOrDefault(l => l.childId == dwellerId);
            return rec?.spouseId ?? string.Empty;
        }

        public List<string> GetSiblings(string dwellerId)
        {
            var parents = _state.lineages
                .Where(l => l.childId == dwellerId && l.isActive && !string.IsNullOrWhiteSpace(l.parentId))
                .Select(l => l.parentId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (parents.Count == 0) return new List<string>();

            return _state.lineages
                .Where(l => l.isActive && parents.Contains(l.parentId) && !string.Equals(l.childId, dwellerId, StringComparison.OrdinalIgnoreCase))
                .Select(l => l.childId)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public List<string> GetAncestors(string dwellerId)
        {
            var ancestors = new List<string>();
            var queue = new Queue<string>();
            queue.Enqueue(dwellerId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var parents = _state.lineages
                    .Where(l => l.childId == current && l.isActive && !string.IsNullOrWhiteSpace(l.parentId))
                    .Select(l => l.parentId);

                foreach (var p in parents)
                {
                    if (!ancestors.Contains(p, StringComparer.OrdinalIgnoreCase))
                    {
                        ancestors.Add(p);
                        queue.Enqueue(p);
                    }
                }
            }

            return ancestors;
        }

        public List<string> GetDescendants(string dwellerId)
        {
            var descendants = new List<string>();
            var queue = new Queue<string>();
            queue.Enqueue(dwellerId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var children = _state.lineages
                    .Where(l => l.parentId == current && l.isActive && !string.IsNullOrWhiteSpace(l.childId))
                    .Select(l => l.childId);

                foreach (var c in children)
                {
                    if (!descendants.Contains(c, StringComparer.OrdinalIgnoreCase))
                    {
                        descendants.Add(c);
                        queue.Enqueue(c);
                    }
                }
            }

            return descendants;
        }

        public int GetLineageDepth(string dwellerId)
        {
            int depth = 0;
            string? current = dwellerId;

            while (!string.IsNullOrEmpty(current))
            {
                var parentRec = GetParent(current);
                if (parentRec != null && !string.IsNullOrEmpty(parentRec.parentId))
                {
                    depth++;
                    current = parentRec.parentId;
                }
                else
                {
                    break;
                }
            }

            return depth;
        }

        public FamilyUnit FormFamilyUnit(string familyName, int foundingDay, IEnumerable<string> memberIds)
        {
            var unit = new FamilyUnit
            {
                unitId = $"fam_{_state.familyUnits.Count + 1}",
                familyName = familyName ?? "Wanderer",
                foundingDay = foundingDay,
                memberIds = memberIds?.ToList() ?? new List<string>(),
                status = "active",
                currentGeneration = 1
            };

            _state.familyUnits.Add(unit);
            OnFamilyUnitCreated?.Invoke(unit);
            return unit;
        }

        public FamilyUnit? GetFamilyUnit(string dwellerId)
        {
            return _state.familyUnits.FirstOrDefault(u => u.memberIds.Contains(dwellerId, StringComparer.OrdinalIgnoreCase));
        }

        public FamilyEvent RecordFamilyEvent(string eventType, int day, IEnumerable<string> participants, string description, string significance = "moderate")
        {
            var ev = new FamilyEvent
            {
                eventId = $"fev_{_nextEventSeq++}",
                eventType = eventType,
                day = day,
                participantIds = participants?.ToList() ?? new List<string>(),
                description = description ?? string.Empty,
                significance = significance
            };

            _state.familyEvents.Add(ev);
            OnFamilyEvent?.Invoke(ev);
            return ev;
        }

        public IReadOnlyList<FamilyEvent> GetFamilyEvents(string dwellerId)
        {
            return _state.familyEvents
                .Where(e => e.participantIds.Contains(dwellerId, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        public float GetKinshipAffinityBonus(string dwellerA, string dwellerB)
        {
            if (string.Equals(dwellerA, dwellerB, StringComparison.OrdinalIgnoreCase)) return 0f;

            // Spouse: +25
            if (string.Equals(GetSpouse(dwellerA), dwellerB, StringComparison.OrdinalIgnoreCase))
                return 25f;

            // Parent/Child: +20
            var parentA = GetParent(dwellerA);
            if (parentA != null && string.Equals(parentA.parentId, dwellerB, StringComparison.OrdinalIgnoreCase))
                return 20f;
            var parentB = GetParent(dwellerB);
            if (parentB != null && string.Equals(parentB.parentId, dwellerA, StringComparison.OrdinalIgnoreCase))
                return 20f;

            // Sibling: +15
            if (GetSiblings(dwellerA).Contains(dwellerB, StringComparer.OrdinalIgnoreCase))
                return 15f;

            // Ancestor / Descendant: +10
            if (GetAncestors(dwellerA).Contains(dwellerB, StringComparer.OrdinalIgnoreCase) ||
                GetDescendants(dwellerA).Contains(dwellerB, StringComparer.OrdinalIgnoreCase))
                return 10f;

            return 0f;
        }

        public List<LineageRecord> GetLineage(string dwellerId)
        {
            return _state.lineages.FindAll(l => l.parentId == dwellerId || l.childId == dwellerId);
        }

        public LineageRecord? GetParent(string dwellerId)
        {
            return _state.lineages.Find(l => l.childId == dwellerId && l.isActive);
        }

        public void TickDay(int day)
        {
            _currentDay = day;
        }

        public LineageState CaptureState() => CloneState(_state);

        public void RestoreState(LineageState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static LineageState CloneState(LineageState src)
        {
            if (src == null) return new LineageState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<LineageState>(json) ?? new LineageState();
        }
    }
}
