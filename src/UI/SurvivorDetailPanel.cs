// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Culture;
using Ashfall.Core.UI;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survivor Detail panel. Shows per-survivor info, needs, traits,
    /// and status — bound to the live SurvivorsHostSession for a specific
    /// survivor id.
    ///
    /// Ticket #125: layout chrome (dialog frame, section headers, separators,
    /// close button) is owned by
    /// <c>res://assets/ui/panels/SurvivorDetailPanel.tscn</c>. Dynamic content
    /// (the four lists) is filled at refresh time.
    /// </summary>
    public partial class SurvivorDetailPanel : Control
    {
        public event Action? OnClose;

        private SceneBinder? _binder;
        private VBoxContainer _survivorInfo = null!;
        private VBoxContainer _needsList = null!;
        private VBoxContainer _traitsList = null!;
        private VBoxContainer _statusList = null!;
        private Button _closeButton = null!;

        private SurvivorsHostSession? _survivors;
        private Ashfall.Core.Survivors.SurvivorEnrichmentService? _enrichmentService;
        private string _survivorId = string.Empty;

        /// <summary>Read-only projection supplied by Main; no fitness state is
        /// owned by this panel.</summary>
        public Func<string, FitnessVerdict?>? FitnessProvider { get; set; }

        /// <summary>Read-only personal-claim projection supplied by Main.</summary>
        public Func<string, IReadOnlyList<PersonalBelonging>>? BelongingsProvider { get; set; }

        /// <summary>Read-only authored documentation projection supplied by Main.</summary>
        public Func<string, IReadOnlyList<DocumentationItem>>? DocumentationProvider { get; set; }

        /// <summary>Read-only backstory projection supplied by Main (Plan 174).</summary>
        public Func<string, Ashfall.Core.Survivors.SurvivorBackstory?>? BackstoryProvider { get; set; }

        /// <summary>Read-only bunker faction projection supplied by Main (Plan 148).</summary>
        public Func<string, string?>? IdeologicalFactionProvider { get; set; }

        /// <summary>Read-only romantic relationship projection supplied by Main (Plan 150).</summary>
        public Func<string, (string PartnerId, string Stage, bool IsSoulmate)?>? RomanceProvider { get; set; }

        // Plan 217 — read-only kinship projection (parents/partner/children/
        // family name) derived from the genealogy authority's committed facts.
        // Refresh never mutates lineage state.
        public Func<string, string?>? KinshipProvider { get; set; }

        /// <summary>Read-only family unit projection supplied by Main (Plan 150).</summary>
        public Func<string, string?>? FamilyProvider { get; set; }

        /// <summary>Read-only political bloc projection supplied by Main (Plan 159).</summary>
        public Func<string, string?>? PoliticalBlocProvider { get; set; }

        /// <summary>Read-only age profile projection supplied by Main (Plan 176).</summary>
        public Func<string, Ashfall.Core.Survivors.SurvivorAgeProfile?>? AgeProfileProvider { get; set; }

        /// <summary>Read-only daily routine projection supplied by Main (Plan 188).</summary>
        public Func<string, Ashfall.Core.Survivors.SurvivorRoutineRecord?>? RoutineProvider { get; set; }

        /// <summary>Plan 195 — read-only specialization role readout (role id, display name, level, XP). Null when the survivor holds no role.</summary>
        public Func<string, (string RoleId, string DisplayName, int Level, int ExperiencePoints)?>? RoleProvider { get; set; }

        /// <summary>Plan 142 — truthful current clothing layers, warmth, wetness, and cold reduction; read-only projection.</summary>
        public Func<string, (int Layers, int TotalWarmth, float Wetness, int ColdReductionBp)?>? ClothingProvider { get; set; }

        /// <summary>F14-G / UNBLOCK-01 — read-only accessible body/limb presentation slate derived from the amputation limb authority.</summary>
        public Func<string, Ashfall.Core.Medical.SurvivorBodyPresentationSlate?>? BodySlateProvider { get; set; }

        public bool IsBound => _survivors != null && !string.IsNullOrEmpty(_survivorId);
        public int RenderedRowCount { get; private set; }

        /// <summary>Wired by Main; the panel does not read campaign state directly.</summary>
        public Func<int>? AppDayProvider { get; set; }

        public void Bind(SurvivorsHostSession? survivors, string survivorId, Ashfall.Core.Survivors.SurvivorEnrichmentService? enrichmentService = null)
        {
            if (_survivors != null)
                _survivors.Needs.OnNeedChanged -= HandleNeedChanged;
            _survivors = survivors;
            _survivorId = survivorId ?? string.Empty;
            _enrichmentService = enrichmentService;
            if (_survivors != null)
                _survivors.Needs.OnNeedChanged += HandleNeedChanged;
            RefreshView();
        }

        private void HandleNeedChanged(SurvivorNeedsState _, NeedKind __, float ___)
            => RefreshView();

        public override void _Ready()
        {
            _binder = new SceneBinder(this, typeof(SurvivorDetailPanel));
            _binder.Require<VBoxContainer>("SurvivorInfo");
            _binder.Require<VBoxContainer>("NeedsList");
            _binder.Require<VBoxContainer>("TraitsList");
            _binder.Require<VBoxContainer>("StatusList");
            _binder.Require<Button>("CloseButton");

            _survivorInfo = _binder.Get<VBoxContainer>("SurvivorInfo");
            _needsList = _binder.Get<VBoxContainer>("NeedsList");
            _traitsList = _binder.Get<VBoxContainer>("TraitsList");
            _statusList = _binder.Get<VBoxContainer>("StatusList");
            _closeButton = _binder.Get<Button>("CloseButton");
            _closeButton.Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void RefreshView()
        {
            if (_survivorInfo == null || _needsList == null || _traitsList == null || _statusList == null) return;

            AshfallUiHelpers.EmptyChildren(_survivorInfo);
            AshfallUiHelpers.EmptyChildren(_needsList);
            AshfallUiHelpers.EmptyChildren(_traitsList);
            AshfallUiHelpers.EmptyChildren(_statusList);

            RenderedRowCount = 0;

            if (_survivors == null || string.IsNullOrEmpty(_survivorId))
            {
                _survivorInfo.AddChild(MakeDimLine(Tr("ui.survivor.info.no_selection", "No survivor selected.")));
                return;
            }

            var s = _survivors.RosterState.FirstOrDefault(r => r != null && r.Id == _survivorId);
            if (s == null)
            {
                _survivorInfo.AddChild(MakeDimLine(TrFormat("ui.survivor.info.not_found", _survivorId)));
                return;
            }

            var rad = _survivors.RadStateFor(s.Id);
            var def = _survivors.Roster?.FindDefinition(s.Id);
            var view = _enrichmentService?.GetView(s.Id, def);

            // ── Survivor info ──
            AddRow(_survivorInfo, TrFormat("ui.survivor.info.name", view?.DisplayName ?? Name(s.Id)), Ashfall.Core.UI.Theme.Pale);
            AddRow(_survivorInfo, TrFormat("ui.survivor.info.profession", view?.ProfessionLabel ?? (!string.IsNullOrEmpty(def?.profession) ? def.profession : Tr("ui.survivor.info.unspecified", "Unspecified"))), Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 2;

            // Plan 195 — truthful current specialization role; read-only projection.
            var roleReadout = RoleProvider?.Invoke(_survivorId);
            if (roleReadout.HasValue && !string.IsNullOrEmpty(roleReadout.Value.RoleId))
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.specialization",
                    roleReadout.Value.DisplayName, roleReadout.Value.Level, roleReadout.Value.ExperiencePoints), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            // Plan 142 — truthful current clothing layers and cold mitigation; read-only projection.
            var clothingReadout = ClothingProvider?.Invoke(_survivorId);
            if (clothingReadout.HasValue && clothingReadout.Value.Layers > 0)
            {
                var c = clothingReadout.Value;
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.clothing",
                    c.Layers, c.TotalWarmth, $"{c.Wetness:P0}", $"{c.ColdReductionBp / 100f:F0}"), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            // F14-G / UNBLOCK-01 — accessible words-not-colour body presentation slate;
            // read-only projection over the amputation limb authority. Intact limbs
            // are summarised by the headline and not enumerated.
            var bodySlate = BodySlateProvider?.Invoke(_survivorId);
            if (bodySlate != null)
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.body", bodySlate.SummaryStatus, bodySlate.GripCapability), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
                foreach (var limb in bodySlate.Limbs)
                {
                    if (string.Equals(limb.ConditionTag, "intact", StringComparison.OrdinalIgnoreCase)) continue;
                    string maint = limb.RequiresMaintenance && !string.IsNullOrEmpty(limb.MaintenanceNotice)
                        ? $" — {limb.MaintenanceNotice}"
                        : string.Empty;
                    AddRow(_survivorInfo, $"  {limb.DisplayName}: {limb.StatusText}{maint}", Ashfall.Core.UI.Theme.Dim);
                    RenderedRowCount++;
                }
                if (bodySlate.HasPhantomPain)
                {
                    AddRow(_survivorInfo, $"  {bodySlate.PhantomPainAlert}", Ashfall.Core.UI.Theme.Warm);
                    RenderedRowCount++;
                }
            }

            if (view != null && !string.IsNullOrEmpty(view.BeliefProfileLabel) && !string.IsNullOrEmpty(view.BeliefProfileId))
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.worldview", view.BeliefProfileLabel), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            if (view != null && !string.IsNullOrEmpty(view.PersonalKeepsakeItemId))
            {
                string tagInfo = view.KeepsakeItemTags.Count > 0
                    ? $" [{string.Join(", ", view.KeepsakeItemTags)}]"
                    : string.Empty;
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.keepsake", view.KeepsakeItemLabel, tagInfo), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount++;
            }

            if (view != null && !string.IsNullOrEmpty(view.PrimaryDutyAffinity))
            {
                string bonusText = view.DutyComfortBonusPermille > 0
                    ? " " + TrFormat("ui.survivor.info.duty_bonus", view.DutyComfortBonusPermille / 10)
                    : string.Empty;
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.duty", view.PrimaryDutyAffinity, bonusText), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            if (view != null && view.CorePersonalityTraits.Count > 0)
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.traits", string.Join(" · ", view.CorePersonalityTraits)), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount++;
            }

            if (view != null && !string.IsNullOrEmpty(view.IdeologicalTensionBeliefId))
            {
                var (_, frictionLabel) = Ashfall.Core.Survivors.SurvivorEnrichmentService.ResolveFrictionBelief(view.BeliefProfileId);
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.ideological_tension", frictionLabel), Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            var faction = IdeologicalFactionProvider?.Invoke(s.Id);
            if (!string.IsNullOrEmpty(faction))
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.faction", faction), Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            var romance = RomanceProvider?.Invoke(s.Id);
            if (romance != null)
            {
                string partnerName = Name(romance.Value.PartnerId);
                string soulmateTag = romance.Value.IsSoulmate ? " " + Tr("ui.survivor.info.soulmate", "· Soulmate") : string.Empty;
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.relationship", partnerName, romance.Value.Stage, soulmateTag), Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            var family = FamilyProvider?.Invoke(s.Id);
            if (!string.IsNullOrEmpty(family))
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.family", family), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            // Plan 217 — committed kinship facts (genealogy authority), read-only.
            var kinship = KinshipProvider?.Invoke(s.Id);
            if (!string.IsNullOrEmpty(kinship))
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.lineage", kinship), Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            var politicalBloc = PoliticalBlocProvider?.Invoke(s.Id);
            if (!string.IsNullOrEmpty(politicalBloc))
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.political_bloc", politicalBloc), Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            var ageProfile = AgeProfileProvider?.Invoke(s.Id);
            if (ageProfile != null)
            {
                string retirementStatus = ageProfile.IsRetired
                    ? " " + Tr("ui.survivor.info.retired_elder", "[Retired Elder]")
                    : (ageProfile.IsRetirementEligible ? " " + Tr("ui.survivor.info.retirement_eligible", "[Retirement Eligible]") : string.Empty);
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.demographic", ageProfile.EffectiveAgeYears, ageProfile.Stage, retirementStatus), Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            var belongings = BelongingsProvider?.Invoke(s.Id);
            if (belongings != null && belongings.Count > 0)
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.personal_effects", belongings.Count), Ashfall.Core.UI.Theme.Lethe);
                int visible = Math.Min(3, belongings.Count);
                for (int i = 0; i < visible; i++)
                {
                    var belonging = belongings[i];
                    string favorite = belonging.IsFavorite ? " " + Tr("ui.survivor.info.favorite", "· favorite") : string.Empty;
                    AddRow(_survivorInfo,
                        "  " + TrFormat("ui.survivor.info.personal_effect_row", belonging.ItemName, belonging.Category, $"{belonging.SentimentalValue:0}", favorite),
                        Ashfall.Core.UI.Theme.Dim);
                }
                if (belongings.Count > visible)
                    AddRow(_survivorInfo, "  " + TrFormat("ui.survivor.info.more", belongings.Count - visible), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount += 1 + visible + (belongings.Count > visible ? 1 : 0);
            }

            var docs = DocumentationProvider?.Invoke(s.Id);
            if (docs != null && docs.Count > 0)
            {
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.authored_records", docs.Count), Ashfall.Core.UI.Theme.Lethe);
                int visible = Math.Min(3, docs.Count);
                for (int i = 0; i < visible; i++)
                {
                    var doc = docs[i];
                    AddRow(_survivorInfo,
                        "  " + TrFormat("ui.survivor.info.record_row", doc.Title, doc.Type, $"{doc.Quality:0}"),
                        Ashfall.Core.UI.Theme.Dim);
                }
                if (docs.Count > visible)
                    AddRow(_survivorInfo, "  " + TrFormat("ui.survivor.info.more", docs.Count - visible), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount += 1 + visible + (docs.Count > visible ? 1 : 0);
            }

            AddRow(_survivorInfo, TrFormat("ui.survivor.info.alive", s.IsAlive), s.IsAlive ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Critical);
            AddRow(_survivorInfo, TrFormat("ui.survivor.info.max_health", s.MaxHealthCap), Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 2;

            // Plan 176 — campaign tenure, days only (no months/years in UI).
            var rosterEntry = _survivors?.Roster?.Find(_survivorId);
            if (rosterEntry != null)
            {
                int currentDay = AppDayProvider?.Invoke() ?? rosterEntry.joinedDay;
                int tenureDays = rosterEntry.CampaignAgeDays(currentDay);
                AddRow(_survivorInfo, TrFormat("ui.survivor.info.tenure", tenureDays, rosterEntry.joinedDay), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount++;
            }

            // ── Needs ──
            // The early guard already returned for a null session; `!` keeps the
            // nullable analysis quiet across the intervening method calls.
            var profile = _survivors!.Needs.Profile;
            AddRow(_needsList, $"{Tr("ui.survivor.need.health", "Health")}: {s.Health:0} / {s.MaxHealthCap:0}", profile.IsHealthWarn(s.Health) ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
            AddRow(_needsList, $"{Tr("ui.survivor.need.hunger", "Hunger")}: {s.Hunger:0}", profile.IsHungerCritical(s.Hunger) ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"{Tr("ui.survivor.need.thirst", "Thirst")}: {s.Thirst:0}", profile.IsThirstCritical(s.Thirst) ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"{Tr("ui.survivor.need.fatigue", "Fatigue")}: {s.Fatigue:0}", profile.IsFatigueCritical(s.Fatigue) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"{Tr("ui.survivor.need.warmth", "Warmth")}: {s.Warmth:0}", profile.IsWarmthCritical(s.Warmth) ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"{Tr("ui.survivor.need.morale", "Morale")}: {s.Morale:0}", profile.IsMoraleCritical(s.Morale) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"{Tr("ui.survivor.need.hygiene", "Hygiene")}: {s.Hygiene:0}", Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 7;

            // Plan 137 — Needs -> Performance Cascade
            var perf = Ashfall.Core.Survivors.NeedsPerformanceBridge.Project(s);
            if (perf != null && perf.OverallBand != Ashfall.Core.Survivors.PerformanceBand.Optimal)
            {
                (float r, float g, float b, float a) bandColor = perf.OverallBand switch
                {
                    Ashfall.Core.Survivors.PerformanceBand.Critical => Ashfall.Core.UI.Theme.Critical,
                    Ashfall.Core.Survivors.PerformanceBand.Severe => Ashfall.Core.UI.Theme.Critical,
                    Ashfall.Core.Survivors.PerformanceBand.Impaired => Ashfall.Core.UI.Theme.Warm,
                    _ => Ashfall.Core.UI.Theme.Lethe
                };
                string warningIcon = perf.OverallBand >= Ashfall.Core.Survivors.PerformanceBand.Severe ? "⚠ " : string.Empty;
                AddRow(_needsList, TrFormat("ui.survivor.performance.row", warningIcon,
                    perf.OverallBand.ToString().ToUpperInvariant(),
                    $"{perf.WorkSpeedMultiplier * 100:0}", $"{perf.CombatAccuracyMultiplier * 100:0}"), bandColor);
                RenderedRowCount++;
            }

            // ── Traits (from rad state) ──
            if (rad != null)
            {
                // Task 7 — three-band dose colouring. The row previously jumped
                // straight from "clean" to Critical at the warn threshold, so a
                // survivor sitting in the warn band looked the same as one at
                // zero. It now shares AshfallUiBands with the HUD chip, the
                // StatusPanel card, and the host's radiation_high toast.
                //
                // Task 11 — the colour now explains itself. Without a tooltip a
                // player sees an amber number with no way to learn that 50 mSv
                // is the warn line and 80 mSv is critical.
                var doseRow = MakeRowLabel(
                    $"{Tr("ui.survivor.trait.radiation_dose", "Radiation Dose")}: {rad.RadiationDose:0} mSv",
                    AshfallUiBands.ForDose(rad.RadiationDose));
                doseRow.TooltipText = TrFormat("ui.survivor.trait.dose_tooltip",
                    $"{Ashfall.Core.Radiation.RadiationSystem.WarnThreshold:0}",
                    $"{Ashfall.Core.Radiation.RadiationSystem.AcuteThreshold:0}");
                _traitsList.AddChild(doseRow);

                var lifetimeRow = MakeRowLabel(
                    $"{Tr("ui.survivor.trait.lifetime_exposure", "Lifetime Exposure")}: {rad.LifetimeRadiationExposure:0} mSv",
                    // Task 10 — colour the lifetime total from the shared chronic band.
                    rad.LifetimeRadiationExposure >= Ashfall.Core.Radiation.RadiationSystem.ChronicLifetimeThreshold
                        ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
                lifetimeRow.TooltipText = TrFormat("ui.survivor.trait.lifetime_tooltip",
                    $"{Ashfall.Core.Radiation.RadiationSystem.ChronicLifetimeThreshold:0}");
                _traitsList.AddChild(lifetimeRow);
                AddRow(_traitsList, $"{Tr("ui.survivor.trait.rad_resistance", "Rad Resistance")}: {rad.HasRadResistance}{(rad.HasRadResistance ? $" ({rad.RadResistanceHoursRemaining:0}h)" : "")}",
                    rad.HasRadResistance ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Dim);
                AddRow(_traitsList, $"{Tr("ui.survivor.trait.acute_sickness", "Acute Sickness")}: {rad.HasAcuteRadiationSickness}", rad.HasAcuteRadiationSickness ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Dim);
                AddRow(_traitsList, $"{Tr("ui.survivor.trait.chronic_illness", "Chronic Illness")}: {rad.HasChronicIllness}", rad.HasChronicIllness ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount += 5;
            }
            else
            {
                _traitsList.AddChild(MakeDimLine(Tr("ui.survivor.info.no_radiation", "No radiation state tracked.")));
            }

            // Plan 174 — Procedural Survivor Backstory
            var backstory = BackstoryProvider?.Invoke(s.Id);
            if (backstory != null && !string.IsNullOrEmpty(backstory.OccupationId))
            {
                AddRow(_traitsList, TrFormat("ui.survivor.trait.origin", backstory.OccupationId), Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            // ── Status ──
            AddRow(_statusList, TrFormat("ui.survivor.status.critical_flags", s.WasHungerCritical, s.WasThirstCritical, s.WasWarmthCritical),
                (s.WasHungerCritical || s.WasThirstCritical || s.WasWarmthCritical) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount++;

            var fitness = FitnessProvider?.Invoke(s.Id);
            if (fitness != null)
            {
                string fitnessText = fitness.Level == FitnessLevel.Fit
                    ? Tr("ui.survivor.fitness.fit", "FIT")
                    : fitness.Level == FitnessLevel.Impaired ? Tr("ui.survivor.fitness.impaired", "IMPAIRED") : Tr("ui.survivor.fitness.unfit", "UNFIT / INCAPACITATED");
                AddRow(_statusList, TrFormat("ui.survivor.status.fitness", fitnessText),
                    fitness.Level == FitnessLevel.Fit ? Ashfall.Core.UI.Theme.Lethe :
                    fitness.Level == FitnessLevel.Impaired ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical);
                var reasons = new List<string>();
                reasons.AddRange(fitness.BlockingReasons);
                reasons.AddRange(fitness.DegradedFactors);
                if (reasons.Count > 0)
                    AddRow(_statusList, Tr("ui.survivor.status.factors", "Factors:") + " " + string.Join(", ", reasons).Replace('_', ' '), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount += reasons.Count > 0 ? 2 : 1;
            }

            var needsSystem = _survivors?.Needs;
            int modifierDay = AppDayProvider?.Invoke() ?? needsSystem?.CurrentDay ?? -1;
            var modifiers = needsSystem?.ModifierStack.GetForSurvivor(s.Id)
                .Where(modifier => modifier.IsActive(modifierDay))
                .ToList();
            if (modifiers != null && modifiers.Count > 0)
            {
                modifiers.Sort((left, right) =>
                {
                    int magnitude = Math.Abs(right.DeltaPerHour).CompareTo(Math.Abs(left.DeltaPerHour));
                    if (magnitude != 0) return magnitude;
                    int source = string.CompareOrdinal(left.SourceId, right.SourceId);
                    return source != 0 ? source : ((int)left.Need).CompareTo((int)right.Need);
                });
                AddRow(_statusList, Tr("ui.survivor.status.top_contributors", "Top active need contributors"), Ashfall.Core.UI.Theme.Pale);
                int count = Math.Min(5, modifiers.Count);
                for (int i = 0; i < count; i++)
                {
                    var modifier = modifiers[i];
                    AddRow(_statusList,
                        "  " + TrFormat("ui.survivor.status.modifier_row", FormatModifierSource(modifier.SourceId), modifier.Need, $"{modifier.DeltaPerHour:+0.##;-0.##;0}"),
                        Ashfall.Core.UI.Theme.Dim);
                }
                if (modifiers.Count > count)
                    AddRow(_statusList, TrFormat("ui.survivor.status.other_contributors", modifiers.Count - count), Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount += count + 1 + (modifiers.Count > count ? 1 : 0);
            }

            var recent = needsSystem?.ModifierStack.GetRecentForSurvivor(s.Id);
            if (recent != null && recent.Count > 0)
            {
                AddRow(_statusList, Tr("ui.survivor.status.recent_contributors", "Recent need contributors"), Ashfall.Core.UI.Theme.Pale);
                int count = Math.Min(4, recent.Count);
                for (int i = 0; i < count; i++)
                {
                    var contribution = recent[i];
                    AddRow(_statusList,
                        "  " + TrFormat("ui.survivor.status.recent_row", FormatModifierSource(contribution.SourceId), contribution.Need, $"{contribution.DeltaPerHour:+0.##;-0.##;0}"),
                        Ashfall.Core.UI.Theme.Dim);
                }
                RenderedRowCount += count + 1;
            }
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
            => parent.AddChild(MakeRowLabel(text, col));

        /// <summary>
        /// Task 11 — builds the row label without parenting it, so a caller can
        /// attach a band tooltip before the row joins the tree.
        /// </summary>
        private static Label MakeRowLabel(string text, (float r, float g, float b, float a) col)
        {
            var label = new Label
            {
                Text = text,
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            return label;
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
            if (string.IsNullOrEmpty(id)) return Tr("ui.survivor.info.unknown", "Unknown");
            int us = id.IndexOf('_');
            return us >= 0 ? id.Substring(us + 1).Replace('_', ' ') : id;
        }

        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);

        private static string FormatModifierSource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId)) return Tr("ui.survivor.info.unknown_source", "Unknown source");
            return sourceId.Replace('.', ' ').Replace('_', ' ').Replace(':', ' ');
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
