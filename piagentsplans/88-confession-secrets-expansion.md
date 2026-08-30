# Plan 88 — Confession Secrets Expansion (8 → 20 survivor confession secrets)

## Goal (2 lines)
Expand `confession_secrets.json` from 8 verified secrets to 20. The confession
system lets survivors reveal dark personal secrets to each other — each secret
has a forgiveness path and a grudge path with affinity and morale consequences.
The system is confirmed live (ContentUtilizationScanner), but 8 secrets is too
few for the full range of survivor archetypes.

## Why (P2)
- Verified: `confession_secrets.json` has 8 entries (archetype_id, secret_title,
  secret_text, forgiveness_outcome, forgiveness_affinity, forgiveness_morale,
  grudge_outcome, grudge_affinity, grudge_morale). The confession system is
  confirmed live.
- Creates the confession-pillar: confessions are the game's deepest
  interpersonal-morality system — each survivor carries a secret that, when
  revealed, creates a forgiveness-or-grudge decision with permanent
  relationship consequences. 8 secrets covers 8 archetypes; 20 covers the full
  survivor roster and adds secrets for compound situations (addiction, theft,
  cowardice, betrayal, complicity, self-harm, despair, hope).
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/confession_secrets.json` (expand 8 → 20 secrets)
- Read-only: grep for the consuming system to confirm how archetype_id resolves
  and how forgiveness/grudge outcomes are applied

## Content grammar (per secret)
- `archetype_id`: references an existing survivor archetype (the_surgeon,
  the_soldier, the_pharmacist, etc.). Some new secrets may introduce new
  archetypes — confirm the archetype exists in the survivor catalog first.
- `secret_title`: 2–5 words evoking the secret's weight ("The Patient They
  Lost", "The Stolen Morphine").
- `secret_text`: 2–4 sentences in the survivor's voice, using `{name}` as a
  placeholder for the survivor's name. Match the existing quality — each
  secret is a specific, human, morally complicated confession.
- `forgiveness_outcome`: 1–3 sentences describing the forgiveness response.
- `forgiveness_affinity`: +5 to +30 (relationship gain).
- `forgiveness_morale`: +5 to +20 (morale gain).
- `grudge_outcome`: 1–3 sentences describing the grudge response.
- `grudge_affinity`: -5 to -40 (relationship loss).
- `grudge_morale`: -5 to -20 (morale loss).
- Trade-off: some secrets have high forgiveness reward but also high grudge
  risk. The player must judge the listener.

## Steps
1. Grep for the consuming system to confirm how archetype_id resolves and how
   forgiveness/grudge outcomes are applied to survivor relationships.
2. Read the existing 8 secrets to confirm the quality bar and the {name}
   placeholder convention.
3. Confirm which survivor archetypes exist (grep survivor catalog for
   archetype ids).
4. Author 12 new secrets:
   - the_engineer: "The Bridge They Didn't Reinforce" (a bridge they certified
     safe collapsed during evacuation).
   - the_nurse: "The Overdose They Missed" (they were too tired to check a
     dosage and a patient died).
   - the_cook: "The Ration They Skimmed" (they took extra food for themselves
     during a shortage).
   - the_farmer: "The Crop They Burned" (they destroyed a harvest to drive up
     prices before the exchange).
   - the_priest: "The Faith They Lost" (they stopped believing but kept
     preaching because people needed hope).
   - the_journalist: "The Story They Killed" (they buried a story that would
     have exposed a pre-war cover-up).
   - the_pilot: "The Flight They Refused" (they refused to fly an evacuation
     mission and someone else died doing it).
   - the_scientist: "The Data They Falsified" (they altered research results
     to secure funding).
   - the_carpenter: "The Shelter They Built Badly" (they cut corners on a
     shelter that failed).
   - the_child: "The Friend They Left" (they ran and left a friend behind
     during the exchange).
   - the_old_man: "The Regret They Carry" (they survived by doing nothing
     while others acted and died).
   - the_hunter: "The Kill They Didn't Need" (they shot someone who was
     surrendering).
5. Each secret: distinct archetype, specific human detail, balanced
   forgiveness/grudge outcomes. No two secrets should have identical
   consequence profiles.
6. Cross-reference: every archetype_id resolves to an existing archetype (or
   add the archetype to the survivor catalog if new); every {name} placeholder
   is used correctly.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: confession catalog loads 20 secrets, all archetype_ids resolve,
   forgiveness/grudge affinity and morale within valid ranges, all secrets
   have non-empty secret_text and both outcomes.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is archetype_id resolution (step 3): confirm
each archetype exists before authoring.

## Definition of Done
- `confession_secrets.json` has 20 secrets, all archetype_ids resolving,
  integrity + tests green.

## Follow-on
- Plan 66 (guilt sources) — confessions trigger guilt.
- Plan 65 (final wishes) — a confessed secret can be a dying survivor's wish.
- Plan 52 (recurring NPC arcs) — confessions deepen NPC relationships.
- Plan 27C (psychological contamination) — grudges feed psychological decline.
- Existing 27A (body and mind) — confessions are the interpersonal-morality
  layer.
