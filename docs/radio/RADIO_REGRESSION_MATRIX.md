# Full Radio Regression Matrix

> **Document Status:** Authoritative Verification Suite
> **Authority:** Plan 24 (Task 24BI)
> **Target Scenarios:** 20 Core Regression Verification Gates

---

## 1. Regression Scenarios & Proof Contracts

| # | Scenario | Authoritative Contract | Verification Mechanism | Status |
|---|---|---|---|---|
| **1** | Day/Frequency Tuning Resolution | Tuning to station frequency within tolerance returns active scheduled broadcast or clear carrier | `RadioTunerTests`, `RadioScheduleCoordinatorTests` | PASS |
| **2** | Weather Forecast Integration | Severe storm in `WeatherSystem` prompts immediate urgent warning on `88.50 MHz` | `RadioWeatherIntegrationTests` | PASS |
| **3** | Orbital Harrow Warning | Orbital perigee decay schedules teletype warning on `104.70 MHz` / `88.50 MHz` | `RadioOrbitalWarningTests` | PASS |
| **4** | Faction War Schedule Evolution | War escalation shifts broadcast tone and unlocks wartime communiques | `FactionWarRadioTests` | PASS |
| **5** | Station Capture / Silence | Destroyed faction transmitter switches frequency to authentic static / silence | `RadioStationStateTests` | PASS |
| **6** | Genuine Rescue -> Recruit | Resolving genuine rescue mission spawns valid recruit with authentic `SurvivorCatalog` traits | `RadioDistressSystemTests` | PASS |
| **7** | Distress -> Grim Outcome | Expired or late distress mission reveals memorial log, supplies, and dead carrier | `RadioDistressSystemTests` | PASS |
| **8** | Distress -> False / Trap | Investigating decoy signal spawns raider encounter; combat resolution terminates mission | `RadioDistressSystemTests` | PASS |
| **9** | Mystery Call -> Sigint | Pre-war beacon provides cipher clue resolved through `SignalIntelligenceCatalog` | `RadioDistressSystemTests` | PASS |
| **10**| Number Station -> Cipher Decode | Intercepting cipher carrier with key item in inventory reveals canonical map node | `CipherQuestChainEngineTests` | PASS |
| **11**| Cassette Recording Isolation | Replaying recorded cassette displays transcript without duplicating live world rewards | `RadioRecordingSystemTests` | PASS |
| **12**| Route Disruption Bulletin | Map route closure is broadcast by Lineman's Loop / Garrison Logistics | `RadioRouteBulletinTests` | PASS |
| **13**| Disease Outbreak Warning | Cistern contamination triggers public health advisory with purification steps | `RadioDiseaseWarningTests` | PASS |
| **14**| Verdict Census Carrier | Day 210 + Reckoning Culpable opens 99.0 MHz Census machine registers | `VerdictRadioSystemTests` | PASS |
| **15**| Save During Active Distress | Reloading during active distress call preserves exact days remaining | `RadioSaveMigrationTests` | PASS |
| **16**| Save After Cassette Recording | Recorded tapes and signal log entries persist across save/load | `RadioSaveMigrationTests` | PASS |
| **17**| Save During Active Jamming | Jammed frequency state round-trips cleanly and recovers after storm/EMP | `RadioSaveMigrationTests` | PASS |
| **18**| V1 -> V2 Save Migration | Legacy V1 radio saves load seamlessly into V2 schema with clean default fields | `RadioSaveMigrationTests` | PASS |
| **19**| Exported-Build Dial Parity | All radio catalogs and fallback text resolve identically in standalone builds | `HostCli` `--radio-selftest` | PASS |
| **20**| Deterministic Tuner Trace | Identical seed + day + frequency produces 100% byte-identical tuner result | `RadioDeterminismTests` | PASS |
