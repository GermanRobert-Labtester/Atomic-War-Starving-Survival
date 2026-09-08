# Plan 102 Closeout Report — Foundry Accords Expansion

**Status:** Complete for the pure-data expansion.
**Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Runtime scope:** catalog lookup, reference validation, and presentation smoke coverage only.

## Summary

The live authority already contained 12 regional treaty records: four
Foundry-signatory District 8 accords and eight unrelated regional accords. The
six Plan 102 additions are explicit Foundry-signatory records, bringing the
Foundry subset to exactly 10 and the file total to 18. No existing record was
renamed, edited, deleted, or reordered.

## Baseline

| Treaty ID | Day | Title | Foundry signatory |
|---|---:|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | 280 | The Brine Pipe & Iodine Exchange | Yes |
| `treaty_cluster_labour_schedule` | 305 | The Cluster Labour Schedule | Yes |
| `treaty_road_iron_charter` | 330 | The Road Iron Charter | Yes |
| `treaty_the_cluster_charter` | 365 | The Cluster Charter | Yes |

The eight pre-existing non-Foundry records remain in the catalog unchanged.

## Schema and Runtime Semantics

`RegionalTreatiesFile` contains `schema_version`, `collection_id`, and a
`treaties` array. Each `RegionalTreatyEntry` contains the authored identity,
ratification day, explicit signatory IDs, territory, water/power numbers,
tariff, articles, penalties, tags, and an optional `term_days` DTO field.

`RegionalTreatyCatalog` loads and indexes static definitions. It does not parse
legal prose into consequences. `water_allocation_lpm` is liters per minute and
`power_quota_kw` is kilowatts; zero is valid and is used for timing, training,
and recordkeeping accords. `ratified_day` is a campaign day in the existing
1–365 timeline. The live consequence catalog remains the owner of typed
mechanical effects.

## Final Foundry Treaty Roster

| ID | Day | Function | Signatories | Water | Power | Policy status |
|---|---:|---|---|---:|---:|---|
| `treaty_brine_pipe_and_iodine_exchange` | 280 | Brine infrastructure / iodine exchange | Foundry, Office | 40.0 | 12.0 | Live baseline |
| `treaty_saltworks_access` | 285 | Measured saltworks access | Foundry, Office, Scale | 30.0 | 0.0 | Dependency-ready |
| `treaty_membrane_repair` | 300 | Membrane technical repair | Foundry, Office | 12.0 | 18.0 | Dependency-ready |
| `treaty_cluster_labour_schedule` | 305 | Shift limits and boil order | Foundry, Office, Cutters | 25.0 | 8.0 | Live baseline |
| `treaty_coal_window` | 320 | Safe ice-road coal timing | Foundry, Cutters | 0.0 | 0.0 | Dependency-ready |
| `treaty_road_iron_charter` | 330 | Road iron and haulage | Foundry, Cutters, Fleet | 15.0 | 6.0 | Live baseline |
| `treaty_apprentice_exchange` | 345 | Supervised casting/salvage training | Foundry, Office, Scavenger Guild | 0.0 | 0.0 | Dependency-ready |
| `treaty_crisis_mutual_aid` | 350 | Bounded emergency response | Foundry, Office, Cutters, Fleet, Archivists, Grain Exchange, Hydro Barons, Scavenger Guild | 20.0* | 10.0* | Dependency-ready |
| `treaty_the_incident_book` | 355 | Incident reporting and amendment control | Foundry, Office, Archivists | 0.0 | 0.0 | Dependency-ready |
| `treaty_the_cluster_charter` | 365 | Civic recognition / charter signature | Foundry, Office, Cutters, Fleet | 0.0 | 0.0 | Intentional exemption |

\* Crisis values are the Foundry's bounded three-day commitment, not a share
guaranteed to every signatory.

## Faction Authority

All IDs resolve against existing authority: `faction_silent_foundry` and its
relationship directory in `foundry_faction.json`; Office, Cutters, and Fleet
in `holdfast_factions.json`; Archivists, Grain Exchange, Hydro Barons, and
Scavenger Guild in `currents.json` plus the Foundry relationship directory; and
The Scale in `crossing_factions.json`. `Cluster` is used only as an existing
regional/institutional name, never as an invented faction ID. The crisis pact
uses eight explicit IDs and no wildcard.

## Territory, Tags, and Legal Tone

Every new demarcation reuses established Foundry, saltworks, membrane hall,
ice-road, Cut, weigh-hut, Cluster school, incident-book, register-hall, or
Archivist-copy terminology. The 18 records use 52 lowercase tags. New tags are
limited to `access`, `maintenance`, `logistics`, `training`, `inspection`,
`emergency`, `security`, `records`, and `accountability`.

Articles use the existing single-string `ARTICLE 1:` / `ARTICLE 2:` /
`ARTICLE 3:` convention. Penalties are authored, proportional terms—access
suspension, priority loss, replacement obligations, inspection, or tariff
restoration—not executable logic.

## Plan 103 and Cross-Plan Hooks

The current `foundry_treaty_consequences.json` remains unchanged with 15 live
policy rows covering its existing eight treaty IDs. The six new IDs are
dependency-ready and intentionally have no dangling policy references. Future
typed bindings may connect them to standing, access, tariff, inspection,
dialogue, witness, or epilogue systems; Plan 102 does not add those systems.

The Incident Book provides a stable continuity hook for witness/document work,
and the Apprentice Exchange provides authored training terms without creating a
new education or labor runtime. No static treaty prose is added to saves.

## Verification

Baseline before edits:

- `godot --headless --path . -- --data-integrity-selftest`: 298/298 catalogs, 0 errors.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: 9,784/9,784 passed.
- `dotnet build Ashfall.csproj`: 0 warnings, 0 errors.
- `godot --headless --path . -- --silent-foundry-selftest`: 26/26 passed.

Post-change:

- `jq empty Assets/StreamingAssets/Data/foundry_accords.json`: pass.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FoundryAccordExpansionTests --no-restore`: 11/11 passed.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SilentFoundrySystemTests --no-restore`: 35/35 passed.
- `dotnet build Ashfall.csproj --no-restore`: 0 warnings, 0 errors.
- `godot --headless --path . -- --silent-foundry-selftest`: 27/27 passed.
- `godot --headless --path . -- --data-integrity-selftest`: 298/298 catalogs,
  0 errors, 0 warnings.
- `godot --headless --path . -- --content-utilization-selftest`: CI gate PASS;
  581 catalogs scanned, 0 orphaned.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: 9,806/9,806
  passed.
- `dotnet build Ashfall.csproj`: 0 warnings, 0 errors.
- `godot --headless --path . -- --real-campaign-journey-selftest`: PASS
  (the existing Godot shutdown emitted resource-leak diagnostics after the
  self-test result).
- Scoped `git diff --check` for Plan 102 files: pass.
- `python3 scripts/ci/run-gates.py --tier fast`: blocked at the first gate by
  a pre-existing trailing-whitespace finding in
  `docs/narrative/PLAN_89_MUSTER_EPILOGUES_EXPANSION_CLOSEOUT.md`; that
  unrelated file was not modified.

The Plan 102-scoped regression is complete. The repository-wide fast tier
requires separate cleanup of the unrelated existing whitespace finding.

## Deferred

- Typed consequence policies for the six new agreements.
- Dialogue, witness, standing, and epilogue consumers that may reference the
  stable treaty IDs.
- Any live resource application beyond the existing informational quota fields.
