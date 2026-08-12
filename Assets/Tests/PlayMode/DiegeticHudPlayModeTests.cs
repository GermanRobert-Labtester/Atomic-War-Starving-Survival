using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Play-mode gate: diegetic HUD must mount a real UIDocument panel so
    /// hatch ammo / encounter log / stores focus are on screen, not only
    /// detached string view-models.
    /// </summary>
    [TestFixture]
    public class DiegeticHudPlayModeTests
    {
        private GameObject _go;
        private HUD _hud;
        private DiegeticHudController _diegetic;

        // L-2: per-test ScriptableObjects used to be destroyed by an inline
        // call at the end of the test body, so a failed Assert above it (which
        // throws) skipped cleanup and leaked the object into the rest of the
        // Editor test run. TearDown now drains this list unconditionally.
        private readonly List<Object> _toDestroy = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _go = null;
            _hud = null;
            _diegetic = null;

            for (int i = 0; i < _toDestroy.Count; i++)
                if (_toDestroy[i] != null) Object.DestroyImmediate(_toDestroy[i]);
            _toDestroy.Clear();
        }

        private IEnumerator BuildHud()
        {
            _go = new GameObject("DiegeticHud (playmode)");
            _go.SetActive(false);
            _hud = _go.AddComponent<HUD>();
            _go.SetActive(true);

            // One frame for Awake / UIDocument panel creation.
            yield return null;

            _diegetic = _hud.EnsureDiegeticHud();
            Assert.That(_diegetic, Is.Not.Null);

            // UIDocument needs a frame after panelSettings assignment to expose root.
            yield return null;
            yield return null;

            _diegetic.EnsureDocumentMounted();
            _diegetic.EnsureBuilt();
            yield return null;
        }

        [UnityTest]
        public IEnumerator EnsureDiegeticHud_MountsLiveUIDocument()
        {
            yield return BuildHud();

            Assert.That(_diegetic.IsDocumentMounted, Is.True);
            Assert.That(_diegetic.Document, Is.Not.Null);
            Assert.That(_diegetic.Document.panelSettings, Is.Not.Null);
            Assert.That(_diegetic.Document.rootVisualElement, Is.Not.Null,
                "UIDocument should expose a live root once panel settings are assigned.");

            Assert.That(_diegetic.View, Is.Not.Null);
            Assert.That(_diegetic.View.Root, Is.Not.Null);
            Assert.That(_diegetic.View.HatchPanel, Is.Not.Null);
            Assert.That(_diegetic.View.EncounterPanel, Is.Not.Null);
            Assert.That(_diegetic.View.StoresPanel, Is.Not.Null);

            // Encounter strip is always in the tree.
            var encounter = _diegetic.Document.rootVisualElement.Q<VisualElement>(DiegeticHudView.EncounterPanelName)
                ?? _diegetic.View.EncounterPanel;
            Assert.That(encounter, Is.Not.Null);

            var docRoot = _diegetic.Document.rootVisualElement;
            Assert.That(docRoot.Q("location-detail-panel"), Is.Not.Null, "Play Mode must keep location-detail-panel");
            Assert.That(docRoot.Q("radiation-phase-root"), Is.Not.Null, "Play Mode must keep radiation-phase-root");
            Assert.That(docRoot.Q("siege-status"), Is.Not.Null, "Play Mode must keep siege-status");
        }

        [UnityTest]
        public IEnumerator Paint_HatchOpen_ShowsAmmoAndArmsOnPanel()
        {
            yield return BuildHud();

            var hatch = _hud.HatchDefenseHUD;
            hatch.BindAmmoUi(
                () => "AMMO STOCKPILE\n  9×19mm / FMJ  ×12",
                () => "ARMS PREVIEW\n  vs raiders 5.0");
            hatch.Open();
            _diegetic.Paint();
            yield return null;

            Assert.That(_diegetic.View.HatchPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("AMMO STOCKPILE", _diegetic.View.HatchAmmo.text);
            StringAssert.Contains("ARMS PREVIEW", _diegetic.View.HatchArms.text);
        }

        [UnityTest]
        public IEnumerator ScavengeDispatch_MountsRealLocationPreview()
        {
            yield return BuildHud();

            var location = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            _toDestroy.Add(location);
            location.id = "suburban_house";
            location.displayName = "Suburban House";
            location.description = "Intact house in a low-density neighborhood.";
            location.dangerLevel = 2f;
            location.travelHours = 1f;
            location.baseRadsPerHour = 10f;
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            _toDestroy.Add(catalog);
            catalog.locations.Add(location);
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var system = new LocationScavengingSystem(null, new Inventory(), null, seed: 12);

            _hud.BindScavengeDispatch(
                catalog,
                () => new List<Survivor> { survivor },
                _ => null,
                _ => "idle",
                () => "ACTIVE MISSIONS: none.",
                (_, __) => "--- PREFLIGHT ---\nRETURN: expected in 1h when the mission clock closes.",
                _ => "~10?",
                _ => "1 search slot · 66% baseline recovery each.");
            _hud.ScavengeDispatchHUD.Open();
            _diegetic.Paint();
            yield return null;

            Assert.That(_diegetic.View.ScavengePanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("Suburban House", _diegetic.View.ScavengeBody.text);
            StringAssert.Contains("SUPPLY: 1 search slot", _diegetic.View.ScavengeBody.text);
            StringAssert.Contains("--- PREFLIGHT ---", _diegetic.View.ScavengeBody.text);
        }

        [UnityTest]
        public IEnumerator OverflowCrate_MountsStoredReturnsAndTransferHint()
        {
            yield return BuildHud();

            var water = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(water);
            water.id = "clean_water";
            water.displayName = "Clean Water";
            water.type = ItemType.Water;
            water.stackMax = 1;
            water.weight = 1f;
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            var fieldBag = new Inventory { Capacity = 4, MaxWeight = 10f };
            overflow.Add(water, 2);

            using (var crate = new OverflowCrateSystem(overflow, fieldBag))
            {
                _hud.BindOverflowCrate(crate.GetSnapshot);
                _hud.OverflowCrateHUD.Open();
                _hud.EnsureDiegeticHud();
                _diegetic.Paint();
                yield return null;

                Assert.That(_diegetic.View.OverflowCratePanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
                StringAssert.Contains("BUNKER OVERFLOW CRATE", _diegetic.View.OverflowCrateBody.text);
                StringAssert.Contains("Clean Water  ×2", _diegetic.View.OverflowCrateBody.text);
                StringAssert.Contains("move one", _diegetic.View.OverflowCrateBody.text);
            }
        }

        [UnityTest]
        public IEnumerator FieldGearLoadout_MountsProtectionAndStowHint()
        {
            yield return BuildHud();

            var suit = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(suit);
            suit.id = "hazmat_suit";
            suit.displayName = "Hazmat Suit";
            suit.type = ItemType.Protective;
            suit.stackMax = 1;
            suit.weight = 5f;
            suit.isEquipable = true;
            suit.equipSlot = EquipSlot.Body;
            suit.radProtection = 80f;
            suit.durability = 100f;
            var fieldBag = new Inventory { Capacity = 4, MaxWeight = 30f };
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            fieldBag.Add(suit, 1);

            using (var loadout = new FieldGearLoadoutSystem(fieldBag, overflow))
            {
                _hud.BindFieldGearLoadout(loadout.GetSnapshot);
                _hud.FieldGearLoadoutHUD.Open();
                _hud.EnsureDiegeticHud();
                _diegetic.Paint();
                yield return null;

                Assert.That(_diegetic.View.FieldGearLoadoutPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
                StringAssert.Contains("FIELD GEAR LOADOUT", _diegetic.View.FieldGearLoadoutBody.text);
                StringAssert.Contains("Hazmat Suit", _diegetic.View.FieldGearLoadoutBody.text);
                StringAssert.Contains("[U] stow", _diegetic.View.FieldGearLoadoutBody.text);
            }
        }

        [UnityTest]
        public IEnumerator BunkerRationing_MountsPolicyAndProjectedConsequences()
        {
            yield return BuildHud();

            int food = 3;
            int water = 3;
            var rationing = new BunkerRationingSystem(
                resource => resource == RationResource.Food ? food : water,
                (resource, amount) =>
                {
                    if (resource == RationResource.Food)
                    {
                        int issued = Mathf.Min(food, amount);
                        food -= issued;
                        return issued;
                    }

                    int issuedWater = Mathf.Min(water, amount);
                    water -= issuedWater;
                    return issuedWater;
                });
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" },
                new Survivor { Id = "jonah_riggs", DisplayName = "Jonah Riggs" }
            };

            _hud.BindBunkerRationing(() => rationing.GetSnapshot(survivors));
            _hud.BunkerRationingHUD.Open();
            _hud.EnsureDiegeticHud();
            _diegetic.Paint();
            yield return null;

            Assert.That(_diegetic.View.BunkerRationingPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("BUNKER RATION BOARD", _diegetic.View.BunkerRationingBody.text);
            StringAssert.Contains("FOOD: STANDARD", _diegetic.View.BunkerRationingBody.text);
            StringAssert.Contains("Projected relief", _diegetic.View.BunkerRationingBody.text);
        }

        [UnityTest]
        public IEnumerator WaterPurification_MountsCisternQueueAndRationLink()
        {
            yield return BuildHud();

            var purifierDefinition = ScriptableObject.CreateInstance<AtomicWar._Game.Shelter.Modules.WaterPurifierModuleSO>();
            _toDestroy.Add(purifierDefinition);
            purifierDefinition.ModuleId = "water_purifier";
            purifierDefinition.DisplayName = "Water Purifier";
            purifierDefinition.ConversionHoursPerUnit = 2f;
            var shelter = new AtomicWar._Game.Shelter.Shelter();
            shelter.AddModule(new AtomicWar._Game.Shelter.ShelterModuleInstance(purifierDefinition, 1)
            {
                IsEnabled = true,
                FilterHealth = 90f
            });
            var storage = new AtomicWar._Game.Shelter.WaterStorage
            {
                CleanWater = 2f,
                DirtyWater = 3f,
                IrradiatedWater = 1f
            };
            var waterEconomy = new WaterEconomySystem();
            var ration = new BunkerRationingSnapshot
            {
                CleanCisternWaterOnHand = 2,
                WaterOnHand = 2,
                WaterRequired = 2,
                ProjectedWaterCoverage = 1f,
                ProjectedThirstReduction = 50f
            };

            _hud.BindWaterPurification(
                () => waterEconomy.GetSnapshot(shelter, storage),
                () => ration);
            _hud.WaterPurificationHUD.Open();
            _hud.EnsureDiegeticHud();
            _diegetic.Paint();
            yield return null;

            Assert.That(_diegetic.View.WaterPurificationPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("BUNKER WATER TERMINAL", _diegetic.View.WaterPurificationBody.text);
            StringAssert.Contains("clean 2", _diegetic.View.WaterPurificationBody.text);
            StringAssert.Contains("RATION LINK", _diegetic.View.WaterPurificationBody.text);
        }

        [UnityTest]
        public IEnumerator AirHeatManagement_MountsClimateTelemetryAndPriorityHint()
        {
            yield return BuildHud();

            var filterDefinition = ScriptableObject.CreateInstance<AtomicWar._Game.Shelter.Modules.AirFiltrationModuleSO>();
            _toDestroy.Add(filterDefinition);
            filterDefinition.ModuleId = "air_filtration";
            filterDefinition.DisplayName = "Air Filter";
            filterDefinition.DegradationRatePerHour = 2f;
            var heaterDefinition = ScriptableObject.CreateInstance<AtomicWar._Game.Shelter.Modules.HeaterModuleSO>();
            _toDestroy.Add(heaterDefinition);
            heaterDefinition.ModuleId = "heater";
            heaterDefinition.DisplayName = "Heater";
            heaterDefinition.FuelConsumptionRatePerHour = 1f;
            var shelter = new AtomicWar._Game.Shelter.Shelter();
            shelter.AddModule(new AtomicWar._Game.Shelter.ShelterModuleInstance(filterDefinition, 1)
            {
                FilterHealth = 80f,
                IsEnabled = true
            });
            shelter.AddModule(new AtomicWar._Game.Shelter.ShelterModuleInstance(heaterDefinition, 1)
            {
                Fuel = 12f,
                IsEnabled = true
            });
            var power = AtomicWar._Game.Shelter.PowerNetwork.CreateDefault();
            power.ApplyToShelter(shelter);
            using (var climate = new AtomicWar._Game.Shelter.AirHeatManagementSystem(
                shelter, power, () => 2f, () => -14f))
            {
                _hud.BindAirHeatManagement(climate.GetSnapshot);
                _hud.AirHeatManagementHUD.Open();
                _hud.EnsureDiegeticHud();
                _diegetic.Paint();
                yield return null;

                Assert.That(_diegetic.View.AirHeatManagementPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
                StringAssert.Contains("AIR + HEAT CONTROL", _diegetic.View.AirHeatManagementBody.text);
                StringAssert.Contains("INDOORS: 2", _diegetic.View.AirHeatManagementBody.text);
                StringAssert.Contains("Priority 1 keeps a load longest", _diegetic.View.AirHeatManagementBody.text);
            }
        }

        [UnityTest]
        public IEnumerator BunkerMaintenance_MountsRepairOrderAndGridInterlock()
        {
            yield return BuildHud();

            var filterDefinition = ScriptableObject.CreateInstance<AtomicWar._Game.Shelter.Modules.AirFiltrationModuleSO>();
            _toDestroy.Add(filterDefinition);
            filterDefinition.ModuleId = "air_filtration";
            filterDefinition.DisplayName = "Air Filter";
            var shelter = new AtomicWar._Game.Shelter.Shelter();
            shelter.AddModule(new AtomicWar._Game.Shelter.ShelterModuleInstance(filterDefinition, 1)
            {
                FilterHealth = 36f,
                IsEnabled = true
            });
            var power = AtomicWar._Game.Shelter.PowerNetwork.CreateDefault();
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" }
            };
            using (var maintenance = new AtomicWar._Game.Shelter.BunkerMaintenanceSystem(
                shelter,
                power,
                id => id == "mechanical_parts" ? 3 : 1,
                (_, __) => true,
                () => survivors))
            {
                maintenance.AssignSurvivor("elena_vasquez");
                _hud.BindBunkerMaintenance(maintenance.GetSnapshot, () => survivors);
                _hud.BunkerMaintenanceHUD.Open();
                _hud.EnsureDiegeticHud();
                _diegetic.Paint();
                yield return null;

                Assert.That(_diegetic.View.BunkerMaintenancePanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
                StringAssert.Contains("BUNKER MAINTENANCE", _diegetic.View.BunkerMaintenanceBody.text);
                StringAssert.Contains("GRID INTERLOCK: clear.", _diegetic.View.BunkerMaintenanceBody.text);
                StringAssert.Contains("AIR FILTER: 36%", _diegetic.View.BunkerMaintenanceBody.text);
            }
        }

        [UnityTest]
        public IEnumerator SurvivorTaskBoard_MountsReservedWorkOrder()
        {
            yield return BuildHud();

            _hud.BindSurvivorTaskBoard(() => new SurvivorTaskBoardSnapshot
            {
                LastReport = "QUEUED: Air Filter awaits Elena Vasquez.",
                ActiveTasks = new List<SurvivorTaskBoardTask>
                {
                    new SurvivorTaskBoardTask
                    {
                        DisplayName = "Bunker repair",
                        Status = "queued",
                        AssignedSurvivorName = "Elena Vasquez",
                        Priority = MaintenanceRepairPriority.Critical,
                        RequiredWorkHours = 2f
                    }
                },
                SurvivorAssignments = new List<SurvivorTaskBoardAssignment>
                {
                    new SurvivorTaskBoardAssignment
                    {
                        SurvivorName = "Elena Vasquez",
                        AssignmentLabel = "Bunker repair [reserved]",
                        IsReserved = true
                    }
                }
            });
            _hud.SurvivorTaskBoardHUD.Open();
            _hud.EnsureDiegeticHud();
            _diegetic.Paint();
            yield return null;

            Assert.That(_diegetic.View.SurvivorTaskBoardPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("SURVIVOR TASK BOARD", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("BUNKER REPAIR", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("Bunker repair [reserved]", _diegetic.View.SurvivorTaskBoardBody.text);
        }

        [UnityTest]
        public IEnumerator SurvivorTaskBoard_MountsDutyShiftSelection()
        {
            yield return BuildHud();

            _hud.BindSurvivorTaskBoard(() => new SurvivorTaskBoardSnapshot
            {
                LastReport = "ASSIGNED: Elena Vasquez to air filtration.",
                ActiveTasks = new List<SurvivorTaskBoardTask>
                {
                    new SurvivorTaskBoardTask
                    {
                        IsWorkShift = true,
                        WorkShiftDuty = WorkShiftDuty.AirFiltration,
                        DisplayName = "air filtration shift",
                        Status = "relief ready",
                        AssignedSurvivorName = "Elena Vasquez",
                        ProgressHours = 2f,
                        ReliefSurvivorId = "marcus_reed",
                        ReliefSurvivorName = "Marcus Reed",
                        HoursSinceHandover = 2f,
                        RotationHours = 4f,
                        HandoverCount = 1
                    }
                },
                ShiftRecommendations = new List<SurvivorWorkShiftRecommendationSnapshot>
                {
                    new SurvivorWorkShiftRecommendationSnapshot
                    {
                        Duty = WorkShiftDuty.AirFiltration,
                        SuggestedSurvivorName = "Marcus Reed",
                        Priority = WorkShiftRecommendationPriority.Critical,
                        Reason = "Air quality is becoming unsafe."
                    }
                },
                ShiftSlots = new List<SurvivorWorkShiftSlotSnapshot>
                {
                    new SurvivorWorkShiftSlotSnapshot
                    {
                        Duty = WorkShiftDuty.AirFiltration,
                        DisplayName = "air filtration",
                        IsSupported = true,
                        AssignedSurvivorId = "elena_vasquez",
                        AssignedSurvivorName = "Elena Vasquez",
                        ReliefSurvivorId = "marcus_reed",
                        ReliefSurvivorName = "Marcus Reed",
                        HoursWorked = 2f,
                        HoursSinceHandover = 2f,
                        RotationHours = 4f,
                        HandoverCount = 1,
                        EffectSummary = "25% less filter wear",
                        Availability = new WorkShiftAvailabilityForecast
                        {
                            Duty = WorkShiftDuty.AirFiltration,
                            IsStaffed = true,
                            Status = WorkShiftAvailabilityStatus.Low,
                            Summary = "Safe for 8.0h at 2.0/h; reserve is low."
                        }
                    }
                },
                SurvivorAssignments = new List<SurvivorTaskBoardAssignment>
                {
                    new SurvivorTaskBoardAssignment
                    {
                        SurvivorId = "elena_vasquez",
                        SurvivorName = "Elena Vasquez",
                        AssignmentLabel = "air filtration [reserved]",
                        IsReserved = true
                    }
                }
            });
            _hud.SurvivorTaskBoardHUD.Open();
            _hud.EnsureDiegeticHud();
            _diegetic.Paint();
            yield return null;

            StringAssert.Contains("DUTY SHIFTS", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("SHIFT RECOMMENDATIONS", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("CRITICAL · AIR FILTRATION", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("[R] APPROVE", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("AIR FILTRATION", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("CREW PICK: Elena Vasquez", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("air filtration [reserved]", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("rotate 2.0/4.0h", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("RELIEF: Marcus Reed", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("25% less filter wear", _diegetic.View.SurvivorTaskBoardBody.text);
            StringAssert.Contains("Safe for 8.0h at 2.0/h; reserve is low.", _diegetic.View.SurvivorTaskBoardBody.text);
        }

        [UnityTest]
        public IEnumerator KeyboardFocus_SelectsAmmo_ShowsStoresTooltip()
        {
            yield return BuildHud();

            var strip = _hud.InventoryStripUI;
            strip.TooltipResolver = Item_AmmoTypes.FormatItemTooltip;
            strip.MilitaryExclusiveChecker = Item_AmmoTypes.IsMilitaryExclusiveTooltip;

            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(item);
            item.id = "ammo_556x45_ap";
            item.displayName = "ammo_556x45_ap";
            item.type = ItemType.Weapon;
            item.stackMax = 99;
            item.weight = 0.02f;

            var inv = new Inventory { Capacity = 8, MaxWeight = 40f };
            inv.Add(item, 4);
            strip.Sync(inv);

            Assert.That(strip.SelectNext(), Is.True);
            _hud.RefreshDiegeticHud();
            yield return null;

            Assert.That(_diegetic.View.StoresPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("MILITARY EXCLUSIVE", _diegetic.View.StoresTooltip.text);
        }

        [UnityTest]
        public IEnumerator VitalsAndEvent_ApplyFigmaPanelStates()
        {
            yield return BuildHud();

            var needs = new Dictionary<string, NeedBarData>
            {
                { "hunger", new NeedBarData { CurrentValue = 92f, MaxValue = 100f } },
                { "thirst", new NeedBarData { CurrentValue = 10f, MaxValue = 100f, IsCritical = true } },
                { "fatigue", new NeedBarData { CurrentValue = 48f, MaxValue = 100f } },
                { "warmth", new NeedBarData { CurrentValue = 71f, MaxValue = 100f } }
            };

            _diegetic.PaintVitals(5, 3f, 0.42f, 0.1f, needs);
            _diegetic.PaintEventModal(true, "UNKNOWN TRANSMISSION", "The relay is live.",
                new[] { new EventChoiceLine("Answer", true) });
            yield return null;

            Assert.That(_diegetic.View.VitalsPanel.ClassListContains("diegetic-panel--critical"), Is.True);
            Assert.That(_diegetic.View.EventPanel.ClassListContains("diegetic-panel--warning"), Is.True);

            needs["thirst"].IsCritical = false;
            _diegetic.PaintVitals(5, 4f, 0.42f, 0.1f, needs);
            _diegetic.PaintEventModal(false, null, null, null);
            yield return null;

            Assert.That(_diegetic.View.VitalsPanel.ClassListContains("diegetic-panel--critical"), Is.False);
            Assert.That(_diegetic.View.EventPanel.ClassListContains("diegetic-panel--warning"), Is.False);
        }
    }
}
