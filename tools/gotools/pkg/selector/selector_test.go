// SPDX-License-Identifier: MIT
package selector

import (
	"os"
	"path/filepath"
	"testing"
)

// mkdirs creates the given relative directories under root.
func mkdirs(t *testing.T, root string, dirs ...string) {
	t.Helper()
	for _, d := range dirs {
		if err := os.MkdirAll(filepath.Join(root, d), 0o755); err != nil {
			t.Fatalf("mkdir %s: %v", d, err)
		}
	}
}

// selectedNames returns the selected target paths as a set.
func selectedNames(plan *SelectionPlan) map[string]bool {
	got := make(map[string]bool, len(plan.SelectedTests))
	for _, s := range plan.SelectedTests {
		got[s.TargetFile] = true
	}
	return got
}

func TestSelectTests_HostL10nToolingMapToExistingSuites(t *testing.T) {
	root := t.TempDir()
	mkdirs(t, root,
		"Ashfall.Core.Tests/UI",
		"Ashfall.Core.Tests/Localization",
		"Ashfall.Core.Tests/Tooling",
		"Ashfall.Core.Tests/Data",
	)

	plan := SelectTestsForFiles(root, []string{
		"src/UI/GreenhousePanel.cs",
		"assets/l10n/strings.csv",
		"scripts/ci/run-gates.py",
	})

	got := selectedNames(plan)
	for _, want := range []string{
		"Ashfall.Core.Tests/UI",
		"Ashfall.Core.Tests/Localization",
		"Ashfall.Core.Tests/Tooling",
	} {
		if !got[want] {
			t.Errorf("expected %q to be selected; got %v", want, got)
		}
	}
	if len(plan.SelectedTests) != 3 {
		t.Errorf("expected 3 deduplicated targets, got %d (%v)", len(plan.SelectedTests), got)
	}
}

func TestSelectTests_DataCatalogAndGodotConfigMapToToolingData(t *testing.T) {
	root := t.TempDir()
	mkdirs(t, root,
		"Ashfall.Core.Tests/Data",
		"Ashfall.Core.Tests/Tooling",
	)

	plan := SelectTestsForFiles(root, []string{
		"Assets/StreamingAssets/Data/items.json",
		".github/workflows/ci.yml",
	})

	got := selectedNames(plan)
	if !got["Ashfall.Core.Tests/Data"] {
		t.Errorf("expected data catalog to map to Ashfall.Core.Tests/Data; got %v", got)
	}
	if !got["Ashfall.Core.Tests/Tooling"] {
		t.Errorf("expected workflow change to map to Ashfall.Core.Tests/Tooling; got %v", got)
	}
}

func TestSelectTests_CoreDomainBehaviourUnchanged(t *testing.T) {
	root := t.TempDir()
	mkdirs(t, root, "Ashfall.Core.Tests/Shelter")

	plan := SelectTestsForFiles(root, []string{"Assets/Ashfall.Core/Shelter/HydroponicsSystem.cs"})

	got := selectedNames(plan)
	if !got["Ashfall.Core.Tests/Shelter"] {
		t.Errorf("expected Core subsystem change to map to its dir; got %v", got)
	}
}

func TestSelectTests_UnmappedFileYieldsNoTargets(t *testing.T) {
	root := t.TempDir()
	mkdirs(t, root, "Ashfall.Core.Tests/UI")

	plan := SelectTestsForFiles(root, []string{"README.md"})
	if len(plan.SelectedTests) != 0 {
		t.Errorf("expected no targets for an unmapped file, got %v", selectedNames(plan))
	}
}
