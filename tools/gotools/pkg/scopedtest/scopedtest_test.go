package scopedtest

import (
	"bytes"
	"strings"
	"testing"
)

func TestRunScopedTestsBanFullWithoutPassphrase(t *testing.T) {
	var stdout, stderr bytes.Buffer
	opts := RunOptions{
		RunFullTests: true,
		UserOverride: "run everything please",
		Stdout:       &stdout,
		Stderr:       &stderr,
	}

	_, err := RunScopedTests(opts)
	if err == nil {
		t.Fatalf("expected error when running full tests without exact passphrase")
	}

	if !strings.Contains(err.Error(), "VIOLATION: Full test suite requested without explicit authorization") {
		t.Errorf("unexpected error message: %v", err)
	}
}

func TestRunScopedTestsDryRun(t *testing.T) {
	var stdout, stderr bytes.Buffer
	opts := RunOptions{
		RepoRoot: ".",
		Files:    []string{"tests/test_parser.py"},
		DryRun:   true,
		Stdout:   &stdout,
		Stderr:   &stderr,
	}

	res, err := RunScopedTests(opts)
	if err != nil {
		t.Fatalf("unexpected error in dry-run: %v", err)
	}
	if res.TotalRun != 1 {
		t.Errorf("expected 1 test planned, got %d", res.TotalRun)
	}
	if !strings.Contains(stdout.String(), "Dry-run mode") {
		t.Errorf("expected dry-run notification in stdout")
	}
}
