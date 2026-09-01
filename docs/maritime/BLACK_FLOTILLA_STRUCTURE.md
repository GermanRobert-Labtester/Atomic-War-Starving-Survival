# Black Flotilla Structure — Fleets, Ranks, Codes (Plan 23 / Task 23A)

Repository truth: the faction model has **no subfaction state** — one faction entry
(`faction_black_flotilla`, holdfast_factions.json), one stance track
(`FactionStanceEngine`), one radio band. The three fleets are therefore **divisions of
one faction**, expressed through NPC roles, radio voice, item niches, and quest content —
not through new standing state.

## The three fleets

### 1. Salvage Fleet — "the mooring"
Practical, material-focused, bargaining culture. Protective of stamped salvage claims.
- Player surface: best trade access at moderate standing (exchange opens at trust ≥ 0,
  claim-tag courtesy at ≥ 30); wreck-rights and cargo disputes; recovery tool quests.
- Radio voice: coordinates, tonnage, claim marks, tonnage runs, "wet or it didn't happen".
- People: Quartermaster **Cass Polder** (exchange deck), Anchormother **Odile Vanter**
  (political coordinator, arbitrates claim disputes).

### Escort Fleet
Convoy protection and route control; disciplined threat assessment; suspicious of
unregistered traffic; capable of blockade when relations sour.
- Player surface: inspection vs. safe-passage reactions scale with standing; interested
  in fuel, weapons serials, route intel; convoy schedules as trade/intel.
- Radio language: challenge-response, convoy order, warnings ("heave to… show ribbon…").
- People: Board Officer **Halloran Vesk** — blockade-minded; argues with Polder about
  taxing claim-holders, with Odile about who owns the water.

### Deep-Dive Fleet
Prestige through depth; high casualty culture; technical expertise; ribbon traditions;
permanent tension between caution (Dive-Chief Hael) and status-seeking.
- Player-facing: gatekeeper for deep-site gear/knowledge (ribbons, line, air shorthand);
  higher trust threshold (cooperation at trust ≥ 55); quests around missing divers,
  contamination recoveries, elite equipment.
- Radio language: depth-in-line, "the grey door stood open, the black door is shut",
  ribbon counts, honest-air short-hands for air and line.
- People: Dive-Chief **Jorin Hael**; struck-off diver **Lotte Verrill** (ex-Deep Fleet,
  blacklisted after the Barrik incident).

## Rank and code culture (4–6 ranks, kept narrative)

| Rank / title | Fleet | Mechanical hook |
|---|---|---|
| Anchormother | political coordinator (fleet-master) | Standing thresholds; treaty/blockade voice |
| Quartermaster | Salvage | Trade surface, claim registry |
| Dive-Chief | Deep | Deep-site access, ribbon awards |
| Codekeeper | Signals (fleet-wide) | Radio ledger, burial records, code teaching |
| Board Officer | Escort | Hail/inspection, blockade |
| Struck-off diver | (none — blacklisted) | Alternative discovery path, risk dives |

Code-ribbon meanings (3): **bar-and-dot** = challenge-response (escort); **black ribbon**
= line lost / diver not coming back; **third ribbon** = a deep mark no one wants to earn
(one per lost crew on a single hull). Claim marks are lead tags punched per season.
No ribbon is a mechanical key except where a quest/NPC want consumes it (23A.4 rule).

## Faction grammar mapping (extracted)

Faction identity fields live in `holdfast_factions.json` (`HoldfastFactionEntry`):
id, display_name, alignment, home_region, is_active, trust, wants[], offers[],
signature_quote, access_rule, badge_asset_id. Fleets are **content classification**
(NPC professions, item roles, radio line pools), NOT subfaction authorities.
Standing thresholds: `BlackFlotillaStanding` (Core, `FactionStanceEngine` semantics).
