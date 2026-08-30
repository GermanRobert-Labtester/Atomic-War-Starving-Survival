# Plan 115 — Crossing Encounters & Crises Expansion (10 enc + 5 crises → 25 + 12)

## Goal (2 lines)
Expand `crossing_encounters.json` from 10 encounters + 5 crises to 25
encounters + 12 crises. The Crossing expansion's catalog
(`CrossingCatalog.cs` confirmed live) drives location-based encounters (with
choices and threat levels) and multi-phase community crises (forfeit, vote,
arbitration). 10 encounters + 5 crises is thin for the Crossing's
nobody's-charter settlement pillar.

## Why (P2)
- Verified: `crossing_encounters.json` has `encounters` (10) and `crises`
  (5). Encounters have id, name, target_location, description, threat_level,
  choices. Crises have id, name, phases, description, resolution.
  `CrossingCatalog.cs` loads both; `CrossingSession.cs` runs the runtime.
- The Crossing is the fourth coordinated expansion — a contested charter
  settlement where factions (Underwrite, Compact, etc.) arbitrate debt,
  votes, and territory. 5 crises means the political life of the settlement
  is over after a few events. 10 encounters means exploration of Crossing
  locations is sparse.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/crossing_encounters.json` (expand
  `encounters` 10 → 25, `crises` 5 → 12)
- Read-only: `Assets/Ashfall.Core/CrossingCatalog.cs` (confirm encounter/
  crisis DTO and required fields)
- Read-only: `Assets/Ashfall.Core/CrossingSession.cs` (confirm how
  target_location, threat_level, and choices resolve)

## Content grammar (per encounter)
- `id`: snake_case, prefix `encounter_` (confirm in step 1).
- `name`: evocative encounter title.
- `target_location`: a Crossing location id (must resolve).
- `description`: 1–3 sentences setting the scene.
- `threat_level`: integer 1–5 (confirm range in step 1).
- `choices`: array of choice objects (confirm choice DTO in step 1 — likely
  choiceId/text/outcome/...).

## Content grammar (per crisis)
- `id`: snake_case, prefix `crisis_` (confirmed convention).
- `name`: evocative crisis title ("Wyn's Granary Forfeit").
- `phases`: array of phase names (strings — the escalation stages).
- `description`: 1–2 sentences framing the crisis.
- `resolution`: 1 sentence describing the resolution condition.

## Steps
1. Read `CrossingCatalog.cs` to confirm the encounter and crisis DTOs, all
   required vs optional fields, and the choice structure.
2. Read `CrossingSession.cs` to confirm how `target_location` resolves
   (against which location catalog) and how `threat_level` scales.
3. Inventory the 10 existing encounters and 5 crises: which Crossing
   locations are covered, which factions are involved. Identify gaps.
4. Author 15 new encounters:
   - 5 trade-route encounters (caravan ambush, smuggler checkpoint, frozen
     barge, toll bridge, black-market drop).
   - 5 hazard encounters (collapsed crossing, contaminated ford, ice
     fracture, UXO field crossing, blackout tunnel).
   - 5 social encounters (refugee blockade, faction patrol standoff,
     desperate family, wounded trader, orphaned child at the gate).
5. Author 7 new crises:
   - `crisis_the_water_claim`: the hydro barons claim the Crossing's well;
     phases: Claim, Counter-petition, Arbitration, Ruling.
   - `crisis_the_grain_riot`: ration shortage triggers a riot; phases:
     Shortage, Hoarding Accusation, Distribution Force, Calm or Collapse.
   - `crisis_the_charter_amendment`: a faction proposes amending the
     charter to exclude a group; phases: Proposal, Debate, Vote, Enactment.
   - `crisis_the_debt_forgiveness`: a bloc demands blanket debt
     forgiveness; phases: Demand, Counter, Assembly, Verdict.
   - `crisis_the_refugee_admission`: a refugee group requests admission;
     phases: Arrival, Vetting, Resource Test, Admission or Refusal.
   - `crisis_the_arbitrator_bribe`: an arbitrator is accused of bribery;
     phases: Accusation, Evidence, Tribunal, Replacement or Vindication.
   - `crisis_the_quarantine_break`: a diseased resident breaks quarantine;
     phases: Break, Exposure Trace, Containment, Reconciliation.
6. Each encounter: distinct target_location, threat_level scaled to the
   hazard, 2–4 choices with distinct outcomes.
7. Each crisis: 3–5 phases, a resolution that is achievable through the
   existing arbitration/assembly mechanics.
8. Cross-reference: every target_location resolves; every id unique; no two
   encounters share the same target_location + threat_level pair.
9. Wire 4 new crises to Plan 98 (standing record factions — crises shift
   faction standing in the Crossing).
10. Wire 3 new encounters to Plan 76 (expedition destinations — encounters
    reveal new Crossing-adjacent locations).
11. Validate: `--data-integrity-selftest` (all target_locations resolve).
12. xUnit: Crossing catalog loads 25 encounters + 12 crises, all ids
    unique, all target_locations resolve, all choices non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the choice DTO for encounters must be confirmed (step 1) before
authoring; if choices have nested outcome fields (faction standing, item
grants), cross-references must resolve. Crises are simpler (string phases).

## Definition of Done
- `crossing_encounters.json` has 25 encounters + 12 crises, all ids unique,
  all target_locations resolving, 4 crises wired to standing record factions,
  3 encounters to expedition destinations, integrity + tests green.

## Follow-on
- Plan 98 (standing record factions) — crises shift faction standing.
- Plan 76 (expedition destinations) — encounters reveal locations.
- Plan 102 (foundry accords) — crises may produce treaties.
- Plan 89 (epilogues) — Crossing outcomes feed endings.
- Plan 112 (disease catalog) — the quarantine break crisis ties to disease.
