// GameBootstrap.Quests.cs — wire the 7 "new content" quests (QuestRegistry)
// into the host. Follows the same pattern as GameBootstrap.IntoTheAsh.cs:
// construct at boot, inject host callbacks as Func/Action delegates,
// subscribe quest events, register save state, and drive trigger conditions
// from a day-gated registry tick ("quest_registry").
using System;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Morally-weighted narrative quests (faction / personal / shelter).
        /// Sibling of <see cref="ExpansionQuests"/>: that system owns the
        /// flag-driven chain track; this registry owns the 7 new quests.
        /// Save state is registered under the "quest_registry" save id.
        /// </summary>
        public QuestRegistry QuestRegistry { get; private set; }

        // ── Trigger day gates (daily trigger tick) ──────────────────────
        private const int MilitiaGrainWarStartDay = 8;   // a runner arrives
        private const int CultGlowCommunionStartDay = 10; // the invitation
        private const int GarrisonLastOrderStartDay = 12; // military frequency noticed
        /// <summary>DeepWell triggers when the water situation is failing.</summary>
        private const float DeepWellWaterFillTrigger = 0.25f;

        // quest_deep_well stage 3 material bill (per quest spec).
        private const int DeepWellConcretePatchMix = 8;
        private const int DeepWellCopperTubing = 6;
        private const int DeepWellBearingSets = 2;
        private const int DeepWellGeneratorParts = 1;
        /// <summary>Noise attracts one guaranteed raid on this excavation day.</summary>
        private const float DeepWellRaidOnExcavationDay = 4f;
        private const float DeepWellRaidStrength = 40f;

        // ─────────────────────────────────────────────────────────────────
        // Boot (called once from InitSaveAndExpeditions, after SaveSystem,
        // EconomySystem, MedicalSystem and CraftingSystem all exist).
        // ─────────────────────────────────────────────────────────────────

        private void BootQuestRegistry()
        {
            QuestRegistry = new QuestRegistry();

            foreach (var quest in QuestRegistry.All)
                WireQuestHostCallbacks(quest);

            // Cult leash (Expansion II): the communion quest drives missed-
            // communion pressure through the canonical wiring helper.
            FactionPressureWiring.AttachCultQuest(
                QuestRegistry.Get<Quest_CultGlowCommunion>(QuestRegistry.IdCultGlowCommunion));

            WireQuestRegistryEvents();
            WireQuestSystemBridges();
            WireRepairGasketCraftEffect();

            // Save/load — generic ISaveable slot, no SaveSystem DTO changes.
            SaveSystem?.RegisterSaveable(QuestRegistry, "quest_registry",
                q => q.CaptureState(),
                (q, o) => q.RestoreState((QuestRegistry.State)o));

            GameLog.Log("[GameBootstrap] QuestRegistry booted: 7 quests wired.");
        }

        // ── Host callbacks (shared by all 7 quests) ─────────────────────

        private void WireQuestHostCallbacks(QuestRuntime quest)
        {
            if (quest == null) return;

            quest.GetDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0f;
            // No quest consumes GetInt yet; reserved for quest-side counters.
            quest.GetInt = key => 0;

            // Quest files use story-facing faction ids ("faction_garrison",
            // "faction_survivors"); the economy tracks canonical FactionSO ids.
            quest.AddFactionTrust = (factionId, delta) =>
                EconomySystem?.ModifyTrust(MapQuestFactionId(factionId), delta);
            // Subtract* passes an already-negative delta (see quest tests).
            quest.SubtractFactionTrust = (factionId, delta) =>
                EconomySystem?.ModifyTrust(MapQuestFactionId(factionId), delta);

            quest.BroadcastRadioMessage = (factionId, message, confidence) =>
            {
                if (FactionRadioIntercepts == null || string.IsNullOrEmpty(message)) return;
                // Reuse HatchRepel for one-liner pressure/news lines — same
                // convention as FactionPressureWiring (no new InterceptKind).
                FactionRadioIntercepts.Push(
                    MapQuestFactionId(factionId),
                    FactionRadioInterceptSystem.InterceptKind.HatchRepel,
                    message,
                    TimeSystem != null ? TimeSystem.CurrentDay : 0);
            };

            quest.ApplyMorale = (survivorId, delta) =>
            {
                if (NeedsSystem == null) return;
                if (survivorId == null)
                {
                    // null = the whole roster (e.g. the communion's shared high).
                    if (Survivors == null) return;
                    for (int i = 0; i < Survivors.Count; i++)
                        if (Survivors[i] != null && Survivors[i].IsAlive)
                            NeedsSystem.Modify(Survivors[i], NeedKind.Morale, delta);
                    return;
                }
                var target = FindQuestSurvivor(survivorId);
                if (target != null) NeedsSystem.Modify(target, NeedKind.Morale, delta);
            };

            quest.ApplyRadiationDose = (survivorId, millisieverts) =>
            {
                if (RadiationSystem == null) return;
                if (survivorId == null)
                {
                    if (Survivors == null) return;
                    for (int i = 0; i < Survivors.Count; i++)
                        if (Survivors[i] != null && Survivors[i].IsAlive)
                            RadiationSystem.Expose(Survivors[i], millisieverts, 1f);
                    return;
                }
                var target = FindQuestSurvivor(survivorId);
                if (target != null) RadiationSystem.Expose(target, millisieverts, 1f);
            };

            quest.AddAffliction = (survivorId, afflictionId) =>
            {
                var target = FindQuestSurvivor(survivorId);
                if (target != null) MedicalSystem?.Inflict(target, afflictionId);
            };
            quest.RemoveAffliction = (survivorId, afflictionId) =>
            {
                var target = FindQuestSurvivor(survivorId);
                if (target != null) MedicalSystem?.CureOutright(target, afflictionId);
            };

            quest.GrantPerk = (survivorId, perkId, level) =>
            {
                var target = FindQuestSurvivor(survivorId ?? quest.Def?.OwnerSurvivorId);
                if (target == null || SkillProgression == null || string.IsNullOrEmpty(perkId)) return;
                EnsureQuestPerkRegistered(perkId);
                SkillProgression.TryGrantPerk(target, perkId,
                    TimeSystem != null ? TimeSystem.CurrentDay : 0);
            };

            quest.GiveItem = (survivorId, itemId, count) =>
            {
                if (Inventory == null || string.IsNullOrEmpty(itemId) || count <= 0) return;
                var def = _itemCatalog != null ? _itemCatalog.GetById(itemId) : null;
                if (def == null)
                {
                    GameLog.LogWarning($"[Quests] GiveItem skipped — unknown item id '{itemId}'.");
                    return;
                }
                Inventory.Add(def, count);
            };
            quest.TakeItem = (survivorId, itemId, count) =>
            {
                if (Inventory == null || string.IsNullOrEmpty(itemId) || count <= 0) return;
                Inventory.RemoveById(itemId, count);
            };

            // No location-destruction system exists; record it as a world
            // flag so map/scavenging code and saves can observe the change.
            quest.MarkLocationDestroyed = (locationId, kind) =>
            {
                if (string.IsNullOrEmpty(locationId)) return;
                SaveSystem?.SetWorldFlag("location_destroyed:" + locationId, true);
                GameLog.Log($"[Quests] Location destroyed ({kind}): {locationId}");
            };

            quest.KillSurvivor = survivorId =>
            {
                var target = FindQuestSurvivor(survivorId);
                if (target == null)
                {
                    GameLog.LogWarning("[Quests] KillSurvivor skipped — no survivor id supplied by quest.");
                    return;
                }
                SurvivorNeedWrite.Kill(target);
            };
            // No capture/prisoner system exists; world flags carry the state.
            quest.CaptureSurvivor = survivorId =>
            {
                if (string.IsNullOrEmpty(survivorId)) return;
                SaveSystem?.SetWorldFlag("survivor_captured:" + survivorId, true);
            };
            quest.ReleaseSurvivor = survivorId =>
            {
                if (string.IsNullOrEmpty(survivorId)) return;
                SaveSystem?.SetWorldFlag("survivor_captured:" + survivorId, false);
            };

            quest.RecordMoralEntry = text =>
            {
                if (string.IsNullOrEmpty(text)) return;
                EventBus.Raise(new MoralChronicleEntryRequested(
                    TimeSystem != null ? TimeSystem.CurrentDay : 0,
                    text,
                    MoralChronicleEntryKind.FactionChoice));
            };

            quest.TriggerRaidSoon = (factionId, hours) =>
            {
                if (EconomySystem == null) return;
                // "Raid soon" runs through the canonical trust-threshold
                // cascade: forcing trust below the faction's raid threshold
                // makes ModifyTrust fire TryLaunchRaid on its own path.
                EconomySystem.ModifyTrust(MapQuestFactionId(factionId), -100f);
            };
        }

        /// <summary>
        /// Quest files speak in story-facing faction ids; map them to the
        /// canonical <see cref="FactionSO.Ids"/> the economy registers.
        /// Unknown ids pass through unchanged (ModifyTrust tolerates them).
        /// </summary>
        private static string MapQuestFactionId(string factionId)
        {
            switch (factionId)
            {
                case "faction_garrison": return FactionSO.Ids.MilitaryRemnants;
                case "faction_militia":
                case "faction_upland_militia": return FactionSO.Ids.UplandMilitia;
                case "faction_cult_of_the_glow": return FactionSO.Ids.CultOfTheGlow;
                case "faction_survivors": return FactionSO.Ids.SafeHavenCommunity;
                default: return factionId;
            }
        }

        /// <summary>
        /// Quest owner ids use a "survivor_" prefix ("survivor_elena_vasquez")
        /// while roster entries carry the archetype id ("elena_vasquez").
        /// Resolve across both conventions; null/empty never matches.
        /// </summary>
        private Survivor FindQuestSurvivor(string id)
        {
            if (string.IsNullOrEmpty(id) || Survivors == null) return null;
            const string prefix = "survivor_";
            string archetype = id.StartsWith(prefix, StringComparison.Ordinal)
                ? id.Substring(prefix.Length) : null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null) continue;
                if (sv.Id == id || sv.ArchetypeId == id) return sv;
                if (archetype != null && sv.ArchetypeId == archetype) return sv;
            }
            return null;
        }

        /// <summary>
        /// Quest reward perks (perk_field_triage, perk_medic_apprentice,
        /// perk_garden_tender) have no catalog assets; register runtime defs
        /// so SkillProgression.TryGrantPerk can award them.
        /// </summary>
        private void EnsureQuestPerkRegistered(string perkId)
        {
            if (SkillProgression == null || string.IsNullOrEmpty(perkId)) return;
            if (SkillProgression.GetPerk(perkId) != null) return;
            var perk = ScriptableObject.CreateInstance<PerkSO>();
            perk.id = perkId;
            perk.displayName = perkId;
            perk.description = perkId;
            perk.disciplineId = "";
            perk.xpThreshold = 999999f; // milestone-only: never earned via action XP
            perk.skillBonus = 0.15f;
            perk.isExpertPerk = false;
            SkillProgression.RegisterPerk(perk);
        }

        // ── Quest events → log + world flags ────────────────────────────

        private void WireQuestRegistryEvents()
        {
            Action<QuestRuntimeState> onStateChanged = state =>
            {
                if (state == null) return;
                GameLog.Log($"[Quest] {state.QuestId}: stage {state.Stage}, {state.Status}.");
                if (state.Status == QuestStatus.Success)
                    SaveSystem?.SetWorldFlag(QuestFlags.QuestCompletedPrefix + state.QuestId, true);
            };
            QuestRegistry.OnQuestStateChanged += onStateChanged;
            _subscriptions.Track(() => QuestRegistry.OnQuestStateChanged -= onStateChanged);
        }

        // ── Bridges into live systems (raid lock, triage counters) ──────

        private void WireQuestSystemBridges()
        {
            // The Rifle Will Not Leave: a raid during the quest locks it —
            // the child picks the rifle back up permanently.
            if (EconomySystem != null)
            {
                Action<FactionRaidResult> onRaidResolved = result =>
                {
                    if (result == null || !result.Launched) return;
                    var rifle = QuestRegistry?.Get<Quest_ChildSoldierRifle>(QuestRegistry.IdChildSoldierRifle);
                    if (rifle?.State != null && rifle.State.Status == QuestStatus.InProgress)
                        rifle.OnRaidDuringQuest();
                };
                EconomySystem.OnRaidResolved += onRaidResolved;
                _subscriptions.Track(() => EconomySystem.OnRaidResolved -= onRaidResolved);
            }

            // The Hands That Don't Shake: cures count up, deaths count
            // against Elena. "Under her care" is approximated by quest
            // activity — the bunker has a single medic archetype.
            if (MedicalSystem != null)
            {
                Action<Survivor, ActiveAffliction> onCured = (sv, affliction) =>
                {
                    var triage = QuestRegistry?.Get<Quest_ElenaTriage>(QuestRegistry.IdElenaTriage);
                    if (triage?.State != null && triage.State.Status == QuestStatus.InProgress)
                        triage.RecordTreatmentSuccess(Quest_ElenaTriage.Owner);
                };
                MedicalSystem.OnAfflictionCured += onCured;
                _subscriptions.Track(() => MedicalSystem.OnAfflictionCured -= onCured);
            }
            if (NeedsSystem != null)
            {
                Action<Survivor> onDied = sv =>
                {
                    var triage = QuestRegistry?.Get<Quest_ElenaTriage>(QuestRegistry.IdElenaTriage);
                    if (triage?.State == null || triage.State.Status != QuestStatus.InProgress) return;
                    var elena = FindQuestSurvivor(Quest_ElenaTriage.Owner);
                    if (elena != null && ReferenceEquals(sv, elena)) return; // her own death ends it elsewhere
                    triage.RecordPatientDiedUnderCare(Quest_ElenaTriage.Owner);
                };
                NeedsSystem.OnDied += onDied;
                _subscriptions.Track(() => NeedsSystem.OnDied -= onDied);
            }
        }

        // ── repair_gasket craft → hatch seal integrity ──────────────────
        // The recipe consumes its own gasket ingredients up front and
        // produces no item; its Spec declares the hatch_seal_integrity
        // effect. Calling RepairHatchSeal() here would charge a second
        // (different) rubber_gasket item, so the completion applies the
        // spec's EffectAmount through ShelterDegradation's effect path.
        private void WireRepairGasketCraftEffect()
        {
            if (CraftingSystem == null) return;
            float amount = 0.15f;
            var specs = NewRecipesCatalog.BuildAll();
            for (int i = 0; i < specs.Count; i++)
            {
                if (specs[i].Id == NewRecipesCatalog.Ids.RepairGasket
                    && specs[i].EffectKey == "hatch_seal_integrity")
                {
                    amount = specs[i].EffectAmount;
                    break;
                }
            }
            Action<Recipe> onCraftCompleted = recipe =>
            {
                if (recipe == null || recipe.id != NewRecipesCatalog.Ids.RepairGasket) return;
                if (ShelterDegradation != null && ShelterDegradation.ApplyHatchSealPatch(amount))
                    GameLog.Log("[Quests] repair_gasket: hatch seal integrity +" + amount.ToString("0.##"));
            };
            CraftingSystem.OnCraftCompleted += onCraftCompleted;
            _subscriptions.Track(() => CraftingSystem.OnCraftCompleted -= onCraftCompleted);
        }

        // ── Daily trigger tick (registry key "quest_registry") ──────────

        private void TickQuestTriggersDaily(int day)
        {
            if (QuestRegistry == null) return;

            // Faction quests — day-gated arrivals.
            TryStartQuest(QuestRegistry.IdMilitiaGrainWar, day,
                day >= MilitiaGrainWarStartDay);
            TryStartQuest(QuestRegistry.IdCultGlowCommunion, day,
                day >= CultGlowCommunionStartDay);
            TryStartQuest(QuestRegistry.IdGarrisonLastOrder, day,
                day >= GarrisonLastOrderStartDay);

            // Personal quests — start when the owner is on the roster.
            TryStartQuest(QuestRegistry.IdElenaTriage, day,
                FindQuestSurvivor(Quest_ElenaTriage.Owner) != null);
            TryStartQuest(QuestRegistry.IdMechanicHighwayHeart, day,
                FindQuestSurvivor(Quest_MechanicHighwayHeart.Owner) != null);
            TryStartQuest(QuestRegistry.IdChildSoldierRifle, day,
                FindQuestSurvivor(Quest_ChildSoldierRifle.Owner) != null);

            // Shelter quest — the water is failing.
            TryStartQuest(QuestRegistry.IdDeepWell, day,
                Inventory != null && Inventory.WaterFillRatio() < DeepWellWaterFillTrigger);

            TickChildSoldierRifleDaily();
            TickDeepWellDaily(day);
        }

        private void TryStartQuest(string questId, int day, bool condition)
        {
            if (!condition) return;
            var quest = QuestRegistry.Get(questId);
            if (quest?.State != null && quest.State.Status != QuestStatus.NotStarted) return;
            QuestRegistry.Start(questId, day);
        }

        /// <summary>
        /// The Rifle Will Not Leave: a therapist (or the child) working the
        /// conversation advances the talk track once per day; stage 2 counts
        /// voluntary disarmament days the same way.
        /// </summary>
        private void TickChildSoldierRifleDaily()
        {
            var rifle = QuestRegistry.Get<Quest_ChildSoldierRifle>(QuestRegistry.IdChildSoldierRifle);
            if (rifle?.State == null || rifle.State.Status != QuestStatus.InProgress) return;

            if (rifle.State.Stage == 1)
            {
                var therapist = FindQuestSurvivor("survivor_the_therapist");
                if (therapist != null) rifle.RecordTalkDay(therapist.Id);
            }
            else if (rifle.State.Stage == 2)
            {
                rifle.RecordDisarmamentDay();
            }
        }

        /// <summary>
        /// Water From Below: stages 1–2 are survey work (one day each);
        /// stage 3 waits until the material bill is in the inventory and
        /// consumes it; stage 4 is the 8-day excavation with one guaranteed
        /// raid from the noise.
        /// </summary>
        private void TickDeepWellDaily(int day)
        {
            var well = QuestRegistry.Get<Quest_DeepWell>(QuestRegistry.IdDeepWell);
            if (well?.State == null || well.State.Status != QuestStatus.InProgress) return;

            switch (well.State.Stage)
            {
                case 1:
                case 2:
                    well.ResolveProjectApproved();
                    break;
                case 3:
                    if (!HasDeepWellMaterials()) break;
                    ConsumeDeepWellMaterials();
                    well.ResolveProjectApproved();
                    break;
                case 4:
                    well.RecordExcavationDay();
                    // Noise attracts attention: one raid mid-dig, driven
                    // straight through the hatch-defense resolution path.
                    if (well.GetProgress(Quest_DeepWell.DaysKey) >= DeepWellRaidOnExcavationDay
                        && well.GetProgress(Quest_DeepWell.RaidGuaranteedKey) < 0.5f)
                    {
                        well.SetProgress(Quest_DeepWell.RaidGuaranteedKey, 1f);
                        HatchDefenseSystem?.ResolveFactionRaid(
                            FactionSO.Ids.ScavengerCamp, DeepWellRaidStrength, day,
                            ignoreDayGate: true);
                    }
                    break;
            }
        }

        private bool HasDeepWellMaterials()
        {
            if (Inventory == null) return false;
            return Inventory.CountById("concrete_patch_mix") >= DeepWellConcretePatchMix
                && Inventory.CountById("copper_tubing_1m") >= DeepWellCopperTubing
                && Inventory.CountById("bearing_set_industrial") >= DeepWellBearingSets
                && Inventory.CountById("generator_parts") >= DeepWellGeneratorParts;
        }

        private void ConsumeDeepWellMaterials()
        {
            Inventory.RemoveById("concrete_patch_mix", DeepWellConcretePatchMix);
            Inventory.RemoveById("copper_tubing_1m", DeepWellCopperTubing);
            Inventory.RemoveById("bearing_set_industrial", DeepWellBearingSets);
            Inventory.RemoveById("generator_parts", DeepWellGeneratorParts);
        }
    }
}
