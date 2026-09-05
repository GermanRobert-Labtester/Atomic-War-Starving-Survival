# ASHFALL Radio Research Capability Matrix (AF-B1 / Plan 60)

**Document ID:** RADIO-RES-MATRIX-01
**Date:** 2026-09-05
**Scope:** Specification of research nodes, equipment gating, and radio reception capabilities in ASHFALL.

---

## 1. Core Architectural Directive

> **INVARIANT**: Research unlocks receiver and equipment capabilities. Research NEVER gates, filters, or alters what a radio station broadcasts in the simulation or what schedule slot is active.

A radio station broadcasts on its authored schedule regardless of whether the player's bunker has electricity, a functioning tube radio, or a scrap crystal set. Player research changes the bunker's ability to tune, decode, amplify, or locate those signals.

---

## 2. Research Capability Mapping

| Research Node ID | Gated Capability | Reception Impact | Host / Core Surface |
|---|---|---|---|
| `knowledge_radio_basics` | Basic AM/FM Reception (88–108 MHz) | Enables tuning standard broadcast bands; unlocks `RadioPanel` basic tuner. | `RadioHostSession`, `RadioPanel` |
| `knowledge_radio_advanced` | Shortwave & High Frequency Band (10–30 MHz, 130–150 MHz) | Unlocks military/clandestine bands (including Numbers Station and automated relay). | `RadioHostSession`, `RadioStationCatalog` |
| `knowledge_signal_triangulation` | Direction Finding & Acoustic Bearing | Enables `SignalTriangulationSystem` bearing computation for discovered emitters. | `SignalTriangulationSystem`, `TriangulationPanel` |
| `knowledge_clandestine_decryption` | SIGINT Cipher Rotor Decoding | Decodes encrypted numbers station messages into actionable intel or coordinates. | `RadioScheduleCoordinator`, `RadioIntercept` |
| `knowledge_antenna_arrays` | Mast Array & RF Pre-Amplification | Eliminates distance loss; grants `+20%` effective signal strength (`antenna_bonus`). | `RadioSignalStrength`, `RadioReceptionFactors` |
| `knowledge_portable_transceiver` | Field Expedition Radio | Allows expedition parties to receive bunker broadcasts and distress signals on the march. | `ExpeditionHostSession`, `PatrolRadioHooks` |

---

## 3. Degradation Reasons & Environmental Attenuation

The signal model computes `RadioSignalStrength` deterministically from:
1. **Raw Base Strength** (from station transmitter profile: 0.0 to 1.0).
2. **Degradation Reasons** (additive or multiplicative penalties):
   - `distance_loss`: Signal attenuation based on distance from transmitter coordinates.
   - `weather_attenuation`: Atmospheric ionization, ash storms, or fallout clouds.
   - `power_brownout`: Bunker receiver power dips or transmitter brownout.
   - `receiver_damage`: Degraded condition of bunker radio module.
   - `jamming`: Electronic counter-measures active on that frequency band.
3. **Enhancement Bonuses**:
   - `antenna_bonus`: Upgraded mast array or rooftop whip antenna.
   - `amplifier_bonus`: Vacuum tube pre-amplifier module installed.

---

## 4. Invariant Verification

Any test asserting that researching `knowledge_radio_basics` or `knowledge_radio_advanced` alters a station's broadcast message, schedule slot, or frequency will fail the authority gate. Schedules and broadcast availability are pure functions of `(stationId, campaignDay, hour)`.
