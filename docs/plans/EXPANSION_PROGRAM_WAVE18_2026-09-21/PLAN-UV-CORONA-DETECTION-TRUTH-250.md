# PLAN-UV-CORONA-DETECTION-TRUTH-250 — Solar Event Detection: Observatories & Warnings

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SIGNALS-REMOTE-SENSING-49, PLAN-WEATHER-SONDE-TRUTH-168, PLAN-CRISIS-DISASTER-RESPONSE-80.
**Non-goals:** no sensing suite (Plan 49), no weather instruments (Plan 168), no
response protocols (Plan 80).

## 1. Outcome
`Radio/UvCoronaDetectionEngine.cs` (**205 lines**) is reachable and unaddressed:
detecting solar/UV events that disable radio and damage exposed systems. It is
the rare warning system whose lead time matters — and whose instrument can be
blinded by the very event it warns about.

| Deliverable | Detail |
|---|---|
| Detection model | instrument class with sensitivity and a documented lead-time window before the event |
| Blinding | when the event starts, instruments saturate/lose the signal per a rule; post-event data is degraded |
| Warning path | warnings route to Plan 80 and Plan 209 (radio advisories); no silent warning |
| Infrastructure effects | radio blackout/damage routes to Plan 209/48 owners per event magnitude |
| Save truth | pending warnings and instrument state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Radio/UvCoronaDetectionEngine.cs` (205 lines; unaddressed — Wave 18 audit).
- Plan 49's sensing family provides context; Plan 168 the instrument-condition pattern (reused, not duplicated).
- Plan 209/48 receive effects; Plan 80 protocols.

## 3. Packages
- **UCT-250A** detection/sensitivity table + lead-time fixtures.
- **UCT-250B** blinding rule tests.
- **UCT-250C** warning routing to Plans 80/209.
- **UCT-250D** infrastructure effect routing per magnitude.
- **UCT-250E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Lead time matches the table; blinding degrades data per rule.
- Effects and warnings appear in named owners; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/`.

## 5. Risks
Perfect foresight → lead-time and blinding are fixtures.
Silent blackout → effects always route with a record.

---

## 6. Expanded census (2 files · 332 lines)

Scope: `Assets/Ashfall.Core/Radio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `UvCoronaDetectionCatalog.cs` | 127 | Catalog | — | 0 | 0 | 0 |
| `UvCoronaDetectionEngine.cs` | 205 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `uv_corona_detector_catalog.json` | object[3 keys] |

**State surfaces:** `UvCoronaDetectionEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radio/` |
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
| `PLAN-RADIO-FAMILY-TRUTH-266` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `UCT-250A` | `UvCoronaDetectionCatalog.cs`, `UvCoronaDetectionEngine.cs` |
| `UCT-250B` | no name match — resolve at claim time |
| `UCT-250C` | no name match — resolve at claim time |
| `UCT-250D` | no name match — resolve at claim time |
| `UCT-250E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Radio/UvCoronaDetectionEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_recon` |
| `recon_telemetry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--uv-corona-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/uv_corona_detector_catalog.json` |

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

Host files (`src/`) whose names share a domain token: **0**
(0 of them panels/HUD).

| Host file |
|---|
| — | no host filename shares a token with this domain |

**Verdict:** no host file shares a token with this domain — the surface may be driven through a generic panel, or may not be surfaced at all. Verify before claiming a route.

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

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `acoustic_detection` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

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
**Surface:** save sections 0 (laddered 0) · RNG streams 1 · host files 1 · catalogs 1 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-UV-CORONA-DETECTION-TRUTH-250
wave: 18
status: PROPOSED — foreman claim required
packages: UCT-250A, UCT-250B, UCT-250C, UCT-250D, UCT-250E
claim paths:
  - acoustic_detection  # §19 candidate host surface
  - Assets/StreamingAssets/Data/uv_corona_detector_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --advanced-industrial-recon-selftest
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
