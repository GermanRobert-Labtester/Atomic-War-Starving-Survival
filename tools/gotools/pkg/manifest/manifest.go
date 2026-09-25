package manifest

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"image"
	_ "image/png"
	"io"
	"os"
	"path/filepath"
	"strings"
	"sync"
	"time"
)

type AssetEntry struct {
	ID         string `json:"id"`
	RelativePath string `json:"relative_path"`
	Category   string `json:"category"` // sprite, ui, audio, font, other
	Width      int    `json:"width,omitempty"`
	Height     int    `json:"height,omitempty"`
	SizeBytes  int64  `json:"size_bytes"`
	SHA256     string `json:"sha256"`
}

type AssetManifest struct {
	BuiltAt     time.Time    `json:"built_at"`
	TotalAssets int          `json:"total_assets"`
	TotalBytes  int64        `json:"total_bytes"`
	Assets      []AssetEntry `json:"assets"`
}

func categorizeAsset(path string) string {
	ext := strings.ToLower(filepath.Ext(path))
	switch ext {
	case ".png", ".jpg", ".jpeg", ".webp":
		if strings.Contains(path, "ui") || strings.Contains(path, "UI") {
			return "ui"
		}
		return "sprite"
	case ".wav", ".ogg", ".mp3":
		return "audio"
	case ".ttf", ".otf", ".woff", ".woff2":
		return "font"
	case ".tscn", ".tres":
		return "godot_scene"
	case ".gdshader", ".shader":
		return "shader"
	default:
		return "other"
	}
}

func BuildManifest(repoRoot string, assetDirs []string) (*AssetManifest, error) {
	manifest := &AssetManifest{
		BuiltAt: time.Now().UTC(),
	}

	var candidatePaths []string
	for _, ad := range assetDirs {
		fullDir := filepath.Join(repoRoot, ad)
		if _, err := os.Stat(fullDir); os.IsNotExist(err) {
			continue
		}
		_ = filepath.WalkDir(fullDir, func(p string, d os.DirEntry, err error) error {
			if err != nil || d.IsDir() {
				return nil
			}
			name := strings.ToLower(d.Name())
			if strings.HasSuffix(name, ".import") || strings.HasSuffix(name, ".uid") {
				return nil
			}
			candidatePaths = append(candidatePaths, p)
			return nil
		})
	}

	var mu sync.Mutex
	var wg sync.WaitGroup
	tasks := make(chan string, len(candidatePaths))
	for _, p := range candidatePaths {
		tasks <- p
	}
	close(tasks)

	numWorkers := 8
	for i := 0; i < numWorkers; i++ {
		wg.Add(1)
		go func() {
			defer wg.Done()
			for p := range tasks {
				info, err := os.Stat(p)
				if err != nil {
					continue
				}

				rel, _ := filepath.Rel(repoRoot, p)
				relSlash := filepath.ToSlash(rel)
				cat := categorizeAsset(relSlash)

				id := strings.TrimSuffix(filepath.Base(p), filepath.Ext(p))
				entry := AssetEntry{
					ID:           id,
					RelativePath: relSlash,
					Category:     cat,
					SizeBytes:    info.Size(),
				}

				// Compute hash
				if f, err := os.Open(p); err == nil {
					h := sha256.New()
					_, _ = io.Copy(h, f)
					entry.SHA256 = hex.EncodeToString(h.Sum(nil))
					f.Close()
				}

				// If image, inspect dimensions
				if cat == "sprite" || cat == "ui" {
					if f, err := os.Open(p); err == nil {
						cfg, _, err := image.DecodeConfig(f)
						if err == nil {
							entry.Width = cfg.Width
							entry.Height = cfg.Height
						}
						f.Close()
					}
				}

				mu.Lock()
				manifest.Assets = append(manifest.Assets, entry)
				manifest.TotalAssets++
				manifest.TotalBytes += entry.SizeBytes
				mu.Unlock()
			}
		}()
	}

	wg.Wait()
	return manifest, nil
}

func (m *AssetManifest) SaveJSON(outputPath string) error {
	data, err := json.MarshalIndent(m, "", "  ")
	if err != nil {
		return err
	}
	return os.WriteFile(outputPath, data, 0644)
}
