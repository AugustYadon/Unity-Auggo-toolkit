using UnityEngine;

namespace AuggoToolkit
{
    /// Draws a Vector2 field as a square you drag in the Inspector, both axes 0..1 (up is 1).
    /// Handy for any pair of values that are fun to move together, like a filter's frequency and Q.
    ///
    ///   [XYPad] public Vector2 pad;
    ///   [XYPad("low", "high", "broad", "narrow")] public Vector2 resonance;
    ///
    /// The labels are optional hints drawn inside the pad: left, right, bottom, top.
    ///
    /// This class lives in Runtime/, not Editor/, because the scripts that use the tag ship in
    /// builds. The drawer that paints it is editor-only, in Editor/XYPadDrawer.cs.
    public class XYPadAttribute : PropertyAttribute
    {
        public readonly string Left, Right, Bottom, Top;

        public XYPadAttribute(string left = null, string right = null, string bottom = null, string top = null)
        {
            Left = left;
            Right = right;
            Bottom = bottom;
            Top = top;
        }
    }
}
