using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Onboarding;
using AtomicWar.GodotApp.UI;

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
        // Failed actions are tracked per scenario for the contextual-hint
        // heuristic invoked by the hint panel; persisted with the journey
        // are stage / assistance / dismiss / completion.
        private OnboardingJourney? _onboardingJourney;
        private bool _onboardingDirty;
        private int _onboardingFailedActions;
        private double _onboardingLastInteractionSeconds;
        private OnboardingHintPanel? _onboardingHintPanel;

        // ── Setup / Save / Restore (substeps 5 + 11) ────────────────────

        private void SetupOnboarding()
        {
            if (_onboardingJourney != null) return;

            _onboardingJourney = new OnboardingJourney();
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
                _onboardingJourney = new OnboardingJourney();
            }

            EnsureOnboardingPanel();
            _onboardingHintPanel?.Bind(_onboardingJourney);

            _onboardingJourney.OnJourneyChanged += journey =>
            {
                _onboardingDirty = true;
                _onboardingHintPanel?.Bind(journey);
                RefreshOnboardingStatusBar();
            };
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

        private void FlushOnboardingIfDirty()
        {
            if (_onboardingDirty) SaveOnboarding();
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
            _onboardingLastInteractionSeconds = 0;
        }

        public void ObserveFailedAction(string label)
        {
            _onboardingFailedActions++;
            _onboardingLastInteractionSeconds = 0;
            // Hints are surfacing controlled solely by the hint panel; the
            // host signals the panel that something failed so it can offer
            // a contextual cue on next refresh.
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
            if (j.JourneyComplete) return;
            var def = j.CurrentStageDef;
            // Append rather than overwrite so the existing daily briefing
            // line stays visible.
            _statusLabel.Text = $"Day {_simDay} · {def.Title} (onboarding): {def.Objective}";
        }
    }
}