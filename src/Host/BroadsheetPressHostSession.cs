// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : BroadsheetPressSaveStore
// Core State : Ashfall.Core.Print.BroadsheetPressState
// Host Caller: Main.BroadsheetPress
// Purpose    : Expansion 30 — press consumables (type tray, ink, paper) and the
//              bound archive of what the shelter printed. Rumor facts stay with
//              RumorSystem; morale stays with the survivors' needs authority.
// ============================================================================

using Ashfall.Core.Print;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class BroadsheetPressSaveStore
    {
        public const string FileName = "broadsheet_press_save.json";
        public const string SectionName = "broadsheet_press";

        private static readonly SaveStore<BroadsheetPressState> s_store =
            SaveStoreHub.Checksummed<BroadsheetPressState>(FileName, nameof(BroadsheetPressSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(BroadsheetPressState state) => s_store.CaptureBare(state);
        public static BroadsheetPressState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(BroadsheetPressState state) => s_store.TrySave(state);
        public static BroadsheetPressState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 30 host session. Wraps the stateful
    /// <see cref="BroadsheetPressLedger"/> over the signed pure
    /// <see cref="PublicBroadsheetPressEngine"/>. The press never decides what is
    /// true: a debunk run only reports the correction the engine computed, and the
    /// caller routes it through the canonical rumor owner.
    /// </summary>
    public sealed class BroadsheetPressHostSession : HostSessionBase
    {
        private readonly BroadsheetPressLedger _ledger;

        public BroadsheetPressLedger Ledger => _ledger;
        public BroadsheetPressCensus Census => _ledger.GetCensus();
        public int PublicationCount => _ledger.PublicationCount;
        public string LastEvent { get; private set; } = string.Empty;

        public BroadsheetPressHostSession(BroadsheetPressState? state = null)
        {
            _ledger = new BroadsheetPressLedger(state);
        }

        public static BroadsheetPressHostSession Create(BroadsheetPressState? state = null) =>
            new BroadsheetPressHostSession(state);

        /// <summary>Runs one publication and archives it. Returns the engine's immutable result.</summary>
        public PrintRunResult Publish(
            string? publicationId,
            PublicationKind kind,
            int targetCopies,
            int compositorSkillPermille,
            int shelterPopulation,
            string? headline = null,
            int day = 1)
        {
            var result = _ledger.ExecuteRun(
                publicationId, kind, targetCopies, compositorSkillPermille,
                shelterPopulation, headline, day);

            LastEvent = result.BlockedByShortage
                ? $"Print run blocked: {result.ShortageReason}."
                : $"Printed {result.CopiesPrinted} × {kind} (reach {result.AudienceReachPermille}‰, " +
                  $"morale {result.MoraleStabilizationPermille}‰).";
            RaiseStateChanged();
            return result;
        }

        /// <summary>Resets worn type with fresh foundry stock.</summary>
        public void RestoreTypeTray(int freshTypePiecesAdded)
        {
            _ledger.RestoreTypeTray(freshTypePiecesAdded);
            LastEvent = $"Type tray reset with {freshTypePiecesAdded} fresh piece(s); " +
                        $"wear now {_ledger.Tray.TypeWearPermille}‰.";
            RaiseStateChanged();
        }

        /// <summary>Restocks ink, paper, and type from shelter stores.</summary>
        public void RestockConsumables(int inkPermille, int paperPermille, int freshTypePieces)
        {
            _ledger.RestockConsumables(inkPermille, paperPermille, freshTypePieces);
            LastEvent = $"Press restocked (ink {_ledger.Tray.InkReservoirPermille}‰, " +
                        $"paper {_ledger.Tray.PaperStockPermille}‰).";
            RaiseStateChanged();
        }

        /// <summary>
        /// Pure debunk calculation. The result is a credibility correction the host
        /// applies to the canonical rumor record; rumor state is never owned here.
        /// </summary>
        public int CalculateDebunkCorrection(int rumorStrengthPermille, int pamphletAudienceReachPermille, int evidenceQualityPermille) =>
            _ledger.CalculateDebunkCorrection(rumorStrengthPermille, pamphletAudienceReachPermille, evidenceQualityPermille);

        public PressedPublication? FindPublication(string publicationId) => _ledger.FindPublication(publicationId);

        public BroadsheetPressState CaptureState() => _ledger.CaptureState();
        public void RestoreState(BroadsheetPressState? state) => _ledger.RestoreState(state);
        public void Clear() => _ledger.Clear();
    }
}
