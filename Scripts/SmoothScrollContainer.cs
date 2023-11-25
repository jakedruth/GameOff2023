using Godot;
using System;

[GlobalClass]
public partial class SmoothScrollContainer : ScrollContainer
{
    [Export] public double speed;
    [Export] public double offset;

    private Control _currentFocus;
    private double _t;
    private double _hStart;
    private double _hTarget;

    public override void _Ready()
    {
        base._Ready();
        GetViewport().GuiFocusChanged += OnFocusChange;
        ScrollHorizontal = (int)offset;
    }

    private void OnFocusChange(Control node)
    {
        _currentFocus = node;
        _t = 0;
        _hStart = ScrollHorizontal;
        _hTarget = ((Control)_currentFocus.GetParent()).Position.X + offset;
        GD.Print(((Control)_currentFocus.GetParent()).Position);
    }

    public override void _Process(double delta)
    {
        if (_currentFocus == null || _t >= 1)
            return;

        _t += Mathf.Clamp(speed * delta, 0, 1);
        double k = EaseOut(_t);
        ScrollHorizontal = Mathf.RoundToInt(Mathf.Lerp(_hStart, _hTarget, k));
    }

    private double EaseIn(double x)
    {
        return x * x;
    }

    private double EaseOut(double x)
    {
        return 1 - (1 - x) * (1 - x);
    }

    private double EaseInOut(double x)
    {
        return x < 0.5
            ? 2 * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }
}
