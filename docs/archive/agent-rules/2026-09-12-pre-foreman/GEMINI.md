# Antigravity Agent Rules — ASHFALL Project

These rules are **always active** for every Antigravity session in this workspace.
They exist to prevent token waste, runaway context consumption, and hallucinated
"all green" reports. Read them before taking any action.

---

## RULE 1 — NEVER POLL BACKGROUND TASKS

**The single most expensive mistake possible.** Each `manage_task status` call
re-sends the full context window (~35–50k tokens). Polling 20 times = ~1M tokens
burned with zero productive work.

### Forbidden pattern
```
launch task → manage_task status → RUNNING → manage_task status → RUNNING → ...
```

### Required pattern
```
launch task → do parallel useful work OR stop calling tools
             ↳ system will automatically notify you when the task finishes
```

**Rules:**
- Call `manage_task status` at most **ONCE** after launch to confirm the task started.
- After that single check, **stop** and wait. Do not loop. Do not poll again.
- If you need to do other work while waiting, do it — but never in a polling loop.
- The system sends a wakeup message automatically on completion. Trust it.

---

## RULE 2 — NEVER CLAIM "ALL GREEN" WITHOUT EVIDENCE

Do not report a pass, fix, or "all risks closed" without verified evidence:
- A test result you actually read (not assumed).
- A command you actually ran and whose output you actually checked.
- A file you actually read (not recalled from context that may be stale).

If the evidence is still pending (e.g., a test is still running), say so explicitly.
Never synthesize a confident "PASS" from a prior partial run or assumption.

---

## RULE 3 — PARALLEL WORK, NOT SEQUENTIAL POLLING

When waiting for a long-running task (dotnet test, godot --headless), use that
time productively:
- Run independent verification steps in parallel (e.g., scene-lint while tests run).
- Audit related files while the build completes.
- Do NOT sit idle polling a status endpoint.

If there is genuinely nothing to do in parallel, stop calling tools. The system
will wake you when there is new information.

---

## RULE 4 — READ BEFORE WRITING

Before editing any file in `Assets/Ashfall.Core/`, `src/`, or
`Ashfall.Core.Tests/`:
1. Read the current file state (it may have changed since context was built).
2. Confirm the exact line range you intend to edit.
3. Make the smallest possible diff — never rewrite whole files unless required.

---

## RULE 5 — VERIFICATION MATRIX FOR THIS PROJECT

After any code change, run this matrix. Launch Godot self-tests in parallel while
dotnet test runs. Do NOT poll dotnet test — wait for its completion notification.

| Command | Must exit | Must show |
|---|---|---|
| `dotnet test Ashfall.Core.Tests` | 0 | 0 failed |
| `godot --headless --path . -- --content-utilization-selftest` | 0 | CI gate PASS |
| `godot --headless --path . -- --data-integrity-selftest` | 0 | 0 errors |
| `godot --headless --path . -- --scene-binding-selftest` | 0 | 22/22 passed |
| `python3 scripts/ci/scene-lint.py` | 0 | 0 errors |

---

## RULE 6 — QUOTA HYGIENE

Context is expensive. Minimize re-invocations of large-context tool calls:
- Never call `manage_task status` more than once per wait cycle.
- Prefer `grep_search` + `view_file` (targeted) over reading entire large files.
- Batch independent file reads into the same tool step when possible.
- Do not re-summarize artifact contents back to the user — point to the artifact instead.

---

## RULE 7 — COMMAND TIMEOUT DISCIPLINE

**Default timeout for every task/command is 180 seconds.** This applies to:
- `godot --headless` self-tests
- `dotnet test` / `dotnet build`
- `python3` scripts
- Any shell command run as a background task

### Escalation ladder

```
First run   → 180s timeout
Failed? (timeout, not error) → wait, retry once more at 180s
Still timing out after 2x? → increment by 20s → 200s
Still timing out after 2x at 200s? → increment again → 220s
... and so on, +20s per pair of failures
```

**Rules:**
- **Never** jump straight to a high timeout because you expect the command to be slow.
- **Never** increase the timeout after a single timeout failure — retry at the same limit first.
- A command must fail the **same timeout threshold twice** before you earn the right to add 20s.
- Document the escalation in your response when you increase a timeout so the user knows why.
- If a command exceeds 300s even after escalation, stop and report it as a potential hang — do not keep increasing blindly.

### Example escalation trace (correct)
```
Attempt 1 @ 180s → timed out
Attempt 2 @ 180s → timed out again  ← now allowed to escalate
Attempt 3 @ 200s → timed out
Attempt 4 @ 200s → timed out again  ← now allowed to escalate
Attempt 5 @ 220s → completed ✓
```
