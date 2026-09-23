# PLAN-RECREATION-MORALE-50 — Downtime, Hobbies, Music, Games & Social Venues

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-FAMILY-DYNASTY-43,
PLAN-ORPHAN-SEAL-01 Wave 4.
**Non-goals:** no real sports leagues or copyrighted media, no second morale
authority (`MoraleMarkSystem`/needs remain canonical), no casino/gambling
loop that trivializes the economy.

---

## 1. Outcome

Morale is currently mostly a consequence of survival pressures; recreation is
fragmented across `Recreation/SurvivorDowntimeSystem.cs`, the host-unreachable
`HobbySystem` (10 hobbies, 4 mastery tiers, group sessions) and
`CassettePlaybackSystem`, plus live `VinylMoraleSystem` (with flashback
suppression), `SurvivorSocialCoordinator`, social events, and
`recreation.json` / `hobby_definitions.json`. This plan makes rest and play a
**deliberate allocation** with real payoffs and real opportunity costs.

Player loop: **schedule downtime → pick activities → build skill and bonds →
mark occasions → cope with trauma → return to work sharper**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Downtime | `SurvivorDowntimeSystem`, duty roster | allocate rest hours | fatigue, morale, output |
| Hobbies | `HobbySystem`, `hobby_definitions.json` | practise, join groups | mastery tiers, morale, affinity |
| Music & media | `VinylMoraleSystem`, `CassettePlaybackSystem` | play records/cassettes | shared mood, flashback risk, memories |
| Games & sport | `recreation.json` | organise a match/contest | bonds, rivalries, morale |
| Venues | mess/social room policies | set house rules | inclusion, conflict, cohesion |
| Coping | mental-health owners | therapy, faith, drink | trauma management, risk |
| Occasions | culture vertical | celebrate, remember | tradition, grief work |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `Recreation/SurvivorDowntimeSystem.cs`, `Survivors/HobbySystem.cs` (orphan, 5 tests), `Audio/CassettePlaybackSystem.cs` (orphan), `VinylMoraleSystem` (flashback suppression event), `SurvivorSocialCoordinator` (workouts/bonds), social event systems, mental-health/trauma owners |
| Data | `recreation.json`, `hobby_definitions.json`, `shelter_social_events.json`, `shelter_celebrations.json`, `survivor_voice_lines.json` |
| Sealed prior | Plan 161 hobby Core (5/5), Plan 216 exercise (via social coordinator), Plan 174 companion morale, Plan 42 voice lines, Plan 52/169 audio policy |
| Contracts | morale effects route through marks/needs; no parallel morale store; audio cues follow accessibility policy |

---

## 3. Packages

### RC-50A — Downtime allocation
- Rest/downtime is a schedulable resource competing with work and training;
  fatigue and morale respond with diminishing returns.
- **Acceptance:** allocation visible; overwork has consequences already modeled
  by duty fitness; no free morale farm.
- **Verify:** duty roster + needs focused suites.

### RC-50B — Hobbies and mastery
- Bind `HobbySystem` to rooms/facilities and group sessions; mastery tiers
  give small, bounded morale/bond bonuses and the occasional useful output
  (crafted keepsake, performance, collection).
- **Acceptance:** facility gating (`CanConductSession`) enforced; group
  affinity effects use relations; determinism via seeded progression.
- **Verify:** `HobbySystemTests` + `Plan161HobbyIntegrationTests` extend.

### RC-50C — Music and media playback
- Vinyl/cassette playback as a venue activity: curated fictional tracks, a
  shared mood effect, and careful trauma handling (a song can trigger a
  flashback — the existing suppression event becomes a choice).
- **Acceptance:** playback has cooldowns and no infinite loop; flashback path
  bounded; captions for any critical cue; no real copyrighted titles.
- **Verify:** `--audio-selftest` + mental-health suites.

### RC-50D — Games, contests and competitions
- Authored games (cards, board, sport, marksmanship, chess-like) with
  participants, stakes (small favours/keepsakes, never the economy), and
  rivalry/bond outcomes.
- **Acceptance:** outcomes deterministic; stakes bounded; no gambling spiral;
  content fictional.
- **Verify:** focused recreation tests + relations.

### RC-50E — Social venues and house rules
- Mess/social-room policies: who may attend, drink policy, curfew, quiet
  hours; policies trade cohesion against conflict and productivity.
- **Acceptance:** policies are visible and reversible; conflicts route through
  existing drama/friction owners; no duplicate social state.
- **Verify:** shelter social focused suites.

### RC-50F — Coping and recovery arcs
- Recreation supports trauma management alongside therapy/faith: steady
  downtime reduces stress accumulation; over-reliance carries a cost
  (dependency/escalation routed to existing mental-health owners).
- **Acceptance:** bounded, explainable; no cure-by-hobby; crisis paths remain
  canonical.
- **Verify:** mental-health crisis suites.

### RC-50G — Content volumes
- +8 hobbies, +12 activities, +10 games, +8 venue policies, +12 social event
  rows; fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Morale becomes idle-game | opportunity cost vs work; diminishing returns; caps |
| Gambling/economy exploit | stakes are social, never currency |
| Trauma mishandled | restrained framing; player choice; professional care still primary |
| Duplicate morale systems | everything routes through marks/needs/relations owners |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/
bash scripts/run_test.sh Ashfall.Core.Tests/Recreation/
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --data-integrity-selftest
```

---

## 6. Expanded census (5 files · 1,554 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Save 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MoraleMarkSystem.cs` | 176 | System | **yes** | 0 | 0 | 2 |
| `HobbySystem.cs` | 344 | System | **yes** | 0 | 0 | 2 |
| `MoraleContagionCatalog.cs` | 65 | Catalog | — | 0 | 0 | 0 |
| `MoraleContagionSave.cs` | 199 | Save | — | 0 | 0 | 0 |
| `MoraleContagionSystem.cs` | 770 | System | — | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `hobby_definitions.json` | object[2 keys] |

**State surfaces:** `MoraleMarkSystem.cs`, `HobbySystem.cs`, `MoraleContagionSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 16 name references across the test tree |
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

Domain files: 6. Other plans referencing their names: **13**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 4 |
| `EVIDENCE` | 3 |
| `PLAN-MORALE-CONTAGION-TRUTH-162` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-BELIEF-IDEOLOGY-36` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RC-50A` | no name match — resolve at claim time |
| `RC-50B` | no name match — resolve at claim time |
| `RC-50C` | no name match — resolve at claim time |
| `RC-50D` | no name match — resolve at claim time |
| `RC-50E` | no name match — resolve at claim time |
| `RC-50F` | no name match — resolve at claim time |
| `RC-50G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **3** · Test files: **14** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/DutyRosterHostSession.cs`, `src/Host/MoraleContagionHostSession.cs`, `src/Main.MoraleContagion.cs` |
| Tests (`Ashfall.Core.Tests/`) | 14 | `Ashfall.Core.Tests/DutyRoster/DutyRosterHostSessionTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`, `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs`, `Ashfall.Core.Tests/DutyRosterSaveTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **1**; isolated: **3**.

| From | → To |
|---|---|
| `MoraleContagionCatalog` | `MoraleContagionSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `morale` |
| `morale_contagion` |
| `recreation` |
| `vinyl_morale` |

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

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnMarkCleared` | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `OnMarkSet` | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `OnMoraleApplied` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnMoraleDelta` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` |
| `OnMoraleDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMoraleDrainRequested` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnPermanentMoraleBuffApplied` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/contagion_events.json` |
| `Assets/StreamingAssets/Data/hobby_definitions.json` |
| `Assets/StreamingAssets/Data/recreation.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 5 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Recreation` | 1 | 5 |

**Verdict:** 5 cases sit under matching regions — run those first (`Recreation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/MoraleContagionHostSession.cs` |
| `src/Host/MoraleContagionSaveStore.cs` |
| `src/Host/RecreationSaveStore.cs` |
| `src/Host/VinylMoraleHostSession.cs` |
| `src/Host/VinylMoraleSaveStore.cs` |
| `src/Main.MoraleContagion.cs` |
| `src/UI/VinylMoralePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `morale` | no |
| `morale_contagion` | no |
| `recreation` | no |
| `vinyl_morale` | no |

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
(GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `contagion_events.json` | UNRESOLVED |
| `recreation.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 13
**Surface:** save sections 4 (laddered 0) · RNG streams 0 · host files 7 · catalogs 5 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RECREATION-MORALE-50
wave: —
status: PROPOSED — foreman claim required
packages: RC-50A, RC-50B, RC-50C, RC-50D, RC-50E, RC-50F, RC-50G
claim paths:
  - src/Host/MoraleContagionHostSession.cs  # §19 candidate host surface
  - src/Host/MoraleContagionSaveStore.cs  # §19 candidate host surface
  - src/Host/RecreationSaveStore.cs  # §19 candidate host surface
  - src/Host/VinylMoraleHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/contagion_events.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/hobby_definitions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Recreation/
dependencies:
  - coordinate: 13 other plan(s) name these artifacts (§12)
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
