# Plan 103 — Foundry Treaty Consequences Expansion (6 → 15 consequence policies)

## Goal (2 lines)
Expand `foundry_treaty_consequences.json` from 6 verified policies to 15.
The Foundry treaty consequence system (`SilentFoundryConsequencePolicy.cs`
confirmed live) defines outcome policies for treaty compliance or violation
— each policy links a treaty to a faction and an outcome (met, missed,
breached). 6 policies is too few for 10 treaties (Plan 102).

## Why (P2)
- Verified: `foundry_treaty_consequences.json` has 6 entries (treaty_id,
  faction_id, outcome, consequence_text, mechanical_effect).
  `SilentFoundryConsequencePolicy.cs` is confirmed in Core.
- Creates the treaty-enforcement pillar: treaties need consequences —
  when a faction meets, misses, or breaches a treaty, the system applies
  mechanical effects (resource changes, standing shifts, access changes).
  6 policies covers the existing 4 treaties; 15 policies covers the
  expanded 10 treaties from Plan 102.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` (expand
  6 → 15 policies)
- Read-only: `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
  (confirm schema and how outcomes and mechanical_effects are applied)
- `Assets/StreamingAssets/Data/foundry_accords.json` (treaty_id must resolve
  — expanded by Plan 102)

## Content grammar (per policy)
- `treaty_id`: must resolve to an existing treaty in foundry_accords.json.
- `faction_id`: must resolve to an existing faction.
- `outcome`: "met" / "missed" / "breached" — the treaty compliance state.
- `consequence_text`: 1–2 sentences describing the narrative consequence.
- `mechanical_effect`: structured effect (resource delta, standing delta,
  access change — confirm the exact schema by reading the existing entries).
- Each treaty should have policies for at least 2 outcomes (met and missed
  or breached).

## Steps
1. Read `SilentFoundryConsequencePolicy.cs` to confirm the schema and how
   outcomes and mechanical_effects are applied.
2. Read the existing 6 policies to confirm the quality bar and the
   mechanical_effect schema.
3. Read `foundry_accords.json` (expanded by Plan 102) to confirm which
   treaty ids exist.
4. Author 9 new policies for the 6 new treaties from Plan 102:
   - `treaty_saltworks_access` met: water allocation honored, +standing.
   - `treaty_saltworks_access` breached: pipe maintenance withheld,
     -standing, water access reduced.
   - `treaty_coal_window` met: coal delivered on schedule, foundry
     operational.
   - `treaty_coal_window` missed: coal window forfeit, foundry production
     reduced.
   - `treaty_membrane_repair` met: membrane repaired, contamination reduced.
   - `treaty_membrane_repair` breached: membrane fails, contamination
     rises, iodine allocation suspended.
   - `treaty_crisis_mutual_aid` met: mutual aid delivered during crisis,
     all factions +standing.
   - `treaty_crisis_mutual_aid` breached: aid withheld during crisis,
     breaching faction -standing, crisis worsens.
   - `treaty_the_incident_book` met: incident book clean, charter
     renewal proceeds.
5. Each policy: distinct treaty_id, faction_id, outcome, consequence_text,
   and mechanical_effect. Match the existing grounded tone.
6. Cross-reference: every treaty_id resolves to a treaty in
   foundry_accords.json; every faction_id resolves; every outcome is a
   valid value.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: foundry treaty consequences catalog loads 15 policies, all
   treaty_ids resolve, all faction_ids resolve, all outcomes valid, all
   consequence_text non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is mechanical_effect schema (step 1):
confirm the exact structure of mechanical_effect before authoring — it
may be a string, an object, or a structured effect.

## Definition of Done
- `foundry_treaty_consequences.json` has 15 policies, all treaty_ids
  resolving, all faction_ids resolving, integrity + tests green.

## Follow-on
- Plan 102 (foundry accords) — treaties define the pacts; this plan
  defines their consequences.
- Plan 98 (standing record factions) — consequences affect faction standing.
- Plan 89 (muster epilogues) — treaty outcomes affect endings.
- Plan 92 (faction war dialogue) — treaty breaches are discussed in dialogue.
- Existing 25 (faction ecology) — this plan provides the enforcement data.
