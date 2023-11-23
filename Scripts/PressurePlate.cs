using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class PressurePlate : Area2D, ISwitch
{
    [Export] public bool CurrentState { get; set; }
    List<IInteractable> ISwitch.Interactables { get; set; }
    private Sprite2D _sprite;

    [Export] public Node2D[] interatableNodes;
    private List<Node2D> _activeNodes;


    public override void _Ready()
    {
        ((ISwitch)this).Init(interatableNodes);
        ((ISwitch)this).SetState(CurrentState);
        _activeNodes = new List<Node2D>();

        _sprite = GetChild<Sprite2D>(1);

        AreaEntered += OnNodeEntered;
        AreaExited += OnNodeExited;
        BodyEntered += OnNodeEntered;
        BodyExited += OnNodeExited;
    }

    private void OnNodeEntered(Node2D node)
    {
        _activeNodes.Add(node);
        if (_activeNodes.Count > 0)
            ((ISwitch)this).SetState(true);
    }

    private void OnNodeExited(Node2D node)
    {
        _activeNodes.Remove(node);
        if (_activeNodes.Count == 0)
            ((ISwitch)this).SetState(false);
    }

    void ISwitch.OnStateChanged(bool newState)
    {
        int y = newState ? 40 : 32;
        _sprite.RegionRect = new Rect2(48, y, 16, 8);
    }
}

