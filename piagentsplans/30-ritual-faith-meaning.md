# Plan 30 — Ritual, Faith & Meaning: The Spiritual World

> **Theme:** How people make *meaning* after the end. This is the least-systematized human
> layer — grief, ritual, faith, superstition, holidays — yet it's where a post-apocalyptic
> story lives. Built strictly on existing systems (morale, memorial, guilt, ideology, cohort).
>
> **Key evidence (verified):** `MemorialSystem`, `GuiltInsomniaSystem`, `MoralBranchingSystem`,
> `IdeologicalFrictionSystem`, `CohortSystem`, `VinylMoraleSystem` live; narrative docs include
> `bunker_children_folklore.json` (+ batch_2) — folklore exists as data. No belief/ritual
> system exists → this is **content on existing morale/social systems**, not a new meter.

---

## Task 30A — Bunker folklore, rituals & children's culture

**Goal:** Expand `bunker_children_folklore.json` and author the rituals/superstitions that
emerge underground — the culture survivors invent to cope.

**Files:** `bunker_children_folklore.json` (+ batch_2), `events.json` (ritual events),
`bunker_graffiti_postings.json` (extend), read-only `CohortSystem`, `NeedsSystem` (morale),
`IdeologicalFrictionSystem`.

**Substeps:**
1. Read the two folklore docs to lock the voice (children's rhymes about the exchange, whispered rules, bogeymen that are really radiation).
2. Author 12 folklore pieces (counting rhymes about dosimeters, a lullaby about the deep cold, a cautionary tale about the door) — dark, true, child-logic.
3. Author 8 emergent rituals (tapping the door twice, leaving a ration for "the ones outside," a birthday candle rationed) — small morale effects via `NeedsSystem`.
4. Author 6 superstitions that cause friction (someone won't sleep near the vent; a "lucky" bunk) — ties to `IdeologicalFrictionSystem`/`RationConflictSystem`.
5. Author how children (12A cohort) learn/perpetuate folklore — a generation that never saw the sky invents its own myths.
6. Author 4 moments where folklore becomes real comfort (a rhyme calms a panicking child) — morale/trauma hook.
7. Wire a few rituals to gameplay texture (a pre-expedition luck ritual; a meal ritual) — always optional, small, human.
8. Validate ids; data-integrity selftest; narrative-continuity.
9. xUnit: ritual event morale effect, folklore unlock, friction from superstition.
10. `DataRuleComplianceTests` (no real religions/myths; invented, grounded).

**Next steps:** folklore that turns out *true* (a rhyme encodes a real survival rule — a reward
for attentive players); a folklore codex (17C); a child who maps the bunker by its stories.

---

## Task 30B — Grief, mourning & memorial rites

**Goal:** Build the rites around death — how the bunker mourns — on the existing
`MemorialSystem` + `GuiltInsomniaSystem` + relations, so loss has ceremony, not just a flag.

**Files:** `events.json` (mourning events), memorial data, narrative docs, read-only
`MemorialSystem.cs`, `SurvivorRelationsSystem.cs`, `GuiltInsomniaSystem.cs`, `FinalWishSystem` (06A).

**Substeps:**
1. Read `MemorialSystem` + the grief cascade in `SurvivorRelationsSystem` to map what mourning exists today.
2. Author 6 funeral/memorial rites (a burial detail, a name spoken at roll-call, a belonging divided, a bunk left empty one night) with small morale/grief effects.
3. Author the mourning arc: acute grief → the empty-shift day → the first laugh after → a memorial — staged over days via existing systems.
4. Wire rites to grief cascade: a *held* rite softens the cascade; a skipped rite (no time, siege) deepens it.
5. Author 4 grief-conflict events (someone can't stop working; someone wants the bunk reassigned now; a fight over the deceased's things).
6. Connect memorial rites to final wishes (06A) and vigil deaths (09C) — a well-mourned death vs. a hard one.
7. Author a memorial-wall accumulation (12C) — the wall grows; reading it is a morale act.
8. Validate ids; data-integrity selftest; narrative-continuity.
9. xUnit: rite softens grief cascade, mourning arc stages, memorial-wall accrual, determinism.
10. Tone review: grief must be restrained and human — no melodrama, no exploitation.

**Next steps:** a yearly day-of-the-dead rite (the bunker remembers everyone at once); a grief
that becomes a quest (carry the ashes to a place, 06A); mourning interrupted by a crisis (war 06C).

---

## Task 30C — Belief, schism & meaning-making

**Goal:** Author the spiritual/ideological landscape — invented post-exchange belief systems,
their friction, and their comfort — strictly via `IdeologicalFrictionSystem` + morale, no new
belief meter.

**Files:** `events.json` + `faction_lore.json` (belief sets), `moral_choice_quests*.json`
(reuse), read-only `IdeologicalFrictionSystem.cs`, `MoralBranchingSystem.cs`, `LeadershipSystem.cs`.

**Substeps:**
1. Read `IdeologicalFrictionSystem` + the 12B belief sets to avoid duplication (this task is *institutional* belief vs. 12B's bunk-level friction).
2. Invent 3 post-exchange belief movements (the Ash faith — the exchange as judgment; the Rebuilders — meaning through works; the Listeners — the radio/number-stations as voices) — fictional, grounded, no real religions.
3. Author each movement's practice, comfort, and blind spot (what it helps with, what it can't see).
4. Author 8 belief events (a conversion, a schism over a practice, a crisis of faith after a death, a charismatic preacher arrives).
5. Author friction between movements (12B) and with the secular leadership (`LeadershipSystem`) — who comforts the dying, who buries them.
6. Author the moral texture: belief genuinely helps (morale, grief) AND genuinely blinds (a refused treatment, a fatalist risk) — no easy answer.
7. Wire a movement's rise to a leadership crisis (a faction within the bunker) and to 06C (a movement that welcomes the war as judgment).
8. Validate ids; data-integrity selftest; dialog-graph lint; `DataRuleComplianceTests`.
9. xUnit: belief event morale/friction effects, schism escalation, leadership challenge.
10. Cross-tool QA + a careful sensitivity review (belief is deeply human — write with respect, no mockery, no preaching).

**Next steps:** a belief movement becomes a faction (25A); a pilgrim NPC (20B) walking to a
"clean" place; the epilogue weighs what the bunker came to believe (15A); a schism that splits
the bunker as a late-game crisis.
