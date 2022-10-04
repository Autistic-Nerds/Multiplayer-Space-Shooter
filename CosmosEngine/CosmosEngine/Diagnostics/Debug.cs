
using CosmosEngine.Diagnostics;
using CosmosEngine.Modules;
using CosmosEngine.Rendering;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CosmosEngine
{
	public class Debug : EditorModule<Debug>, IUpdateModule, IRenderModule
	{
		/// <summary>
		/// <list type="bullet">
		/// <item><see cref="CosmosEngine.LogOption.Collapse"/></item>
		/// </list>
		/// </summary>
		public const LogOption DefaultOption = LogOption.Collapse;
		private LogMessage current;
		private bool updating;
		private float delay;
		private readonly List<LogMessage> itemsToAdd = new List<LogMessage>();
		private readonly List<LogMessage> logMessages = new List<LogMessage>();

		private bool displayUi;
		private Vector2 displayPosition;

		internal List<LogMessage> LogMessages => logMessages;

		public static bool DisplayLogs { get => Instance.displayUi; set => Instance.displayUi = value; }
		public static Vector2 DisplayPosition { get => Instance.displayPosition; set => Instance.displayPosition = value; }

		public override void Initialize()
		{
			base.Initialize();
			displayUi = true;
			displayPosition = new Vector2(5, 45);
		}

		/// <summary>
		/// Logs a warning message to the debug console.
		/// </summary>
		/// <param name="message"></param>
		public static void LogWarning(object message) => Log(message, LogFormat.Warning);
		/// <summary>
		/// Logs an error message to the debug console.
		/// </summary>
		/// <param name="message"></param>
		public static void LogError(object message) => Log(message, LogFormat.Error);

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Debug.Log(object, LogFormat, object, LogOption)"/>
		/// </summary>
		public static void Log(object message) => Log(message, LogFormat.Message);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Debug.Log(object, LogFormat, object, LogOption)"/>
		/// </summary>
		public static void Log(object message, LogFormat format) => Log(message, format, null);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Debug.Log(object, LogFormat, object, LogOption)"/>
		/// </summary>
		public static void Log(object message, LogFormat format, object context) => Log(message, format, context, DefaultOption);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Debug.Log(object, LogFormat, object, LogOption)"/>
		/// </summary>
		public static void Log(object message, LogFormat format, LogOption option) => Log(message, format, null, option);
		/// <summary>
		/// Logs a message to the debug console.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="format"></param>
		/// <param name="context"></param>
		/// <param name="option"></param>
		[Conditional("EDITOR")]
		public static void Log(object message, LogFormat format, object context, LogOption option)
		{
			if (!Exist)
				return;

			string initial = "";
			//string stacktrace = "";
			List<string> stacktrace = new List<string>();
			StackTrace trace = new StackTrace(true);
			StackFrame[] frames = trace.GetFrames();
			int lastValidIndex = -1;
			for (int i = 0; i < frames.Length - 1; i++)
			{
				StackFrame frame = frames[i];
				MethodBase method = frame.GetMethod();
				Type declaringType = method.DeclaringType;
				if (declaringType == typeof(Debug))
					continue;
				if (string.IsNullOrEmpty(frame.GetFileName()))
					continue;
				if (!string.IsNullOrEmpty(declaringType.Namespace))
				{
					if (declaringType.Namespace.Contains("Microsoft"))
						continue;
					if(!declaringType.Namespace.Contains("CosmosEngine"))
					{
						lastValidIndex = stacktrace.Count;
					}
				}

				string[] filePath = frame.GetFileName().Split('\\');
				string fileName = filePath[filePath.Length - 1];

				string stackTraceText = $"{declaringType.FullName}:{method.Name}{method.GetParameters().ParameterName()}" +
					$" at <colour>{fileName}:{frame.GetFileLineNumber()}";
				stacktrace.Add(stackTraceText);

				if (string.IsNullOrEmpty(initial))
				{
					initial = $"{declaringType.FullName}.{method.Name}";
					if (option.HasFlag(LogOption.NoStacktrace))
						break;
				}
			}

			System.Text.StringBuilder finalTrace = new System.Text.StringBuilder();
			for (int i = 0; i <= lastValidIndex; i++)
			{
				if (i > 0)
					finalTrace.AppendLine();
				finalTrace.Append(stacktrace[i]);
			}

			LogMessage log = new LogMessage(message == null ? "null" : message.ToString(),
				format, option, finalTrace.ToString(), initial);

			if (context != null)
			{
				if (option.HasFlag(LogOption.Collection))
				{
					IEnumerable enumerable = (context as IEnumerable);
					if (enumerable != null && !(context is string))
					{
						log.Table = enumerable;
					}
				}
			}
			Instance.AddLog(log);
		}

		[Conditional("EDITOR")]
		public void AddLog(LogMessage log)
		{
			if (updating)
			{
				itemsToAdd.Add(log);
			}
			else
			{
				if (log.Option.HasFlag(LogOption.Collapse))
				{
					int index = Instance.logMessages.FindIndex(item => item.Compare(log, log.Option.HasFlag(LogOption.CompareInitialCallOnly)));
					if (index >= 0)
					{
						if (!log.Option.HasFlag(LogOption.IgnoreCallCount))
							LogMessages[index].Count++;
						if(log.Option.HasFlag(LogOption.Collection))
						{
							LogMessages[index].Table = log.Table;
						}
						LogMessages[index].Message = log.Message;
						return;
					}
				}
				logMessages.Add(log);
			}
		}

		public static void FormatLog(string message, params object[] args) => FormatLog(message, LogFormat.Message, args);

		[Conditional("EDITOR")]
		public static void FormatLog(string message, LogFormat format, params object[] args)
		{
			string log = string.Format(message, args);
			Log(log, format);
		}

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Debug.QuickLog(object, LogFormat)"/>
		/// </summary>
		/// <param name="message"></param>
		public static void QuickLog(object message) => QuickLog(message, LogFormat.Message);

		/// <summary>
		/// Logs a message to the debug console. It's only possible to invoke one QuickLog from a method, only the newest QuickLog message will be displayed.
		/// <list type="bullet">
		/// <item><see cref="CosmosEngine.LogOption.Collapse"/></item>
		/// <item><see cref="CosmosEngine.LogOption.CompareInitialCallOnly"/></item>
		/// <item><see cref="CosmosEngine.LogOption.IgnoreCallCount"/></item>
		/// </list>
		/// </summary>
		[Conditional("EDITOR")]

		public static void QuickLog(object message, LogFormat format) => Log(message, format, DefaultOption | LogOption.CompareInitialCallOnly | LogOption.IgnoreCallCount);

		public static void LogTable(IEnumerable context)
		{
			LogTable(string.Empty, context);
		}
		public static void LogTable(object message, IEnumerable context)
		{
			LogTable(message, context, LogFormat.Message);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="context"></param>
		/// <param name="format"></param>
		[Conditional("EDITOR")]
		public static void LogTable(object message, IEnumerable context, LogFormat format)
		{
			if (context == null)
			{
				Debug.Log($"Can't log a null table.", LogFormat.Warning);
				return;
			}
			if (message == null || (message is string && string.IsNullOrEmpty((string)message)))
				message = $"{context.GetType().Name}";
			Log(message, format, context, DefaultOption | LogOption.IgnoreCallCount | LogOption.Collection | LogOption.NoStacktrace);
		}

		[Conditional("EDITOR")]
		public static void TimeLog(Action method)
		{
			Stopwatch sw = Stopwatch.StartNew();
			method.Invoke();
			sw.Stop();
			Log($"Invoked method: {method.Method.Name} took {sw.Elapsed.TotalMilliseconds:F2}ms", LogFormat.Complete, DefaultOption);
		}

		public void Update()
		{
#if EDITOR
			if (!DisplayLogs)
				return;

			updating = true;
			if (InputState.Pressed(InputModule.MouseButton.Left))
			{
				for (int i = 0; i < logMessages.Count;i ++)
				{
					LogMessage log = logMessages[i];
					if(log.Rect.Contains(InputState.MousePosition))
					{
						log.Expanded = !log.Expanded;
						break;
					}
				}
			}

			if (InputState.Held(InputModule.Keys.Delete))
			{
				if (delay > Time.ElapsedTime)
					return;
				int indexToRemvoe = -1;
				for (int i = 0; i < logMessages.Count; i++)
				{
					LogMessage log = logMessages[i];
					if(log.Rect.Contains(InputState.MousePosition))
					{
						if(log == current)
						{
							log.Expanded = false;
							current = null;
						}
						indexToRemvoe = i;
						break;
					}
				}
				if(indexToRemvoe >= 0)
				{
					logMessages.RemoveAt(indexToRemvoe);
				}
				delay = Time.ElapsedTime + 0.1f;
			}


			updating = false;
			foreach (LogMessage log in itemsToAdd)
				AddLog(log);
			itemsToAdd.Clear();
#endif
		}

		public void RenderWorld() { }

		public void RenderUI()
		{
#if EDITOR
			if (!DisplayLogs)
				return;

			updating = true;
			Vector2 position = displayPosition;
			int fontSize;
			foreach (LogMessage msg in LogMessages)
			{
				if (position.Y > Screen.Height - 16) 
					break;

				string[] log = msg.SplitLines();
				for (int i = 0; i < log.Length; i++)
				{
					string[] message = log[i].Split("<colour>");
					string text = message[0];
					if (i == 0)
					{
						position.X = DisplayPosition.X;
						Draw.Sprite(msg.Icon, position, 0.0f, new Vector2(0.4f, 0.4f), null, Vector2.Zero, Colour.White, short.MaxValue);
						fontSize = 11;
						message[0] += $" {(msg.Count > 1 ? $"({msg.Count})" : "")}";
						Rect rect = msg.Rect;
						rect.Set(position.X, position.Y, rect.Size.X, rect.Size.Y);
						msg.Rect = rect;
						position.X = DisplayPosition.X + 25;
					}
					else
					{
						if (!msg.Expanded)
							break;
						fontSize = 10;
						position.X = DisplayPosition.X + 40;
					}

					Draw.Text(message[0], Font.Inter, fontSize, position, Colour.White, short.MaxValue);
					if(message.Length > 1)
					{
						Draw.Text(message[1], Font.Inter, fontSize, position + Vector2.Right * Font.Inter.MeasureString(message[0]).X, new Colour(135,221,135), short.MaxValue);
					}
					position += Vector2.Down * Font.Inter.FontHeight(fontSize + 1);
				}
			}
			updating = false;
#endif
		}
	}
}