# Medical pipeline journey

The current journey is coordinated by `MedicalPipelineCoordinator`:

1. domain handler exposes a plausible condition;
2. evidence can move diagnosis from unknown to suspected;
3. explicit diagnosis moves it to confirmed where required;
4. treatment preview validates patient, handler, diagnosis, capability, and
   inventory availability;
5. immediate treatments reserve, consume atomically, apply through the domain,
   and release the consumed claim;
6. scheduled treatments reserve first and create a
   `MedicalProcedureSchedule` row;
7. the medical day owner advances the row by campaign hours;
8. completion revalidates patient availability, consumes reserved medicine,
   applies the handler, releases claims, and emits completion;
9. cancellation moves the row to history and releases unconsumed claims;
10. pipeline, reservation, diagnosis, and schedule state round-trip through
    `MedicalPipelineSaveState`.

The medical panel now projects active schedule rows with patient, treatment,
remaining hours, reserved item display names, current status, and a typed
cancel action. It does not invent medic assignment because the current Core
schedule does not persist one; availability is revalidated by the existing
patient lifecycle authority at completion.

Oxygen support is the first authored 24-hour treatment in the catalog, so the
schedule projection exercises a production treatment rather than a UI-only
fixture. Radiation and other immediate treatments retain their domain handler
paths and item transactions.
