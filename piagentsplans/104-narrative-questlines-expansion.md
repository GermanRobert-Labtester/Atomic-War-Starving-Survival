# Plan 104 — Narrative Questlines Expansion (4 → 12 survivor-specific questlines)

## Goal (2 lines)
Expand `narrative_questlines.json` from 4 verified questlines to 12. The
narrative questline system (confirmed live via ContentUtilizationScanner)
defines multi-stage personal quests for named survivors — each questline has
a survivor, target location, and 4 stages (Discovery, Investigation, Crisis,
Resolution) with branching choices and trait grants. 4 questlines is too few
for a roster of 129 survivors.

## Why (P2)
- Verified: `narrative_questlines.json` has 4 questlines (quest_id,
  survivor_id, title, target_location_id, stages with stage number, name,
  description, objective_items, branch_a, branch_b). The system is confirmed
  live via ContentUtilizationScanner. Each questline is a richly written
  4-stage personal arc with moral branching.
- Creates the personal-quest pillar: narrative questlines are the game's
  deepest character arcs — each is a 4-stage story (discovery → investigation
  → crisis → resolution) with a moral branch that grants a trait. 4
  questlines covers 4 survivors; 12 covers the core cast with distinct
  personal stories.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/narrative_questlines.json` (expand 4 → 12)
- Read-only: grep for the consuming system to confirm how quest_id,
  survivor_id, and target_location_id resolve
- `Assets/StreamingAssets/Data/starting_survivors.json` (survivor_id must
  resolve)

## Content grammar (per questline)
- `quest_id`: snake_case with prefix `quest_` (confirmed prefix).
- `survivor_id`: must resolve to an existing survivor in
  starting_survivors.json.
- `title`: 2–5 words evoking the quest's theme.
- `target_location_id`: must resolve to an existing `loc_` id.
- `stages`: 4 stages (0=Discovery, 1=Investigation, 2=Crisis, 3=Resolution):
  - `stage`: integer (0–3).
  - `name`: 1 word (Discovery, Investigation, Crisis, Resolution).
  - `description`: 2–4 sentences of prose. Match the existing quality —
    grounded, specific, emotionally charged.
  - `objective_items`: array of item ids needed for this stage.
  - `branch_a` / `branch_b` (Crisis stage only): 2 moral branches, each with:
    - `id`: snake_case branch id.
    - `label`: 2–3 words.
    - `description`: 1–2 sentences describing the outcome.
    - `trait_granted`: trait id (trait_* prefix).
    - `morale_delta`: integer morale change.
  - `Resolution` stage: describes both branch outcomes.
- Moral weight: every crisis should force a genuine dilemma with no obvious
  right answer. The two branches should be equally viable but lead to
  different traits and consequences.

## Steps
1. Grep for the consuming system to confirm how quest_id, survivor_id, and
   target_location_id resolve.
2. Read the existing 4 questlines to confirm the quality bar (The Cracked
   Floor, The Dying Signal, The Refugee Mass Influx, The ARS Crisis — each
   is a richly written personal arc with moral branching).
3. Read `starting_survivors.json` to confirm which survivor ids exist.
4. Author 8 new questlines for 8 named survivors:
   - The machinist who built a weapon they regret (target: machine works).
   - The teacher who lost their students (target: school gymnasium).
   - The cook who poisoned a raider camp (target: field kitchen).
   - The farmer whose land was irradiated (target: abandoned farm).
   - The priest who lost their faith (target: ash sign shrine).
   - The journalist who buried a story (target: printworks).
   - The electrician who caused a blackout (target: power substation).
   - The hunter who killed a friend (target: hunting blind).
5. Each questline: 4 stages with a moral crisis branch. Match the existing
   quality — grounded, specific, emotionally charged, no obvious right
   answer.
6. Cross-reference: every quest_id unique; every survivor_id resolves; every
   target_location_id resolves to an existing location; every objective_item
   resolves in items.json; every trait_granted follows existing conventions.
7. Wire 2 questlines into Plan 52 (recurring NPC arcs — questline survivors
   recur).
8. Wire 2 questlines into Plan 95 (journal voice — quest events trigger
   journal entries).
9. Validate: `--data-integrity-selftest` (all ids resolve).
10. xUnit: narrative questline catalog loads 12 questlines, all ids unique,
    all survivor_ids resolve, all target_location_ids resolve, all stages
    have non-empty descriptions, all crisis stages have 2 branches.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is survivor_id resolution (step 3): confirm
each survivor exists in starting_survivors.json before authoring.

## Definition of Done
- `narrative_questlines.json` has 12 questlines, all ids resolving, 2 wired
  to NPC arcs, 2 wired to journal voice, integrity + tests green.

## Follow-on
- Plan 52 (recurring NPC arcs) — questline survivors recur.
- Plan 95 (journal voice) — quest events trigger journal entries.
- Plan 88 (confession secrets) — questline survivors may confess.
- Plan 65 (final wishes) — questline survivors may have final wishes.
- Plan 66 (guilt sources) — questline choices generate guilt.
