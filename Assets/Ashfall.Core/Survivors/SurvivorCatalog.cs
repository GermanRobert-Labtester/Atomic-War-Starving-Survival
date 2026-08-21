using System;
using System.Collections.Generic;

using Ashfall.Core.IO;
namespace Ashfall.Core.Survivors
{
    /// <summary>One survivor definition from survivors.json (the authority).</summary>
    [Serializable]
    public class SurvivorDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string profession = string.Empty;
        public string bio = string.Empty;
        public float baseHealth = 100f;
    }

    /// <summary>One roster entry: a definition instantiated into the bunker.</summary>
    [Serializable]
    public class SurvivorRosterEntry
    {
        public string survivorId = string.Empty;
        public string definitionId = string.Empty;
        public int joinedDay = 0;
        public bool isAlive = true;
        public string deathReason = string.Empty;
    }

    /// <summary>Serialized roster state (save/load safe).</summary>
    [Serializable]
    public class SurvivorRosterState
    {
        public string systemId = SurvivorRosterSystem.SystemId;
        public List<SurvivorRosterEntry> entries = new List<SurvivorRosterEntry>();
    }

    /// <summary>
    /// Engine-agnostic survivor roster: joins definitions from the catalog
    /// into the bunker, tracks life/death with reasons, raises events. The
    /// needs/radiation host systems consume roster members; this system owns
    /// the roster ledger only. Save/load per the house pattern.
    /// </summary>
    public class SurvivorRosterSystem
    {
        public const string SystemId = "survivor_roster_system";

        private readonly SurvivorRosterState _state;
        private readonly List<SurvivorDefinition> _catalog = new List<SurvivorDefinition>();
        private readonly Dictionary<string, SurvivorRosterEntry> _byId = new Dictionary<string, SurvivorRosterEntry>();

        public event Action<SurvivorRosterEntry> OnSurvivorJoined;
        public event Action<SurvivorRosterEntry, string> OnSurvivorDied; // entry, reason
        public event Action<SurvivorRosterState> OnStateChanged;

        public SurvivorRosterSystem(SurvivorRosterState state = null!)
        {
            _state = state ?? new SurvivorRosterState();
            if (_state.entries == null) _state.entries = new List<SurvivorRosterEntry>();
            RebuildIndex();
        }

        public SurvivorRosterState State => _state;
        public IReadOnlyList<SurvivorDefinition> Catalog => _catalog;
        public IReadOnlyList<SurvivorRosterEntry> Roster => _state.entries;

        // ── Catalog ────────────────────────────────────────────────────

        public void RegisterDefinition(SurvivorDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            if (_catalog.Exists(d => d.id == def.id)) return;
            _catalog.Add(def);
        }

        public void RegisterRange(IEnumerable<SurvivorDefinition> defs)
        {
            if (defs == null) return;
            foreach (var def in defs) RegisterDefinition(def);
        }

        public SurvivorDefinition? FindDefinition(string definitionId)
        {
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i].id == definitionId) return _catalog[i];
            return null;
        }

        // ── Roster ─────────────────────────────────────────────────────

        /// <summary>Join a definition into the bunker. One survivor per definition id.</summary>
        public bool Join(string definitionId, int day)
        {
            var def = FindDefinition(definitionId);
            if (def == null) return false;
            if (_byId.ContainsKey(definitionId)) return false;

            var entry = new SurvivorRosterEntry
            {
                survivorId = definitionId,
                definitionId = definitionId,
                joinedDay = day
            };
            _state.entries.Add(entry);
            _byId[definitionId] = entry;
            OnSurvivorJoined?.Invoke(entry);
            RaiseChanged();
            return true;
        }

        /// <summary>Mark a survivor dead. Reasons are the survivor's last line.</summary>
        public bool Die(string survivorId, string reason)
        {
            if (!_byId.TryGetValue(survivorId, out var entry)) return false;
            if (!entry.isAlive) return false;
            entry.isAlive = false;
            entry.deathReason = reason ?? string.Empty;
            OnSurvivorDied?.Invoke(entry, entry.deathReason);
            RaiseChanged();
            return true;
        }

        public SurvivorRosterEntry? Find(string survivorId)
        {
            return _byId.TryGetValue(survivorId, out var e) ? e : null;
        }

        public int LivingCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _state.entries.Count; i++)
                    if (_state.entries[i].isAlive) count++;
                return count;
            }
        }

        // ── Save / Load ────────────────────────────────────────────────

        public SurvivorRosterState CaptureState()
        {
            var copy = new SurvivorRosterState { systemId = _state.systemId };
            var ordered = new List<SurvivorRosterEntry>(_state.entries);
            ordered.Sort((a, b) => string.CompareOrdinal(a.survivorId, b.survivorId));
            for (int i = 0; i < ordered.Count; i++)
            {
                var e = ordered[i];
                copy.entries.Add(new SurvivorRosterEntry
                {
                    survivorId = e.survivorId,
                    definitionId = e.definitionId,
                    joinedDay = e.joinedDay,
                    isAlive = e.isAlive,
                    deathReason = e.deathReason
                });
            }
            return copy;
        }

        public void RestoreState(SurvivorRosterState saved)
        {
            _state.entries.Clear();
            _byId.Clear();
            if (saved != null)
            {
                _state.systemId = SystemId;
                if (saved.entries != null)
                {
                    for (int i = 0; i < saved.entries.Count; i++)
                    {
                        var e = saved.entries[i];
                        if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
                        var copy = new SurvivorRosterEntry
                        {
                            survivorId = e.survivorId,
                            definitionId = e.definitionId,
                            joinedDay = e.joinedDay,
                            isAlive = e.isAlive,
                            deathReason = e.deathReason
                        };
                        _state.entries.Add(copy);
                        _byId[copy.survivorId] = copy;
                    }
                }
            }
            RaiseChanged();
        }

        private void RebuildIndex()
        {
            _byId.Clear();
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e != null && !string.IsNullOrEmpty(e.survivorId))
                    _byId[e.survivorId] = e;
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }

    /// <summary>Engine-agnostic loader for survivors.json (the roster catalog).</summary>
    public static class SurvivorCatalogLoader
    {
        public const string FileName = "survivors.json";

        public static List<SurvivorDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<SurvivorDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var parsed = json.Deserialize<SurvivorDefinition[]>(raw);
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    var def = parsed[i];
                    if (def == null || string.IsNullOrEmpty(def.id)) continue;
                    if (string.IsNullOrEmpty(def.displayName)) def.displayName = def.id;
                    result.Add(def);
                }
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return result;
                                }
            return result;
        }
    }
}
