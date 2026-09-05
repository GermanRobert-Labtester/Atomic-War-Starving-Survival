# Verdict Radio Schema Contract

> **Data Path:** `Assets/StreamingAssets/Data/verdict_radio.json`
> **C# DTO:** `VerdictCatalogLoader.VerdictRadioEntry` in `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

---

## 1. JSON Schema Definition

```json
{
  "schema_version": 1,
  "broadcasts": [
    {
      "id": "radio_verdict_<slug>",
      "frequency": "99.0 MHz",
      "dayTrigger": 210,
      "source": "Infrastructure Name",
      "message": "Terse machine-issued status prose.",
      "signalStrength": "S1",
      "kind": "telemetry",
      "audio_cue": "optional_audio_cue_id"
    }
  ]
}
```

## 2. Field Specifications

| Field | Type | Required | Description | Constraints / Formats |
|---|---|---|---|---|
| `id` | string | **Yes** | Unique identifier | Prefix `radio_verdict_`, snake_case |
| `frequency` | string | **Yes** | Carrier frequency string | `"99.0 MHz"` (canonical carrier) or `"88.5 MHz"` (bleed/civilian) |
| `dayTrigger` | integer | **Yes** | Earliest campaign day eligible | Must be `>= 210` and `<= 365` |
| `source` | string | **Yes** | Originating facility or register | 1–5 words naming canonical infrastructure |
| `message` | string | **Yes** | Broadcast payload | 1–4 terse sentences; max 250 characters; machine register tone |
| `signalStrength`| string | **Yes** | Radio carrier strength | In `{"S1", "S2", "S3", "S4", "S5"}` |
| `kind` | string | **Yes** | Diegetic taxonomy | `telemetry`, `maintenance`, `census`, `calibration`, `anomaly`, `emergency`, `carrier`, `call`, `witness`, `readings`, `count` |
| `audio_cue` | string | No | Audio manager playback hook | Omitted for new broadcasts unless defined in `AudioCueCatalog.cs` |
