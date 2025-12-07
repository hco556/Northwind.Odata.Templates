using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace MudBlazorWebAppTest.Extensions
{
    public static class SimpleODataClientExtensions
    {
        /// <summary>
        /// Convert a single OData entry (IDictionary&lt;string, object&gt; or dynamic) to a POCO of type T.
        /// </summary>
        public static T ToObject<T>(object entry) where T : class, new()
        {
            if (entry == null) return default;

            if (entry is T tEntry) return tEntry;

            if (entry is IDictionary<string, object> dict)
            {
                return (T)MapDictionaryToType(dict, typeof(T));
            }

            // handle common dynamic shapes (ExpandoObject, IDictionary)
            if (entry is IDictionary genericDict)
            {
                var stringDict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (DictionaryEntry de in genericDict)
                {
                    if (de.Key == null) continue;
                    stringDict[de.Key.ToString()] = de.Value;
                }
                return (T)MapDictionaryToType(stringDict, typeof(T));
            }

            // If it's already a primitive or JsonElement etc., try direct conversion
            return (T)ConvertValue(entry, typeof(T));
        }

        /// <summary>
        /// Convert a sequence of OData entries to a list of POCOs.
        /// </summary>
        public static IList<T> ToObjectList<T>(IEnumerable<object> entries) where T : class, new()
        {
            var list = new List<T>();
            if (entries == null) return list;

            foreach (var e in entries)
            {
                var obj = ToObject<T>(e);
                if (obj != null) list.Add(obj);
            }

            return list;
        }

        // Internal mapping helpers

        private static object MapDictionaryToType(IDictionary<string, object> dict, Type targetType)
        {
            if (dict == null) return null;

            // If targetType is dictionary-like, attempt to return a dictionary
            if (typeof(IDictionary).IsAssignableFrom(targetType) || IsGenericDictionary(targetType))
            {
                // Try to create an instance of the target dictionary type and populate it
                var instance = Activator.CreateInstance(targetType);
                var addMethod = targetType.GetMethod("Add", new[] { typeof(object), typeof(object) }) ??
                                targetType.GetMethod("Add");
                if (instance is IDictionary id)
                {
                    foreach (var kv in dict)
                        id[kv.Key] = kv.Value;
                    return id;
                }
            }

            // If targetType is a primitive or string, attempt conversion of a single property named "value" or first value
            if (IsSimpleType(targetType))
            {
                // try to find a property named "value" or take first value
                if (dict.TryGetValue("value", out var v)) return ConvertValue(v, targetType);
                var first = dict.Values.FirstOrDefault();
                return ConvertValue(first, targetType);
            }

            var result = Activator.CreateInstance(targetType);

            var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanWrite)
                                  .ToArray();

            foreach (var prop in props)
            {
                // match by property name ignoring case
                if (!TryGetValueIgnoreCase(dict, prop.Name, out var rawValue)) continue;
                if (rawValue == null)
                {
                    prop.SetValue(result, null);
                    continue;
                }

                var propType = prop.PropertyType;

                // handle collections
                if (typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
                {
                    var elementType = GetCollectionElementType(propType) ?? typeof(object);
                    var listInstance = CreateListOfType(elementType);

                    if (rawValue is IEnumerable<object> rawEnumerable)
                    {
                        foreach (var item in rawEnumerable)
                        {
                            object mappedItem = MapValueToType(item, elementType);
                            listInstance.Add(mappedItem);
                        }
                    }
                    else if (rawValue is IEnumerable rawEnum)
                    {
                        foreach (var item in rawEnum)
                        {
                            object mappedItem = MapValueToType(item, elementType);
                            listInstance.Add(mappedItem);
                        }
                    }
                    else
                    {
                        // single item -> add converted single element
                        listInstance.Add(MapValueToType(rawValue, elementType));
                    }

                    // convert listInstance to the property type if needed
                    object finalCollection = ConvertListToPropertyType(listInstance, propType, elementType);
                    prop.SetValue(result, finalCollection);
                    continue;
                }

                // handle complex/nested objects
                if (rawValue is IDictionary<string, object> nestedDict)
                {
                    var nestedObj = MapDictionaryToType(nestedDict, propType);
                    prop.SetValue(result, nestedObj);
                    continue;
                }

                // handle generic IDictionary
                if (rawValue is IDictionary rawGenericDict)
                {
                    var nestedObj = MapDictionaryToType(ConvertToStringObjectDictionary(rawGenericDict), propType);
                    prop.SetValue(result, nestedObj);
                    continue;
                }

                // fallback: convert simple value
                var converted = ConvertValue(rawValue, propType);
                prop.SetValue(result, converted);
            }

            return result;
        }

        private static object MapValueToType(object value, Type targetType)
        {
            if (value == null) return null;

            if (targetType == typeof(object)) return value;

            if (value is IDictionary<string, object> dict)
            {
                return MapDictionaryToType(dict, targetType);
            }

            if (value is IDictionary genericDict)
            {
                return MapDictionaryToType(ConvertToStringObjectDictionary(genericDict), targetType);
            }

            // If target is simple type, convert directly
            if (IsSimpleType(targetType))
            {
                return ConvertValue(value, targetType);
            }

            // If target is a complex POCO and value is a primitive (e.g., navigation property represented by id),
            // try to convert primitive to target if possible, otherwise return null.
            if (IsSimpleType(value.GetType()) && !IsSimpleType(targetType))
            {
                try
                {
                    return ConvertValue(value, targetType);
                }
                catch
                {
                    return null;
                }
            }

            // If value is enumerable and target is complex, attempt to map first element
            if (value is IEnumerable<object> enumObj && !typeof(string).IsAssignableFrom(targetType))
            {
                var first = enumObj.FirstOrDefault();
                return MapValueToType(first, targetType);
            }

            return ConvertValue(value, targetType);
        }

        private static IDictionary<string, object> ConvertToStringObjectDictionary(IDictionary genericDict)
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (DictionaryEntry de in genericDict)
            {
                if (de.Key == null) continue;
                dict[de.Key.ToString()] = de.Value;
            }
            return dict;
        }

        private static bool TryGetValueIgnoreCase(IDictionary<string, object> dict, string key, out object value)
        {
            if (dict.TryGetValue(key, out value)) return true;

            // case-insensitive search
            var comparer = StringComparer.OrdinalIgnoreCase;
            foreach (var kv in dict)
            {
                if (comparer.Equals(kv.Key, key))
                {
                    value = kv.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

        private static object ConvertValue(object value, Type targetType)
        {
            if (value == null) return null;

            var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // If already assignable
            if (underlying.IsInstanceOfType(value)) return value;

            // Handle enums
            if (underlying.IsEnum)
            {
                if (value is string s)
                    return Enum.Parse(underlying, s, ignoreCase: true);

                try
                {
                    var numeric = Convert.ChangeType(value, Enum.GetUnderlyingType(underlying));
                    return Enum.ToObject(underlying, numeric);
                }
                catch
                {
                    return Enum.Parse(underlying, value.ToString(), ignoreCase: true);
                }
            }

            // Handle Guid
            if (underlying == typeof(Guid))
            {
                if (value is Guid g) return g;
                return Guid.Parse(value.ToString());
            }

            // Handle DateTimeOffset and DateTime
            if (underlying == typeof(DateTimeOffset))
            {
                if (value is DateTimeOffset dto) return dto;
                if (value is DateTime dt) return new DateTimeOffset(dt);
                return DateTimeOffset.Parse(value.ToString());
            }

            if (underlying == typeof(DateTime))
            {
                if (value is DateTime dt) return dt;
                return DateTime.Parse(value.ToString());
            }

            // Handle boolean-like strings
            if (underlying == typeof(bool))
            {
                if (value is string sv)
                {
                    if (bool.TryParse(sv, out var bv)) return bv;
                    if (int.TryParse(sv, out var iv)) return iv != 0;
                }
            }

            // If value is JsonElement (System.Text.Json), try to extract primitive
            var jsonElementType = value.GetType().FullName;
            if (jsonElementType == "System.Text.Json.JsonElement")
            {
                // use ToString fallback
                value = value.ToString();
            }

            // Final attempt using Convert.ChangeType
            try
            {
                return Convert.ChangeType(value, underlying);
            }
            catch
            {
                // last resort: try to use string constructor or parse
                var str = value.ToString();
                if (underlying == typeof(string)) return str;

                var parseMethod = underlying.GetMethod("Parse", new[] { typeof(string) });
                if (parseMethod != null)
                {
                    return parseMethod.Invoke(null, new object[] { str });
                }

                // give up and return default for target type
                return underlying.IsValueType ? Activator.CreateInstance(underlying) : null;
            }
        }

        private static bool IsSimpleType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type) ?? type;
            return t.IsPrimitive
                   || t.IsEnum
                   || t == typeof(string)
                   || t == typeof(decimal)
                   || t == typeof(DateTime)
                   || t == typeof(DateTimeOffset)
                   || t == typeof(Guid)
                   || t == typeof(TimeSpan)
                   || t == typeof(Uri);
        }

        private static bool IsGenericDictionary(Type type)
        {
            if (!type.IsGenericType) return false;
            var gen = type.GetGenericTypeDefinition();
            return gen == typeof(Dictionary<,>) || gen == typeof(IDictionary<,>);
        }

        private static Type GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsArray) return collectionType.GetElementType();

            var ifaces = collectionType.GetInterfaces().Concat(new[] { collectionType });
            foreach (var iface in ifaces)
            {
                if (!iface.IsGenericType) continue;
                var gen = iface.GetGenericTypeDefinition();
                if (gen == typeof(IEnumerable<>) || gen == typeof(ICollection<>) || gen == typeof(IList<>))
                {
                    return iface.GetGenericArguments()[0];
                }
            }

            return null;
        }

        private static IList CreateListOfType(Type elementType)
        {
            var listType = typeof(List<>).MakeGenericType(elementType);
            return (IList)Activator.CreateInstance(listType);
        }

        private static object ConvertListToPropertyType(IList listInstance, Type propertyType, Type elementType)
        {
            // If propertyType is assignable from listInstance, return directly
            if (propertyType.IsInstanceOfType(listInstance)) return listInstance;

            // If propertyType is array
            if (propertyType.IsArray)
            {
                var array = Array.CreateInstance(elementType, listInstance.Count);
                listInstance.CopyTo(array, 0);
                return array;
            }

            // If propertyType is IEnumerable<T> or IList<T>, try to create that generic type and copy
            var targetListType = typeof(List<>).MakeGenericType(elementType);
            if (propertyType.IsAssignableFrom(targetListType))
            {
                var target = Activator.CreateInstance(targetListType);
                var addMethod = targetListType.GetMethod("Add");
                foreach (var item in listInstance)
                    addMethod.Invoke(target, new[] { item });
                return target;
            }

            // Try to create instance of propertyType and populate if it implements ICollection<T> or IList
            try
            {
                var instance = Activator.CreateInstance(propertyType);
                if (instance is IList il)
                {
                    foreach (var item in listInstance) il.Add(item);
                    return il;
                }

                // try ICollection<T>
                var collInterface = propertyType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
                if (collInterface != null)
                {
                    var add = collInterface.GetMethod("Add");
                    foreach (var item in listInstance)
                        add.Invoke(instance, new[] { item });
                    return instance;
                }
            }
            catch
            {
                // ignore and fall through
            }

            // fallback: return the List<T> instance (may still be assignable to IEnumerable)
            return listInstance;
        }
    }
}
