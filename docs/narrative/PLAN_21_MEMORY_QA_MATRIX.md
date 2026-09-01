# Plan 21 — Memory & Heirloom QA Matrix

This document defines the Quality Assurance and Verification matrix for **Plan 21: Phantom Memory & Heirloom World Layer**.

---

## 1. Test Suite Architecture

| Test Suite File | Responsibility | Test Cases Covered |
|---|---|---|
| `PhantomMemoryEngineTests.cs` | Trigger matching, taxonomy parsing, affinity amplification, lore-only resolution, one-shot idempotence, and deterministic serialization. | 15+ cases |
| `HeirloomSystemTests.cs` | Instance generation, 3-stage provenance logging, bounded history truncation, survivor-death succession via lineage/relationships, memorial tags, and roundtrip serialization. | 12+ cases |
| `ConfessionSecretSystemTests.cs` | Secret discovery, moral leverage resolution (Expose, Blackmail, Keep), interpersonal grudge vs forgiveness mechanics, idempotence guards, and roundtrip serialization. | 12+ cases |

---

## 2. Verification Gates & Pass Criteria

| Gate | Command | Expected Output | Status |
|---|---|---|---|
| **Core Unit Tests** | `dotnet test Ashfall.Core.Tests` | 0 failed, all new test suites passing | PASS |
| **Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | 0 findings, 0 errors across 151 catalogs | PASS |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS, 0 new orphans | PASS |
| **Scene Bindings** | `godot --headless --path . -- --scene-binding-selftest` | 22/22 passed | PASS |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | 26 scenes checked, 0 errors | PASS |

---

## 3. Detailed QA Test Matrix

### A. Phantom Memory Engine QA

| Test ID | Objective | Input / Condition | Expected Result |
|---|---|---|---|
| `QA-PHANTOM-01` | Direct Item ID Matching | Item ID `childs_mitten` with background `child_refugee` | Matches `phantom_trigger_p4_child_mitten` over generic `childhood` fallback. |
| `QA-PHANTOM-02` | Category Inference Fallback | Unmapped item `rusty_caliper` inferred as `work_tool` | Fallback matches `work_tool` trigger rule for `machinist`. |
| `QA-PHANTOM-03` | Background Affinity Amplification | Trigger with `affinity_background` matching survivor background | Morale payload scaled by 1.5×, trigger probability scaled by 1.25×. |
| `QA-PHANTOM-04` | Lore-Only Resolution | Trigger with `lore_only: true` evaluated | Generates descriptive memory without triggering motivation or breakdown rolls. |
| `QA-PHANTOM-05` | One-Shot Idempotence | Repeat item inspection for non-repeatable trigger | First inspection fires; second returns empty, preventing duplicate memory spam. |
| `QA-PHANTOM-06` | Deterministic Seed Replay | Same seed + identical inventory sequence | Generates identical trigger outcomes and motivation rolls. |
| `QA-PHANTOM-07` | Save/Restore State | Engine state captured and restored | `seenTriggerIds` and `processedItemIds` preserved byte-for-byte. |

### B. Heirloom System QA

| Test ID | Objective | Input / Condition | Expected Result |
|---|---|---|---|
| `QA-HEIRLOOM-01` | Catalog Loading & Lookup | Query `heirloom_grandfathers_dosimeter` | Returns 3 stages, correct title, base item `dosimeter`, and holder memories. |
| `QA-HEIRLOOM-02` | Instance Creation & Assignment | Assign heirloom to survivor `survivor_01` | Initializes instance with current holder ID, active stage 1, and creates provenance entry. |
| `QA-HEIRLOOM-03` | Kin Succession on Death | Holder dies; kin `survivor_child` exists in `GenerationalLineageExtension` | Automatically transfers heirloom to kin, unlocks stage 3, and appends `death_inheritance` provenance record. |
| `QA-HEIRLOOM-04` | Bond Succession Fallback | Holder dies; no kin, but high trust bond in `SurvivorRelationsSystem` exists | Transfers heirloom to highest affinity living ally. |
| `QA-HEIRLOOM-05` | Communal Storage Fallback | Holder dies; no living kin or allies | Resets `current_holder_id = ""` to store in shelter communal stash. |
| `QA-HEIRLOOM-06` | Provenance Bounded Cap | Heirloom passed through 30 transfers | Provenance list capped at 24 entries without memory leak or unbounded growth. |
| `QA-HEIRLOOM-07` | Holder Memory Affinity | Survivor holding `heirloom_mothers_recipe_tin` with `cook` profession | Morale boost applied (+14.0) matching `affinity_key: "cook"`. |
| `QA-HEIRLOOM-08` | Save/Restore State | Heirloom system state captured and restored | All active instances, provenance records, and legacy markers restored. |

### C. Confession & Secret System QA

| Test ID | Objective | Input / Condition | Expected Result |
|---|---|---|---|
| `QA-SECRET-01` | Discovery Registration | Discover `secret_surgeon_lost_patient` via `silver_scalpel` | Marks secret as discovered; logs discovery day and source. |
| `QA-SECRET-02` | Expose Action Leverage | Expose `secret_soldier_civilian_order` | Faction standing modified (+15 with rebels), guilt added (+15 to soldier), secret marked resolved as `exposed`. |
| `QA-SECRET-03` | Blackmail Action Leverage | Blackmail `secret_pharmacist_stolen_morphine` | Supplies awarded, hardening increased (+0.15), secret marked resolved as `blackmailed`. |
| `QA-SECRET-04` | Keep Action Leverage | Keep `secret_mother_child_left` in confidence | Survivor trust increased (+35), secret marked resolved as `kept`. |
| `QA-SECRET-05` | Interpersonal Forgiveness | Resolve interpersonal secret with forgiveness | Target affinity (+20) and morale (+15) increased; forgiveness text returned. |
| `QA-SECRET-06` | Interpersonal Grudge | Resolve interpersonal secret with grudge | Target affinity (-40) and morale (-20) decreased; grudge text returned. |
| `QA-SECRET-07` | Idempotent Leverage Resolution | Attempting to Expose an already Blackmailed secret | Operation rejected as invalid; state remains stable. |
| `QA-SECRET-08` | Save/Restore State | Confession secret system state captured and restored | Discovered secrets, resolution states, and leverage timestamps preserved. |

---

## 4. Edge Case & Failure Mode Matrix

| Edge Case Scenario | System Handling | Safety Verification |
|---|---|---|
| Missing item definition for trigger | `PhantomMemoryEngine` falls back to category matching; validator flags in static scan. | No null reference; logged as diagnostic warning. |
| Deceased holder with 0 surviving members | `HeirloomSystem` stores heirloom in communal storage with empty holder ID. | No index out of bounds; safe fallback. |
| Blackmail executed on absent/dead NPC | System checks survivor existence before applying relation/guilt deltas. | Gracefully applies bunker resource gains without crash. |
| Multi-stage heirloom fast-forwarded | Stage index clamped between 1 and `Stages.Count`. | Preserves historical narrative integrity. |
| Cyclic relationship graph on succession | Sorts relationship ties by highest trust, then by stable survivor ID comparison. | Deterministic succession without infinite loops. |
