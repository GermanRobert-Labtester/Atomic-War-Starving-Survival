# Final Wish Consequence Matrix

**Document:** `docs/survivors/FINAL_WISH_CONSEQUENCE_MATRIX.md`

---

## 1. Consequence Dispatch Architecture

`FinalWishSystem` executes consequences deterministically via C# events and host callbacks:

```csharp
private void CompleteWish(string survivorId, FinalWishSurvivorState state)
{
    state.wishCompleted = true;
    state.isActive = false;
    ApplyPermanentShelterMoraleBuff?.Invoke(WishCompletedMoraleBuff);
    OnFinalWishCompleted?.Invoke(survivorId);
    OnPermanentMoraleBuffApplied?.Invoke(WishCompletedMoraleBuff);
    OnStateChanged?.Invoke();
}
```

---

## 2. Supported Consequence Channels

| Consequence Channel | Invocation Point | Mechanical Delta | System Receiver |
|---|---|---|---|
| **Shelter Morale (Success)** | `CompleteWish` | `+15.0f` permanent buff | `HoldfastRuntimeSession` / Morale ledger |
| **Shelter Morale (Expired)** | `OnPrognosisExpired` | `-10.0f` permanent penalty | `HoldfastRuntimeSession` / Morale ledger |
| **Narrative Event** | `OnFinalWishCompleted` | Fires `"narrative_final_wish_completed"` | `NarrativeEventBus` / Chronicle |
| **Survivor Effects Recalculation** | `OnFinalWishCompleted` | Updates survivor status to completed | `Phase0HostSession.RecomputeSurvivorEffects` |
| **Skill Knowledge Transfer** | Step metadata (`skill_transfer`) | Grants apprentice skill unlocked | `SurvivorSkillsSystem` |
| **NPC Arc Advancement** | Step metadata (`requires_npc`) | Updates NPC relationship / flag | `NpcArcHostSession` |

---

## 3. Anti-Exploitation & Atomicity Rules

1. **One-Shot Execution:** `CompleteWish` sets `state.wishCompleted = true` and `state.isActive = false`. Any subsequent calls to `AdvanceWishStep` return `false`.
2. **Prognosis Expiry Protection:** If `state.wishCompleted` is already true, `OnPrognosisExpired` early-exits without applying the `-10` penalty.
3. **No Morale Farming:** Because a survivor can only receive one terminal prognosis in their lifetime, wishes cannot be cycled or farmed.
