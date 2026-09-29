// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : Plan49DepthPassHostSession
// Purpose      : Plan 49 (C1[16]) depth passes — pour the remaining dead
//                content onto the live rails. Binds the four orphaned
//                authored catalogs through their existing Core loaders into
//                constructed owners so each family is loaded AND consumed:
//                  phantom_heirlooms.json      -> HeirloomCatalog + HeirloomSystem
//                  trade_screen_scenarios.json -> TradeScreenScenarioLoader
//                  audio_logs_expansion_05.json-> AudioConditionSystem
//                  memorials_expansion_05.json -> MemorialSystem
//                Authored catalogs are static data — no new save section, no
//                mutable gameplay authority (Plan 41 owns heirloom runtime
//                state; this session only consumes the authored catalog
//                through live owner queries).
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Memorial;
using Ashfall.Core.Phantoms;

namespace AtomicWar.GodotApp
{
    /// <summary>Read-only depth-pass projection for probes and surfaces.</summary>
    public sealed class DepthPassSnapshot
    {
        public int HeirloomCount { get; set; }
        public int ScenarioCount { get; set; }
        public int AudioLogCount { get; set; }
        public int MemorialTextCount { get; set; }
        public List<string> BoundCatalogs { get; } = new();
    }

    public sealed class Plan49DepthPassHostSession
    {
        public HeirloomCatalog Heirlooms { get; } = new();
        public HeirloomSystem HeirloomOwner { get; } = null!;
        public IReadOnlyList<TradeScreenScenario> TradeScenarios { get; private set; } =
            Array.Empty<TradeScreenScenario>();
        public AudioConditionSystem AudioConditions { get; } = new();
        public MemorialSystem Memorials { get; } = null!;
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public Plan49DepthPassHostSession()
        {
            HeirloomOwner = new HeirloomSystem(Heirlooms);
            Memorials = new MemorialSystem(new Ashfall.Core.Memorial.MemorialState());
        }

        public static Plan49DepthPassHostSession Create(string dataDir)
        {
            var session = new Plan49DepthPassHostSession();
            session.LoadCatalogs(dataDir);
            return session;
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir) || !Directory.Exists(dataDir)) return;

            // 1) phantom_heirlooms.json -> HeirloomCatalog.Load + HeirloomSystem.
            string heirloomPath = Path.Combine(dataDir, "phantom_heirlooms.json");
            if (File.Exists(heirloomPath))
            {
                Heirlooms.Load(File.ReadAllText(heirloomPath), new SystemTextJsonSerializer());
                LastEvent = $"Heirloom catalog bound ({Heirlooms.AllHeirlooms.Count} authored heirlooms).";
            }

            // 2) trade_screen_scenarios.json -> TradeScreenScenarioLoader.
            string scenarioPath = Path.Combine(dataDir, "trade_screen_scenarios.json");
            if (File.Exists(scenarioPath))
            {
                TradeScenarios = TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(scenarioPath));
                LastEvent = $"Trade-screen scenarios bound ({TradeScenarios.Count} authored scenarios).";
            }

            // 3) audio_logs_expansion_05.json -> AudioConditionSystem.
            string audioLogPath = Path.Combine(dataDir, "audio_logs_expansion_05.json");
            if (File.Exists(audioLogPath))
            {
                AudioConditions.LoadAudioLogCatalog(File.ReadAllText(audioLogPath));
                LastEvent = $"Audio-log catalog bound ({AudioConditions.AudioLogCount} authored logs).";
            }

            // 4) memorials_expansion_05.json -> MemorialSystem.
            string memorialPath = Path.Combine(dataDir, "memorials_expansion_05.json");
            if (File.Exists(memorialPath))
            {
                Memorials.LoadMemorialTexts(File.ReadAllText(memorialPath));
                LastEvent = $"Memorial text catalog bound ({Memorials.MemorialTextCount} authored texts).";
            }

            StateChanged?.Invoke();
        }

        /// <summary>Live authored-catalog consumption: heirloom provenance by base item.</summary>
        public HeirloomDefinition? GetHeirloomForItem(string baseItemId) => Heirlooms.GetByBaseItemId(baseItemId);

        /// <summary>Live authored-catalog consumption: audio-log text for an audio key.</summary>
        public string? GetAudioLogText(string logId) => AudioConditions.GetAudioLogBody(logId);
        public string? GetAudioLogListeningNote(string logId) => AudioConditions.GetAudioLogListeningNote(logId);
        public IReadOnlyList<AuthoredAudioLog> GetAudioLogsForDay(int day) => AudioConditions.GetAudioLogsForDay(day);

        /// <summary>Live authored-catalog consumption: memorial text by name.</summary>
        public string? GetMemorialText(string name) => Memorials.GetMemorialText(name);

        public DepthPassSnapshot GetSnapshot()
        {
            var snapshot = new DepthPassSnapshot
            {
                HeirloomCount = Heirlooms.AllHeirlooms.Count,
                ScenarioCount = TradeScenarios.Count,
                AudioLogCount = AudioConditions.AudioLogCount,
                MemorialTextCount = Memorials.MemorialTextCount
            };
            if (snapshot.HeirloomCount > 0) snapshot.BoundCatalogs.Add("phantom_heirlooms.json");
            if (snapshot.ScenarioCount > 0) snapshot.BoundCatalogs.Add("trade_screen_scenarios.json");
            if (snapshot.AudioLogCount > 0) snapshot.BoundCatalogs.Add("audio_logs_expansion_05.json");
            if (snapshot.MemorialTextCount > 0) snapshot.BoundCatalogs.Add("memorials_expansion_05.json");
            return snapshot;
        }
    }
}
