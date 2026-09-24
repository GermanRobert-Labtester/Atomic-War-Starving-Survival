#!/usr/bin/env python3
"""Rebuild requested 2026-09-24 authority addenda for items 16–30.

The dated source text remains above the addendum. The 250k-character review
registers are candidate, non-canon planning cells, not verified game content.
Run the distinct editorial polishing pass after this generator.
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
    spec("docs/maritime/PLAN23_SAVE_COMPATIBILITY.md",
         "Safe Cracking System", "SafeCrackingSystem",
         "Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs",
         "MaritimeHostSession / SafeCrackModal / Main.Maritime",
         "MaritimeHostSave.SafeCrack in maritime section",
         "dive_sites.json", "dive_sites",
         "RegisterSafe, InspectSafe, Attempt, AttemptAccessible, TransferLoot, Abandon, CaptureState, RestoreState",
         "MaritimeHostSession constructs, restores, and saves SafeCrackingSystem, and SafeCrackModal calls the host. The current TransferSafeLoot host method calls Core, builds a text list, and returns it; no canonical inventory add is visible in that method or the modal callback. Its local attempt/loot RNG is re-created from safe count and stable safe ID, so the campaign-seed and attempt-index contract needs inspection.",
         "one authored dive-site safe can be opened, its loot committed to canonical inventory once, and its opened/looted flags restored without a second award",
         "SafeCrackingSystem owns safe puzzle state and loot-transfer eligibility; inventory owns items; dive site owns safe registration and current location",
         "Keep SafeCrackingState in MaritimeHostSave; a transfer receipt must reconcile item delivery and the Core looted flag.",
         "Do not mark loot transferred on a text-only response or create a parallel safe inventory; preserve accessible attempt parity.",
         "C5 and C17", "a salt-stiff safe log",
         "first safe registration|dive-room arrival|combination attempt|near miss|tool wear|accessible attempt|rising noise|alarm triggered|safe jammed|safe opened|loot preview|inventory full|loot transfer|repeat transfer|abandoned safe|old maritime save"),
    spec("docs/production/PRODUCTION_SAVE_MIGRATION.md",
         "Salt Mine Extraction System", "SaltMineExtractionSystem",
         "Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs",
         "SilentFoundryHostSession / BrineExtractionPanel / Main.Economy",
         "ExpansionHubSave.saltMine merged from foundry host",
         "none by exact mine type; narrative/salt_mine_inscriptions.json is prose", "",
         "RegisterVein, UnlockVein, AssignWorkers, TickDaily, DeliverToTreaty, ReplaceDrill, RepairPump, SetPower, CaptureState, RestoreState",
         "SilentFoundryHostSession already owns SaltMine, ticks it, and exposes operations; Main merges its state into ExpansionHubSave and restores it into the foundry host. The host currently seeds its daily RNG from day times 31, opens a default hard-coded vein, and has a brine panel. The next package should verify campaign seed, real vein/workforce/power inputs, and stock/treaty settlement.",
         "one existing vein accepts real workers and power, yields a settled material batch on the campaign day, and keeps the same reserves and treaty record after reload",
         "SaltMineExtractionSystem owns vein reserves and extraction state; duty roster owns workers, power grid owns service, inventory/economy owns delivered materials, treaty owner accepts obligations",
         "Keep SaltMineState in ExpansionHubSave and the foundry host; no duplicate mine or treaty section.",
         "Do not present a textual brine quota as a settled transfer unless canonical stock and treaty receipt both change.",
         "C4 and C11", "a mine shift ledger",
         "vein survey|first unlock|worker assignment|power outage|pump failure|drill replacement|brine batch|salt batch|sulfur trace|exposure warning|treaty quota|delivery accepted|delivery refused|night shift|old save|depleted seam"),
    spec("docs/production/PRODUCTION_CONTENT_UTILIZATION.md",
         "Apiculture System", "ApicultureSystem",
         "Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs",
         "GreenhouseHostSession / GreenhousePanel / Main.World",
         "GreenhouseState.apiculture via GreenhouseSaveStore",
         "greenhouse_items.json", "items",
         "InstallHive, LinkPlots, TickDaily, GetPollinationBonus, RefillFeed, RefillWater, ReplaceQueen, Harvest, CaptureState, RestoreState",
         "GreenhouseHostSession already creates ApicultureSystem, restores it, installs a default hive on fresh saves, ticks it, adds honey/wax proxies to inventory, and GreenhousePanel reads the hive. FeedHive removes clean water before checking whether RefillFeed/RefillWater succeed; TickDay passes fixed temperature 22C and radiation 2 rather than current world conditions. Audit these inputs and transaction boundaries before new hive content.",
         "one current hive consumes valid supplies, responds to real greenhouse conditions, modifies an existing plot's yield, and restores its state and harvest stock consistently",
         "ApicultureSystem owns hive condition and pollination; greenhouse owns crop growth; inventory owns feed, water, honey proxy, and wax proxy quantities",
         "Retain the apiculture field nested in GreenhouseState and its old-save default hive policy.",
         "Do not add a second crop-yield bonus or remove water before hive eligibility is validated.",
         "C3 and C14", "a hive inspection card",
         "fresh hive|queen condition|colony population|feed shortage|water shortage|cold greenhouse|radiation spike|swarm warning|colony death|linked plot|pollination gain|honey ready|wax output|harvest receipt|empty hive|old greenhouse save"),
    spec("docs/world/SEASONAL_RESOURCE_SWINGS.md",
         "Seasonal Event System", "SeasonalEventSystem",
         "Assets/Ashfall.Core/World/SeasonalEventSystem.cs",
         "WeatherIntelligenceCoordinator / WorldHostSession / campaign day owner",
         "WorldWeather envelope via WeatherIntelligenceSaveState.seasonal",
         "seasonal_events.json", "events",
         "BindDefinitions, TickDay, Mitigate, CaptureState, RestoreState",
         "WeatherIntelligenceCoordinator constructs SeasonalEventSystem, ticks it during its own TickDay, captures it, and WorldHostSession binds seasonal_events.json. Main.CampaignOwners invokes the coordinator day tick. The older resource-swing table is a design projection; prove each event's actual mitigation command and each resource consumer before claiming economy effects.",
         "one current seasonal event triggers once on a canonical day, shows its active state, applies one approved resource consequence, and restores without replay",
         "SeasonalEventSystem owns event lifecycle and cooldown; weather/calendar owns season, economy/greenhouse/water owners apply their own consequences",
         "Keep seasonal state in WeatherIntelligenceSaveState within the world envelope.",
         "Do not introduce a second season calendar or subtract resources from a panel based on a prose availability curve.",
         "C3 and C12", "a seasonal field notice",
         "season opening|ash fall|deep freeze|first thaw|black bloom|event trigger|mitigation preview|mitigation accepted|mitigation refused|resource shortage|greenhouse response|water warning|repeat day|cooldown|event resolution|old world save"),
    spec("docs/progression/SKILL_CATALOG_MIGRATION.md",
         "Latent Expert Awakening System", "LatentExpertAwakeningSystem",
         "Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs",
         "candidate survivor progression host / current narrative questline fact producers",
         "LatentAwakeningSaveState needs an approved survivor/progression owner",
         "skills.json", "skills",
         "RegisterDefinition, RecordProgress, TryAwaken, IsAwakened, GetProgress, CaptureState, RestoreState",
         "The Core type has twelve hard-coded awakening definitions and save/restore, but production src references name it only in a comment on NarrativeQuestlineHostSession. The skill catalog's scanner claim is not a loader or live awakening. First resolve trait/skill ID validity and the canonical skill/trait owner before binding event facts.",
         "one real completed work fact advances one validated latent trait and grants the corresponding skill/trait through existing progression ownership exactly once",
         "LatentExpertAwakeningSystem owns threshold progress and awakened IDs; skill progression owns skill levels and survivor trait owner owns trait application",
         "Choose an existing survivor/progression save envelope; no independent duplicate skill record.",
         "Do not count a quest narrative line as completed work or grant a skill merely because progress crossed a threshold without a canonical owner receipt.",
         "C9 and C16", "a skill record margin",
         "first qualifying work|repeated work|wrong survivor|wrong skill|threshold crossed|latent trait revealed|skill grant|trait grant|already awakened|save midway|old survivor|death before grant|quest callback|work cancellation|catalog mismatch|epilogue credential"),
    spec("docs/radio/RADIO_RECORDING_CONTRACT.md",
         "Radio Recording System", "RadioRecordingSystem",
         "Assets/Ashfall.Core/Radio/RadioRecordingSystem.cs",
         "RadioHostSession / existing radio panel and inventory host",
         "RadioSaveState.recordedCassettes via RadioHostSession",
         "radio.json", "radio_broadcasts",
         "RecordBroadcast, ReplayCassette, CalculateTradeValue, CaptureState, RestoreState",
         "RadioHostSession constructs and saves RecordingSystem, but the current src search finds no production call to RecordBroadcast or ReplayCassette. Core records metadata from ScheduledBroadcastResult and never consumes blank tape or grants a physical cassette. The dated contract's inventory transaction is therefore unproven. Tape ID formatting also interpolates a float frequency and should be reviewed for culture-invariant deterministic identity.",
         "one currently tuned eligible broadcast consumes one blank tape, records one cassette entry, grants one inventory item, and replays without retriggering broadcast consequences",
         "RadioRecordingSystem owns recorded metadata; RadioHostSession owns tuned live delivery; inventory owns blank and recorded physical items; radio scheduling owns broadcast facts",
         "Keep recorded entries inside RadioSaveState, with a transaction receipt for inventory and restored playback.",
         "Do not grant a recorded item from a failed broadcast, call RecordBroadcast on panel refresh, or replay one-shot world effects.",
         "C8 and C11", "a labeled cassette sleeve",
         "live signal|blank tape|weak reception|dead air|record command|tape exhausted|inventory full|cassette label|replay request|trade appraisal|lost cassette|duplicate click|old radio save|signal interruption|unknown broadcast|archived voice"),
    spec("docs/world/PLAN_11_WORLD_EXPLORATION_QA_MATRIX.md",
         "Cipher Quest Chain Engine", "CipherQuestChainEngine",
         "Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs",
         "current WorldExploration HostCli only; candidate radio/map/quest adapter",
         "CipherQuestState list needs approved quest/map save owner",
         "none by exact engine; three Chains are currently static in Core", "",
         "RecordBroadcastHeard, RecordKeyAcquired, EvaluateDecode, MarkResolved, CaptureState, RestoreState",
         "The current engine defines three static cipher chains and mutable stages, but production src constructs it only in HostCli.WorldExploration. A real player route needs a heard-broadcast fact, inventory key acquisition, decode command, map reveal, quest state, and save custody without duplicating radio or map flags.",
         "one current chain advances from an actually heard broadcast and acquired key to one decode and one map reveal, then restores without repeating the reveal",
         "CipherQuestChainEngine owns chain stage; radio owns heard delivery, inventory owns key, quest/flag ledger owns quest status, map owns discovered location",
         "Select the existing quest/map envelope and receipt before persisting the chain; static definitions remain until content migration is separately approved.",
         "Do not reveal a location on a mere frequency scan or create a second explored-map flag.",
         "C6, C8, and C10", "a cipher worksheet",
         "first interception|partial signal|key acquired|wrong key|decode attempt|decoded phrase|map coordinate|location revealed|quest activation|repeat broadcast|lost key|late discovery|old save|unknown chain|resolved hunt|journal annotation"),
    spec("docs/world/PLAN_11_WORLD_EXPLORATION_QA_MATRIX.md",
         "World Evolution Engine", "WorldEvolutionEngine",
         "Assets/Ashfall.Core/World/WorldEvolutionEngine.cs",
         "current WorldExploration HostCli only; candidate WorldHostSession day adapter",
         "WorldEvolutionState needs approved world save custody",
         "world_evolution_events.json", "events",
         "TickDay, ApplyEvent, CaptureState, RestoreState",
         "WorldEvolutionEngine loads world_evolution_events.json or defaults on missing/invalid data, but production src constructs it only in HostCli.WorldExploration. WorldHostSession already owns LocationEvolutionSystem, LandmarkDegradationSystem, and WastelandMapSystem, so a new day adapter must feed those same owners and persist triggered IDs. The loader's catch-all fallback can hide malformed authored data and needs an integrity decision.",
         "one current authored evolution event triggers on the campaign day, changes one existing world owner, and restores with its triggered ID intact",
         "WorldEvolutionEngine owns event eligibility/triggered set; world location, landmark, map, and flag owners apply resulting changes",
         "Embed WorldEvolutionState in the existing world envelope after a migration decision; do not create a second map or landmark save.",
         "Do not silently turn malformed JSON into default events in production or apply the same site mutation twice after reload.",
         "C5, C6, and C12", "a revisited location ledger",
         "day threshold|world flag|landmark erosion|location change|map reveal|road closure|old district|new settlement|first trigger|repeat day|late save|missing catalog|bad row|conflicting event|player revisit|epilogue trace"),
    spec("docs/radio/RADIO_CONTENT_UTILIZATION.md",
         "Radio Schedule Coordinator", "RadioScheduleCoordinator",
         "Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs",
         "RadioHostSession / Main.RadioProgramProduction and alert producers",
         "stateless coordinator; RadioSaveState stores playback and tuned receiver state",
         "radio.json", "radio_broadcasts",
         "Resolve, InjectWeatherAlert, InjectOrbitalAlert, InjectDiseaseAlert, InjectRouteAlert, InjectFoundryAlert, InjectTreatyAlert, InjectTrappingAlert, ClearDynamicAlerts",
         "RadioHostSession constructs ScheduleCoordinator and calls Resolve on tuning; Main.RadioProgramProduction calls Resolve for delivered programs, while treaty and trapping alert producers inject current messages. This is already a live scheduling seam. The remaining audit is priority, clearing, delivery receipt, and whether every authored slot can be reached without duplicate one-shot playback.",
         "one current station slot and one injected alert resolve through the live tuned receiver with deterministic priority and one playback receipt",
         "RadioScheduleCoordinator owns schedule resolution only; RadioHostSession owns tuned day/frequency and played keys; source systems own alert facts",
         "No new coordinator save: persist any accepted delivery through current RadioSaveState played keys and upstream alert owners.",
         "Do not let an alert injection create a new weather, treaty, disease, or trapping fact, and do not replay one-shot messages on retune.",
         "C8 and C17", "a station rundown sheet",
         "scheduled slot|tuned frequency|dead air|weather alert|treaty alert|trapping alert|disease alert|foundry alert|route alert|priority tie|same-day retune|missed slot|archived transmission|alert cleared|program delivery|old radio save"),
    spec("docs/ecology/PLAN28_PHASE8_SIGN_OFF.md",
         "Ecological Infestation System", "EcologicalInfestationSystem",
         "Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs",
         "Main.EcologicalInfestations / existing world and disease owners",
         "ecological_infestation section via EcologicalInfestationSaveStore",
         "ecological_infestations.json", "infestations",
         "LoadDefinitions, IsEligibleToTrigger, TryTrigger, TryClear, TryTolerateAndHarvest, TickDay, CaptureState, RestoreState",
         "Main.EcologicalInfestations already constructs, loads, restores, ticks with campaign RNG forks, applies bounded food loss and disease input, and saves the system. The Phase 8 sign-off is historical. The remaining package is player-facing prevention/clear/tolerate actions and ensuring all ecological cross-owner consequences have one receipt.",
         "one current infestation becomes visible from a real trigger, accepts one valid response, changes only its canonical resource/disease consumers, and restores without repeat loss",
         "EcologicalInfestationSystem owns infestation record; food inventory, greenhouse, wildlife, and disease owners apply their respective effects",
         "Retain the ecological_infestation save section and its day owner.",
         "Do not create a second pest population simulation or apply daily food loss again when a panel opens.",
         "C3 and C14", "an infestation inspection card",
         "first trace|season gate|food cache|crop bed|wildlife vector|disease warning|clear option|tolerate option|harvest yield|failed clearing|daily spread|resource cap|return visit|restored day|old save|quiet aftermath"),
    spec("docs/plans/EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-YEAR-OF-ASH-TRUTH-146.md",
         "Year of Ash Ice Road System", "YearOfAshIceRoadSystem",
         "Assets/Ashfall.Core/YearOfAsh/YearOfAshIceRoadSystem.cs",
         "no direct production host; compare Holdfast CoreDemoSession.IceRoad",
         "YearOfAshSave.iceRoad optional versus existing HoldfastSave.IceRoad",
         "none by exact system; current weather/season and Holdfast ice-road facts", "",
         "TickDay, GetTradeMultiplier, GetExpeditionExposureRisk, CaptureState, RestoreState",
         "YearOfAshSave optionally captures YearOfAshIceRoadSystem, but production src has no direct construction of it. The active campaign already constructs IceRoadSystem in CoreDemoSession, saves it through HoldfastSave, uses it for travel gating and duty. This is a direct two-ice-road authority risk; decide whether the Year-of-Ash type is a derived overlay, a compatibility type, or superseded before any host attach.",
         "prove one winter road fact can inform trade/exposure from the canonical Holdfast ice-road state without creating a second open/closed answer",
         "Existing IceRoadSystem owns route availability and thickness; weather/calendar owns temperature and storms; YearOfAshIceRoadSystem can only be a derived pressure model if the foreman approves that boundary",
         "Do not activate YearOfAshSave.iceRoad as a second road state until migration/retirement decision is signed.",
         "No second campaign clock, open/closed gate, or contradictory expedition route ruling.",
         "C5 and C12", "an ice-road gauge card",
         "first freeze|thin ice|open road|closed road|storm window|trade availability|expedition exposure|waystation handoff|toll notice|thaw warning|day transition|cold reversal|old Holdfast save|Year-of-Ash save|conflicting road states|epilogue crossing"),
    spec("docs/combat/PLAN_86_AUTHORITY_MAP.md",
         "Combat Breaching Engine", "CombatBreachingEngine",
         "Assets/Ashfall.Core/Combat/CombatBreachingEngine.cs",
         "TacticalCombatSystem.Breaching; inspect production combat host and barrier route",
         "TacticalCombatSystem barrier/breach state, not an engine-private section",
         "breaching_equipment_catalog.json", "tools",
         "LoadCatalog, Evaluate, Begin, Advance, Abandon; TacticalCombatSystem.EvaluateBreach, BeginBreach, AdvanceBreach",
         "CombatBreachingEngine is created indirectly by TacticalCombatSystem.EnsureBreachingEngine and used for Evaluate/Begin/Advance/Abandon through its tactical façade. Direct src references to the engine are zero, but that does not mean breaching logic is absent. The actual unresolved premise is whether the production combat host loads the catalog, supplies inventory/vehicle/skill/RNG ports, and exposes a reachable obstacle command.",
         "one current tactical barrier accepts a catalog-backed breach action, preserves its progress and tool wear, and exposes an observable noise/path result",
         "TacticalCombatSystem owns barrier state and applies engine results; inventory owns tools and wear; sound/threat owners consume emitted noise",
         "Persist through tactical combat's existing barrier/encounter state; no separate breaching save.",
         "Do not call CombatBreachingEngine directly from Godot UI or equate a vault-door panel with a tactical barrier.",
         "C5 and C15", "a breaching work order",
         "barrier survey|tool selection|unsupported material|missing vehicle|skill check|quiet breach|loud breach|tool wear|noise report|partial progress|abandoned breach|cleared path|failed attempt|combat save|old barrier|post-breach route"),
    spec("docs/plans/EXPANSION_PROGRAM_WAVE13_2026-09-21/README.md",
         "Narrative Continuity Engine", "NarrativeContinuityEngine",
         "Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs",
         "NarrativeContinuitySelfTest / HostCli diagnostic path",
         "none: read-only offline content analysis",
         "many authored narrative JSON catalogs; no single runtime catalog", "",
         "Analyze, NarrativeContinuityReport; diagnostic JSON/Markdown output",
         "NarrativeContinuityEngine is an engine-free, read-only analyzer invoked by NarrativeContinuitySelfTest from the host CLI. It is not a gameplay authority and needs no campaign host, day tick, or save section. The useful integration is a bounded content-acceptance/CI gate that consumes its report and preserves its allowlist and diagnostic provenance.",
         "one reproducible continuity report classifies current authored references and fails only on unallowlisted actionable breaks with file and ID evidence",
         "NarrativeContinuityEngine owns analysis verdicts only; authored JSON remains source truth; existing content acceptance pipeline owns promotion decisions",
         "No campaign persistence; store generated reports only as tooling artifacts under approved output paths.",
         "Do not create an in-game continuity ledger, mutate JSON during analysis, or count a scan as player integration.",
         "C10 and C16", "an editor's continuity docket",
         "missing quest target|unknown faction|orphan flag|late consequence|branch conflict|duplicate ID|speaker mismatch|unreachable transcript|stale location|allowlisted exception|false positive|new corpus batch|release candidate|CI rerun|report diff|closed finding"),
    spec("docs/radio/PLAN_119_UV_CORONA_DETECTION_CLOSEOUT.md",
         "UV Corona Detection Engine", "UvCoronaDetectionEngine",
         "Assets/Ashfall.Core/Radio/UvCoronaDetectionEngine.cs",
         "HostCli.AdvancedIndustrialRecon only; candidate power/expedition/inventory host",
         "UvCoronaDetectionState needs approved recon or expedition save custody",
         "uv_corona_detector_catalog.json", "detectors",
         "BindCatalog, BindInventory, Equip, Scan, AdvanceDay, CaptureState, RestoreState",
         "The dated closeout delivered Core, catalog, focused checks, and a CLI probe. Production src constructs UvCoronaDetectionEngine only in HostCli.AdvancedIndustrialRecon. It needs a real detector item, battery inventory, campaign-seeded scan, environmental observation source, and player surface before claiming gameplay reachability.",
         "one equipped canonical detector scans one real target, consumes battery exactly once, records bounded evidence, and restores without a second scan",
         "UV engine owns equipment calibration and observations; inventory owns battery and detector items; expedition/world owner supplies site and environment facts",
         "Choose existing recon/expedition save custody and restore prior observations before UI bind; no parallel inventory.",
         "Do not let high confidence manufacture a power fault or reveal a location without an approved consumer.",
         "C4, C6, and C8", "a UV survey film sheet",
         "detector equip|battery check|first scan|rain interference|dust interference|long range|calibration drift|false glow|verified corona|repeat scan|no target|battery depleted|old observation|save midway|field report|power repair clue"),
    spec("docs/world/PLAN_121_GPR_CARTOGRAPHY_CLOSEOUT.md",
         "Ground Penetrating Radar Engine", "GroundPenetratingRadarEngine",
         "Assets/Ashfall.Core/World/GroundPenetratingRadarEngine.cs",
         "HostCli.AdvancedIndustrialRecon only; candidate cartography/expedition host",
         "GroundPenetratingRadarState needs approved map/recon save custody",
         "gpr_exploration_catalog.json", "equipment",
         "BindCatalog, BindInventory, Equip, BeginSurvey, Tick, TryCreateLead, FindObservation, CaptureState, RestoreState",
         "The dated closeout delivered Core, catalog, focused checks, and a CLI probe. Production src constructs GroundPenetratingRadarEngine only in HostCli.AdvancedIndustrialRecon. A player route needs a real equipment item, terrain/anomaly source, power transaction, day-aware survey progress, map lead handoff, save custody, and a truthful confidence UI.",
         "one current terrain survey consumes canonical power, records one observation, produces one eligible lead, and restores without duplicate map disclosure",
         "GPR engine owns survey/observation/lead candidates; inventory owns power and equipment; map owner accepts discovery and expedition owner supplies location",
         "Choose one current map/recon save envelope, then reconcile generated lead IDs with existing map discovery state.",
         "Do not equate an uncertain buried lead with confirmed loot or create a second map fog authority.",
         "C5 and C6", "a gridded subsurface survey sheet",
         "equipment mount|power check|survey start|slow pass|terrain attenuation|weak return|deep anomaly|repeat scan|lead candidate|lead rejected|lead accepted|map annotation|expedition follow-up|battery depleted|old observation|restored survey"),
]


def section(subject):
    authored = rows(subject)
    draft = intro(subject, authored).replace(
        "gen_requested_ten_integration_expansions_21_30.py",
        "gen_requested_fifteen_authority_plans_16_30.py", 1)
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
