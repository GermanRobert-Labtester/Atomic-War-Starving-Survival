using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Cross-system: faction trust → aggression-scaled hatch raids → succession /
    /// surrender, plus workbench hatch-install UX lines.
    /// </summary>
    [TestFixture]
    public class FactionHatchIntegrationTests
    {
        private const float Eps = 1e-3f;
        private readonly List<Object> _toDestroy = new List<Object>();
        private List<FactionSO> _factions;
        private WorldPhase _phase = WorldPhase.NuclearWinter;

        [SetUp]
        public void SetUp()
        {
            _factions = DynamicEconomySystem.CreateDefaultFactions();
            _phase = WorldPhase.NuclearWinter;
        }

        [TearDown]
        public void TearDown()
        {
            if (_factions != null)
            {
                for (int i = 0; i < _factions.Count; i++)
                    Object.DestroyImmediate(_factions[i]);
                _factions = null;
            }
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        private ItemDefinition MakeItem(string id, ItemType type = ItemType.Material)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = 50;
            item.weight = 0.2f;
            _toDestroy.Add(item);
            return item;
        }

        private (DynamicEconomySystem eco, HatchDefenseSystem hatch, Shelter shelter, Inventory inv)
            MakeWiredStack(int day = 40, float securityOverride = 80f)
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 2));
            var inv = new Inventory { Capacity = 40, MaxWeight = 200f };
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "s1", DisplayName = "Watch" }
            };
            survivors[0].Needs.Morale = 60f;
            survivors[0].Needs.Health = 100f;

            var hatch = new HatchDefenseSystem(
                () => shelter,
                () => inv,
                () => survivors,
                () => day,
                null,
                new System.Random(11));
            hatch.SecurityOverride = securityOverride;

            var eco = new DynamicEconomySystem(() => _phase, shelter, new System.Random(3));
            for (int i = 0; i < _factions.Count; i++)
                eco.RegisterFaction(_factions[i]);
            eco.SetHatchDefense(hatch);
            eco.SetDayProvider(() => day);
            return (eco, hatch, shelter, inv);
        }

        [Test]
        public void TrustDrop_ToRaidThreshold_LaunchesAggressionScaledHatchRaid()
        {
            var (eco, hatch, _, inv) = MakeWiredStack(day: 40, securityOverride: 5f);
            // Loot so breach can steal
            var food = MakeItem("canned_food", ItemType.Food);
            inv.Add(food, 5);

            string fid = FactionSO.Ids.MilitaryRemnants;
            // Military has high base aggression (0.7) → strength ~58
            float expectedStrength = 30f + eco.GetRaidAggression(fid) * 40f;
            Assert.That(eco.GetRaidAggression(fid), Is.GreaterThan(0.6f));

            eco.SetTrust(fid, -49f); // just above default -50
            Assert.That(eco.GetStance(fid), Is.Not.EqualTo(TradeStance.HostileRaid));

            // Crossing the line launches raid via ModifyTrust
            eco.ModifyTrust(fid, -5f);
            Assert.That(eco.GetTrust(fid), Is.LessThanOrEqualTo(-50f));
            Assert.That(eco.GetStance(fid), Is.EqualTo(TradeStance.HostileRaid));

            // Explicit second raid still works while hostile
            var raid = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(raid.Launched, Is.True);
            Assert.That(raid.RaidStrength, Is.EqualTo(expectedStrength).Within(0.5f));
            Assert.That(raid.Aggression, Is.EqualTo(eco.GetRaidAggression(fid)).Within(Eps));
            Assert.That(hatch.TotalRaidsResolved, Is.GreaterThan(0));
        }

        [Test]
        public void HighAggression_HitsHarderThanLowAggression()
        {
            var (ecoHigh, _, _, invH) = MakeWiredStack(day: 40, securityOverride: 5f);
            invH.Add(MakeItem("canned_food", ItemType.Food), 4);
            var (ecoLow, _, _, invL) = MakeWiredStack(day: 40, securityOverride: 5f);
            invL.Add(MakeItem("scrap_metal"), 2);

            string mil = FactionSO.Ids.MilitaryRemnants;
            string prep = FactionSO.Ids.DoomsdayPreppers;

            ecoHigh.SetTrust(mil, -80f);
            ecoLow.SetTrust(prep, -80f);
            ecoHigh.SetRaidAggression(mil, 1f);
            ecoLow.SetRaidAggression(prep, 0.1f);

            var hard = ecoHigh.TryLaunchRaid(mil, ignoreDayGate: true);
            var soft = ecoLow.TryLaunchRaid(prep, ignoreDayGate: true);

            Assert.That(hard.Launched, Is.True);
            Assert.That(soft.Launched, Is.True);
            Assert.That(hard.RaidStrength, Is.GreaterThan(soft.RaidStrength + 10f),
                "Max aggression should hit much harder than minimal aggression");
            Assert.That(hard.RaidStrength, Is.EqualTo(70f).Within(Eps)); // 30 + 40
            Assert.That(soft.RaidStrength, Is.EqualTo(34f).Within(Eps)); // 30 + 4
        }

        [Test]
        public void Succession_BlendsTrust_AndBumpsGeneration()
        {
            var (eco, _, _, _) = MakeWiredStack();
            string fid = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(fid, -90f);
            eco.SetRaidAggression(fid, 0.9f);
            string oldLeader = eco.GetLeaderName(fid);

            FactionSuccessionResult seen = null;
            eco.OnFactionSuccession += r => seen = r;

            var result = eco.ApplySuccession(fid, newLeaderName: "Ring Road Cell", trustBlendTowardStarting: 0.6f, newAggression: 0.4f);

            Assert.That(result.Applied, Is.True);
            Assert.That(result.Generation, Is.EqualTo(1));
            Assert.That(eco.GetSuccessionGeneration(fid), Is.EqualTo(1));
            Assert.That(eco.GetLeaderName(fid), Is.EqualTo("Ring Road Cell"));
            Assert.That(result.PreviousLeader, Is.EqualTo(oldLeader));
            // Trust moves toward startingTrust (0 for scavengers) from -90
            Assert.That(eco.GetTrust(fid), Is.GreaterThan(-90f));
            Assert.That(eco.GetTrust(fid), Is.LessThan(0f));
            Assert.That(eco.GetRaidAggression(fid), Is.EqualTo(0.4f).Within(Eps));
            Assert.That(seen, Is.Not.Null);
            Assert.That(seen.NewLeader, Is.EqualTo("Ring Road Cell"));
        }

        [Test]
        public void TwoRepels_AutoSurrender_LiftsTrust_AndBlocksFurtherRaids()
        {
            // High security always repels
            var (eco, hatch, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(fid, -80f);
            eco.SetRaidAggression(fid, 0.5f);

            FactionSurrenderResult surrender = null;
            eco.OnFactionSurrender += r => surrender = r;

            var r1 = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(r1.Launched, Is.True);
            Assert.That(r1.Repelled, Is.True, "High DEF should repel first raid");
            Assert.That(eco.GetConsecutiveRepels(fid), Is.EqualTo(1));
            Assert.That(eco.HasSurrendered(fid), Is.False);

            var r2 = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(r2.Launched, Is.True);
            Assert.That(r2.Repelled, Is.True);
            Assert.That(eco.HasSurrendered(fid), Is.True, "Second repel should auto-surrender");
            Assert.That(r2.SurrenderedAfter, Is.True);
            Assert.That(surrender, Is.Not.Null);
            Assert.That(surrender.Auto, Is.True);
            Assert.That(eco.GetTrust(fid), Is.GreaterThan(eco.GetFaction(fid).raidThreshold));
            Assert.That(eco.GetRaidAggression(fid), Is.LessThan(0.5f));

            var blocked = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(blocked.Launched, Is.False, "Surrendered faction must not raid again");
            Assert.That(blocked.Message, Does.Contain("stood down").IgnoreCase);
            Assert.That(hatch.TotalRaidsResolved, Is.EqualTo(2));
        }

        [Test]
        public void ForceSurrender_AfterBreachPath_StillEndsHostility()
        {
            var (eco, _, _, inv) = MakeWiredStack(day: 40, securityOverride: 5f);
            inv.Add(MakeItem("canned_food", ItemType.Food), 3);
            string fid = FactionSO.Ids.MilitaryRemnants;
            eco.SetTrust(fid, -90f);

            var breach = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(breach.Launched, Is.True);
            // Low security likely breached — streak resets
            Assert.That(eco.GetConsecutiveRepels(fid), Is.EqualTo(0));

            var s = eco.ForceSurrender(fid);
            Assert.That(s.Applied, Is.True);
            Assert.That(eco.HasSurrendered(fid), Is.True);
            Assert.That(eco.GetStance(fid), Is.Not.EqualTo(TradeStance.HostileRaid));
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Launched, Is.False);
        }

        [Test]
        public void WorkbenchUI_ListsAndInstallsHatchUpgrades()
        {
            var shelter = new Shelter();
            var scrap = MakeItem("scrap_metal");
            var mech = MakeItem("mechanical_parts");
            var inv = new Inventory { Capacity = 40, MaxWeight = 200f };
            inv.Add(scrap, 30);
            inv.Add(mech, 20);

            var hatch = new HatchDefenseSystem(
                () => shelter, () => inv, () => new List<Survivor>(), () => 40,
                null, new System.Random(1));

            var crafting = new CraftingSystem(inv);
            crafting.AddStation(new CraftingStation
            {
                id = WorkbenchSystem.StationId,
                displayName = "Workbench",
                Condition = 100f
            });
            var bench = new WorkbenchSystem(inv, id =>
            {
                if (id == "scrap_metal") return scrap;
                if (id == "mechanical_parts") return mech;
                return null;
            }, crafting, () => shelter, () => 40);
            bench.SetHatchDefense(hatch);

            var go = new GameObject("WbHatchUi");
            _toDestroy.Add(go);
            var ui = go.AddComponent<WorkbenchUI>();
            ui.Bind(bench);
            ui.Open();
            ui.Refresh();

            Assert.That(ui.IsOpen, Is.True);
            Assert.That(ui.HatchInstallLineCount, Is.GreaterThanOrEqualTo(3));
            Assert.That(ui.PanelSummary, Does.Contain("HATCH DEFENSE"));
            Assert.That(ui.PanelSummary, Does.Contain("Install hatch"));

            int installIdx = -1;
            for (int i = 0; i < ui.Lines.Count; i++)
            {
                if (ui.Lines[i].Kind == WorkbenchActionKind.InstallHatch
                    && ui.Lines[i].ModuleId == HatchDefenseModuleSO.BlastDoorId)
                {
                    installIdx = i;
                    break;
                }
            }
            Assert.That(installIdx, Is.GreaterThanOrEqualTo(0));
            Assert.That(ui.Lines[installIdx].CanExecute, Is.True);

            float secBefore = hatch.GetShelterSecurity();
            Assert.IsTrue(ui.Execute(installIdx));
            Assert.That(shelter.GetModule(HatchDefenseModuleSO.BlastDoorId), Is.Not.Null);
            Assert.That(hatch.GetShelterSecurity(), Is.GreaterThan(secBefore));
            Assert.That(inv.Count(scrap), Is.LessThan(30));
        }

        [Test]
        public void KeybindDocs_PlayerInputHandler_ExposesWorkbenchAndHatchKeys()
        {
            var go = new GameObject("InputKeys");
            _toDestroy.Add(go);
            var input = go.AddComponent<AtomicWar._Game.Core.PlayerInputHandler>();
            Assert.That(input.WorkbenchKey, Is.EqualTo(KeyCode.B));
            Assert.That(input.HatchDefenseKey, Is.EqualTo(KeyCode.H));
            Assert.That(input.MapKey, Is.EqualTo(KeyCode.M));
        }

        [Test]
        public void EconomySave_RoundTripsAggressionSuccessionSurrender()
        {
            var (eco, _, _, _) = MakeWiredStack();
            string fid = FactionSO.Ids.MilitaryRemnants;
            eco.SetTrust(fid, -70f);
            eco.SetRaidAggression(fid, 0.88f);
            eco.ApplySuccession(fid, "Colonel Ash", 0.3f, 0.5f);
            eco.ForceSurrender(fid);

            var snap = eco.CaptureState();
            var (eco2, _, _, _) = MakeWiredStack();
            eco2.RestoreState(snap);

            Assert.That(eco2.GetTrust(fid), Is.EqualTo(eco.GetTrust(fid)).Within(Eps));
            Assert.That(eco2.GetRaidAggression(fid), Is.EqualTo(eco.GetRaidAggression(fid)).Within(Eps));
            Assert.That(eco2.GetSuccessionGeneration(fid), Is.EqualTo(1));
            Assert.That(eco2.GetLeaderName(fid), Is.EqualTo("Colonel Ash"));
            Assert.That(eco2.HasSurrendered(fid), Is.True);
        }
    }
}
