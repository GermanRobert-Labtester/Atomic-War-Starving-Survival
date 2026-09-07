# Plan 205 — Wasteland Cargo Airdrop & Emergency Supply Recovery — Closeout

**Flagship:** Plans 202–205. **Status: IMPLEMENTED & VERIFIED** (commit `00802b18`, Wave F hardening in the flagship integration log).

## Contracts

- **Scheduling (§7.4):** drops originate from **resolved radio contacts** —
  `Main` matches `RadioDistressSystem.OnSignalResolved` outcome types against
  profile `trigger_signal_outcomes` (only real
  `radio_distress_signals.json` outcomes: supply_cache, military, knowledge,
  child_voice). The engine's `max_active_drops` gates flooding with typed
  failures; declined offers are journalled, never silently dropped.
- **Wind (§7.5, §8.6):** the surface wind vector lives in
  `World/WeatherSystem` (single authority) — direction + speed rolled
  deterministically alongside every weather roll from the same seeded rng;
  severe kinds run higher speed bands; capture/restore carries it; old saves
  default calm. No second weather source anywhere.
- **Drift & landing (Traps I+H):** wind snapshot persisted at release;
  landing computed once, quantized to integer grid coordinates — never
  recomputed or rerolled after load.
- **Damage (§7.7-7.8):** abstract impact curve (base + wind factor vs
  light/heavy thresholds) degrades `cargo_integrity_pct`; per-entry
  deterministic quantity losses at landing. Fragile cargo rules remain
  per-profile, not per-item hardcoding.
- **Beacon (§7.9):** active on landing for `beacon_duration_days`, expires
  (day/night presentation is the host's), reactivatable while the crate is
  intact and on schedule.
- **Interception race (§7.10):** deterministic daily accrual from landing;
  **recovery stops the clock**; ≥10000bp → intercepted (lost), journalled.
- **Recovery (§7.11-7.13, Trap G):** the landing registers a canonical
  expedition destination (`ExpeditionDefinitionRegistry`) — recovery is a
  standard sortie; crate transfer flows through
  `ExpeditionSystem.TryGrantLoot`, so **`maxLootCapacityKg` is enforced by
  the expedition authority per item**; partial recovery keeps the remainder
  crated (retryable, never duplicated, never injected into shelter
  inventory outside the expedition return flow).
- **Crate contents (Trap H):** generated once at scheduling from authored
  pools, persisted in the event state — never rerolled after load.
- **Deviation note:** `DamagedMapSystem` is a fragment-puzzle authority
  (fragments→zones), not a world-point registry — destinations use
  `ExpeditionDefinitionRegistry`, a stronger no-duplicate-map guarantee.

## Failure-state matrix (§18, verified by test)

| Failure | Behavior |
|---|---|
| Invalid profile / missing contact | `airdrop.profile_invalid` / `airdrop.missing_signal` |
| Max active drops | `airdrop.max_active` — journalled offer declined |
| Beacon damaged/expired | dark beacon; reactivation blocked unless landed |
| Hostiles arrive first | phase `intercepted` — cargo lost |
| Expedition capacity full | partial recovery; remainder stays crated |
| Save/load mid-descent | same wind, same progress, same landing (gated) |

## Verification

15 tests (`CargoAirdropEngineTests`) + Wave F cross-plan replay
(continuous vs day-15 and multi-point splits over 30 days).
Suite: 9166 total / 9157 passed at closeout (sole failures = concurrent
pharma stream). No actionable parachute engineering procedure is included —
descent profiles, canopies, and release hardware are abstracted entirely.
