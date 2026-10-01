using UnityEngine;

namespace AuggoToolkit.Samples
{
    /// Select the "XY Pad Demo" object and drag inside the squares in the Inspector.
    public class XYPadDemo : MonoBehaviour
    {
        [XYPad] public Vector2 plainPad = new Vector2(0.5f, 0.5f);

        [Tooltip("Labels are optional: left, right, bottom, top.")]
        [XYPad("low", "high", "broad", "narrow")] public Vector2 labelledPad = new Vector2(0.75f, 0.2f);

        [Tooltip("Pads store 0..1. Map them to real ranges yourself; this one maps x to 120-5000 Hz on a log scale.")]
        public string labelledPadAsHz;

        void OnValidate()
        {
            labelledPadAsHz = Mathf.Round(120f * Mathf.Pow(5000f / 120f, labelledPad.x)) + " Hz";
        }
    }
}
