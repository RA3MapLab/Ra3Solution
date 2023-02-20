using System.Collections.Generic;

namespace MapCoreLib.Util
{
    public static class CollectionUtil
    {
        public static void AddIfAbsent<K, V>(this Dictionary<K, V> dictionary,K key, V value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
        
        public static void put<K, V>(this Dictionary<K, V> dictionary,K key, V value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }
        
        public static V getOrNull<K, V>(this Dictionary<K, V> dictionary,K key) where V: class
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return null;
        }
    }
}