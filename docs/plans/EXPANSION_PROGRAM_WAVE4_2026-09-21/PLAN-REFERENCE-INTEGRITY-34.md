# PLAN-REFERENCE-INTEGRITY-34 — Cross-Catalog References, ID Stability & Alias Policy

**Wave:** 4 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-DATA-AUTHORITY-14, PLAN-DATA-CONSUMER-22, PLAN-SAVE-GOVERNANCE-12.
**Expanded appendix:** [`PLAN-REFERENCE-INTEGRITY-34_APPENDIX-A_REFERENCE_GRAPH.md`](PLAN-REFERENCE-INTEGRITY-34_APPENDIX-A_REFERENCE_GRAPH.md)
— the generated cross-catalog reference graph over all data catalogs, by id
family with owner and referencing catalogs (top families: `item_` 332 ids / 112
referencing files; `loc_` 117 / 59; `faction_` 43 / 60; `quest_` 300 / 14;
`scrap_` 6 / 84). RF-34B adds a validator rule per exposed edge.
**Non-goals:** no ID renumbering, no schema rewrite, no forced case
normalization of authored prose.

---

## 1. Outcome

The data layer validates catalogs (finalised by the integrity validator and the
data-integrity selftest), but the **reference edges between catalogs and saves**
are unevenly enforced: some families have strict resolvers (loot, locations,
recipes), others resolve lazily, and some comparisons use case-insensitive
matching. ID renames have occurred historically with migration helpers
(`DoseQuestMigration`, `VerdictQuestMigration`, save-section aliases), so the
policy that keeps old saves and old content working exists in fragments.

Deliverables:

1. a **reference graph**: every catalog→catalog and save→catalog reference edge,
   with its current validator and strictness;
2. a **strictness rule**: dangling references fail; unreachable-but-valid rows
   warn; unknown-id handling is explicit per edge;
3. an **ID lifecycle policy**: create, rename (alias table + deprecation
   window), retire (tombstone), and never reuse;
4. **case discipline**: canonical ids compare exactly; the 46
   `OrdinalIgnoreCase` sites are audited and classified;
5. **save-reference validation**: ids persisted in saves validate on load with
   a recoverable fallback, never a crash or silent drop.

---

## 2. Evidence (2026-09-21)

| Fact | Value | Command |
|---|---:|---|
| Validator | `CatalogIntegrityValidator` + `CatalogIntegrityRules/Checkers` | files |
| Rule type | single `CatalogIntegrityRule` shape | `CatalogIntegrityRules.cs` |
| Catalogs walked | 318+ | data-integrity selftest |
| Alias/migration precedents | `DoseQuestMigration`, `VerdictQuestMigration`, `LifecycleSectionAliases` | files |
| Case-insensitive comparisons | 46 `OrdinalIgnoreCase` in catalog/inventory code | grep |
| Unique-id guard | `UniqueItemClaimRegistry`, `StableHash` | files |
| Path/registry gates | catalog registry, filename uniqueness, case-collision | `CI_GATE_MANIFEST.json` |

---

## 3. Packages

### RF-34A — Reference graph inventory
- Generate `docs/data/REFERENCE_GRAPH.md`: edge list
  `source catalog → field → target catalog | validator | strictness | notes`.
- Include save-stored references (item ids, survivor ids, quest ids, flag
  names, section keys, stream ids).
- **Acceptance:** every id-bearing field in every catalog appears; uncovered
  edges are `UNVALIDATED`.
- **Verify:** generator `--check`.

### RF-34B — Strictness rules
- Adopt: `dangling = FAIL`, `unreachable = WARN`, `unknown = explicit policy`
  (`reject` for gameplay-critical, `ignore-with-default` for optional
  presentation). Extend `CatalogIntegrityRules` with the missing edge families.
- **Acceptance:** zero `UNVALIDATED` gameplay-critical edges; data-integrity
  selftest reports per-edge counts; a seeded dangling row fails with the exact
  path.
- **Verify:** `godot --headless --path . -- --data-integrity-selftest`.

### RF-34C — ID lifecycle policy
- Write `docs/data/ID_POLICY.md`: id shape (snake_case, prefixed by family),
  uniqueness, rename process (add alias → migrate consumers → deprecate with a
  dated window → tombstone), and a hard prohibition on reuse.
- Provide an alias table format and runtime resolver hook so old saves/content
  resolve through aliases; a retired id resolves to its tombstone with a typed
  warning.
- **Acceptance:** a renamed id in a test fixture resolves through the alias
  table in both catalogs and saves; reuse of a retired id fails the gate.
- **Verify:** focused alias tests + catalog gate.

### RF-34D — Case discipline
- Audit the 46 `OrdinalIgnoreCase` comparisons: keep only where input is
  genuinely user/system-cased (paths, display search); forbid for canonical id
  resolution.
- **Acceptance:** canonical id lookups are `Ordinal`; the case-collision gate
  covers catalog ids; a fixture with two ids differing only by case fails.
- **Verify:** `bash scripts/ci/case-collision-gate.sh` + focused tests.

### RF-34E — Save-reference validation
- On load, every persisted id is validated: missing target → fallback
  (`item_unknown`-style placeholder or dropped-with-journal-notice), never a
  crash, never a silent mutation of unrelated state.
- **Acceptance:** a save fixture with a dangling item/quest/location id loads
  with a typed notice; the fuzz family covers each reference class.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Save/` +
  `--save-load-selftest`.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Strict validation blocks legacy content | alias/tombstone path; strict only for gameplay-critical edges |
| Alias tables grow unbounded | dated deprecation window; tombstones retained, resolver bounded |
| Case rule breaks mod content | mod ids validated against the policy with a typed error; docs publish the rule |
| Dangling fail breaks an in-flight author | gate reports all violations in one run with exact rows |

## 5. Verification

```bash
python3 scripts/ci/generate-reference-graph.py --check
godot --headless --path . -- --data-integrity-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Save/
bash scripts/ci/case-collision-gate.sh
```

---

## 6. Expanded census (1 files · 339 lines)

Scope: `Assets/Ashfall.Core/IO/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CatalogBootValidator.cs` | 339 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/IO/` (create if absent) |
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

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **13**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-LAUNCH-FACE-06` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-DETERMINISM-REPLAY-13` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-RELEASE-OPS-20` | 1 |
| `PLAN-SELFTEST-TRUTH-23` | 1 |
| `PLAN-INPUT-HARDENING-25` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `CI_GATE_MANIFEST.json` |
| `CatalogBootValidator.cs` |
| `CatalogIntegrityRules.cs` |
| `PLAN-REFERENCE-INTEGRITY-34_APPENDIX-A_REFERENCE_GRAPH.md` |
| `docs/data/ID_POLICY.md` |
| `docs/data/REFERENCE_GRAPH.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `RF-34A` | `PLAN-REFERENCE-INTEGRITY-34_APPENDIX-A_REFERENCE_GRAPH.md`, `docs/data/REFERENCE_GRAPH.md` |
| `RF-34B` | `CatalogIntegrityRules.cs` |
| `RF-34C` | `docs/data/ID_POLICY.md` |
| `RF-34D` | no name match — resolve at claim time |
| `RF-34E` | `PLAN-REFERENCE-INTEGRITY-34_APPENDIX-A_REFERENCE_GRAPH.md`, `docs/data/REFERENCE_GRAPH.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **1** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.Application.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/CatalogLoadContractTests.cs`, `Ashfall.Core.Tests/Plan12BFrictionTests.cs`, `Ashfall.Core.Tests/Tooling/CiGateManifestDriftTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **10** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |
| `--port-contract-selftest` |
| `--real-main-journey-selftest` |
| `--save-load-failure-selftest` |
| `--save-load-failure-uitest` |
| `--save-load-selftest` |
| `--save-load-ui-failure-selftest` |
| `--selftest-manifest` |
| `--social-drift-selftest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnFrictionDetected` | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |

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

Host files (`src/`) whose names share a domain token: **0**
(0 of them panels/HUD).

| Host file |
|---|
| — | no host filename shares a token with this domain |

**Verdict:** no host file shares a token with this domain — the surface may be driven through a generic panel, or may not be surfaced at all. Verify before claiming a route.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 13
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 0 · catalogs 3 · test regions 0 · flags 10

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-REFERENCE-INTEGRITY-34
wave: —
status: PROPOSED — foreman claim required
packages: RF-34A, RF-34B, RF-34C, RF-34D, RF-34E
claim paths:
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/barter_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --data-integrity-selftest
dependencies:
  - coordinate: 13 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
