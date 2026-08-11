using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Dispatch UI contract: it presents real imported locations and delegates
    /// the action to LocationScavengingSystem rather than maintaining a second
    /// mission state. Core-provided block reasons must prevent the send.
    /// </summary>
    [TestFixture]
    public class ScavengeDispatchHudTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        [Test]
        public void DispatchSelected_StartsRealMission_AndReportsCompletion()
        {
            var location = MakeLocation();
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            catalog.locations.Add(location);
            _toDestroy.Add(catalog);

            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            survivor.Needs.Health = 82f;
            survivor.RadiationDose = 4f;
            var crew = new List<Survivor> { survivor };
            var system = new LocationScavengingSystem(null, new Inventory(), null, seed: 12);

            var go = new GameObject("ScavengeDispatchHudTests");
            _toDestroy.Add(go);
            var dispatch = go.AddComponent<ScavengeDispatchHUD>();
            dispatch.Bind(
                catalog,
                () => crew,
                _ => null,
                _ => "idle",
                () => "ACTIVE MISSIONS: none.",
                (_, __) => "--- PREFLIGHT ---\nRETURN: expected in 1h when the mission clock closes.\nPROJECTED DOSE: ~10 over 1h — surveyed estimate.",
                _ => "~10?",
                _ => "1 search slot · 66% baseline recovery each.");
            dispatch.OnDispatchRequested += (operative, destination) =>
            {
                if (system.StartMission(operative, destination))
                {
                    dispatch.ReportMissionStarted(operative.DisplayName, destination.displayName, destination.travelHours);
                    return true;
                }
                dispatch.ReportDispatchHeld("Dispatch rejected by the mission system.");
                return false;
            };
            system.OnMissionCompleted += (mission, loot) =>
                dispatch.ReportMissionCompleted(mission.LocationName, loot != null ? loot.Count : 0);

            dispatch.Open();

            StringAssert.Contains("Suburban House", dispatch.PanelSummary);
            StringAssert.Contains("rad ~10?", dispatch.PanelSummary);
            StringAssert.Contains("SUPPLY: 1 search slot", dispatch.PanelSummary);
            StringAssert.Contains("--- PREFLIGHT ---", dispatch.PanelSummary);
            StringAssert.Contains("PROJECTED DOSE: ~10", dispatch.PanelSummary);
            Assert.That(dispatch.CanDispatchSelected(out var reason), Is.True, reason);
            Assert.That(dispatch.DispatchSelected(), Is.True);
            Assert.That(system.ActiveMissions, Has.Count.EqualTo(1));
            StringAssert.Contains("DISPATCHED: Elena Vasquez", dispatch.LastOutcome);

            system.Tick(1f);

            Assert.That(system.ActiveMissions, Is.Empty);
            StringAssert.Contains("RETURNED: Suburban House", dispatch.LastOutcome);
        }

        [Test]
        public void DispatchSelected_ReturnsFalse_WhenCoreRejectsDespiteUiSideCheckPassing()
        {
            // Audit M-4: DispatchSelected() used to return true as soon as
            // OnDispatchRequested had a subscriber, regardless of what that
            // subscriber actually did. A rejection that only Core knows about
            // (StartScavengeMission failing, hatch sealed mid-flight, etc.)
            // must surface as false here, not be masked by the event firing.
            var location = MakeLocation();
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            catalog.locations.Add(location);
            _toDestroy.Add(catalog);

            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var crew = new List<Survivor> { survivor };

            var go = new GameObject("ScavengeDispatchHudReturnSemanticsTests");
            _toDestroy.Add(go);
            var dispatch = go.AddComponent<ScavengeDispatchHUD>();
            dispatch.Bind(
                catalog,
                () => crew,
                _ => null, // The UI-side check sees nothing wrong.
                _ => "idle",
                () => "ACTIVE MISSIONS: none.",
                (_, __) => string.Empty,
                _ => "?",
                _ => "?");
            dispatch.OnDispatchRequested += (operative, destinationLocation) =>
            {
                dispatch.ReportDispatchHeld("Dispatch rejected by the mission system.");
                return false; // Core rejects for a reason the UI never independently checked.
            };
            dispatch.Open();

            Assert.That(dispatch.DispatchSelected(), Is.False,
                "A subscriber-side rejection must propagate as false, not be masked by the event having fired.");
            StringAssert.Contains("HELD: Dispatch rejected", dispatch.LastOutcome);
        }

        [Test]
        public void OnMissionCompleted_DispatchPanelShowsSurvivorReady_DuringEventCallback()
        {
            // Audit M-3: Tick() used to fire OnMissionCompleted before removing
            // the mission from _active. GameBootstrap.RefreshMapKnowledgeHUD is
            // wired directly to OnMissionCompleted and calls
            // ScavengeDispatchHUD.Refresh(), whose block-reason delegate scans
            // ActiveMissions -- so the panel would report the just-returned
            // survivor as "already scavenging" until some later, unrelated
            // refresh corrected it.
            var location = MakeLocation();
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            catalog.locations.Add(location);
            _toDestroy.Add(catalog);

            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var crew = new List<Survivor> { survivor };
            var system = new LocationScavengingSystem(null, new Inventory(), null, seed: 12);

            var go = new GameObject("ScavengeDispatchHudEventOrderingTests");
            _toDestroy.Add(go);
            var dispatch = go.AddComponent<ScavengeDispatchHUD>();
            dispatch.Bind(
                catalog,
                () => crew,
                // Mirrors GameBootstrap.GetScavengeDispatchBlockReason's core check.
                candidate =>
                {
                    var active = system.ActiveMissions;
                    for (int i = 0; i < active.Count; i++)
                        if (active[i] != null && active[i].SurvivorId == candidate.Id)
                            return "Operative is already scavenging.";
                    return null;
                },
                _ => "idle",
                () => "ACTIVE MISSIONS: none.",
                (_, __) => string.Empty,
                _ => "?",
                _ => "?");

            // Mirrors GameBootstrap: OnMissionCompleted -> RefreshMapKnowledgeHUD -> dispatch.Refresh().
            system.OnMissionCompleted += (mission, loot) => dispatch.Refresh();

            Assert.That(system.StartMission(survivor, location), Is.True);
            system.Tick(1f);

            StringAssert.Contains("STATUS: READY", dispatch.PanelSummary);
            StringAssert.DoesNotContain("already scavenging", dispatch.PanelSummary);
        }

        [Test]
        public void DispatchSelected_WhenCoreReportsSealedHatch_HoldsWithoutMission()
        {
            var location = MakeLocation();
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            catalog.locations.Add(location);
            _toDestroy.Add(catalog);

            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var crew = new List<Survivor> { survivor };
            var system = new LocationScavengingSystem(null, new Inventory(), null, seed: 12);
            var go = new GameObject("ScavengeDispatchHudBlockTests");
            _toDestroy.Add(go);
            var dispatch = go.AddComponent<ScavengeDispatchHUD>();
            dispatch.Bind(
                catalog,
                () => crew,
                _ => "Hatch sealed — no one leaves.",
                _ => "idle",
                () => "ACTIVE MISSIONS: none.",
                (_, __) => "--- PREFLIGHT ---\nMETER: working geiger.",
                _ => "?",
                _ => "?");

            dispatch.Open();

            Assert.That(dispatch.CanDispatchSelected(out var reason), Is.False);
            Assert.That(reason, Is.EqualTo("Hatch sealed — no one leaves."));
            Assert.That(dispatch.DispatchSelected(), Is.False);
            Assert.That(system.ActiveMissions, Is.Empty);
            StringAssert.Contains("HELD: Hatch sealed", dispatch.LastOutcome);
        }

        [Test]
        public void CrewSelection_CyclesEligibleSurvivors_AndDisplaysMissionRoster()
        {
            var location = MakeLocation();
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            catalog.locations.Add(location);
            _toDestroy.Add(catalog);

            var marcus = new Survivor
            {
                Id = "marcus_olejnik",
                DisplayName = "Marcus Olejnik",
                State = SurvivorState.Working
            };
            marcus.Needs.Health = 64f;
            marcus.RadiationDose = 9f;
            var elena = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            elena.Needs.Health = 82f;
            elena.RadiationDose = 4f;
            var suki = new Survivor { Id = "suki_tanaka", DisplayName = "Suki Tanaka" };
            suki.Needs.Health = 76f;
            suki.RadiationDose = 2f;
            var crew = new List<Survivor> { marcus, elena, suki };

            var go = new GameObject("ScavengeDispatchCrewRosterTests");
            _toDestroy.Add(go);
            var dispatch = go.AddComponent<ScavengeDispatchHUD>();
            dispatch.Bind(
                catalog,
                () => crew,
                survivor => survivor.State == SurvivorState.Working
                    ? "Operative is assigned elsewhere."
                    : null,
                survivor => survivor.State == SurvivorState.Working ? "assigned elsewhere" : "idle",
                () => "ACTIVE MISSIONS:\n- Elena Vasquez · scavenge · Suburban House · 0.5h remaining",
                (_, __) => "--- PREFLIGHT ---\nGEAR CHECK: face Gas Mask · body Hazmat Suit · shielding 110 rad/h.",
                _ => "~10?",
                _ => "?");

            dispatch.Open();

            Assert.That(dispatch.SelectedSurvivorId, Is.EqualTo("elena_vasquez"),
                "the board chooses the first Core-eligible survivor, not a busy crew member");
            StringAssert.Contains("Marcus Olejnik  health 64  dose 9  task assigned elsewhere", dispatch.PanelSummary);
            StringAssert.Contains("Elena Vasquez  health 82  dose 4  task idle  ready", dispatch.PanelSummary);
            StringAssert.Contains("ACTIVE MISSIONS:", dispatch.PanelSummary);
            StringAssert.Contains("Suburban House · 0.5h remaining", dispatch.PanelSummary);
            StringAssert.Contains("GEAR CHECK: face Gas Mask · body Hazmat Suit", dispatch.PanelSummary);

            Assert.That(dispatch.SelectNextSurvivor(), Is.True);
            Assert.That(dispatch.SelectedSurvivorId, Is.EqualTo("suki_tanaka"),
                "cycling skips Core-held crew while retaining their roster visibility");
        }

        [Test]
        public void ActiveMissionRoster_RoundTripsThroughScavengingSave()
        {
            var location = MakeLocation();
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var suit = ScriptableObject.CreateInstance<ItemDefinition>();
            suit.id = "hazmat_suit";
            suit.displayName = "Hazmat Suit";
            suit.type = ItemType.Protective;
            suit.isEquipable = true;
            suit.equipSlot = EquipSlot.Body;
            suit.radProtection = 80f;
            suit.durability = 100f;
            _toDestroy.Add(suit);
            var inventory = new Inventory();
            Assert.That(inventory.Add(suit, 1), Is.True);
            Assert.That(inventory.Equip(suit), Is.True);
            var source = new LocationScavengingSystem(null, inventory, null, seed: 12);

            Assert.That(source.StartMission(survivor, location), Is.True);
            source.Tick(0.25f);
            var save = source.CaptureState();

            var restored = new LocationScavengingSystem(null, new Inventory(), null, seed: 12);
            restored.SetSurvivorLookup(id => id == survivor.Id ? survivor : null);
            restored.RestoreState(save);

            Assert.That(restored.ActiveMissions, Has.Count.EqualTo(1));
            var mission = restored.ActiveMissions[0];
            Assert.That(mission.Survivor, Is.SameAs(survivor));
            Assert.That(mission.SurvivorId, Is.EqualTo("elena_vasquez"));
            Assert.That(mission.LocationId, Is.EqualTo("suburban_house"));
            Assert.That(mission.HoursRemaining, Is.EqualTo(0.75f).Within(0.001f));
            Assert.That(mission.AmbientRadPerHour, Is.EqualTo(10f));
            Assert.That(mission.RadPerHour, Is.EqualTo(0f),
                "the saved mission must retain its protected, actually applied dose rate");
        }

        [Test]
        public void AfterAction_ReportTracksDoseGearOverflow_AndRoundTrips()
        {
            var location = MakeLocation();
            location.dangerLevel = 14f; // Four guaranteed generic search rolls.

            var hazmatSuit = ScriptableObject.CreateInstance<ItemDefinition>();
            hazmatSuit.id = "hazmat_suit";
            hazmatSuit.displayName = "Hazmat Suit";
            hazmatSuit.type = ItemType.Protective;
            hazmatSuit.stackMax = 1;
            hazmatSuit.weight = 1f;
            hazmatSuit.isEquipable = true;
            hazmatSuit.equipSlot = EquipSlot.Body;
            hazmatSuit.radProtection = 2f;
            hazmatSuit.durability = 100f;
            _toDestroy.Add(hazmatSuit);

            var cleanWater = ScriptableObject.CreateInstance<ItemDefinition>();
            cleanWater.id = "clean_water";
            cleanWater.displayName = "Clean Water";
            cleanWater.type = ItemType.Water;
            cleanWater.stackMax = 1;
            cleanWater.weight = 1f;
            _toDestroy.Add(cleanWater);

            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            catalog.items.Add(cleanWater);
            _toDestroy.Add(catalog);
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(needsProfile);

            var fieldBag = new Inventory { Capacity = 1, MaxWeight = 20f };
            Assert.That(fieldBag.Add(hazmatSuit, 1), Is.True);
            Assert.That(fieldBag.Equip(hazmatSuit), Is.True);
            Assert.That(fieldBag.Add(cleanWater, 1), Is.True, "pre-fill the only field-bag slot");
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            var radiation = new RadiationSystem(new NeedsSystem(needsProfile));
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var source = new LocationScavengingSystem(
                radiation, fieldBag, catalog, seed: 12, overflowStash: overflow);
            source.BindWorldLoot(enabled: false);
            ScavengeAfterActionReport emitted = null;
            source.OnScavengeAfterActionReady += report => emitted = report;

            Assert.That(source.StartMission(survivor, location), Is.True);
            source.Tick(location.travelHours);

            Assert.That(emitted, Is.Not.Null);
            Assert.That(emitted.RecoveredItemIds, Is.Empty,
                "a full field bag must not claim it received the return");
            Assert.That(emitted.OverflowItemIds, Has.Count.EqualTo(4));
            Assert.That(emitted.UnsecuredItemIds, Is.Empty,
                "the live composition always provides a persistent overflow crate");
            Assert.That(overflow.Count(cleanWater), Is.EqualTo(4),
                "every returned item must remain retrievable from overflow");
            Assert.That(emitted.RadiationGained, Is.EqualTo(8f).Within(0.001f));
            Assert.That(emitted.FaceGear.ItemId, Is.Null.Or.Empty);
            Assert.That(emitted.BodyGear.ItemId, Is.EqualTo("hazmat_suit"));
            Assert.That(emitted.BodyGear.DurabilityPercent, Is.EqualTo(100f).Within(0.001f));

            var saved = source.CaptureState();
            var restored = new LocationScavengingSystem(null, new Inventory(), catalog, seed: 12);
            restored.RestoreState(saved);
            var overflowSave = overflow.CaptureState();
            var restoredOverflow = new Inventory();
            restoredOverflow.RestoreState(overflowSave, id => id == "clean_water" ? cleanWater : null);

            Assert.That(restored.LastAfterActionReport, Is.Not.Null);
            Assert.That(restored.LastAfterActionReport.OverflowItemIds, Has.Count.EqualTo(4));
            Assert.That(restored.LastAfterActionReport.RadiationGained, Is.EqualTo(8f).Within(0.001f));
            Assert.That(restored.LastAfterActionReport.BodyGear.ItemId, Is.EqualTo("hazmat_suit"));
            Assert.That(restoredOverflow.Count(cleanWater), Is.EqualTo(4),
                "the persistent overflow crate must retain returned items across its save round-trip");
        }

        [Test]
        public void PreviewLootChances_MatchesWhatRollLootActuallyUses()
        {
            // Audit M-5: search-slot count and recovery chance used to be
            // computed twice -- once inside RollLoot, once (with the same
            // literals hand-copied) inside ScavengeDispatchHUD.RebuildPanel().
            // PreviewLootChances is now the single source both read from; pin
            // its output at a few danger levels so a future balance-pass edit
            // can no longer silently drift the two apart again.
            LocationScavengingSystem.PreviewLootChances(0f, out int slots0, out float chance0);
            Assert.That(slots0, Is.EqualTo(1));
            Assert.That(chance0, Is.EqualTo(60f).Within(0.01f));

            LocationScavengingSystem.PreviewLootChances(2f, out int slots2, out float chance2);
            Assert.That(slots2, Is.EqualTo(1));
            Assert.That(chance2, Is.EqualTo(66f).Within(0.01f));

            LocationScavengingSystem.PreviewLootChances(14f, out int slots14, out float chance14);
            Assert.That(slots14, Is.EqualTo(4), "search slots must clamp at 4 regardless of danger level");
            Assert.That(chance14, Is.EqualTo(100f).Within(0.01f), "recovery chance must clamp at 100%");
        }

        [Test]
        public void DispatchBoard_RendersCoreAfterActionSummary()
        {
            var go = new GameObject("ScavengeDispatchAfterActionTests");
            _toDestroy.Add(go);
            var dispatch = go.AddComponent<ScavengeDispatchHUD>();

            dispatch.ReportAfterAction(
                "RETURNED: Suburban House — bagged 1 item. 2 moved to the bunker overflow crate. Dose +8. Gear: face none · body Hazmat Suit 100%.");

            StringAssert.Contains("overflow crate", dispatch.LastOutcome);
            StringAssert.Contains("Dose +8", dispatch.LastOutcome);
        }

        [Test]
        public void DiegeticView_PaintsAndHidesScavengeDispatchPanel()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintScavengeDispatch(true, "SCAVENGE DISPATCH\nSTATUS: READY");

            Assert.That(view.ScavengePanel.style.display.value, Is.EqualTo(DisplayStyle.Flex));
            Assert.That(view.ScavengeBody.text, Is.EqualTo("SCAVENGE DISPATCH\nSTATUS: READY"));

            view.PaintScavengeDispatch(false, null);

            Assert.That(view.ScavengePanel.style.display.value, Is.EqualTo(DisplayStyle.None));
        }

        private LocationDefinitionSO MakeLocation()
        {
            var location = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            location.id = "suburban_house";
            location.displayName = "Suburban House";
            location.description = "Intact house in a low-density neighborhood.";
            location.dangerLevel = 2f;
            location.travelHours = 1f;
            location.baseRadsPerHour = 10f;
            _toDestroy.Add(location);
            return location;
        }
    }
}
