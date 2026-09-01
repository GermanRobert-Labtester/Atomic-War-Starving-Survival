# Plan 94 — Verdict Radio Broadcasts Expansion (13 → 30 machine-register radio broadcasts)

## Goal (2 lines)
Expand `verdict_radio.json` from 13 verified broadcasts to 30. The Verdict
radio system (`VerdictRadioSystem.cs` confirmed live) defines machine-register
broadcasts the player intercepts — telemetry, maintenance logs, census
carrier signals. These are the Verdict expansion's radio layer, separate from
the faction radio corpus (Plan 73). 13 broadcasts is too few for a 300+ day
investigation campaign.

## Why (P2)
- Verified: `verdict_radio.json` has 13 entries (id, frequency, dayTrigger,
  source, message, signalStrength, kind). `VerdictRadioSystem.cs` and
  `VerdictSave.cs` are confirmed live.
- Creates the machine-voice pillar: Verdict radio is the voice of the
  pre-war machine infrastructure — telemetry bursts, maintenance schedules,
  census carrier signals, calibration readings. These broadcasts are how the
  player learns what the machines are doing without anyone telling them. 13
  broadcasts covers ~13 days; 30 covers a sustained investigation arc.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_radio.json` (expand 13 → 30 broadcasts)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` (confirm
  schema and how dayTrigger gates broadcast availability)

## Content grammar (per broadcast)
- snake_case `id` with prefix `radio_verdict_` (confirmed prefix).
- frequency: string ("99.0 MHz" — the machine register's carrier frequency).
- dayTrigger: integer day when the broadcast becomes interceptable.
- source: 1 sentence describing the broadcast's origin ("Census Carrier,
  Machine Registers", "Fuse World, Service Bay").
- message: 1–3 sentences of broadcast text. Match the existing quality —
  terse, machine-like, slightly uncanny. These are automated systems reading
  data, not humans talking. The uncanny part is what the data implies.
- signalStrength: "S1" to "S5" (signal strength — weaker signals are harder
  to intercept).
- kind: broadcast type (telemetry, maintenance, census, calibration,
  anomaly, test, emergency — confirm accepted kinds by reading existing
  entries).
- Day distribution: broadcasts should span the campaign (day 200–365+),
  with increasing frequency as the investigation deepens.

## Steps
1. Read `VerdictRadioSystem.cs` to confirm the schema and how dayTrigger
   gates broadcast availability (does the player intercept broadcasts on or
   after dayTrigger?).
2. Read the existing 13 broadcasts to confirm the quality bar and accepted
   `kind` values (telemetry, maintenance — are there others?).
3. Author 17 new broadcasts spanning days 210–365+:
   - Telemetry (5): meter readings at different times, each reading slightly
     different — the machine is measuring something that changes.
   - Maintenance (4): scheduled service orders, each noting "nothing was
     wrong" — the machine maintains itself on a schedule nobody set.
   - Census (3): census carrier signals, counting something — the count
     changes between broadcasts, but nobody knows what's being counted.
   - Calibration (2): dosimeter calibration readings, noting drift — the
     machine is honest about its own error.
   - Anomaly (2): unexpected readings that don't match any schedule — the
     machine detected something it wasn't looking for.
   - Emergency (1): a single emergency broadcast on day 350+ — the machine
     breaks its own schedule for the first and only time.
4. Each broadcast: distinct id, dayTrigger, source, message, signalStrength,
   kind. Match the existing terse, machine-like tone.
5. Cross-reference: every broadcast id unique; dayTrigger strictly increasing
   (or at least non-decreasing); every kind is an accepted value.
6. Wire 3 broadcasts into Plan 82 Verdict investigation sites (broadcasts
   reference site locations or provide clues that lead to sites).
7. Wire 2 broadcasts into Plan 84 muster witnesses (a broadcast corroborates
   or contradicts a witness's testimony).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: Verdict radio catalog loads 30 broadcasts, all ids unique,
   dayTrigger within valid range, all kinds accepted, all messages non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `kind` values (step 2): confirm accepted
broadcast kinds before authoring.

## Definition of Done
- `verdict_radio.json` has 30 broadcasts, all ids unique, 3 wired to Verdict
  sites, 2 wired to muster witnesses, integrity + tests green.

## Follow-on
- Plan 82 (Verdict locations) — broadcasts reference investigation sites.
- Plan 84 (muster witnesses) — broadcasts corroborate or contradict
  testimony.
- Plan 73 (faction radio) — Verdict radio and faction radio are separate
  systems that complement each other.
- Plan 93 (Verdict NPCs) — NPCs reference radio broadcasts.
- Existing 24 (radio signals) — this plan provides the Verdict radio data.
