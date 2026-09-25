package main

import (
	"encoding/json"
	"flag"
	"fmt"
	"io"
	"os"
	"path/filepath"
	"time"

	"ashfall/gotools/pkg/agentsync"
	"ashfall/gotools/pkg/checkplan"
	"ashfall/gotools/pkg/config"
	"ashfall/gotools/pkg/indexer"
	"ashfall/gotools/pkg/manifest"
	"ashfall/gotools/pkg/orchestrator"
	"ashfall/gotools/pkg/parser"
	"ashfall/gotools/pkg/proxy"
	"ashfall/gotools/pkg/runner"
	"ashfall/gotools/pkg/scanner"
	"ashfall/gotools/pkg/scopedtest"
	"ashfall/gotools/pkg/selector"
	"ashfall/gotools/pkg/validator"
)

func printUsage() {
	fmt.Println(`ASHFALL Development & CI Tool Suite (Go)

Usage:
  ashfall-dev <command> [options]

Commands:
  index            Fast repository file indexer (supports --watch and --serve)
  select-tests     Changed-file / changed-test selector enforcing test hierarchy
  run-scoped-tests Runs targeted tests mapped to changed files (enforces no-full-test policy)
  check-plan       Verifies approved plan in .ai/plans/ for code changes
  parse-results    Test-result parser with taxonomy classification and markdown summary
  validate-json    Fast JSON schema & UTF-8 validator (sub-second runtime)
  validate-config  JSON Schema validator for quest, save, and config files (JSON/YAML)
  scan-saves       Save-file & save-store contract scanner
  build-manifest   Asset manifest builder with image dimensions and SHA256 hashes
  run-tasks        Parallel subprocess / task runner with timeout and RSS metrics
  llm-proxy        Lightweight local LLM API proxy and router
  sync-agents      Synchronize and check drift across all 13 client agent rulebooks
  agent-core       Resident long-running AI agent orchestrator (< 20 MB RAM)

Use "ashfall-dev <command> -h" for options on a specific command.`)
}

func main() {
	if len(os.Args) < 2 {
		printUsage()
		os.Exit(1)
	}

	cmd := os.Args[1]
	args := os.Args[2:]

	switch cmd {
	case "index":
		runIndex(args)
	case "select-tests":
		runSelectTests(args)
	case "run-scoped-tests":
		runRunScopedTests(args)
	case "check-plan":
		runCheckPlan(args)
	case "parse-results":
		runParseResults(args)
	case "validate-json":
		runValidateJSON(args)
	case "validate-config":
		runValidateConfig(args)
	case "scan-saves":
		runScanSaves(args)
	case "build-manifest":
		runBuildManifest(args)
	case "run-tasks":
		runTasks(args)
	case "llm-proxy":
		runLLMProxy(args)
	case "sync-agents":
		runSyncAgents(args)
	case "agent-core":
		runAgentCore(args)
	case "-h", "--help", "help":
		printUsage()
	default:
		fmt.Fprintf(os.Stderr, "Unknown command: %s\n\n", cmd)
		printUsage()
		os.Exit(1)
	}
}

func runIndex(args []string) {
	fs := flag.NewFlagSet("index", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	withHash := fs.Bool("hash", false, "Compute SHA256 hash for every file")
	asJSON := fs.Bool("json", false, "Output results as formatted JSON")
	withList := fs.Bool("list", false, "Include full file list in JSON output")
	serveAddr := fs.String("serve", "", "Start resident HTTP API server (e.g. 127.0.0.1:8081)")
	watch := fs.Bool("watch", false, "Watch repository changes dynamically with fsnotify")
	_ = fs.Parse(args)

	if *serveAddr != "" || *watch {
		resIdx, err := indexer.NewResidentIndexer(*rootDir)
		if err != nil {
			fmt.Fprintf(os.Stderr, "Error creating resident indexer: %v\n", err)
			os.Exit(1)
		}
		if *watch {
			if err := resIdx.StartWatcher(); err != nil {
				fmt.Fprintf(os.Stderr, "Error starting file watcher: %v\n", err)
				os.Exit(1)
			}
		}
		if *serveAddr != "" {
			if err := resIdx.StartHTTP(*serveAddr); err != nil {
				fmt.Fprintf(os.Stderr, "Error starting HTTP server: %v\n", err)
				os.Exit(1)
			}
		}
		return
	}

	report, err := indexer.IndexRepository(*rootDir, *withHash, *withList)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error indexing repository: %v\n", err)
		os.Exit(1)
	}

	if *asJSON {
		data, _ := report.ToJSON(true)
		fmt.Println(string(data))
	} else {
		fmt.Print(report.Summary())
	}
}

func runSelectTests(args []string) {
	fs := flag.NewFlagSet("select-tests", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	compareRef := fs.String("ref", "", "Git ref to compare diff against (e.g. HEAD, origin/main)")
	asJSON := fs.Bool("json", false, "Output plan as JSON")
	_ = fs.Parse(args)

	changed, err := selector.GetChangedFiles(*rootDir, *compareRef)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error inspecting git changes: %v\n", err)
		os.Exit(1)
	}

	plan := selector.SelectTestsForFiles(*rootDir, changed)

	if *asJSON {
		data, _ := json.MarshalIndent(plan, "", "  ")
		fmt.Println(string(data))
	} else {
		fmt.Printf("=== Changed-Test Selector (Found %d changed files) ===\n", len(plan.ChangedFiles))
		fmt.Println(plan.Explanation)
		if len(plan.SelectedTests) > 0 {
			fmt.Println("\nRecommended Targeted Commands (Hierarchical):")
			for _, t := range plan.SelectedTests {
				fmt.Printf("  [%s] %s\n    -> Run: %s\n", t.Tier, t.TargetFile, t.Command)
			}
		}
	}
}

func runParseResults(args []string) {
	fs := flag.NewFlagSet("parse-results", flag.ExitOnError)
	code := fs.Int("exit-code", 0, "Process exit code")
	asJSON := fs.Bool("json", false, "Output as JSON")
	_ = fs.Parse(args)

	input, err := io.ReadAll(os.Stdin)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to read input from stdin: %v\n", err)
		os.Exit(1)
	}

	parsed := parser.ParseTestOutput(string(input), *code)
	if *asJSON {
		data, _ := parsed.ToJSON()
		fmt.Println(string(data))
	} else {
		fmt.Println(parsed.MarkdownReport())
	}
}

func runValidateJSON(args []string) {
	fs := flag.NewFlagSet("validate-json", flag.ExitOnError)
	dataDir := fs.String("dir", "Assets/StreamingAssets/Data", "Directory containing JSON files to validate")
	asJSON := fs.Bool("json", false, "Output results as JSON")
	_ = fs.Parse(args)

	summary, err := validator.ValidateDirectory(*dataDir)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error validating directory %s: %v\n", *dataDir, err)
		os.Exit(1)
	}

	if *asJSON {
		data, _ := json.MarshalIndent(summary, "", "  ")
		fmt.Println(string(data))
	} else {
		fmt.Printf("=== Fast JSON Schema Validator ===\n")
		fmt.Printf("Checked %d files in %d ms | Valid: %d | Violations: %d\n",
			summary.TotalChecked, summary.DurationMs, summary.ValidCount, summary.InvalidCount)
		if summary.InvalidCount > 0 {
			for _, v := range summary.Violations {
				fmt.Printf("  ❌ [%s] %s: %s\n", v.Rule, v.FilePath, v.Message)
			}
			os.Exit(1)
		} else {
			fmt.Println("✅ All catalog files adhere strictly to schema policy (schema_version >= 1, root object {}).")
		}
	}
}

func runScanSaves(args []string) {
	fs := flag.NewFlagSet("scan-saves", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	saveDir := fs.String("save-dir", "saves", "Saved game slots directory")
	asJSON := fs.Bool("json", false, "Output report as JSON")
	_ = fs.Parse(args)

	stores, err := scanner.DiscoverSaveStores(*rootDir)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error discovering save stores: %v\n", err)
		os.Exit(1)
	}

	files, _ := scanner.ScanSaveDirectory(filepath.Join(*rootDir, *saveDir))

	report := scanner.SaveAuditReport{
		DiscoveredStores: stores,
		ScannedFiles:     files,
		StoreCount:       len(stores),
		FileCount:        len(files),
	}

	if *asJSON {
		data, _ := json.MarshalIndent(report, "", "  ")
		fmt.Println(string(data))
	} else {
		fmt.Printf("=== Save-File & Store Scanner ===\n")
		fmt.Printf("Discovered %d SaveStore declarations in C# codebase.\n", report.StoreCount)
		for _, s := range report.DiscoveredStores {
			fmt.Printf("  - %-32s (Section: %-20s, Path: %s)\n", s.ClassName, s.SectionName, s.SavePath)
		}
		if report.FileCount > 0 {
			fmt.Printf("\nScanned %d active save file(s):\n", report.FileCount)
			for _, f := range report.ScannedFiles {
				fmt.Printf("  - %s (Ver: %d, SHA: %.8s, Status: %s)\n", f.FilePath, f.SchemaVersion, f.ComputedSHA256, f.Status)
			}
		}
	}
}

func runBuildManifest(args []string) {
	fs := flag.NewFlagSet("build-manifest", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	outPath := fs.String("out", "assets/ASSET_MANIFEST_GENERATED.json", "Output path for JSON manifest")
	_ = fs.Parse(args)

	dirs := []string{"assets", "Assets"}
	m, err := manifest.BuildManifest(*rootDir, dirs)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error building manifest: %v\n", err)
		os.Exit(1)
	}

	_ = os.MkdirAll(filepath.Dir(filepath.Join(*rootDir, *outPath)), 0755)
	if err := m.SaveJSON(filepath.Join(*rootDir, *outPath)); err != nil {
		fmt.Fprintf(os.Stderr, "Error saving manifest: %v\n", err)
		os.Exit(1)
	}

	fmt.Printf("=== Asset Manifest Builder ===\n")
	fmt.Printf("Indexed %d assets (%.2f MB) -> Saved to %s\n",
		m.TotalAssets, float64(m.TotalBytes)/(1024*1024), *outPath)
}

func runTasks(args []string) {
	fs := flag.NewFlagSet("run-tasks", flag.ExitOnError)
	concurrency := fs.Int("j", 4, "Concurrency / worker count")
	_ = fs.Parse(args)

	pool := runner.NewTaskRunnerPool(*concurrency)

	// Example default task when no sub-commands supplied
	tasks := []runner.Task{
		{
			ID:      "json_validation",
			Command: os.Args[0],
			Args:    []string{"validate-json"},
			Timeout: 10 * time.Second,
		},
		{
			ID:      "index_repo",
			Command: os.Args[0],
			Args:    []string{"index"},
			Timeout: 15 * time.Second,
		},
	}

	results := pool.RunTasks(tasks)
	fmt.Printf("=== Parallel Task Runner (Workers: %d) ===\n", *concurrency)
	for _, r := range results {
		status := "PASS"
		if r.ExitCode != 0 {
			status = fmt.Sprintf("FAIL (code %d)", r.ExitCode)
		}
		fmt.Printf("  - [%s] Task: %-18s Duration: %-8v Peak RSS: %d KB\n",
			status, r.ID, r.Duration, r.PeakRSSKb)
	}
}

func runLLMProxy(args []string) {
	fs := flag.NewFlagSet("llm-proxy", flag.ExitOnError)
	addr := fs.String("addr", "127.0.0.1:8088", "Listen address")
	_ = fs.Parse(args)

	routes := []proxy.RouteTarget{
		{
			Prefix:     "/v1",
			BackendURL: "http://127.0.0.1:11434", // Default local Ollama
		},
	}

	srv := proxy.NewLLMProxyServer(*addr, routes)
	if err := srv.Start(); err != nil {
		fmt.Fprintf(os.Stderr, "LLM Proxy error: %v\n", err)
		os.Exit(1)
	}
}

func runSyncAgents(args []string) {
	fs := flag.NewFlagSet("sync-agents", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	checkOnly := fs.Bool("check", false, "Check for drift without modifying files")
	_ = fs.Parse(args)

	ok, updated, err := agentsync.SyncAgentRulebooks(*rootDir, *checkOnly)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error synchronizing agent rulebooks: %v\n", err)
		os.Exit(1)
	}

	if *checkOnly {
		if !ok {
			fmt.Println("❌ Drift detected across agent rulebooks.")
			os.Exit(1)
		}
		fmt.Println("OK: All 13 client rulebooks are in sync with AGENTS.md.")
	} else {
		for _, f := range updated {
			fmt.Printf("Updated %s\n", f)
		}
		fmt.Println("Wrote docs/agents/AGENTS_SYNC_REPORT.md")
	}
}

func runAgentCore(args []string) {
	fs := flag.NewFlagSet("agent-core", flag.ExitOnError)
	addr := fs.String("addr", "127.0.0.1:8082", "Listen address for Game-Agent-Core HTTP server")
	rootDir := fs.String("root", ".", "Repository root directory")
	_ = fs.Parse(args)

	srv := orchestrator.NewAgentCoreServer(*addr, *rootDir)
	if err := srv.Start(); err != nil {
		fmt.Fprintf(os.Stderr, "Game-Agent-Core error: %v\n", err)
		os.Exit(1)
	}
}

func runValidateConfig(args []string) {
	if len(args) < 2 {
		fmt.Fprintf(os.Stderr, "Usage: ashfall-dev validate-config <schema.json> <file.[json|yaml]>\n")
		os.Exit(2)
	}
	schemaPath := args[0]
	instancePath := args[1]

	if err := config.ValidateFile(schemaPath, instancePath); err != nil {
		for _, line := range config.PrettyValidationError(err) {
			fmt.Fprintln(os.Stderr, line)
		}
		os.Exit(1)
	}
	fmt.Println("OK")
}

func runRunScopedTests(args []string) {
	fs := flag.NewFlagSet("run-scoped-tests", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	compareRef := fs.String("ref", "", "Git ref to compare diff against (e.g. HEAD, origin/main)")
	dryRun := fs.Bool("dry-run", false, "Print selected test targets without executing them")
	full := fs.Bool("full", false, "Request full test suite run (REQUIRES --user-override='RUN FULL TESTS')")
	userOverride := fs.String("user-override", "", "Explicit user override passphrase (must be exactly 'RUN FULL TESTS')")
	_ = fs.Parse(args)

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

func runCheckPlan(args []string) {
	fs := flag.NewFlagSet("check-plan", flag.ExitOnError)
	rootDir := fs.String("root", ".", "Repository root directory")
	stagedOnly := fs.Bool("staged", false, "Check only git staged files")
	compareRef := fs.String("ref", "", "Git ref to compare diff against (e.g. HEAD, origin/main)")
	_ = fs.Parse(args)

	explicitFiles := fs.Args()

	ok, msg, err := checkplan.CheckApprovedPlan(*rootDir, *stagedOnly, *compareRef, explicitFiles)
	if err != nil {
		fmt.Fprintf(os.Stderr, "[check-plan] ERROR: %v\n", err)
		os.Exit(2)
	}

	if !ok {
		fmt.Fprintf(os.Stderr, "\n[check-plan] COMMIT/CI REJECTED:\n%s\n\n", msg)
		os.Exit(1)
	}

	fmt.Printf("[check-plan] OK: %s\n", msg)
}
