// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Onboarding;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Main-partial orchestration of the persisted first-hour onboarding
    /// journey. Owns the active <see cref="OnboardingJourney"/>, the
    /// accessible <see cref="OnboardingHintPanel"/>, and the bridge between
    /// genuine runtime commands (panel opens, duty assignments, day advances,
    /// ration directives) and journey signal recording. Implements the spec
    /// substeps:
    /// </summary>
    public partial class Main : Control
    {
        // ── Fields ────────────────────────────────────────────────────────
        // The [journey] remains engine-free in Core and survives save/load.
        // Persisted with the journey are stage / assistance / dismiss /
        // completion; the host additionally tracks only the dirty flag that
        // triggers a save. (No per-scenario failure counter exists: failed
        // commands simply refresh the hint panel.)
        private OnboardingJourney? _onboardingJourney;
        private bool _onboardingDirty;
        private OnboardingHintPanel? _onboardingHintPanel;

        // ── Setup / Save / Restore (substeps 5 + 11) ────────────────────

        private void SetupOnboarding()
        {
            if (_onboardingJourney != null) return;

            if (AtomicWar.GodotApp.Settings.UserSettingsStore.Current.TutorialMode == 2)
            {
                _onboardingJourney = null;
                return;
            }

            _onboardingJourney = OnboardingJourney.CreateFirstHour();
            try
            {
                var saved = OnboardingSaveStore.TryLoad();
                if (saved != null)
                {
                    _onboardingJourney = OnboardingJourney.Restore(saved);
                }
            }
            catch (InvalidOperationException ex)
            {
                GD.PushWarning($"[Onboarding] Save load rejected: {ex.Message}. Starting fresh.");
                _onboardingJourney = OnboardingJourney.CreateFirstHour();
            }

            EnsureOnboardingPanel();
            _onboardingHintPanel?.Bind(_onboardingJourney);
            if (_tutorialPanel != null)
            {
                _tutorialPanel.OnContextualAcknowledged -= OnContextualTutorialAcknowledged;
                _tutorialPanel.OnContextualAcknowledged += OnContextualTutorialAcknowledged;
            }

            _onboardingJourney.OnJourneyChanged += journey =>
            {
                _onboardingDirty = true;
                _onboardingHintPanel?.Bind(journey);
                RefreshOnboardingStatusBar();
            };
            _onboardingJourney.OnContextualTutorialRequested += tutorialId =>
            {
                _onboardingDirty = true;
                FlushContextualTutorialQueue();
            };
            FlushContextualTutorialQueue();
            // Day tick after a load: read live day and reconcile.
            _onboardingJourney.SetDay(Math.Max(1, _simDay));
            RefreshOnboardingStatusBar();
        }

        private void EnsureOnboardingPanel()
        {
            if (_onboardingHintPanel != null && _onboardingHintPanel.IsInsideTree())
                return;
            var panel = new OnboardingHintPanel();
            panel.OnShowMeWhereRequested += route => OpenPlayerPanel(route);
            panel.OnCurrentStepSkipped += SkipCurrentStage;
            panel.OnJourneyReplayed += ReplayJourney;
            panel.OnHintDismissed += () => RefreshOnboardingStatusBar();
            panel.OnAssistanceChanged += SetOnboardingAssistance;
            AddChild(panel);
            _onboardingHintPanel = panel;
        }

        private void FlushContextualTutorialQueue()
        {
            if (_onboardingJourney == null || _tutorialPanel == null) return;
            if (_onboardingJourney.ContextualTutorialQueue.Count == 0) return;
            if (_tutorialPanel.IsContextualLessonVisible) return;
            string tutorialId = _onboardingJourney.ContextualTutorialQueue[0];
            _tutorialPanel.ShowContextual(tutorialId);
        }

        private void OnContextualTutorialAcknowledged(string tutorialId)
        {
            if (_onboardingJourney == null) return;
            if (_onboardingJourney.AcknowledgeContextualTutorial(tutorialId))
                _onboardingDirty = true;
            FlushContextualTutorialQueue();
        }

        private void RestoreOnboardingFromDisk()
        {
            // Tear down any prior handle and reload from the campaign envelope.
            _onboardingJourney = null;
            _onboardingDirty = false;
            SetupOnboarding();
        }

        private void SaveOnboarding()
        {
            if (_onboardingJourney == null) return;
            var section = "onboarding";
            var payload = OnboardingSaveStore.TryCapturePersisted(_onboardingJourney.CaptureState());
            if (CaptureSection(section, payload))
            {
                _onboardingDirty = false;
                GD.Print("[Ashfall Godot] Onboarding save written.");
            }
        }

        // ── Observation hooks (substep 4) ────────────────────────────────

        /// <summary>Records a real, reproducible sigil against a genuine
        /// runtime command. The tracker's argument is a stable ID that the
        /// spec promises will never be silently remapped.</summary>
        public void ObserveSigil(string sigil)
        {
            if (_onboardingJourney == null) SetupOnboarding();
            if (_onboardingJourney == null || string.IsNullOrWhiteSpace(sigil)) return;
            _onboardingJourney.RecordSigil(sigil);
            // Plan 46 — the canonical onboarding/UI sigil seam also feeds the
            // local play-session recorder; the JSONL stream stays an audit read
            // model and never feeds gameplay.
            RecordPlayMetricSigil(sigil);
        }

        public void ObserveFailedAction(string label)
        {
            // Hints are surfaced solely by the hint panel; the host signals the
            // panel that a command failed so it refreshes (the panel renders the
            // current objective/hint copy). There is deliberately no per-scenario
            // counter or cooldown bookkeeping here — the panel owns presentation.
            _onboardingHintPanel?.RefreshView();
        }

        private void ObserveCurrentDay() => _onboardingJourney?.SetDay(_simDay);

        // ── Recovery affordances (substep 7) ─────────────────────────────

        public void SkipCurrentStage()
        {
            if (_onboardingJourney == null) return;
            _onboardingJourney.SkipCurrent();
        }

        public void SkipAllOnboardingStages()
        {
            _onboardingJourney?.SkipAllRemaining();
        }

        public void ReplayJourney()
        {
            _onboardingJourney?.Replay();
        }

        private void ResetOnboardingJourney()
        {
            if (_onboardingJourney == null)
                SetupOnboarding();
            _onboardingJourney?.Replay();
            _onboardingHintPanel?.RefreshView();
        }

        private void ApplyOnboardingSettings(Ashfall.Core.Settings.UserSettingsData settings)
        {
            // Plan 184 Path α — refresh owner for preference-sensitive surfaces.
            RefreshAccessibilityPreferenceSurfaces();

            if (settings.TutorialMode == 2)
            {
                if (_onboardingHintPanel != null)
                    _onboardingHintPanel.Visible = false;
                _onboardingJourney = null;
                _onboardingDirty = false;
                return;
            }

            if (_onboardingJourney == null)
                SetupOnboarding();
            _onboardingHintPanel?.RefreshView();
        }

        /// <summary>
        /// Re-renders open panels that cache hazard/dose presentation so
        /// <c>hazard_text_labels</c> (and later preference-aware text) stay truthful
        /// after APPLY &amp; SAVE. Engine scale/modulate already applied in
        /// <see cref="Settings.UserSettingsStore.Apply"/>.
        /// </summary>
        private void RefreshAccessibilityPreferenceSurfaces()
        {
            _doseLedgerPanel?.RefreshView();
            _radiationHistoryPanel?.RefreshView();
            // Plan 51 — the presentation motion profile is preference-sensitive.
            RefreshHoldfastPresentation();
        }

        public void DismissOnboardingHint()
        {
            _onboardingHintPanel?.MarkHintDismissed();
        }

        public void SetOnboardingAssistance(OnboardingAssistance level)
        {
            if (_onboardingJourney != null)
            {
                _onboardingJourney.SetAssistance(level);
                _onboardingDirty = true;
            }
            _onboardingHintPanel?.CycleAssistance(level);
        }

        public bool HasShowMeWhereOffered(OnboardingStage stage) =>
            _onboardingJourney?.HasShownShowMeWhere(stage) ?? false;

        public void RecordShowMeWhere(OnboardingStage stage) =>
            _onboardingJourney?.RecordShowMeWhere(stage);

        /// <summary>Surfaces the current objective unobtrusively in the status
        /// label so a new player sees a reminder even when the hint panel is
        /// not open. Restful, never modal.</summary>
        private void RefreshOnboardingStatusBar()
        {
            if (_onboardingJourney == null || _statusLabel == null) return;
            var j = _onboardingJourney;
            if (j.JourneyComplete)
            {
                // Truthful terminal state: the final objective no longer applies,
                // so surface the completion copy instead of leaving a stale
                // "CURRENT: …" claim on the status label forever.
                _statusLabel.Text = AshfallLocalization.Tr(
                    "onboarding.status.first_hour_complete",
                    "The first-hour sequence is complete. The shelter is yours to manage.");
                return;
            }
            var def = j.CurrentStageDef;
            // Replace the status line with the current objective so a new player
            // sees a reminder even when the hint panel is closed.
            // One normalization shared with the hint panel so the status bar
            // and the panel name the same localization row.
            string stageId = OnboardingHintPanel.StageLocalizationId(def.Id);
            string title = AshfallLocalization.Tr($"onboarding.{stageId}.title", def.Title);
            string objective = AshfallLocalization.Tr($"onboarding.{stageId}.objective", def.Objective);
            _statusLabel.Text = AshfallLocalization.TrFormat(
                "onboarding.status.bar", _simDay, title, objective);
        }
    }
}
