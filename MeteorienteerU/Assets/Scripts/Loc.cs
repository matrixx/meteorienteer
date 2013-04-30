using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Loc : MonoBehaviour
{
	public class Translation
	{
		private Dictionary<string, string> translation = new Dictionary<string, string>();
		public Translation(TextAsset source)
		{
			Match keym = Regex.Match(source.text, ".+;\\s*(\\n|\\r\\n)");
			while (keym.Success)
			{
				string key = keym.Value.Trim();
				key = key.Remove(key.Length - 1);
				Match next = keym.NextMatch();
				string trstr = "";
				if (next.Success)
				{
					trstr = source.text.Substring(keym.Index + keym.Length, next.Index - (keym.Index + keym.Length)).Trim().Replace(";;",";");
				}
				else
				{
					trstr = source.text.Substring(keym.Index + keym.Length).Trim();
				}
				translation.Add(key, trstr);
				keym = next;
			}
		}
		public string GetString(string id)
		{
			string ret;
			if (translation.TryGetValue(id, out ret))
			{
				return ret;
			}
			else
			{
				return id;
			}
		}
	}

	private static Loc _singleton;
	public static Loc Singleton
	{
		get
		{
			return _singleton;
		}
	}

	private Dictionary<SystemLanguage, Translation> translations = new Dictionary<SystemLanguage, Translation>();
	private SystemLanguage currentLanguage = SystemLanguage.English;

	public static string Str(string id)
	{
		Translation trl;
		if (Singleton.translations.TryGetValue(Singleton.currentLanguage, out trl))
		{
			return trl.GetString(id);
		}
		else if (Singleton.translations.TryGetValue(SystemLanguage.English, out trl))
		{
			return trl.GetString(id);
		}
		else return id;
	}

	void Awake()
	{
		_singleton = this;
		Debug.Log("öm");

		// Language detection, currently commented out because no real translation yet.
		//currentLanguage = Application.systemLanguage;

		Object[] transAssets = Resources.LoadAll("Translations", typeof(TextAsset));
		foreach (TextAsset transAsset in transAssets)
		{
			SystemLanguage lang = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), transAsset.name);
			translations.Add(lang, new Translation(transAsset));
		}
	}
}