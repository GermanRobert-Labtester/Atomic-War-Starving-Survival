// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RadiationEconomySaveStore
// Core State : Ashfall.Core.Radiation.RadiationEconomySaveState
// Host Caller: Main.RadiationEconomy
// Purpose    : Radiation economy bridge host session & persistence. The Core
//              bridge owns contaminated-trade price multipliers, block rules,
//              and the evaluation ledger; the host composes the authored
//              catalog. It does not own base item prices.
// ============================================================================

using System;
using System.IO;
using Ashfall.Core.Radiation;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RadiationEconomySaveStore
    {
        public const string FileName = "radiation_economy_save.json";
        public const string SectionName = "radiation_economy";

        private static readonly SaveStore<RadiationEconomySaveState> s_store =
            SaveStoreHub.Checksummed<RadiationEconomySaveState>(FileName, nameof(RadiationEconomySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(RadiationEconomySaveState state) => s_store.CaptureBare(state);
        public static RadiationEconomySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RadiationEconomySaveState state) => s_store.TrySave(state);
        public static RadiationEconomySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="RadiationEconomyBridge"/>.</summary>
    public sealed class RadiationEconomyHostSession : HostSessionBase
    {
        private readonly RadiationEconomyBridge _bridge;

        public RadiationEconomyBridge Bridge => _bridge;
        public bool CatalogReady { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public RadiationEconomyHostSession(RadiationEconomySaveState? state = null)
        {
            _bridge = new RadiationEconomyBridge();
            if (state != null) _bridge.RestoreState(state);
        }

        public static RadiationEconomyHostSession Create(RadiationEconomySaveState? state = null) =>
            new RadiationEconomyHostSession(state);

        public bool LoadCatalog(string dataDirectory)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory)) return false;
            string path = Path.Combine(dataDirectory, "radiation_economy_social.json");
            if (!File.Exists(path)) return false;
            try
            {
                _bridge.LoadCatalog(File.ReadAllText(path));
                CatalogReady = true;
                return true;
            }
            catch (Exception)
            {
                CatalogReady = false;
                return false;
            }
        }

        public TradeEvaluation EvaluateTrade(
            string itemId, string category, int contaminationLevel, float basePrice, string? targetFactionId = null)
        {
            var evaluation = _bridge.EvaluateTrade(itemId, category, contaminationLevel, basePrice, targetFactionId);
            LastEvent = evaluation.isBlocked
                ? $"Blocked {itemId}: {evaluation.blockReason}"
                : $"Evaluated {itemId} at x{evaluation.priceMultiplier:0.00}.";
            RaiseStateChanged();
            return evaluation;
        }

        public RadiationEconomySaveState CaptureState() => _bridge.CaptureState();

        public void RestoreState(RadiationEconomySaveState state)
        {
            _bridge.RestoreState(state);
            LastEvent = "Restored radiation economy state.";
            RaiseStateChanged();
        }

        public bool TrySave() => RadiationEconomySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = RadiationEconomySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
