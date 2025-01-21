using System;
using Godot;
using SteamP2P.Scripts.HUD;
using SteamP2P.Scripts.SteamDev;

namespace SteamP2P.Scripts;

public partial class Game : Node {
	[Export] private HUDMultiplayer? _hudMultiplayer;

	public override void _Ready() {
		base._Ready();
		if (_hudMultiplayer == null) return;
		_hudMultiplayer.HostPressed += HUDMultiplayer_OnHostPressed;
		_hudMultiplayer.ClientPressed += HUDMultiplayer_OnClientPressed;
	}

	private void HUDMultiplayer_OnHostPressed(object? sender, EventArgs e) {
		SteamNetwork.Instance?.HostServer();
		_hudMultiplayer!.Visible = false;
	}

	private void HUDMultiplayer_OnClientPressed(object? sender, EventArgs e) {
		SteamNetwork.Instance?.JoinServer();
		_hudMultiplayer!.Visible = false;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (Input.IsActionJustPressed("ui_cancel")) GetTree().Quit();
	}
}