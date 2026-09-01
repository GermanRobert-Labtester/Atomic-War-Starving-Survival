# Tide Window Matrix (Plan 23 / Task 23C)

Authority: `TideCalendar` (Core, deterministic) — phase derives **purely from the
authoritative campaign day** (4-day cycle: day%4 → Low, Rising, High, Falling).
No wall clock, no RNG, no serialized tide state; old saves (no day authority)
default to ungated. Windows are gameplay **windows**: the player can plan around
them, work another task, or advance time — never sit in real time.

## Authored windows (6 sites, 4 distinct patterns)

| Site | `tide_window` | Semantics | Player-facing feedback |
|---|---|---|---|
| `site_exp09_flooded_metro` | `low` | shallows exposed only at Low | "Open only at low tide" |
| `site_exp09_ss_sovereign` | `slack` | launch only on a turning tide (Rising/Falling) | "Open at slack water only" |
| `site_exp09_submerged_siphon` | `high` | deep-water approach at High | "Open only at high water" |
| `site_exp09_offshore_relay` | `falling` | narrow entry drains open on Falling | "Opens on the falling tide" |
| `site_exp09_flooded_field_hospital` | `falling` | barge reachable as water drops | "Opens on the falling tide" |
| `site_exp09_submerged_siphon` | `high` | deep approach easier at High | "Open only at high water" |
| `site_exp09_sunken_submarine` | `unsafe_at_peak` | closed during peak flow (Rising) | "Unsafe during peak flow" |
| all others | `any` | ungated | "Any tide" |

(Seven non-any windows across 6 distinct patterns; the Sovereign's slack gate is the
high-tier example — the ferry/barge/metro tier stays discoverable with minimal gates.)

## Player-facing rules

- Windows derive from the **campaign day** (`TideCalendar.PhaseFor`), never real time —
  saving, loading, or waiting in real time cannot shift a window.
- `DaysUntilOpen(window, day)` gives the deterministic forecast (0 = open now).
- Forecast source: the tide-table knowledge item — audit result: the canonical
  `lighthouse_logbook` (items.json, "recording coastal weather, tides…") is the
  existing tide-table item; the Codekeeper (Uma Tarran) and Cape Beacon trade are its
  acquisition paths. **No second calendar UI** — windows surface through the existing
  Maritime Atlas detail rows (tide window + live phase + days-until-open).
- Alternate activity: any site with `any` window (8 of 14) is always launchable, plus
  the entire inland game.
- No permanent lockout: the cycle is 4 days; every window recurs at least twice per
  cycle. `save/load cannot change the expected window` (pure function of day).

## Consumers

- `MaritimeDiveSystem.CanLaunch(siteId, campaignDay, items, out blocker)` — the launch
  gate (tide + gear), returning stable blocker keys (`tide:<phase>`).
- `MaritimeAtlasPanel.TideText` — presents the authored rule + today's phase + days
  until open through `CampaignDayProvider` (no UI-owned tide state).
- Flotilla radio weather advisory references live tide/current language (Task 23D).
