# Plan 102 — Foundry Accords Expansion (4 → 10 inter-faction treaties)

## Goal (2 lines)
Expand `foundry_accords.json` from 4 verified treaties to 10. The Foundry
accord system (`SilentFoundryTypes.cs` confirmed live) defines inter-faction
treaties — each has signatory factions, demarcated territory, water/power
allocation, tariff schedule, treaty articles, and penalties. 4 treaties is
too few for a multi-faction diplomatic system.

## Why (P2)
- Verified: `foundry_accords.json` has 4 entries (treaty_id, ratified_day,
  treaty_title, signatory_factions, demarcated_territory, water_allocation_lpm,
  power_quota_kw, tariff_schedule, treaty_articles, penalties, tags).
  `SilentFoundryTypes.cs` and `SilentFoundryHeadlessDemo.cs` are confirmed
  in Core.
- Creates the diplomatic-treaty pillar: treaties are how factions formalize
  their relationships — resource allocation, territorial boundaries, labor
  schedules, trade terms. 4 treaties cover the Foundry's core relationships
  (brine pipe, labour schedule, road iron, cluster charter); 10 treaties
  would cover the full diplomatic web including conflict resolution,
  resource disputes, and crisis pacts.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/foundry_accords.json` (expand 4 → 10 treaties)
- Read-only: `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs` (confirm
  schema and how treaties are loaded and applied)
- `Assets/StreamingAssets/Data/foundry_faction.json` (signatory_factions
  must resolve)

## Content grammar (per treaty)
- snake_case `treaty_id` with prefix `treaty_` (confirmed prefix).
- `ratified_day`: integer day the treaty was ratified.
- `treaty_title`: evocative title ("The Brine Pipe & Iodine Exchange").
- `signatory_factions`: array of faction ids — must resolve to existing
  factions.
- `demarcated_territory`: 1 sentence describing the territorial boundary.
- `water_allocation_lpm`: float (liters per minute allocated).
- `power_quota_kw`: float (kilowatts allocated).
- `tariff_schedule`: 1–2 sentences describing the trade terms.
- `treaty_articles`: 1 paragraph with ARTICLE 1, 2, 3 clauses. Match the
  existing formal, legalistic tone.
- `penalties`: 1 sentence describing what happens if the treaty is broken.
- `tags`: array of keyword tags for categorization.
- Diversity: cover resource exchange, labor, territorial, security,
  crisis response, and diplomatic recognition treaties.

## Steps
1. Read `SilentFoundryTypes.cs` to confirm the schema and how treaties are
   loaded and applied (do they affect resource allocation? faction standing?).
2. Read the existing 4 treaties to confirm the quality bar (the brine pipe,
   labour schedule, road iron, and cluster charter are the model — formal,
  specific, grounded).
3. Read `foundry_faction.json` to confirm which faction ids exist for
   signatory_factions.
4. Author 6 new treaties:
   - `treaty_saltworks_access`: Foundry + Office + Scale — saltworks access
     in exchange for pipe maintenance and water rights.
   - `treaty_coal_window`: Foundry + Cutters — ice-road coal haulage
     schedule, with penalty for missed windows.
   - `treaty_membrane_repair`: Foundry + Office — membrane hall repair
     pact, exchanging castings for iodine and labor.
   - `treaty_crisis_mutual_aid`: Foundry + all factions — mutual aid in
     case of fallout storm or siege, with no tariff.
   - `treaty_apprentice_exchange`: Foundry + Cluster — apprentice training
     exchange, exchanging casting skills for literacy and arithmetic.
   - `treaty_the_incident_book`: Foundry + Office — incident reporting
     pact, requiring open incident entries before charter renewal.
5. Each treaty: distinct signatory_factions, demarcated_territory,
   water/power allocation, tariff schedule, articles, and penalties. Match
   the existing formal, legalistic tone.
6. Cross-reference: every treaty_id unique; every signatory_faction resolves
   to an existing faction; every tag follows existing conventions.
7. Wire 2 treaties into Plan 103 (foundry treaty consequences — treaties
   have consequence policies).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: foundry accords catalog loads 10 treaties, all ids unique, all
   signatory_factions resolve, all articles and penalties non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is signatory_faction resolution (step 3):
confirm all faction ids exist in foundry_faction.json or the faction catalog.

## Definition of Done
- `foundry_accords.json` has 10 treaties, all ids resolving, 2 wired to
  treaty consequences, integrity + tests green.

## Follow-on
- Plan 103 (foundry treaty consequences) — treaties have consequence policies.
- Plan 98 (standing record factions) — treaties reference factions.
- Plan 92 (faction war dialogue) — treaties are discussed in dialogue.
- Plan 89 (muster epilogues) — treaty outcomes affect endings.
- Existing 25 (faction ecology) — this plan provides the diplomatic data.
