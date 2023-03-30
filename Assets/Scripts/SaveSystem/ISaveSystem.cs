namespace Eidolon.SaveSystem
{
    public interface ISaveSystem
    {
        public string Get(string key);
        public void Add(string key, string value);
        public void Save();
    }
}