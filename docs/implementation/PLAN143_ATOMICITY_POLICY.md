# Plan 143 atomicity policy

Choice execution has a preflight barrier. The system validates the event,
choice, stage, branch, survivor, morale target, canonical faction, journal
authority, and expedition location before invoking any downstream mutation.

The commit order is deterministic:

1. calculate the bounded arc-state transition;
2. apply the already-preflighted morale mutation;
3. apply faction-intel knowledge discovery;
4. record the expedition offer token;
5. apply canonical faction standing;
6. publish the committed choice, journal, and feedback notifications.

No commit callback is allowed to reject after its matching preflight has
passed. If any preflight fails, no callback is invoked, no completion or branch
is stored, and no downstream state changes. This is the smallest transaction
boundary compatible with the existing host systems; it does not introduce a
generic transaction engine. The arc state is assigned only after the
preflighted downstream commits complete.

Duplicate commits are idempotent: a completed event returns an already-resolved
result and does not invoke any adapter twice. Loading never calls commit.
