using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

using AtomicWar._Game.Endgame;

using AtomicWar._Game.Encounters;

using AtomicWar._Game.World;

using AtomicWar._Game.Narrative;

using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// CoreFamilies bulk wiring: Capture/Restore smoke + SaveSystem slot registration
    /// for remaining Action/Item/NPC/Affliction/Visitor/... systems.
    /// </summary>
    [TestFixture]
    public class CoreFamiliesWiringTests
    {
        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_corefam_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// Reflection-based Capture/Restore round-trip check. A bare
        /// Assert.IsNotNull(save) only proves CaptureState() didn't return null —
        /// it does not prove RestoreState() actually restores anything, so a
        /// no-op or partially-broken RestoreState would still pass it. This
        /// mutates each named field on the captured DTO to a distinguishable
        /// non-default value, restores it into a second fresh instance,
        /// re-captures, and asserts the named fields survived the round trip.
        /// Field names are the DTO's own public fields that the class's
        /// RestoreState is confirmed (by source inspection) to actually read;
        /// fields never consumed by RestoreState (e.g. hardcoded id tags
        /// re-stamped on every capture) are intentionally not passed in.
        /// </summary>
        private static void AssertRoundTrips<TSource, TSave>(
            Func<TSource> makeSource,
            Func<TSource, TSave> capture,
            Action<TSource, TSave> restore,
            params string[] fieldsToVerify)
            where TSave : class
        {
            var a = makeSource();
            var save = capture(a);
            Assert.IsNotNull(save, typeof(TSource).Name + ".CaptureState() returned null");

            var saveType = typeof(TSave);
            var mutations = new List<(FieldInfo field, object expected)>();
            foreach (var name in fieldsToVerify)
            {
                var field = saveType.GetField(name, BindingFlags.Public | BindingFlags.Instance);
                if (field == null) continue;
                if (!TryMutate(field.FieldType, field.GetValue(save), out object newValue)) continue;
                try
                {
                    field.SetValue(save, newValue);
                    mutations.Add((field, newValue));
                }
                catch (FieldAccessException) { /* readonly at the CLR level; skip */ }
            }

            var b = makeSource();
            restore(b, save);
            var save2 = capture(b);
            Assert.IsNotNull(save2, typeof(TSource).Name + ".CaptureState() returned null after restore");

            foreach (var (field, expected) in mutations)
            {
                object actual = field.GetValue(save2);
                Assert.AreEqual(expected, actual,
                    $"{typeof(TSource).Name}.RestoreState did not round-trip field '{field.Name}'");
            }
        }

        /// <summary>Best-effort distinguishable mutation for a field's runtime type. Returns
        /// false (and does nothing) for types this helper does not know how to compare
        /// generically (collections, nested reference types, etc.) so callers can skip
        /// verifying those fields rather than risk a false-positive assertion.</summary>
        private static bool TryMutate(Type t, object current, out object result)
        {
            if (t == typeof(string))
            {
                result = ((current as string) ?? string.Empty) + "_rt_check";
                return true;
            }
            if (t == typeof(int)) { result = ((int)current) + 12345; return true; }
            if (t == typeof(float)) { result = ((float)current) + 12345.5f; return true; }
            if (t == typeof(double)) { result = ((double)current) + 12345.5d; return true; }
            if (t == typeof(bool)) { result = !(bool)current; return true; }
            if (t.IsEnum)
            {
                foreach (var v in Enum.GetValues(t))
                {
                    if (!v.Equals(current)) { result = v; return true; }
                }
            }
            result = null;
            return false;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
            var ss = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = weather,
                TemperatureSystem = temp,
                NeedsSystem = needs,
                RadiationSystem = rad,
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = dir
            });
            wire(ss);
            return ss;
        }

        [Test]
        public void Action_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Action_AdministerPlacebo(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "clean_water_used", "discovered", "discovery_chance", "success_count", "survivor_id");
            AssertRoundTrips(() => new Action_BarricadeDoor(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "barricadedRoomIds", "barricaderIds", "requiresCrowbarToBreak");
            AssertRoundTrips(() => new Action_BoilBatteries(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "chargeRestored", "canOnlyDoOnce");
            AssertRoundTrips(() => new Action_BroadcastPropaganda(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "targetFactionId", "isFactionRemovedFromPool", "removalDaysRemaining");
            AssertRoundTrips(() => new Action_BurnCharcoal(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "coConcentration", "requiresSealedBarrel");
            AssertRoundTrips(() => new Action_BuryTimeCapsule(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "action_id", "item_id", "item_data", "is_buried", "capsule_location", "retrieved_in_new_game");
            AssertRoundTrips(() => new Action_CallCaravan(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "powerCostWatts", "arrivalTimeHours", "isCaravanEnRoute", "isCaravanKilledEnRoute");
            AssertRoundTrips(() => new Action_CoverTracks(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "IsActive");
            AssertRoundTrips(() => new Action_CrackMainframe(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hoursElapsed", "isCompleted", "isRunning", "serverFried");
            AssertRoundTrips(() => new Action_Decrypt(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "displayName", "hoursRequired", "intelligenceThreshold", "successChance", "isDecoding");
            AssertRoundTrips(() => new Action_DemandTribute(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "securityThreshold", "armoryThreshold", "tributeFoodPerDay", "tributeWaterPerDay", "hasVassals");
            AssertRoundTrips(() => new Action_EstablishRoute(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "routeEstablished", "patrolSurvivorIds", "ammoPerDay", "moneyPerDay");
            AssertRoundTrips(() => new Action_Exile(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "survivorId", "backpackGiven", "foodGiven", "weaponGiven", "exileDay", "somberClosureTriggered", "executed");
            // Action_Fish.RestoreState is an intentional no-op (dormant ghost, not
            // Boot/Save wired) -- no field survives a round trip, so none are verified.
            AssertRoundTrips(() => new Action_Fish(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            AssertRoundTrips(() => new Action_HarvestOrgans(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "moralePenalty", "organTradeValue", "requiresSurgeon", "hasBeenUsed");
            AssertRoundTrips(() => new Action_InfectSelf(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "curesRadiationSickness", "maxHealthCap", "isInfected", "infectedSurvivorId");
            AssertRoundTrips(() => new Action_IsotopeTrace(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "action_id", "requires_pristine_geiger", "reveals_safe_paths");
            AssertRoundTrips(() => new Action_Mercy(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "displayName", "morphineCost", "somberClosureMoraleBuff");
            AssertRoundTrips(() => new Action_MixCement(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "HoursRemaining", "IsCured", "IsCuring", "IsWet");
            AssertRoundTrips(() => new Action_MixChems(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "durationHours", "cardiacArrestChance", "isActive", "hoursRemaining", "hasMorphine", "hasAdrenaline", "hasAntiRad");
            AssertRoundTrips(() => new Action_Overwatch(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "ambushReduction", "rangeNodes", "stationedNodeId", "stationedSniperId");
            AssertRoundTrips(() => new Action_PhysicalTherapy(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "days_completed", "efficiency_percent", "is_therapy_active", "limb_type", "survivor_id");
            AssertRoundTrips(() => new Action_PirateRadio(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "displayName", "requiresVinylRecords", "moraleBoostAllies", "raiderCombatReduction", "broadcastDurationHours", "hoursRemaining", "isBroadcasting");
            AssertRoundTrips(() => new Action_PlaceBait(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "hoursUntilSpawn", "isPlaced");
            AssertRoundTrips(() => new Action_PullTooth(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "has_pliers", "has_whiskey", "survivor_id", "tooth_pulled");
            AssertRoundTrips(() => new Action_RigCorpse(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "requiresGrenade", "karmaPenalty", "trapDamage", "passiveLootChance", "isTrapPlaced", "riggedNodeId");
            AssertRoundTrips(() => new Action_RoutePower(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "action_id", "breakers_required", "copper_per_breaker", "breakers_repaired", "elevator_activated");
            AssertRoundTrips(() => new Action_Sabotage(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "targetFactionId", "isMissionSuccessful", "isAgentCaughtAndKilled", "globalRaidLevelOverride");
            AssertRoundTrips(() => new Action_ScorchedEarth(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "isAvailable", "requiresRaidInProgress", "requiresNoAmmo", "killsAllInside", "destroysAllLoot");
            AssertRoundTrips(() => new Action_SealRoom(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "cementCost");
            AssertRoundTrips(() => new Action_SelfSurgery(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "hoursRequired", "deathChance", "isAloneRequired", "hasBeenAttempted", "hoursElapsed");
            // Action_SilentTakedown.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new Action_SilentTakedown(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            AssertRoundTrips(() => new Action_SiphonGas(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "hoursRequired", "poisoningChance", "fuelYieldUnits", "poisoningAffliction");
            AssertRoundTrips(() => new Action_StabilizeDNA(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "requiresImmunosuppressants", "requiresSurgeon");
            // Action_Stargazing.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new Action_Stargazing(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            AssertRoundTrips(() => new Action_WorshipIdol(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hoursSpentList", "idolType", "moraleGenerated", "productivityLoss", "roomId", "worshippingSurvivorIds");
        }

        [Test]
        public void Affliction_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Affliction_AdrenalineCrash(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "afflictionId", "staminaDropToZero", "restRequiredHours", "hoursRemaining");
            AssertRoundTrips(() => new AmnesiaSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Affliction_Brainwashed("affliction_brainwashed"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "survivorId", "hoursOfPropaganda", "brainwashThresholdHours", "isBrainwashed", "defectChance", "lastFrequencyId");
            AssertRoundTrips(() => new BrittleBonesSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new CaveMadnessSystem("affliction_cave_madness"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "daysBelowLevel4", "depthThresholdDays", "isMad", "moraleDrainPerDay", "survivorId");
            AssertRoundTrips(() => new FeralRegressionSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new ImaginaryFriendSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Affliction_NerveDamage(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "survivorId", "isDamaged", "firearmAccuracyPenalty", "surgeryDisabled", "craftingDisabled", "triggerCause");
            AssertRoundTrips(() => new Affliction_OldAge(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "afflictionId", "bedriddenDayCounts", "bedriddenSurvivorIds", "dailyStatLoss", "dayThreshold", "isBedridden", "passedSurvivorIds", "trackedSurvivorIds");
            AssertRoundTrips(() => new Affliction_PhantomLimb(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "episode_active", "episode_hours_remaining", "episodes_per_day", "morphine_hours_remaining", "morphine_suppressed", "survivor_id", "total_episodes");
            AssertRoundTrips(() => new Affliction_RadHallucinations(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "affliction_id", "stage_threshold", "fake_loot_count");
            var afflictionRadiationBlindness = new RadiationBlindnessSystem();
            var afflictionRadiationBlindnessSave = afflictionRadiationBlindness.CaptureState();
            Assert.IsNotNull(afflictionRadiationBlindnessSave);
            afflictionRadiationBlindness.RestoreState(afflictionRadiationBlindnessSave);
            Assert.IsNotNull(afflictionRadiationBlindness.CaptureState());
            AssertRoundTrips(() => new Affliction_ScurvyDegeneration(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "survivorId", "daysSinceScurvy", "degenerationThresholdDays", "isDegenerating", "bleedingFromScars");
            var afflictionSporeLung = new SporeLungSystem();
            var afflictionSporeLungSave = afflictionSporeLung.CaptureState();
            Assert.IsNotNull(afflictionSporeLungSave);
            afflictionSporeLung.RestoreState(afflictionSporeLungSave);
            Assert.IsNotNull(afflictionSporeLung.CaptureState());
            AssertRoundTrips(() => new Affliction_Sterile(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "afflictionId", "isPermanent", "radThreshold", "sterileSurvivorIds");
            AssertRoundTrips(() => new SurvivorsGuiltSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Affliction_TBI(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "head_trauma_count", "severity", "speech_slur", "survivor_id");
            AssertRoundTrips(() => new Affliction_ThyroidCancer(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "affliction_id", "max_stamina_cap", "max_health_cap", "is_progressing", "diagnosed_survivor_ids", "halted_survivor_ids");
            var afflictionTrenchFoot = new TrenchFootSystem();
            var afflictionTrenchFootSave = afflictionTrenchFoot.CaptureState();
            Assert.IsNotNull(afflictionTrenchFootSave);
            afflictionTrenchFoot.RestoreState(afflictionTrenchFootSave);
            Assert.IsNotNull(afflictionTrenchFoot.CaptureState());
        }

        [Test]
        public void AudioEvent_All_CaptureRestore()
        {
            AssertRoundTrips(() => new AudioEvent_Deafening(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "durationMinutes", "isActive");
            AssertRoundTrips(() => new AudioEvent_Heartbeat(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "healthThreshold", "isActive");
        }

        [Test]
        public void Combat_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Combat_BleedOut(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "mechanicId", "turnsUntilDeath");
            AssertRoundTrips(() => new Combat_Flanking(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "flankingDamageBonus", "mechanicId", "positionKeys", "positionLanes");
            AssertRoundTrips(() => new Combat_Suppression(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "accuracyDropToZero", "durationTurns", "mechanicId", "pinnedEnemyIds");
        }

        [Test]
        public void CombatStance_All_CaptureRestore()
        {
            AssertRoundTrips(() => new CombatStance_LastStand(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "stanceId", "accuracyMultiplier", "damageMultiplier", "canFlee", "deathIsInstant");
        }

        [Test]
        public void Crisis_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Crisis_FeralFlora(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "crisisId", "isOvergrown", "daysSinceLastHarvest", "overgrowthThresholdDays", "airVentClogPercent", "plantHealthPool", "requiresMachete");
            AssertRoundTrips(() => new Crisis_StructuralFailure(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "crisisId", "daysAtZeroIntegrity", "breachThresholdDays", "isBreached", "shieldingPermanentlyZeroed", "evacuationRequired");
        }

        [Test]
        public void Durability_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Durability_Suppressor(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "IsBroken", "MaxShotsRolled", "ShotsRemaining");
        }

        [Test]
        public void Endgame_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Endgame_Ultimatum(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "dominantFactionId", "daysRemaining", "isUltimatumActive", "isGameWon", "isGameLost", "selectedEnding");
        }

        [Test]
        public void Hazard_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Hazard_CookOff(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cookOffChance", "durabilityThreshold", "hazardId");
            AssertRoundTrips(() => new Hazard_ExplosiveCrafting(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "craftingId", "fatigueThreshold", "skillThreshold", "detonationChance", "isActive");
            AssertRoundTrips(() => new Hazard_FriendlyFire(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hazardId", "friendlyFireChance", "anxietyThreshold");
            AssertRoundTrips(() => new MethaneSystem("hazard_methane"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "breachChance", "hazardId", "isDetonated", "isGasPresent");
            AssertRoundTrips(() => new Hazard_MimicCrate(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hazard_id", "perception_threshold", "explosion_damage", "detected_crate_ids", "exploded_survivor_ids", "destroyed_loot_crate_ids");
            AssertRoundTrips(() => new Hazard_SurgicalBotch(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "botch_chance", "complication_affliction", "last_botch_surgery", "second_surgery_difficulty");
            AssertRoundTrips(() => new Hazard_WeaponBurst(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hazardId", "burstChance");
        }

        [Test]
        public void HiddenStat_All_CaptureRestore()
        {
            AssertRoundTrips(() => new HiddenStat_Unseen(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "statId", "currentLevel", "peakThreshold", "risePerDarkRoom");
        }

        [Test]
        public void Item_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Item_AICoreData(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "installedCoreType", "isCoreInstalled", "lecturedSurvivorIds", "lockedRoomIds");
            AssertRoundTrips(() => new Item_AmmoTypes(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemIdAP", "itemIdHP", "itemIdStandard");
            AssertRoundTrips(() => new Item_Ammonia(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isStoredInBox", "toxicGasCloudId");
            AssertRoundTrips(() => new Item_Amphetamines(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "durationHoursRemaining", "actionSpeedMultiplier", "isFatigueLockedAtZero", "heartAttackRiskPerStormHour");
            AssertRoundTrips(() => new Item_AshGhillie(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "stealthBonus", "fireVulnerability", "burnDamageOnIgnition", "isEquipped", "durability");
            AssertRoundTrips(() => new Item_AutoDoc(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "powerDrainActive", "surgerySuccessRate", "isInstalled", "requiresMedicalBed");
            AssertRoundTrips(() => new Item_BioPlastic(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "aestheticAuraPenalty", "replacesPlasticScrap");
            AssertRoundTrips(() => new Item_BloodBag(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isSpoiled", "hoursWithoutPower");
            AssertRoundTrips(() => new Item_BoneSaw(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "requiresGangrene", "traumaInflicted", "infectionGuaranteed", "hoursRequired", "hasBeenUsed");
            AssertRoundTrips(() => new Item_C4(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "deafensSurvivor", "itemId", "triggersReinforcements");
            AssertRoundTrips(() => new Item_Caltrops(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "delayHours", "damageToRaiders", "maxUses", "usesRemaining", "deployedNodeId");
            AssertRoundTrips(() => new Item_CarrierBird(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "predatorEatChance", "isReleased", "isAlive", "lastMessageOrItemId");
            AssertRoundTrips(() => new Item_ChildsDrawing(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isPinnedOnWall", "parentMoraleBonus", "parentTraumaPenaltyOnChildDeath");
            AssertRoundTrips(() => new Item_Cigarettes(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "stressReliefAmount", "maxStaminaPenaltyPerSmoke", "barterValueValue");
            AssertRoundTrips(() => new Item_ClimbingGear(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isEquipped", "setupTimeHours", "rubbleStaminaMultiplier");
            AssertRoundTrips(() => new Item_Decoy(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "distractionRadius", "distractionDurationSeconds", "requiresBatteries", "requiresElectronics");
            AssertRoundTrips(() => new Item_DogTags(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "karmaGainOnReturn", "factionRepGainOnReturn", "scrapValueOnSell");
            AssertRoundTrips(() => new Item_EMPGrenade(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "bunkerPowerOutageHours", "disablesRobotics", "itemId");
            AssertRoundTrips(() => new Item_EncryptedDrive(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isDecrypted", "revealsFogOfWarAndSecretBunkers");
            AssertRoundTrips(() => new Item_EpiPen(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "hoursUntilCrash", "healthCrashMultiplier", "fatigueCrashValue");
            AssertRoundTrips(() => new Item_Exosuit(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "entombedFlags", "equippedSurvivorIds", "isLockedUp");
            AssertRoundTrips(() => new Item_FaradayPack(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isEMPShielded", "maxCarryCapacityKg");
            AssertRoundTrips(() => new Item_ForeignBook(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isTranslated", "translationDaysRequired", "daysSpentTranslating", "intelligenceThreshold", "intelNodesYielded");
            AssertRoundTrips(() => new Item_GeigerCalibrator(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isRepaired", "earlyWarningHours");
            AssertRoundTrips(() => new GlowingMushroomSystem("item_glowing_mushroom"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "chemicalScrapYield", "isHarvestable", "itemId", "lightOutput", "radiationPerHour", "roomId");
            AssertRoundTrips(() => new Item_GoldBars(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "scavengerTradeValue", "satisfiesEndgameFactionTribute");
            AssertRoundTrips(() => new Item_Guitar(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "powerRequired", "noviceSkillThreshold", "masterSkillThreshold", "noviceNoiseGenerated", "masterMoraleBonus", "curesDepressionAtMaster");
            AssertRoundTrips(() => new Item_Heirloom(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "efficiencyBonus", "equippedById", "isCreated", "itemId", "moraleBuff", "originalOwnerId", "toolType");
            AssertRoundTrips(() => new Item_IBeam(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "weight", "requiresVehicle", "requiresPortableWinch", "stackMax");
            AssertRoundTrips(() => new Item_ImpureIodine(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "radReduction", "toxicityHours", "isConsumed");
            AssertRoundTrips(() => new Item_JuggernautArmor(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "canFlee", "equippedBySurvivorId", "immuneToSmallArms", "isEquipped", "itemId", "speedMultiplier");
            AssertRoundTrips(() => new Item_KevlarVest(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "ballisticDamageReductionRatio", "radiationProtection", "coldProtection", "animalBiteProtection");
            AssertRoundTrips(() => new Item_Keycards(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "item_id_prefix", "found_card_ids", "unlocked_door_ids");
            AssertRoundTrips(() => new Item_Landmine(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "deployedNodeId", "isDeployed", "isIndiscriminate", "itemId");
            AssertRoundTrips(() => new Item_LeadApron(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "torsoRadiationProtection", "movementSpeedPenalty", "thermalProtection", "ballisticProtection");
            AssertRoundTrips(() => new Item_LiquidStitches(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "infectionChance", "minorInfectionAffliction");
            AssertRoundTrips(() => new Item_Maggots(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "cureRate", "horrorDebuff", "painDebuff", "stackMax", "currentStack");
            AssertRoundTrips(() => new Item_MilGasMask(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isAirborneAfflictionBlocked", "filterDurabilityMinutes", "isSuffocating");
            AssertRoundTrips(() => new Item_MutantGland(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "sourceCreature", "weight", "isConsumable", "stackMax");
            AssertRoundTrips(() => new Item_Nanites(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hasBeenInjected", "injectionCount");
            AssertRoundTrips(() => new Item_NightVision(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "batteryCharges", "stealthBonusRatio", "accuracyBonusRatio");
            AssertRoundTrips(() => new Item_PackMule(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "carryWeightMultiplier", "speedMultiplier", "panicChance");
            AssertRoundTrips(() => new Item_PasswordNote(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "item_id", "codes", "location_hints");
            AssertRoundTrips(() => new Item_PhotoAlbum(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "tradeValue", "stressDecayReductionRatio", "isPlacedOnDesk");
            AssertRoundTrips(() => new Item_PotassiumIodide(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "preventiveBlockRatio", "reactiveBlockRatio");
            AssertRoundTrips(() => new Item_PresidentialSeal(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "negotiationSuccessBonusRatio", "isEquipped");
            AssertRoundTrips(() => new Item_PrussianBlue(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isCraftable", "curedAfflictionId");
            AssertRoundTrips(() => new Item_RTGBattery(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "powerOutput", "radiationPerHour", "isPluggedIn", "isLeadLined", "weight");
            AssertRoundTrips(() => new Item_SeedLedger(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isDecrypted", "decryptionDaysRequired", "daysSpentDecrypting", "tradeValue", "cropUnlocked");
            AssertRoundTrips(() => new Item_ShockCollar(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "isEquipped", "captiveId", "powerRequired", "isCollarActive");
            AssertRoundTrips(() => new Item_Snowshoes(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "negatesBlizzardAshDriftPenalty", "currentDurability", "asphaltDurabilityDrainPerKm");
            AssertRoundTrips(() => new Item_SurgicalTubing(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isHighTierCraftingComponent");
            AssertRoundTrips(() => new Item_TearGas(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "stunDurationTurns", "uselessVsGasMask", "uselessVsMutants");
            AssertRoundTrips(() => new Item_TeddyBear(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "equippedChildId", "isEquippedByChild", "isDestroyedOrStolen", "mentalBreakSeverityOnLoss");
            AssertRoundTrips(() => new Item_TrashHazmat(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "radProtection", "tearChance", "isTorn");
            AssertRoundTrips(() => new Item_UndeliveredMail(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isRead", "isBurnedAsTinder", "tinderHeatDurationHours");
            AssertRoundTrips(() => new Item_VacuumTubes(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "isEMPProof", "isFragile", "isIntact", "requiredForHamRadio");
            AssertRoundTrips(() => new Item_VinylCollection(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "currentTrack", "jazzSleepBonus", "classicalCraftingBonus");
            AssertRoundTrips(() => new Item_Vitamins(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "chargesRemaining", "preventsScurvyAndListless");
            AssertRoundTrips(() => new Item_WalkieTalkie(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "batteriesRemaining", "maxBatteries", "isEquipped", "isManualControl");
            AssertRoundTrips(() => new Item_WastelandSoap(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "hygieneRestored", "chemicalBurnChance");
            AssertRoundTrips(() => new Item_WaterTabs(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "tabletCount");
            AssertRoundTrips(() => new Item_WeldingGoggles(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "negatesFlashBlindness", "negatesCataracts", "perceptionAccuracyPenalty");
            AssertRoundTrips(() => new Item_WristDosimeter(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "displayName", "slotType", "isBroken");
        }

        [Test]
        public void Location_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Location_Arcade(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "locationId", "displayName", "tokensAvailable", "childScavengersPresent", "acceptedCurrency", "tradeValuePerToken");
            AssertRoundTrips(() => new Location_SlaveMarket(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "locationId", "displayName", "medicineCostPerSurvivor", "permanentTraitsAdded");
            AssertRoundTrips(() => new Location_StrandedYacht(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "locationId", "displayName", "mercenaryCount", "mercenaryCombatPower", "luxuryItems", "moraleBonusPerItem", "nutritionValue", "isCleared");
        }

        [Test]
        public void Map_All_CaptureRestore()
        {
            AssertRoundTrips(() => new AquiferSystem("map_aquifer"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "caveInRiskIncrease", "floodingRiskIncrease", "hasPumpInstalled", "locationId", "providesInfiniteWater");
        }

        [Test]
        public void Misc_All_CaptureRestore()
        {
            var ashDriftSystem = new AshDriftSystem();
            var ashDriftSystemSave = ashDriftSystem.CaptureState();
            Assert.IsNotNull(ashDriftSystemSave);
            ashDriftSystem.RestoreState(ashDriftSystemSave);
            Assert.IsNotNull(ashDriftSystem.CaptureState());
            var burnWardSystem = new BurnWardSystem();
            var burnWardSystemSave = burnWardSystem.CaptureState();
            Assert.IsNotNull(burnWardSystemSave);
            burnWardSystem.RestoreState(burnWardSystemSave);
            Assert.IsNotNull(burnWardSystem.CaptureState());
            var cognitiveDecaySystem = new CognitiveDecaySystem();
            var cognitiveDecaySystemSave = cognitiveDecaySystem.CaptureState();
            Assert.IsNotNull(cognitiveDecaySystemSave);
            cognitiveDecaySystem.RestoreState(cognitiveDecaySystemSave);
            Assert.IsNotNull(cognitiveDecaySystem.CaptureState());
            AssertRoundTrips(() => new LightningStrikesSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hasLightningRod", "strikeChancePerStormHour", "destroyedModules");
            // LocationStateRuinSystem.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new LocationStateRuinSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            AssertRoundTrips(() => new MobileCampSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "isCampingActive", "vehicleType", "fatigueRestoredPerHour", "nightAmbushChance");
            // MoralDilemmaSystem.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new MoralDilemmaSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            // NeedleSterilizationSystem.RestoreState is an intentional no-op
            // (dormant ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new NeedleSterilizationSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            // NightScavengeSystem.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new NightScavengeSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            // ProstheticCraftingSystem.RestoreState is an intentional no-op
            // (dormant ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new ProstheticCraftingSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            var seismicVentsSystem = new SeismicVentsSystem();
            var seismicVentsSystemSave = seismicVentsSystem.CaptureState();
            Assert.IsNotNull(seismicVentsSystemSave);
            seismicVentsSystem.RestoreState(seismicVentsSystemSave);
            Assert.IsNotNull(seismicVentsSystem.CaptureState());
            var severeFrostbiteSystem = new SevereFrostbiteSystem();
            var severeFrostbiteSystemSave = severeFrostbiteSystem.CaptureState();
            Assert.IsNotNull(severeFrostbiteSystemSave);
            severeFrostbiteSystem.RestoreState(severeFrostbiteSystemSave);
            Assert.IsNotNull(severeFrostbiteSystem.CaptureState());
            var tetanusAfflictionSystem = new TetanusAfflictionSystem();
            var tetanusAfflictionSystemSave = tetanusAfflictionSystem.CaptureState();
            Assert.IsNotNull(tetanusAfflictionSystemSave);
            tetanusAfflictionSystem.RestoreState(tetanusAfflictionSystemSave);
            Assert.IsNotNull(tetanusAfflictionSystem.CaptureState());
            // hourAccumulator is intentionally excluded: RestoreState clamps it to
            // [0, 24] (a corrupted/out-of-range save must not desync the clock), so
            // the generic "+12345.5" mutation this helper applies to every float
            // field is not a valid round-trip probe for this one -- it is real
            // production behavior, not a bug.
            AssertRoundTrips(() => new TimeSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "day", "totalElapsedSeconds");
            var toothDecaySystem = new ToothDecaySystem();
            var toothDecaySystemSave = toothDecaySystem.CaptureState();
            Assert.IsNotNull(toothDecaySystemSave);
            toothDecaySystem.RestoreState(toothDecaySystemSave);
            Assert.IsNotNull(toothDecaySystem.CaptureState());
            var vehicleStrandingSystem = new VehicleStrandingSystem();
            var vehicleStrandingSystemSave = vehicleStrandingSystem.CaptureState();
            Assert.IsNotNull(vehicleStrandingSystemSave);
            vehicleStrandingSystem.RestoreState(vehicleStrandingSystemSave);
            Assert.IsNotNull(vehicleStrandingSystem.CaptureState());
            var vehicleSystem = new VehicleSystem();
            var vehicleSystemSave = vehicleSystem.CaptureState();
            Assert.IsNotNull(vehicleSystemSave);
            vehicleSystem.RestoreState(vehicleSystemSave);
            Assert.IsNotNull(vehicleSystem.CaptureState());
            var visionLossSystem = new VisionLossSystem();
            var visionLossSystemSave = visionLossSystem.CaptureState();
            Assert.IsNotNull(visionLossSystemSave);
            visionLossSystem.RestoreState(visionLossSystemSave);
            Assert.IsNotNull(visionLossSystem.CaptureState());
            var visitorRNGSystem = new VisitorRNGSystem();
            var visitorRNGSystemSave = visitorRNGSystem.CaptureState();
            Assert.IsNotNull(visitorRNGSystemSave);
            visitorRNGSystem.RestoreState(visitorRNGSystemSave);
            Assert.IsNotNull(visitorRNGSystem.CaptureState());
        }

        [Test]
        public void NPC_All_CaptureRestore()
        {
            AssertRoundTrips(() => new NPC_AddictsPassive(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "inSevereWithdrawal", "highValueTradeGoods");
            AssertRoundTrips(() => new NPC_AggroScavengers(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "warningShotFired", "isHostile", "armorRating", "weaponsEquipped");
            AssertRoundTrips(() => new NPC_AggroTrader(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isCorneredInDeadEnd", "forcedJunkItem", "forcedPricePremium", "purchaseMade", "isHostile");
            AssertRoundTrips(() => new NPC_Bandits(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isHostile", "demandedInventoryRatio", "extortionPaid");
            AssertRoundTrips(() => new NPC_BlackOps(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isHostileToEveryone", "perceptionCheckThreshold", "boobyTrapBleedDamage");
            AssertRoundTrips(() => new NPC_Broker(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "npcId", "appearsDay", "isVisible", "availableBlueprints", "priceInRaiders", "priceInGold");
            AssertRoundTrips(() => new NPC_Cannibals(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "hp", "moveSpeedMultiplier", "weaponsEquipped", "horrorMoraleDebuff");
            AssertRoundTrips(() => new NPC_ChemScientists(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isMustardGasProductionActive", "militaryGuardCount", "isSabotagedOrExecuted", "researchStolen", "rebelTrustGainOnExecution", "chemistryXpGainOnSteal");
            AssertRoundTrips(() => new NPC_CityResidents(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isPassive", "empathMoraleDropOnLooting");
            AssertRoundTrips(() => new NPC_Collaborators(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isSaved", "isExecuted", "moraleDropOnWatch", "rewardItem");
            AssertRoundTrips(() => new NPC_Conscripts(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "totalCount", "aliveCount", "isSurrendered", "fleeChance", "riflesDroppedCount", "playerMoralePenaltyOnExecution");
            AssertRoundTrips(() => new NPC_DesperateFamily(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isStarving", "charityHopeBuff", "foodRequiredForCharity", "isRobbed", "isHelped");
            AssertRoundTrips(() => new NPC_DrunksAggro(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "hp", "hasHighPainTolerance", "damageReductionFactor", "karmaLossOnDeath", "lootDrop");
            AssertRoundTrips(() => new NPC_Homeless(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isPassive", "diseaseRiskChance", "phase1Affliction", "intelNodeSold");
            AssertRoundTrips(() => new NPC_LonePsychopath(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "canFlee", "bearTrapCount", "tripwireCount", "huntProgress", "requiredHuntProgress", "isHuntedDown");
            AssertRoundTrips(() => new NPC_Looters(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isHidingInShadows", "isAttacking", "healthPercentageThreshold", "encumbranceThreshold");
            AssertRoundTrips(() => new NPC_Mercenaries(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "hireCostPreWarMoney", "isHiredToClear", "lootPenaltyRatio");
            AssertRoundTrips(() => new NPC_MilitaryPatrol(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isHostileToPlayer", "tollDemanded", "tollPaid", "foodTollRequired", "medsTollRequired", "armorRating", "suppressingFireActive");
            AssertRoundTrips(() => new NPC_PassiveScavengers(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isFled", "hourlyLootDepletionRate", "totalLootDepleted");
            AssertRoundTrips(() => new NPC_PassiveTrader(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "guardCount", "basePriceMultiplier");
            AssertRoundTrips(() => new NPC_PsychopathPair(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isSniperAlive", "isMeleeAlive", "isFrenzyActive", "damageMultiplier", "isImmuneToPain");
            AssertRoundTrips(() => new NPC_RebelMilitia(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isHostile", "requiredKarmaThreshold", "weaponsEquipped", "caughtStealing");
            AssertRoundTrips(() => new NPC_RebelModerates(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isPassive", "radioBackupCalled", "backupWaveCount", "requiredMedsForIntel");
            AssertRoundTrips(() => new NPC_RebelSnipers(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "squadSize", "currentCoverStage", "requiredCoverStages", "isMeleeRangeReached", "headshotDamage");
            AssertRoundTrips(() => new NPC_RebelZealots(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "executeOnSight", "daysSinceMilitaryTrade", "maxDaysTradeMemory");
            AssertRoundTrips(() => new NPC_Slavers(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "usesTearGas", "usesStunBatons", "isPlayerEnslaved", "destinationNodeId");
            AssertRoundTrips(() => new NPC_SpecOps(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isNightVisionEquipped", "flashbangCharges", "baseLethalityMultiplier", "canSurrender", "pristineGearLoot");
            AssertRoundTrips(() => new NPC_Survivalists(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isPassive", "lethalityRating", "wearsHazmatSuit", "acceptedTradeItems");
            AssertRoundTrips(() => new NPC_TaxCollector(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "npcId", "factionId", "taxPercentage", "isVisible", "hasArrived");
            AssertRoundTrips(() => new NPC_Terrorists(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "hp", "isSuicideVestEquipped", "hasDetonated", "detonationAoeDamage", "allLootDestroyed");
            AssertRoundTrips(() => new NPC_TheNegotiator(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isCoveredByPlayerSniper", "isPeaceBrokered", "factionRewards");
            AssertRoundTrips(() => new NPC_TheOld(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isDefenseless", "teaMoraleRestore", "storySkillXpGain", "isProtectedFromRaid");
            AssertRoundTrips(() => new NPC_TheParents(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isDoorLocked", "hasBabyInside", "isHostile", "isSneakedPast", "isKilled", "babyFoodItem");
            AssertRoundTrips(() => new NPC_TravelingCouple(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isPartner1Alive", "isPartner2Alive", "isVengeanceActive", "vengeanceMultiplier");
        }

        [Test]
        public void Node_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Node_AutomatedArmory(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "node_id", "turret_disabled_seconds", "is_disabled", "disable_timer_remaining", "last_disable_method", "survivors_shot");
            AssertRoundTrips(() => new Node_GhostShip(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "node_id", "is_discovered", "fuel_remaining", "tetanus_chance", "rooms_explored", "maze_depth", "trapped_survivors", "fuel_harvest_rate_per_hour");
            AssertRoundTrips(() => new Node_MutantHive(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "node_id", "is_discovered", "webbing_active", "speed_multiplier", "cocoons_total", "cocoons_opened", "swarm_spawn_chance", "cocoon_ids", "looted_cocoon_ids");
            AssertRoundTrips(() => new Node_PlayerBank(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "nodeId", "isEstablished", "storedItemIds", "isGuarded", "banditRaidChance");
            AssertRoundTrips(() => new Node_Sector7G(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "node_id", "is_discovered", "access_code", "code_entered", "is_unlocked", "dev_names", "loot_available");
            AssertRoundTrips(() => new Node_SporeHive(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "nodeId", "displayName", "toxicGasLevel", "podCount", "podsOpened", "lootPerPod", "sporeReleaseChance");
        }

        [Test]
        public void Pet_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Pet_FeralCat(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "petId", "pestReductionRate", "bringsDeadRats");
        }

        [Test]
        public void Project_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Project_BioReactor(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "projectId", "isBuilt", "constructionDays", "daysSpent", "powerOutput", "moraleDisgustDebuff", "biomassStored", "maxBiomassCapacity");
            AssertRoundTrips(() => new Project_DeepWell(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "projectId", "constructionDaysRequired", "daysSpent", "isComplete", "pipesRequired", "pumpsRequired", "pipesProvided", "pumpsProvided");
            AssertRoundTrips(() => new Project_Elevator(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "projectId", "isBuilt", "constructionDays", "daysSpent", "powerRequired", "negatesHaulingFatigue", "trappedSurvivorId", "o2Remaining");
            AssertRoundTrips(() => new Project_Minecart(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "projectId", "isBuilt", "constructionDays", "daysSpent", "movementSpeedMultiplier", "ramDamage");
            AssertRoundTrips(() => new Project_RadioArray(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "projectId", "isBuilt", "constructionDays", "daysSpent", "towersBuilt", "towersRequired", "tracksCaravans", "tracksRaids", "tracksWeather");
            AssertRoundTrips(() => new Project_SurfaceDome(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "projectId", "isBuilt", "constructionDays", "daysSpent", "hatchVisibilityBonus", "powerSavings", "isShattered");
        }

        [Test]
        public void ShelterEvent_All_CaptureRestore()
        {
            AssertRoundTrips(() => new ShelterEvent_CaravanAmbush(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "traderFactionId", "ammoNeededToDefend", "discountGainedRatio", "isFactionBlacklisted");
            AssertRoundTrips(() => new ShelterEvent_FalseCure(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "isBroadcastReceived", "isJourneyUndertaken", "destinationNodeId", "trapRevealed", "moralePenalty", "radHealingPromised");
            AssertRoundTrips(() => new ShelterEvent_Ransom(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "demandingFaction", "waterRansomDemand", "globalKarmaGainOnPay", "moraleDropOnRadioExecution", "isRansomPaid", "isExecutedOnRadio");
            AssertRoundTrips(() => new ShelterEvent_Refugees(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "totalRefugeeCount", "maxAdmitCapacity", "admittedRefugeeIds", "turnedAwayRefugeeIds");
            AssertRoundTrips(() => new ShelterEvent_TheMirror(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "IsActive", "IsDepressed", "LockedSurvivorId", "ResolutionPending");
            AssertRoundTrips(() => new ShelterEvent_Tribute(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "warlordFactionId", "fuelDemand", "foodDemand", "intervalDays", "lastPaidDay", "isProtectionActive", "isLevel5SiegeTriggered");
        }

        [Test]
        public void Skirmish_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Skirmish_Bandit_vs_Terror("skirmish_bandit_vs_terror"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "locationId", "isBanditRescued", "isBanditSlaughtered", "hiddenStashLocationId");
            AssertRoundTrips(() => new Skirmish_Mil_vs_Rebel("skirmish_mil_vs_rebel"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "locationId", "rebelTrustLossOnHelpMil", "militaryTrustLossOnHelpRebel", "milAmmoReward", "rebelAmmoReward", "playerIntervenedForMilitary", "playerIntervenedForRebels");
            AssertRoundTrips(() => new Skirmish_Mil_vs_Terror("skirmish_mil_vs_terror"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "locationId", "strayBulletChance", "strayBulletDamage", "combatTurnsElapsed", "hasGainedExplodedModifier");
            AssertRoundTrips(() => new Skirmish_Rebel_vs_Bandit("skirmish_rebel_vs_bandit"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "locationId", "karmaDropOnInterveneAgainstRebels", "isLootingBanditsStealing");
            AssertRoundTrips(() => new Skirmish_Rebel_vs_Terror("skirmish_rebel_vs_terror"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "locationId", "isPermanentTraderUnlocked", "traderId");
        }

        [Test]
        public void Trader_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Trader_PlagueConvoy(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "traderId", "displayName", "isCoughingVisible", "discountPercentage", "introducedPlagueAffliction");
        }

        [Test]
        public void Trait_All_CaptureRestore()
        {
            var traitAnthropophobia = new Trait_Anthropophobia();
            var traitAnthropophobiaSave = traitAnthropophobia.CaptureState();
            Assert.IsNotNull(traitAnthropophobiaSave);
            traitAnthropophobia.RestoreState(traitAnthropophobiaSave);
            Assert.IsNotNull(traitAnthropophobia.CaptureState());
            AssertRoundTrips(() => new ClairvoyantSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Trait_GenerationalTrauma(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "appliedTeenIds", "moraleCapPenalty", "traitId", "witnessedChildIds", "witnessedParentIds");
            AssertRoundTrips(() => new Trait_InheritedGenetics(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "affinityPenaltyWithPure", "parentRadThreshold", "traitId");
            AssertRoundTrips(() => new Trait_Matriarch(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "descendantsRequired", "matriarchIds", "traitId");
            AssertRoundTrips(() => new Trait_PTSD(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "hidden_under_bed", "panic_active", "panic_hours_remaining", "survivor_id", "trigger_type");
        }

        [Test]
        public void UIEvent_All_CaptureRestore()
        {
            AssertRoundTrips(() => new UIEvent_BlurredVision(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "toxicityThreshold", "feverThreshold", "blurIntensity");
            AssertRoundTrips(() => new UIEvent_CorruptionScare(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "displayDurationSeconds", "isActive");
            AssertRoundTrips(() => new UIEvent_FalseInventory(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "paranoiaThreshold", "flickerChance");
            AssertRoundTrips(() => new UIEvent_GhostRadio(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId");
            AssertRoundTrips(() => new UIEvent_Hacking(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "event_id", "vault_id", "max_tries", "tries_remaining", "is_permanently_locked", "is_unlocked", "correct_word", "word_pool", "revealed_duds");
            AssertRoundTrips(() => new UIEvent_LowPower(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "batteryThreshold", "isGlitching");
            AssertRoundTrips(() => new UIEvent_MapRot(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "rottedNodeIds", "visitedDays", "visitedNodeIds");
            AssertRoundTrips(() => new PhantomBlipSystem(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "isActive", "phantomHordeSize", "phantomDirection", "durationMinutes", "radiationAnxietyThreshold");
        }

        [Test]
        public void Vehicle_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Vehicle_ArmoredTruck(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "vehicleId", "displayName", "encumbranceCapacityKg", "isImmuneToBanditAmbush", "fuelConsumptionMultiplier", "noiseOutputPercentage");
            AssertRoundTrips(() => new Vehicle_Motorcycle(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "vehicleId", "displayName", "maxPassengers", "speedMultiplier", "fuelConsumptionMultiplier", "hasRadiationShielding", "hasWeatherProtection");
            AssertRoundTrips(() => new Vehicle_Rowboat(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "vehicleId", "maxPassengers", "speedMultiplier", "fuelConsumption", "staminaCostPerHour", "sniperVulnerability", "isCrafted", "hullDurability");
        }

        [Test]
        public void Visitor_All_CaptureRestore()
        {
            // Visitor_AbandonedState.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new Visitor_AbandonedState(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            AssertRoundTrips(() => new Visitor_ChurchHostile(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "isBellTowerSniperActive", "isGraveyardApproachRequired", "noiseTrapCount", "isDetectedBySniper");
            AssertRoundTrips(() => new Visitor_ChurchSanctuary(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "isSafeZone", "isFatigueFrozen", "isRestingMidExpedition", "fatigueRestoredPerHour");
            // Visitor_ExplodedState.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new Visitor_ExplodedState(), x => x.CaptureState(), (x, s) => x.RestoreState(s));
            AssertRoundTrips(() => new Visitor_FleeingHorde(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "id", "displayName", "isStormPanicActive", "isCombatDisabled", "isStaminaRaceActive");
            AssertRoundTrips(() => new Visitor_HospitalPatients(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "isDespairAuraActive", "maxStaminaMultiplier");
            AssertRoundTrips(() => new Visitor_HospitalStaff(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "isTaxDemanded", "isTaxPaid", "medicalTaxRatio", "willHealPlayer");
            AssertRoundTrips(() => new Visitor_MilTrainingYard(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "militaryNpcCount", "hasHighTierLoot", "requiresEndgameWeapons", "bunkerRaidMultiplier", "raidMultiplierDurationDays");
            AssertRoundTrips(() => new Visitor_QuestFaction(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "activeQuestId", "targetLocationId", "isSpawned");
            AssertRoundTrips(() => new Visitor_RebelTrainingYard(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "cardId", "displayName", "trapDensity", "barricadeRating", "rebelTrustGainOnFlank");
        }

        [Test]
        public void Weapon_All_CaptureRestore()
        {
            AssertRoundTrips(() => new Weapon_Chainsaw(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "fuelPerUse", "ignoresArmor", "noiseDecibels", "weaponId");
            AssertRoundTrips(() => new Weapon_Flamethrower(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "weaponId", "fuelPerUse", "fearRadius", "tankExplodeOnCritChance");
            AssertRoundTrips(() => new Weapon_HMG(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "weaponId", "requiresOperators", "isMounted", "isJammed", "shredsRaidLevel", "mountLocation");
            AssertRoundTrips(() => new Weapon_RPG(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "weaponId", "securityDamagePercent", "concussesAirlockOccupants");
        }

        [Test]
        public void WorldEvent_All_CaptureRestore()
        {
            AssertRoundTrips(() => new WorldEvent_Deforestation(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "displayName", "triggerDay", "isActive", "woodLootMultiplier");
            AssertRoundTrips(() => new WorldEvent_FinalWinter(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "triggerDay", "isActive", "currentTemperature", "daysRemaining", "cropsDestroyed", "surfaceWaterFrozen", "bunkerFreezeDeadline");
            AssertRoundTrips(() => new WorldEvent_Fissure(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "event_id", "is_triggered", "trigger_day", "destroyed_connections", "map_split", "aircraft_required", "severed_nodes");
            AssertRoundTrips(() => new WorldEvent_GreatFamine(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "triggerDay", "isActive", "foodLootTableZeroed", "hydroponicsViable", "cannibalismAvailable");
            AssertRoundTrips(() => new WorldEvent_Megafauna(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "eventId", "displayName", "isActive", "currentNodeId", "daysToCrossMap", "daysRemaining", "meatYield", "requiresExplosives");
        }

        [Test]
        public void AllFamilies_SaveSlots_RegisterAndRoundTrip()
        {
            string dir = TempDir("all");
            try
            {
                var actionAdministerPlacebo = new Action_AdministerPlacebo();
                var actionBarricadeDoor = new Action_BarricadeDoor();
                var actionBoilBatteries = new Action_BoilBatteries();
                var actionBroadcastPropaganda = new Action_BroadcastPropaganda();
                var actionBurnCharcoal = new Action_BurnCharcoal();
                var actionBuryTimeCapsule = new Action_BuryTimeCapsule();
                var actionCallCaravan = new Action_CallCaravan();
                var actionCoverTracks = new Action_CoverTracks();
                var actionCrackMainframe = new Action_CrackMainframe();
                var actionDecrypt = new Action_Decrypt();
                var actionDemandTribute = new Action_DemandTribute();
                var actionEstablishRoute = new Action_EstablishRoute();
                var actionExile = new Action_Exile();
                var actionFish = new Action_Fish();
                var actionHarvestOrgans = new Action_HarvestOrgans();
                var actionInfectSelf = new Action_InfectSelf();
                var actionIsotopeTrace = new Action_IsotopeTrace();
                var actionMercy = new Action_Mercy();
                var actionMixCement = new Action_MixCement();
                var actionMixChems = new Action_MixChems();
                var actionOverwatch = new Action_Overwatch();
                var actionPhysicalTherapy = new Action_PhysicalTherapy();
                var actionPirateRadio = new Action_PirateRadio();
                var actionPlaceBait = new Action_PlaceBait();
                var actionPullTooth = new Action_PullTooth();
                var actionRigCorpse = new Action_RigCorpse();
                var actionRoutePower = new Action_RoutePower();
                var actionSabotage = new Action_Sabotage();
                var actionScorchedEarth = new Action_ScorchedEarth();
                var actionSealRoom = new Action_SealRoom();
                var actionSelfSurgery = new Action_SelfSurgery();
                var actionSilentTakedown = new Action_SilentTakedown();
                var actionSiphonGas = new Action_SiphonGas();
                var actionStabilizeDNA = new Action_StabilizeDNA();
                var actionStargazing = new Action_Stargazing();
                var actionWorshipIdol = new Action_WorshipIdol();
                var afflictionAdrenalineCrash = new Affliction_AdrenalineCrash();
                var afflictionAmnesia = new AmnesiaSystem();
                var afflictionBrainwashed = new Affliction_Brainwashed("affliction_brainwashed");
                var afflictionBrittleBones = new BrittleBonesSystem();
                var afflictionCaveMadness = new CaveMadnessSystem("affliction_cave_madness");
                var afflictionFeralRegression = new FeralRegressionSystem();
                var afflictionImaginaryFriend = new ImaginaryFriendSystem();
                var afflictionNerveDamage = new Affliction_NerveDamage();
                var afflictionOldAge = new Affliction_OldAge();
                var afflictionPhantomLimb = new Affliction_PhantomLimb();
                var afflictionRadHallucinations = new Affliction_RadHallucinations();
                var afflictionRadiationBlindness = new RadiationBlindnessSystem();
                var afflictionScurvyDegeneration = new Affliction_ScurvyDegeneration();
                var afflictionSporeLung = new SporeLungSystem();
                var afflictionSterile = new Affliction_Sterile();
                var afflictionSurvivorsGuilt = new SurvivorsGuiltSystem();
                var afflictionTBI = new Affliction_TBI();
                var afflictionThyroidCancer = new Affliction_ThyroidCancer();
                var afflictionTrenchFoot = new TrenchFootSystem();
                var ashDriftSystem = new AshDriftSystem();
                var audioEventDeafening = new AudioEvent_Deafening();
                var audioEventHeartbeat = new AudioEvent_Heartbeat();
                var burnWardSystem = new BurnWardSystem();
                var cognitiveDecaySystem = new CognitiveDecaySystem();
                var combatStanceLastStand = new CombatStance_LastStand();
                var combatBleedOut = new Combat_BleedOut();
                var combatFlanking = new Combat_Flanking();
                var combatSuppression = new Combat_Suppression();
                var crisisFeralFlora = new Crisis_FeralFlora();
                var crisisStructuralFailure = new Crisis_StructuralFailure();
                var durabilitySuppressor = new Durability_Suppressor();
                var endgameUltimatum = new Endgame_Ultimatum();
                var hazardCookOff = new Hazard_CookOff();
                var hazardExplosiveCrafting = new Hazard_ExplosiveCrafting();
                var hazardFriendlyFire = new Hazard_FriendlyFire();
                var hazardMethane = new MethaneSystem("hazard_methane");
                var hazardMimicCrate = new Hazard_MimicCrate();
                var hazardSurgicalBotch = new Hazard_SurgicalBotch();
                var hazardWeaponBurst = new Hazard_WeaponBurst();
                var hiddenStatUnseen = new HiddenStat_Unseen();
                var itemAICoreData = new Item_AICoreData();
                var itemAmmoTypes = new Item_AmmoTypes();
                var itemAmmonia = new Item_Ammonia();
                var itemAmphetamines = new Item_Amphetamines();
                var itemAshGhillie = new Item_AshGhillie();
                var itemAutoDoc = new Item_AutoDoc();
                var itemBioPlastic = new Item_BioPlastic();
                var itemBloodBag = new Item_BloodBag();
                var itemBoneSaw = new Item_BoneSaw();
                var itemC4 = new Item_C4();
                var itemCaltrops = new Item_Caltrops();
                var itemCarrierBird = new Item_CarrierBird();
                var itemChildsDrawing = new Item_ChildsDrawing();
                var itemCigarettes = new Item_Cigarettes();
                var itemClimbingGear = new Item_ClimbingGear();
                var itemDecoy = new Item_Decoy();
                var itemDogTags = new Item_DogTags();
                var itemEMPGrenade = new Item_EMPGrenade();
                var itemEncryptedDrive = new Item_EncryptedDrive();
                var itemEpiPen = new Item_EpiPen();
                var itemExosuit = new Item_Exosuit();
                var itemFaradayPack = new Item_FaradayPack();
                var itemForeignBook = new Item_ForeignBook();
                var itemGeigerCalibrator = new Item_GeigerCalibrator();
                var itemGlowingMushroom = new GlowingMushroomSystem("item_glowing_mushroom");
                var itemGoldBars = new Item_GoldBars();
                var itemGuitar = new Item_Guitar();
                var itemHeirloom = new Item_Heirloom();
                var itemIBeam = new Item_IBeam();
                var itemImpureIodine = new Item_ImpureIodine();
                var itemJuggernautArmor = new Item_JuggernautArmor();
                var itemKevlarVest = new Item_KevlarVest();
                var itemKeycards = new Item_Keycards();
                var itemLandmine = new Item_Landmine();
                var itemLeadApron = new Item_LeadApron();
                var itemLiquidStitches = new Item_LiquidStitches();
                var itemMaggots = new Item_Maggots();
                var itemMilGasMask = new Item_MilGasMask();
                var itemMutantGland = new Item_MutantGland();
                var itemNanites = new Item_Nanites();
                var itemNightVision = new Item_NightVision();
                var itemPackMule = new Item_PackMule();
                var itemPasswordNote = new Item_PasswordNote();
                var itemPhotoAlbum = new Item_PhotoAlbum();
                var itemPotassiumIodide = new Item_PotassiumIodide();
                var itemPresidentialSeal = new Item_PresidentialSeal();
                var itemPrussianBlue = new Item_PrussianBlue();
                var itemRTGBattery = new Item_RTGBattery();
                var itemSeedLedger = new Item_SeedLedger();
                var itemShockCollar = new Item_ShockCollar();
                var itemSnowshoes = new Item_Snowshoes();
                var itemSurgicalTubing = new Item_SurgicalTubing();
                var itemTearGas = new Item_TearGas();
                var itemTeddyBear = new Item_TeddyBear();
                var itemTrashHazmat = new Item_TrashHazmat();
                var itemUndeliveredMail = new Item_UndeliveredMail();
                var itemVacuumTubes = new Item_VacuumTubes();
                var itemVinylCollection = new Item_VinylCollection();
                var itemVitamins = new Item_Vitamins();
                var itemWalkieTalkie = new Item_WalkieTalkie();
                var itemWastelandSoap = new Item_WastelandSoap();
                var itemWaterTabs = new Item_WaterTabs();
                var itemWeldingGoggles = new Item_WeldingGoggles();
                var itemWristDosimeter = new Item_WristDosimeter();
                var lightningStrikesSystem = new LightningStrikesSystem();
                var locationStateRuinSystem = new LocationStateRuinSystem();
                var locationArcade = new Location_Arcade();
                var locationSlaveMarket = new Location_SlaveMarket();
                var locationStrandedYacht = new Location_StrandedYacht();
                var mapAquifer = new AquiferSystem("map_aquifer");
                var mobileCampSystem = new MobileCampSystem();
                var moralDilemmaSystem = new MoralDilemmaSystem();
                var nPCAddictsPassive = new NPC_AddictsPassive();
                var nPCAggroScavengers = new NPC_AggroScavengers();
                var nPCAggroTrader = new NPC_AggroTrader();
                var nPCBandits = new NPC_Bandits();
                var nPCBlackOps = new NPC_BlackOps();
                var nPCBroker = new NPC_Broker();
                var nPCCannibals = new NPC_Cannibals();
                var nPCChemScientists = new NPC_ChemScientists();
                var nPCCityResidents = new NPC_CityResidents();
                var nPCCollaborators = new NPC_Collaborators();
                var nPCConscripts = new NPC_Conscripts();
                var nPCDesperateFamily = new NPC_DesperateFamily();
                var nPCDrunksAggro = new NPC_DrunksAggro();
                var nPCHomeless = new NPC_Homeless();
                var nPCLonePsychopath = new NPC_LonePsychopath();
                var nPCLooters = new NPC_Looters();
                var nPCMercenaries = new NPC_Mercenaries();
                var nPCMilitaryPatrol = new NPC_MilitaryPatrol();
                var nPCPassiveScavengers = new NPC_PassiveScavengers();
                var nPCPassiveTrader = new NPC_PassiveTrader();
                var nPCPsychopathPair = new NPC_PsychopathPair();
                var nPCRebelMilitia = new NPC_RebelMilitia();
                var nPCRebelModerates = new NPC_RebelModerates();
                var nPCRebelSnipers = new NPC_RebelSnipers();
                var nPCRebelZealots = new NPC_RebelZealots();
                var nPCSlavers = new NPC_Slavers();
                var nPCSpecOps = new NPC_SpecOps();
                var nPCSurvivalists = new NPC_Survivalists();
                var nPCTaxCollector = new NPC_TaxCollector();
                var nPCTerrorists = new NPC_Terrorists();
                var nPCTheNegotiator = new NPC_TheNegotiator();
                var nPCTheOld = new NPC_TheOld();
                var nPCTheParents = new NPC_TheParents();
                var nPCTravelingCouple = new NPC_TravelingCouple();
                var needleSterilizationSystem = new NeedleSterilizationSystem();
                var nightScavengeSystem = new NightScavengeSystem();
                var nodeAutomatedArmory = new Node_AutomatedArmory();
                var nodeGhostShip = new Node_GhostShip();
                var nodeMutantHive = new Node_MutantHive();
                var nodePlayerBank = new Node_PlayerBank();
                var nodeSector7G = new Node_Sector7G();
                var nodeSporeHive = new Node_SporeHive();
                var petFeralCat = new Pet_FeralCat();
                var projectBioReactor = new Project_BioReactor();
                var projectDeepWell = new Project_DeepWell();
                var projectElevator = new Project_Elevator();
                var projectMinecart = new Project_Minecart();
                var projectRadioArray = new Project_RadioArray();
                var projectSurfaceDome = new Project_SurfaceDome();
                var prostheticCraftingSystem = new ProstheticCraftingSystem();
                var seismicVentsSystem = new SeismicVentsSystem();
                var severeFrostbiteSystem = new SevereFrostbiteSystem();
                var shelterEventCaravanAmbush = new ShelterEvent_CaravanAmbush();
                var shelterEventFalseCure = new ShelterEvent_FalseCure();
                var shelterEventRansom = new ShelterEvent_Ransom();
                var shelterEventRefugees = new ShelterEvent_Refugees();
                var shelterEventTheMirror = new ShelterEvent_TheMirror();
                var shelterEventTribute = new ShelterEvent_Tribute();
                var skirmishBandit_vs_Terror = new Skirmish_Bandit_vs_Terror("skirmish_bandit_vs_terror");
                var skirmishMil_vs_Rebel = new Skirmish_Mil_vs_Rebel("skirmish_mil_vs_rebel");
                var skirmishMil_vs_Terror = new Skirmish_Mil_vs_Terror("skirmish_mil_vs_terror");
                var skirmishRebel_vs_Bandit = new Skirmish_Rebel_vs_Bandit("skirmish_rebel_vs_bandit");
                var skirmishRebel_vs_Terror = new Skirmish_Rebel_vs_Terror("skirmish_rebel_vs_terror");
                var tetanusAfflictionSystem = new TetanusAfflictionSystem();
                var timeSystemSys = new TimeSystem();
                var toothDecaySystem = new ToothDecaySystem();
                var traderPlagueConvoy = new Trader_PlagueConvoy();
                var traitAnthropophobia = new Trait_Anthropophobia();
                var traitClairvoyant = new ClairvoyantSystem();
                var traitGenerationalTrauma = new Trait_GenerationalTrauma();
                var traitInheritedGenetics = new Trait_InheritedGenetics();
                var traitMatriarch = new Trait_Matriarch();
                var traitPTSD = new Trait_PTSD();
                var uIEventBlurredVision = new UIEvent_BlurredVision();
                var uIEventCorruptionScare = new UIEvent_CorruptionScare();
                var uIEventFalseInventory = new UIEvent_FalseInventory();
                var uIEventGhostRadio = new UIEvent_GhostRadio();
                var uIEventHacking = new UIEvent_Hacking();
                var uIEventLowPower = new UIEvent_LowPower();
                var uIEventMapRot = new UIEvent_MapRot();
                var uIEventPhantomBlip = new PhantomBlipSystem();
                var vehicleStrandingSystem = new VehicleStrandingSystem();
                var vehicleSystem = new VehicleSystem();
                var vehicleArmoredTruck = new Vehicle_ArmoredTruck();
                var vehicleMotorcycle = new Vehicle_Motorcycle();
                var vehicleRowboat = new Vehicle_Rowboat();
                var visionLossSystem = new VisionLossSystem();
                var visitorRNGSystem = new VisitorRNGSystem();
                var visitorAbandonedState = new Visitor_AbandonedState();
                var visitorChurchHostile = new Visitor_ChurchHostile();
                var visitorChurchSanctuary = new Visitor_ChurchSanctuary();
                var visitorExplodedState = new Visitor_ExplodedState();
                var visitorFleeingHorde = new Visitor_FleeingHorde();
                var visitorHospitalPatients = new Visitor_HospitalPatients();
                var visitorHospitalStaff = new Visitor_HospitalStaff();
                var visitorMilTrainingYard = new Visitor_MilTrainingYard();
                var visitorQuestFaction = new Visitor_QuestFaction();
                var visitorRebelTrainingYard = new Visitor_RebelTrainingYard();
                var weaponChainsaw = new Weapon_Chainsaw();
                var weaponFlamethrower = new Weapon_Flamethrower();
                var weaponHMG = new Weapon_HMG();
                var weaponRPG = new Weapon_RPG();
                var worldEventDeforestation = new WorldEvent_Deforestation();
                var worldEventFinalWinter = new WorldEvent_FinalWinter();
                var worldEventFissure = new WorldEvent_Fissure();
                var worldEventGreatFamine = new WorldEvent_GreatFamine();
                var worldEventMegafauna = new WorldEvent_Megafauna();
                int before = 0;
                var ss = MakeSave(dir, s =>
                {
                    before = s.SaveableCount;
                    s.SetActionAdministerPlacebo(actionAdministerPlacebo);
                    s.SetActionBarricadeDoor(actionBarricadeDoor);
                    s.SetActionBoilBatteries(actionBoilBatteries);
                    s.SetActionBroadcastPropaganda(actionBroadcastPropaganda);
                    s.SetActionBurnCharcoal(actionBurnCharcoal);
                    s.SetActionBuryTimeCapsule(actionBuryTimeCapsule);
                    s.SetActionCallCaravan(actionCallCaravan);
                    s.SetActionCoverTracks(actionCoverTracks);
                    s.SetActionCrackMainframe(actionCrackMainframe);
                    s.SetActionDecrypt(actionDecrypt);
                    s.SetActionDemandTribute(actionDemandTribute);
                    s.SetActionEstablishRoute(actionEstablishRoute);
                    s.SetActionExile(actionExile);
                    s.SetActionFish(actionFish);
                    s.SetActionHarvestOrgans(actionHarvestOrgans);
                    s.SetActionInfectSelf(actionInfectSelf);
                    s.SetActionIsotopeTrace(actionIsotopeTrace);
                    s.SetActionMercy(actionMercy);
                    s.SetActionMixCement(actionMixCement);
                    s.SetActionMixChems(actionMixChems);
                    s.SetActionOverwatch(actionOverwatch);
                    s.SetActionPhysicalTherapy(actionPhysicalTherapy);
                    s.SetActionPirateRadio(actionPirateRadio);
                    s.SetActionPlaceBait(actionPlaceBait);
                    s.SetActionPullTooth(actionPullTooth);
                    s.SetActionRigCorpse(actionRigCorpse);
                    s.SetActionRoutePower(actionRoutePower);
                    s.SetActionSabotage(actionSabotage);
                    s.SetActionScorchedEarth(actionScorchedEarth);
                    s.SetActionSealRoom(actionSealRoom);
                    s.SetActionSelfSurgery(actionSelfSurgery);
                    s.SetActionSilentTakedown(actionSilentTakedown);
                    s.SetActionSiphonGas(actionSiphonGas);
                    s.SetActionStabilizeDNA(actionStabilizeDNA);
                    s.SetActionStargazing(actionStargazing);
                    s.SetActionWorshipIdol(actionWorshipIdol);
                    s.SetAfflictionAdrenalineCrash(afflictionAdrenalineCrash);
                    s.SetAfflictionAmnesia(afflictionAmnesia);
                    s.SetAfflictionBrainwashed(afflictionBrainwashed);
                    s.SetAfflictionBrittleBones(afflictionBrittleBones);
                    s.SetAfflictionCaveMadness(afflictionCaveMadness);
                    s.SetAfflictionFeralRegression(afflictionFeralRegression);
                    s.SetAfflictionImaginaryFriend(afflictionImaginaryFriend);
                    s.SetAfflictionNerveDamage(afflictionNerveDamage);
                    s.SetAfflictionOldAge(afflictionOldAge);
                    s.SetAfflictionPhantomLimb(afflictionPhantomLimb);
                    s.SetAfflictionRadHallucinations(afflictionRadHallucinations);
                    s.SetAfflictionRadiationBlindness(afflictionRadiationBlindness);
                    s.SetAfflictionScurvyDegeneration(afflictionScurvyDegeneration);
                    s.SetAfflictionSporeLung(afflictionSporeLung);
                    s.SetAfflictionSterile(afflictionSterile);
                    s.SetAfflictionSurvivorsGuilt(afflictionSurvivorsGuilt);
                    s.SetAfflictionTBI(afflictionTBI);
                    s.SetAfflictionThyroidCancer(afflictionThyroidCancer);
                    s.SetAfflictionTrenchFoot(afflictionTrenchFoot);
                    s.SetAshDriftSystem(ashDriftSystem);
                    s.SetAudioEventDeafening(audioEventDeafening);
                    s.SetAudioEventHeartbeat(audioEventHeartbeat);
                    s.SetBurnWardSystem(burnWardSystem);
                    s.SetCognitiveDecaySystem(cognitiveDecaySystem);
                    s.SetCombatStanceLastStand(combatStanceLastStand);
                    s.SetCombatBleedOut(combatBleedOut);
                    s.SetCombatFlanking(combatFlanking);
                    s.SetCombatSuppression(combatSuppression);
                    s.SetCrisisFeralFlora(crisisFeralFlora);
                    s.SetCrisisStructuralFailure(crisisStructuralFailure);
                    s.SetDurabilitySuppressor(durabilitySuppressor);
                    s.SetEndgameUltimatum(endgameUltimatum);
                    s.SetHazardCookOff(hazardCookOff);
                    s.SetHazardExplosiveCrafting(hazardExplosiveCrafting);
                    s.SetHazardFriendlyFire(hazardFriendlyFire);
                    s.SetHazardMethane(hazardMethane);
                    s.SetHazardMimicCrate(hazardMimicCrate);
                    s.SetHazardSurgicalBotch(hazardSurgicalBotch);
                    s.SetHazardWeaponBurst(hazardWeaponBurst);
                    s.SetHiddenStatUnseen(hiddenStatUnseen);
                    s.SetItemAICoreData(itemAICoreData);
                    s.SetItemAmmoTypes(itemAmmoTypes);
                    s.SetItemAmmonia(itemAmmonia);
                    s.SetItemAmphetamines(itemAmphetamines);
                    s.SetItemAshGhillie(itemAshGhillie);
                    s.SetItemAutoDoc(itemAutoDoc);
                    s.SetItemBioPlastic(itemBioPlastic);
                    s.SetItemBloodBag(itemBloodBag);
                    s.SetItemBoneSaw(itemBoneSaw);
                    s.SetItemC4(itemC4);
                    s.SetItemCaltrops(itemCaltrops);
                    s.SetItemCarrierBird(itemCarrierBird);
                    s.SetItemChildsDrawing(itemChildsDrawing);
                    s.SetItemCigarettes(itemCigarettes);
                    s.SetItemClimbingGear(itemClimbingGear);
                    s.SetItemDecoy(itemDecoy);
                    s.SetItemDogTags(itemDogTags);
                    s.SetItemEMPGrenade(itemEMPGrenade);
                    s.SetItemEncryptedDrive(itemEncryptedDrive);
                    s.SetItemEpiPen(itemEpiPen);
                    s.SetItemExosuit(itemExosuit);
                    s.SetItemFaradayPack(itemFaradayPack);
                    s.SetItemForeignBook(itemForeignBook);
                    s.SetItemGeigerCalibrator(itemGeigerCalibrator);
                    s.SetItemGlowingMushroom(itemGlowingMushroom);
                    s.SetItemGoldBars(itemGoldBars);
                    s.SetItemGuitar(itemGuitar);
                    s.SetItemHeirloom(itemHeirloom);
                    s.SetItemIBeam(itemIBeam);
                    s.SetItemImpureIodine(itemImpureIodine);
                    s.SetItemJuggernautArmor(itemJuggernautArmor);
                    s.SetItemKevlarVest(itemKevlarVest);
                    s.SetItemKeycards(itemKeycards);
                    s.SetItemLandmine(itemLandmine);
                    s.SetItemLeadApron(itemLeadApron);
                    s.SetItemLiquidStitches(itemLiquidStitches);
                    s.SetItemMaggots(itemMaggots);
                    s.SetItemMilGasMask(itemMilGasMask);
                    s.SetItemMutantGland(itemMutantGland);
                    s.SetItemNanites(itemNanites);
                    s.SetItemNightVision(itemNightVision);
                    s.SetItemPackMule(itemPackMule);
                    s.SetItemPasswordNote(itemPasswordNote);
                    s.SetItemPhotoAlbum(itemPhotoAlbum);
                    s.SetItemPotassiumIodide(itemPotassiumIodide);
                    s.SetItemPresidentialSeal(itemPresidentialSeal);
                    s.SetItemPrussianBlue(itemPrussianBlue);
                    s.SetItemRTGBattery(itemRTGBattery);
                    s.SetItemSeedLedger(itemSeedLedger);
                    s.SetItemShockCollar(itemShockCollar);
                    s.SetItemSnowshoes(itemSnowshoes);
                    s.SetItemSurgicalTubing(itemSurgicalTubing);
                    s.SetItemTearGas(itemTearGas);
                    s.SetItemTeddyBear(itemTeddyBear);
                    s.SetItemTrashHazmat(itemTrashHazmat);
                    s.SetItemUndeliveredMail(itemUndeliveredMail);
                    s.SetItemVacuumTubes(itemVacuumTubes);
                    s.SetItemVinylCollection(itemVinylCollection);
                    s.SetItemVitamins(itemVitamins);
                    s.SetItemWalkieTalkie(itemWalkieTalkie);
                    s.SetItemWastelandSoap(itemWastelandSoap);
                    s.SetItemWaterTabs(itemWaterTabs);
                    s.SetItemWeldingGoggles(itemWeldingGoggles);
                    s.SetItemWristDosimeter(itemWristDosimeter);
                    s.SetLightningStrikesSystem(lightningStrikesSystem);
                    s.SetLocationStateRuinSystem(locationStateRuinSystem);
                    s.SetLocationArcade(locationArcade);
                    s.SetLocationSlaveMarket(locationSlaveMarket);
                    s.SetLocationStrandedYacht(locationStrandedYacht);
                    s.SetMapAquifer(mapAquifer);
                    s.SetMobileCampSystem(mobileCampSystem);
                    s.SetMoralDilemmaSystem(moralDilemmaSystem);
                    s.SetNPCAddictsPassive(nPCAddictsPassive);
                    s.SetNPCAggroScavengers(nPCAggroScavengers);
                    s.SetNPCAggroTrader(nPCAggroTrader);
                    s.SetNPCBandits(nPCBandits);
                    s.SetNPCBlackOps(nPCBlackOps);
                    s.SetNPCBroker(nPCBroker);
                    s.SetNPCCannibals(nPCCannibals);
                    s.SetNPCChemScientists(nPCChemScientists);
                    s.SetNPCCityResidents(nPCCityResidents);
                    s.SetNPCCollaborators(nPCCollaborators);
                    s.SetNPCConscripts(nPCConscripts);
                    s.SetNPCDesperateFamily(nPCDesperateFamily);
                    s.SetNPCDrunksAggro(nPCDrunksAggro);
                    s.SetNPCHomeless(nPCHomeless);
                    s.SetNPCLonePsychopath(nPCLonePsychopath);
                    s.SetNPCLooters(nPCLooters);
                    s.SetNPCMercenaries(nPCMercenaries);
                    s.SetNPCMilitaryPatrol(nPCMilitaryPatrol);
                    s.SetNPCPassiveScavengers(nPCPassiveScavengers);
                    s.SetNPCPassiveTrader(nPCPassiveTrader);
                    s.SetNPCPsychopathPair(nPCPsychopathPair);
                    s.SetNPCRebelMilitia(nPCRebelMilitia);
                    s.SetNPCRebelModerates(nPCRebelModerates);
                    s.SetNPCRebelSnipers(nPCRebelSnipers);
                    s.SetNPCRebelZealots(nPCRebelZealots);
                    s.SetNPCSlavers(nPCSlavers);
                    s.SetNPCSpecOps(nPCSpecOps);
                    s.SetNPCSurvivalists(nPCSurvivalists);
                    s.SetNPCTaxCollector(nPCTaxCollector);
                    s.SetNPCTerrorists(nPCTerrorists);
                    s.SetNPCTheNegotiator(nPCTheNegotiator);
                    s.SetNPCTheOld(nPCTheOld);
                    s.SetNPCTheParents(nPCTheParents);
                    s.SetNPCTravelingCouple(nPCTravelingCouple);
                    s.SetNeedleSterilizationSystem(needleSterilizationSystem);
                    s.SetNightScavengeSystem(nightScavengeSystem);
                    s.SetNodeAutomatedArmory(nodeAutomatedArmory);
                    s.SetNodeGhostShip(nodeGhostShip);
                    s.SetNodeMutantHive(nodeMutantHive);
                    s.SetNodePlayerBank(nodePlayerBank);
                    s.SetNodeSector7G(nodeSector7G);
                    s.SetNodeSporeHive(nodeSporeHive);
                    s.SetPetFeralCat(petFeralCat);
                    s.SetProjectBioReactor(projectBioReactor);
                    s.SetProjectDeepWell(projectDeepWell);
                    s.SetProjectElevator(projectElevator);
                    s.SetProjectMinecart(projectMinecart);
                    s.SetProjectRadioArray(projectRadioArray);
                    s.SetProjectSurfaceDome(projectSurfaceDome);
                    s.SetProstheticCraftingSystem(prostheticCraftingSystem);
                    s.SetSeismicVentsSystem(seismicVentsSystem);
                    s.SetSevereFrostbiteSystem(severeFrostbiteSystem);
                    s.SetShelterEventCaravanAmbush(shelterEventCaravanAmbush);
                    s.SetShelterEventFalseCure(shelterEventFalseCure);
                    s.SetShelterEventRansom(shelterEventRansom);
                    s.SetShelterEventRefugees(shelterEventRefugees);
                    s.SetShelterEventTheMirror(shelterEventTheMirror);
                    s.SetShelterEventTribute(shelterEventTribute);
                    s.SetSkirmishBandit_vs_Terror(skirmishBandit_vs_Terror);
                    s.SetSkirmishMil_vs_Rebel(skirmishMil_vs_Rebel);
                    s.SetSkirmishMil_vs_Terror(skirmishMil_vs_Terror);
                    s.SetSkirmishRebel_vs_Bandit(skirmishRebel_vs_Bandit);
                    s.SetSkirmishRebel_vs_Terror(skirmishRebel_vs_Terror);
                    s.SetTetanusAfflictionSystem(tetanusAfflictionSystem);
                    s.SetTimeSystem(timeSystemSys);
                    s.SetToothDecaySystem(toothDecaySystem);
                    s.SetTraderPlagueConvoy(traderPlagueConvoy);
                    s.SetTraitAnthropophobia(traitAnthropophobia);
                    s.SetTraitClairvoyant(traitClairvoyant);
                    s.SetTraitGenerationalTrauma(traitGenerationalTrauma);
                    s.SetTraitInheritedGenetics(traitInheritedGenetics);
                    s.SetTraitMatriarch(traitMatriarch);
                    s.SetTraitPTSD(traitPTSD);
                    s.SetUIEventBlurredVision(uIEventBlurredVision);
                    s.SetUIEventCorruptionScare(uIEventCorruptionScare);
                    s.SetUIEventFalseInventory(uIEventFalseInventory);
                    s.SetUIEventGhostRadio(uIEventGhostRadio);
                    s.SetUIEventHacking(uIEventHacking);
                    s.SetUIEventLowPower(uIEventLowPower);
                    s.SetUIEventMapRot(uIEventMapRot);
                    s.SetUIEventPhantomBlip(uIEventPhantomBlip);
                    s.SetVehicleStrandingSystem(vehicleStrandingSystem);
                    s.SetVehicleSystem(vehicleSystem);
                    s.SetVehicleArmoredTruck(vehicleArmoredTruck);
                    s.SetVehicleMotorcycle(vehicleMotorcycle);
                    s.SetVehicleRowboat(vehicleRowboat);
                    s.SetVisionLossSystem(visionLossSystem);
                    s.SetVisitorRNGSystem(visitorRNGSystem);
                    s.SetVisitorAbandonedState(visitorAbandonedState);
                    s.SetVisitorChurchHostile(visitorChurchHostile);
                    s.SetVisitorChurchSanctuary(visitorChurchSanctuary);
                    s.SetVisitorExplodedState(visitorExplodedState);
                    s.SetVisitorFleeingHorde(visitorFleeingHorde);
                    s.SetVisitorHospitalPatients(visitorHospitalPatients);
                    s.SetVisitorHospitalStaff(visitorHospitalStaff);
                    s.SetVisitorMilTrainingYard(visitorMilTrainingYard);
                    s.SetVisitorQuestFaction(visitorQuestFaction);
                    s.SetVisitorRebelTrainingYard(visitorRebelTrainingYard);
                    s.SetWeaponChainsaw(weaponChainsaw);
                    s.SetWeaponFlamethrower(weaponFlamethrower);
                    s.SetWeaponHMG(weaponHMG);
                    s.SetWeaponRPG(weaponRPG);
                    s.SetWorldEventDeforestation(worldEventDeforestation);
                    s.SetWorldEventFinalWinter(worldEventFinalWinter);
                    s.SetWorldEventFissure(worldEventFissure);
                    s.SetWorldEventGreatFamine(worldEventGreatFamine);
                    s.SetWorldEventMegafauna(worldEventMegafauna);
                });
                Assert.AreEqual(before + 255, ss.SaveableCount);
                Assert.IsTrue(ss.Save("slot"));
                var actionAdministerPlacebo2 = new Action_AdministerPlacebo();
                var actionBarricadeDoor2 = new Action_BarricadeDoor();
                var actionBoilBatteries2 = new Action_BoilBatteries();
                var actionBroadcastPropaganda2 = new Action_BroadcastPropaganda();
                var actionBurnCharcoal2 = new Action_BurnCharcoal();
                var actionBuryTimeCapsule2 = new Action_BuryTimeCapsule();
                var actionCallCaravan2 = new Action_CallCaravan();
                var actionCoverTracks2 = new Action_CoverTracks();
                var actionCrackMainframe2 = new Action_CrackMainframe();
                var actionDecrypt2 = new Action_Decrypt();
                var actionDemandTribute2 = new Action_DemandTribute();
                var actionEstablishRoute2 = new Action_EstablishRoute();
                var actionExile2 = new Action_Exile();
                var actionFish2 = new Action_Fish();
                var actionHarvestOrgans2 = new Action_HarvestOrgans();
                var actionInfectSelf2 = new Action_InfectSelf();
                var actionIsotopeTrace2 = new Action_IsotopeTrace();
                var actionMercy2 = new Action_Mercy();
                var actionMixCement2 = new Action_MixCement();
                var actionMixChems2 = new Action_MixChems();
                var actionOverwatch2 = new Action_Overwatch();
                var actionPhysicalTherapy2 = new Action_PhysicalTherapy();
                var actionPirateRadio2 = new Action_PirateRadio();
                var actionPlaceBait2 = new Action_PlaceBait();
                var actionPullTooth2 = new Action_PullTooth();
                var actionRigCorpse2 = new Action_RigCorpse();
                var actionRoutePower2 = new Action_RoutePower();
                var actionSabotage2 = new Action_Sabotage();
                var actionScorchedEarth2 = new Action_ScorchedEarth();
                var actionSealRoom2 = new Action_SealRoom();
                var actionSelfSurgery2 = new Action_SelfSurgery();
                var actionSilentTakedown2 = new Action_SilentTakedown();
                var actionSiphonGas2 = new Action_SiphonGas();
                var actionStabilizeDNA2 = new Action_StabilizeDNA();
                var actionStargazing2 = new Action_Stargazing();
                var actionWorshipIdol2 = new Action_WorshipIdol();
                var afflictionAdrenalineCrash2 = new Affliction_AdrenalineCrash();
                var afflictionAmnesia2 = new AmnesiaSystem();
                var afflictionBrainwashed2 = new Affliction_Brainwashed("affliction_brainwashed");
                var afflictionBrittleBones2 = new BrittleBonesSystem();
                var afflictionCaveMadness2 = new CaveMadnessSystem("affliction_cave_madness");
                var afflictionFeralRegression2 = new FeralRegressionSystem();
                var afflictionImaginaryFriend2 = new ImaginaryFriendSystem();
                var afflictionNerveDamage2 = new Affliction_NerveDamage();
                var afflictionOldAge2 = new Affliction_OldAge();
                var afflictionPhantomLimb2 = new Affliction_PhantomLimb();
                var afflictionRadHallucinations2 = new Affliction_RadHallucinations();
                var afflictionRadiationBlindness2 = new RadiationBlindnessSystem();
                var afflictionScurvyDegeneration2 = new Affliction_ScurvyDegeneration();
                var afflictionSporeLung2 = new SporeLungSystem();
                var afflictionSterile2 = new Affliction_Sterile();
                var afflictionSurvivorsGuilt2 = new SurvivorsGuiltSystem();
                var afflictionTBI2 = new Affliction_TBI();
                var afflictionThyroidCancer2 = new Affliction_ThyroidCancer();
                var afflictionTrenchFoot2 = new TrenchFootSystem();
                var ashDriftSystem2 = new AshDriftSystem();
                var audioEventDeafening2 = new AudioEvent_Deafening();
                var audioEventHeartbeat2 = new AudioEvent_Heartbeat();
                var burnWardSystem2 = new BurnWardSystem();
                var cognitiveDecaySystem2 = new CognitiveDecaySystem();
                var combatStanceLastStand2 = new CombatStance_LastStand();
                var combatBleedOut2 = new Combat_BleedOut();
                var combatFlanking2 = new Combat_Flanking();
                var combatSuppression2 = new Combat_Suppression();
                var crisisFeralFlora2 = new Crisis_FeralFlora();
                var crisisStructuralFailure2 = new Crisis_StructuralFailure();
                var durabilitySuppressor2 = new Durability_Suppressor();
                var endgameUltimatum2 = new Endgame_Ultimatum();
                var hazardCookOff2 = new Hazard_CookOff();
                var hazardExplosiveCrafting2 = new Hazard_ExplosiveCrafting();
                var hazardFriendlyFire2 = new Hazard_FriendlyFire();
                var hazardMethane2 = new MethaneSystem("hazard_methane");
                var hazardMimicCrate2 = new Hazard_MimicCrate();
                var hazardSurgicalBotch2 = new Hazard_SurgicalBotch();
                var hazardWeaponBurst2 = new Hazard_WeaponBurst();
                var hiddenStatUnseen2 = new HiddenStat_Unseen();
                var itemAICoreData2 = new Item_AICoreData();
                var itemAmmoTypes2 = new Item_AmmoTypes();
                var itemAmmonia2 = new Item_Ammonia();
                var itemAmphetamines2 = new Item_Amphetamines();
                var itemAshGhillie2 = new Item_AshGhillie();
                var itemAutoDoc2 = new Item_AutoDoc();
                var itemBioPlastic2 = new Item_BioPlastic();
                var itemBloodBag2 = new Item_BloodBag();
                var itemBoneSaw2 = new Item_BoneSaw();
                var itemC42 = new Item_C4();
                var itemCaltrops2 = new Item_Caltrops();
                var itemCarrierBird2 = new Item_CarrierBird();
                var itemChildsDrawing2 = new Item_ChildsDrawing();
                var itemCigarettes2 = new Item_Cigarettes();
                var itemClimbingGear2 = new Item_ClimbingGear();
                var itemDecoy2 = new Item_Decoy();
                var itemDogTags2 = new Item_DogTags();
                var itemEMPGrenade2 = new Item_EMPGrenade();
                var itemEncryptedDrive2 = new Item_EncryptedDrive();
                var itemEpiPen2 = new Item_EpiPen();
                var itemExosuit2 = new Item_Exosuit();
                var itemFaradayPack2 = new Item_FaradayPack();
                var itemForeignBook2 = new Item_ForeignBook();
                var itemGeigerCalibrator2 = new Item_GeigerCalibrator();
                var itemGlowingMushroom2 = new GlowingMushroomSystem("item_glowing_mushroom");
                var itemGoldBars2 = new Item_GoldBars();
                var itemGuitar2 = new Item_Guitar();
                var itemHeirloom2 = new Item_Heirloom();
                var itemIBeam2 = new Item_IBeam();
                var itemImpureIodine2 = new Item_ImpureIodine();
                var itemJuggernautArmor2 = new Item_JuggernautArmor();
                var itemKevlarVest2 = new Item_KevlarVest();
                var itemKeycards2 = new Item_Keycards();
                var itemLandmine2 = new Item_Landmine();
                var itemLeadApron2 = new Item_LeadApron();
                var itemLiquidStitches2 = new Item_LiquidStitches();
                var itemMaggots2 = new Item_Maggots();
                var itemMilGasMask2 = new Item_MilGasMask();
                var itemMutantGland2 = new Item_MutantGland();
                var itemNanites2 = new Item_Nanites();
                var itemNightVision2 = new Item_NightVision();
                var itemPackMule2 = new Item_PackMule();
                var itemPasswordNote2 = new Item_PasswordNote();
                var itemPhotoAlbum2 = new Item_PhotoAlbum();
                var itemPotassiumIodide2 = new Item_PotassiumIodide();
                var itemPresidentialSeal2 = new Item_PresidentialSeal();
                var itemPrussianBlue2 = new Item_PrussianBlue();
                var itemRTGBattery2 = new Item_RTGBattery();
                var itemSeedLedger2 = new Item_SeedLedger();
                var itemShockCollar2 = new Item_ShockCollar();
                var itemSnowshoes2 = new Item_Snowshoes();
                var itemSurgicalTubing2 = new Item_SurgicalTubing();
                var itemTearGas2 = new Item_TearGas();
                var itemTeddyBear2 = new Item_TeddyBear();
                var itemTrashHazmat2 = new Item_TrashHazmat();
                var itemUndeliveredMail2 = new Item_UndeliveredMail();
                var itemVacuumTubes2 = new Item_VacuumTubes();
                var itemVinylCollection2 = new Item_VinylCollection();
                var itemVitamins2 = new Item_Vitamins();
                var itemWalkieTalkie2 = new Item_WalkieTalkie();
                var itemWastelandSoap2 = new Item_WastelandSoap();
                var itemWaterTabs2 = new Item_WaterTabs();
                var itemWeldingGoggles2 = new Item_WeldingGoggles();
                var itemWristDosimeter2 = new Item_WristDosimeter();
                var lightningStrikesSystem2 = new LightningStrikesSystem();
                var locationStateRuinSystem2 = new LocationStateRuinSystem();
                var locationArcade2 = new Location_Arcade();
                var locationSlaveMarket2 = new Location_SlaveMarket();
                var locationStrandedYacht2 = new Location_StrandedYacht();
                var mapAquifer2 = new AquiferSystem("map_aquifer");
                var mobileCampSystem2 = new MobileCampSystem();
                var moralDilemmaSystem2 = new MoralDilemmaSystem();
                var nPCAddictsPassive2 = new NPC_AddictsPassive();
                var nPCAggroScavengers2 = new NPC_AggroScavengers();
                var nPCAggroTrader2 = new NPC_AggroTrader();
                var nPCBandits2 = new NPC_Bandits();
                var nPCBlackOps2 = new NPC_BlackOps();
                var nPCBroker2 = new NPC_Broker();
                var nPCCannibals2 = new NPC_Cannibals();
                var nPCChemScientists2 = new NPC_ChemScientists();
                var nPCCityResidents2 = new NPC_CityResidents();
                var nPCCollaborators2 = new NPC_Collaborators();
                var nPCConscripts2 = new NPC_Conscripts();
                var nPCDesperateFamily2 = new NPC_DesperateFamily();
                var nPCDrunksAggro2 = new NPC_DrunksAggro();
                var nPCHomeless2 = new NPC_Homeless();
                var nPCLonePsychopath2 = new NPC_LonePsychopath();
                var nPCLooters2 = new NPC_Looters();
                var nPCMercenaries2 = new NPC_Mercenaries();
                var nPCMilitaryPatrol2 = new NPC_MilitaryPatrol();
                var nPCPassiveScavengers2 = new NPC_PassiveScavengers();
                var nPCPassiveTrader2 = new NPC_PassiveTrader();
                var nPCPsychopathPair2 = new NPC_PsychopathPair();
                var nPCRebelMilitia2 = new NPC_RebelMilitia();
                var nPCRebelModerates2 = new NPC_RebelModerates();
                var nPCRebelSnipers2 = new NPC_RebelSnipers();
                var nPCRebelZealots2 = new NPC_RebelZealots();
                var nPCSlavers2 = new NPC_Slavers();
                var nPCSpecOps2 = new NPC_SpecOps();
                var nPCSurvivalists2 = new NPC_Survivalists();
                var nPCTaxCollector2 = new NPC_TaxCollector();
                var nPCTerrorists2 = new NPC_Terrorists();
                var nPCTheNegotiator2 = new NPC_TheNegotiator();
                var nPCTheOld2 = new NPC_TheOld();
                var nPCTheParents2 = new NPC_TheParents();
                var nPCTravelingCouple2 = new NPC_TravelingCouple();
                var needleSterilizationSystem2 = new NeedleSterilizationSystem();
                var nightScavengeSystem2 = new NightScavengeSystem();
                var nodeAutomatedArmory2 = new Node_AutomatedArmory();
                var nodeGhostShip2 = new Node_GhostShip();
                var nodeMutantHive2 = new Node_MutantHive();
                var nodePlayerBank2 = new Node_PlayerBank();
                var nodeSector7G2 = new Node_Sector7G();
                var nodeSporeHive2 = new Node_SporeHive();
                var petFeralCat2 = new Pet_FeralCat();
                var projectBioReactor2 = new Project_BioReactor();
                var projectDeepWell2 = new Project_DeepWell();
                var projectElevator2 = new Project_Elevator();
                var projectMinecart2 = new Project_Minecart();
                var projectRadioArray2 = new Project_RadioArray();
                var projectSurfaceDome2 = new Project_SurfaceDome();
                var prostheticCraftingSystem2 = new ProstheticCraftingSystem();
                var seismicVentsSystem2 = new SeismicVentsSystem();
                var severeFrostbiteSystem2 = new SevereFrostbiteSystem();
                var shelterEventCaravanAmbush2 = new ShelterEvent_CaravanAmbush();
                var shelterEventFalseCure2 = new ShelterEvent_FalseCure();
                var shelterEventRansom2 = new ShelterEvent_Ransom();
                var shelterEventRefugees2 = new ShelterEvent_Refugees();
                var shelterEventTheMirror2 = new ShelterEvent_TheMirror();
                var shelterEventTribute2 = new ShelterEvent_Tribute();
                var skirmishBandit_vs_Terror2 = new Skirmish_Bandit_vs_Terror("skirmish_bandit_vs_terror");
                var skirmishMil_vs_Rebel2 = new Skirmish_Mil_vs_Rebel("skirmish_mil_vs_rebel");
                var skirmishMil_vs_Terror2 = new Skirmish_Mil_vs_Terror("skirmish_mil_vs_terror");
                var skirmishRebel_vs_Bandit2 = new Skirmish_Rebel_vs_Bandit("skirmish_rebel_vs_bandit");
                var skirmishRebel_vs_Terror2 = new Skirmish_Rebel_vs_Terror("skirmish_rebel_vs_terror");
                var tetanusAfflictionSystem2 = new TetanusAfflictionSystem();
                var timeSystemSys2 = new TimeSystem();
                var toothDecaySystem2 = new ToothDecaySystem();
                var traderPlagueConvoy2 = new Trader_PlagueConvoy();
                var traitAnthropophobia2 = new Trait_Anthropophobia();
                var traitClairvoyant2 = new ClairvoyantSystem();
                var traitGenerationalTrauma2 = new Trait_GenerationalTrauma();
                var traitInheritedGenetics2 = new Trait_InheritedGenetics();
                var traitMatriarch2 = new Trait_Matriarch();
                var traitPTSD2 = new Trait_PTSD();
                var uIEventBlurredVision2 = new UIEvent_BlurredVision();
                var uIEventCorruptionScare2 = new UIEvent_CorruptionScare();
                var uIEventFalseInventory2 = new UIEvent_FalseInventory();
                var uIEventGhostRadio2 = new UIEvent_GhostRadio();
                var uIEventHacking2 = new UIEvent_Hacking();
                var uIEventLowPower2 = new UIEvent_LowPower();
                var uIEventMapRot2 = new UIEvent_MapRot();
                var uIEventPhantomBlip2 = new PhantomBlipSystem();
                var vehicleStrandingSystem2 = new VehicleStrandingSystem();
                var vehicleSystem2 = new VehicleSystem();
                var vehicleArmoredTruck2 = new Vehicle_ArmoredTruck();
                var vehicleMotorcycle2 = new Vehicle_Motorcycle();
                var vehicleRowboat2 = new Vehicle_Rowboat();
                var visionLossSystem2 = new VisionLossSystem();
                var visitorRNGSystem2 = new VisitorRNGSystem();
                var visitorAbandonedState2 = new Visitor_AbandonedState();
                var visitorChurchHostile2 = new Visitor_ChurchHostile();
                var visitorChurchSanctuary2 = new Visitor_ChurchSanctuary();
                var visitorExplodedState2 = new Visitor_ExplodedState();
                var visitorFleeingHorde2 = new Visitor_FleeingHorde();
                var visitorHospitalPatients2 = new Visitor_HospitalPatients();
                var visitorHospitalStaff2 = new Visitor_HospitalStaff();
                var visitorMilTrainingYard2 = new Visitor_MilTrainingYard();
                var visitorQuestFaction2 = new Visitor_QuestFaction();
                var visitorRebelTrainingYard2 = new Visitor_RebelTrainingYard();
                var weaponChainsaw2 = new Weapon_Chainsaw();
                var weaponFlamethrower2 = new Weapon_Flamethrower();
                var weaponHMG2 = new Weapon_HMG();
                var weaponRPG2 = new Weapon_RPG();
                var worldEventDeforestation2 = new WorldEvent_Deforestation();
                var worldEventFinalWinter2 = new WorldEvent_FinalWinter();
                var worldEventFissure2 = new WorldEvent_Fissure();
                var worldEventGreatFamine2 = new WorldEvent_GreatFamine();
                var worldEventMegafauna2 = new WorldEvent_Megafauna();
                var ss2 = MakeSave(dir, s =>
                {
                    s.SetActionAdministerPlacebo(actionAdministerPlacebo2);
                    s.SetActionBarricadeDoor(actionBarricadeDoor2);
                    s.SetActionBoilBatteries(actionBoilBatteries2);
                    s.SetActionBroadcastPropaganda(actionBroadcastPropaganda2);
                    s.SetActionBurnCharcoal(actionBurnCharcoal2);
                    s.SetActionBuryTimeCapsule(actionBuryTimeCapsule2);
                    s.SetActionCallCaravan(actionCallCaravan2);
                    s.SetActionCoverTracks(actionCoverTracks2);
                    s.SetActionCrackMainframe(actionCrackMainframe2);
                    s.SetActionDecrypt(actionDecrypt2);
                    s.SetActionDemandTribute(actionDemandTribute2);
                    s.SetActionEstablishRoute(actionEstablishRoute2);
                    s.SetActionExile(actionExile2);
                    s.SetActionFish(actionFish2);
                    s.SetActionHarvestOrgans(actionHarvestOrgans2);
                    s.SetActionInfectSelf(actionInfectSelf2);
                    s.SetActionIsotopeTrace(actionIsotopeTrace2);
                    s.SetActionMercy(actionMercy2);
                    s.SetActionMixCement(actionMixCement2);
                    s.SetActionMixChems(actionMixChems2);
                    s.SetActionOverwatch(actionOverwatch2);
                    s.SetActionPhysicalTherapy(actionPhysicalTherapy2);
                    s.SetActionPirateRadio(actionPirateRadio2);
                    s.SetActionPlaceBait(actionPlaceBait2);
                    s.SetActionPullTooth(actionPullTooth2);
                    s.SetActionRigCorpse(actionRigCorpse2);
                    s.SetActionRoutePower(actionRoutePower2);
                    s.SetActionSabotage(actionSabotage2);
                    s.SetActionScorchedEarth(actionScorchedEarth2);
                    s.SetActionSealRoom(actionSealRoom2);
                    s.SetActionSelfSurgery(actionSelfSurgery2);
                    s.SetActionSilentTakedown(actionSilentTakedown2);
                    s.SetActionSiphonGas(actionSiphonGas2);
                    s.SetActionStabilizeDNA(actionStabilizeDNA2);
                    s.SetActionStargazing(actionStargazing2);
                    s.SetActionWorshipIdol(actionWorshipIdol2);
                    s.SetAfflictionAdrenalineCrash(afflictionAdrenalineCrash2);
                    s.SetAfflictionAmnesia(afflictionAmnesia2);
                    s.SetAfflictionBrainwashed(afflictionBrainwashed2);
                    s.SetAfflictionBrittleBones(afflictionBrittleBones2);
                    s.SetAfflictionCaveMadness(afflictionCaveMadness2);
                    s.SetAfflictionFeralRegression(afflictionFeralRegression2);
                    s.SetAfflictionImaginaryFriend(afflictionImaginaryFriend2);
                    s.SetAfflictionNerveDamage(afflictionNerveDamage2);
                    s.SetAfflictionOldAge(afflictionOldAge2);
                    s.SetAfflictionPhantomLimb(afflictionPhantomLimb2);
                    s.SetAfflictionRadHallucinations(afflictionRadHallucinations2);
                    s.SetAfflictionRadiationBlindness(afflictionRadiationBlindness2);
                    s.SetAfflictionScurvyDegeneration(afflictionScurvyDegeneration2);
                    s.SetAfflictionSporeLung(afflictionSporeLung2);
                    s.SetAfflictionSterile(afflictionSterile2);
                    s.SetAfflictionSurvivorsGuilt(afflictionSurvivorsGuilt2);
                    s.SetAfflictionTBI(afflictionTBI2);
                    s.SetAfflictionThyroidCancer(afflictionThyroidCancer2);
                    s.SetAfflictionTrenchFoot(afflictionTrenchFoot2);
                    s.SetAshDriftSystem(ashDriftSystem2);
                    s.SetAudioEventDeafening(audioEventDeafening2);
                    s.SetAudioEventHeartbeat(audioEventHeartbeat2);
                    s.SetBurnWardSystem(burnWardSystem2);
                    s.SetCognitiveDecaySystem(cognitiveDecaySystem2);
                    s.SetCombatStanceLastStand(combatStanceLastStand2);
                    s.SetCombatBleedOut(combatBleedOut2);
                    s.SetCombatFlanking(combatFlanking2);
                    s.SetCombatSuppression(combatSuppression2);
                    s.SetCrisisFeralFlora(crisisFeralFlora2);
                    s.SetCrisisStructuralFailure(crisisStructuralFailure2);
                    s.SetDurabilitySuppressor(durabilitySuppressor2);
                    s.SetEndgameUltimatum(endgameUltimatum2);
                    s.SetHazardCookOff(hazardCookOff2);
                    s.SetHazardExplosiveCrafting(hazardExplosiveCrafting2);
                    s.SetHazardFriendlyFire(hazardFriendlyFire2);
                    s.SetHazardMethane(hazardMethane2);
                    s.SetHazardMimicCrate(hazardMimicCrate2);
                    s.SetHazardSurgicalBotch(hazardSurgicalBotch2);
                    s.SetHazardWeaponBurst(hazardWeaponBurst2);
                    s.SetHiddenStatUnseen(hiddenStatUnseen2);
                    s.SetItemAICoreData(itemAICoreData2);
                    s.SetItemAmmoTypes(itemAmmoTypes2);
                    s.SetItemAmmonia(itemAmmonia2);
                    s.SetItemAmphetamines(itemAmphetamines2);
                    s.SetItemAshGhillie(itemAshGhillie2);
                    s.SetItemAutoDoc(itemAutoDoc2);
                    s.SetItemBioPlastic(itemBioPlastic2);
                    s.SetItemBloodBag(itemBloodBag2);
                    s.SetItemBoneSaw(itemBoneSaw2);
                    s.SetItemC4(itemC42);
                    s.SetItemCaltrops(itemCaltrops2);
                    s.SetItemCarrierBird(itemCarrierBird2);
                    s.SetItemChildsDrawing(itemChildsDrawing2);
                    s.SetItemCigarettes(itemCigarettes2);
                    s.SetItemClimbingGear(itemClimbingGear2);
                    s.SetItemDecoy(itemDecoy2);
                    s.SetItemDogTags(itemDogTags2);
                    s.SetItemEMPGrenade(itemEMPGrenade2);
                    s.SetItemEncryptedDrive(itemEncryptedDrive2);
                    s.SetItemEpiPen(itemEpiPen2);
                    s.SetItemExosuit(itemExosuit2);
                    s.SetItemFaradayPack(itemFaradayPack2);
                    s.SetItemForeignBook(itemForeignBook2);
                    s.SetItemGeigerCalibrator(itemGeigerCalibrator2);
                    s.SetItemGlowingMushroom(itemGlowingMushroom2);
                    s.SetItemGoldBars(itemGoldBars2);
                    s.SetItemGuitar(itemGuitar2);
                    s.SetItemHeirloom(itemHeirloom2);
                    s.SetItemIBeam(itemIBeam2);
                    s.SetItemImpureIodine(itemImpureIodine2);
                    s.SetItemJuggernautArmor(itemJuggernautArmor2);
                    s.SetItemKevlarVest(itemKevlarVest2);
                    s.SetItemKeycards(itemKeycards2);
                    s.SetItemLandmine(itemLandmine2);
                    s.SetItemLeadApron(itemLeadApron2);
                    s.SetItemLiquidStitches(itemLiquidStitches2);
                    s.SetItemMaggots(itemMaggots2);
                    s.SetItemMilGasMask(itemMilGasMask2);
                    s.SetItemMutantGland(itemMutantGland2);
                    s.SetItemNanites(itemNanites2);
                    s.SetItemNightVision(itemNightVision2);
                    s.SetItemPackMule(itemPackMule2);
                    s.SetItemPasswordNote(itemPasswordNote2);
                    s.SetItemPhotoAlbum(itemPhotoAlbum2);
                    s.SetItemPotassiumIodide(itemPotassiumIodide2);
                    s.SetItemPresidentialSeal(itemPresidentialSeal2);
                    s.SetItemPrussianBlue(itemPrussianBlue2);
                    s.SetItemRTGBattery(itemRTGBattery2);
                    s.SetItemSeedLedger(itemSeedLedger2);
                    s.SetItemShockCollar(itemShockCollar2);
                    s.SetItemSnowshoes(itemSnowshoes2);
                    s.SetItemSurgicalTubing(itemSurgicalTubing2);
                    s.SetItemTearGas(itemTearGas2);
                    s.SetItemTeddyBear(itemTeddyBear2);
                    s.SetItemTrashHazmat(itemTrashHazmat2);
                    s.SetItemUndeliveredMail(itemUndeliveredMail2);
                    s.SetItemVacuumTubes(itemVacuumTubes2);
                    s.SetItemVinylCollection(itemVinylCollection2);
                    s.SetItemVitamins(itemVitamins2);
                    s.SetItemWalkieTalkie(itemWalkieTalkie2);
                    s.SetItemWastelandSoap(itemWastelandSoap2);
                    s.SetItemWaterTabs(itemWaterTabs2);
                    s.SetItemWeldingGoggles(itemWeldingGoggles2);
                    s.SetItemWristDosimeter(itemWristDosimeter2);
                    s.SetLightningStrikesSystem(lightningStrikesSystem2);
                    s.SetLocationStateRuinSystem(locationStateRuinSystem2);
                    s.SetLocationArcade(locationArcade2);
                    s.SetLocationSlaveMarket(locationSlaveMarket2);
                    s.SetLocationStrandedYacht(locationStrandedYacht2);
                    s.SetMapAquifer(mapAquifer2);
                    s.SetMobileCampSystem(mobileCampSystem2);
                    s.SetMoralDilemmaSystem(moralDilemmaSystem2);
                    s.SetNPCAddictsPassive(nPCAddictsPassive2);
                    s.SetNPCAggroScavengers(nPCAggroScavengers2);
                    s.SetNPCAggroTrader(nPCAggroTrader2);
                    s.SetNPCBandits(nPCBandits2);
                    s.SetNPCBlackOps(nPCBlackOps2);
                    s.SetNPCBroker(nPCBroker2);
                    s.SetNPCCannibals(nPCCannibals2);
                    s.SetNPCChemScientists(nPCChemScientists2);
                    s.SetNPCCityResidents(nPCCityResidents2);
                    s.SetNPCCollaborators(nPCCollaborators2);
                    s.SetNPCConscripts(nPCConscripts2);
                    s.SetNPCDesperateFamily(nPCDesperateFamily2);
                    s.SetNPCDrunksAggro(nPCDrunksAggro2);
                    s.SetNPCHomeless(nPCHomeless2);
                    s.SetNPCLonePsychopath(nPCLonePsychopath2);
                    s.SetNPCLooters(nPCLooters2);
                    s.SetNPCMercenaries(nPCMercenaries2);
                    s.SetNPCMilitaryPatrol(nPCMilitaryPatrol2);
                    s.SetNPCPassiveScavengers(nPCPassiveScavengers2);
                    s.SetNPCPassiveTrader(nPCPassiveTrader2);
                    s.SetNPCPsychopathPair(nPCPsychopathPair2);
                    s.SetNPCRebelMilitia(nPCRebelMilitia2);
                    s.SetNPCRebelModerates(nPCRebelModerates2);
                    s.SetNPCRebelSnipers(nPCRebelSnipers2);
                    s.SetNPCRebelZealots(nPCRebelZealots2);
                    s.SetNPCSlavers(nPCSlavers2);
                    s.SetNPCSpecOps(nPCSpecOps2);
                    s.SetNPCSurvivalists(nPCSurvivalists2);
                    s.SetNPCTaxCollector(nPCTaxCollector2);
                    s.SetNPCTerrorists(nPCTerrorists2);
                    s.SetNPCTheNegotiator(nPCTheNegotiator2);
                    s.SetNPCTheOld(nPCTheOld2);
                    s.SetNPCTheParents(nPCTheParents2);
                    s.SetNPCTravelingCouple(nPCTravelingCouple2);
                    s.SetNeedleSterilizationSystem(needleSterilizationSystem2);
                    s.SetNightScavengeSystem(nightScavengeSystem2);
                    s.SetNodeAutomatedArmory(nodeAutomatedArmory2);
                    s.SetNodeGhostShip(nodeGhostShip2);
                    s.SetNodeMutantHive(nodeMutantHive2);
                    s.SetNodePlayerBank(nodePlayerBank2);
                    s.SetNodeSector7G(nodeSector7G2);
                    s.SetNodeSporeHive(nodeSporeHive2);
                    s.SetPetFeralCat(petFeralCat2);
                    s.SetProjectBioReactor(projectBioReactor2);
                    s.SetProjectDeepWell(projectDeepWell2);
                    s.SetProjectElevator(projectElevator2);
                    s.SetProjectMinecart(projectMinecart2);
                    s.SetProjectRadioArray(projectRadioArray2);
                    s.SetProjectSurfaceDome(projectSurfaceDome2);
                    s.SetProstheticCraftingSystem(prostheticCraftingSystem2);
                    s.SetSeismicVentsSystem(seismicVentsSystem2);
                    s.SetSevereFrostbiteSystem(severeFrostbiteSystem2);
                    s.SetShelterEventCaravanAmbush(shelterEventCaravanAmbush2);
                    s.SetShelterEventFalseCure(shelterEventFalseCure2);
                    s.SetShelterEventRansom(shelterEventRansom2);
                    s.SetShelterEventRefugees(shelterEventRefugees2);
                    s.SetShelterEventTheMirror(shelterEventTheMirror2);
                    s.SetShelterEventTribute(shelterEventTribute2);
                    s.SetSkirmishBandit_vs_Terror(skirmishBandit_vs_Terror2);
                    s.SetSkirmishMil_vs_Rebel(skirmishMil_vs_Rebel2);
                    s.SetSkirmishMil_vs_Terror(skirmishMil_vs_Terror2);
                    s.SetSkirmishRebel_vs_Bandit(skirmishRebel_vs_Bandit2);
                    s.SetSkirmishRebel_vs_Terror(skirmishRebel_vs_Terror2);
                    s.SetTetanusAfflictionSystem(tetanusAfflictionSystem2);
                    s.SetTimeSystem(timeSystemSys2);
                    s.SetToothDecaySystem(toothDecaySystem2);
                    s.SetTraderPlagueConvoy(traderPlagueConvoy2);
                    s.SetTraitAnthropophobia(traitAnthropophobia2);
                    s.SetTraitClairvoyant(traitClairvoyant2);
                    s.SetTraitGenerationalTrauma(traitGenerationalTrauma2);
                    s.SetTraitInheritedGenetics(traitInheritedGenetics2);
                    s.SetTraitMatriarch(traitMatriarch2);
                    s.SetTraitPTSD(traitPTSD2);
                    s.SetUIEventBlurredVision(uIEventBlurredVision2);
                    s.SetUIEventCorruptionScare(uIEventCorruptionScare2);
                    s.SetUIEventFalseInventory(uIEventFalseInventory2);
                    s.SetUIEventGhostRadio(uIEventGhostRadio2);
                    s.SetUIEventHacking(uIEventHacking2);
                    s.SetUIEventLowPower(uIEventLowPower2);
                    s.SetUIEventMapRot(uIEventMapRot2);
                    s.SetUIEventPhantomBlip(uIEventPhantomBlip2);
                    s.SetVehicleStrandingSystem(vehicleStrandingSystem2);
                    s.SetVehicleSystem(vehicleSystem2);
                    s.SetVehicleArmoredTruck(vehicleArmoredTruck2);
                    s.SetVehicleMotorcycle(vehicleMotorcycle2);
                    s.SetVehicleRowboat(vehicleRowboat2);
                    s.SetVisionLossSystem(visionLossSystem2);
                    s.SetVisitorRNGSystem(visitorRNGSystem2);
                    s.SetVisitorAbandonedState(visitorAbandonedState2);
                    s.SetVisitorChurchHostile(visitorChurchHostile2);
                    s.SetVisitorChurchSanctuary(visitorChurchSanctuary2);
                    s.SetVisitorExplodedState(visitorExplodedState2);
                    s.SetVisitorFleeingHorde(visitorFleeingHorde2);
                    s.SetVisitorHospitalPatients(visitorHospitalPatients2);
                    s.SetVisitorHospitalStaff(visitorHospitalStaff2);
                    s.SetVisitorMilTrainingYard(visitorMilTrainingYard2);
                    s.SetVisitorQuestFaction(visitorQuestFaction2);
                    s.SetVisitorRebelTrainingYard(visitorRebelTrainingYard2);
                    s.SetWeaponChainsaw(weaponChainsaw2);
                    s.SetWeaponFlamethrower(weaponFlamethrower2);
                    s.SetWeaponHMG(weaponHMG2);
                    s.SetWeaponRPG(weaponRPG2);
                    s.SetWorldEventDeforestation(worldEventDeforestation2);
                    s.SetWorldEventFinalWinter(worldEventFinalWinter2);
                    s.SetWorldEventFissure(worldEventFissure2);
                    s.SetWorldEventGreatFamine(worldEventGreatFamine2);
                    s.SetWorldEventMegafauna(worldEventMegafauna2);
                });
                Assert.IsTrue(ss2.Load("slot"));
                Assert.IsNotNull(actionAdministerPlacebo2.CaptureState());
                Assert.IsNotNull(actionBarricadeDoor2.CaptureState());
                Assert.IsNotNull(actionBoilBatteries2.CaptureState());
                Assert.IsNotNull(actionBroadcastPropaganda2.CaptureState());
                Assert.IsNotNull(actionBurnCharcoal2.CaptureState());
                UnityEngine.Object.DestroyImmediate(ScriptableObject.CreateInstance<NeedsProfile>()); // profile cleaned via MakeSave scope
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Test]
        public void RemainingComplex_CaptureRestore()
        {
            AssertRoundTrips(() => new Action_Crawlspace(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "caveInChance", "biteChance", "lootMin", "lootMax");
            AssertRoundTrips(() => new Action_Play(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "moraleGenerated", "noiseGenerated", "quietRulesActive");
            AssertRoundTrips(() => new Action_SlaughterPet(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "foodYield", "traumaIsPermanent", "hasBeenUsed");
            AssertRoundTrips(() => new Action_TeachChild(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "skillXpPerHour", "adultFatiguePerHour");
            AssertRoundTrips(() => new Action_TellStories(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "actionId", "anxietyFreezeHours", "requiresBooks", "adultTimeCostHours", "frozenChildIds", "frozenExpiryTimestamps");
            AssertRoundTrips(() => new Item_AshGoat("item_ash_goat"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "noiseLevel", "smellLevel", "milkPerDay", "isFed", "totalMilkProduced");
            AssertRoundTrips(() => new Item_Boots(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Item_LiveTrap("item_live_trap"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "plagueChance", "isSet", "ratsCapturedToday");
            AssertRoundTrips(() => new Item_MutantChicken("item_mutant_chicken"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "isFed", "isFeral", "eggProductionPerDay", "eggsLaidToday");
            AssertRoundTrips(() => new Item_Toys(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "itemId", "baseTradeValue", "factionMultiplier", "cultDestroys");
            AssertRoundTrips(() => new Trait_AshTongue(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Trait_Kleptomaniac(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Trait_Mascot(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "traitId", "resilienceBuffMult", "requiresChildWellFed", "requiresChildHappy");
            AssertRoundTrips(() => new Trait_StuntedEmpathy(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "traitId", "deathWitnessThreshold", "deathsWitnessed", "moraleShattered", "guaranteedSociopathOnAdult", "trackedChildIds", "trackedDeathCounts", "trackedShatteredFlags");
            AssertRoundTrips(() => new Trait_Superstitious(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Affliction_BunkerFever(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "keys", "values");
            AssertRoundTrips(() => new Affliction_ZoonoticFlu(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "afflictionId", "contagionRate", "spreadsViaVents", "isQuarantined", "isInfected", "sourceAnimal", "infectedSurvivorId");
            AssertRoundTrips(() => new Module_RationLock(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "moduleId", "dailyCap", "resentmentPerDay");
            AssertRoundTrips(() => new Node_Orphanage(), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "nodeId", "childCount", "hasBeenVisited");
            AssertRoundTrips(() => new Pet_GuardDog("pet_guard_dog"), x => x.CaptureState(), (x, s) => x.RestoreState(s),
                "petId", "meatRationsRequired", "isMalnourished", "canFight", "meatFedToday", "veggiesFedToday");
        }

        [Test]
        public void FalloutStormHazard_CaptureRestore()
        {
            var weather = new AtomicWar._Game.Environment.WeatherSystem(null, 3);
            // FalloutStormHazardSystem.RestoreState is an intentional no-op (dormant
            // ghost, not Boot/Save wired) -- no field survives a round trip.
            AssertRoundTrips(() => new FalloutStormHazardSystem(weather), x => x.CaptureState(), (x, s) => x.RestoreState(s));
        }
    }
}
