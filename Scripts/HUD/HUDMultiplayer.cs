using System;
using Godot;

namespace SteamP2P.Scripts.HUD;

public partial class HUDMultiplayer : Control {
	public event EventHandler HostPressed;
	public event EventHandler ClientPressed;
	
	[Export] private Button? _hostButton;
	[Export] private Button? _clientButton;

	public override void _Ready() {
		base._Ready();
		if (_hostButton != null) _hostButton.Pressed += OnHostPressed;
		if (_clientButton != null) _clientButton.Pressed += OnClientPressed;
	}

	private void OnHostPressed() => HostPressed(this, EventArgs.Empty);
	private void OnClientPressed() => ClientPressed(this, EventArgs.Empty);
}