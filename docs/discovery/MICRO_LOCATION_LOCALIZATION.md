# Micro-Location Localization Specification & Readiness Audit

**Subsystem:** Localization & Diegetic Text
**Authority:** `Assets/Ashfall.Core/Localization/LocalizationService.cs`
**CSV Catalog:** `assets/l10n/strings.csv`
**Data Catalog:** `Assets/StreamingAssets/Data/micro_locations.json`
**Tests:** `Ashfall.Core.Tests/Localization/MicroLocationLocalizationTests.cs`
**Status:** Certified / Production Ready

---

## 1. Executive Summary

All 28 micro-locations (including the 25 production core entries and 3 expansion entries) are fully registered in the ASHFALL localization infrastructure. Title, description, and every authored choice adhere to deterministic, locale-invariant key naming while keeping underlying gameplay state (item IDs, quantities, flags, location IDs, danger thresholds, and probabilities) 100% independent of the active language.

---

## 2. Key Formula & Catalog Structure

Every string follows the canonical scheme:
- **Title:** `discovery.{encounter_id}.title`
- **Description:** `discovery.{encounter_id}.description`
- **Choice:** `discovery.{encounter_id}.choice.{choice_id}`

### Total Inventory
- **Micro-Locations:** 28 encounters
- **Registered Keys:** 135 strings across `strings.csv` and `LocalizationService` defaults.
- **Languages:**
  - `en`: 100% complete source baseline.
  - `de`: Exemplar translations fully populated (`micro_roadside_memorial`, `micro_crashed_truck`, `micro_observation_post`, `micro_frozen_bus`). Non-exemplar keys cleanly fall back to source English without missing-key warnings or empty labels.
  - `pseudo`: Stress-testing locale with ~30-40% string expansion and accent substitutions to verify UI container wrap resistance.

---

## 3. Exemplar Translations (German)

### 3.1 Roadside Memorial (`micro_roadside_memorial`)
- `discovery.micro_roadside_memorial.title` -> `"Straßenrand-Gedenkstätte"`
- `discovery.micro_roadside_memorial.description` -> `"Geschmolzene Talgreste sitzen in verrosteten Rationsdosen um einen verbogenen Pfahl. Ein Foto wurde weggerissen, nur eine blutbefleckte Ecke blieb unter einem Stein zurück. Das Wachs ist zu blassen, grauen Scheiben erstarrt."`
- `discovery.micro_roadside_memorial.choice.leave_memorial` -> `"Unberührt lassen."`
- `discovery.micro_roadside_memorial.choice.take_offering` -> `"Die Kerzenreste und alle kleinen Opfergaben mitnehmen."`

### 3.2 Crashed Supply Truck (`micro_crashed_truck`)
- `discovery.micro_crashed_truck.title` -> `"Abgestürzter Versorgungslaster"`
- `discovery.micro_crashed_truck.description` -> `"Ein militärischer Logistiktransporter liegt zerschmettert im gefrorenen Graben, die Windschutzscheibe nach außen geborsten. Zerrissene Strahlungswarnschilder klammern sich an die verbogenen Hecktüren. Der Laderaum wurde längst geplündert, aber eine Kiste liegt aufgebrochen im Frost."`
- `discovery.micro_crashed_truck.choice.search_truck_cargo` -> `"Die aufgebrochene Kiste und das Fahrerhaus nach Brauchbarem durchsuchen."`
- `discovery.micro_crashed_truck.choice.search_truck_cab` -> `"Das Fahrerhaus nach Dokumenten oder persönlichen Gegenständen untersuchen."`
- `discovery.micro_crashed_truck.choice.ignore_truck` -> `"Weitergehen. Jemand hat bereits alles Brauchbare mitgenommen."`

### 3.3 Military Observation Post (`micro_observation_post`)
- `discovery.micro_observation_post.title` -> `"Militärischer Beobachtungsposten"`
- `discovery.micro_observation_post.description` -> `"Eine schwere Optikhalterung weist noch immer lautlos auf die verlassene Kreuzung hinunter. Hektische Gitterkoordinaten und Abschusszahlen sind an die Betonwand gekritzelt und brechen mitten im Satz ab. Eine einzelne abgefeuerte Scharfschützenhülse liegt auf der gefrorenen Fensterbank."`
- `discovery.micro_observation_post.choice.search_observation_post` -> `"Den Posten nach Optiken oder Aufklärungsdokumenten durchsuchen."`
- `discovery.micro_observation_post.choice.read_grid_references` -> `"Die Gitterkoordinaten und Daten von der Wand abschreiben."`
- `discovery.micro_observation_post.choice.ignore_observation_post` -> `"Der Posten wurde geplündert. Weitergehen."`

### 3.4 Frozen Evacuation Bus (`micro_frozen_bus`)
- `discovery.micro_frozen_bus.title` -> `"Gefrorener Evakuierungsbus"`
- `discovery.micro_frozen_bus.description` -> `"Die Bustüren sind weit aufgefroren, sodass der aschebeladene Wind durch die Kabine heult. Der Einzelschuh eines Kindes steht aufrecht unter einer Gepäckablage. Die Fenster sind auf der Innenseite von dickem, schmierigem Frost überzogen."`
- `discovery.micro_frozen_bus.choice.search_bus_luggage` -> `"Die Gepäckablage nach Vorräten durchsuchen."`
- `discovery.micro_frozen_bus.choice.leave_bus` -> `"Den Bus unberührt lassen."`
- `discovery.micro_frozen_bus.choice.read_bus_tag` -> `"Die Transitmarke auf dem Armaturenbrett nach einem Bestimmungsort prüfen."`

---

## 4. Gameplay Invariance Contract

Under Invariant 1 and 4, changing the display language or running under pseudo-localization alters **only presentation strings**. The simulation state engine guarantees:
1. `EncounterDefinition.id` remains invariant.
2. `choice.choiceId` remains invariant.
3. `choice.grantItemId` is never localized (e.g., `cloth`, `canned_food`, `sealed_government_document`).
4. Numerical deltas (`moraleDelta`, `guiltDelta`, `factionStandingDelta`, `grantItemQuantity`) are invariant.
5. Location IDs (`rural_gas_station`, `government_bunker`) and journal clue keys (`micro_observation_post_grid`, etc.) remain identical across all locales.

Verified deterministically by `Ashfall.Core.Tests/Localization/MicroLocationLocalizationTests.cs`.
