using System;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace XLabs.Ioc.Unity
{
    public class UnityResolver : IResolver
    {
        private readonly IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            this.container = container;
        }

        #region IResolver Members

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T">Type of instance to get.</typeparam>
        /// <returns>An instance of {T} if successful, otherwise null.</returns>
        public T Resolve<T>() where T : class
        {
            try
            {
                return container.Resolve<T>();
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolve a dependency by type.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
        public object Resolve(Type type)
        {
            try
            {
                return container.Resolve(type);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T">Type of instance to get.</typeparam>
        /// <returns>All instances of {T} if successful, otherwise null.</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return container.ResolveAll<T>();
        }

        /// <summary>
        /// Resolve a dependency by type.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return container.ResolveAll(type);
        }
        #endregion
    }
}

