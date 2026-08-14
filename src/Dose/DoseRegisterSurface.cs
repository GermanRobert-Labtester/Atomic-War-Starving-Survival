using System;
using System.Text;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp.Dose
{
    /// <summary>
    /// ASHFALL: THE DOSE — the player-facing Dose Register surface (PART C).
    /// One folder of paperwork with four tabs (Ledger / Sick / Cohort /
    /// Voluntary) and the four chaired antagonist rows (PART B) on top.
    /// Thin presentation only: renders DoseLedgerHostSession state and
    /// forwards one-button actions to the host's demo helpers. Zero rules.
    /// </summary>
    public partial class DoseRegisterSurface : PanelContainer
    {
        private DoseLedgerHostSession _session;
        private Label _lblNpcs;
        private Label _lblLedger;
        private Label _lblSick;
        private Label _lblCohort;
        private Label _lblVoluntary;
        private Button _btnCalibrate;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(420, 320);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "THE DOSE REGISTER — ONE FOLDER OF PAPERWORK",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", 13);
            rootVbox.AddChild(title);

            _lblNpcs = new Label { Text = "The four who keep the books: ..." };
            _lblNpcs.AddThemeFontSizeOverride("font_size", 11);
            rootVbox.AddChild(_lblNpcs);

            var tabs = new TabContainer();
            tabs.CustomMinimumSize = new Vector2(0, 240);
            rootVbox.AddChild(tabs);

            _lblLedger = new Label();
            tabs.AddChild(MakeTab("Ledger", _lblLedger));
            _lblSick = new Label();
            tabs.AddChild(MakeTab("Sick", _lblSick));
            _lblCohort = new Label();
            tabs.AddChild(MakeTab("Cohort", _lblCohort));
            _lblVoluntary = new Label();
            tabs.AddChild(MakeTab("Voluntary", _lblVoluntary));

            var actionRow = new HBoxContainer();
            actionRow.AddThemeConstantOverride("separation", 6);
            rootVbox.AddChild(actionRow);

            AddAction(actionRow, "Book a reading", OnBookReading);
            AddAction(actionRow, "Name to sick list", OnNameToSick);
            AddAction(actionRow, "Assign morphine tray", OnAssignMorphine);
            AddAction(actionRow, "Book a child baseline", OnBookChild);
            AddAction(actionRow, "Correct baseline", OnCorrectBaseline);
            AddAction(actionRow, "Sign an hour", OnSignVolunteer);
            AddAction(actionRow, "Mark it done", OnCompleteVolunteer);
            _btnCalibrate = AddAction(actionRow, "Calibrate (Piet)", OnCalibrate);
        }

        private static Control MakeTab(string name, Label label)
        {
            var box = new VBoxContainer();
            box.AddThemeConstantOverride("separation", 4);
            box.Name = name;
            label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            label.AddThemeFontSizeOverride("font_size", 11);
            box.AddChild(label);
            return box;
        }

        private static Button AddAction(Container row, string text, Action handler)
        {
            var button = new Button { Text = text };
            button.Pressed += handler;
            row.AddChild(button);
            return button;
        }

        public void BindSession(DoseLedgerHostSession session)
        {
            UnbindSession();
            _session = session;
            if (_session != null)
                _session.StateChanged += RefreshView;
        }

        private void UnbindSession()
        {
            if (_session == null) return;
            _session.StateChanged -= RefreshView;
            _session = null;
        }

        public override void _ExitTree()
        {
            UnbindSession();
            base._ExitTree();
        }

        public void RefreshView()
        {
            if (_session == null) return;

            var npcSb = new StringBuilder();
            for (int i = 0; i < _session.Registers.npcs.Count; i++)
            {
                var n = _session.Registers.npcs[i];
                npcSb.Append(n.name).Append(" — ").Append(n.disposition).Append('\n');
            }
            _lblNpcs.Text = npcSb.Length > 0 ? npcSb.ToString().TrimEnd() : "The four who keep the books: absent from the register.";
            _lblNpcs.TooltipText = "Book / Name / Assign / Sign. Refusing to write is a valid entry; the ledger records it as silence.";

            _lblLedger.Text = RenderLedger();
            _lblSick.Text = RenderSick();
            _lblCohort.Text = RenderCohort();
            _lblVoluntary.Text = RenderVoluntary();
            _btnCalibrate.Text = _session.Ledger.State.calibrationOverdue ? "Calibrate (Piet) — OVERDUE" : "Calibrate (Piet)";
        }

        private string RenderLedger()
        {
            var sb = new StringBuilder();
            var l = _session.Ledger;
            sb.Append(l.State.calibrationOverdue ? "CALIBRATION OVERDUE.\n" : $"Calibration: {l.State.readingsSinceLastCalibration}/{DoseLedgerSystem.ReadingsPerCalibration} readings.\n");
            for (int i = 0; i < l.Entries.Count; i++)
            {
                var e = l.Entries[i];
                if (e == null) continue;
                int band = DoseLedgerSystem.BandFor(e.cumulativeMsv);
                sb.Append(e.survivorId).Append(": ").Append(e.cumulativeMsv.ToString("F1"))
                  .Append(" mSv [").Append(DoseRegistersCatalogLoader.BandLabel(_session.Registers, band))
                  .Append("]");
                bool flux = false;
                for (int h = 0; h < e.readingsHistory.Count; h++)
                    if (e.readingsHistory[h].fluxAmbiguous) { flux = true; break; }
                if (flux)
                    sb.Append(" §");
                sb.Append('\n');
            }
            return sb.Length == 0 ? "No tagged survivors. The pen is dry until someone seals a dosimeter." : sb.ToString();
        }

        private string RenderSick()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _session.SickList.Bands.Count; i++)
            {
                var b = _session.SickList.Bands[i];
                sb.Append(DoseRegistersCatalogLoader.BandLabel(_session.Registers, b.band))
                  .Append(" — ").Append(b.survivorId);
                if (b.palliativePlan != null && b.palliativePlan.Length > 0)
                    sb.Append(" (plan: ").Append(PlanLabel(b.palliativePlan)).Append(")");
                if (b.releaseDay >= 0)
                    sb.Append(" [released day ").Append(b.releaseDay).Append("]");
                sb.Append('\n');
            }
            return sb.Length == 0
                ? "The bed order is empty. A Red name stays until someone writes it."
                : sb.ToString().TrimEnd();
        }

        private string RenderCohort()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _session.Cohort.Children.Count; i++)
            {
                var c = _session.Cohort.Children[i];
                sb.Append(c.survivorId).Append(" — guess: ").Append(GuessLabel(c.guessBand))
                  .Append(c.baselineCorrected ? " (corrected: " + c.trueBand + ")" : " (uncorrected)")
                  .Append('\n');
            }
            return sb.Length == 0
                ? "The chalk board is blank. A guess is written in pencil, and the board can be erased."
                : sb.ToString().TrimEnd();
        }

        private string PlanLabel(string planId)
        {
            for (int i = 0; i < _session.Registers.plans.Count; i++)
                if (_session.Registers.plans[i].id == planId)
                    return _session.Registers.plans[i].label;
            return planId;
        }

        private static string GuessLabel(string guess)
        {
            switch (guess)
            {
                case "low": return "Low";
                case "medium": return "Honest";
                case "high": return "High";
                default: return guess;
            }
        }

        private string RenderVoluntary()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _session.Voluntary.Entries.Count; i++)
            {
                var e = _session.Voluntary.Entries[i];
                sb.Append(e.survivorId).Append(" — ").Append(e.task)
                  .Append(e.completed ? " [done, banked " + e.doseIncurred.ToString("F1") + " mSv]" : " [open]")
                  .Append('\n');
            }
            return sb.Length == 0
                ? "The signature list is empty. Signing an hour spends ink; the dose lands back on the ledger the moment it completes."
                : sb.ToString().TrimEnd();
        }

        // ── One-button actions (diegetic: Book / Name / Assign / Sign) ──

        private void OnBookReading()
        {
            if (_session == null) return;
            _session.SealDemoSurvivors();
            var text = _session.ScribeReading(120f, highEnergy: true);
            ShowStatus(text);
        }

        private void OnNameToSick()
        {
            if (_session == null) return;
            _session.SealDemoSurvivors();
            ShowStatus(_session.DiagnoseDemo(DoseLedgerSystem.BandRed));
        }

        private void OnAssignMorphine()
        {
            if (_session == null) return;
            bool ok = _session.SickList.AssignPalliative("survivor_gunner_mikhail", "plan_morphine_tray");
            ShowStatus(ok ? "Morphine tray assigned to the veteran." : "No such name on the sick list.");
        }

        private void OnBookChild()
        {
            if (_session == null) return;
            ShowStatus(_session.BookDemoChild());
        }

        private void OnCorrectBaseline()
        {
            if (_session == null) return;
            bool ok = _session.Cohort.CorrectBaseline("sv_cohort_demo", "high");
            ShowStatus(ok ? "Baseline corrected to high." : "No such child on the board.");
        }

        private void OnSignVolunteer()
        {
            if (_session == null) return;
            bool ok = _session.Voluntary.Volunteer("survivor_gunner_mikhail", "brine line inspection", 46, "Someone has to walk it.");
            ShowStatus(ok ? "Veteran signed an hour." : "Already signed.");
        }

        private void OnCompleteVolunteer()
        {
            if (_session == null) return;
            bool ok = _session.Voluntary.CompleteVolunteer("survivor_gunner_mikhail", "brine line inspection", 60f, 47);
            ShowStatus(ok ? "Hour marked done; 60 mSv banked on the ledger." : "No open task to complete.");
        }

        private void OnCalibrate()
        {
            if (_session == null) return;
            _session.Ledger.Calibrate("survivor_gunner_mikhail", 47);
            ShowStatus("Calibrated. The drift resets; the drift is normal. — Piet");
        }

        private void ShowStatus(string text)
        {
            GD.Print("[DoseRegister] " + text);
            RefreshView();
        }
    }
}
