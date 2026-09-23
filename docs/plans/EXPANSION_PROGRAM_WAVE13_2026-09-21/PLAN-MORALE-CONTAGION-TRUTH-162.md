# PLAN-MORALE-CONTAGION-TRUTH-162 — How Mood Spreads Through a Shelter

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MORALE-UNREST-TRUTH-129, PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-SHELTER-CAPACITY-AUTHORITY-103.
**Implementation scaffold:** [`PLAN-MORALE-CONTAGION-TRUTH-162_APPENDIX-A_SCAFFOLD.md`](PLAN-MORALE-CONTAGION-TRUTH-162_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-MORALE-UNREST-TRUTH-129` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no collective mark model (Plan 129 owns marks), no individual
model (Plan 64), no occupancy model (Plan 103).

## 1. Outcome
`Survivors/MoraleContagionSystem.cs` (770 lines) is reachable and unaddressed.
Plan 129 owns holdfast morale marks and thresholds; Plan 64 owns individual
psychological state. Contagion is the **link** between them — how one
survivor's state affects neighbors, through which contacts, at what rate —
and nothing states whether it exists, how it is bounded, or how it is
persisted.

| Deliverable | Detail |
|---|---|
| Contact model | who influences whom from documented relations (household, bunk, duty pair) — contacts read existing owners, no private graph |
| Spread rule | per-day influence from source state to receiver state with documented thresholds and damping; no oscillation |
| Bounds | contagion cannot push a survivor below/above the individual model's legal range; clamping is explicit |
| Asymmetry | documented which states spread (fear, grief) and which do not (private resolve), so it is not a universal mood field |
| Determinism | per-day evaluation in a stable order; seeded only where the design says so; no wall clock |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` (770 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 129's mark model and Plan 64's individual state are the two ends this system links.
- Plan 103's occupancy supplies contact proximity data.
- Plan 101's roster supplies duty-pair contacts.

## 3. Packages
- **MCT-162A** contact model + source table (no private graph proof).
- **MCT-162B** spread rule + damping test (no oscillation over a scripted week).
- **MCT-162C** bounds/clamping tests against Plan 64's ranges.
- **MCT-162D** asymmetry table + test (non-spreading states stay put).
- **MCT-162E** determinism: stable evaluation order + day-based cadence.

## 4. Acceptance & verification
- Contacts match their owners (relation/occupancy/roster) with no private additions.
- A scripted week shows damping, not oscillation; bounds never exceeded.
- Non-spreading states show zero transmission in the fixture.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Universal mood field → the asymmetry table and bounds are permanent fixtures.
Overlap with 129 → contagion changes individual state; marks/thresholds stay in 129.

---

## 6. Expanded census (3 files · 1,034 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Save 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MoraleContagionCatalog.cs` | 65 | Catalog | — | 0 | 0 | 0 |
| `MoraleContagionSave.cs` | 199 | Save | — | 0 | 0 | 0 |
| `MoraleContagionSystem.cs` | 770 | System | **yes** | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `contagion_events.json` | object[3 keys] |

**State surfaces:** `MoraleContagionSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 5 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-RECREATION-MORALE-50` | 3 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MCT-162A` | no name match — resolve at claim time |
| `MCT-162B` | no name match — resolve at claim time |
| `MCT-162C` | no name match — resolve at claim time |
| `MCT-162D` | no name match — resolve at claim time |
| `MCT-162E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/MoraleContagionHostSession.cs`, `src/Main.MoraleContagion.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Flagship11/CrossSystemSmokeTests.cs`, `Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs`, `Ashfall.Core.Tests/Survivors/Plan24NeedsSourceMigrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `MoraleContagionCatalog` | `MoraleContagionSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `morale` |
| `morale_contagion` |
| `vinyl_morale` |

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

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnMoraleApplied` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnMoraleDelta` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` |
| `OnMoraleDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMoraleDrainRequested` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnPermanentMoraleBuffApplied` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/contagion_events.json` |

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

Host files (`src/`) whose names share a domain token: **6**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/MoraleContagionHostSession.cs` |
| `src/Host/MoraleContagionSaveStore.cs` |
| `src/Host/VinylMoraleHostSession.cs` |
| `src/Host/VinylMoraleSaveStore.cs` |
| `src/Main.MoraleContagion.cs` |
| `src/UI/VinylMoralePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `morale` | no |
| `morale_contagion` | no |
| `vinyl_morale` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

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
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `contagion_events.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 6 · catalogs 2 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MORALE-CONTAGION-TRUTH-162
wave: 13
status: PROPOSED — foreman claim required
packages: MCT-162A, MCT-162B, MCT-162C, MCT-162D, MCT-162E
claim paths:
  - src/Host/MoraleContagionHostSession.cs  # §19 candidate host surface
  - src/Host/MoraleContagionSaveStore.cs  # §19 candidate host surface
  - src/Host/VinylMoraleHostSession.cs  # §19 candidate host surface
  - src/Host/VinylMoraleSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/contagion_events.json  # §17 catalog (verify schema + consumer)
  - contagion_events.json  # §17 catalog (verify schema + consumer)
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
