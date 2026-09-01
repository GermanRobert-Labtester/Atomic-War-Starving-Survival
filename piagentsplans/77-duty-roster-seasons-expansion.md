# Plan 77 — Duty Roster Seasons Expansion (1 → 8 seasons)

## Goal (2 lines)
Expand `duty_roster_seasons.json` from 1 verified season to 8. The duty roster
season system defines time-windowed encounter weight and steam-trip chance
boosts that make the shelter's rhythm shift across the campaign. 1 season means
the roster never changes — the shelter feels static.

## Why (P2)
- Verified: `duty_roster_seasons.json` has 1 entry (id, windowMinDays,
  windowMaxDays, encounterWeight, steamTripChanceBoost).
  `DutyRosterCatalog.cs` is confirmed in Core. The duty roster system (existing
  12B) is fully wired but only one season window exists.
- Creates the temporal-rhythm pillar: seasons make the shelter's workload shift
  across the campaign — early confusion, mid-campaign consolidation, late-game
  siege. Without multiple seasons, every day feels the same.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/duty_roster_seasons.json` (expand 1 → 8 seasons)
- Read-only: `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs` (confirm schema
  and how seasons are selected by day range)

## Content grammar (per season)
- snake_case `id` with prefix `season_` (confirmed prefix).
- Day window: windowMinDays / windowMaxDays — non-overlapping ranges spanning
  the campaign (0–365+). Each season is a campaign phase.
- encounterWeight: 0.5–2.5 — how likely shelter-internal encounters fire during
  this window (early: high confusion; mid: moderate; late: variable by threat).
- steamTripChanceBoost: 0.0–0.15 — bonus to steam-trip (expedition) chance during
  this window (some seasons encourage expeditions, others discourage).
- Grounded tone: each season reflects a campaign phase (first winter, spring
  thaw, faction consolidation, siege, second winter, etc.).

## Steps
1. Read `DutyRosterCatalog.cs` to confirm the season schema and how seasons are
   selected (by current day falling within windowMinDays/windowMaxDays).
2. Confirm the existing season (`season_second_winter`, days 8–12) and design
   7 new seasons around it, covering the full campaign arc.
3. Author 7 new seasons:
   - `season_first_ashfall` (days 0–7): high encounterWeight (confusion), low
     steamTripChanceBoost (nobody goes out yet).
   - `season_settling` (days 13–30): moderate encounterWeight, rising
     steamTripChanceBoost (first organized expeditions).
   - `season_spring_thaw` (days 31–60): lower encounterWeight, high
     steamTripChanceBoost (weather opens up, expeditions peak).
   - `season_faction_pressure` (days 61–120): rising encounterWeight (faction
     patrols, raids), moderate steamTripChanceBoost.
   - `season_first_siege` (days 121–180): high encounterWeight, low
     steamTripChanceBoost (shelter under pressure, fewer expeditions).
   - `season_consolidation` (days 181–240): moderate encounterWeight, rising
     steamTripChanceBoost (recovery and organized expeditions).
   - `season_long_winter` (days 241–365): high encounterWeight, low
     steamTripChanceBoost (deep winter, survival focus).
4. Cross-reference: all season ids unique; day windows non-overlapping and
   gap-free (every day 0–365 falls within exactly one season, or confirm the
   system handles gaps gracefully).
5. Validate: `--data-integrity-selftest`; run a headless day-advance test to
   confirm seasons activate at the correct day.
6. xUnit: duty roster catalog loads 8 seasons, all ids unique, day windows
   non-overlapping, encounterWeight and steamTripChanceBoost within valid ranges.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is day-window gaps (step 4): confirm the system
handles days outside any season window before assuming gap-free coverage.

## Definition of Done
- `duty_roster_seasons.json` has 8 seasons, all ids unique, day windows
  non-overlapping, integrity + tests green.

## Follow-on
- Plan 70 (shelter schedules) — seasons gate which schedules are available.
- Plan 57 (incidents) — seasons modulate incident frequency.
- Plan 48 (weather gates) — seasons align with weather season windows.
- Plan 74 (campaign chapters) — seasons map to chapter transitions.
- Existing 12B (duty roster) — this plan provides the seasonal data it lacked.
