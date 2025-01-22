using Godot;
using SteamP2P.Scripts.Moons.Helpers;

namespace SteamP2P.Scripts;

public partial class UnitMovementInputs : MultiplayerSynchronizer {
	[Export] public Vector2 DirectionalInput { get; private set; }
	[Export] public int PlayerID { get; private set; }

	public void SetPlayerID(int playerID) {
		PlayerID = playerID;
		CallDeferred(MethodName.UpdateMultiplayerAuthority);
	}

	private void UpdateMultiplayerAuthority() {
		SetMultiplayerAuthority(PlayerID);
		GD.Print($"UpdateMultiplayerAuthority: {PlayerID}");
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (GetMultiplayerAuthority() != PlayerID) SetMultiplayerAuthority(PlayerID);
		if (!IsMultiplayerAuthority()) return;
		DirectionalInput = Input.GetVector(
			InputStrings.MoveLeft,
			InputStrings.MoveRight,
			InputStrings.MoveUp,
			InputStrings.MoveDown
		).Normalized();
	}
}
