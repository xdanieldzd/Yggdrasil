using Yggdrasil.Helpers;
using Yggdrasil.Configuration;

namespace Yggdrasil
{
	public class ApplicationConfig : JsonConfigHandler<ApplicationConfig>
	{
		[System.Web.Script.Serialization.ScriptIgnore]
		public override string DefaultFilename => "Settings.json";

		public string UpdateServer { get; set; } = $"http://magicstone.de/dzd/progupdates/{System.Windows.Forms.Application.ProductName}.txt";  // TODO: magicstone is gone; replace or remove
		public string LastDataPath { get; set; } = string.Empty;
		public string LastROMPath { get; set; } = string.Empty;
		public GameDataManager.Languages Language { get; set; } = GameDataManager.Languages.English;
		public Logger.Level LogLevel { get; set; } = Logger.Level.Info;
	}
}
