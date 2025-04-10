namespace CSM.Framework.ServiceLocation
{
    public interface IServiceLocator
    {
        T GetService<T>() where T : class;
        object GetService(Type type);
        void SetDefaultServiceFactory(Func<Type, object?> serviceFactory);
    }
}
