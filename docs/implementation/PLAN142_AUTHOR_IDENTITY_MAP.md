# Plan 142 Author Identity Map

## Resolution rule

An authored string becomes a survivor identity only on an exact match against
the canonical survivor catalog. No role, name fragment, or prose inference is
used.

| Source | Exact survivor resolution | Unresolved handling |
|---|---:|---|
| `journal_entries_expansion_05.json` | 28/28 | none |
| `narrative/journals_expansion.json` | 0/40 | preserve source author as `AuthorName`; keep `AuthorId` empty |
| `journal_entries_batch_1.json` | 18/18 | none |
| `journal_entries_batch_2.json` | 15/15 | none |
| `journal_entries_batch_3.json` | 88/88 | none |

Expansion-05 exact IDs are `elena_vasquez` and `marcus_olejnik`.
The narrative ambient names (`quartermaster_yelena`, `child_rima`,
`doctor_ivan`, `courier_bram`, `scavenger_mira`, `teacher_suki`,
`engineer_tomas`, and `farmer_old_petr`) are not silently mapped to similarly
named survivor roles.

The unresolved path is intentionally not a fabricated author. It is a
display-only attribution from the data authority; the empty `AuthorId`
prevents survivor-state joins and codex identity claims.

System-generated entries continue to use the existing `Unknown` fallback when
their producer supplies no author.
