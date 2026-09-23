# PLAN-CRISIS-DISASTER-RESPONSE-80 — Fire, Flood, Collapse, Chemical & Evacuation

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-ARCHITECTURE-40, PLAN-BASE-DEFENSE-RAIDS-61.
**Expanded appendix:** [`PLAN-CRISIS-DISASTER-RESPONSE-80_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-CRISIS-DISASTER-RESPONSE-80_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's crisis & disaster response
systems (3 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no dynamic destructible simulation; no real emergency-service
procedures; existing hazard owners stay canonical.

## Outcome
Disaster is authored in pieces: `DisasterResponseSystem` + `disaster_templates.json`
(6 crisis archetypes, 5 protocols, resilience rating — sealed at Core),
`ShelterFireHazardSystem` (incidents consumed by crisis alerts), `SumpFloodingSystem`,
`ExplosionHazard`, `RadiationSystem` acute states, `SubterraneanSubsidenceEngine`,
`EmergencyAlertSystem` (orphan), `EmergencyMusterReadinessEngine` (orphan),
`CrisisPresentationCoordinator`, `Evacuation`/airlock systems. The missing
layer is a **response cycle the player trains for**.

| Phase | Mechanic | Owner |
|---|---|---|
| Prevent | inspections, drills, maintenance, storage rules | shelter maintenance + policies |
| Detect | alarms, sensors, patrols, watch | fire/sump/radiation/early warning |
| Respond | protocol activation, muster, evacuation, isolation | disaster response + muster + airlocks |
| Contain | suppress fire, pump flood, shore collapse, seal plume | canonical hazard owners |
| Recover | casualties, damage, repairs, relocation | medical, maintenance, rooms |
| Learn | incident review, doctrine, resilience rating | archive/journal + rating |

## Evidence
- Core: `Shelter/DisasterResponseSystem.cs` (6/6), `ShelterFireHazardSystem`, `SumpFloodingSystem` (39/39), `Excavation/SubterraneanSubsidenceEngine` (orphan), `Emergency/EmergencyAlertSystem.cs` (orphan), `Shelter/EmergencyMusterReadinessEngine.cs` (orphan), `CrisisPresentationCoordinator`, `Plan194CrisisProducerWireTests` 6/6.
- Data: `disaster_templates.json`, `shelter_shielding.json`, `fallout_patterns.json`, `weather_hardening_upgrades.json`.
- Sealed prior: Plan 158 (6/6), Plan 194 producer wire (fire/flood/radiation/fate), `--evacuation-selftest`, `--sump-flooding-selftest`.
- Contracts: alerts are presentation-only (no alert save); protocols reduce damage, never zero; stability < 40% alert.

## Packages
- **DR-80A** inspection/drill loop: scheduled checks and drills reduce failure and improve muster readiness.
- **DR-80B** alarm truth: every hazard surfaces a typed alert with location and severity; coalescing per Plan 169.
- **DR-80C** protocol play: activate a protocol (per disaster type), assign teams, apply bounded mitigation.
- **DR-80D** evacuation/relocation: route to safe rooms/surface; capacity and panic handled via needs/morale.
- **DR-80E** contain/recover: hazard-specific suppression actions through canonical owners; casualty + damage accounting.
- **DR-80F** incident review: doctrine entries modify future readiness (bounded).
- **DR-80G** content volumes: +8 disaster templates, +10 protocols, +8 drills; fictional.

## Acceptance & verification
- A seeded fire/flood/collapse each has a winnable response with preparation and a costly one without; no unwinnable spiral.
- `godot --headless --path . -- --disaster-response-selftest`; fire/sump suites; `--evacuation-selftest`.

## Risks
Hazard spam → only threshold-crossing alerts; noise policy in cognitive mode (Plan 51).

---

## 6. Expanded census (3 files · 1,004 lines)

Scope: `Assets/Ashfall.Core/Emergency/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `EmergencyAlertSystem.cs` | 355 | System | **yes** | 0 | 1 | 2 |
| `DisasterResponseSystem.cs` | 457 | System | **yes** | 1 | 0 | 2 |
| `EmergencyMusterReadinessEngine.cs` | 192 | System | **yes** | 0 | 0 | 0 |

**Totals:** 1 banned refs · 1 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `muster_camp_scenes.json` | object[2 keys] |
| `muster_faction_actions.json` | object[2 keys] |
| `muster_epilogues.json` | object[2 keys] |
| `muster_faction_culture.json` | array[25] |
| `muster_witnesses.json` | object[2 keys] |
| `disaster_templates.json` | object[4 keys] |

**State surfaces:** `EmergencyAlertSystem.cs`, `DisasterResponseSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Emergency/` |
| Test references | 3 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 3. Other plans referencing their names: **4**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DR-80A` | `EmergencyMusterReadinessEngine.cs` |
| `DR-80B` | `EmergencyAlertSystem.cs` |
| `DR-80C` | `DisasterResponseSystem.cs` |
| `DR-80D` | no name match — resolve at claim time |
| `DR-80E` | no name match — resolve at claim time |
| `DR-80F` | `EmergencyMusterReadinessEngine.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Emergency/Plan194EmergencyAlertIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/EmergencyMusterReadinessEngineTests.cs`, `Ashfall.Core.Tests/Shelter/Plan158DisasterResponseIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

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
| `mental_health_crisis` |
| `muster` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--muster-selftest` |
| `--muster-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/disaster_templates.json` |
| `Assets/StreamingAssets/Data/emergency_alerts.json` |
| `Assets/StreamingAssets/Data/muster_camp_scenes.json` |
| `Assets/StreamingAssets/Data/muster_epilogues.json` |
| `Assets/StreamingAssets/Data/muster_faction_actions.json` |
| `Assets/StreamingAssets/Data/muster_faction_culture.json` |
| `Assets/StreamingAssets/Data/muster_witnesses.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 6 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Emergency` | 1 | 6 |

**Verdict:** 6 cases sit under matching regions — run those first (`Emergency`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **11**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/MentalHealthCrisisHostSession.cs` |
| `src/Host/MusterHostSession.cs` |
| `src/Host/MusterSaveStore.cs` |
| `src/Main.BriefingCrisis.cs` |
| `src/Main.Muster.cs` |
| `src/Main.UiTests.Muster.cs` |
| `src/UI/DesperationCrisisPanel.cs` |
| `src/UI/EmergencyResponseHud.cs` |
| `src/UI/MentalHealthCrisisPanel.cs` |
| `src/UI/MusterAtlasPanel.cs` |
| `src/UI/MusterPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `mental_health_crisis` | no |
| `muster` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `muster` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(GAMEPLAY_CONSUMED 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `muster_camp_scenes.json` | UNRESOLVED |
| `muster_epilogues.json` | GAMEPLAY_CONSUMED |
| `muster_faction_actions.json` | UNRESOLVED |
| `muster_faction_culture.json` | UNRESOLVED |
| `muster_witnesses.json` | GAMEPLAY_CONSUMED |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 12 · catalogs 12 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CRISIS-DISASTER-RESPONSE-80
wave: 7
status: PROPOSED — foreman claim required
packages: DR-80A, DR-80B, DR-80C, DR-80D, DR-80E, DR-80F, DR-80G
claim paths:
  - src/Host/MentalHealthCrisisHostSession.cs  # §19 candidate host surface
  - src/Host/MusterHostSession.cs  # §19 candidate host surface
  - src/Host/MusterSaveStore.cs  # §19 candidate host surface
  - src/Main.BriefingCrisis.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/disaster_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/emergency_alerts.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Emergency/
  - godot --headless --path . -- --muster-selftest
dependencies:
  - coordinate: 4 other plan(s) name these artifacts (§12)
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
