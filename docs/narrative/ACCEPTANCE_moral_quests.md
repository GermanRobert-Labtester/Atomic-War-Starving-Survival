# Narrative Acceptance Check — Moral-Choice Quests Expansion

**Slice:** `moral_choice_quests_expansion.json` — 50 morale-choice quests with 200 moral/empathy-traded choices, from commit `40816057`.

**Method:** `ashfall-narrative-check` — branch/reachability, mechanical text/data alignment, tone, fictional-world compliance.

---

## Slice Map

| Field | Value |
|---|---|
| Schema | mirrors `moral_choice_quests.json` (exact field match, both quest and choice level) |
| Quests | 50 |
| Choices | 200 (4 per quest, all quests) |
| Categories | comfort (10), dead (10), listen (9), share (11), trust (10) |
| id prefix | `quest_moral_*` (self-registering DefinitionKey) |
| location_id | empty (matching existing convention) |
| min_day range | 5–40 |
| max_day range | 100–200 |
| moral_delta range | -10 to +10 |
| empathy_delta range | -3 to +3 |

---

## Branch / Reachability Findings

**No blocking findings.** Every quest has exactly 4 choices (the existing file uses 3–4; all mine use 4, which is within the valid range). Every choice is a valid terminal — each has a distinct `label`, `outcome_text`, and `epitaph`, with integer `moral_delta` and `empathy_delta`. There are no dead ends, impossible conditions, or duplicate choices.

### 12-point structural validation (all PASS)

| # | Check | Result |
|---|---|---|
| 1 | Duplicate quest ids | NONE (PASS) |
| 2 | Choice count 3–4 per quest | ALL PASS (all 4) |
| 3 | min_day ≤ max_day | ALL PASS |
| 4 | Moral/empathy deltas are integers, reasonable range | ALL PASS (moral -10..10, empathy -3..3) |
| 5 | Choice field completeness (label, moral_delta, empathy_delta, outcome_text, epitaph) | ALL PASS |
| 6 | Quest field completeness (all 9 required fields) | ALL PASS |
| 7 | No empty required string fields | ALL PASS |
| 8 | location_id empty (existing-file convention) | ALL PASS |
| 9 | No duplicate choice labels within a quest | ALL PASS |
| 10 | Category distribution | 5 categories, 9–11 each (balanced) |
| 11 | Moral delta spread (not all same within a quest) | ALL PASS |
| 12 | id prefix is `quest_` | ALL PASS |

---

## Mechanical Text/Data Alignment

### Schema parity (PASS)

Exact field match with the shipped `moral_choice_quests.json` at both the quest level (`id`/`display_name`/`category`/`trigger`/`discovery`/`location_id`/`min_day`/`max_day`/`choices`) and the choice level (`label`/`moral_delta`/`empathy_delta`/`outcome_text`/`epitaph`). No extra or missing fields.

### ID collisions (PASS)

Zero id collisions with the 60 existing shipped quests. The 50 new `quest_moral_*` ids are all unique within the file and across the existing file.

### Prose game-ID references (PASS)

All prose fields (`trigger`, `discovery`, `label`, `outcome_text`, `epitaph`) are bare — no `item_`/`loc_`/`faction_`/`survivor_`/etc. references that would need to resolve against the catalog. The quests are self-contained moral dilemmas with no mechanical dependencies on other game systems.

### Canonical integrity (PASS)

`godot --headless -- --data-integrity-selftest` → **PASS — 0 errors, 0 warnings across 115 catalogs.** The 50 new `quest_moral_*` ids self-register cleanly (4001 authored ids, +50 from the prior 3951).

---

## Tone & Fictional-World Compliance

### Tone (PASS)

- **Genuine moral tradeoffs:** every quest has 4 choices with no obviously-correct option. The moral_delta range (-10 to +10) and empathy_delta range (-3 to +3) create real tradeoffs — e.g. the dying stranger: sit until the end (+10/+3), sit a while then leave (+3/+1), leave food and go (+1/0), walk on (-8/-1).
- **No combat-default solutions:** the strongest quests are comfort (sitting with the dying, the despairing, the broken) and dead (the unburied, the ceremony for a stranger's faith). No quest resolves to "fight."
- **Restrained voice:** the recurring "the thing is the X / the X is the thing" figure carries moral weight without melodrama. The epitaphs are brief, memorable, and earned.
- **Emotional range:** share (generosity vs. scarcity), trust (belief vs. caution), comfort (presence vs. abandonment), dead (duty vs. avoidance), listen (attention vs. refusal).

### Fictional-world compliance (PASS)

- **No real countries/wars/people:** zero hits across all 50 quests.
- **No supernatural confirmation:** the cultist's prophecy (quest_moral_listen_prophecy) is explicitly "could be madness, could be true" — the player listens but is never asked to confirm the supernatural.
- **No magic/fantasy:** no spells, ghosts, demons, chosen-ones.
- **No glorified violence / gore spectacle:** the burned survivor and the remorseful raider are restrained; the horror is in the moral weight, not the viscera.
- **No copied IP.**

---

## Findings Summary

| # | Finding | Severity | Status |
|---|---|---|---|
| 1 | 12-point structural validation | PASS | All 12 checks pass |
| 2 | Schema parity with existing file | PASS | Exact field match |
| 3 | No id collisions with existing quests | PASS | Zero collisions |
| 4 | No prose game-ID references | PASS | All bare |
| 5 | Canonical integrity | PASS | 0/0 across 115 catalogs |
| 6 | Tone — genuine moral tradeoffs, no combat-default | PASS | — |
| 7 | No real countries/wars/people | PASS | Zero hits |
| 8 | No supernatural confirmation | PASS | Cultist prophecy explicitly ambiguous |
| 9 | No magic/fantasy | PASS | — |
| 10 | No glorified violence / gore | PASS | — |

**No BLOCKING findings. No CONTENT_DECISION findings. All checks PASS.**

---

## Quality Gate

- ✅ Every blocking finding includes a reproducible path or exact missing edge — **none** (no blocking findings).
- ✅ Tone judgments identify the relevant rule and quoted context (above).
- ✅ No continuity issue is relabeled as a style preference.

**Conclusion:** The 50 moral-choice quests pass the narrative acceptance check with zero findings. They are structurally sound (50 quests × 4 choices = 200 valid terminals, no duplicates, no impossible ranges), mechanically aligned (exact schema parity, zero id collisions, zero dangling references), and tonally/fictionally compliant (genuine moral tradeoffs, no combat-default, no supernatural confirmation, no real-world references).
