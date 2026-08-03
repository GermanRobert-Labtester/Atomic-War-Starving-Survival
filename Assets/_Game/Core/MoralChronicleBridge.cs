using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Core-side bridge MonoBehaviour (Prompt #42).
    ///
    /// Subscribes to EventBus&lt;CampaignEndedEvent&gt; and EventBus&lt;MoralDilemmaEvent&gt;
    /// to populate and show the MoralChronicleUI when the campaign ends.
    ///
    /// Converts Core-typed data (CampaignResult, Survivor list) into plain primitives
    /// and Events-assembly DTOs so the UI assembly stays free of a Core dependency.
    /// </summary>
    public class MoralChronicleBridge : MonoBehaviour
    {
        // ── Wiring — set by GameBootstrap or inspector ────────────────────────

        public MoralChronicleUI ChronicleUI { get; private set; }

        // Survivor list snapshot taken at campaign end (set by InitWithSurvivors).
        private IReadOnlyList<Survivor> _survivors;

        // Running moral timeline (populated as events occur during play).
        private readonly List<MoralChronicleEntry> _timeline = new List<MoralChronicleEntry>();

        // Cached journal snapshot text (latest journal entries at campaign end).
        private string _journalSnapshot = string.Empty;

        // ── Events ───────────────────────────────────────────────────────────

        /// <summary>Fired when the player requests to return to main menu.</summary>
        public event Action OnMainMenuRequested;

        // ─────────────────────────────────────────────────────────────────────
        // Initialise
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Wire up the bridge. Call from GameBootstrap after both systems are created.
        /// </summary>
        public void Initialise(MoralChronicleUI chronicleUI, IReadOnlyList<Survivor> survivors)
        {
            ChronicleUI = chronicleUI;
            _survivors = survivors;

            if (ChronicleUI != null)
            {
                ChronicleUI.OnMainMenuRequested += HandleMainMenuRequest;
                ChronicleUI.OnJournalSnapshotRequested += HandleJournalSnapshotRequest;
            }

            // Subscribe to endgame
            EventBus.Subscribe<CampaignEndedEvent>(OnCampaignEnded);
            // Subscribe to moral dilemma resolved events to record timeline entries
            EventBus.Subscribe<MoralDilemmaEvent>(OnMoralDilemmaEvent);
        }

        private void OnDestroy()
        {
            if (ChronicleUI != null)
            {
                ChronicleUI.OnMainMenuRequested -= HandleMainMenuRequest;
                ChronicleUI.OnJournalSnapshotRequested -= HandleJournalSnapshotRequest;
            }
            EventBus.Unsubscribe<CampaignEndedEvent>(OnCampaignEnded);
            EventBus.Unsubscribe<MoralDilemmaEvent>(OnMoralDilemmaEvent);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Timeline recording
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Manually push a moral entry (call from GameBootstrap for any notable event).
        /// </summary>
        public void RecordMoralEntry(int day, string description,
            MoralChronicleEntryKind kind = MoralChronicleEntryKind.Unknown,
            string survivorName = null)
        {
            if (string.IsNullOrEmpty(description)) return;
            _timeline.Add(new MoralChronicleEntry
            {
                Day = day,
                Description = description,
                Kind = kind,
                SurvivorName = survivorName ?? string.Empty
            });
        }

        /// <summary>Set the cached journal snapshot text (call just before campaign ends).</summary>
        public void SetJournalSnapshot(string text)
        {
            _journalSnapshot = text ?? string.Empty;
        }

        // ─────────────────────────────────────────────────────────────────────
        // EventBus handlers
        // ─────────────────────────────────────────────────────────────────────

        private void OnCampaignEnded(CampaignEndedEvent evt)
        {
            if (ChronicleUI == null) return;

            var result = evt.Result;

            // Build survivor fate summaries from the live survivor list
            var fates = BuildSurvivorFates(_survivors);

            // Build human-readable labels from Core enums (bridge translates)
            string outcomeLabel = evt.IsVictory ? "VICTORY" : "DEFEAT";
            string gameModeLabel = BuildGameModeLabel(result);
            string campaignStart = result.StartCalendarDate.ToString("MMMM d, yyyy");

            ChronicleUI.Show(
                isVictory: evt.IsVictory,
                outcomeLabel: outcomeLabel,
                outcomeSummary: result.OutcomeSummary,
                gameModeLabel: gameModeLabel,
                daysSurvived: evt.DaysSurvived,
                targetDurationDays: result.TargetDurationDays,
                campaignStartDate: campaignStart,
                survivorFates: fates,
                timeline: _timeline
            );
        }

        private void OnMoralDilemmaEvent(MoralDilemmaEvent dilemmaEvt)
        {
            if (dilemmaEvt == null || !dilemmaEvt.IsResolved) return;

            string choiceText = BuildDilemmaChoiceText(dilemmaEvt);
            _timeline.Add(new MoralChronicleEntry
            {
                Day = dilemmaEvt.Day,
                Description = choiceText,
                Kind = MoralChronicleEntryKind.DesperateChoice
            });
        }

        // ─────────────────────────────────────────────────────────────────────
        // Button handlers
        // ─────────────────────────────────────────────────────────────────────

        private void HandleMainMenuRequest()
        {
            ChronicleUI?.ConsumeMainMenuRequest();
            OnMainMenuRequested?.Invoke();
        }

        private void HandleJournalSnapshotRequest()
        {
            ChronicleUI?.ConsumeJournalSnapshotRequest(_journalSnapshot);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────────────

        private static List<SurvivorFateSummary> BuildSurvivorFates(IReadOnlyList<Survivor> survivors)
        {
            var result = new List<SurvivorFateSummary>();
            if (survivors == null) return result;

            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null) continue;

                string fate = s.IsAlive
                    ? "Alive"
                    : BuildDeathCause(s);

                result.Add(new SurvivorFateSummary
                {
                    DisplayName = string.IsNullOrEmpty(s.DisplayName) ? s.Id : s.DisplayName,
                    FateDescription = fate,
                    Survived = s.IsAlive,
                    TotalRadiationAbsorbed = s.LifetimeRadiationExposure,
                    TraumaCount = s.Traumas?.Count ?? 0
                });
            }
            return result;
        }

        private static string BuildDeathCause(Survivor s)
        {
            if (s.HasAcuteRadiationSickness || s.LifetimeRadiationExposure >= 400f)
                return "Dead — acute radiation sickness";
            if (s.HasChronicIllness)
                return "Dead — chronic radiation illness";
            if (s.Needs != null && s.Needs.Hunger >= 95f)
                return "Dead — starvation";
            return "Dead";
        }

        private static string BuildGameModeLabel(CampaignResult result)
        {
            if (result == null) return string.Empty;
            return result.Mode switch
            {
                GameModeKind.Story => $"Story ({result.TargetDurationDays} days)",
                GameModeKind.Expert => $"Expert ({result.TargetDurationDays} days)",
                GameModeKind.Custom => $"Custom ({result.TargetDurationDays} days)",
                _ => $"{result.Mode} ({result.TargetDurationDays} days)"
            };
        }

        private static string BuildDilemmaChoiceText(MoralDilemmaEvent evt)
        {
            string choice = evt.ChosenResolution switch
            {
                DesperateChoiceKind.Butchering => "We made the unforgivable choice — butchering to survive.",
                DesperateChoiceKind.Starve => "We chose to endure starvation rather than compromise ourselves.",
                DesperateChoiceKind.AbandonWeak => "We abandoned the weakest among us to the cold.",
                _ => "A desperate choice was made."
            };
            return choice;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Save/load — lightweight: timeline and snapshot are campaign-duration
        // data; reconstructed from EventBus on load if not persisted.
        // ─────────────────────────────────────────────────────────────────────

        [Serializable]
        public class BridgeSave
        {
            public MoralChronicleEntry[] Timeline;
            public string JournalSnapshot;
        }

        public BridgeSave CaptureState()
        {
            return new BridgeSave
            {
                Timeline = _timeline.ToArray(),
                JournalSnapshot = _journalSnapshot
            };
        }

        public void RestoreState(BridgeSave save)
        {
            _timeline.Clear();
            _journalSnapshot = string.Empty;
            if (save == null) return;
            _journalSnapshot = save.JournalSnapshot ?? string.Empty;
            if (save.Timeline != null)
            {
                for (int i = 0; i < save.Timeline.Length; i++)
                {
                    if (save.Timeline[i] != null) _timeline.Add(save.Timeline[i]);
                }
            }
        }
    }
}
