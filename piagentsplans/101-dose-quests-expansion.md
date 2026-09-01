# Plan 101 — Dose Quests Expansion (4 → 12 dose-ledger questlines)

## Goal (2 lines)
Expand `dose_quests.json` from 4 verified questlines to 12. The dose quest
system (`DoseQuestMigration.cs` confirmed live) defines multi-stage moral
quests tied to the dose-ledger system — each quest has stages with narrative
prompts, choices, morale/guilt deltas, and item grants. 4 questlines is too
few for a radiation-bureaucracy system that should span the full campaign.

## Why (P2)
- Verified: `dose_quests.json` has 4 questlines (questlineId, title, synopsis,
  factionTag, minDay, maxDay, stages with stageId, title, narrativePrompt,
  isTerminal, choices with choiceId, text, nextStageId, moraleDelta,
  guiltDelta, grantItemId, grantItemQuantity, outcomeNarrative).
  `DoseQuestMigration.cs` is confirmed in Core.
- Creates the dose-quest pillar: dose quests are the radiation-bureaucracy
  narrative layer — each quest forces a moral decision about how the shelter
  tracks and manages radiation exposure. 4 quests cover the first reading,
  the sick room, the child's baseline, and the signed hour; 12 quests would
  cover the full arc from first reading through endgame.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/dose_quests.json` (expand 4 → 12 questlines)
- Read-only: `Assets/Ashfall.Core/DoseQuestMigration.cs` (confirm schema and
  how questlineId, stages, and choices resolve)
- `Assets/StreamingAssets/Data/items.json` (grantItemId must resolve)

## Content grammar (per questline)
- `questlineId`: snake_case with prefix `quest_the_dose_*` or `quest_*`
  (confirmed prefix pattern).
- `title`: 2–5 words evoking the quest's moral weight.
- `synopsis`: 1–2 sentences describing the moral dilemma.
- `factionTag`: "none" or a faction id.
- `minDay` / `maxDay`: day window for quest availability.
- `stages`: array of stage objects, each with:
  - `stageId`: snake_case id unique within the questline.
  - `title`: 1–3 words.
  - `narrativePrompt`: 2–4 sentences of prose setting the scene. Match the
    existing quality — cold, specific, human, bureaucratic.
  - `isTerminal`: boolean (terminal stages have empty choices).
  - `choices`: array of 2–3 choice objects, each with:
    - `choiceId`: snake_case id.
    - `text`: 1 sentence in the player's voice.
    - `nextStageId`: the stage this choice leads to.
    - `moraleDelta`: integer morale change.
    - `guiltDelta`: integer guilt change.
    - `grantItemId` (optional): item id granted by this choice.
    - `grantItemQuantity` (optional): quantity granted.
    - `outcomeNarrative`: 1–3 sentences describing the outcome.
- Moral weight: every quest should force a genuine dilemma — no obvious
  right answer. The player should feel the cost of every choice.

## Steps
1. Read `DoseQuestMigration.cs` to confirm the schema and how questlineId,
   stages, and choices are resolved.
2. Read the existing 4 questlines to confirm the quality bar (the first
   reading, the sick room, the child's baseline, the signed hour — each is
   a cold, specific, morally weighted dilemma).
3. Read `items.json` to confirm which item ids exist for grantItemId.
4. Author 8 new questlines spanning the mid-to-late campaign:
   - `quest_the_dose_the_second_dosimeter` (day 60): a second dosimeter is
     found. Decide who gets it — the sick or the scouts.
   - `quest_the_dose_the_voluntary_register` (day 80): a survivor
     volunteers for high-dose work. Decide whether to accept or refuse.
   - `quest_the_dose_the_calibration_dispute` (day 100): the clockmaker
     and the registrar disagree about a reading. Whose number stands?
   - `quest_the_dose_the_sick_list_grows` (day 120): three survivors are
     Red-band. The morphine tray has two doses. Who is named first?
   - `quest_the_dose_the_cohort_audit` (day 160): the machine requests a
     cohort baseline audit. The numbers will be bad. Book them or refuse?
   - `quest_the_dose_the_black_band` (day 200): a survivor crosses into
     the Black band. They are dying. What do you tell them?
   - `quest_the_dose_the_calibration_honesty` (day 240): the clockmaker
     discovers the dosimeter drift is worse than reported. Correct the
     ledger or maintain the kinder numbers?
   - `quest_the_dose_the_final_count` (day 300): the machine presents its
     count. The count names the shelter's persons. Accept it as read, or
     refuse the count?
5. Each questline: 2–4 stages with 2–3 choices per non-terminal stage.
   Match the existing cold, bureaucratic, morally weighted tone.
6. Cross-reference: every questlineId unique; every stageId unique within
   its questline; every nextStageId resolves to a stage in the same
   questline; every grantItemId resolves in items.json.
7. Wire 2 questlines into Plan 90 (dose registers — quests reference dose
   bands and care plans).
8. Wire 2 questlines into Plan 95 (journal voice — quest outcomes trigger
   journal entries).
9. Validate: `--data-integrity-selftest` (all ids resolve).
10. xUnit: dose quest catalog loads 12 questlines, all ids unique, all
    stage transitions valid (nextStageId resolves), all grantItemId
    resolve, all choices have non-empty text and outcomeNarrative.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is stage transitions (step 6): every
nextStageId must resolve to a stage within the same questline — no
cross-questline transitions.

## Definition of Done
- `dose_quests.json` has 12 questlines, all ids resolving, all stage
  transitions valid, 2 wired to dose registers, 2 wired to journal voice,
  integrity + tests green.

## Follow-on
- Plan 90 (dose registers) — quests reference dose bands and care plans.
- Plan 95 (journal voice) — quest outcomes trigger journal entries.
- Plan 66 (guilt sources) — dose quest choices generate guilt.
- Plan 81 (dose locations) — quests may reference dose locations.
- Plan 79 (autopsy procedures) — terminal-band quests may lead to autopsy.
