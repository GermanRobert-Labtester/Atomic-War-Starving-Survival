# PLAN-HEIRLOOM-PHANTOM-TRUTH-149 — Keepsakes, Inheritance & Phantom Trigger Contract

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SECRETS-CONFESSION-TRUTH-127, PLAN-FAMILY-DYNASTY-43, PLAN-SAVE-GOVERNANCE-12.
**Implementation scaffold:** [`PLAN-HEIRLOOM-PHANTOM-TRUTH-149_APPENDIX-A_SCAFFOLD.md`](PLAN-HEIRLOOM-PHANTOM-TRUTH-149_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SECRETS-CONFESSION-TRUTH-127` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no secret lifecycle (Plan 127 owns it), no dynasty graph (Plan 43),
no new lore beyond existing catalogs.

## 1. Outcome
`Phantoms/` holds five files: `ConfessionSecretCatalog/System` (Plan 127's
scope), `HeirloomCatalog/System` (unclaimed), and `PhantomTriggerDto` (the wire
shape between them). Heirlooms are objects with memory — who held them, what
they witnessed, when they change hands — and phantom triggers are the events
that surface the past. Neither has a stated contract.

| Deliverable | Detail |
|---|---|
| Heirloom lifecycle | acquired → held → bequeathed/lost/destroyed with the inventory owner as the item authority |
| Provenance record | holder history and witnessed events stored per heirloom id; display reads the record, never invents text |
| Phantom triggers | `PhantomTriggerDto` fields documented; trigger evaluation is seeded and day-based, no wall clock |
| Inheritance | bequest rules at death, gated by Plan 43's family state where a relation exists |
| Save truth | heirloom provenance and pending triggers restore; a load never re-fires a consumed trigger |

## 2. Evidence
- `Assets/Ashfall.Core/Phantoms/`: `HeirloomCatalog.cs`, `HeirloomSystem.cs`, `PhantomTriggerDto.cs`, plus the confession pair owned by Plan 127 (verified).
- Plan 127 owns confession/secret lifecycle; the boundary is stated in both plans.
- Plan 43 owns family relations for bequest gating.
- Inventory remains the item authority (Plan 93 conservation applies to transfers).

## 3. Packages
- **HPT-149A** heirloom lifecycle + item-authority rule.
- **HPT-149B** provenance record + display-from-record test.
- **HPT-149C** phantom trigger fields + seeded, day-based evaluation tests.
- **HPT-149D** bequest path gated by Plan 43 state + conservation check.
- **HPT-149E** save round-trip; no trigger re-fire on load.

## 4. Acceptance & verification
- An heirloom's holder history matches the inventory owner's transfer log.
- Triggers fire once, deterministically; a reload does not re-fire a consumed trigger.
- Bequests respect family state; no item duplication in the conservation wrapper.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Phantoms/`.

## 5. Risks
Overlap with 127 → object memory vs personal secret are separate records; the shared DTO is the interface, not a shared store.
Provenance text invention → display reads records; a fixture asserts no generated prose.

---

## 6. Expanded census (3 files · 650 lines)

Scope: `Assets/Ashfall.Core/Phantoms/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `HeirloomCatalog.cs` | 108 | Catalog | **yes** | 0 | 0 | 0 |
| `HeirloomSystem.cs` | 486 | System | **yes** | 0 | 0 | 2 |
| `PhantomTriggerDto.cs` | 56 | DTO/Type | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `phantom_heirlooms.json` | array[12] |
| `phantom_triggers.json` | array[20] |
| `dweller_heirlooms_master.json` | object[3 keys] |
| `heirloom_seed_viability_reports.json` | array[7] |

**State surfaces:** `HeirloomSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Phantoms/` |
| Test references | 7 name references across the test tree |
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

Domain files: 5. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ANOMALY-PHANTOM-63` | 5 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |
| `PLAN-SECRETS-CONFESSION-TRUTH-127` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `HPT-149A` | `HeirloomCatalog.cs`, `HeirloomSystem.cs` |
| `HPT-149B` | no name match — resolve at claim time |
| `HPT-149C` | `PhantomTriggerDto.cs` |
| `HPT-149D` | no name match — resolve at claim time |
| `HPT-149E` | `PhantomTriggerDto.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs`, `Ashfall.Core.Tests/HeirloomSystemTests.cs`, `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `HeirloomSystem` | `HeirloomCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `phantom_memory` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnPhantomKnock` | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/heirloom_seed_viability_reports.json` |
| `Assets/StreamingAssets/Data/phantom_heirlooms.json` |
| `Assets/StreamingAssets/Data/phantom_triggers.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **3**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/PhantomMemoryHostSession.cs` |
| `src/Host/PhantomMemorySaveStore.cs` |
| `src/UI/PhantomMemoryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `phantom_memory` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `psychology_arc_trigger` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 1, OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `narrative/heirloom_seed_viability_reports.json` | CODEX_ONLY |
| `phantom_heirlooms.json` | OPTIONAL |
| `phantom_triggers.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 1 (laddered 0) · RNG streams 1 · host files 4 · catalogs 6 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HEIRLOOM-PHANTOM-TRUTH-149
wave: 12
status: PROPOSED — foreman claim required
packages: HPT-149A, HPT-149B, HPT-149C, HPT-149D, HPT-149E
claim paths:
  - src/Host/PhantomMemoryHostSession.cs  # §19 candidate host surface
  - src/Host/PhantomMemorySaveStore.cs  # §19 candidate host surface
  - src/UI/PhantomMemoryPanel.cs  # §19 candidate host surface
  - psychology_arc_trigger  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/heirloom_seed_viability_reports.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/phantom_heirlooms.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
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
