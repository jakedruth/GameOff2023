using Godot;
using System;

[GlobalClass]
public partial class BuildSettings : Resource
{
	[Export] private string mainMenuPath;
	[Export] private string levelSelectPath;
	[Export] private LevelInfo[] levels { get; set; }
	public int LevelCount => levels.Length;
	public LevelInfo GetLevelInfo(int index) => levels[index];

	public BuildSettings()
	{
		levels = new LevelInfo[0];
	}

	public PackedScene GetMainMenu()
	{
		if (GD.Load(mainMenuPath) is PackedScene scene)
			return scene;

		// Handle bad path
		return null;
	}

	public PackedScene GetLevelSelect()
	{
		if (GD.Load(levelSelectPath) is PackedScene scene)
			return scene;

		// Handle bad path
		return null;
	}

	public PackedScene GetLevel(int index)
	{
		string path = levels[index].path;
		if (GD.Load(path) is PackedScene scene)
			return scene;

		// Handle bad path or index
		return null;
	}
}