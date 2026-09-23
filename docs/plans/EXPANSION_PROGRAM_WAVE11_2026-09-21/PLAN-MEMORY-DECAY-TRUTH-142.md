# PLAN-MEMORY-DECAY-TRUTH-142 — Personal Recall Fidelity, Fade Rules & Protected Facts

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-BACKSTORY-REVEAL-TRUTH-126, PLAN-DETERMINISM-CROSS-HOST-89.
**Implementation scaffold:** [`PLAN-MEMORY-DECAY-TRUTH-142_APPENDIX-A_SCAFFOLD.md`](PLAN-MEMORY-DECAY-TRUTH-142_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-BACKSTORY-REVEAL-TRUTH-126` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no rumor system (Plan 120 owns external propagation), no backstory
revelation (Plan 126), no psychological model rewrite (Plan 64).

## 1. Outcome
`Cognition/MemoryDecaySystem.cs` is the only file in its directory and has no
plan coverage. Personal memory decay is dangerous ground: done carelessly it
either erases save-referenced facts or becomes a hidden difficulty lever. Done
right it is a documented retention model over facts that other systems own.

| Deliverable | Detail |
|---|---|
| Retention classes | fact classes with fade rates (recent, rehearsed, traumatic, trivial) — day-based, deterministic |
| Protected facts | milestones, save-referenced ids, and authored anchors never fade; a protected-set test proves it |
| Recall fidelity | recall returns the stored fact, a degraded summary flag, or nothing — never a different fact |
| No erasure of authority | decay marks recall quality; it never deletes records other owners depend on |
| Determinism | fade decisions use game days + a registered stream where variance exists; no wall clock |

## 2. Evidence
- `Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs` (the whole directory; verified).
- Plan 126 owns revelation of hidden facts; this plan owns retention after revelation.
- Plan 120 owns rumor rows; personal decay is a different record family.
- Plan 64 owns psychological state this system may inform.

## 3. Packages
- **MDY-142A** retention class + fade rate table.
- **MDY-142B** protected-fact set + tests (milestones, save-referenced ids).
- **MDY-142C** recall fidelity tests (exact / degraded / absent).
- **MDY-142D** no-erasure guard: records referenced by other owners remain intact.
- **MDY-142E** day-based determinism test (OS clock change no-op).

## 4. Acceptance & verification
- Protected facts never degrade in a scripted year; unprotected classes degrade per table.
- Recall never returns a different fact; the degraded flag is explicit.
- Reference integrity holds after decay (Plan 34's checks still pass in the fixture).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Cognition/` (create if absent).

## 5. Risks
Silent difficulty lever → fade rates are data rows surfaced in Plan 73's registry; no hidden tuning.
Fact erasure → protected-set + no-erasure guards are permanent tests.

---

## 6. Expanded census (1 files · 536 lines)

Scope: `Assets/Ashfall.Core/Cognition/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MemoryDecaySystem.cs` | 536 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `standing_record_memory.json` | array[52] |
| `npc_memory_dialogue.json` | object[2 keys] |
| `memory_decay_rates.json` | object[3 keys] |

**State surfaces:** `MemoryDecaySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Cognition/` |
| Test references | 2 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MDY-142A` | no name match — resolve at claim time |
| `MDY-142B` | no name match — resolve at claim time |
| `MDY-142C` | no name match — resolve at claim time |
| `MDY-142D` | no name match — resolve at claim time |
| `MDY-142E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Main.Plans162_185.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs`, `Ashfall.Core.Tests/Cognition/Plan185MemoryDecayIntegrationTests.cs`, `Ashfall.Core.Tests/Narrative/Plan147_151NpcAnimalIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `phantom_memory` |
| `relationship_decay` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--relationship-decay-selftest` |
| `--standing-record-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnCaregivingDialogueUnlocked` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnStandingCalled` | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` |
| `OnStandingPenalty` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (94 files, 658 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Factions` | 10 | 72 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `PlayerCommand` | 1 | 1 |
| `Quests` | 4 | 25 |
| `Radio` | 47 | 354 |

**Verdict:** 658 cases sit under matching regions — run those first (`Audio`, `Combat`, `Factions`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **190**
(23 of them panels/HUD).

| Host file |
|---|
| `src/Host/CaregivingHostSession.cs` |
| `src/Host/CaregivingSaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |
| `src/Host/HostCli.PlansB86_B89.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **18**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caregiving` | no |
| `collectible_discovery` | no |
| `combat` | no |
| `counter_intelligence` | no |
| `crossing` | no |
| `dynamic_quests` | no |
| `encounters` | no |
| `faction_espionage` | no |
| `factions` | no |
| `oral_lore` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `black_market_debt_event` |
| `breach_obstacle_secondary_effect` |
| `combat` |
| `companion_animal` |
| `radio` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **96**
(CODEX_ONLY 17, GAMEPLAY_CONSUMED 60, OPTIONAL 2, UNRESOLVED 17).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 17 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **7**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_broken_compact_locked` |
| `flag_branch_iron_way_locked` |
| `flag_branch_listener_locked` |
| `flag_branch_mercy_road_locked` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 18 (laddered 0) · RNG streams 5 · host files 24 · catalogs 22 · test regions 8 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MEMORY-DECAY-TRUTH-142
wave: 11
status: PROPOSED — foreman claim required
packages: MDY-142A, MDY-142B, MDY-142C, MDY-142D, MDY-142E
claim paths:
  - src/Host/CaregivingHostSession.cs  # §19 candidate host surface
  - src/Host/CaregivingSaveStore.cs  # §19 candidate host surface
  - src/Host/CollectibleEffectDispatcher.cs  # §19 candidate host surface
  - src/Host/ContentUtilizationRuntimeCollector.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/crossing_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/crossing_factions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --plans-122-125-balance-soak
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
