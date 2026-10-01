// SPDX-License-Identifier: MIT
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
        private MedicalTextCatalog? _medicalTexts;

        public bool IsBound => _medicalHost != null;
        public int RenderedHealthCount => _healthStats?.GetChildCount() ?? 0;

        private static MedicalTextCatalog? LoadDefaultMedicalTexts()
        {
            try
            {
                string dataDir = CatalogPath.ResolveDataDir();
                var fileIo = CatalogPath.CreateFileIOForDataDir(dataDir);
                return MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo);
            }
            catch
            {
                return null;
            }
        }

        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return Tr("ui.medical.unnamed", "[UNNAMED]");
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
            if (degradation <= 0f) return Tr("ui.medical.lung.clear", "CLEAR");
            if (degradation < RespiratoryDegenerationSystem.SevereCoughThreshold)
                return Tr("ui.medical.lung.mild", "MILD COUGH");
            if (degradation < RespiratoryDegenerationSystem.IrreversibleThreshold)
                return TrFormat("ui.medical.severe_cough", $"{RespiratoryDegenerationSystem.SevereCoughStaminaPenalty * 100:F0}");
            if (degradation < RespiratoryDegenerationSystem.TerminalLungThreshold)
                return permanent ? Tr("ui.medical.lung.permanent", "PERMANENT LUNG DAMAGE  [INHALER REQUIRED]") : Tr("ui.medical.lung.critical", "CRITICAL — INHALER REQUIRED");
            return Tr("ui.medical.lung.terminal", "TERMINAL LUNG DAMAGE");
        }

        public void Bind(
            MedicalHostSession medical,
            SurvivorsHostSession? survivors = null,
            InventoryHostSession? inventory = null,
            RespiratoryDegenerationSystem? respiratory = null,
            MedicalTextCatalog? medicalTexts = null)
        {
            _medicalHost = medical;
            _survivorsHost = survivors;
            _inventoryHost = inventory;
            if (medicalTexts != null)
                _medicalTexts = medicalTexts;
            else if (_medicalTexts == null)
                _medicalTexts = LoadDefaultMedicalTexts();

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

            AshfallUiHelpers.EmptyChildren(_healthStats);
            AshfallUiHelpers.EmptyChildren(_treatmentList);
            AshfallUiHelpers.EmptyChildren(_supplyList);

            if (_medicalHost == null)
            {
                _healthStats.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.no_session", "No medical session bound.")));
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.no_treatment_ledger", "No treatment ledger available.")));
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.no_inventory", "No inventory session bound.")));
                return;
            }

            // ── Survivor health, dosimetry, and affliction rows ────────
            if (_survivorsHost == null || _survivorsHost.RosterState.Count == 0)
            {
                _healthStats.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.no_health_readout", "No survivor health readout bound.")));
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
                        currentDose >= Ashfall.Core.Radiation.RadiationSystem.WarnThreshold ? "badge_rad_sickness" : "badge_exhaustion", 22));

                    var name = AshfallUiHelpers.MakeSmall(FormatSurvivorName(survivor.Id));
                    name.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    row.AddChild(name);

                    var hp = AshfallUiHelpers.MakeMono(TrFormat("ui.medical.hp", $"{survivor.Health:0}", $"{survivor.MaxHealthCap:0}"));
                    hp.AddThemeColorOverride("font_color",
                        AshfallUiHelpers.ToColor(survivor.Health < Ashfall.Core.Survivors.NeedsProfile.DefaultHealthWarn
                            ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm));
                    row.AddChild(hp);

                    var dose = AshfallUiHelpers.MakeMono(
                        TrFormat("ui.medical.rad", $"{currentDose:0}", hasResistance ? " " + Tr("ui.medical.resist_suffix", "[⚡RESIST]") : ""));
                    dose.AddThemeColorOverride("font_color",
                        AshfallUiHelpers.ToColor(currentDose >= Ashfall.Core.Radiation.RadiationSystem.WarnThreshold
                            ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                    row.AddChild(dose);

                    row.AddChild(AshfallUiHelpers.MakeMono(TrFormat("ui.medical.hunger", $"{survivor.Hunger:0}")));
                    row.AddChild(AshfallUiHelpers.MakeMono(TrFormat("ui.medical.thirst", $"{survivor.Thirst:0}")));
                    card.AddChild(row);

                    // ── Treatment action row (Task #133: all treatments go
                    //    through the validated pipeline transaction path when
                    //    the pipeline is bound) ────────────────────────────
                    string targetId = survivor.Id;
                    var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                    var btnHeal = AshfallUiHelpers.MakeButton(
                        TrFormat("ui.medical.bandage", 25, bandageCount), () =>
                        {
                            if (TryExecuteTreatment(targetId, MedicalTreatmentCatalog.TreatmentBandage))
                            {
                                _medicalHost.AddCareEntry(targetId, Tr("ui.medical.care_bandage", "Applied sterile bandage."));
                                OnTreatmentAdministered?.Invoke();
                                RefreshView();
                            }
                        });
                    btnHeal.Disabled = bandageCount <= 0 || survivor.Health >= survivor.MaxHealthCap;
                    btnHeal.CustomMinimumSize = new Vector2(160, 28);
                    actionRow.AddChild(btnHeal);

                    var btnIodine = AshfallUiHelpers.MakeButton(
                        TrFormat("ui.medical.iodine", iodineCount), () =>
                        {
                            if (TryExecuteTreatment(targetId, MedicalTreatmentCatalog.TreatmentIodine))
                            {
                                _medicalHost.AddCareEntry(targetId, Tr("ui.medical.care_iodine", "Administered Potassium Iodide."));
                                OnTreatmentAdministered?.Invoke();
                                RefreshView();
                            }
                        });
                    btnIodine.Disabled = iodineCount <= 0;
                    btnIodine.CustomMinimumSize = new Vector2(160, 28);
                    actionRow.AddChild(btnIodine);

                    var btnRadAway = AshfallUiHelpers.MakeButton(
                        TrFormat("ui.medical.anti_rad", 40, radAwayCount), () =>
                        {
                            if (TryExecuteTreatment(targetId, MedicalTreatmentCatalog.TreatmentAntiRad))
                            {
                                _medicalHost.AddCareEntry(targetId, Tr("ui.medical.care_anti_rad", "Administered anti-rad chelation agent."));
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
                        var respText = AshfallUiHelpers.MakeSmall(TrFormat("ui.medical.lung_row", respLabel, $"{respDeg:F0}"));
                        respText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                        bool respCritical = respDeg >= RespiratoryDegenerationSystem.SevereCoughThreshold;
                        respText.AddThemeColorOverride("font_color",
                            AshfallUiHelpers.ToColor(respCritical
                                ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                        respRow.AddChild(respText);

                        if (reliefHours > 0f)
                        {
                            var relief = AshfallUiHelpers.MakeMono(TrFormat("ui.medical.inhaler_relief", $"{reliefHours:F0}"));
                            relief.AddThemeColorOverride("font_color",
                                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                            respRow.AddChild(relief);
                        }
                        card.AddChild(respRow);

                        // Inhaler treatment action row
                        var inhalerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                        bool canApplyInhaler = inhalerCount > 0 && respDeg > 0f;
                        string inhalerBtnText = canApplyInhaler
                            ? TrFormat("ui.medical.inhaler_apply", $"{RespiratoryDegenerationSystem.InhalerDegradationReduction:F0}", inhalerCount)
                            : TrFormat("ui.medical.inhaler_plain", inhalerCount);
                        string inhalerReason = !canApplyInhaler
                            ? (inhalerCount <= 0 ? Tr("ui.medical.inhaler_no_item", "No inhaler in inventory — craft recipe_inhaler") : Tr("ui.medical.inhaler_no_damage", "No respiratory damage"))
                            : string.Empty;

                        string respTargetId = survivor.Id;
                        var btnInhaler = AshfallUiHelpers.MakeButton(inhalerBtnText, () =>
                        {
                            if (TryExecuteTreatment(respTargetId, MedicalTreatmentCatalog.TreatmentInhaler))
                            {
                                _medicalHost.AddCareEntry(respTargetId, Tr("ui.medical.care_inhaler", "Applied improvised inhaler."));
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
                                TrFormat("ui.medical.herbal_tea", $"{RespiratoryDegenerationSystem.HerbalTeaDegradationReduction:F0}", herbalTeaCount),
                                () =>
                                {
                                    if (TryExecuteTreatment(teaTargetId, MedicalTreatmentCatalog.TreatmentHerbalTea))
                                    {
                                        _medicalHost.AddCareEntry(teaTargetId, Tr("ui.medical.care_herbal_tea", "Administered herbal tea."));
                                        OnTreatmentAdministered?.Invoke();
                                        RefreshView();
                                    }
                                });
                            btnTea.CustomMinimumSize = new Vector2(200, 28);
                            inhalerRow.AddChild(btnTea);
                        }

                        card.AddChild(inhalerRow);
                    }

                    // ── Clinical context note (Plan 141) ─────────────────
                    string? clinicalConditionKey = currentDose >= Ashfall.Core.Radiation.RadiationSystem.WarnThreshold
                        ? MedicalTreatmentCatalog.RadiationSicknessId
                        : (respDeg >= RespiratoryDegenerationSystem.SevereCoughThreshold
                            ? MedicalTreatmentCatalog.RespiratoryDegenerationId
                            : (survivor.Health < Ashfall.Core.Survivors.NeedsProfile.DefaultHealthWarn ? MedicalTreatmentCatalog.HealthDeficitId : null));

                    if (clinicalConditionKey != null && _medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, clinicalConditionKey, survivor.Id);
                        if (prose != null)
                        {
                            var noteRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                            string summary = prose.DiagnosisSummary.Length > 80 ? prose.DiagnosisSummary.Substring(0, 77) + "..." : prose.DiagnosisSummary;
                            var noteLabel = AshfallUiHelpers.MakeMetadata($"[CLINICAL NOTE] {prose.DisplayName}: {summary} · Observe: {prose.SymptomLine}");
                            noteLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                            noteRow.AddChild(noteLabel);
                            card.AddChild(noteRow);
                        }
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
                    AshfallUiHelpers.MakeMetadata(Tr("ui.medical.no_chem_dependencies", "No active chemical dependencies or withdrawal ledgers.")));

            _treatmentList.AddChild(AshfallUiHelpers.MakeDataRow(
                Tr("ui.medical.active_penalties", "Active Cohort Penalties"),
                TrFormat("ui.medical.cohort_penalty", $"{_medicalHost.ActiveCraftingPenalty:P0}", $"{_medicalHost.ActiveCombatPenalty:P0}"),
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));

            _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(MedicalVigilText.Format(_medicalHost.Vigil)));

            RenderScheduledProcedures();
            RenderDiseaseSection();

            // ── Medical supplies on hand ───────────────────────────────
            if (_inventoryHost == null)
            {
                _supplyList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.inventory_not_bound", "Inventory session not bound.")));
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
                    AshfallUiHelpers.MakeMetadata(TrFormat("ui.medical.last_event", _medicalHost.LastEvent)));
        }

        /// <summary>
        /// Projects the pipeline's persisted procedure ledger. The panel never
        /// advances or validates a procedure; it only offers the existing
        /// typed cancellation command and displays reservation/readiness data
        /// already owned by Core.
        /// </summary>
        private void RenderScheduledProcedures()
        {
            var host = _medicalHost;
            var pipeline = host?.Pipeline;
            if (pipeline == null) return;

            _treatmentList.AddChild(AshfallUiHelpers.MakeSeparator());
            _treatmentList.AddChild(AshfallUiHelpers.MakeSubsectionHeader(Tr("ui.medical.section.scheduled", "SCHEDULED PROCEDURES")));

            var active = pipeline.Schedule.Active;
            if (active.Count == 0)
            {
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.no_procedures", "No procedures currently queued.")));
                return;
            }

            foreach (var row in active.OrderBy(r => r.procedureId))
            {
                var def = MedicalTreatmentCatalog.Get(row.treatmentId);
                string displayName = def?.DisplayName ?? row.treatmentId.Replace('_', ' ');
                string readiness = row.remainingHours <= 0f
                    ? Tr("ui.medical.ready_for_tick", "READY FOR TICK")
                    : TrFormat("ui.medical.ready_in", $"{row.remainingHours:0.#}");
                string reservations = string.Join(", ", row.reservationIds
                    .Select(id => pipeline.Reservations.TryGet(id, out var claim)
                        ? $"{ResolveItemDisplayName(claim.targetId)} ×{claim.quantity}"
                        : "reservation missing")
                    .OrderBy(value => value, StringComparer.Ordinal));
                if (string.IsNullOrEmpty(reservations)) reservations = "none";

                var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                card.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                card.AddChild(AshfallUiHelpers.MakeDataRow(
                    $"#{row.procedureId} {displayName}",
                    $"{FormatSurvivorName(row.survivorId)} · {readiness}",
                    AshfallUiHelpers.ToColor(row.remainingHours <= 0f
                        ? Ashfall.Core.UI.Theme.Lethe
                        : Ashfall.Core.UI.Theme.Pale)));
                card.AddChild(AshfallUiHelpers.MakeMetadata(TrFormat("ui.medical.reserved", reservations)));

                var cancel = AshfallUiHelpers.MakeButton(Tr("ui.medical.cancel_reservation", "CANCEL / RELEASE RESERVATION"), () =>
                {
                    var result = pipeline.ExecuteCancel(row.procedureId);
                    if (result.Success)
                        host!.AddCareEntry(row.survivorId, TrFormat("ui.medical.care_cancelled", displayName));
                    else
                        GD.PushWarning($"[Medical] procedure {row.procedureId} cancellation refused: {result.ReasonCode}");
                    RefreshView();
                });
                cancel.CustomMinimumSize = new Vector2(250, 28);
                card.AddChild(cancel);
                var panel = AshfallUiHelpers.MakePanel();
                panel.AddChild(card);
                _treatmentList.AddChild(panel);
            }
        }

        private string ResolveItemDisplayName(string itemId)
        {
            if (_inventoryHost?.Catalog != null)
            {
                var definition = _inventoryHost.Catalog.Get(itemId);
                if (definition != null && !string.IsNullOrEmpty(definition.displayName))
                    return definition.displayName;
            }
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo
                .ToTitleCase(itemId.Replace('_', ' '));
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
        /// Task #133: submit a validated treatment command through the pipeline
        /// transaction path. The pipeline — not this panel — decides legality
        /// (patient, contraindication, medicine, diagnosis) and performs the
        /// atomic reserve → consume → apply. Returns false (with a status-line
        /// reason) when blocked; no partial consumption is possible.
        /// </summary>
        private bool TryExecuteTreatment(string survivorId, string treatmentId)
        {
            var pipeline = _medicalHost?.Pipeline;
            if (pipeline == null || _survivorsHost == null)
                return false;
            if (!Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
                return false;

            var result = pipeline.ExecuteTreatment(sv, treatmentId);
            if (result.Success)
                return true;
            GD.PushWarning($"[Medical] treatment {treatmentId} refused for {survivorId}: {result.ReasonCode}");
            return false;
        }

        /// <summary>
        /// Task #133 P1: targeted isolation command — the disease handler is
        /// chosen per call, so quarantine/release never needs a per-disease
        /// treatment id. The pipeline still owns every rule.
        /// </summary>
        private bool TryExecuteTreatment(string survivorId, string treatmentId, string targetAfflictionId)
        {
            var pipeline = _medicalHost?.Pipeline;
            if (pipeline == null) return false;
            if (!Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)) return false;
            if (!Ashfall.Core.Medical.AfflictionId.IsValid(targetAfflictionId, out _)) return false;

            var result = pipeline.ExecuteTreatment(
                sv, treatmentId, target: new Ashfall.Core.Medical.AfflictionId(targetAfflictionId));
            if (result.Success)
                return true;
            GD.PushWarning($"[Medical] treatment {treatmentId} refused for {survivorId}: {result.ReasonCode}");
            return false;
        }

        /// <summary>
        /// Task #133 P1: untargeted clinical examination through the pipeline.
        /// The player never names a disease — the examination confirms whatever
        /// is already suspected, so hidden identities cannot be probed.
        /// </summary>
        private bool TryIdentify(string survivorId)
        {
            var pipeline = _medicalHost?.Pipeline;
            if (pipeline == null) return false;
            if (!Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)) return false;

            var result = pipeline.ExecuteIdentify(sv);
            if (result.Success)
                return true;
            GD.PushWarning($"[Medical] identify refused for {survivorId}: {result.ReasonCode}");
            return false;
        }

        /// <summary>Task #133 P1: camp-wide vector protocol through the pipeline.</summary>
        private bool TryExecuteProtocol(string protocolId)
        {
            var pipeline = _medicalHost?.Pipeline;
            if (pipeline == null) return false;

            var result = pipeline.ExecuteProtocol(protocolId);
            if (result.Success)
                return true;
            GD.PushWarning($"[Medical] protocol {protocolId} refused: {result.ReasonCode}");
            return false;
        }

        /// <summary>
        /// Task #133 P1 — disease ward section: unidentified illnesses (masked),
        /// confirmed infections with isolation actions, and the camp-wide
        /// vector protocols. Every action routes through the pipeline; this
        /// panel owns no clinical rules and never names a disease the pipeline
        /// has not confirmed.
        /// </summary>
        private void RenderDiseaseSection()
        {
            var pipeline = _medicalHost?.Pipeline;
            if (pipeline == null || _survivorsHost == null)
            {
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.medical.disease_ward_offline", "Disease ward offline (pipeline not bound).")));
                return;
            }

            _treatmentList.AddChild(AshfallUiHelpers.MakeSeparator());
            _treatmentList.AddChild(AshfallUiHelpers.MakeSubsectionHeader(Tr("ui.medical.section.disease_ward", "DISEASE WARD — ISOLATION & PROTOCOLS")));

            var projector = new PatientRecordProjector(pipeline);
            bool anyRow = false;
            foreach (var survivor in _survivorsHost.RosterState)
            {
                if (survivor == null || !survivor.IsAliveState) continue;
                if (!Ashfall.Core.Survivors.SurvivorId.TryParse(survivor.Id, out var sv)) continue;
                var record = projector.Project(sv);

                foreach (var affliction in record.Afflictions)
                {
                    bool unidentified = string.Equals(
                        affliction.AfflictionId, MedicalTreatmentCatalog.UnidentifiedIllnessId, StringComparison.Ordinal);
                    bool isDisease = unidentified
                        || affliction.AfflictionId.StartsWith("disease_", StringComparison.Ordinal);
                    if (!isDisease) continue;

                    var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    string label = unidentified
                        ? TrFormat("ui.medical.affliction_unidentified", FormatSurvivorName(survivor.Id), affliction.StageLabel)
                        : TrFormat("ui.medical.affliction_row", FormatSurvivorName(survivor.Id), affliction.StageLabel, $"{affliction.SeverityValue:0}");
                    var labelNode = AshfallUiHelpers.MakeSmall(label);
                    labelNode.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    row.AddChild(labelNode);

                    if (unidentified)
                    {
                        string target = survivor.Id;
                        var btn = AshfallUiHelpers.MakeButton(Tr("ui.medical.identify", "IDENTIFY"), () =>
                        {
                            if (TryIdentify(target)) RefreshView();
                        });
                        row.AddChild(btn);
                    }
                    else
                    {
                        string target = survivor.Id;
                        string diseaseId = affliction.AfflictionId;
                        bool quarantineOk = pipeline.PreviewTreatment(
                            sv, MedicalTreatmentCatalog.TreatmentQuarantine,
                            target: new Ashfall.Core.Medical.AfflictionId(diseaseId)).IsAvailable;
                        bool releaseOk = pipeline.PreviewTreatment(
                            sv, MedicalTreatmentCatalog.TreatmentRelease,
                            target: new Ashfall.Core.Medical.AfflictionId(diseaseId)).IsAvailable;

                        var btnQuarantine = AshfallUiHelpers.MakeButton(Tr("ui.medical.isolate", "ISOLATE"), () =>
                        {
                            if (TryExecuteTreatment(target, MedicalTreatmentCatalog.TreatmentQuarantine, diseaseId))
                                RefreshView();
                        });
                        btnQuarantine.Disabled = !quarantineOk;
                        row.AddChild(btnQuarantine);

                        var btnRelease = AshfallUiHelpers.MakeButton(Tr("ui.medical.release", "RELEASE"), () =>
                        {
                            if (TryExecuteTreatment(target, MedicalTreatmentCatalog.TreatmentRelease, diseaseId))
                                RefreshView();
                        });
                        btnRelease.Disabled = !releaseOk;
                        row.AddChild(btnRelease);
                    }

                    _treatmentList.AddChild(row);
                    anyRow = true;

                    if (!unidentified && _medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, affliction.AfflictionId, survivor.Id);
                        if (prose != null)
                        {
                            var subRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                            string summary = prose.DiagnosisSummary.Length > 80 ? prose.DiagnosisSummary.Substring(0, 77) + "..." : prose.DiagnosisSummary;
                            var clinicalLabel = AshfallUiHelpers.MakeMetadata("  " + TrFormat("ui.medical.clinical_note", summary, prose.SymptomLine));
                            clinicalLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                            subRow.AddChild(clinicalLabel);
                            _treatmentList.AddChild(subRow);
                        }
                    }
                }
            }
            if (!anyRow)
                _treatmentList.AddChild(AshfallUiHelpers.MakeMetadata(
                    Tr("ui.medical.no_infections", "No suspected or confirmed infections in the shelter.")));

            // Camp-wide vector protocols: one application each; the pipeline
            // consumes the catalog countermeasure through the inventory.
            foreach (var protocol in pipeline.Protocols.OrderBy(p => p.ProtocolId, StringComparer.Ordinal))
            {
                var preview = pipeline.PreviewProtocol(protocol.ProtocolId);
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var label = AshfallUiHelpers.MakeSmall(
                    $"{protocol.DisplayName} — {DescribeProtocolCost(protocol)}");
                label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(label);

                string protocolId = protocol.ProtocolId;
                var btn = AshfallUiHelpers.MakeButton(Tr("ui.medical.apply", "APPLY"), () =>
                {
                    if (TryExecuteProtocol(protocolId)) RefreshView();
                });
                btn.Disabled = !preview.IsAvailable;
                btn.TooltipText = preview.IsAvailable ? Tr("ui.medical.treatment_ready", "ok") : preview.FailureCode;
                row.AddChild(btn);
                _treatmentList.AddChild(row);
            }
        }

        private static string DescribeProtocolCost(Ashfall.Core.Medical.IMedicalProtocolHandler protocol)
        {
            if (protocol.ItemCosts.Count == 0) return Tr("ui.medical.no_supplies", "no supplies needed");
            return string.Join(", ", protocol.ItemCosts.Select(kv => $"{kv.Value}× {kv.Key.Replace('_', ' ')}"));
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
                avgHp <= Ashfall.Core.Survivors.NeedsProfile.DefaultHealthCritical ? AshfallMetricCard.Criticality.Critical
                : avgHp <= Ashfall.Core.Survivors.NeedsProfile.DefaultHealthWarn ? AshfallMetricCard.Criticality.Warn
                : avgHp >= 75 ? AshfallMetricCard.Criticality.Normal
                : AshfallMetricCard.Criticality.Caution;

            AshfallMetricCard.Criticality doseCrit =
                maxDose < Ashfall.Core.Radiation.RadiationSystem.WarnThreshold * 0.5f ? AshfallMetricCard.Criticality.Normal
                : maxDose < Ashfall.Core.Radiation.RadiationSystem.WarnThreshold ? AshfallMetricCard.Criticality.Caution
                : maxDose < Ashfall.Core.Radiation.RadiationSystem.AcuteThreshold ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            _statusRail.Set("cohort",   $"{living}/{cohort}",  cohortCrit);
            _statusRail.Set("avgHp",    $"{avgHp:0}%",          hpCrit);
            _statusRail.Set("doseMax",  $"{maxDose:0} mSv",     doseCrit);
            _statusRail.Set("activeTx", $"{activeTx}",          AshfallMetricCard.Criticality.Normal);

            // Vigil: state from the medical engine's public state machine; the
            // line is localized by the panel, and the rail severity comes from
            // the state itself instead of English substring matching.
            string vigilState = Tr("ui.medical.vigil_standby", "STANDBY");
            AshfallMetricCard.Criticality vigilCrit = AshfallMetricCard.Criticality.Normal;
            if (_medicalHost?.Vigil != null)
            {
                string line = MedicalVigilText.Format(_medicalHost.Vigil);
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string upper = line.ToUpperInvariant();
                    vigilState = upper.Length > 18 ? upper.Substring(0, 18) : upper;
                    vigilCrit = _medicalHost.Vigil.IsActive
                        ? AshfallMetricCard.Criticality.Caution
                        : AshfallMetricCard.Criticality.Normal;
                }
            }
            _statusRail.Set("vigil", vigilState, vigilCrit);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            AddChild(AshfallUiHelpers.MakeBackdropOverlay());

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
            _shell.SetContent(scrollRoot);

            var contentBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollMargin.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.medical.section.health", "SURVIVOR HEALTH, DOSIMETRY & RESPIRATORY")));
            _healthStats = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            _healthStats.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentBox.AddChild(_healthStats);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.medical.section.treatment", "TREATMENT & DETOXIFICATION LEDGER")));
            _treatmentList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_treatmentList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.medical.section.supplies", "MEDICAL SUPPLIES ON HAND")));
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
                CatalogDiagnostics.Warn("<scroll>", "ScrollToChild", ex_CATDIAG);
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
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }

        public void Unbind()
        {
            if (_respiratory != null)
            {
                _respiratory.OnStateChanged -= OnRespiratoryStateChanged;
                _respiratory = null;
            }
            _medicalHost = null;
            _survivorsHost = null;
            _inventoryHost = null;
            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
