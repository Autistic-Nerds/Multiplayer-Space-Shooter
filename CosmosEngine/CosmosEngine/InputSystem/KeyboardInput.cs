using System;
using System.Collections.Generic;
using System.Text;
using CosmosEngine.InputModule;

namespace CosmosEngine
{
	public class KeyboardInput
	{
		private Stack<char> previousDeleted = new Stack<char>();

		public enum KeyboardModifier
		{
			None,
			Shift,
			Alt,
			Ctrl,
		}

		private readonly StringBuilder input = new StringBuilder();

		public string Read => input.ToString();

		public void Clear()
		{
			input.Clear();
		}

		public void ReadNext()
		{
			KeyboardModifier modifier = KeyboardModifier.None;

			if (InputState.Held(Keys.LeftShift) || InputState.Held(Keys.RightShift))
				modifier = KeyboardModifier.Shift;
			else if (InputState.Held(Keys.RightAlt))
				modifier = KeyboardModifier.Alt;

			bool caps = (InputState.KeyboardState.CapsLock || modifier == KeyboardModifier.Shift) && !(InputState.KeyboardState.CapsLock && modifier == KeyboardModifier.Shift);



			if (InputState.Pressed(Keys.Back))
			{
				if (InputState.Held(Keys.LeftAlt))
				{
					if (previousDeleted.TryPop(out char result))
					{
						input.Append(result);
					}
				}
				else
				{
					previousDeleted.Push(input[input.Length - 1]);
					input.Length--;
				}
			}
			else
			{

			}


			if (InputState.Pressed(Keys.A))
				input.Append((caps ? "A" : "a"));
			else if (InputState.Pressed(Keys.B))
				input.Append((caps ? "B" : "b"));
			else if (InputState.Pressed(Keys.C))
				input.Append((caps ? "C" : "c"));
			else if (InputState.Pressed(Keys.D))
				input.Append((caps ? "D" : "d"));
			else if (InputState.Pressed(Keys.E))
				input.Append((caps ? "E" : "e"));
			else if (InputState.Pressed(Keys.F))
				input.Append((caps ? "F" : "f"));
			else if (InputState.Pressed(Keys.G))
				input.Append((caps ? "G" : "g"));
			else if (InputState.Pressed(Keys.H))
				input.Append((caps ? "H" : "h"));
			else if (InputState.Pressed(Keys.I))
				input.Append((caps ? "I" : "i"));
			else if (InputState.Pressed(Keys.J))
				input.Append((caps ? "J" : "j"));
			else if (InputState.Pressed(Keys.K))
				input.Append((caps ? "K" : "k"));
			else if (InputState.Pressed(Keys.L))
				input.Append((caps ? "L" : "l"));
			else if (InputState.Pressed(Keys.M))
				input.Append((caps ? "M" : "m"));
			else if (InputState.Pressed(Keys.N))
				input.Append((caps ? "N" : "n"));
			else if (InputState.Pressed(Keys.O))
				input.Append((caps ? "O" : "o"));
			else if (InputState.Pressed(Keys.P))
				input.Append((caps ? "P" : "p"));
			else if (InputState.Pressed(Keys.Q))
				input.Append((caps ? "Q" : "q"));
			else if (InputState.Pressed(Keys.R))
				input.Append((caps ? "R" : "r"));
			else if (InputState.Pressed(Keys.S))
				input.Append((caps ? "S" : "s"));
			else if (InputState.Pressed(Keys.T))
				input.Append((caps ? "T" : "t"));
			else if (InputState.Pressed(Keys.U))
				input.Append((caps ? "U" : "u"));
			else if (InputState.Pressed(Keys.V))
				input.Append((caps ? "V" : "v"));
			else if (InputState.Pressed(Keys.W))
				input.Append((caps ? "W" : "w"));
			else if (InputState.Pressed(Keys.X))
				input.Append((caps ? "X" : "x"));
			else if (InputState.Pressed(Keys.Y))
				input.Append((caps ? "Y" : "y"));
			else if (InputState.Pressed(Keys.Z))
				input.Append((caps ? "Z" : "z"));

			else if (InputState.Pressed(Keys.D1))
				input.Append((modifier == KeyboardModifier.Shift ? "!" :
					modifier == KeyboardModifier.Alt ? "" : "1"));
			else if (InputState.Pressed(Keys.D2))
				input.Append((modifier == KeyboardModifier.Shift ? "\"" :
					modifier == KeyboardModifier.Alt ? "@" : "2"));
			else if (InputState.Pressed(Keys.D3))
				input.Append((modifier == KeyboardModifier.Shift ? "#" :
					modifier == KeyboardModifier.Alt ? "£" : "3"));
			else if (InputState.Pressed(Keys.D4))
				input.Append((modifier == KeyboardModifier.Shift ? "¤" :
					modifier == KeyboardModifier.Alt ? "$" : "4"));
			else if (InputState.Pressed(Keys.D5))
				input.Append((modifier == KeyboardModifier.Shift ? "%" :
					modifier == KeyboardModifier.Alt ? "€" : "5"));
			else if (InputState.Pressed(Keys.D6))
				input.Append((modifier == KeyboardModifier.Shift ? "&" :
					modifier == KeyboardModifier.Alt ? "" : "6"));
			else if (InputState.Pressed(Keys.D7))
				input.Append((modifier == KeyboardModifier.Shift ? "/" :
					modifier == KeyboardModifier.Alt ? "{" : "7"));
			else if (InputState.Pressed(Keys.D8))
				input.Append((modifier == KeyboardModifier.Shift ? "(" :
					modifier == KeyboardModifier.Alt ? "[" : "8"));
			else if (InputState.Pressed(Keys.D9))
				input.Append((modifier == KeyboardModifier.Shift ? ")" :
					modifier == KeyboardModifier.Alt ? "]" : "9"));
			else if (InputState.Pressed(Keys.D0))
				input.Append((modifier == KeyboardModifier.Shift ? "=" :
					modifier == KeyboardModifier.Alt ? "}" : "0"));

			else if (InputState.Pressed(Keys.Space))
				input.Append(" ");
			else if (InputState.Pressed(Keys.Enter))
				input.Append("\n");
			else if (InputState.Pressed(Keys.Tab))
				input.Append("\t");
		}
	}
}