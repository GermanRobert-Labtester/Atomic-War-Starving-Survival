using System;
using System.Collections.Generic;

namespace Ashfall.Core.Verdict
{
    /// <summary>One Verdict NPC — a figure with flag-gated availability and phase reactions.</summary>
    [Serializable]
    public class VerdictNpcEntry
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string role = string.Empty;
        public string kind = "paper_ghost";  // tape_echo | paper_ghost | living | readings
        public string gatingFlag = string.Empty;
        public string locationId = string.Empty;
        public int phaseMin = 1;
        public List<string> dialogue = new List<string>();
    }

    [Serializable]
    public class VerdictNpcState
    {
        public List<string> spokenNpcIds = new List<string>();
    }

    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — the six figures of the machine's
    /// human record. Each is a flag-gated encounter card: available only when
    /// its gate flag is set, reactive to the Reckoning phase, one-shot spoken.
    /// No human faction is spawned — the Tempest stays a utility.
    /// </summary>
    public sealed class VerdictNpcSystem
    {
        private readonly VerdictNpcState _state;
        private readonly List<VerdictNpcEntry> _catalog = new List<VerdictNpcEntry>();

        public VerdictNpcState State => _state;
        public IReadOnlyList<VerdictNpcEntry> Catalog => _catalog;

        public event Action<VerdictNpcEntry> OnSpoken;

        public VerdictNpcSystem(VerdictNpcState state = null)
        {
            _state = state ?? new VerdictNpcState();
        }

        public void Register(VerdictNpcEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.id)) return;
            if (!_catalog.Exists(e => e.id == entry.id)) _catalog.Add(entry);
        }

        public VerdictNpcEntry Find(string id)
        {
            foreach (var e in _catalog)
                if (e.id == id) return e;
            return null;
        }

        /// <summary>NPCs whose gate flag is set and whose phase requirement is met.</summary>
        public List<VerdictNpcEntry> GetAvailable(
            IReadOnlyCollection<string> setFlags, int phase, string locationId = null)
        {
            var result = new List<VerdictNpcEntry>();
            foreach (var e in _catalog)
            {
                if (e.phaseMin > 1 && phase < e.phaseMin) continue;
                if (!string.IsNullOrEmpty(e.gatingFlag) &&
                    (setFlags == null || !ContainsFlag(setFlags, e.gatingFlag))) continue;
                if (!string.IsNullOrEmpty(locationId) && e.locationId != locationId) continue;
                result.Add(e);
            }
            return result;
        }

        /// <summary>Spend the NPC's only interjection. Idempotent per NPC.</summary>
        public bool Speak(string npcId, string locationId = null)
        {
            var npc = Find(npcId);
            if (npc == null) return false;
            if (_state.spokenNpcIds.Contains(npcId)) return false; // one-shot
            if (!string.IsNullOrEmpty(locationId) && npc.locationId != locationId) return false;

            _state.spokenNpcIds.Add(npcId);
            OnSpoken?.Invoke(npc);
            return true;
        }

        private static bool ContainsFlag(IReadOnlyCollection<string> flags, string id)
        {
            foreach (var f in flags)
                if (string.Equals(f, id, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        public VerdictNpcState CaptureState()
        {
            var copy = new VerdictNpcState();
            copy.spokenNpcIds.AddRange(_state.spokenNpcIds);
            return copy;
        }

        public void RestoreState(VerdictNpcState state)
        {
            if (state == null) return;
            _state.spokenNpcIds.Clear();
            _state.spokenNpcIds.AddRange(state.spokenNpcIds);
        }
    }

    /// <summary>Loader for verdict_npcs.json.</summary>
    public static class VerdictNpcCatalogLoader
    {
        public const string FileName = "verdict_npcs.json";

        public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return 0;
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return 0;
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            try
            {
                var list = json.Deserialize<List<VerdictNpcEntry>>(raw);
                if (list == null) return 0;
                int count = 0;
                foreach (var e in list)
                {
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    system.Register(e);
                    count++;
                }
                return count;
            }
            catch { return 0; }
        }
    }
}
