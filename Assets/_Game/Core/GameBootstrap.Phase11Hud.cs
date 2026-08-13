using System;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private string _phase11FocusedSurvivorId;
        private bool _phase11HudWired;
        private bool _phase11TicksRegistered;

        private void RegisterPhase11Ticks()
        {
            if (_phase11TicksRegistered) return;
            _phase11TicksRegistered = true;
            _registry.RegisterPerSubstep("phase11KeepsakeGrief", h => TickKeepsakeGriefSurvivors(h));
            _registry.RegisterPerSubstep("phase11AddictionFade", h => TickAddictionRecoveredFade(h));
        }

        private void TickKeepsakeGriefSurvivors(float gameHours)
        {
            if (PersonalKeepsakeSystem == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null) continue;
                PersonalKeepsakeSystem.TickGriefDecay(sv, gameHours);
                if (_hud?.KeepsakeSlotUi != null &&
                    string.Equals(sv.Id, _phase11FocusedSurvivorId, StringComparison.Ordinal))
                {
                    _hud.KeepsakeSlotUi.SetKeepsake(sv.Id, sv.PersonalKeepsakeItemId,
                        sv.HasLostKeepsake, sv.KeepsakeGriefLevel);
                }
            }
        }

        private void TickAddictionRecoveredFade(float gameHours)
        {
            if (_hud?.AddictionDetoxIndicator == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null) continue;
                _hud.AddictionDetoxIndicator.TickRecoveredFade(sv.Id, gameHours);
            }
        }

        private void WirePhase11ExpansionHud()
        {
            if (_hud == null || _phase11HudWired) return;
            _phase11HudWired = true;

            BindPhase11Documents();
            RegisterPhase11Ticks();

            WirePhase11RadiationIndicator();
            WirePhase11PhantomVignette();
            WirePhase11Hypervigilance();
            WirePhase11MoralBranch();
            WirePhase11KeepsakeSlot();
            WirePhase11MemorialWall();
            WirePhase11TerminalPrognosis();
            WirePhase11AddictionDetox();
            WirePhase11PortraitSelection();

            PaintPhase11InitialState();
        }

        /// <summary>
        /// Phase 11 widgets query named elements inside DiegeticHud.uxml, so they
        /// must share the DiegeticHud UIDocument rather than owning their own.
        /// </summary>
        private void BindPhase11Documents()
        {
            var doc = _hud.DiegeticHud != null ? _hud.DiegeticHud.Document : null;
            if (doc == null) return;

            _hud.RadiationPhaseIndicator?.BindDocument(doc);
            _hud.PhantomMemoryVignette?.BindDocument(doc);
            _hud.HypervigilanceIndicator?.BindDocument(doc);
            _hud.MoralBranchDisplay?.BindDocument(doc);
            _hud.KeepsakeSlotUi?.BindDocument(doc);
            _hud.MemorialWallUi?.BindDocument(doc);
            _hud.TerminalPrognosisBanner?.BindDocument(doc);
            _hud.AddictionDetoxIndicator?.BindDocument(doc);
        }

        private void WirePhase11PortraitSelection()
        {
            if (_hud.SurvivorPortraitCard == null) return;
            Action<string> onClick = OnPhase11PortraitSelected;
            _hud.SurvivorPortraitCard.OnCardClicked += onClick;
            _subscriptions.Track(() => _hud.SurvivorPortraitCard.OnCardClicked -= onClick);
        }

        private void OnPhase11PortraitSelected(string survivorId)
        {
            _phase11FocusedSurvivorId = survivorId;
            RefreshPhase11FocusedSurvivor(survivorId);
        }

        private void RefreshPhase11FocusedSurvivor(string survivorId)
        {
            var sv = FindSurvivorById(survivorId);
            if (sv == null) return;

            _hud.RadiationPhaseIndicator?.SetFocusedSurvivor(survivorId);
            _hud.HypervigilanceIndicator?.SetFocusedSurvivor(survivorId);
            _hud.MoralBranchDisplay?.SetFocusedSurvivor(survivorId);
            _hud.KeepsakeSlotUi?.SetFocusedSurvivor(survivorId);
            _hud.TerminalPrognosisBanner?.SetFocusedSurvivor(survivorId);
            _hud.AddictionDetoxIndicator?.SetFocusedSurvivor(survivorId);
            _hud.MemorialWallUi?.SetActiveSurvivor(survivorId);

            if (RadiationPhaseProgression != null && _hud.RadiationPhaseIndicator != null)
            {
                _hud.RadiationPhaseIndicator.SetPhase(survivorId, sv.SicknessPhase);
                _hud.RadiationPhaseIndicator.SetTooltipText(
                    RadiationPhaseProgression.GetPhasePrognosisText(sv));
            }

            _hud.HypervigilanceIndicator?.UpdateLevel(survivorId, sv.HypervigilanceLevel);
            _hud.MoralBranchDisplay?.SetBranch(survivorId, sv.BranchDirection);
            _hud.KeepsakeSlotUi?.SetKeepsake(survivorId, sv.PersonalKeepsakeItemId,
                sv.HasLostKeepsake, sv.KeepsakeGriefLevel);

            if (sv.HasTerminalPrognosis && _hud.TerminalPrognosisBanner != null)
                _hud.TerminalPrognosisBanner.Show(survivorId, sv.TerminalPrognosisDaysRemaining, "");
            else
                _hud.TerminalPrognosisBanner?.HideForSurvivor(survivorId);

            PaintAddictionState(sv);
        }

        private void WirePhase11RadiationIndicator()
        {
            if (RadiationPhaseProgression == null || _hud.RadiationPhaseIndicator == null) return;
            Action<Survivor, RadiationSicknessPhase, RadiationSicknessPhase> onPhase =
                (sv, oldPhase, newPhase) =>
                {
                    _hud.RadiationPhaseIndicator.SetPhase(sv.Id, newPhase);
                    _hud.RadiationPhaseIndicator.SetTooltipText(
                        RadiationPhaseProgression.GetPhasePrognosisText(sv));
                };
            RadiationPhaseProgression.OnPhaseChanged += onPhase;
            _subscriptions.Track(() => RadiationPhaseProgression.OnPhaseChanged -= onPhase);
        }

        private void WirePhase11PhantomVignette()
        {
            if (PhantomMemorySystem == null || _hud.PhantomMemoryVignette == null) return;
            Action<Survivor, string, bool> onTrigger = (sv, itemId, isMot) =>
            {
                string text = PhantomMemorySystem.ResolveTriggerText(sv, itemId, isMot);
                _hud.PhantomMemoryVignette.Trigger(sv.DisplayName, text, isMot);
            };
            PhantomMemorySystem.OnPhantomTrigger += onTrigger;
            _subscriptions.Track(() => PhantomMemorySystem.OnPhantomTrigger -= onTrigger);
        }

        private void WirePhase11Hypervigilance()
        {
            if (CombatTraumaSystem == null || _hud.HypervigilanceIndicator == null) return;
            Action<Survivor, float> onHyper = (sv, level) =>
                _hud.HypervigilanceIndicator.UpdateLevel(sv.Id, level);
            CombatTraumaSystem.OnHypervigilanceIncreased += onHyper;
            _subscriptions.Track(() => CombatTraumaSystem.OnHypervigilanceIncreased -= onHyper);

            Action<Survivor> onAlarm = sv =>
                _hud.HypervigilanceIndicator.TriggerFalseAlarm(sv.Id);
            CombatTraumaSystem.OnFalseAlarmTriggered += onAlarm;
            _subscriptions.Track(() => CombatTraumaSystem.OnFalseAlarmTriggered -= onAlarm);
        }

        private void WirePhase11MoralBranch()
        {
            if (MoralBranchingSystem == null || _hud.MoralBranchDisplay == null) return;
            Action<Survivor, MoralBranchDirection> onBranch = (sv, dir) =>
                _hud.MoralBranchDisplay.SetBranch(sv.Id, dir);
            MoralBranchingSystem.OnBranchDecided += onBranch;
            _subscriptions.Track(() => MoralBranchingSystem.OnBranchDecided -= onBranch);
        }

        private void WirePhase11KeepsakeSlot()
        {
            if (PersonalKeepsakeSystem == null || _hud.KeepsakeSlotUi == null) return;
            Action<Survivor, string> onLost = (sv, itemId) =>
                _hud.KeepsakeSlotUi.SetKeepsake(sv.Id, sv.PersonalKeepsakeItemId, true, sv.KeepsakeGriefLevel);
            PersonalKeepsakeSystem.OnKeepsakeLost += onLost;
            _subscriptions.Track(() => PersonalKeepsakeSystem.OnKeepsakeLost -= onLost);
        }

        private void WirePhase11MemorialWall()
        {
            if (MemorialWallSystem == null || _hud.MemorialWallUi == null) return;
            Action<MemorialEntry> onEntry = entry => _hud.MemorialWallUi.AddEntry(entry);
            MemorialWallSystem.OnMemorialEntryAdded += onEntry;
            _subscriptions.Track(() => MemorialWallSystem.OnMemorialEntryAdded -= onEntry);

            Action<string> onPay = survivorId =>
            {
                var sv = FindSurvivorById(survivorId);
                if (sv != null) MemorialWallSystem.PayRespects(sv);
            };
            _hud.MemorialWallUi.OnPayRespectsRequested += onPay;
            _subscriptions.Track(() => _hud.MemorialWallUi.OnPayRespectsRequested -= onPay);
        }

        private void WirePhase11TerminalPrognosis()
        {
            if (FinalWishSystem == null || _hud.TerminalPrognosisBanner == null) return;
            Action<Survivor, string, float> onDeclared = (sv, wishId, days) =>
                _hud.TerminalPrognosisBanner.Show(sv.Id, days, wishId);
            FinalWishSystem.OnTerminalPrognosisDeclared += onDeclared;
            _subscriptions.Track(() => FinalWishSystem.OnTerminalPrognosisDeclared -= onDeclared);

            Action<Survivor> onCompleted = sv =>
                _hud.TerminalPrognosisBanner.SetWishOutcome(sv.Id,
                    TerminalPrognosisBanner.WishOutcome.Completed);
            FinalWishSystem.OnFinalWishCompleted += onCompleted;
            _subscriptions.Track(() => FinalWishSystem.OnFinalWishCompleted -= onCompleted);

            Action<Survivor> onFailed = sv =>
                _hud.TerminalPrognosisBanner.SetWishOutcome(sv.Id,
                    TerminalPrognosisBanner.WishOutcome.Failed);
            FinalWishSystem.OnFinalWishFailed += onFailed;
            _subscriptions.Track(() => FinalWishSystem.OnFinalWishFailed -= onFailed);
        }

        private void WirePhase11AddictionDetox()
        {
            if (ChemicalDependencySystem == null || _hud.AddictionDetoxIndicator == null) return;

            Action<Survivor, string> onFormed = (sv, itemId) =>
                _hud.AddictionDetoxIndicator.ShowDependency(sv.Id, itemId,
                    AddictionDetoxIndicator.DetoxState.Dependent);
            ChemicalDependencySystem.OnDependencyFormed += onFormed;
            _subscriptions.Track(() => ChemicalDependencySystem.OnDependencyFormed -= onFormed);

            Action<Survivor, string> onWithdrawal = (sv, itemId) =>
                _hud.AddictionDetoxIndicator.ShowDependency(sv.Id, itemId,
                    AddictionDetoxIndicator.DetoxState.Withdrawal);
            ChemicalDependencySystem.OnWithdrawalStarted += onWithdrawal;
            _subscriptions.Track(() => ChemicalDependencySystem.OnWithdrawalStarted -= onWithdrawal);

            Action<Survivor, string> onDetox = (sv, itemId) =>
                _hud.AddictionDetoxIndicator.ShowDependency(sv.Id, itemId,
                    AddictionDetoxIndicator.DetoxState.Recovered);
            ChemicalDependencySystem.OnDetoxCompleted += onDetox;
            _subscriptions.Track(() => ChemicalDependencySystem.OnDetoxCompleted -= onDetox);
        }

        private void PaintPhase11InitialState()
        {
            if (_hud == null || Survivors == null || Survivors.Count == 0) return;
            MemorialWallSystem?.SyncDeadSurvivors(Survivors);
            if (MemorialWallSystem != null && _hud.MemorialWallUi != null)
            {
                _hud.MemorialWallUi.ClearEntries();
                var entries = MemorialWallSystem.Entries;
                for (int i = 0; i < entries.Count; i++)
                    _hud.MemorialWallUi.AddEntry(entries[i]);
            }

            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null) continue;
                _hud.RadiationPhaseIndicator?.SetPhase(sv.Id, sv.SicknessPhase);
                _hud.HypervigilanceIndicator?.UpdateLevel(sv.Id, sv.HypervigilanceLevel);
                _hud.MoralBranchDisplay?.SetBranch(sv.Id, sv.BranchDirection);
                _hud.KeepsakeSlotUi?.SetKeepsake(sv.Id, sv.PersonalKeepsakeItemId,
                    sv.HasLostKeepsake, sv.KeepsakeGriefLevel);
                if (sv.HasTerminalPrognosis)
                    _hud.TerminalPrognosisBanner?.Show(sv.Id, sv.TerminalPrognosisDaysRemaining, "");
                PaintAddictionState(sv);
            }

            _phase11FocusedSurvivorId = Survivors[0].Id;
            RefreshPhase11FocusedSurvivor(_phase11FocusedSurvivorId);
        }

        private void PaintAddictionState(Survivor sv)
        {
            if (_hud.AddictionDetoxIndicator == null || sv == null) return;
            if (sv.IsInWithdrawal)
            {
                string itemId = sv.ChemicalDependencies != null && sv.ChemicalDependencies.Count > 0
                    ? sv.ChemicalDependencies[0].ItemId : "unknown";
                _hud.AddictionDetoxIndicator.ShowDependency(sv.Id, itemId,
                    AddictionDetoxIndicator.DetoxState.Withdrawal);
                return;
            }
            if (sv.ChemicalDependencies != null && sv.ChemicalDependencies.Count > 0)
            {
                var dep = sv.ChemicalDependencies[0];
                var state = dep.InManagedDetox
                    ? AddictionDetoxIndicator.DetoxState.ManagedDetox
                    : AddictionDetoxIndicator.DetoxState.Dependent;
                _hud.AddictionDetoxIndicator.ShowDependency(sv.Id, dep.ItemId, state);
                return;
            }
            _hud.AddictionDetoxIndicator.ShowDependency(sv.Id, "", AddictionDetoxIndicator.DetoxState.Clean);
        }
    }
}
