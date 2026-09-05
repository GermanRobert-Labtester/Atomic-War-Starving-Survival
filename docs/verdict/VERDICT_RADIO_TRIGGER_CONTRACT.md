# Verdict Radio Trigger Contract

> **Runtime Implementation:** `VerdictRadioSystem.Poll(int day, ReckoningPhase phase)`

---

## 1. Trigger Availability Logic

```csharp
public List<string> Poll(int day, ReckoningPhase phase)
{
    var fired = new List<string>();
    if (phase < ReckoningPhase.Culpable) return fired;
    if (day < CarrierOpenDay) return fired; // Day 210

    for (int i = 0; i < _corpus.Count; i++)
    {
        var e = _corpus[i];
        if (e == null || _firedIds.Contains(e.id)) continue;
        if (day < e.dayTrigger) continue;
        _firedIds.Add(e.id);
        if (_bus != null)
            _bus.Publish("radio.verdict.broadcast", e);
        fired.Add(e.id);
    }
    return fired;
}
```

## 2. Key Operational Semantics

1. **Carrier Window Requirement:**
   `CarrierOpenDay = 210`. Regardless of `dayTrigger`, no broadcast can fire before Day 210.
2. **Phase Prerequisite:**
   The Reckoning phase must have reached at least `ReckoningPhase.Culpable`. During `Dormant` or `Knowing`, radio stays silent.
3. **Trigger Window:**
   Availability is evaluated as `day >= e.dayTrigger`. Broadcasts become eligible **on** the trigger day and remain eligible on all subsequent days until intercepted.
4. **Missed-Day Resilience:**
   If the player advances multiple days without tuning in, all broadcasts whose `dayTrigger <= currentDay` will fire in deterministic catalog order upon the next poll.
5. **One-Shot Guard:**
   Once `_firedIds.Add(e.id)` is executed, the broadcast will never fire again in the campaign.
