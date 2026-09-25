package runner

import (
	"bytes"
	"context"
	"fmt"
	"os/exec"
	"syscall"
	"time"

	"golang.org/x/sync/errgroup"
)

type Task struct {
	ID         string        `json:"id"`
	Command    string        `json:"command"`
	Args       []string      `json:"args"`
	WorkingDir string        `json:"working_dir"`
	Timeout    time.Duration `json:"timeout"`
}

type TaskResult struct {
	ID        string        `json:"id"`
	ExitCode  int           `json:"exit_code"`
	Duration  time.Duration `json:"duration"`
	Output    string        `json:"output"`
	PeakRSSKb int64         `json:"peak_rss_kb"`
	Error     string        `json:"error,omitempty"`
	TimedOut  bool          `json:"timed_out"`
}

type TaskRunnerPool struct {
	Concurrency int
}

func NewTaskRunnerPool(concurrency int) *TaskRunnerPool {
	if concurrency <= 0 {
		concurrency = 4
	}
	return &TaskRunnerPool{Concurrency: concurrency}
}

func (p *TaskRunnerPool) RunTasks(tasks []Task) []TaskResult {
	results := make([]TaskResult, len(tasks))
	var g errgroup.Group
	g.SetLimit(p.Concurrency)

	for i, task := range tasks {
		idx := i
		t := task
		g.Go(func() error {
			results[idx] = executeTask(t)
			return nil
		})
	}

	_ = g.Wait()
	return results
}

func executeTask(t Task) TaskResult {
	res := TaskResult{
		ID: t.ID,
	}

	timeout := t.Timeout
	if timeout <= 0 {
		timeout = 180 * time.Second
	}

	ctx, cancel := context.WithTimeout(context.Background(), timeout)
	defer cancel()

	start := time.Now()
	cmd := exec.CommandContext(ctx, t.Command, t.Args...)
	if t.WorkingDir != "" {
		cmd.Dir = t.WorkingDir
	}

	var buf bytes.Buffer
	cmd.Stdout = &buf
	cmd.Stderr = &buf

	err := cmd.Run()
	res.Duration = time.Since(start)
	res.Output = buf.String()

	if ctx.Err() == context.DeadlineExceeded {
		res.TimedOut = true
		res.ExitCode = 124
		res.Error = fmt.Sprintf("task exceeded timeout of %v", timeout)
		return res
	}

	if err != nil {
		if exitErr, ok := err.(*exec.ExitError); ok {
			res.ExitCode = exitErr.ExitCode()
			if rUsage, ok := exitErr.SysUsage().(*syscall.Rusage); ok {
				res.PeakRSSKb = rUsage.Maxrss
			}
		} else {
			res.ExitCode = 1
			res.Error = err.Error()
		}
	} else {
		res.ExitCode = 0
		if cmd.ProcessState != nil {
			if rUsage, ok := cmd.ProcessState.SysUsage().(*syscall.Rusage); ok {
				res.PeakRSSKb = rUsage.Maxrss
			}
		}
	}

	return res
}

// BuildOrchestrator runs dotnet, pytest, or cargo build/test pipelines with strict timeouts
func RunDotnetBuild(repoRoot string, release bool) TaskResult {
	args := []string{"build", "--no-incremental", "-m", "-p:UseSharedCompilation=false", "-p:UseRazorBuildServer=false", "-p:RunAnalyzersDuringBuild=false", "/nodeReuse:false"}
	if release {
		args = append(args, "-c", "Release")
	}
	return executeTask(Task{
		ID:         "dotnet_build",
		Command:    "dotnet",
		Args:       args,
		WorkingDir: repoRoot,
		Timeout:    180 * time.Second,
	})
}

func RunPytestShard(repoRoot string, target string, extraArgs ...string) TaskResult {
	args := []string{"-n", "4", "-m", "fast", target}
	args = append(args, extraArgs...)
	return executeTask(Task{
		ID:         "pytest_" + target,
		Command:    "pytest",
		Args:       args,
		WorkingDir: repoRoot,
		Timeout:    60 * time.Second,
	})
}
