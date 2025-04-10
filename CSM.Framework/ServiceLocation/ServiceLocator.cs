namespace CSM.Framework.ServiceLocation
{
    public sealed class ServiceLocator : IServiceLocator
    {
        private Func<Type, object?>? defaultServiceFactory;

        public T GetService<T>() where T : class
        {
            if (defaultServiceFactory == null)
            {
                return default!;
            }
            var type = typeof(T);
            if (defaultServiceFactory(type) is T service)
            {
                return service;
            }
            throw new Exception($"Resolution of type {type} failed");
        }

        public object GetService(Type type)
        {
            return defaultServiceFactory?.Invoke(type)!;
        }

        public void SetDefaultServiceFactory(Func<Type, object?> serviceFactory)
        {
            defaultServiceFactory = serviceFactory;
        }
    }
}
