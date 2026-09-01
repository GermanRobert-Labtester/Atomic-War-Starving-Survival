# Dynamic World Save Contract & Migration Integrity

> **Authority:** `src/Host/WorldSaveStore.cs`, `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`

---

## 1. Persisted State Hierarchy

```json
{
  "State": {
    "systemId": "world_weather_system",
    "currentKind": "Clear",
    "totalElapsedHours": 120.0,
    "hoursUntilNextCheck": 4.5,
    "rollCount": 20,
    "restrictToNonHazardWeather": false
  },
  "SkyArmor": {
    "cells": [
      { "gridX": 10, "material": 2, "thicknessMeters": 1.5, "currentDurability": 100.0 }
    ]
  },
  "WeatherIntelligence": {
    "station": {
      "systemId": "weather_station",
      "isInstalled": true,
      "isCalibrated": true,
      "installDay": 1,
      "calibrationDay": 2,
      "forecastHorizonDays": 7,
      "accuracy": 0.85,
      "durability": 100.0,
      "hasSensorFault": false,
      "faultReason": "",
      "lastForecastDay": 5,
      "cachedForecast": []
    },
    "orbital": {
      "systemId": "orbital_harrow_telemetry",
      "telemetryActive": true,
      "lastImpactDay": -1,
      "nextImpactDay": 12,
      "warningLeadDays": 3,
      "targetGridX": 10,
      "affectedCellSpread": 2,
      "impactEnergyMj": 35.0,
      "scheduledEventId": "event_orbital_heavy_kinetic_impact",
      "scheduledEventName": "Tungsten Penetrator Plunge",
      "revealedSiteId": "loc_excavation_command_vault",
      "isBraced": false,
      "braceUsed": false,
      "impactHistory": [],
      "warnings": [],
      "activeSalvage": [],
      "revealedSites": []
    },
    "seasonal": {
      "systemId": "seasonal_event_system",
      "activeEvents": [],
      "cooldownKeys": [],
      "cooldownDays": [],
      "resolvedEvents": []
    }
  },
  "LocationEvolution": { "evolutions": [] },
  "Wildlife": { "populations": [] },
  "Landmark": { "landmarks": [] },
  "Checksum": "abc123..."
}
```

---

## 2. Backward Compatibility & Legacy Migration

- Saves missing the `WeatherIntelligence` or `seasonal` object automatically instantiate clean default state containers without throwing exceptions.
- Restoring state never rerolls scheduled weather sequence or pending kinetic strike coordinates.
