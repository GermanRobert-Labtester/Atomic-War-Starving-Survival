# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Repo-wide 6-loop sweep round 2 — unbound placeholders + full small-panel l10n

> **STATUS: APPROVED BY USER**

User-directed ("do 6 looping phases of find issues, repair them, harden spots where
issues found and repeat repo wide!"). Continuation of the round-1 sweep.

## The 6 loops

**Loops 1–3 — remaining unbound/unavailable placeholders (6 panels):**

1. `AutopsyReportPanel` → `ui.autopsy.unbound`.
2. `CrossingQuestPanel` → `ui.crossing_quest.unbound`.
3. `EmergencyResponseHud` → `ui.emergency_response.action_unavailable`.
4. `MaritimePanel` → `ui.maritime.unavailable`.
5. `MusterPanel` → `ui.muster.unbound`.
6. `StandingRecordPanel` → `ui.standing_record.unavailable`.

**Loops 4–6 — fully localize and register 6 chrome-only panels:**

1. `CraftingPanel` → `ui.crafting.no_session` / `.no_active`.
2. `DynamicQuestlinePanel` → `ui.dynamic_questline.active_operations` / `.resolved_failed`.
3. `InSarMappingPanel` → `ui.insar.survey_platform` / `.repeat_pass_survey`.
4. `InventoryDetailPanel` → `ui.inventory_detail.no_item` / `.no_stats`.
5. `JournalDetailPanel` → `ui.journal_detail.no_session` / `.no_entries`.
6. `ShelterSchedulePanel` → `ui.shelter_schedule.toggle_curfew` / `.emergency_override`.

## Hardening

- Six panels added to the CI drift gate `LOCALIZED_SURFACES`, so
  `RegisteredPanels_HaveNoRawChromeLiteral` now holds them to **zero** raw chrome
  and every key they name must resolve with German parity.
- `Text=` UI-literal ratchet tightened **510 → 490**.
- Raw `Make*(...)` chrome ratchet tightened **1411 → 1399**.

## Evidence

Host build 0 warnings / 0 errors; `StringsCsvLocaleGateTests` 4/4;
`StatusPanelThresholdTests` 211/211 (incl. `RegisteredPanels_HaveNoRawChromeLiteral`
and `LocalizedSurfaces_RegisteredInDriftGate`); `LocalizationRatchetTests` 3/3;
`ExpeditionLocaleKeysTests` 5/5; `l10n_drift_gate` PASS (1485 keys, 940
localized-surface references, German parity verified); 0 duplicate catalog keys;
`git diff --check` clean. No full suite; no commit. Concurrent-writer churn caused
two transient ratchet/CSV failures that passed on re-run; the durable ones were
repaired.

## Non-goals

No save section, mutable state, gameplay decision, or new authority.
