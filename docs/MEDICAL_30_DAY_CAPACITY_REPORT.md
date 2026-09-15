# Medical 30-day capacity report

`MedicalPipelinePhase2Tests.ThirtyDayScheduledTreatmentWorkload_IsDeterministicAndConservesOxygen`
uses the real respiratory domain, the real coordinator, reservation ledger,
24-hour schedule, and inventory.

- workload: one oxygen-support procedure per day for 30 days;
- starting respiratory degradation: `20`;
- controlled daily exposure: `24` storm hours through the respiratory owner;
- oxygen supply: `30`;
- completed procedures: `30`;
- active procedures at day 30: `0`;
- oxygen remaining: `0`;
- reserved oxygen after each completion: `0`;
- repeated identical run: same completion count, final degradation, and
  pipeline checksum — PASS.

This is a capacity/conservation characterization, not a claim that all medical
workloads are balanced. It proves that the scheduled path can sustain a
defined tense workload without reservation leakage or duplicate execution.
The same suite also proves that a bound, incomplete chelation capability
returns `research_required` without consuming `rad_away`.
