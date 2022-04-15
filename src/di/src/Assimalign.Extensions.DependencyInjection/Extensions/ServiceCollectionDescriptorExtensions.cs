using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection
{
	using Assimalign.Extensions.DependencyInjection;


	public static class ServiceCollectionDescriptorExtensions
    {
		public static IServiceCollection Add(this IServiceCollection collection, ServiceDescriptor descriptor)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (descriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			collection.Add(descriptor);
			return collection;
		}

		public static IServiceCollection Add(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (descriptors == null)
			{
				throw new ArgumentNullException("descriptors");
			}
			foreach (ServiceDescriptor descriptor in descriptors)
			{
				collection.Add(descriptor);
			}
			return collection;
		}

		public static void TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor)
		{
			ServiceDescriptor descriptor2 = descriptor;
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (descriptor2 == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			if (!collection.Any((ServiceDescriptor d) => d.ServiceType == descriptor2.ServiceType))
			{
				collection.Add(descriptor2);
			}
		}

		public static void TryAdd(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (descriptors == null)
			{
				throw new ArgumentNullException("descriptors");
			}
			foreach (ServiceDescriptor descriptor in descriptors)
			{
				TryAdd(collection, descriptor);
			}
		}

		public static void TryAddTransient(this IServiceCollection collection, Type service)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, service);
			TryAdd(collection, descriptor);
		}

		public static void TryAddTransient(this IServiceCollection collection, Type service, Type implementationType)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (implementationType == null)
			{
				throw new ArgumentNullException("implementationType");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, implementationType);
			TryAdd(collection, descriptor);
		}

		public static void TryAddTransient(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (implementationFactory == null)
			{
				throw new ArgumentNullException("implementationFactory");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, implementationFactory);
			TryAdd(collection, descriptor);
		}

		public static void TryAddTransient<TService>(this IServiceCollection collection) where TService : class
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			TryAddTransient(collection, typeof(TService), typeof(TService));
		}

		public static void TryAddTransient<TService, TImplementation>(this IServiceCollection collection) where TService : class where TImplementation : class, TService
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			TryAddTransient(collection, typeof(TService), typeof(TImplementation));
		}

		public static void TryAddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			TryAdd(services, ServiceDescriptor.Transient(implementationFactory));
		}

		public static void TryAddScoped(this IServiceCollection collection, Type service)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, service);
			TryAdd(collection, descriptor);
		}

		public static void TryAddScoped(this IServiceCollection collection, Type service, Type implementationType)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (implementationType == null)
			{
				throw new ArgumentNullException("implementationType");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, implementationType);
			TryAdd(collection, descriptor);
		}

		public static void TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (implementationFactory == null)
			{
				throw new ArgumentNullException("implementationFactory");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
			TryAdd(collection, descriptor);
		}

		public static void TryAddScoped<TService>(this IServiceCollection collection) where TService : class
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			TryAddScoped(collection, typeof(TService), typeof(TService));
		}

		public static void TryAddScoped<TService, TImplementation>(this IServiceCollection collection) where TService : class where TImplementation : class, TService
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			TryAddScoped(collection, typeof(TService), typeof(TImplementation));
		}

		public static void TryAddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			TryAdd(services, ServiceDescriptor.Scoped(implementationFactory));
		}

		public static void TryAddSingleton(this IServiceCollection collection, Type service)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, service);
			TryAdd(collection, descriptor);
		}

		public static void TryAddSingleton(this IServiceCollection collection, Type service, Type implementationType)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (implementationType == null)
			{
				throw new ArgumentNullException("implementationType");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, implementationType);
			TryAdd(collection, descriptor);
		}

		public static void TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (implementationFactory == null)
			{
				throw new ArgumentNullException("implementationFactory");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
			TryAdd(collection, descriptor);
		}

		public static void TryAddSingleton<TService>(this IServiceCollection collection) where TService : class
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			TryAddSingleton(collection, typeof(TService), typeof(TService));
		}

		public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection collection) where TService : class where TImplementation : class, TService
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			TryAddSingleton(collection, typeof(TService), typeof(TImplementation));
		}

		public static void TryAddSingleton<TService>(this IServiceCollection collection, TService instance) where TService : class
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
			TryAdd(collection, descriptor);
		}

		public static void TryAddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			TryAdd(services, ServiceDescriptor.Singleton(implementationFactory));
		}

		public static void TryAddEnumerable(this IServiceCollection services, ServiceDescriptor descriptor)
		{
			ServiceDescriptor descriptor2 = descriptor;
			if (services == null)
			{
				throw new ArgumentNullException("services");
			}
			if (descriptor2 == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			Type implementationType = descriptor2.GetImplementationType();
			if (implementationType == typeof(object) || implementationType == descriptor2.ServiceType)
			{
				throw new ArgumentException(System.SR.Format(System.SR.TryAddIndistinguishableTypeToEnumerable, implementationType, descriptor2.ServiceType), "descriptor");
			}
			if (!services.Any((ServiceDescriptor d) => d.ServiceType == descriptor2.ServiceType && d.GetImplementationType() == implementationType))
			{
				services.Add(descriptor2);
			}
		}

		public static void TryAddEnumerable(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptors)
		{
			if (services == null)
			{
				throw new ArgumentNullException("services");
			}
			if (descriptors == null)
			{
				throw new ArgumentNullException("descriptors");
			}
			foreach (ServiceDescriptor descriptor in descriptors)
			{
				TryAddEnumerable(services, descriptor);
			}
		}

		public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor)
		{
			ServiceDescriptor descriptor2 = descriptor;
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (descriptor2 == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			ServiceDescriptor serviceDescriptor = collection.FirstOrDefault((ServiceDescriptor s) => s.ServiceType == descriptor2.ServiceType);
			if (serviceDescriptor != null)
			{
				collection.Remove(serviceDescriptor);
			}
			collection.Add(descriptor2);
			return collection;
		}

		public static IServiceCollection RemoveAll<T>(this IServiceCollection collection)
		{
			return RemoveAll(collection, typeof(T));
		}

		public static IServiceCollection RemoveAll(this IServiceCollection collection, Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			for (int num = collection.Count - 1; num >= 0; num--)
			{
				ServiceDescriptor serviceDescriptor = collection[num];
				if (serviceDescriptor.ServiceType == serviceType)
				{
					collection.RemoveAt(num);
				}
			}
			return collection;
		}
	}
}
