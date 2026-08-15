# ASHFALL / Atomic War — CI & Unity secrets

**Authoritative editor:** `6000.5.5f1` (see `ProjectSettings/ProjectVersion.txt`)  
**Workflows:** `.github/workflows/ci.yml` (primary gate), `.github/workflows/build.yml` (multi-platform builds)

---

## Required GitHub repository secrets

game-ci (`unity-test-runner@v4`, `unity-builder@v4`) needs a valid Unity license. Configure these under **Settings → Secrets and variables → Actions**:

| Secret | Purpose | How to obtain |
|--------|---------|----------------|
| `UNITY_LICENSE` | Full contents of a Unity license activation file (`.ulf`) | Activate a seat (Personal/Plus/Pro) on a machine, then copy the license file body. For CI, game-ci docs recommend the [manual activation](https://game.ci/docs/github/activation) flow: run their activation request, upload the request to license.unity3d.com, download the `.ulf`, paste **entire file** into the secret. |
| `UNITY_EMAIL` | Unity account email used for that license | Same account that owns the seat |
| `UNITY_PASSWORD` | Unity account password (or password-like secret for that account) | Prefer a dedicated CI account; rotate if leaked |

`GITHUB_TOKEN` is provided automatically by Actions; no manual secret is required for test result check runs.

### Optional / not used yet

| Name | Notes |
|------|--------|
| `UNITY_SERIAL` | Alternate to `UNITY_LICENSE` for serial-based Pro/Enterprise activation (not wired in current workflows) |

---

## Local verification (no secrets)

```bash
# Editor path on this machine
UNITY="$HOME/Unity/Hub/Editor/6000.5.5f1/Editor/Unity"
PROJECT="."

# Compile + EditMode smoke (example filter)
"$UNITY" -batchmode -nographics -projectPath "$PROJECT" \
  -runTests -testPlatform EditMode \
  -testFilter "AtomicWar.Tests.EditMode.AuditP0SaveAndFoundationTests" \
  -testResults test-results-local.xml \
  -logFile test-log-local.txt
```

Data-only gate (no Unity license):

```bash
# Same syntax check as ci.yml "validate" job
python3 -c "
import json, pathlib, sys
root = pathlib.Path('Assets/StreamingAssets')
errs = []
for p in root.rglob('*.json'):
    try: json.loads(p.read_text(encoding='utf-8'))
    except Exception as e: errs.append(f'{p}: {e}')
sys.exit(1 if errs else 0)
"
grep -q 'm_EditorVersion: 6000.5.5f1' ProjectSettings/ProjectVersion.txt
```

---

## Fail-fast save restore in CI

`SaveSystem.DefaultFailFastRestoreForEnvironment()` returns **true** when compiled under `UNITY_EDITOR` or `DEVELOPMENT_BUILD`.  
`GameBootstrap` applies that to `SaveSystem.FailFastRestore` after construction.

| Context | FailFastRestore |
|---------|-----------------|
| Unity Editor / EditMode tests / game-ci | **true** (all-or-nothing ISaveable restore) |
| Development player build | **true** |
| Release player build | **false** (best-effort; log and continue) |

Override any instance with `saveSystem.FailFastRestore = …` after construction.

---

## Package pin notes (AUDIT-006)

- Keep **`com.unity.modules.physicscore2d`** — required by `com.unity.modules.physics2d` and present as `ProjectSettings/PhysicsCoreProjectSettings2D.asset`.
- Do **not** open this project on Unity **6000.3.x**; module resolution fails there. Use **6000.5.5f1** only (CI and local).
- `com.unity.multiplayer.center` was removed: this is a single-player game and nothing referenced it.

---

## Git LFS is not in use (read before touching `.gitattributes`)

`.gitattributes` is the upstream Unity template, which marks most binary types
as `lfs`. **This project never adopted LFS** — `git lfs ls-files` reports zero
objects — and the only tracked files matching any `lfs` pattern are the six
radio `.wav` clips, committed as ordinary blobs.

That mismatch was a live data-loss trap: the LFS clean filter turned each real
`.wav` into a 130-byte pointer, so every fresh clone reported all six as
modified, and any `git add -A` would have replaced the audio with pointers whose
LFS objects exist nowhere. The audio extensions are now pinned back to plain
binary at the bottom of `.gitattributes`.

If you ever want LFS for real, remove that override **and** run `git lfs migrate`
against a remote with LFS enabled — do not do one without the other.

---

## Integration branch (AUDIT-002)

Long-running dirty work is staged on:

```text
integration/audit-p1
```

Prefer PRs from that branch (or stacked feature branches cut from it) rather than force-pushing `main`. See `docs/audit/ISSUE_REGISTER.md` AUDIT-002.

---

## Setting secrets from a local machine (operator)

GitHub repo currently has **zero** Actions secrets (`gh secret list` empty).  
game-ci EditMode fails immediately with:

```text
Missing Unity License File and no Serial was found
```

### Option A — Personal license (`.ulf`) via game-ci activation

1. Follow https://game.ci/docs/github/activation
2. Produce a license file, then:

```bash
# From a machine that has the .ulf contents (do NOT commit the file)
gh secret set UNITY_LICENSE < Unity_v6000.x.ulf
gh secret set UNITY_EMAIL -b 'you@example.com'
gh secret set UNITY_PASSWORD -b 'your-unity-password'
```

3. Re-run failed workflows on the PR:

```bash
gh workflow run "ASHFALL CI" --ref integration/audit-p1
# or:
gh run rerun <run-id> --failed
```

### Option B — Serial-based Pro/Enterprise

```bash
gh secret set UNITY_SERIAL -b 'XXXX-XXXX-XXXX-XXXX-XXXX'
gh secret set UNITY_EMAIL -b 'you@example.com'
gh secret set UNITY_PASSWORD -b 'your-unity-password'
```

(Requires workflow to also pass serial — current YAML uses license env vars; add `UNITY_SERIAL` to `env:` if using this path.)

### What already passes without secrets

| Job | Needs license? | Status on PR #1 |
|-----|----------------|-----------------|
| Data Validation Gate (JSON + ProjectVersion pin) | No | **PASS** |
| EditMode Tests (game-ci) | **Yes** | **FAIL** (empty secrets) |
| Build Linux64 | Yes | skipped when tests fail |

### Local trust signal (this machine)

Full EditMode on Unity **6000.5.5f1** (no GitHub secrets needed):

```bash
Unity -batchmode -nographics -projectPath . -runTests -testPlatform EditMode \
  -testResults test-results-ci-local-editmode.xml -logFile test-log-ci-local-editmode.txt
```

Last local run: EditMode **1037 passed / 0 failed**, PlayMode **61 passed / 0 failed**
(2026-08-05, audit pass).

**Run PlayMode too.** It is a separate `-testPlatform` and is easy to forget; it
was red at 59/61 for an unknown period while EditMode was green, because nothing
in this file told anyone to run it:

```bash
Unity -batchmode -nographics -projectPath . -runTests -testPlatform PlayMode \
  -testResults test-results-playmode.xml -logFile test-log-playmode.txt
```

Additionally verified from a **cold clone** on the same date — `git clone` of
`integration/audit-p1` into an empty directory, no `Library/`, full asset
reimport — which also produced **1035 passed / 0 failed, 0 compile errors**
(the count predates the two save-hardening tests added afterwards). This is
the check that actually proves a fresh checkout builds; running the suite in
a working tree that already has a populated `Library/` does not.

> **Keep this number current.** It was previously recorded as `780 passed / 0 failed`, which had gone
> stale: roughly 250 tests had been added since that run, and the suite was actually **red** (2
> failures) while this line still read green. A stale pass count is worse than no pass count — it is
> a false trust signal. Re-run the full command above and update this line whenever you rely on it.

## Godot host (active engine) — cold-build trust signal

Unity is the legacy codebase (see AGENTS.md); the active verification surface is
Godot + dotnet. Run from a cold state so the check proves a fresh checkout builds:

```bash
rm -rf .godot/mono/temp Ashfall.Core/bin Ashfall.Core/obj
dotnet build Ashfall.csproj                 # expect: 0 errors, 0 warnings
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # expect: 732 passed / 0 failed
godot --headless --path . -- --data-integrity-selftest     # expect: PASS, 0 errors
godot --headless --path . -- --expansions-selftest         # expect: 236/236 GREEN (+ verdict chain)
godot --headless --path . -- --verdict-selftest            # expect: VERDICT_SELFTEST PASS
godot --headless --path . -- --muster-selftest             # expect: PASS 25/25
godot --headless --path . -- --dose-ledger-selftest        # expect: PASS
godot --headless --path . -- --muster-uitest --dose-uitest # expect: both PASS (run separately)
```

Last local cold-build run: build 0 errors / 0 warnings, **408 passed / 0 failed**,
data gate 0 errors across 59 catalogs, expansions 236/236, all 18 selftests PASS
(2026-08-14, Loop 7 + Nobody's Charter Phase 6 closing slice).
