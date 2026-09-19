// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Tests for the MedicalHostSession ticking contract:
    /// validating that TickHours advances active chemical dependencies,
    /// manages detox progression, applies crafting/combat withdrawal penalties,
    /// requests morale drains, and that TickVigil advances bedside presence.
    /// Note: TickDemo was permanently retired in Task #133 in favor of TickHours
    /// (enforced by MedicalPipelineArchitectureGateTests.MedicalHostSession_HasNoTickDemo).
    /// </summary>
    public class MedicalHostSessionTests
    {
        private sealed class TestMedicalHostSession : StatefulSessionBase
        {
            public ChemicalDependencySystem Engine { get; }
            public VigilStateMachine Vigil { get; }

            public float TotalMoraleDrain { get; private set; }
            public float ActiveCraftingPenalty { get; private set; }
            public float ActiveCombatPenalty { get; private set; }
            public string LastEvent { get; private set; } = string.Empty;

            public TestMedicalHostSession(ChemicalDependencySystem? engine = null, VigilStateMachine? vigil = null)
            {
                Engine = engine ?? new ChemicalDependencySystem();
                Vigil = vigil ?? new VigilStateMachine();

                Engine.OnMoraleDrainRequested += (sv, amount) =>
                {
                    TotalMoraleDrain += amount;
                    RaiseStateChanged();
                };
                Engine.OnCraftingPenaltyChanged += (sv, factor) =>
                {
                    ActiveCraftingPenalty = factor;
                    RaiseStateChanged();
                };
                Engine.OnCombatPenaltyChanged += (sv, factor) =>
                {
                    ActiveCombatPenalty = factor;
                    RaiseStateChanged();
                };
                Engine.OnDependencyFormed += (sv, item) =>
                {
                    LastEvent = $"Dependency formed: {sv} on {item}.";
                    RaiseStateChanged();
                };
                Engine.OnDetoxCompleted += (sv, item) =>
                {
                    LastEvent = $"Detox complete: {sv} clean of {item}.";
                    RaiseStateChanged();
                };
                Engine.OnStateChanged += () => RaiseStateChanged();

                Vigil.OnVigilStarted += id => { LastEvent = $"Vigil begun for {id}."; RaiseStateChanged(); };
                Vigil.OnVigilCompleted += skipped => { LastEvent = $"Vigil completed (skipped: {skipped})"; RaiseStateChanged(); };
            }

            public void TickHours(float hours)
            {
                foreach (var sv in new List<string>(Engine.Ledger.Keys))
                    Engine.TickHours(sv, hours);
            }

            public void TickVigil(double deltaSeconds)
            {
                if (Vigil == null || !Vigil.IsActive) return;
                Vigil.Tick((float)deltaSeconds);
                RaiseStateChanged();
            }

            public float VigilProgress =>
                Vigil == null || Vigil.DurationSeconds <= 0f ? 0f
                : Math.Clamp(Vigil.ElapsedSeconds / Vigil.DurationSeconds, 0f, 1f);

            public string HoldVigil(string survivorId)
            {
                if (string.IsNullOrEmpty(survivorId)) return "No patient named for the vigil.";
                if (Vigil.IsActive) return $"A vigil is already kept for {Vigil.DwellerId}.";
                Vigil.StartVigil(survivorId, Array.Empty<string>());
                RaiseStateChanged();
                return $"Vigil begun for {survivorId}. Sit with them.";
            }

            public string BeginDetox(string survivorId, string itemId, bool managed)
            {
                bool ok = managed
                    ? Engine.BeginManagedDetox(survivorId, itemId)
                    : Engine.BeginColdTurkey(survivorId, itemId);
                return ok ? $"Detox begun for {survivorId} ({itemId})." : "Detox refused (below threshold or unknown).";
            }
        }

        [Fact]
        public void TickHours_EmptyLedger_CausesNoMoraleDrainOrStateChange()
        {
            var session = new TestMedicalHostSession();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            session.TickHours(24f);

            Assert.Equal(0f, session.TotalMoraleDrain);
            Assert.Equal(0f, session.ActiveCraftingPenalty);
            Assert.Equal(0f, session.ActiveCombatPenalty);
            Assert.False(stateChanged);
        }

        [Fact]
        public void TickHours_ManagedDetox_AdvancesProgressAndDrainsMorale()
        {
            var session = new TestMedicalHostSession();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            // Form dependency: 2 doses cross threshold (0.15 + 0.15 = 0.30 >= 0.30)
            session.Engine.OnSubstanceConsumed("sv_patient", "narcotic_morphine", ChemicalDependencyKind.Opioid);
            session.Engine.OnSubstanceConsumed("sv_patient", "narcotic_morphine", ChemicalDependencyKind.Opioid);
            session.BeginDetox("sv_patient", "narcotic_morphine", managed: true);

            stateChanged = false;
            session.TickHours(24f);

            Assert.True(session.TotalMoraleDrain > 0f);
            Assert.True(stateChanged);
            Assert.True(session.IsDirty);
        }

        [Fact]
        public void TickHours_ManagedDetox_CompletesWhenThresholdExceeded()
        {
            var session = new TestMedicalHostSession();
            session.Engine.OnSubstanceConsumed("sv_patient", "narcotic_morphine", ChemicalDependencyKind.Opioid);
            session.Engine.OnSubstanceConsumed("sv_patient", "narcotic_morphine", ChemicalDependencyKind.Opioid);
            session.BeginDetox("sv_patient", "narcotic_morphine", managed: true);

            // Managed detox completes at 96 hours
            session.TickHours(100f);

            Assert.Contains("clean of narcotic_morphine", session.LastEvent);
            Assert.Empty(session.Engine.Ledger["sv_patient"]);
        }

        [Fact]
        public void TickHours_ColdTurkey_AppliesCraftingAndCombatPenaltiesAndClearsOnCompletion()
        {
            var session = new TestMedicalHostSession();
            session.Engine.OnSubstanceConsumed("sv_patient", "sedative_amp", ChemicalDependencyKind.Sedative);
            session.Engine.OnSubstanceConsumed("sv_patient", "sedative_amp", ChemicalDependencyKind.Sedative);
            session.BeginDetox("sv_patient", "sedative_amp", managed: false);

            session.TickHours(10f);

            Assert.Equal(ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty, session.ActiveCraftingPenalty);
            Assert.Equal(ChemicalDependencySystem.ColdTurkeyTremorCombatPenalty, session.ActiveCombatPenalty);

            // Cold turkey duration is 72 hours
            session.TickHours(70f);

            Assert.Equal(0f, session.ActiveCraftingPenalty);
            Assert.Equal(0f, session.ActiveCombatPenalty);
            Assert.Contains("clean of sedative_amp", session.LastEvent);
        }

        [Fact]
        public void TickHours_CleanDecay_DecaysLevelWithoutWithdrawalPenalties()
        {
            var session = new TestMedicalHostSession();
            // Single dose = 0.15 (below 0.30 threshold, clean decay)
            session.Engine.OnSubstanceConsumed("sv_patient", "alcohol_clean", ChemicalDependencyKind.Alcohol);

            // Decay is 0.05 per day (24h)
            session.TickHours(24f);

            var dep = session.Engine.Ledger["sv_patient"][0];
            Assert.InRange(dep.dependencyLevel, 0.09f, 0.11f);
            Assert.Equal(0f, session.ActiveCraftingPenalty);
            Assert.Equal(0f, session.ActiveCombatPenalty);
            Assert.Equal(0f, session.TotalMoraleDrain);

            // Tick remaining days to resolve completely
            session.TickHours(72f);
            Assert.Empty(session.Engine.Ledger["sv_patient"]);
        }

        [Fact]
        public void TickHours_MultipleSurvivors_TicksAllTrackedSurvivors()
        {
            var session = new TestMedicalHostSession();
            session.Engine.OnSubstanceConsumed("sv_alpha", "morphine", ChemicalDependencyKind.Opioid);
            session.Engine.OnSubstanceConsumed("sv_alpha", "morphine", ChemicalDependencyKind.Opioid);
            session.BeginDetox("sv_alpha", "morphine", managed: true);

            session.Engine.OnSubstanceConsumed("sv_beta", "morphine", ChemicalDependencyKind.Opioid);
            session.Engine.OnSubstanceConsumed("sv_beta", "morphine", ChemicalDependencyKind.Opioid);
            session.BeginDetox("sv_beta", "morphine", managed: true);

            float drainBefore = session.TotalMoraleDrain;
            session.TickHours(24f);

            // Both survivors should have generated morale drain
            Assert.True(session.TotalMoraleDrain > drainBefore);
        }

        [Fact]
        public void TickHours_ZeroAndNegativeHours_NoOp()
        {
            var session = new TestMedicalHostSession();
            session.Engine.OnSubstanceConsumed("sv_patient", "morphine", ChemicalDependencyKind.Opioid);

            float levelBefore = session.Engine.Ledger["sv_patient"][0].dependencyLevel;
            session.TickHours(0f);
            session.TickHours(-10f);

            Assert.Equal(levelBefore, session.Engine.Ledger["sv_patient"][0].dependencyLevel);
            Assert.Equal(0f, session.TotalMoraleDrain);
        }

        [Fact]
        public void TickVigil_AdvancesVigilElapsedSecondsAndProgress()
        {
            var session = new TestMedicalHostSession();
            session.HoldVigil("sv_dying");

            Assert.True(session.Vigil.IsActive);
            float progressBefore = session.VigilProgress;

            session.TickVigil(15.0);

            Assert.Equal(15f, session.Vigil.ElapsedSeconds);
            Assert.True(session.VigilProgress > progressBefore);
        }
    }
}
