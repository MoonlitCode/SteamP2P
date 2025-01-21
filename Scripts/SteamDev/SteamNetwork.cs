using Godot;
using Godot.Collections;

namespace SteamP2P.Scripts.SteamDev;

public partial class SteamNetwork : Node {
	public static SteamNetwork? Instance { get; private set; }

	[Export] private PackedScene? _playerController;
	private Dictionary<long, PlayerController2D>? _playerControllerDict = new();
	private Node2D? _playerSpawnNode;

	// Network
	private const string SERVER_ADDRESS = "127.0.0.1";
	private const int SERVER_PORT = 8080;
	private const int MAX_CLIENTS = 4;

	public override void _Ready() {
		base._Ready();
		Instance ??= this;
	}

	public void HostServer() {
		GD.Print("HostServer");
		// Safety for when user dies and reference is changed reloading a scene.
		// Only needed Server-side because it's the only location players get added.
		_playerSpawnNode = (Node2D)GetTree().GetCurrentScene().GetNode("PlayerSpawnNode");

		var serverPeer = new ENetMultiplayerPeer();
		serverPeer.CreateServer(SERVER_PORT, MAX_CLIENTS);
		Multiplayer.MultiplayerPeer = serverPeer;
		Multiplayer.MultiplayerPeer.PeerConnected += OnPeerConnected;
		Multiplayer.MultiplayerPeer.PeerDisconnected += OnPeerDisconnected;
		// Needed for host ID to PlayerController
		OnPeerConnected(1);
	}

	public void JoinServer() {
		GD.Print("JoinServer");
		var clientPeer = new ENetMultiplayerPeer();
		clientPeer.CreateClient(SERVER_ADDRESS, SERVER_PORT);
		Multiplayer.MultiplayerPeer = clientPeer;
	}

	private void OnPeerConnected(long id) {
		GD.Print($"OnPeerConnected ID: {id}");
		var playerToAdd = _playerController?.Instantiate();
		if (playerToAdd is not PlayerController2D playerController2D) return;
		_playerControllerDict!.Add(id, playerController2D);
		_playerSpawnNode?.AddChild(playerController2D, true);
		playerController2D.SetPlayerID(id);
	}

	[Rpc]
	private void OnPeerDisconnected(long id) {
		GD.Print($"OnPeerDisconnected ID: {id}");
		var playerToRemove = _playerControllerDict?[id];
		if (playerToRemove == null) return;
		playerToRemove.QueueFree();
		_playerControllerDict?.Remove(id);
	}
}