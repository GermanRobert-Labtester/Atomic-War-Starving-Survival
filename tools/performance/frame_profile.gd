extends SceneTree

# ASHFALL frame profiler (perf sprint 1).
#
# Runs the real Main scene headlessly/with a display for a bounded duration and
# captures per-frame frame time, engine process/physics time, draw calls,
# memory, and object/node counts. Writes a JSON summary and an optional per-frame
# CSV. Observational only: it never changes game state.
#
# Environment:
#   ASHFALL_FRAME_PROFILE_PATH      JSON output path (required)
#   ASHFALL_FRAME_PROFILE_CSV_PATH  optional per-frame CSV output path
#   ASHFALL_FRAME_PROFILE_SECONDS   capture duration (default 300)
#
# Per-system caveat: Godot exposes aggregate engine monitors, not per-node C#
# `_Process` timing. `enabled_ui_process_frames` reports how many frames each
# watched C# node was actually processing, which is the per-system signal this
# tool can capture without host-side custom monitors. Frame-time percentiles and
# memory/object snapshots are exact.

var samples: Array[float] = []
var process_samples: Array[float] = []
var physics_samples: Array[float] = []
var draw_samples: Array[float] = []
var memory_samples: Array[float] = []
var object_samples: Array[float] = []
var node_samples: Array[float] = []
var started: int
var first_frame_ms: float = 0.0
var duration_seconds: float = 300.0
var output_path: String
var csv_output_path: String = ""
var watched_nodes: Array[Node] = []
var callback_counts: Dictionary = {}
var previous_frame_usec: int
var warmup_frames: int = 5
var peak_memory_bytes: float = 0.0
var peak_object_count: float = 0.0

func _initialize() -> void:
	started = Time.get_ticks_usec()
	output_path = OS.get_environment("ASHFALL_FRAME_PROFILE_PATH")
	csv_output_path = OS.get_environment("ASHFALL_FRAME_PROFILE_CSV_PATH")
	if OS.has_environment("ASHFALL_FRAME_PROFILE_SECONDS"):
		duration_seconds = float(OS.get_environment("ASHFALL_FRAME_PROFILE_SECONDS"))
	root.add_child(load("res://scenes/Main.tscn").instantiate())
	Engine.max_fps = 15
	process_frame.connect(record_frame)

func record_frame() -> void:
	if first_frame_ms == 0:
		first_frame_ms = Time.get_ticks_usec() / 1000.0
		Engine.max_fps = 15 # User settings applied during Main._Ready must not override the benchmark.
		print("FRAME_PROFILE_FIRST_FRAME_MS=", first_frame_ms)
		collect_nodes(root)
		return
	if warmup_frames > 0:
		warmup_frames -= 1
		started = Time.get_ticks_usec()
		previous_frame_usec = started
		return

	for node in watched_nodes:
		if is_instance_valid(node) and node.is_processing():
			var key = str(node.get_script().resource_path)
			callback_counts[key] = callback_counts.get(key, 0) + 1

	var now = Time.get_ticks_usec()
	var memory_bytes = Performance.get_monitor(Performance.MEMORY_STATIC)
	var object_count = Performance.get_monitor(Performance.OBJECT_COUNT)
	var node_count = Performance.get_monitor(Performance.OBJECT_NODE_COUNT)

	samples.append((now - previous_frame_usec) / 1000.0)
	previous_frame_usec = now
	process_samples.append(Performance.get_monitor(Performance.TIME_PROCESS) * 1000.0)
	physics_samples.append(Performance.get_monitor(Performance.TIME_PHYSICS_PROCESS) * 1000.0)
	draw_samples.append(Performance.get_monitor(Performance.RENDER_TOTAL_DRAW_CALLS_IN_FRAME))
	memory_samples.append(memory_bytes)
	object_samples.append(object_count)
	node_samples.append(node_count)
	peak_memory_bytes = max(peak_memory_bytes, memory_bytes)
	peak_object_count = max(peak_object_count, object_count)

	if Time.get_ticks_usec() - started >= duration_seconds * 1000000.0:
		finish()

func finish() -> void:
	var result = {
		"scene": "res://scenes/Main.tscn",
		"duration_seconds": (Time.get_ticks_usec() - started) / 1000000.0,
		"first_frame_engine_ms": first_frame_ms,
		"frames": samples.size(),
		"frame_ms": stats(samples),
		"process_ms": stats(process_samples),
		"physics_ms": stats(physics_samples),
		"draw_calls": stats(draw_samples),
		"memory_bytes": stats(memory_samples),
		"object_count": stats(object_samples),
		"node_count": stats(node_samples),
		"peak_memory_bytes": peak_memory_bytes,
		"peak_object_count": peak_object_count,
		"renderer": RenderingServer.get_video_adapter_name(),
		"static_memory_bytes": OS.get_static_memory_usage(),
		"notes": "15 FPS cap; engine startup clock not excluded from first_frame_engine_ms; per-system C# _Process timing is not exposed by Godot aggregate monitors (see enabled_ui_process_frames).",
		"enabled_ui_process_frames": callback_counts,
		"effective_fps_cap": Engine.max_fps,
	}
	if output_path != "":
		var file = FileAccess.open(output_path, FileAccess.WRITE)
		if file:
			file.store_string(JSON.stringify(result, "  "))
	if csv_output_path != "":
		write_csv(csv_output_path)
	print("FRAME_PROFILE_RESULT=", JSON.stringify(result))
	quit(0)

func write_csv(path: String) -> void:
	var file = FileAccess.open(path, FileAccess.WRITE)
	if not file:
		push_warning("FRAME_PROFILE_CSV_OPEN_FAIL " + path)
		return
	file.store_line("frame,frame_ms,process_ms,physics_ms,draw_calls,memory_bytes,object_count,node_count")
	for i in range(samples.size()):
		var line = "%d,%.4f,%.4f,%.4f,%.4f,%.4f,%.4f,%.4f" % [
			i,
			samples[i],
			_at(process_samples, i),
			_at(physics_samples, i),
			_at(draw_samples, i),
			_at(memory_samples, i),
			_at(object_samples, i),
			_at(node_samples, i),
		]
		file.store_line(line)

func _at(values: Array[float], index: int) -> float:
	return values[index] if index < values.size() else 0.0

func collect_nodes(node: Node) -> void:
	if node.get_script() != null:
		var path = str(node.get_script().resource_path)
		for name in ["FeedbackPanel.cs", "UiBackgroundCarousel.cs", "DailyBriefingModal.cs", "ExpeditionPanel.cs", "SnapshotOrchestrator.cs"]:
			if path.ends_with(name):
				watched_nodes.append(node)
				callback_counts[path] = 0
	for child in node.get_children():
		collect_nodes(child)

func stats(values: Array[float]) -> Dictionary:
	if values.is_empty():
		return {"count": 0, "mean": 0.0, "p50": 0.0, "p90": 0.0, "p95": 0.0, "p99": 0.0, "max": 0.0}
	var sorted = values.duplicate()
	sorted.sort()
	var total = 0.0
	for value in values:
		total += value
	return {
		"count": values.size(),
		"mean": total / values.size(),
		"p50": sorted[int(values.size() * 0.50)],
		"p90": sorted[int(values.size() * 0.90)],
		"p95": sorted[int(values.size() * 0.95)],
		"p99": sorted[min(int(values.size() * 0.99), values.size() - 1)],
		"max": sorted[-1],
	}
