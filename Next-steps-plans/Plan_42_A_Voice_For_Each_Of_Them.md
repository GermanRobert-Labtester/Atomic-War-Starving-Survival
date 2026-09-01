# Plan 42 — A Voice for Each of Them: Survivors Who Say Things

> **Wave:** Continuity Wave 6 — *The People In It*
> **Depends on:** 40A (identity: belief, profession, keepsake, phantom), 41A (memory: what they have
> to be sad about), 31 (state transitions that trigger speech), 25A/25C (keyed text — this plan must
> not create a new class of hardcoded English).
>
> **Theme:** the game has 118 authored radio broadcasts giving voice to *nobody in particular*, 43
> authored duty-roster "marks" with inspect/bark sentences, a procedural eulogy engine, and 129
> survivor definitions with `bio` and `profession` — and **survivors never say anything**. There is
> no line-selection system keyed to who someone is and what just happened to them. The people the
> player is trying to keep alive are silent.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The only spoken-word content is external | 118 broadcasts across `radio.json` (50) + `year_of_ash_radio.json` (50) + `verdict_radio.json` (13) + `radio_distress_signals.json` (5); 10+ `audio_cue` VO references and 15+ VO assets produced in 7B |
| 2 | A bark/inspect sentence mechanism exists — for roster marks | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs:90` — *"Inspect/bark sentence from `duty_roster_marks.json`, else the saved payload"*; `DutyRosterCatalog.cs:118,125 MarksFile`; `duty_roster_marks.json` = 43 defs, consumed by 1 non-test `src/` file |
| 3 | Nothing analogous exists for survivors | `grep -rniE "voiceline\|voice_line\|bark\|quip\|dialogueTree" Assets/Ashfall.Core src/` → the only real hit is the roster-mark one above; `GetBarkSentence` exists **nowhere** (0 Core, 0 src) |
| 4 | Identity inputs for voice exist (post-40A) | `survivors.json` 129 defs with `bio`, `profession`, `traitIds`, `latentExpertTrait`, `isChild`; enrichment layer with `belief_profile_id` / `keepsake` / `phantom_background`; `antigravity_survivor_fields.json` (11: `manifesto_law_code` + stance) |
| 5 | State inputs for voice exist | the day-event stream (Wave 4's 31: 27 kinds, 7 rendered), needs (`NeedKind` incl. Fatigue/Hygiene), fitness verdict (24A), relations affinity (coordinator), grief (41A) |
| 6 | Faction voice register is already a design artifact | `docs/ui/FACTION_VOICE_MATRIX.md` (per-faction voice rules) + Wave 3's 25C step 5 (preserve register as metadata) — the same discipline extends to individuals |
| 7 | Somatic/audio flashback machinery exists | `CombatTraumaSystem` + 7B's combat/medical cue wiring (`SILENCE_AUDIT.md` §4.2: 26 combat event kinds mapped) — a trigger surface voice can hang off |
| 8 | Where lines would go already exist | `SurvivorDetailPanel.cs`, `GameHudOverlay`/`StatusPanel`, the briefing (31B click-through), journal (`JournalSystem` + codex), and the guidance overlay (17B) — four surfaces, all already fed by other systems |
| 9 | No dialogue system exists or is needed | `docs/MORAL_CHOICE_SYSTEM.md` + the three working `ResolveChoice` idioms (expedition/door/roster) cover decision UI; voice is *flavour delivery* into existing surfaces, not a conversation engine |
| 10 | Text-in-code risk is live | Wave 3's 25B counts 372 inline `Text = "` sites in `src/UI`; adding voice without the key layer would multiply that number by the cast size |

**Design boundary:** this plan is *not* a dialogue tree, not a conversational AI, not voice acting,
and not a chat system. It is a deterministic line-selection table keyed to (who × state × place ×
moment), delivered through surfaces that already exist. Coordination with parallel 147/148/150/159:
they want survivor *content*; this plan is the speaker they need.

---

## Task 42A — The line bank: authored voice keyed to identity and state

**Goal:** a data-authorable line catalog with deterministic selection, per-survivor voice registers,
and a hard rule that no line is chosen by chance the player can't reason about.

**Files:** new `Assets/Ashfall.Core/Voice/SurvivorVoiceSystem.cs` + `VoiceLineCatalogLoader.cs`,
new `Assets/StreamingAssets/Data/survivor_voice_lines.json`, `docs/voice/VOICE_LINE_SPEC.md`,
`docs/ui/FACTION_VOICE_MATRIX.md` (extend), `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`
(new tier), `ashfall-write` content pipeline, 25A/25C key layer.

### Substeps

1. **Specify the key schema before writing a single line**: `id`, `speaker` (survivor id, archetype,
   or `belief_profile_id`), `trigger` (a `DayEventKinds` value from 31, e.g. `ration_cut`,
   `survivor_perished`, `season_changed`), `conditions` (min/max need band, affinity pair, place
   `loc_`, fitness level, day window), `register` (clipped/bureaucratic/devotional — reuse the faction
   matrix vocabulary), `text_key` (a 25A key, never raw prose in C#), `weight`, `cooldown_days`.
2. **Author the first slice narrow and deep**: one trigger (`survivor_perished`) × 4 registers × a
   handful of conditions. Fifteen excellent lines beat 300 mediocre ones, and the slice proves the
   schema before content investment.
3. **Deterministic selection**: seeded pick from matching lines with per-survivor cooldown and
   no-immediate-repeat (the same rules the radio dedup already uses, so no new selection idiom),
   through `ISeededRng`/`CampaignStreamIds` (Invariant 4).
4. **Belief and profession must change phrasing, not just topic** — a fitter and a medic describe the
   same storm differently; that is the whole value of 40A. Encode it as `register` + `lexicon_tag`.
5. **Coverage gate**: every `survivor` in `survivors.json` resolves to at least a default register,
   and every authored line's `speaker`/`trigger`/`loc_`/`flag_` reference resolves — a new
   `CatalogIntegrityValidator` tier, so voice content cannot rot into dead data (Wave 1's 18B lesson).
6. **Unused-line reporting**: a Tier-2 report listing authored lines that never fire in a 200-day
   seeded soak, so content investment goes where it lands (mirrors Wave 5's mass-balance idea).
7. **Children and elders get their own register** (41C age classes), including the rule that a child
   does not narrate a mass grave — restraint here is the tone requirement.
8. **No invented interiority**: lines may reference only facts the survivor could know, given
   32C/41B's information channels — a person cannot comment on a sector they've never heard of, and
   that constraint is checkable against the reveal/knowledge state.
9. **Language policy**: every line is a key with an English value (25A), so voice doesn't become the
   next 400-string localization debt.
10. **Tests**: schema validation, per-condition selection determinism, cooldown behaviour, coverage
    tier, unused-line report shape, and a knowledge-constraint test that an out-of-context line never
    fires.
11. **Run the checklist** + `--data-integrity-selftest` + `ashfall-narrative-check`.

**DoD:** a line bank with a schema, a gate, and a first vertical slice.

---

## Task 42B — Where words land: delivery through existing surfaces

**Goal:** route selected lines into the four surfaces that already carry text, so "people talk" is
experienced without adding a chat panel or stealing attention from decisions.

**Files:** `src/UI/SurvivorDetailPanel.cs`, `src/UI/GameHudOverlay.cs` / `StatusPanel`,
`src/Main.Campaign.cs` (briefing), `JournalSystem` + `journal`/`codex` routes,
`src/Host/Phase0HostSession.cs` (flashback triggers), `src/Audio/AudioEventBridge.cs`,
`src/UI/AshfallUiHelpers.cs`, `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md`.

### Substeps

1. **Bind the voice system's output to a sink** (35A/36A's `ProducerPorts` discipline): a line is
   *delivered* to a route (briefing / journal / panel / HUD murmur) or it is reported unbound — never
   discarded in silence.
2. **Reserve attention honestly**: murmur-level lines (HUD, detail panel) versus event-level
   (briefing/journal) versus interruption-level (none — the game must never interrupt a decision with
   chatter). Set that budget in data.
3. **Journal as the archive**: spoken lines persist as journal entries with speaker + day + trigger,
   so the player can reread what a person said when their bunkmate died — that's the emotional payoff
   and it costs one write call.
4. **Distinguish the two channels**: radio broadcasts are the world's voice, survivor lines are the
   shelter's; never render them in the same lane (the radio panel is the wrong home for a private
   remark).
5. **Flashback triggers**: somatic/audio flashback cues already exist in Core and the audio bridge —
   route them as a distinct high-priority voice class with its own register (fragmented, present
   tense, no exposition), and never fire them where they'd be misread as a gameplay alert (17C's
   Alerts-bus stacking rule).
6. **Per-screen density limits**: at most N lines per day per surface, aggregated like 31A's
   transitions, so the shelter doesn't become a podcast during a crisis (which is when it matters
   most and when the player is least able to read).
7. **Murmur legibility**: `ashfall-ui-access` — lines must be dismissible, keyboard-readable, and not
   the only carrier of any mechanical fact. **Voice is texture plus record, never the only source of
   a warning.** (If a line says "the filter is clogged", the briefing/panel must also say it.)
8. **Rebind safety**: panels rebind on session swap (Wave 1's 16B/16C) — delivery must not
   double-post a line after a load; assert one delivery per (line, day) pair.
9. **Audio parity**: reuse existing cue families for any vocalisation (no new production batch in
   this plan); a line may optionally carry a cue id validated against the catalog.
10. **Tests**: delivery-per-route, budget enforcement, journal persistence, no-warning-only-by-voice
    assertion, rebind idempotency, and a determinism test on the line sequence.
11. **Snapshots** for the detail panel and briefing with lines present.
12. **Run the checklist** + `--audio-selftest`.

**DoD:** survivors speak in the right register at the right frequency, and it is always in the
journal afterwards.

---

## Task 42C — What they say about you: reputation, being talked about, and the listener

**Goal:** close the social loop — lines should be *about* things the player did and about other
survivors, so speech becomes evidence of the world state rather than ambient chatter.

**Files:** `SurvivorRelationsSystem` + coordinator read model, `GuiltInsomniaSystem`,
`RationConflictSystem`, `IdeologicalFrictionSystem`, `LeadershipSystem` (43),
`CensusClaimSystem`/`VoluntaryRegisterSystem`, `docs/systems/SURVIVOR_IDENTITY.md`, 40C's
knowledge-ladder rule.

### Substeps

1. **Define the three speech objects**: about the world (weather, shortage), about people
   (grudges, bonds, grief), about the player's decisions (ration cuts, triage calls, duty
   assignments) — the third is the one that makes the player feel watched, and it needs `causeId`
   from 31A step 5 to exist.
2. **Grievances become sentences**: `RationConflictSystem` already raises morale deltas; give it an
   authored line family so the complaint is heard and quotable, and 40C's discovery rule applies (the
   player learns a grievance exists only where they could overhear it).
3. **Friction is verbal**: two clashing belief profiles should produce paired lines across a day (one
   each, escalating register) rather than only an affinity number.
4. **Praise and blame for policy**: after a ration cut or a triage refusal, at least one survivor
   registers it — and *which* survivor depends on profession, belief, and who is affected, so the
   player can learn the room.
5. **Being talked about**: a survivor with low standing should hear it second-hand (a line from a
   third party) before it becomes a direct confrontation — the mediation system already exists; give
   it an audible stage.
6. **Leader's voice**: the designated leader speaks for the shelter (43's policy announcements);
   a stressed or broken leader's register changes (LeadershipSystem already models stress and break
   risk) — voice is the cheapest possible read-out of that state.
7. **Rumour carriage** (coordinate with parallel 131, which proposes a full information network):
   a line can carry a fact from a place the survivor was, subject to the knowledge constraint
   (42A step 8) — implement as a *delivery channel* here, not as a new simulation.
8. **No mechanical effect without a channel**: if a line implies a consequence (someone will refuse a
   shift), that consequence must exist in state and be traceable — never voice-only causality.
9. **Content budget guard**: line families are authored and gated like any content (42A step 5); the
   temptation to bolt prose into C# must be refused by the 25B new-string gate.
10. **Tone**: cold, tired, human, restrained; no exposition dumps, no moralising narration of the
    player's choices, no joke lines (`ashfall-write`, `ashfall-narrative-check`).
11. **Tests**: each speech object type fires from the right state, second-hand-before-confrontation
    ordering, leader register follows stress band, and a soak test asserting no line contradicts
    state (e.g. praising full bellies while `ration_short`).
12. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** what people say is evidence of what the player did — and can be reported back to them.

---

## Cross-Task Dependencies

```
40A (identity) ──► 42A (who speaks, in what register) ──► 42B (delivery) ──► 42C (about what)
31A (event kinds + causeId) ──► 42A step 1 (triggers), 42C steps 1,4
41A (memory/grief) ──► 42C steps 2,5 (grievance, being talked about)
36A (port contract) ──► 42B step 1 (unbound delivery = failure, not silence)
25A/25B (keys + no-literal gate) ──► the whole plan
24A/43 (fitness, leadership) ──► 42C step 6
   parallel 131 (rumour network) / 147 (NPC memory) / 148 (friction events) run ON this channel
```

**Execution order:** 40A → 31A → 42A → 42B → 42C. Voice last in the identity chain: a line selected
from invented data (pre-40A) or delivered into a surface that drops it (pre-36A) is the same
wasted-authoring mistake Wave 1's 18B exists to prevent.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors (+ voice tier)
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --audio-selftest                 # cue ids validate
7. godot --headless --path . -- --content-utilization-selftest   # voice catalog: consumed
8. ashfall-narrative-check + ashfall-write review                # tone, register, variation
9. soak report: authored-line utilisation over 200 seeded days     # 42A step 6
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 42A | 2 new | 1 | 1 new (slice) | 0 | 10–14 | Medium | LOW (new catalog, gated) |
| 42B | 0–1 | 4 | 0 | 4 | 8–12 | Low–Med | LOW (additive on live surfaces) |
| 42C | 1 | 2 | 1 | 2 | 10–14 | Medium–High | LOW–MED |

**Guardrails:** no dialogue tree, no conversation engine, no new panel, no new audio family, no
line that is the only carrier of a mechanical warning, no prose in C#, and no chatter budget that
outruns the briefing — the player should be able to say *why* someone talked, not just hear that
they did.
