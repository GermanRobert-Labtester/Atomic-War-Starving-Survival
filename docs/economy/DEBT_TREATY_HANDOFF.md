# Plan 40 — Treaty Handoff

## Treaty Integration
- `conseq_treaty_breach` fires `OnStandingPenalty` with -25 delta
- Only treaty-backed debt can produce treaty breach
- Ordinary credit defaults do NOT trigger treaty violation

## Treaty-Backed Templates
Currently none of the15 templates are explicitly treaty-backed. This is by design — treaty-backed debt should be added in a future expansion when the treaty system has specific debt-related treaty effects.

## Future Extension
To add treaty-backed debt:
1. Create a template with `consequenceId: conseq_treaty_breach`
2. Ensure the creditor faction has an active treaty
3. Default triggers treaty violation through `RegionalTreatySystem`
