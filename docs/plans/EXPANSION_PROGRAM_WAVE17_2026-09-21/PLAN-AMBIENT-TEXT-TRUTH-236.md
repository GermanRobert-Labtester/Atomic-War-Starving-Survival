# PLAN-AMBIENT-TEXT-TRUTH-236 — Environmental Text: Placement, Tone & Repetition

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PROCEDURAL-NARRATIVE-TRUTH-216, PLAN-CODEX-SURFACE-TRUTH-110, PLAN-ORIGINALITY-LICENSING-60.
**Non-goals:** no procedural composition rules (Plan 216), no codex entries
(Plan 110), no licensing model (Plan 60).

## 1. Outcome
Two reachable systems are unaddressed: `AtmosphereTextSystem.cs` (**266**,
Core root) and `EnvironmentalTextSystem.cs` (**176**). These place short
authored lines into the world (signs, graffiti, notes, overheard fragments) —
the layer most prone to repetition and tone drift, and the one with no owner.

| Deliverable | Detail |
|---|---|
| Placement model | ambient text bound to documented contexts (site class, room, event) from existing catalogs; no runtime invention |
| Selection | seeded selection with no immediate repetition per context; a seen-line ledger bounds reuse |
| Tone/provenance | lines come only from authored catalogs with record ids; the plan defers tone to Plan 60's rules |
| Readability | displayed lines respect reading surfaces (Plan 110's patterns) and localization keys (Plan 52/88) |
| Save truth | seen-line ledger restores; no re-selection that changes history |

## 2. Evidence
- `Assets/Ashfall.Core/AtmosphereTextSystem.cs` (266) and `EnvironmentalTextSystem.cs` (176) — both unaddressed (Wave 17 audit).
- Plan 216 owns procedural composition; these systems place authored lines — boundary stated.
- Plan 52/88 supply localization keys; Plan 110 the display patterns.
- Plan 60 governs originality.

## 3. Packages
- **ATT-236A** placement model + context table.
- **ATT-236B** selection/no-repeat tests (seeded).
- **ATT-236C** provenance check (authored catalogs only).
- **ATT-236D** localization/display-surface test.
- **ATT-236E** save round-trip of the seen-ledger.

## 4. Acceptance & verification
- No ambient line repeats within its context window; selection is seed-stable.
- Every line resolves to a catalog id and a localization key.
- `bash scripts/run_test.sh` on the narrative test region.

## 5. Risks
Tone drift → authored-only rule plus Plan 60 review.
Repetition → the seen-ledger and window are fixtures.

---

## 6. Expanded census (2 files · 442 lines)

Scope (corrected): the two Core-root premise systems
(`AtmosphereTextSystem.cs`, `EnvironmentalTextSystem.cs`) plus `Narrative/` and
`UI/` files matching the ambient/atmosphere tokens. A directory-only scan was
empty because both authorities live at the Core root.

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AtmosphereTextSystem.cs` | 266 | System | **yes** | 0 | 0 | 0 |
| `EnvironmentalTextSystem.cs` | 176 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `environmental_atmosphere_expansion.json` | object[3 keys] |
| `environmental_texts_expansion_05.json` | object[2 keys] |
| `atmosphere_profiles.json` | object[2 keys] |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 2 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Authorship rule | both systems select from authored catalogs only (Plan 60/216 boundary) |
| Localization | displayed lines must resolve to keys (Plan 52/88) |

## 9. Rollout sequence

1. Premise re-check: both root systems unchanged since authoring.
2. Placement model: bind contexts to catalogs; no runtime invention.
3. Selection: seeded, no-repeat window per context; seen-ledger persists.
4. Display: localization keys and reading surfaces (Plan 110 patterns).
5. Verification: narrative region + census regenerated.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Selection | seeded, no immediate repetition in-context |
| Provenance | every line resolves to a catalog id and a localization key |
| Persistence | seen-ledger round-trips; no re-roll on load |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **2**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SHELTER-ARCHITECTURE-40` | 2 |
| `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 1 |

**Reading:** incoming edges are coordination risk.

---

## 13. Authority binding map

Symbols used: 5. Host files: **1** · Test files: **3** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/WorldHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Ashfall.Core.Tests/Plan17ALoreBaselineTests.cs`, `Ashfall.Core.Tests/Plan17BAtmosphereTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `oral_lore` |
| `shelter_atmosphere` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--atmosphere-selftest` |
| `--shelter-atmosphere-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnBaselineCorrected` | `Assets/Ashfall.Core/CohortSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |
| `Assets/StreamingAssets/Data/environmental_texts_expansion_05.json` |

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

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Main.ShelterAtmosphere.cs` |
| `src/UI/ShelterAtmospherePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `shelter_atmosphere` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `environmental_atmosphere_expansion.json` | GAMEPLAY_CONSUMED |
| `environmental_texts_expansion_05.json` | GAMEPLAY_CONSUMED |

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
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 5 · catalogs 5 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AMBIENT-TEXT-TRUTH-236
wave: 17
status: PROPOSED — foreman claim required
packages: ATT-236A, ATT-236B, ATT-236C, ATT-236D, ATT-236E
claim paths:
  - src/Host/ShelterAtmosphereHostSession.cs  # §19 candidate host surface
  - src/Host/ShelterAtmosphereSaveStore.cs  # §19 candidate host surface
  - src/Host/ShelterAtmosphereSelfTest.cs  # §19 candidate host surface
  - src/Main.ShelterAtmosphere.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --atmosphere-selftest
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
