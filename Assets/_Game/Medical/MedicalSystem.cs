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
            if (survivor.State == SurvivorState.Idle || survivor.State == SurvivorState.Working)
            {
                survivor.State = SurvivorState.Sick;
            }
            OnAfflictionGained?.Invoke(survivor, active);
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

            float hours = ComputeTreatmentHours(recipe, medic.MedicalSkill);
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

        public static float ComputeTreatmentHours(TreatmentRecipeSO recipe, float medicalSkill)
        {
            if (recipe == null) return 1f;
            float skill = Mathf.Clamp01(medicalSkill);
            // Unskilled: 1.25× base; expert (1.0): 0.5× base
            float factor = Mathf.Lerp(1.25f, 0.5f, skill);
            return Mathf.Max(0.1f, recipe.baseTreatmentHours * factor);
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
                        ActiveTreatmentRecipeId = a.ActiveTreatmentRecipeId
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
                        ActiveTreatmentRecipeId = a.ActiveTreatmentRecipeId
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
                    healthDrain: 8f, progressionHours: 12f, progressesTo: null,
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
            float lethality = 1f)
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
            return a;
        }
    }
}
