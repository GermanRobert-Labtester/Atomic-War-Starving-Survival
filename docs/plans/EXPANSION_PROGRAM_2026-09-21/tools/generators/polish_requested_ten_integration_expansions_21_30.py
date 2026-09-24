#!/usr/bin/env python3
"""Distinct source-specific editorial phase for requested integration items 21–30."""

from __future__ import annotations

from pathlib import Path

from gen_requested_ten_integration_expansions_21_30 import ROOT, SPECS, MARKER_PREFIX

START = "## Editorial source review and first-package handoff"
END = "## 4. Dependency-ordered implementation package"

REVIEWS = [
    """### Resolve the subclass finding before wiring

The dated registry classifies `NvisC4ISystem` as a dead type. Current source shows that it is a constructor-only subclass of `NvisCommunicationsSystem`; the base is built in `Main.Plans130_133.cs` with a campaign radio RNG fork, power-grid provider, catalog loader, save restore, host session, recall journal callback, and day tick. A direct reference count of zero for the subclass is therefore a naming/retirement question, not a missing-radio proof. The first implementation packet should compare public behavior of the subclass and base, then either document the subclass as compatibility or retire it under the repository's authority-retirement protocol. It should not instantiate both.

If the player route needs more C4I behavior, describe the missing operation in terms of the base's existing `BeginStatusTransmission`, `RequestRecall`, and `AcknowledgeRecall` calls. Trace a real expedition ID and survivor ID into the recall queue and prove acknowledgment does not happen merely because the radio panel displayed a button. The power provider must still gate the transmission. Save under `nvis_communications`; restore must not emit another recall event. The two existing channel IDs are the first content cohort. Write an operator's log only after the transmission record exists; do not imply that a regional receiver heard a packet that Core rejected.

**First focused acceptance:** powered channel, one queued status transmission, one recall request, one acknowledgment, then a restore with no duplicate journal entry. If no additional behavior is needed, close the subclass orphan as a classification correction and leave the live base path alone.
""",
    """### Make the patient record's owner explicit

`PalliativeCareDignityEngine` is static, yet `AdvanceDailyCare` mutates `PalliativePatientRecord`: it reduces pain and lucidity according to protocol and medicine availability, adjusts dignity, decrements prognosis, and increments days in the current grief stage. A host cannot call this on a temporary read model or the care day vanishes on save. The plan's first owner decision is which existing medical record holds that patient state and how a terminal-care designation enters it. The ward may own bed and caregiver assignment; health history may retain a longitudinal note; survivor fate remains the only death transition. None should independently tick prognosis.

`EvaluateGriefStageProgression` uses `StableHash` with world seed, survivor ID, simulation tick, and current stage. Preserve that deterministic input if the host advances it. The named grief stages are mechanical states, not a claim that every dying person follows a fixed emotional sequence; authored bedside prose should be individual and non-diagnostic. `CalculateMemorialEcho` returns a candidate result after death. Apply it through the existing memorial/legacy owner once, keyed to the death fact, and never on a screen refresh. A high analgesia choice may lower lucidity in current Core, so the UI must state the real tradeoff without framing relief as a moral failure.

**First focused acceptance:** one patient enrolled, one care day with real medicine and caregiver inputs, save/reload equality, and no second prognosis decrement on panel reopen. The memorial adapter remains a later package until the death and legacy owner contract is verified.
""",
    """### Radar contacts must not become raids by themselves

The `PerimeterEarlyWarningEngine` has mode, calibration, active contacts, save/restore, and threat/false-alarm events. It reports power draw of 0, 50, 250, or 600 watts across Off, LowPowerStandby, ActiveScan, and HighFrequencySweep. `ProcessScanSweep` receives an explicit `seededRollPermille`, current tick, sector, distance, hostility, and storm/dust flag. The current perimeter host covers built defenses and the night-watch seam, while raid resolution remains separate. The first host adapter should subscribe to a real threat observation and supply the campaign-seeded roll; it must not synthesize a raid merely to make radar fire.

The save design must compare `PerimeterEarlyWarningSaveState` against `PerimeterDefenseSave` before embedding. If the latter cannot hold radar state without breaking migration or rollback, the foreman must choose an owned additive section; the plan does not authorize a second wall condition store. Contacts should expire by the radar's `ClearExpiredContacts` rule, while a durable journal notice is recorded once when a threat is first detected. The UI should distinguish hostile classification, low-confidence dust return, and actual breached perimeter. A false alarm is a reason to investigate, not proof of a hostile party.

**First focused acceptance:** one real approach within range, one storm-induced false return with fixed roll, one out-of-range no-op, and a restore that preserves mode/calibration/contact identity without republishing detection.
""",
    """### Bind the press before a tablet can exist

The 2026-09-21 scaffold says there is no name-matched catalog. Current `tablet_manufacturing_catalog.json` contains a press definition, three formulations, and three release classes; the engine exposes `BindCatalog` and `BindInventory`. The default inventory delegates return zero or no-op. A press constructed against those defaults would not represent a valid campaign transaction. The first package needs a medical-production host that binds the real inventory owner, a campaign-day provider, pharmaceutical and technician skills, and a campaign RNG stream before accepting `ConstructPress` or `StageBatch`.

Use `CaptureFullState` and `RestoreFullState`, because press-only `CaptureState` omits active batch and output buffer. `TickDay` advances manufacture; `ClaimOutputs` is the one inventory handoff after a resolved batch. The host should reconcile output capacity before claiming, and it must preserve a batch in the buffer if inventory cannot receive it. The medical pipeline administers a tablet to a patient later; manufacturing completion is not treatment. Prose can describe tooling wear, stamp quality, and a sealed batch only after those state fields actually exist in the result. An empty room or missing chemist may affect quality only through the current provider contract.

**First focused acceptance:** construct, stage one current formulation, tick to resolution, claim once, save/reload before and after claim, and verify all input/output item deltas in canonical inventory.
""",
    """### Distinguish condition from a one-day projection

The orphan label is misleading here: `SurvivorBodyPresentationSlate` calls `ProstheticConditionWearEngine.EvaluateDailyWear` for both upper and lower limbs. Its inputs use zero labor intensity and full maintenance, then it displays `NetConditionPermille` as “condition.” That is a projected post-wear value, not necessarily the item owner's current condition. The first package should decide whether the slate means to show current condition or expected next-day condition, label it accordingly, and preserve the separate maintenance warning. It should not subtract the same daily wear again merely because a screen opens.

The engine itself is a pure evaluator. The canonical item/body owner must be located before applying daily wear, with exactly one write per campaign day per prosthetic item. Labor intensity comes from the day's actual duty or expedition facts; maintenance quality comes from a real action or facility, not a UI default. A broken prosthetic may reduce grip or mobility through the existing body projection, but the engine must not generate a new injury or inventory item. If the current host already performs an equivalent condition tick, reuse it and close the exact-name orphan as indirectly consumed.

**First focused acceptance:** compare current condition with projected net condition, reopen the slate without mutation, apply one authorized day update, and restore the same result without a second decrement.
""",
    """### Make interlock legality part of dispatch

The Iron Road design already distinguishes `RailwaySystem` topology and dispatch, `RailTrackMaintenanceLedger` wear/load feasibility, and `RailwayInterlockEngine` switch locks, signal aspects, reservations, obstruction, and tamper. The rail maintenance sibling is hosted; that does not imply the interlock is. `railway_interlock_catalog.json` currently defines three junctions and two maintenance profiles. First promotion should map those junction IDs to real rail-network segments and determine whether the existing dispatch command can ask `RequestRoute` before moving a train. `IsExpeditionPathLegal` can be a preflight but cannot be a panel-only warning that dispatch ignores.

The host sequence is: obtain canonical path and expedition ID; check maintenance feasibility; inspect and reserve each required junction in a stable order; commit dispatch; release reservations on completion, cancellation, or failed dispatch. The rollback policy must specify what happens if one of several junction reservations fails. A signal denial should produce a visible route reason and no vehicle movement. Save/restore must preserve an active reservation tied to the same expedition ID; it cannot recreate free track from a locked route. The interlock may own junction state, but it must not clone `RailwaySystem` topology or `rail_maintenance` wear.

**First focused acceptance:** one clear route, one denied route, cancellation release, and save/reload mid-reservation with identical legality. Route-tamper narrative comes only after a real tamper event.
""",
    """### Write the existing RehabRecord once per day

`RehabilitationProgressionEngine` is pure and already feeds `RehabilitationSlateProjection`; `SurvivorBodyState` contains the `RehabRecord` it transforms. `StartRehabilitation` begins fitting at 500 permille quality. `AdvanceDaily` advances through fitting, adaptation, and mastery with clamped resilience, and `GetQualityFactor` is a read helper. The missing production question is who calls `AdvanceDaily` and writes the returned record to the canonical body state. A new “rehabilitation save” would duplicate that record. The plan should verify that the body owner's existing save actually serializes `RehabRecord` before setting a day owner.

One fitted limb may need a unique association with its record; the current `RehabRecord` has a prosthetic type key, phase, days, and quality ramp, not an obvious per-limb event ID. Do not promise two simultaneous independent rehabilitation arcs until the body schema can distinguish them. The panel may show an estimate from `RehabilitationSlateProjection`, but it cannot move the phase. A daily transition should consume the same canonical survivor/day clock used by the medical and work systems, with a replay guard so a reload does not skip or repeat adaptation. Narrative milestones belong after the phase transition, not at first preview.

**First focused acceptance:** fitted limb enters fitting once, advances one day exactly once, survives reload, and displays the same phase and quality. Multi-limb behavior remains a schema decision.
""",
    """### Respect the sealed merchant-restock contract

`RestockAllocationEngine.Allocate` is deterministic math. It distributes a capacity across weighted categories, doubles weight below scarcity floor, apportions leftovers by largest remainder, then sorts items by stock/target-par ratio with authored order and ordinal ID tie breaks. It has no mutable state or save requirement. `ShelterBarterSystem.RestockCaravan` already changes caravan stock on arrival, and `Main.Plans147.cs` owns that day tick. DEC-05 sealed merchant restock display order. The first premise audit must compare the allocator's proposed ordering with that seal and ask whether the current merchant catalog actually supplies a capacity limit. If no such capacity exists, this engine can remain a pure optional tool without a fabricated host route.

If the capacity model is authorized, call the allocator inside the existing restock transition, apply its result to `ShelterBarterSystem` stock, and preserve the authored display order when presenting the merchant list. Do not let the allocator become a second stock ledger or alter a purchase settlement. A receipt can say that a shelf was short only if the resulting stock shows that shortage. The `merchant_caravans.json` four current rows are the ID census; no new caravan is needed for first acceptance.

**First focused acceptance:** a capacity-constrained restock with a scarcity floor, exact total allocated, deterministic tie break, saved caravan stock, and unchanged DEC-05 display order. If capacity is absent, close as a non-integrated pure helper by explicit decision.
""",
    """### Do not run clinical rejection with the optional roll omitted

`SurgicalGraftRejectionEngine` holds graft records and exposes integration, rejection, and rising-risk events. The existing medical ward and pipeline own procedure eligibility, bed, sterility, reservation, and patient record routing. A graft operation must go through that clinical path before `PerformGraft` adds a record. The engine's `ProcessDailyTick` receives an optional `seededRngRoll`; when the callback is null, rejection risk can rise but the rejection check never occurs. A production host must supply a deterministic callback rather than accept the default. Its event subscribers must be disposed with the medical session.

The catch-up loop processes all elapsed days but passes `currentDay` to the callback for each subday. A delayed multi-day tick could therefore reuse the same day-keyed roll. The implementation package must either tick strictly one campaign day at a time or revise the Core callback contract to include each elapsed day with a focused determinism fixture. Immunosuppressant administration consumes real medicine through inventory and belongs in one clinical command; calling `AdministerImmunosuppressant` alone is not proof that a dose was spent. Save the graft records in a chosen medical section, and restore without publishing integration or rejection events again.

**First focused acceptance:** one ward-approved graft, an actual dose, one deterministic day tick, status/risk projection, and a mid-course round trip. Delayed-day behavior is an explicit gate before broad rollout.
""",
    """### Preserve the difference between an award and an object

`TrophySystem` loads `trophies.json`, currently eleven rows, and records an exactly-once opportunity when `RecordQuarryPreserved` sees an eligible species. Its `TrophySaveState` stores awarded trophy IDs and unlocked recipe IDs. The live wildlife trapping and shelter decor hosts offer the two sides of the player loop, but there is no direct TrophySystem host call in the current search. The first adapter should listen to a real preserved-quarry fact, resolve the canonical species ID, ask the trophy system for the award, then expose the unlocked recipe in the existing crafting route. It must not put a finished mount into inventory at award time.

Only crafting can consume materials and create the physical trophy item. Only decor placement can grant localized morale through its current owner. Trophy prose should respect the Bone Shop plan's restraint rule: distinguish a hunter's record, a practical specimen, and an exploitative display without rewarding cruelty merely for a grisly label. A species sighting or kill alone is not the `quarry_harvested` condition. After reload, a previously awarded opportunity is visible but does not fire `OnTrophyReady` again. If a catalog row references an absent species, item, or recipe, the integrity pipeline should reject or isolate it before a player sees a dead affordance.

**First focused acceptance:** one legal preserved quarry, one recipe unlock, duplicate harvest no-op, restore with no duplicate event, then a separate craft and decor path through existing owners.
""",
]


def polish_section(section: str, review: str) -> str:
    if START in section:
        before, remainder = section.split(START, 1)
        _, after = remainder.split(END, 1)
        section = before + END + after
    section = section.replace("The a ", "The ").replace("The an ", "The ")
    section = section.replace("a a ", "a ").replace("a an ", "an ")
    section = section.replace("the the ", "the ")
    if section.count(END) != 1:
        raise ValueError("dependency package marker is not unique")
    return section.replace(END, START + "\n\n" + review.strip() + "\n\n" + END, 1)


def main() -> None:
    by_path: dict[str, list[tuple[dict, str]]] = {}
    for spec, review in zip(SPECS, REVIEWS, strict=True):
        by_path.setdefault(spec["path"], []).append((spec, review))
    for relpath, entries in by_path.items():
        path = ROOT / relpath
        content = path.read_text()
        for i, (spec, review) in enumerate(entries):
            marker = MARKER_PREFIX + spec["name"]
            start = content.index(marker)
            next_start = content.find("\n---\n\n" + MARKER_PREFIX, start + len(marker))
            end = next_start if next_start >= 0 else len(content)
            polished = polish_section(content[start:end], review)
            content = content[:start] + polished + content[end:]
        path.write_text(content)
        print(f"{relpath}\t{len(content)} characters\t{len(entries)} polished sections")


if __name__ == "__main__":
    main()
