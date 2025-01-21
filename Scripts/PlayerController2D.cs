using Godot;
using SteamP2P.Scripts.Moons.Helpers;

namespace SteamP2P.Scripts;

public partial class PlayerController2D : Node {
	[Export] private MultiplayerSynchronizer? _authoritySynchronizer;
	[Export] private CharacterBody2D? _characterBody2D;
	[Export] private Sprite2D? _sprite2D;
	[Export] private float _moveSpeed = 6000.0f;
	[Export] private float _rotateSpeed = 5.0f;
	[Export] public long PlayerID { get; private set; } = 1;

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (!IsMultiplayerAuthority()) return;
		if (_characterBody2D == null) return;
		HandleMovement(delta);
		HandleRotation(delta);
	}

	public void SetPlayerID(long id) {
		PlayerID = id;
		Name = $"{nameof(PlayerController2D)}: {id.ToString()}";
		_authoritySynchronizer?.SetMultiplayerAuthority((int)id);
		SetMultiplayerAuthority((int)id);
		GD.Print($"{PlayerID} set PlayerID: {id}");
	}

	private void HandleMovement(double delta) {
		var velocity = _characterBody2D!.Velocity;
		var direction = Input.GetVector(
			InputStrings.MoveLeft,
			InputStrings.MoveRight,
			InputStrings.MoveUp,
			InputStrings.MoveDown
		).Normalized();

		if (direction != Vector2.Zero) {
			velocity.X = direction.X * _moveSpeed * (float)delta;
			velocity.Y = direction.Y * _moveSpeed * (float)delta;
		}
		else {
			velocity.X = Mathf.Lerp(velocity.X, 0.0f, (float)delta);
			velocity.Y = Mathf.Lerp(velocity.Y, 0.0f, (float)delta);
		}

		_characterBody2D.Velocity = velocity;
		_characterBody2D?.MoveAndSlide();
	}

	private void HandleRotation(double delta) {
		if (_sprite2D == null) return;
		var direction = Input.GetAxis(
			InputStrings.RotateCCW,
			InputStrings.RotateCW
		);
		_sprite2D.Rotation += direction * _rotateSpeed * (float)delta;
	}
}