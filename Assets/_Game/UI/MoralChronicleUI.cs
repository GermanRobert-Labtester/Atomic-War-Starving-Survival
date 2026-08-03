using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Survival fate summary for a single survivor, displayed on the Moral Chronicle screen.
    /// Uses only primitive types so MoralChronicleUI stays in the UI assembly without a Core dependency.
    /// </summary>
    [Serializable]
    public struct SurvivorFateSummary
    {
        public string DisplayName;
        /// <summary>"Alive", "Dead — radiation", "Dead — starvation", etc.</summary>
        public string FateDescription;
        public bool Survived;
        public float TotalRadiationAbsorbed;
        public int TraumaCount;
    }

    /// <summary>
    /// Post-game campaign summary and moral chronicle UI (Prompt #42).
    ///
    /// Displays: final cause of victory/defeat, total days survived, individual
    /// survivor fates, and a chronological timeline of moral choices. Offers
    /// "Main Menu" and "View Final Journal Snapshot" navigation buttons.
    ///
    /// Populated entirely via Show() with plain data — no reference to Core types,
    /// so the UI assembly stays free of a Core dependency.
    /// </summary>
    public class MoralChronicleUI : MonoBehaviour
    {
        // ── Visibility state ──────────────────────────────────────────────────

        public bool IsVisible { get; private set; }

        // ── Campaign summary ─────────────────────────────────────────────────

        public bool IsVictory { get; private set; }
        public string OutcomeLabel { get; private set; } = string.Empty;
        public string OutcomeSummary { get; private set; } = string.Empty;
        public string GameModeLabel { get; private set; } = string.Empty;
        public int DaysSurvived { get; private set; }
        public int TargetDurationDays { get; private set; }
        public string CampaignStartDate { get; private set; } = string.Empty;

        // ── Survivor fates ───────────────────────────────────────────────────

        public IReadOnlyList<SurvivorFateSummary> SurvivorFates => _survivorFates;
        private readonly List<SurvivorFateSummary> _survivorFates = new List<SurvivorFateSummary>();

        // ── Moral timeline ───────────────────────────────────────────────────

        public IReadOnlyList<MoralChronicleEntry> Timeline => _timeline;
        private readonly List<MoralChronicleEntry> _timeline = new List<MoralChronicleEntry>();

        // ── Journal snapshot state ───────────────────────────────────────────

        public bool JournalSnapshotVisible { get; private set; }
        public string JournalSnapshotText { get; private set; } = string.Empty;

        // ── Navigation button state ──────────────────────────────────────────

        /// <summary>
        /// Set to true when the "Main Menu" button is activated.
        /// MoralChronicleBridge reads and resets this to trigger scene reload.
        /// </summary>
        public bool MainMenuRequested { get; private set; }

        /// <summary>
        /// Set to true when "View Final Journal Snapshot" button is activated.
        /// Reset by the bridge after handling.
        /// </summary>
        public bool JournalSnapshotRequested { get; private set; }

        // ── Computed display text ────────────────────────────────────────────

        public string StatusLine { get; private set; } = "CHRONICLE: —";
        public string DetailSummary { get; private set; } = string.Empty;

        // ── Events ───────────────────────────────────────────────────────────

        /// <summary>Fired the first time the screen becomes visible after Show().</summary>
        public event Action OnChronicleShown;
        /// <summary>Fired when the player activates the "Main Menu" button.</summary>
        public event Action OnMainMenuRequested;
        /// <summary>Fired when the player activates the "View Final Journal Snapshot" button.</summary>
        public event Action OnJournalSnapshotRequested;

        // ─────────────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Populate and show the chronicle. Call from MoralChronicleBridge when
        /// CampaignEndedEvent fires.
        /// </summary>
        public void Show(
            bool isVictory,
            string outcomeLabel,
            string outcomeSummary,
            string gameModeLabel,
            int daysSurvived,
            int targetDurationDays,
            string campaignStartDate,
            IEnumerable<SurvivorFateSummary> survivorFates,
            IEnumerable<MoralChronicleEntry> timeline)
        {
            IsVictory = isVictory;
            OutcomeLabel = outcomeLabel ?? (isVictory ? "VICTORY" : "DEFEAT");
            OutcomeSummary = outcomeSummary ?? string.Empty;
            GameModeLabel = gameModeLabel ?? string.Empty;
            DaysSurvived = Mathf.Max(1, daysSurvived);
            TargetDurationDays = Mathf.Max(1, targetDurationDays);
            CampaignStartDate = campaignStartDate ?? string.Empty;

            _survivorFates.Clear();
            if (survivorFates != null)
            {
                foreach (var f in survivorFates)
                    _survivorFates.Add(f);
            }

            _timeline.Clear();
            if (timeline != null)
            {
                foreach (var e in timeline)
                {
                    if (e != null) _timeline.Add(e);
                }
            }

            // Sort timeline chronologically (oldest first)
            _timeline.Sort((a, b) => a.Day.CompareTo(b.Day));

            MainMenuRequested = false;
            JournalSnapshotRequested = false;
            JournalSnapshotVisible = false;
            JournalSnapshotText = string.Empty;
            IsVisible = true;

            Refresh();
            OnChronicleShown?.Invoke();
        }

        public void Hide()
        {
            IsVisible = false;
            Refresh();
        }

        public void Clear()
        {
            IsVisible = false;
            IsVictory = false;
            OutcomeLabel = string.Empty;
            OutcomeSummary = string.Empty;
            GameModeLabel = string.Empty;
            DaysSurvived = 0;
            TargetDurationDays = 0;
            CampaignStartDate = string.Empty;
            _survivorFates.Clear();
            _timeline.Clear();
            MainMenuRequested = false;
            JournalSnapshotRequested = false;
            JournalSnapshotVisible = false;
            JournalSnapshotText = string.Empty;
            StatusLine = "CHRONICLE: —";
            DetailSummary = string.Empty;
        }

        // ── Button activators — called by Unity UI event system (Button.onClick) ──

        /// <summary>
        /// Called by the "Main Menu" button's onClick event.
        /// Sets MainMenuRequested = true and fires OnMainMenuRequested for the bridge.
        /// </summary>
        public void ActivateMainMenu()
        {
            MainMenuRequested = true;
            OnMainMenuRequested?.Invoke();
        }

        /// <summary>
        /// Called by the "View Final Journal Snapshot" button's onClick event.
        /// </summary>
        public void ActivateJournalSnapshot()
        {
            JournalSnapshotRequested = true;
            OnJournalSnapshotRequested?.Invoke();
        }

        /// <summary>
        /// Called by the bridge after it has handled MainMenuRequested (to reset the flag).
        /// </summary>
        public void ConsumeMainMenuRequest() => MainMenuRequested = false;

        /// <summary>
        /// Called by the bridge after it has handled JournalSnapshotRequested.
        /// Provides the rendered journal text to display.
        /// </summary>
        public void ConsumeJournalSnapshotRequest(string journalText)
        {
            JournalSnapshotRequested = false;
            JournalSnapshotText = journalText ?? string.Empty;
            JournalSnapshotVisible = true;
            Refresh();
        }

        public void CloseJournalSnapshot()
        {
            JournalSnapshotVisible = false;
            Refresh();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Refresh — builds the text representation consumed by HUD / Unity UI
        // ─────────────────────────────────────────────────────────────────────

        public void Refresh()
        {
            if (!IsVisible)
            {
                StatusLine = "CHRONICLE: —";
                DetailSummary = string.Empty;
                return;
            }

            string victoryTag = IsVictory ? "VICTORY" : "DEFEAT";
            StatusLine =
                $"CHRONICLE [{victoryTag}]  {GameModeLabel}  Day {DaysSurvived}/{TargetDurationDays}";

            var sb = new StringBuilder();

            // ── Header ──────────────────────────────────────────────────────
            sb.AppendLine("╔══════════════════════════════════╗");
            sb.AppendLine($"  ASHFALL — CAMPAIGN CHRONICLE");
            sb.AppendLine($"  {OutcomeLabel}");
            sb.AppendLine("╚══════════════════════════════════╝");
            sb.AppendLine();

            // ── Campaign metadata ────────────────────────────────────────────
            sb.AppendLine($"Mode       : {GameModeLabel}");
            sb.AppendLine($"Started    : {CampaignStartDate}");
            sb.AppendLine($"Days       : {DaysSurvived} / {TargetDurationDays} target");
            sb.AppendLine();

            // ── Outcome ──────────────────────────────────────────────────────
            sb.AppendLine("── OUTCOME ────────────────────────");
            sb.AppendLine(OutcomeSummary);
            sb.AppendLine();

            // ── Survivor fates ───────────────────────────────────────────────
            if (_survivorFates.Count > 0)
            {
                sb.AppendLine("── SURVIVORS ──────────────────────");
                for (int i = 0; i < _survivorFates.Count; i++)
                {
                    var f = _survivorFates[i];
                    string mark = f.Survived ? "○" : "✕";
                    string trauma = f.TraumaCount > 0 ? $"  trauma×{f.TraumaCount}" : string.Empty;
                    string rad = f.TotalRadiationAbsorbed > 0f
                        ? $"  RAD {f.TotalRadiationAbsorbed:0}"
                        : string.Empty;
                    sb.AppendLine($"  {mark} {f.DisplayName}  —  {f.FateDescription}{rad}{trauma}");
                }
                sb.AppendLine();
            }

            // ── Moral timeline ───────────────────────────────────────────────
            if (_timeline.Count > 0)
            {
                sb.AppendLine("── MORAL CHRONICLE ────────────────");
                for (int i = 0; i < _timeline.Count; i++)
                {
                    var e = _timeline[i];
                    string who = string.IsNullOrEmpty(e.SurvivorName) ? string.Empty : $" [{e.SurvivorName}]";
                    sb.AppendLine($"  Day {e.Day,3}{who}  {e.Description}");
                }
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("── MORAL CHRONICLE ────────────────");
                sb.AppendLine("  No significant moral events recorded.");
                sb.AppendLine();
            }

            // ── Navigation buttons (text stubs for headless / test context) ──
            sb.AppendLine("[ Main Menu ]      [ View Final Journal Snapshot ]");

            // ── Journal snapshot overlay ─────────────────────────────────────
            if (JournalSnapshotVisible && !string.IsNullOrEmpty(JournalSnapshotText))
            {
                sb.AppendLine();
                sb.AppendLine("── FINAL JOURNAL ──────────────────");
                sb.AppendLine(JournalSnapshotText);
                sb.AppendLine("[ Close Journal ]");
            }

            DetailSummary = sb.ToString().TrimEnd();
        }
    }
}
