package catalogaudit

import (
	"os"
	"path/filepath"
	"testing"
)

func writeFile(t *testing.T, path, body string) {
	t.Helper()
	if err := os.MkdirAll(filepath.Dir(path), 0o755); err != nil {
		t.Fatalf("mkdir: %v", err)
	}
	if err := os.WriteFile(path, []byte(body), 0o644); err != nil {
		t.Fatalf("write %s: %v", path, err)
	}
}

func testPolicy() Policy {
	return Policy{
		SchemaVersion: "1.0.0",
		DataDir:       "data",
		IDNamingRegex: "^[a-z0-9]+(_[a-z0-9]+)*$",
		DefaultDomain: "unclassified",
		IDDomains: []DomainRule{
			{Domain: "item", Globs: []string{"items.json", "*_items.json"}},
			{Domain: "scavenging_table", Globs: []string{"scavenging_tables.json"}},
			{Domain: "location", Globs: []string{"*location*.json", "expeditions.json"}},
		},
		ReferenceRules: []ReferenceRule{
			{SourceGlob: "expeditions.json", Field: "lootCategories", TargetDomain: "item"},
			{SourceGlob: "expeditions.json", Field: "scavenging_table_id", TargetDomain: "scavenging_table"},
		},
		SchemaVersionDefault:    1,
		UnitSuffixAdvisoryRegex: `_[0-9]+[a-z]+_of_[0-9]+[a-z]+$`,
		SchemaVersionExpectations: map[string]int{
			"items.json": 2,
		},
	}
}

func seedValid(t *testing.T, root string) {
	t.Helper()
	writeFile(t, filepath.Join(root, "data", "items.json"),
		`{"schema_version":2,"items":[{"id":"bandage"},{"id":"dried_rations"}]}`)
	writeFile(t, filepath.Join(root, "data", "scavenging_tables.json"),
		`{"schema_version":1,"tables":[{"id":"salvage_common"}]}`)
	writeFile(t, filepath.Join(root, "data", "expeditions.json"),
		`{"schema_version":1,"expeditions":[{"id":"loc_a","lootCategories":["bandage"],"scavenging_table_id":"salvage_common"}]}`)
}

func TestRunCleanCorpus(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(rep.Findings) != 0 {
		t.Fatalf("expected no findings, got %+v", rep.Findings)
	}
}

func TestReferenceIntegrityCatchesDanglingLootCategory(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "expeditions.json"),
		`{"schema_version":1,"expeditions":[{"id":"loc_a","lootCategories":["bandages"],"scavenging_table_id":"salvage_common"}]}`)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "reference_integrity", "expeditions.json:lootCategories:bandages") {
		t.Fatalf("expected dangling lootCategory finding, got %+v", rep.Findings)
	}
}

func TestDuplicateIDsAcrossDomains(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "shelter_location.json"),
		`{"schema_version":1,"locations":[{"id":"bandage"}]}`)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "duplicate_ids", "bandage") {
		t.Fatalf("expected cross-domain duplicate finding, got %+v", rep.Findings)
	}
}

func TestDuplicateIDsAllowlistSuppresses(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "shelter_location.json"),
		`{"schema_version":1,"locations":[{"id":"bandage"}]}`)
	p := testPolicy()
	p.DuplicateIDAllowlist = []string{"bandage"}
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if hasFinding(rep, "duplicate_ids", "bandage") {
		t.Fatalf("allowlisted duplicate must be suppressed, got %+v", rep.Findings)
	}
}

func TestNamingConventionCatchesBadID(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "verdict_items.json"),
		`{"schema_version":1,"items":[{"id":"Bad-ID"}]}`)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "id_naming", "verdict_items.json:Bad-ID") {
		t.Fatalf("expected naming finding, got %+v", rep.Findings)
	}
}

func TestSchemaVersionDrift(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "items.json"),
		`{"schema_version":1,"items":[{"id":"bandage"}]}`)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "schema_version_drift", "items.json:1") {
		t.Fatalf("expected schema drift finding, got %+v", rep.Findings)
	}
}

func TestBaselineSuppressesAndRoundTrips(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "expeditions.json"),
		`{"schema_version":1,"expeditions":[{"id":"loc_a","lootCategories":["bandages"],"scavenging_table_id":"salvage_common"}]}`)
	first, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(first.Findings) == 0 {
		t.Fatal("expected a finding to baseline")
	}
	baseline := BaselineFromReport(first)
	second, err := Run(root, testPolicy(), baseline)
	if err != nil {
		t.Fatalf("Run2: %v", err)
	}
	if len(second.Findings) != 0 {
		t.Fatalf("baselined finding must be suppressed, got %+v", second.Findings)
	}
	if len(second.StaleBaseline) != 0 {
		t.Fatalf("no baseline entry should be stale, got %v", second.StaleBaseline)
	}
}

func TestStaleBaselineReported(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	b := NewBaseline()
	b.Accepted["id_naming"] = []string{"gone.json:ghost"}
	rep, err := Run(root, testPolicy(), b)
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(rep.StaleBaseline) != 1 || rep.StaleBaseline[0] != "id_naming:gone.json:ghost" {
		t.Fatalf("expected one stale baseline entry, got %v", rep.StaleBaseline)
	}
}

// Loop-1 hardening: a malformed catalog must never disappear as an empty
// catalog. It is reported separately and fails a --check run.
func TestMalformedCatalogIsReportedAsParseError(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "broken.json"), `{"schema_version":1,"items":[}`)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(rep.ParseErrors) != 1 || rep.ParseErrors[0].File != "broken.json" {
		t.Fatalf("expected one parse error for broken.json, got %+v", rep.ParseErrors)
	}
}

// Loop-1 hardening: an allowlisted duplicate is policy-acknowledged and must
// NOT leak into a regenerated baseline (the two acknowledgement mechanisms stay
// separate).
func TestBaselineFromReportExcludesPolicyAllowlist(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "shelter_location.json"),
		`{"schema_version":1,"locations":[{"id":"bandage"}]}`)
	p := testPolicy()
	p.DuplicateIDAllowlist = []string{"bandage"}
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	b := BaselineFromReport(rep)
	if len(b.Accepted["duplicate_ids"]) != 0 {
		t.Fatalf("policy-acknowledged duplicate must not enter the baseline, got %v", b.Accepted)
	}
}

// Loop-1 hardening: re-generating the baseline preserves findings that were
// already suppressed by the previous baseline.
func TestBaselineRoundTripPreservesExistingAcceptance(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "verdict_items.json"),
		`{"schema_version":1,"items":[{"id":"Bad-ID"}]}`)
	first, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	b := BaselineFromReport(first)
	second, err := Run(root, testPolicy(), b)
	if err != nil {
		t.Fatalf("Run2: %v", err)
	}
	regenerated := BaselineFromReport(second)
	if len(regenerated.Accepted["id_naming"]) != 1 {
		t.Fatalf("regenerated baseline dropped an accepted finding: %+v", regenerated.Accepted)
	}
}

// Loop-3 hardening: the shipped policy + corpus must stay clean. This runs the
// real policy over the real data directory so a future data/policy drift is
// caught by `go test` as well as by the CI gate.
func TestRealCorpusPolicyIsClean(t *testing.T) {
	dir, err := os.Getwd()
	if err != nil {
		t.Fatalf("getwd: %v", err)
	}
	root := ""
	for i := 0; i < 8; i++ {
		if _, err := os.Stat(filepath.Join(dir, "docs", "ci", "catalog_audit_policy.json")); err == nil {
			root = dir
			break
		}
		parent := filepath.Dir(dir)
		if parent == dir {
			break
		}
		dir = parent
	}
	if root == "" {
		t.Skip("repository root with docs/ci/catalog_audit_policy.json not found")
	}
	policy, err := LoadPolicy(filepath.Join(root, "docs", "ci", "catalog_audit_policy.json"))
	if err != nil {
		t.Fatalf("LoadPolicy: %v", err)
	}
	baseline, err := LoadBaseline(filepath.Join(root, "docs", "ci", "catalog_audit_baseline.json"))
	if err != nil {
		t.Fatalf("LoadBaseline: %v", err)
	}
	// G14 — the shipped baseline must be empty: acknowledged values belong in a
	// policy allowlist (reviewed, documented), never hidden in the baseline.
	if len(baseline.Accepted) != 0 {
		t.Fatalf("shipped baseline must be empty, got %v", baseline.Accepted)
	}
	// F15 — policy/baseline format discipline: a policy schema bump must be
	// mirrored on the baseline so the two never silently diverge.
	if policy.SchemaVersion != baseline.SchemaVersion {
		t.Fatalf("policy schema_version %q must match baseline schema_version %q", policy.SchemaVersion, baseline.SchemaVersion)
	}
	rep, err := Run(root, policy, baseline)
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(rep.Findings) != 0 {
		t.Fatalf("shipped corpus has unbaselined catalog findings: %+v", rep.Findings)
	}
	if len(rep.ParseErrors) != 0 {
		t.Fatalf("shipped corpus has unreadable catalogs: %+v", rep.ParseErrors)
	}
	if len(rep.StaleBaseline) != 0 {
		t.Fatalf("shipped policy has stale acknowledgements: %v", rep.StaleBaseline)
	}
	// E12 — shrink-only: the policy allowlist must not grow beyond the 10
	// intentional cross-domain reuse ids without a reviewed change.
	if len(rep.Accepted) > 10 {
		t.Fatalf("acknowledged duplicate ids grew beyond 10: %+v", rep.Accepted)
	}
	for _, f := range rep.Accepted {
		if f.Source != "policy" {
			t.Fatalf("shipped corpus should have no baseline-only acceptances, got %+v", f)
		}
	}
}

// Loop-3 hardening: a policy that targets an undeclared domain must be
// rejected at load time rather than silently skipping the reference check.
func TestValidatePolicyRejectsUndeclaredTargetDomain(t *testing.T) {
	root := t.TempDir()
	writeFile(t, filepath.Join(root, "policy.json"), `{
		"schema_version":"1.0.0",
		"id_naming_regex":"^[a-z0-9_]+$",
		"id_domains":[{"domain":"item","globs":["items.json"]}],
		"reference_rules":[{"source_glob":"expeditions.json","field":"lootCategories","target_domain":"typo_domain"}]
	}`)
	if _, err := LoadPolicy(filepath.Join(root, "policy.json")); err == nil {
		t.Fatal("expected LoadPolicy to reject an undeclared target domain")
	}
}

// Loop-3 hardening: an uncompilable naming regex must be rejected.
func TestValidatePolicyRejectsBadRegex(t *testing.T) {
	root := t.TempDir()
	writeFile(t, filepath.Join(root, "policy.json"), `{
		"schema_version":"1.0.0",
		"id_naming_regex":"^[a-z",
		"id_domains":[{"domain":"item","globs":["items.json"]}]
	}`)
	if _, err := LoadPolicy(filepath.Join(root, "policy.json")); err == nil {
		t.Fatal("expected LoadPolicy to reject an uncompilable id_naming_regex")
	}
}

// E05 — excluded reference contracts are echoed in the report.
func TestReferenceRulesExcludedAreEchoed(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	p := testPolicy()
	p.ReferenceRulesExcluded = []ExcludedRule{{SourceGlob: "locations_expansion3.json", Field: "lootCategories", Reason: "abstract tags"}}
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(rep.ReferenceRulesExcluded) != 1 || rep.ReferenceRulesExcluded[0].Field != "lootCategories" {
		t.Fatalf("excluded rules not echoed: %+v", rep.ReferenceRulesExcluded)
	}
}

// E06 — unit-suffix ids produce a non-failing advisory.
func TestUnitSuffixAdvisory(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	writeFile(t, filepath.Join(root, "data", "verdict_items.json"),
		`{"schema_version":1,"items":[{"id":"copper_wire_10m_of_10m"},{"id":"plain_item"}]}`)
	rep, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if len(rep.Findings) != 0 {
		t.Fatalf("advisory must not become a finding: %+v", rep.Findings)
	}
	found := false
	for _, a := range rep.Advisories {
		if a.Check == "id_unit_suffix" && a.Key == "copper_wire_10m_of_10m" {
			found = true
		}
	}
	if !found {
		t.Fatalf("expected unit-suffix advisory, got %+v", rep.Advisories)
	}
}

// E07 — mirror resolution is reported as an advisory.
func TestMirrorResolutionAdvisory(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	p := testPolicy()
	p.IDDomains = append(p.IDDomains, DomainRule{Domain: "mirror", Globs: []string{"asset_registry.json"}})
	writeFile(t, filepath.Join(root, "data", "asset_registry.json"),
		`{"schema_version":1,"assets":[{"id":"bandage"},{"id":"asset_only_id"}]}`)
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if rep.MirrorResolution == nil || rep.MirrorResolution.MirrorUnresolved != 1 {
		t.Fatalf("expected 1 unresolved mirror id, got %+v", rep.MirrorResolution)
	}
	if len(rep.Findings) != 0 {
		t.Fatalf("mirror advisory must not become a finding: %+v", rep.Findings)
	}
}

// F01 — a reference rule whose source glob matches no catalog is dead policy.
func TestPolicySourceGlobMustMatchACatalog(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	p := testPolicy()
	p.ReferenceRules = []ReferenceRule{{SourceGlob: "does_not_exist.json", Field: "lootCategories", TargetDomain: "item"}}
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "reference_integrity", "policy:lootCategories:source-glob-does_not_exist.json") {
		t.Fatalf("expected dead-policy finding, got %+v", rep.Findings)
	}
}

// F02 — the allowlist must be sorted for deterministic output.
func TestValidatePolicyRejectsUnsortedAllowlist(t *testing.T) {
	root := t.TempDir()
	writeFile(t, filepath.Join(root, "policy.json"), `{"schema_version":"1.0.0","id_naming_regex":"^[a-z0-9_]+$","id_domains":[{"domain":"item","globs":["items.json"]}],"duplicate_id_allowlist":["b_id","a_id"]}`)
	if _, err := LoadPolicy(filepath.Join(root, "policy.json")); err == nil {
		t.Fatal("expected an unsorted allowlist to be rejected")
	}
}

// F02 — duplicate allowlist entries must be rejected.
func TestValidatePolicyRejectsDuplicateAllowlist(t *testing.T) {
	root := t.TempDir()
	writeFile(t, filepath.Join(root, "policy.json"), `{"schema_version":"1.0.0","id_naming_regex":"^[a-z0-9_]+$","id_domains":[{"domain":"item","globs":["items.json"]}],"duplicate_id_allowlist":["a_id","a_id"]}`)
	if _, err := LoadPolicy(filepath.Join(root, "policy.json")); err == nil {
		t.Fatal("expected a duplicate allowlist entry to be rejected")
	}
}

// F03 — HasStale mirrors the stale-baseline state used by --strict-stale.
func TestHasStale(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	b := NewBaseline()
	b.Accepted["id_naming"] = []string{"gone.json:ghost"}
	rep, err := Run(root, testPolicy(), b)
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !rep.HasStale() {
		t.Fatal("expected HasStale to be true")
	}
	clean, err := Run(root, testPolicy(), NewBaseline())
	if err != nil {
		t.Fatalf("Run2: %v", err)
	}
	if clean.HasStale() {
		t.Fatal("expected HasStale to be false")
	}
}

// G02 — a container-scoped rule only checks the named root array.
func TestReferenceRuleContainerScoping(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	p := testPolicy()
	p.IDDomains = append(p.IDDomains, DomainRule{Domain: "canonical_item", Globs: []string{"items.json"}})
	p.ReferenceRules = []ReferenceRule{{SourceGlob: "combat_catalog.json", Field: "id", Container: "ammo", TargetDomain: "canonical_item"}}
	writeFile(t, filepath.Join(root, "data", "combat_catalog.json"),
		`{"schema_version":1,"ammo":[{"id":"bandage"},{"id":"ammo_missing"}],"weapons":[{"id":"weapon_missing"}]}`)
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "reference_integrity", "combat_catalog.json:id:ammo_missing") {
		t.Fatalf("expected ammo finding, got %+v", rep.Findings)
	}
	if hasFinding(rep, "reference_integrity", "combat_catalog.json:id:weapon_missing") {
		t.Fatalf("weapons must be out of scope, got %+v", rep.Findings)
	}
}

// G02 — a missing container fails closed rather than silently checking nothing.
func TestReferenceRuleMissingContainerFailsClosed(t *testing.T) {
	root := t.TempDir()
	seedValid(t, root)
	p := testPolicy()
	p.IDDomains = append(p.IDDomains, DomainRule{Domain: "canonical_item", Globs: []string{"items.json"}})
	p.ReferenceRules = []ReferenceRule{{SourceGlob: "combat_catalog.json", Field: "id", Container: "ammo", TargetDomain: "canonical_item"}}
	writeFile(t, filepath.Join(root, "data", "combat_catalog.json"), `{"schema_version":1,"weapons":[{"id":"bandage"}]}`)
	rep, err := Run(root, p, NewBaseline())
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if !hasFinding(rep, "reference_integrity", "policy:id:container-ammo") {
		t.Fatalf("expected missing-container finding, got %+v", rep.Findings)
	}
}

// G11 — a field cannot be both enforced and excluded.
func TestValidatePolicyRejectsEnforcedExcludedOverlap(t *testing.T) {
	root := t.TempDir()
	writeFile(t, filepath.Join(root, "policy.json"), `{
		"schema_version":"1.0.0",
		"id_naming_regex":"^[a-z0-9_]+$",
		"id_domains":[{"domain":"item","globs":["items.json"]}],
		"reference_rules":[{"source_glob":"x.json","field":"id","target_domain":"item"}],
		"reference_rules_excluded":[{"source_glob":"x.json","field":"id","reason":"contradiction"}]
	}`)
	if _, err := LoadPolicy(filepath.Join(root, "policy.json")); err == nil {
		t.Fatal("expected enforced/excluded overlap to be rejected")
	}
}

// G11 — an excluded rule must carry a reason.
func TestValidatePolicyRejectsExcludedWithoutReason(t *testing.T) {
	root := t.TempDir()
	writeFile(t, filepath.Join(root, "policy.json"), `{
		"schema_version":"1.0.0",
		"id_naming_regex":"^[a-z0-9_]+$",
		"id_domains":[{"domain":"item","globs":["items.json"]}],
		"reference_rules_excluded":[{"source_glob":"x.json","field":"id","reason":""}]
	}`)
	if _, err := LoadPolicy(filepath.Join(root, "policy.json")); err == nil {
		t.Fatal("expected an excluded rule without a reason to be rejected")
	}
}

func hasFinding(rep *Report, check, key string) bool {
	for _, f := range rep.Findings {
		if f.Check == check && f.Key == key {
			return true
		}
	}
	return false
}
