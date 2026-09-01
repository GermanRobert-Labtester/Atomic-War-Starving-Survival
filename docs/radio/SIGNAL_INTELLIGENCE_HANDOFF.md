# Signal Intelligence Handoff

> **Document Status:** Authoritative Cross-System Integration Contract
> **Authority:** Plan 24 (Task 24AC, 24AE, 24AH, 24AI, 24AJ)
> **Primary Consumer:** `CipherQuestChainEngine.cs`, `SignalTriangulationSystem.cs`, `WastelandMapSystem.cs`

---

## 1. The Intercept-to-Discovery Pipeline

```text
[ RadioTuner / Intercept ]
          |
          v
[ RadioSignalLog.RecordIntercept ] (Marks broadcast heard, logs frequency & S-units)
          |
          +---> Standard Transmission: Visible in Intercept Log & History Grid
          |
          +---> Cipher Carrier Broadcast:
          |         |
          |         v
          |     [ CipherQuestChainEngine.RecordBroadcastHeard ]
          |         |
          |         +---> Check for Matching Key Item in Inventory
          |         |         |
          |         |         +---[ Key Present ]---> Automatic Decode -> Map Node Revealed
          |         |         +---[ Key Missing ]---> Quest Log: "Coded Signal Intercepted"
          |
          +---> Directional Observation:
                    |
                    v
                [ SignalTriangulationSystem.RecordObservation ]
                    |
                    +---[ Observations >= 3 & Confidence >= 0.70 ]---> Map Node Revealed
```

---

## 2. Signal Triangulation Integration

1. **Direction Finding (DF):** When tuned to an active transmitter or distress call, the player can take a directional observation from the shelter antenna or during an expedition with a handheld receiver.
2. **Confidence Math:** Triangulation confidence scales with signal strength, receiver antenna calibration, and low atmospheric noise. Severe storms apply a noise penalty.
3. **Map Revelation:** When confidence reaches `0.70` across at least 3 distinct observations, the true node is marked on `WastelandMapSystem` (Plan 16), allowing expedition dispatch.

---

## 3. Future Oscilloscope Minigame Extension Point (Task 24AJ)

The Core decode architecture exposes clean, read-only frequency, modulation mode, and carrier waveform metrics without requiring timing reflexes. A future UI frontend can attach a visual CRT oscilloscope / Lissajous curve shader without modifying underlying puzzle logic.
