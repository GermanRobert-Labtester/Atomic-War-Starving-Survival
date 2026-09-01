# Plan 114 — Year of Ash Questlines Expansion (8 → 15 questlines)

## Goal (2 lines)
Expand `year_of_ash_questlines.json` from 8 multi-stage questlines to 15. The
Year of Ash expansion's quest catalog (`YearOfAshCatalogLoader.cs` confirmed
live) drives mid-to-late-campaign faction crises (garrison blood debts, cult
revelations, seed vault expeditions, aqueduct decisions) with staged
narrative, faction standing, and day-windowed availability. 8 questlines for a
full expansion is thin; the Year of Ash faction-crisis arc needs more cases
to make the late-campaign political blocs feel active.

## Why (P2)
- Verified: `year_of_ash_questlines.json` has 8 questlines. Each has
  questlineId, title, synopsis, factionTag, minDay, maxDay, firstStageId,
  and a `stages` array (stageId, title, narrativePrompt, unlockOnDay,
  isTerminal, terminalOutcome, choices). `YearOfAshCatalogLoader.cs` loads
  it; `DoorEncounterSystem.cs` is the runtime.
- The Year of Ash expansion is the late-campaign political pillar. 8
  questlines means the garrison, Ash Sign, Rebuilders, hydro barons, and
  black ops factions each have only 1–2 active obligations. The day windows
  (185–355) have room for 7 more crises without overlap.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/year_of_ash_questlines.json` (expand `quests`
  8 → 15)
- Read-only: `Assets/Ashfall.Core/YearOfAsh/DoorEncounterCatalogLoader.cs`
  (confirm questline/stage/choice DTO and required fields)
- Read-only: `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` (confirm
  runtime consumption and save compatibility)

## Content grammar (per questline)
- `questlineId`: snake_case, prefix `quest_` + faction short name (confirmed
  convention: `quest_garrison_blood_debt`, `quest_ash_sign_revelation`).
- `title`: evocative crisis title ("The Garrison Blood Debt").
- `synopsis`: 1–3 sentences framing the faction crisis.
- `factionTag`: a faction tag from the Year of Ash faction set
  (`faction_central_garrison`, `faction_ash_sign`, `faction_rebuilders`,
  `faction_hydro_barons`, `faction_black_ops` — confirm full set in step 1).
- `minDay` / `maxDay`: day window (185–355 range; stagger to avoid overlap).
- `firstStageId`: the stageId of the entry stage.
- `stages`: array of stage objects (stageId, title, narrativePrompt,
  unlockOnDay, isTerminal, terminalOutcome, choices).
- `choices`: array with choiceId, text, nextStageId, moraleDelta,
  guiltDelta, grantItemId, grantItemQuantity, targetFactionId,
  factionStandingDelta, unlockEncounterId, conditions, outcomeNarrative.

## Steps
1. Read `DoorEncounterCatalogLoader.cs` to confirm the questline/stage/
   choice DTO and all required vs optional fields.
2. Read `DoorEncounterSystem.cs` to confirm runtime consumption and that new
   questline ids are additive (save-safe).
3. Inventory the 8 existing questlines: factionTag distribution, day
   windows. Identify which factions have only 1 questline and need more.
4. Author 7 new questlines:
   - `quest_garrison_amnesty_offer`: garrison, day 195–330 — the garrison
     offers amnesty to deserters in your shelter in exchange for conscript
     service; surrender your people or shelter the deserters and face
     embargo.
   - `quest_ash_sign_pilgrimage`: Ash Sign, day 210–340 — the cult announces
     a pilgrimage through irradiated territory; aid it, block it, or
     redirect it, with different survivor casualties.
   - `quest_rebuilder_irrigation`: Rebuilders, day 220–330 — the Rebuilders
     propose an irrigation channel that would restore farming but divert
     water from the hydro barons' aqueduct.
   - `quest_hydro_baron_water_tax`: hydro barons, day 240–350 — Baron Seraph
     imposes a water tax on all shelter access; pay, resist, or negotiate a
     counter-alliance with the Rebuilders.
   - `quest_black_ops_blackmail`: black ops, day 260–355 — a black ops
     operative offers intelligence in exchange for sheltering a defector
     the garrison wants dead.
   - `quest_garrison_mutiny`: garrison, day 280–355 — a garrison mutiny
     splits the faction; the player chooses which side to supply, changing
     which garrison controls the checkpoint.
   - `quest_rebuilder_seed_failure`: Rebuilders, day 290–355 — the seed vault
     expedition (existing questline) fails; the Rebuilders blame the player
     or a faction; the fallout reshapes the agricultural alliance.
5. Each questline: 4–7 stages, 2–4 choices per stage, at least one terminal
   outcome that shifts faction standing for two factions (crises should
   create cross-faction consequences).
6. Cross-reference: every questlineId unique; every stageId unique within
   its questline; every nextStageId resolves; every grantItemId resolves;
   every targetFactionId resolves.
7. Wire 3 new questlines to Plan 98 (standing record factions — crises
   shift faction standing).
8. Wire 2 new questlines to Plan 102 (foundry accords — crises may produce
   or break treaties).
9. Wire 2 new questlines to Plan 76 (expedition destinations — crises
   unlock new expedition sites).
10. Validate: `--data-integrity-selftest` (all ids resolve).
11. xUnit: Year of Ash quest catalog loads 15 questlines, all questlineIds
    unique, all stage chains consistent, all item/faction refs resolve.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — same structural complexity as Plan 113 (nested stages + choices).
The trap unique to Year of Ash is cross-faction consequences: a choice that
shifts standing for a faction that doesn't exist will fail validation.
Confirm all targetFactionIds resolve.

## Definition of Done
- `year_of_ash_questlines.json` has 15 questlines, all ids unique, all
  stage chains consistent, all item/faction refs resolving, 3 wired to
  standing record factions, 2 to foundry accords, 2 to expedition
  destinations, integrity + tests green.

## Follow-on
- Plan 98 (standing record factions) — crises shift faction standing.
- Plan 102 (foundry accords) — crises produce or break treaties.
- Plan 76 (expedition destinations) — crises unlock expedition sites.
- Plan 89 (epilogues) — Year of Ash outcomes feed endings.
- Plan 109 (echo quests) — crisis resolutions may trigger echoes.
