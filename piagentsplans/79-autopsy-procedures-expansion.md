# Plan 79 — Autopsy Procedures Expansion (3 → 12 procedures)

## Goal (2 lines)
Expand `autopsy_procedures.json` from 3 verified procedures to 12. The autopsy
system (`AutopsySystem.cs` confirmed live) lets the player perform post-mortem
examinations on dead survivors to determine cause of death, unlock research, and
identify disease/pathogen risks. 3 procedures is too few for a medical-death
system that should cover the full range of ASHFALL's death causes.

## Why (P2)
- Verified: `autopsy_procedures.json` has 3 entries (procedure_id, display_name,
  required_tools, required_consumables, airborne_risk, pathogen_risk,
  procedure_hours, possible_findings, research_unlocks).
  `AutopsySystem.cs` and `AutopsyProcedureCatalogLoader.cs` are confirmed live.
- Creates the medical-investigation pillar: autopsies reveal why survivors died
  — radiation, disease, trauma, chemical exposure, starvation, hypothermia.
  This information unlocks research and prevents future deaths. 3 procedures
  cover only radiation, toxicology, and containment; the other death causes are
  invisible.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/autopsy_procedures.json` (expand 3 → 12 procedures)
- Read-only: `Assets/Ashfall.Core/AutopsySystem.cs`,
  `Assets/Ashfall.Core/AutopsyProcedureCatalogLoader.cs` (confirm schema and how
  findings/research_unlocks resolve)
- `Assets/StreamingAssets/Data/items.json` (required_tools and
  required_consumables must resolve as item ids)

## Content grammar (per procedure)
- snake_case `id` with prefix `procedure_` (confirmed prefix).
- Death-cause coverage: each procedure targets a distinct cause of death
  (radiation, toxicology, pathogen, trauma/ballistic, hypothermia/exposure,
  starvation/dehydration, asphyxiation/CO, blast injury, chemical, disease
  outbreak, combat trauma, unknown/suspicious death).
- required_tools: 2–4 item ids (medical scissors, protective gloves, surgical
  kit, surgical mask, bone saw, etc.) — must resolve in items.json.
- required_consumables: 1–3 item ids (bandages, clean water, antibiotics,
  sterilizing agent) — must resolve in items.json.
- airborne_risk: 0.05–0.35 (risk to the examiner of airborne contamination).
- pathogen_risk: 0.03–0.25 (risk of pathogen release during procedure).
- procedure_hours: 2–8 (time cost).
- possible_findings: 2–4 finding ids (finding_* prefix) describing what the
  autopsy can reveal.
- research_unlocks: 1–2 research ids (research_* prefix) unlocked by findings.
- Risk-reward trade-off: higher-risk procedures unlock more valuable research.

## Steps
1. Read `AutopsySystem.cs` to confirm how procedures are selected (by cause of
   death? by player choice?) and how findings/research_unlocks are applied.
2. Read `AutopsyProcedureCatalogLoader.cs` to confirm the schema and how
   possible_findings and research_unlocks resolve.
3. Read `items.json` to confirm which medical tool and consumable items exist;
   note gaps for step 6.
4. Author 9 new procedures:
   - Trauma/ballistic autopsy (combat deaths, bullet wounds, blast injury).
   - Hypothermia/exposure autopsy (cold deaths, frostbite).
   - Starvation/dehydration autopsy (need-death, organ failure from starvation).
   - Asphyxiation/CO autopsy (air-filtration failure, CO poisoning).
   - Blast injury autopsy (explosion deaths, shrapnel, concussive damage).
   - Chemical exposure autopsy (toxic environment, chemical weapons residue).
   - Disease outbreak autopsy (epidemic deaths, pathogen identification).
   - Combat trauma autopsy (detailed combat-death analysis, wound patterns).
   - Suspicious death autopsy (unknown cause, possible foul play, poisoning).
5. Each procedure: distinct required_tools/consumables, risk profile, findings,
   and research unlocks. Higher-risk procedures (containment, disease outbreak)
   unlock more valuable research.
6. Add any missing medical item ids to `items.json` (e.g. `bone_saw`,
   `autopsy_table`, `sterilizing_agent`) — only if a procedure's required items
   do not exist.
7. Cross-reference: every procedure_id unique; every required_tool and
   required_consumable resolves in items.json; every finding_* and research_*
   id follows existing conventions.
8. Wire 3 procedures into Plan 09 (medical disease depth) — disease outbreak,
   chemical exposure, and pathogen autopsies feed the disease system.
9. Validate: `--data-integrity-selftest` (all ids resolve).
10. xUnit: autopsy catalog loads 12 procedures, all ids unique, all tool/
    consumable ids resolve, risk values within valid ranges, findings and
    research_unlocks are non-empty arrays.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is finding/research id resolution (step 7):
confirm the finding_* and research_* prefixes are accepted by the catalog
validator before authoring.

## Definition of Done
- `autopsy_procedures.json` has 12 procedures, all ids resolving, 3 wired to
  the medical disease system, integrity + tests green.

## Follow-on
- Plan 09 (medical disease depth) — disease/chemical/pathogen autopsies feed
  the disease system.
- Plan 65 (final wishes) — autopsy findings can reveal a dying survivor's
  unknown condition.
- Plan 69 (grave epitaphs) — autopsy cause-of-death links to epitaph cause.
- Plan 55 (crafting recipes) — autopsy tools may need crafting recipes.
- Plan 27's consent-aware casework layer consumes autopsy findings.
