using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Loc : MonoBehaviour
{
	
	public TextAsset file;
	
	public class Translation
	{
		private Dictionary<string, string> translation = new Dictionary<string, string>();
		public Translation(TextAsset source, int language)
		{
			Match line = Regex.Match(source.text, ".+");
			/*string file = source.text.Remove(0, firstLine.Value.Length - 1);
			Match keym = Regex.Match (file, "\\w+;");*/
			line = line.NextMatch();
			while(line.Success){
				Match keym = Regex.Match (line.Value, "[^;]+;");
				keym = keym.NextMatch ();
				string key = keym.Value.Trim();
				if(key.Length > 1){
					key = key.Remove (key.Length - 1);
				}
				for(int i = 0; i < language; i++){
					keym = keym.NextMatch();
				}
				string transl = keym.Value.Trim ();
				if(transl.Length > 1){
					transl = transl.Remove (transl.Length - 1);
				}
				if(key != "" && !translation.ContainsKey(key)){
					translation.Add(key, transl);
					Debug.Log (key + " | " + transl);
				}
				line = line.NextMatch ();
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
		
		// Language detection
		currentLanguage = Application.systemLanguage;
		
		Match firstLine = Regex.Match(file.text, ".+");
		Match langm = Regex.Match(firstLine.Value, "\\w+;");
		// language number: 0 is Finnish, 1 is English etc
		int languageNumber = 0;
		while (langm.Success){
			string lang = langm.Value.Trim();
			if(lang.Length > 1){
				lang = lang.Remove (lang.Length - 1);
				SystemLanguage language = (SystemLanguage)System.Enum.Parse (typeof(SystemLanguage), lang);
				translations.Add (language, new Translation(file, languageNumber));
			}
			langm = langm.NextMatch();
			languageNumber++;
		}
		/*string firstLine = keym.Value.Trim();
		if(firstLine.Length > 1){
			key = firstLine.Remove (firstLine.Length - 1);
		}*/
		/*foreach (TextAsset transAsset in transAssets)
		{
			SystemLanguage lang = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), transAsset.name);
			translations.Add(lang, new Translation(transAsset, language));
			language++;
		}*/
	}
}
