# Plan 113 — Verdict Questlines Expansion (8 → 15 questlines)

## Goal (2 lines)
Expand `verdict_questlines.json` from 8 multi-stage questlines to 15. The
Verdict expansion's quest catalog (`VerdictQuestCatalogLoader.cs` confirmed
live) drives investigation chains tied to the Tempest and Archivist factions,
each with staged narrative, faction standing, and day-windowed availability.
8 questlines for a full expansion is thin; the Verdict investigation arc needs
more cases to make the signal-intelligence and counting-house themes land.

## Why (P2)
- Verified: `verdict_questlines.json` has 8 questlines. Each has
  questlineId, title, synopsis, factionTag, minDay, maxDay, firstStageId,
  and a `stages` array (stageId, title, narrativePrompt, unlockOnDay,
  isTerminal, terminalOutcome, choices with choiceId/text/nextStageId/
  moraleDelta/guiltDelta/grantItemId/factionStandingDelta/...).
  `VerdictQuestCatalogLoader.cs` loads it; `VerdictQuestMigration.cs`
  handles save migration.
- The Verdict expansion is one of the four coordinated expansions. 8
  questlines means the investigation pillar feels short and the Tempest/
  Archivist factions have too few active obligations. The day windows
  (160–360) have room for 7 more cases without overlap.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_questlines.json` (expand `quests` 8 → 15)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs`
  (confirm stage/choice DTO and required fields)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs` (confirm
  save compatibility is unaffected by new questline ids)

## Content grammar (per questline)
- `questlineId`: snake_case, prefix `quest_verdict_` (confirmed convention).
- `title`: evocative case title ("The Warm Range", "The Reckoning Call").
- `synopsis`: 1–2 sentences framing the investigation.
- `factionTag`: `faction_the_tempest` or `faction_archivists` (confirm valid
  faction tags in step 1; may add a third Verdict faction if warranted).
- `minDay` / `maxDay`: day window (160–360 range; stagger to avoid overlap).
- `firstStageId`: the stageId of the entry stage.
- `stages`: array of stage objects (stageId, title, narrativePrompt,
  unlockOnDay, isTerminal, terminalOutcome, choices).
- `choices`: array with choiceId, text, nextStageId, moraleDelta,
  guiltDelta, grantItemId, grantItemQuantity, targetFactionId,
  factionStandingDelta, unlockEncounterId, conditions, outcomeNarrative.

## Steps
1. Read `VerdictQuestCatalogLoader.cs` to confirm the questline/stage/choice
   DTO and all required vs optional fields.
2. Read `VerdictQuestMigration.cs` to confirm new questline ids do not break
   save migration (additive ids should be safe).
3. Inventory the 8 existing questlines: factionTag distribution, day windows,
   terminal outcomes. Identify gaps (e.g. only Tempest + Archivist; no
   neutral/independent cases).
4. Author 7 new questlines:
   - `quest_verdict_the_dead_frequency`: Tempest, day 170–340 — a broadcast
     station transmitting a count that ended years ago; the player must
     decide whether to silence or preserve it.
   - `quest_verdict_the_missing_reel`: Archivists, day 180–350 — a single
     reel is absent from the 2016-reel count; the gap is a person.
   - `quest_verdict_the_cold_reading`: Tempest, day 190–360 — a radiation
     monitoring post reports a reading that should be impossible; the
     instruments or the operator are lying.
   - `quest_verdict_the_unsigned_tally`: Archivists, day 200–360 — a tally
     sheet with no signature and a body count that doesn't match the graves.
   - `quest_verdict_the_interference_pattern`: Tempest, day 220–360 — a
     signal pattern suggests a second transmitter the count never
     registered; finding it changes what "complete" means.
   - `quest_verdict_the_last_entry`: Archivists, day 240–360 — the final log
     entry in a sealed facility names a person the records say was never
     there.
   - `quest_verdict_the_open_count`: independent (no factionTag or a neutral
     tag), day 250–360 — a count that was never closed; the player chooses
     whether to close it or leave it open, with faction consequences either
     way.
5. Each questline: 4–7 stages, 2–4 choices per stage, at least one terminal
   outcome that shifts faction standing, at least one that grants an item.
6. Cross-reference: every questlineId unique; every stageId unique within its
   questline; every nextStageId resolves to a stage in the same questline;
   every grantItemId resolves in the item catalog; every targetFactionId
   resolves.
7. Wire 3 new questlines to Plan 82 (Verdict locations — cases reference
   investigation sites).
8. Wire 2 new questlines to Plan 94 (Verdict radio — cases reference radio
   broadcasts).
9. Wire 2 new questlines to Plan 93 (Verdict NPCs — cases involve named
   Verdict NPCs).
10. Validate: `--data-integrity-selftest` (all ids resolve).
11. xUnit: Verdict quest catalog loads 15 questlines, all questlineIds
    unique, all stage chains internally consistent, all item/faction refs
    resolve.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — questlines are the most structurally complex data in this batch
(nested stages + choices + cross-references). The traps are: nextStageId
chains that dead-end, and grantItemId/targetFactionId that don't resolve.
Validate incrementally.

## Definition of Done
- `verdict_questlines.json` has 15 questlines, all ids unique, all stage
  chains consistent, all item/faction refs resolving, 3 wired to Verdict
  locations, 2 to Verdict radio, 2 to Verdict NPCs, integrity + tests green.

## Follow-on
- Plan 82 (Verdict locations) — cases reference investigation sites.
- Plan 94 (Verdict radio) — cases reference broadcasts.
- Plan 93 (Verdict NPCs) — cases involve named NPCs.
- Plan 109 (echo quests) — Verdict quest resolutions may trigger echoes.
- Plan 89 (epilogues) — Verdict outcomes feed endings.
