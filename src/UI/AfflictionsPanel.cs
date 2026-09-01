using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Afflictions panel showing current afflictions, chronic
    /// conditions, and available treatments. Bound to the live Medical /
    /// Survivors / Respiratory / Inventory sessions.
    ///
    /// Ticket #125: layout chrome (dialog frame, sections, separators,
    /// close button, hint) is owned by
    /// <c>res://assets/ui/panels/AfflictionsPanel.tscn</c>. This binder
    /// projects presentation data into the dynamic lists (active,
    /// chronic, treatments) and wires the close action.
    /// </summary>
    public partial class AfflictionsPanel : Control
    {
        public event Action? OnClose;

        private SceneBinder? _binder;

        private VBoxContainer _activeList = null!;
        private VBoxContainer _chronicList = null!;
        private VBoxContainer _treatmentList = null!;
        private Button _closeButton = null!;
        public bool IsBound { get; private set; }
        public int RenderedActiveCount { get; private set; }

        private MedicalHostSession? _medical;
        private SurvivorsHostSession? _survivors;
        private InventoryHostSession? _inventory;
        private RespiratoryDegenerationSystem? _respiratory;

        public void Bind(
            MedicalHostSession? medical = null,
            SurvivorsHostSession? survivors = null,
            InventoryHostSession? inventory = null,
            RespiratoryDegenerationSystem? respiratory = null)
        {
            _medical = medical;
            _survivors = survivors;
            _inventory = inventory;
            _respiratory = respiratory;
            IsBound = _medical != null || _survivors != null;
            RefreshView();
        }

        public override void _Ready()
        {
            _binder = new SceneBinder(this, typeof(AfflictionsPanel));
            _binder.Require<VBoxContainer>("ActiveList");
            _binder.Require<VBoxContainer>("ChronicList");
            _binder.Require<VBoxContainer>("TreatmentList");
            _binder.Require<Button>("CloseButton");

            _activeList = _binder.Get<VBoxContainer>("ActiveList");
            _chronicList = _binder.Get<VBoxContainer>("ChronicList");
            _treatmentList = _binder.Get<VBoxContainer>("TreatmentList");
            _closeButton = _binder.Get<Button>("CloseButton");
            _closeButton.Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void RefreshView()
        {
            if (_activeList == null || _chronicList == null || _treatmentList == null) return;

            AshfallUiHelpers.EmptyChildren(_activeList);
            AshfallUiHelpers.EmptyChildren(_chronicList);
            AshfallUiHelpers.EmptyChildren(_treatmentList);

            RenderedActiveCount = 0;
            RenderActive();
            RenderChronic();
            RenderTreatments();
        }

        private void RenderActive()
        {
            if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
            {
                _activeList.AddChild(MakeDimLine("No survivor roster bound."));
                return;
            }

            foreach (var s in _survivors.RosterState)
            {
                if (s == null || !s.IsAlive) continue;
                var rad = _survivors.RadStateFor(s.Id);
                float respDeg = _respiratory?.RespiratoryDegradation(s.Id) ?? 0f;

                if (s.Health < 30f)
                {
                    AddAffliction(_activeList, $"{Name(s.Id)} — Critical health ({s.Health:0}/100)",
                        Ashfall.Core.UI.Theme.Critical);
                    RenderedActiveCount++;
                }
                if (rad is { HasAcuteRadiationSickness: true })
                {
                    AddAffliction(_activeList, $"{Name(s.Id)} — Acute radiation sickness (dose {rad.RadiationDose:0} mSv)",
                        Ashfall.Core.UI.Theme.Critical);
                    RenderedActiveCount++;
                }
                if (respDeg >= RespiratoryDegenerationSystem.SevereCoughThreshold)
                {
                    AddAffliction(_activeList, $"{Name(s.Id)} — Severe respiratory degeneration ({respDeg:0}%)",
                        Ashfall.Core.UI.Theme.Critical);
                    RenderedActiveCount++;
                }
                else if (respDeg > 0f)
                {
                    AddAffliction(_activeList, $"{Name(s.Id)} — Respiratory irritation ({respDeg:0}%)",
                        Ashfall.Core.UI.Theme.Warm);
                    RenderedActiveCount++;
                }

                // Task #133 P1 — disease rows from the pipeline projection.
                // Identities stay masked until an explicit identify confirms
                // them; this panel is read-only (actions live in MedicalPanel).
                // Task #133 P1c — psychology rows (trauma / flashbacks / guilt
                // insomnia) ride the same PatientRecord projection, read-only.
                if (_medical?.Pipeline != null
                    && Ashfall.Core.Survivors.SurvivorId.TryParse(s.Id, out var projectSv))
                {
                    var record = new PatientRecordProjector(_medical.Pipeline).Project(projectSv);
                    foreach (var affliction in record.Afflictions)
                    {
                        bool unidentified = string.Equals(
                            affliction.AfflictionId,
                            MedicalTreatmentCatalog.UnidentifiedIllnessId,
                            StringComparison.Ordinal);
                        bool isDisease = !unidentified
                            && affliction.AfflictionId.StartsWith("disease_", StringComparison.Ordinal);
                        bool isPsychology = IsPsychologyAffliction(affliction.AfflictionId);
                        if (!unidentified && !isDisease && !isPsychology)
                            continue;

                        if (unidentified)
                        {
                            AddAffliction(_activeList,
                                $"{Name(s.Id)} — {affliction.StageLabel} (unidentified)",
                                Ashfall.Core.UI.Theme.Warm);
                        }
                        else if (isPsychology)
                        {
                            // Phase-0 conditions are player-facing; the stage
                            // label carries the state (severity stays with the
                            // Phase-0 panel until a diagnosis flow exists).
                            bool critical = affliction.StageLabel.Contains("CRITICAL", StringComparison.Ordinal);
                            AddAffliction(_activeList,
                                $"{Name(s.Id)} — {affliction.StageLabel}",
                                critical ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm);
                        }
                        else
                        {
                            AddAffliction(_activeList,
                                $"{Name(s.Id)} — {affliction.StageLabel} (day {affliction.SeverityValue:0})",
                                Ashfall.Core.UI.Theme.Critical);
                        }
                        RenderedActiveCount++;
                    }
                }
            }

            if (RenderedActiveCount == 0)
                _activeList.AddChild(MakeDimLine("No active afflictions."));
        }

        private void RenderChronic()
        {
            if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
            {
                _chronicList.AddChild(MakeDimLine("No survivor roster bound."));
                return;
            }

            int chronicCount = 0;
            foreach (var s in _survivors.RosterState)
            {
                if (s == null || !s.IsAlive) continue;
                var rad = _survivors.RadStateFor(s.Id);

                if (rad is { HasChronicIllness: true })
                {
                    AddAffliction(_chronicList, $"{Name(s.Id)} — Chronic radiation illness (lifetime {rad.LifetimeRadiationExposure:0} mSv)",
                        Ashfall.Core.UI.Theme.Entropy);
                    chronicCount++;
                }
                if (_respiratory is { } r && r.HasPermanentLungDamage(s.Id))
                {
                    AddAffliction(_chronicList, $"{Name(s.Id)} — Permanent lung damage", Ashfall.Core.UI.Theme.Entropy);
                    chronicCount++;
                }
            }

            if (_medical?.Engine != null)
            {
                foreach (var kv in _medical.Engine.Ledger)
                {
                    foreach (var dep in kv.Value)
                    {
                        if (dep.dependencyLevel >= ChemicalDependencySystem.DependencyThreshold)
                        {
                            AddAffliction(_chronicList, $"{Name(kv.Key)} — {dep.kind} dependency ({dep.dependencyLevel:P0})",
                                Ashfall.Core.UI.Theme.Entropy);
                            chronicCount++;
                        }
                    }
                }
            }

            if (chronicCount == 0)
                _chronicList.AddChild(MakeDimLine("No chronic conditions."));
        }

        private void RenderTreatments()
        {
            if (_inventory?.Inventory == null)
            {
                _treatmentList.AddChild(MakeDimLine("No inventory session bound."));
                return;
            }

            var rows = new (string label, int count)[]
            {
                ("Bandage (+25 HP)", CountItem("bandage", "item_bandage")),
                ("Iodine Pills (rad resistance)", CountItem("iodine_pills", "item_potassium_iodide")),
                ("Anti-Rad / Chelation (−40 mSv)", CountItem("rad_away", "item_rad_away")),
                ("Inhaler (respiratory relief)", CountItem("inhaler")),
                ("Herbal Tea (respiratory soothe)", CountItem("herbal_tea")),
                ("Antibiotics (infection)", CountItem("antibiotics", "item_antibiotics")),
            };

            bool any = false;
            foreach (var (label, count) in rows)
            {
                if (count <= 0) continue;
                AddAffliction(_treatmentList, $"{label} — {count} in stock", Ashfall.Core.UI.Theme.Warm);
                any = true;
            }

            if (!any)
                _treatmentList.AddChild(MakeDimLine("No treatment supplies in stock."));
        }

        private void AddAffliction(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        /// <summary>Task #133 P1c: the three observe-only Phase-0 psychology projections.</summary>
        private static bool IsPsychologyAffliction(string afflictionId)
        {
            return afflictionId == MedicalTreatmentCatalog.CombatTraumaId
                || afflictionId == MedicalTreatmentCatalog.SomaticFlashbackId
                || afflictionId == MedicalTreatmentCatalog.GuiltInsomniaId;
        }

        private static string Name(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            int us = id.IndexOf('_');
            return us >= 0 ? id.Substring(us + 1).Replace('_', ' ') : id;
        }

        private int CountItem(string primaryId, string fallbackId = null!)
        {
            if (_inventory?.Inventory == null) return 0;
            int count = _inventory.Inventory.CountById(primaryId);
            if (count == 0 && fallbackId != null)
                count = _inventory.Inventory.CountById(fallbackId);
            return count;
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

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
