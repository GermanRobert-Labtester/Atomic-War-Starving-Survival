# UI A11Y §2B/§2C REMAINING ACCENT TOKEN SWEEP — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue doing more UI correction and UI
precision work!" (2026-09-29). Eighth package in the audit-fix series:
finishes the hardcoded-accent consolidation in the seven files the audit
§2b/§2c tables still listed (contrast already verified ≥6:1 for every
mapped token on Ink).

## Bounded outcome

29 hardcoded accent literals in 7 files replaced with canonical tokens:

| Hand-rolled literal | Token | Rationale |
|---|---|---|
| (1,0.3,0.3) (1,0.4,0.4) (1,0.45,0.4) (1,0.5,0.5) (0.95,0.5,0.5) (0.95,0.5,0.4) | Critical | danger/alarm text and rows (6.19:1) |
| (1,0.6,0.2) (1,0.85,0.3) (0.95,0.85,0.4) (0.9,0.7,0.4) | Warning | warning/severe/pending (5.99:1) |
| (0.53,1,0.67) (0.4,0.9,0.5) | Success | success/delivered/valid (10.62:1) |
| (0.4,0.85,0.95) (0.5,0.8,0.9) | Info | info/valid-will/open-capsule (7.01:1) |
| (0.9,0.9,0.9) (0.85,0.85,0.85) | Pale | primary-ish text |
| (0.7,0.7,0.7) (0.6,0.6,0.6) | Dim | empty-state/muted text (the 0.6 grey predates the documented Dim contrast fix) |
| ShelterPanel (0.83,0.67,0.38) (0.43,0.64,0.66) (0.58,0.56,0.52) | Warm / Info / Muted | MakeDataRow re-derived palette |

Files: EmergencyResponseHud (severity header modulate + metric/roster/log
accents — tinted crisis *backdrop* at :125 stays), SaveLoadPanel,
SurvivorDeathLegacyPanel, TimeCapsulePanel, RelationshipDecayPanel,
PersonalQuestPanel, ShelterPanel (MakeDataRow palette). Six files gain
`using DesignTheme = Ashfall.Core.UI.Theme;` (ShelterPanel already has it).

## Exact files

The 7 files above, the gate
`Ashfall.Core.Tests/UI/UiA11yAccentTokenGateTests.cs`, governance (this plan,
`WORKTREE_OWNERSHIP.md`, `.ai/state.md`).

## MUST NOT

- Touch EmergencyResponseHud's tinted backdrop (0.12,0.02,0.02) or normal
  backdrop (now PanelScrim); touch any other file; change layout/sizing;
  alter Core.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Gate test: none of the 18 audited literals remain in the 7 files;
   alias present in each.
3. `--ui-layout-selftest` + `--player-panels-uitest` headless.
