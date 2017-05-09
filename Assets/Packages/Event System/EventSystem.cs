using UniRx;

namespace Packages.EventSystem
{
    public interface IEventBase { }

    public class GlobalEventSystem
    {
        public static Subject<IEventBase> Events = new Subject<IEventBase>();

        public static void Reset()
        {
            if (Events != null)
            {
                Events.Dispose();
            }

            Events = new Subject<IEventBase>();
        }

        public static void RaiseEvent(IEventBase raisedEvent)
        {
            Events.OnNext(raisedEvent);
        }
    }

    public class EventSystem
    {
        public Subject<IEventBase> Events = new Subject<IEventBase>();

        public void Reset()
        {
            if (Events != null)
            {
                Events.Dispose();
            }

            Events = new Subject<IEventBase>();
        }

        public void RaiseEvent(IEventBase raisedEvent)
        {
            Events.OnNext(raisedEvent);
        }
    }
}
