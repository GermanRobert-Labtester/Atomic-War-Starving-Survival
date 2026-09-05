# Archive Desk Save Contract

> **Persistence Architecture:** Serialization contract for `ArchiveDeskSystem`, `TranscriptionJob`, and compatibility guarantees across save versions.

---

## 1. Saved State Schema

`ArchiveDeskState` (captured via `ArchiveDeskSystem.CaptureState()` and stored in `ArchiveDeskSaveStore`):

```json
{
  "systemId": "archive_desk",
  "queue": [
    {
      "jobId": "trans_12_evidence_sample_archivist_1",
      "evidenceId": "evidence_sample",
      "archivistId": "archivist_1",
      "inkId": "ink_archival_carbon",
      "dayStarted": 12,
      "progressHours": 4.0,
      "totalHoursRequired": 4.0,
      "isComplete": true,
      "isCancelled": false,
      "legibilityScore": 0.95,
      "journalEntryId": "entry_evidence_sample"
    }
  ],
  "unlockedEvidenceIds": [
    "evidence_sample"
  ],
  "totalTranscriptions": 1
}
```

---

## 2. Save Compatibility Invariants

1. **Ink ID Reference:** Jobs reference `inkId` by string key. Static ink catalog definitions are NOT serialized into the save file, preventing bloat.
2. **Backward Compatibility:** Older saves containing transcriptions queued with the baseline 3 inks (`ink_iron_gall`, `ink_soot_lamp`, `ink_plant_dye`) restore seamlessly because those IDs are permanently preserved.
3. **No Save Version Bump:** The schema of `ArchiveDeskState` and `TranscriptionJob` remains unchanged.
