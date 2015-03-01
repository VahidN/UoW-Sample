namespace EF_Sample07.ServiceLayer.Contracts
{
    public interface ISessionProvider
    {
        object Get(string key);
        T Get<T>(string key) where T : class;
        void Remove(string key);
        void RemoveAll();
        void Store(string key, object value);
    }
}