# PLAN 123 — Sound-Ranging Closeout (Phase 12)

**Verb:** `--sound-ranging-selftest` (alias) · **Characterization:** `docs/combat/PLAN_123_SOUND_RANGING_CHARACTERIZATION.md` · **Authority map:** `docs/combat/PLAN_123_SOUND_RANGING_AUTHORITY_MAP.md`

## 1. Files changed

| Layer | Files |
|---|---|
| Data | `sound_ranging_catalog.json` (schema_version 1) |
| Core | `SoundRangingCatalog.cs` (loader/validator), `SoundRangingThreatEngine.cs` (defensive observation engine, Capture/Restore) |
| Host | `SoundRangingHostSession.cs` (typed `OnThreatEstimate` projection), `SoundRangingSaveStore.cs`, Main wiring, harness/soak |
| UI | `SoundRangingPanel.cs` |
| Save | `sound_ranging` row (owner `combat`) + `sound_ranging_save.json` |
| Tests | `Plan123SoundRangingCatalogTests` (8), `Plan123SoundRangingThreatEngineTests` (14), persistence subset |

## 2. Catalog entries

1 array profile (Mk I: 4 sensors, 8° base error, 3-cell region, 9000 confidence cap), 1 sensor profile, 2 timing-quality profiles, 3 atmospheric-error profiles (clear/wind/storm), 2 source-class signatures, 1 maintenance profile. Validators: dup-ID rejection, ranges, array→sensor/maintenance FKs. **Schema invariant (test-enforced): the catalog carries NO targeting fields** (`target_cell`/`firing_solution`/`counter_battery`/etc. are structurally forbidden).

## 3. Tests added (22 focused cases)

Catalog: structural validation, coarse-resolution design gate (bearing error ≥ 2°), FK resolution, **no-targeting-schema scan**, dup-ID, determinism. Engine: no-event→no-threat; coarse bounded estimate; salvo correlation narrowing to the 1-cell floor with capped confidence; wind broadening + storm refusal; damaged-sensor widening; <2 sensors cannot localize; array-offline typed failure; calibration drift→invalid→recalibration; moving-source break; staleness decay→5-day expiry; same-seed parity; sabotage recovery; **DTO reflection scan: no weapon-targeting fields, exactly bearing/radius/confidence**; error never tighter than authored base.

## 4. Selftests run

41/41 (threat projection event + defensive-schema gate), 23/23 (75-day stages C/G), 24/24 (8-cell event matrix), data-integrity 317/317.

## 5. Save/migration matrix

| Save point | Reload | Gate |
|---|---|---|
| missing section | undeployed array, no threats | `Sra_null_save_is_clean_migration` |
| partial array | node states preserved | node seam tests |
| active threat estimate | bearing/radius/confidence preserved | `Sra_roundtrip_preserves_active_threat` + `sra_save_roundtrip` |
| stale observations | remain stale, expire deterministically | DecayDay tests + harness |
| sabotaged node | operational flags preserved | node seam |
| unknown sensor profile | guarded (clamps) | RestoreState |

## 6. Determinism evidence

Same-seed parity over 5-event sequences; matrix cells reproducible per-cell seeds; 75-day harness `stageH_threat_hash` (save@71 → replay parity).

## 7. Content-utilization evidence

Sensor install/repair items (`item_bedrock_sensor_rig`, `item_low_noise_sensor_amplifier`, `item_military_radio_module`, `item_battery_reconditioned`) resolve in `items.json` (validator-walked). Consumers of the typed intel: `OnThreatEstimate` projection (map/warning consumers), expedition route-avoidance seam — full scenario effect visible in the 75-day harness stages C/G.

## 8. Scene-binding evidence

Route `sound_ranging` registered + routed; PanelRouteGate 20/20; panel lifecycle 16/16; a11y PASS; scene lint 30/0. Panel presents confidence as text labels (Low Confidence / Probable Sector / High Confidence Threat Zone) — no coordinates exist to display.

## 9. Known limitations

- **No per-strike hostile-fire emitter exists upstream** (plan §24 premise drift): `FactionWarSystem` counts aggregate strikes only. The engine owns the `HostileFireObservation` input seam; wiring a producer (extending FactionWar) is a separate foreman-approved workstream. The harness feeds observations directly, which is the designed intake contract.
- The dormant `Radio/AcousticDirectionFindingCatalog.cs` (zero consumers) was deliberately left untouched — quarantine candidate, not an extension target.
- Ally-settlement warning delivery routes through existing radio/diplomacy owners (typed event only).

## 10. Follow-ups

Emitter wiring decision (§4.1 of the authority map); map-marker projection when the map owner adds a threat-marker type; second-tool review (§21) PERFORMED 2026-09-13 (`docs/PLANS_122_125_SECOND_TOOL_REVIEW.md`, PASSED).
