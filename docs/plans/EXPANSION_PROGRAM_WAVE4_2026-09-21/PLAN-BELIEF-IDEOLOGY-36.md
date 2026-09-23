# PLAN-BELIEF-IDEOLOGY-36 — Faith, Doctrine, Pilgrimage & Schism

**Wave:** 4 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-ORPHAN-SEAL-01 Wave 4,
PLAN-WARLORDS-DIPLOMACY-29.
**Expanded appendix:** [`PLAN-BELIEF-IDEOLOGY-36_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-BELIEF-IDEOLOGY-36_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's belief & ideology
systems (1 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real-world religions, no proselytizing framing toward real
groups, no second morale authority (`MoraleMarkSystem` remains canonical).

---

## 1. Outcome

The wasteland has beliefs but no believer loop. Data exists
(`wasteland_religions.json`, `belief_movements.json`, `ideological_events.json`,
`spiritual_rituals.json`) and authorities exist (`ZealotrySystem` sealed with
20/20, `SpiritualRitualCalendarEngine`, `IdeologicalFrictionSystem`,
`IdeologicalFrictionEvents`, `SpiritualMeaningCoordinator`), but the player
cannot adopt, practise, spread, dispute, or suffer faith.

**All content is fictional and original** (the `ZealotrySystem` loader already
enforces a fictional-only guard — reuse that precedent).

Player loop: **choose or inherit a belief → keep observances → face friction →
mediate or schism → pilgrimage → leave a legacy of faith**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Belief stance | `ZealotrySystem`, `belief_movements.json` | adopt/tolerate/suppress | fervour, resistance, conversion pressure |
| Ritual calendar | `SpiritualRitualCalendarEngine`, `spiritual_rituals.json` | keep/forgo rituals | morale, identity, tradition streak |
| Friction | `IdeologicalFrictionSystem`, `ideological_events.json` | mediate, side, exile | affinity, schism risk |
| Schism | `IdeologicalFrictionEvents` | split or reconcile | two congregations, shelter politics |
| Pilgrimage | map + expedition owners | travel to a site | discovery, danger, transformation |
| Meaning | `SpiritualMeaningCoordinator` | bury, memorialise | grief dispersion, chronicle |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Data | `wasteland_religions.json`, `belief_movements.json`, `ideological_events.json`, `spiritual_rituals.json` |
| Sealed prior | Plan 175 Zealotry (20/20 + 5/5 host), Plan 148 ideological friction (6/6), Plan 162 archive, `SpiritualMeaningCoordinator` wired into death fate |
| Host-unreachable | `SpiritualRitualCalendarEngine`, `IdeologicalFrictionSystem` |
| Guarantees | fictional-only loader guard; crisis → bounded morale route; violence stops at typed `AssaultThreat` (host routes) |
| Surfaces | chronicle panel, memorial, culture region (Wave 1 Plan 04) |

---

## 3. Packages

### BL-36A — Belief authority and factions
- Bind beliefs to factions (doctrine aligns with faction identity), to
  survivors (stance + fervour), and to the shelter (tolerance policy). No new
  morale store; effects route through morale marks and relations.
- **Acceptance:** stance changes are explainable; faction alignment shifts use
  the existing standing owner; fictional guard enforced.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/ZealotrySystemTests.cs`
  + faction suites.

### BL-36B — Conversion, resistance and crisis
- Conversion attempts scale with fervour, relationship, and framing; failures
  create friction, not free wins; crisis and violence stop at typed threats.
- **Acceptance:** determinism per seed; bounded deltas; a failed conversion has
  a relationship cost; no unbounded chain conversions.
- **Verify:** Zealotry host wiring tests + focused.

### BL-36C — Ritual calendar and observances
- `SpiritualRitualCalendarEngine` adds observances to the culture almanac
  (Plan 04): fasting, remembrance, vigils, processions — each with a small,
  bounded effect and a resource/opportunity cost.
- **Acceptance:** calendar visible; skipping has a cost; tradition streaks
  persist; overlaps with holidays resolved by one priority rule.
- **Verify:** `--culture-selftest` + focused.

### BL-36D — Pilgrimage and holy sites
- Authored sites (wasteland-native, fictional) on the map; pilgrimage is an
  expedition type with its own encounter band, discovery, and a transformation
  outcome (fervour, trait, relic, doubt). Reuses expedition/loot owners.
- **Acceptance:** sites are discoverable and reachable; outcomes deterministic;
  no new map authority; danger is forecast-visible.
- **Verify:** expedition suites + `--world-selftest`.

### BL-36E — Schism and mediation
- At defined thresholds (`IdeologicalFrictionEvents`), a schism splits
  congregation practice: shared quarters, separate rites, possible exile;
  mediation choices apply nuanced affinity/morale deltas.
- **Acceptance:** schism is reversible by reconciliation; the shelter keeps
  functioning (no hard lock); journal/chronicle coverage.
- **Verify:** `bash scripts/run_test.sh` ideological-friction tests + culture.

### BL-36F — Content volumes
- +6 belief movements, +12 rituals, +10 friction events, +5 pilgrimage sites,
  +20 doctrine lines; all fictional, all consumer-bound; content/utilization
  gates green.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Real-world resemblance | fictional-only guard + review; authored names/rites are original |
| Faith becomes a pure buff | costs, opportunity costs, friction, and resistance built in |
| Schism bricks the shelter | reversible mediation; duties reassigned, not deleted |
| Pilgrimage trivializes expeditions | site-specific hazards + limited frequency |

## 5. Verification

```bash
godot --headless --path . -- --culture-selftest
godot --headless --path . -- --data-integrity-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/
bash scripts/run_test.sh Ashfall.Core.Tests/Spiritual/
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/
```

---

## 6. Expanded census (4 files · 626 lines)

Scope: `Assets/Ashfall.Core/Spiritual/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 1 · Support 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SpiritualCatalogLoader.cs` | 116 | Loader | — | 0 | 0 | 0 |
| `SpiritualMeaningCoordinator.cs` | 207 | System | — | 0 | 0 | 2 |
| `SpiritualModels.cs` | 121 | Support | — | 0 | 0 | 0 |
| `SpiritualRitualCalendarEngine.cs` | 182 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `belief_movements.json` | object[2 keys] |
| `spiritual_rituals.json` | object[2 keys] |
| `bunker_rituals_and_cults.json` | object[3 keys] |

**State surfaces:** `SpiritualMeaningCoordinator.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Spiritual/` |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 4. Other plans referencing them: **4**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `BL-36A` | no name match — resolve at claim time |
| `BL-36B` | no name match — resolve at claim time |
| `BL-36C` | `SpiritualRitualCalendarEngine.cs`, `SpiritualCatalogLoader.cs`, `SpiritualMeaningCoordinator.cs` |
| `BL-36D` | no name match — resolve at claim time |
| `BL-36E` | no name match — resolve at claim time |
| `BL-36F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.Campaign.cs`, `src/Main.Spiritual.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Spiritual/Plan30SpiritualWorldTests.cs`, `Ashfall.Core.Tests/Spiritual/SpiritualRitualCalendarEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **0**; isolated: **4**.

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
| `spiritual` |
| `spiritual_meaning` |

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

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/belief_movements.json` |
| `Assets/StreamingAssets/Data/spiritual_rituals.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (2 files, 12 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Spiritual` | 2 | 12 |

**Verdict:** 12 cases sit under matching regions — run those first (`Spiritual`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **6**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/SpiritualSaveStore.cs` |
| `src/Main.Spiritual.cs` |
| `src/UI/PanelSceneLoader.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `spiritual` | no |
| `spiritual_meaning` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `belief_movements.json` | UNRESOLVED |
| `spiritual_rituals.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 6 · catalogs 4 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BELIEF-IDEOLOGY-36
wave: —
status: PROPOSED — foreman claim required
packages: BL-36A, BL-36B, BL-36C, BL-36D, BL-36E, BL-36F
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/Host/SpiritualSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/belief_movements.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/spiritual_rituals.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Spiritual/
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
