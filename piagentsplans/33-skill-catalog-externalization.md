# Plan 33 — Skill Catalog Externalization (47 hardcoded → skills.json)

## Goal (2 lines)
Create `skills.json` — the data-authority catalog that `SkillProgressionSystem` and
`SkillDef.cs` already reference but which **does not exist** (verified: file missing, 47 skills
hardcoded in C#). This closes a JSON-authority invariant violation and makes skills moddable.

## Why (P1)
- Verified: `skills.json` is referenced by `SkillDef.cs` but the file does not exist; 47 skill
  definitions are hardcoded in C# — a data-authority invariant violation (Invariant 6).
- `SkillProgressionSystem` is fully implemented, save-supported, and tick-registered; the
  content is the only missing layer.
- Unlocks Plan 34+ content: skill-gated encounters, apprenticeship arcs, trade specialties.

## Files to touch
- `Assets/StreamingAssets/Data/skills.json` (CREATE — new catalog, ~50 entries)
- Read-only: `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` (confirm the skill
  schema: id, display name, category, progression curve, XP thresholds, effect fields),
  `Assets/Ashfall.Core/Survivors/SkillDef.cs` (confirm the field set the loader expects)
- `NEW SYSTEM JUSTIFICATION REQUIRED`: a loader (`SkillCatalogLoader.cs` in
  `Assets/Ashfall.Core/`) is needed to read the JSON — but only if no loader exists. Check
  first: `grep -rn "skills.json\|SkillCatalog\|SkillDef" Assets/Ashfall.Core/` to confirm
  whether a loader is already wired. If `SkillDef.cs` already loads from JSON, this is pure
  data. If it hardcodes, the loader is the one Core change required — minimal, no gameplay
  logic, just deserialization.

## Content grammar (per skill)
- snake_case `id` with prefix `skill_` (confirmed prefix in `CatalogIntegrityValidator`).
- category: survival / medical / technical / social / combat / scavenging.
- progression: XP-per-use or XP-per-event; tier thresholds (novice → competent → expert →
  master); per-tier effect magnitude.
- grounded: field surgery, water filtration, diesel mechanics, radio repair, trapping,
  negotiation, lockpicking, demolition, herbalism, surveying, etc. No magic skills.
- Each skill must hook into an existing system check (e.g. `skill_field_surgery` improves
  `MedicalSystem` outcomes; `skill_water_filtration` improves `WaterFilter` yield).

## Steps
1. Read `SkillProgressionSystem.cs` + `SkillDef.cs` end-to-end: extract the exact field schema,
  the progression curve formula, and whether loading is already JSON-wired or hardcoded.
2. Inventory all 47 hardcoded skills; classify by category and system hook.
3. Create `skills.json` with `schema_version: 1`, porting all 47 existing skills to JSON
   with byte-identical effect values (no balance changes in this task — pure externalization).
4. If a loader does not exist: create `SkillCatalogLoader.cs` using `SystemTextJsonSerializer`
  (core default), following the pattern of an existing loader (e.g. `YearOfAshCatalogLoader`).
5. Wire the loader into the catalog bootstrap path (check `GameBootstrap` catalog init — find
  where other `*CatalogLoader` files are registered and add alongside).
6. Remove the 47 hardcoded definitions from C# ONLY after the JSON path is confirmed loading
  (keep a backup comment pointing to the JSON authority; do not leave dead code).
7. Verify: `--data-integrity-selftest` (all `skill_` ids resolve); skill progression still
  works in a headless boot; save round-trip preserves skill XP.
8. xUnit: skill catalog loads, all 47 ids present, progression applies XP correctly, save
  round-trip preserves skill state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the one Core change (loader) is mechanical deserialization, but removing hardcoded
definitions is a refactor that must preserve exact behavior. Mitigated by step 6 (only remove
after JSON confirmed) and step 8 (determinism + save round-trip tests).

## Definition of Done
- `skills.json` exists with 50 entries (47 ported + 3 new), all `skill_` ids resolving,
  loader wired, hardcoded definitions removed, integrity + tests green, skill progression
  and save round-trip unchanged.

## Follow-on
- Plan 34 (research tree) uses the same externalization pattern.
- Skill-gated encounters (W12 in roadmap 31) consume the new catalog.
- Trade specialties (Plan existing 26B) can reference skill ids as prerequisites.
