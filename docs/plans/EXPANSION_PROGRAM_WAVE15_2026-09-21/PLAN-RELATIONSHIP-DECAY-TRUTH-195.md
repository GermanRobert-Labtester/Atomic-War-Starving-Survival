# PLAN-RELATIONSHIP-DECAY-TRUTH-195 — Bonds Under Neglect: Drift, Repair & Thresholds

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FAMILY-DYNASTY-43, PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122, PLAN-MORALE-CONTAGION-TRUTH-162.
**Implementation scaffold:** [`PLAN-RELATIONSHIP-DECAY-TRUTH-195_APPENDIX-A_SCAFFOLD.md`](PLAN-RELATIONSHIP-DECAY-TRUTH-195_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-FAMILY-DYNASTY-43` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no relationship graph (Plan 43 owns it), no obligation records
(Plan 122), no mood spread (Plan 162).

## 1. Outcome
`Survivors/RelationshipDecaySystem.cs` (**428 lines**) is reachable and
unaddressed. Plan 43 owns relationship values; this system is the **passive
drift** — the slow change when people stop sharing meals, shifts, or rooms —
and nothing states its rate, floor, or how repair works. Untended, decay is
either invisible or a silent eraser of authored bonds.

| Deliverable | Detail |
|---|---|
| Decay model | per-day drift by relationship class, modulated by contact (Plan 103 occupancy, Plan 101 duty pairs) |
| Floor/ceiling rules | decay has a documented floor above hostility unless a triggering event intervenes; authored bonds may be flagged decay-resistant |
| Repair paths | shared meals, time, gifts, and duty together restore per documented actions; each routes to the existing interaction owners |
| Threshold effects | crossing a band (close → cordial → strained) has a visible signal through the relationship surface, not a hidden modifier |
| Determinism | per-day evaluation in stable order; no wall clock |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs` (428 lines; unaddressed — Wave 13/15 audit).
- Plan 43 owns the values this system writes through.
- Plan 162's contact model overlaps structurally; the boundary: contagion moves mood, decay moves affinity — stated in both plans.
- Plan 122's obligations change affinity through events, not passive drift.

## 3. Packages
- **RDT-195A** decay table + contact modulation test.
- **RDT-195B** floor/resistance rules + fixtures (authored bond, hostile start).
- **RDT-195C** repair action tests (each path restores per rule).
- **RDT-195D** band-signal visibility test.
- **RDT-195E** determinism (stable order, game-day cadence).

## 4. Acceptance & verification
- Drift matches the table under scripted contact; resistant bonds hold.
- Repair restores per action; no path exceeds the original value.
- Bands signal visibly; same state + day → same drift.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Silent erasure → floor/resistance rules and band signals are fixtures.
Overlap with 162/122 → boundary statements are asserted by tests in each plan.

---

## 6. Expanded census (1 files · 428 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `RelationshipDecaySystem.cs` | 428 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `relationship_decay_profiles.json` | object[2 keys] |
| `relationship_bands.json` | object[3 keys] |

**State surfaces:** `RelationshipDecaySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FAMILY-DYNASTY-43` | 1 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RDT-195A` | `RelationshipDecaySystem.cs` |
| `RDT-195B` | no name match — resolve at claim time |
| `RDT-195C` | no name match — resolve at claim time |
| `RDT-195D` | no name match — resolve at claim time |
| `RDT-195E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/RelationshipDecayHostSession.cs`, `src/Host/RelationshipDecaySelfTest.cs`, `src/Main.RelationshipDecay.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs`, `Ashfall.Core.Tests/Survivors/RelationshipDecaySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `relationship_decay` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--real-main-journey-selftest` |
| `--relationship-decay-selftest` |

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

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/infiltrator_profiles.json` |
| `Assets/StreamingAssets/Data/memory_decay_rates.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_profiles_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/nuclear_core_profiles.json` |
| `Assets/StreamingAssets/Data/nutrition_profiles.json` |
| `Assets/StreamingAssets/Data/psychology_profiles.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (34 files, 217 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `Nutrition` | 1 | 5 |

**Verdict:** 217 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Education`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **316**
(8 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **30**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `bio_fermentation` | no |
| `black_market` | no |
| `black_projects_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **67**
(CODEX_ONLY 46, GAMEPLAY_CONSUMED 13, OPTIONAL 3, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_survivor_fields.json` | OPTIONAL |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 30 (laddered 0) · RNG streams 15 · host files 24 · catalogs 22 · test regions 7 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RELATIONSHIP-DECAY-TRUTH-195
wave: 15
status: PROPOSED — foreman claim required
packages: RDT-195A, RDT-195B, RDT-195C, RDT-195D, RDT-195E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --real-main-journey-selftest
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
