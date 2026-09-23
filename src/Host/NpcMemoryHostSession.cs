// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : NpcMemorySaveStore
// Core State : Ashfall.Core.Narrative.NpcMemorySaveState
// Host Caller: Main.NpcMemory (SetupNpcMemory / SaveNpcMemory)
// Purpose    : Plan 147 — Per-NPC memory and relationship depth: trust, grudge,
//              favors owed, forgiveness, dialogue tone, and trade multipliers.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class NpcMemorySaveStore
    {
        public const string FileName = "npc_memory_save.json";
        public const string SectionName = "npc_memory";

        private static readonly SaveStore<NpcMemorySaveState> s_store =
            SaveStoreHub.Checksummed<NpcMemorySaveState>(FileName, nameof(NpcMemorySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(NpcMemorySaveState state) => s_store.CaptureBare(state);
        public static NpcMemorySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(NpcMemorySaveState state) => s_store.TrySave(state);
        public static NpcMemorySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 147 (Per-NPC Memory & Relationship Depth).
    /// Tracks individual survivor and wasteland NPC memories of player actions,
    /// managing dynamic personal trust, grudge levels, favors owed, forgiveness reconciliation,
    /// dialogue tone, and trade pricing modifiers.
    /// </summary>
    public sealed class NpcMemoryHostSession : HostSessionBase
    {
        private readonly NpcMemorySystem _system;
        private string _lastEvent = string.Empty;

        public NpcMemorySystem System => _system;
        public string LastEvent => _lastEvent;

        public NpcMemoryCensus Census => _system.GetCensus();
        public IReadOnlyDictionary<string, NpcRelationship> Relationships => _system.Relationships;

        public NpcMemoryHostSession(string? dataDir = null, NpcMemorySystem? system = null)
        {
            _system = system ?? new NpcMemorySystem();

            _system.OnMemoryRecorded += (npcId, entry) =>
            {
                _lastEvent = $"Memory recorded for {npcId}: {entry.Action} (Day {entry.Day}, Intensity {entry.Intensity:F0})";
                RaiseStateChanged();
            };

            _system.OnGrudgeForgiven += (npcId, reason, reduction) =>
            {
                _lastEvent = $"Grudge forgiven for {npcId} ({reason}, -{reduction:F0} grudge)";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir);
            }
        }

        public static NpcMemoryHostSession Create(string dataDir, NpcMemorySystem? system = null)
        {
            return new NpcMemoryHostSession(dataDir, system);
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, "npc_memory_dialogue.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _system.LoadDialogueCatalog(json);
                _lastEvent = $"Loaded {_system.GetAllDialogueTemplates().Count} NPC memory dialogue templates.";
                RaiseStateChanged();
            }
        }

        public NpcMemoryEntry RecordAction(
            string npcId,
            NpcMemoryActionType action,
            int day,
            string targetId = "",
            float intensity = 50f,
            params string[] tags)
        {
            return _system.RecordAction(npcId, action, day, targetId, intensity, tags);
        }

        public void TickDailyDecay(int currentDay, float decayRatePerDay = 1.0f)
        {
            _system.TickDailyDecay(currentDay, decayRatePerDay);
            _lastEvent = $"Ticked daily NPC memory decay for Day {currentDay}.";
            RaiseStateChanged();
        }

        public bool Forgive(string npcId, string reason, float restitutionAmount = 0f)
        {
            return _system.Forgive(npcId, reason, restitutionAmount);
        }

        public float GetTradePriceMultiplier(string npcId) => _system.GetTradePriceMultiplier(npcId);
        public bool IsTradeRefused(string npcId) => _system.IsTradeRefused(npcId);
        public NpcDialogueTone GetDialogueTone(string npcId) => _system.GetDialogueTone(npcId);
        public IReadOnlyList<NpcMemoryDialogueDef> GetDialogueTemplatesForTone(NpcDialogueTone tone) =>
            _system.GetDialogueTemplatesForTone(tone);

        public NpcRelationship GetOrCreate(string npcId) => _system.GetOrCreate(npcId);
        public NpcRelationship? Get(string npcId) => _system.Get(npcId);

        public NpcMemorySaveState CaptureState() => _system.CaptureState();

        public void RestoreState(NpcMemorySaveState state)
        {
            _system.RestoreState(state);
            _lastEvent = "Restored NPC memory state.";
            RaiseStateChanged();
        }
    }
}
