
using System;

namespace CosmosEngine.InputModule
{
	[Flags]
	public enum GamepadButton
	{
		/// <summary>Directional pad up.</summary>
		DPadUp = 1,
		/// <summary>Directional pad down.</summary>
		DPadDown = 2,
		/// <summary>Directional pad left.</summary>
		DPadLeft = 4,
		/// <summary>Directional pad right.</summary>
		DPadRight = 8,
		/// <summary>START button.</summary>
		Start = 16, // 0x00000010
		/// <summary>BACK button.</summary>
		Back = 32, // 0x00000020
		/// <summary>Left stick button (pressing the left stick).</summary>
		LeftStick = 64, // 0x00000040
		/// <summary>Right stick button (pressing the right stick).</summary>
		RightStick = 128, // 0x00000080
		/// <summary>Left bumper (shoulder) button.</summary>
		LeftShoulder = 256, // 0x00000100
		/// <summary>Right bumper (shoulder) button.</summary>
		RightShoulder = 512, // 0x00000200
		/// <summary>Big button.</summary>
		BigButton = 2048, // 0x00000800
		/// <summary>A button.</summary>
		A = 4096, // 0x00001000
		/// <summary>B button.</summary>
		B = 8192, // 0x00002000
		/// <summary>X button.</summary>
		X = 16384, // 0x00004000
		/// <summary>Y button.</summary>
		Y = 32768, // 0x00008000
		/// <summary>Left stick is towards the left.</summary>
		LeftThumbstickLeft = 2097152, // 0x00200000
		/// <summary>Right trigger.</summary>
		RightTrigger = 4194304, // 0x00400000
		/// <summary>Left trigger.</summary>
		LeftTrigger = 8388608, // 0x00800000
		/// <summary>Right stick is towards up.</summary>
		RightThumbstickUp = 16777216, // 0x01000000
		/// <summary>Right stick is towards down.</summary>
		RightThumbstickDown = 33554432, // 0x02000000
		/// <summary>Right stick is towards the right.</summary>
		RightThumbstickRight = 67108864, // 0x04000000
		/// <summary>Right stick is towards the left.</summary>
		RightThumbstickLeft = 134217728, // 0x08000000
		/// <summary>Left stick is towards up.</summary>
		LeftThumbstickUp = 268435456, // 0x10000000
		/// <summary>Left stick is towards down.</summary>
		LeftThumbstickDown = 536870912, // 0x20000000
		/// <summary>Left stick is towards the right.</summary>
		LeftThumbstickRight = 1073741824, // 0x40000000
	}
}