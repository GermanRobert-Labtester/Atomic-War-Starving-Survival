using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Medical triage pipeline: afflictions drain Health, progress if untreated,
    /// and are cured via TreatmentRecipeSO (+ optional medical bed). Latent tissue
    /// damage from the radiation prognosis pipeline makes infections 3× more lethal.
    ///
    /// Health need pressure from this system is the medical authority path: each
    /// Tick applies drain solely from active afflictions (and cures restore health).
    /// </summary>
    public class MedicalSystem
    {
        /// <summary>LatentDamage at/above this multiplies infection lethality.</summary>
        public const float ImmuneCollapseLatentThreshold = 30f;

        /// <summary>Infections become this many times more lethal under immune collapse.</summary>
        public const float ImmuneCollapseLethalityFactor = 3f;

        /// <summary>Shelter module id for the medical bed.</summary>
        public const string MedicalBedModuleId = "medical_bed";

        /// <summary>
        /// Module id for a generic "comfort" station (bed, book nook, etc.)
        /// used by AI comfort-cure actions. Optional; not required for the
        /// mental-break cure path itself.
        /// </summary>
        public const string ComfortStationModuleId = "comfort_station";

        /// <summary>
        /// Hours between required Caregive actions for a comatose patient.
        /// Past this window, neglect accelerates hunger/thirst/health collapse.
        /// Internal Horror — The Bedridden.
        /// </summary>
        public const float ComaCareIntervalHours = 6f;

        /// <summary>Extra health drain/hr while a coma patient is past care interval.</summary>
        public const float ComaNeglectHealthDrainPerHour = 6f;

        /// <summary>Hunger/thirst gained per hour while comatose (cannot self-feed).</summary>
        public const float ComaNeedRisePerHour = 4f;

        /// <summary>Clean water units consumed per Caregive action.</summary>
        public const int CaregiveWaterCost = 1;

        /// <summary>Optional medicine units consumed per Caregive (0 if none held).</summary>
        public const int CaregiveMedicineCost = 1;

        private readonly NeedsSystem _needs;
        private readonly Inventory.Inventory _inventory;
        private readonly Shelter.Shelter _shelter;
        private readonly Dictionary<string, AfflictionSO> _afflictions = new Dictionary<string, AfflictionSO>();
        private readonly Dictionary<string, TreatmentRecipeSO> _treatments = new Dictionary<string, TreatmentRecipeSO>();
        private readonly Dictionary<string, List<ActiveAffliction>> _bySurvivor =
            new Dictionary<string, List<ActiveAffliction>>();

        public event Action<Survivor, ActiveAffliction> OnAfflictionGained;
        public event Action<Survivor, ActiveAffliction> OnAfflictionCured;
        public event Action<Survivor, ActiveAffliction, ActiveAffliction> OnAfflictionProgressed;
        public event Action<Survivor, ActiveAffliction> OnProgressionHalted;

        /// <summary>
        /// Host hook: called when an item is consumed during a treatment
        /// (e.g. morphine, anti-rad). Injected by GameBootstrap so Medical
        /// stays free of AddictionSystem refs. Signature: (survivor, itemId, currentDay).
        /// </summary>
        public System.Action<Survivor, string, int> OnTreatmentItemConsumed;

        /// <summary>
        /// Host hook: returns the current campaign day. Injected by GameBootstrap.
        /// </summary>
        public System.Func<int> GetCurrentDay;
        public event Action OnMedicalStateChanged;

        public MedicalSystem(
            NeedsSystem needs,
            Inventory.Inventory inventory = null,
            Shelter.Shelter shelter = null)
        {
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _inventory = inventory;
            _shelter = shelter;
        }

        public void RegisterAffliction(AfflictionSO def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            _afflictions[def.id] = def;
        }

        public void RegisterTreatment(TreatmentRecipeSO recipe)
        {
            if (recipe == null || string.IsNullOrEmpty(recipe.id)) return;
            _treatments[recipe.id] = recipe;
        }

        public AfflictionSO GetAfflictionDef(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _afflictions.TryGetValue(id, out var d) ? d : null;
        }

        public TreatmentRecipeSO GetTreatment(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _treatments.TryGetValue(id, out var t) ? t : null;
        }

        public IReadOnlyList<ActiveAffliction> GetActive(Survivor survivor)
        {
            if (survivor == null || string.IsNullOrEmpty(survivor.Id))
                return Array.Empty<ActiveAffliction>();
            if (!_bySurvivor.TryGetValue(survivor.Id, out var list))
                return Array.Empty<ActiveAffliction>();
            return list;
        }

        public bool HasAffliction(Survivor survivor, string afflictionId)
        {
            var list = GetActive(survivor);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].AfflictionId == afflictionId) return true;
            }
            return false;
        }

        public bool HasAnyUntreated(Survivor survivor)
        {
            var list = GetActive(survivor);
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].ProgressionHalted && !list[i].IsTreating) return true;
            }
            return list.Count > 0;
        }

        /// <summary>Inflict an affliction. No-op if already present or def unknown.</summary>
        public bool Inflict(Survivor survivor, string afflictionId)
        {
            if (survivor == null || !survivor.IsAlive || string.IsNullOrEmpty(afflictionId)) return false;
            if (!_afflictions.TryGetValue(afflictionId, out var def)) return false;
            if (HasAffliction(survivor, afflictionId)) return false;

            if (!_bySurvivor.TryGetValue(survivor.Id, out var list))
            {
                list = new List<ActiveAffliction>();
                _bySurvivor[survivor.Id] = list;
            }

            var active = ActiveAffliction.Create(def);
            list.Add(active);
            if (afflictionId == AfflictionSO.Ids.Coma)
            {
                survivor.State = SurvivorState.Incapacitated;
                active.HoursSinceLastCare = 0f;
            }
            else if (survivor.State == SurvivorState.Idle || survivor.State == SurvivorState.Working)
            {
                survivor.State = SurvivorState.Sick;
            }
            OnAfflictionGained?.Invoke(survivor, active);
            OnMedicalStateChanged?.Invoke();
            return true;
        }

        /// <summary>True when the survivor has an active coma affliction.</summary>
        public bool IsComatose(Survivor survivor)
        {
            return HasAffliction(survivor, AfflictionSO.Ids.Coma);
        }

        /// <summary>True when a comatose patient is overdue for Caregive.</summary>
        public bool NeedsCare(Survivor patient)
        {
            if (patient == null || !patient.IsAlive) return false;
            var list = GetActive(patient);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].AfflictionId != AfflictionSO.Ids.Coma) continue;
                return list[i].HoursSinceLastCare >= ComaCareIntervalHours * 0.5f;
            }
            return false;
        }

        /// <summary>
        /// Caregive: another survivor feeds water (+ optional medicine) to a
        /// comatose patient, resetting the neglect clock. Returns false if
        /// no coma, no caregiver, or insufficient water.
        /// </summary>
        public bool TryCaregive(
            Survivor caregiver,
            Survivor patient,
            Func<string, ItemDefinition> itemLookup = null)
        {
            if (caregiver == null || !caregiver.IsAlive) return false;
            if (patient == null || !patient.IsAlive) return false;
            if (caregiver.Id == patient.Id) return false;
            if (!IsComatose(patient)) return false;
            if (!_bySurvivor.TryGetValue(patient.Id, out var list)) return false;

            ActiveAffliction coma = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].AfflictionId == AfflictionSO.Ids.Coma)
                {
                    coma = list[i];
                    break;
                }
            }
            if (coma == null) return false;

            // Water is mandatory when inventory is present.
            if (_inventory != null)
            {
                ItemDefinition water = null;
                var waterSlot = _inventory.FindSlot("clean_water")
                    ?? _inventory.FindSlot("water");
                if (waterSlot?.Item != null)
                    water = waterSlot.Item;
                if (water == null && itemLookup != null)
                    water = itemLookup("clean_water") ?? itemLookup("water");

                if (water == null || _inventory.Count(water) < CaregiveWaterCost)
                    return false;
                if (!_inventory.Remove(water, CaregiveWaterCost))
                    return false;

                // Medicine optional — consume if available, don't fail without it.
                ItemDefinition med = null;
                var medSlot = _inventory.FindSlot("antibiotics")
                    ?? _inventory.FindSlot("medicine")
                    ?? _inventory.FindSlot("bandage");
                if (medSlot?.Item != null && medSlot.Item.type == ItemType.Medical)
                    med = medSlot.Item;
                if (med != null && _inventory.Count(med) >= CaregiveMedicineCost)
                    _inventory.Remove(med, CaregiveMedicineCost);
            }

            coma.HoursSinceLastCare = 0f;

            // Mild recovery from care
            _needs.Modify(patient, NeedKind.Thirst, -25f);
            _needs.Modify(patient, NeedKind.Hunger, -15f);
            _needs.Modify(patient, NeedKind.Health, 3f);

            // Caregiver fatigue
            _needs.Modify(caregiver, NeedKind.Fatigue, 8f);

            OnMedicalStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Emergency field care: consume one halt item (e.g. bandage) and freeze
        /// progression on matching afflictions. Does not fully cure.
        /// </summary>
        public bool TryEmergencyHalt(Survivor patient, string itemId, ItemDefinition itemDef = null)
        {
            if (patient == null || !patient.IsAlive || string.IsNullOrEmpty(itemId)) return false;
            if (!_bySurvivor.TryGetValue(patient.Id, out var list) || list.Count == 0) return false;

            var targets = new List<ActiveAffliction>();
            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i];
                if (a.ProgressionHalted) continue;
                if (!_afflictions.TryGetValue(a.AfflictionId, out var def)) continue;
                if (def.emergencyHaltItemId != itemId) continue;
                targets.Add(a);
            }
            if (targets.Count == 0) return false;

            if (_inventory != null)
            {
                if (itemDef == null)
                {
                    var slot = _inventory.FindSlot(itemId);
                    if (slot?.Item == null || slot.Amount < 1) return false;
                    itemDef = slot.Item;
                }
                if (_inventory.Count(itemDef) < 1) return false;
                if (!_inventory.Remove(itemDef, 1)) return false;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var a = targets[i];
                a.ProgressionHalted = true;
                a.IsTreating = false;
                a.TreatmentHoursRemaining = 0f;
                OnProgressionHalted?.Invoke(patient, a);
            }

            OnMedicalStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Begin a full treatment recipe. Consumes ingredients (medic skill may spare
        /// secondary ones). High Medical skill shortens treatment hours.
        /// </summary>
        public bool TryStartTreatment(
            Survivor medic,
            Survivor patient,
            TreatmentRecipeSO recipe,
            Func<string, ItemDefinition> itemLookup = null)
        {
            if (medic == null || !medic.IsAlive || patient == null || !patient.IsAlive) return false;
            if (recipe == null || string.IsNullOrEmpty(recipe.targetAfflictionId)) return false;
            if (!HasAffliction(patient, recipe.targetAfflictionId)) return false;

            if (recipe.requiresMedicalBed && !HasOperationalMedicalBed())
                return false;

            if (!_bySurvivor.TryGetValue(patient.Id, out var list)) return false;
            ActiveAffliction target = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].AfflictionId == recipe.targetAfflictionId)
                {
                    target = list[i];
                    break;
                }
            }
            if (target == null || target.IsTreating) return false;

            if (!ConsumeIngredients(recipe, medic.MedicalSkill, itemLookup))
                return false;

            // Notify host (AddictionSystem) of consumed treatment items
            if (OnTreatmentItemConsumed != null && recipe.ingredients != null)
            {
                int day = GetCurrentDay != null ? GetCurrentDay() : 0;
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var ing = recipe.ingredients[i];
                    if (ing != null && ing.item != null && !string.IsNullOrEmpty(ing.item.id))
                    {
                        OnTreatmentItemConsumed(patient, ing.item.id, day);
                    }
                }
            }

            float hours = ComputeTreatmentHours(recipe, medic.MedicalSkill, medic);
            target.IsTreating = true;
            target.ProgressionHalted = true; // treatment freezes progression
            target.TreatmentHoursRemaining = hours;
            target.ActiveTreatmentRecipeId = recipe.id;

            if (recipe.requiresPatientRest)
            {
                patient.State = SurvivorState.Resting;
            }

            OnMedicalStateChanged?.Invoke();
            return true;
        }

        /// <summary>Advance affliction clocks, health drain, treatments, and progressions.</summary>
        public void Tick(IReadOnlyList<Survivor> survivors, float gameHours)
        {
            if (survivors == null || gameHours <= 0f) return;

            for (int s = 0; s < survivors.Count; s++)
            {
                var survivor = survivors[s];
                if (survivor == null || !survivor.IsAlive) continue;
                if (!_bySurvivor.TryGetValue(survivor.Id, out var list) || list.Count == 0) continue;

                // Snapshot length; progressions may append
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (!survivor.IsAlive) break;

                    var active = list[i];
                    if (!_afflictions.TryGetValue(active.AfflictionId, out var def)) continue;

                    active.HoursActive += gameHours;

                    // Coma care clock + neglect (Internal Horror — The Bedridden)
                    if (active.AfflictionId == AfflictionSO.Ids.Coma)
                    {
                        active.HoursSinceLastCare += gameHours;
                        ApplyComaNeeds(survivor, gameHours);
                        if (active.HoursSinceLastCare >= ComaCareIntervalHours)
                        {
                            // Past care window: accelerated collapse
                            _needs.Modify(survivor, NeedKind.Health,
                                -ComaNeglectHealthDrainPerHour * gameHours);
                            _needs.Modify(survivor, NeedKind.Hunger,
                                ComaNeedRisePerHour * 1.5f * gameHours);
                            _needs.Modify(survivor, NeedKind.Thirst,
                                ComaNeedRisePerHour * 1.5f * gameHours);
                        }
                        survivor.State = SurvivorState.Incapacitated;
                    }

                    // Active treatment countdown (frozen progression, still drains)
                    if (active.IsTreating)
                    {
                        ApplyHealthDrain(survivor, def, gameHours);
                        active.TreatmentHoursRemaining -= gameHours;
                        if (active.TreatmentHoursRemaining <= 0f)
                            CompleteTreatment(survivor, list, active);
                        continue;
                    }

                    // Progression before full-period drain so large ticks still honor
                    // RadBurns → Sepsis → Death instead of dying mid-stage from drain alone.
                    if (!active.ProgressionHalted
                        && def.progressionHours > 0f
                        && !string.IsNullOrEmpty(def.progressesToId)
                        && active.HoursUntilProgression <= gameHours)
                    {
                        float pre = Mathf.Max(0f, active.HoursUntilProgression);
                        float post = gameHours - pre;
                        if (pre > 0f)
                            ApplyHealthDrain(survivor, def, pre);
                        if (!survivor.IsAlive) break;

                        ProgressAffliction(survivor, list, active, def);
                        if (!survivor.IsAlive) break;

                        // Remainder of the tick under the progressed affliction (if present)
                        if (post > 0f && HasAffliction(survivor, def.progressesToId)
                            && _afflictions.TryGetValue(def.progressesToId, out var nextDef))
                        {
                            ApplyHealthDrain(survivor, nextDef, post);
                        }
                        continue;
                    }

                    if (!active.ProgressionHalted
                        && def.progressionHours > 0f
                        && !string.IsNullOrEmpty(def.progressesToId))
                    {
                        active.HoursUntilProgression -= gameHours;
                    }

                    ApplyHealthDrain(survivor, def, gameHours);
                    ApplyFatigueDrain(survivor, def, gameHours);
                    ApplyStaminaCap(survivor, def);
                }
            }
        }

        private void ApplyHealthDrain(Survivor survivor, AfflictionSO def, float hours)
        {
            if (survivor == null || def == null || hours <= 0f || !survivor.IsAlive) return;
            float drain = def.healthDrainPerHour * EffectiveLethality(survivor, def) * hours;
            if (drain > 0f)
                _needs.Modify(survivor, NeedKind.Health, -drain);
        }

        /// <summary>
        /// Comatose survivors cannot feed or water themselves — hunger/thirst rise
        /// even if the rest of the bunker is stocked.
        /// </summary>
        private void ApplyComaNeeds(Survivor survivor, float hours)
        {
            if (survivor == null || hours <= 0f || !survivor.IsAlive) return;
            _needs.Modify(survivor, NeedKind.Hunger, ComaNeedRisePerHour * hours);
            _needs.Modify(survivor, NeedKind.Thirst, ComaNeedRisePerHour * hours);
        }

        /// <summary>
        /// Apply the per-hour fatigue drain defined on the affliction
        /// (Prompt #47 — biological trade side effects). No-op for
        /// afflictions without a <c>fatigueDrainPerHour</c>. Models
        /// blood_loss and similar consequences that tax the body beyond
        /// the standard health drain.
        /// </summary>
        private void ApplyFatigueDrain(Survivor survivor, AfflictionSO def, float hours)
        {
            if (survivor == null || def == null || hours <= 0f || !survivor.IsAlive) return;
            if (def.fatigueDrainPerHour <= 0f) return;
            _needs.Modify(survivor, NeedKind.Fatigue, def.fatigueDrainPerHour * hours);
        }

        /// <summary>
        /// Apply the hard stamina cap defined on the affliction. If
        /// <c>staminaCap &gt; 0</c>, the survivor's Fatigue is clamped to
        /// the cap value (Fatigue rises with exhaustion, so a low cap
        /// means the donor cannot recover above that floor). No-op for
        /// afflictions without a cap. This is how blood_loss keeps the
        /// donor "functional but never fully rested" for ~7 days.
        /// </summary>
        private void ApplyStaminaCap(Survivor survivor, AfflictionSO def)
        {
            if (survivor == null || def == null || !survivor.IsAlive) return;
            if (def.staminaCap < 0f) return;
            float cap = Mathf.Clamp(def.staminaCap, 0f, 100f);
            if (survivor.Needs.Fatigue > cap)
            {
                // Direct set rather than Modify: Needs.Modify clamps to [0,100]
                // but we want the cap to act as a hard floor the survivor cannot
                // rest below. We bypass the normal decay by writing Fatigue
                // back down to the cap, after which further rest is suppressed.
                survivor.Needs.Fatigue = cap;
            }
        }

        public float EffectiveLethality(Survivor survivor, AfflictionSO def)
        {
            if (def == null) return 1f;
            float mult = Mathf.Max(0f, def.baseLethality);
            if (def.isInfection && survivor != null
                && survivor.LatentDamage >= ImmuneCollapseLatentThreshold)
            {
                mult *= ImmuneCollapseLethalityFactor;
            }
            // Immune-collapse affliction itself also flags the body as compromised
            if (def.isInfection && survivor != null
                && HasAffliction(survivor, AfflictionSO.Ids.ImmuneCollapse))
            {
                mult *= ImmuneCollapseLethalityFactor;
            }
            return mult;
        }

        public bool HasOperationalMedicalBed()
        {
            if (_shelter == null) return false;
            var mod = _shelter.GetModule(MedicalBedModuleId);
            return mod != null && mod.IsOperational;
        }

        public static float ComputeTreatmentHours(TreatmentRecipeSO recipe, float medicalSkill, Survivor medic = null)
        {
            if (recipe == null) return 1f;
            float skill = Mathf.Clamp01(medicalSkill);
            // Unskilled: 1.25× base; expert (1.0): 0.5× base
            float factor = Mathf.Lerp(1.25f, 0.5f, skill);
            float hours = Mathf.Max(0.1f, recipe.baseTreatmentHours * factor);
            if (medic != null && medic.HasDisability(DisabilitySO.Ids.Tremors))
            {
                hours *= 2.0f; // Reduces action speed by 50%
            }

            // Prompt #193 — High-Yield antibiotics/iodine act twice as fast
            if (recipe.ingredients != null)
            {
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var item = recipe.ingredients[i]?.item;
                    if (item != null && SurvivalPerkSystem.IsHighYieldMed(item.id))
                    {
                        hours *= SurvivalPerkSystem.HighYieldTreatmentSpeedMult;
                        break;
                    }
                }
            }
            return hours;
        }

        public MedicalSystemSave CaptureState()
        {
            var save = new MedicalSystemSave();
            var rows = new List<SurvivorAfflictionsSave>();
            foreach (var kv in _bySurvivor)
            {
                var list = kv.Value;
                if (list == null || list.Count == 0) continue;
                var row = new SurvivorAfflictionsSave
                {
                    SurvivorId = kv.Key,
                    Afflictions = new ActiveAfflictionSave[list.Count]
                };
                for (int i = 0; i < list.Count; i++)
                {
                    var a = list[i];
                    row.Afflictions[i] = new ActiveAfflictionSave
                    {
                        AfflictionId = a.AfflictionId,
                        HoursActive = a.HoursActive,
                        HoursUntilProgression = a.HoursUntilProgression,
                        ProgressionHalted = a.ProgressionHalted,
                        IsTreating = a.IsTreating,
                        TreatmentHoursRemaining = a.TreatmentHoursRemaining,
                        ActiveTreatmentRecipeId = a.ActiveTreatmentRecipeId,
                        HoursSinceLastCare = a.HoursSinceLastCare
                    };
                }
                rows.Add(row);
            }
            save.BySurvivor = rows.ToArray();
            return save;
        }

        public void RestoreState(MedicalSystemSave save)
        {
            _bySurvivor.Clear();
            if (save?.BySurvivor == null) return;
            for (int i = 0; i < save.BySurvivor.Length; i++)
            {
                var row = save.BySurvivor[i];
                if (row == null || string.IsNullOrEmpty(row.SurvivorId) || row.Afflictions == null)
                    continue;
                var list = new List<ActiveAffliction>();
                for (int j = 0; j < row.Afflictions.Length; j++)
                {
                    var a = row.Afflictions[j];
                    if (a == null || string.IsNullOrEmpty(a.AfflictionId)) continue;
                    list.Add(new ActiveAffliction
                    {
                        AfflictionId = a.AfflictionId,
                        HoursActive = a.HoursActive,
                        HoursUntilProgression = a.HoursUntilProgression,
                        ProgressionHalted = a.ProgressionHalted,
                        IsTreating = a.IsTreating,
                        TreatmentHoursRemaining = a.TreatmentHoursRemaining,
                        ActiveTreatmentRecipeId = a.ActiveTreatmentRecipeId,
                        HoursSinceLastCare = a.HoursSinceLastCare
                    });
                }
                if (list.Count > 0) _bySurvivor[row.SurvivorId] = list;
            }
            OnMedicalStateChanged?.Invoke();
        }

        /// <summary>
        /// Factory: build the default ASHFALL affliction set for bootstrap / tests.
        /// </summary>
        public static List<AfflictionSO> CreateDefaultAfflictions()
        {
            var list = new List<AfflictionSO>
            {
                MakeAffliction(AfflictionSO.Ids.Bleeding, "Bleeding", AfflictionPhase.Phase1,
                    healthDrain: 2f, progressionHours: 24f, progressesTo: AfflictionSO.Ids.Sepsis,
                    haltItem: "bandage", infection: false),
                MakeAffliction(AfflictionSO.Ids.GunshotWound, "Gunshot Wound", AfflictionPhase.Phase1,
                    healthDrain: 1.5f, progressionHours: 48f, progressesTo: AfflictionSO.Ids.Sepsis,
                    haltItem: "bandage", infection: false),
                MakeAffliction(AfflictionSO.Ids.BrokenBone, "Broken Bone", AfflictionPhase.Phase1,
                    healthDrain: 0.5f, progressionHours: 0f, progressesTo: null,
                    haltItem: "splint", infection: false, requiresBed: true),
                MakeAffliction(AfflictionSO.Ids.Dysentery, "Dysentery", AfflictionPhase.Phase1,
                    healthDrain: 1f, progressionHours: 36f, progressesTo: AfflictionSO.Ids.BacterialInfection,
                    haltItem: null, infection: true),
                MakeAffliction(AfflictionSO.Ids.BacterialInfection, "Bacterial Infection", AfflictionPhase.Phase1,
                    healthDrain: 2f, progressionHours: 24f, progressesTo: AfflictionSO.Ids.Sepsis,
                    haltItem: null, infection: true),
                MakeAffliction(AfflictionSO.Ids.Sepsis, "Sepsis", AfflictionPhase.Phase1,
                    healthDrain: 8f, progressionHours: 12f, progressesTo: AfflictionSO.Ids.Coma,
                    haltItem: null, infection: true, lethality: 1.5f),
                MakeAffliction(AfflictionSO.Ids.RadBurns, "Radiation Burns", AfflictionPhase.Phase2,
                    healthDrain: 2.5f, progressionHours: 36f, progressesTo: AfflictionSO.Ids.Sepsis,
                    haltItem: "bandage", infection: false, requiresBed: true),
                MakeAffliction(AfflictionSO.Ids.ImmuneCollapse, "Immune Collapse", AfflictionPhase.Phase2,
                    healthDrain: 0.5f, progressionHours: 0f, progressesTo: null,
                    haltItem: null, infection: false, requiresBed: true),
                MakeAffliction(AfflictionSO.Ids.HeavyMetalPoisoning, "Heavy Metal Poisoning", AfflictionPhase.Phase2,
                    healthDrain: 1.5f, progressionHours: 72f, progressesTo: AfflictionSO.Ids.Sepsis,
                    haltItem: null, infection: false, requiresBed: true),
                // Prompt #47 — BloodLoss. Faction convoy traded a pint of the
                // survivor's blood for water/iodine. The body is functional
                // but never fully rested: stamina cap 30, +2 fatigue/hr, and
                // 24h to a bacterial infection (the needle was not sterile).
                // Cure window is ~7 days of high food/water.
                MakeAffliction(AfflictionSO.Ids.BloodLoss, "Blood Loss", AfflictionPhase.Phase1,
                    healthDrain: 0.4f, progressionHours: 24f,
                    progressesTo: AfflictionSO.Ids.BacterialInfection,
                    haltItem: null, infection: false,
                    requiresBed: false, lethality: 0.5f,
                    fatigueDrain: 2f, staminaCap: 30f),
                // Internal Horror — bedridden triage. Cannot self-feed;
                // Caregive resets neglect clock. Slow base drain; neglect multiplies it.
                MakeAffliction(AfflictionSO.Ids.Coma, "Coma", AfflictionPhase.Phase1,
                    healthDrain: 0.5f, progressionHours: 0f, progressesTo: null,
                    haltItem: null, infection: false, requiresBed: true, lethality: 1f),
                // Internal Horror — botulism from rusted canned goods.
                // Respiratory paralysis; progresses to coma if untreated.
                MakeAffliction(AfflictionSO.Ids.Botulism, "Botulism", AfflictionPhase.Phase1,
                    healthDrain: 3.5f, progressionHours: 18f,
                    progressesTo: AfflictionSO.Ids.Coma,
                    haltItem: null, infection: false, requiresBed: true, lethality: 1.4f,
                    fatigueDrain: 4f, staminaCap: 20f),
                // Prompt #13 — rat poison planted in medical cache iodine foil.
                MakeAffliction(AfflictionSO.Ids.PoisonIngestion, "Poison Ingestion", AfflictionPhase.Phase1,
                    healthDrain: 6f, progressionHours: 18f,
                    progressesTo: AfflictionSO.Ids.Sepsis,
                    haltItem: "anti_toxin", infection: false, requiresBed: false, lethality: 1.4f),
            };
            return list;
        }

        public static TreatmentRecipeSO CreateGunshotBandageHaltRecipe(ItemDefinition bandage)
        {
            var r = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            r.id = "treat_gunshot_bandage";
            r.displayName = "Field Dress Gunshot";
            r.targetAfflictionId = AfflictionSO.Ids.GunshotWound;
            r.baseTreatmentHours = 0.5f;
            r.requiresMedicalBed = false;
            r.requiresPatientRest = false;
            r.haltOnly = true;
            r.healthRestoreOnCure = 5f;
            r.ingredients = new List<TreatmentIngredient>
            {
                new TreatmentIngredient { item = bandage, itemId = "bandage", amount = 1 }
            };
            return r;
        }

        public static TreatmentRecipeSO CreateGunshotFullRecipe(ItemDefinition bandage, ItemDefinition tweezers)
        {
            var r = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            r.id = "treat_gunshot_full";
            r.displayName = "Surgical Gunshot Care";
            r.targetAfflictionId = AfflictionSO.Ids.GunshotWound;
            r.baseTreatmentHours = 4f;
            r.requiresMedicalBed = true;
            r.requiresPatientRest = true;
            r.haltOnly = false;
            r.healthRestoreOnCure = 25f;
            r.ingredients = new List<TreatmentIngredient>
            {
                new TreatmentIngredient { item = bandage, itemId = "bandage", amount = 1 },
                new TreatmentIngredient { item = tweezers, itemId = "tweezers", amount = 1 }
            };
            return r;
        }

        /// <summary>
        /// Attempt to cure a broken survivor via a medical-bed intervention.
        /// The break must have <c>requiresMedicalBed == true</c> and the
        /// shelter must have a working <c>medical_bed</c> module. Returns
        /// true if the break was cured.
        /// </summary>
        public bool TryCureMentalBreak(
            Survivor patient,
            MentalBreakSystem mentalBreakSystem,
            Shelter.Shelter shelter)
        {
            if (patient == null || !patient.IsAlive) return false;
            if (mentalBreakSystem == null) return false;
            if (!patient.HasMentalBreak) return false;
            var br = mentalBreakSystem.GetBreak(patient.currentMentalBreakId);
            if (br == null) return false;
            if (!br.requiresMedicalBed) return false;
            if (shelter == null) return false;
            var medBed = shelter.GetModule(MedicalBedModuleId);
            if (medBed == null || !medBed.IsOperational) return false;

            mentalBreakSystem.Cure(patient);
            return true;
        }

        /// <summary>
        /// True if a mental break could be cured by a medical-bed
        /// intervention right now. Pure read; does not mutate state.
        /// Used by AI scoring to decide whether to send the medic in.
        /// </summary>
        public bool CanCureMentalBreak(
            Survivor patient,
            MentalBreakSystem mentalBreakSystem,
            Shelter.Shelter shelter)
        {
            if (patient == null || !patient.IsAlive) return false;
            if (mentalBreakSystem == null || shelter == null) return false;
            if (!patient.HasMentalBreak) return false;
            var br = mentalBreakSystem.GetBreak(patient.currentMentalBreakId);
            if (br == null || !br.requiresMedicalBed) return false;
            var medBed = shelter.GetModule(MedicalBedModuleId);
            return medBed != null && medBed.IsOperational;
        }

        private void CompleteTreatment(Survivor survivor, List<ActiveAffliction> list, ActiveAffliction active)
        {
            TreatmentRecipeSO recipe = null;
            if (!string.IsNullOrEmpty(active.ActiveTreatmentRecipeId))
                _treatments.TryGetValue(active.ActiveTreatmentRecipeId, out recipe);

            active.IsTreating = false;
            active.TreatmentHoursRemaining = 0f;

            if (recipe != null && recipe.haltOnly)
            {
                active.ProgressionHalted = true;
                active.ActiveTreatmentRecipeId = null;
                if (recipe.healthRestoreOnCure > 0f)
                    _needs.Modify(survivor, NeedKind.Health, recipe.healthRestoreOnCure);
                OnProgressionHalted?.Invoke(survivor, active);
                OnMedicalStateChanged?.Invoke();
                return;
            }

            EvaluateDisabilityForLongAffliction(survivor, active);
            float restore = recipe != null ? recipe.healthRestoreOnCure : 10f;
            list.Remove(active);
            if (restore > 0f)
                _needs.Modify(survivor, NeedKind.Health, restore);

            if (list.Count == 0 && survivor.State == SurvivorState.Sick)
                survivor.State = SurvivorState.Idle;

            OnAfflictionCured?.Invoke(survivor, active);
            OnMedicalStateChanged?.Invoke();
        }

        private void ProgressAffliction(
            Survivor survivor,
            List<ActiveAffliction> list,
            ActiveAffliction active,
            AfflictionSO def)
        {
            EvaluateDisabilityForLongAffliction(survivor, active);
            string nextId = def.progressesToId;
            list.Remove(active);

            // Sepsis with no further progression: lethal cascade via massive drain already;
            // if sepsis itself "progresses" with empty id, force death when health already low.
            if (string.IsNullOrEmpty(nextId))
            {
                if (def.id == AfflictionSO.Ids.Sepsis)
                {
                    _needs.Modify(survivor, NeedKind.Health, -100f);
                }
                OnMedicalStateChanged?.Invoke();
                return;
            }

            if (!_afflictions.TryGetValue(nextId, out var nextDef))
            {
                OnMedicalStateChanged?.Invoke();
                return;
            }

            // Replace with worse state (avoid duplicates)
            if (!HasAffliction(survivor, nextId))
            {
                var next = ActiveAffliction.Create(nextDef);
                list.Add(next);
                OnAfflictionProgressed?.Invoke(survivor, active, next);
                OnAfflictionGained?.Invoke(survivor, next);
            }
            else
            {
                OnAfflictionProgressed?.Invoke(survivor, active, null);
            }
            OnMedicalStateChanged?.Invoke();
        }

        private void EvaluateDisabilityForLongAffliction(Survivor survivor, ActiveAffliction active)
        {
            if (survivor == null || active == null) return;
            if (active.HoursActive > 72f)
            {
                string disabilityId = GetDisabilityForAffliction(active.AfflictionId);
                if (!string.IsNullOrEmpty(disabilityId) && !survivor.HasDisability(disabilityId))
                {
                    survivor.DisabilityIds.Add(disabilityId);
                    if (disabilityId == DisabilitySO.Ids.ScarredLungs)
                    {
                        _needs.Modify(survivor, NeedKind.Health, 0f); // Clamps health to MaxHealthCap (75)
                    }
                }
            }
        }

        private static string GetDisabilityForAffliction(string afflictionId)
        {
            if (string.Equals(afflictionId, AfflictionSO.Ids.Sepsis, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(afflictionId, AfflictionSO.Ids.RadBurns, StringComparison.OrdinalIgnoreCase))
            {
                return DisabilitySO.Ids.ScarredLungs;
            }
            if (string.Equals(afflictionId, AfflictionSO.Ids.BrokenBone, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(afflictionId, AfflictionSO.Ids.GunshotWound, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(afflictionId, AfflictionSO.Ids.Bleeding, StringComparison.OrdinalIgnoreCase))
            {
                return DisabilitySO.Ids.Limp;
            }
            if (string.Equals(afflictionId, AfflictionSO.Ids.Dysentery, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(afflictionId, AfflictionSO.Ids.BacterialInfection, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(afflictionId, AfflictionSO.Ids.HeavyMetalPoisoning, StringComparison.OrdinalIgnoreCase))
            {
                return DisabilitySO.Ids.Tremors;
            }
            return DisabilitySO.Ids.ScarredLungs;
        }

        private bool ConsumeIngredients(
            TreatmentRecipeSO recipe,
            float medicalSkill,
            Func<string, ItemDefinition> itemLookup)
        {
            if (recipe.ingredients == null || recipe.ingredients.Count == 0) return true;
            if (_inventory == null) return true; // tests may omit inventory

            // Validate first
            var resolved = new List<(ItemDefinition item, int amount, bool secondary)>();
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ing = recipe.ingredients[i];
                if (ing == null || ing.amount <= 0) continue;
                var item = ing.item;
                if (item == null && itemLookup != null && !string.IsNullOrEmpty(ing.ResolvedId))
                    item = itemLookup(ing.ResolvedId);
                if (item == null)
                {
                    var slot = _inventory.FindSlot(ing.ResolvedId);
                    item = slot?.Item;
                }
                if (item == null || _inventory.Count(item) < ing.amount) return false;
                resolved.Add((item, ing.amount, i > 0));
            }

            float skill = Mathf.Clamp01(medicalSkill);
            for (int i = 0; i < resolved.Count; i++)
            {
                var (item, amount, secondary) = resolved[i];
                // High skill may spare secondary ingredients (not the primary dressing)
                if (secondary && skill > 0.6f && UnityEngine.Random.value < skill * 0.4f)
                    continue;
                if (!_inventory.Remove(item, amount)) return false;
            }
            return true;
        }

        private static AfflictionSO MakeAffliction(
            string id,
            string name,
            AfflictionPhase phase,
            float healthDrain,
            float progressionHours,
            string progressesTo,
            string haltItem,
            bool infection,
            bool requiresBed = false,
            float lethality = 1f,
            float fatigueDrain = 0f,
            float staminaCap = -1f)
        {
            var a = ScriptableObject.CreateInstance<AfflictionSO>();
            a.id = id;
            a.displayName = name;
            a.description = name;
            a.phase = phase;
            a.healthDrainPerHour = healthDrain;
            a.progressionHours = progressionHours;
            a.progressesToId = progressesTo ?? "";
            a.emergencyHaltItemId = haltItem ?? "";
            a.isInfection = infection;
            a.requiresMedicalBed = requiresBed;
            a.baseLethality = lethality;
            // Prompt #47 — biological trade side effects. Default 0/-1 keeps
            // every existing affliction backward-compatible.
            a.fatigueDrainPerHour = fatigueDrain;
            a.staminaCap = staminaCap;
            return a;
        }
    }
}
