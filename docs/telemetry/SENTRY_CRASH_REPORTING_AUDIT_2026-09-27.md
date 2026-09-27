# Sentry Crash-Reporting Audit — 2026-09-27

> **STATUS: AUDIT COMPLETE — premise of enhancement-plan Task 4 DISPROVEN; no crash-reporting configuration exists to verify.**

Enhancement-plan Task 4 (Repository Enhancement Plan, `Repo_enhancement_plans/`) asked to
audit "Sentry crash-reporting configuration" on the premise that 51 files reference the
Sentry SDK and that references deep inside `Assets/Ashfall.Core/` suggest architecture
bleed. This audit re-verified every premise at current HEAD (read-only, evidence below).

## Verdict

1. **The Sentry.io SDK is never used.** Zero `SentrySdk.Init`, zero `CaptureException`,
   zero `AddBreadcrumb`, zero `ConfigureScope`, zero `using Sentry` anywhere in `src/`,
   `Assets/Ashfall.Core/`, or `Ashfall.Core.Tests/`. There is no crash pipeline at all —
   no global exception handler, no `AppDomain.UnhandledException` hook, no Godot crash
   hook feeding any reporter.
2. **Every `Sentry` match in C# source is a local gameplay symbol** meaning a guard
   sentry, not the SDK. Core examples (all verified lines):
   - `Assets/Ashfall.Core/AirlockSecuritySystem.cs:61` — `public void AssignSentry(string dwellerId)`
   - `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:272` — `const float CampSentryDetectionBonus = 0.3f` (camp sentry reduces encounter chance; `:854` `bool hasSentry`)
   - `Assets/Ashfall.Core/Narrative/NightWatchCatalog.cs:66` — `GetBySentry(string sentryId)`
   - `Assets/Ashfall.Core/WildlifeTrappingSystem.cs:574` — display string `"Three-Eyed Sentry Crow"`
   - `Assets/Ashfall.Core/Shelter/TrophySystem.cs:354` — display string `"Three-Eyed Sentry Crow Feathers"`
   - `Assets/Ashfall.Core/Narrative/BunkerGraffitiProjection.cs:89` — comment `"Airlock Hatch & Sentry Cupola"`
   Host examples: `src/Host/AirlockSecurityHostSession.cs:32` (`AssignSentry` wrapper),
   `src/Host/ExpeditionHostSession.cs:1483` (`hasSentry` passthrough),
   `src/UI/ExpeditionCampPanel.cs:213-215` (label `"Sentry: Active/None"`),
   `src/UI/AirlockSecurityPanel.cs:53` (panel title). The remaining ~82 matches are
   plan-prose text and lore strings in `scripts/`, `tools/`, and JSON catalogs.
3. **No architecture bleed.** `Assets/Ashfall.Core/` has no csproj of its own; Core
   sources compile via the root shim `Ashfall.Core/Ashfall.Core.csproj` (tests) and via
   `<Compile Include="Assets/Ashfall.Core/**/*.cs">` in the host `Ashfall.csproj:42`.
   Neither Core path references the Sentry package, and no Core file uses any SDK
   symbol. The engine-agnostic contract holds. **Task 4.1.2 (route Core call sites
   through `ILog`) has no candidates — negative finding.**
4. **No configuration exists to verify.** No DSN anywhere (zero DSN-shaped strings,
   zero `SENTRY_DSN`/`Sentry__` occurrences in code, config, scripts). No Environment,
   Release, Debug, or BeforeSend/PII-scrubbing settings. Nothing to compare against
   `project.godot:15` `config/version="1.1.0"` (if initialization is ever added, that
   line is the canonical Release source).
5. **The one real defect: a dangling package reference.** `Sentry` 6.9.0 is pinned
   (`Directory.Packages.props:16`) and referenced (`Ashfall.csproj:50`
   `<PackageReference Include="Sentry" />`), restored into every host build including
   exports, yet never activated. `scripts/ci/export-build.sh` and `export_presets.cfg`
   contain no Sentry gating or trim configuration. The repo's own history notes this:
   `scripts/ci/git-object-inventory.sh:353` ("Sentry not used"), `sources.md:242`.

## Existing tests

All 14 Sentry-named test files exercise local gameplay sentry mechanics (camp-sentry
encounter reduction, `AssignSentry`, `GetBySentry`). Zero tests exercise the SDK; zero
crash-reporting tests exist. Task 4.3's "confirm the Sentry hook receives it" has no
hook to confirm — none has been built.

## Disposition (recorded, not silently executed)

- **Preferred:** remove the dangling reference (`Ashfall.csproj:50` +
  `Directory.Packages.props:16`) — zero behavior change, honest dependency surface.
  **Blocked right now:** `Ashfall.csproj` is claimed and dirty by the active
  performance-lane build builder (`WORKTREE_OWNERSHIP.md`, 2026-09-27 claim); editing
  it would race that lane. Recorded for execution when that claim closes.
- **Alternative (feature, needs explicit user approval):** actually wire a crash
  pipeline — a thin `src/Host/` adapter initializing the SDK from an environment DSN
  (never a literal), Release from `project.godot` `config/version`, disabled when the
  DSN is absent, and excluded or gated in export builds. This is new player-facing
  telemetry infrastructure, which the enhancement plan's negative scope (Task 4 must
  produce zero gameplay behavior change; behavior-altering discoveries become findings
  for approval) forbids landing as part of an audit.

## Secret scan

No DSN/credential values found in code or config. A root `.env` file exists locally
(never tracked; see the audit command log) — tracked-status re-verified separately in
the task record for this wave.

## Evidence commands (reproducible)

```
rg -c --no-heading 'Sentry' src/ Assets/Ashfall.Core/ Ashfall.Core.Tests/ scripts/ tools/
rg -n 'SentrySdk|SentryOptions|using Sentry|CaptureException|AddBreadcrumb|ConfigureScope|UnhandledException' --glob '!*.md' --glob '!**/bin/**' --glob '!**/obj/**' --glob '!artifacts/**'
rg -n 'PackageReference Include="Sentry"' Ashfall.csproj
rg -n 'Sentry' Directory.Packages.props
rg -n 'SENTRY_DSN|Sentry__' . --glob '!*.md' --glob '!**/bin/**' --glob '!**/obj/**'
```

Audit executed read-only by the 2026-09-27 Task 4 sweep agent; findings re-verified by
the implementing agent before this record was committed. No production code was
changed by Task 4.
