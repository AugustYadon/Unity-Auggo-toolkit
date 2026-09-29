# Samples

One folder per showcase piece, each with its own scene.

```
Samples~/
  WheelPicker/
    WheelPicker.unity
    README.md
  DateRangePicker/
    ...
```

Register each in `package.json` so Package Manager offers an Import button:

```json
"samples": [
  {
    "displayName": "Wheel Picker",
    "description": "Scrollable value picker with snapping",
    "path": "Samples~/WheelPicker"
  }
]
```

Nothing here yet — the first candidates are the wheel picker and date-range picker from
Quantum Habits.
