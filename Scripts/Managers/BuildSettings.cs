using Godot;
using System;

[GlobalClass]
public partial class BuildSettings : Resource
{
	[Export] private string[] _levelPaths { get; set; }

	public BuildSettings()
	{
		_levelPaths = new string[0];
	}

	public PackedScene GetLevel(int index)
	{
		string path = _levelPaths[index];
		if (GD.Load(path) is PackedScene scene)
			return scene;

		// Handle bad path or index
		return null;
	}
}
