using System;
using System.Linq;
using Godot;
using Ashfall.Core.Medical;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Medical panel.
    /// Shows survivor health, dosimetry, respiratory affliction state, and chemical
    /// dependency ledger. Treatment buttons consume real inventory items and call
    /// authoritative Core/host APIs.  Thin presentation layer only — no medical rules here.
    /// </summary>
    public partial class MedicalPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnTreatmentAdministered;

        private VBoxContainer _healthStats = null!;
        private VBoxContainer _treatmentList = null!;
        private VBoxContainer _supplyList = null!;

        // Dashboard shell + reusable chrome. Owned by this panel; bound to
        // real Core state in RefreshView.
        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private AshfallSidebar? _sidebar;

        private MedicalHostSession? _medicalHost;
        private SurvivorsHostSession? _survivorsHost;
        private InventoryHostSession? _inventoryHost;
        private RespiratoryDegenerationSystem? _respiratory;

        public bool IsBound => _medicalHost != null;
        public int RenderedHealthCount => _healthStats?.GetChildCount() ?? 0;

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

        private static string RespiratoryLabel(float degradation, bool permanent)
        {
            if (degradation <= 0f) return "CLEAR";
            if (degradation < RespiratoryDegenerationSystem.SevereCoughThreshold)
                return "MILD COUGH";
            if (degradation < RespiratoryDegenerationSystem.IrreversibleThreshold)
                return $"SEVERE COUGH  [STAMINA -{RespiratoryDegenerationSystem.SevereCoughStaminaPenalty * 100:F0}%]";
            if (degradation < RespiratoryDegenerationSystem.TerminalLungThreshold)
                return permanent ? "PERMANENT LUNG DAMAGE  [INHALER REQUIRED]" : "CRITICAL — INHALER REQUIRED";
            return "TERMINAL LUNG DAMAGE";
        }

        public void Bind(
            MedicalHostSession medical,
            SurvivorsHostSession? survivors = null,
            InventoryHostSession? inventory = null,
            RespiratoryDegenerationSystem? respiratory = null)
        {
            _medicalHost = medical;
            _survivorsHost = survivors;
            _inventoryHost = inventory;

            // Unsubscribe before re-subscribing to avoid duplicate events if Bind is called again
            if (_respiratory != null)
                _respiratory.OnStateChanged -= OnRespiratoryStateChanged;
            _respiratory = respiratory;
            if (_respiratory != null)
                _respiratory.OnStateChanged += OnRespiratoryStateChanged;

            RefreshView();
        }

        private void OnRespiratoryStateChanged() => RefreshView();

        public void RefreshView()
        {
            if (_healthStats == null || _treatmentList == null || _supplyList == null) return;

            RefreshStatusRail();

            ClearChildren(_healthStats);
            ClearChildren(_treatmentList);
            ClearChildren(_supplyList);

            if (_medicalHost == null)
            {
                _healthStats.AddChild(AshfallUiHelpers.MakeMetadata("No medical session bound."));
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata("No treatment ledger available."));
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata("No inventory session bound."));
                return;
            }

            // ── Survivor health, dosimetry, and affliction rows ────────
            if (_survivorsHost == null || _survivorsHost.RosterState.Count == 0)
            {
                _healthStats.AddChild(AshfallUiHelpers.MakeMetadata("No survivor health readout bound."));
            }
            else
            {
                var slices = _survivorsHost.CaptureSave().survivors
                    .Where(s => s != null)
                    .ToDictionary(s => s.id, StringComparer.Ordinal);

                int bandageCount  = CountItem("bandage", "item_bandage");
                int iodineCount   = CountItem("iodine_pills", "item_potassium_iodide");
                int radAwayCount  = CountItem("rad_away", "item_rad_away");
                int inhalerCount  = CountItem("inhaler");
                int herbalTeaCount = CountItem("herbal_tea");

                foreach (var survivor in _survivorsHost.RosterState)
                {
                    if (survivor == null) continue;
                    slices.TryGetValue(survivor.Id, out var slice);
                    float currentDose = slice?.radiationDose ?? 0f;
                    bool hasResistance = slice?.hasRadResistance ?? false;

                    float respDeg = _respiratory?.RespiratoryDegradation(survivor.Id) ?? 0f;
                    bool permanent = _respiratory?.HasPermanentLungDamage(survivor.Id) ?? false;
                    bool needsInhaler = _respiratory?.RequiresInhaler(survivor.Id) ?? false;
                    float reliefHours = _respiratory?.InhalerReliefHours(survivor.Id) ?? 0f;

                    var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                    card.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                    // ── Vital row ──────────────────────────────────────
                    var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    row.AddChild(AshfallUiHelpers.MakeBadgeIcon(
                        currentDose >= 50f ? "badge_rad_sickness" : "badge_exhaustion", 22));

                    var name = AshfallUiHelpers.MakeSmall(FormatSurvivorName(survivor.Id));
                    name.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    row.AddChild(name);

                    var hp = AshfallUiHelpers.MakeMono($"HP {survivor.Health:0}/{survivor.MaxHealthCap:0}");
                    hp.AddThemeColorOverride("font_color",
                        AshfallUiHelpers.ToColor(survivor.Health < 30
                            ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm));
                    row.AddChild(hp);

                    var dose = AshfallUiHelpers.MakeMono(
                        $"RAD {currentDose:0} mSv{(hasResistance ? " [⚡RESIST]" : "")}");
                    dose.AddThemeColorOverride("font_color",
                        AshfallUiHelpers.ToColor(currentDose >= 50f
                            ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                    row.AddChild(dose);

                    row.AddChild(AshfallUiHelpers.MakeMono($"HUN {survivor.Hunger:0}"));
                    row.AddChild(AshfallUiHelpers.MakeMono($"THI {survivor.Thirst:0}"));
                    card.AddChild(row);

                    // ── Treatment action row ───────────────────────────
                    string targetId = survivor.Id;
                    var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                    var btnHeal = AshfallUiHelpers.MakeButton(
                        $"BANDAGE (+25 HP) [{bandageCount}]", () =>
                        {
                            if (_inventoryHost != null &&
                                (_inventoryHost.Inventory.RemoveById("bandage", 1) ||
                                 _inventoryHost.Inventory.RemoveById("item_bandage", 1)))
                            {
                                _survivorsHost.HealSurvivor(targetId, 25f);
                                _medicalHost.AddCareEntry(targetId, "Applied sterile bandage.");
                                OnTreatmentAdministered?.Invoke();
                                RefreshView();
                            }
                        });
                    btnHeal.Disabled = bandageCount <= 0 || survivor.Health >= survivor.MaxHealthCap;
                    btnHeal.CustomMinimumSize = new Vector2(160, 28);
                    actionRow.AddChild(btnHeal);

                    var btnIodine = AshfallUiHelpers.MakeButton(
                        $"IODINE (+RESIST) [{iodineCount}]", () =>
                        {
                            if (_inventoryHost != null &&
                                (_inventoryHost.Inventory.RemoveById("iodine_pills", 1) ||
                                 _inventoryHost.Inventory.RemoveById("item_potassium_iodide", 1)))
                            {
                                _survivorsHost.AdministerIodine(targetId);
                                _medicalHost.AddCareEntry(targetId, "Administered Potassium Iodide.");
                                OnTreatmentAdministered?.Invoke();
                                RefreshView();
                            }
                        });
                    btnIodine.Disabled = iodineCount <= 0;
                    btnIodine.CustomMinimumSize = new Vector2(160, 28);
                    actionRow.AddChild(btnIodine);

                    var btnRadAway = AshfallUiHelpers.MakeButton(
                        $"ANTI-RAD (−40 mSv) [{radAwayCount}]", () =>
                        {
                            if (_inventoryHost != null &&
                                (_inventoryHost.Inventory.RemoveById("rad_away", 1) ||
                                 _inventoryHost.Inventory.RemoveById("item_rad_away", 1)))
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

                    // ── Respiratory affliction row (only when system is bound) ──
                    if (_respiratory != null)
                    {
                        var respRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                        respRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_exhaustion", 18));

                        string respLabel = RespiratoryLabel(respDeg, permanent);
                        var respText = AshfallUiHelpers.MakeSmall($"LUNG: {respLabel}  ({respDeg:F0}%)");
                        respText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                        bool respCritical = respDeg >= RespiratoryDegenerationSystem.SevereCoughThreshold;
                        respText.AddThemeColorOverride("font_color",
                            AshfallUiHelpers.ToColor(respCritical
                                ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                        respRow.AddChild(respText);

                        if (reliefHours > 0f)
                        {
                            var relief = AshfallUiHelpers.MakeMono($"[INHALER ACTIVE {reliefHours:F0}h]");
                            relief.AddThemeColorOverride("font_color",
                                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                            respRow.AddChild(relief);
                        }
                        card.AddChild(respRow);

                        // Inhaler treatment action row
                        var inhalerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                        bool canApplyInhaler = inhalerCount > 0 && respDeg > 0f;
                        string inhalerBtnText = canApplyInhaler
                            ? $"APPLY INHALER (−{RespiratoryDegenerationSystem.InhalerDegradationReduction:F0}% lung) [{inhalerCount}]"
                            : $"APPLY INHALER [{inhalerCount}]";
                        string inhalerReason = !canApplyInhaler
                            ? (inhalerCount <= 0 ? "No inhaler in inventory — craft recipe_inhaler" : "No respiratory damage")
                            : string.Empty;

                        string respTargetId = survivor.Id;
                        var btnInhaler = AshfallUiHelpers.MakeButton(inhalerBtnText, () =>
                        {
                            if (_inventoryHost != null &&
                                _inventoryHost.Inventory.RemoveById("inhaler", 1))
                            {
                                _respiratory.ApplyInhaler(respTargetId);
                                _medicalHost.AddCareEntry(respTargetId, "Applied improvised inhaler.");
                                OnTreatmentAdministered?.Invoke();
                                RefreshView();
                            }
                        });
                        btnInhaler.Disabled = !canApplyInhaler;
                        btnInhaler.CustomMinimumSize = new Vector2(240, 28);
                        inhalerRow.AddChild(btnInhaler);

                        if (!string.IsNullOrEmpty(inhalerReason))
                        {
                            var reasonLabel = AshfallUiHelpers.MakeMetadata(inhalerReason);
                            reasonLabel.AddThemeColorOverride("font_color",
                                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                            inhalerRow.AddChild(reasonLabel);
                        }

                        // Herbal tea treatment (mild relief, no station required)
                        if (herbalTeaCount > 0 && respDeg > 0f)
                        {
                            string teaTargetId = survivor.Id;
                            var btnTea = AshfallUiHelpers.MakeButton(
                                $"HERBAL TEA (−{RespiratoryDegenerationSystem.HerbalTeaDegradationReduction:F0}%) [{herbalTeaCount}]",
                                () =>
                                {
                                    if (_inventoryHost != null &&
                                        _inventoryHost.Inventory.RemoveById("herbal_tea", 1))
                                    {
                                        _respiratory.ApplyHerbalTea(teaTargetId);
                                        _medicalHost.AddCareEntry(teaTargetId, "Administered herbal tea.");
                                        OnTreatmentAdministered?.Invoke();
                                        RefreshView();
                                    }
                                });
                            btnTea.CustomMinimumSize = new Vector2(200, 28);
                            inhalerRow.AddChild(btnTea);
                        }

                        card.AddChild(inhalerRow);
                    }

                    var panelWrap = AshfallUiHelpers.MakePanel();
                    panelWrap.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    panelWrap.AddChild(card);
                    _healthStats.AddChild(panelWrap);
                }
            }

            // ── Chemical dependency ledger ─────────────────────────────
            int dependencyCount = 0;
            foreach (var entry in _medicalHost.Engine.Ledger)
            {
                foreach (var dependency in entry.Value)
                {
                    dependencyCount++;
                    string mode = dependency.inManagedDetox
                        ? "Managed Detox"
                        : dependency.inColdTurkey ? "Cold Turkey" : "Active Use";

                    var depRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    depRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_chemical_dependency", 22));
                    var depText = AshfallUiHelpers.MakeSmall(
                        $"{entry.Key} // {dependency.itemId} · Level {dependency.dependencyLevel:P0} · [{mode}]");
                    depText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    depRow.AddChild(depText);
                    _treatmentList.AddChild(depRow);
                }
            }
            if (dependencyCount == 0)
                _treatmentList.AddChild(
                    AshfallUiHelpers.MakeMetadata("No active chemical dependencies or withdrawal ledgers."));

            _treatmentList.AddChild(AshfallUiHelpers.MakeDataRow(
                "Active Cohort Penalties",
                $"Crafting {_medicalHost.ActiveCraftingPenalty:P0} · Combat {_medicalHost.ActiveCombatPenalty:P0}",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));

            _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(_medicalHost.VigilStatusLine()));

            // ── Medical supplies on hand ───────────────────────────────
            if (_inventoryHost == null)
            {
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata("Inventory session not bound."));
            }
            else
            {
                foreach (string itemId in new[]
                    { "iodine_pills", "rad_away", "bandage", "inhaler", "herbal_tea",
                      "item_potassium_iodide", "item_blight_treatment" })
                {
                    int count = _inventoryHost.Inventory.CountById(itemId);
                    if (count <= 0 && itemId.StartsWith("item_")) continue; // hide zero-count legacy aliases
                    var supplyRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    supplyRow.AddChild(AshfallUiHelpers.MakeItemIcon(itemId, 22));
                    var supplyName = AshfallUiHelpers.MakeSmall(
                        itemId.Replace('_', ' ').ToUpperInvariant());
                    supplyName.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    supplyRow.AddChild(supplyName);
                    var supplyCount = AshfallUiHelpers.MakeMono($"{count} on hand");
                    supplyCount.AddThemeColorOverride("font_color",
                        AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                    supplyRow.AddChild(supplyCount);
                    _supplyList.AddChild(supplyRow);
                }
            }

            if (!string.IsNullOrWhiteSpace(_medicalHost.LastEvent))
                _supplyList.AddChild(
                    AshfallUiHelpers.MakeMetadata($"Last medical event: {_medicalHost.LastEvent}"));
        }

        private int CountItem(string primaryId, string fallbackId = null!)
        {
            if (_inventoryHost == null) return 0;
            int count = _inventoryHost.Inventory.CountById(primaryId);
            if (count == 0 && fallbackId != null)
                count = _inventoryHost.Inventory.CountById(fallbackId);
            return count;
        }

        /// <summary>
        /// Populates the top status rail from Core state. Posts cohort, avg HP,
        /// max dose, active treatment count, and vigil/resting breaks into the
        /// five metric chips. Bound to no-host + no-survivors fallback values
        /// so the rail is always inspectable.
        /// </summary>
        private void RefreshStatusRail()
        {
            if (_statusRail == null) return;

            int cohort = 0, living = 0;
            float hpTotal = 0f;
            float hpMaxTotal = 1f;
            float maxDose = 0f;
            if (_survivorsHost != null)
            {
                foreach (var s in _survivorsHost.RosterState)
                {
                    if (s == null) continue;
                    cohort++;
                    if (s.IsAliveState)
                    {
                        living++;
                        hpTotal += Math.Max(0, s.Health);
                        hpMaxTotal += Math.Max(1, s.MaxHealthCap);
                    }
                    float dose = s.Health == 0 ? 0f : (s.Health > 0 ? 1f : 0f);
                    // Per-survivor dose is read from the survivors' save slice.
                }
                if (_survivorsHost.CaptureSave()?.survivors != null)
                {
                    foreach (var slice in _survivorsHost.CaptureSave().survivors)
                    {
                        if (slice == null) continue;
                        if (slice.radiationDose > maxDose) maxDose = slice.radiationDose;
                    }
                }
            }

            float avgHp = cohort > 0 && hpMaxTotal > 0 ? (hpTotal / hpMaxTotal) * 100f : 0f;
            int activeTx = 0;
            if (_medicalHost != null)
            {
                foreach (var entry in _medicalHost.Engine.Ledger)
                    if (entry.Value != null) activeTx += entry.Value.Count;
            }

            AshfallMetricCard.Criticality cohortCrit =
                cohort == 0 ? AshfallMetricCard.Criticality.Normal
                : living == cohort ? AshfallMetricCard.Criticality.Normal
                : living >= (cohort * 0.75f) ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Warn;

            AshfallMetricCard.Criticality hpCrit =
                avgHp >= 75 ? AshfallMetricCard.Criticality.Normal
                : avgHp >= 50 ? AshfallMetricCard.Criticality.Caution
                : avgHp > 0 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            AshfallMetricCard.Criticality doseCrit =
                maxDose < 25 ? AshfallMetricCard.Criticality.Normal
                : maxDose < 50 ? AshfallMetricCard.Criticality.Caution
                : maxDose < 100 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            _statusRail.Set("cohort",   $"{living}/{cohort}",  cohortCrit);
            _statusRail.Set("avgHp",    $"{avgHp:0}%",          hpCrit);
            _statusRail.Set("doseMax",  $"{maxDose:0} mSv",     doseCrit);
            _statusRail.Set("activeTx", $"{activeTx}",          AshfallMetricCard.Criticality.Normal);

            // Vigil: state from the medical engine (the underlying wording is
            // kept verbatim so the host doesn't lose semantics).
            string vigilState = "STANDBY";
            AshfallMetricCard.Criticality vigilCrit = AshfallMetricCard.Criticality.Caution;
            if (_medicalHost != null)
            {
                var line = _medicalHost.VigilStatusLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string upper = line.ToUpperInvariant();
                    vigilState = upper.Length > 18 ? upper.Substring(0, 18) : upper;
                    if (upper.Contains("CRITICAL") || upper.Contains("EMERGENCY"))
                        vigilCrit = AshfallMetricCard.Criticality.Critical;
                    else if (upper.Contains("WARN") || upper.Contains("DAMAGE"))
                        vigilCrit = AshfallMetricCard.Criticality.Warn;
                    else if (upper.Contains("CLEAR") || upper.Contains("HOLDING"))
                        vigilCrit = AshfallMetricCard.Criticality.Normal;
                }
            }
            _statusRail.Set("vigil", vigilState, vigilCrit);
        }

        private static void ClearChildren(Node parent)
        {
            AshfallUiHelpers.EmptyChildren(parent);
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

            // Dashboard shell — sidebar provides nav between sub-sections;
            // status rail holds the medical vitals that the Stitch reference
            // puts in its MEDICAL TRIAGE header row.
            _shell = new AshfallDashboardShell(
                "MEDICAL TRIAGE & DEPENDENCY", 880, 600);
            center.AddChild(_shell);
            _sidebar = _shell.SetSidebar(new[]
            {
                new AshfallSidebar.Item { Id = "health",     Label = "Health",          Hint = "DOSIMETRY + RESP" },
                new AshfallSidebar.Item { Id = "treatments", Label = "Treatments",      Hint = "DETOX LEDGER" },
                new AshfallSidebar.Item { Id = "supplies",   Label = "Supplies",        Hint = "MEDICAL STORES" },
            }, "MEDICAL OPS", "health");

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("cohort",    "COHORT",        "—", AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("avgHp",     "AVG HP",        "—%", AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("doseMax",   "MAX DOSE",      "0 mSv", AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("activeTx",  "ACTIVE TX",     "0", AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("vigil",     "VIGIL",         "STANDBY", AshfallMetricCard.Criticality.Caution, 140);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

            // Content slot — scroll container with three named sub-sections.
            var scrollRoot = new ScrollContainer();
            scrollRoot.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollRoot.SizeFlagsVertical = SizeFlags.ExpandFill;
            var scrollMargin = new MarginContainer();
            scrollMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            scrollRoot.AddChild(scrollMargin);
            _shell.SetContent(new MarginContainer()); // placeholder; replaced below
            _shell.SetContent(scrollRoot);

            var contentBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollMargin.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVIVOR HEALTH, DOSIMETRY & RESPIRATORY"));
            _healthStats = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            _healthStats.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentBox.AddChild(_healthStats);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENT & DETOXIFICATION LEDGER"));
            _treatmentList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_treatmentList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("MEDICAL SUPPLIES ON HAND"));
            _supplyList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_supplyList);

            if (_sidebar != null)
            {
                _sidebar.OnSelected += id =>
                {
                    // Anchor each sub-section into view via scroll-to-offset.
                    if (id == "health" && _healthStats != null)
                        ScrollToChild(scrollRoot, _healthStats);
                    else if (id == "treatments" && _treatmentList != null)
                        ScrollToChild(scrollRoot, _treatmentList);
                    else if (id == "supplies" && _supplyList != null)
                        ScrollToChild(scrollRoot, _supplyList);
                };
            }

            RefreshView();
        }

        private static void ScrollToChild(ScrollContainer scroll, Control child)
        {
            if (scroll == null || child == null) return;
            // Best-effort: walk the control ancestors summing Position.Y until
            // we hit the scroll container.
            try
            {
                float targetOffset = 0f;
                Node walker = child;
                while (walker != null && walker != scroll)
                {
                    if (walker is Control w && walker != scroll)
                        targetOffset += w.Position.Y;
                    walker = walker.GetParent();
                }
                if (targetOffset > 0)
                {
                    scroll.ScrollVertical = (int)Math.Max(0, targetOffset - 8);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                // ignore — scroll happens best-effort
            }
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
