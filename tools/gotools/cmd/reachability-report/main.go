// SPDX-License-Identifier: MIT
// Command reachability-report renders the content-reachability disposition
// registry into a single dashboard artifact and fails on missing or
// expiry-less dispositions.
//
// Inputs:
//
//	docs/ci/content_reachability_dispositions.json  (owned dispositions)
//	artifacts/content-utilization.json              (current graph classification)
//
// Output:
//
//	artifacts/content-reachability-report.md
package main

import (
	"encoding/json"
	"flag"
	"fmt"
	"os"
	"path/filepath"
	"sort"
	"strings"
)

const (
	unresolvedClassification = 6 // Ashfall.Core.Content.ContentClassification.UNRESOLVED
	gameplayClassification   = 0 // GAMEPLAY_CONSUMED
)

type exemption struct {
	ContentPath     string `json:"contentPath"`
	Owner           string `json:"owner"`
	Classification  string `json:"classification"`
	Rationale       string `json:"rationale"`
	TrackingTicket  string `json:"trackingTicket"`
	ExpiryCondition string `json:"expiryCondition"`
}

type registry struct {
	SchemaVersion string      `json:"schemaVersion"`
	Exemptions    []exemption `json:"exemptions"`
}

type catalog struct {
	Path           string `json:"path"`
	Classification int    `json:"classification"`
	Loader         string `json:"loader"`
}

type utilization struct {
	Catalogs []catalog `json:"catalogs"`
}

func main() {
	root := flag.String("root", ".", "repository root")
	out := flag.String("out", "", "output markdown path (default artifacts/content-reachability-report.md)")
	flag.Parse()

	if err := run(*root, *out); err != nil {
		fmt.Fprintln(os.Stderr, "reachability-report: "+err.Error())
		os.Exit(1)
	}
}

func run(root, outPath string) error {
	policyPath := filepath.Join(root, "docs", "ci", "content_reachability_dispositions.json")
	graphPath := filepath.Join(root, "artifacts", "content-utilization.json")
	if outPath == "" {
		outPath = filepath.Join(root, "artifacts", "content-reachability-report.md")
	}

	policy, err := loadRegistry(policyPath)
	if err != nil {
		return err
	}
	graph, err := loadUtilization(graphPath)
	if err != nil {
		return err
	}

	current := make(map[string]catalog, len(graph.Catalogs))
	for _, c := range graph.Catalogs {
		current[c.Path] = c
	}

	dispositioned := make(map[string]bool, len(policy.Exemptions))
	var failures []string
	for _, e := range policy.Exemptions {
		dispositioned[e.ContentPath] = true
		if strings.TrimSpace(e.ExpiryCondition) == "" {
			failures = append(failures, "disposition without an expiry: "+e.ContentPath)
		}
		if strings.TrimSpace(e.Owner) == "" {
			failures = append(failures, "disposition without an owner: "+e.ContentPath)
		}
	}

	// Any UNRESOLVED catalog without a disposition is a hard failure.
	var unresolved []string
	for path, c := range current {
		if c.Classification == unresolvedClassification && !dispositioned[path] {
			unresolved = append(unresolved, path)
		}
	}
	sort.Strings(unresolved)
	for _, path := range unresolved {
		failures = append(failures, "unresolved catalog without a disposition: "+path)
	}

	report := render(policy, current)
	if err := os.MkdirAll(filepath.Dir(outPath), 0o755); err != nil {
		return err
	}
	if err := os.WriteFile(outPath, []byte(report), 0o644); err != nil {
		return err
	}

	fmt.Printf("reachability-report: wrote %s (%d dispositions, %d unresolved-without-disposition)\n",
		outPath, len(policy.Exemptions), len(unresolved))
	if len(failures) > 0 {
		return fmt.Errorf("%d reachability failure(s):\n  - %s", len(failures), strings.Join(failures, "\n  - "))
	}
	return nil
}

func render(policy *registry, current map[string]catalog) string {
	var b strings.Builder
	b.WriteString("# Content Reachability Report\n\n")
	b.WriteString("Generated from `docs/ci/content_reachability_dispositions.json` against `artifacts/content-utilization.json`.\n\n")

	byClass := map[string]map[string][]exemption{}
	for _, e := range policy.Exemptions {
		if byClass[e.Classification] == nil {
			byClass[e.Classification] = map[string][]exemption{}
		}
		byClass[e.Classification][e.Owner] = append(byClass[e.Classification][e.Owner], e)
	}

	classes := make([]string, 0, len(byClass))
	for c := range byClass {
		classes = append(classes, c)
	}
	sort.Strings(classes)

	var consumed, removalCandidates []string
	for _, e := range policy.Exemptions {
		c, ok := current[e.ContentPath]
		if !ok {
			continue
		}
		if c.Classification == gameplayClassification {
			consumed = append(consumed, e.ContentPath)
		}
		if e.Classification == "DORMANT" && c.Classification == unresolvedClassification && strings.TrimSpace(c.Loader) == "" {
			removalCandidates = append(removalCandidates, e.ContentPath)
		}
	}
	sort.Strings(consumed)
	sort.Strings(removalCandidates)

	b.WriteString("## Summary\n\n")
	b.WriteString(fmt.Sprintf("- Dispositions: %d\n", len(policy.Exemptions)))
	b.WriteString(fmt.Sprintf("- Now gameplay-consumed (disposition can retire): %d\n", len(consumed)))
	b.WriteString(fmt.Sprintf("- Dormant unreferenced removal candidates: %d\n", len(removalCandidates)))
	b.WriteString(fmt.Sprintf("- Owners: %d\n\n", countOwners(policy.Exemptions)))

	b.WriteString("## Removal candidates (dormant + unresolved + no loader)\n\n")
	if len(removalCandidates) == 0 {
		b.WriteString("None. Every dormant catalog carries loader or source evidence; none is safe to delete without a reference audit.\n\n")
	} else {
		for _, path := range removalCandidates {
			b.WriteString(fmt.Sprintf("- `%s`\n", path))
		}
		b.WriteString("\n")
	}

	for _, class := range classes {
		owners := byClass[class]
		var ownerNames []string
		for o := range owners {
			ownerNames = append(ownerNames, o)
		}
		sort.Strings(ownerNames)
		b.WriteString(fmt.Sprintf("## %s (%d)\n\n", class, countClass(policy.Exemptions, class)))
		b.WriteString("| Catalog | Owner | Expiry | Current graph class |\n|---|---|---|---|\n")
		for _, owner := range ownerNames {
			rows := owners[owner]
			sort.Slice(rows, func(i, j int) bool { return rows[i].ContentPath < rows[j].ContentPath })
			for _, e := range rows {
				b.WriteString(fmt.Sprintf("| `%s` | %s | %s | %d |\n",
					e.ContentPath, e.Owner, e.ExpiryCondition, current[e.ContentPath].Classification))
			}
		}
		b.WriteString("\n")
	}
	return b.String()
}

func countOwners(es []exemption) int {
	set := map[string]bool{}
	for _, e := range es {
		set[e.Owner] = true
	}
	return len(set)
}

func countClass(es []exemption, class string) int {
	n := 0
	for _, e := range es {
		if e.Classification == class {
			n++
		}
	}
	return n
}

func loadRegistry(path string) (*registry, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, fmt.Errorf("read dispositions %s: %w", path, err)
	}
	var r registry
	if err := json.Unmarshal(data, &r); err != nil {
		return nil, fmt.Errorf("parse dispositions %s: %w", path, err)
	}
	if len(r.Exemptions) == 0 {
		return nil, fmt.Errorf("dispositions %s is empty", path)
	}
	return &r, nil
}

func loadUtilization(path string) (*utilization, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, fmt.Errorf("read utilization %s: %w", path, err)
	}
	var u utilization
	if err := json.Unmarshal(data, &u); err != nil {
		return nil, fmt.Errorf("parse utilization %s: %w", path, err)
	}
	return &u, nil
}
