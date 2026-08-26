# Snapshot Golden Regeneration — Approval Request (2026-08-26)

**Pipeline:** implemented in `961df334` (`--ui-snapshot-uitest` diff gate + `--ui-snapshot-regenerate`)
**State:** 29 goldens regenerated in the working tree, **awaiting approval before commit**
**Post-regen gate:** `UI_SNAPSHOT_UITEST PASS — 29 targets, 29 match, 0 drift, 0 fail` (exit 0)

## Why every golden drifted

The Aug-18 baseline (`0fb0c340`) predates the entire batch-3+ UI wiring effort: every one of the
29 target panels has 1–5 commits of changes since — placeholder panels bound to live host sessions,
empty-state rewrites, status rails, the ShelterPanel 2D-layout anchor, and dashboard nav work.
The goldens captured the *placeholder-era* rendering; the panels now render live-state-aware UI.

## Validation performed (before regenerating)

| Check | Result |
|---|---|
| Environment fidelity | Near-zero mean RGB shift (+0.2/0.1/0.1) on low-drift panels — renderer matches the original capture environment (Forward+, DISPLAY=:0, same GPU class per `docs/ui/snapshot_manifest.json`) |
| Run-to-run determinism | Two consecutive diff runs produced **29/29 byte-identical captures** — the gate is stable, regenerated goldens are reproducible |
| Drift localization | Spatial analysis: low-drift panels show scattered text-region diffs (content/empty-state changes), high-drift panels show blocky region changes matching their known redesigns (ShelterPanel 2D anchor) |
| Headless guard | `--headless` run reports 29/29 FAIL "renderer unavailable" with display hint — no blank goldens can be written by CI mistake |
| Post-regen gate | Diff gate re-run: **29/29 MATCH, exit 0** |

## Per-panel drift at regeneration (sorted by severity)

| Target | Pixels drifted | Classification |
|---|---|---|
| journal_default | 51.68% | CONTENT — JournalPanel restructure (5 commits) |
| shelter_default | 41.13% | LAYOUT — ShelterPanel 2D-layout anchor redesign (4 commits) |
| caravan_barter_default | 33.93% | CONTENT — live bind rework |
| shelter_hud_default | 30.53% | CONTENT — pre-existing drift from 0fb0c340 (panel modified same day after capture) |
| standing_record_atlas_default | 25.54% | CONTENT — atlas rework |
| muster_atlas_default | 25.21% | CONTENT — atlas rework |
| research_atlas_default | 25.02% | CONTENT — sidebar/atlas rework |
| survival_workstation_default | 20.67% | CONTENT — live bind rework |
| radio_default | 19.46% | CONTENT — LastIntercept fix + rework |
| skill_matrix_default | 16.37% | CONTENT — live bind rework |
| quests_atlas_default | 15.30% | CONTENT — atlas rework |
| expedition_radar_default | 15.20% | CONTENT — radar rework |
| maritime_atlas_default | 14.80% | CONTENT — atlas rework |
| combat_hud_default | 14.35% | CONTENT — HUD rework |
| greenhouse_default | 14.20% | CONTENT — live bind rework |
| trade_default | 13.08% | CONTENT — trade screen rework |
| factions_narrative_default | 12.42% | CONTENT — narrative shell rework |
| silent_foundry_default | 12.30% | CONTENT — foundry rework |
| duty_roster_default | 12.26% | CONTENT — duty roster rework |
| map_atlas_default | 12.19% | CONTENT — atlas rework |
| faction_matrix_default | 9.99% | CONTENT — matrix rework |
| weather_default | 8.32% | CONTENT — live bind rework |
| dose_ledger_default | 8.14% | CONTENT — live bind rework |
| inventory_default | 7.63% | CONTENT — live bind rework |
| weather_dashboard_default | 7.49% | CONTENT — same panel as weather_default |
| medical_default | 6.39% | CONTENT — live bind rework (4 commits) |
| survivors_default | 5.60% | CONTENT — live bind rework |
| verdict_dashboard_default | 5.16% | CONTENT — header-region change (localized) |
| verdict_default | 3.61% | TEXT/AA — minor text + antialiasing |

All drift is attributable to intentional UI evolution committed since the baseline; no
unexplained visual regressions were found. Diff captures for side-by-side inspection are in
`snapshot-capture/` (gitignored scratch) until overwritten by the next run.

## Changeset awaiting approval (NOT committed)

- `snapshots/*.png` — 29 regenerated goldens (Git LFS)
- `docs/ui/snapshot_manifest.json` — baseline_bytes resync + regeneration record

## After approval

```
git add snapshots/ docs/ui/snapshot_manifest.json docs/ui/SNAPSHOT_REGEN_APPROVAL_2026-08-26.md
git commit -m "chore(snapshot): approve + commit regenerated goldens (29 targets, post batch-3+ UI wiring)"
```

## Recurring use

```
# gate (drift check — exit 1 on drift/fail):
godot --path . --rendering-method forward_plus -- --ui-snapshot-uitest

# approved golden update:
godot --path . --rendering-method forward_plus -- --ui-snapshot-regenerate
```

Both require a real display (not `--headless`) — documented in the gate's own failure output.
