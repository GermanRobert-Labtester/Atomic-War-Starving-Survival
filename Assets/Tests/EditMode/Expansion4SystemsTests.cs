using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class Expansion4SystemsTests
    {
        [Test]
        public void StructuralEntropy_HighMoistureAndCo2_AcceleratesRebarCorrosionAndTriggersSpalling()
        {
            var entropySystem = new StructuralEntropySystem();
            var room = new ShelterRoom("bunkhouse", null)
            {
                Humidity = 0.9f,
                Co2Ppm = 1200f
            };
            entropySystem.RegisterRoom(room);

            var survivor = new Survivor { Id = "s1", DisplayName = "Test Worker", CurrentRoomId = "bunkhouse" };
            var survivors = new List<Survivor> { survivor };
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needsSystem = new NeedsSystem(needsProfile);
            entropySystem.BindDependencies(() => survivors, needsSystem);

            // Tick for 500 hours to simulate extreme material degradation
            for (int i = 0; i < 500; i++)
            {
                entropySystem.Tick(1.0f);
            }

            Assert.IsTrue(room.RebarCorrosion > 0f, "RebarCorrosion should tick up over time.");
            
            // Force spalling trigger to verify spalling event payload and counterplay
            entropySystem.TriggerSpalling(room);

            Assert.IsTrue(room.IsSpalling, "Room should be marked as spalling.");
            Assert.IsTrue(room.MaterialShielding < 1.0f, "Material shielding should drop after spalling.");
            Assert.AreEqual(80f, survivor.Needs.Health, "Occupant should take spalling health damage.");

            // Verify Epoxy Injector counterplay
            var engineer = new Survivor { Id = "s2", DisplayName = "Architect", ArchetypeId = "survivor_concrete_boss" };
            bool repaired = entropySystem.InjectEpoxy(room, engineer);

            Assert.IsTrue(repaired, "InjectEpoxy should succeed.");
            Assert.AreEqual(0f, room.RebarCorrosion, "RebarCorrosion should reset to 0 after epoxy injection.");
            Assert.IsFalse(room.IsSpalling, "Room should no longer be spalling after repair.");
        }

        [Test]
        public void OzoneScourge_FalseSpring_AppliesOpticAndBlisteringAfflictions()
        {
            var seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            var weatherSystem = new WeatherSystem(seasonProfile, 42);
            weatherSystem.ForceWeather(WeatherKind.FalseSpring);

            var scourgeSystem = new OzoneScourgeSystem(weatherSystem);
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needsSystem = new NeedsSystem(needsProfile);
            scourgeSystem.BindNeedsSystem(needsSystem);

            Assert.AreEqual(OzoneScourgeSystem.FalseSpringUVIndex, scourgeSystem.GetAmbientUV(), "Ambient UV should spike during FalseSpring.");
            Assert.IsTrue(scourgeSystem.IsOzoneScourgeActive());

            // Unshielded camera inspection test
            var observer = new Survivor { Id = "obs1", DisplayName = "Observer" };
            bool feedOk = scourgeSystem.InspectCameraFeed(hasWeldersGlassFilter: false, observer);

            Assert.IsFalse(feedOk, "Unshielded feed inspection should fail.");
            Assert.IsTrue(observer.HasTrait(OzoneScourgeSystem.Affliction_SnowBlindness), "Observer should gain SnowBlindness.");

            // Unshielded surface expedition test
            var scavenger = new Survivor { Id = "scav1", DisplayName = "Scavenger" };
            scourgeSystem.EvaluateExpeditionSurfaceExposure(scavenger, hasLeadVisor: false, hasAshGhillie: false);

            Assert.IsTrue(scavenger.HasTrait(OzoneScourgeSystem.Affliction_UV_Blistering), "Scavenger should suffer UV Blistering.");
            Assert.IsTrue(scavenger.HasTrait(OzoneScourgeSystem.Affliction_CornealBurn), "Scavenger should suffer Corneal Burn.");
            Assert.AreEqual(85f, scavenger.Needs.Health);
        }

        [Test]
        public void GenerationalPsychology_ChildAgesUp_FiresComingOfAgeAndBunkerBornTraits()
        {
            var genSystem = new GenerationalPsychologySystem();
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needsSystem = new NeedsSystem(needsProfile);
            var mentalBreakSystem = new MentalBreakSystem();
            genSystem.BindDependencies(needsSystem, mentalBreakSystem);

            var child = new Survivor
            {
                Id = "c1",
                DisplayName = "Child 1",
                IsChild = true,
                Age = 12
            };
            var survivors = new List<Survivor> { child };

            bool eventFired = false;
            genSystem.OnComingOfAge += (sv, evt) =>
            {
                eventFired = true;
                Assert.IsTrue(evt.IsBunkerBorn);
                Assert.AreEqual("c1", evt.SurvivorId);
            };

            genSystem.DailyTick(365, survivors);

            Assert.IsTrue(eventFired, "ComingOfAge event should be raised.");
            Assert.IsFalse(child.IsChild, "Survivor should no longer be marked child.");
            Assert.IsTrue(child.IsBunkerBorn, "Survivor should be marked Bunker-Born.");
            Assert.IsTrue(child.HasTrait(GenerationalPsychologySystem.Trait_AgoraphobiaSevere));
            Assert.IsTrue(child.HasTrait(GenerationalPsychologySystem.Trait_ArtifactReverence));

            // Surface deployment of Bunker-Born triggers panic attack
            bool deployed = genSystem.EvaluateExpeditionDeployment(child);
            Assert.IsFalse(deployed, "Deployment of agoraphobic bunker-born survivor should fail.");
            Assert.AreEqual(GenerationalPsychologySystem.MentalBreak_PanicAttack, child.currentMentalBreakId);
        }

        [Test]
        public void LetheProtocol_DepletionTriggersWakingSickness_AndMoralChoicesWork()
        {
            var letheSystem = new LetheProtocolSystem();
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needsSystem = new NeedsSystem(needsProfile);
            var mentalBreakSystem = new MentalBreakSystem();
            letheSystem.BindDependencies(needsSystem, mentalBreakSystem);

            var s1 = new Survivor { Id = "s1", DisplayName = "Survivor 1" };
            var survivors = new List<Survivor> { s1 };

            // Consume doses until reservoir drops below critical 20% red line
            letheSystem.ConsumeAmnesticDose(600f, survivors);

            Assert.IsTrue(letheSystem.ReservoirLevel <= LetheProtocolSystem.CriticalRedLineLevel);
            Assert.IsTrue(letheSystem.IsWakingSicknessActive, "Waking Sickness should activate when reservoir drops below 20%.");
            Assert.IsTrue(s1.HasTrait(LetheProtocolSystem.Affliction_Hyperthymesia), "Survivor should gain Hyperthymesia trauma.");

            // Choice 1: Synthesize the Lie
            letheSystem.SynthesizeLie(3f);
            Assert.IsTrue(letheSystem.IsSynthesizedLieActive);
            Assert.IsFalse(letheSystem.IsWakingSicknessActive);

            // Choice 2: Embrace the Waking
            letheSystem.EmbraceTheWaking(survivors);
            Assert.IsTrue(letheSystem.IsEmbraced);
            Assert.IsTrue(s1.HasTrait(LetheProtocolSystem.Trait_HardenedSoul));

            // Choice 3: Lobotomy option
            var volatileSurvivor = new Survivor { Id = "v1", DisplayName = "Volatile Survivor" };
            bool lobotomized = letheSystem.PerformLobotomy(volatileSurvivor);

            Assert.IsTrue(lobotomized);
            Assert.IsTrue(volatileSurvivor.HasTrait(LetheProtocolSystem.Trait_Lobotomized));
            Assert.AreEqual(150f, volatileSurvivor.BaseMaxStamina);
        }

        [Test]
        public void Factions_Expansion4_SunSeekersOsteophagesArchivistsFunctionality()
        {
            var sunSeekers = new NPC_SunSeekers();
            Assert.IsTrue(sunSeekers.CanTradeDuringWeather(WeatherKind.FalseSpring));
            Assert.IsFalse(sunSeekers.CanTradeDuringWeather(WeatherKind.Clear));

            var osteophages = new NPC_Osteophages();
            bool processed = osteophages.ProcessScrapRecycling("salvaged_tech_trash", out string resultItem, out int resultAmount);
            Assert.IsTrue(processed);
            Assert.AreEqual("item_copper_wire", resultItem);
            Assert.AreEqual(3, resultAmount);

            var archivists = new NPC_Archivists();
            bool tithed = archivists.SubmitAncestralTithe("item_pre_war_photo_album", out float moraleBonus, out string rewardItem);
            Assert.IsTrue(tithed);
            Assert.AreEqual(25f, moraleBonus);
            Assert.AreEqual("item_encrypted_drive", rewardItem);
        }

        [Test]
        public void UI_Expansion4Components_InitializeAndTriggerCues()
        {
            var goWireframe = new GameObject("Wireframe");
            var wireframe = goWireframe.AddComponent<StructuralStressWireframe>();

            var concreteBoss = new Survivor { Id = "cb1", DisplayName = "Boss", ArchetypeId = "survivor_concrete_boss" };
            wireframe.OnSurvivorSelected(concreteBoss);
            Assert.IsTrue(wireframe.IsOverlayActive, "Concrete Boss selection should activate structural stress overlay.");

            var goVignette = new GameObject("Vignette");
            var vignette = goVignette.AddComponent<MemoryFlashVignette>();

            var hyperthymesiaSurvivor = new Survivor { Id = "h1", DisplayName = "Patient" };
            hyperthymesiaSurvivor.Traits.Add("trait_hyperthymesia");

            bool tinnitusFired = false;
            vignette.OnTinnitusTriggered += () => tinnitusFired = true;
            vignette.TriggerMemoryFlash(hyperthymesiaSurvivor);

            Assert.IsTrue(vignette.IsFlashing);
            Assert.IsTrue(tinnitusFired);

            var goOzone = new GameObject("OzoneUI");
            var ozoneOverlay = goOzone.AddComponent<OzoneScourgeOverlay>();
            var seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            var weatherSystem = new WeatherSystem(seasonProfile, 42);
            weatherSystem.ForceWeather(WeatherKind.FalseSpring);
            var ozoneSystem = new OzoneScourgeSystem(weatherSystem);
            ozoneOverlay.BindOzoneSystem(ozoneSystem);

            ozoneOverlay.OnCameraFeedViewed(hasWeldersGlassFilter: false, deltaTime: 3.0f);
            Assert.IsTrue(ozoneOverlay.IsWarningActive, "Optic degradation warning should trigger when viewing unshielded feed.");

            var goGauge = new GameObject("Gauge");
            var dripGauge = goGauge.AddComponent<LetheDripGauge>();
            var letheSystem = new LetheProtocolSystem();
            dripGauge.BindLetheSystem(letheSystem);

            letheSystem.ConsumeAmnesticDose(600f, null);
            Assert.IsTrue(dripGauge.IsRedLineWarning, "Sight gauge should enter red line warning state when amnestic reservoir is low.");
            Assert.AreEqual(0.25f, dripGauge.GetDropletSpeedMultiplier());

            Object.DestroyImmediate(goWireframe);
            Object.DestroyImmediate(goVignette);
            Object.DestroyImmediate(goOzone);
            Object.DestroyImmediate(goGauge);
        }
    }
}
