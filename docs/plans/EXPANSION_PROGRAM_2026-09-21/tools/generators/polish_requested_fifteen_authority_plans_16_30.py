#!/usr/bin/env python3
"""Distinct source-specific editorial pass for requested items 16–30."""

from __future__ import annotations

from gen_requested_fifteen_authority_plans_16_30 import ROOT, SPECS, MARKER_PREFIX

START = "## Editorial source review and first-package handoff"
END = "## 4. Dependency-ordered implementation package"

REVIEWS = [
    """### Do not let the transfer flag outrun the inventory receipt

The older save-compatibility table is correct that SafeCrackingState is nested in the maritime envelope; current `MaritimeHostSession` restores `SafeCrack` and `SafeCrackModal` offers the action. Yet `TransferSafeLoot` asks Core for loot and formats a text list. The inspected host method and modal callback do not add the returned items to inventory. Because Core's transfer call advances its looted state, a player can see a successful narrative transfer without receiving canonical stock. The first package should define a two-owner commit: validate safe eligibility and inventory capacity, resolve the deterministic loot once, add every accepted quantity through inventory, then persist a receipt that prevents a second claim. If existing APIs cannot make the transition atomic, the integrator should extend the present maritime/inventory seam before exposing the button as operational. Do not call `TransferLoot` as a preview.

The host currently recreates `CoreSeededRng` from safe count and `StableHash.Of(safeId)` for attempts and loot. Preserve replay, but determine whether repeated attempts should use an attempt-specific campaign stream or a persisted RNG state; a stable same roll for every try can unintentionally change risk. The accessible `AttemptAccessible` path must receive equivalent tool, skill, noise, alarm, and save handling. Dive-site safe definitions are already nested in `dive_sites.json`; do not create a second mutable safe catalog in the modal. Candidate safe prose should reflect the actual material, sound, and recovered contents only after the corresponding state transition.

**First acceptance:** open one real safe, claim one inventory batch, repeat claim no-op, restore with same flags and item count, and verify accessible attempt does not bypass cost or noise.
""",
    """### Keep the mine inside the foundry and hub envelope

`SilentFoundryHostSession.SaltMine` already exists. The host's daily path calls `SetPower` from the power grid and `TickDaily(day, new CoreSeededRng(day * 31))`; Main captures `SaltMine.CaptureState()` into `ExpansionHubSave.saltMine` and restores it into that same foundry host. The old production migration note should be updated as a current ownership statement, not a proposal for a new save store. A production audit should verify that the same mine instance receives panel actions, day ticks, and save capture. Its default `OpenSaltMine` helper registers a hard-coded vein and worker count, so a real mine route needs location/vein eligibility, a duty-roster worker assignment, and inventory-backed drill/pump repair costs. If the day-only seed is intentional, document why world seed does not affect mine incidents; otherwise use a campaign stream without changing existing saves silently.

Salt, brine, and sulfur in `SaltMineState` are process storage. A treaty delivery is a separate obligation, and an inventory export is a separate transaction. The host's `DeliverToTreaty` result must not be interpreted as stock transferred unless the canonical stock and treaty owner confirm it. The `BrineExtractionPanel` can show reserves and refusal reasons but cannot generate a vein, a shift, or a delivery. Inscriptions and shift logs from the master document's industrial cluster should be assigned to actual unlocked veins and recorded days; decorative prose cannot imply an accident the engine did not emit.

**First acceptance:** a real vein and roster shift, one powered day, one saved batch, a refused and accepted delivery, and restore with identical reserve and receipt values.
""",
    """### Replace default-condition assumptions with real greenhouse facts

`GreenhouseHostSession` already owns Apiculture, persists it as `GreenhouseState.apiculture`, and exposes it through `GreenhousePanel`. A fresh save installs `hive_01`; older saves restore the nested field if present. `HarvestHoney` adds food-ration and crafting-part proxies through inventory, while `Harvest` in Core decides yield. Keep the distinction between honey/wax measures and item counts in the UI. `TickDay` currently passes fixed 22°C and radiation 2 plus a caller-provided contamination value. Those numbers should be replaced by the existing greenhouse room climate and radiation owners only after their actual current API and day order are traced. A new independent hive climate meter would duplicate those owners.

`FeedHive` removes one `clean_water` before calling `RefillFeed` and `RefillWater`, and it does not check the two Core return values. If the hive ID is absent or feed is already full, water can disappear without an accepted action. The first packet should preflight hive eligibility, then settle water and both refill effects once, with a concrete refusal. Likewise installation should consume any approved hive materials through inventory before Core marks a new hive installed. A pollination bonus must be applied at the existing crop-harvest transition, not a second time when the greenhouse panel refreshes. Candidate hive logs may describe red-light inspection, feeder care, and colony loss only when those facts exist in the saved hive.

**First acceptance:** invalid hive costs zero water, valid feeding changes both owners, a real temperature/radiation day changes hive state, and one crop harvest receives one pollination bonus after reload.
""",
    """### Separate a seasonal event from a resource transaction

`WorldHostSession` binds `seasonal_events.json` into `WeatherIntelligenceCoordinator.Seasonal`; `Main.CampaignOwners` calls the coordinator day tick, and the world envelope saves the seasonal state. Core permits at most one new seasonal event per day, tracks active events and cooldowns, and `Mitigate(eventId)` only marks an event mitigated. It does not spend `mitigationItemId` or `mitigationCost`. The first player command must preflight the current event and canonical stock, settle the item cost through inventory, then call `Mitigate` and persist the response once. If there is no atomic owner seam, the packet stops there. Do not describe a button as a paid mitigation before this receipt exists.

The old resource-swing table describes weather pressure across seasons. It does not itself change water, food, or greenhouse yield. For each proposed effect, name the current event ID, producer, resource consumer, exact day phase, and whether mitigation changes the effect. Use the calendar/weather owner for season ID and seeded campaign RNG; never choose a season from panel text. On restore, active events, expiry, cooldown, and resolved IDs must agree. A notice can warn of likely shortage while the inventory remains unchanged; it cannot claim a loss absent a canonical delta.

**First acceptance:** one authored event triggers once on the correct season/day, a refused mitigation spends nothing, an accepted mitigation spends the catalog cost once, and restore does not reroll or re-charge.
""",
    """### Never consume awakening without a valid skill actor

`LatentExpertAwakeningSystem` has twelve hard-coded definitions, a progress record, `CaptureState`, and `RestoreState`; production src contains no direct constructor or `RecordProgress` caller. `RecordProgress` marks the record awakened at threshold and calls `_skillSystem.TryGrantSkill(actor, skillId, day)` only when both the skill system and actor are present. It does not check that grant's return value before emitting `OnTraitAwakened`. A host that supplies a null actor can permanently mark a survivor awakened without giving the skill. The first package needs a canonical survivor/SkillActor resolver and a successful skill-grant receipt before the awakened record commits. If Core cannot express that safely, revise the existing Core method rather than compensating with an extra skill grant in a host callback.

The skill migration guide describes `skills.json`, but the awakening definitions are not loaded from that catalog; they are registered in Core. Validate every default `traitId`/`skillId` against current trait and skill authorities before claiming content reachability. Progress should be fed only from an accepted surgery, repair, expedition, crafting, or other canonical work fact, with an event identity guard so reload and panel rebind cannot count the same fact again. A description of surprise competence belongs after the grant and should name the actual work, not an invisible statistical threshold.

**First acceptance:** one real accepted work fact advances one survivor, a missing actor cannot create a phantom awakening, a threshold grants the skill once, and old/new save restore preserve progress and grant parity.
""",
    """### Recording needs an inventory transaction and stable tape identity

`RadioHostSession` constructs `RadioRecordingSystem` and saves `recordedCassettes`, but the inspected production source has no `RecordBroadcast` or `ReplayCassette` command call. Core records metadata only: it does not consume `item_blank_magnetic_tape` or grant a cassette item. The historical recording contract promises both, so this is a missing transaction, not a missing save. The first command must revalidate `LastScheduledBroadcast` as an eligible non-silent live delivery, preflight blank tape and output capacity, commit the tape exchange, record metadata once, and expose the cassette by ID. If the inventory API cannot reserve or roll back the exchange, the integration packet must define a bounded transaction adapter on the existing inventory owner. Replay reads the saved transcript and audio cue; it never invokes schedule resolution or broadcasts a world event.

Core's cassette ID includes interpolated `FrequencyMhz` and a count. Specify invariant-culture formatting and collision behavior before generating durable IDs across locales or restored histories. `CalculateTradeValue` currently returns 1 for routine chatter, while the dated contract says routine transmissions have zero value; resolve this explicit policy conflict before binding barter. Neither a title keyword nor a narrative claim should be the sole proof of strategic intelligence when trade value affects currency. Cipher re-analysis may consume a recorded heard fact, but the cipher engine must get a receipt rather than a replay side effect.

**First acceptance:** blank tape decremented once, one physical item and one saved entry created, replay read-only, duplicate record refused, and locale/restore yield stable ID and trade classification.
""",
    """### The cipher chain is currently a three-row Core island

`CipherQuestChainEngine.Chains` contains three static definitions: relay_count, winter_ledger, and last_rotation. Production src constructs it only in `HostCli.WorldExploration`. Its methods can record heard broadcast and acquired key, evaluate decode, reveal a map location, and save state, but no live radio/quest/map host routes were found in the inspected source. The first package should use one existing chain, check that its broadcast, station, item, quest, flag, and location IDs resolve in the current catalogs, then wire one-way events: radio delivered → heard; canonical inventory acquisition → key found; explicit decode command → map reveal and quest consequence. Map fog and quest flags stay with their present owners. A mere receiver tune or cassette replay is not automatically a first hearing unless the approved radio record contract says so.

`RestoreState` may update the map if supplied, so restore order must be written down and tested against a map that already knows the location. Replaying a saved decode must not publish another discovery or quest reward. If a chain's authored location is absent, the plan should stop that chain and record a catalog-integrity finding rather than invent a map node. Cipher prose can be enigmatic, but the player needs a legible evidence trail: what was heard, which physical key was found, and why the map marks a plausible lead rather than perfect omniscience.

**First acceptance:** one valid chain advances through real radio and item facts, decodes once, reveals one existing location, and restores without duplicate map/journal effects.
""",
    """### World evolution can only mutate existing world owners

`WorldEvolutionEngine` currently runs in the World Exploration CLI probe, not the production world host. It loads `world_evolution_events.json` and falls back to defaults when the file is absent or its deserialization throws; the broad catch can hide broken authored rows. The first package should decide that production data errors are reported through the current integrity pipeline while retaining any explicitly approved legacy default behavior. `TickDay` receives active world flags plus `LocationEvolutionSystem`, `LandmarkDegradationSystem`, and `WastelandMapSystem`: all must be the same instances `WorldHostSession` uses, not new test instances. The engine's triggered-ID set is the once-only guard and belongs in the existing world envelope after a migration decision.

The plan should map each current event row to a real flag or day threshold, a concrete world delta, and a visible map/expedition effect. If an event attempts a site mutation, let the existing location owner validate it; the evolution engine records only that the event fired. A restored campaign must not fire a past event because its flag happens to be present again. Candidate revisits should express uncertainty from known changes, not retcon the landscape each time the map panel opens. The first package is one current row that changes one existing location owner and can be inspected on the map.

**First acceptance:** malformed catalog row produces a visible integrity finding, valid event fires once with stable day and IDs, and mid-world restore retains both the triggered receipt and owner state.
""",
    """### Schedule resolution is live, while alert lifetime needs a contract

`RadioHostSession` constructs `RadioScheduleCoordinator`, resolves on tuning, and stores played broadcast keys in RadioSaveState. `Main.RadioProgramProduction` also asks it to resolve a scheduled delivery. Treaty and trapping producers inject dynamic alerts; the coordinator defines weather, orbital, disease, route, and foundry injection slots as well. This is not a zero-host orphan. The current content-utilization table should distinguish a loaded station/slot from a delivered transmission and should test priority only at a real tuned frequency. `Resolve` checks silent/jammed station state before dynamic alerts; a planned alert cannot bypass a dead carrier unless the current contract explicitly changes.

Dynamic alerts are fields on the coordinator and `ClearDynamicAlerts` erases them; they are not separately saved. Establish whether upstream owners re-inject still-active alerts after restore, and when the host clears them at day or session boundaries. Otherwise an alert can vanish after load or linger indefinitely. A delivery receipt belongs with radio playback keys, not in a second schedule ledger. `ScheduledBroadcastResult` is also the input for radio recording and program production; those consumers must receive the same accepted result rather than independently resolving another random slot. Candidate rundowns should name the station's public knowledge and recorded time; a station must not describe an unseen treaty before its producer published the alert.

**First acceptance:** one authored slot and one live alert resolve deterministically, silent/jammed gates hold, retune does not duplicate a one-shot, and restore has a defined alert reconstruction outcome.
""",
    """### Phase 8 sign-off predates a live infestation owner

`Main.EcologicalInfestations` now constructs the system, loads `ecological_infestations.json`, restores a dedicated section, ticks from the canonical campaign day with world-evolution RNG forks, and connects bounded food loss and a disease source. The old sign-off's partial label is therefore stale. The next package should focus on accessible discovery and player response: how the current infestation record becomes visible, which `TryClear` or `TryTolerateAndHarvest` command a panel can invoke, what inventory/worker inputs each requires, and how the relevant owner settles its own effect. The system's `MaxFoodLossPerDay` bound is not a license for a second UI deduction.

Map a single authored infestation to its season/site precondition, trigger roll, active record, daily consequence, clearing attempt, and post-clear cooldown. The disease pipeline remains the sole clinical authority; an infestation may provide exposure evidence but cannot directly manufacture a diagnosis. A tolerated infestation can be a tradeoff only if its harvest reaches canonical inventory exactly once. Master-document ecology prose can distinguish spoor, damaged stores, and uncertain species identification; a fictional inspection note must not state an exact biological cause before the bestiary or disease owner supplies one.

**First acceptance:** one current infestation triggers on a real season/site, one food/disease effect occurs once per day, a valid response has a clear cost and result, and restored state does not replay a lost batch.
""",
    """### Two ice-road systems cannot independently decide travel

`YearOfAshSave` has an optional `iceRoad` field and can capture `YearOfAshIceRoadSystem`, but production src has no direct instance. The active Holdfast campaign already owns `IceRoadSystem` through `CoreDemoSession`, persists it in HoldfastSave, drives the travel gate and duty roster, and exposes the ice-road status. The Year-of-Ash type holds another open/closed state and computes trade/exposure multipliers. Wiring it as a second daily road simulation would violate the one-authority rule and could produce a journey allowed by one system and blocked by the other. The first package is an explicit foreman architecture decision: retire the optional second state, make the Year-of-Ash type a pure projection from Holdfast road/weather facts, or define a one-way adapter with a single writer and migration precedence.

The Wave 12 plan may use storm windows and deep-freeze text as context, but it must not create a second campaign clock or revise the already sealed Holdfast opening rules from prose. A trade multiplier can be a derived advisory or a real economy consumer only after the current caravan/trade owner accepts it. Expedition exposure risk belongs to the existing expedition/medical route and cannot be charged at both dispatch and return. An ice-road gauge can display measured thickness and warning uncertainty from the canonical state; it should not promise safety because a secondary model says the road is open.

**First acceptance:** same day/weather produces one road verdict in all surfaces, optional old Year-of-Ash state migrates or is ignored by a documented rule, and trade/exposure consumers receive at most one approved multiplier.
""",
    """### Indirect TacticalCombat integration supersedes the old map's gap claim

The 2026-09-06 B86 authority map says `TacticalCombatSystem.Obstacles.cs` is missing. Current Core has `TacticalCombatSystem.Breaching.cs`: it calls `CombatBreachingEngine` through `EnsureBreachingEngine`, loads a catalog, configures logistics, and exposes `EvaluateBreach`, `BeginBreach`, `AdvanceBreach`, and `AbandonBreach`. The new plan should correct the old premise. The remaining question is whether the production combat host creates barriers from encounter content, binds the current `breaching_equipment_catalog.json`, supplies canonical inventory/vehicle/skill and seeded RNG, and offers those tactical commands to the player. A `VaultDoorBreachingPanel` exists but is a different surface; it cannot be used as proof of tactical obstacle reachability.

The engine's noise and wear are returned to TacticalCombatSystem ports; those ports must connect to the existing stealth/threat and equipment-condition owners. Combat state owns partial breach progress. A cleared tactical barrier can modify path blocking inside that encounter; a durable world route change requires a separate explicit fact to the current location or route owner. Do not let an encounter-local breach silently clear railway track or minefield state. Candidate work orders should name obstruction class, tool affordance, hazard, and refusal in player terms without operational real-world explosive detail.

**First acceptance:** current encounter barrier, catalog-valid tool, one preflight refusal, one progressing breach, one noise/wear effect, and save/reload of partial progress through tactical combat state.
""",
    """### Continuity analysis is a tooling gate, not a campaign system

`NarrativeContinuityEngine.Analyze()` reads authored JSON and returns a report; `NarrativeContinuitySelfTest` is invoked by HostCli and writes JSON/Markdown artifacts. The Wave 13 README's type-level census counted it as an authority candidate, but no day tick, save section, or player panel is appropriate. Finish this integration as a content-acceptance lane: choose a bounded set of current narrative catalogs, run analysis in the existing tooling command, compare findings with `NarrativeContinuityAllowlist`, and fail promotion only on actionable unallowlisted breaks with file, ID, and reference evidence. A deterministic report sort and stable exit code matter more than a Godot UI.

The scan must not mutate source JSON or hide all warnings by widening the allowlist. A false positive should be documented with its actual system-provided flag or intentional cross-file reference and a narrow exemption. A truly missing quest target should point to the exact authoring file and consumer; content promotion can then repair the row or deliberately retire it. Generated report artifacts belong in an approved temporary or CI output path, not inside `Assets/StreamingAssets/Data/`. The master document's narrative continuity and anti-padding rules apply: adding more text should increase checked relationships, not suppress the checker.

**First acceptance:** same corpus yields byte-stable classified report, one deliberate broken reference fails the bounded gate, one justified allowlist entry remains narrow, and the scanner leaves all source files untouched.
""",
    """### UV observes supplied faults and spends battery once

`UvCoronaDetectionEngine` has catalog, inventory port, equipment state, observations, `Scan`, daily calibration drift, and capture/restore. The closeout covers Core and a CLI probe, while production src constructs the engine only in HostCli.AdvancedIndustrialRecon. A real host must bind the current `uv_corona_detector_catalog.json`, a canonical detector item and battery inventory, a campaign-seeded RNG, and fault inputs from the existing power/industry owner. `Scan` consumes battery through `IPlayerInventoryPort.TryConsume` before returning observations; do not add another host debit. Validate the target and environment before the call so an invalid scan never loses a battery. A failed scan remains an explicit failure code in the read model, not a fabricated clean bill of health.

`CoronaObservation` is evidence about an electrical fault supplied by another owner. It must not repair, cause, or settle that fault. Repeated observation and calibration drift should remain bounded and deterministic after restore; the save captures engine state and RNG state. A field report may describe visible glow and uncertain confidence, then refer the player to a power-grid repair route. The first package can be a single existing substation/power fault and one detector profile, rather than an entire new industrial-recon network.

**First acceptance:** equipped real detector, one battery consumed by an eligible scan, one saved observation, same-seed/reload parity, and no power-grid mutation from inspection alone.
""",
    """### GPR leads are evidence, not discovered loot

`GroundPenetratingRadarEngine` has a catalog, inventory port, equipment state, active survey, observations, idempotent leads, and capture/restore. Current production src constructs it only in HostCli.AdvancedIndustrialRecon. `BeginSurvey` validates catalog IDs and consumes mode power from inventory, while `Tick` advances the current survey and records an observation; `TryCreateLead` requires confidence at least 0.55 and refuses duplicate leads for a target. The host should bind actual terrain and target IDs from the map/expedition owner and use campaign-seeded RNG. A panel refresh must not call `Tick` or create a lead. Save the active survey and RNG state before claiming mid-survey persistence.

The GPR state can say that an anomaly may be present. The map remains sole authority for fog, location discovery, and travel topology. A lead may be offered to an expedition or cartography surface only through an accepted map annotation with an ID receipt; it cannot put an item into inventory or promise a bunker before excavation. A repeated pass may improve confidence while preserving earlier observations. If the catalog's mode power cost cannot be represented by the current inventory item, reject the route explicitly and fix the catalog/owner mapping before UI release.

**First acceptance:** a real target begins with one power debit, advances over the authored scan time, records one observation, produces one eligible lead at most, and restores mid-survey without duplicate map disclosure.
""",
]


def polish_section(section: str, review: str) -> str:
    if START in section:
        before, remainder = section.split(START, 1)
        _, after = remainder.split(END, 1)
        section = before + END + after
    for before, after in (("The a ", "The "), ("The an ", "The "),
                          ("a a ", "a "), ("a an ", "an "),
                          ("the the ", "the ")):
        section = section.replace(before, after)
    if section.count(END) != 1:
        raise ValueError("ambiguous dependency marker")
    return section.replace(END, START + "\n\n" + review.strip() + "\n\n" + END, 1)


def main() -> None:
    by_path = {}
    for subject, review in zip(SPECS, REVIEWS, strict=True):
        by_path.setdefault(subject["path"], []).append((subject, review))
    for relative_path, entries in by_path.items():
        path = ROOT / relative_path
        content = path.read_text()
        for subject, review in entries:
            marker = MARKER_PREFIX + subject["name"]
            start = content.index(marker)
            next_start = content.find("\n---\n\n" + MARKER_PREFIX, start + len(marker))
            end = next_start if next_start >= 0 else len(content)
            content = content[:start] + polish_section(content[start:end], review) + content[end:]
        path.write_text(content)
        print(f"{relative_path}\t{len(content)} characters\t{len(entries)} polished sections")


if __name__ == "__main__":
    main()
