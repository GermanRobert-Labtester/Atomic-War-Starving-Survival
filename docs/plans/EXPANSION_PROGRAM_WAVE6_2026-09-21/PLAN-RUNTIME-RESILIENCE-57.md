# PLAN-RUNTIME-RESILIENCE-57 — Safe Mode, Watchdogs & Recoverable Failures

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SILENT-FAILURE-35, PLAN-INPUT-HARDENING-25.
**Non-goals:** no crash reporter service, no auto-updater, no engine fork.

## Outcome
When content or a mod is broken, the current failure modes range from a silent
default to an init crash. `SessionDurabilityManager` (host-pending) and the
save backup stores have recovery pieces, but there is no **safe mode** and no
watchdog for hung day ticks. This plan makes a bad load survivable.

| Failure | Today | Target |
|---|---|---|
| Corrupt save | backup restore path exists | explicit safe-mode offer: restore latest good, continue with placeholder, or start fresh |
| Bad mod | load may fail late | reject before campaign start with a typed list; disable-set persists |
| Bad catalog row | data-integrity selftest only | startup validates critical catalog families; on failure, quarantine the catalog and boot degraded with a notice |
| Hung day tick | none | watchdog with per-owner budget; abort the owner, keep the day, log the incident |
| Repeated fatal | none | safe-mode boot that skips optional subsystems (manifest entries flagged optional) |
| Lost settings | none | settings recovery from default + backup |

## Evidence
- `SessionDurabilityManager` (10 tests, 0 host refs) implements corruption recording, backup create/restore, checksum verify.
- Save failure-path selftest exists (`--save-load-ui-failure-selftest`).
- `SubsystemManifest` lifecycle phases + `BootstrapLifecycleGate` (fresh/restore/reset).
- Catch policy gate; `ActionResult`; `ILog`.
- Mod path: `ModSupportSystem` host-pending; `JsonModLayeringTests` 19/19.

## Packages
- **RR-57A** safe-mode boot: a flag/autodetect (crash marker or failed integrity) that boots with optional subsystems skipped and a visible banner.
- **RR-57B** catalog quarantine: critical families validated at startup; failed catalogs moved aside (not deleted) with a notice and continue.
- **RR-57C** tick watchdog: per-owner budget from PLAN-RUNTIME-PERF-16; on overrun, abort owner, emit incident, keep campaign state consistent.
- **RR-57D** mod preflight: validate mod manifests/data before campaign start; users can disable mods from the failure screen.
- **RR-57E** recovery surface: one screen offering restore/continue/fresh, with exact save slots and timestamps.
- **RR-57F** crash marker: a lightweight "clean exit" marker in `user://` to detect the previous run failed.

## Acceptance & verification
- Corrupt save, bad mod, and bad catalog each reach a playable state with typed notices; watchdog aborts a synthetic hung owner.
- `godot --headless --path . -- --safe-mode-selftest`; `--session-durability-selftest`; `--mods-selftest`; save failure selftest.

## Risks
Safe mode masking bugs → every degraded boot emits a diagnostics entry and the incident is surfaced; QA treats it as a failure.

---

## 6. Expanded census (3 files · 630 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BootstrapLifecycleGate.cs` | 128 | Support | **yes** | 0 | 0 | 0 |
| `LedgerTruthIntegrityGate.cs` | 137 | Support | — | 0 | 0 | 0 |
| `SessionDurabilityManager.cs` | 365 | System | **yes** | 2 | 0 | 2 |

**Totals:** 2 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `weather_route_gates.json` | object[2 keys] |
| `standing_gates.json` | object[2 keys] |
| `blast_gate_mechanical_audits.json` | array[8] |

**State surfaces:** `SessionDurabilityManager.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 4 name references across the test tree |
| Determinism | 2 banned refs to fix or justify |
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
Domain files: 3. Other plans referencing them: **10**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 2 |
| `PLAN-LIFECYCLE-SEALING-32` | 2 |
| `PLAN-BOOTSTRAP-GATE-TRUTH-147` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `RR-57A` | `BootstrapLifecycleGate.cs`, `LedgerTruthIntegrityGate.cs` |
| `RR-57B` | no name match — resolve at claim time |
| `RR-57C` | no name match — resolve at claim time |
| `RR-57D` | no name match — resolve at claim time |
| `RR-57E` | no name match — resolve at claim time |
| `RR-57F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **4** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Launch/Plan57StoreKitTruthIntegrationTests.cs`, `Ashfall.Core.Tests/Orchestration/BootstrapLifecycleGateTests.cs`, `Ashfall.Core.Tests/Orchestration/LedgerTruthIntegrityGateTests.cs`, `Ashfall.Core.Tests/Save/SessionDurabilityManagerTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names)

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `nuclear_core_lifecycle` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |
| `--dose-ledger-selftest` |
| `--expedition-panel-lifecycle` |
| `--ledger-debt-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 5 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Lifecycle` | 1 | 5 |

**Verdict:** 5 cases sit under matching regions — run those first (`Lifecycle`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **136**
(5 of them panels/HUD).

| Host file |
|---|
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
| `src/Host/CaregivingHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `nuclear_core_lifecycle` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 6, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/therapist_session_notes.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_2.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_3.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 2 (laddered 1) · RNG streams 0 · host files 12 · catalogs 14 · test regions 1 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RUNTIME-RESILIENCE-57
wave: 6
status: PROPOSED — foreman claim required
packages: RR-57A, RR-57B, RR-57C, RR-57D, RR-57E, RR-57F
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/ledger_debt_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Lifecycle/
  - godot --headless --path . -- --data-integrity-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
