# Dose Location Accumulation Contract

> **Mechanism:** How duration spent in a dose location transforms into a booked reading on a survivor's dosimeter ledger.

---

## 1. Accumulation Pipeline

The complete flow from location presence to permanent ledger record:

```text
Location Baseline (µSv/h)
      │
      ▼  × Dwell Duration (hours)
Unshielded Exposure (µSv)
      │
      ▼  ÷ 1000
Nominal Exposure (mSv)
      │
      ▼  × Environmental / Weather Modifiers (Fallout storm, etc.)
Incident Environmental Dose (mSv)
      │
      ▼  [BookReading Invocation]
  ┌───┴──────────────────────────────────────────┐
  │ Pre-exposure Anti-Rad: × 0.50                │
  │ Personal Shielding Factor: × factor          │
  │ Post-exposure Anti-Rad Chelation: × 0.60     │
  │ Flux Ambiguity (High-Energy): × (0.85 - 1.15)│
  └───────────────────┬──────────────────────────┘
                      ▼
            Booked Reading (mSv)
                      │
                      ▼
         Appended to readingsHistory
                      │
                      ▼
    cumulativeMsv += booked; Check Band Thresholds
```

---

## 2. Invariants & Guardrails

1. **Tag Requirement:** Only survivors who have been issued an active `assignedDosimeterTag` in `DoseLedgerSystem` receive booked readings in `readingsHistory`. Unbadged survivors suffer physical damage in `RadiationSystem`, but their records remain blank in the Dose Ledger ("the shelter's silence").
2. **Deterministic Attenuation:** Pre-exposure anti-rad halves incident dose (`× 0.5f`). Personal shielding applies linearly. Post-exposure anti-rad reduces booked dose by 40% (`× 0.6f`).
3. **Band Escalation:** Cumulative mSv triggers milestone events at 100 mSv (Amber), 300 mSv (Red), and 600 mSv (Black ceiling).
4. **Source Attribution:** Every `DoseReading` records `source` set to the location ID (e.g. `loc_ruined_hospital_grounds`), ensuring historical audits can inspect where every fraction of a millisievert was acquired.
