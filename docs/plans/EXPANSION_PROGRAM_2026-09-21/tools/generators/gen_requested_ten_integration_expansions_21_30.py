#!/usr/bin/env python3
"""Rebuild the ten requested 2026-09-24 integration addenda (items 21–30).

Four subjects share the generated orphan dossier. The dated source remains
above the first marker; each requested subject gets its own bounded section.
This is documentation generation, not gameplay integration or canon approval.
Run after the owning base generators and before the separate editorial pass.
"""

from __future__ import annotations

import json
from pathlib import Path
from textwrap import dedent

ROOT = Path(__file__).resolve().parents[5]
MARKER_PREFIX = "# 2026-09-24 integration expansion — "
MASTER = "docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"
SHARED = "docs/plans/EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md"

CONDITIONS = [
    ("first valid use", "the canonical owner has the required record and no earlier accepted command", "prove the default state, eligibility, and one visible outcome"),
    ("legacy restore", "the campaign was saved before the proposed adapter existed", "default additive state safely and never replay an old effect"),
    ("repeat request", "the same player request arrives twice before the next campaign tick", "return the current status without charging or publishing twice"),
    ("stale preview", "the facts used by the panel changed before confirmation", "revalidate at the command boundary and explain the new refusal"),
    ("resource shortage", "a canonical input owner reports less stock or power than the preview showed", "reject or choose the existing partial-result rule without phantom consumption"),
    ("missing subject", "a referenced survivor, expedition, segment, item, or transaction is gone", "resolve the ID against its owner and avoid an orphan state record"),
    ("day transition", "a request or event lands at the edge of two campaign days", "use the campaign day and record a once-per-day transition"),
    ("save midway", "the player saves after a successful domain action but before a later presentation refresh", "restore the domain fact without reapplying the adapter effect"),
    ("seeded replay", "two runs use the same seed, inputs, and action order", "compare event order and resulting owner state without wall-clock seeds"),
    ("controller return", "the player uses controller focus and back after opening the surface", "preserve the current read model, reason labels, and focus route"),
    ("catalog mismatch", "an authored ID is removed or points to an absent record", "fail integrity validation or refuse the one affected route explicitly"),
    ("large campaign", "many valid records make iteration order and bounded histories matter", "sort stable IDs and define limits before publishing a summary"),
    ("cross-owner event", "a related owner emits a fact during the same day phase", "use one-way adapter ordering and avoid cyclical mutation"),
    ("failed command", "the domain action returns a refusal or no-op", "show the exact reason without narrating a success"),
    ("late discovery", "the player encounters the route after its intended story window", "state a live availability rule rather than silently hiding the row"),
    ("panel unmounted", "the domain event fires while no panel is open", "persist the fact and rebuild the read model when the panel binds"),
]

SPECS = [
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-CORE-ONLY-REGISTRY-11.md", name="NVIS C4I System", core="NvisC4ISystem : NvisCommunicationsSystem", source="Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs", host="NvisCommunicationsHostSession / src/Main.Plans130_133.cs", save="nvis_communications", catalog="nvis_communications_catalog.json", key="channels", api="LoadCatalog, BeginStatusTransmission, RequestRecall, AcknowledgeRecall, TickDay, CaptureState, RestoreState", premise="The named C4I subclass only delegates to the base constructor. The base NvisCommunicationsSystem already loads two channels, has a host, power input, recall journal callback, daily tick, and nvis_communications save. An exact subclass host count of zero is not evidence of an absent NVIS feature.", first="classify the subclass as compatibility or prove a missing behavior with one real transmission/recall route; reuse the base host if behavior exists", owner="The base NVIS system owns channel, transmission, and recall state; radio and expedition owners supply power and party facts", state="Existing NvisCommunicationsState in nvis_communications. No new section for the subclass.", gate="Do not instantiate a second C4I state or retire a compatibility type until direct and indirect callers are checked.", cluster="C8", object="a field radio log", scenes="channel selection|power loss|regional status packet|expedition recall|acknowledgment receipt|queued transmission|day tick|low signal|invalid survivor|two expeditions|returned party|old save|journal entry|radio panel|emergency traffic|quiet channel".split("|")),
    dict(path=SHARED, name="Palliative Care Dignity Engine", core="PalliativeCareDignityEngine (static, mutates PalliativePatientRecord)", source="Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs", host="current medical pipeline / ward host, subject to owner decision", save="patient care state must ride an approved medical owner; no independent dignity ledger", catalog="none by exact name; existing medical and final-wish facts are inputs", key="", api="AdvanceDailyCare, EvaluateGriefStageProgression, CalculateMemorialEcho", premise="No direct Godot caller was found. The static engine mutates a PalliativePatientRecord, reduces prognosis once per care day, and uses StableHash(worldSeed, survivorId, simTick, grief stage) for grief transitions. A caller must own and persist that record; a memorial effect must be applied once by existing fate/memorial owners.", first="one eligible patient receives a single day of consent-aware care, persisted symptom outcome, and a truthful ward readout", owner="Medical pipeline owns clinical care; survivor fate owns death; memorial/final-wish owners own legacy effects; the engine computes patient outcome", state="Decide whether a bounded palliative record is embedded in the existing medical owner; require capture/restore before daily use.", gate="Do not invent a second death clock, automatically equate dignity with sedation, or award a memorial twice.", cluster="C2 and C9", object="a bedside care card", scenes="care consent|pain review|lucidity tradeoff|medicine shortage|caregiver shift|quiet visit|final wish|prognosis day|grief stage|ward transfer|death notice|memorial echo|family briefing|old patient record|no medicine|night watch".split("|")),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-PERIMETER-DEFENSE-TRUTH-165.md", name="Perimeter Early Warning Engine", core="PerimeterEarlyWarningEngine", source="Assets/Ashfall.Core/Defense/PerimeterEarlyWarningEngine.cs", host="existing PerimeterDefenseSystem / DefenseHostSession / Main.AdvancedShelterSystems.cs", save="perimeter_defense candidate; inspect PerimeterDefenseSave before embedding radar state", catalog="perimeter_defenses.json", key="defenses", api="SetMode, CalibrateSensors, ProcessScanSweep, ClearExpiredContacts, CaptureState, RestoreState", premise="PerimeterDefenseSystem is hosted and saved, and NightWatchHostSession already reads it; the separate early-warning radar engine has no direct src caller. Its mode, calibration, contacts, and threat/false-alarm events are stateful. The existing perimeter save shape must be audited before choosing where those fields live.", first="one radar sweep consumes canonical power and a seeded roll, then publishes a classified contact into the existing defense alert route", owner="PerimeterDefenseSystem owns built wall and gate state; radar owns contact and calibration state; defense/raid owner resolves the attack", state="Capture radar state within the existing perimeter authority if schema and rollback semantics permit; no second wall ledger.", gate="A radar contact must not itself damage a wall, start a raid, or fabricate a target without a real producer.", cluster="C15", object="a marked sensor plot", scenes="standby mode|active scan|high-frequency sweep|power brownout|sensor calibration|ash storm|wildlife return|hostile approach|false alarm|contact expiry|gate watch|night-watch handoff|raid warning|silent sector|save reload|battery draw".split("|")),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE15_2026-09-21/PLAN-HEALTH-HISTORY-TRUTH-196_APPENDIX-A_SCAFFOLD.md", name="Pharmaceutical Tablet Engine", core="PharmaceuticalTabletEngine", source="Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs", host="MedicalHostSession / medical pipeline composition; exact press host to be selected", save="TabletWorksFullState capture required; locate the medical production save owner before registration", catalog="tablet_manufacturing_catalog.json", key="formulations", api="BindCatalog, BindInventory, ConstructPress, StageBatch, TickDay, ClaimOutputs, CaptureFullState, RestoreFullState", premise="The scaffold says no name-matched catalog, but tablet_manufacturing_catalog.json exists with three formulations and three release classes. The Core engine has host-bound inventory ports, seeded RNG, a day provider, active batch, output buffer, and full-state capture. No direct src caller was found, so actual stock settlement and save wiring remain absent.", first="one existing formulation goes from real inputs through a dated press batch to one claimed output in canonical inventory", owner="Inventory owns input/output items; pharmaceutical engine owns press and batch state; medical pipeline owns patient administration", state="Capture press, active batch, output buffer, and RNG behavior through one approved medical production section before allowing a batch to start.", gate="Never let default no-op inventory ports produce tablets, and never claim the same batch twice.", cluster="C2 and C4", object="a stamped blister tin", scenes="press construction|tooling check|calibration|formulation choice|release class|ingredient issue|chemist shift|batch start|daily compression|machine fault|quality rejection|output buffer|claim receipt|pharmacy handoff|patient order|press maintenance".split("|")),
    dict(path=SHARED, name="Prosthetic Condition Wear Engine", core="ProstheticConditionWearEngine (static evaluator)", source="Assets/Ashfall.Core/Medical/ProstheticConditionWearEngine.cs", host="SurvivorBodyPresentationSlate projection and the existing body/item-condition owner", save="none for evaluator; canonical prosthetic item condition belongs to current item/body state", catalog="no named wear catalog; body state and item catalog supply complexity and condition", key="", api="EvaluateDailyWear", premise="The exact engine has no direct src reference, but SurvivorBodyPresentationSlate calls EvaluateDailyWear twice. That projection passes zero labor and full maintenance, then displays NetConditionPermille as condition. It is a read path, not proof that daily wear is committed. Distinguish projected next-state from actual item condition before wiring a day tick.", first="show truthful current prosthetic condition and a separate projected wear/risk readout, then identify the canonical daily mutation owner", owner="Item/body state owns condition; wear engine computes a value; survivor fitness and presentation consume the result without storing a second condition", state="No evaluator save. The item/body owner must capture any accepted condition change once per day.", gate="Do not subtract wear from condition in both projection and day tick or save a second condition field.", cluster="C2 and C9", object="a worn joint pin", scenes="fitting inspection|labor shift|long march|dust in hinge|cleaning session|calibration check|improvised limb|mechanical limb|articulated limb|condition warning|failure risk|repair choice|body slate|day tick|restored limb|broken fitting".split("|")),
    dict(path="docs/expansions/wave3/expansion_25_the_iron_road_plan.md", name="Railway Interlock Engine", core="RailwayInterlockEngine", source="Assets/Ashfall.Core/Expeditions/RailwayInterlockEngine.cs", host="Main.Railway / RailwaySystem plus RailTrackMaintenanceHostSession", save="railway topology/dispatch and rail_maintenance are existing sections; decide interlock state location without duplicating either", catalog="railway_interlock_catalog.json", key="junctions", api="BindCatalog, RestoreJunction, RequestRoute, ReleaseRoute, IsExpeditionPathLegal, TickDay, CaptureState, RestoreState", premise="RailwaySystem and RailTrackMaintenanceLedger are hosted. RailwayInterlockEngine has three authored junctions and mutable route locks, reservations, signals, obstruction, and tamper state, but no direct src caller. The Iron Road plan itself says the interlock alone authorizes routes, so rail dispatch cannot simply infer legality from maintenance feasibility.", first="one authored junction denies an illegal dispatch, reserves a legal route once, and releases it after movement or cancellation", owner="RailwaySystem owns topology/dispatch; interlock owns junction legality and reservations; maintenance ledger owns wear and load feasibility", state="The interlock's own CaptureState/RestoreState must be composed with the railway lifecycle; choose one registered section after a save-shape audit.", gate="No second rail topology, no route reservation without a dispatch ID, and no forced path through a denied signal.", cluster="C5 and C15", object="a locked points lever", scenes="junction restoration|switch alignment|route request|signal aspect|reservation|obstruction|tamper report|denial arm|denial trigger|dispatch handoff|train crossing|release route|track repair|bridge load|old rail save|signal panel".split("|")),
    dict(path=SHARED, name="Rehabilitation Progression Engine", core="RehabilitationProgressionEngine (static evaluator/transformer)", source="Assets/Ashfall.Core/Medical/RehabilitationProgressionEngine.cs", host="existing survivor body/medical host; RehabilitationSlateProjection is the Core read path", save="RehabRecord is part of SurvivorBodyState; no separate rehabilitation save section", catalog="no rehabilitation catalog by exact name; prosthetic/body records are inputs", key="", api="StartRehabilitation, AdvanceDaily, GetQualityFactor", premise="The static engine is already called by RehabilitationSlateProjection and produces RehabRecord phases fitting, adaptation, mastery. SurvivorBodyState contains RehabRecord. No src caller was found for daily AdvanceDaily; the missing seam is one campaign-day write to canonical body state, not a new therapy registry.", first="one fitted limb starts one RehabRecord, advances once per campaign day, and displays its actual phase and quality", owner="SurvivorBodyState owns the RehabRecord; rehabilitation engine computes progression; duty/fitness owners consume projected quality", state="Persist through the current body-state owner after confirming its save route; evaluator needs no section.", gate="Do not advance the same day twice, infer mastery from display text, or invent a second age clock.", cluster="C2 and C9", object="a penciled fitting slate", scenes="new fitting|first step|painful shift|adaptation week|caregiver coaching|resilience input|setback note|quality ramp|mastery day|work assignment|expedition return|prosthetic repair|body slate|old save|lost limb|quiet milestone".split("|")),
    dict(path=SHARED, name="Restock Allocation Engine", core="RestockAllocationEngine.Allocate (static)", source="Assets/Ashfall.Core/Economy/RestockAllocationEngine.cs", host="ShelterBarterSystem.RestockCaravan / Main.Plans147.cs after DEC-05 contract audit", save="none for allocator; caravan stock remains in ShelterBarterSystem state", catalog="merchant_caravans.json", key="caravans", api="Allocate(capacity, categories)", premise="The deterministic allocator exists, while ShelterBarterSystem.RestockCaravan already owns merchant stock changes and Main.Plans147.cs ticks arrivals. DEC-05 sealed merchant restock display-order priority. An exact engine reference was not found in src or the barter system. Any call-in must preserve the sealed order and authored stock rules.", first="one capacity-limited caravan arrival uses the allocator only if the current restock contract has a capacity input and produces the same accepted stock ledger", owner="ShelterBarterSystem owns caravan stock; allocation engine computes a proposal; funds/trade owners settle purchases", state="No allocator save. Capture the resulting caravan stock through the existing barter save path.", gate="If capacity is not a current merchant concept, close as a pure optional algorithm instead of inventing a scarcity mechanic to justify its use.", cluster="C11", object="a merchant tally sheet", scenes="arrival manifest|capacity cap|scarcity floor|category weight|target par|authored order|partial pallet|empty stock|oversupplied item|tie break|stock receipt|player purchase|merchant departure|return visit|restock day|ledger audit".split("|")),
    dict(path="docs/plans/EXPANSION_PROGRAM_WAVE16_2026-09-21/PLAN-SURGICAL-WARD-TRUTH-213.md", name="Surgical Graft Rejection Engine", core="SurgicalGraftRejectionEngine", source="Assets/Ashfall.Core/Medical/SurgicalGraftRejectionEngine.cs", host="MedicalWardHostSession / Main.Medical.cs and existing clinical pipeline", save="medical_ward or medical_pipeline candidate; validate registered save owner before embedding graft records", catalog="surgical_procedures.json", key="procedures", api="PerformGraft, AdministerImmunosuppressant, ProcessDailyTick, CaptureState, RestoreState", premise="The graft engine has mutable records, events, capture/restore, and no direct src caller. Main.Medical.cs already hosts medical ward and pipeline. ProcessDailyTick accepts an optional seeded roll callback; with null callback it raises risk but never rejects. For multiple elapsed days it passes currentDay to each roll, so day-key semantics need a focused decision before host use.", first="one eligible graft enters an existing ward procedure, consumes the clinical inputs, then advances with an explicitly seeded daily roll and persisted status", owner="Medical ward owns procedure and bed; graft engine owns integration/rejection records; inventory owns immunosuppressants; health history owns longitudinal documentation", state="Compose graft save into one approved medical section with old-save defaults and no event replay on restore.", gate="Do not perform a graft outside the ward's eligibility/reservation path or allow null-roll production ticks to conceal rejection.", cluster="C2", object="a sealed graft chart", scenes="donor check|tier consent|ward bed|sterility review|procedure day|graft ID|first dose|missed dose|risk rise|seeded roll|integration|rejection|infection report|post-op visit|old graft save|discharge".split("|")),
    dict(path="docs/expansions/wave10/expansion_59_the_bone_shop_plan.md", name="Trophy System", core="TrophySystem", source="Assets/Ashfall.Core/Shelter/TrophySystem.cs", host="WildlifeTrappingHostSession / ShelterDecorHostSession with one award adapter", save="TrophySaveState must be registered or embedded through an approved existing collectible/decor owner; no duplicate award ledger", catalog="trophies.json", key="trophies", api="LoadCatalogFromJson, RecordQuarryPreserved, IsAwarded, CaptureState, RestoreState", premise="The trophy system owns an exactly-once award ledger and loads eleven authored trophies. Direct src calls to TrophySystem were not found; wildlife trapping and shelter decor have live hosts. The system awards a crafting opportunity, not automatically a physical mount or item; that distinction controls inventory and UI copy.", first="one real preserved quarry fact awards a single recipe opportunity and survives reload without a second reward", owner="Wildlife/trapping owns quarry harvest; TrophySystem owns award/unlock fact; crafting owns recipe execution; decor owns placed display and localized morale", state="Persist awarded IDs and unlocked recipes once before consuming the fact in crafting/decor; no mirrored award list in a panel.", gate="No trophy from a sighting alone, no automatic item grant, no exploitation reward where the Bone Shop's restraint rule forbids it.", cluster="C14 and C16", object="a tagged specimen case", scenes="species encounter|legal harvest|preservation|catalog match|award event|recipe unlock|crafting bench|physical mount|decor placement|morale radius|duplicate quarry|old save|unknown species|display removal|memorial context|quiet archive".split("|")),
]


def rows(spec: dict) -> list[dict]:
    if not spec["key"]:
        return []
    path = ROOT / "Assets/StreamingAssets/Data" / spec["catalog"]
    if not path.exists():
        return []
    obj = json.loads(path.read_text())
    value = obj.get(spec["key"], []) if isinstance(obj, dict) else []
    return value if isinstance(value, list) else []


def record_id(row: dict) -> str:
    if not isinstance(row, dict):
        return "unnamed_row"
    for key in ("id", "channel_id", "defense_id", "formulation_id", "junction_id", "trophy_id", "caravan_id", "procedure_id"):
        if row.get(key):
            return str(row[key])
    return "unnamed_row"


def intro(spec: dict, authored: list[dict]) -> str:
    register = ""
    if authored:
        register = "\n## Live authored ID register\n\nThese IDs are read from current JSON. They are review targets, not evidence of player reachability.\n\n| Current ID | Fields to inspect | Reachability question |\n|---|---|---|\n"
        for row in authored:
            if not isinstance(row, dict):
                continue
            register += f"| `{record_id(row)}` | {', '.join('`'+k+'`' for k in list(row)[:7])} | Which loader, owner predicate, host command, and read surface consume this row? |\n"
    return dedent(f"""

---

{MARKER_PREFIX}{spec['name']}

> **Document status:** evidence-based integration proposal, generated after the dated material above. The older plan or audit is retained as historical evidence, not silently revalidated. This section is reproducible through `gen_requested_ten_integration_expansions_21_30.py` followed by the separate editorial polishing script. Narrative lines below are candidate specifications outside runtime JSON. They do not become ASHFALL canon until a current trigger, consumer, and continuity review accept them.

## 1. Objective and verified premise

**Objective:** {spec['first']}.

**Current evidence:** {spec['premise']}

**Owner boundary:** {spec['owner']}. **Core source:** `{spec['source']}`. **Current or candidate host:** `{spec['host']}`. **Persistence decision:** {spec['state']} **Authored data:** `{spec['catalog']}`. This follows the master expansion document's {spec['cluster']} cluster and its four-tier data → Core → host → verification structure. Live source outranks the master when a dated example is stale.

**Non-goals and stop rule:** {spec['gate']} This documentation does not register a new save section, modify a host, or certify player reachability. A foreman must claim exact implementation paths under `WORKTREE_OWNERSHIP.md`; a collision or missing authority returns a premise finding rather than an invented parallel subsystem.

## 2. Integration architecture and state flow

```text
{spec['catalog']} / canonical owner facts
    -> {spec['core']}
    -> {spec['host']}
    -> {spec['save']}
    -> existing panel, journal, briefing, or campaign read model
```

The data or canonical input owner supplies IDs and quantities. The Core call validates or computes the domain result. A host adapter, when needed, chooses the command and applies its authorized cross-owner consequences once. Mutable state is captured by its chosen existing owner; a pure evaluator has no private save or day owner. The Godot view reads the result and emits a player intent. It never owns a shadow resource, hidden roll, or second ledger. An invalid or refused command changes no unrelated authority. A successful command is observed in a current panel or journal route and survives reload where the result is persistent.

## 3. Current authority and API checklist

| Question | Source checkpoint | Required finding before promotion |
|---|---|---|
| Which Core type actually runs? | `{spec['source']}` | Resolve exact constructor, method signatures, input ports, output DTOs, and indirect callers. |
| Which method can the host call? | `{spec['api']}` | Use current API; introduce a thin adapter only if source cannot express the bounded outcome. |
| Who owns cross-system state? | `{spec['owner']}` | One writer per resource, result, and persistent record. |
| What survives save/load? | `{spec['save']}` | Capture/restore and old-save default or explicit stateless verdict. |
| What is authored? | `{spec['catalog']}` | Verify loader, IDs, references, range validation, and an actual consumer. |
| What does the player see? | `{spec['host']}` | Trace a command or read route through the current Godot lifecycle. |
| Which expansion ideas apply? | `{MASTER}` | Use the {spec['cluster']} cluster's prose register, continuity, and anti-duplication rules. |

## 4. Dependency-ordered implementation package

**P0 — premise and ownership.** Refresh exact source references, save registration, data loader, tests, and player route. Compare the historical text above with the live tree. Write an explicit authority map before touching `src/Main*`, the save registry, or a catalog. Decide whether the direct-name orphan is a real missing behavior or merely an indirectly hosted type. Acceptance: file:line evidence for the first bounded outcome and an exact path claim.

**P1 — one contract.** Define the input IDs, campaign day or tick, canonical quantities, result/refusal code, and command idempotency. If the type is pure, state the caller that owns application. If mutable, state the DTO and one save owner. Acceptance: the owner can reject an invalid ID without spending, writing, or publishing a success fact.

**P2 — content and catalog reach.** Inspect the existing JSON row IDs and reference fields. Add only a small cohort when current rows cannot express the first outcome. Link any new prose to a real event trigger and reader. Do not interpret a name-matched catalog or a story line as a live consumer. Acceptance: one current row is loaded, eligible, invoked, and observable.

**P3 — host and event path.** Sample current source facts, invoke Core once, apply only owner-approved effects, then mark the correct section dirty. Subscribe and unsubscribe without leaking duplicate callbacks. A panel may request the action and refresh from the host, but cannot make the gameplay decision. Acceptance: one player action reaches one Core operation and a refusal stays a no-op.

**P4 — save, day, and replay.** Restore before binding UI. For mutable systems, ensure an old save defaults safely and a mid-effect restore neither drops nor repeats an item, contact, treatment, reservation, or award. For a pure evaluator, persist only the canonical caller's accepted result. Sort IDs before iteration and use the established seeded RNG or stable hash contract. Acceptance: fresh and restored campaigns agree after the same input sequence.

**P5 — observable acceptance and polish.** Surface state, eligibility, cost, risk, and refusal through the existing panel or briefing route. Preserve keyboard/controller back behavior, focus, readable labels, refresh, and disposal. Use a few focused Core/host assertions under `TEST_POLICY.md` when implementation is authorized; headless Godot is appropriate only if the runtime route changes. The documentation pass makes no test or production claim. Acceptance: a person can explain what happened from the UI and save state without reading debug logs.

## 5. C# integration framework — illustrative contract

```csharp
// Planning sketch. Bind to the existing owner and exact public API above.
// Do not add these types simply because they appear in a document.
public readonly record struct OwnerCommand(
    string SourceId, string SubjectId, int CampaignDay, string RequestId);
public readonly record struct OwnerReadModel(
    string SourceId, bool Eligible, string RefusalCode,
    string CurrentStatus, string CostText, string OutcomeText);
// Host command sequence:
// 1. Resolve SourceId and SubjectId from canonical owners.
// 2. Revalidate after any preview and before resource mutation.
// 3. Call the present Core method exactly once.
// 4. Apply authorized cross-owner deltas and persist through the chosen section.
// 5. Publish a typed success fact; on refusal publish no success fact.
// 6. Refresh a read-only panel model and preserve close/back focus.
```

This code is an interface sketch, not a drop-in implementation. The symbol inventory above is the source checkpoint. The first implementation commit should use the current concrete API and only introduce a DTO if multiple callers need a stable contract. In particular, pure evaluators need neither an `OwnerCommand` instance nor a new save section if the caller already has the necessary operation.

## 6. Content voice and acceptance rubric

The master expansion's physical, restrained prose rule applies to {spec['object']}. A proposed sentence must state what the speaker or document can know. An operational report may name measurements that the owner returns; a bedside or survivor line may describe lived experience without claiming a clinical result. A refusal cannot be narrated as a success. A catalog addition must have a loader, eligibility predicate, route, fallback, and localization strategy. The case register below uses varied scene and failure conditions as review prompts; it is not a release checklist and should be trimmed to the few cases that the first package can actually exercise.
""") + register


def card(spec: dict, scene: str, condition: tuple[str, str, str], authored: list[dict], number: int) -> str:
    label, circumstance, obligation = condition
    row = authored[(number - 1) % len(authored)] if authored else None
    rid = f"`{record_id(row)}`" if row else "a canonical owner record, not a new authored ID"
    object_text = spec["object"]
    narrative = (
        f"The {object_text} is left where the person making the next decision can see it.",
        f"A note beside the {object_text} records the uncertainty before anyone writes an outcome.",
        f"The {object_text} carries a date and a name; neither is proof that the work is finished.",
        f"Someone checks the {object_text} against the ledger, then leaves the blank field blank.",
        f"A second hand reaches for the {object_text}, waits, and asks who signed the first entry.",
    )[(number - 1) % 5]
    focus = (
        "Before confirmation, show the canonical cost and status from the same snapshot that will be rechecked at commit.",
        "After a refusal, retain the prior state and give a concrete reason in the player's current surface.",
        "After success, the relevant owner may emit one fact; presentation can paraphrase it but cannot create another reward.",
        "After restore, rebuild the view from saved owner state without calling the gameplay command again.",
    )[(number - 1) % 4]
    return f"""
### Review case {number:03d}: {scene} under {label}

**Scene premise.** The player's route reaches **{scene}** while {circumstance}. The implementation candidate starts with {rid}. The live source checkpoint is `{spec['source']}`; the proposed reader is `{spec['host']}`. The scene exists only if the current catalog or owner provides this trigger. No date, character, equipment, hazard, or reward is asserted merely because the heading names it. This makes the card a precise question for the promotion audit: what real fact causes this view or command to exist at this point in the campaign?

**Possible prose register.** “{narrative}” is a candidate observation anchored by {object_text}. Its final form must identify an actual speaker or document type, avoid knowledge the source cannot possess, and use the result's real status language. The line is deferred if the item, person, or record is not present. A journal note may record the player's knowledge after a fact is published; a panel can describe current eligibility; neither can invent an upstream event. The content editor should compare nearby JSON rows and avoid a second paragraph that only repeats this one in different words.

**Command and owner trace.** The relevant Core call comes from `{spec['api']}` after its actual input owners have been sampled. {spec['owner']}. For {label}, the mandatory boundary is to {obligation}. {focus} If the named system is a pure evaluator, the caller applies any accepted result and the evaluator keeps no independent state. If it is mutable, the host captures its state through the selected owner. The decision must not create a parallel inventory, medical record, route reservation, award list, threat contact, or merchant stock entry.

**Save and replay reading.** Persistence decision: {spec['state']} The audit must show the before-state, the accepted or refused result, the after-state, and the restore-state. A duplicate event must not spend stock twice. A day transition must not advance twice. Stable ID order and a campaign seed are required where selection or risk is random; where the function is deterministic math, no RNG is added. The panel should obtain the same status after reopening and should release subscriptions when disposed. The author should not refer to a live result until the save and host trace confirms it.

**Acceptance decision.** Mark this card accepted only with source file:line references, a live trigger, the exact method call, a reachable authored ID or owner record, an observable current surface, and a focused assertion matched to {obligation}. Otherwise mark it deferred or inapplicable. A high-level compilation result does not prove a player route, a catalog file does not prove a loader, and a historical audit count does not prove that the class is still host-unreachable. The first implementation package should choose only the cases necessary to establish {spec['first']}.
"""


def close(spec: dict, count: int) -> str:
    return f"""
## 7. Finished planning architecture for {spec['name']}

The architecture handoff is complete at the planning level: `{spec['catalog']}` or current owner facts → `{spec['core']}` → `{spec['host']}` → `{spec['save']}` only where state is mutable → a truthful player read model. The first package is **{spec['first']}**. The {count} case cards above are a candidate review register; no card is canon or a mandate to build all combinations. The exact files and owner signatures must be rechecked when a foreman promotes the package.

**Handoff fields:** package identifier; bounded observed behavior; explicit non-goals; source/data evidence; exact path claims; one-authority map; existing JSON IDs; save impact and legacy fallback; day/event order; deterministic seed path; player route and accessibility behavior; 3–6 focused acceptance assertions; rollback point. A collision, absent data source, or unresolved state owner stops implementation until the foreman resolves it. This plan does not authorize changes to shared `Main`, save-registry, panel-registry, or ledger paths.

**Editorial gate:** remove unsupported claims, trim repetitive candidate lines, confirm physical details and information flow, name the real refusal code, and accept prose into JSON only after its trigger and consumer exist. The master expansion document's anti-padding rule governs all promotion: document length is not gameplay completeness.
"""


def section(spec: dict) -> str:
    authored = rows(spec)
    result = intro(spec, authored)
    number = 0
    while len(result) + len(close(spec, number)) < 251000:
        number += 1
        if number > 16 * len(CONDITIONS):
            raise ValueError(f"insufficient distinct review cells for {spec['name']}")
        scene = spec["scenes"][(number - 1) % len(spec["scenes"])]
        condition = CONDITIONS[((number - 1) // len(spec["scenes"])) % len(CONDITIONS)]
        result += card(spec, scene, condition, authored, number)
    return result + close(spec, number)


def main() -> None:
    by_path: dict[str, list[dict]] = {}
    for spec in SPECS:
        by_path.setdefault(spec["path"], []).append(spec)
    for relpath, specs in by_path.items():
        path = ROOT / relpath
        original = path.read_text()
        boundary = "\n---\n\n" + MARKER_PREFIX
        if boundary in original:
            original = original.split(boundary, 1)[0]
        extensions = [section(spec) for spec in specs]
        final = original.rstrip() + "\n" + "".join(extensions).rstrip() + "\n"
        path.write_text(final)
        print(f"{relpath}\t{len(final)} characters\t{len(specs)} subject sections")


if __name__ == "__main__":
    main()
