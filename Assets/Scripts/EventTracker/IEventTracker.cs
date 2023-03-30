namespace Eidolon.EventTracker
{
    public interface IEventTracker
    {
        public void TrackEvent(string type, string data);
    }
}