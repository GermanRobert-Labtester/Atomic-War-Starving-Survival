# Plan 26 — Knowledge, Research & Skills: The Progression World

> **Theme:** How survivors *learn*. This is the biggest latent-progression seam found in the
> audit: the `ResearchSystem` **hardcodes 15 knowledge nodes inline** with no JSON tech tree,
> `trade_specialties.json` has **4** entries, `library_manuals.json` has **3**, and 73 distinct
> `latentExpertTrait` values sit on survivors with (likely) no unlock content.
>
> **Key evidence (verified):** `ResearchSystem.cs` registers 15 `knowledge_*` nodes in code
> (violates the JSON-authority invariant — data should live in `StreamingAssets/Data/`);
> `trade_specialties.json` = 4; `library_manuals.json` = 3 (ids all None — broken?);
> 129 survivors, 73 distinct latent expert traits, 101 professions.

---

## Task 26A — Externalize & expand the research/knowledge tech tree (15 → 40)

**Goal:** Move the hardcoded research catalog into the data authority and expand it into a
real tech tree — this is also an **invariant fix** (data belongs in JSON, not C#).

**Files:** new `Assets/StreamingAssets/Data/research_knowledge.json`, a Core catalog loader
(follow the `*CatalogLoader.cs` pattern), `Assets/Ashfall.Core/Research/ResearchSystem.cs`
(remove hardcoded registration → load), read-only `ResearchKnowledgeDef.cs`.

**Substeps:**
1. Read `ResearchSystem` + `ResearchKnowledgeDef` to extract the node schema (id, name, category, day-cost, prerequisites, breakthrough item award).
2. Extract the 15 hardcoded nodes verbatim into `research_knowledge.json` (schema_version, snake_case) — behavior-identical migration first.
3. Add a `ResearchKnowledgeCatalogLoader` (Core, `IJsonSerializer`) + swap `ResearchSystem` to load from it; keep a tiny built-in fallback only if tests require it.
4. Verify the 15 nodes load identically (a binding test pins each node's fields).
5. Design 3 new branches on the existing categories (survival/medical/engineering/combat/radio/solar): advanced, applied, and mastery tiers.
6. Author 25 new nodes with sensible prerequisite DAGs (no cycles, no orphan prereqs) and breakthrough items resolving to real `item_*`.
7. Key 4 nodes to unlock content from other plans (better dive gear 23B, cloud-seeding #17, field-guide intel 20A, preservation 22B).
8. Validate ids + prereq graph (DAG check); data-integrity selftest.
9. xUnit: catalog loads, prereq gating, day-progress tick, breakthrough award, save round-trip.
10. **Refactor note:** this closes a data-authority invariant violation — flag it in the commit.

**Next steps:** a research UI tree view; research breakthroughs as Verdict evidence of
"progress" (15B); a lost-knowledge mechanic (a manual 26C required to unlock a branch).

---

## Task 26B — Skills, trade specialties & latent-expert awakening

**Goal:** Give the skill system real content — expand trade specialties (4 → 16) and make the
73 latent expert traits *awakenable* through play.

**Files:** `trade_specialties.json`, the skills catalog (confirm location — `SkillDef.cs` says
canonical ids live in `skills.json` which **does not exist** — reconcile this), `survivors.json`
(latent traits), read-only `SkillProgressionSystem.cs`, `SkillAtrophySystem.cs`, `TradeSpecialtySystem.cs`.

**Substeps:**
1. **Reconcile the missing `skills.json`:** `SkillDef.cs` documents it as the canonical skill-id source but it's absent — find where skill defs actually load from (may be hardcoded too). If missing, this is a second data-authority gap to fix alongside 26A.
2. Read `SkillProgressionSystem` + `SkillDef` for the skill/perk schema (10 domains, level 1–10).
3. Read `TradeSpecialtySystem` + the 4 specialties (Apprentice/Journeyman/Master tiers).
4. Author 12 new trade specialties mapped to real professions (101 exist) — a miller, a cobbler, a wireman, a bone-setter — each with tier perks.
5. Read how `latentExpertTrait` is meant to fire; if no unlock path exists, design one (a trigger event: first time doing X, a teacher, a crisis).
6. Author awakening content for 12 high-value latent traits (the event that reveals the ex-medic's hands, the ex-engineer's eye).
7. Wire awakenings to `SkillProgressionSystem` XP boosts + a journal note (17C).
8. Validate ids (skill_, trait_); data-integrity selftest.
9. xUnit: specialty tier progression, latent awakening trigger, skill XP grant, atrophy interplay.
10. Balance sim: specialties must differentiate survivors, not create one dominant build.

**Next steps:** master-tier perks (registry suggestion); a "guild apprenticeship" for children
(12A) using trade specialties; specialty-driven expedition role bonuses.

---

## Task 26C — Library, manuals & autopsy knowledge

**Goal:** Expand `library_manuals.json` (3, ids None — **likely broken**) and
`autopsy_procedures.json` (3) into a knowledge-*acquisition* loop: books and procedures that
teach research/skills.

**Files:** `library_manuals.json`, `autopsy_procedures.json`, `items.json` (book/manual items),
read-only `ResearchSystem`, `ArchiveDeskSystem`, `AutopsySystem`, `AutopsyProcedureCatalogLoader.cs`.

**Substeps:**
1. **Fix `library_manuals.json`:** 3 manuals with `id: None` is a broken catalog — read the loader, repair ids, add a binding test so it can't silently regress.
2. Read `AutopsySystem` + `AutopsyProcedureCatalogLoader` to learn the procedure→knowledge schema.
3. Author 12 library manuals (a pre-war medical text, a radio operator's handbook, a seed-saving guide, a metallurgy primer) each granting research progress or a skill unlock (ties to 26A/26B).
4. Place manuals as loot (17B documents, 11A digs, 23B wrecks) and as 20B NPC trades.
5. Author 6 new autopsy procedures (each reveals a cause of death → medical knowledge + sometimes a disease clue for 09A).
6. Wire autopsy discovery to `DiseaseSystem` intel (learning a pathogen's nature from a body) and to Verdict evidence (15B) where a death was suspicious.
7. Author a "library" shelf state — manuals collected vs. read vs. applied (uses ArchiveDesk 17B).
8. Validate ids; data-integrity selftest.
9. xUnit: manual grants knowledge, autopsy reveals cause + disease clue, broken-id regression test.
10. Narrative-continuity: manual contents must not contradict the medical/tech canon.

**Next steps:** a scholar NPC (20B) who trades for manuals; a "complete the library" codex
milestone; a burned-library quest (recover the one surviving copy).
