# PLAN-DETERMINISM-CROSS-HOST-89 — Headless ↔ In-Game Replay Equality Proof

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DETERMINISM-REPLAY-13, PLAN-SAVE-GOVERNANCE-12, PLAN-AUTOMATED-QA-CAMPAIGNS-74.
**Implementation scaffold:** [`PLAN-DETERMINISM-CROSS-HOST-89_APPENDIX-A_SCAFFOLD.md`](PLAN-DETERMINISM-CROSS-HOST-89_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DETERMINISM-REPLAY-13` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new simulation, no parallel clock, no second RNG; this plan
proves the existing one, it does not replace it.

## 1. Outcome
Plan 13 adds the static gate (no wall-clock, no `System.Random`, stream
registry). Plan 74 runs scenario matrices. What is still unproven is
**equality between the two execution hosts**: the same seed advanced N days
under `godot --headless` and under the in-game day loop must produce identical
per-day state, not merely no-crash. Today's 63 registered streams and the
checksummed envelope make that provable.

| Deliverable | Detail |
|---|---|
| Replay harness | run one seed for N days headless; dump per-day state checksum + stream draw counts |
| In-game parity capture | same seed, same days through the live day loop; dump the same tuple |
| Divergence bisect | on mismatch, dump per-day and per-section checksums to name the first divergent section |
| Fork-order freeze | a test that changes composition order and fails if stream draw order shifts |
| Bounded recipe | a 7-day paired run small enough for a focused window (not the full suite) |

## 2. Evidence
- `Assets/Ashfall.Core/Random/CampaignRngStream.cs`: 63 registered streams
  (Plan 13 Appendix A carries the host/Core consumer counts).
- `SaveChecksum.Compute` used by `Save/SaveEnvelopeHelper.cs` (write, verify,
  re-write) and `Save/SaveStore.cs`; envelope is checksummed end-to-end.
- 218 unique `--*-selftest` flags in `src/Host/HostCli.cs` provide the headless
  entry points; Plan 23 audits their truthfulness.
- Plan 74 owns the scenario matrix; this plan adds the host-parity pair it lacks.

## 3. Packages
- **DXH-89A** headless replay dump: one verb, one seed, N days, deterministic JSON (day, checksum, stream counts).
- **DXH-89B** in-game capture: same tuple through the live day loop; saved beside the headless dump.
- **DXH-89C** comparator + bisect: first divergent day → first divergent section → named owner file.
- **DXH-89D** fork-order test: reorder two setups in a test composition; expect failure with the shifted stream named.
- **DXH-89E** bounded recipe doc: exact commands, runtime budget, expected output; wired to Plan 74's rotation.

## 4. Acceptance & verification
- 7-day paired run: identical per-day checksums and draw counts for the chosen seed.
- Fork-order test fails on deliberate reorder and passes unchanged.
- Divergence bisect names a section within one run when a mutation is injected.
- `godot --headless --path . -- --<chosen>-selftest` (one run) + `bash scripts/run_test.sh` on the focused determinism region.

## 5. Risks
Host capture accidentally reading wall-clock → the gate (Plan 13) plus the
per-day dump makes any drift visible at day 1, not at release.

---

## 6. Expanded census (1 files · 259 lines)

Scope: `Assets/Ashfall.Core/Random/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignRngStream.cs` | 259 | Support | **yes** | 0 | 0 | 7 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `CampaignRngStream.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 3 name references across the test tree |
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
Governed artifacts: 6. Other plans referencing them: **14**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-THREADING-ASYNCHRONY-72` | 2 |
| `PLAN-SAVE-MIGRATION-CORRIDOR-87` | 2 |
| `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 2 |
| `PLAN-CAMPAIGN-PORTABILITY-104` | 2 |
| `PLAN-SAVE-SLOT-UX-105` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Assets/Ashfall.Core/Random/CampaignRngStream.cs` |
| `CampaignRngStream.cs` |
| `PLAN-DETERMINISM-CROSS-HOST-89_APPENDIX-A_SCAFFOLD.md` |
| `Save/SaveEnvelopeHelper.cs` |
| `Save/SaveStore.cs` |
| `src/Host/HostCli.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `DXH-89A` | `Assets/Ashfall.Core/Random/CampaignRngStream.cs`, `CampaignRngStream.cs` |
| `DXH-89B` | no name match — resolve at claim time |
| `DXH-89C` | no name match — resolve at claim time |
| `DXH-89D` | `Assets/Ashfall.Core/Random/CampaignRngStream.cs`, `CampaignRngStream.cs` |
| `DXH-89E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **265** · Test files: **26** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 265 | `src/Audio/AudioSelfTest.cs`, `src/Host/AgricultureSaveStore.cs`, `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 26 | `Ashfall.Core.Tests/BareSaveStoreSealTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignRngStreamTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `SaveStore` | `SaveEnvelopeHelper` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `agriculture` |
| `airlock_security` |
| `amphibious_draisine` |
| `amputation` |
| `campaign` |
| `campaign_day` |
| `collectible_discovery` |
| `contractor_roster` |
| `draisine_recovery` |
| `duty_roster` |
| `shelter_security` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--agriculture-selftest` |
| `--amphibious-draisine-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--campaign-journey-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--save-store-checksum-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnRosterBurned` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnRosterChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| `OnRosterUpdated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnSecurityChanged` | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/amphibious_draisine_catalog.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (151 files, 1124 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `DutyRoster` | 5 | 49 |
| `Integration` | 16 | 74 |
| `PlayerCommand` | 1 | 1 |
| `Quests` | 4 | 25 |
| `Shelter` | 87 | 754 |

**Verdict:** 1124 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Campaign`, `DutyRoster`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **284**
(13 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |

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
| `amputation` | no |
| `campaign` | no |
| `campaign_day` | no |
| `collectible_discovery` | no |
| `contractor_roster` | no |
| `draisine_recovery` | no |
| `duty_roster` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `black_market_debt_event` |
| `duty_roster` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **120**
(CODEX_ONLY 68, GAMEPLAY_CONSUMED 38, OPTIONAL 1, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `dose_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 14
**Surface:** save sections 30 (laddered 0) · RNG streams 7 · host files 19 · catalogs 22 · test regions 8 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DETERMINISM-CROSS-HOST-89
wave: 8
status: PROPOSED — foreman claim required
packages: DXH-89A, DXH-89B, DXH-89C, DXH-89D, DXH-89E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/amphibious_draisine_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --agriculture-selftest
dependencies:
  - coordinate: 14 other plan(s) name these artifacts (§12)
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
