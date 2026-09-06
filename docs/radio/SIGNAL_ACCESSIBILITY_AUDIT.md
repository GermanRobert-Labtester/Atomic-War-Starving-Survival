# ASHFALL — Distress Signal Accessibility & Multimodal Presentation Audit (Task 20)

> **Document Status:** Authoritative Accessibility & Sensory Presentation Audit
> **Authority:** CVAA / WCAG 2.1 AA Compliance for Game Communications & Audio
> **Target Scope:** Multimodal feedback, color independence, closed captioning, screen-reader text, and motor controls for distress signals
> **Verification Harness:** `Ashfall.Core.Tests/Radio/RadioSignalAccessibilityPresentationTests.cs` (12 tests, 100% pass)

---

## 1. Executive Summary

Radio interception in post-apocalyptic survival games typically relies on audio static, Morse code beeps, and flashing colored dials. To ensure full accessibility for deaf/hard-of-hearing players, color-blind players, and players utilizing screen readers or keyboard navigation, ASHFALL's radio distress subsystem enforces strict multi-modal parity:

1. **Every auditory cue has an equivalent textual transcript.**
2. **Every color-coded indicator has a distinct textual badge and shape.**
3. **Every analog meter has a quantitative and qualitative textual readout.**
4. **All tuning dials support discrete stepped keyboard and gamepad increments.**

---

## 2. WCAG 1.4.1: Color Independence & Visual Badging

Color must never be the sole medium for conveying state, authenticity, or danger.

### 2.1 Authenticity Textual Badging Standard

| Classification | Primary Color Token | Textual Accessibility Badge | Icon / Glyph |
|---|---|---|---|
| Genuine Rescue | `#50FA7B` (Terminal Green) | `[GENUINE RESCUE]` | `[+]` Cross |
| Trap / False Flag | `#FF5555` (Alert Red) | `[SUSPECTED TRAP / LURE]` | `[!]` Hazard Triangle |
| Automated Beacon | `#8BE9FD` (Cyan) | `[AUTOMATED BEACON]` | `[*]` Pulse Star |
| Memorial Archive | `#BD93F9` (Muted Purple) | `[MEMORIAL ARCHIVE]` | `[^]` Monument Diamond |
| Unknown Signal | `#6272A4` (Muted Slate) | `[UNKNOWN TRANSMISSION]` | `[?]` Question Mark |

Players with red-green color blindness (protanopia/deuteranopia) or complete monochromacy receive unambiguous information through bracketed badge text and geometric glyphs.

---

## 3. WCAG 1.2: Closed Captioning & Audio Transcripts

All 25 distress signals feature complete textual transcripts for spoken dialogue and non-verbal audio artifacts:

### 3.1 Non-Verbal Audio Annotation Schema

All non-speech audio events are transcribed within asterisks or square brackets:
- `*static*` / `*heavy static burst*`
- `*a piano plays through the static—Chopin's Nocturne in E-flat major*`
- `*coughing and ragged breathing*`
- `*warning klaxon sounded in background*`
- `*continuous repeating tone*`
- `[morse code: S-O-S]`

A hard-of-hearing player reading the text receives the exact same atmospheric and narrative information as a hearing player listening to the audio stream.

---

## 4. Qualitative Clarity Tiers

Tuning into a weak or noisy transmission produces an analog clarity value between `0.0f` and `1.0f`. To reduce cognitive strain and support screen readers, clarity is mapped into discrete qualitative tiers:

| Clarity Range | Descriptive Tier Label | Screen Reader Announcement | Gameplay Effect |
|---|---|---|---|
| `0.00 – 0.14` | **Unintelligible** | "Signal unintelligible; carrier wave detected." | Scrambled text; cannot decode fragments. |
| `0.15 – 0.29` | **Heavy Static** | "Heavy static; sporadic words audible." | Fragment 1 partially readable; moral choice locked. |
| `0.30 – 0.49` | **Faint Audio** | "Faint audio; voice identifiable." | Fragments 1–2 readable; moral choice unlocked. |
| `0.50 – 0.74` | **Broken Signal** | "Broken signal; message coherent." | Fragments 1–3 readable; triangulation available. |
| `0.75 – 0.94` | **Clear Audio** | "Clear audio; strong reception." | Full message readable; exact coordinates revealed. |
| `0.95 – 1.00` | **Optimal Reception** | "Optimal reception; perfect lock." | Maximum clarity; full log recorded to Codex. |

---

## 5. Screen Reader & Natural Language Status Formatting

When cycling through intercepted transmissions or reviewing the journal, status descriptions provide natural conversational text rather than raw enum names or cryptic integers:

| Enum Identifier | Screen Reader Accessible Label |
|---|---|
| `DistressSignalStatus.Inactive` | "Inactive Signal" |
| `DistressSignalStatus.Intercepted` | "Signal Intercepted" |
| `DistressSignalStatus.Triangulated` | "Source Triangulated" |
| `DistressSignalStatus.Dispatched` | "Rescue Expedition Dispatched" |
| `DistressSignalStatus.ResolvedRescued` | "Survivors Rescued" |
| `DistressSignalStatus.ResolvedGrimTooLate` | "Arrived Too Late" |
| `DistressSignalStatus.ResolvedTrapDefeated` | "Ambush Neutralized" |
| `DistressSignalStatus.ResolvedMysteryDecoded`| "Mystery Decoded" |
| `DistressSignalStatus.Expired` | "Transmission Expired" |
| `DistressSignalStatus.ResolvedTrapAvoided` | "Hostile Trap Avoided" |
| `DistressSignalStatus.ResolvedIgnored` | "Transmission Ignored" |

### 5.1 Urgency Presentation (Countdown Units)
Rather than raw numbers (e.g. `days_remaining: 1`), countdowns are formatted with explicit units and urgency warnings:
- `0`: "Transmission Expired"
- `1`: "1 day remaining — Imminent loss"
- `2+`: "N days remaining"

---

## 6. Motor Accessibility & Tuning Controls

Tuning through radio bands (50 MHz to 900+ MHz) can be tedious or difficult for players with motor impairments if continuous analog dragging is required.
- **Three-Tier Stepped Tuning:**
  - Fine step: `±0.1 MHz` (D-pad Left / Right or Left / Right Arrow).
  - Medium step: `±1.0 MHz` (Bumpers LB / RB or PageUp / PageDown).
  - Band jump: `±10.0 MHz` (Triggers LT / RT or Shift + Arrow).
- **Auto-Seek / Next Signal:** Dedicated keybind (`Tab` / `Y Button`) steps directly to the next nearest detected carrier wave.
- **Focus Indicators:** High-contrast focus borders (`#F1FA8C` neon yellow, 2px border) surround the active tuner element.

---

## 7. Verification Evidence

All 12 accessibility presentation criteria are mechanically enforced in CI by:
`Ashfall.Core.Tests/Radio/RadioSignalAccessibilityPresentationTests.cs`
- `Multimodal_SignalIdentification_SurfacesTextualMetadata`: PASS
- `Authenticity_TextualBadges_IndependentOfColor`: PASS
- `ReadableFrequency_Formatting_Standardized`: PASS
- `ClosedCaptioning_AudioEffects_AreTextuallyTranscribed`: PASS
- `Status_Descriptions_AreScreenReaderFriendly`: PASS
- `ClarityLevels_MapToQualitativeTiers`: PASS (6 inline data tiers verified)
- `DaysRemaining_Presentation_ProvidesAccessibleUnits`: PASS
