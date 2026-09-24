# UNBLOCK — Plans 171 & 174: Dynamic Quest Generation + Mechanical Origin Seam

**Status:** DONE 2026-09-24 (integrator, user-authorized).
**Claim:** `claim-unblock-plans-171-174-2026-09-24`.
**Evidence:** `--dynamic-quest-selftest` 12/12, `--origin-mechanics-selftest` 12/12,
host + Core builds 0 errors / 0 warnings.

## Premise (verified in source before editing)

- **Plan 171** — `DynamicQuestGenerator` and its authored
  `dynamic_quest_templates.json` existed, but the catalog was registered
  `UNRESOLVED` (never loaded) and the generator's lifecycle was only reachable
  through a static bridge. The sealed Plan 171 disposition
  (`PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md` §5) is: *reduce to
  deterministic candidate/parameter generation; source templates from JSON; hand
  accepted candidates to the canonical quest runtime and save owner.*
- **Plan 174 / C3-174** — `SurvivorEnrichmentService.GetOriginModifier` existed
  with 0 host references: the mechanical origin (skill, trade specialty,
  keepsake) was never applied to gameplay. The backstory portion of Plan 174 was
  already integrated and corrected (DEC-317).

## Plan 171 — Dynamic Quest Generation

**Core**
- `DynamicQuestTemplateCatalogLoader` (new, strict): rejects unsupported schema,
  empty/duplicate template ids, unknown quest types, empty titles/descriptions,
  difficulty outside 1..5, a non-positive required quantity, negative or
  non-finite rewards, and a reward item declared without a positive count.
- `ProceduralQuestType` extended with the two authored types the enum was
  missing (`ScoutExploration`, `DisputeResolution`) so the Core contract matches
  the authored catalog.
- `DynamicQuestGenerator.BindAuthoredTemplates` (validated seam) and
  `GetCensus()` (`DynamicQuestGeneratorCensus`); reward-item fields carried
  through generation and capture/restore; `RestoreState` schema-gated.

**Host**
- `DynamicQuestHostSession` loads the authored catalog through the strict loader
  and exposes the template catalog, deterministic candidates, accept/progress/
  complete, deadlines, and the census.
- `Main.DynamicQuestGeneration.cs` binds it at campaign start and on restore.
- The canonical `QuestRuntimeCoordinator` remains the accepted-quest lifecycle
  and save owner — no second quest lifecycle and no second save section.
- CLI probe `--dynamic-quest-selftest` (12 checks). The orphaned
  `dynamic_quest_templates.json` is now loaded, so it is no longer dead content.

## Plan 174 — Mechanical Origin Effects Seam (C3-174)

**Core**
- `ResolveSkillFromProfession` gained the authored `metallurgist` profession
  (previously unmapped, so an authored survivor resolved no skill) and
  `ResolveTradeSpecialty` maps it to `tools`.

**Host**
- `Main.OriginMechanics.cs` applies each enriched survivor's origin once per
  campaign: the primary skill through the canonical `SkillProgressionSystem` and
  the personal keepsake through the canonical inventory owner. The keepsake grant
  is gated on the item actually resolving in the item catalog, so an orphaned
  authored id cannot spam the inventory log or invent an item.
- Called at campaign start; reset with the campaign lifecycle.
- CLI probe `--origin-mechanics-selftest` (12 checks): resolution, determinism,
  no fabricated skills, orphan reporting, and host wiring.

## Recorded data gap (not a code gap)

The authored enrichment catalogs declare 76 distinct keepsake ids, but **65 of
them do not exist in any data JSON** (only 11 resolve in `items.json`). This is a
pre-existing authoring gap in the Plan 40A enrichment catalog. The host guards
against it by granting only resolvable keepsakes; the probe reports the orphan
count. Recorded in `KNOWN_DEBT.md` as a data-authoring debt row.

## Deferred with named reasons

- Routing the generator's authored candidates into the canonical
  `QuestRuntimeCoordinator` as an additional template source: the two template
  schemas (`dynamic_quest_templates.json` vs `quest_templates.json`) differ and
  merging them would require a designed schema bridge, or it becomes a second
  template authority.
- `metallurgist` trade-specialty mastery milestones (the trade system assigns by
  crafting attribution; the specialty id is now resolved but milestone attribution
  stays with the crafting owner).
- Authoring the 65 orphan keepsake items (recorded data debt).
- Dynamic-quest board UI and survivor origin UI: presentation only.

## Verification

```
host + Core builds: 0 errors / 0 warnings
--dynamic-quest-selftest 12/12        --origin-mechanics-selftest 12/12
--data-integrity-selftest PASS        --port-contract-selftest PASS (298 seams)
--7-day-smoke-selftest PASS           --selftest-manifest 171 tests
Quests 34/34  Survivors 522/522  Mods 60/60  Shelter 785/785
architecture map 239 subsystems (100%) · port contract 298 seams
```
