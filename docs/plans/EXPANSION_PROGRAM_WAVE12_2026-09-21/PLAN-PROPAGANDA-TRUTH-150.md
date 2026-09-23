# PLAN-PROPAGANDA-TRUTH-150 — Influence Actions, Audience Reach & Blowback

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-POLITICS-69, PLAN-MORALE-UNREST-TRUTH-129, PLAN-PRINT-MEDIA-TRUTH-128.
**Implementation scaffold:** [`PLAN-PROPAGANDA-TRUTH-150_APPENDIX-A_SCAFFOLD.md`](PLAN-PROPAGANDA-TRUTH-150_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-RUMOR-PROPAGATION-TRUTH-120` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no print/facility work (Plan 128), no collective-morale model
(Plan 129), no faction diplomacy (Plan 29).

## 1. Outcome
`Propaganda/PropagandaSystem.cs` is a single-file authority with no plan
coverage: a system that shapes what the holdfast believes. Influence systems
become either a free win button or a hidden modifier unless their actions,
audience, cost, and blowback are explicit.

| Deliverable | Detail |
|---|---|
| Action set | each propaganda action with its audience, medium (dependent on Plan 128 outputs where relevant), cost, and cooldown |
| Reach truth | audience determined by existing holders (residents, factions, visitors); the system does not define its own population list |
| Effect routing | effects write through Plan 129's mark model and Plan 69's surface — no private belief score |
| Blowback | a documented counter-effect when the message contradicts known facts (rumors from Plan 120, revealed facts from Plan 126) |
| Determinism | action outcomes seeded where variance exists; expiry on the canonical clock |

## 2. Evidence
- `Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs` (the whole directory; verified).
- Plan 129's mark model and Plan 128's press capacity are the natural bridges.
- Plan 120 owns rumor state the blowback reads; Plan 126 supplies revealed facts.
- Plan 29 owns faction standing if the audience is a faction.

## 3. Packages
- **PRA-150A** action table (audience, medium, cost, cooldown).
- **PRA-150B** reach model reading existing population holders.
- **PRA-150C** effect routing tests into Plan 129/69 owners (no private score).
- **PRA-150D** blowback rules + contradiction fixture (known fact blocks or backfires).
- **PRA-150E** determinism + cooldown/expiry on the canonical clock.

## 4. Acceptance & verification
- Each action consumes its cost once; effects appear in the named owners, not in a local value.
- A message contradicting a known fact triggers the documented blowback.
- Same seed + same audience → same outcome; cooldowns use game time.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Propaganda/` (create if absent).

## 5. Risks
Free-win influence → costs, cooldowns, and blowback are the three constraints, each tested.
Belief duplication → no private score; routing tests enforce owners.

---

## 6. Expanded census (1 files · 507 lines)

Scope: `Assets/Ashfall.Core/Propaganda/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PropagandaSystem.cs` | 507 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `propaganda_campaigns.json` | object[3 keys] |
| `propaganda_templates.json` | object[2 keys] |
| `stencil_propaganda_smear_logs.json` | array[7] |

**State surfaces:** `PropagandaSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Propaganda/` |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-RADIO-MEDIA-42` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PRA-150A` | no name match — resolve at claim time |
| `PRA-150B` | no name match — resolve at claim time |
| `PRA-150C` | no name match — resolve at claim time |
| `PRA-150D` | no name match — resolve at claim time |
| `PRA-150E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **4** · Test files: **4** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/PropagandaHostSession.cs`, `src/Host/PropagandaSaveStore.cs`, `src/Host/PropagandaSelfTest.cs`, `src/Main.Propaganda.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Flagship11/Flagship11CatalogTests.cs`, `Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs`, `Ashfall.Core.Tests/Propaganda/Plan168PropagandaIntegrationTests.cs`, `Ashfall.Core.Tests/Propaganda/PropagandaSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/narrative_discovery_manifest.json`, `Assets/StreamingAssets/Data/propaganda_campaigns.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `propaganda_campaigns` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |
| `--propaganda-campaign-selftest` |
| `--propaganda-selftest` |
| `--real-main-journey-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **12** (115 files, 870 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Communication` | 3 | 17 |
| `Factions` | 10 | 72 |
| `Flagship11` | 7 | 63 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 870 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Communication`, `Factions`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **508**
(14 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **28**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `collectible_discovery` | no |
| `communication` | no |
| `crossing` | no |
| `death_legacy` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **11**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `cupola_foundry` |
| `deep_coast` |
| `disease` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **349**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 48, OPTIONAL 6, UNRESOLVED 16).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 16 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_become_warlord` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 28 (laddered 0) · RNG streams 11 · host files 23 · catalogs 22 · test regions 10 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PROPAGANDA-TRUTH-150
wave: 12
status: PROPOSED — foreman claim required
packages: PRA-150A, PRA-150B, PRA-150C, PRA-150D, PRA-150E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_logs_expansion_05.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --narrative-selftest
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
