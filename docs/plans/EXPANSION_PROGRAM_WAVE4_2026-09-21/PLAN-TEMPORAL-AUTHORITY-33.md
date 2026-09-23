# PLAN-TEMPORAL-AUTHORITY-33 — One Clock, One Order, Predictable Catch-Up

**Wave:** 4 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-DETERMINISM-REPLAY-13, PLAN-SAVE-GOVERNANCE-12.
**Expanded appendix:** [`PLAN-TEMPORAL-AUTHORITY-33_APPENDIX-A_HOUR_CONSUMERS.md`](PLAN-TEMPORAL-AUTHORITY-33_APPENDIX-A_HOUR_CONSUMERS.md)
— hour-consumer inventory: **19 files** referencing `TickHour`/`HourOfDay` (TM-33A clock-source audit input).

**Non-goals:** no real-time multiplayer semantics, no second calendar, no
frame-timing changes.

---

## 1. Outcome

Time in ASHFALL has several representations: a sim clock, a campaign day, an
hour-of-day, seasons, an authored war timeline offset (playable day 180 →
authored 480), and wall-clock reads. The ledger shows this has already caused
two repairs: the war-chain clock alias (`FactionWarChainRunner.ToAuthoredDay`)
and the unreachable `SchedulePhase.Night` (`ShelterScheduleSystem.PhaseForHour`
/ `TickHour`).

This plan names **one temporal authority per concept**, declares tick ordering,
and makes load/catch-up deterministic.

Deliverables:

1. a **time-source inventory**: every representation, its owner, its
   converters, and its consumers;
2. a **tick-order contract**: day owners declare phase + order; the order is
   stable, documented, and asserted;
3. **catch-up rules**: load, fast-forward, and pause cannot double-tick or skip
   an owner; a mid-day reload resumes at the same owner boundary;
4. **Core wall-clock elimination**: inject the existing clock port instead of
   `DateTime.Now` in simulation paths;
5. **offset aliases**: playable↔authored-day mapping is a read-only helper with
   tests, not ad-hoc arithmetic per system.

---

## 2. Evidence (2026-09-21)

| Fact | Value | Command |
|---|---:|---|
| Sim clock authority | `SimClock` (`Clock/ISimClock.cs`) | file |
| Campaign day coordinator | `Campaign/CampaignDayCoordinator.cs` + save/codec | file |
| Day-owner references | 104 | `grep -c DayOwner src/Main.CampaignOwners.cs` |
| Files using hour/tick-hour | 45 | `grep -rln "TickHour\|HourOfDay"` |
| Wall-clock reads in Core | 12 | `grep -rn "DateTime.Now\|DateTime.UtcNow" Assets/Ashfall.Core` |
| Wall-clock port exists | `IWallClock` | `Assets/Ashfall.Core/IWallClock.cs` |
| Prior repairs | war clock offset (playable 180 → authored 480), `PhaseForHour` night fix | `KNOWN_DEBT` |
| Ordering evidence | day owners carry phase/priority (e.g. hygiene before medical) | owner declarations |

---

## 3. Packages

### TM-33A — Time-source inventory
- `docs/architecture/TEMPORAL_AUTHORITY.md`: each time concept —
  `sim_tick`, `campaign_day`, `hour_of_day`, `season`, `authored_war_day`,
  `wall_clock` — with owner, storage, converters, readers, and mutation rule.
- **Acceptance:** every `DateTime` read, `TickHour` consumer, and day counter
  maps to a row; no unnamed time source.
- **Verify:** generator `--check`.

### TM-33B — Tick order contract
- Publish the ordered owner list with phase, dependency reason, and
  once-per-day guarantee. Extend the triad/owner gate to assert the order has
  not changed silently; order changes require a plan note.
- **Acceptance:** a new owner added without a declared position fails the gate;
  the published order matches runtime registration.
- **Verify:** `godot --headless --path . -- --owner-order-selftest` (new) +
  focused triad gate.

### TM-33C — Catch-up and pause semantics
- Define: day advance is atomic (all owners exactly once); load resumes at the
  next owner boundary; fast-forward loops whole days only; pause stops the
  coordinator without mutating RNG.
- **Acceptance:** a 30-day run with random mid-day save/reload equals the
  continuous run (extends the DR-13B harness to boundary placement); no owner
  runs twice on load.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/` +
  `godot --headless --path . -- --campaign-soak-selftest`.

### TM-33D — Core wall-clock elimination
- Replace the 12 Core `DateTime` reads with the `IWallClock` port (or delete
  where the value was unused); simulation state must never depend on wall
  clock.
- **Acceptance:** zero `DateTime.Now/UtcNow` in Core simulation paths; the RNG
  and checksum tests are unaffected; a test proves two runs at different wall
  times produce identical state.
- **Verify:** `grep` gate + determinism suite.

### TM-33E — Offset and alias helpers
- One read-only helper for playable↔authored day conversion, used by war/
  chronicle/thermal consumers; tests pin the 180→480 mapping and clamping
  rules.
- **Acceptance:** no ad-hoc `+ 300` arithmetic outside the helper; the helper
  has boundary tests at day 1, 180, 360, 480.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarClockTests.cs`.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Reordering owners changes outcomes | order frozen; changes are explicit packages with replay proof |
| Wall-clock injection touches many files | only 12 sites; inject the existing port; no new abstraction |
| Catch-up probe expensive | boundary placements sampled (5 positions) in the 30-day run |
| Alias helper changes existing arithmetic | pin current behavior first (characterization), then delegate |

## 5. Verification

```bash
python3 scripts/ci/generate-temporal-authority.py --check
godot --headless --path . -- --owner-order-selftest
godot --headless --path . -- --campaign-soak-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarClockTests.cs
```

---

## 6. Expanded census (3 files · 976 lines)

Scope: `Assets/Ashfall.Core/Campaign/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignCalendar.cs` | 400 | Support | — | 0 | 0 | 0 |
| `CampaignCalendarReadModel.cs` | 63 | Support | — | 0 | 0 | 0 |
| `CampaignDayCoordinator.cs` | 513 | System | **yes** | 0 | 0 | 8 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `water_clock_orifice_silt_records.json` | array[7] |

**State surfaces:** `CampaignDayCoordinator.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Campaign/` |
| Test references | 12 name references across the test tree |
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
Domain files: 3. Other plans referencing them: **2**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-CAMPAIGN-FAMILY-TRUTH-272` | 3 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `TM-33A` | no name match — resolve at claim time |
| `TM-33B` | no name match — resolve at claim time |
| `TM-33C` | no name match — resolve at claim time |
| `TM-33D` | no name match — resolve at claim time |
| `TM-33E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **7** · Test files: **9** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/CampaignDayPersistenceAdapter.cs`, `src/Host/HostCli.WorldPlaytest.cs`, `src/Main.Campaign.cs`, `src/Main.Holdfast.cs`, `src/Main.UiTests.RealCampaignJourney.cs` |
| Tests (`Ashfall.Core.Tests/`) | 9 | `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`, `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorSourceGateTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignRngStreamTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **3**; isolated: **0**.

| From | → To |
|---|---|
| `CampaignCalendar` | `CampaignCalendarReadModel` |
| `CampaignCalendar` | `CampaignDayCoordinator` |
| `CampaignDayCoordinator` | `CampaignCalendar` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (32 files, 187 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |

**Verdict:** 187 cases sit under matching regions — run those first (`Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.UiTests.RealCampaignJourney.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |

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

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 7 · catalogs 1 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TEMPORAL-AUTHORITY-33
wave: —
status: PROPOSED — foreman claim required
packages: TM-33A, TM-33B, TM-33C, TM-33D, TM-33E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Main.Campaign.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
