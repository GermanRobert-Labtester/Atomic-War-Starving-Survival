# PLAN-HELIOGRAPH-TRUTH-235 — Mirror Signals: Line-of-Sight Links & Message Discipline

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SIGNALS-REMOTE-SENSING-49, PLAN-RADIO-STATION-TRUTH-209, PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-WEATHER-SPATIAL (Plan 28/95 inputs).
**Non-goals:** no radio operations (Plan 209), no sensing suite (Plan 49), no
route topology (Plan 95).

## 1. Outcome
`HeliographSystem.cs` (**271 lines**, Core root) is reachable and unaddressed:
line-of-sight mirror signaling between stations. It is the low-tech channel
where **weather and geography** decide whether a message arrives — and where
observation exposes the sender. Nothing states link availability, message
capacity, or exposure.

| Deliverable | Detail |
|---|---|
| Link model | stations with line-of-sight links determined by Plan 95 positions and terrain; a link exists only when visibility holds |
| Weather gating | cloud/fog from Plan 28 breaks links per a documented rule; no hidden pass-through |
| Message discipline | capacity per hour and priority queue; a long message visibly takes time |
| Exposure | signaling can be observed by hostiles per a documented check; detection routes to Plan 224/161 |
| Save truth | queue and link state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/HeliographSystem.cs` (271 lines; unaddressed — Wave 17 audit).
- Plan 95 supplies positions/terrain for line-of-sight.
- Plan 28 supplies weather gating; Plan 209 is the radio sibling.
- Plan 224/161 receive detection outcomes.

## 3. Packages
- **HGT-235A** link model + visibility test.
- **HGT-235B** weather gating fixtures.
- **HGT-235C** capacity/queue tests.
- **HGT-235D** exposure check + detection routing.
- **HGT-235E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Links appear/disappear with visibility and weather per the rules; capacity holds under queue load.
- Exposure routes to security systems; save/load preserves state.
- `bash scripts/run_test.sh` on the signals test region.

## 5. Risks
Weather-proof link → gating is a fixture.
Silent exposure → detection always routes with a record.

---

## 6. Expanded census (5 files · 1,287 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 4 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `RadioSignalLog.cs` | 149 | Support | — | 0 | 0 | 4 |
| `SignalAuthenticityEvaluator.cs` | 158 | Support | — | 0 | 0 | 0 |
| `SignalTriangulationSystem.cs` | 694 | System | — | 0 | 0 | 2 |
| `SignalTrustAvailability.cs` | 107 | Support | — | 0 | 0 | 0 |
| `SignalTrustLedger.cs` | 179 | Support | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `heliograph.json` | object[2 keys] |
| `radio_distress_signals_expansion.json` | object[2 keys] |
| `radio_distress_signals.json` | object[2 keys] |

**State surfaces:** `RadioSignalLog.cs`, `SignalTriangulationSystem.cs`, `SignalTrustLedger.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 11 name references across the test tree |
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
Domain files: 6. Other plans referencing them: **5**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 5 |
| `PLAN-RADIO-FAMILY-TRUTH-266` | 5 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-ESPIONAGE-COUNTERINTEL-41` | 1 |
| `PLAN-RADIO-MEDIA-42` | 1 |

**Reading:** incoming edges are coordination risk.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **0**; isolated files:
**6**.

**Top edges (by source name):**

| From | → To |
|---|---|
| — | no intra-domain references found |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `HeliographSystem` | 0 |
| `RadioSignalLog` | 0 |
| `SignalAuthenticityEvaluator` | 0 |
| `SignalTriangulationSystem` | 0 |
| `SignalTrustAvailability` | 0 |
| `SignalTrustLedger` | 0 |

**Class split:** hub 0 · sink 0 · source 0 · isolated 6.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **5** · Test files: **12** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/HeliographHostSession.cs`, `src/Host/HostCli.PlansB86_B89.cs`, `src/Host/RadioHostSession.cs`, `src/Main.Plans94_97.cs`, `src/UI/TriangulationPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/HeliographSystemTests.cs`, `Ashfall.Core.Tests/Plans9497CatalogTests.cs`, `Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs`, `Ashfall.Core.Tests/ProductionGameplayApiTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `heliograph` |
| `radio` |
| `radio_program_production` |
| `radio_station` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dose-ledger-selftest` |
| `--ledger-debt-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnTriangulationCompleted` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnTriangulationFailed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnTrustChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/heliograph.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json` |
| `Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scripts_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_3.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (47 files, 354 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Radio` | 47 | 354 |

**Verdict:** 354 cases sit under matching regions — run those first (`Radio`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **20**
(8 of them panels/HUD).

| Host file |
|---|
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/HeliographHostSession.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |
| `src/Host/RadioProgramProductionHostSession.cs` |
| `src/Host/RadioProgramProductionSaveStore.cs` |
| `src/Host/RadioSaveStore.cs` |
| `src/Host/RadioStationSaveStore.cs` |
| `src/Main.RadioProgramProduction.cs` |
| `src/Radio/FactionRadioHudPanel.cs` |
| `src/Radio/FactionRadioSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `heliograph` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `radio` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **20**
(CODEX_ONLY 7, GAMEPLAY_CONSUMED 8, OPTIONAL 1, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `heliograph.json` | UNRESOLVED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/radio_broadcast_rundowns.json` | CODEX_ONLY |
| `narrative/radio_mysteries_expansion.json` | CODEX_ONLY |
| `narrative/radio_scriptbook.json` | CODEX_ONLY |
| `narrative/radio_scripts_expansion.json` | CODEX_ONLY |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_betrayed_trust` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 5 (laddered 1) · RNG streams 1 · host files 14 · catalogs 22 · test regions 1 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HELIOGRAPH-TRUTH-235
wave: 17
status: PROPOSED — foreman claim required
packages: HGT-235A, HGT-235B, HGT-235C, HGT-235D, HGT-235E
claim paths:
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - src/Host/HeliographHostSession.cs  # §19 candidate host surface
  - src/Host/RadioCatalogSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_radio_corpus.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
  - godot --headless --path . -- --dose-ledger-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
