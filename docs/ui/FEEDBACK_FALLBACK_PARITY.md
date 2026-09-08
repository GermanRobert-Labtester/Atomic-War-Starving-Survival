# Feedback Catalog & Fallback Parity Report

## 1. Parity Findings

A key-by-key, field-by-field automated diff between:
1. `Assets/StreamingAssets/Data/feedback_messages.json`
2. `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs` (`CreateDefaultContainer()`)

Result:
- **Total entries compared:** 200
- **Total differences found:** 0
- **Key mismatches:** 0
- **Category mismatches:** 0
- **Severity mismatches:** 0
- **Template string mismatches:** 0
- **Parameter count mismatches:** 0
- **Display duration mismatches:** 0

---

## 2. Rationale for Fallback Retention & Single-Source Authority

Why does `FeedbackMessageCatalogLoader.CreateDefaultContainer()` exist?
1. **Packaging & Headless CI Safety:** In isolated unit tests or minimal environments where `StreamingAssets/Data/` is not copied to the working directory, `CreateDefaultContainer()` guarantees zero runtime crashes or null references.
2. **Exported-Build Resiliency:** If asset bundling paths change or JSON fails to deserialize, the fallback container ensures core feedback messages are always available.
3. **Canonical Authority:** The JSON file remains the **authoritative authored source of truth**. Core tests enforce 100% parity between the authored JSON and the fallback container, preventing authoring drift.
