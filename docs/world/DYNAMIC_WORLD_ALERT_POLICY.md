# Dynamic World Alert Policy

> **Authority:** `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`, `src/Host/WorldHostSession.cs`

---

## 1. Alert Classification & Escalation Hierarchy

| Priority Level | Category | Examples | Trigger Timing | UI Presentation | Audio Notification |
|---|---|---|---|---|---|
| **Critical** | Imminent Hazard | Orbital Strike Day 0, Severe Fallout Storm | Day advance / immediate | Full modal prompt / Red Banner | Klaxon alert cue |
| **Urgent** | High Hazard Warning | Orbital Impact in 24h, Blizzard in 24h | Daily briefing | Amber HUD warning pill | Radio chirp / warning tone |
| **Preparation** | Strategic Advisory | 3–7 day weather outlook, Station degraded | Panel open / Daily briefing | Status advisory label | Silent |
| **Informational** | Normal Cycle | Season transition, Clear sky window | Daily summary log | Grey notification line | Silent |

---

## 2. Noise Suppression & De-duplication

1. **Weather Alerts:** Trigger audio alerts only when transitioning into true hazard states (`FalloutStorm`, `BlackRain`, `Blizzard`).
2. **Orbital Alerts:** Dispatched once upon detection and once at 24-hour imminent strike threshold.
3. **Seasonal Events:** Capped at 1 new event alert per day.
