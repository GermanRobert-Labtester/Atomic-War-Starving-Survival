# Verdict Radio NPC Handoff Contract (Plan 93 Integration)

> **NPC Authority:** `Assets/StreamingAssets/Data/verdict_npcs.json`
> **Dialogue Authority:** `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

---

## 1. Character-to-Radio Resonance

The 30 Verdict radio broadcasts operate as the environmental soundscape that several key Verdict NPCs reference or react to:

| NPC ID | Character Name & Role | Associated Radio Broadcast(s) | NPC Perspective & Reaction Seam |
|---|---|---|---|
| `npc_eden_vale` | Eden Vale (Amateur radio operator) | `radio_verdict_eden_was_here`, `radio_verdict_unscheduled_burst_88` | Eden tracked the 88.5 MHz carrier bleed and recorded the unscheduled burst timings on magnetic tape. |
| `npc_ferris_voss` | Ferris Voss (Fire-control engineer) | `radio_verdict_fuse_serviced`, `radio_verdict_substation_breaker_test` | Voss observed the automated 12 ms breaker trip and recognized the system servicing itself without an operator. |
| `npc_maro_veen` | Maro Veen (Tape loop voice) | `radio_verdict_carrier_on_window`, `radio_verdict_carrier_override_standby` | Veen's voice serves as the master pilot tone and final carrier shutdown announcement. |
| `npc_whisper_cipher` | Whisper Cipher (Relay aggregate readings) | `radio_verdict_relay_switch_pass4`, `radio_verdict_telemetry_phase_inversion` | The aggregate telemetry stream broadcast over 99.0 MHz carrier. |
| `npc_selya_saltmarsh` | Selya Saltmarsh (Census clerk) | `radio_verdict_subsector_ledger_update`, `radio_verdict_holding_capacity_parity` | Selya tracks the quota parity and notes the remaining margin of 1 person before the audit window closes. |

---

## 2. Decoupled Architecture
- Broadcasts do not call NPC methods or mutate NPC dialogue directly.
- The NPC system interrogates heard broadcasts via the shared event bus (`radio.verdict.broadcast`) or `VerdictRadioState.firedIds`.
