using CosmosEngine;
using CosmosEngine.Collection;
using CosmosEngine.Netcode;
using CosmosEngine.UI;
using System.Collections.Generic;

namespace SpaceBattle
{
	internal class NetworkChat : NetcodeBehaviour
	{
		private string chatMsg = "";
		private KeyboardInput keyboard;
		private TextField textchatObject;
		private string chatLog = "";
		private bool chatOpen;

		private readonly DirtyList<ChatMessage> chatMessages = new DirtyList<ChatMessage>();

		public class ChatMessage
		{
			private float timeToLive;
			private TextField textField;
			private float x;
			private float alpha;
			private bool alive;

			public ChatMessage(float timeToLive, TextField textField, float x)
			{
				this.timeToLive = timeToLive;
				this.textField = textField;
				this.x = x;
				this.alpha = 1.0f;
				this.alive = true;
			}

			public bool Alive => alive;

			public void Position(int y)
			{
				textField.RectTransform.Position = new Vector2(x, y);
			}

			public void Tick()
			{
				if(timeToLive <= 0.0f)
				{
					alive = false;
					textField.GameObject.Enabled = false;
				}
				else
				{
					timeToLive -= Time.DeltaTime;
				}
			}
		}

		protected override void Start()
		{
			keyboard = new KeyboardInput();
			GameObject go = new GameObject("Chat Window");
			go.AddComponent<Image>().Colour = new Colour(0, 0, 0, 80);
			textchatObject = go.AddComponent<TextField>();
			textchatObject.RectTransform.SizeDelta = new Vector2(320, 25);
			textchatObject.FontSize = 11;
			textchatObject.Font = Font.Inter;
			textchatObject.RectTransform.Anchour = new Vector2(0.025f, 0.95f);
			textchatObject.HorizontalAlignment = HorizontalAlignment.Left;
			textchatObject.VerticalAlignment = VerticalAlignment.Middle;
		}

		protected override void Update()
		{
			if (NetcodeHandler.IsConnected)
			{
				if (chatOpen)
				{
					if (!textchatObject.GameObject.Enabled)
						textchatObject.GameObject.SetActive(true);
					if(InputState.Pressed(CosmosEngine.InputModule.Keys.Escape))
					{
						chatOpen = false;
						keyboard.Clear();
						return;
					}
					if (InputState.Pressed(CosmosEngine.InputModule.Keys.Enter))
					{
						chatMsg = keyboard.Read;
						if(NetcodeHandler.IsServer)
						{
							SendChatMessage(chatMsg);
						}
						else
						{ 
							Rpc(nameof(SendChatMessage), null, chatMsg);
						}
						keyboard.Clear();
						chatMsg = "";
						chatOpen = false;
					}
					else
					{
						keyboard.ReadNext();
						chatMsg = keyboard.Read;
					}
				}
				else
				{
					if (textchatObject.GameObject.Enabled)
						textchatObject.GameObject.SetActive(false);
					if (InputState.Pressed(CosmosEngine.InputModule.Keys.Enter))
					{
						chatOpen = true;
					}
				}
			}

			for(int i = 0; i < chatMessages.Count; i++)
			{
				ChatMessage msg = chatMessages[i];
				msg.Tick();
				msg.Position(-25 - (i * 25));
				if (!msg.Alive)
					chatMessages.IsDirty = true;
			}
			chatMessages.DisposeAll(item => !item.Alive);


			if (textchatObject != null)
			{
				textchatObject.Text = chatMsg;
			}
		}

		[ServerRPC(ignoreAuthority = true)]
		private void SendChatMessage(string msg)
		{
			RelayChatMessage(msg);
			Rpc(nameof(RelayChatMessage), null, msg);
		}

		[ClientRPC]
		private void RelayChatMessage(string msg)
		{
			if (string.IsNullOrWhiteSpace(msg))
				return;

			GameObject go = new GameObject("Chat Window");
			go.AddComponent<Image>().Colour = new Colour(0, 0, 0, 80);
			TextField textField = go.AddComponent<TextField>();
			textField.RectTransform.SizeDelta = new Vector2(320, 25);
			textField.FontSize = 11;
			textField.Font = Font.Inter;
			textField.RectTransform.Anchour = new Vector2(0.025f, 0.95f);
			textField.HorizontalAlignment = HorizontalAlignment.Left;
			textchatObject.VerticalAlignment = VerticalAlignment.Middle;

			textField.Text = msg;
			float ttl = Mathf.Max(1f, (((msg.Length / 4.7f) * 100f) / 60f));
			chatMessages.Insert(0, new ChatMessage(ttl, textField, textchatObject.RectTransform.Position.X));

			chatLog += $"\n{msg}";
		}
	}
}