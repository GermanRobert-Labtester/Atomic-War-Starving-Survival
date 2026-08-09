using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Medical milestone perks (Prompts #201–#205).
    /// Earned through Phase-2 cures, raid bandaging, Manifest radiation care,
    /// amputation, and coma revival — not XP grind.
    /// Plain C#, save/load safe. Inventory-free (Survivors asmdef has no Inventory ref).
    /// </summary>
    public class MedicalPerkSystem
    {
        // ── Perk ids ─────────────────────────────────────────────────────
        /// <summary>
        /// Milestone Steady Hands (earned via Phase-2 cures, not XP).
        /// Renamed away from "perk_steady_hands": that id collided with the
        /// XP-threshold medical *expert* perk registered in
        /// <see cref="SkillProgressionSystem.RegisterDefaultPerks"/>, and the
        /// later registration silently overwrote it — leaving medical-track
        /// experts with no XP-earnable expert perk at all.
        /// </summary>
        public const string SteadyHandsId = "perk_steady_hands_field";

        /// <summary>
        /// Pre-migration id for the milestone perk, still present in saves
        /// written before the collision fix. See
        /// <see cref="SkillProgressionSystem.MigrateLegacySteadyHands"/>.
        /// </summary>
        public const string LegacySteadyHandsId = "perk_steady_hands";
        public const string TriageUnderFireId = "perk_triage_under_fire";
        public const string RadiologistId = "perk_radiologist";
        public const string AnatomistId = "perk_anatomist";
        public const string ParamedicId = "perk_paramedic";

        // ── Thresholds ───────────────────────────────────────────────────
        public const int Phase2CuresForSteadyHands = 3;

        // ── Effect constants ─────────────────────────────────────────────
        /// <summary>Surgical actions take 30% less time (×0.7 duration).</summary>
        public const float SteadyHandsSurgeryDurationMult = 0.70f;

        /// <summary>Base chance of accidental Bleeding on a surgical completion.</summary>
        public const float BaseSurgeryBleedChance = 0.20f;

        /// <summary>Default PhantomPain daily chance when Anatomist did not operate.</summary>
        public const float DefaultPhantomPainDailyChance = 0.25f;

        /// <summary>Death's Door window at 0 HP before true death (hours).</summary>
        public const float DeathsDoorHours = 2f;

        /// <summary>HP restored by adrenaline on Death's Door.</summary>
        public const float AdrenalineReviveHealth = 1f;

        public const string MedicalKitItemId = "medical_kit";
        public const string AdrenalineItemId = "adrenaline";
        public const string BandageItemId = "bandage";
        public const string SepsisAfflictionId = "sepsis";
        public const string ComaAfflictionId = "coma";
        public const string BleedingAfflictionId = "bleeding";

        private SkillProgressionSystem _progression;
        private PersonalQuestSystem _personalQuests;
        private readonly Dictionary<string, MedicalCounters> _bySurvivor =
            new Dictionary<string, MedicalCounters>();

        /// <summary>Patients amputated by an Anatomist — no PhantomPain rolls.</summary>
        private readonly HashSet<string> _cleanAmputees = new HashSet<string>();

        /// <summary>Survivor id → hours remaining on Death's Door.</summary>
        private readonly Dictionary<string, float> _deathsDoor =
            new Dictionary<string, float>();

        /// <summary>Medics currently performing a medical Utility AI action.</summary>
        private readonly HashSet<string> _activeMedicalActors = new HashSet<string>();

        /// <summary>Optional living-survivor list for colony-wide Paramedic checks.</summary>
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        public event Action<Survivor, string> OnMedicalPerkEarned;
        public event Action<Survivor, string, int> OnMilestoneProgress;
        public event Action<Survivor> OnDeathsDoorEntered;
        public event Action<Survivor> OnDeathsDoorExpired;
        public event Action<Survivor, Survivor> OnDeathsDoorRevived; // patient, medic

        public void Bind(
            SkillProgressionSystem progression,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _progression = progression;
            _getSurvivors = getSurvivors;
            _progression?.RegisterMedicalPerks();
        }

        public void RegisterCatalog() => _progression?.RegisterMedicalPerks();

        public void SetSurvivorProvider(Func<IReadOnlyList<Survivor>> getSurvivors) =>
            _getSurvivors = getSurvivors;

        /// <summary>Prompt #214–#215 — latent expert traits (Miracle Worker surgery).</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        // ── Queries ──────────────────────────────────────────────────────

        public bool Has(string survivorId, string perkId)
        {
            if (_progression == null || string.IsNullOrEmpty(survivorId)) return false;
            return _progression.HasActivePerk(survivorId, perkId);
        }

        public bool Has(Survivor sv, string perkId) =>
            sv != null && Has(sv.Id, perkId);

        public MedicalCounters GetCounters(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new MedicalCounters();
            return GetOrCreate(survivorId).Clone();
        }

        public bool AnyLivingHas(IReadOnlyList<Survivor> survivors, string perkId)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive && Has(sv, perkId))
                    return true;
            }
            return false;
        }

        private bool ColonyHasParamedic()
        {
            var list = _getSurvivors?.Invoke();
            return AnyLivingHas(list, ParamedicId);
        }

        // ── #201 Steady Hands ────────────────────────────────────────────

        /// <summary>
        /// Record a successful cure. Phase-2 afflictions (and Sepsis) count
        /// toward Steady Hands after 3 cures by the treating medic.
        /// </summary>
        public void RecordPhase2Cure(
            Survivor medic,
            string afflictionId,
            bool isPhase2,
            int currentDay = 0)
        {
            if (medic == null || !medic.IsAlive) return;
            if (!IsPhase2Cure(afflictionId, isPhase2)) return;

            var c = GetOrCreate(medic.Id);
            c.Phase2Cures++;
            OnMilestoneProgress?.Invoke(medic, "phase2_cures", c.Phase2Cures);
            if (c.Phase2Cures >= Phase2CuresForSteadyHands)
                TryGrant(medic, SteadyHandsId, currentDay);

            // Prompt #215 — Surgeon quest: Phase-2 ops under extreme stress (Morale &lt; 30).
            _personalQuests?.RecordStressPhase2Operation(medic, currentDay);
        }

        public static bool IsPhase2Cure(string afflictionId, bool isPhase2)
        {
            if (isPhase2) return true;
            // Prompt #201 lists Sepsis as the Phase-2 example even if catalog phase differs.
            return string.Equals(afflictionId, SepsisAfflictionId, StringComparison.OrdinalIgnoreCase);
        }

        public bool HasSteadyHands(Survivor sv) => Has(sv, SteadyHandsId);

        /// <summary>
        /// Duration multiplier for surgical actions.
        /// Steady Hands → 0.7; Miracle Worker (#215) → 0.5; stacked when both.
        /// </summary>
        public float GetSurgeryDurationMultiplier(Survivor medic)
        {
            float m = HasSteadyHands(medic) ? SteadyHandsSurgeryDurationMult : 1f;
            if (_personalQuests != null)
                m *= _personalQuests.GetSurgeryDurationMultiplier(medic);
            return m;
        }

        /// <summary>Prompt #215 — Miracle Worker: surgical tools not consumed.</summary>
        public bool ConsumesSurgicalTools(Survivor medic) =>
            _personalQuests == null || _personalQuests.ConsumesSurgicalTools(medic);

        /// <summary>Prompt #215 — Miracle Worker: cure ARS without chelation.</summary>
        public bool CanCureArsWithoutChelation(Survivor medic) =>
            _personalQuests != null && _personalQuests.CanCureArsWithoutChelation(medic);

        /// <summary>
        /// Chance (0..1) of accidental Bleeding after a surgical action.
        /// Steady Hands → 0.
        /// </summary>
        public float GetSurgeryBleedChance(Survivor medic) =>
            HasSteadyHands(medic) ? 0f : BaseSurgeryBleedChance;

        /// <summary>
        /// Roll accidental surgery bleed. Returns true if Bleeding should be inflicted.
        /// </summary>
        public bool RollSurgeryBleed(Survivor medic, System.Random rng = null)
        {
            float chance = GetSurgeryBleedChance(medic);
            if (chance <= 0f) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("medicalperksystem");
            return rng.NextDouble() < chance;
        }

        // ── #202 Triage Under Fire ───────────────────────────────────────

        /// <summary>
        /// Bandaging while a HatchBreach raid window is active grants the perk.
        /// </summary>
        public void RecordBandageDuringRaid(Survivor medic, bool raidActive, int currentDay = 0)
        {
            if (medic == null || !medic.IsAlive || !raidActive) return;
            TryGrant(medic, TriageUnderFireId, currentDay);
        }

        public bool HasTriageUnderFire(Survivor sv) => Has(sv, TriageUnderFireId);

        /// <summary>
        /// Medical Utility AI actions by this medic are not interrupted by hostile damage
        /// during expeditions or raids.
        /// </summary>
        public bool IsMedicalActionUninterruptible(Survivor medic) =>
            HasTriageUnderFire(medic);

        public void BeginMedicalAction(Survivor medic)
        {
            if (medic == null || string.IsNullOrEmpty(medic.Id)) return;
            _activeMedicalActors.Add(medic.Id);
        }

        public void EndMedicalAction(Survivor medic)
        {
            if (medic == null || string.IsNullOrEmpty(medic.Id)) return;
            _activeMedicalActors.Remove(medic.Id);
        }

        public bool IsPerformingMedicalAction(Survivor medic) =>
            medic != null && _activeMedicalActors.Contains(medic.Id);

        /// <summary>
        /// When hostile damage lands on a medic mid-medical-action, return true if the
        /// action should be interrupted. Triage Under Fire → never interrupt.
        /// </summary>
        public bool ShouldInterruptMedicalActionByDamage(Survivor medic)
        {
            if (medic == null || !IsPerformingMedicalAction(medic)) return false;
            return !IsMedicalActionUninterruptible(medic);
        }

        // ── #203 Radiologist ─────────────────────────────────────────────

        /// <summary>
        /// Treating a survivor currently in Manifest radiation sickness earns Radiologist.
        /// </summary>
        public void RecordManifestRadiationTreatment(Survivor medic, Survivor patient, int currentDay = 0)
        {
            if (medic == null || !medic.IsAlive || patient == null) return;
            if (patient.PrognosisStage != PrognosisStage.Manifest) return;
            TryGrant(medic, RadiologistId, currentDay);
        }

        public bool HasRadiologist(Survivor sv) => Has(sv, RadiologistId);

        /// <summary>
        /// Radiologist can read LatentDamage + OnsetTimer without consuming a MedicalKit.
        /// </summary>
        public bool CanReadLatentWithoutKit(Survivor examiner) => HasRadiologist(examiner);

        /// <summary>
        /// Reveal hidden LatentDamage / OnsetTimer.
        /// Radiologist: free look (gums/eyes). Others: host must pass tryConsumeKit that
        /// consumes one medical_kit (return true if consumed).
        /// </summary>
        public bool TryRevealLatentDamage(
            Survivor examiner,
            Survivor patient,
            Func<bool> tryConsumeKit,
            out float latentDamage,
            out float onsetTimer)
        {
            latentDamage = 0f;
            onsetTimer = 0f;
            if (examiner == null || !examiner.IsAlive || patient == null) return false;

            if (!CanReadLatentWithoutKit(examiner))
            {
                if (tryConsumeKit == null || !tryConsumeKit()) return false;
            }

            latentDamage = patient.LatentDamage;
            onsetTimer = patient.OnsetTimer;
            return true;
        }

        // ── #204 Anatomist ───────────────────────────────────────────────

        /// <summary>Performing an amputation earns Anatomist for the surgeon.</summary>
        public void RecordAmputation(Survivor surgeon, Survivor patient, int currentDay = 0)
        {
            if (surgeon == null || !surgeon.IsAlive) return;
            TryGrant(surgeon, AnatomistId, currentDay);

            // If surgeon already has (or just earned) Anatomist, mark patient clean.
            if (Has(surgeon, AnatomistId) && patient != null && !string.IsNullOrEmpty(patient.Id))
                _cleanAmputees.Add(patient.Id);
        }

        public bool HasAnatomist(Survivor sv) => Has(sv, AnatomistId);

        /// <summary>True when this amputee never develops PhantomPain (Anatomist surgery).</summary>
        public bool SuppressesPhantomPain(string patientId) =>
            !string.IsNullOrEmpty(patientId) && _cleanAmputees.Contains(patientId);

        public float GetPhantomPainDailyChance(string patientId) =>
            SuppressesPhantomPain(patientId) ? 0f : DefaultPhantomPainDailyChance;

        // ── #205 Paramedic ───────────────────────────────────────────────

        /// <summary>Reviving a survivor from Coma (cure) earns Paramedic for the medic.</summary>
        public void RecordComaRevive(Survivor medic, int currentDay = 0)
        {
            if (medic == null || !medic.IsAlive) return;
            TryGrant(medic, ParamedicId, currentDay);
        }

        public bool HasParamedic(Survivor sv) => Has(sv, ParamedicId);

        public bool IsOnDeathsDoor(Survivor sv) =>
            sv != null && _deathsDoor.ContainsKey(sv.Id);

        public float GetDeathsDoorHoursRemaining(Survivor sv)
        {
            if (sv == null || !_deathsDoor.TryGetValue(sv.Id, out float h)) return 0f;
            return h;
        }

        /// <summary>
        /// NeedsSystem death gate: if colony has a Paramedic, enter Death's Door
        /// instead of dying. Returns true when death is deferred.
        /// </summary>
        public bool TryEnterDeathsDoor(Survivor sv)
        {
            if (sv == null) return false;
            // Already dead — nothing to defer.
            if (sv.State == SurvivorState.Dead && !IsOnDeathsDoor(sv)) return false;

            if (IsOnDeathsDoor(sv))
            {
                // Already on the door — keep deferring until timer expires (Tick forces death).
                return _deathsDoor.TryGetValue(sv.Id, out float h) && h > 0f;
            }

            if (!ColonyHasParamedic()) return false;

            _deathsDoor[sv.Id] = DeathsDoorHours;
            if (sv.State != SurvivorState.Dead)
                sv.State = SurvivorState.Incapacitated;
            // Keep Health at 0 — they are at Death's Door.
            sv.Needs.Health = 0f;
            OnDeathsDoorEntered?.Invoke(sv);
            return true;
        }

        /// <summary>Advance Death's Door timers; expire → true death.</summary>
        public void TickDeathsDoor(
            float gameHours,
            IReadOnlyList<Survivor> survivors,
            Action<Survivor> forceDeath = null)
        {
            if (gameHours <= 0f || _deathsDoor.Count == 0) return;

            var expired = new List<string>();
            var keys = new List<string>(_deathsDoor.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string id = keys[i];
                _deathsDoor[id] = Mathf.Max(0f, _deathsDoor[id] - gameHours);
                if (_deathsDoor[id] <= 0f)
                    expired.Add(id);
            }

            for (int i = 0; i < expired.Count; i++)
            {
                string id = expired[i];
                _deathsDoor.Remove(id);
                Survivor sv = FindSurvivor(survivors, id);
                if (sv == null) continue;
                OnDeathsDoorExpired?.Invoke(sv);
                if (forceDeath != null)
                    forceDeath(sv);
                else if (sv.State != SurvivorState.Dead)
                {
                    sv.State = SurvivorState.Dead;
                    sv.Needs.Health = 0f;
                }
            }
        }

        /// <summary>
        /// Only a Paramedic can drag someone off Death's Door with adrenaline → 1 HP.
        /// Host supplies tryConsumeAdrenaline (return true if one unit consumed).
        /// </summary>
        public bool TryAdministerAdrenaline(
            Survivor medic,
            Survivor patient,
            Func<bool> tryConsumeAdrenaline = null)
        {
            if (medic == null || !medic.IsAlive || patient == null) return false;
            if (!HasParamedic(medic)) return false;
            if (!IsOnDeathsDoor(patient)) return false;

            if (tryConsumeAdrenaline != null && !tryConsumeAdrenaline())
                return false;

            _deathsDoor.Remove(patient.Id);
            // DEATH-004: the survivor is at Death's Door (Health=0, State=Incapacitated).
            // The pre-fix code wrote `patient.Needs.Health = 1f` directly, which
            // was a zombie-state write. The new SurvivorNeedWrite.SetHealth refuses
            // writes to non-alive survivors unless forceRevive is true. The
            // IsAlive check below fails for a Death's-Door patient (they are
            // Incapacitated, not Alive), so we need forceRevive: true. We then
            // set State=Idle so the patient is officially alive again before
            // any subsequent Modify/EvaluateDeath runs.
            SurvivorNeedWrite.SetHealth(patient, AdrenalineReviveHealth, -1f, null, forceRevive: true);
            if (patient.State == SurvivorState.Incapacitated || patient.State == SurvivorState.Dead)
                patient.State = SurvivorState.Idle;

            OnDeathsDoorRevived?.Invoke(patient, medic);
            return true;
        }

        // ── Grant / storage ──────────────────────────────────────────────

        private bool TryGrant(Survivor sv, string perkId, int currentDay)
        {
            if (_progression == null || sv == null) return false;
            if (_progression.HasActivePerk(sv.Id, perkId)
                || _progression.HasDormantPerk(sv.Id, perkId))
                return false;

            bool granted = _progression.TryGrantPerk(sv, perkId, currentDay);
            if (granted)
                OnMedicalPerkEarned?.Invoke(sv, perkId);
            return granted;
        }

        private MedicalCounters GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var c))
            {
                c = new MedicalCounters();
                _bySurvivor[survivorId] = c;
            }
            return c;
        }

        private static Survivor FindSurvivor(IReadOnlyList<Survivor> list, string id)
        {
            if (list == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].Id == id)
                    return list[i];
            }
            return null;
        }

        public MedicalPerkSave CaptureState()
        {
            var save = new MedicalPerkSave
            {
                Entries = new List<MedicalCounterSave>(),
                CleanAmputeeIds = new List<string>(),
                DeathsDoor = new List<DeathsDoorSave>()
            };
            foreach (var kv in _bySurvivor)
            {
                save.Entries.Add(new MedicalCounterSave
                {
                    SurvivorId = kv.Key,
                    Phase2Cures = kv.Value.Phase2Cures
                });
            }
            foreach (var id in _cleanAmputees)
                save.CleanAmputeeIds.Add(id);
            foreach (var kv in _deathsDoor)
            {
                save.DeathsDoor.Add(new DeathsDoorSave
                {
                    SurvivorId = kv.Key,
                    HoursRemaining = kv.Value
                });
            }
            return save;
        }

        public void RestoreState(MedicalPerkSave save)
        {
            _bySurvivor.Clear();
            _cleanAmputees.Clear();
            _deathsDoor.Clear();
            _activeMedicalActors.Clear();
            if (save == null) return;

            if (save.Entries != null)
            {
                for (int i = 0; i < save.Entries.Count; i++)
                {
                    var e = save.Entries[i];
                    if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                    _bySurvivor[e.SurvivorId] = new MedicalCounters
                    {
                        Phase2Cures = e.Phase2Cures
                    };
                }
            }

            if (save.CleanAmputeeIds != null)
            {
                for (int i = 0; i < save.CleanAmputeeIds.Count; i++)
                {
                    if (!string.IsNullOrEmpty(save.CleanAmputeeIds[i]))
                        _cleanAmputees.Add(save.CleanAmputeeIds[i]);
                }
            }

            if (save.DeathsDoor != null)
            {
                for (int i = 0; i < save.DeathsDoor.Count; i++)
                {
                    var d = save.DeathsDoor[i];
                    if (d == null || string.IsNullOrEmpty(d.SurvivorId)) continue;
                    _deathsDoor[d.SurvivorId] = d.HoursRemaining;
                }
            }
        }

        public sealed class MedicalCounters
        {
            public int Phase2Cures;

            public MedicalCounters Clone() => new MedicalCounters
            {
                Phase2Cures = Phase2Cures
            };
        }
    }

    [Serializable]
    public class MedicalPerkSave
    {
        public List<MedicalCounterSave> Entries = new List<MedicalCounterSave>();
        public List<string> CleanAmputeeIds = new List<string>();
        public List<DeathsDoorSave> DeathsDoor = new List<DeathsDoorSave>();
    }

    [Serializable]
    public class MedicalCounterSave
    {
        public string SurvivorId;
        public int Phase2Cures;
    }

    [Serializable]
    public class DeathsDoorSave
    {
        public string SurvivorId;
        public float HoursRemaining;
    }
}
