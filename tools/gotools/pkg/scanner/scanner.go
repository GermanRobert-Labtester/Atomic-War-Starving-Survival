package scanner

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"os"
	"path/filepath"
	"regexp"
	"strings"
)

type SaveStoreDeclaration struct {
	ClassName   string `json:"class_name"`
	SourceFile  string `json:"source_file"`
	SectionName string `json:"section_name"`
	SavePath    string `json:"save_path"`
	HasChecksum bool   `json:"has_checksum"`
}

type SaveFileScanResult struct {
	FilePath       string `json:"file_path"`
	SectionName    string `json:"section_name"`
	SchemaVersion  int    `json:"schema_version"`
	ByteSize       int64  `json:"byte_size"`
	ChecksumValid  bool   `json:"checksum_valid"`
	ComputedSHA256 string `json:"computed_sha256"`
	Status         string `json:"status"`
}

type SaveAuditReport struct {
	DiscoveredStores []SaveStoreDeclaration `json:"discovered_stores"`
	ScannedFiles     []SaveFileScanResult   `json:"scanned_files"`
	StoreCount       int                    `json:"store_count"`
	FileCount        int                    `json:"file_count"`
}

var (
	reClass = regexp.MustCompile(`public\s+(?:static\s+|sealed\s+)?class\s+([A-Za-z0-9_]*SaveStore[A-Za-z0-9_]*|[A-Za-z0-9_]+)\b`)
	reSection = regexp.MustCompile(`public\s+const\s+string\s+SectionName\s*=\s*"([^"]+)"`)
	reSavePath = regexp.MustCompile(`(?:SavePath|FileName)\s*=\s*"([^"]+)"`)
)

func DiscoverSaveStores(repoRoot string) ([]SaveStoreDeclaration, error) {
	var stores []SaveStoreDeclaration

	searchDirs := []string{
		filepath.Join(repoRoot, "src"),
		filepath.Join(repoRoot, "Assets", "Ashfall.Core"),
	}

	for _, sdir := range searchDirs {
		_ = filepath.WalkDir(sdir, func(p string, d os.DirEntry, err error) error {
			if err != nil || d.IsDir() {
				if d != nil && d.IsDir() && (d.Name() == "bin" || d.Name() == "obj") {
					return filepath.SkipDir
				}
				return nil
			}
			if !strings.HasSuffix(d.Name(), ".cs") || strings.Contains(d.Name(), "Test") {
				return nil
			}

			content, err := os.ReadFile(p)
			if err != nil {
				return nil
			}
			str := string(content)

			if strings.Contains(str, "SaveStore") || (strings.Contains(str, "SectionName") && strings.Contains(str, "SavePath")) {
				rel, _ := filepath.Rel(repoRoot, p)
				cMatches := reClass.FindAllStringSubmatch(str, -1)
				secMatches := reSection.FindStringSubmatch(str)
				pathMatches := reSavePath.FindStringSubmatch(str)

				cName := ""
				if len(cMatches) > 0 {
					cName = cMatches[0][1]
				}
				sec := ""
				if len(secMatches) > 1 {
					sec = secMatches[1]
				}
				sp := ""
				if len(pathMatches) > 1 {
					sp = pathMatches[1]
				}

				if cName != "" && (sec != "" || sp != "" || strings.HasSuffix(cName, "SaveStore")) {
					stores = append(stores, SaveStoreDeclaration{
						ClassName:   cName,
						SourceFile:  filepath.ToSlash(rel),
						SectionName: sec,
						SavePath:    sp,
						HasChecksum: strings.Contains(str, "Checksum") || strings.Contains(str, "checksum"),
					})
				}
			}
			return nil
		})
	}
	return stores, nil
}

func ScanSaveDirectory(saveDir string) ([]SaveFileScanResult, error) {
	var results []SaveFileScanResult
	if _, err := os.Stat(saveDir); os.IsNotExist(err) {
		return results, nil
	}

	_ = filepath.WalkDir(saveDir, func(p string, d os.DirEntry, err error) error {
		if err != nil || d.IsDir() {
			return nil
		}
		if !strings.HasSuffix(d.Name(), ".json") {
			return nil
		}

		data, err := os.ReadFile(p)
		if err != nil {
			return nil
		}

		h := sha256.Sum256(data)
		hStr := hex.EncodeToString(h[:])

		var envelope map[string]interface{}
		_ = json.Unmarshal(data, &envelope)

		schemaVer := 0
		if sv, ok := envelope["schema_version"].(float64); ok {
			schemaVer = int(sv)
		}

		status := "OK"
		if schemaVer == 0 {
			status = "MISSING_SCHEMA_VERSION"
		}

		results = append(results, SaveFileScanResult{
			FilePath:       p,
			ByteSize:       int64(len(data)),
			SchemaVersion:  schemaVer,
			ComputedSHA256: hStr,
			Status:         status,
		})
		return nil
	})

	return results, nil
}
