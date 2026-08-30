# Plan 109 — Moral Choice Echo Quests Expansion (32 → 60 echo quests)

## Goal (2 lines)
Expand the `echo_quests.quests` array in `moral_choice_chains.json` from 32
callback quests to 60. The MoralChoiceSystem (`MoralChoiceSystem.cs` confirmed
live) fires echo quests when a prior moral quest was resolved a specific way,
referencing the earlier choice and presenting delayed consequences. 32 echoes
across 4 branches is too few to make early moral decisions feel like they echo
through the whole campaign.

## Why (P2)
- Verified: `moral_choice_chains.json` has 4 branches (by design — the 4
  permanent paths), 88 quest_gates (healthy), but only 32 echo_quests. The
  echo layer is the thinnest part of the moral-choice system and the part that
  makes choices *feel* like they matter weeks later.
- Echo quests are the single best lever for the "choices that echo later"
  design goal. A mercy decision on Day 20 should produce a callback on Day 60.
  32 echoes across ~88 gated quests means most choices have no delayed payoff.
- Pure DATA work — zero new Core code. `MoralChoiceChainCatalogLoader.cs`
  loads the array; `MoralChoiceSystem.cs` fires echoes by matching
  `triggered_by` + `triggered_by_choice` against resolved quest history.

## Files to touch
- `Assets/StreamingAssets/Data/moral_choice_chains.json` (expand
  `echo_quests.quests` 32 → 60)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs`
  (confirm echo quest DTO fields)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` (confirm
  how `triggered_by` / `triggered_by_choice` / `min_days_after` resolve)

## Content grammar (per echo quest)
- `quest_id`: snake_case, prefix `quest_moral_echo_` (confirmed convention).
- `triggered_by`: the quest_id of the earlier moral quest whose resolution
  fires this echo.
- `triggered_by_choice`: integer index into the source quest's choices, or
  `null` if any resolution fires it.
- `min_days_after`: minimum days between the triggering resolution and this
  echo becoming available (creates the "delayed payoff" feeling).
- `branch`: the branch this echo belongs to, or `null` if branch-agnostic.

## Steps
1. Read `MoralChoiceChainData.cs` to confirm the echo quest DTO and that no
   additional fields are required by the loader.
2. Read `MoralChoiceSystem.cs` to confirm how `triggered_by` is matched
   against resolved quest history (by id only, or id + choice index).
3. Inventory all 88 `quest_gates` and the 32 existing echoes; map which gated
   quests currently have no echo callback.
4. Author 28 new echo quests distributed across the 4 branches:
   - 8 mercy-road echoes (delayed kindness payoffs — the person you helped
     returns, the child you fed grows, the raider you spared warns you).
   - 8 iron-way echoes (delayed ruthlessness consequences — the camp you
     raided rebuilt and remembers, the survivor you abandoned left a note).
   - 7 listener-thread echoes (delayed wisdom callbacks — the story you
     collected becomes useful, the witness you interviewed resurfaces).
   - 5 broken-compact echoes (delayed betrayal fallout — the trust you
     weaponized collapses, the ally you sold out finds you).
5. Each echo: distinct `triggered_by` (reference a real gated quest),
   meaningful `min_days_after` (20–80 days for delayed payoff), correct
   `branch`.
6. Ensure no two echoes share the same `quest_id`; every `triggered_by`
   resolves to an existing quest_id in `quest_gates`.
7. Wire 6 echoes into Plan 95 (journal voice — echo resolutions trigger
   journal entries).
8. Wire 4 echoes into Plan 88 (confessions — echo survivors may confess).
9. Wire 3 echoes into Plan 89 (epilogues — echo outcomes feed ending
   determination).
10. Validate: `--data-integrity-selftest` (all triggered_by ids resolve).
11. xUnit: moral choice chain catalog loads 60 echo quests, all triggered_by
    resolve to existing quest_gates, all quest_ids unique.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `triggered_by_choice` indexing (step 2):
confirm whether choice indices are 0-based and whether `null` means "any
resolution" before authoring.

## Definition of Done
- `moral_choice_chains.json` echo_quests.quests has 60 entries, all
  triggered_by resolving, all quest_ids unique, 6 wired to journal voice, 4 to
  confessions, 3 to epilogues, integrity + tests green.

## Follow-on
- Plan 95 (journal voice) — echo resolutions trigger journal entries.
- Plan 88 (confessions) — echo survivors may confess.
- Plan 89 (epilogues) — echo outcomes determine endings.
- Plan 110 (gossip) — echo events propagate as camp chatter.
- Plan 100 (faction reactions) — echo choices shift faction standing.
