# Radio Audio Hooks & Performance Bible

> **Document Status:** Authoritative Audio Handoff for Plan 07
> **Authority:** Plan 24 (Task 24AT, 24AU, 24AV)

---

## 1. Audio Hooks Architecture

The radio system is completely decoupled from mandatory audio assets. Every broadcast is 100% playable via textual display, accessibility subtitles, and visual static meters. When audio production (Plan 07) delivers VO clips, they attach via `audio_cue` catalog IDs.

| Station / Broadcast | Voice Profile | Recommended Filter / Effect | Ambient Bed | Mapped / Candidate Audio Cue |
|---|---|---|---|---|
| `station_civil_defense` | 50s male, crisp mid-Atlantic, clipped authoritative cadence | Bandpass filter 300Hz–3.4kHz, mild tape saturation, 50Hz hum | Subdued studio room tone | `radio_vo_civil_defense_bulletin` |
| `station_garrison_overlord` | 40s gravelly, harsh military diction, rapid phonetic groups | High compression, radio static burst on mic key, squelch tail | Diesel generator clatter | `radio_vo_ch7_milband` |
| `station_vitrified_crater` | Deep resonant male/female chant, slow echoing cadence | Large vault reverb, extreme low-end boost, zero high hiss | Pure analog vacuum hiss | `radio_vo_kind_hatch` |
| `station_open_classroom` | 30s warm female, patient, chalk tap opens broadcast | Clean near-mic acoustic, subtle room flutter | Faint classroom children murmurs | `radio_vo_classroom_lesson` |
| `station_numbers_sigint` | Cold synthetic female monotone / clockwork chime | Linear phase vocoder, hard quantization | 1kHz carrier tone, heterodyne whistle | `radio_vo_numbers_station_triad` |
| `station_automated_relay` | Robotic speech synthesizer, mechanical clicks | Severe 8-bit downsampling, periodic telemetry beep | High atmospheric static | `radio_vo_ch3_ash_road` |

---

## 2. Accessibility & Subtitle Guarantee (Task 24AV)

- **Complete Text Fallback:** Text subtitles are always rendered directly in the radio log and main HUD display.
- **Visual Signal Cues:** Signal strength is visually rendered via S-meter bar charts and VU needles (0–9 S-units, 0–5 bars).
- **Zero Sound-Only Puzzles:** All cipher groups, numbers station codes, and distress coordinates appear in clear text transcripts.
