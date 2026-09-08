# Starting Cohort Narrative Compatibility

The flagship profiles avoid definitions with active survivor questlines.
This prevents a starting member from being simultaneously required as a
later discovery target.

Compatibility checks:

- no selected member has an active questline in the current survivor catalog;
- no selected member is a faction leader, captive, child, named expansion
  arrival, or dead/missing authored character;
- no profile changes recruitment flags, quest state, journal state, faction
  standing, or epilogue eligibility;
- memorial/final-wish/confession systems continue to consume actual campaign
  roster state after initialization;
- canonical survivor biographies and display metadata remain authoritative.

If a future profile needs a narrative-special survivor, it must first add a
narrow already-recruited condition to that survivor's owning quest and add a
dedicated compatibility test. Plan 138 does not take that path.
