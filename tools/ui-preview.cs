using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using Ashfall.Core.UI;

/// <summary>
/// Standalone UI boot harness. It deliberately has no Godot reference and no
/// render-server dependency: it resolves the same project assets as the Godot
/// panels, composes deterministic review frames, and writes PNGs.
/// </summary>
internal static class HeadlessUiPreview
{
    private const int OutputWidth = 1920;
    private const int OutputHeight = 1080;

    public static int Main(string[] args)
    {
        try
        {
            var options = PreviewOptions.Parse(args);
            return Run(options);
        }
        catch (Exception error)
        {
            Console.Error.WriteLine($"[HeadlessUiPreview] FAIL: {error.Message}");
            return 1;
        }
    }

    private static int Run(PreviewOptions options)
    {
        string projectRoot = FindProjectRoot(options.ProjectRoot);
        string outputDirectory = Path.IsPathRooted(options.OutputDirectory)
            ? options.OutputDirectory
            : Path.Combine(projectRoot, options.OutputDirectory);
        Directory.CreateDirectory(outputDirectory);

        Console.WriteLine("[HeadlessUiPreview] boot: standalone runtime");
        Console.WriteLine($"[HeadlessUiPreview] root: {projectRoot}");
        Console.WriteLine("[HeadlessUiPreview] stage: validate UI asset manifest");

        var assets = new Dictionary<string, RgbaImage>(StringComparer.Ordinal);
        var assetInfo = new List<AssetInfo>();
        foreach (string relativePath in UiAssetManifest.RequiredPreviewTextures())
        {
            RgbaImage image = LoadAsset(projectRoot, relativePath);
            assets[relativePath] = image;
            assetInfo.Add(new AssetInfo(relativePath, image.Width, image.Height));
            Console.WriteLine($"[HeadlessUiPreview] asset: OK {relativePath} ({image.Width}x{image.Height})");
        }

        Console.WriteLine("[HeadlessUiPreview] stage: _CreateMenu()");
        var screenshots = new List<string>();
        for (int i = 0; i < UiAssetManifest.MainMenuBackgrounds.Count; i++)
        {
            string relativePath = UiAssetManifest.MainMenuBackgrounds[i];
            string outputName = $"menu-{i:00}-{Path.GetFileNameWithoutExtension(relativePath)}.png";
            string outputPath = Path.Combine(outputDirectory, outputName);
            _CreateMenu(assets[relativePath], i).SavePng(outputPath);
            screenshots.Add(outputName);
        }

        Console.WriteLine("[HeadlessUiPreview] stage: _CreateHUD()");
        string hudOutputName = "hud.png";
        string hudOutputPath = Path.Combine(outputDirectory, hudOutputName);
        _CreateHUD(
            assets[UiAssetManifest.TitleBackground],
            assets[UiAssetManifest.PanelBackground],
            assets[UiAssetManifest.HeaderBar]).SavePng(hudOutputPath);
        screenshots.Add(hudOutputName);

        Console.WriteLine("[HeadlessUiPreview] stage: _CreateGameOver()");
        for (int i = 0; i < UiAssetManifest.GameOverBackgrounds.Count; i++)
        {
            string relativePath = UiAssetManifest.GameOverBackgrounds[i];
            string outputName = $"game-over-{i:00}-{Path.GetFileNameWithoutExtension(relativePath)}.png";
            string outputPath = Path.Combine(outputDirectory, outputName);
            _CreateGameOver(assets[relativePath], i).SavePng(outputPath);
            screenshots.Add(outputName);
        }

        var report = new PreviewReport
        {
            GeneratedUtc = DateTimeOffset.UtcNow,
            ProjectRoot = projectRoot,
            BootSequence = new[]
            {
                "resolve_project_root",
                "validate_ui_asset_manifest",
                "_CreateMenu",
                "_CreateHUD",
                "_CreateGameOver",
                "write_png_previews"
            },
            Assets = assetInfo,
            Screenshots = screenshots
        };

        string reportPath = Path.Combine(outputDirectory, "ui-preview-report.json");
        File.WriteAllText(reportPath, JsonSerializer.Serialize(report, new JsonSerializerOptions
        {
            WriteIndented = true
        }));

        Console.WriteLine($"[HeadlessUiPreview] PASS: {screenshots.Count} PNG previews written to {outputDirectory}");
        Console.WriteLine($"[HeadlessUiPreview] report: {reportPath}");
        return 0;
    }

    private static RgbaImage LoadAsset(string projectRoot, string relativePath)
    {
        string fullPath = Path.Combine(projectRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"UI asset path did not resolve: {relativePath}", fullPath);

        return RgbaImage.LoadPng(fullPath);
    }

    private static string FindProjectRoot(string? requestedRoot)
    {
        if (!string.IsNullOrWhiteSpace(requestedRoot))
        {
            string explicitRoot = Path.GetFullPath(requestedRoot);
            if (LooksLikeProjectRoot(explicitRoot))
                return explicitRoot;

            throw new DirectoryNotFoundException($"Not an ASHFALL project root: {explicitRoot}");
        }

        string? current = Directory.GetCurrentDirectory();
        while (!string.IsNullOrEmpty(current))
        {
            if (LooksLikeProjectRoot(current))
                return current;

            current = Directory.GetParent(current)?.FullName;
        }

        throw new DirectoryNotFoundException(
            "Could not find project.godot and Assets/UI from the current directory. " +
            "Run from the repository or pass --project-root <path>.");
    }

    private static bool LooksLikeProjectRoot(string path)
    {
        return File.Exists(Path.Combine(path, "project.godot")) &&
               Directory.Exists(Path.Combine(path, "Assets"));
    }

    private static RgbaImage _CreateMenu(RgbaImage source, int backgroundIndex)
    {
        RgbaImage canvas = RgbaImage.Cover(source, OutputWidth, OutputHeight);
        canvas.FillRect(0, 0, canvas.Width, canvas.Height, Color.Black.WithAlpha(148));

        int panelX = 600;
        int panelY = 172;
        int panelWidth = 720;
        int panelHeight = 736;
        canvas.FillRect(panelX, panelY, panelWidth, panelHeight, Color.InkPanel);
        canvas.StrokeRect(panelX, panelY, panelWidth, panelHeight, Color.Line, 2);

        canvas.DrawTextCentered("ASHFALL", 250, 5, Color.Warm);
        canvas.DrawTextCentered("ATOMIC WAR: STARVING SURVIVAL", 338, 2, Color.Muted);
        canvas.FillRect(panelX + 88, 390, panelWidth - 176, 2, Color.Line);
        canvas.DrawTextCentered("THE EXCHANGE IS OVER. THE ASH IS SETTLING.", 430, 1, Color.Pale);

        DrawButton(canvas, "NEW GAME", 490);
        DrawButton(canvas, "CONTINUE", 590);
        DrawButton(canvas, "QUIT", 690);

        canvas.DrawTextCentered($"BACKGROUND {backgroundIndex + 1}/{UiAssetManifest.MainMenuBackgrounds.Count}", 830, 1, Color.Dim);
        canvas.DrawTextCentered("[ENTER] NEW GAME   [C] CONTINUE   [ESC] QUIT", 866, 1, Color.Dim);
        return canvas;
    }

    private static RgbaImage _CreateHUD(RgbaImage source, RgbaImage panelTexture, RgbaImage headerTexture)
    {
        RgbaImage canvas = RgbaImage.Cover(source, OutputWidth, OutputHeight);
        canvas.FillRect(0, 0, canvas.Width, canvas.Height, Color.Black.WithAlpha(112));
        canvas.BlitStretch(panelTexture, 0, 0, OutputWidth, 96, 0.42f);
        canvas.BlitStretch(headerTexture, 0, 0, OutputWidth, 72, 0.22f);
        canvas.FillRect(0, 0, canvas.Width, 82, Color.Ink.WithAlpha(232));
        canvas.StrokeRect(0, 0, canvas.Width, 82, Color.Line, 2);

        int x = 34;
        canvas.DrawText("DAY 1", x, 24, 2, Color.Warm);
        x += 170;
        canvas.DrawText("HP: 100/100", x, 24, 2, Color.Pale);
        x += 290;
        canvas.DrawText("RAD: 0.0 mSv", x, 24, 2, Color.Lethe);
        x += 300;
        canvas.DrawText("VALUE: 100", x, 24, 2, Color.Hot);
        canvas.DrawText("MENU [ESC]", 1660, 24, 2, Color.Pale);

        canvas.FillRect(80, 236, 640, 2, Color.Line);
        canvas.DrawText("THE HOLDFAST", 80, 272, 4, Color.Warm);
        canvas.DrawText("A playable boot reaches the HUD with the same asset contract.", 80, 340, 1, Color.Pale);
        return canvas;
    }

    private static RgbaImage _CreateGameOver(RgbaImage source, int backgroundIndex)
    {
        RgbaImage canvas = RgbaImage.Cover(source, OutputWidth, OutputHeight);
        canvas.FillRect(0, 0, canvas.Width, canvas.Height, Color.Black.WithAlpha(196));

        int panelX = 650;
        int panelY = 232;
        int panelWidth = 620;
        int panelHeight = 616;
        canvas.FillRect(panelX, panelY, panelWidth, panelHeight, Color.InkPanel);
        canvas.StrokeRect(panelX, panelY, panelWidth, panelHeight, Color.Line, 2);

        canvas.DrawTextCentered("THE LEDGER IS CLOSED", 320, 3, Color.Warm);
        canvas.FillRect(panelX + 80, 392, panelWidth - 160, 2, Color.Line);
        canvas.DrawTextCentered("THE BUNKER FELL SILENT.", 448, 2, Color.Pale);
        canvas.DrawTextCentered("SURVIVED 1 DAY. FINAL VALUE: 100.", 506, 1, Color.Muted);
        DrawButton(canvas, "NEW GAME", 600, panelX, panelWidth);
        DrawButton(canvas, "RETURN TO MENU", 700, panelX, panelWidth);
        canvas.DrawTextCentered($"BACKGROUND {backgroundIndex + 1}/{UiAssetManifest.GameOverBackgrounds.Count}", 810, 1, Color.Dim);
        return canvas;
    }

    private static void DrawButton(RgbaImage canvas, string label, int y, int panelX = 600, int panelWidth = 720)
    {
        int width = label.Length > 10 ? 420 : 300;
        int x = panelX + (panelWidth - width) / 2;
        canvas.FillRect(x, y, width, 58, Color.Ink.WithAlpha(220));
        canvas.StrokeRect(x, y, width, 58, Color.Line, 2);
        canvas.DrawTextCentered(label, y + 20, 2, Color.Pale, x, width);
    }

    private sealed class PreviewOptions
    {
        public string? ProjectRoot { get; private set; }
        public string OutputDirectory { get; private set; } = "Builds/UiPreview";

        public static PreviewOptions Parse(string[] args)
        {
            var options = new PreviewOptions();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--project-root":
                        options.ProjectRoot = RequireValue(args, ref i, "--project-root");
                        break;
                    case "--output":
                        options.OutputDirectory = RequireValue(args, ref i, "--output");
                        break;
                    case "--help":
                        Console.WriteLine("dotnet run --project tools/ui-preview.csproj -- [--project-root PATH] [--output PATH]");
                        Environment.Exit(0);
                        break;
                    default:
                        throw new ArgumentException($"Unknown argument: {args[i]}");
                }
            }

            return options;
        }

        private static string RequireValue(string[] args, ref int index, string option)
        {
            if (index + 1 >= args.Length)
                throw new ArgumentException($"{option} requires a value.");

            index++;
            return args[index];
        }
    }

    private sealed class PreviewReport
    {
        public DateTimeOffset GeneratedUtc { get; set; }
        public string ProjectRoot { get; set; } = string.Empty;
        public string[] BootSequence { get; set; } = Array.Empty<string>();
        public List<AssetInfo> Assets { get; set; } = new List<AssetInfo>();
        public List<string> Screenshots { get; set; } = new List<string>();
    }

    private sealed class AssetInfo
    {
        public AssetInfo(string path, int width, int height)
        {
            Path = path;
            Width = width;
            Height = height;
        }

        public string Path { get; }
        public int Width { get; }
        public int Height { get; }
    }

    private readonly struct Rgba
    {
        public Rgba(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public Rgba WithAlpha(byte alpha) => new Rgba(R, G, B, alpha);
        public Rgba WithAlpha(int alpha) => WithAlpha((byte)Math.Clamp(alpha, 0, 255));

        public static readonly Rgba Black = new Rgba(0, 0, 0);
        public static readonly Rgba Ink = new Rgba(9, 11, 12);
        public static readonly Rgba InkPanel = new Rgba(9, 11, 12, 220);
        public static readonly Rgba Line = new Rgba(217, 196, 152, 100);
        public static readonly Rgba Warm = new Rgba(211, 170, 98);
        public static readonly Rgba Hot = new Rgba(244, 200, 117);
        public static readonly Rgba Pale = new Rgba(230, 224, 210);
        public static readonly Rgba Muted = new Rgba(147, 143, 132);
        public static readonly Rgba Dim = new Rgba(102, 103, 95);
        public static readonly Rgba Lethe = new Rgba(110, 163, 168);
    }

    // Keeps the composition code readable without depending on Godot's Color.
    private static class Color
    {
        public static Rgba Black => Rgba.Black;
        public static Rgba Ink => Rgba.Ink;
        public static Rgba InkPanel => Rgba.InkPanel;
        public static Rgba Line => Rgba.Line;
        public static Rgba Warm => Rgba.Warm;
        public static Rgba Hot => Rgba.Hot;
        public static Rgba Pale => Rgba.Pale;
        public static Rgba Muted => Rgba.Muted;
        public static Rgba Dim => Rgba.Dim;
        public static Rgba Lethe => Rgba.Lethe;
    }

    private sealed class RgbaImage
    {
        private static readonly uint[] CrcTable = BuildCrcTable();

        public RgbaImage(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), "PNG dimensions must be positive.");

            Width = width;
            Height = height;
            Pixels = new byte[checked(width * height * 4)];
        }

        public int Width { get; }
        public int Height { get; }
        private byte[] Pixels { get; }

        public static RgbaImage LoadPng(string path)
        {
            using var input = File.OpenRead(path);
            byte[] signature = ReadExactly(input, 8);
            byte[] expectedSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
            if (!signature.SequenceEqual(expectedSignature))
                throw new InvalidDataException($"Not a PNG: {path}");

            int width = 0;
            int height = 0;
            byte bitDepth = 0;
            byte colorType = 0;
            byte interlace = 0;
            using var compressed = new MemoryStream();

            while (input.Position < input.Length)
            {
                byte[] lengthBytes = ReadExactly(input, 4);
                int length = checked((int)ReadUInt32(lengthBytes, 0));
                string chunkType = Encoding.ASCII.GetString(ReadExactly(input, 4));
                byte[] data = ReadExactly(input, length);
                _ = ReadExactly(input, 4); // CRC is not needed after the file-level signature check.

                if (chunkType == "IHDR")
                {
                    width = checked((int)ReadUInt32(data, 0));
                    height = checked((int)ReadUInt32(data, 4));
                    bitDepth = data[8];
                    colorType = data[9];
                    interlace = data[12];
                }
                else if (chunkType == "IDAT")
                {
                    compressed.Write(data, 0, data.Length);
                }
                else if (chunkType == "IEND")
                {
                    break;
                }
            }

            if (width == 0 || height == 0 || bitDepth != 8 || interlace != 0 || (colorType != 2 && colorType != 6))
                throw new InvalidDataException(
                    $"Unsupported PNG format in {path}; expected 8-bit RGB/RGBA, non-interlaced.");

            int sourceBytesPerPixel = colorType == 6 ? 4 : 3;
            int rowLength = checked(width * sourceBytesPerPixel);
            byte[] decoded = Decompress(compressed.ToArray());
            int requiredLength = checked((rowLength + 1) * height);
            if (decoded.Length < requiredLength)
                throw new InvalidDataException($"PNG pixel stream is truncated: {path}");

            var image = new RgbaImage(width, height);
            var previousRow = new byte[rowLength];
            var currentRow = new byte[rowLength];
            int cursor = 0;
            for (int y = 0; y < height; y++)
            {
                byte filter = decoded[cursor++];
                Buffer.BlockCopy(decoded, cursor, currentRow, 0, rowLength);
                cursor += rowLength;
                Unfilter(currentRow, previousRow, sourceBytesPerPixel, filter);

                for (int x = 0; x < width; x++)
                {
                    int sourceOffset = x * sourceBytesPerPixel;
                    int destinationOffset = (y * width + x) * 4;
                    image.Pixels[destinationOffset] = currentRow[sourceOffset];
                    image.Pixels[destinationOffset + 1] = currentRow[sourceOffset + 1];
                    image.Pixels[destinationOffset + 2] = currentRow[sourceOffset + 2];
                    image.Pixels[destinationOffset + 3] = colorType == 6
                        ? currentRow[sourceOffset + 3]
                        : (byte)255;
                }

                (previousRow, currentRow) = (currentRow, previousRow);
            }

            return image;
        }

        public static RgbaImage Cover(RgbaImage source, int width, int height)
        {
            var result = new RgbaImage(width, height);
            double scale = Math.Max((double)width / source.Width, (double)height / source.Height);
            double visibleWidth = width / scale;
            double visibleHeight = height / scale;
            double left = (source.Width - visibleWidth) / 2.0;
            double top = (source.Height - visibleHeight) / 2.0;

            for (int y = 0; y < height; y++)
            {
                int sourceY = Math.Clamp((int)((top + y / scale)), 0, source.Height - 1);
                for (int x = 0; x < width; x++)
                {
                    int sourceX = Math.Clamp((int)((left + x / scale)), 0, source.Width - 1);
                    int sourceOffset = (sourceY * source.Width + sourceX) * 4;
                    int destinationOffset = (y * width + x) * 4;
                    Buffer.BlockCopy(source.Pixels, sourceOffset, result.Pixels, destinationOffset, 4);
                }
            }

            return result;
        }

        public void FillRect(int x, int y, int width, int height, Rgba color)
        {
            int left = Math.Max(0, x);
            int top = Math.Max(0, y);
            int right = Math.Min(Width, x + width);
            int bottom = Math.Min(Height, y + height);
            for (int py = top; py < bottom; py++)
                for (int px = left; px < right; px++)
                    BlendPixel(px, py, color);
        }

        public void StrokeRect(int x, int y, int width, int height, Rgba color, int thickness)
        {
            FillRect(x, y, width, thickness, color);
            FillRect(x, y + height - thickness, width, thickness, color);
            FillRect(x, y, thickness, height, color);
            FillRect(x + width - thickness, y, thickness, height, color);
        }

        public void BlitStretch(RgbaImage source, int x, int y, int width, int height, float opacity)
        {
            byte[] sourcePixels = source.Pixels;
            for (int py = 0; py < height; py++)
            {
                int destinationY = y + py;
                if (destinationY < 0 || destinationY >= Height) continue;
                int sourceY = Math.Clamp(py * source.Height / Math.Max(1, height), 0, source.Height - 1);
                for (int px = 0; px < width; px++)
                {
                    int destinationX = x + px;
                    if (destinationX < 0 || destinationX >= Width) continue;
                    int sourceX = Math.Clamp(px * source.Width / Math.Max(1, width), 0, source.Width - 1);
                    int sourceOffset = (sourceY * source.Width + sourceX) * 4;
                    var color = new Rgba(
                        sourcePixels[sourceOffset],
                        sourcePixels[sourceOffset + 1],
                        sourcePixels[sourceOffset + 2],
                        (byte)Math.Clamp((int)(sourcePixels[sourceOffset + 3] * opacity), 0, 255));
                    BlendPixel(destinationX, destinationY, color);
                }
            }
        }

        public void DrawText(string text, int x, int y, int scale, Rgba color)
        {
            int cursor = x;
            foreach (char character in text.ToUpperInvariant())
            {
                string[] glyph = Glyph(character);
                for (int gy = 0; gy < glyph.Length; gy++)
                    for (int gx = 0; gx < glyph[gy].Length; gx++)
                        if (glyph[gy][gx] == '1')
                            FillRect(cursor + gx * scale, y + gy * scale, scale, scale, color);

                cursor += 6 * scale;
            }
        }

        public void DrawTextCentered(string text, int y, int scale, Rgba color, int regionX = 0, int regionWidth = -1)
        {
            if (regionWidth < 0) regionWidth = Width;
            int textWidth = text.Length * 6 * scale - scale;
            int x = regionX + Math.Max(0, (regionWidth - textWidth) / 2);
            DrawText(text, x, y, scale, color);
        }

        public void SavePng(string path)
        {
            using var output = File.Create(path);
            output.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });

            byte[] header = new byte[13];
            WriteUInt32(header, 0, (uint)Width);
            WriteUInt32(header, 4, (uint)Height);
            header[8] = 8;
            header[9] = 6; // RGBA
            WriteChunk(output, "IHDR", header);

            byte[] raw = new byte[checked((Width * 4 + 1) * Height)];
            int cursor = 0;
            for (int y = 0; y < Height; y++)
            {
                raw[cursor++] = 0;
                Buffer.BlockCopy(Pixels, y * Width * 4, raw, cursor, Width * 4);
                cursor += Width * 4;
            }

            using var compressed = new MemoryStream();
            using (var zlib = new ZLibStream(compressed, CompressionLevel.Fastest, leaveOpen: true))
                zlib.Write(raw, 0, raw.Length);
            WriteChunk(output, "IDAT", compressed.ToArray());
            WriteChunk(output, "IEND", Array.Empty<byte>());
        }

        private void BlendPixel(int x, int y, Rgba source)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height || source.A == 0)
                return;

            int offset = (y * Width + x) * 4;
            float sourceAlpha = source.A / 255f;
            float destinationAlpha = Pixels[offset + 3] / 255f;
            float outputAlpha = sourceAlpha + destinationAlpha * (1f - sourceAlpha);
            if (outputAlpha <= 0f) return;

            Pixels[offset] = (byte)Math.Clamp((int)((source.R * sourceAlpha + Pixels[offset] * destinationAlpha * (1f - sourceAlpha)) / outputAlpha), 0, 255);
            Pixels[offset + 1] = (byte)Math.Clamp((int)((source.G * sourceAlpha + Pixels[offset + 1] * destinationAlpha * (1f - sourceAlpha)) / outputAlpha), 0, 255);
            Pixels[offset + 2] = (byte)Math.Clamp((int)((source.B * sourceAlpha + Pixels[offset + 2] * destinationAlpha * (1f - sourceAlpha)) / outputAlpha), 0, 255);
            Pixels[offset + 3] = (byte)Math.Clamp((int)(outputAlpha * 255f), 0, 255);
        }

        private static void Unfilter(byte[] row, byte[] previousRow, int bytesPerPixel, byte filter)
        {
            if (filter == 0) return;
            if (filter < 1 || filter > 4)
                throw new InvalidDataException($"Unsupported PNG filter: {filter}");

            for (int i = 0; i < row.Length; i++)
            {
                int left = i >= bytesPerPixel ? row[i - bytesPerPixel] : 0;
                int above = previousRow[i];
                int upperLeft = i >= bytesPerPixel ? previousRow[i - bytesPerPixel] : 0;
                int predictor = filter switch
                {
                    1 => left,
                    2 => above,
                    3 => (left + above) / 2,
                    4 => Paeth(left, above, upperLeft),
                    _ => 0
                };
                row[i] = (byte)((row[i] + predictor) & 0xff);
            }
        }

        private static int Paeth(int left, int above, int upperLeft)
        {
            int estimate = left + above - upperLeft;
            int leftDistance = Math.Abs(estimate - left);
            int aboveDistance = Math.Abs(estimate - above);
            int upperLeftDistance = Math.Abs(estimate - upperLeft);
            if (leftDistance <= aboveDistance && leftDistance <= upperLeftDistance) return left;
            return aboveDistance <= upperLeftDistance ? above : upperLeft;
        }

        private static byte[] Decompress(byte[] bytes)
        {
            using var input = new MemoryStream(bytes);
            using var zlib = new ZLibStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            zlib.CopyTo(output);
            return output.ToArray();
        }

        private static byte[] ReadExactly(Stream stream, int count)
        {
            var bytes = new byte[count];
            int offset = 0;
            while (offset < count)
            {
                int read = stream.Read(bytes, offset, count - offset);
                if (read == 0) throw new EndOfStreamException("Unexpected end of PNG stream.");
                offset += read;
            }
            return bytes;
        }

        private static uint ReadUInt32(byte[] bytes, int offset)
        {
            return (uint)(bytes[offset] << 24 | bytes[offset + 1] << 16 | bytes[offset + 2] << 8 | bytes[offset + 3]);
        }

        private static void WriteUInt32(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)value;
        }

        private static void WriteChunk(Stream output, string type, byte[] data)
        {
            byte[] length = new byte[4];
            WriteUInt32(length, 0, (uint)data.Length);
            output.Write(length);
            byte[] typeBytes = Encoding.ASCII.GetBytes(type);
            output.Write(typeBytes);
            output.Write(data);

            uint crc = 0xffffffffu;
            foreach (byte value in typeBytes) crc = CrcTable[(crc ^ value) & 0xff] ^ (crc >> 8);
            foreach (byte value in data) crc = CrcTable[(crc ^ value) & 0xff] ^ (crc >> 8);
            crc ^= 0xffffffffu;
            byte[] crcBytes = new byte[4];
            WriteUInt32(crcBytes, 0, crc);
            output.Write(crcBytes);
        }

        private static uint[] BuildCrcTable()
        {
            var table = new uint[256];
            for (uint i = 0; i < table.Length; i++)
            {
                uint value = i;
                for (int bit = 0; bit < 8; bit++)
                    value = (value & 1) == 1 ? 0xedb88320u ^ (value >> 1) : value >> 1;
                table[i] = value;
            }
            return table;
        }

        private static string[] Glyph(char character)
        {
            string pattern = character switch
            {
                'A' => "01110/10001/10001/11111/10001/10001/10001",
                'B' => "11110/10001/10001/11110/10001/10001/11110",
                'C' => "01111/10000/10000/10000/10000/10000/01111",
                'D' => "11110/10001/10001/10001/10001/10001/11110",
                'E' => "11111/10000/10000/11110/10000/10000/11111",
                'F' => "11111/10000/10000/11110/10000/10000/10000",
                'G' => "01111/10000/10000/10111/10001/10001/01111",
                'H' => "10001/10001/10001/11111/10001/10001/10001",
                'I' => "11111/00100/00100/00100/00100/00100/11111",
                'J' => "00111/00010/00010/00010/00010/10010/01100",
                'K' => "10001/10010/10100/11000/10100/10010/10001",
                'L' => "10000/10000/10000/10000/10000/10000/11111",
                'M' => "10001/11011/10101/10101/10001/10001/10001",
                'N' => "10001/11001/10101/10011/10001/10001/10001",
                'O' => "01110/10001/10001/10001/10001/10001/01110",
                'P' => "11110/10001/10001/11110/10000/10000/10000",
                'Q' => "01110/10001/10001/10001/10101/10010/01101",
                'R' => "11110/10001/10001/11110/10100/10010/10001",
                'S' => "01111/10000/10000/01110/00001/00001/11110",
                'T' => "11111/00100/00100/00100/00100/00100/00100",
                'U' => "10001/10001/10001/10001/10001/10001/01110",
                'V' => "10001/10001/10001/10001/10001/01010/00100",
                'W' => "10001/10001/10001/10101/10101/11011/10001",
                'X' => "10001/10001/01010/00100/01010/10001/10001",
                'Y' => "10001/10001/01010/00100/00100/00100/00100",
                'Z' => "11111/00001/00010/00100/01000/10000/11111",
                '0' => "01110/10001/10011/10101/11001/10001/01110",
                '1' => "00100/01100/00100/00100/00100/00100/01110",
                '2' => "01110/10001/00001/00010/00100/01000/11111",
                '3' => "11110/00001/00001/01110/00001/00001/11110",
                '4' => "00010/00110/01010/10010/11111/00010/00010",
                '5' => "11111/10000/10000/11110/00001/00001/11110",
                '6' => "01110/10000/10000/11110/10001/10001/01110",
                '7' => "11111/00001/00010/00100/01000/01000/01000",
                '8' => "01110/10001/10001/01110/10001/10001/01110",
                '9' => "01110/10001/10001/01111/00001/00001/01110",
                ':' => "00000/00100/00100/00000/00100/00100/00000",
                '[' => "01110/01000/01000/01000/01000/01000/01110",
                ']' => "01110/00010/00010/00010/00010/00010/01110",
                '-' => "00000/00000/00000/11111/00000/00000/00000",
                '/' => "00001/00010/00100/01000/10000/00000/00000",
                '.' => "00000/00000/00000/00000/00000/00110/00110",
                _ => "00000/00000/00000/00000/00000/00000/00000"
            };
            return pattern.Split('/');
        }
    }
}
