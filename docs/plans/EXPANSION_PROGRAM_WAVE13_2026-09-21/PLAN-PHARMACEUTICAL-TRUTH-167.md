# PLAN-PHARMACEUTICAL-TRUTH-167 — Tablet Production, Dosage & Dependency Risk

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-ACUTE-TRAUMA-CARE-124, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-INDUSTRY-AUTOMATION-45.
**Implementation scaffold:** [`PLAN-PHARMACEUTICAL-TRUTH-167_APPENDIX-A_SCAFFOLD.md`](PLAN-PHARMACEUTICAL-TRUTH-167_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-MICROFLUIDIC-DIAGNOSTIC-TRUTH-182` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no outbreak model (Plan 47), no ward/surgery (Plan 124), no
generic quality model (Plan 112 owns tiers).

## 1. Outcome
`Medical/PharmaceuticalTabletEngine.cs` (721 lines) is reachable and
unaddressed. Plans 47 and 124 own disease and care; nothing owns the
**production and dosage** side: which tablets can be made, at what potency,
with what dependency risk, and how they enter the medical pipeline.

| Deliverable | Detail |
|---|---|
| Production model | inputs (reagents, station, skill) → output tablets at a quality tier from Plan 112; no private tier system |
| Dosage truth | each medicine's effect is a row per dose with documented caps; overdose and dependency thresholds are explicit |
| Dependency coupling | dependency risk routes to `Medical/DependencyTaperWithdrawalEngine` (Plan 124 package) as an input, not a parallel counter |
| Stock truth | tablets are inventory items (Plan 93); production consumes reagents through the same wrapper |
| Save truth | in-progress production and dependency state restore; a load never re-rolls potency |

## 2. Evidence
- `Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs` (721 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 124 owns `DependencyTaperWithdrawalEngine`; this plan feeds it.
- Plan 112 supplies produced-goods quality tiers the tablets use.
- Plan 45 owns the industry engines reagents come from.

## 3. Packages
- **PHT-167A** production model + reagent/station table.
- **PHT-167B** dosage rows + cap/overdose fixtures.
- **PHT-167C** dependency input to Plan 124 (no private counter proof).
- **PHT-167D** conservation: reagent consumption and tablet output balance.
- **PHT-167E** save round-trip (in-progress batch + potency).

## 4. Acceptance & verification
- Potency follows the quality tier; same inputs + seed → same output.
- Overdose caps trigger exactly at their documented rows; dependency appears in Plan 124's engine.
- Conservation wrapper balances across a scripted production day.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Pharma becoming a cure-all → dosage caps and dependency are the constraints, both tested.
Tier duplication → quality stays in Plan 112; this plan reads it.

---

## 6. Expanded census (1 files · 721 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PharmaceuticalTabletEngine.cs` | 721 | System | **yes** | 1 | 0 | 4 |

**Totals:** 1 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `tablet_manufacturing_catalog.json` | object[8 keys] |

**State surfaces:** `PharmaceuticalTabletEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 1 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PHT-167A` | `PharmaceuticalTabletEngine.cs` |
| `PHT-167B` | no name match — resolve at claim time |
| `PHT-167C` | no name match — resolve at claim time |
| `PHT-167D` | `PharmaceuticalTabletEngine.cs` |
| `PHT-167E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/PharmaceuticalTabletEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

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

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/tablet_manufacturing_catalog.json` |

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

Host files (`src/`) whose names share a domain token: **1**
(1 of them panels/HUD).

| Host file |
|---|
| `src/UI/ThreePanePanelScaffold.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(CODEX_ONLY 1).

| Catalog | Classification |
|---|---|
| `narrative/three_strand_rope_closing_logs.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 1 · catalogs 2 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PHARMACEUTICAL-TRUTH-167
wave: 13
status: PROPOSED — foreman claim required
packages: PHT-167A, PHT-167B, PHT-167C, PHT-167D, PHT-167E
claim paths:
  - src/UI/ThreePanePanelScaffold.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/tablet_manufacturing_catalog.json  # §17 catalog (verify schema + consumer)
  - narrative/three_strand_rope_closing_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
