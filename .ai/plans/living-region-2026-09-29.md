# Feature / Task Plan: The Living Region — settlements, refugees and prices shift with the war

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_living_region_plan.md`. Family index: `docs/expansions/expansion_world_moves_without_you_index.md`.
> Nothing here is claimed, implemented, or committed. The foreman must add the `INTEGRATION_PLANS.md` entry and `WORKTREE_OWNERSHIP.md` claims per package. Decisions DEC-LR-01…10 are **proposals**, unsigned.

> **Editorial polish (prose pass):** sections **0**, **1b**, **1c** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `regional_pulse_lines.json` rows; they belong in data, never in code.

---

## 0. Prologue — The Region

> *"The war does not visit the shelter. The war is the weather the shelter is standing in."*

Twelve settlements exist in the data today as fixed points on a map: names, coordinates,
threat levels, attitudes. They are not places yet. A place is what a name becomes when something
is *happening* to it while you are looking somewhere else.

The Living Region is the plan that turns a map into a **situation**. Steady, Strained, Failing,
Emptied, Swollen — five rungs, each with a cause tag, each changing on its own schedule and in its
own direction. And the only way the player learns any of it is by hearsay: *Heard*, *Told*,
*Seen*. Three grades of not-quite-knowing.

**Tone & register.** Reportorial, distant, humane. The register is the bulletin board and the
traveller's account: what someone said, what someone confirmed, what someone saw. Prose should
hunger for certainty without supplying it. The world is not withholding; the world is *far away*,
and distance is the only censor this plan needs.

**Mystery & texture.** The Pulse projector takes an immutable snapshot and returns pillar states,
a rung, and a cause. It has no theory of the war and will not acquire one. DEC-LR-03 caps the
ladder at five rungs with **no "Thriving"** — which is the plan's quietest and most unsettling
authorial decision. Nothing gets better. It only stops getting worse.

**The second layer.** The three grades of hearsay — *Heard, Told, Seen* — are also three distances
from someone else's suffering. The plan never closes that distance and never pretends to. What the
Board offers is not knowledge but *attentiveness*: the willingness to worry accurately about a
place you will never visit. The war is weather; the Board is the window it is weather seen
through. Worrying accurately is the only aid this plan ever delivers, and it is enough to change
what the shelter does with its hands.

## 1. Goal & Outcome

> *Design intent: the player should be able to name three settlements they have never visited and
> worry about one of them. That is the entire feature.*

- **Goal:** Twelve authored settlements gain a small, persisted, war-aware condition (Steady / Strained / Failing / Emptied / Swollen); refugee waves move conserved population between regions and petition the shelter's gate; prices nudge through the market's existing shock seam; the player learns it all through graded news (Heard / Told / Seen).
- **Outcome (observable):** on a fixed seed, a scripted war-tension rise produces a wave, a rung change, a price nudge and a gate petition, in that order, identically on replay and after a save/load round-trip.
- **Non-Goals:** no new economy/price/ownership/population authority; no new save section; no new routed panel; no change to FactionWar outcomes; no restoration of retired code (Rule 10); no Year One pacing change when ship-dark; no Unity.
- **"Done" means:** every acceptance row in §6 passes via `bin/run-scoped-tests`; ship-dark parity test passes; the handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**The five rungs are a sentence with no verbs.**

Steady / Strained / Failing / Emptied / Swollen. Note that *Emptied* is not the bottom and
*Swollen* is not the top. Both are arrivals, not ends. A settlement can be Swollen with refugees
and Strained by the same fact. Let the panel allow two rungs to be true in the reader's head even
when the data holds one.

**Heard / Told / Seen is the plan's epistemology.**

LR-P7 requires wrong-by-one *Heard* lines with a later correcting *Told* line. That correction is
the single most human mechanic in this family: the world does not lie to you, it just gets to you
late and slightly wrong, and then — to its credit — corrects itself. Keep the correction visible
in the surface. Do not silently overwrite.

**What the player is never told.**

- Why a settlement is Failing. The **cause tag** is always set on a rung change (§6.6) — but a cause
  is a category, not an explanation. `war`, `scarcity`, `plague` describe pressures, not decisions.
- Where the refugees go after the gate. LR-P6 offers open / ration / refuse / redirect and then
  stops. Stores decide feasibility; nobody records the rest.
- Whether the war is being won. `FactionWarSystem` is read-only here and the Pulse never asks it.
- What the four unmapped region vocabularies were for (E5). One mapping file is added and nothing
  is deleted.

**Voice — sample fragments (content candidates for `regional_pulse_lines.json`).**

> "Heard: the Iron Basin is Strained. Heard is not known. We are telling you anyway because you
> asked."

> "Told: correction on Ash Flats. Not Strained — Failing, since the eleventh. We are sorry. We were
> wrong by one and we have been wrong by one before."

> "Seen: Emptied. I walked through it. There is no word for what a place is when the rung is
> correct and the word is not enough."

> "Swollen is not thriving. Swollen is what it looks like when everyone else's Failing arrives at
> once."

**Design texture beats.**

- **Five rungs and no "Thriving" (DEC-LR-03).** This is the plan's thesis in a single enum. Do not
  extend it. A world with no word for *better* is the world this game is set in.
- **Population is conserved (LR-P4).** The property test is an ethic: nobody is created, nobody
  vanishes. A wave is a movement, not a spawn.
- **Every rung change has a cause tag and a Board line (§6.6).** Even bad news deserves to be
  attributable. That rule is the plan's fairness guarantee.
- **Prices nudge and decay to exactly neutral (LR-P5).** Economics in this world are weather —
  they pass through.

---

## 1c. The Deeper Layer — scenes, artifacts & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the shelter leaves lying around.**

> "Board, corner: Ash Flats — Heard, Strained. Under it, later, in a different pencil: not Strained."

> "Wave tally: 40 arrived. The tally is conserved arithmetic. The names are not on it and were never going to be."

> "Petition at the gate, folded twice. The gate answers four ways and the petition has already been read twice."

**Scenes the player may piece together.**

> "A settlement goes from Strained to Swollen in one entry and the Board does not comment. The Board's job is rungs, not grief."

> "The Seen line is written by someone who walked through it. The Seen lines are short."

**Held silences (texture, not register rows).**

- Why there is no *Thriving* rung beyond DEC-LR-03. The enum is the thesis; whether the world agrees with it is not consulted and must not be. Texture only.
- Who writes the corrections. A *Told* line overturns a *Heard* line from someone equally far away; the plan credits no one and the bulletin must never be given a face.

**Third pass — three fragments (texture only; §12 register unchanged).**

> "Heard: Failing. Told: Failing since the eleventh. The second line is longer and no kinder."

> "Population conserved. The wave arrives at the gate as arithmetic and leaves as someone's decision."

> "The Board's rung is a word. Words can be read aloud at a gate without starting anything."

**Fourth pass — three distances from someone else's suffering (texture only; §12 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §12 gains no row and loses no silence;
fragments remain content candidates for `regional_pulse_lines.json`.)*

**The shape of the polish.** The Board's grammar is the plan's whole ethics: a rung is a word, a
grade is a distance, and the prose should never let the two blur. *Heard* is third person and far;
*Told* is second person and late; *Seen* is first person and short. Keep that person discipline and
the reader will feel the distance closing without ever arriving.

**What the shelter leaves lying around.**

> "Board, bottom corner: a place name with no rung beside it. The name was written first. The rung
> is still being rumoured."

> "Seen line, one visit only. Whoever walked through it has not walked through anywhere else the
> Board knows about."

> "Correction, dated later than the thing it corrects. The Board's honesty has a lag, and the lag
> is published with the honesty."

**Held silences (texture, not register rows).**

- Where the Board is kept. It has a corner, two pencils and no room; the wall is not described and
  must not be. Texture only.
- What the war does on the days no line arrives. The Pulse is an immutable snapshot (§0); the days
  between snapshots are the region's own and must stay unreported.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Evolving world ticks in phase 4 `world_evolution`, incl. wildlife→scarcity and dominance→ownership. | `src/Main.CampaignOwners.cs` `EvolvingWorldDayOwner` (~L2150–2330) | LIVE |
| E2 | Migration engine persists `RegionWeights`, `LastAppliedPhase`, `AppliedTransitionKeys`; consequence engine persists exactly-once keys; both ticked by the campaign. | `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs`, `MigrationConsequenceEngine.cs`; `src/Main.MigrationConsequence.cs`; sections `human_migration`, `migration_consequence` in `Save/SaveSectionRegistry.cs` | LIVE |
| E3 | Market shock seam is idempotent and already used by migration. | `src/Main.MigrationConsequence.cs` `ApplyMigrationMarketConsequence` | LIVE |
| E4 | 12 settlements static; no mutable per-settlement state. | `settlements.json`; `Assets/Ashfall.Core/World/SettlementCatalog.cs` | GAP |
| E5 | Four unmapped region vocabularies. | `settlements.json`, `seasonal_human_migration.json`, `caravan_trade_routes.json`, `regional_prices.json`, `map_regions.json`, `world_evolution_seeds.json` | GAP |
| E6 | `FactionWarSystemState`: `activeWarTension`, `dominantFactionId`, standings, control %, defense pressures. | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | LIVE |
| E7 | Dominance→ownership loop re-owns only seeds authored to the new dominant. | `EvolvingWorldDayOwner` | VERIFY |
| E8 | `PatrolTerritoryAuthority` has no `src/` reference. | grep `src/` | GAP / VERIFY |
| E9 | Embargo rules (14) and weather shocks exist and decay. | `trade_embargoes.json`; `TradeEmbargoSystem.cs` | LIVE |
| E10 | Selftest verbs: `--evolving-world-selftest`, `--world-evolution-selftest`, `--human-migration-selftest`, `--migration-consequence-selftest`, `--outpost-settlement-selftest`. | `src/Host/HostCli*.cs` | LIVE (VERIFY current arguments) |
| E11 | Gate/arrival authority for strangers at the shelter. **Found (2026-09-29, later pass):** three existing systems — `AirlockSecuritySystem` (`VisitorArrives`, `ResolveIncident`: Admit/Inspect/Quarantine/TurnAway/Defend; section `airlock_security`), `DoorEncounterSystem` (80 authored knocks), `VisitorIntegrationSystem` (the stay; `SourceVisitorId` handoff). | `Assets/Ashfall.Core/AirlockSecuritySystem.cs`; `YearOfAsh/DoorEncounterSystem.cs`; `Visitors/VisitorIntegrationSystem.cs` | **VERIFY (P0)** — confirm the live call path; agree **one shared gate adapter** with *The Quiet War* (QW-P0) and *The Plague Year* (PY-P3/P4); do not build a second |

## 3. Authority table (one authority per concern)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Population weights per region | `SeasonalHumanMigrationEngine` (`human_migration`) | nested `settlementLive[]` + `waves[]` (additive) — **DEC-LR-02** |
| Market price nudges | market owner's shock seam (E3) | calls only |
| Location ownership | `LocationEvolutionSystem` via `EvolvingWorldDayOwner` | truthful conquest **only if DEC-LR-05 approves** |
| War state | `FactionWarSystem` | read-only |
| Route/embargo state | `TradeEmbargoSystem`, `CaravanTradeNetworkSystem` | read-only |
| Health input | `DiseaseSystem` (Plague Year) | read-only, neutral if absent |
| Strangers at the door | E11 owner | one adapter |
| Region vocabulary | new data `region_vocabulary.json` | one mapping file; nothing deleted |

## 4. Claimed Paths & Affected Files (proposed; foreman records claims. `INT` = integrator-owned shared seam)

**Core (engine-free, netstandard2.1):**
- `Assets/Ashfall.Core/World/RegionVocabularyMap.cs` (new) + loader
- `Assets/Ashfall.Core/World/RegionalPulseProjector.cs` (new, pure)
- `Assets/Ashfall.Core/World/RefugeeWaveLedger.cs` (new, pure, deterministic)
- `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs` (additive nested state; public mutation API for conserved transfer)
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (`INT`)
- `Assets/Ashfall.Core/Random/` stream ids (`INT`)
**Data:** `Assets/StreamingAssets/Data/region_vocabulary.json`, `regional_pulse.json` (bands, thresholds, cause tags, wave kinds), `regional_pulse_lines.json` (prose)
**Host:** `src/Main.CampaignOwners.cs` (`INT`, one new day-owner registration in phase 4 after `world_evolution`), `src/Main.MigrationConsequence.cs` (`INT`, hook), one gate adapter (file from P0)
**Presentation:** existing atlas/trade/radio/journal surfaces only (**DEC-LR-06**)
**Tests:** `Ashfall.Core.Tests/World/RegionalPulseTests.cs`, `RefugeeWaveLedgerTests.cs`, `RegionVocabularyMapTests.cs`, `Ashfall.Core.Tests/Save/LivingRegionSaveTests.cs`

## 5. Packages

### LR-P0 — Premise audit (Auditor; read-only)
- Re-verify E1–E11 with `path:line`; close E7/E8/E11; census every consumer of `Population`, `threat_level`, `attitude` in `SettlementCatalog`; write `docs/plans/LIVING_REGION_PREMISE_AUDIT_<date>.md`; foreman signs DEC-LR-01…10.
- **Accept:** each VERIFY row is resolved or converted into a named blocker.

### LR-P1 — Region Vocabulary Map (Core + data)
- Canonical vocabulary = supply-tag set (`settlement, iron_basin, industrial_belt, ash_flats, deep_coast`); map settlements, price regions, sectors, `reg_*` onto it.
- **Accept:** every settlement, price-atlas region, and evolving-world sector resolves to exactly one canonical region; integrity validator fails on an unmapped id; absence of the file changes nothing.

### LR-P2 — Regional Pulse projector (Core, pure)
- Inputs (immutable snapshot): scarcity deltas, embargo/route state, war tension + dominance, settlement authored threat, health flag, migration weight vs baseline.
- Output per settlement: pillar states, rung, **cause tag**. Hysteresis: 5 consecutive days.
- **Accept:** table-driven tests; same inputs → same output; no RNG; no I/O; no Godot; cause tag always set on a rung change.

### LR-P3 — Persisted live state + save (Core)
- Nested `settlementLive[]` (`settlementId, populationDelta, rung, rungSinceDay, causeTag, refugeesHosted`) and `waves[]` in the migration save state; schema bump with default-empty read.
- **Accept:** round-trip identical; old save loads; no new section in `SaveSectionRegistry`; ship-dark parity (no catalog → byte-identical saves to today).

### LR-P4 — Refugee wave ledger (Core)
- Wave kinds authored in `regional_pulse.json`; exactly-once keys; population **conserved** through the engine's own mutation API.
- Deterministic randomness only via a `CampaignStreamIds` fork (new id is `INT`); no `System.Random`.
- **Accept:** property test — total population weight is invariant across any wave sequence; replay with the same seed gives the same waves.

### LR-P5 — Day-owner + market seam (Host, `INT`)
- Register one owner after `world_evolution` in phase 4; on rung change or wave landing call the market shock seam with bounded (proposed 800–1400 permille), decaying, exactly-once nudges.
- **Accept:** nudge decays to exactly neutral; repeated day tick is idempotent; prices stay in [500, 2000].

### LR-P6 — Gate petitions (Host adapter)
- Waves become ≤N petitions/day through the E11 owner: open / ration / refuse / redirect. Stores decide feasibility.
- **Accept:** refusing changes standing/memory only through existing seams; no petition when the shelter is closed; save/load mid-wave preserves remaining petitions.

### LR-P7 — Graded news (Presentation)
- Heard/Told/Seen views over the same state; wrong-by-one Heard lines with later correcting Told line.
- **Accept:** presenter tests prove Heard never exposes exact rung; Seen equals state; keyboard/controller focus unaffected on any touched panel.

### LR-P8 — War settlement profile hook (optional; depends on Year Two P1B)
- Read `war_settlement` (armistice / partition / siege / ashes) from the active Chapter Profile; legacy = siege at current values.
- **Accept:** legacy save behaviour unchanged; missing profile = legacy.

### LR-P9 — Content waves W1–W4 and governance close
- Prose files per the companion §9; validator + voice lock; plan header `FULLY INTEGRATED` ×3 and archival only when the integrator accepts.

## 6. Acceptance criteria (integration proof, not compile-green)
1. Core authority, host owner, event path, persistence and an observable outcome agree for each package (CLAUDE.md "integrated" definition).
2. Deterministic replay: two runs, same seed, identical Pulse/wave/price sequence.
3. Save round-trip mid-wave.
4. Ship-dark parity: with none of the new data present, `--evolving-world-selftest`, `--human-migration-selftest`, `--migration-consequence-selftest` outputs are unchanged.
5. Conservation property (LR-P4).
6. Every rung change has a cause tag and a Board line.

## 7. Cross-plan boundaries
- **Year Two (P6 The Road):** hostile pressure on outposts stays with `OutpostPressureModel`; the Pulse may *supply* a regional-tension read-only input. The Pulse never attacks an outpost.
- **The Long Line: Freight:** routes' blocked/slow state is an input; the company never writes Pulse state.
- **The Plague Year:** outbreak declared → Health pillar fails; Plague Year owns quarantine politics.
- **The Drowned Coast:** flooded-out waves and coastal `deep_coast` harbours are consumers.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-LR-01 | Canonical region vocabulary = supply-tag set. | architecture | Yes |
| DEC-LR-02 | Live state nests in `human_migration` (not a new section, not `migration_consequence`). | architecture | Yes; decide in P0 |
| DEC-LR-03 | Rung hysteresis 5 days; five rungs, no "Thriving". | design | Yes |
| DEC-LR-04 | News grades are presentation-only. | design | Yes |
| DEC-LR-05 | Conquest is truthful (owner flips when a war outcome earns it) vs. leave the E7 loop as is. | design | Decide after E7 |
| DEC-LR-06 | No new routed panel; extend existing surfaces. | UI | Yes |
| DEC-LR-07 | Price nudge bounds 800–1400 permille. | tuning | Yes |
| DEC-LR-08 | Waves may only be met on the road via existing encounter authorities. | scope | Yes |
| DEC-LR-09 | Health pillar soft-reads Plague Year; neutral if absent. | compatibility | Yes |
| DEC-LR-10 | Chapter Profile `war_settlement` is optional (LR-P8). | compatibility | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search source and data for `Refugee`, `Pulse`, `SettlementLive`)
- [ ] Premise re-verified (Rule 7); `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md` re-read; no overlapping live claim
- [ ] Signed decisions for the package in hand (§8)

## 10. Verification
- [ ] `bin/run-scoped-tests` on the new test files above plus existing migration/consequence/evolving-world tests (exact list from P0's changed-file selector)
- [ ] `--evolving-world-selftest`, `--human-migration-selftest`, `--migration-consequence-selftest` (VERIFY arguments)
- [ ] Scoped tests only (<30 s each); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: E11 has no existing owner (a second gate system would be needed); population conservation cannot be expressed through the engine's API without a second store; the vocabulary map needs a new region concept; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the region larger than the twelve points that meter it. Any future plan that answers one must
name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| LR-OM-1 | Why is there no "Thriving" rung? | DEC-LR-03 is a design rule, and it is also the plan's entire worldview. Adding one would be a tone change, not a feature. | Never — locked by decision. |
| LR-OM-2 | Where do redirected refugees go? | LR-P6 ends at the gate. Stores decide feasibility; the ledger conserves population and does not narrate destinations. | Never — conservation is the point. |
| LR-OM-3 | What were the four unmapped region vocabularies for? | E5 is a real inconsistency. One mapping file is added and **nothing is deleted**. The old vocabularies survive untranslated. | Never — the artefact reads as history. |
| LR-OM-4 | Is `Emptied` a state or a verdict? | The rung derives from inputs with five-day hysteresis. Whether anyone in the fiction uses the word as a judgement is not authored. | *The Record Keepers*, if a Board line is ever archived. |
| LR-OM-5 | Does the Pulse know about the shelter? | Its inputs are regional and its outputs are per-settlement. The shelter is a gate and a market, never an input. | Never — a rule, not a gap. |
| LR-OM-6 | Why do *Heard* lines get it wrong by exactly one? | LR-P7 requires it. The bias is authored as a mechanism and never as a character flaw in whoever is reporting. | Never — texture by omission. |
