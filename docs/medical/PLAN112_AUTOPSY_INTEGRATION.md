# Plan 112 autopsy integration

Autopsy is a real exposure producer, but its current host adapter routes
pathogen findings to the single authored `disease_zoonotic_flu` exposure
contract. It does not have a disease-id result field or a catalog-driven
autopsy mapping.

The four new diseases are not attached to autopsy findings. This preserves the
existing keyword route and avoids pretending that an autopsy can identify
hepatitis, meningococcal disease, dysentery, or spore-wound dermatitis when
the current case DTO does not carry that diagnosis.

## Required future seam

Plan 79 follow-up should add a typed, validated disease finding ID or an
explicit catalog mapping owned by the autopsy authority. That change must
prove:

1. finding-to-disease IDs resolve;
2. the host source contract authorizes each mapping;
3. the exposure roll remains seeded and one-shot;
4. treatment and save behavior remain owned by `DiseaseSystem`.

Until then, all four additions remain available through direct disease
exposure and the medical ward without an invented autopsy bridge.
