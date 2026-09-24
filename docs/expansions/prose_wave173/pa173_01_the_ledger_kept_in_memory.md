# Subject Plan PA173-01 — The Ledger Kept in Memory

**Lane A · Cluster C11 · PROPOSAL.** This is a content subject plan, not an implementation plan. The twenty Lane A candidates form a serial queue across separate future waves; the factory permits at most one plan per lane in any one wave.

## Subject

Plan a voice-focused expansion pass around Mara’s remembered inventory—medicine, axle parts, and a lost crossing—and the exactness of her prices. Let the ledger’s absent page remain a human pressure, not a hidden economic mechanic. Use the current record and its existing authored fields. This proposal approves no copy, new canon, data mutation, or player-facing behavior.

## Premise evidence

- **VERIFIED:** `enc_arc_mara_waystation` exists in `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json`. Current `description` evidence: “Two wagons stand in the lee of the water station, tarped against the ash. The factor works through her ledger by memory: medicine, axle parts, the crossing she lost. Her prices are fast and exact, and she mentions once, without emphasis, that she pays favors back with interest.”.
- **VERIFIED:** the compiled v2.0 authority names Lane A Cluster C11 as the NPC encounter / trade register opening and requires Template S/R, current-premise verification, continuity review, and explicit route/verification notes.
- **HIGH CONFIDENCE:** the exact selector is absent from prior prose-wave indexes. That proves anchor freshness only; it does not prove a prose gap or live consumption.
- **UNKNOWN:** The downstream quest choices and any trade-system consumer must be checked before changing the description.

## Why this and not something else

A-11/C11 ledger and statement prose; the live encounter supplies a distinct named trader, goods, location, and choice language. This candidate uses an existing text-bearing record and can be reviewed without inventing a system. The related register and cluster are directly supported by the current source; claims about missing content or runtime reachability remain unverified.

## What must not change

Preserve npcId, choice IDs, deltas, quest links, encounter weights, and all quantities. Do not add prices, stock, favor debt, or a new trade state. Preserve every id, date, number, item, choice, flag, attribution, branch, and uncertainty not explicitly proposed for prose review. No new mechanic, route, save state, outcome, balance rule, or quest gate. Any new line remains DRAFT until canon review. **Epilogue permutations:** none are opened or changed by this proposal.

## Recommended integration route (Template R)

**Tier:** DATA-ONLY, conditional on the current schema permitting the prose field and the existing consumer presenting it. **Seams:** `narrative_encounters_npc_arcs.json` → existing schema/catalog-integrity validation → verified loader → existing record presenter. **Save impact:** NONE for static text. **Determinism impact:** NONE. **Verification class:** focused data-integrity validation plus content-utilization proof; run the owning narrative contract or headless test only if the eventual change touches its runtime route. **Ownership:** no files are claimed here; recheck `INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md` and claim the exact data path before any implementation. **Route note:** do not add a parallel catalog or new Core authority. If the field has no live consumer, stop and reclassify as DOCS-ONLY or close as stale.

## Continuity checklist result

The selector is new relative to earlier prose-wave anchors, but shared-domain duplication is still an open check. At promotion: inspect the full source file and neighboring records; compare linked IDs and cross-catalog facts; search `docs/` and `UNCLAIMED_CORPUS_CENSUS.md` for an existing owner or higher-priority unclaimed row; verify voice, date/timeline, attribution, choices, and epilogue boundaries; confirm the target field and consumer. Distress-signal scenarios remain SEALED and untouched. No new fact is introduced by this plan.

## Verification class

For a later data edit: run the current catalog integrity gate and content-utilization check, inspect the JSON diff and identifier collision result, and use focused narrative tests if the relevant consumer has them. Do not run the full test suite for prose-only changes. Runtime and save/determinism tests are unnecessary unless scope changes beyond static prose; any such scope change needs a fresh plan.

## Open premises

The downstream quest choices and any trade-system consumer must be checked before changing the description. Also verify the exact field contract, loader, and presentation route during integration. **Facts used:** the live record above and the v2.0 Lane A archetype. **New canon introduced:** none. **Risk:** unsupported interpretation, duplicate prose, or an orphaned text field.
