#!/usr/bin/env python3
"""Rebuild the fifteen 2026-09-24 authority integration planning addenda.

The short dated source document stays intact above the first addendum marker.
Candidate review cases are non-canon questions, not gameplay data or a claim
that the named feature is unintegrated. Run the separate editorial pass after.
"""

from __future__ import annotations

from gen_requested_ten_integration_expansions_21_30 import (
    ROOT, MARKER_PREFIX, intro, card, close, CONDITIONS, rows,
)


def spec(path, name, core, source, host, save, catalog, key, api,
         premise, first, owner, state, gate, cluster, obj, scenes):
    return dict(path=path, name=name, core=core, source=source, host=host,
                save=save, catalog=catalog, key=key, api=api,
                premise=premise, first=first, owner=owner, state=state,
                gate=gate, cluster=cluster, object=obj,
                scenes=scenes.split("|"))


SPECS = [
    spec("docs/bodymind/PSYCHOLOGICAL_CONTAMINATION_RECOVERY_MATRIX.md",
         "Psychological Contamination System", "PsychologicalContaminationSystem",
         "Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs",
         "MaritimeHostSession / Main.Maritime", "maritime envelope via MaritimeHostSave.Psychology",
         "psychological_trauma.json", "trauma_types",
         "ApplyContamination, HasContamination, IsActionBlocked, GetStage, GroundSurvivor, ApplyShelterRest, Tick, CaptureState, RestoreState",
         "MaritimeHostSession constructs and saves Psychology, raises changes on apply/expiry, and exposes a demonstration contamination command. The recovery matrix predates that host evidence. Verify whether regular campaign day ticks, companion grounding, and actual duty restrictions use its live state before calling the system unintegrated.",
         "one real maritime exposure enters the existing psychological state, survives restore, and has a truthful recovery and assignment readout",
         "Maritime psychology owns contamination entries; survivor assignment, morale, bond, and medical owners provide inputs and accept only their own effects",
         "Use the existing maritime section and PsychContaminationSave; do not create a general psychology save from these entries.",
         "Do not conflate this maritime contamination model with the separate trauma, therapy, or ideological-friction owners; no automatic diagnosis from narrative tone.",
         "C5 and C9", "a diver's debrief card",
         "first contaminated return|quiet recovery day|companioned grounding|repeated dive|low morale return|assignment mismatch|refused dive order|shelter rest|two affected divers|recovered survivor|missing companion|dangerous salvage|failed reassurance|day-boundary recovery|maritime panel status|later expedition recall"),
    spec("docs/expansions/CROSSING_DEPTH_AUDIT.md",
         "Crossing Arbitration System", "CrossingArbitrationSystem",
         "Assets/Ashfall.Core/CrossingArbitrationSystem.cs",
         "ExpansionHostSession / Crossing player surface", "ExpansionHubSave arbitration state",
         "crossing_quests.json", "quests",
         "LoadBackerPool, CallStanding, DeclareBacker, TryBribeBacker, OverturnRuling, GetRulingHistory, CaptureState, RestoreState",
         "ExpansionHostSession constructs Arbitration, loads a default backer pool, subscribes to changes, and captures it in ExpansionHubSave. The remaining premise is whether live quests and player commands can call a standing, declare a backer, and display an active or overturned ruling.",
         "one reachable dispute proceeds through a standing call, backer declaration, verdict readout, and save/reload without a duplicate ruling",
         "Arbitration owns rulings and backer histories; CrossingQuestSystem owns quest progress and VouchAccessSystem owns access",
         "Keep the arbitration state in the existing expansion hub envelope.",
         "A backer's support is neither a quest flag nor faction standing; never make a second vote ledger in the panel.",
         "C7 and C10", "a signed standing petition",
         "first standing|principled backer|ambivalent backer|public refusal|bribe offer|bribe refused|bribe accepted|counter-backer petition|overturned ruling|repeat standing|unavailable supporter|outdated petition|quest-stage handoff|vouch consequence|public notice|appeal archive"),
    spec("docs/economy/DEBT_SAVE_CONTRACT.md",
         "Ledger Debt System", "LedgerDebtSystem",
         "Assets/Ashfall.Core/LedgerDebtSystem.cs",
         "ExpansionHostSession + DebtConsequenceDispatcher", "ExpansionHubSave ledger plus dispatcher bridge state",
         "ledger_debt_templates.json", "templates",
         "PresentContract, SignContract, CancelDraft, TickDaily, PayContract, ForgiveContract, RenegotiateContract, TotalOwed, CaptureState, RestoreState",
         "ExpansionHostSession constructs Ledger, loads the debt templates, attaches DebtConsequenceDispatcher, and captures ledger plus bridge state in the expansion hub envelope. Current Plan 40 work includes templates. The open concern is whether offers, payments, forfeits, and UI text settle against canonical funds and goods rather than only a ledger number.",
         "one catalog-backed debt offer can be previewed, signed, paid or refused, and restored with the same balance and no repeated consequence",
         "LedgerDebtSystem owns debt contracts; the canonical funds/goods ledger settles transfers; DebtConsequenceDispatcher translates one accepted debt fact to existing consequence owners",
         "Retain ExpansionHubSave; reconcile dispatcher/bridge receipts before adding fields.",
         "No second currency balance, creditor inventory, compounding clock, or autonomous loan-shark settlement.",
         "C11", "a creased debt statement",
         "offer terms|draft signature|debtor refusal|creditor identity|interest tick|partial resources|payment attempt|forfeit warning|deadline crossed|renegotiation|forgiveness|tampered entry|closed contract|old save|dispatcher consequence|ledger view"),
    spec("docs/world/CARAVAN_CIRCUIT_MATRIX.md",
         "Traveling Caravan System", "TravelingCaravanSystem",
         "Assets/Ashfall.Core/TravelingCaravanSystem.cs",
         "TravelingCaravanHostSession / Main.Economy and Main.ShelterBatch3",
         "caravan section via CaravanSaveStore", "caravans.json", "caravans",
         "SpawnCaravan, DailyTick, GetCaravanAtNode, CheckRouteEncounter, ResolveRouteEncounterChoice, TryBuyItem, CaptureState, RestoreState",
         "TravelingCaravanHostSession already creates, ticks, trades, and saves caravan state. Both Main.Economy and Main.ShelterBatch3 declare or create caravan sessions; audit runtime ownership before adding another route. Host Buy passes a mutable player-rations argument, so canonical economy settlement requires explicit proof.",
         "one live caravan arrives on a current route, offers a canonical-stock trade, and preserves route and goods state after reload",
         "TravelingCaravanSystem owns moving caravans and their stock; campaign clock owns the day; inventory/funds owner settles the player's side of a trade",
         "Use CaravanSaveStore through the one chosen Main field and day owner.",
         "Do not instantiate two active caravan sessions or debit only a copied rations integer.",
         "C5 and C11", "a caravan route board",
         "road arrival|weather delay|faction embargo|empty wagon|merchant specialty|trade preview|stock shortage|ration shortfall|route encounter|choice resolution|waystation stop|missed caravan|repeat purchase|day transition|caravan departure|old route restore"),
    spec("docs/expansions/STANDING_RECORD_DEPTH_AUDIT.md",
         "Location Memory System", "LocationMemorySystem",
         "Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs",
         "StandingRecordHostSession and ExpansionHostSession", "standing_record envelope and ExpansionHubSave both contain memory state",
         "standing_record_memory.json", "items",
         "Load, Unlock, ApplyMutation, GetActiveRecast, GetStratumText, RecordSiteVisit, CaptureState, RestoreState",
         "StandingRecordEngine owns a LocationMemorySystem and StandingRecordHostSession saves its unified envelope; ExpansionHostSession also constructs LocationMemorySystem and captures it through ExpansionHubSave. This is a two-instance authority collision until live Main bindings, event paths, and migration precedence are proven.",
         "one canonical site visit records a memory stratum and revisit recast in exactly one save owner and one player atlas route",
         "Choose one standing-record owner for visit history, mutation flags, strata, and recasts; expedition/quest owners only publish visit facts",
         "Resolve duplicate standing_record versus expansion_hub memory state and choose migration precedence before editing a save codec.",
         "No new memory cache, room-visit counter, or prose fact derived from the wrong host instance.",
         "C5, C6, and C13", "a margin note on a site map",
         "first site visit|second visit|mutated room|unlocked stratum|uncertain witness|recast text|room destroyed|site renamed|duplicate arrival|stale expedition note|late discovery|atlas binding|quest mutation|day transition|old envelope|conflicting saves"),
    spec("docs/expansions/STANDING_RECORD_DEPTH_AUDIT.md",
         "Site Encounter System", "SiteEncounterSystem",
         "Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs",
         "StandingRecordHostSession and ExpansionHostSession", "standing_record envelope and ExpansionHubSave both contain encounter state",
         "standing_record_layouts.json", "items",
         "StartEncounter, ResolveEncounter, IsResolved, ScrapePlate, RestoreOverlayAccess, CaptureState, RestoreState",
         "StandingRecordEngine owns a SiteEncounterSystem while ExpansionHostSession constructs another and saves it in ExpansionHubSave. The two save paths may diverge. The first task is to trace the actual player room entry and choose one canonical encounter instance, including legacy restore precedence.",
         "one real room entry starts one encounter, one choice resolves it, and atlas/reload agree on the resolved ID",
         "The chosen standing-record owner writes encounter history and overlay access; room layout and quest owners provide facts without keeping a second resolution list",
         "Consolidate or explicitly bridge the two existing envelopes; no third encounter save.",
         "Do not trigger a site encounter from a panel refresh or use plate scraping as a generic history reset.",
         "C5 and C13", "a room threshold record",
         "first room entry|encounter eligibility|quiet room|hazard room|companion witness|unread plate|plate scraped|overlay restored|choice refusal|resolved encounter|repeat entry|room mutation|site revisit|missing layout|old envelope|conflicting saves"),
    spec("docs/spiritual/SPIRITUAL_AUTHORITY_MAP.md",
         "Cohort System", "CohortSystem",
         "Assets/Ashfall.Core/CohortSystem.cs", "DoseLedgerHostSession / campaign cohort route",
         "DoseLedgerSave.cohort", "cohort_tuning.json", "",
         "BookChild, CorrectBaseline, TryMaturation, CheckCohortMaturation, MarkChildLost, IsWorkEligible, IsSchoolEligible, CalculateChildFoodUnits, CaptureState, RestoreState",
         "DoseLedgerHostSession constructs CohortSystem and DoseLedgerSave captures it. It currently contains a demonstration booking path. The current integration question is which actual birth, ration, school, loss, and maturation facts reach this state and which player surface reads them; a dated partial label cannot erase the existing save.",
         "one real child record enters the dose-ledger cohort state, contributes to a canonical ration decision, and progresses once across a restored day",
         "Cohort owns child life-stage and baseline record; genealogy, school, ration, and survivor roster own their respective downstream consequences",
         "Keep cohort in DoseLedgerSave, with old-save defaults and no second genealogy child record of the same fact.",
         "Do not reintroduce the retired OnChildAged event or create a faith meter from cohort memories.",
         "C9 and C13", "a child's census line",
         "new child booking|baseline uncertainty|baseline correction|ration policy|school age|first lesson|coming of age|work eligibility|family loss|orphan care|two guardians|old save|seasonal census|mentor memory|long horizon|epilogue witness"),
    spec("docs/bodymind/BODY_MIND_AUTHORITY_MAP.md",
         "Voluntary Register System", "VoluntaryRegisterSystem",
         "Assets/Ashfall.Core/VoluntaryRegisterSystem.cs",
         "DoseLedgerHostSession / task assignment and dose route", "DoseLedgerSave.voluntaryRegister",
         "none by exact type; existing task and radiation owners provide IDs", "",
         "Volunteer, CompleteVolunteer, GetEntry, CaptureState, RestoreState",
         "DoseLedgerHostSession constructs VoluntaryRegisterSystem and DoseLedgerSave captures it, but the visible example signs and completes a hard-coded volunteer task. Production eligibility, consent, task assignment, and incurred dose must be traced from their existing owners.",
         "one real eligible survivor consents to a real hazardous assignment, records its actual dose once, and can review the signed entry after reload",
         "VoluntaryRegister owns consent and completion record; task assignment owns work, radiation ledger owns dose, and survivor roster owns identity",
         "Retain the voluntaryRegister field in DoseLedgerSave and reconcile old records by survivor/task identity.",
         "A voluntary signature must never replace dose accounting, waive safety rules, or be inferred from clicking a status panel.",
         "C2 and C9", "a signed volunteer register",
         "hazard briefing|explicit consent|refused consent|task assignment|dose warning|shift completion|task cancellation|multiple volunteers|repeat signature|survivor missing|dose discrepancy|medical follow-up|old save|bereavement record|register correction|public acknowledgment"),
    spec("docs/utility_ai/UTILITY_OVERRIDE_CONTRACT.md",
         "Utility AI System", "UtilityAiSystem",
         "Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs",
         "UtilityAiHostSession and UtilityAiPanel", "no private mutable state in UtilityAiSystem; override owner to be verified",
         "utility_actions.json", "actions",
         "SelectAction, ScoreAll, Load",
         "UtilityAiHostSession exists, and UtilityAiPanel also constructs a fresh UtilityAiSystem to call ScoreAll. The system scores catalog actions rather than owning an autonomous survivor simulation. Existing Plan 72 enlarged the catalog to 20 non-override actions; its contract states that no executor yet supports emergency interruption. The remaining gap is proving that ordinary action outcomes use the one live duty/task owner, while override remains a gated future path.",
         "one live survivor's eligible ordinary actions are scored, shown, and dispatched through the current task owner without panel-owned decisions",
         "UtilityAiSystem computes scores; duty/task owners own accepted assignments and survivor needs; the panel renders only a projection",
         "The scorer is stateless; any override persistence belongs to the existing assignment/intent owner after a source audit.",
         "Do not store a second action queue, treat a freshly constructed panel scorer as independent authority, or mark an action as override without an interruption executor.",
         "C9 and C17", "a shift decision slip",
         "routine task|urgent thirst|injury restriction|explicit override|expired override|two equal scores|missing action ID|fatigue penalty|skill preference|room unavailable|resource shortage|survivor incapacitated|panel refresh|controller selection|day handoff|reloaded assignment"),
    spec("docs/verdict/VERDICT_RADIO_NPC_HANDOFF.md",
         "Verdict Radio System", "VerdictRadioSystem",
         "Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs",
         "VerdictHostSession / VerdictPanel", "VerdictSave.radio fired IDs",
         "verdict_radio.json", "broadcasts",
         "Poll, HasFired, LoadFrom, CaptureState, RestoreState",
         "VerdictHostSession creates VerdictRadioSystem with the event bus, clock, and current corpus; VerdictPanel reads the session; the save records fired IDs. Existing Plan 94 expanded the broadcast catalog. The remaining question is exact day/phase trigger reach and whether broadcast-to-NPC handoff has a single receipt.",
         "one live Reckoning phase publishes one eligible broadcast, records its fired ID, and shows the same NPC handoff after restore",
         "VerdictRadio owns once-fired broadcast IDs; Reckoning owns phase; radio presentation and VerdictNpcSystem own their respective output routes",
         "Keep fired IDs in VerdictSave; delivery receipt must use the current event/save owner, not a parallel radio archive.",
         "Do not replay a transmission when the panel reopens or infer an NPC reaction from a broadcast that never fired.",
         "C8 and C13", "a receiver's transmission log",
         "phase opening|eligible transmission|missed day|duplicate poll|quiet frequency|fired broadcast|NPC rumor|NPC refusal|signal interruption|verdict change|late restore|archived transcript|panel reopen|same-day phase change|unknown ID|closing transmission"),
    spec("docs/plans/wave11_part1/A2_PLAN41_IMPLEMENTATION_LOG.md",
         "Procedural Eulogy Engine", "ProceduralEulogyEngine",
         "Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs",
         "MemorialSystem consumer; inspect live memorial host provider binding",
         "MemorialEntry.EulogyText plus optional EulogySaveState", "narrative/eulogy_corpus_batch_1.json", "",
         "ComposeEulogy, CaptureState, RestoreState; MemorialSystem.Memorialize",
         "The dated Plan 41 implementation log says eulogies were wired; current Core confirms MemorialSystem.Memorialize calls EulogyEngine.ComposeEulogy only when its nullable provider is bound and no eulogy text was supplied. No direct src reference to ProceduralEulogyEngine was found. Trace provider construction before claiming live funeral prose.",
         "one real memorial receives one survivor-specific eulogy from a bound provider and preserves that exact text after restore",
         "MemorialSystem owns memorial creation and EulogyText; ProceduralEulogyEngine composes text from a DwellerLifeRecord; fate and journal owners supply life facts",
         "Persist the accepted eulogy on MemorialEntry; any engine archive must be reconciled with memorial save instead of duplicating text authority.",
         "Do not generate a death, memorial, or life fact to feed the composer; do not reroll prose on reload.",
         "C9 and C13", "a folded memorial program",
         "first memorial|known vocation|surviving friend|unresolved quarrel|child witness|remembered bark|empty life record|provided eulogy|repeat memorial|family request|quiet ceremony|private archive|old save|duplicate death fact|late provenance|epilogue remembrance"),
    spec("docs/spiritual/SPIRITUAL_AUTHORITY_MAP.md",
         "Ideological Friction System", "IdeologicalFrictionSystem and IdeologicalFrictionEvents",
         "Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs",
         "IdeologicalFrictionHostSession / Main.IdeologicalFriction / SurvivorSocialCoordinator",
         "ideological_friction section; compare coordinator's affinity save",
         "ideological_events.json", "events",
         "RegisterBelief, GetAffinity, TickRoommates, CaptureState, RestoreState; host event commands",
         "The ledger records Plan 148 full integration on 2026-09-23, including IdeologicalFrictionHostSession, Main day owner, save section, and UI. Separately, SurvivorSocialCoordinator has a Friction instance. The immediate task is a collision audit and classification correction, not a new ideology host.",
         "trace one current roommate affinity and one hosted ideological event through their owners and document whether they share or intentionally separate state",
         "The social coordinator owns baseline roommate affinity; the Plan 148 event host owns events, confrontations, and related quests only if current code confirms that split",
         "Preserve the completed ideological_friction section; verify coordinator persistence and avoid dual capture of the same affinity.",
         "No faith meter, no duplicate belief registry, and no reimplementation of completed Plan 148.",
         "C9 and C10", "a communal meeting note",
         "first belief registration|roommate disagreement|quiet tolerance|shared ritual|public confrontation|private mediation|conversion attempt|faction bloc|opposed roommates|new arrival|changed belief|saved affinity|hosted event|quest consequence|panel readout|old save"),
    spec("docs/factions/PATROL_TRIGGER_MATRIX.md",
         "Warlord Doctrine System", "WarlordDoctrineSystem",
         "Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs",
         "YearOfAshHostSession / patrol and faction war adapters",
         "YearOfAsh save envelope", "warlord_doctrines.json", "doctrines",
         "SetTerritoryState, TravelDangerModifier, Observe, ReportedState, TickDaily, SettleTribute, CaptureState, RestoreState",
         "YearOfAshHostSession already constructs WarlordDoctrineSystem with a catalog. The patrol matrix describes encounter selection, but a patrol result must be traced to warlord territory, doctrine, and reported versus actual state before declaring a missing system. The war/faction owners must stay authoritative for standing and battle outcomes.",
         "one current warlord territory fact changes an eligible patrol or travel danger through the existing route and survives reload",
         "WarlordDoctrineSystem owns doctrine and territory observation; TravelEncounterSystem selects encounters, FactionWar owns combat/standing, and economy owns tribute transfers",
         "Retain YearOfAsh state; prove report/territory restore and once-per-day doctrine tick.",
         "Do not let a patrol text line create territory, a raid, or a second tribute balance.",
         "C5 and C7", "a patrol field order",
         "border patrol|uncertain territory|reported control|actual control|doctrine shift|tribute demand|paid tribute|unpaid tribute|hostile access|winter hazard|rival pressure|observed withdrawal|false rumor|travel gate|saved patrol|late faction turn"),
    spec("docs/standing_record/STANDING_RECORD_FACTION_WIRING_TRACER.md",
         "Standing Record Engine", "StandingRecordEngine",
         "Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs",
         "StandingRecordHostSession / StandingRecordAtlasPanel; compare ExpansionHostSession",
         "StandingRecordSave unified state; ExpansionHubSave has sibling layout/memory/encounter state",
         "standing_record_memory.json", "items",
         "Load, UnlockExpansion, Tick, ApplySiteMutation, GetActiveRecast, CaptureState, RestoreState",
         "StandingRecordHostSession owns a unified StandingRecordEngine and atlas binding, while ExpansionHostSession separately constructs layout, memory, and encounter children and saves them in ExpansionHubSave. The existing tracer focuses faction wiring; an architecture closeout must settle which instance receives visit and mutation facts.",
         "one canonical standing-record instance receives a real visit/mutation and drives one atlas projection and one durable envelope",
         "StandingRecordEngine should be the unified owner only after migration and caller audit; faction standing, quests, and expeditions remain external fact producers",
         "Compare both envelopes and choose a documented migration/precedence rule before a consolidation change.",
         "Do not wire the atlas to one instance while the campaign mutates or saves another.",
         "C6 and C13", "an annotated standing atlas",
         "expansion unlock|first arrival|room inspection|memory layer|encounter start|encounter resolution|site mutation|faction overlay|quest follow-up|plate scrape|overlay return|revisit recast|day advancement|atlas reopen|two envelopes|legacy migration"),
    spec("docs/world/FORECAST_ACCURACY_MATRIX.md",
         "Weather Station System", "WeatherStationSystem",
         "Assets/Ashfall.Core/WeatherStationSystem.cs",
         "WorldHostSession.WeatherIntelligence / world panel",
         "WorldWeather envelope via WeatherIntelligenceSaveState.station",
         "weather_route_gates.json", "gates",
         "Install, Calibrate, Repair, GenerateForecast, GetForecast, IsRouteSafe, GetConfidence, CaptureState, RestoreState",
         "WeatherIntelligenceCoordinator constructs WeatherStationSystem, ticks forecasts, and captures station state; WorldHostSession restores the coordinator and exposes station status and demo install/calibrate routes. This is an indirectly hosted system, not a zero-host orphan. Audit real upgrade costs, campaign weather time, and route-gate consumers.",
         "one real installed station forecasts a current route, conveys calibrated confidence, and restores without rerolling the forecast",
         "WeatherSystem owns actual weather; WeatherStationSystem owns forecast and station condition; route planners consume advisory safety without rewriting actual weather",
         "Keep station within WeatherIntelligenceSaveState and the current world envelope.",
         "Never present a forecast as certain weather or install/calibrate for free merely to activate a panel.",
         "C12 and C17", "a hand-marked forecast strip",
         "station installation|first forecast|calibration|sensor wear|repair|sensor fault|cleared fault|route forecast|false confidence|storm approach|safe window|unsafe window|old forecast|new day|world panel|restored forecast"),
]


def section(subject):
    authored = rows(subject)
    draft = intro(subject, authored).replace(
        "gen_requested_ten_integration_expansions_21_30.py",
        "gen_requested_fifteen_authority_plans.py", 1)
    number = 0
    while len(draft) + len(close(subject, number)) < 251000:
        number += 1
        if number > len(subject["scenes"]) * len(CONDITIONS):
            raise ValueError("case register exhausted for " + subject["name"])
        scene = subject["scenes"][(number - 1) % len(subject["scenes"])]
        condition = CONDITIONS[((number - 1) // len(subject["scenes"])) % len(CONDITIONS)]
        draft += card(subject, scene, condition, authored, number)
    return draft + close(subject, number)


def main():
    by_path = {}
    for subject in SPECS:
        by_path.setdefault(subject["path"], []).append(subject)
    for relative_path, subjects in by_path.items():
        path = ROOT / relative_path
        original = path.read_text()
        boundary = "\n---\n\n" + MARKER_PREFIX
        if boundary in original:
            original = original.split(boundary, 1)[0]
        completed = original.rstrip() + "\n" + "".join(section(s) for s in subjects).rstrip() + "\n"
        path.write_text(completed)
        print(f"{relative_path}\t{len(completed)} characters\t{len(subjects)} sections")


if __name__ == "__main__":
    main()
