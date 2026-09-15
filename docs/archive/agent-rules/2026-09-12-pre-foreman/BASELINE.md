# Rulebook Context Baseline

The snapshot contains 14 root-level rulebooks captured immediately before the
foreman rollout.

| Measure | Before | Active after rollout | Reduction |
|---|---:|---:|---:|
| Total lines | 13,798 | 2,045 | 11,753 (85.2%) |
| Total bytes | 1,223,597 | 107,687 | 1,115,910 (91.2%) |
| Canonical `AGENTS.md` lines | 1,066 | 147 | 919 (86.2%) |
| Canonical `AGENTS.md` bytes | 95,667 | 7,820 | 87,847 (91.8%) |

The active files retain universal safety and architecture rules and point to
small concern-specific documents. The archived files preserve the complete
pre-rollout text and hashes; they can be consulted when historical detail is
needed without consuming every agent's initial context.
