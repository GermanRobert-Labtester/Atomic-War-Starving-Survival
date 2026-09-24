# Items 21–30 integration-plan closeout

**Date:** 2026-09-24  
**Scope:** ten user-requested subject expansions, planning and editorial work only.  
**Expansion source:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`, checked against current Core, Godot host, JSON, and governance files.  
**Status:** integration architecture and polishing complete at the document level. Production integration requires separately claimed packages and focused verification.

## Expanded documents

Four subjects share the generated orphan-authority dossier. Each has its own marked section above 250,000 characters there; the shared file is not counted as four unrelated source files. Historical text above the addenda is retained as dated evidence.

| Item | Subject | Expanded source and section | Integration classification |
|---:|---|---|---|
| 21 | NVIS C4I | [Core-only registry](EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-CORE-ONLY-REGISTRY-11.md) | Constructor-only subclass of a hosted NVIS base; classify before wiring. |
| 22 | Palliative Care Dignity | [orphan dossier](EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md#2026-09-24-integration-expansion--palliative-care-dignity-engine) | Static engine mutates a patient record; medical owner and memorial adapter need decisions. |
| 23 | Perimeter Early Warning | [perimeter plan](EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-PERIMETER-DEFENSE-TRUTH-165.md) | Radar contact state should extend the live perimeter/defense seam. |
| 24 | Pharmaceutical Tablet | [health-history scaffold](EXPANSION_PROGRAM_WAVE15_2026-09-21/PLAN-HEALTH-HISTORY-TRUTH-196_APPENDIX-A_SCAFFOLD.md) | Press, batch, inventory ports, and full-state save need a medical production host. |
| 25 | Prosthetic Condition Wear | [orphan dossier](EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md#2026-09-24-integration-expansion--prosthetic-condition-wear-engine) | Already used by a Core body projection; distinguish preview from daily item-condition write. |
| 26 | Railway Interlock | [Iron Road plan](../expansions/wave3/expansion_25_the_iron_road_plan.md) | Junction legality/reservation must guard the live rail dispatch path. |
| 27 | Rehabilitation Progression | [orphan dossier](EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md#2026-09-24-integration-expansion--rehabilitation-progression-engine) | Pure transformer with Core projection; write `RehabRecord` through the body owner once per day. |
| 28 | Restock Allocation | [orphan dossier](EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md#2026-09-24-integration-expansion--restock-allocation-engine) | Pure allocator; only attach if the current caravan contract has capacity, preserving DEC-05 order. |
| 29 | Surgical Graft Rejection | [surgical ward plan](EXPANSION_PROGRAM_WAVE16_2026-09-21/PLAN-SURGICAL-WARD-TRUTH-213.md) | Stateful graft records require a ward-approved procedure, seeded roll, day semantics, and chosen medical save owner. |
| 30 | Trophy System | [Bone Shop plan](../expansions/wave10/expansion_59_the_bone_shop_plan.md) | Exactly-once preserved-quarry award feeds recipe unlock; crafting/decor own item and display. |

## Finished integration architecture

```text
Authoritative JSON + canonical campaign facts
    -> existing engine-free Core owner or pure evaluator
    -> one current host/command seam, if a command is needed
    -> one existing or explicitly approved state capture path
    -> current Godot panel, journal, or briefing read model
```

The architecture is complete as a **handoff decision tree**. Direct class-name host counts are interpreted with indirect callers: `NvisC4ISystem` is a constructor-only subclass of the hosted `NvisCommunicationsSystem`; prosthetic wear and rehabilitation already feed Core presentation projections. Those three should not gain duplicate hosts just to satisfy a reachability counter. The static palliative and restock engines need an applying owner but no private host-state ledger. The stateful radar, tablet press, interlock, graft, and trophy owners need one approved persistence composition before being used for live effects.

| Concern | Existing authority | First bounded implementation route | Save position |
|---|---|---|---|
| NVIS | `NvisCommunicationsSystem` and `NvisCommunicationsHostSession` | Classify subclass, then use current status/recall route if behavior is missing. | `nvis_communications` already registered. |
| Palliative care | Medical pipeline, ward, survivor fate, memorial owners | One patient care day; death/memorial adapter follows only after owner mapping. | Embed patient record in an approved medical owner; no independent dignity ledger. |
| Radar | `PerimeterDefenseSystem`, `DefenseHostSession`, Night Watch | One seeded sweep from a real approach observation. | Audit `PerimeterDefenseSave`; avoid another wall store. |
| Tablets | Inventory and clinical pipeline with `PharmaceuticalTabletEngine` | One current formulation from input issue to one claimed output. | `TabletWorksFullState` required; select medical production owner. |
| Prosthetic wear | Survivor body/item condition owner | Truthful current vs projected condition, then one authorized daily write. | No evaluator section; persist item/body condition. |
| Railway interlock | `RailwaySystem` topology/dispatch and `RailTrackMaintenanceLedger` | One junction request/denial/reservation/release guarding dispatch. | Compose interlock state with rail lifecycle after save-shape audit. |
| Rehabilitation | `SurvivorBodyState.RehabRecord` | One fitted limb advances one campaign day. | Existing body-state save, pending serialization check. |
| Restock | `ShelterBarterSystem.RestockCaravan` | Capacity-limited allocation only if capacity is a live merchant input. | Existing caravan stock; no allocator section. |
| Graft rejection | Medical ward and pipeline | One ward-approved graft with actual medicine and seeded day roll. | Choose `medical_ward` or `medical_pipeline` after owner audit. |
| Trophies | Wildlife/trapping, crafting, shelter decor | Preserved quarry → one recipe opportunity → separate craft/display. | Persist `TrophySaveState` through one approved award owner. |

## Evidence corrections and promotion gates

1. The NVIS base is already composed in `Main.Plans130_133.cs` and saved as `nvis_communications`. The C4I subclass adds only constructor delegation in current source.
2. The pharmaceutical scaffold's “no name-matched catalog” premise is stale: `tablet_manufacturing_catalog.json` has three formulations and three release classes. Default inventory delegates are no-op/zero, so host binding is a precondition to live production. Save the full press/batch/output state, not just press state.
3. `SurvivorBodyPresentationSlate` invokes wear evaluation with zero labor and full maintenance and displays projected net condition. A read-only panel opening must not become a wear tick.
4. The rehab engine's `RehabRecord` is part of `SurvivorBodyState`, and `RehabilitationSlateProjection` already reads it. The missing write route is a once-per-day body-state transition, subject to save verification.
5. `ShelterBarterSystem.RestockCaravan` already owns stock; DEC-05 sealed merchant display order. The allocator cannot create a second stock ledger or silently reorder the shop.
6. `SurgicalGraftRejectionEngine.ProcessDailyTick` permits a null roll callback, which omits rejection checks. Its multi-day catch-up loop passes the same `currentDay` to each callback invocation. A host must choose a seeded per-day contract before rollout.
7. `TrophySystem` awards a crafting opportunity, not a finished physical item. Item creation and localized morale remain with crafting and decor.

## Regeneration and polishing record

The addenda are generated by `docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/generators/gen_requested_ten_integration_expansions_21_30.py`; the separate `polish_requested_ten_integration_expansions_21_30.py` inserts subject-specific source reviews and corrects generated grammar. Run the owning base generator first when regenerating a scaffold or orphan inventory, then run these two scripts in order. The marked addenda preserve original dated material and keep proposed prose outside runtime JSON. Each subject section includes a phased integration package, authority/API checklist, C# contract sketch, candidate case register, and final handoff. The case register is a review pool, not approved game canon or a demand to implement every permutation.

**Static checks:** the ten individual subject addenda measure 252,712–255,709 characters each. Each marker appears once, Markdown code fences are balanced, all ten closeout links resolve, and scoped `git diff --check` reports no whitespace errors. Re-running the polish script, and re-running the generator-plus-polish chain, produced identical document hashes. No runtime tests were run for this documentation-only package.

## Remaining implementation decisions

- Confirm exact save composition for patient care, radar, tablet press, interlock, graft records, and trophy awards before any stateful host action becomes live.
- Identify a real radar approach producer, a real preserved-quarry producer, and the dispatch command that can enforce interlock reservations. Absent producers must be reported as premise gaps.
- Verify whether caravan capacity exists today. If absent, classify the restock allocator as optional pure math; do not change DEC-05 to justify it.
- Resolve one-limb versus multi-limb `RehabRecord` identity before promising simultaneous rehab arcs.
- Keep each promotion to one first bounded outcome with exact path claims, source signatures, current IDs, save migration, and focused verification under `TEST_POLICY.md`.

No production source, JSON catalog, or runtime test was changed by this documentation package. The closeout completes the integration **planning** architecture; runtime completion is a later claimed package.
