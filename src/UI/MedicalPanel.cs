using System;
using System.Linq;
using Godot;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Medical panel.
    /// Shows health status, radiation levels, treatments, and medical supplies
    /// with condition badges and supply telemetry.
    /// </summary>
    public partial class MedicalPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _healthStats = null!;
        private VBoxContainer _treatmentList = null!;
        private VBoxContainer _supplyList = null!;

        private MedicalHostSession? _medicalHost;
        private SurvivorsHostSession? _survivorsHost;
        private InventoryHostSession? _inventoryHost;

        public bool IsBound => _medicalHost != null;
        public int RenderedHealthCount => _healthStats?.GetChildCount() ?? 0;

        public event Action? OnTreatmentAdministered;

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
            return id switch
            {
                "survivor_dr_sarah_chen" or "survivor_sarah_chen" => "Dr. Sarah Chen",
                "survivor_gunner_mikhail" or "survivor_mikhail_volkov" => "Gunner Mikhail",
                "elena_vasquez" or "survivor_elena_vasquez" => "Elena Vasquez",
                _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
            };
        }

        public void Bind(
            MedicalHostSession medical,
            SurvivorsHostSession? survivors = null,
            InventoryHostSession? inventory = null)
        {
            _medicalHost = medical;
            _survivorsHost = survivors;
            _inventoryHost = inventory;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_healthStats == null || _treatmentList == null || _supplyList == null) return;

            while (_healthStats.GetChildCount() > 0)
            {
                var child = _healthStats.GetChild(0);
                _healthStats.RemoveChild(child);
                child.QueueFree();
            }
            while (_treatmentList.GetChildCount() > 0)
            {
                var child = _treatmentList.GetChild(0);
                _treatmentList.RemoveChild(child);
                child.QueueFree();
            }
            while (_supplyList.GetChildCount() > 0)
            {
                var child = _supplyList.GetChild(0);
                _supplyList.RemoveChild(child);
                child.QueueFree();
            }

            if (_medicalHost == null)
            {
                _healthStats.AddChild(AshfallUiHelpers.MakeMetadata("No medical session bound."));
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata("No treatment ledger available."));
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata("No inventory session bound."));
                return;
            }

            if (_survivorsHost == null || _survivorsHost.RosterState.Count == 0)
            {
                _healthStats.AddChild(AshfallUiHelpers.MakeMetadata("No survivor health readout bound."));
            }
            else
            {
                var slices = _survivorsHost.CaptureSave().survivors
                    .Where(slice => slice != null)
                    .ToDictionary(slice => slice.id, StringComparer.Ordinal);

                int bandageCount = _inventoryHost?.Inventory.CountById("bandage") ?? 0;
                if (bandageCount == 0 && _inventoryHost != null)
                    bandageCount = _inventoryHost.Inventory.CountById("item_bandage");

                int iodineCount = _inventoryHost?.Inventory.CountById("iodine_pills") ?? 0;
                if (iodineCount == 0 && _inventoryHost != null)
                    iodineCount = _inventoryHost.Inventory.CountById("item_potassium_iodide");

                int radAwayCount = _inventoryHost?.Inventory.CountById("rad_away") ?? 0;
                if (radAwayCount == 0 && _inventoryHost != null)
                    radAwayCount = _inventoryHost.Inventory.CountById("item_rad_away");

                foreach (var survivor in _survivorsHost.RosterState)
                {
                    if (survivor == null) continue;
                    slices.TryGetValue(survivor.Id, out var slice);
                    float currentDose = slice?.radiationDose ?? 0f;
                    bool hasResistance = slice?.hasRadResistance ?? false;

                    var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                    var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    var icon = AshfallUiHelpers.MakeBadgeIcon(currentDose >= 50f ? "badge_rad_sickness" : "badge_exhaustion", 22);
                    row.AddChild(icon);

                    var name = AshfallUiHelpers.MakeSmall(FormatSurvivorName(survivor.Id));
                    name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                    row.AddChild(name);

                    var hp = AshfallUiHelpers.MakeMono($"HP {survivor.Health:0}/{survivor.MaxHealthCap:0}");
                    hp.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(survivor.Health < 30 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm));
                    row.AddChild(hp);

                    var dose = AshfallUiHelpers.MakeMono($"RAD {currentDose:0} mSv{(hasResistance ? " [⚡RESIST]" : "")}");
                    dose.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(currentDose >= 50f ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                    row.AddChild(dose);

                    var hunger = AshfallUiHelpers.MakeMono($"HUN {survivor.Hunger:0}");
                    row.AddChild(hunger);

                    var thirst = AshfallUiHelpers.MakeMono($"THI {survivor.Thirst:0}");
                    row.AddChild(thirst);
                    card.AddChild(row);

                    // Treatment action row
                    var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    string targetId = survivor.Id;

                    var btnHeal = AshfallUiHelpers.MakeButton($"HEAL (+25 HP) [{bandageCount}]", () =>
                    {
                        if (_inventoryHost != null && (_inventoryHost.Inventory.RemoveById("bandage", 1) || _inventoryHost.Inventory.RemoveById("item_bandage", 1)))
                        {
                            _survivorsHost.HealSurvivor(targetId, 25f);
                            _medicalHost.AddCareEntry(targetId, "Applied sterile bandage.");
                            OnTreatmentAdministered?.Invoke();
                            RefreshView();
                        }
                    });
                    btnHeal.Disabled = bandageCount <= 0 || survivor.Health >= survivor.MaxHealthCap;
                    btnHeal.CustomMinimumSize = new Vector2(150, 28);
                    actionRow.AddChild(btnHeal);

                    var btnIodine = AshfallUiHelpers.MakeButton($"IODINE (+RESIST) [{iodineCount}]", () =>
                    {
                        if (_inventoryHost != null && (_inventoryHost.Inventory.RemoveById("iodine_pills", 1) || _inventoryHost.Inventory.RemoveById("item_potassium_iodide", 1)))
                        {
                            _survivorsHost.AdministerIodine(targetId);
                            _medicalHost.AddCareEntry(targetId, "Administered Potassium Iodide.");
                            OnTreatmentAdministered?.Invoke();
                            RefreshView();
                        }
                    });
                    btnIodine.Disabled = iodineCount <= 0;
                    btnIodine.CustomMinimumSize = new Vector2(170, 28);
                    actionRow.AddChild(btnIodine);

                    var btnRadAway = AshfallUiHelpers.MakeButton($"ANTI-RAD (-40 mSv) [{radAwayCount}]", () =>
                    {
                        if (_inventoryHost != null && (_inventoryHost.Inventory.RemoveById("rad_away", 1) || _inventoryHost.Inventory.RemoveById("item_rad_away", 1)))
                        {
                            _survivorsHost.AdministerAntiRad(targetId, 40f);
                            _medicalHost.AddCareEntry(targetId, "Administered anti-rad chelation agent.");
                            OnTreatmentAdministered?.Invoke();
                            RefreshView();
                        }
                    });
                    btnRadAway.Disabled = radAwayCount <= 0 || currentDose <= 0f;
                    btnRadAway.CustomMinimumSize = new Vector2(170, 28);
                    actionRow.AddChild(btnRadAway);

                    card.AddChild(actionRow);

                    var panel = AshfallUiHelpers.MakePanel();
                    panel.AddChild(card);
                    _healthStats.AddChild(panel);
                }
            }

            int dependencyCount = 0;
            foreach (var entry in _medicalHost.Engine.Ledger)
            {
                foreach (var dependency in entry.Value)
                {
                    dependencyCount++;
                    string mode = dependency.inManagedDetox
                        ? "Managed Detox"
                        : dependency.inColdTurkey ? "Cold Turkey" : "Active Use";

                    var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    var badge = AshfallUiHelpers.MakeBadgeIcon("badge_chemical_dependency", 22);
                    row.AddChild(badge);

                    var text = AshfallUiHelpers.MakeSmall($"{entry.Key} // {dependency.itemId} · Level {dependency.dependencyLevel:P0} · [{mode}]");
                    text.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                    row.AddChild(text);

                    _treatmentList.AddChild(row);
                }
            }
            if (dependencyCount == 0)
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata("No active chemical dependencies or withdrawal ledgers."));

            _treatmentList.AddChild(AshfallUiHelpers.MakeDataRow(
                "Active Cohort Penalties",
                $"Crafting {_medicalHost.ActiveCraftingPenalty:P0} · Combat {_medicalHost.ActiveCombatPenalty:P0}",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));

            _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(_medicalHost.VigilStatusLine()));

            if (_inventoryHost == null)
            {
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata("Inventory session not bound."));
            }
            else
            {
                foreach (string itemId in new[] { "iodine_pills", "rad_away", "bandage", "item_potassium_iodide", "item_blight_treatment" })
                {
                    var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    var icon = AshfallUiHelpers.MakeItemIcon(itemId, 22);
                    row.AddChild(icon);

                    var name = AshfallUiHelpers.MakeSmall(itemId.Replace('_', ' ').ToUpperInvariant());
                    name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                    row.AddChild(name);

                    var count = AshfallUiHelpers.MakeMono($"{_inventoryHost.Inventory.CountById(itemId)} on hand");
                    count.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                    row.AddChild(count);

                    _supplyList.AddChild(row);
                }
            }

            if (!string.IsNullOrWhiteSpace(_medicalHost.LastEvent))
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata($"Last medical event: {_medicalHost.LastEvent}"));
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(700, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("MEDICAL TRIAGE & DEPENDENCY", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(660, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVIVOR HEALTH & DOSIMETRY"));
            _healthStats = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_healthStats);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENT & DETOXIFICATION LEDGER"));
            _treatmentList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_treatmentList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("MEDICAL SUPPLIES ON HAND"));
            _supplyList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_supplyList);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
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
