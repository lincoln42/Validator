using System;
using System.Collections.Generic;
using System.Linq;

namespace Validator.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the <see cref="Type"/> of the elements of a collection <see cref="Type"/>
        /// </summary>
        /// <param name="type">The collection <see cref="Type"/></param>
        /// <returns>The <see cref="Type"/> of the collection's elements</returns>
        public static Type GetCollectionTypeElementType(this Type type)
        {
            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (typeof(Array).IsAssignableFrom(type))
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            // type implements/extends IEnumerable<T>;
            var enumType = type.GetInterfaces()
                                    .Where(t => t.IsGenericType &&
                                           t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                    .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
            return enumType ?? type;
        }
    }
}
