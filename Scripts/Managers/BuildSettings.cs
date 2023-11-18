using Godot;
using System;

[GlobalClass]
public partial class BuildSettings : Resource
{
	[Export(PropertyHint.File)] private string[] levelPaths;

	public PackedScene GetLevel(int index)
	{
		string path = levelPaths[index];
		if (GD.Load(path) is PackedScene scene)
			return scene;

		// Handle bad path or index
		return null;
	}
}
