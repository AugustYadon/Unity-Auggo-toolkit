# XY Pad

`[XYPad]` draws a `Vector2` field as a square you drag in the Inspector. Both axes are 0..1,
up is 1, and the four axis labels are optional.

## Where it came from

**Atomic Tones, noise resonance (2026-09).** The app's noise has a resonant band you can move,
like reshaping your mouth around a "hhh" sound. The band has two settings that only make sense
together: where it sits (frequency) and how narrow it is (Q). Two separate sliders made it hard
to hear what the pair was doing. A pad lets you sweep both at once, the way a filter pad works
in a DAW, so we could find good defaults by ear before there was any in-app UI for it.

That's the kind of need it fills: two values that are tuned *together*, by feel, in Play mode.

## In this sample

`XYPad.unity` has one object, **XY Pad Demo**. Select it and drag in the Inspector:

- `plainPad` has no labels
- `labelledPad` has all four, and its readout shows mapping the 0..1 value to a real range
  (here 120–5000 Hz on a log scale, as Atomic Tones does)
