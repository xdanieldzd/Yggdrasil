using System;
using System.IO;
using System.Web.Script.Serialization;

namespace Yggdrasil.Configuration
{
	public abstract class JsonConfigHandler<T> where T : JsonConfigHandler<T>, new()
	{
		[ScriptIgnore]
		public virtual string DefaultFilename { get; }

		[ScriptIgnore]
		public static T Instance
		{
			get
			{
				if (instance == null) instance = Load();
				return instance;
			}
		}

		static T instance = null;

		static JsonConfigHandler() { }

		~JsonConfigHandler()
		{
			Instance.Save();
		}

		public void Save()
		{
			Save(this as T);
		}

		public static void Save(T settings)
		{
			var path = settings.GetFullPath();
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, (new JavaScriptSerializer()).Serialize(settings));
		}

		public static T Load()
		{
			T t = new T();
			var path = t.GetFullPath();

			if (File.Exists(path))
				t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(path));
			return t;
		}

		private string GetFullPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), System.Windows.Forms.Application.ProductName, DefaultFilename);
		}
	}
}
