// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : TradeTellHostSession
// Purpose      : PLAN-TRADE-TELL-TRUTH-248 — market tells/market signals.
//                Loads the authored `trade_tell_lines.json` corpus into the
//                Core TradeTellEngine and exposes deterministic tell selection
//                (stance x trust band). Pure derived read model: no save
//                section, no mutable campaign state.
// ============================================================================

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public sealed class TradeTellSnapshot
    {
        public int BandCount { get; set; }
        public int PoolCount { get; set; }
        public int LineCount { get; set; }
    }

    public sealed class TradeTellHostSession
    {
        private TradeTellEngine _engine = new();

        public TradeTellEngine Engine => _engine;
        public string LastEvent { get; private set; } = string.Empty;

        public static TradeTellHostSession Create(string dataDir)
        {
            var session = new TradeTellHostSession();
            session.LoadCorpus(dataDir);
            return session;
        }

        public void LoadCorpus(string dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) return;
            string path = Path.Combine(dataDir, "trade_tell_lines.json");
            if (!File.Exists(path)) return;
            try
            {
                _engine = TradeTellEngine.LoadFromJson(File.ReadAllText(path));
                LastEvent = $"Trade tell corpus loaded ({_engine.PoolCount} stance/band pools).";
            }
            catch (Exception ex)
            {
                _engine = new TradeTellEngine();
                LastEvent = "Trade tell corpus failed to load: " + ex.Message;
            }
        }

        /// <summary>Deterministic tell selection — campaign RNG supplied by caller.</summary>
        public TradeTell? SelectTell(TradeStance stance, float trust, ISeededRng rng)
        {
            if (_engine.TrySelectTell(stance, trust, rng, out var tell))
            {
                LastEvent = $"Tell read: {tell.Id} ({tell.Band}).";
                return tell;
            }
            return null;
        }

        public TradeTellSnapshot GetSnapshot() => new TradeTellSnapshot
        {
            BandCount = _engine.BandCount,
            PoolCount = _engine.PoolCount,
            LineCount = _engine.LineCount
        };
    }
}
