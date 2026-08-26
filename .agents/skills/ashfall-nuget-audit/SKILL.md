---
name: ashfall-nuget-audit
description: Audits NuGet dependencies, target-framework alignment (netstandard2.1 vs net8.0 vs net9.0), outdated/vulnerable packages, and supply-chain hygiene. Use when bumping packages, changing targets, or monthly.
---

# ASHFALL NuGet Audit

## ROLE
Three targets must agree: `Assets/Ashfall.Core/` (`netstandard2.1`), `Ashfall.csproj` Godot host (`net8.0`), `Ashfall.Core.Tests` (`net9.0`). You prevent vulnerable transitives, target drift, and stale pins.

## RULES
1. Core has 0 engine refs — audit must not propose `GodotSharp`/`UnityEngine` into Core.
2. Use `dotnet` CLI only — `dotnet list package --outdated --include-transitive`, `dotnet list package --vulnerable`.
3. Read-only audit; never `dotnet add package` without explicit user approval.

## WORKFLOW
### PHASE 1 — Inventory
- `dotnet list Ashfall.Core/Ashfall.Core.csproj package` / `Ashfall.csproj` / `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj package --include-transitive`
- Record TFMs from `*.csproj` `<TargetFramework>`; check `<LangVersion>`, `netstandard2.1` compatibility of Core deps.

### PHASE 2 — Vulnerability & Freshness
- `dotnet list package --vulnerable` → CVE severity table.
- `dotnet list package --outdated` → major/minor drift; flag major bumps needing API review.
- `dotnet nuget verify` where locally available; check `nuget.config` feeds.

### PHASE 3 — Framework Alignment
- Dep that requires `net8.0` referenced by `netstandard2.1` Core = blocker.
- Test `net9.0` ahead of host `net8.0` — note feature-use risk (e.g., new BCL APIs not in host).
- Duplicate package with different versions across projects → consolidate.

### PHASE 4 — Verify
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` + `dotnet build Ashfall.csproj` 0 warnings.
- `dotnet test` green if versions changed (proposed only; not applied in audit mode).

## OUTPUT
`docs/deps/NUGET_AUDIT.md` — per-project table: package | current | latest | vulnerable? | TFM compatible? | action (patch/minor/major/pin) + consolidated version proposal.

## QUALITY GATE
- 0 vulnerable packages, 0 TFM-incompatible deps, no version divergence for same package across projects (or explicit justification).
