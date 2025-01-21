using Godot;
using Godot.Collections;

namespace SteamP2P.Scripts;

public partial class PlayerSpawner : Node {
	//note DEPRECATED
	
	[Export] private PackedScene? _playerNode;
	[Export] private MultiplayerSpawner? _playerSpawner;
	private Dictionary<int, Node> _players = new();

	public override void _Ready() {
		base._Ready();
		if (!HasComponents()) return;

		_playerSpawner!.AddSpawnableScene(_playerNode!.ResourcePath);
		_playerSpawner.Spawn(1);
		if (!IsMultiplayerAuthority()) return;
		Multiplayer.PeerConnected += id => {
			_playerSpawner.Spawn(1);
		};
		Multiplayer.PeerDisconnected += id => {
			RemovePlayer(1);
		};
	}

	public void SpawnPlayer(int playerLobbyID) {
		if (!HasComponents()) return;

		var instantiatedPlayer = _playerNode!.Instantiate();
		instantiatedPlayer.SetMultiplayerAuthority(playerLobbyID);
		_players.Add(playerLobbyID, instantiatedPlayer);
	}

	public void RemovePlayer(int playerLobbyID) {
		if (!HasComponents()) return;

		_players[playerLobbyID].QueueFree();
		_players.Remove(playerLobbyID);
	}

	private bool HasComponents() {
		if (_playerNode == null) {
			GD.PrintErr($"[PlayerSpawner] PlayerNode is null");
			return false;
		}

		if (_playerSpawner == null) {
			GD.PrintErr($"[PlayerSpawner] PlayerSpawner is null");
			return false;
		}

		return true;
	}
}