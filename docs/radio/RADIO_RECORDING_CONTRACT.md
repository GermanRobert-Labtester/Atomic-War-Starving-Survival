# Radio Recording & Cassette Replay Contract

> **Document Status:** Authoritative Cassette Recording & Replay Rules
> **Authority:** Plan 24 (Task 24Q, 24R)
> **Primary Systems:** `RadioRecordingSystem.cs`, `VinylMoraleSystem.cs`, `InventorySystem.cs`

---

## 1. Core Principles of Recording

1. **Recording Captures Metadata & Audio Prose:** When the player uses a blank magnetic tape (`item_blank_magnetic_tape`) to record an active broadcast, an authoritative recorded cassette item (e.g. `item_cassette_recorded_log`) is created in inventory.
2. **Replay Is Non-Mutating:** Replaying a recorded cassette plays the audio clip and displays the recorded transcript. Replay **NEVER** re-triggers one-shot world flags, never re-spawns distress missions, and never awards duplicated experience or standing.
3. **Cipher Re-Analysis Permitted:** A player who records an encrypted numbers station broadcast can listen to the tape later when they find the matching cipher codebook. The tape satisfies the "heard broadcast" condition in `CipherQuestChainEngine`.

---

## 2. Cassette Trade & Information Goods (Task 24R)

- **High-Value Recordings:** Only rare, strategic intelligence recordings have barter value (e.g. Garrison officer mutiny wiretap, secret artesian wellhead coordinates, Verdict census leak).
- **Routine Transmissions:** Routine civilian weather reports or basic static recordings have zero trade value, preventing inventory spam.
- **Copying / Duplication Guard:** Tapes cannot be infinitely duplicated without consuming physical magnetic tape stock and power, preventing infinite wealth exploits.
