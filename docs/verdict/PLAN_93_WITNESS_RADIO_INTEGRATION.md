# Plan 93 — Muster Witness, Radio, and Recurring NPC Integration

> **Cross-Plan References:**
> - Plan 84: Muster Witnesses (`Assets/StreamingAssets/Data/muster_witnesses.json`)
> - Plan 94: Verdict Radio (`Assets/StreamingAssets/Data/verdict_radio.json`)
> - Plan 52: Recurring NPCs (`Assets/StreamingAssets/Data/characters.json`)

---

## 1. Plan 84 Muster Witness Integration

Exactly 3 Verdict NPCs are designated for identity reuse as witnesses within the Muster testimony network:

| Verdict NPC ID | Character Name | Verdict Role & Kind | Muster Witness Anchor Location | Witness Evidence Role |
|---|---|---|---|---|
| `npc_garrick_daal` | Garrick Daal | Cliff-Bunker Signalman (`tape_echo`) | `loc_clifftop_observation_bunker` | Testifies on automated packet origin rerouting and coastal repeater failures. |
| `npc_sena_korr` | Dr. Sena Korr | Marine-Lab Researcher (`paper_ghost`) | `loc_sealed_marine_laboratory` | Testifies on biological contamination baseline tampering prior to official dates. |
| `npc_karel_norn` | Karel Norn | Border-Relay Operator (`tape_echo`) | `loc_decommissioned_signal_relay` | Testifies on pre-authorized civilian warning transmission preceding the command order. |

### Identity Invariant Rule
- **No Cloned Personas:** The characters are not duplicated as new people in `characters.json` or `survivors.json`.
- **Ontological Compatibility:** Both `tape_echo` and `paper_ghost` forms are archival records or recorded depositions. Plan 84 consumes them as preserved documentary testimony, not as living physical attendees at the campfire.

---

## 2. Plan 94 Verdict Radio Cross-References

Three Verdict NPCs possess direct resonance with the frequency transmissions in `verdict_radio.json`:

1. **`npc_ilya_venn` (Weather Observer):**
   - Corroborates `radio_verdict_eden_was_here` (88.5 MHz) and weather service transmission bleed; his preserved paper barograph proves central weather packet falsification.
2. **`npc_garrick_daal` (Cliff Signalman):**
   - Connects to `radio_verdict_meter_reads_1142` (99.0 MHz) and the automated carrier burst; his tape testimony proves the system acknowledged packets autonomously.
3. **`npc_karel_norn` (Border Operator):**
   - Links to `radio_verdict_count_is_open` (99.0 MHz / 88.5 MHz); his log records the exact minute warning traffic was forced onto the carrier tone.

---

## 3. Plan 52 Recurring NPC Compatibility

| NPC Kind | Physical Living Recurrence? | Allowed Recurrence Modes |
|---|---|---|
| `paper_ghost` | **No** (Deceased / Historical Archival) | Citations in ledgers, field notes, recovered journals, lab documents. |
| `tape_echo` | **No** (Recorded Voice) | Radio broadcasts, audio playback, playback transcripts. |
| `readings` | **No** (Instrument Telemetry) | Sensor readouts, meter logs. |
| `living` | **Yes** (`npc_selya_saltmarsh`, `npc_tomas_reid`, `npc_elena_vane`) | Physical meetings, dialogue encounters, tribunal testimony. |

All 9 new Plan 93 investigation NPCs are either `paper_ghost` or `tape_echo`, preserving the solemn, empty reality of the abandoned survey outposts without creating impossible living survivors in radio-sterile ruins.
