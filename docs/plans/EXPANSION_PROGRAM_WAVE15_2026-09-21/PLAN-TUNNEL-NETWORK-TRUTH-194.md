# PLAN-TUNNEL-NETWORK-TRUTH-194 — Underground Links: Digging, Support & Passage

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DEEP-STRATA-83, PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-TUNNEL-NETWORK-TRUTH-194_APPENDIX-A_SCAFFOLD.md`](PLAN-TUNNEL-NETWORK-TRUTH-194_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DEEP-STRATA-83` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no strata generation (Plan 83), no route topology for the surface
(Plan 95), no new dig content.

## 1. Outcome
`Underground/TunnelNetworkSystem.cs` (**441 lines**) is reachable and
unaddressed: player-dug passages connecting sites underground. Strata (Plan 83)
and surface routes (Plan 95) exist; the tunnel graph's own contract — digging
cost, support condition, collapse risk, and passage rights — is unstated.

| Deliverable | Detail |
|---|---|
| Tunnel graph | segments with depth, length, and support condition; connects declared endpoints only |
| Digging cost | documented labor/time/materials per segment length and depth; consumes through existing owners |
| Collapse risk | support condition (Plan 119 contract) below threshold raises collapse probability; collapses block passage visibly |
| Passage rules | who/what may transit (people, cargo) with capacity limits; ties to Plan 30 travel when used as a leg |
| Save truth | graph and conditions restore; a load never collapses or opens a segment silently |

## 2. Evidence
- `Assets/Ashfall.Core/Underground/TunnelNetworkSystem.cs` (441 lines; unaddressed — Wave 13/15 audit).
- Plan 83 owns the strata the tunnels pass through.
- Plan 95 owns spatial facts; tunnel endpoints attach to them.
- Plan 119 supplies support decay.

## 3. Packages
- **TNT-194A** graph model + endpoint attachment check.
- **TNT-194B** dig cost + consumption tests.
- **TNT-194C** collapse risk/blockage fixtures.
- **TNT-194D** passage capacity + travel-leg contract.
- **TNT-194E** save round-trip; no silent state change on load.

## 4. Acceptance & verification
- Every segment connects declared endpoints; an unconnected one is reported.
- Collapse blocks passage and is visible; support condition follows the shared contract.
- Save/load preserves graph and conditions.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Underground/` (create if absent).

## 5. Risks
Second map → endpoints reference Plan 95 facts; the attachment check enforces it.
Free excavation → costs and collapse risk are the constraints.

---

## 6. Expanded census (1 files · 441 lines)

Scope: `Assets/Ashfall.Core/Underground/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `TunnelNetworkSystem.cs` | 441 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `underground_tunnels.json` | object[3 keys] |

**State surfaces:** `TunnelNetworkSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Underground/` |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TNT-194A` | no name match — resolve at claim time |
| `TNT-194B` | no name match — resolve at claim time |
| `TNT-194C` | no name match — resolve at claim time |
| `TNT-194D` | no name match — resolve at claim time |
| `TNT-194E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Underground/Plan167TunnelNetworkIntegrationTests.cs`, `Ashfall.Core.Tests/Underground/TunnelNetworkSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `piezometer_network` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--rumor-network-selftest` |

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

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/underground_fungi_flora.json` |
| `Assets/StreamingAssets/Data/piezometer_network_catalog.json` |
| `Assets/StreamingAssets/Data/pneumatic_network_catalog.json` |
| `Assets/StreamingAssets/Data/rail_network.json` |
| `Assets/StreamingAssets/Data/underground_flora.json` |
| `Assets/StreamingAssets/Data/underground_tunnels.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (25 files, 140 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `Rail` | 1 | 5 |
| `Underground` | 2 | 13 |

**Verdict:** 140 cases sit under matching regions — run those first (`Audio`, `Integration`, `NarrativeConsequence`, `Rail`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/RumorNetworkHostSession.cs` |
| `src/Host/RumorNetworkSaveStore.cs` |
| `src/Host/RumorNetworkSelfTest.cs` |
| `src/Main.RumorNetwork.cs` |
| `src/UI/ThreePanePanelScaffold.cs` |
| `src/UI/UndergroundPrintingPressPanel.cs` |
| `src/UI/WaystationNetworkPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caravan_trade_network` | no |
| `fungi_cultivation` | no |
| `piezometer_network` | no |
| `pneumatic_dispatch` | no |
| `rail_grinding` | no |
| `waystation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `route_engineering_rail_grinding` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **12**
(CODEX_ONLY 10, GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/pneumatic_carrier_capsule_logs.json` | CODEX_ONLY |
| `narrative/pneumatic_cylinder_leather_assays.json` | CODEX_ONLY |
| `narrative/pneumatic_tube_diverter_audits.json` | CODEX_ONLY |
| `narrative/screw_press_felt_reports.json` | CODEX_ONLY |
| `narrative/therapist_session_notes.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_2.json` | CODEX_ONLY |
| `narrative/therapist_session_notes_batch_3.json` | CODEX_ONLY |
| `narrative/three_strand_rope_closing_logs.json` | CODEX_ONLY |
| `narrative/underground_fungi_flora.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 6 (laddered 0) · RNG streams 1 · host files 8 · catalogs 16 · test regions 5 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TUNNEL-NETWORK-TRUTH-194
wave: 15
status: PROPOSED — foreman claim required
packages: TNT-194A, TNT-194B, TNT-194C, TNT-194D, TNT-194E
claim paths:
  - src/Host/RumorNetworkHostSession.cs  # §19 candidate host surface
  - src/Host/RumorNetworkSaveStore.cs  # §19 candidate host surface
  - src/Host/RumorNetworkSelfTest.cs  # §19 candidate host surface
  - src/Main.RumorNetwork.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/underground_fungi_flora.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/piezometer_network_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --rumor-network-selftest
dependencies:
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
