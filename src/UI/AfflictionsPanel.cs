// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using Ashfall.Core.Medical;
using AtomicWar.GodotApp.Host;
using AtomicWar.GodotApp.Localization;

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
        private MedicalTextCatalog? _medicalTexts;
        private ChronicConditionHostSession? _chronic;
        private Func<string, string, string, ActionResult>? _fitAccommodation;
        private Func<string, string, ActionResult>? _removeAccommodation;
        private string _accommodationFeedback = string.Empty;

        public void Bind(
            MedicalHostSession? medical = null,
            SurvivorsHostSession? survivors = null,
            InventoryHostSession? inventory = null,
            RespiratoryDegenerationSystem? respiratory = null,
            MedicalTextCatalog? medicalTexts = null,
            ChronicConditionHostSession? chronicConditions = null,
            Func<string, string, string, ActionResult>? fitAccommodation = null,
            Func<string, string, ActionResult>? removeAccommodation = null)
        {
            // Live refresh: affliction rows track survivor/inventory state and
            // newly recorded chronic conditions while the panel is open.
            if (_survivors != null) _survivors.StateChanged -= RefreshView;
            if (_inventory != null) _inventory.StateChanged -= RefreshView;
            if (_chronic != null) _chronic.OnConditionRecorded -= OnChronicConditionRecorded;

            _chronic = chronicConditions;
            _fitAccommodation = fitAccommodation;
            _removeAccommodation = removeAccommodation;
            _accommodationFeedback = string.Empty;
            if (_chronic != null) _chronic.OnConditionRecorded += OnChronicConditionRecorded;

            _medical = medical;
            _survivors = survivors;
            _inventory = inventory;
            _respiratory = respiratory;
            _medicalTexts = medicalTexts ?? LoadDefaultMedicalTexts();
            IsBound = _medical != null || _survivors != null;

            if (_survivors != null) _survivors.StateChanged += RefreshView;
            if (_inventory != null) _inventory.StateChanged += RefreshView;
            RefreshView();
        }

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

        /// <summary>Bridges the chronic session's condition event to the subscription-safe refresh.</summary>
        private void OnChronicConditionRecorded(string message) => RefreshView();

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
                    if (_medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.HealthDeficitId, s.Id);
                        if (prose != null)
                        {
                            string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                            AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
                        }
                    }
                }
                if (rad is { HasAcuteRadiationSickness: true })
                {
                    AddAffliction(_activeList, $"{Name(s.Id)} — Acute radiation sickness (dose {rad.RadiationDose:0} mSv)",
                        Ashfall.Core.UI.Theme.Critical);
                    RenderedActiveCount++;
                    if (_medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.RadiationSicknessId, s.Id);
                        if (prose != null)
                        {
                            string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                            AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
                        }
                    }
                }
                if (respDeg >= RespiratoryDegenerationSystem.SevereCoughThreshold)
                {
                    AddAffliction(_activeList, $"{Name(s.Id)} — Severe respiratory degeneration ({respDeg:0}%)",
                        Ashfall.Core.UI.Theme.Critical);
                    RenderedActiveCount++;
                    if (_medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.RespiratoryDegenerationId, s.Id);
                        if (prose != null)
                        {
                            string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                            AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
                        }
                    }
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
                            if (_medicalTexts != null)
                            {
                                var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, affliction.AfflictionId, s.Id);
                                if (prose != null)
                                {
                                    string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                                    AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
                                }
                            }
                        }
                        else
                        {
                            AddAffliction(_activeList,
                                $"{Name(s.Id)} — {affliction.StageLabel} (day {affliction.SeverityValue:0})",
                                Ashfall.Core.UI.Theme.Critical);
                            if (_medicalTexts != null)
                            {
                                var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, affliction.AfflictionId, s.Id);
                                if (prose != null)
                                {
                                    string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                                    AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
                                }
                            }
                        }
                        RenderedActiveCount++;
                    }
                }

                // Plan 143: Medical Afflictions -> Quest & Work Bridge projection
                if (_medical?.Bridge != null)
                {
                    var activeAfflictionIds = new List<string>();
                    if (s.Health < 30f) activeAfflictionIds.Add(MedicalTreatmentCatalog.HealthDeficitId);
                    if (rad is { HasAcuteRadiationSickness: true }) activeAfflictionIds.Add(MedicalTreatmentCatalog.RadiationSicknessId);
                    if (respDeg > 0f) activeAfflictionIds.Add(MedicalTreatmentCatalog.RespiratoryDegenerationId);
                    if (_medical.Pipeline != null && Ashfall.Core.Survivors.SurvivorId.TryParse(s.Id, out var projSv))
                    {
                        var rec = new PatientRecordProjector(_medical.Pipeline).Project(projSv);
                        foreach (var a in rec.Afflictions)
                        {
                            if (!string.IsNullOrEmpty(a.AfflictionId) && !activeAfflictionIds.Contains(a.AfflictionId))
                                activeAfflictionIds.Add(a.AfflictionId);
                        }
                    }

                    if (activeAfflictionIds.Count > 0)
                    {
                        var workMods = _medical.Bridge.CalculateWorkModifiers(activeAfflictionIds);
                        if (workMods.SpeedMultiplier < 1.0f || workMods.ExcludedDutyTypes.Count > 0)
                        {
                            string dutyExcl = workMods.ExcludedDutyTypes.Count > 0 ? $" · Excluded duties: {string.Join(", ", workMods.ExcludedDutyTypes)}" : string.Empty;
                            AddDimSubline(_activeList, $"   ↳ Work Impact: {workMods.SpeedMultiplier * 100:0}% speed, {workMods.QualityMultiplier * 100:0}% quality{dutyExcl}");
                        }
                        var unlocked = _medical.Bridge.GetUnlockedQuestTags(activeAfflictionIds);
                        if (unlocked.Count > 0)
                        {
                            AddDimSubline(_activeList, $"   ↳ Unlocked Quests: {string.Join(", ", unlocked)}");
                        }
                    }
                }
            }

            if (RenderedActiveCount == 0)
                _activeList.AddChild(MakeDimLine("No active afflictions."));

            // Plan 193/198 — bounded medical record projection (day + event id only;
            // no free-text notes, no second diagnosis store).
            if (_medical?.Pipeline != null)
            {
                var rosterIds = _survivors?.RosterState;
                if (rosterIds != null)
                {
                    for (int ri = 0; ri < rosterIds.Count; ri++)
                    {
                        var rs = rosterIds[ri];
                        if (rs == null) continue;
                        var recent = _medical.Pipeline.Record.ForSurvivor(rs.Id, 2);
                        if (recent.Count == 0) continue;

                        _activeList.AddChild(MakeDimLine($"Recent medical record — {Name(rs.Id)}:"));
                        for (int ei = 0; ei < recent.Count; ei++)
                        {
                            var entry = recent[ei];
                            string detail = string.IsNullOrEmpty(entry.detail) ? string.Empty : $" · {entry.detail}";
                            AddDimSubline(_activeList, $"   Day {entry.day} — {MedicalRecordLabel(entry.kind)}{detail}");
                        }
                    }
                }
            }
        }

        /// <summary>Non-stigmatizing display label for a recorded medical event kind.</summary>
        private static string MedicalRecordLabel(string kind) => kind switch
        {
            MedicalRecordKinds.DiagnosisSuspected => "condition suspected",
            MedicalRecordKinds.DiagnosisConfirmed => "condition identified",
            MedicalRecordKinds.PatientStabilized => "patient stabilized",
            MedicalRecordKinds.PatientRecovered => "patient recovered",
            MedicalRecordKinds.TreatmentScheduled => "treatment scheduled",
            MedicalRecordKinds.TreatmentCompleted => "treatment completed",
            MedicalRecordKinds.TreatmentRefused => "treatment not given",
            MedicalRecordKinds.ProtocolExecuted => "camp protocol carried out",
            _ => "medical event"
        };

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
                    if (_medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, "chronic_radiation", s.Id);
                        if (prose != null)
                        {
                            string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                            AddDimSubline(_chronicList, $"   ↳ {summary}");
                        }
                    }
                }
                if (_respiratory is { } r && r.HasPermanentLungDamage(s.Id))
                {
                    AddAffliction(_chronicList, $"{Name(s.Id)} — Permanent lung damage", Ashfall.Core.UI.Theme.Entropy);
                    chronicCount++;
                    if (_medicalTexts != null)
                    {
                        var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, "permanent_lung_damage", s.Id);
                        if (prose != null)
                        {
                            string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                            AddDimSubline(_chronicList, $"   ↳ {summary}");
                        }
                    }
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
                            if (_medicalTexts != null)
                            {
                                var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.ChemicalDependencyId, kv.Key);
                                if (prose != null)
                                {
                                    string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
                                    AddDimSubline(_chronicList, $"   ↳ {summary}");
                                }
                            }
                        }
                    }
                }
            }

            // Plan 193 / T18: tracked chronic conditions + fitted accommodations
            // are truthful rows in the existing CHRONIC list, and each condition
            // now carries the player's accommodation decision (FIT / REMOVE)
            // routed to the host command. The cost gate reads the real inventory
            // authority; nothing is written here.
            if (_chronic != null && _survivors?.RosterState != null)
            {
                if (!string.IsNullOrEmpty(_accommodationFeedback))
                    AddDimSubline(_chronicList, _accommodationFeedback);

                foreach (var s in _survivors.RosterState)
                {
                    if (s == null || !s.IsAlive) continue;
                    var trackedConditionIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var c in _chronic.GetSurvivorConditions(s.Id))
                    {
                        trackedConditionIds.Add(c.ConditionId);
                        var def = _chronic.System.GetConditionDef(c.ConditionId);
                        AddAffliction(_chronicList,
                            $"{Name(s.Id)} — {def?.display_name ?? c.ConditionId}",
                            Ashfall.Core.UI.Theme.Entropy);
                        AddDimSubline(_chronicList,
                            $"   ↳ cause: {c.Cause} · since day {c.OnsetDay} · {def?.severity ?? c.Severity}");
                        chronicCount++;

                        RenderAccommodationDecision(s.Id, c.ConditionId, def?.recommended_accommodation_id);
                    }

                    // Fitted accommodations whose condition is not tracked as a
                    // row above stay visible (legacy/orphan fits are not hidden).
                    foreach (var a in _chronic.GetSurvivorAccommodations(s.Id))
                    {
                        if (!string.IsNullOrEmpty(a.ConditionRefId)
                            && trackedConditionIds.Contains(a.ConditionRefId)) continue;
                        var def = _chronic.System.GetAccommodationDef(a.AccommodationId);
                        AddAffliction(_chronicList,
                            $"(+ {Name(s.Id)} — {def?.display_name ?? a.AccommodationId})",
                            Ashfall.Core.UI.Theme.Success);
                        chronicCount++;
                    }
                }
            }

            if (chronicCount == 0)
                _chronicList.AddChild(MakeDimLine("No chronic conditions."));
        }

        /// <summary>
        /// T18 — the per-condition accommodation decision row. The recommended
        /// accommodation is shown with its authored maintenance cost; FIT is
        /// enabled only when the real inventory authority has every item, and a
        /// fitted accommodation offers REMOVE. The row writes nothing itself.
        /// </summary>
        private void RenderAccommodationDecision(string survivorId, string conditionId, string? recommendationId)
        {
            if (string.IsNullOrWhiteSpace(recommendationId)) return;
            var def = _chronic?.System.GetAccommodationDef(recommendationId);
            if (def == null)
            {
                AddDimSubline(_chronicList,
                    $"   ↳ recommended accommodation '{recommendationId}' is not in the catalog.");
                return;
            }

            var row = AshfallUiHelpers.MakeHBox();
            string cost = def.maintenance_cost_items != null && def.maintenance_cost_items.Count > 0
                ? string.Join(", ", def.maintenance_cost_items)
                : "no materials";
            bool fitted = IsAccommodationFitted(survivorId, def.accommodation_id);

            var label = new Label
            {
                Text = fitted
                    ? $"   ↳ accommodation fitted: {def.display_name}"
                    : $"   ↳ accommodation: {def.display_name} · requires {cost}"
            };
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            row.AddChild(label);

            if (fitted)
            {
                string accId = def.accommodation_id;
                row.AddChild(AshfallUiHelpers.MakeButton(
                    AshfallLocalization.Tr("ui.afflictions.remove_accommodation", "REMOVE"),
                    () => RemoveAccommodation(survivorId, accId)));
            }
            else
            {
                var missing = MissingAccommodationCostItems(def);
                bool affordable = missing.Count == 0;
                string cond = conditionId;
                var fit = AshfallUiHelpers.MakeButton(
                    AshfallLocalization.Tr("ui.afflictions.fit_accommodation", "FIT"),
                    () => FitAccommodation(survivorId, cond, def.accommodation_id),
                    disabled: !affordable);
                fit.TooltipText = affordable
                    ? $"Fit {def.display_name} (consumes {cost})."
                    : $"Missing: {string.Join(", ", missing)}.";
                row.AddChild(fit);
            }
            _chronicList.AddChild(row);
        }

        /// <summary>True when the survivor has this accommodation fitted and active.</summary>
        private bool IsAccommodationFitted(string survivorId, string accommodationId)
        {
            if (_chronic == null) return false;
            var active = _chronic.GetSurvivorAccommodations(survivorId);
            for (int i = 0; i < active.Count; i++)
            {
                if (string.Equals(active[i].AccommodationId, accommodationId, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Authored cost items the inventory authority does not currently hold.</summary>
        private List<string> MissingAccommodationCostItems(AccommodationDef def)
        {
            var missing = new List<string>();
            if (def?.maintenance_cost_items == null) return missing;
            foreach (var itemId in def.maintenance_cost_items)
            {
                if (string.IsNullOrWhiteSpace(itemId)) continue;
                if ((_inventory?.Inventory?.CountById(itemId) ?? 0) < 1)
                    missing.Add(itemId);
            }
            return missing;
        }

        /// <summary>Routes the accommodation decision through the host command.</summary>
        private void FitAccommodation(string survivorId, string conditionId, string accommodationId)
        {
            var def = _chronic?.System.GetAccommodationDef(accommodationId);
            string name = def?.display_name ?? accommodationId;
            if (_fitAccommodation == null)
            {
                _accommodationFeedback = $"Cannot fit {name}: accommodation command not wired.";
                RefreshView();
                return;
            }
            var result = _fitAccommodation(survivorId, conditionId, accommodationId);
            _accommodationFeedback = result.IsSuccess
                ? $"Fitted {name} for {Name(survivorId)}."
                : ActionRefusalText.Line(result, $"Cannot fit {name}");
            RefreshView();
        }

        /// <summary>Routes the accommodation removal through the host command.</summary>
        private void RemoveAccommodation(string survivorId, string accommodationId)
        {
            var def = _chronic?.System.GetAccommodationDef(accommodationId);
            string name = def?.display_name ?? accommodationId;
            if (_removeAccommodation == null)
            {
                _accommodationFeedback = $"Cannot remove {name}: accommodation command not wired.";
                RefreshView();
                return;
            }
            var result = _removeAccommodation(survivorId, accommodationId);
            _accommodationFeedback = result.IsSuccess
                ? $"Removed {name} from {Name(survivorId)}."
                : ActionRefusalText.Line(result, $"Cannot remove {name}");
            RefreshView();
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

        private void AddDimSubline(VBoxContainer parent, string text)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
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

            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
