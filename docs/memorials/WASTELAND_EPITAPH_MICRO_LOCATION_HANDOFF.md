# Wasteland Grave Epitaphs — Micro-Location Discovery Handoff

**Originating Plan:** Plan 49 (`Assets/StreamingAssets/Data/micro_locations.json`)
**Consumer:** Travel discoveries along expedition routes
**Integration Point:** `micro_improvised_grave`

---

## 1. Micro-Location Structure

`micro_locations.json` contains:

```json
{
  "id": "micro_improvised_grave",
  "title": "Improvised Grave",
  "choices": [
    { "choiceId": "respect_grave" },
    { "choiceId": "inspect_grave_marker", "journalUnlockId": "micro_improvised_grave_marker" },
    { "choiceId": "disturb_grave" }
  ]
}
```

---

## 2. Integration Contract

1. **Single Text Authority:** `micro_locations.json` does not embed hardcoded grave epitaph text. When an expedition party inspects an improvised grave marker, the display text is resolved from `wasteland_grave_epitaphs.json` using the route's deterministic seed and the local sector hazard profile (e.g. fallout zone -> `radiation`, conflict boundary -> `combat`, blizzard ridge -> `exposure`/`frostbite`).
2. **No Data Duplication:** Environmental grave encounters dynamically draw from the 30-entry pool without creating redundant catalogs.
