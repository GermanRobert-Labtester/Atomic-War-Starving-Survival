package validator

import (
	"encoding/json"
	"fmt"
	"os"
	"path/filepath"
	"strings"
	"sync"
	"time"
	"unicode/utf8"

	"github.com/goccy/go-yaml"
	"golang.org/x/sync/errgroup"
)

type ValidationViolation struct {
	FilePath string `json:"file_path"`
	Rule     string `json:"rule"`
	Message  string `json:"message"`
}

type ValidationSummary struct {
	TotalChecked int                   `json:"total_checked"`
	ValidCount   int                   `json:"valid_count"`
	InvalidCount int                   `json:"invalid_count"`
	DurationMs   int64                 `json:"duration_ms"`
	Violations   []ValidationViolation `json:"violations,omitempty"`
}

func ValidateJSONFile(path string) []ValidationViolation {
	var violations []ValidationViolation

	data, err := os.ReadFile(path)
	if err != nil {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "readable",
			Message:  fmt.Sprintf("Failed to read file: %v", err),
		})
		return violations
	}

	// 1. Valid UTF-8 check
	if !utf8.Valid(data) {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "utf8_encoding",
			Message:  "File is not valid UTF-8",
		})
		return violations
	}

	trimmed := strings.TrimSpace(string(data))
	if trimmed == "" {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "non_empty",
			Message:  "File is empty",
		})
		return violations
	}

	// 2. Root must be JSON object, never bare array
	if strings.HasPrefix(trimmed, "[") {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "object_root",
			Message:  "Root JSON element is an array; must be an object {}",
		})
		return violations
	}

	var root map[string]interface{}
	if err := json.Unmarshal(data, &root); err != nil {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "valid_json",
			Message:  fmt.Sprintf("JSON parse error: %v", err),
		})
		return violations
	}

	// 3. Schema version must exist and be >= 1 for data catalogs
	rawVer, exists := root["schema_version"]
	if !exists {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "schema_version_present",
			Message:  "Missing mandatory 'schema_version' field",
		})
	} else {
		switch v := rawVer.(type) {
		case float64:
			if v < 1 || float64(int(v)) != v {
				violations = append(violations, ValidationViolation{
					FilePath: path,
					Rule:     "schema_version_integer",
					Message:  fmt.Sprintf("schema_version must be a positive integer >= 1, got %v", v),
				})
			}
		default:
			violations = append(violations, ValidationViolation{
				FilePath: path,
				Rule:     "schema_version_type",
				Message:  fmt.Sprintf("schema_version must be integer, got %T", rawVer),
			})
		}
	}

	return violations
}

func ValidateYAMLFile(path string) []ValidationViolation {
	var violations []ValidationViolation

	data, err := os.ReadFile(path)
	if err != nil {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "readable",
			Message:  fmt.Sprintf("Failed to read file: %v", err),
		})
		return violations
	}

	var parsed interface{}
	if err := yaml.Unmarshal(data, &parsed); err != nil {
		violations = append(violations, ValidationViolation{
			FilePath: path,
			Rule:     "valid_yaml",
			Message:  fmt.Sprintf("YAML parse error: %v", err),
		})
	}
	return violations
}

func ValidateDirectory(dir string) (*ValidationSummary, error) {
	start := time.Now()
	summary := &ValidationSummary{}

	var files []string
	err := filepath.WalkDir(dir, func(p string, d os.DirEntry, err error) error {
		if err != nil {
			return nil
		}
		if d.IsDir() {
			if d.Name() == ".git" || d.Name() == "bin" || d.Name() == "obj" {
				return filepath.SkipDir
			}
			return nil
		}
		ext := strings.ToLower(filepath.Ext(d.Name()))
		if ext == ".json" || ext == ".yaml" || ext == ".yml" {
			files = append(files, p)
		}
		return nil
	})

	if err != nil {
		return nil, err
	}

	summary.TotalChecked = len(files)

	var mu sync.Mutex
	fileChan := make(chan string, len(files))
	for _, f := range files {
		fileChan <- f
	}
	close(fileChan)

	var g errgroup.Group
	numWorkers := 16
	for i := 0; i < numWorkers; i++ {
		g.Go(func() error {
			for f := range fileChan {
				var v []ValidationViolation
				if strings.HasSuffix(f, ".json") {
					v = ValidateJSONFile(f)
				} else {
					v = ValidateYAMLFile(f)
				}

				mu.Lock()
				if len(v) == 0 {
					summary.ValidCount++
				} else {
					summary.InvalidCount++
					summary.Violations = append(summary.Violations, v...)
				}
				mu.Unlock()
			}
			return nil
		})
	}

	_ = g.Wait()
	summary.DurationMs = time.Since(start).Milliseconds()
	return summary, nil
}
