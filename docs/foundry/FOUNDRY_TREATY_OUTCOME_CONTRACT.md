# Foundry Treaty Outcome Vocabulary & State Lifecycle Contract

**Runtime Source:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
**C# Enum:** `Ashfall.Core.Foundry.FoundryTreatyOutcome`

---

## 1. Outcome Vocabulary

The runtime code enforces an exact, closed string vocabulary via `SilentFoundryConsequencePolicyCatalog.KnownOutcomes`:

```csharp
public static readonly string[] KnownOutcomes = { "met", "missed", "violated" };
```

| String Value | C# Enum Value | Numeric Value | Description | Carries Consequence? |
|---|---|---|---|---|
| `""` | `FoundryTreatyOutcome.NotRatified` | `0` | Pre-ratification day; neutral baseline. | No |
| `""` | `FoundryTreatyOutcome.Pending` | `1` | Ratified, but initial 30-day assessment cycle unreached. | No |
| `"met"` | `FoundryTreatyOutcome.Met` | `2` | Obligations upheld, quotas met, or peace observed. | **Yes** |
| `"missed"` | `FoundryTreatyOutcome.Missed` | `3` | Quota shortfall, tariff delay, or logistical default without malice. | **Yes** |
| `"violated"` | `FoundryTreatyOutcome.Violated` | `4` | Active breach, unauthorized overtime, strike, pollution, or armed incursion. | **Yes** |

> [!IMPORTANT]
> **Roadmap Vocabulary Note:** While high-level design prose sometimes uses the colloquial synonym `"breached"`, the authoritative runtime enum and string validator in `SilentFoundryConsequencePolicy.cs` strictly accepts `"violated"`. Authoring `"breached"` directly into the JSON triggers `SilentFoundryConsequencePolicyCatalog` load errors. All breach policies are therefore authored with `"violated"`.

---

## 2. Transition & Terminal Semantics

1. **Cycle-Based Evaluation:**
   - Evaluated periodically on assessment days derived from `ratified_day + n * 30`.
   - Each assessment cycle evaluates the conditions for that interval and resolves to `Met`, `Missed`, or `Violated`.

2. **Idempotency Key:**
   - `(treaty_id, cycleMarker)` where `cycleMarker = assessmentDay`.
   - An outcome consequence cannot apply more than once for the same cycle, regardless of save/load, UI reopen, or re-assessment on the same sim day.

3. **Distinction Between Missed and Violated:**
   - `Missed`: A failure of production, delivery, or volume (e.g. 2/4 pipes cast, fuel payment delayed). Penalties are bounded (`-5.0` to `-6.0`) and focus on economic friction and temporary access gating.
   - `Violated`: A breach of fundamental conduct rules (e.g. strike/overtime on charging floor, toxic bilge dumping into aquifer, armed skirmish in neutral buffer). Penalties are severe (`-8.0` to `-15.0`) and provoke route closures, embargoes, and potential faction war escalation.
