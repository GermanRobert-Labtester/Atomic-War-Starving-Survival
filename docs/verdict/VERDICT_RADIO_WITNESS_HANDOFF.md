# Verdict Radio Witness Handoff Contract (Plan 84 Integration)

> **Witness Network Authority:** `Assets/StreamingAssets/Data/muster_witnesses.json`
> **Rule:** Machine broadcasts furnish objective timestamps, register counts, and technical telemetry that corroborate or challenge witness depositions.

---

## 1. Witness Corroboration & Contradiction Anchors

| Broadcast ID | Focus Witness Candidate | Corroboration or Contradiction | Objective Data Anchor |
|---|---|---|---|
| `radio_verdict_repeater_origin_mismatch` | Garrick Daal (`npc_garrick_daal`), Cliff Signalman | **Corroboration** | Garrick stated that after midnight packets arrived from unmapped origins and the system acknowledged them automatically. The broadcast logs: `Packet 884 origin header mismatch: physical path deviates from routing table. Packet forwarded as valid.` |
| `radio_verdict_spectrometry_drift_stjude` | Dr. Sena Korr (`npc_sena_korr`), Marine Researcher | **Contradiction / Procedural Clash** | Dr. Korr noted sample 12 was discarded and relabeled to fabricate an official start date. The machine broadcast dismisses the drift: `Sample rack 12 isotope drift: +0.08 permille. Hardware offset retained. Analysis valid as filed.` The machine's procedural indifference conflicts with human forensic integrity. |
| `radio_verdict_telemetry_phase_inversion` | Karel Norn (`npc_karel_norn`), Border-Relay Operator | **Corroboration** | Karel recorded the civil warning crossing the border before authorization. The broadcast documents an 18 ms telemetry phase inversion preceding the official signal header. |
| `radio_verdict_stilling_well_delta` | Mara Elsen (`npc_mara_elsen`), Tide-Gauge Keeper | **Corroboration** | Mara noted the harbor climbed 6 cm with zero storm activity. The broadcast logs: `Stilling well datum reads +6 cm against baseline brass scale. Tidal harmonic model predicts -14 cm. Discrepancy logged as sensor offset.` |

---

## 2. Invariant Rules
- The radio does not express moral or personal judgments on witness character.
- Discrepancies represent real differences between recorded sensor telemetry and human memory/observation.
