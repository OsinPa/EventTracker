using UnityEngine;

namespace Eidolon.SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        public string Get(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void Add(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}