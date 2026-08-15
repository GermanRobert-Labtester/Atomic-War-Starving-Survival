using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {

        private void InitDiaryAndHatchSystems()
        {
            InitDiaryCatalog();
            InitHatchSideSystems();
        }

        private void InitDiaryCatalog()
        {
            // Prompt #5 — Diary Fragment Catalog (Previous Tenants)
            // ───────────────────────────────────────────────────────────
            DiaryCatalog = new List<DiaryFragmentSO>();
            // Load diary fragments from Resources or StreamingAssets
            var loadedDiaries = Resources.LoadAll<DiaryFragmentSO>("Diaries");
            if (loadedDiaries != null && loadedDiaries.Length > 0)
            {
                DiaryCatalog.AddRange(loadedDiaries);
            }
            // If no authored diaries exist, create default ones inline so the
            // rubble-clearing system has content to reveal.
            if (DiaryCatalog.Count == 0)
            {
                DiaryCatalog.Add(CreateDefaultDiary(new DiarySeed
                {
                    Id = "diary_filter_is_a_lie",
                    Title = "Torn Notebook Page",
                    Text = "The filter is a lie. I watched them install it. It doesn't purify anything — " +
                           "it just pushes the radon deeper into the vents. The reading at the intake looks " +
                           "clean because it bypasses the sensor. We've been breathing poison for three weeks. " +
                           "I don't know how to tell the others. — M.",
                    Author = "M.",
                    RoomId = "deep_vault",
                    WarnsSystem = "air_filtration",
                    Page = 0,
                    Total = 3
                }));

                DiaryCatalog.Add(CreateDefaultDiary(new DiarySeed
                {
                    Id = "diary_water_truth",
                    Title = "Water-Stained Journal",
                    Text = "The catchment on the roof is cracked. Has been since the first mortar. " +
                           "Every time it rains, we cheer — but the water tastes like metal and the " +
                           "geiger clicks faster every time we boil it. I tried to patch it last week " +
                           "but the suit tore and I couldn't stay out there. The crack is getting wider. " +
                           "— Unknown",
                    Author = "Unknown",
                    RoomId = "deep_vault",
                    WarnsSystem = "water_purifier",
                    Page = 1,
                    Total = 3
                }));

                DiaryCatalog.Add(CreateDefaultDiary(new DiarySeed
                {
                    Id = "diary_shielding_rot",
                    Title = "Last Entry of the Engineer",
                    Text = "The shielding in the deep vault was never finished. They poured half the " +
                           "concrete and ran out of aggregate. The plans say six inches. There's maybe " +
                           "two. I've been sleeping against the wrong wall for a month. The skin on " +
                           "my back is peeling and I don't think it's just dry air anymore. " +
                           "If you're reading this — check the east wall. Check it with a dosimeter, " +
                           "not the panel. The panel lies. — Engineer Kostya",
                    Author = "Engineer Kostya",
                    RoomId = "deep_vault",
                    WarnsSystem = "radiation_shielding",
                    Page = 2,
                    Total = 3
                }));
            }
            // Wire diary reveal into JournalSystem (simplified — logs via debug; full
            // JournalSystem integration can use AddEntryFactory when needed)
            var clearRubbleAction = Actions.Find(a => a is ClearRubbleActionSO) as ClearRubbleActionSO;
            if (clearRubbleAction != null)
            {
                clearRubbleAction.OnDiaryRevealed = (roomId, fragmentIndex) =>
                {
                    if (DiaryCatalog != null)
                    {
                        foreach (var diary in DiaryCatalog)
                        {
                            if (diary != null && diary.foundInRoomId == roomId && diary.pageOrder == fragmentIndex && !diary.IsFound)
                            {
                                diary.IsFound = true;
                                GameLog.Log($"[Diary] Found in {roomId}: \"{diary.title}\" — {diary.text}");
                                return diary.text;
                            }
                        }
                    }
                    return null;
                };
            }

            // Hatch-dilemma prompt: tracks the active "knock at the
            // hatch" decision and provides a timeout so the survivor
            // doesn't sit in AtHatchDilemma forever. The UI flow is
            // wired in OnHatchDilemmaReady_Handle (EventRunner.Run shows
            // the modal; the prompt's Tick advances the timeout).
            HatchDilemmaPromptField = new HatchDilemmaPrompt();

            // Hatch defense (Prompt #33): security vs raids, guard duty, loot theft
            
        }

        private void InitHatchSideSystems()
        {HatchDefenseSystem = new HatchDefenseSystem(
                getShelter: () => Shelter,
                getInventory: () => Inventory,
                getSurvivors: () => Survivors,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                inflictTrauma: (sv, affId) => MedicalSystem?.Inflict(sv, affId),
                rng: new System.Random(_worldSeed + 33));
            // Starting hatch plate: reinforced locks at level 1
            Shelter.AddModule(new ShelterModuleInstance(
                HatchDefenseModuleSO.ReinforcedLocksId, 1)
            {
                SecurityContribution = 10f,
                FilterHealth = 100f
            });
            // Workbench lists hatch install / upgrade lines (scrap sink)
            WorkbenchSystem?.SetHatchDefense(HatchDefenseSystem);
            HatchDefenseSystem.SetNeedsSystem(NeedsSystem);

            // Dynamic phase economy + faction trust matrix
            EconomySystem = new DynamicEconomySystem(
                getPhase: () => WorldPhaseSystem.CurrentPhase,
                shelter: Shelter,
                decisionSeed: _worldSeed + 91);
            foreach (var fac in DynamicEconomySystem.CreateDefaultFactions())
                EconomySystem.RegisterFaction(fac);
            EconomySystem.SetHatchDefense(HatchDefenseSystem);
            EconomySystem.SetDayProvider(() => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            // Cult of the Glow (trustInversion): disposition tracks party radiation dose.
            EconomySystem.SetPartyRadiationProvider(GetPartyAverageRadiationDose);
            // #16 polish: ARS reverence + intact-hazmat contempt providers.
            EconomySystem.SetPartyHasArsProvider(PartyHasAcuteRadiationSyndrome);
            EconomySystem.SetPartyIntactHazmatProvider(PartyWearsIntactHazmat);
            // REPROMOTE-001 — PassiveTrader weather exchange rates on barter quotes.
            // Lambda resolves NPCPassiveTrader/Weather at call time (BootNPC already ran).
            EconomySystem.SetWeatherItemPriceMultiplier(itemId =>
            {
                if (NPCPassiveTrader == null || WeatherSystem == null) return 1f;
                return NPCPassiveTrader.GetPriceMultiplierForItem(
                    itemId, WeatherSystem.Current.ToString());
            });
            EconomySystem.BindEventRunner(EventRunner);

            // Post-repel parley modal + faction radio intercept log
            ParleyOfferPromptField = new ParleyOfferPrompt();
            FactionRadioIntercepts = new FactionRadioInterceptSystem();
            FactionRadioIntercepts.Bind(
                EconomySystem,
                () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EconomySystem.OnRaidResolved += OnFactionRaidResolved_Handle;
            _subscriptions.Track(() => EconomySystem.OnRaidResolved -= OnFactionRaidResolved_Handle);

            Action<FactionRadioInterceptSystem.InterceptEntry> onFactionRadioIntercept = entry =>
            {
                if (entry == null || string.IsNullOrEmpty(entry.Message)) return;
                GameLog.Log($"[Radio intercept] {entry.Message}");
                PushRadioInterceptToHud(entry);
            };
            FactionRadioIntercepts.OnIntercept += onFactionRadioIntercept;
            _subscriptions.Track(() => FactionRadioIntercepts.OnIntercept -= onFactionRadioIntercept);

            // Expansion II Part II — create the four faction-pressure systems
            // and wire them into the host. The wiring is a static class
            // that subscribes its own OnRaidResolved handler and pipes
            // OnX events to the radio intercept log.
            GarrisonComplianceLedger = GarrisonComplianceLedger ?? new System_GarrisonComplianceLedger();
            MilitiaContributionTax = MilitiaContributionTax ?? new System_MilitiaContributionTax();
            CultLeash = CultLeash ?? new System_CultLeash();
            WarlordTributeSystem = WarlordTributeSystem ?? new System_WarlordTribute();
            // Register with the save system so they round-trip cleanly.
            if (SaveSystem != null)
            {
                SaveSystem.SetGarrisonComplianceLedgerSystem(GarrisonComplianceLedger);
                SaveSystem.SetMilitiaContributionTaxSystem(MilitiaContributionTax);
                SaveSystem.SetCultLeashSystem(CultLeash);
                SaveSystem.SetWarlordTributeSystem(WarlordTributeSystem);
            }

            FactionPressureWiring.GarrisonLedger = GarrisonComplianceLedger;
            FactionPressureWiring.MilitiaTax = MilitiaContributionTax;
            FactionPressureWiring.CultLeash = CultLeash;
            FactionPressureWiring.WarlordTribute = WarlordTributeSystem;
            FactionPressureWiring.RadioIntercepts = FactionRadioIntercepts;
            FactionPressureWiring.ShelterIdProvider = () => "shelter_player";
            FactionPressureWiring.DayProvider = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;
            FactionPressureWiring.WireIntoHost(this);
            _subscriptions.Track(() => FactionPressureWiring.Unwire());

            // Cult of the Glow quest: subscribe to the new OnCommunionMissed
            // event so terminal branches feed the leash system. The quest
            // is created lazily; FactionPressureWiring.AttachCultQuest(...)
            // tracks the subscription idempotently.
            // (No QuestRegistry wired in this host — the wiring helper has
            // a public AttachCultQuest for any later caller to invoke.)

        }


    }
}
