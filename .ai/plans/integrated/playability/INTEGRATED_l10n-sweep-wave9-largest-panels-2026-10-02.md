# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# l10n sweep wave 9 — the three largest unregistered panels

> **STATUS: APPROVED BY USER**

User-directed ("work on this small task! Remaining: the broad ~150-file
localization sweep (largest: FactionsPanel, GreenhousePanel, ShelterBarterPanel)").

## Result

| Panel | Raw-chrome literals before | After | Registered |
|---|---:|---:|---|
| `src/UI/FactionsPanel.cs` | 40 | 0 | ✅ `l10n_drift_gate.py` |
| `src/UI/GreenhousePanel.cs` | 36 | 0 | ✅ (concurrent wave) |
| `src/UI/ShelterBarterPanel.cs` | 33 | 0 | ✅ `l10n_drift_gate.py` |

All player-facing chrome (section headers, titles, data-row labels, buttons,
card frames, status lines) now routes through `AshfallUiText.Tr`/`TrFormat`, and
every referenced key has a catalog row with German + placeholder parity.
`RegisteredPanels_HasNoRawChromeLiteral` now enforces all three surfaces.

## Exact files

- `src/UI/FactionsPanel.cs`, `src/UI/GreenhousePanel.cs`, `src/UI/ShelterBarterPanel.cs`
- `assets/l10n/strings.csv` (`ui.factions.*`, `ui.greenhouse.*`, `ui.barter.*` rows)
- `scripts/ci/l10n_drift_gate.py` (register all three panels)
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs` (identical-row allowlist + pin)
- `.ai/state.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, this plan

## Repairs found while integrating (concurrent-agent collisions)

1. **Unquoted comma** in `ui.factions.warlord.tribute_short` (English) split the
   row; quoted it.
2. **Key-prefix collision**: my `ui.factions.standing` dot-prefixed the concurrent
   `ui.factions.standing.*` set; renamed to `ui.factions.standing_trust`.
3. **Compile error** in `ShelterBarterPanel` (`TrFormat` string used as a Node);
   restored the `MakeMetadata(...)` wrapper.
4. **Identical en/de allowlist**: added `ui.greenhouse.filter.title`,
   `ui.factions.standing.neutral`, `ui.barter.gate_line`, `ui.barter.error.fallback`
   and bumped the bounded pin 21→24.

## Evidence

`python3 scripts/ci/l10n_drift_gate.py` PASS (1467 keys, 928 localized-surface references, German parity);
`dotnet build Ashfall.csproj --no-restore -m:1 /p:UseSharedCompilation=false` PASS (0 warnings / 0 errors); `StatusPanelThresholdTests` 211/211 (incl.
`RegisteredPanels_HasNoRawChromeLiteral`, `StringsCsv_IdenticalEnglishGerman_*`,
`StringsCsv_KeyPrefixCollisions_StayBounded`, `LocalizedSurfaces_RegisteredInDriftGate`);
`StringsCsvLocaleGateTests` 4/4; `LocalizationRatchetTests` 3/3; bounded 15-FPS
`--player-panels-uitest` PASS (22/22 lifecycle gates, GreenhousePanel included);
`git diff --check` clean. No full suite.

## Remaining sweep

The rest of the ~150-file sweep continues as later waves; the ratchet keeps every
newly registered panel clean.
