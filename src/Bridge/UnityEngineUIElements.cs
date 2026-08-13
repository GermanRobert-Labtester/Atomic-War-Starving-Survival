using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UIElements
{
    public delegate void EventCallback<TEventType>(TEventType evt);

    public enum DisplayStyle { Flex, None }
    public enum FlexDirection { Column, Row, ColumnReverse, RowReverse }
    public enum Justify { FlexStart, Center, FlexEnd, SpaceBetween, SpaceAround }
    public enum Align { Auto, FlexStart, Center, FlexEnd, Stretch }
    public enum Position { Relative, Absolute }
    public enum LengthUnit { Pixel, Percent, Auto, None }
    public enum PickingMode { Position = 0, Ignore = 1 }
    public enum TrickleDown { No = 0, TrickleDown = 1 }

    public struct Length
    {
        public float value { get; set; }
        public LengthUnit unit { get; set; }
        public Length(float value, LengthUnit unit = LengthUnit.Pixel) { this.value = value; this.unit = unit; }
        public static Length Percent(float val) => new Length(val, LengthUnit.Percent);
        public static implicit operator Length(float val) => new Length(val);
    }

    public struct StyleLength
    {
        public Length value { get; set; }
        public StyleLength(float val) { value = new Length(val); }
        public StyleLength(Length len) { value = len; }
        public static implicit operator StyleLength(float val) => new StyleLength(val);
        public static implicit operator StyleLength(Length val) => new StyleLength(val);
    }

    public struct StyleColor
    {
        public Color value { get; set; }
        public StyleColor(Color c) { value = c; }
        public static implicit operator StyleColor(Color c) => new StyleColor(c);
    }

    public struct StyleEnum<T> where T : struct
    {
        public T value { get; set; }
        public StyleEnum(T v) { value = v; }
        public static implicit operator StyleEnum<T>(T v) => new StyleEnum<T>(v);
    }

    public class IStyle
    {
        public StyleLength width { get; set; }
        public StyleLength height { get; set; }
        public StyleLength minWidth { get; set; }
        public StyleLength minHeight { get; set; }
        public StyleLength maxWidth { get; set; }
        public StyleLength maxHeight { get; set; }
        public StyleLength marginTop { get; set; }
        public StyleLength marginBottom { get; set; }
        public StyleLength marginLeft { get; set; }
        public StyleLength marginRight { get; set; }
        public StyleLength paddingTop { get; set; }
        public StyleLength paddingBottom { get; set; }
        public StyleLength paddingLeft { get; set; }
        public StyleLength paddingRight { get; set; }
        public StyleLength borderTopLeftRadius { get; set; }
        public StyleLength borderTopRightRadius { get; set; }
        public StyleLength borderBottomLeftRadius { get; set; }
        public StyleLength borderBottomRightRadius { get; set; }
        public StyleColor backgroundColor { get; set; }
        public StyleColor color { get; set; }
        public StyleEnum<DisplayStyle> display { get; set; }
        public StyleEnum<FlexDirection> flexDirection { get; set; }
        public StyleEnum<Justify> justifyContent { get; set; }
        public StyleEnum<Align> alignItems { get; set; }
        public StyleEnum<Position> position { get; set; }
        public StyleEnum<FontStyle> unityFontStyleAndWeight { get; set; }
        public StyleEnum<TextAnchor> unityTextAlign { get; set; }
        public StyleLength top { get; set; }
        public StyleLength bottom { get; set; }
        public StyleLength left { get; set; }
        public StyleLength right { get; set; }
        public StyleLength fontSize { get; set; }
        public float flexGrow { get; set; }
        public float flexShrink { get; set; }
        public float opacity { get; set; } = 1f;
    }

    // =========================================================================
    // UI Events
    // =========================================================================
    public abstract class EventBase
    {
        public VisualElement? target { get; set; }
        public void StopPropagation() { }
    }

    public class ChangeEvent<T> : EventBase
    {
        public T previousValue { get; set; } = default!;
        public T newValue { get; set; } = default!;
    }

    public class GeometryChangedEvent : EventBase
    {
        public Rect oldRect { get; set; }
        public Rect newRect { get; set; }
    }

    public class NavigationMoveEvent : EventBase
    {
        public enum Direction { None, Left, Up, Right, Down }
        public Direction direction { get; set; }
    }

    public class KeyDownEvent : EventBase
    {
        public KeyCode keyCode { get; set; }
        public char character { get; set; }
    }

    public class PointerDownEvent : EventBase
    {
        public int button { get; set; }
        public Vector2 position { get; set; }
    }

    public class PointerUpEvent : EventBase
    {
        public int button { get; set; }
        public Vector2 position { get; set; }
    }

    public class PointerEnterEvent : EventBase { }
    public class PointerLeaveEvent : EventBase { }
    public class PointerMoveEvent : EventBase { }
    public class ClickEvent : EventBase { }
    public class FocusInEvent : EventBase { }
    public class FocusOutEvent : EventBase { }

    // =========================================================================
    // Scheduling & Stylesheets
    // =========================================================================
    public interface IVisualElementScheduledItem
    {
        IVisualElementScheduledItem Every(long intervalMs);
        IVisualElementScheduledItem StartingIn(long intervalMs);
        void Pause();
        void Resume();
    }

    public interface IVisualElementScheduler
    {
        IVisualElementScheduledItem Execute(Action updateEvent);
    }

    internal class DefaultScheduledItem : IVisualElementScheduledItem
    {
        public IVisualElementScheduledItem Every(long intervalMs) => this;
        public IVisualElementScheduledItem StartingIn(long intervalMs) => this;
        public void Pause() { }
        public void Resume() { }
    }

    internal class DefaultVisualElementScheduler : IVisualElementScheduler
    {
        public IVisualElementScheduledItem Execute(Action updateEvent) => new DefaultScheduledItem();
    }

    public struct VisualElementStyleSheetSet
    {
        public void Add(StyleSheet styleSheet) { }
        public void Remove(StyleSheet styleSheet) { }
        public bool Contains(StyleSheet styleSheet) => false;
        public void Clear() { }
    }

    // =========================================================================
    // Visual Elements
    // =========================================================================
    public class VisualElement
    {
        public string name { get; set; } = "";
        public string tooltip { get; set; } = "";
        public bool focusable { get; set; } = true;
        public bool enabledSelf => true;
        public PickingMode pickingMode { get; set; } = PickingMode.Position;
        public IStyle style { get; } = new IStyle();
        public VisualElementStyleSheetSet styleSheets => new VisualElementStyleSheetSet();
        public IVisualElementScheduler schedule => new DefaultVisualElementScheduler();
        public object? userData { get; set; }
        public VisualElement? parent { get; internal set; }

        private readonly List<VisualElement> _children = new();
        private readonly HashSet<string> _classes = new();

        public int childCount => _children.Count;
        public VisualElement this[int index] => _children[index];

        public void Add(VisualElement child)
        {
            child.parent = this;
            _children.Add(child);
        }

        public void Remove(VisualElement child)
        {
            if (_children.Remove(child)) child.parent = null;
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _children.Count)
            {
                var child = _children[index];
                _children.RemoveAt(index);
                child.parent = null;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _children.Count; i++) _children[i].parent = null;
            _children.Clear();
        }

        public void RemoveFromHierarchy()
        {
            parent?.Remove(this);
        }

        public IEnumerable<VisualElement> Children() => _children;

        public void AddToClassList(string className) => _classes.Add(className);
        public void RemoveFromClassList(string className) => _classes.Remove(className);
        public bool ClassListContains(string className) => _classes.Contains(className);
        public void EnableInClassList(string className, bool enable) { if (enable) AddToClassList(className); else RemoveFromClassList(className); }
        public void ToggleInClassList(string className) { if (ClassListContains(className)) RemoveFromClassList(className); else AddToClassList(className); }

        public VisualElement? Q(string? name = null, string? className = null)
        {
            return Q<VisualElement>(name, className);
        }

        public T? Q<T>(string? name = null, string? className = null) where T : VisualElement
        {
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c is T match)
                {
                    if (name != null && c.name != name) continue;
                    if (className != null && !c.ClassListContains(className)) continue;
                    return match;
                }
                var sub = c.Q<T>(name, className);
                if (sub != null) return sub;
            }
            return null;
        }

        public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, TrickleDown useTrickleDown = TrickleDown.No) where TEventType : EventBase { }
        public void UnregisterCallback<TEventType>(EventCallback<TEventType> callback, TrickleDown useTrickleDown = TrickleDown.No) where TEventType : EventBase { }
        public void SetEnabled(bool enabled) { }
        public void Focus() { }
        public void MarkDirtyRepaint() { }
    }

    public class BaseField<T> : VisualElement
    {
        public virtual T value { get; set; } = default!;
        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<T>> callback) { }
        public void UnregisterValueChangedCallback(EventCallback<ChangeEvent<T>> callback) { }
    }

    public class Label : VisualElement
    {
        public string text { get; set; } = "";
        public Label() { }
        public Label(string text) { this.text = text; }
    }

    public class Button : VisualElement
    {
        public string text { get; set; } = "";
        public Action? clicked;
        public Button() { }
        public Button(Action clickEvent) { clicked = clickEvent; }
    }

    public class ProgressBar : VisualElement
    {
        public float value { get; set; }
        public float lowValue { get; set; } = 0f;
        public float highValue { get; set; } = 100f;
        public string title { get; set; } = "";
    }

    public class ScrollView : VisualElement
    {
        public Vector2 scrollOffset { get; set; }
    }

    public class Slider : BaseField<float>
    {
        public float lowValue { get; set; } = 0f;
        public float highValue { get; set; } = 1f;
    }

    public class Toggle : BaseField<bool>
    {
        public string text { get; set; } = "";
    }

    public class DropdownField : BaseField<string>
    {
        public List<string> choices { get; set; } = new();
        public int index { get; set; }
    }

    public class StyleSheet : ScriptableObject { }
    public class VisualTreeAsset : ScriptableObject
    {
        public VisualElement CloneTree() => new VisualElement();
        public VisualElement Instantiate() => new VisualElement();
    }

    public enum PanelScaleMode { ScaleWithScreenSize = 0, ConstantPixelSize = 1, ConstantPhysicalSize = 2 }
    public enum PanelScreenMatchMode { MatchWidthOrHeight = 0, Shrink = 1, Expand = 2 }

    public class PanelSettings : ScriptableObject
    {
        public PanelScaleMode scaleMode { get; set; } = PanelScaleMode.ScaleWithScreenSize;
        public PanelScreenMatchMode screenMatchMode { get; set; } = PanelScreenMatchMode.MatchWidthOrHeight;
        public Vector2Int referenceResolution { get; set; } = new Vector2Int(1920, 1080);
        public float match { get; set; } = 0.5f;
        public float sortingOrder { get; set; } = 0f;
        public bool clearColor { get; set; } = true;
        public Color colorClearValue { get; set; } = Color.black;
    }

    public class ThemeStyleSheet : ScriptableObject { }

    public class UIDocument : MonoBehaviour
    {
        public VisualTreeAsset? visualTreeAsset { get; set; }
        public PanelSettings? panelSettings { get; set; }
        public VisualElement rootVisualElement { get; } = new VisualElement();
    }
}

namespace UnityEngine.TextCore.Text
{
    public class FontAsset : ScriptableObject
    {
        public Texture2D[]? atlasTextures { get; set; }
        public Material? material { get; set; }
        public static FontAsset? CreateFontAsset(Font font, int samplingPointSize, int atlasPadding, GlyphRenderMode renderMode, int atlasWidth, int atlasHeight, AtlasPopulationMode mode, bool enableMultiAtlasSupport) => ScriptableObject.CreateInstance<FontAsset>();
    }

    public enum GlyphRenderMode { SDFAA }
    public enum AtlasPopulationMode { Dynamic }
    public class Material : UnityEngine.Object { }
}

namespace UnityEngine.TextCore.LowLevel
{
    public enum GlyphRenderMode { SDFAA }
}
