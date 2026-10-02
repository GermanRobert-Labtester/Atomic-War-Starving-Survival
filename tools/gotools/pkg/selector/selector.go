package selector

import (
	"bufio"
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
)

type TargetKind string

const (
	KindXUnit  TargetKind = "xunit"
	KindPytest TargetKind = "pytest"
	KindGodot  TargetKind = "godot"
	KindNone   TargetKind = "none"
)

type SelectedTest struct {
	TargetFile  string     `json:"target_file"`
	Kind        TargetKind `json:"kind"`
	Command     string     `json:"command"`
	Tier        string     `json:"tier"` // Fast, Medium, Full
	TriggerFile string     `json:"trigger_file"`
}

type SelectionPlan struct {
	ChangedFiles  []string       `json:"changed_files"`
	SelectedTests []SelectedTest `json:"selected_tests"`
	Explanation   string         `json:"explanation"`
}

func GetChangedFiles(repoRoot string, compareRef string) ([]string, error) {
	var files []string
	seen := make(map[string]bool)

	// 1. Check uncommitted changes (git status --porcelain)
	cmdStatus := exec.Command("git", "status", "--porcelain")
	cmdStatus.Dir = repoRoot
	outStatus, err := cmdStatus.Output()
	if err == nil {
		scanner := bufio.NewScanner(strings.NewReader(string(outStatus)))
		for scanner.Scan() {
			line := strings.TrimSpace(scanner.Text())
			if len(line) < 4 {
				continue
			}
			path := strings.TrimSpace(line[3:])
			// handle renamed: old -> new
			if idx := strings.Index(path, "->"); idx != -1 {
				path = strings.TrimSpace(path[idx+2:])
			}
			path = strings.Trim(path, "\"")
			if !seen[path] && path != "" {
				seen[path] = true
				files = append(files, filepath.ToSlash(path))
			}
		}
	}

	// 2. Check diff against ref if provided (e.g. HEAD, origin/main)
	if compareRef != "" {
		cmdDiff := exec.Command("git", "diff", "--name-only", compareRef)
		cmdDiff.Dir = repoRoot
		outDiff, err := cmdDiff.Output()
		if err == nil {
			scanner := bufio.NewScanner(strings.NewReader(string(outDiff)))
			for scanner.Scan() {
				path := strings.TrimSpace(scanner.Text())
				if !seen[path] && path != "" {
					seen[path] = true
					files = append(files, filepath.ToSlash(path))
				}
			}
		}
	}

	return files, nil
}

func SelectTestsForFiles(repoRoot string, changedFiles []string) *SelectionPlan {
	plan := &SelectionPlan{
		ChangedFiles: changedFiles,
	}

	testSet := make(map[string]bool)

	for _, file := range changedFiles {
		clean := filepath.ToSlash(file)

		// 1. Direct test files changed
		if strings.HasPrefix(clean, "Ashfall.Core.Tests/") && strings.HasSuffix(clean, ".cs") {
			fullPath := filepath.Join(repoRoot, clean)
			if _, err := os.Stat(fullPath); err == nil && !testSet[clean] {
				testSet[clean] = true
				plan.SelectedTests = append(plan.SelectedTests, SelectedTest{
					TargetFile:  clean,
					Kind:        KindXUnit,
					Command:     fmt.Sprintf("bash scripts/run_test.sh %s", clean),
					Tier:        "Fast (<30s)",
					TriggerFile: file,
				})
			}
			continue
		}

		if strings.HasPrefix(clean, "tests/") && strings.HasSuffix(clean, ".py") {
			if !testSet[clean] {
				testSet[clean] = true
				plan.SelectedTests = append(plan.SelectedTests, SelectedTest{
					TargetFile:  clean,
					Kind:        KindPytest,
					Command:     fmt.Sprintf("pytest -n 4 -m fast %s", clean),
					Tier:        "Fast (<30s)",
					TriggerFile: file,
				})
			}
			continue
		}

		// 2. Domain / Core C# files: Assets/Ashfall.Core/<Subsystem>/<Name>.cs
		if strings.HasPrefix(clean, "Assets/Ashfall.Core/") && strings.HasSuffix(clean, ".cs") {
			subPath := strings.TrimPrefix(clean, "Assets/Ashfall.Core/")
			dir := filepath.Dir(subPath)
			base := strings.TrimSuffix(filepath.Base(subPath), ".cs")

			candidates := []string{
				fmt.Sprintf("Ashfall.Core.Tests/%s/%sTests.cs", dir, base),
				fmt.Sprintf("Ashfall.Core.Tests/%s/%sTest.cs", dir, base),
				fmt.Sprintf("Ashfall.Core.Tests/%sTests.cs", base),
				fmt.Sprintf("Ashfall.Core.Tests/%s", dir),
			}

			matched := false
			for _, cand := range candidates {
				if cand == "Ashfall.Core.Tests/." {
					continue
				}
				candPath := filepath.Join(repoRoot, cand)
				if _, err := os.Stat(candPath); err == nil {
					if !testSet[cand] {
						testSet[cand] = true
						plan.SelectedTests = append(plan.SelectedTests, SelectedTest{
							TargetFile:  cand,
							Kind:        KindXUnit,
							Command:     fmt.Sprintf("bash scripts/run_test.sh %s", cand),
							Tier:        "Fast (<30s)",
							TriggerFile: file,
						})
					}
					matched = true
					break
				}
			}

			if !matched {
				// Fallback to targeted regional search
				plan.SelectedTests = append(plan.SelectedTests, SelectedTest{
					TargetFile:  fmt.Sprintf("Ashfall.Core.Tests (Filter: %s)", base),
					Kind:        KindXUnit,
					Command:     fmt.Sprintf("dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FullyQualifiedName~%s", base),
					Tier:        "Fast (<30s)",
					TriggerFile: file,
				})
			}
			continue
		}

		// 3. Python tools changed
		if strings.HasPrefix(clean, "tools/") && strings.HasSuffix(clean, ".py") {
			base := strings.TrimSuffix(filepath.Base(clean), ".py")
			pyTestCand := fmt.Sprintf("tests/test_%s.py", base)
			if _, err := os.Stat(filepath.Join(repoRoot, pyTestCand)); err == nil {
				if !testSet[pyTestCand] {
					testSet[pyTestCand] = true
					plan.SelectedTests = append(plan.SelectedTests, SelectedTest{
						TargetFile:  pyTestCand,
						Kind:        KindPytest,
						Command:     fmt.Sprintf("pytest -n 4 -m fast %s", pyTestCand),
						Tier:        "Fast (<30s)",
						TriggerFile: file,
					})
				}
			}
			continue
		}

		// 4. Godot host C# sources: src/<Dir>/<Name>.cs. Panels and host
		// sessions are covered by the xUnit UI/Tooling source-contract suites,
		// not by Core domain tests, so fall back to the closest existing dir.
		if strings.HasPrefix(clean, "src/") && strings.HasSuffix(clean, ".cs") {
			sub := strings.TrimPrefix(clean, "src/")
			dir := filepath.Dir(sub)
			base := strings.TrimSuffix(filepath.Base(sub), ".cs")
			var candidates []string
			if dir != "." {
				candidates = append(candidates,
					fmt.Sprintf("Ashfall.Core.Tests/%s/%sTests.cs", dir, base),
					fmt.Sprintf("Ashfall.Core.Tests/%s/%sTest.cs", dir, base),
					fmt.Sprintf("Ashfall.Core.Tests/%s", dir),
				)
			}
			candidates = append(candidates, "Ashfall.Core.Tests/Tooling")
			if addXUnitTarget(repoRoot, candidates, testSet, plan, file) {
				continue
			}
		}

		// 5. Authored game data: catalog changes are covered by the Core
		// data-authority and presentation suites.
		if strings.HasPrefix(clean, "Assets/StreamingAssets/Data/") {
			if addXUnitTarget(repoRoot, []string{"Ashfall.Core.Tests/Data", "Ashfall.Core.Tests/Tooling"}, testSet, plan, file) {
				continue
			}
		}

		// 6. Localization catalog: strings.csv / POT drift is covered by the
		// Localization suites (locale gate, ratchet, key parity).
		if strings.HasPrefix(clean, "assets/l10n/") {
			if addXUnitTarget(repoRoot, []string{"Ashfall.Core.Tests/Localization", "Ashfall.Core.Tests/Tooling"}, testSet, plan, file) {
				continue
			}
		}

		// 7. Gate/tooling/CI config: the Tooling source-contract and gate-drift
		// suites assert on the manifest, scripts, and workflows themselves.
		if strings.HasPrefix(clean, "scripts/") || strings.HasPrefix(clean, "docs/ci/") ||
			strings.HasPrefix(clean, ".github/") || strings.HasPrefix(clean, "tools/gotools/") {
			if addXUnitTarget(repoRoot, []string{"Ashfall.Core.Tests/Tooling"}, testSet, plan, file) {
				continue
			}
		}
	}

	if len(plan.SelectedTests) == 0 {
		plan.Explanation = "No affected test targets mapped for current changed files. Full test suite run suppressed per TEST_POLICY.md rule 5."
	} else {
		plan.Explanation = fmt.Sprintf("Selected %d high-signal, targeted test runner(s) for %d changed files.",
			len(plan.SelectedTests), len(changedFiles))
	}

	return plan
}

// addXUnitTarget records the first existing xUnit file/directory candidate for
// a changed production file. Directories are valid run_test.sh targets, so a
// host/data/tooling change always resolves to a bounded suite instead of the
// whole project. Returns true when a target was matched (newly added or already
// present for this invocation).
func addXUnitTarget(
	repoRoot string,
	candidates []string,
	testSet map[string]bool,
	plan *SelectionPlan,
	trigger string,
) bool {
	for _, cand := range candidates {
		if cand == "" || cand == "Ashfall.Core.Tests/." {
			continue
		}
		if _, err := os.Stat(filepath.Join(repoRoot, cand)); err != nil {
			continue
		}
		if !testSet[cand] {
			testSet[cand] = true
			plan.SelectedTests = append(plan.SelectedTests, SelectedTest{
				TargetFile:  cand,
				Kind:        KindXUnit,
				Command:     fmt.Sprintf("bash scripts/run_test.sh %s", cand),
				Tier:        "Fast (<30s)",
				TriggerFile: trigger,
			})
		}
		return true
	}
	return false
}
