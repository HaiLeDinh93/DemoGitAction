using System.Collections.Generic;
using System;

namespace CarMD.Fleet.Core.Common
{
    [Serializable]
    public class KeyValue
    {
        public KeyValue()
        {
        }

        public KeyValue(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { set; get; }
        public object Value { set; get; }
    }

    [Serializable]
    public class KeyValueComparer<T> : IComparer<KeyValue>
    {
        private bool SortByKey { get; set; }

        public KeyValueComparer()
        {
            SortByKey = false;
        }

        public KeyValueComparer(bool sortByKey)
        {
            SortByKey = sortByKey;
        }

        public int Compare(KeyValue x, KeyValue y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            if (!SortByKey)
                return Comparer<object>.Default.Compare(x.Value, y.Value);

            return Comparer<object>.Default.Compare(x.Key, y.Key);
        }
    }
}