package main

import (
	"flag"
	"fmt"
	"os"

	"ashfall/gotools/pkg/scopedtest"
)

func main() {
	fs := flag.NewFlagSet("run-scoped-tests", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	compareRef := fs.String("ref", "", "Git ref to compare diff against (e.g. HEAD, origin/main)")
	dryRun := fs.Bool("dry-run", false, "Print selected test targets without executing them")
	full := fs.Bool("full", false, "Request full test suite run (REQUIRES --user-override='RUN FULL TESTS')")
	userOverride := fs.String("user-override", "", "Explicit user override passphrase (must be exactly 'RUN FULL TESTS')")
	_ = fs.Parse(os.Args[1:])

	explicitFiles := fs.Args()

	opts := scopedtest.RunOptions{
		RepoRoot:     *rootDir,
		Files:        explicitFiles,
		CompareRef:   *compareRef,
		DryRun:       *dryRun,
		RunFullTests: *full,
		UserOverride: *userOverride,
	}

	res, err := scopedtest.RunScopedTests(opts)
	if err != nil {
		fmt.Fprintf(os.Stderr, "\n[run-scoped-tests] ERROR: %v\n\n", err)
		os.Exit(2)
	}

	if res.TotalFailed > 0 {
		os.Exit(1)
	}
}
