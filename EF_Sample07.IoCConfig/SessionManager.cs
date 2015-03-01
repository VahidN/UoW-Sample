using System.Web;

namespace EF_Sample07.IoCConfig
{
    public interface ISessionProvider
    {
        object Get(string key);
        T Get<T>(string key) where T : class;
        void Remove(string key);
        void RemoveAll();
        void Store(string key, object value);
    }

    public class DefaultWebSessionProvider : ISessionProvider
    {
        private readonly HttpSessionStateBase _session;

        public DefaultWebSessionProvider(HttpSessionStateBase session)
        {
            _session = session;
        }

        public object Get(string key)
        {
            return _session[key];
        }

        public T Get<T>(string key) where T : class
        {
            return _session[key] as T;
        }

        public void Remove(string key)
        {
            _session.Remove(key);
        }

        public void RemoveAll()
        {
            _session.RemoveAll();
        }

        public void Store(string key, object value)
        {
            _session[key] = value;
        }
    }
}