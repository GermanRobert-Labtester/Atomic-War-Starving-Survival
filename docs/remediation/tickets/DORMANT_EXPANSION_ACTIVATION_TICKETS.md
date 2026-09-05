# Designed-Dormant Expansion Activation Tickets

**Audit:** local post-PR #36 issue **#50**
**Date:** 2026-09-05
**Gate:** `LoaderWiringGateTests` allowlist — entries must not sit forever without tickets.

These loaders are intentionally unwired in production until their expansion /
plan is activated. Removing an allowlist entry requires a production
`Load` / `LoadAndRegister` call site (or deleting the loader).

| Ticket | Loader | Expansion / Plan | Activation condition | Owner lane | Expiry review |
|---|---|---|---|---|---|
| **DX-01** | `SkyLayerArmorCatalogLoader` | Expansion 11 — Orbital Harrow | Expansion 11 flagged live in campaign bootstrap + sky-layer armor UI bound | Shelter / sky defense | 2026-Q4 |
| **DX-02** | `SpiritualCatalogLoader` | Plan 30 — spiritual-meaning coordinator | Plan 30 coordinator session constructed from `Setup*` and consumed by a player surface | Narrative / meaning | 2026-Q4 |
| **DX-03** | `HoldfastNpcCatalogLoader` | Holdfast NPC definitions | Holdfast quest-loop integration consumes NPC catalog (not CLI-only) | Holdfast expansion | 2026-Q4 |

## Rules

1. Do **not** delete allowlist dispositions to “green” `LoaderWiringGateTests`.
2. Activation PRs must: wire the loader, prune the allowlist entry, add a journey or selftest pin, and close the matching DX ticket here.
3. If an expansion is cancelled, delete the loader + catalog + allowlist row in the same PR.
4. Quarterly review: any ticket past Expiry without progress gets either a new expiry with justification or cancellation.
