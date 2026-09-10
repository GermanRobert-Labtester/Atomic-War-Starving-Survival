// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 190–193: amputation triage workflow.
    /// Presentation-only — surgical commands are emitted via
    /// <see cref="OnActionRequested"/> and resolved by the host through the
    /// bound <see cref="AmputationSystem"/> and the canonical inventory
    /// authority. Every consequence text is honest about shock, bleeding,
    /// recovery and phantom-pain costs.
    /// </summary>
    public partial class AmputationTriagePanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private AmputationSystem? _system;
        private ItemList _patientList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedPatientIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(AmputationSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("TRIAGE TABLE // AMPUTATION & PROSTHETICS", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("patients", "Under Care", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("gangrenous", "Gangrenous", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("amputated", "Amputated", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("prosthetics", "Prosthetics", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);

            _patientList = new ItemList
            {
                CustomMinimumSize = new Vector2(280, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _patientList.ItemSelected += index => { _selectedPatientIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_patientList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private List<string> PatientIds()
        {
            var ids = new List<string>();
            if (_system == null) return ids;
            foreach (var kv in _system.State.survivorLimbs)
            {
                bool needsCare = kv.Value != null && kv.Value.Any(l => l != null &&
                    (l.condition == LimbCondition.Wounded || l.condition == LimbCondition.Infected
                     || l.condition == LimbCondition.Gangrenous || l.condition == LimbCondition.Amputated));
                if (needsCare) ids.Add(kv.Key);
            }
            ids.Sort(StringComparer.Ordinal);
            return ids;
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _patientList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var patients = PatientIds();
            _selectedPatientIndex = Math.Clamp(_selectedPatientIndex, -1, Math.Max(0, patients.Count - 1));

            int gangrenous = 0, amputated = 0, prosthetics = 0;
            foreach (var kv in _system.State.survivorLimbs)
            {
                if (kv.Value == null) continue;
                foreach (var l in kv.Value)
                {
                    if (l == null) continue;
                    if (l.condition == LimbCondition.Gangrenous) gangrenous++;
                    if (l.condition == LimbCondition.Amputated) amputated++;
                    if (l.condition == LimbCondition.Prosthetic || l.condition == LimbCondition.Bionic) prosthetics++;
                }
            }

            if (_statusRail != null)
            {
                _statusRail.Set("patients", patients.Count.ToString(), patients.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("gangrenous", gangrenous.ToString(), gangrenous > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("amputated", amputated.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("prosthetics", prosthetics.ToString(), AshfallMetricCard.Criticality.Normal);
            }

            _patientList.Clear();
            for (int i = 0; i < patients.Count; i++)
            {
                string worst = WorstCondition(patients[i]);
                _patientList.AddItem($"{ItemDisplay.Prettify(patients[i])} — {worst}", null, false);
                if (i == _selectedPatientIndex)
                    _patientList.Select(i);
            }

            var selected = _selectedPatientIndex >= 0 && _selectedPatientIndex < patients.Count
                ? patients[_selectedPatientIndex] : null;

            if (selected == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No one is under triage care. Wounded limbs that turn gangrenous come to this table — the alternative is worse.",
                    title: "NO PATIENTS"));
                return;
            }

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader($"PATIENT: {ItemDisplay.Prettify(selected).ToUpperInvariant()}"));

            if (_system.State.survivorLimbs.TryGetValue(selected, out var limbs) && limbs != null)
            {
                foreach (var limb in limbs)
                {
                    if (limb == null) continue;
                    string condition = LimbConditionText(limb.condition);
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow(LimbText(limb.limb), condition,
                        limb.condition == LimbCondition.Gangrenous ? AshfallUiHelpers.ColorCritical :
                        limb.condition == LimbCondition.Infected || limb.condition == LimbCondition.Wounded ? AshfallUiHelpers.ColorWarning :
                        limb.condition == LimbCondition.Prosthetic || limb.condition == LimbCondition.Bionic ? AshfallUiHelpers.ColorInfo :
                        AshfallUiHelpers.ColorDim));
                    if (limb.hasPhantomPain)
                        _detail.AddChild(AshfallUiHelpers.MakeMetadata("  Phantom pain episodes reported."));
                    if (limb.recoveryDaysLeft > 0)
                        _detail.AddChild(AshfallUiHelpers.MakeDataRow("  Recovery", $"{limb.recoveryDaysLeft} days left", AshfallUiHelpers.ColorDim));
                }
            }

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            var limbsForActions = _system.State.survivorLimbs.TryGetValue(selected, out var ls) ? ls : null;
            bool acted = false;

            if (limbsForActions != null)
            {
                foreach (var limb in limbsForActions)
                {
                    if (limb == null) continue;

                    if (limb.condition == LimbCondition.Gangrenous || limb.condition == LimbCondition.Infected)
                    {
                        var ampBtn = AshfallUiHelpers.MakeButton($"AMPUTATE {LimbText(limb.limb).ToUpperInvariant()}", () =>
                            OnActionRequested?.Invoke("amputate", $"{selected}:{(int)limb.limb}"));
                        ampBtn.TooltipText = "Major surgery: shock risk, bleeding, long recovery, possible phantom pain. Gangrene leaves no better option.";
                        row.AddChild(ampBtn);
                        acted = true;
                    }
                    else if (limb.condition == LimbCondition.Wounded)
                    {
                        var treatBtn = AshfallUiHelpers.MakeButton($"CLEAN & DRESS {LimbText(limb.limb).ToUpperInvariant()}", () =>
                            OnActionRequested?.Invoke("treat", $"{selected}:{(int)limb.limb}"));
                        treatBtn.TooltipText = "Wound cleaning reduces infection. Deep wounds may still turn.";
                        row.AddChild(treatBtn);
                        acted = true;
                    }
                    else if (limb.condition == LimbCondition.Amputated)
                    {
                        var prosBtn = AshfallUiHelpers.MakeButton($"FIT PROSTHETIC — {LimbText(limb.limb).ToUpperInvariant()}", () =>
                            OnActionRequested?.Invoke("prosthetic", $"{selected}:{(int)limb.limb}"));
                        prosBtn.TooltipText = "Fits a prosthetic limb from storage, restoring some function.";
                        row.AddChild(prosBtn);
                        acted = true;
                    }
                }
            }

            if (!acted)
                row.AddChild(AshfallUiHelpers.MakeMetadata("Nothing surgical is indicated for this patient right now."));
            _detail.AddChild(row);
        }

        private string WorstCondition(string survivorId)
        {
            if (_system != null && _system.State.survivorLimbs.TryGetValue(survivorId, out var limbs) && limbs != null)
            {
                if (limbs.Any(l => l != null && l.condition == LimbCondition.Gangrenous)) return "GANGRENE";
                if (limbs.Any(l => l != null && l.condition == LimbCondition.Infected)) return "INFECTED";
                if (limbs.Any(l => l != null && l.condition == LimbCondition.Wounded)) return "WOUNDED";
                if (limbs.Any(l => l != null && l.condition == LimbCondition.Amputated)) return "AMPUTEE";
            }
            return "STABLE";
        }

        private static string LimbText(LimbId limb) => limb switch
        {
            LimbId.LeftArm => "Left arm",
            LimbId.RightArm => "Right arm",
            LimbId.LeftLeg => "Left leg",
            LimbId.RightLeg => "Right leg",
            _ => limb.ToString()
        };

        private static string LimbConditionText(LimbCondition c) => c switch
        {
            LimbCondition.Intact => "Intact",
            LimbCondition.Wounded => "Wounded",
            LimbCondition.Infected => "Infected",
            LimbCondition.Gangrenous => "GANGRENOUS",
            LimbCondition.Amputated => "Amputated",
            LimbCondition.Prosthetic => "Prosthetic fitted",
            LimbCondition.Bionic => "Bionic fitted",
            _ => c.ToString()
        };

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
