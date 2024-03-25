using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int score;
	public void game_over()
	{
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
	}
	public void NewGame()
	{
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		score = 0;
		var player = GetNode<player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		GetNode<Timer>("StartTimer").Start();
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(score);
		hud.ShowMessage("Get Ready!");
		GetNode<AudioStreamPlayer>("Music").Play();
	}
	private void OnMobTimerTimeout()
	{
	// Note: Normally it is best to use explicit types rather than the `var`
	// keyword. However, var is acceptable to use here because the types are
	// obviously Mob and PathFollow2D, since they appear later on the line.

	// Create a new instance of the Mob scene.
	mob newmob = MobScene.Instantiate<mob>();

	// Choose a random location on Path2D.
	var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
	mobSpawnLocation.ProgressRatio = GD.Randf();

	// Set the mob's direction perpendicular to the path direction.
	float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

	// Set the mob's position to a random location.
	newmob.Position = mobSpawnLocation.Position;

	// Add some randomness to the direction.
	direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
	newmob.Rotation = direction;

	// Choose the velocity.
	var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
	newmob.LinearVelocity = velocity.Rotated(direction);

	// Spawn the mob by adding it to the Main scene.
	AddChild(newmob);
}
	private void OnScoreTimerTimeout()
	{
		score++;
		GetNode<HUD>("HUD").UpdateScore(score);
	}
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
