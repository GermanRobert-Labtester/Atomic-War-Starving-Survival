using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Day-30 Flashpoint Choreographer: buildup day side effects
    /// (idempotent across save/load), barter-only trader panic,
    /// choreography state machine (step order, delays, accessibility
    /// override), and save/load round-trip.
    /// </summary>
    [TestFixture]
    public class FlashpointChoreographerTests
    {
        private const float Eps = 1e-4f;

        // EventBus is a static singleton; we must clear subscribers between
        // tests so one test's handlers don't fire in another.
        [TearDown]
        public void TearDown()
        {
            EventBus.Clear();
        }

        // -------------------------------------------------------------------
        // Buildup day side effects
        // -------------------------------------------------------------------

        [Test]
        public void BuildupDay_FiresEventAndAppliesEconomyModifier_OnDayTick()
        {
            var sequence = MakeSequence();
            var economyModifier = new FlashpointEconomyModifier
            {
                id = "trader_panic",
                enableBarterOnlyMode = true,
                acceptedItemIds = new List<string> { "iodine_pills", "clean_water", "fuel" },
                demandSpikes = new List<FlashpointDemandSpike>
                {
                    new FlashpointDemandSpike { itemId = "iodine_pills", multiplierDelta = 2f },
                    new FlashpointDemandSpike { itemId = "clean_water", multiplierDelta = 1.5f }
                }
            };
            sequence.economyModifiers.Add(economyModifier);
            sequence.buildupDays.Add(new FlashpointBuildupDay
            {
                day = 26,
                audioCueId = "audio_cue_trader_panic",
                economyModifierId = "trader_panic",
                worldFlagKey = "flashpoint_buildup_day_26"
            });

            var economy = new DynamicEconomySystem();
            var systems = MakeSystems(economySystem: economy);
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            FlashpointBuildupDayEntered captured = default;
            int fired = 0;
            EventBus.Subscribe<FlashpointBuildupDayEntered>(e => { captured = e; fired++; });

            ch.OnDayTick(26);

            Assert.That(fired, Is.EqualTo(1), "Event must fire exactly once");
            Assert.That(captured.Day, Is.EqualTo(26));
            Assert.That(captured.AudioCueId, Is.EqualTo("audio_cue_trader_panic"));
            Assert.That(captured.EconomyModifierId, Is.EqualTo("trader_panic"));
            Assert.That(captured.WorldFlagKey, Is.EqualTo("flashpoint_buildup_day_26"));
            Assert.That(economy.BarterOnlyMode, Is.True, "Barter-only mode must be enabled");
            Assert.That(economy.BarterOnlyAcceptedItemIds, Is.EquivalentTo(
                new[] { "iodine_pills", "clean_water", "fuel" }));
            Assert.That(economy.GetDemandMultiplier("iodine_pills"), Is.EqualTo(3f).Within(Eps),
                "Demand must spike by 2 from the base 1");
            Assert.That(economy.GetDemandMultiplier("clean_water"), Is.EqualTo(2.5f).Within(Eps));
        }

        [Test]
        public void BuildupDay_IsIdempotent_AcrossRepeatedDayTicks()
        {
            var sequence = MakeSequence();
            sequence.buildupDays.Add(new FlashpointBuildupDay
            {
                day = 27,
                audioCueId = "audio_cue_military_codes",
                economyModifierId = null,
                worldFlagKey = "flashpoint_buildup_day_27"
            });

            var economy = new DynamicEconomySystem();
            var systems = MakeSystems(economySystem: economy);
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            int fired = 0;
            EventBus.Subscribe<FlashpointBuildupDayEntered>(_ => fired++);

            ch.OnDayTick(27);
            ch.OnDayTick(27);
            ch.OnDayTick(27);

            Assert.That(fired, Is.EqualTo(1), "Same-day ticks must not double-apply");
            Assert.That(ch.BuildupDaysProcessed, Is.EquivalentTo(new[] { 27 }));
        }

        [Test]
        public void BuildupDay_WithNoMatchingEntry_DoesNotFire()
        {
            var sequence = MakeSequence();
            sequence.buildupDays.Add(new FlashpointBuildupDay
            {
                day = 29,
                audioCueId = "audio_cue_silence",
                economyModifierId = null,
                worldFlagKey = "flashpoint_buildup_day_29"
            });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            int fired = 0;
            EventBus.Subscribe<FlashpointBuildupDayEntered>(_ => fired++);

            ch.OnDayTick(24); // not configured
            ch.OnDayTick(25); // not configured
            ch.OnDayTick(26); // not configured

            Assert.That(fired, Is.EqualTo(0));
            Assert.That(ch.BuildupDaysProcessed.Count, Is.EqualTo(0));
        }

        // -------------------------------------------------------------------
        // Choreography state machine
        // -------------------------------------------------------------------

        [Test]
        public void Choreography_StartsOnNuclearExchange_AndFiresStartedEvent()
        {
            var sequence = MakeSequence();
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            int startedFired = 0;
            EventBus.Subscribe<FlashpointChoreographyStarted>(_ => startedFired++);

            Assert.That(ch.IsChoreographyActive, Is.False);
            ch.OnNuclearExchange();
            Assert.That(ch.IsChoreographyActive, Is.True);
            Assert.That(startedFired, Is.EqualTo(1));
        }

        [Test]
        public void Choreography_FiresStepsInOrder_AfterAccumulatedDelays()
        {
            var sequence = MakeSequence();
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "sirens", delayFromPreviousSeconds = 0f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "weather_shift", delayFromPreviousSeconds = 2f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "radiation_hud_unlock", delayFromPreviousSeconds = 1f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            var firedOrder = new List<string>();
            EventBus.Subscribe<FlashpointSirensSpooling>(_ => firedOrder.Add("sirens"));
            EventBus.Subscribe<FlashpointWeatherShifted>(_ => firedOrder.Add("weather_shift"));
            EventBus.Subscribe<FlashpointRadiationHudUnlocked>(_ => firedOrder.Add("radiation_hud_unlock"));

            ch.OnNuclearExchange();
            // Step 0 fires immediately (delay 0).
            ch.Tick(0.01f);
            Assert.That(firedOrder, Is.EqualTo(new[] { "sirens" }));

            // Step 1 fires after 2s of accumulated real time.
            ch.Tick(1f);
            Assert.That(firedOrder, Is.EqualTo(new[] { "sirens" }));
            ch.Tick(1.5f);
            Assert.That(firedOrder, Is.EqualTo(new[] { "sirens", "weather_shift" }));

            // Step 2 fires 1s after step 1.
            ch.Tick(0.5f);
            Assert.That(firedOrder, Is.EqualTo(new[] { "sirens", "weather_shift" }));
            ch.Tick(0.6f);
            Assert.That(firedOrder, Is.EqualTo(new[] { "sirens", "weather_shift", "radiation_hud_unlock" }));

            // Step 3 (complete) fires after the configured 0s delay.
            ch.Tick(0.01f);
            Assert.That(ch.IsChoreographyCompleted, Is.True);
        }

        [Test]
        public void Choreography_OnNuclearExchange_IsIdempotent()
        {
            var sequence = MakeSequence();
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "sirens", delayFromPreviousSeconds = 0f });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            int startedFired = 0;
            EventBus.Subscribe<FlashpointChoreographyStarted>(_ => startedFired++);

            ch.OnNuclearExchange();
            ch.OnNuclearExchange();
            ch.OnNuclearExchange();

            Assert.That(startedFired, Is.EqualTo(1));
        }

        [Test]
        public void Choreography_EmptySteps_MarksCompleteImmediately()
        {
            var sequence = MakeSequence(); // no steps
            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            ch.OnNuclearExchange();

            Assert.That(ch.IsChoreographyCompleted, Is.True);
        }

        [Test]
        public void Choreography_NullSequence_DoesNotThrow()
        {
            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(null, () => false, systems, () => false);

            Assert.DoesNotThrow(() => ch.OnDayTick(25));
            Assert.DoesNotThrow(() => ch.OnNuclearExchange());
            Assert.That(ch.IsChoreographyCompleted, Is.True,
                "Without a sequence, the choreography should short-circuit to complete");
        }

        // -------------------------------------------------------------------
        // Accessibility override
        // -------------------------------------------------------------------

        [Test]
        public void Choreography_FlashStep_UsesSafeDuration_WhenAccessibilityEnabled()
        {
            var sequence = MakeSequence();
            sequence.accessibility.defaultFlashSeconds = 4f;
            sequence.accessibility.safeFlashSeconds = 1.5f;
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "flash", delayFromPreviousSeconds = 0f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => true, systems, () => false); // safe ON

            FlashpointFlashStarted captured = default;
            EventBus.Subscribe<FlashpointFlashStarted>(e => captured = e);

            ch.OnNuclearExchange();
            ch.Tick(0.01f);

            Assert.That(captured.DurationSeconds, Is.EqualTo(1.5f).Within(Eps));
            Assert.That(captured.IsAccessibilitySafe, Is.True);
        }

        [Test]
        public void Choreography_FlashStep_UsesDefaultDuration_WhenAccessibilityDisabled()
        {
            var sequence = MakeSequence();
            sequence.accessibility.defaultFlashSeconds = 4f;
            sequence.accessibility.safeFlashSeconds = 1.5f;
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "flash", delayFromPreviousSeconds = 0f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false); // safe OFF

            FlashpointFlashStarted captured = default;
            EventBus.Subscribe<FlashpointFlashStarted>(e => captured = e);

            ch.OnNuclearExchange();
            ch.Tick(0.01f);

            Assert.That(captured.DurationSeconds, Is.EqualTo(4f).Within(Eps));
            Assert.That(captured.IsAccessibilitySafe, Is.False);
        }

        [Test]
        public void Choreography_ShockwaveStep_ReducesShakeAmplitude_WhenAccessibilityEnabled()
        {
            var sequence = MakeSequence();
            sequence.accessibility.safeShakeMultiplier = 0.5f;
            sequence.steps.Add(new FlashpointChoreographyStep
            {
                actionId = "shockwave",
                delayFromPreviousSeconds = 0f,
                cameraShakeAmplitude = 0.4f
            });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var systems = MakeSystems();

            var chSafe = new FlashpointChoreographer(sequence, () => true, systems, () => false);
            FlashpointShockwaveHit safeHit = default;
            EventBus.Subscribe<FlashpointShockwaveHit>(e => safeHit = e);
            chSafe.OnNuclearExchange();
            chSafe.Tick(0.01f);
            Assert.That(safeHit.Intensity, Is.EqualTo(0.2f).Within(Eps),
                "Safe mode must halve the camera-shake amplitude");

            EventBus.Clear();
            var chDefault = new FlashpointChoreographer(sequence, () => false, systems, () => false);
            FlashpointShockwaveHit defaultHit = default;
            EventBus.Subscribe<FlashpointShockwaveHit>(e => defaultHit = e);
            chDefault.OnNuclearExchange();
            chDefault.Tick(0.01f);
            Assert.That(defaultHit.Intensity, Is.EqualTo(0.4f).Within(Eps));
        }

        // -------------------------------------------------------------------
        // Save / load
        // -------------------------------------------------------------------

        [Test]
        public void Choreographer_SaveAndRestore_PreservesBuildupDaysAndStepIndex()
        {
            var sequence = MakeSequence();
            sequence.buildupDays.Add(new FlashpointBuildupDay { day = 25, audioCueId = "a", economyModifierId = null, worldFlagKey = "f25" });
            sequence.buildupDays.Add(new FlashpointBuildupDay { day = 26, audioCueId = "b", economyModifierId = null, worldFlagKey = "f26" });
            sequence.buildupDays.Add(new FlashpointBuildupDay { day = 27, audioCueId = "c", economyModifierId = null, worldFlagKey = "f27" });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "sirens", delayFromPreviousSeconds = 0f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 5f });

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => true);

            // Process buildup days 25, 26, 27.
            ch.OnDayTick(25);
            ch.OnDayTick(26);
            ch.OnDayTick(27);

            // Start the choreography and run one step.
            ch.OnNuclearExchange();
            ch.Tick(0.01f);

            // Capture state.
            var save = ch.CaptureState();
            Assert.That(save.BuildupDaysProcessed, Is.EquivalentTo(new[] { 25, 26, 27 }));
            Assert.That(save.ChoreographyStepIndex, Is.EqualTo(0));
            Assert.That(save.ChoreographyCompleted, Is.False);

            // Restore into a fresh instance.
            var ch2 = new FlashpointChoreographer(sequence, () => false, systems, () => true);
            ch2.RestoreState(save);
            Assert.That(ch2.BuildupDaysProcessed, Is.EquivalentTo(new[] { 25, 26, 27 }));

            // Re-applying the same buildup days must be a no-op.
            int fired = 0;
            EventBus.Subscribe<FlashpointBuildupDayEntered>(_ => fired++);
            ch2.OnDayTick(25);
            ch2.OnDayTick(26);
            ch2.OnDayTick(27);
            Assert.That(fired, Is.EqualTo(0), "Restored state must skip already-processed buildup days");
        }

        [Test]
        public void Choreographer_RestoreOnFreshInstance_MarksChoreographyActive()
        {
            // A save from a session where the exchange already fired should
            // immediately resume the choreography on next load, not wait
            // for a second OnNuclearExchange (which is guarded and won't fire).
            var sequence = MakeSequence();
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var save = new FlashpointChoreographerSave
            {
                ChoreographyStepIndex = -1,
                ChoreographyCompleted = false
            };

            var systems = MakeSystems();
            var ch = new FlashpointChoreographer(sequence, () => false, systems, hasFlashpointTriggered: () => true);
            ch.RestoreState(save);

            Assert.That(ch.IsChoreographyActive, Is.True,
                "Restore should mark the choreography as started because the exchange already fired");
        }

        // -------------------------------------------------------------------
        // EMP step side effects
        // -------------------------------------------------------------------

        [Test]
        public void Choreography_EmpStep_AppliesMoraleAndUnpausesSystems()
        {
            // Set up a real WeatherSystem + RadiationSystem and verify the
            // EMP step puts them in the post-flash state. Use a real Inventory
            // and Shelter so EMPEvent.ApplyGlobal works end-to-end.
            var sequence = MakeSequence();
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "emp", delayFromPreviousSeconds = 0f });
            sequence.steps.Add(new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f });

            var seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            seasonProfile.weatherCheckIntervalHours = 1f;
            seasonProfile.seasons = new[] {
                new SeasonWindow { id = "any", displayName = "Any", startDay = 0,
                    clearWeight = 1f, rainWeight = 0f, overcastWeight = 0f,
                    ashfallWeight = 0f, falloutStormWeight = 0f, blizzardWeight = 0f }
            };

            var inventory = new Inventory { Capacity = 10, MaxWeight = 50f };
            var geiger = ScriptableObject.CreateInstance<ItemDefinition>();
            geiger.id = "geiger_counter";
            geiger.type = ItemType.Device;
            geiger.empShielded = false;
            geiger.stackMax = 1;
            inventory.Add(geiger, 1);

            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });

            var radio = new RadioState { AvailableFuel = 10f };

            var weather = new WeatherSystem(seasonProfile, seed: 7) { RestrictToNonHazardWeather = true };
            weather.ForceWeather(WeatherKind.Clear); // pre-flash state

            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(needsProfile);
            var radiation = new RadiationSystem(needs) { IsPaused = true };

            var survivor = new Survivor { Id = "sv_test", DisplayName = "Test" };
            survivor.Needs.Morale = 80f;
            needs.Register(survivor);
            radiation.Register(survivor);

            var systems = new FlashpointChoreographerSystems
            {
                Inventory = inventory,
                Shelter = shelter,
                RadioState = radio,
                WeatherSystem = weather,
                RadiationSystem = radiation,
                EconomySystem = new DynamicEconomySystem(),
                Survivors = new[] { survivor },
                ExchangeMoraleHit = 25f
            };

            var ch = new FlashpointChoreographer(sequence, () => false, systems, () => false);

            FlashpointEmptiedDevices captured = default;
            EventBus.Subscribe<FlashpointEmptiedDevices>(e => captured = e);

            ch.OnNuclearExchange();
            ch.Tick(0.01f);

            Assert.That(captured.DevicesBroken, Is.EqualTo(1), "Unshielded geiger must be EMP'd");
            Assert.That(captured.ModulesDisabled, Is.EqualTo(1), "Unshielded air filtration must be disabled");
            Assert.That(captured.RadioDestroyed, Is.True);
            Assert.That(captured.MoraleHitApplied, Is.EqualTo(25f).Within(Eps));
            Assert.That(survivor.Needs.Morale, Is.EqualTo(55f).Within(Eps));
            Assert.That(weather.RestrictToNonHazardWeather, Is.False);
            Assert.That(weather.Current, Is.EqualTo(WeatherKind.Ashfall));
            Assert.That(radiation.IsPaused, Is.False);

            Object.DestroyImmediate(seasonProfile);
            Object.DestroyImmediate(geiger);
            Object.DestroyImmediate(needsProfile);
        }

        // -------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------

        private static FlashpointSequenceSO MakeSequence()
        {
            var so = ScriptableObject.CreateInstance<FlashpointSequenceSO>();
            so.sequenceId = "test";
            so.buildupDays = new List<FlashpointBuildupDay>();
            so.economyModifiers = new List<FlashpointEconomyModifier>();
            so.steps = new List<FlashpointChoreographyStep>();
            so.accessibility = new FlashpointAccessibilityOverrides();
            return so;
        }

        private static FlashpointChoreographerSystems MakeSystems(
            DynamicEconomySystem economySystem = null)
        {
            return new FlashpointChoreographerSystems
            {
                Inventory = new Inventory { Capacity = 10, MaxWeight = 50f },
                Shelter = new Shelter(),
                RadioState = new RadioState(),
                WeatherSystem = null,
                RadiationSystem = null,
                EconomySystem = economySystem ?? new DynamicEconomySystem(),
                Survivors = new List<Survivor>(),
                ExchangeMoraleHit = 25f
            };
        }
    }
}
