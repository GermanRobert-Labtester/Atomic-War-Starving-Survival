#!/usr/bin/env python3
"""Source-specific editorial pass for the fifteen authority addenda.

Replaces its own source-review block on rerun. It never edits the historical
portion of a target document. Candidate prose remains non-canon.
"""

from __future__ import annotations

from gen_requested_fifteen_authority_plans import ROOT, SPECS, MARKER_PREFIX

START = "## Editorial source review and first-package handoff"
END = "## 4. Dependency-ordered implementation package"

REVIEWS = [
    """### Recovery belongs to the existing maritime state

`MaritimeHostSession.Psychology` is already created and restored with the maritime envelope. The historical matrix must therefore be read as a recovery-design candidate, not a request to create another contamination store. Its row on natural decay maps to `Tick(gameDays, survivorId, currentMorale, currentAssignment)`; the owner of elapsed campaign days must provide `gameDays`, and the survivor owner must supply current morale and assignment. `GroundSurvivor` takes an actual companion ID and bond strength; a narrative mention of comfort is insufficient input. `ApplyShelterRest` modifies recovery only after the shelter rest owner accepts the rest. Check `IsActionBlocked` at assignment or expedition dispatch, rather than showing a warning after a dangerous assignment already began.

The first implementation slice is a real post-dive exposure, one day of eligible recovery, and a readout that names the limitation and remaining state without diagnosing the person from flavor text. The maritime host's demonstration method currently uses a generic exposure and a fixed morale of 50; it is not a substitute for the live return event. Preserve existing PsychContaminationSave on old and new saves. A failed exposure or refused grounding leaves that state unchanged. Pair any candidate debrief prose with a current contamination type and a truthful speaker. Avoid language that says the survivor is permanently broken when Core can expire the entry.

**Acceptance:** a real dive return yields one entry; the assignment gate sees the same state; a day tick changes it once; reload retains its stage; reopening the panel does not apply exposure again.
""",
    """### Rulings are already stateful; expose a real Crossing dispute

`ExpansionHostSession` constructs Arbitration, subscribes to its changes, includes it in `ExpansionHubSaveCodec`, and exposes `ArbitrationLine()`. It also loads a default backer pool in host code. The bounded work is a live dispute route that asks `CallStanding(topic, currentDay)`, then accepts a `DeclareBacker` result or a `TryBribeBacker` result under the Core rules. If backer definitions are moved to JSON, compare existing Crossing NPC IDs and preserve migration; do not duplicate the pool in a panel. A refused bribe is a public mark only when `TryBribeBacker` returns that result and its event is consumed.

The same topic may have history, an active ruling, and an overturned ruling. A UI summary must use `GetRuling`, `GetRulingHistory`, and the active/overturned predicates rather than a single generic success label. `CrossingQuestSystem` owns quest stage and `VouchAccessSystem` owns crossing access. Define the one-way adapter that turns a finished arbitration fact into any quest consequence, with an idempotent receipt. Candidate petitions and testimonies must identify which backer knows the evidence; the narrator cannot reveal a hidden bargain to the public notice. The first route may use one existing quest topic; broad backer content comes after that route works.

**Acceptance:** call, declare, refusal or acceptance, overturn where eligible, then restore; no duplicate ruling or unearned vouch.
""",
    """### Contract ink and funds settlement are different authorities

`ExpansionHostSession` owns one `LedgerDebtSystem` and one `DebtConsequenceDispatcher`; its save captures ledger, dispatcher, embargo, and debt bridge state together. `ledger_debt_templates.json` is already loaded there. The dated save contract's additive creditor/template fields are a migration note, not evidence that a player can pay. First trace `PresentContract`, `SignContract`, `TickDaily`, and `PayContract` to the current funds or goods owner. `TotalOwed` reads a debt; it cannot be treated as a second wallet. A contract payment must either commit in both the canonical funds owner and the ledger or leave both unchanged. If no transaction API supports that atomicity, the package stops for an owner decision rather than writing a compensation hack in the panel.

The dispatcher should respond to a unique accepted default/forfeit fact and persist its fired set; restore should not replay a past embargo or standing penalty. A printed statement may show principal, rate, day, creditor, and forfeit only from the accepted contract. The interest calculation and due day come from Core, not from prose or a visual progress bar. The first content cohort is the existing template rows; add no lender archetype solely to reach a word target. Because XP-04 funds/goods legs are decision gated in the master document, any loan-shark cross-link remains conditional on the named economy signature.

**Acceptance:** one offer, one signature, a deterministic day advance, one settled payment or explicit refusal, and a mid-contract restore with no repeated consequence.
""",
    """### Choose the caravan instance that the player actually trades with

`Main.Economy.SetupCaravans` creates `_caravans`, binds the canonical embargo system, world map, weather availability provider, and `caravan` save section. `Main.ShelterBatch3` separately creates `_travelingCaravan` and binds the visible `TravelingCaravanPanel` to that instance. This is a current, concrete two-instance risk: the panel may show/trade against an instance that the economy day owner saves and ticks separately. The first change is an ownership audit and a shared-session binding decision by the integrator. Do not merge saves by choosing the larger active caravan list; establish which legacy data was captured and what route invoked each instance.

`TravelingCaravanHostSession.Buy` passes `playerRations` by reference into `TryBuyItem`, but a caller can pass a copy of canonical stock. Prove that the caller writes the accepted balance back through the inventory/economy owner and that item receipt has the same commit boundary. If that cannot be guaranteed, expose a preview only until a transaction adapter is approved. A route board can distinguish expected arrival, delayed road, and known current node, but may not claim the merchant arrived because the panel locally advanced its clock. Use the current `caravans.json` records as authored identity; `caravan_trade_routes.json` and map facts must be checked before adding extra stops.

**Acceptance:** the panel and day tick reference the same session object, a route arrival persists, and a purchase changes canonical stock and funds exactly once.
""",
    """### Select a single memory writer before adding recast prose

`StandingRecordEngine` constructs and captures `LocationMemorySystem` inside a unified state, while `ExpansionHostSession` constructs another LocationMemorySystem and captures it in `ExpansionHubSave`. `StandingRecordHostSession` exposes `AllStrata` and the atlas reads that host. This is more than a missing UI link: visits recorded in the expansion hub can disagree with the atlas's visit count or recast. The integrator must map every `RecordSiteVisit`, `ApplyMutation`, and restore call and choose one live writer. For an existing campaign with both envelopes, specify precedence, merge policy for visit counts, and non-replay of recasts. A merge must be deterministic by stable site and stratum IDs; it must not sum the same visit twice.

`standing_record_memory.json` contains authored strata; the plan should make each candidate note answer who left it, when it becomes legible, and which layer of the site supports that knowledge. A found note can be uncertain about history. It cannot announce a mutation that no quest, encounter, or visit fact applied. The first package needs one first visit and one revisit at a current site, and a known atlas route. The rest of the review cards are content prompts after owner consolidation, not a mandate for hundreds of new memories.

**Acceptance:** one visit produces one count in the chosen owner, one recast after a real mutation, and the same atlas text after restore from an old and a new envelope.
""",
    """### Encounter resolution follows one room and one save

`StandingRecordEngine` also owns a `SiteEncounterSystem`, while `ExpansionHostSession` owns a separate one. Both capture histories. The source's `StartEncounter` accepts an ID, room, kind, day, and contextual data; a UI should not synthesize a room entry or use a generic catch-all encounter to make a button responsive. Start only after `LocationLayoutSystem` confirms entry, then resolve through `ResolveEncounter` once. `IsResolved` should suppress a second reward or story outcome on return. `ScrapePlate` and `RestoreOverlayAccess` are specific operations and should retain their own eligibility; neither is a substitute for clearing history.

Before moving a caller, list the two instances' existing save paths and the panel bound to each. A consolidation plan must state how it handles a legacy save in which one instance resolved the encounter and the other did not. The safe default is never to replay a resolved reward, but the exact precedence requires current content IDs and a named owner decision. A room note can describe material clues and uncertainty; it must not say a threat was defeated while the encounter remains active. The first content cohort is one existing room and one existing encounter ID, subject to loader proof.

**Acceptance:** one room entry, one eligible start, one resolution, repeat entry no-op, and identical resolved state in the atlas and saved envelope.
""",
    """### Cohort life stage is already persisted with the dose ledger

`DoseLedgerHostSession.Cohort` is constructed with the dose ledger and `DoseLedgerSaveCodec` captures it. Existing host helpers book a demonstration child and correct its baseline; a real birth/care route has to supply a stable child ID, parent IDs, birth day, and the uncertainty band. `CorrectBaseline` should occur only when the source of better information exists. `CheckCohortMaturation` is the day-level transition; do not call `TryMaturation` again from a panel or duplicate an aged-child event removed by the earlier integration work.

School eligibility, work eligibility, and child food units are queries over this owner, while education, duty roster, and rations execute their own outcomes. The long-horizon census and epilogue should read the persisted child record rather than create a second child profile. A child's memory may matter to a later witness, but the spiritual authority map explicitly forbids a new piety or devotion number. Candidate prose should allow ambiguity about inherited stories and avoid reducing a child to a reward for a parent. The first package should be one real booking and one policy consumer, then the maturation boundary.

**Acceptance:** a booked child changes canonical child-ration demand, survives reload, and matures on the intended day exactly once without a duplicate roster entry.
""",
    """### The volunteer register records consent, not the dose itself

`DoseLedgerHostSession.Voluntary` and `DoseLedgerSave.voluntaryRegister` already exist. Its demonstration method signs and completes a hard-coded corridor task. A production command should resolve a current survivor and task ID, display the hazard briefing, and call `Volunteer` only after an explicit accepted intent. A refusal is not a volunteer record. `CompleteVolunteer` receives `doseIncurred`; this must come from the authoritative dose ledger's actual work outcome, not an estimate shown before the shift. If a task aborts, document whether the register remains pending or is canceled through an existing API; inventing a completed dose of zero is not a neutral workaround.

The body/mind map should show direction: assignment owner approves work; voluntary register records consent; dose ledger records exposure; completion records the observed dose and may publish a human acknowledgment. A signed entry must not waive equipment or medical limits. Prose should distinguish consent under pressure from a free choice and should not imply that a survivor can consent on behalf of someone else. A restored register is a record of what happened, not permission to rerun a shift.

**Acceptance:** refuse and accept paths, one real completed task, actual ledger dose equality, and reload without a second completion.
""",
    """### Keep the scorer pure and the assignment owner singular

`UtilityAiHostSession` loads `utility_actions.json` and uses `SelectAction` in `EvaluateDemo` with `new SeededRng(2026)`. `UtilityAiPanel.RefreshView` creates its own scorer and a new UtilityAiSystem to call `ScoreAll` for a hard-coded demo survivor. That may be harmless for a stateless score display, but it is not a live survivor decision path. The first integration package must trace an actual survivor context from needs, skills, injury, current room, and task owners, then dispatch the selected action through the one assignment owner. If selection has seeded tie breaks, use the campaign RNG stream; do not promote the demo's fixed seed.

The historical override contract states that all 20 actions are non-override because no emergency interruption executor exists. Preserve that status. An override is a later package, gated on an explicit executor and its issue/expiry/veto/persistence contract. The panel may display scores and a reason, but it may not create an override or a second action queue. The current catalog's 20-action expansion is a content starting point, not evidence that all actions have real executors. Each proposed action row needs a loader, gate, executor, and visible outcome before release. The first package should select one ordinary action with a current executor.

**Acceptance:** a live context scores deterministically, one accepted ordinary action reaches the task owner, vetoes explain themselves, no unsupported override runs, and panel refresh causes no action.
""",
    """### Poll once at the Reckoning boundary and hand off only fired facts

`VerdictHostSession` builds `VerdictRadioSystem` from the current bus, clock, and `verdict_radio.json` entries; the verdict save includes fired IDs and the panel reads the host. The previous 30-broadcast content expansion should not be repeated. The remaining integration question is the phase/day caller: which live Reckoning transition calls `Poll(day, phase)`, how the returned IDs become radio output, and how an NPC reaction is keyed to a fired broadcast rather than to a catalog match. `HasFired` and `CaptureState` are the guard and restore evidence. A stale radio transcript may remain readable, but should not play again simply because the player reopened the panel.

The handoff contract must preserve information flow. A local NPC can react only if they plausibly heard a transmitted packet or were told of it by an existing rumor path. A broadcast may report public events, not hidden player facts that the station could not know. The first package uses one existing broadcast and one NPC response, if that NPC route is live. An unmet trigger remains silent, and a phase change should not cause a retroactive line unless the catalog specifies that window.

**Acceptance:** one eligible broadcast fires once, a second poll is quiet, save/reload retains the fired ID, and the NPC line appears only after the actual radio event.
""",
    """### A Core eulogy consumer is not proof the provider was bound

The old Plan 41 log says the eulogy was wired. `MemorialSystem.Memorialize` does contain a conditional `EulogyEngine.ComposeEulogy` call when eulogy text is empty, and it stores the accepted result in `MemorialEntry.EulogyText`. Current `src/Main.Campaign.cs` constructs MemorialSystem but the inspected setup contains no `EulogyEngine` assignment; `rg` found no direct host construction of `ProceduralEulogyEngine`. The first work item is therefore a precise provider-binding audit. If no other setup assigns it later, bind it in the current memorial owner only after the life-record source is identified. `DwellerLifeRecord` fields must come from existing survivor, skill, relation, journey, and journal records; a missing fact stays absent.

`EulogySaveState.archivedEulogyTexts` and `MemorialEntry.EulogyText` can hold overlapping prose. Choose the memorial entry as the authoritative accepted text unless a current save migration requires the archive for another purpose. Composition happens once when the memorial is committed; reload displays the saved text. A manually authored eulogy should take precedence as Core already intends. Candidate lines should show a particular action or object the person left behind, not a generic praise formula or invented biography. The existing eulogy corpus is a voice/reference source only if its loader and consumer can be proven.

**Acceptance:** a bound provider composes once from one real life record; a supplied eulogy is preserved; both restore verbatim with no reroll.
""",
    """### Classify the two ideology states before proposing another integration

`INTEGRATION_PLANS.md` records Plan 148 full integration on 2026-09-23. `IdeologicalFrictionHostSession` has event state, a save store, Main setup/day calls, and a player surface. Separately, `SurvivorSocialCoordinator.Friction` constructs the original `IdeologicalFrictionSystem`, whose `RegisterBelief`, `GetAffinity`, and `TickRoommates` track baseline compatibility. The first task is a collision audit: determine whether the event host references that same Friction object or maintains a separate one, and whether both serialize affinity. If they are intentionally distinct, name the data boundary; if they duplicate it, specify one migration and one writer. Never stamp the entire feature NON-INT from an old direct-reference count.

The spiritual map's no-faith-meter rule remains active. A belief ID can influence roommate affinity through the social owner and an authored ideological event through the Plan 148 host; those are two consumers of one identity, not license to create a numeric piety state. Public confrontations may reveal what an NPC said; private affinity must not be shown as exact knowledge unless the panel has a justified read route. The first work package may be only a classification correction if current hosts already share the same state and complete the route.

**Acceptance:** one belief and one event are traced from source fact to the correct save/read surface, with restore preserving both and no duplicate confrontation.
""",
    """### Patrol selection must consume doctrine without becoming doctrine

`YearOfAshHostSession` already constructs `WarlordDoctrineSystem` and loads the doctrine catalog. The patrol matrix currently explains `TravelEncounterSystem.SelectEncounter`; expand it with an explicit producer/consumer trace, not a second patrol selector. `WarlordDoctrineSystem` owns territory, reports, a daily strategic action, and `TravelDangerModifier`. An observed report may differ from actual territory state, so UI text should distinguish confirmed control from rumor. `TickDaily` takes `ISeededRng` and `WarlordContext`; the campaign stream and weather/faction facts must supply those inputs in stable order.

If doctrine changes a patrol weight, define the adapter's exact moment before encounter selection and preserve TravelEncounterSystem's own result authority. A hostile-access rule can reject travel without manufacturing a battle. `SettleTribute` changes doctrine state, while canonical economy must settle the paid amount; a user-facing receipt needs both accepted facts. Existing faction war owns combat and standing. Candidate patrol orders should use public doctrine language, while captured internal orders can expose motive only after discovery. Start with one current territory and one patrol archetype, and inspect `warlord_doctrines.json` references before writing new orders.

**Acceptance:** same-seed territory and context produce the same patrol eligibility; restore preserves reported and actual states; a rejected or unpaid tribute does not appear settled.
""",
    """### The atlas and expedition hub must refer to the same record

`StandingRecordHostSession` creates `StandingRecordEngine`, loads its layout/memory/encounter children, and captures a unified `StandingRecordSave`. Its atlas panel binds directly to that host. `ExpansionHostSession` separately constructs and saves the same three child-system types in `ExpansionHubSave`. The faction wiring tracer should become an authority diagram with both actual branches and a migration decision gate. A faction standing change is an external fact that may unlock or annotate a record; it cannot be copied into a second faction-standing number. A site mutation should enter the chosen record engine once and then refresh the atlas. Until one writer is selected, expanding narrative strata increases the chance of two contradictory histories.

The first package is an instance map: constructors, Main fields, save sections, restore order, panel binding, expedition/quest mutation callers, and legacy-save precedence. Only then should it choose which host survives as the production record path. For conflict resolution, compare stable site IDs, mutation flags, visit counts, and resolved encounter IDs; never choose whichever snapshot has the most text. The older envelope may require a one-time import, but import must be idempotent and recorded. A current site/faction link is enough to prove the path before more atlas lore is added.

**Acceptance:** one visit/mutation produces one atlas update and one canonical saved state; old saves migrate once without lost or duplicated history.
""",
    """### Forecasts are a saved projection of weather, not the weather owner

`WeatherIntelligenceCoordinator` constructs `WeatherStationSystem`, calls `GenerateForecast` during its day tick, builds the read model, and captures `WeatherIntelligenceSaveState.station`. `WorldHostSession` restores that coordinator, binds `weather_route_gates.json`, and exposes status plus demo install/calibrate methods. The old accuracy matrix remains useful for formulas, but its host count is stale. A real installation command needs current material, power, skill, and construction-owner evidence; a demo convenience method cannot spend them. The station's `Repair` and `Degrade` should follow the existing maintenance or weather damage facts, not a panel timer.

Actual weather remains `WeatherSystem` state. A forecast entry carries uncertainty, date, and confidence; route planning may use `IsRouteSafe(day, routeId)` as advice or gate only if its authority is explicitly approved. Do not rewrite the actual weather to match a prediction. On restore, the cached forecast must display identically until the next authorized forecast generation. Candidate forecast strips should name what the station measured and what it infers, making a wrong forecast legible rather than silently changing history. The first package is an installed station, one route forecast, and one current route consumer.

**Acceptance:** installation consumes approved resources once, forecast is deterministic for same seed/day, confidence is visible, and reload does not reroll it.
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
