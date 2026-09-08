# Feedback Confirmation Contract

## 1. Scope & Objective

Confirmation templates in `feedback_messages.json` (category `confirmation`) exist to prevent irreversible player errors during high-consequence actions.

---

## 2. Invariants

1. **Domain Ownership of Legality:**
   - The confirmation dialog never determines whether an action is legal. The domain authority (e.g. `SurvivorsHostSession`, `HoldfastQuestSystem`) decides eligibility before the confirmation dialog is displayed.
2. **Single Invocation on Confirm:**
   - Confirming invokes the pre-validated command callback exactly once. Double-clicking or rapid key presses must be locked after the first submit.
3. **Zero Mutation on Cancel:**
   - Dismissing or cancelling the dialog performs zero state changes or domain mutations.
4. **State Invalidation Safety:**
   - If underlying domain state changes while the dialog is open (e.g. the survivor dies or resources change before confirmation is clicked), the dialog re-validates before executing, or aborts safely.
5. **Keyboard & Focus Handling:**
   - Focus is pushed onto the dialog (defaulting to the Cancel/Dismiss button for safety).
   - Upon dismissal, focus is restored to the initiating control via `ModalManager`.

---

## 3. Implemented Confirmation Flows

1. **Exile Survivor (`delete_survivor`):**
   - Message: `"Are you sure you want to exile {0}? This cannot be undone."`
   - Command: `SurvivorsHostSession.Exile(survivorId)`.
2. **Abandon Quest (`abandon_quest`):**
   - Message: `"Are you sure you want to abandon this quest? Progress will be lost."`
   - Command: `HoldfastQuestSystem.AbandonQuest(questId)`.
3. **Administer Rare Medicine (`use_medicine`):**
   - Message: `"Are you sure you want to use {0} medicine on {1}? This cannot be undone."`
   - Command: `MedicalHostSession.TreatPatient(patientId, medicineId)`.
