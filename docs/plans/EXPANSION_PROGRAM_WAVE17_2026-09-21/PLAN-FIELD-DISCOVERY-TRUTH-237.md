# PLAN-FIELD-DISCOVERY-TRUTH-237 — Survey Skills as Discovery Paths

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DISCOVERY-STATE-108, PLAN-DISCOVERY-CONSEQUENCE-TRUTH-211, PLAN-WATER-AGRICULTURE-46, PLAN-FOOD-CUISINE-39.
**Non-goals:** no knowledge-state machine (Plan 108), no consequence table
(Plan 211), no water/food production (Plans 46/39).

## 1. Outcome
Two reachable systems are unaddressed: `HydroGeologyDiscoverySystem.cs`
(**260**) and `GrainMillingDiscoverySystem.cs` (**248**). Each turns a **skill
action** (surveying terrain, recognizing a milling technique) into a discovery
— the path by which knowledge becomes available without reading a document.
Plan 108 owns the knowledge states; the survey→discovery rule is unowned.

| Deliverable | Detail |
|---|---|
| Survey model | a survey action with documented inputs (skill level Plan 113, tool, terrain class Plan 95) and a discovery roll from a registered stream |
| Discovery product | success grants a knowledge key through Plan 208's gate or a discovery state through Plan 108 — one owner per product |
| Failure honesty | a failed survey yields "nothing found here" (bounded retries) rather than an infinite re-roll |
| Consumer mapping | hydrogeology feeds Plan 46 sites; milling feeds Plan 39/208 knowledge — each named |
| Save truth | survey outcomes and retry bounds restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/HydroGeologyDiscoverySystem.cs` (260) and `GrainMillingDiscoverySystem.cs` (248) — both unaddressed (Wave 17 audit).
- Plan 208's knowledge gate is where a discovered technique lands.
- Plan 46/39 are the consumers; Plan 95 terrain classes the survey reads.
- Plan 113 supplies skill levels.

## 3. Packages
- **FDT-237A** survey model + input table.
- **FDT-237B** seeded discovery roll + bounds test.
- **FDT-237C** product routing (knowledge key / discovery state).
- **FDT-237D** failure/retry fixtures.
- **FDT-237E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Discoveries route to their owners; retries are bounded with a visible exhaustion state.
- Same seed + same inputs → same discovery outcome.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Infinite re-roll → retry bounds are fixtures.
Owner ambiguity → each product names one owner; no dual writes.

---

## 6. Expanded census (6 files · 2,118 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Support 2 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `GrainMillingCatalog.cs` | 256 | Catalog | — | 0 | 0 | 0 |
| `GrainMillingDiscoverySystem.cs` | 248 | System | **yes** | 0 | 0 | 2 |
| `GrainMillingProjection.cs` | 586 | Support | — | 0 | 0 | 0 |
| `HydroGeologyCatalog.cs` | 248 | Catalog | — | 0 | 0 | 0 |
| `HydroGeologyDiscoverySystem.cs` | 260 | System | **yes** | 0 | 0 | 2 |
| `HydroGeologyProjection.cs` | 520 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `GrainMillingDiscoverySystem.cs`, `HydroGeologyDiscoverySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 8 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 6. Other plans referencing them: **2**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 2 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 2 |

**Reading:** incoming edges are coordination risk.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **4**; isolated files:
**0**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `GrainMillingDiscoverySystem` | `GrainMillingCatalog` |
| `GrainMillingDiscoverySystem` | `GrainMillingProjection` |
| `HydroGeologyDiscoverySystem` | `HydroGeologyCatalog` |
| `HydroGeologyDiscoverySystem` | `HydroGeologyProjection` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `GrainMillingCatalog` | 1 |
| `GrainMillingProjection` | 1 |
| `HydroGeologyCatalog` | 1 |
| `HydroGeologyProjection` | 1 |
| `GrainMillingDiscoverySystem` | 0 |
| `HydroGeologyDiscoverySystem` | 0 |

**Class split:** hub 0 · sink 4 · source 2 · isolated 0.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/GrainMillingArchiveSaveStore.cs`, `src/Host/HydroGeologyArchiveSaveStore.cs`, `src/Main.Plans154.cs`, `src/Main.Plans157.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/GrainMillingCatalogTests.cs`, `Ashfall.Core.Tests/HydroGeologyCatalogTests.cs`, `Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs`, `Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `field_guide` |
| `grain_milling_archive` |
| `grain_processing` |

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

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **10**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/field_guide.json` |
| `Assets/StreamingAssets/Data/grain_processing.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/faction_field_documents.json` |
| `Assets/StreamingAssets/Data/narrative/field_reports_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/grain_silo_weevil_audits.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

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
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/FieldGuideSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/GrainProcessingHostSession.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
| `field_guide` | no |
| `grain_milling_archive` | no |
| `grain_processing` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 6, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `field_guide.json` | UNRESOLVED |
| `grain_processing.json` | UNRESOLVED |
| `narrative/expedition_field_reports.json` | CODEX_ONLY |
| `narrative/expedition_field_reports_batch_2.json` | CODEX_ONLY |
| `narrative/faction_field_documents.json` | CODEX_ONLY |
| `narrative/field_reports_expansion.json` | CODEX_ONLY |
| `narrative/grain_silo_weevil_audits.json` | CODEX_ONLY |
| `narrative/wildlife_field_encounter_logs.json` | CODEX_ONLY |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 4 (laddered 0) · RNG streams 0 · host files 5 · catalogs 18 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FIELD-DISCOVERY-TRUTH-237
wave: 17
status: PROPOSED — foreman claim required
packages: FDT-237A, FDT-237B, FDT-237C, FDT-237D, FDT-237E
claim paths:
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/FieldGuideSaveStore.cs  # §19 candidate host surface
  - src/Host/GrainMillingArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/GrainProcessingHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/discovery_consequences.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/field_guide.json  # §17 catalog (verify schema + consumer)
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
