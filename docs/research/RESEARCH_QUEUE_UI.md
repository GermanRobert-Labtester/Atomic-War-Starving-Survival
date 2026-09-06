# Plan 35: Research Queue Player Surface Architecture

## Overview
Plan 35 transforms the research foundation into a fully interactive, deterministic research queue player surface. It provides structured eligibility queries, single active research slot enforcement, day-delta progression without drift, idempotent save/load, and a complete UI lifecycle binding in `ResearchPanel.cs`.

## Core Components & Architecture

### 1. Research Eligibility Contract (`Assets/Ashfall.Core/Research/ResearchEligibility.cs`)
- **`ResearchEligibilityCode`**: Strongly typed status codes:
  - `Eligible`: All prerequisites met, no conflicting active research.
  - `UnknownNode`: Requested node is absent from the knowledge catalog.
  - `NotDiscovered`: Node requires discovery or prerequisite unlocking.
  - `MissingPrerequisites`: Node has unmet prerequisite knowledge requirements.
  - `AlreadyActive`: The node is currently in progress.
  - `AnotherResearchActive`: Another node is already in the active slot (single-slot policy).
  - `AlreadyCompleted`: Node is already researched.
- **`ResearchEligibility`**: Immutable value type reporting `CanStart`, `IsDiscovered`, `IsCompleted`, `IsActive`, `MissingPrerequisites` list, and `ConflictingActiveResearchId`.

### 2. Core Engine Additions (`Assets/Ashfall.Core/Research/ResearchSystem.cs`)
- **`GetEligibility(string id)`**: Pure query checking prerequisites, active status, and completion state without mutations.
- **`StartResearch(string id, int day)`**: Validates eligibility via `GetEligibility(id)`; sets active node and initial day budget; prevents multiple concurrent research.
- **`GetDaysRemaining(string id)`**: Returns remaining days for active node or total required days for unstarted nodes.
- **`GetAvailableNodes()` / `GetLockedNodes()` / `GetCompletedNodes()`**: Partition catalog nodes for UI rendering.
- **`GetDependents(string id)`**: Diamond-safe graph traversal determining all nodes that require the specified node as a prerequisite.

### 3. Godot Host Session (`src/Host/ResearchHostSession.cs`)
- Exposes `TryStart(string id, int day)`, `AvailableNodes()`, `LockedNodes()`, `CompletedNodes()`, `DaysRemaining(string id)`, `GetEligibility(string id)`, and `GetDependents(string id)`.
- Event hooks: `OnResearchCompleted` subscribes to engine completions, cleanly forwarding state changes and triggering journal entries in `Main.ExpandedShelterSystems.cs`.

### 4. Player Surface UI (`src/UI/ResearchPanel.cs`)
- Replaced stub modal layout with a full interactive `ScrollContainer` view:
  - **Active Research**: Displays currently researching node, duration, remaining days, progress bar, and completion status.
  - **Available to Research**: Lists nodes eligible to start with dedicated `START` buttons triggering `Host.TryStart`.
  - **Locked (Prerequisites Missing)**: Lists unresearched nodes with missing prerequisite badges.
  - **Completed Research**: Lists archived completed discoveries and breakthrough items awarded.
  - **Discovered Knowledge**: Preserves historical row-count metric for `PanelBindLifecycleSelfTest` (Gate 5 compliance).

## Verification
- **Unit Tests**: `Ashfall.Core.Tests/Research/ResearchQueueEligibilityTests.cs` (13 tests covering unknown nodes, prerequisites, active conflicts, completion, graph traversal, and save round-trips).
- **Lifecycle Selftest**: `godot --headless --path . -- --panel-bind-lifecycle-selftest` (Gate 5 verified cleanly).
