using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class WildlifeSaveState
    {
        public int schema_version = 1;
        public string systemId = WildlifeMigrationSystem.SystemId;
        public int lastMigrationDay = -1;
        public List<WildlifePackRecord> packs = new List<WildlifePackRecord>();
    }

    [Serializable]
    public sealed class WildlifePackRecord
    {
        public string packId = string.Empty;
        public string speciesId = string.Empty;
        public string currentSectorId = string.Empty;
        public int population = 5;
        public float aggressionScore = 0.5f;
        public float starvationLevel;
        public bool isRabid;
        public int lastThreatFiredDay = -1;
    }

    public sealed class WildlifeMigrationSystem
    {
        public const string SystemId = "wildlife_migration";
        private WildlifeSaveState _state = new WildlifeSaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public WildlifeSaveState State => _state;
        public event Action<WildlifePackRecord> OnPackMigrated;

        public WildlifeMigrationSystem(ISeededRng rng = null, ILog log = null)
        {
            _rng = rng ?? new SeededRng(42);
            _log = log ?? NullLog.Instance;
        }

        public ActionResult RegisterPack(string packId, string speciesId, string sectorId, int population)
        {
            if (_state.packs.Exists(p => string.Equals(p.packId, packId, StringComparison.Ordinal)))
                return ActionResult.Blocked("pack_exists", "wildlife.pack_exists");

            var pack = new WildlifePackRecord
            {
                packId = packId,
                speciesId = speciesId,
                currentSectorId = sectorId,
                population = population
            };
            _state.packs.Add(pack);
            return ActionResult.Success("wildlife.pack_registered");
        }

        public ActionResult MigratePack(string packId, string targetSectorId)
        {
            var pack = _state.packs.Find(p => string.Equals(p.packId, packId, StringComparison.Ordinal));
            if (pack == null) return ActionResult.Failed("unknown_pack", "wildlife.unknown_pack");

            pack.currentSectorId = targetSectorId;
            OnPackMigrated?.Invoke(pack);
            return ActionResult.Success("wildlife.pack_migrated");
        }

        public void TickDay(int day)
        {
            _state.lastMigrationDay = day;
            foreach (var pack in _state.packs)
            {
                pack.starvationLevel = Math.Min(1f, pack.starvationLevel + 0.05f);
                if (pack.starvationLevel > 0.7f)
                {
                    pack.aggressionScore = Math.Min(1f, pack.aggressionScore + 0.1f);
                }
            }
        }

        public WildlifeSaveState CaptureState() => _state;

        public void RestoreState(WildlifeSaveState saved)
        {
            if (saved == null) return;
            _state = saved;
        }
    }
}
