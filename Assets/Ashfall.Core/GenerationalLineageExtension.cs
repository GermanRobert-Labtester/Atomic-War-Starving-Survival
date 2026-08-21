using System;
using System.Collections.Generic;
using Ashfall.Core.Legacy;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class LineageState
    {
        public string systemId = GenerationalLineageExtension.SystemId;
        public List<LineageRecord> lineages = new List<LineageRecord>();
    }

    [Serializable]
    public sealed class LineageRecord
    {
        public string parentId = string.Empty;
        public string childId = string.Empty;
        public string relationshipType = string.Empty; // "parent", "adopted", "mentor"
        public int establishedDay;
        public bool isActive = true;
        public List<string> inheritedTraitIds = new List<string>();
    }

    /// <summary>
    /// Extends GenerationalSuccessionEngine with explicit parent/child lineage
    /// tracking, inherited trait snapshots, and succession ceremonies.
    /// </summary>
    public sealed class GenerationalLineageExtension
    {
        public const string SystemId = "generational_lineage";
        private LineageState _state = new LineageState();
        private readonly GenerationalSuccessionEngine _engine;
        private readonly ILog _log;
        private int _currentDay;

        public LineageState State => _state;
        public event Action<string, string> OnLineageEstablished;
        public event Action<string, string> OnSuccessionPerformed;
        public event Action OnLineageChanged;

        public GenerationalLineageExtension(GenerationalSuccessionEngine engine, ILog log = null!)
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

            _state.lineages.Add(new LineageRecord
            {
                parentId = parentId, childId = childId,
                relationshipType = relationshipType ?? "parent",
                establishedDay = _currentDay
            });

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

            _log.Info($"[Lineage] succession: {retireeId} -> {successorId} (gen {succRecord.generationIndex})");
            OnSuccessionPerformed?.Invoke(retireeId, successorId);
            OnLineageChanged?.Invoke();
            return ActionResult.Success("lineage.succession",
                new Dictionary<string, double> { { "generation", succRecord.generationIndex } });
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

        public LineageState CaptureState() => _state;
        public void RestoreState(LineageState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnLineageChanged?.Invoke();
        }
    }
}
