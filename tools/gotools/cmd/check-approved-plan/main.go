package main

import (
	"flag"
	"fmt"
	"os"

	"ashfall/gotools/pkg/checkplan"
)

func main() {
	fs := flag.NewFlagSet("check-approved-plan", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	stagedOnly := fs.Bool("staged", false, "Check only git staged files")
	compareRef := fs.String("ref", "", "Git ref to compare diff against (e.g. HEAD, origin/main)")
	_ = fs.Parse(os.Args[1:])

	explicitFiles := fs.Args()

	ok, msg, err := checkplan.CheckApprovedPlan(*rootDir, *stagedOnly, *compareRef, explicitFiles)
	if err != nil {
		fmt.Fprintf(os.Stderr, "[check-approved-plan] ERROR: %v\n", err)
		os.Exit(2)
	}

	if !ok {
		fmt.Fprintf(os.Stderr, "\n[check-approved-plan] COMMIT/CI REJECTED:\n%s\n\n", msg)
		os.Exit(1)
	}

	fmt.Printf("[check-approved-plan] OK: %s\n", msg)
}
