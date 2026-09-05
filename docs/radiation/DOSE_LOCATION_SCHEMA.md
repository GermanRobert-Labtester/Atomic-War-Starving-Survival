# Dose Location Schema Contract

> **Authority:** `Assets/StreamingAssets/Data/dose_locations.json`
> **C# DTO:** `DoseLocationDef` (`Assets/Ashfall.Core/DoseContentCatalog.cs`)
> **Container Wrapper:** `DoseLocationsRoot` (`{"schema_version": 1, "locations": [...]}`)

---

## 1. DTO Specification

```csharp
[Serializable]
public class DoseLocationDef
{
    public string id = string.Empty;
    public string displayName = string.Empty;
    public string sector = string.Empty;
    public int riskLevel;
    public float radiationUsv;
    public string description = string.Empty;
}
```

---

## 2. Field Definitions and Constraints

| Field | Type | Required | Valid Range / Format | Semantic Role |
|---|---|:---:|---|---|
| `id` | `string` | **Yes** | `loc_[a-z0-9_]+` | Unique identifier; registered in `CatalogIntegrityValidator` definition symbol table. |
| `displayName` | `string` | **Yes** | Non-empty string | Human-readable title rendered in Dose Ledger UI surfaces. |
| `sector` | `string` | **Yes** | `bunker` \| `surface` \| `expedition` \| `external` \| `faction` | Environmental category classifying the geographic exposure regime. |
| `riskLevel` | `int` | **Yes** | `0` to `8` | Authoring and visual risk rating abstraction (0 = completely shielded, 8 = lethal hot zone). |
| `radiationUsv` | `float` | **Yes** | `0.01` to `80.0` | Numerical baseline ambient exposure rate expressed in microsieverts per hour (µSv/h). |
| `description` | `string` | **Yes** | 1–3 grounded sentences | Diegetic environmental narrative explaining physical causes of radiological elevation. |

---

## 3. Container JSON Format

```json
{
  "schema_version": 1,
  "locations": [
    {
      "id": "loc_the_dose_room",
      "displayName": "Room Six, the Ledger Table",
      "sector": "bunker",
      "riskLevel": 0,
      "radiationUsv": 0.02,
      "description": "A bolted-down table, four chairs, a fan that turns by hand. One chair keeps the red pencil. The papers on the table are the reason anyone descends the corridor at all."
    }
  ]
}
```
