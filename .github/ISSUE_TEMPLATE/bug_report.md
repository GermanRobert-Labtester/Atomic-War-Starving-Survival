---
name: Bug Report
about: Create a structured defect report for ASHFALL simulation, presentation, persistence, or tooling
title: '[BUG] '
labels: bug
assignees: ''
---

## Description
<!-- A clear and concise description of the defect or unexpected behavior. -->

---

## Environment & Version Diagnostics
<!-- Run `godot --headless --path . -- --version` to generate your exact environment report. -->
- **Game / Build Version:** <!-- e.g., 0.1.0 or Git commit hash -->
- **Operating System:** <!-- e.g., Linux x86_64, Windows 11, macOS 14 -->
- **Simulation Seed:** <!-- e.g., 1337 or 42 (crucial for deterministic reproduction) -->
- **Save Schema Version:** <!-- e.g., SchemaVersion from save envelope or --version -->
- **Host Engine:** Godot 4.7+ .NET (Mono)

<details>
<summary>Full CLI `--version` Output (Click to expand)</summary>

```text
<!-- Paste full output of: godot --headless --path . -- --version -->
```

</details>

---

## Steps to Reproduce
1. Boot game or launch headless verb: `...`
2. Navigate to panel or execute action: `...`
3. Advance simulation / day / triage step: `...`
4. Observe failure: `...`

---

## Expected vs. Actual Behavior
- **Expected Behavior:** <!-- Describe what should have occurred per game design or contract -->
- **Actual Behavior:** <!-- Describe what actually occurred -->

---

## Host Self-Test / Verification Output (if applicable)
<!-- If reproducing via CLI or self-test, paste the test runner output below -->
<details>
<summary>Headless Host-Test Output</summary>

```text
<!-- e.g., output of: godot --headless --path . -- --<domain>-selftest -->
```

</details>

---

## Additional Context / Logs
<!-- Include user://logs/godot.log, save files, or visual screenshots if applicable. -->
