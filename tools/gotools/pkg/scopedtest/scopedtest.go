package scopedtest

import (
	"fmt"
	"io"
	"os"
	"os/exec"
	"strings"
	"time"

	"ashfall/gotools/pkg/selector"
)

type RunOptions struct {
	RepoRoot     string
	Files        []string
	CompareRef   string
	DryRun       bool
	RunFullTests bool
	UserOverride string
	Stdout       io.Writer
	Stderr       io.Writer
}

type RunResult struct {
	TotalRun    int
	TotalPassed int
	TotalFailed int
	Duration    time.Duration
	Failures    []string
	Advice      string
}

const FullTestPassphrase = "RUN FULL TESTS"

func RunScopedTests(opts RunOptions) (*RunResult, error) {
	if opts.Stdout == nil {
		opts.Stdout = os.Stdout
	}
	if opts.Stderr == nil {
		opts.Stderr = os.Stderr
	}

	// 1. Explicit Ban Check
	if opts.RunFullTests {
		if strings.TrimSpace(opts.UserOverride) != FullTestPassphrase &&
			strings.TrimSpace(os.Getenv("ASHFALL_TEST_OVERRIDE")) != FullTestPassphrase {
			errMsg := fmt.Sprintf(
				"VIOLATION: Full test suite requested without explicit authorization.\n"+
					"- NEVER run the full test suite unless the user types exactly: %q.\n"+
					"If you feel the need to run the full suite, instead:\n"+
					"  1. List which additional tests you think are relevant.\n"+
					"  2. Ask: \"Should I run these extra tests now, or only the scoped ones?\"",
				FullTestPassphrase)
			return nil, fmt.Errorf("%s", errMsg)
		}
	}

	// 2. Identify target files
	files := opts.Files
	if len(files) == 0 {
		var err error
		files, err = selector.GetChangedFiles(opts.RepoRoot, opts.CompareRef)
		if err != nil {
			return nil, fmt.Errorf("failed to detect changed files: %w", err)
		}
	}

	if len(files) == 0 {
		_, _ = fmt.Fprintln(opts.Stdout, "[run-scoped-tests] No changed files detected. No scoped tests to run.")
		return &RunResult{}, nil
	}

	// 3. Map changed files to targeted tests
	plan := selector.SelectTestsForFiles(opts.RepoRoot, files)

	if len(plan.SelectedTests) == 0 {
		_, _ = fmt.Fprintln(opts.Stdout, "[run-scoped-tests] "+plan.Explanation)
		return &RunResult{Advice: plan.Explanation}, nil
	}

	_, _ = fmt.Fprintf(opts.Stdout, "[run-scoped-tests] Found %d changed files -> Mapped %d targeted test target(s):\n",
		len(plan.ChangedFiles), len(plan.SelectedTests))
	for i, t := range plan.SelectedTests {
		_, _ = fmt.Fprintf(opts.Stdout, "  %d. [%s] %s -> %s\n", i+1, t.Tier, t.TargetFile, t.Command)
	}

	if opts.DryRun {
		_, _ = fmt.Fprintln(opts.Stdout, "\n[run-scoped-tests] (Dry-run mode: tests not executed)")
		return &RunResult{TotalRun: len(plan.SelectedTests)}, nil
	}

	// 4. Execute scoped tests
	startTime := time.Now()
	res := &RunResult{
		TotalRun: len(plan.SelectedTests),
	}

	_, _ = fmt.Fprintln(opts.Stdout, "\n==================== EXECUTING SCOPED TESTS ====================")

	for i, t := range plan.SelectedTests {
		_, _ = fmt.Fprintf(opts.Stdout, "\n>>> [%d/%d] Running %s (%s)...\n", i+1, len(plan.SelectedTests), t.TargetFile, t.Tier)
		testStart := time.Now()

		cmd := exec.Command("bash", "-c", t.Command)
		cmd.Dir = opts.RepoRoot
		cmd.Stdout = opts.Stdout
		cmd.Stderr = opts.Stderr

		err := cmd.Run()
		testDuration := time.Since(testStart)

		if err != nil {
			res.TotalFailed++
			res.Failures = append(res.Failures, fmt.Sprintf("%s (%s)", t.TargetFile, t.Command))
			_, _ = fmt.Fprintf(opts.Stderr, ">>> FAILED [%s] in %v\n", t.TargetFile, testDuration)
		} else {
			res.TotalPassed++
			_, _ = fmt.Fprintf(opts.Stdout, ">>> PASSED [%s] in %v\n", t.TargetFile, testDuration)
		}
	}

	res.Duration = time.Since(startTime)
	_, _ = fmt.Fprintln(opts.Stdout, "================================================================")
	_, _ = fmt.Fprintf(opts.Stdout, "Scoped Test Summary: %d ran, %d passed, %d failed in %v\n",
		res.TotalRun, res.TotalPassed, res.TotalFailed, res.Duration)

	if res.TotalFailed > 0 {
		_, _ = fmt.Fprintln(opts.Stderr, "\n[ALERT - TEST POLICY ENFORCEMENT]:")
		_, _ = fmt.Fprintln(opts.Stderr, "Maximum 10-15 test-edit steps per failing test.")
		_, _ = fmt.Fprintln(opts.Stderr, "If you cannot resolve failing tests within 10-15 steps, DO NOT repeatedly hit tests with micro-edits!")
		_, _ = fmt.Fprintln(opts.Stderr, "Auto-flag the issue in .ai/state.md for a bug validator to fix instead.")
	}

	return res, nil
}
