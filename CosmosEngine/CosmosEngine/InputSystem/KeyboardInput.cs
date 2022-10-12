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
			var keys = InputState.KeyboardState.GetPressedKeys();
			string s = "";
			foreach(var k in keys)
			{
				s += $"{k} ";
			}
			Debug.QuickLog(s);
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
					if (input.Length > 0)
					{
						previousDeleted.Push(input[input.Length - 1]);
						input.Length--;
					}
				}
			}
			else
			{

			}


			if (InputState.Pressed(Keys.A))
				input.Append((caps ? "A" : 
					modifier == KeyboardModifier.Alt ? "" : "a"));
			else if (InputState.Pressed(Keys.B))
				input.Append((caps ? "B" :
					modifier == KeyboardModifier.Alt ? "" : "b"));
			else if (InputState.Pressed(Keys.C))
				input.Append((caps ? "C" :
					modifier == KeyboardModifier.Alt ? "" : "c"));
			else if (InputState.Pressed(Keys.D))
				input.Append((caps ? "D" :
					modifier == KeyboardModifier.Alt ? "" : "d"));
			else if (InputState.Pressed(Keys.E))
				input.Append((caps ? "E" :
					modifier == KeyboardModifier.Alt ? "" : "e"));
			else if (InputState.Pressed(Keys.F))
				input.Append((caps ? "F" :
					modifier == KeyboardModifier.Alt ? "" : "f"));
			else if (InputState.Pressed(Keys.G))
				input.Append((caps ? "G" :
					modifier == KeyboardModifier.Alt ? "" : "g"));
			else if (InputState.Pressed(Keys.H))
				input.Append((caps ? "H" :
					modifier == KeyboardModifier.Alt ? "" : "h"));
			else if (InputState.Pressed(Keys.I))
				input.Append((caps ? "I" :
					modifier == KeyboardModifier.Alt ? "" : "i"));
			else if (InputState.Pressed(Keys.J))
				input.Append((caps ? "J" :
					modifier == KeyboardModifier.Alt ? "" : "j"));
			else if (InputState.Pressed(Keys.K))
				input.Append((caps ? "K" :
					modifier == KeyboardModifier.Alt ? "" : "k"));
			else if (InputState.Pressed(Keys.L))
				input.Append((caps ? "L" :
					modifier == KeyboardModifier.Alt ? "" : "l"));
			else if (InputState.Pressed(Keys.M))
				input.Append((caps ? "M" :
					modifier == KeyboardModifier.Alt ? "" : "m"));
			else if (InputState.Pressed(Keys.N))
				input.Append((caps ? "N" :
					modifier == KeyboardModifier.Alt ? "" : "n"));
			else if (InputState.Pressed(Keys.O))
				input.Append((caps ? "O" :
					modifier == KeyboardModifier.Alt ? "" : "o"));
			else if (InputState.Pressed(Keys.P))
				input.Append((caps ? "P" :
					modifier == KeyboardModifier.Alt ? "" : "p"));
			else if (InputState.Pressed(Keys.Q))
				input.Append((caps ? "Q" :
					modifier == KeyboardModifier.Alt ? "" : "q"));
			else if (InputState.Pressed(Keys.R))
				input.Append((caps ? "R" :
					modifier == KeyboardModifier.Alt ? "" : "r"));
			else if (InputState.Pressed(Keys.S))
				input.Append((caps ? "S" :
					modifier == KeyboardModifier.Alt ? "" : "s"));
			else if (InputState.Pressed(Keys.T))
				input.Append((caps ? "T" :
					modifier == KeyboardModifier.Alt ? "" : "t"));
			else if (InputState.Pressed(Keys.U))
				input.Append((caps ? "U" :
					modifier == KeyboardModifier.Alt ? "" : "u"));
			else if (InputState.Pressed(Keys.V))
				input.Append((caps ? "V" :
					modifier == KeyboardModifier.Alt ? "" : "v"));
			else if (InputState.Pressed(Keys.W))
				input.Append((caps ? "W" :
					modifier == KeyboardModifier.Alt ? "" : "w"));
			else if (InputState.Pressed(Keys.X))
				input.Append((caps ? "X" :
					modifier == KeyboardModifier.Alt ? "" : "x"));
			else if (InputState.Pressed(Keys.Y))
				input.Append((caps ? "Y" :
					modifier == KeyboardModifier.Alt ? "" : "y"));
			else if (InputState.Pressed(Keys.Z))
				input.Append((caps ? "Z" :
					modifier == KeyboardModifier.Alt ? "" : "z"));

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

			else if (InputState.Pressed(Keys.OemComma))
				input.Append((
					modifier == KeyboardModifier.Shift ? ";" :
					modifier == KeyboardModifier.Alt ? "" : ","));
			else if (InputState.Pressed(Keys.OemPeriod))
				input.Append((
					modifier == KeyboardModifier.Shift ? ":" :
					modifier == KeyboardModifier.Alt ? "" : "."));
			else if (InputState.Pressed(Keys.OemMinus))
				input.Append((
					modifier == KeyboardModifier.Shift ? "_" :
					modifier == KeyboardModifier.Alt ? "" : "-"));
			else if (InputState.Pressed(Keys.OemPipe))
				input.Append((
					modifier == KeyboardModifier.Shift ? "§" :
					modifier == KeyboardModifier.Alt ? "" : "½"));
			else if (InputState.Pressed(Keys.OemPlus))
				input.Append((
					modifier == KeyboardModifier.Shift ? "?" :
					modifier == KeyboardModifier.Alt ? "" : "+"));
			else if (InputState.Pressed(Keys.OemBackslash))
				input.Append((
					modifier == KeyboardModifier.Shift ? ">" :
					modifier == KeyboardModifier.Alt ? "\\" : "<"));

			//DANISH KEYBOARD SPECIFIC LAMO
			else if (InputState.Pressed(Keys.OemOpenBrackets))
				input.Append((
					modifier == KeyboardModifier.Shift ? "`" :
					modifier == KeyboardModifier.Alt ? "|" : "´"));
			else if (InputState.Pressed(Keys.OemSemicolon))
				input.Append((
					modifier == KeyboardModifier.Shift ? "^" :
					modifier == KeyboardModifier.Alt ? "~" : "¨"));
			else if (InputState.Pressed(Keys.OemCloseBrackets))
				input.Append((
					modifier == KeyboardModifier.Shift ? "Å" :
					modifier == KeyboardModifier.Alt ? "" : "å"));
			else if (InputState.Pressed(Keys.OemTilde))
				input.Append((
					modifier == KeyboardModifier.Shift ? "Æ" :
					modifier == KeyboardModifier.Alt ? "" : "æ"));
			else if (InputState.Pressed(Keys.OemQuotes))
				input.Append((
					modifier == KeyboardModifier.Shift ? "Ø" :
					modifier == KeyboardModifier.Alt ? "" : "ø"));
			else if (InputState.Pressed(Keys.OemQuestion))
				input.Append((
					modifier == KeyboardModifier.Shift ? "*" :
					modifier == KeyboardModifier.Alt ? ""  :
					modifier == KeyboardModifier.Alt ? "" : "'"));
		}
	}
}