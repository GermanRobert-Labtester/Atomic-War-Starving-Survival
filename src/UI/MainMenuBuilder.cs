using System;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Constructs the in-game operations menu, categorizing dev, simulation,
    /// and expansion actions (including the 6 Muster systems) while keeping Main.cs clean.
    /// </summary>
    public static class MainMenuBuilder
    {
        public static void BuildMenu(
            VBoxContainer menuContainer,
            Action<string, Action> addBtn,
            Action<string> addHeader,
            Action onColdCount,
            Action onHydroBarons,
            Action onIronRaiders,
            Action onLongWalk,
            Action onProvisioned,
            Action onScavengerGuild,
            Action onStartGame,
            Action onTickIceRoad,
            Action onCycleWeather,
            Action onShowBriefing,
            Action onCensusLevy,
            Action onOrder12C,
            Action onUnlockPlant,
            Action onRepairMembrane,
            Action onToggleOutfall,
            Action onSaveHoldfast,
            Action onHoldfastOpen,
            Action onHoldfastNewLedger,
            Action onCycleEnding,
            Action onRosterInspectWall,
            Action onRosterPencil,
            Action onRosterInk,
            Action onRosterBurn,
            Action onRosterTickNight,
            Action onRosterVisitor,
            Action onRosterSecondWinter,
            Action onWaystationTick,
            Action onWaystationWatch,
            Action onStandingRecord,
            Action onRecordWalkKm19,
            Action onCrossingVouch,
            Action onCrossingBurn,
            Action onArbitrationLoadBackers,
            Action onArbitrationCallStanding,
            Action onArbitrationBribe,
            Action onArbitrationOverturn,
            Action onLedgerSign,
            Action onLedgerTick,
            Action onLedgerPay,
            Action onGreenhousePlant,
            Action onGreenhouseTick,
            Action onDoorEncounter,
            Action onTickYearOfAsh,
            Action onQuestlines,
            Action onPhantomScavenge,
            Action onPhantomTick,
            Action onPhase0Scavenge,
            Action onPhase0Noise,
            Action onPhase0Craft,
            Action onPhase0Tick,
            Action onDoseSeal,
            Action onDoseScribe,
            Action onDoseDiagnose,
            Action onDoseCohort,
            Action onDoseVolunteer,
            Action onDoseRegister,
            Action onMusterEscalate,
            Action onMusterRally,
            Action onMusterWitnesses,
            Action onVerdictOpen,
            Action onVerdictTick,
            Action onVerdictCensus,
            Action onMaritimeStartDive,
            Action onMaritimeTickDive,
            Action onMaritimeScavenge,
            Action onExpeditionTick,
            Action onExpeditionDive,
            Action onViewCodex,
            Action onDiagnostics,
            Action onEconomyOpen,
            Action onInventoryOpen,
            Action onSurvivorsOpen,
            Action openSettings,
            Action openCrafting,
            Action openRadio,
            Action openMedical,
            Action openPhase0,
            Action openDutyRoster,
            Action openExpedition,
            Action openWeather,
            Action openQuests,
            Action openJournal,
            Action openFactions,
            Action openShelter,
            Action openCombat,
            Action openMap,
            Action openSaveLoad,
            Action onExitGame)
        {
            if (menuContainer == null) return;

            // ── Primary Simulation Actions ──
            addHeader("SURVIVAL OPERATIONS");
            addBtn("Start Survival Simulation", onStartGame);
            addBtn("Tick ice-road day", onTickIceRoad);
            addBtn("Cycle weather", onCycleWeather);
            addBtn("Show quest briefing", onShowBriefing);
            addBtn("Census honour levy", onCensusLevy);
            addBtn("Order 12-C (office acts)", onOrder12C);

            // ── Six Muster Systems (Expansion 06) ──
            addHeader("EXPANSION 06: THE MUSTER");
            addBtn("Cold Count (142.850 MHz)", onColdCount);
            addBtn("Hydro-Barons (Rate Card)", onHydroBarons);
            addBtn("Iron Raiders (Den Defense)", onIronRaiders);
            addBtn("The Long Walk (Circuit)", onLongWalk);
            addBtn("The Provisioned (Homestead)", onProvisioned);
            addBtn("Scavenger Guild (Claim Map)", onScavengerGuild);
            addBtn("Muster: escalate to Day 260", onMusterEscalate);
            addBtn("Muster: rally a deserter", onMusterRally);
            addBtn("Muster: witness accounts", onMusterWitnesses);

            // ── Holdfast & Infrastructure ──
            addHeader("HOLDFAST & INFRASTRUCTURE");
            addBtn("Unlock plant (salt trade)", onUnlockPlant);
            addBtn("Repair membrane (resin)", onRepairMembrane);
            addBtn("Toggle outfall shift", onToggleOutfall);
            addBtn("Save holdfast state", onSaveHoldfast);
            addBtn("Holdfast: open terminal", onHoldfastOpen);
            addBtn("Holdfast: new ledger", onHoldfastNewLedger);
            addBtn("Cycle ending (S4)", onCycleEnding);

            // ── Duty Roster & Waystations ──
            addHeader("DUTY ROSTER & WAYSTATIONS");
            addBtn("Roster: inspect the Chart", onRosterInspectWall);
            addBtn("Roster: morning row (pencil)", onRosterPencil);
            addBtn("Roster: ink the wall (ending)", onRosterInk);
            addBtn("Roster: burn the chart", onRosterBurn);
            addBtn("Roster: tick a night (encounters)", onRosterTickNight);
            addBtn("Roster: queue a visitor (hatch)", onRosterVisitor);
            addBtn("Duty Roster: Second Winter", onRosterSecondWinter);
            addBtn("Waystation: unlock + tick", onWaystationTick);
            addBtn("Waystation: assign watch", onWaystationWatch);
            addBtn("Standing Record: inspect", onStandingRecord);
            addBtn("Standing Record: walk Km 19", onRecordWalkKm19);

            // ── Crossing & Arbitration ──
            addHeader("CROSSING & ARBITRATION");
            addBtn("Crossing: grant vouch (Osran)", onCrossingVouch);
            addBtn("Crossing: burn vouch", onCrossingBurn);
            addBtn("Arbitration: load backers", onArbitrationLoadBackers);
            addBtn("Arbitration: call Standing", onArbitrationCallStanding);
            addBtn("Arbitration: bribe a backer", onArbitrationBribe);
            addBtn("Arbitration: overturn ruling", onArbitrationOverturn);
            addBtn("Ledger: present + sign contract", onLedgerSign);
            addBtn("Ledger: tick day", onLedgerTick);
            addBtn("Ledger: pay contract", onLedgerPay);
            addBtn("Greenhouse: plant + water", onGreenhousePlant);
            addBtn("Greenhouse: tick + harvest", onGreenhouseTick);

            // ── Year of Ash ──
            addHeader("YEAR OF ASH");
            addBtn("Hatch Encounter (Year of Ash)", onDoorEncounter);
            addBtn("Tick Year of Ash (+10 Days)", onTickYearOfAsh);
            addBtn("Year of Ash: questlines", onQuestlines);

            // ── Phantom Memory & Phase 0 ──
            addHeader("PHANTOM MEMORY & PHASE 0");
            addBtn("Phantom Memory: scavenge item", onPhantomScavenge);
            addBtn("Phantom Memory: tick hour", onPhantomTick);
            addBtn("Phase-0: scavenge trigger", onPhase0Scavenge);
            addBtn("Phase-0: raise noise (flashbacks)", onPhase0Noise);
            addBtn("Phase-0: craft specialty item", onPhase0Craft);
            addBtn("Phase-0: tick 6 hours", onPhase0Tick);

            // ── Dose Register & Verdict ──
            addHeader("DOSE REGISTER & VERDICT");
            addBtn("Dose: seal dosimeters", onDoseSeal);
            addBtn("Dose: book a reading", onDoseScribe);
            addBtn("Dose: name to Sick List", onDoseDiagnose);
            addBtn("Dose: book a Cohort child", onDoseCohort);
            addBtn("Dose: sign a volunteer", onDoseVolunteer);
            addBtn("Dose: open the Register", onDoseRegister);
            addBtn("Verdict: open the machine readout", onVerdictOpen);
            addBtn("Verdict: advance reckoning a day", onVerdictTick);
            addBtn("Verdict: census window now", onVerdictCensus);

            // ── Maritime & Expeditions ──
            addHeader("MARITIME & EXPEDITIONS");
            addBtn("Maritime: start stealth dive", onMaritimeStartDive);
            addBtn("Maritime: tick dive 10s", onMaritimeTickDive);
            addBtn("Maritime: scavenge stadium", onMaritimeScavenge);
            addBtn("Expedition: tick 2 hours", onExpeditionTick);
            addBtn("Expedition: start Sovereign dive", onExpeditionDive);

            // ── System Panels & Navigation ──
            addHeader("SYSTEM PANELS");
            addBtn("Open Bunker Ledger  [J]", onViewCodex);
            addBtn("Inspect System Diagnostics", onDiagnostics);
            addBtn("Economy: open market", onEconomyOpen);
            addBtn("Inventory: open panel", onInventoryOpen);
            addBtn("Survivors: open panel", onSurvivorsOpen);
            addBtn("Settings: audio & gameplay", openSettings);
            addBtn("Crafting: open panel", openCrafting);
            addBtn("Radio: open panel", openRadio);
            addBtn("Medical: open panel", openMedical);
            addBtn("Phase-0: conditions & treatment", openPhase0);
            addBtn("Duty Roster: open panel", openDutyRoster);
            addBtn("Expeditions: open panel", openExpedition);
            addBtn("Weather: open panel", openWeather);
            addBtn("Quests: open panel", openQuests);
            addBtn("Journal: open panel", openJournal);
            addBtn("Factions: open panel", openFactions);
            addBtn("Shelter: open panel", openShelter);
            addBtn("Combat: open panel", openCombat);
            addBtn("Map: open panel", openMap);
            addBtn("Save/Load: open panel", openSaveLoad);
            addBtn("Exit Game", onExitGame);
        }
    }
}
