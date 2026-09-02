# ACCEPTANCE — Content Reauthoring Pass (2026-09-02)

Slice: broad prose reauthor of player-facing content (edit/delete only, no structural rewrite).
Audited via `ashfall-narrative-check` workflow: reachability/mechanics first, tone/content rules second.

## Slice map

| File | Scope reauthored | Texts |
|---|---|---|
| `survivors.json` | all 129 bios (were one shared template) | 129 |
| `quests_massive_expansion_200.json` | 200 quests: briefing + stage + 2 choices each | 800 |
| `quests_faction_branching.json` | 200 quests: stage + 2 choices each (briefings kept) | 600 |
| `events.json` | 36 bodies: friction/ration/escalation cluster, miller/wireman milestones, belief + grief scenes | 36 |
| `trade_texts.json` | 4 trader voices (profiles + all attitude lines), 7 market sections | ~190 |
| `trade_specialties.json` | 16 mastery texts | 16 |
| `wasteland_grave_epitaphs.json` | 8 epitaphs (official line + carved addendum) | 8 |

Inspected and intentionally untouched (already pass): `final_wishes.json`, `echoes.json`
(clinical "machine-eye" entries are an intentional contrast device), `micro_locations.json`,
`moral_choice_quest_stubs.json`, `dose_quests.json`, `faction_war_events.json`.

## Mechanical / reachability findings

- `quests_massive_expansion_200.json`: 20 chains × 10 stages; every `prereq_quest_id` ladder
  intact; every choice sets a flag; no dangling refs; **0 duplicate texts (was 595)**.
- `quests_faction_branching.json`: 20 chains × 10 stages; ladders intact; all choice flags
  present; no stray `[item]` tokens in quest prose; **0 duplicates (was 552)**.
- Chain-level abort flags (`flag_<chain>_aborted` set at any stage) are pre-existing design:
  per-stage `flag_<chain>_NN_advanced` records depth, the single abort flag records the chain
  ending. Not a defect.
- `events.json`: 220 bodies, 0 duplicates; no choice lists added or removed (106 notification
  events have no choices by design; none of the 36 edited events lost a choice).
- `survivors.json`: 129 unique bios; no ID/field changes; bios seeded from each survivor's
  existing `activeQuestlineId`/`latentExpertTrait`/`traitIds` (no contradictions introduced).
- `trade_texts.json`: `[item]` offer-template tokens preserved exactly; all attitude keys intact.

**BLOCKING: 0.**

## Tone / content-rule scan

Scanned all 2,165 reauthored or adjacent texts against the data-authority rules
(no real countries/wars/people; no magic/fantasy; no glorified violence; restrained tone):

- Real-world country/alliance/person regex (superset of `DataRuleComplianceTests`): **0 hits.**
- Fantasy/magic markers: **0 hits.**
- Glorification markers: 1 hit — `events.json:coming_of_age_first_watch` (pre-existing, not
  reauthored in this pass): "The watch was not heroic. The watch was the watch." — this is
  anti-glorification, compliant. **STYLE / no action.**
- `DataRuleComplianceTests` and the full suite executed after the edits (see below).

**CONTENT_DECISION items needing owner input: 0.**

## Verification gates (post-edit)

| Gate | Result |
|---|---|
| JSON validity, all edited files | PASS |
| `dotnet test Ashfall.Core.Tests` | PASS — 6450/6450, 0 failed |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `godot --headless -- --data-integrity-selftest` | PASS — 0 errors across 204 catalogs |
