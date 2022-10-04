namespace CosmosEngine.Editor
{
	public static class EditorContent
	{
		private static Sprite logMessage;
		private static Sprite logComplete;
		private static Sprite logWarning;
		private static Sprite logError;

		public static Sprite LogMessage => logMessage ??= new Sprite("Editor/log_alert");
		public static Sprite LogComplete => logComplete ??= new Sprite("Editor/log_complete");
		public static Sprite LogWarning => logWarning ??= new Sprite("Editor/log_warning");
		public static Sprite LogError => logError ??= new Sprite("Editor/log_error");
	}
}