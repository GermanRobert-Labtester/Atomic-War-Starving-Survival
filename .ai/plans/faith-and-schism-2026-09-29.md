# Feature / Task Plan: Faith and Schism — sects, the Question, and a ladder whose rungs bite (over the existing zealotry owner)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_faith_and_schism_plan.md`. Family index: `docs/expansions/expansion_new_pressures_and_places_index.md`.
> Not a claim. `ZealotrySystem` (Plan 175), the ritual calendar (Expansion 13, integrated), `IdeologicalFrictionSystem`, `BeliefStanceBridge`, `MoraleContagionSystem` and the governance blocs each keep their meaning. This plan **makes the escalation ladder consequential and reachable** and adds authored sect rows and a partition rule. It introduces no second belief, ritual or conflict authority.

> **Editorial polish (prose pass):** sections **0**, **1b** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `schism_lines.json` rows; they belong in data, never in code. DEC-FS-10
> governs: fictional movements only, both halves authored as reasonable.

---

## 0. Prologue — The Question

> *"A faith is a story a shelter agrees to keep telling. A schism is the moment somebody notices
> the story has a hole in it — and keeps the hole."*

Nothing here is a real religion and nothing here is a real argument. What the plan stages is the
small, terrible mechanics of *disagreement under scarcity*: two people who agree on every fact and
still cannot share a bunk.

The Question is the plan's heart. A movement asks its own blind spot out loud — once, formally,
with a season on the clock — and the player answers. Whatever they answer, the answer becomes a
position, and positions have owners, and owners have rooms, and rooms have bunks. That is the
whole catastrophe: it starts with seating.

**Tone & register.** Earnest, weighty, and fair. The narrator is never ironic about belief.
Every movement is authored as *reasonable* — including the one that splits away. Prose should give
both halves their best sentence. The horror of a schism is not that someone is wrong; it is that
everyone can count.

**Mystery & texture.** The escalation ladder is deliberately unreachable at its last rung (E3:
Schism is clamped). Whether that clamp is a guard or a wound is a governance question (DEC-FS-05)
and the plan refuses to prejudge it. That refusal is the plan's tone in miniature: *we do not know
whether the ceiling was built to protect you.*

## 1. Goal & Outcome

> *Design intent: a ladder whose rungs bite. Each stage should cost something the player can name
> before the split costs something they cannot.*

- **Goal:** (a) Make each rung of `ZealotEscalationStage` apply a bounded, table-driven effect through its existing owner; (b) let a movement **split** into an authored **sect** through a seeded, deterministic partition rule, preceded by an authored **Question** the player can answer; (c) give the player real verbs (answer, mediate, joint rite, grant a room, bunk by creed, edict, exile); (d) expose pilgrims/missionaries at the gate and bounded outward effects.
- **Outcome (observable):** on a fixed seed a movement whose Leader's dissent ≥ 60 (or Devout average ≥ 50) at *Ostracism*+ for four days with a season-old unanswered Question partitions into parent and sect (each ≥ 2 believers) by a seeded weighted draw, changing belief ids only through the belief owner's public method; each rung applies its table effect and no other; answering the Question delays the split; bunk-by-creed reduces roommate friction through the friction owner; with no sect rows and no effect table the ladder and rites behave identically to today; save/load mid-schism round-trips.
- **Non-Goals:** no real religion or proselytising language; no change to conversion, fervor decay or ritual cooldown maths; no change to morale-contagion schism or governance bloc logic; no auto-combat (assault threat is an *offer* to the encounter authority); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**The Question as a literary device.**

Three Questions, one per movement, each naming a blind spot the movement itself cannot see. The
player answers with a stance — not a solution. An unanswered Question is not a pending task; it is
a *season of weather* on the community, and when the season ends the community has changed shape.
The question should be short enough to quote and hard enough to avoid.

**Both halves are reasonable.**

DEC-FS-10 is a writing constraint with teeth: author the sect's creed with the same care as the
parent's. A sect that reads as a villain is a failure of authoring. The partition rule is seeded
and weighted — the draw decides *who leaves*, never *who was right*.

**What the player is never told.**

- Whether the Leader's dissent was caused by the Question or merely correlated with it. The rule is
  a threshold, not a motive.
- Whether bunk-by-creed is kindness or segregation. It lowers friction and raises isolation by
  data, and the data refuses to adjudicate.
- What the Devout believe that the Adherents do not. Role is a mechanical field. Doctrine is not
  authored at that resolution and must not be.
- Whether the schism was preventable. Sixty days is a long time to answer a question. The plan does
  not say the answer would have mattered.

**Voice — sample fragments (content candidates for `schism_lines.json`).**

> "The Question has been on the board for one hundred and nine days. It has stopped being a
> question. It has become a season."

> "They asked for a room. We gave them the east room. We called it generosity and they called it
> distance, and we were both describing the same door."

> "The rite was joint. The singing was not."

> "Two believers left on the ninth. Two remained on the ninth. The set is conserved. Nothing else
> is."

**Design texture beats.**

- **Stage effects must feel like consequences of the last stage, not new content.** Argument →
  Ostracism → Work refusal is a *tightening*, and the panel should show the ladder as one object.
- **The partition is arithmetic and grief at once.** Show the numbers (≥2 each side) plainly and
  the names softly. Do not gamify the draw.
- **Assault threat is an offer, never a fight (DEC-FS-04).** The player must be able to look away.
  That restraint is what separates a tone piece from a spectacle.
- **Exile is the only verb with no return path.** Give it the longest confirmation beat in the
  panel and the plainest wording.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | 3 movements authored (creed, comforts, blind spots, practices, `conflict_profiles`, `faction_leaning`) and 3 mechanical profiles. | `belief_movements.json`; `wasteland_religions.json`; `Spiritual/SpiritualModels.cs` L77–100 | LIVE |
| E2 | Believer state: conviction, fervor, dissent, role (Adherent/Devout/Leader), crisis; shrines; ritual demand/fulfil/fail; broadcast; leader registration; conversion. | `Survivors/ZealotrySystem.cs` L59–130, L203–397 | LIVE |
| E3 | Escalation ladder None→Argument→Ostracism→WorkRefusal→PropertyDamage→AssaultThreat→**Schism**; advances only while an opposing fervent pair exists; the advance is clamped to AssaultThreat, so **Schism is unreachable**. | `ZealotrySystem.cs` L48–56, L502–522, L526–545 | LIVE / GAP (prove with a test) |
| E4 | The only subscriber to `OnEscalationStageChanged` writes a journal line. | `src/Main.Zealotry.cs` L162–170; grep | LIVE / GAP |
| E5 | Opposing pairs come from a static in-code table keyed by belief ids (incl. the 3 movements and the older profiles). | `Survivors/IdeologicalFrictionSystem.cs` L34–60 | LIVE |
| E6 | Roommate friction ticking exists. | `IdeologicalFrictionSystem.cs` L85, L120; `src/Host/IdeologicalFrictionHostSession.cs` | LIVE (VERIFY how rooming feeds it) |
| E7 | Belief → faction trust bridge with a per-(belief, faction, day) budget; never writes standing. | `Spiritual/BeliefStanceBridge.cs` L45–60 | LIVE |
| E8 | Morale-contagion schism: duty-role subgroup despair, cooldown 21 days, `OnMoraleSchismTriggered`. | `Survivors/MoraleContagionSystem.cs` L195–199, L463–529 | LIVE |
| E9 | Governance has an incident kind `IdeologicalSchism` and ideological blocs with grievance basis points. | `Governance/ShelterGovernanceEngine.cs` L11–14, L27–51 | LIVE (VERIFY incident API) |
| E10 | 19 rituals + cooldown ledger; calendar engine (Expansion 13, integrated 2026-09-26). | `spiritual_rituals.json`; `Spiritual/SpiritualRitualCalendarEngine.cs`; save section `spiritual_ritual` | LIVE |
| E11 | `ideological_events.json`: 8 events incl. `event_formal_mediation_quest`, `event_bunker_faction_schism`. | data | LIVE (VERIFY consumers) |
| E12 | Save owner `zealotry` (checksummed store). | `Save/SaveSectionRegistry.cs` L199; `src/Host/ZealotrySaveStore.cs` | LIVE |
| E13 | A public method to reassign a believer's `belief_id`. | grep — none seen | **GAP (P0)** |
| E14 | Justice public API to open an incident (Sabotage) with a clue. | `Narrative/JusticeSystem.cs` L203–216 | LIVE |
| E15 | Combat/encounter authority accepts an *offer* from a host caller without starting a fight. | `src/Main.Muster.cs` pattern; encounter authority | **VERIFY (P0)** |
| E16 | Policy scope for belief edicts. | `PolicySystem`; `assembly_scope_map` (Shelter Governance plan) | **VERIFY (P0)** |
| E17 | Panel: `BeliefsPanel`. | `src/UI/BeliefsPanel.cs` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Who believes what | `ZealotrySystem` | additive public reassignment method (E13); nested `schisms[]`, `questions[]` DTO fields |
| Opposing pairs | `IdeologicalFrictionSystem` | data-driven `conflicts_with[]` on the profile (or one-time extension of the static table) — **DEC-FS-03** |
| Rites | Expansion 13 ledger | sect rite sets as additive rows |
| Faction trust | `FactionStanceEngine` via `BeliefStanceBridge` | none |
| Grievance/incidents/blocs | governance owner | one incident via its public API |
| Crime | `JusticeSystem` | one incident via its public API |
| Encounters | encounter authority | an offer only |
| Sects, Questions, stage-effect table, partition | — | `SchismEngine` (pure Core) + data; state nested in `zealotry` — **DEC-FS-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Survivors/SchismEngine.cs` (new, pure), `Survivors/SchismCatalog.cs` (new), `Survivors/ZealotrySystem.cs` (additive method + nested DTO fields; ladder change only under DEC-FS-05), `Survivors/IdeologicalFrictionSystem.cs` (additive data-driven pair source, **INT** if shared), `Spiritual/SpiritualModels.cs` (additive fields), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `belief_sects.json` (new) or additive rows in `belief_movements.json` + `wasteland_religions.json` (owner files, **INT**), `schism_questions.json`, `schism_stage_effects.json`, `schism_lines.json`, additive rite rows in `spiritual_rituals.json`
**Host:** `src/Main.Zealotry.cs` (`INT`, stage-effect wiring, day tick), `src/Host/IdeologicalFrictionHostSession.cs` (`INT`), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** extend `BeliefsPanel` (Question, verbs, sect roster) — **DEC-FS-08**; no new routed panel
**Tests:** `Ashfall.Core.Tests/Survivors/SchismEngineTests.cs`, `SchismStageEffectsTests.cs`, `SchismPartitionTests.cs`, `Ashfall.Core.Tests/Save/SchismSaveTests.cs`; extend zealotry and friction tests

## 5. Packages

### FS-P0 — Premise audit (Auditor; read-only)
- Close E13, E15, E16 and the E3 stage-cap question (git history/tests: was the cap deliberate?); confirm the static table vs `conflict_profiles`; how rooming feeds roommate ticking; governance incident API; which sources call `ideological_events`; blocs vs movements (no duplicate authority); enumerate every reader of `ZealotrySystemState`; foreman signs DEC-FS-01…10.
- **Accept:** each VERIFY answered with `path:line` or a test; DEC-FS-03/05 chosen; no overlap with a live claim.

### FS-P1 — Sects & Questions (data + Core)
- Six authored sect rows (parent link, `conflicts_with[]`, rite set) and three Questions (stances, costs, season timer); validator registration; snake_case; dormant until a schism.
- **Accept:** validator green; a sect id is unreachable without a schism; each Question names its movement and blind spot.

### FS-P2 — The rule of the split (Core, pure)
- Partition rule (thresholds, cooldown 60 days), seeded weighted draw (`CampaignStreamIds` fork keyed `(day, beliefId)`), ≥ 2 each side else expulsion path; reassignment via the E13 public method.
- **Accept:** same seed → same partition; ship dark with no sect rows; belief conservation (before = after by identity); ladder change (DEC-FS-05) proven by a targeted test.

### FS-P3 — Stage effects (Core + host)
- `schism_stage_effects.json`: Argument (morale), Ostracism (pairwise friction), Work refusal (governance incident), Property damage (justice incident), Assault threat (encounter *offer*), Schism (partition). Replace the journal-only subscriber with an effect router that also journals.
- **Accept:** each rung's effect bounded and table-driven; no effect writes outside its owner's public API; with no table → today's journal-only behaviour.

### FS-P4 — Player verbs (Core + host)
- Answer the Question (3 stances), mediate (existing quest), joint rite (existing ledger), grant a room (shrine tags), bunk by creed (rooming through the friction owner), edict (E16), exile (justice), let it run.
- **Accept:** each verb changes only what it names; bunk-by-creed lowers friction and raises isolation by data; edict absent when E16 fails (hidden, not faked).

### FS-P5 — Outcomes (Core + content)
- Reconciled / cold peace / departure / purge; departure emits a signal for a settlement seed (soft).
- **Accept:** each outcome reachable in a test; departure removes believers through owners' public APIs and conserves survivors.

### FS-P6 — Pilgrims & missionaries (soft, gate adapter)
- Claim rows tied to movement ids; a schism becomes news; the faction bridge moves standing only by its existing budget.
- **Accept:** a no-op when the gate adapter is absent; no direct writes to gate or standing.

### FS-P7 — Generations & chronicle (soft)
- Lineage "schism" event on split; child creed inheritance hook (Year Two).
- **Accept:** no-op without Year Two; one lineage event per schism.

### FS-P8 — Presentation
- Extend `BeliefsPanel`: Question, verbs, sect roster; focus/back preserved.
- **Accept:** presenter tests; panel holds no authority.

### FS-P9 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no sect rows/effect table → escalation, conversion and rites identical on a saved corpus.
3. Conservation: partition preserves the set of believers; nobody is created.
4. Determinism: identical partitions, Question timing and stage effects on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-schism (with a Question pending and bunks assigned).
6. No writes to survivors' relationships, duty, justice, governance or standing except via owners' public APIs.
7. Morale-contagion schism unchanged; a same-day precedence rule (DEC-FS-06) is tested.

## 7. Cross-plan boundaries
- **Expansion 13:** rites/ledger theirs; additive rows only.
- **Shelter Governance:** edicts via its `belief` scope; else no law.
- **The Quiet War:** pilgrims/missionaries via the gate adapter.
- **The Deep / The Sky:** read-only interpretation sources.
- **Radio Free Ashfall:** broadcast reach through the belief owner's method only.
- **Year Two:** creed inheritance hook.
- **The Living Region / The Record Keepers:** read-only.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-FS-01 | Schism and Question state nest in the `zealotry` save DTO; no new section. | architecture | Yes; confirm in P0 |
| DEC-FS-02 | Split = seeded partition of existing believers via a public reassignment method; nobody created. | rule | Yes |
| DEC-FS-03 | Conflict pairs data-driven (`conflicts_with[]`); the static table extended at most once. | architecture | Decide in P0 |
| DEC-FS-04 | Assault threat is an encounter *offer*, never an automatic fight. | rule | Yes |
| DEC-FS-05 | Lift the ladder cap only if P0 proves it was not a deliberate guard. | governance | P0 decides |
| DEC-FS-06 | Belief schism and morale-contagion schism never fire from the same cause on the same day. | rule | Yes |
| DEC-FS-07 | Edicts through Shelter Governance's `belief` scope if present; else none. | boundary | Yes |
| DEC-FS-08 | No new routed panel; extend `BeliefsPanel`. | UI | Yes |
| DEC-FS-09 | Pilgrims/missionaries via the shared gate adapter (soft). | boundary | Yes |
| DEC-FS-10 | Fictional movements only; both halves of a schism authored as reasonable. | tone | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Schism`, `Sect`, `Question`, `Partition`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim on zealotry/friction/governance paths
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing zealotry, ideological-friction, belief-stance, morale-contagion and spiritual-ritual tests (list from P0 selector)
- [ ] Zealotry and friction host selftests (VERIFY args in `HostCli` files)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the stage cap proves deliberate and no alternative path to a split exists; a partition would need to create or delete survivors; effects require writes outside owners' public APIs; a sect cannot be authored without editing the static table per sect and P0 rejects a one-time extension; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep belief larger than the ledger that meters it. Any future plan that answers one must name the
signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| FS-OM-1 | Why is `ZealotEscalationStage.Schism` clamped unreachable? | E3/DEC-FS-05 make this a governance question, not a bug. The ceiling may be a guard or a wound; the plan refuses to prejudge it. | Foreman, after P0 proves intent either way. |
| FS-OM-2 | What is the Question each movement cannot ask itself? | Authored Questions name a *blind spot*, not a doctrine. Filling the doctrine in would make the sect a caricature. | Never — DEC-FS-10's boundary. |
| FS-OM-3 | Do the ritual calendar's 19 rites predate the movements? | Expansion 13 owns the ledger and has already integrated. Sequence is not asserted anywhere. | Expansion 13's owner, if rite provenance is ever authored. |
| FS-OM-4 | Is `MoraleContagionSystem`'s schism the same event seen twice? | DEC-FS-06 says they never fire from one cause on one day. It does not say they are unrelated. Ambiguity is the point. | Never — a rule, not a gap. |
| FS-OM-5 | Why do the opposing pairs live in a static table? | E5 is an artefact. The plan offers `conflicts_with[]` as a fix and does not explain why the world's enmities were ever hard-coded. | Never — the artefact reads as canon. |
| FS-OM-6 | Do pilgrims come because of the schism, or were they already on the road? | FS-P6 makes claims *tied to movement ids* and nothing more. Causation is not modelled and must not be narrated. | The Quiet War's gate adapter, at its own discretion. |
