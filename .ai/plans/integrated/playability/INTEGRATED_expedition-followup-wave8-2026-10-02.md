# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Expedition follow-up wave 8 — remaining player-facing l10n (risk/preview/selectors/refusal)

> **STATUS: APPROVED BY USER**

User-directed ("continue with these last remaining and then we are done!"). Source:
the 15 tasks proposed at the end of the wave-7 report.

## The 15 tasks and result

1. Radar offline metadata + no-selection hint → `ui.expedition.radar.offline` / `.select_row_hint`.
2. Encounter risk tags → `ui.expedition.risk.extreme/high/moderate/low`.
3. Choice preview line → `ui.expedition.preview.morale/default/guilt/standing`.
4. `FormatUnavailableReason` codes → `ui.expedition.unavailable.*`.
5. `FormatDispatchRefusal` messages → `ui.expedition.refusal.*` (completed the concurrent agent's code with its catalog rows).
6. `SurfaceCommandRefusal` prefixes → `ui.expedition.refuel_refused` / `.track_gear_refused`.
7. `"DISPATCH REFUSED — {0}"` → `ui.expedition.dispatch_refused`.
8. Vehicle/weapon selectors + entry format + `BROKEN` → `ui.expedition.vehicle_foot/weapon_sidearm/vehicle_entry/vehicle_broken`.
9. Autoplay encounter banner → already done by the concurrent agent (`ui.expedition.banner_encounter`/`.banner_specific`).
10. Fallbacks `UNKNOWN FACTION` / `PATROL` / `[UNNAMED]` → `ui.expedition.unknown_faction` / `.patrol_token` / `.unnamed`.
11. Radar active-detail value fragments → already wired by the concurrent agent (`radar.detail.travel_value`/`.cargo_value`).
12. Pending-list meta `DAY {0}` → `ui.expedition.pending_day` (`ENCOUNTER #{0}` already done).
13. `"No prior contact"` → `ui.expedition.no_prior_contact`.
14. Estimate suffixes → already done by the concurrent agent (`ui.expedition.estimate_*`); removed my redundant `est.*` keys.
15. Pin + source gate + UID → source↔catalog gate `EveryUiExpeditionKeyLiteral_HasACatalogRow` already covers the panels; `.uid` sidecars are gitignored/untracked, so no artifact is required.

## Exact files

- `src/UI/ExpeditionPanel.cs`
- `src/UI/ExpeditionRadarPanel.cs`
- `assets/l10n/strings.csv` (expedition rows only)
- `.ai/state.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, this plan

## Non-goals

- No new save section, no mutable state, no gameplay decision change.
- No new panel, no new authority, no Unity dependency.
- No full test suite; no commit.

## 3 find → repair → harden loops

1. **Duplicate keys.** The concurrent agent added the same `dispatch_refused`, `refusal.*`, and `unnamed` rows; removed my duplicates (kept the canonical rows). Re-verified 0 duplicate keys across the whole catalog.
2. **Orphans + gate.** Verified 0 orphan wave-8 keys, 0 source→catalog misses, and that `EveryUiExpeditionKeyLiteral_HasACatalogRow` covers the new literals. Removed my 3 redundant `est.*` keys after the concurrent agent localized the estimate line.
3. **Final verification.** Host build 0/0; `ExpeditionLocaleKeysTests` 5/5; `StringsCsvLocaleGateTests` 4/4; `LocalizationRatchetTests` 2/2; `ExpeditionPrepPlanTests` 54/54; bounded `--expedition-panel-uitest` PASS; scoped `git diff --check` clean; expedition key count 205 (floor 127).

## Evidence

Host build 0 warnings / 0 errors; locale/source gate suite green; UI probe PASS.
No full suite; no commit.