# Year of Ash Echo Handoff

No verified Plan 109 source-history adapter accepting Year of Ash questline IDs was found in the
current runtime path. Plan 114 does not force Year of Ash IDs into an echo `triggered_by` namespace.

The seven quests retain stable IDs and terminal history so a later echo adapter can consume them
through a real cross-system contract. Until that adapter exists, the echo handoff is explicitly
deferred rather than represented by duplicate flags.
