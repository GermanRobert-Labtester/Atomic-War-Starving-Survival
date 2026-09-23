# PLAN-PNEUMATIC-DISPATCH-TRUTH-180 — Tube Network, Capsule Routing & Jams

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FLUID-LOGISTICS-TRUTH-179, PLAN-INDUSTRY-AUTOMATION-45, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-PNEUMATIC-DISPATCH-TRUTH-180_APPENDIX-A_SCAFFOLD.md`](PLAN-PNEUMATIC-DISPATCH-TRUTH-180_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-FLUID-LOGISTICS-TRUTH-179` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no fluid network (Plan 179), no industry engines (Plan 45), no
new tube catalog.

## 1. Outcome
`Shelter/PneumaticDispatchSystem.cs` (**628 lines**) is reachable and
unaddressed: a physical message/item transport network inside the shelter.
Pneumatics are a logistics shortcut with real constraints (station capacity,
tube condition, jams) — none of which are stated, so the system is either free
instant delivery or inert.

| Deliverable | Detail |
|---|---|
| Network model | stations and tube segments from built state; dispatch endpoints named |
| Capsule routing | payloads travel with a documented transit time per segment length; no instant delivery |
| Capacity | station throughput per hour; queued capsules wait visibly |
| Jams/breaks | condition per segment; a failed segment jams its queued capsules with a discoverable state and a repair path |
| Save truth | in-transit capsules restore with position/time; a load never teleports or duplicates a payload |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs` (628 lines; unaddressed — Wave 13 audit).
- Plan 179's network model is the structural sibling; this plan reuses its condition contract rather than inventing one.
- Plan 119 supplies segment decay.
- Plan 93 verifies item payload conservation.

## 3. Packages
- **PDT-180A** network model + station/segment table.
- **PDT-180B** transit-time tests per segment class.
- **PDT-180C** throughput/queue tests at capacity.
- **PDT-180D** jam path + repair consumption + conservation check.
- **PDT-180E** in-transit save round-trip (position/time exact).

## 4. Acceptance & verification
- Transit time scales with length; no payload arrives early.
- A jammed segment holds its capsules and blocks new ones until repaired.
- Save/load preserves in-transit exactness; payload counts balance.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Free logistics → transit time, capacity, and jams are the three constraints.
Payload duplication → conservation wrapper + in-transit restore test.

---

## 6. Expanded census (1 files · 628 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PneumaticDispatchSystem.cs` | 628 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `pneumatic_network_catalog.json` | object[5 keys] |
| `pneumatic_carrier_capsule_logs.json` | array[8] |
| `pneumatic_cylinder_leather_assays.json` | array[7] |
| `pneumatic_tube_diverter_audits.json` | array[8] |

**State surfaces:** `PneumaticDispatchSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 1 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ENERGY-NUCLEAR-48` | 1 |
| `PLAN-FLUID-LOGISTICS-TRUTH-179` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PDT-180A` | no name match — resolve at claim time |
| `PDT-180B` | no name match — resolve at claim time |
| `PDT-180C` | no name match — resolve at claim time |
| `PDT-180D` | no name match — resolve at claim time |
| `PDT-180E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 6. Host files: **1** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/Plans74To77HostSessions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Plans74To77SystemsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `piezometer_network` |
| `pneumatic_dispatch` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--rumor-network-selftest` |
| `--time-capsule-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnCarrierHeard` | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/narrative/ammonia_chiller_leak_logs.json` |
| `Assets/StreamingAssets/Data/narrative/apiculture_red_light_audits.json` |
| `Assets/StreamingAssets/Data/narrative/architect_vault_audits.json` |
| `Assets/StreamingAssets/Data/narrative/armored_cockroach_hive_logs.json` |
| `Assets/StreamingAssets/Data/narrative/artesian_well_contamination_logs.json` |
| `Assets/StreamingAssets/Data/narrative/bark_tanning_vat_logs.json` |
| `Assets/StreamingAssets/Data/narrative/beeswax_rendering_dipping_assays.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/boiler_feedwater_deaerator_audits.json` |
| `Assets/StreamingAssets/Data/narrative/bone_degreasing_prep_logs.json` |
| `Assets/StreamingAssets/Data/narrative/brewers_yeast_krausen_audits.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (54 files, 302 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |

**Verdict:** 302 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Holdfast`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **58**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Host/HoldfastDispatchLog.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |
| `src/Host/HostCli.PlansB86_B89.cs` |
| `src/Host/Plans130To133HostSessions.cs` |
| `src/Host/Plans74To77HostSessions.cs` |
| `src/Host/RumorNetworkHostSession.cs` |
| `src/Host/RumorNetworkSaveStore.cs` |
| `src/Host/RumorNetworkSelfTest.cs` |
| `src/Host/TimeCapsuleHostSession.cs` |
| `src/Host/TimeCapsuleSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **9**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `armored_crawlers` | no |
| `caravan_trade_network` | no |
| `cryo_vault` | no |
| `deep_well` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `piezometer_network` | no |
| `pneumatic_dispatch` | no |
| `time_capsules` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **117**
(CODEX_ONLY 107, GAMEPLAY_CONSUMED 6, OPTIONAL 1, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `armored_crawler_modules.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |
| `holdfast_npcs.json` | UNRESOLVED |
| `holdfast_quests.json` | GAMEPLAY_CONSUMED |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 9 (laddered 1) · RNG streams 0 · host files 12 · catalogs 22 · test regions 4 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PNEUMATIC-DISPATCH-TRUTH-180
wave: 14
status: PROPOSED — foreman claim required
packages: PDT-180A, PDT-180B, PDT-180C, PDT-180D, PDT-180E
claim paths:
  - src/Host/HoldfastDispatchLog.cs  # §19 candidate host surface
  - src/Host/HostCli.Plans122to125.cs  # §19 candidate host surface
  - src/Host/HostCli.Plans139_141.cs  # §19 candidate host surface
  - src/Host/HostCli.Plans162_165.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/audio_logs_expansion_05.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/ammonia_chiller_leak_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --plans-122-125-balance-soak
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
