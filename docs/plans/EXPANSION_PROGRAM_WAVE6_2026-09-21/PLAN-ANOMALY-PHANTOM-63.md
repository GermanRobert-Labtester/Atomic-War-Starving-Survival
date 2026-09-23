# PLAN-ANOMALY-PHANTOM-63 — Anomalies, Phantom Memory, Secrets & Heirlooms

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-SIGNALS-REMOTE-SENSING-49.
**Non-goals:** no real-world occult references, no gore, no new fear/belief
authority outside the existing mental-health owners.

## Outcome
The wasteland has a strange-fiction layer that is authored but disconnected:
`AnomalyHazardSystem` + catalog (contact event sealed into the journal),
`PhantomMemoryEngine` + `phantom_triggers.json` (20 backgrounds, motivation/
breakdown bifurcation), `ConfessionSecretSystem` + catalog, `HeirloomSystem` +
catalog, `AbyssalAnomalies*` (hydrophone/geothermal/cryopod/salt-mine
inscriptions), `PhantomMemoryHostSessionTests` 44/44, and heirloom/time-capsule
families. This plan makes the uncanny a **playable investigation layer**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Anomalous sites | `AnomalyHazardSystem` | approach, sample, avoid | contact, exposure, discoveries |
| Phantom memory | `PhantomMemoryEngine` | explore a place | motivation/breakdown outcomes, traits |
| Confessions | `ConfessionSecretSystem` | hear, keep, reveal | trust, guilt, exposure |
| Heirlooms | `HeirloomSystem` | keep, give, display | meaning, morale, inheritance |
| Inscriptions | `AbyssalAnomalies` catalogs | read, record | lore, clues, research hints |
| Abyssal signals | hydrophone acoustic rows | listen, triangulate | map intel, dread |
| Consequences | mental-health owners | manage | trauma, resolve, coping |

## Evidence
- Core: `AnomalyHazardSystem`/`Catalog`, `PhantomMemoryEngine.cs` + `Phantoms/` (7 files), `Narrative/AbyssalAnomalies*` (4 catalogs), `TimeCapsuleSystem`, `HeirloomCatalog`.
- Data: `anomalies.json`, `phantom_triggers.json`, `confession_secrets.json`(or equivalent), `heirlooms.json`, hydro/geothermal/cryopod/salt catalogs.
- Sealed prior: Plan 111 phantom triggers (17/17 + 44/44 host), Plan 49 atmosphere, Plan 212 time capsules (5/5).
- Contracts: phantom outcomes deterministic; one journal key per severity; no parallel trauma state.

## Packages
- **AP-63A** anomaly field: sites with approach bands, sample actions, and typed outcomes (knowledge, hazard, transformation).
- **AP-63B** phantom investigation: triggers fire from real places/items; the motivation↔breakdown branch is player-influenced, not a coin flip.
- **AP-63C** confessions & heirlooms: social/keep-or-reveal choices and heirloom transfer; effects via morale/relations.
- **AP-63D** inscription network: reading/recording feeds research and the chronicle; no duplicate lore store.
- **AP-63E** dread management: exposure accumulates; rest/care/therapy resolve it; crisis routes to existing owners.
- **AP-63F** content volumes: +10 anomalies, +12 triggers, +10 inscriptions, +8 heirlooms; fictional.

## Acceptance & verification
- Determinism on replays; no jumpscare dependency (text-first); all effects bounded.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Phantoms/`; `Plan111` suites; `godot --headless --path . -- --anomaly-selftest`.

## Risks
Horror fatigue → episodic, opt-out via accessibility preset (Plan 51 cognitive mode reduces intensity).

---

## 6. Expanded census (5 files · 1,376 lines)

Scope: `Assets/Ashfall.Core/Phantoms/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · DTO/Type 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ConfessionSecretCatalog.cs` | 129 | Catalog | — | 0 | 0 | 0 |
| `ConfessionSecretSystem.cs` | 288 | System | **yes** | 0 | 0 | 2 |
| `PhantomTriggerDto.cs` | 56 | DTO/Type | — | 0 | 0 | 0 |
| `AnomalyHazardCatalog.cs` | 249 | Catalog | — | 0 | 0 | 0 |
| `AnomalyHazardSystem.cs` | 654 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `phantom_heirlooms.json` | array[12] |
| `phantom_triggers.json` | array[20] |
| `confession_secrets.json` | array[38] |
| `wire_confessions.json` | object[3 keys] |

**State surfaces:** `ConfessionSecretSystem.cs`, `AnomalyHazardSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Phantoms/` |
| Test references | 6 name references across the test tree |
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

Domain files: 7. Other plans referencing their names: **8**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-HEIRLOOM-PHANTOM-TRUTH-149` | 4 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |
| `PLAN-SECRETS-CONFESSION-TRUTH-127` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-WEATHER-ATMOSPHERE-28` | 1 |
| `PLAN-WORLD-FAMILY-TRUTH-267` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AP-63A` | `AnomalyHazardCatalog.cs`, `AnomalyHazardSystem.cs` |
| `AP-63B` | `PhantomTriggerDto.cs` |
| `AP-63C` | `HeirloomSystem.cs`, `ConfessionSecretCatalog.cs`, `ConfessionSecretSystem.cs` |
| `AP-63D` | no name match — resolve at claim time |
| `AP-63E` | no name match — resolve at claim time |
| `AP-63F` | `HeirloomSystem.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 7; intra-domain edges: **2**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ConfessionSecretSystem` | `ConfessionSecretCatalog` |
| `PhantomTriggerDto` | `PhantomMemoryEngine` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ConfessionSecretCatalog` | 1 |
| `PhantomMemoryEngine` | 1 |
| `AnomalyHazardCatalog` | 0 |
| `AnomalyHazardSystem` | 0 |
| `ConfessionSecretSystem` | 0 |
| `HeirloomSystem` | 0 |
| `PhantomTriggerDto` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 7. Host files: **6** · Test files: **13** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/HostCli.PanelTests.cs`, `src/Host/PhantomMemoryHostSession.cs`, `src/Host/Phase0HostSession.cs`, `src/Main.Anomaly.cs`, `src/UI/AnomalyWatchPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 13 | `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`, `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs`, `Ashfall.Core.Tests/HeirloomSystemTests.cs`, `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`, `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `anomaly_hazard` |
| `phantom_memory` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnPhantomKnock` | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/confession_secrets.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/memory_decay_rates.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/heirloom_seed_viability_reports.json` |
| `Assets/StreamingAssets/Data/npc_memory_dialogue.json` |
| `Assets/StreamingAssets/Data/phantom_heirlooms.json` |
| `Assets/StreamingAssets/Data/phantom_triggers.json` |
| `Assets/StreamingAssets/Data/standing_record_memory.json` |

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

Host files (`src/`) whose names share a domain token: **7**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/PhantomMemoryHostSession.cs` |
| `src/Host/PhantomMemorySaveStore.cs` |
| `src/Main.Anomaly.cs` |
| `src/UI/AnomalyWatchPanel.cs` |
| `src/UI/PhantomMemoryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `phantom_memory` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `anomaly_hazard` |
| `psychology_arc_trigger` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 3, OPTIONAL 2).

| Catalog | Classification |
|---|---|
| `confession_secrets.json` | OPTIONAL |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `narrative/canyon_mudflow_hazard_reports.json` | CODEX_ONLY |
| `narrative/heirloom_seed_viability_reports.json` | CODEX_ONLY |
| `phantom_heirlooms.json` | OPTIONAL |
| `phantom_triggers.json` | GAMEPLAY_CONSUMED |
| `standing_record_memory.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 2 (laddered 0) · RNG streams 2 · host files 9 · catalogs 16 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ANOMALY-PHANTOM-63
wave: 6
status: PROPOSED — foreman claim required
packages: AP-63A, AP-63B, AP-63C, AP-63D, AP-63E, AP-63F
claim paths:
  - src/Host/AnomalyHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/ExcavationHazardSaveStore.cs  # §19 candidate host surface
  - src/Host/PhantomMemoryHostSession.cs  # §19 candidate host surface
  - src/Host/PhantomMemorySaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/confession_secrets.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/excavation_hazard_mitigation.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --shelter-hazard-loop-selftest
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
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
