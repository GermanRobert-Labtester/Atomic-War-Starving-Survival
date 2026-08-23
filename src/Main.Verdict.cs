using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Verdict fields (GAP-ARCH-01 Phase 1) ──
        private AtomicWar.GodotApp.VerdictHostSession _verdict = null!;
        private Godot.Label _verdictReadoutLabel = null!;
        private VerdictPanel _verdictPanel = null!;
        private bool _verdictDirty;

        private void FlushVerdictIfDirty()
        {
            if (_verdictDirty) SaveVerdict();
        }

        private void SetupVerdict()
        {
            if (_verdict != null) return;
            _verdict = AtomicWar.GodotApp.VerdictHostSession.Create(_dataDir);
            _verdict.StateChanged += () => { _verdictDirty = true; RefreshVerdictReadout(); };
            UnlockVerdictLore();
            RefreshVerdictReadout();

            // Items 1+8: the diegetic shelter machine surface + a persistent readout strip
            // (previously declared but never added to the tree).
            if (_verdictReadoutLabel == null)
            {
                _verdictReadoutLabel = new Label
                {
                    Text = "[shelter instruments] — standby cycle.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                _verdictReadoutLabel.AddThemeFontSizeOverride("font_size", 12);
                _rightColumn.AddChild(_verdictReadoutLabel);
            }

            if (_verdictPanel == null && _rightColumn != null)
            {
                _verdictPanel = new VerdictPanel();
                _rightColumn.AddChild(_verdictPanel);
            }
            _verdictPanel?.Bind(_verdict);
            _verdictPanel?.RefreshView();

            GD.Print("[Ashfall Godot] Verdict host ready.");
        }

        /// <summary>Advance the Reckoning state machine + census carrier + chain recorders for the current sim day.</summary>
        private void TickVerdict(int day, int livingCount)
        {
            SetupVerdict();
            _verdict.AdvanceDay(day, Math.Max(1, livingCount), _verdict.MachineLog.ReadCount());
            _verdict.TickCensus();
            _verdict.TickCorruption(day);
            _verdict.TickRadio(day);
            _verdict.EnrollEvidenceFromItems(day);

            // Phase 6.D Chain 1 (Census / Human Cost): record any dwellings that
            // dropped out of coverage between this day and the previous tick.
            // DriftTotal grows monotonically; day boundaries reset the delta.
            int driftDelta = ComputeDwellingDriftDelta(livingCount, day);
            if (driftDelta > 0) _verdict.Reckoning.RecordDrift(day, driftDelta);

            UnlockVerdictLore();
            RefreshVerdictReadout();
        }

        private int ComputeDwellingDriftDelta(int livingCount, int day)
        {
            int delta = 0;
            if (day != _previousLivingDay && _previousLivingDay != -1)
            {
                if (_previousLivingCount > livingCount) delta = _previousLivingCount - livingCount;
            }
            _previousLivingDay = day;
            _previousLivingCount = livingCount;
            return Math.Max(0, delta);
        }

        // Phase 6.D Chain 3 (Survival Reckoning) hook surface. Sums
        // LifetimeDose across all living survivors from the live
        // RadiationSystem. Replaces the previous 0f stub.
        public float LivingCumulativeDoseSieverts()
        {
            if (_survivors == null || _survivors.Roster == null) return 0f;
            float total = 0f;
            foreach (var entry in _survivors.Roster.Roster)
            {
                if (!entry.isAlive) continue;
                var dosimeter = _survivors.Radiation.GetDosimeter(entry.survivorId);
                total += dosimeter?.LifetimeDose ?? 0f;
            }
            return total;
        }

        /// <summary>Unlock lore_verdict_* codex beats from authoritative Verdict state
        /// (located knowledge: the ladder only opens when the machine/evidence reaches it).</summary>
        private void UnlockVerdictLore()
        {
            if (_verdict == null || _journal == null) return;
            if (_verdict.MachineLog.ReadCount() >= 1)
                _journal.UnlockEventFired("lore_verdict_geophone_one");
            if (_verdict.Evidence.IsEnrolled("evidence_fuse_linen"))
            {
                _journal.UnlockEventFired("lore_verdict_shift_charters");
                _journal.UnlockEventFired("lore_verdict_standard");
            }
            if (_verdict.Evidence.IsEnrolled("evidence_uxo_register"))
                _journal.UnlockEventFired("lore_verdict_the_hold");
            if (_verdict.Reckoning.State.callResolved)
            {
                _journal.UnlockEventFired("lore_verdict_the_call");
                _journal.UnlockEventFired("lore_verdict_the_count");
            }
        }

        private void RefreshVerdictReadout()
        {
            if (_verdict == null || _verdictReadoutLabel == null) return;
            _verdictReadoutLabel.Text = Ashfall.Core.Verdict.VerdictReadout.LineFor(
                _verdict.Reckoning.State, _verdict.Evidence.Count, _verdict.MachineLog.ReadCount());
        }

        private void SaveVerdict()
        {
            if (_verdict == null) return;
            if (AtomicWar.GodotApp.VerdictSaveStore.TrySave(_verdict.CaptureSave()))
            {
                _verdictDirty = false;
                GD.Print("[Ashfall Godot] Verdict save written.");
            }
        }

        private void CloseVerdictPanel()
        {
            if (_verdictPanel != null) _verdictPanel.Visible = false;
        }

        private void OnVerdictOpenClicked()
        {
            SetupVerdict();
            _statusLabel.Text = _verdict.StatusLine() + "\n" +
                Ashfall.Core.Verdict.VerdictReadout.LineFor(
                    _verdict.Reckoning.State, _verdict.Evidence.Count, _verdict.MachineLog.ReadCount());
        }

        private void OnVerdictTickClicked()
        {
            SetupVerdict();
            _simDay++;
            TickSimDay(_simDay);
            _statusLabel.Text = _verdict.StatusLine();
        }

        private void OnVerdictCensusClicked()
        {
            SetupVerdict();
            _verdict.TickCensus();
            _statusLabel.Text = "Census broadcast checked. " + _verdict.StatusLine();
        }

        /// <summary>Best-available living count without coupling to Survivors internals.</summary>
        private int LivingDwellerCountEstimate()
        {
            if (_survivors != null && _survivors.Roster != null)
            {
                int count = _survivors.Roster.LivingCount;
                if (count > 0) return count;
            }
            return 14;
        }

    }
}
