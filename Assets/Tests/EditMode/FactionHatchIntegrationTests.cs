using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
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
            Assert.That(input.ParleyKey, Is.EqualTo(KeyCode.P),
                "Trade parley / demand surrender is [P] when trade is open");
        }

        [Test]
        public void CanDemandParley_False_WithoutRepel_True_AfterOneRepel()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(fid, -80f);

            Assert.That(eco.CanDemandParley(fid), Is.False,
                "No hatch hold yet — parley must stay locked");
            Assert.That(eco.LastRepelledFactionId, Is.EqualTo(string.Empty));

            var blocked = eco.DemandParley(fid);
            Assert.That(blocked.Applied, Is.False);
            Assert.That(blocked.Message, Does.Contain("hold the hatch").IgnoreCase);

            var raid = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(raid.Launched, Is.True);
            Assert.That(raid.Repelled, Is.True);
            Assert.That(eco.GetConsecutiveRepels(fid), Is.EqualTo(1));
            Assert.That(eco.LastRepelledFactionId, Is.EqualTo(fid));
            Assert.That(eco.CanDemandParley(fid), Is.True,
                "One consecutive repel unlocks demand-parley");
        }

        [Test]
        public void DemandParley_AfterRepel_SurrendersAndBlocksRaids()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.MilitaryRemnants;
            eco.SetTrust(fid, -90f);
            eco.SetRaidAggression(fid, 0.8f);
            string leader = eco.GetLeaderName(fid);

            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(eco.CanDemandParley(fid), Is.True);

            FactionSurrenderResult seen = null;
            eco.OnFactionSurrender += r => seen = r;

            var result = eco.DemandParley(fid);
            Assert.That(result.Applied, Is.True);
            Assert.That(result.Auto, Is.False);
            Assert.That(result.Message, Does.Contain("parley").IgnoreCase);
            Assert.That(eco.HasSurrendered(fid), Is.True);
            Assert.That(eco.CanDemandParley(fid), Is.False, "Already stood down");
            Assert.That(eco.GetTrust(fid), Is.GreaterThan(eco.GetFaction(fid).raidThreshold));
            Assert.That(eco.GetRaidAggression(fid), Is.LessThan(0.8f));
            Assert.That(seen, Is.Not.Null);
            Assert.That(seen.Applied, Is.True);

            var blocked = eco.TryLaunchRaid(fid, ignoreDayGate: true);
            Assert.That(blocked.Launched, Is.False);
            Assert.That(blocked.Message, Does.Contain("stood down").IgnoreCase);
            Assert.That(leader, Is.Not.Empty);
        }

        [Test]
        public void TradeScreen_SurfacesLeaderAggressionAndParleyReady()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(fid, -80f);
            eco.SetRaidAggression(fid, 0.65f);
            eco.ApplySuccession(fid, "Marrow Road Boss", 0.2f, 0.55f);

            var player = new Inventory { Capacity = 20, MaxWeight = 100f };
            var stock = new Inventory { Capacity = 20, MaxWeight = 100f };

            var go = new GameObject("TradeParleyUi");
            _toDestroy.Add(go);
            var ui = go.AddComponent<TradeScreenUI>();
            ui.Bind(eco);
            Assert.IsTrue(ui.Open(fid, player, stock));

            // Before repel: strip shows leader + aggression, no PARLEY READY
            Assert.That(ui.LeaderName, Is.EqualTo("Marrow Road Boss"));
            Assert.That(ui.Aggression, Is.EqualTo(0.55f).Within(Eps));
            Assert.That(ui.SuccessionGeneration, Is.EqualTo(1));
            Assert.That(ui.CanDemandParley, Is.False);
            Assert.That(ui.FactionStatusStrip, Does.Contain("Leader: Marrow Road Boss"));
            Assert.That(ui.FactionStatusStrip, Does.Contain("Aggression"));
            Assert.That(ui.FactionStatusStrip, Does.Not.Contain("PARLEY READY"));

            Assert.That(ui.TryDemandParley(), Is.False, "Cannot parley without a hatch hold");

            // One repel unlocks strip + TryDemandParley
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            ui.Recalculate();
            Assert.That(ui.CanDemandParley, Is.True);
            Assert.That(ui.ConsecutiveRepels, Is.EqualTo(1));
            Assert.That(ui.FactionStatusStrip, Does.Contain("PARLEY READY [P]"));
            Assert.That(ui.FactionStatusStrip, Does.Contain("Hatch holds ×1"));
            Assert.That(ui.BuildQuoteSummary(), Does.Contain("[P] Demand parley"));

            FactionSurrenderResult uiEvent = null;
            ui.OnParleyResolved += r => uiEvent = r;
            Assert.That(ui.TryDemandParley(), Is.True);
            Assert.That(ui.HasSurrendered, Is.True);
            Assert.That(ui.CanDemandParley, Is.False);
            Assert.That(ui.FactionStatusStrip, Does.Contain("STOOD DOWN"));
            Assert.That(ui.LastParleyMessage, Does.Contain("parley").IgnoreCase);
            Assert.That(uiEvent, Is.Not.Null);
            Assert.That(uiEvent.Applied, Is.True);
            Assert.That(eco.HasSurrendered(fid), Is.True);
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

        [Test]
        public void EconomySave_RoundTripsLastRepelledFactionId()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.DoomsdayPreppers;
            eco.SetTrust(fid, -80f);

            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(eco.LastRepelledFactionId, Is.EqualTo(fid));

            var snap = eco.CaptureState();
            Assert.That(snap.LastRepelledFactionId, Is.EqualTo(fid));

            var (eco2, _, _, _) = MakeWiredStack();
            eco2.RestoreState(snap);
            Assert.That(eco2.LastRepelledFactionId, Is.EqualTo(fid),
                "Trade strip / parley gate must remember last repel after load");
            // Consecutive repels also restore so CanDemandParley stays true.
            Assert.That(eco2.GetConsecutiveRepels(fid), Is.EqualTo(1));
            Assert.That(eco2.CanDemandParley(fid), Is.True);
        }

        [Test]
        public void ParleyOfferEvent_HasDemandTradeAndDismissChoices()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(fid, -80f);
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);

            var ev = eco.CreateParleyOfferEvent(fid);
            _toDestroy.Add(ev);
            Assert.That(ev.id, Does.Contain("parley_offer"));
            Assert.That(ev.title, Does.Contain("Flinched").IgnoreCase);
            Assert.That(ev.choices.Count, Is.EqualTo(3));
            Assert.That(ev.choices.Exists(c => c.ChoiceId == "parley_now"), Is.True);
            Assert.That(ev.choices.Exists(c => c.ChoiceId == "open_trade"), Is.True);
            Assert.That(ev.choices.Exists(c => c.ChoiceId == "dismiss"), Is.True);
            Assert.That(ev.bodyText, Does.Contain(eco.GetLeaderName(fid)));
        }

        [Test]
        public void ParleyOfferPrompt_BeginTimeoutAndResolve()
        {
            var prompt = new ParleyOfferPrompt(timeoutGameHours: 2f);
            string readyFid = null;
            prompt.OnPromptReady += id => readyFid = id;
            ParleyOfferPrompt.Resolution timed = ParleyOfferPrompt.Resolution.DemandParley;
            prompt.OnTimeout += r => timed = r;

            prompt.Begin(FactionSO.Ids.MilitaryRemnants, "Colonel Ash");
            Assert.That(prompt.IsActive, Is.True);
            Assert.That(prompt.FactionId, Is.EqualTo(FactionSO.Ids.MilitaryRemnants));
            Assert.That(readyFid, Is.EqualTo(FactionSO.Ids.MilitaryRemnants));

            // Second begin is no-op
            prompt.Begin(FactionSO.Ids.ScavengerCamp, "Other");
            Assert.That(prompt.FactionId, Is.EqualTo(FactionSO.Ids.MilitaryRemnants));

            prompt.Tick(1f);
            Assert.That(prompt.IsActive, Is.True);
            prompt.Tick(1.5f);
            Assert.That(prompt.IsActive, Is.False);
            Assert.That(timed, Is.EqualTo(ParleyOfferPrompt.Resolution.TimedOut));

            // Fresh resolve path
            prompt.Begin(FactionSO.Ids.ScavengerCamp, "Ring Boss");
            ParleyOfferPrompt.Resolution chosen = ParleyOfferPrompt.Resolution.TimedOut;
            prompt.OnResolved += r => chosen = r;
            prompt.Resolve(ParleyOfferPrompt.Resolution.DemandParley);
            Assert.That(chosen, Is.EqualTo(ParleyOfferPrompt.Resolution.DemandParley));
            Assert.That(prompt.IsActive, Is.False);
        }

        [Test]
        public void ParleyOffer_ViaEventRunner_DemandParleyChoice_AppliesSurrender()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.MilitaryRemnants;
            eco.SetTrust(fid, -90f);
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(eco.CanDemandParley(fid), Is.True);

            var runner = new EventRunner();
            var prompt = new ParleyOfferPrompt();
            prompt.Begin(fid, eco.GetLeaderName(fid));

            var ev = eco.CreateParleyOfferEvent(fid);
            _toDestroy.Add(ev);
            GameEvent seen = null;
            runner.OnEventTriggered += (e, _) => seen = e;
            runner.Run(ev, new EventContext());
            Assert.That(seen, Is.SameAs(ev));

            var parleyChoice = ev.choices.Find(c => c.ChoiceId == "parley_now");
            Assert.That(parleyChoice, Is.Not.Null);

            // Simulate modal selection side-effect (bootstrap ApplyParleyOfferChoice)
            var result = eco.DemandParley(fid);
            prompt.Resolve(ParleyOfferPrompt.Resolution.DemandParley);

            Assert.That(result.Applied, Is.True);
            Assert.That(eco.HasSurrendered(fid), Is.True);
            Assert.That(prompt.IsActive, Is.False);
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Launched, Is.False);
        }

        [Test]
        public void FactionRadioIntercepts_SuccessionParleyAndHatchRepel()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(fid, -80f);

            var radio = new FactionRadioInterceptSystem();
            radio.Bind(eco, () => 42);
            FactionRadioInterceptSystem.InterceptEntry last = null;
            radio.OnIntercept += e => last = e;

            eco.ApplySuccession(fid, "Ash Road Cell", 0.4f, 0.5f);
            Assert.That(last, Is.Not.Null);
            Assert.That(last.Kind, Is.EqualTo(nameof(FactionRadioInterceptSystem.InterceptKind.Succession)));
            Assert.That(last.Message, Does.Contain("Ash Road Cell"));
            Assert.That(last.Message, Does.Contain("banner").IgnoreCase);
            Assert.That(last.Day, Is.EqualTo(42));
            Assert.That(radio.LastInterceptMessage, Does.Contain("Ash Road Cell"));

            // Succession blends trust toward starting — push back below raid line.
            eco.SetTrust(fid, -80f);

            // Hatch repel chatter (not auto-surrender)
            last = null;
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(last, Is.Not.Null);
            Assert.That(last.Kind, Is.EqualTo(nameof(FactionRadioInterceptSystem.InterceptKind.HatchRepel)));
            Assert.That(last.Message, Does.Contain("Hatch bounce").IgnoreCase);

            // Manual parley → Parley intercept (not Auto)
            last = null;
            var s = eco.DemandParley(fid);
            Assert.That(s.Applied, Is.True);
            Assert.That(last, Is.Not.Null);
            Assert.That(last.Kind, Is.EqualTo(nameof(FactionRadioInterceptSystem.InterceptKind.Parley)));
            Assert.That(last.Message, Does.Contain("Parley").IgnoreCase);
            Assert.That(last.Message, Does.Contain("Ash Road Cell").Or.Contain("raid").IgnoreCase);

            // Save/load intercept log
            var snap = radio.CaptureState();
            var radio2 = new FactionRadioInterceptSystem();
            radio2.RestoreState(snap);
            Assert.That(radio2.Log.Count, Is.EqualTo(radio.Log.Count));
            Assert.That(radio2.LastInterceptMessage, Is.EqualTo(radio.LastInterceptMessage));

            radio.Unbind();
        }

        [Test]
        public void FactionRadioIntercepts_AutoSurrender_UsesSurrenderKind()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.MilitaryRemnants;
            eco.SetTrust(fid, -90f);
            eco.SetRaidAggression(fid, 0.6f);

            var radio = new FactionRadioInterceptSystem();
            radio.Bind(eco, () => 50);
            FactionRadioInterceptSystem.InterceptEntry surrenderLine = null;
            radio.OnIntercept += e =>
            {
                if (e.Kind == nameof(FactionRadioInterceptSystem.InterceptKind.Surrender))
                    surrenderLine = e;
            };

            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(eco.HasSurrendered(fid), Is.True);
            Assert.That(surrenderLine, Is.Not.Null);
            Assert.That(surrenderLine.Message, Does.Contain("quiet").IgnoreCase
                .Or.Contain("stops").IgnoreCase);
            radio.Unbind();
        }

        [Test]
        public void RadioInterceptHUD_Push_ShowsStatusAndDetail()
        {
            var go = new GameObject("RadioHUDTest");
            _toDestroy.Add(go);
            var hud = go.AddComponent<RadioInterceptHUD>();

            Assert.That(hud.StatusLine, Does.Contain("quiet").IgnoreCase.Or.Contain("RADIO"));
            hud.Push("Hatch bounce. They curse, then dead air.", "HatchRepel",
                FactionSO.Ids.ScavengerCamp, day: 41);

            Assert.That(hud.LineCount, Is.EqualTo(1));
            Assert.That(hud.HasUnread, Is.True);
            Assert.That(hud.LatestKind, Is.EqualTo("HatchRepel"));
            Assert.That(hud.LatestMessage, Does.Contain("Hatch bounce"));
            Assert.That(hud.StatusLine, Does.Contain("RADIO"));
            Assert.That(hud.StatusLine, Does.Contain("HATCH").Or.Contain("NEW"));
            Assert.That(hud.DetailSummary, Does.Contain("Hatch bounce"));
            Assert.That(hud.DetailSummary, Does.Contain("D41"));

            hud.Open();
            Assert.That(hud.IsOpen, Is.True);
            Assert.That(hud.HasUnread, Is.False, "Open marks strip read");

            hud.Push("Parley on the air. Stand-down.", "Parley",
                FactionSO.Ids.ScavengerCamp, day: 42);
            Assert.That(hud.LineCount, Is.EqualTo(2));
            Assert.That(hud.HasUnread, Is.True);
            Assert.That(hud.LatestKind, Is.EqualTo("Parley"));
            Assert.That(hud.StatusLine, Does.Contain("PARLEY").Or.Contain("Parley").IgnoreCase);

            // SetLines replaces
            hud.SetLines(new[]
            {
                new RadioInterceptHUD.Line
                {
                    Message = "Band shift. New banner.",
                    Kind = "Succession",
                    FactionId = FactionSO.Ids.MilitaryRemnants,
                    Day = 10
                }
            });
            Assert.That(hud.LineCount, Is.EqualTo(1));
            Assert.That(hud.LatestKind, Is.EqualTo("Succession"));
            Assert.That(hud.StatusLine, Does.Contain("BAND").Or.Contain("Band").IgnoreCase);
        }

        [Test]
        public void ParleyBarterSoftener_ImprovesSellAndBuyAfterSurrender()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);
            string fid = FactionSO.Ids.ScavengerCamp;
            var food = MakeItem("canned_food", ItemType.Food);
            food.tradeValue = 10f;

            eco.SetTrust(fid, 0f);
            float sellBefore = eco.GetBarterUnitValue(food, fid, playerSelling: true);
            float buyBefore = eco.GetBarterUnitValue(food, fid, playerSelling: false);
            Assert.That(sellBefore, Is.GreaterThan(0f));
            Assert.That(buyBefore, Is.GreaterThan(0f));

            // Hold hatch + parley → HasSurrendered → softener
            eco.SetTrust(fid, -80f);
            Assert.That(eco.TryLaunchRaid(fid, ignoreDayGate: true).Repelled, Is.True);
            Assert.That(eco.DemandParley(fid).Applied, Is.True);
            Assert.That(eco.HasSurrendered(fid), Is.True);

            // Pin trust so only softener differs from pre-parley neutral math
            eco.SetTrust(fid, 0f);
            float sellAfter = eco.GetBarterUnitValue(food, fid, playerSelling: true);
            float buyAfter = eco.GetBarterUnitValue(food, fid, playerSelling: false);

            Assert.That(sellAfter, Is.EqualTo(sellBefore * (1f + DynamicEconomySystem.ParleyBarterSellBonus))
                .Within(Eps));
            Assert.That(buyAfter, Is.EqualTo(buyBefore * (1f - DynamicEconomySystem.ParleyBarterBuyDiscount))
                .Within(Eps));
            Assert.That(sellAfter, Is.GreaterThan(sellBefore));
            Assert.That(buyAfter, Is.LessThan(buyBefore));
        }

        [Test]
        public void ParleyOfferEvent_FactionFlavor_UsesChannelTagsAndDistinctCopy()
        {
            var (eco, _, _, _) = MakeWiredStack(day: 40, securityOverride: 120f);

            // Military
            string mil = FactionSO.Ids.MilitaryRemnants;
            eco.SetTrust(mil, -90f);
            Assert.That(eco.TryLaunchRaid(mil, ignoreDayGate: true).Repelled, Is.True);
            var milEv = eco.CreateParleyOfferEvent(mil);
            _toDestroy.Add(milEv);
            Assert.That(milEv.bodyText, Does.Contain(DynamicEconomySystem.GetParleyChannelTag(mil)));
            Assert.That(milEv.bodyText, Does.Contain("formal").IgnoreCase
                .Or.Contain("Orders").IgnoreCase);
            Assert.That(milEv.choices.Find(c => c.ChoiceId == "parley_now").Text,
                Does.Contain("formal").IgnoreCase.Or.Contain("stand-down").IgnoreCase);

            // Scavenger
            string scav = FactionSO.Ids.ScavengerCamp;
            eco.SetTrust(scav, -80f);
            Assert.That(eco.TryLaunchRaid(scav, ignoreDayGate: true).Repelled, Is.True);
            var scavEv = eco.CreateParleyOfferEvent(scav);
            _toDestroy.Add(scavEv);
            Assert.That(scavEv.bodyText, Does.Contain("CH-3").Or.Contain("ASH ROAD"));
            Assert.That(scavEv.bodyText, Does.Contain("Market").IgnoreCase
                .Or.Contain("road").IgnoreCase);
            Assert.That(scavEv.choices.Find(c => c.ChoiceId == "open_trade").Text,
                Does.Contain("bag").IgnoreCase.Or.Contain("prices").IgnoreCase);

            // Preppers
            string prep = FactionSO.Ids.DoomsdayPreppers;
            eco.SetTrust(prep, -80f);
            Assert.That(eco.TryLaunchRaid(prep, ignoreDayGate: true).Repelled, Is.True);
            var prepEv = eco.CreateParleyOfferEvent(prep);
            _toDestroy.Add(prepEv);
            Assert.That(prepEv.bodyText, Does.Contain("STOCKPILE").Or.Contain("CH-11"));
            Assert.That(prepEv.bodyText, Does.Contain("hymn").IgnoreCase
                .Or.Contain("sermon").IgnoreCase
                .Or.Contain("inventory").IgnoreCase);
            Assert.That(prepEv.choices.Find(c => c.ChoiceId == "dismiss").Text,
                Does.Contain("stores").IgnoreCase.Or.Contain("Silence").IgnoreCase);

            // Distinct channel tags across factions
            Assert.That(DynamicEconomySystem.GetParleyChannelTag(mil),
                Is.Not.EqualTo(DynamicEconomySystem.GetParleyChannelTag(scav)));
            Assert.That(DynamicEconomySystem.GetParleyChannelTag(scav),
                Is.Not.EqualTo(DynamicEconomySystem.GetParleyChannelTag(prep)));
        }
    }
}
