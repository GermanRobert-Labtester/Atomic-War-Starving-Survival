# Plan 27 — The Body & the Mind: Dose Registers, Autopsies & Psychological Contamination

> **Theme:** The medical-psychological interior world. The Dose Register (dosimetry society)
> has 4 NPCs and 4 quests; autopsies have 3 procedures; and a `PsychologicalContaminationSystem`
> sits in Maritime with no content. This plan deepens the human-cost layer.
>
> **Key evidence (verified):** `dose_registers.json` = 4 bands/3 plans/3 guesses/4 registers/4
> NPCs; `dose_quests.json` = 4; `dose_items.json` = 5; `dose_locations.json` = 3;
> `autopsy_procedures.json` = 3; `PsychologicalContaminationSystem.cs` (Maritime) live.

---

## Task 27A — Dose Register society depth (4 → 12 quests)

**Goal:** Expand the Dose Register — the society that tracks everyone's radiation — into a full
faction with quests, politics, and a moral core (who gets the clean beds?).

**Files:** `dose_quests.json`, `dose_registers.json`, `dose_items.json`, `dose_locations.json`,
read-only `DoseLedgerSystem.cs`, `DoseRegistersCatalog.cs`, `DoseContentCatalog.cs`, Dose host session.

**Substeps:**
1. Read `DoseLedgerSystem` + `DoseRegistersCatalog` + the 4 NPCs to learn the register model (bands, plans, guesses).
2. Understand the 4 register NPCs (dr_irina_vel, wyn_omah, piet_abar, saria_voss) — their roles and voices.
3. Author 8 new dose quests (a falsified reading, a stolen dosimeter, a child over the limit, a register audit, a black-market clean-bill).
4. Author the moral core: 3 quests about triage-by-dose (who gets iodine, who gets the clean room) feeding `MoralBranchingSystem`.
5. Author 4 new dose items (a calibrated dosimeter, a forged register chit, a chelation course, a lead-lined token).
6. Author 2 new dose locations (the register hall, a screening station) as `loc_*`.
7. Wire dose state to real consequences (high-dose survivors barred from some roles — ties to duty roster 12B).
8. Validate ids; data-integrity selftest; dialog-graph lint.
9. xUnit: dose quest progression, register band assignment, falsification detection, item effects.
10. Dose UI test (`--dose-uitest` exists) still green; extend for new content.

**Next steps:** the register as a faction in the war (06C — they control medical truth); a
"purge the registers" dark choice; register data as Verdict evidence (15B).

---

## Task 27B — Autopsy & cause-of-death investigation

**Goal:** Expand `autopsy_procedures.json` (3) into a forensic loop: autopsies that reveal how
people died — medical knowledge, disease intel, and sometimes a crime.

**Files:** `autopsy_procedures.json`, `items.json` (instruments), read-only `AutopsySystem.cs`,
`AutopsyProcedureCatalogLoader.cs`, `DiseaseSystem`, `MemorialSystem`.

**Substeps:**
1. Read `AutopsySystem` + the loader to learn the procedure schema (instruments, findings, knowledge yield).
2. Read the 3 existing procedures to lock the clinical, respectful tone (tone rules: no gratuitous gore).
3. Author 6 new procedures (radiation autopsy, infectious autopsy, trauma autopsy, toxicology, nutritional, neonatal) each needing instruments + skill.
4. Give each procedure findings that feed `ResearchSystem` medical knowledge (26A) and `DiseaseSystem` intel (09A).
5. Author 3 forensic autopsies that reveal a *non-natural* cause — a poisoning, a smothering, a staged accident — feeding 21C secrets and 15B Verdict evidence.
6. Wire autopsy of a bonded/kin survivor to grief (`SurvivorRelationsSystem`) — someone must consent.
7. Author the instruments (a bone saw, a field autopsy kit, reagent strips) as real items.
8. Validate ids; data-integrity selftest; `DataRuleComplianceTests` (clinical, not gratuitous).
9. xUnit: procedure requires instruments/skill, findings granted, forensic flag set, grief hook.
10. Narrative-continuity: autopsy findings must align with disease/medical canon.

**Next steps:** a coroner NPC (20B); an "unnatural deaths" cold-case thread across the campaign;
autopsy records in the archive (17B).

---

## Task 27C — Psychological contamination & deep-dive dread

**Goal:** Give `PsychologicalContaminationSystem` (Maritime) actual content — the slow dread of
deep, dark, contaminated places — so it surfaces beyond an unused system.

**Files:** dive/contamination data, `environmental_texts` (dread text), read-only
`PsychologicalContaminationSystem.cs`, `SomaticFlashbackSystem.cs`, `CombatTraumaSystem.cs`,
`GuiltInsomniaSystem.cs`.

**Substeps:**
1. Read `PsychologicalContaminationSystem` to learn what it tracks (exposure, thresholds, effects) and where it's wired (dives only? any dark place?).
2. **Decide scope:** is this dive-specific or a general "dark place dread" system? If general, note it overlaps the trauma systems — keep it scoped to avoid duplication (registry §23 warns against parallel sanity meters).
3. Author contamination sources: the deepest dives (23B), the sealed bunker room (21C), a mass-grave site (11A), the reactor crater.
4. Author escalating exposure effects as *text + small mechanical* (a whispered line, a flinch = accuracy dip, a sleepless night = `GuiltInsomniaSystem` hook) — reuse existing mental systems for effects, don't fork.
5. Author 6 dread texts in the restrained voice (what the beam catches, what it doesn't).
6. Author the recovery path: decompression, a calm survivor's grounding (`CombatTraumaSystem` companion grounding), time.
7. Wire contamination to a visible UI tell (a survivor acting off) so it's not a hidden mechanic (design gateway #8).
8. Validate ids; data-integrity selftest.
9. xUnit: exposure accrues by site/depth, threshold effects fire via existing systems, recovery works, determinism.
10. Cross-tool QA + a careful duplication review vs. the existing trauma systems.

**Next steps:** a survivor who *thrives* in the dark (a trait, 26B) as a dive specialist; a
"what they saw down there" final-wish (06A); contamination as a dive-crew staffing constraint.
