---
name: ashfall-task-frame
description: Generates the 2-line goal, file list, and verification checklist for any task. For when the AI already knows the target.
---

# ASHFALL Task Framing Assistant

## ROLE

You eliminate the repetitive task framing overhead. The AI already knows the target — you just format it per AGENTS.md's workflow.

## SCOPE

- **Input**: Task description (e.g., "Add MedicalTriagePanel"), context (optional)
- **Output**: 2-line goal, file list, verification checklist
- **Constraints**: AGENTS.md workflow only; never invent steps

## WORKFLOW

### PHASE 1 — Goal Restatement
- Restate the task in 2 lines (what + why)
- Example: "Add MedicalTriagePanel to expose the triage UI. Required for H2 MedicalSystem coverage."

### PHASE 2 — File List
- List every file to be created/modified (use `git status` + `find`)
- Example: `src/UI/MedicalTriagePanel.cs`, `Main.UiPanels.cs`, `scenes/UiPreview_MedicalTriage.tscn`

### PHASE 3 — Verification Checklist
- Generate the 5-step verification checklist from AGENTS.md
- Example:
  ```
  1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
  2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass
  3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
  4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
  5. godot --headless --path . -- --bridge-selftest               # Exits 0
  ```

### PHASE 4 — Output
- Format as a ready-to-paste task frame
- Never execute — only generate the frame

## CONSTRAINTS
- Never invent steps outside AGENTS.md's workflow
- Never assume files — always list based on evidence
- Always include the 5-step verification checklist

## OUTPUT
`docs/tasks/TASK_FRAME_<task>.md` — goal, file list, verification checklist

## QUALITY GATE
- Goal ≤ 2 lines
- File list complete (no omissions)
- Verification checklist matches AGENTS.md
