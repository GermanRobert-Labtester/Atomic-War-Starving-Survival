// Command releasepolicy checks that the release workflow
// (.github/workflows/release.yml) cannot pass without actually running
// every gate the CI gate manifest marks release_required, as a genuine
// blocking step, and that the export job cannot upload a build artifact
// that was never produced.
//
// It never runs the release gate, the exporters, or any test; it only
// parses the workflow YAML, the release-gate.sh script text, and the
// manifest JSON and reports where they disagree.
//
// Usage:
//
//	go run tools/gotools/cmd/releasepolicy/main.go --out build/reports/release-policy-results.json
//
// Exit codes:
//
//	0  no policy violations
//	1  one or more policy violations (see --out for detail)
//	2  could not load/parse the workflow, script, or manifest
package main

import (
	"flag"
	"fmt"
	"os"

	"ashfall/gotools/pkg/releasepolicy"
)

func main() {
	fs := flag.NewFlagSet("releasepolicy", flag.ExitOnError)
	root := fs.String("root", ".", "Repository root directory")
	workflow := fs.String("workflow", ".github/workflows/release.yml", "Release workflow path, relative to --root")
	script := fs.String("script", "scripts/ci/release-gate.sh", "Canonical release gate script path, relative to --root")
	exportScript := fs.String("export-script", "scripts/ci/export-build.sh", "Export job script path, relative to --root (used only to detect export-job-satisfied gates)")
	manifest := fs.String("manifest", "docs/ci/CI_GATE_MANIFEST.json", "CI gate manifest path, relative to --root")
	out := fs.String("out", "", "Path to write the compact JSON report (required)")
	_ = fs.Parse(os.Args[1:])

	if *out == "" {
		fmt.Fprintln(os.Stderr, "[releasepolicy] ERROR: --out is required")
		os.Exit(2)
	}

	rep, err := releasepolicy.Run(*root, *workflow, *script, *exportScript, *manifest)
	if err != nil {
		fmt.Fprintf(os.Stderr, "[releasepolicy] ERROR: %v\n", err)
		os.Exit(2)
	}

	if err := rep.WriteJSON(*out); err != nil {
		fmt.Fprintf(os.Stderr, "[releasepolicy] ERROR writing report to %s: %v\n", *out, err)
		os.Exit(2)
	}

	if !rep.Pass {
		fmt.Fprintf(os.Stderr, "[releasepolicy] FAIL — %d violation(s); see %s\n", rep.Metrics.ViolationCount, *out)
		for _, v := range rep.Violations {
			fmt.Fprintf(os.Stderr, "  - [%s] %s: %s\n", v.Kind, v.Location, v.Detail)
		}
		os.Exit(1)
	}

	fmt.Printf("[releasepolicy] PASS — %d release_required gate(s) all reachable from the release workflow (report: %s)\n",
		rep.Metrics.RequiredGateCount, *out)
}
