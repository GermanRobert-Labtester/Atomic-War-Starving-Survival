# ASHFALL — Expansion Wave 4 Index (Expansions 27–31)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-21
**Purpose:** Index and evidence summary for the five Wave 4 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: the shelter becomes a town

Waves 1–3 covered survival domains that keep people alive: generations, belief,
flight, soil, bodies, leisure, depth, hazards, secrets, power, water, fire, age,
rail, and food. Wave 4 covers the domains that let a shelter stop being a camp:
clothing, schooling, glass, print, and masonry. Each one is already seeded in
the repository as live seams with thin content — a durability model with no
garments, an apprenticeship system with a 1.7 KB catalog, a precision-optics
engine with a 1.4 KB glass recipe file, a printing catalog with no press, and
four narrative kiln files with no kiln.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 27 · The Thread | `Inventory.WornGear`, `NeedsSystem` warmth/hygiene, `ShelterAtmosphereSystem.ThermalComfort`, `craft_textile_repair` | no garment model, no laundry | garments, layers, laundry, fiber, leather, insulation, identity |
| 28 · The Lesson | `ApprenticeshipSystem` (+host/save/panel), `LibraryStudySystem` (+host/panel), `ArchiveDeskSystem` | `apprenticeship_catalog.json` = 1.7 KB | schooling, literacy, curriculum, exams, manual authoring |
| 29 · The Glass | `PrecisionOpticsEngine` (+host/save), `OpticsGlassworksCatalog`, `GeodeticSurveyEngine` | `glassworks_recipes.json` = 1.4 KB | furnaces, batches, lenses, vision care, instruments, glazing, mirrors |
| 30 · The Press | `PaperPrintingCatalog`, `ArchiveDeskSystem`, `ArchiveInkCatalogLoader`, `UndergroundPrintingPressPanel` | no print data at all | paper mill, press, type, publications, notices, bound archive |
| 31 · The Kiln | `CeramicsKilnCatalog`, `MasonryBrickworksCatalog`, four narrative kiln/lime/mudbrick/refractory datasets, `weather_hardening_upgrades.json` | narrative only, no gameplay | lime, brick, ceramics, mortar, concrete, refractory, build works |

The strongest authority splits in the wave: garments are items resolved through
the existing equipment and warmth owners; literacy and exams write only through
`SkillProgressionSystem` and the live study system; vision consequences route
through the medical pipeline while radiation browning stays in `RadiationSystem`;
print never touches the radio or the census broadcast; and the kiln's build
results land in the live shelter upgrade path rather than a parallel construction
store.

---

## The five plans

1. `expansion_27_the_thread_plan.md` — 70,320 chars.
   Garments, layers, warmth, laundry, fiber, leather, insulation, work dress,
   mourning dress, and heirlooms. Extends `Inventory`/`WornGear`,
   `NeedsSystem`, and `ShelterAtmosphereSystem`.
2. `expansion_28_the_lesson_plan.md` — 70,265 chars.
   Schooling, literacy, curriculum, apprenticeship tracks, exams, and manual
   authoring. Extends `ApprenticeshipSystem`, `LibraryStudySystem`, and
   `ArchiveDeskSystem`.
3. `expansion_29_the_glass_plan.md` — 70,729 chars.
   Sand to batch, batch to melt, lenses, spectacles, instruments, glazing, and
   mirrors. Extends `PrecisionOpticsEngine`, `GeodeticSurveyEngine`, and the
   greenhouse.
4. `expansion_30_the_press_plan.md` — 70,024 chars.
   Paper making, type, press work, broadsheets, almanacs, notices, and the bound
   archive. Extends `ArchiveDeskSystem` and the printing catalog.
5. `expansion_31_the_kiln_plan.md` — 70,282 chars.
   Lime, brick, ceramics, mortar, concrete, refractories, and build works.
   Extends `CraftingSystem` and the live shelter upgrade path.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second equipment, skill, optics, archive, or
  construction system is introduced.
- **Deterministic.** Outcomes are deterministic functions of inputs and skill;
  where randomness already exists, the live seeded path is used. Paired replay
  hashes must match.
- **Persistence.** New state is additive inside existing save owners
  (inventory/equipment, apprenticeship, library study, precision optics, archive
  desk, shelter/crafting). Legacy saves load neutral; the Triad drift gate must
  pass.
- **Tone.** Restrained, human, fictional. No real brands, uniforms, mastheads,
  institutions, or monuments copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 4

| Plan | Hard contract |
|---|---|
| 27 Thread | Warmth and hygiene only through `NeedsSystem.Modify`; cold kills are warned and recoverable; no fashion stat system; the dead's clothes are a dignified choice |
| 28 Lesson | Adults who cannot read are never humiliated; exams are informative, not punitive; children's lessons are restrained; knowledge loss is tragic, not gratuitous |
| 29 Glass | Sight is capability, not humanity; a survivor who loses vision keeps their place, work, and name; no glass superweapons; eye injuries are clinical, never gore |
| 30 Press | Print verifies, records, and cares; it never manufactures consent; corrections are celebrated; censorship is debated respectfully and never rewarded |
| 31 Kiln | Building is care and craft, not conquest; burns are practical; apprentices are 16+ and supervised; permanence is framed as making a home |

---

## Cross-wave hooks (summary)

- **Wave 1 × Wave 4:** children's clothing and school bands; ritual dress and
  printed hymns; optics and glazing for air and greenhouse; flax, wool, and
  lime for the deep root; adaptive clothing and printed fit guides for the
  rebuilt body.
- **Wave 2 × Wave 4:** feast dress and printed songs; cold underground clothing
  and lamp glass; protective suits and hazard manuals; disguise and clandestine
  leaflets; furnace power, lamp glass, and load diagrams.
- **Wave 3 × Wave 4:** laundry water and ceramic pipes; fire-safe glazing and
  drill notices; reading glasses for the old and memorial masonry; timetables
  and freight bills; storage jars, ovens, and printed recipes.
- **Wave 4 × Wave 4:** cloth patterns printed by the press; paper from rag fiber;
  school windows and spectacles from the glassworks; kiln-fired type metal and
  furnace pots; every plan's tools and materials cross the others.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 4 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an
   API or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and
   focused verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then
   content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe
pre-signature work is Phase 1 (schemas and validators), which is additive and
reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 300,000–340,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. Each
  plan recommends additive sub-objects.
- Whether any Wave 4 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending existing verbs.
- Priority order. Recommended: 27 (Thread) first, because clothing is the
  tightest winter loop; 31 (Kiln) second, because permanence unblocks every
  other build; 29 (Glass) third, because vision and measurement unblock survey
  and clinic work; 30 (Press) fourth, because print compounds the school and the
  archive; 28 (Lesson) fifth only if schooling is treated as a full system
  rather than a content layer, since it is the most sensitive to tone.

---

## Evidence anchors (file references used across the plans)

- `Assets/Ashfall.Core/Inventory/Inventory.cs` — `EquippedItem`, `WornGear`,
  `RecordWear`, durability.
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — `Warmth`, `Hygiene`,
  `WasWarmthCritical`.
- `Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs` — `ThermalComfort`,
  `Cleanliness`, `LightingQuality`.
- `Assets/Ashfall.Core/ApprenticeshipSystem.cs` — `MentorshipDef`,
  `NotifyMentorDeath`, `TranscriptionTask`, `SurvivorWill`.
- `Assets/Ashfall.Core/LibraryStudySystem.cs` — `ManualDefinition`, `StudyJob`,
  `IsManualPowered`, `GetComprehensionRate`.
- `Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs` and
  `src/Host/PrecisionOpticsHostSession.cs`.
- `Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs`,
  `ArchiveDeskSystem.cs`, `ArchiveInkCatalogLoader.cs`.
- `Assets/Ashfall.Core/Narrative/CeramicsKilnCatalog.cs`,
  `MasonryBrickworksCatalog.cs`.
- `Assets/Ashfall.Core/Shelter/CupolaFoundryEngine.cs` — refractory consumption.
- `Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs` — region
  infrastructure.
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`.
- `docs/expansions/wave1/WAVE1_INDEX.md`, `wave2/WAVE2_INDEX.md`,
  `wave3/WAVE3_INDEX.md`.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`,
  `INTEGRATION_PLANS.md`.