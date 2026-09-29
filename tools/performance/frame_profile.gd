extends SceneTree

var samples: Array[float] = []
var process_samples: Array[float] = []
var draw_samples: Array[float] = []
var started: int
var first_frame_ms: float = 0.0
var duration_seconds: float = 300.0
var output_path: String
var watched_nodes: Array[Node] = []
var callback_counts: Dictionary = {}
var previous_frame_usec: int
var warmup_frames: int = 5

func _initialize() -> void:
	started = Time.get_ticks_usec()
	output_path = OS.get_environment("ASHFALL_FRAME_PROFILE_PATH")
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
	samples.append((now - previous_frame_usec) / 1000.0)
	previous_frame_usec = now
	process_samples.append(Performance.get_monitor(Performance.TIME_PROCESS) * 1000.0)
	draw_samples.append(Performance.get_monitor(Performance.RENDER_TOTAL_DRAW_CALLS_IN_FRAME))
	if Time.get_ticks_usec() - started >= duration_seconds * 1000000.0:
		var result = {"scene":"res://scenes/Main.tscn", "duration_seconds": (Time.get_ticks_usec()-started)/1000000.0, "first_frame_engine_ms":first_frame_ms, "frames":samples.size(), "frame_ms":stats(samples), "process_ms":stats(process_samples), "draw_calls":stats(draw_samples), "renderer":RenderingServer.get_video_adapter_name(), "static_memory_bytes":OS.get_static_memory_usage(), "notes":"15 FPS cap; engine startup clock, not cleared OS caches; idle main menu; per-node C# timings unavailable from aggregate Performance monitors"}
		result["enabled_ui_process_frames"] = callback_counts
		result["effective_fps_cap"] = Engine.max_fps
		var file = FileAccess.open(output_path, FileAccess.WRITE)
		file.store_string(JSON.stringify(result, "  "))
		print("FRAME_PROFILE_RESULT=", JSON.stringify(result))
		quit(0)

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
	var sorted = values.duplicate()
	sorted.sort()
	var total = 0.0
	for value in values:
		total += value
	return {"mean":total/values.size(), "p50":sorted[int(values.size()*0.50)], "p95":sorted[int(values.size()*0.95)], "p99":sorted[int(values.size()*0.99)], "max":sorted[-1]}
