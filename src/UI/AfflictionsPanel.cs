using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Medical;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Afflictions panel.
    /// Shows current afflictions, chronic conditions, and available treatments,
    /// bound to the live Medical / Survivors / Respiratory / Inventory sessions.
    /// Unbound systems render an honest "not monitored" row instead of
    /// fabricated affliction strings.
    /// </summary>
    public partial class AfflictionsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblActiveTitle;
        private VBoxContainer _activeList;
        private Label _lblChronicTitle;
        private VBoxContainer _chronicList;
        private Label _lblTreatmentTitle;
        private VBoxContainer _treatmentList;

        private MedicalHostSession? _medical;
        private SurvivorsHostSession? _survivors;
        private InventoryHostSession? _inventory;
        private RespiratoryDegenerationSystem? _respiratory;

        public bool IsBound => _medical != null || _survivors != null;
        public int RenderedActiveCount { get; private set; }

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
            RefreshView();
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

            // Chemical dependencies (chronic substance conditions)
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

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("AFFLICTIONS & TREATMENTS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Active afflictions section
            _lblActiveTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE AFFLICTIONS");
            vbox.AddChild(_lblActiveTitle);

            _activeList = new VBoxContainer();
            _activeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _activeList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_activeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Chronic conditions section
            _lblChronicTitle = AshfallUiHelpers.MakeSectionHeader("CHRONIC CONDITIONS");
            vbox.AddChild(_lblChronicTitle);

            _chronicList = new VBoxContainer();
            _chronicList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _chronicList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_chronicList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Treatments section
            _lblTreatmentTitle = AshfallUiHelpers.MakeSectionHeader("TREATMENTS & MEDICATIONS");
            vbox.AddChild(_lblTreatmentTitle);

            _treatmentList = new VBoxContainer();
            _treatmentList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _treatmentList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_treatmentList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
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
