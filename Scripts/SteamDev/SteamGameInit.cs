using Godot;
using GodotSteam;
using SteamP2P.Scripts.Moons.Helpers;

namespace SteamP2P.Scripts.SteamDev;

public partial class SteamGameInit : Node {
	public override void _Ready() {
		base._Ready();
		OS.SetEnvironment(SteamStrings.AppID, 480.ToString());
		OS.SetEnvironment(SteamStrings.GameID, 480.ToString());
		Steam.SteamInitEx(true);
	}

	public override void _Process(double delta) {
		base._Process(delta);
		Steam.RunCallbacks();
	}
}