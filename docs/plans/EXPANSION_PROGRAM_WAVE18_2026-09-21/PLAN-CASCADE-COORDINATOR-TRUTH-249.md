# PLAN-CASCADE-COORDINATOR-TRUTH-249 — Multi-System Cascades: Order, Depth & Stop Rules

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28, PLAN-CHEMICAL-RECON-TRUTH-183, PLAN-SEISMIC-DYNAMICS-TRUTH-193, PLAN-SILENT-FAILURE-35.
**Non-goals:** no hazard models (Plans 183/193), no weather generation (Plan 28),
no new event types.

## 1. Outcome
`Shelter/CascadeCoordinator.cs` (**212 lines**) is reachable and unaddressed:
a cascade is one event triggering others (a storm floods a shaft, which cuts
power, which spoils stores). Cascades are where simulations go quadratic or
loop forever; the coordinator's contract — evaluation order, depth bound, and
stop rules — is unstated.

| Deliverable | Detail |
|---|---|
| Cascade graph | documented trigger → consequence edges across owners (Plans 28/183/193/48/118) |
| Order/depth | breadth-first evaluation in stable order with a documented depth bound |
| Stop rules | cycle guard and diminiShing effect (a cascade cannot loop or re-trigger itself) |
| Visibility | the chain is reportable after the fact (day, root event, each hop) |
| Save truth | in-flight cascades restore at the same hop; no re-evaluation that doubles effects |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/CascadeCoordinator.cs` (212 lines; unaddressed — Wave 18 audit).
- Plan 1: `WeatherCascadeSystem`/`CascadeTargetSystem` are orphaned siblings — their seal packages should bind to this coordinator.
- Plan 35 bans swallow-catches; the coordinator must not hide a broken hop.
- Plans 28/48/118/183/193 own the effects.

## 3. Packages
- **CCT-249A** cascade edge table + owner routing.
- **CCT-249B** order/depth fixtures (stable, bounded).
- **CCT-249C** cycle/diminishing guard tests.
- **CCT-249D** chain report (root + hops) test.
- **CCT-249E** save round-trip mid-cascade; no double effects.

## 4. Acceptance & verification
- A scripted cascade resolves in documented order within the depth bound; a cycle fixture terminates.
- Effects appear once per hop in owners; the report names the chain.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Quadratic loops → depth/cycle guards are fixtures.
Double application → save round-trip mid-cascade is the proof.

---

## 6. Expanded census (2 files · 381 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CascadeCoordinator.cs` | 212 | System | **yes** | 0 | 0 | 0 |
| `CascadeRuleCatalog.cs` | 169 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `cascade_rules.json` | object[3 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 2 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CCT-249A` | `CascadeCoordinator.cs`, `CascadeRuleCatalog.cs` |
| `CCT-249B` | no name match — resolve at claim time |
| `CCT-249C` | no name match — resolve at claim time |
| `CCT-249D` | no name match — resolve at claim time |
| `CCT-249E` | `CascadeCoordinator.cs`, `CascadeRuleCatalog.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.Cascade.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Shelter/CascadeCoordinatorTests.cs`, `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` |
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

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--real-main-journey-selftest` |
| `--social-drift-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

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

Host files (`src/`) whose names share a domain token: **2**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Main.Cascade.cs` |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 2 · catalogs 1 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CASCADE-COORDINATOR-TRUTH-249
wave: 18
status: PROPOSED — foreman claim required
packages: CCT-249A, CCT-249B, CCT-249C, CCT-249D, CCT-249E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Main.Cascade.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/cascade_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --real-main-journey-selftest
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
