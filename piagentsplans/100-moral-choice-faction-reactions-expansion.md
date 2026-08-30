# Plan 100 — Moral Choice Faction Reactions Expansion (1 → 6 moral threshold events)

## Goal (2 lines)
Expand `moral_choice_faction_reactions.json` from 1 verified threshold
reaction to 6. The moral choice faction reaction system
(`MoralChoiceFactionReactionsCatalogLoader.cs` confirmed live) defines NPC
dialogue triggered when the player crosses moral band boundaries — each
event fires once per save with peacekeeper, raider, and knowledge-keeper
dialogue. 1 event (bounty issued) is far too few for a moral-choice system
with 4 branches and 25+ quest gates.

## Why (P2)
- Verified: `moral_choice_factions_reactions.json` has 1 threshold_reaction
  (`moral_event_bounty_issued`) with peacekeeper_dialogue, raider_dialogue,
  and knowledge_keeper_dialogue arrays, each with speaker, location, and
  lines. `MoralChoiceFactionReactionsCatalogLoader.cs` and
  `MoralChoiceFactionReactionsData.cs` are confirmed live.
- Creates the moral-feedback pillar: the world should acknowledge what the
  player has become — not just through flags and endings, but through
  real-time NPC reactions. When the player crosses a moral threshold, the
  world's factions should notice and respond. 1 event means only the worst
  moral extreme gets feedback; the positive and neutral extremes are silent.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` (expand
  1 → 6 threshold reactions)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`
  (confirm schema and how threshold_reactions are keyed and triggered)

## Content grammar (per threshold reaction)
- Key: `moral_event_*` snake_case id describing the moral boundary crossed.
- `event_description`: 1 sentence explaining when the event fires.
- `peacekeeper_dialogue`: array of dialogue blocks (speaker, location,
  lines array of 3–5 lines). The peacekeeper/lawful faction's reaction.
- `raider_dialogue`: array of dialogue blocks. The outlaw/violent faction's
  reaction.
- `knowledge_keeper_dialogue`: array of dialogue blocks. The neutral/
  scholarly faction's reaction.
- Each faction's reaction should reflect its values — peacekeepers condemn
  evil, raiders respect ruthlessness, knowledge keepers observe and judge
  impartially.
- Tone: match the existing quality — grounded, specific, human. No
  moralizing. The world notices; it doesn't lecture.

## Steps
1. Read `MoralChoiceFactionReactionsCatalogLoader.cs` to confirm the schema
   and how threshold_reactions are keyed (by event id) and triggered (by
   moral band crossing).
2. Read the existing event (`moral_event_bounty_issued`) to confirm the
   quality bar and the 3-faction dialogue structure.
3. Author 5 new threshold reactions:
   - `moral_event_saint_recognized`: fires when player enters VeryGood band
     (+100 or above). Peacekeepers offer alliance; raiders mock but back off;
     knowledge keepers record the player's name.
   - `moral_event_first_mercy`: fires when player completes first mercy-
     road quest. Peacekeepers approve; raiders are wary; knowledge keepers
     note the choice.
   - `moral_event_first_betrayal`: fires when player completes first
     betrayal quest. Peacekeepers warn; raiders nod; knowledge keepers
     note the choice.
   - `moral_event_neutral_drift`: fires when player stays neutral through
     10+ moral choices. Peacekeepers are indifferent; raiders are
     suspicious; knowledge keepers are intrigued.
   - `moral_event_blood_toll`: fires when player enters Evil band (-50 or
     below). Peacekeepers withdraw aid; raiders offer work; knowledge
     keepers close their doors.
4. Each event: 3 faction dialogue arrays, each with 1–2 dialogue blocks,
   each with 3–5 lines. Match the existing grounded tone.
5. Cross-reference: every event id unique; every event key matches a moral
   band boundary that the moral choice system can actually trigger.
6. Wire 2 events into Plan 95 (journal voice — moral threshold events
   trigger journal entries).
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: moral choice faction reactions catalog loads 6 events, all ids
   unique, all 3 faction dialogue arrays non-empty, all lines non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is event triggering (step 5): confirm the
moral choice system actually fires events at the specified moral band
boundaries before authoring.

## Definition of Done
- `moral_choice_faction_reactions.json` has 6 threshold reactions, all ids
  unique, 2 wired to journal voice, integrity + tests green.

## Follow-on
- Plan 95 (journal voice) — moral events trigger journal entries.
- Plan 66 (guilt sources) — moral events generate guilt.
- Plan 88 (confession secrets) — moral events may trigger confessions.
- Plan 89 (muster epilogues) — moral events determine ending availability.
- Existing moral_choice_chains.json — this plan provides the world-reaction
  data the moral choice system needs.
