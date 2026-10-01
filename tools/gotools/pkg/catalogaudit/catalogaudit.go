// Package catalogaudit implements the read-only catalog-hygiene audit that
// backs the `ashfall-dev audit-catalogs` command and the `catalog_audit` CI
// gate.
//
// It is a fast, static pre-flight over the authored JSON under
// Assets/StreamingAssets/Data and deliberately does NOT become a gameplay or
// runtime authority: Ashfall.Core.CatalogIntegrityValidator remains the
// canonical runtime cross-reference gate (--data-integrity-selftest). The four
// checks here are:
//
//	D01 reference_integrity    declared reference fields resolve to a target domain
//	D02 duplicate_ids          an id defined across two different id-domains
//	D03 id_naming              ids conform to the canonical snake_case regex
//	D04 schema_version_drift   a catalog's schema_version matches its expected value
//
// A findings baseline (docs/ci/catalog_audit_baseline.json) holds acknowledged
// findings; --check fails only when a finding is NOT in the baseline, so the
// gate ratchets against new drift without churning the existing corpus.
package catalogaudit

import (
	"encoding/json"
	"fmt"
	"os"
	"path/filepath"
	"regexp"
	"sort"
	"strings"
)

// DomainRule maps a set of catalog filename globs to a logical id-domain.
type DomainRule struct {
	Domain string   `json:"domain"`
	Globs  []string `json:"globs"`
	Note   string   `json:"note,omitempty"`
}

// ReferenceRule declares that every value of Field in a matching source catalog
// must resolve to an id authored in TargetDomain.
type ReferenceRule struct {
	SourceGlob   string `json:"source_glob"`
	Field        string `json:"field"`
	TargetDomain string `json:"target_domain"`
	Note         string `json:"note,omitempty"`
	// Allowlist documents values that are deliberately out of the target domain
	// (e.g. grandfathered market-projection ids). Sorted + unique.
	Allowlist []string `json:"allowlist,omitempty"`
	// Container, when set, scopes the check to the named root array (e.g. the
	// `ammo` array of combat_catalog.json) instead of the whole document.
	Container string `json:"container,omitempty"`
}

// ExcludedRule documents a reference-shaped field the policy deliberately does
// not enforce (e.g. a catalog that uses abstract category tags instead of item
// ids). It is echoed in the JSON report for traceability.
type ExcludedRule struct {
	SourceGlob string `json:"source_glob"`
	Field      string `json:"field"`
	Reason     string `json:"reason"`
}

// MirrorAdvisory reports how many ids in mirror catalogs (asset_registry,
// questline_master) have no primary-domain author. Non-failing: mirrors
// legitimately own asset-only and quest-registry-only ids.
type MirrorAdvisory struct {
	MirrorCatalogs   int      `json:"mirror_catalogs"`
	MirrorUnresolved int      `json:"mirror_unresolved"`
	Sample           []string `json:"sample,omitempty"`
}

// Policy is the authored audit policy (docs/ci/catalog_audit_policy.json).
type Policy struct {
	SchemaVersion             string          `json:"schema_version"`
	Description               string          `json:"description"`
	DataDir                   string          `json:"data_dir"`
	IDNamingRegex             string          `json:"id_naming_regex"`
	UnitSuffixAdvisoryRegex   string          `json:"unit_suffix_advisory_regex"`
	DefaultDomain             string          `json:"default_domain"`
	IDDomains                 []DomainRule    `json:"id_domains"`
	DuplicateIDAllowlist      []string        `json:"duplicate_id_allowlist"`
	ReferenceRules            []ReferenceRule `json:"reference_rules"`
	ReferenceRulesExcluded    []ExcludedRule  `json:"reference_rules_excluded"`
	AdvisoryAllowlist         []string        `json:"advisory_allowlist"`
	MirrorUnresolvedMax       int             `json:"mirror_unresolved_max"`
	SchemaVersionDefault      int             `json:"schema_version_default"`
	SchemaVersionExpectations map[string]int  `json:"schema_version_expectations"`
}

// Finding is one audit observation. Key is the stable baseline identity.
type Finding struct {
	Check  string `json:"check"`
	Key    string `json:"key"`
	File   string `json:"file,omitempty"`
	Detail string `json:"detail,omitempty"`
	// Source is "policy" when the finding is acknowledged by the policy
	// allowlist, or "baseline" when suppressed by the findings baseline. Empty
	// means the finding is new/unacknowledged.
	Source string `json:"source,omitempty"`
}

// Baseline lists acknowledged findings per check.
type Baseline struct {
	SchemaVersion string              `json:"schema_version"`
	Accepted      map[string][]string `json:"accepted"`
}

// Report is the machine-readable audit result.
type Report struct {
	FilesScanned           int             `json:"files_scanned"`
	IDsIndexed             int             `json:"ids_indexed"`
	Findings               []Finding       `json:"findings"`
	Accepted               []Finding       `json:"accepted,omitempty"`
	Advisories             []Finding       `json:"advisories,omitempty"`
	StaleBaseline          []string        `json:"stale_baseline,omitempty"`
	ParseErrors            []Finding       `json:"parse_errors,omitempty"`
	ReferenceRulesExcluded []ExcludedRule  `json:"reference_rules_excluded,omitempty"`
	MirrorResolution       *MirrorAdvisory `json:"mirror_resolution,omitempty"`
	Checked                map[string]int  `json:"checked"`
}

// NewBaseline returns an empty baseline.
func NewBaseline() Baseline {
	return Baseline{SchemaVersion: "1.0.0", Accepted: map[string][]string{}}
}

// HasStale reports whether any baseline or allowlist acknowledgement no longer
// describes a live finding.
func (r *Report) HasStale() bool { return len(r.StaleBaseline) > 0 }

// LoadPolicy reads and validates a policy file.
func LoadPolicy(path string) (Policy, error) {
	var p Policy
	raw, err := os.ReadFile(path)
	if err != nil {
		return p, fmt.Errorf("read policy %s: %w", path, err)
	}
	if err := json.Unmarshal(raw, &p); err != nil {
		return p, fmt.Errorf("parse policy %s: %w", path, err)
	}
	if strings.TrimSpace(p.IDNamingRegex) == "" {
		return p, fmt.Errorf("policy %s: id_naming_regex is required", path)
	}
	if p.DefaultDomain == "" {
		p.DefaultDomain = "unclassified"
	}
	if p.DataDir == "" {
		p.DataDir = "Assets/StreamingAssets/Data"
	}
	if p.SchemaVersionDefault == 0 {
		p.SchemaVersionDefault = 1
	}
	if strings.TrimSpace(p.UnitSuffixAdvisoryRegex) == "" {
		p.UnitSuffixAdvisoryRegex = `_[0-9]+[a-z]+_of_[0-9]+[a-z]+$`
	}
	if err := validatePolicy(p); err != nil {
		return p, fmt.Errorf("policy %s: %w", path, err)
	}
	return p, nil
}

// validatePolicy rejects a policy that cannot be evaluated faithfully. This
// fails closed: a misspelled target domain or an uncompilable naming regex must
// break the build, never silently pass as "no reference rules" or "no ids".
func validatePolicy(p Policy) error {
	if strings.TrimSpace(p.SchemaVersion) == "" {
		return fmt.Errorf("schema_version is required")
	}
	if len(p.IDDomains) == 0 {
		return fmt.Errorf("at least one id_domains entry is required")
	}
	if _, err := regexp.Compile(p.IDNamingRegex); err != nil {
		return fmt.Errorf("id_naming_regex %q: %w", p.IDNamingRegex, err)
	}
	// Determinism: the allowlists must be sorted and duplicate-free so a
	// regenerated report/baseline is byte-stable.
	if err := checkSortedUnique("duplicate_id_allowlist", p.DuplicateIDAllowlist); err != nil {
		return err
	}
	if err := checkSortedUnique("advisory_allowlist", p.AdvisoryAllowlist); err != nil {
		return err
	}
	if p.MirrorUnresolvedMax < 0 {
		return fmt.Errorf("mirror_unresolved_max must be >= 0 (0 disables the ratchet)")
	}
	declared := map[string]bool{}
	for _, d := range p.IDDomains {
		if strings.TrimSpace(d.Domain) == "" {
			return fmt.Errorf("id_domains entry has an empty domain")
		}
		declared[d.Domain] = true
	}
	enforced := map[string]bool{}
	for _, r := range p.ReferenceRules {
		if strings.TrimSpace(r.SourceGlob) == "" {
			return fmt.Errorf("reference rule for field %q has an empty source_glob", r.Field)
		}
		if strings.TrimSpace(r.Field) == "" {
			return fmt.Errorf("reference rule for source_glob %q has an empty field", r.SourceGlob)
		}
		if !declared[r.TargetDomain] {
			return fmt.Errorf("reference rule %s.%s targets undeclared domain %q", r.SourceGlob, r.Field, r.TargetDomain)
		}
		if err := checkSortedUnique("reference_rules.allowlist for "+r.SourceGlob+"."+r.Field, r.Allowlist); err != nil {
			return err
		}
		enforced[r.SourceGlob+"|"+r.Field] = true
	}
	for _, r := range p.ReferenceRulesExcluded {
		if strings.TrimSpace(r.SourceGlob) == "" || strings.TrimSpace(r.Field) == "" || strings.TrimSpace(r.Reason) == "" {
			return fmt.Errorf("reference_rules_excluded entry needs source_glob, field, and reason")
		}
		if enforced[r.SourceGlob+"|"+r.Field] {
			return fmt.Errorf("field %s.%s is both enforced and excluded", r.SourceGlob, r.Field)
		}
	}
	return nil
}

// checkSortedUnique enforces the determinism contract for authored allowlists.
func checkSortedUnique(name string, items []string) error {
	seen := map[string]bool{}
	prev := ""
	for _, item := range items {
		if seen[item] {
			return fmt.Errorf("%s contains duplicate %q", name, item)
		}
		if prev != "" && item < prev {
			return fmt.Errorf("%s must be sorted (found %q before %q)", name, prev, item)
		}
		seen[item] = true
		prev = item
	}
	return nil
}

// LoadBaseline reads a baseline file; a missing file yields an empty baseline.
func LoadBaseline(path string) (Baseline, error) {
	b := NewBaseline()
	raw, err := os.ReadFile(path)
	if err != nil {
		if os.IsNotExist(err) {
			return b, nil
		}
		return b, fmt.Errorf("read baseline %s: %w", path, err)
	}
	if err := json.Unmarshal(raw, &b); err != nil {
		return b, fmt.Errorf("parse baseline %s: %w", path, err)
	}
	if b.Accepted == nil {
		b.Accepted = map[string][]string{}
	}
	return b, nil
}

// WriteBaseline serializes a baseline to disk.
func WriteBaseline(path string, b Baseline) error {
	if b.SchemaVersion == "" {
		b.SchemaVersion = "1.0.0"
	}
	for k := range b.Accepted {
		sort.Strings(b.Accepted[k])
	}
	raw, err := json.MarshalIndent(b, "", "  ")
	if err != nil {
		return err
	}
	raw = append(raw, '\n')
	if err := os.MkdirAll(filepath.Dir(path), 0o755); err != nil {
		return err
	}
	return os.WriteFile(path, raw, 0o644)
}

type idRef struct {
	ID      string
	Pointer string
}

type catalogFile struct {
	Name          string
	Domain        string
	IDs           []idRef
	SchemaVersion *int
}

// Run executes the audit and classifies findings against the baseline.
func Run(root string, p Policy, baseline Baseline) (*Report, error) {
	re, err := regexp.Compile(p.IDNamingRegex)
	if err != nil {
		return nil, fmt.Errorf("invalid id_naming_regex %q: %w", p.IDNamingRegex, err)
	}

	dataDir := p.DataDir
	if !filepath.IsAbs(dataDir) {
		dataDir = filepath.Join(root, dataDir)
	}
	entries, err := os.ReadDir(dataDir)
	if err != nil {
		return nil, fmt.Errorf("read data dir %s: %w", dataDir, err)
	}

	report := &Report{Checked: map[string]int{}}
	files := make([]catalogFile, 0, len(entries))
	parseErrors := []Finding{}
	for _, e := range entries {
		if e.IsDir() || !strings.HasSuffix(strings.ToLower(e.Name()), ".json") {
			continue
		}
		report.FilesScanned++
		cf := catalogFile{Name: e.Name(), Domain: domainFor(p, e.Name())}
		raw, err := os.ReadFile(filepath.Join(dataDir, e.Name()))
		if err != nil {
			parseErrors = append(parseErrors, Finding{Check: "parse_error", Key: e.Name(), File: e.Name(), Detail: err.Error()})
			continue
		}
		var doc interface{}
		if err := json.Unmarshal(raw, &doc); err != nil {
			parseErrors = append(parseErrors, Finding{Check: "parse_error", Key: e.Name(), File: e.Name(), Detail: err.Error()})
			continue
		}
		walk(doc, "", func(key string, value interface{}) {
			switch key {
			case "id":
				if s, ok := value.(string); ok && s != "" {
					cf.IDs = append(cf.IDs, idRef{ID: s, Pointer: ""})
				}
			case "schema_version":
				if n, ok := value.(float64); ok && n == float64(int(n)) {
					v := int(n)
					cf.SchemaVersion = &v
				}
			}
		})
		report.IDsIndexed += len(cf.IDs)
		files = append(files, cf)
	}
	report.ParseErrors = parseErrors

	// Reference-target id sets are built from each domain rule's globs, so a
	// file may contribute to several domains (items.json is both the merged
	// `item` surface and the `canonical_item` authority). Duplicate detection
	// below still uses each file's single assigned domain.
	targetIDs := map[string]map[string]bool{}
	for _, rule := range p.IDDomains {
		set := map[string]bool{}
		for _, f := range files {
			matched := false
			for _, g := range rule.Globs {
				if globMatch(g, f.Name) {
					matched = true
					break
				}
			}
			if !matched {
				continue
			}
			for _, r := range f.IDs {
				set[r.ID] = true
			}
		}
		targetIDs[rule.Domain] = set
	}

	// Assigned-domain id sets (mirror excluded) feed the mirror-resolution
	// advisory, which asks whether an id has a primary (non-mirror) author.
	assignedIDs := map[string]map[string]bool{}
	for _, f := range files {
		if f.Domain == "mirror" {
			continue
		}
		set := assignedIDs[f.Domain]
		if set == nil {
			set = map[string]bool{}
			assignedIDs[f.Domain] = set
		}
		for _, r := range f.IDs {
			set[r.ID] = true
		}
	}

	var findings []Finding

	// D01 — reference integrity.
	for _, rule := range p.ReferenceRules {
		targets := targetIDs[rule.TargetDomain]
		if len(targets) == 0 {
			// Misconfigured policy: surface it rather than silently passing.
			findings = append(findings, Finding{
				Check:  "reference_integrity",
				Key:    "policy:" + rule.Field + ":target-domain-" + rule.TargetDomain,
				Detail: fmt.Sprintf("target domain %q has no ids; reference rule cannot be evaluated", rule.TargetDomain),
			})
			continue
		}
		allowed := map[string]bool{}
		for _, a := range rule.Allowlist {
			allowed[a] = true
		}
		matched := 0
		for _, f := range files {
			if !globMatch(rule.SourceGlob, f.Name) {
				continue
			}
			matched++
			report.Checked["reference_integrity"]++
			raw, err := os.ReadFile(filepath.Join(dataDir, f.Name))
			if err != nil {
				continue
			}
			var doc interface{}
			if err := json.Unmarshal(raw, &doc); err != nil {
				continue
			}
			var roots []interface{}
			if rule.Container != "" {
				m, ok := doc.(map[string]interface{})
				if !ok {
					continue
				}
				arr, ok := m[rule.Container].([]interface{})
				if !ok {
					findings = append(findings, Finding{
						Check:  "reference_integrity",
						Key:    "policy:" + rule.Field + ":container-" + rule.Container,
						File:   f.Name,
						Detail: fmt.Sprintf("container %q not found in %s; reference rule cannot be evaluated", rule.Container, f.Name),
					})
					continue
				}
				roots = arr
			} else {
				roots = []interface{}{doc}
			}
			for _, root := range roots {
				walkStringValues(root, rule.Field, func(value string) {
					if !targets[value] && !allowed[value] {
						findings = append(findings, Finding{
							Check:  "reference_integrity",
							Key:    f.Name + ":" + rule.Field + ":" + value,
							File:   f.Name,
							Detail: fmt.Sprintf("%s=%q does not resolve in domain %q", rule.Field, value, rule.TargetDomain),
						})
					}
				})
			}
		}
		// F01 — a reference rule whose source glob matches no catalog is dead
		// policy and must fail closed rather than silently checking nothing.
		if matched == 0 {
			findings = append(findings, Finding{
				Check:  "reference_integrity",
				Key:    "policy:" + rule.Field + ":source-glob-" + rule.SourceGlob,
				Detail: fmt.Sprintf("source_glob %q matched no catalog; reference rule cannot be evaluated", rule.SourceGlob),
			})
		}
	}

	// D02 — cross-domain duplicate ids.
	allow := map[string]bool{}
	for _, id := range p.DuplicateIDAllowlist {
		allow[id] = true
	}
	domainsByID := map[string]map[string]bool{}
	for _, f := range files {
		if f.Domain == "mirror" {
			continue
		}
		for _, r := range f.IDs {
			d := domainsByID[r.ID]
			if d == nil {
				d = map[string]bool{}
				domainsByID[r.ID] = d
			}
			d[f.Domain] = true
		}
	}
	for id, domains := range domainsByID {
		report.Checked["duplicate_ids"]++
		if len(domains) < 2 {
			continue
		}
		ds := make([]string, 0, len(domains))
		for d := range domains {
			ds = append(ds, d)
		}
		sort.Strings(ds)
		detail := "id defined in multiple id-domains: " + strings.Join(ds, ", ")
		if allow[id] {
			// Policy-acknowledged cross-domain reuse: surface it as accepted so
			// it stays visible (and so a stale allowlist entry is detectable).
			report.Accepted = append(report.Accepted, Finding{Check: "duplicate_ids", Key: id, Detail: detail, Source: "policy"})
			continue
		}
		findings = append(findings, Finding{
			Check:  "duplicate_ids",
			Key:    id,
			Detail: detail,
		})
	}

	// A policy allowlist entry that no longer describes an actual cross-domain
	// duplicate is stale and must be pruned (the allowlist ratchets too).
	for id := range allow {
		if len(domainsByID[id]) < 2 {
			report.StaleBaseline = append(report.StaleBaseline, "duplicate_id_allowlist:"+id)
		}
	}

	// D03 — id naming convention.
	for _, f := range files {
		for _, r := range f.IDs {
			report.Checked["id_naming"]++
			if re.MatchString(r.ID) {
				continue
			}
			findings = append(findings, Finding{
				Check:  "id_naming",
				Key:    f.Name + ":" + r.ID,
				File:   f.Name,
				Detail: fmt.Sprintf("id %q does not match %s", r.ID, p.IDNamingRegex),
			})
		}
	}

	// D04 — schema_version value drift.
	for _, f := range files {
		if f.SchemaVersion == nil {
			continue
		}
		report.Checked["schema_version_drift"]++
		expected := p.SchemaVersionDefault
		if v, ok := p.SchemaVersionExpectations[f.Name]; ok {
			expected = v
		}
		if *f.SchemaVersion != expected {
			findings = append(findings, Finding{
				Check:  "schema_version_drift",
				Key:    f.Name + ":" + itoa(*f.SchemaVersion),
				File:   f.Name,
				Detail: fmt.Sprintf("schema_version %d does not match expected %d", *f.SchemaVersion, expected),
			})
		}
	}

	// E05 — echo the deliberately-excluded reference contracts for traceability.
	report.ReferenceRulesExcluded = p.ReferenceRulesExcluded

	// E06 — unit-suffix advisory census (non-failing).
	if unitRe, uerr := regexp.Compile(p.UnitSuffixAdvisoryRegex); uerr == nil {
		seen := map[string]bool{}
		for _, f := range files {
			for _, r := range f.IDs {
				if seen[r.ID] || !unitRe.MatchString(r.ID) {
					continue
				}
				seen[r.ID] = true
				report.Advisories = append(report.Advisories, Finding{
					Check:  "id_unit_suffix",
					Key:    r.ID,
					File:   f.Name,
					Detail: "id carries a redundant unit suffix (advisory, non-failing)",
				})
			}
		}
		sortFindings(report.Advisories)
	}

	// E07 — mirror-resolution advisory (non-failing): how many ids in mirror
	// catalogs have no primary-domain author. Mirrors legitimately own
	// asset-only and quest-registry-only ids, so this is a census, not a gate.
	mirrorCatalogs := 0
	mirrorIDs := map[string]bool{}
	for _, f := range files {
		if f.Domain != "mirror" {
			continue
		}
		mirrorCatalogs++
		for _, r := range f.IDs {
			mirrorIDs[r.ID] = true
		}
	}
	if mirrorCatalogs > 0 {
		unresolved := []string{}
		for id := range mirrorIDs {
			found := false
			for domain, set := range assignedIDs {
				if domain == "mirror" {
					continue
				}
				if set[id] {
					found = true
					break
				}
			}
			if !found {
				unresolved = append(unresolved, id)
			}
		}
		sort.Strings(unresolved)
		sample := unresolved
		if len(sample) > 10 {
			sample = sample[:10]
		}
		report.MirrorResolution = &MirrorAdvisory{
			MirrorCatalogs:   mirrorCatalogs,
			MirrorUnresolved: len(unresolved),
			Sample:           sample,
		}

		// G03 — optional count ratchet: mirror ids without a primary author must
		// not grow past the reviewed policy ceiling (0 disables the ratchet).
		if p.MirrorUnresolvedMax > 0 && len(unresolved) > p.MirrorUnresolvedMax {
			findings = append(findings, Finding{
				Check:  "mirror_resolution_ratchet",
				Key:    fmt.Sprintf("mirror_resolution:%d>%d", len(unresolved), p.MirrorUnresolvedMax),
				Detail: fmt.Sprintf("%d mirror ids lack a primary author, over the policy ceiling %d", len(unresolved), p.MirrorUnresolvedMax),
			})
		}
	}

	// Classify against the baseline.
	acceptedSet := map[string]map[string]bool{}
	for check, keys := range baseline.Accepted {
		s := map[string]bool{}
		for _, k := range keys {
			s[k] = true
		}
		acceptedSet[check] = s
	}
	seenKeys := map[string]map[string]bool{}
	for _, f := range findings {
		if acceptedSet[f.Check][f.Key] {
			f.Source = "baseline"
			report.Accepted = append(report.Accepted, f)
			continue
		}
		report.Findings = append(report.Findings, f)
		if seenKeys[f.Check] == nil {
			seenKeys[f.Check] = map[string]bool{}
		}
		seenKeys[f.Check][f.Key] = true
	}
	// Accepted findings also count as "seen" so they are not reported stale.
	for _, f := range report.Accepted {
		if seenKeys[f.Check] == nil {
			seenKeys[f.Check] = map[string]bool{}
		}
		seenKeys[f.Check][f.Key] = true
	}
	for check, keys := range baseline.Accepted {
		for _, k := range keys {
			if !seenKeys[check][k] {
				report.StaleBaseline = append(report.StaleBaseline, check+":"+k)
			}
		}
	}
	sort.Strings(report.StaleBaseline)

	if report.Findings == nil {
		report.Findings = []Finding{}
	}
	sortFindings(report.Findings)
	sortFindings(report.Accepted)
	return report, nil
}

// BaselineFromReport builds a baseline from every current finding that is not
// already acknowledged by the policy allowlist, so a freshly-generated baseline
// reproduces the current corpus without double-recording policy decisions.
func BaselineFromReport(rep *Report) Baseline {
	b := NewBaseline()
	for _, f := range append(append([]Finding{}, rep.Accepted...), rep.Findings...) {
		if f.Source == "policy" {
			continue
		}
		b.Accepted[f.Check] = append(b.Accepted[f.Check], f.Key)
	}
	return b
}

func sortFindings(fs []Finding) {
	sort.Slice(fs, func(i, j int) bool {
		if fs[i].Check != fs[j].Check {
			return fs[i].Check < fs[j].Check
		}
		return fs[i].Key < fs[j].Key
	})
}

func domainFor(p Policy, name string) string {
	for _, rule := range p.IDDomains {
		for _, g := range rule.Globs {
			if globMatch(g, name) {
				return rule.Domain
			}
		}
	}
	return p.DefaultDomain
}

func globMatch(pattern, name string) bool {
	ok, err := filepath.Match(pattern, name)
	return err == nil && ok
}

func itoa(n int) string {
	if n == 0 {
		return "0"
	}
	neg := n < 0
	if neg {
		n = -n
	}
	var buf [20]byte
	i := len(buf)
	for n > 0 {
		i--
		buf[i] = byte('0' + n%10)
		n /= 10
	}
	if neg {
		i--
		buf[i] = '-'
	}
	return string(buf[i:])
}

// walk visits every key/value pair in the document tree.
func walk(v interface{}, key string, fn func(key string, value interface{})) {
	switch t := v.(type) {
	case map[string]interface{}:
		for k, child := range t {
			fn(k, child)
			walk(child, k, fn)
		}
	case []interface{}:
		for _, child := range t {
			walk(child, key, fn)
		}
	}
}

// walkStringValues visits every value stored under `field`, whether the value
// is a bare string or an array of strings.
func walkStringValues(v interface{}, field string, fn func(string)) {
	switch t := v.(type) {
	case map[string]interface{}:
		for k, child := range t {
			if k == field {
				switch c := child.(type) {
				case string:
					fn(c)
				case []interface{}:
					for _, item := range c {
						if s, ok := item.(string); ok {
							fn(s)
						}
					}
				}
			}
			walkStringValues(child, field, fn)
		}
	case []interface{}:
		for _, child := range t {
			walkStringValues(child, field, fn)
		}
	}
}
