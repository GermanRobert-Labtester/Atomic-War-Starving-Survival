# Document identity cross-reference matrix

Identity resolution uses exact canonical IDs only. A name appearing in `posted_by` or a transcript is otherwise display prose. No fuzzy matching or phantom survivor creation is performed.

| Authored identity | Exact canonical match | Disposition |
|---|---|---|
| Teacher Suki | `suki_tanaka` in `survivors.json` | Proven display identity; no runtime dependency added because the document is readable without a survivor lookup |
| Dr. Elena Rostov | `elena_rostov` in `survivors.json` | Proven catalog identity from the survivor catalog; the current paperwork corpus uses “Doctor (senior)” and does not force-bind the text |
| Bram | none proven for this corpus's exact short name; `npc_bram_ostrowski` is a separate character catalog entry | Display-only; not bound to a survivor slot or courier dispatch |
| Tomas / Engineer Tomas | `npc_tomas_geret` is a character entry, but the authored “Engineer Tomas” role is not an exact full-name assertion | Display-only in the reader; no character object created |
| Mira / Scavenger | `npc_mira_vos` exists, but the short role reference is not identity proof | Display-only |
| Petr / Farmer Petr | no exact canonical survivor or character identity proven | Display-only |
| Yelena / Quartermaster | no exact canonical survivor or character identity proven | Display-only |
| Rima, Loma family, river woman, unnamed patient, single adult | no exact canonical entity mapping in the active survivor catalog | Display-only; the river woman and single adult remain authored unidentified people |
| Doctor (senior), Clinic Duty Nurse, Quartermaster, Duty Clerk, Captain of the Gate | occupational/posting roles, not person IDs | Display-only role labels |

The source's family relationships, ages, deaths and chronology were compared with `docs/narrative/CONTINUITY_REPORT.md`. The Loma seven-person arrival, river-woman Day 33 death, Bram courier thread, dam/Kestrel-9 thread and ration/census relationship are treated as authored continuity. None creates or mutates a live survivor aggregate when read.
