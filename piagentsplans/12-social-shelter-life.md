# Plan 12 — Social & Shelter Life: Generations, Friction & Customization

> **Theme:** The bunker is where the player lives between crises. Cohort/lineage, ideological
> friction, ration conflict, and duty roster are all live; shelter customization is documented
> white space. This plan makes the interior a place with a society in it.
>
> **Key evidence:** `CohortSystem` + `GenerationalLineageExtension` + `ApprenticeshipSystem`
> (late-game only, no surrounding narrative); `IdeologicalFrictionSystem`, `RationConflictSystem`,
> `SurvivorRelationsSystem`, `DutyRosterSystem` (986 lines, 43 marks) live; white space #3
> (interior decor/trophies) unimplemented.

---

## Task 12A — Generational society: schooling, apprenticeship & adoption

**Goal:** Surround the existing cohort/lineage/apprenticeship systems with narrative and
decision content so multi-year play has a society, not just a shift schedule.

**Files:** new quest/event entries (`questline_master.json`, `events.json`), `survivors.json`
(child/apprentice fields if schema supports), read-only `CohortSystem.cs`,
`GenerationalLineageExtension.cs`, `ApprenticeshipSystem.cs`, `SkillProgressionSystem.cs`.

**Substeps:**
1. Read the three systems to map what cohort state exists (growth stages, maturation age, family links, apprenticeship slots).
2. Identify the exact hooks where content can attach (maturation event, apprenticeship assignment, skill grant).
3. Author a schooling-curriculum decision chain (what do the children learn: letters, mechanics, medicine, marksmanship — each biases a skill seed).
4. Author 6 apprenticeship arcs pairing a child with a master by trade, each with a payoff event (first solo repair, first assisted surgery).
5. Author 4 orphan-adoption arcs (war-warcasualty orphans — ties to 06C) with ideological-friction consequences (who raises them?).
6. Author coming-of-age events (first surface trip, first watch shift) that convert a child into a working survivor.
7. Ensure skill grants flow through `SkillProgressionSystem` — no parallel skill counters.
8. Validate ids; data-integrity selftest; dialog-graph lint for orphan flags.
9. xUnit: maturation triggers, apprenticeship assignment, skill seeding determinism.
10. Save round-trip for cohort/lineage state across a multi-year simulation.

**Next steps:** generational epilogue weighting (the 32-permutation matrix reads who survived
*and who was raised*); elder-knowledge transfer before death (ties to final wishes).

---

## Task 12B — Ideological friction & ration-conflict event packs

**Goal:** Give `IdeologicalFrictionSystem` and `RationConflictSystem` a deep bench of bunk-level
social events so cohabitation generates constant low-grade, human drama.

**Files:** `events.json` (new social events), `faction_lore.json` (belief sets), narrative docs
(`bunker_graffiti_postings.json` exists — extend), read-only `IdeologicalFrictionSystem.cs`,
`RationConflictSystem.cs`, `SurvivorRelationsSystem.cs`, `LeadershipSystem.cs`.

**Substeps:**
1. Read both systems to learn event/trigger surfaces (compatibility, grievance accrual, mediation actions).
2. Author 4 philosophical belief sets as faction-agnostic survivor ideologies (ration-collectivist, every-soul-for-themselves, faith-in-rebuild, ash-nihilist) with friction pairs.
3. Author 10 bunk-friction events (snoring feud, stolen keepsake, work-shirker accusation, forbidden radio listening) each with 2–3 mediation choices and morale/relationship deltas.
4. Author 6 ration-conflict events (uneven scoop, hoarded tin, feast-day demand, sick-gets-more dispute) keyed to `RationConflictSystem` grievance state.
5. Author 4 escalation events where unresolved friction becomes a crisis (a walkout, a sabotage, a challenge to leadership) feeding `LeadershipSystem`.
6. Extend `bunker_graffiti_postings.json` with 10 postings that react to recent events (ambient storytelling).
7. Ensure every event uses existing morale/relationship/resource hooks — no new counters.
8. Validate ids; data-integrity selftest; dialog-graph lint.
9. xUnit: friction accrual → event trigger; mediation choice effects; grievance → escalation.
10. Balance pass (`ashfall-balance-sim`): social-event frequency must not drown survival pressure.

**Next steps:** mediation-skill survivor trait; a "bunker charter" policy decision (ties to
Nobody's Charter expansion).

---

## Task 12C — Shelter interior customization & memorial wall (White Space 3)

**Goal:** Implement the documented white-space feature: room decor slots (posters, trophies,
plaques) granting localized morale — the only plan here that adds a genuinely new (small) system.

**Files:** new `ShelterDecorSystem` in Core (engine-agnostic), `ShelterAssignmentSystem`
(read), `MemorialSystem` (read), `NeedsSystem` (morale hook), new decor items in `items.json`,
room-view UI slots in `src/UI/`, Main triad + save section.

**Substeps:**
1. Read white-space spec (atlas §34.3) + `ShelterAssignmentSystem` room model + `MemorialSystem` plaque data.
2. Design `ShelterDecorSystem`: per-room decor slots, decor item category, per-decor localized morale modifier — all pure C# DTOs, `ISeededRng`-free (no randomness needed).
3. Author 12 decor items (propaganda posters, locomotive nameplate, carved memorial plaque, child's drawing, pressed flower frame) in `items.json`.
4. Wire localized room morale into `NeedsSystem` warmth/morale modifiers (occupants of a decorated room get the buff).
5. Connect `MemorialSystem`: a fulfilled Plan 65 final wish or a vigil-managed death (09C) yields a memorial plaque item.
6. Add decor slots to the room view UI (Godot side only renders; logic stays in Core).
7. Add the Setup/Save/Flush triad + registry save section via `SaveStoreHub`.
8. Data-integrity selftest; snapshot-diff on the room/interior panel (new golden image).
9. xUnit: slot assignment, morale modifier application, plaque generation, save round-trip.
10. `ashfall-godot-scene-lint` on the touched scene; `dotnet build` 0/0.

**Next steps:** trophy mounts from hunting (Plan 13B); decor quality tiers; a "make it a home"
morale milestone. **Cross-tool QA applies** (new system).
