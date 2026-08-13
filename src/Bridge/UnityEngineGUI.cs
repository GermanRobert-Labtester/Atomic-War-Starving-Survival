using System;

namespace UnityEngine
{
    public enum FontStyle { Normal, Bold, Italic, BoldAndItalic }
    public enum TextAnchor { UpperLeft, UpperCenter, UpperRight, MiddleLeft, MiddleCenter, MiddleRight, LowerLeft, LowerCenter, LowerRight }

    public class RectOffset
    {
        public int left { get; set; }
        public int right { get; set; }
        public int top { get; set; }
        public int bottom { get; set; }

        public RectOffset() { }
        public RectOffset(int left, int right, int top, int bottom)
        {
            this.left = left; this.right = right; this.top = top; this.bottom = bottom;
        }
    }

    public class GUIStyleState
    {
        public Texture2D? background { get; set; }
        public Color textColor { get; set; } = Color.white;
    }

    public class GUIStyle
    {
        public GUIStyleState normal { get; set; } = new GUIStyleState();
        public GUIStyleState hover { get; set; } = new GUIStyleState();
        public GUIStyleState active { get; set; } = new GUIStyleState();
        public RectOffset padding { get; set; } = new RectOffset();
        public RectOffset margin { get; set; } = new RectOffset();
        public int fontSize { get; set; } = 12;
        public FontStyle fontStyle { get; set; } = FontStyle.Normal;
        public TextAnchor alignment { get; set; } = TextAnchor.UpperLeft;
        public bool wordWrap { get; set; }

        public GUIStyle() { }
        public GUIStyle(GUIStyle other)
        {
            fontSize = other.fontSize;
            fontStyle = other.fontStyle;
            alignment = other.alignment;
            wordWrap = other.wordWrap;
        }
    }

    public class GUIContent
    {
        public static GUIContent none { get; } = new GUIContent("");
        public string text { get; set; } = "";
        public Texture2D? image { get; set; }
        public string tooltip { get; set; } = "";

        public GUIContent() { }
        public GUIContent(string text) { this.text = text; }
        public GUIContent(string text, string tooltip) { this.text = text; this.tooltip = tooltip; }
    }

    public class GUISkin : Object
    {
        public GUIStyle box { get; set; } = new GUIStyle();
        public GUIStyle label { get; set; } = new GUIStyle();
        public GUIStyle button { get; set; } = new GUIStyle();
        public GUIStyle textField { get; set; } = new GUIStyle();
        public GUIStyle textArea { get; set; } = new GUIStyle();
        public GUIStyle window { get; set; } = new GUIStyle();
    }

    public static class GUI
    {
        public static GUISkin skin { get; set; } = new GUISkin();
        public static Color color { get; set; } = Color.white;
        public static Color backgroundColor { get; set; } = Color.white;

        public static void Box(Rect position, string text) { }
        public static void Box(Rect position, GUIContent content) { }
        public static void Box(Rect position, string text, GUIStyle style) { }
        public static void Label(Rect position, string text) { }
        public static void Label(Rect position, string text, GUIStyle style) { }
        public static void Label(Rect position, GUIContent content) { }
        public static bool Button(Rect position, string text) => false;
        public static bool Button(Rect position, string text, GUIStyle style) => false;
        public static string TextField(Rect position, string text) => text;
        public static string TextArea(Rect position, string text) => text;
        public static float HorizontalSlider(Rect position, float value, float leftValue, float rightValue) => value;
    }

    public static class GUILayout
    {
        public static void BeginArea(Rect screenRect) { }
        public static void BeginArea(Rect screenRect, string text, GUIStyle? style = null) { }
        public static void BeginArea(Rect screenRect, GUIContent content, GUIStyle? style = null) { }
        public static void EndArea() { }
        public static void BeginHorizontal(params GUILayoutOption[] options) { }
        public static void EndHorizontal() { }
        public static void BeginVertical(params GUILayoutOption[] options) { }
        public static void EndVertical() { }
        public static void Label(string text, params GUILayoutOption[] options) { }
        public static void Label(string text, GUIStyle style, params GUILayoutOption[] options) { }
        public static bool Button(string text, params GUILayoutOption[] options) => false;
        public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options) => false;
        public static void Space(float pixels) { }
        public static void FlexibleSpace() { }
        public static float HorizontalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options) => value;

        public static GUILayoutOption Width(float width) => new GUILayoutOption();
        public static GUILayoutOption Height(float height) => new GUILayoutOption();
        public static GUILayoutOption ExpandWidth(bool expand) => new GUILayoutOption();
        public static GUILayoutOption ExpandHeight(bool expand) => new GUILayoutOption();
    }

    public sealed class GUILayoutOption { }
}
