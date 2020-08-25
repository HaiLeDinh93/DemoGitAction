namespace CarMD.Fleet.Core.Utility
{
    using System.Collections.Generic;
    using System.Text;

    public class CollectionUtility
    {
        #region Consts

        public const string DEFAULT_SEPARATOR = ",";
        public const string DEFAULT_XML_ROOT_TAG = "List";
        public const string DEFAULT_XML_ITEM_TAG = "Item";

        #endregion

        /// <summary>
        /// Concatenates a list of string to a string.
        /// </summary>
        public static string ToString<T>(List<T> list)
        {
            return ToString(list, DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Concatenates a list of string to a string.
        /// </summary>
        public static string ToString<T>(List<T> list, string separator)
        {
            if (list == null)
                return string.Empty;

            List<string> result = new List<string>();
            list.ForEach(item => result.Add(item.ToString()));

            return string.Join(separator, result.ToArray());
        }

        /// <summary>
        /// Convert list of string to xml.
        /// </summary>
        public static string ToXML<T>(List<T> list)
        {
            return ToXML(list, DEFAULT_XML_ROOT_TAG, DEFAULT_XML_ITEM_TAG);
        }

        /// <summary>
        /// Convert list of string to xml.
        /// </summary>
        public static string ToXML<T>(List<T> list, string rootTag, string itemTag)
        {
            var xml = new StringBuilder();
            xml.AppendFormat("<{0}>", rootTag);

            if (list != null)
                list.ForEach(item => xml.AppendFormat("<{0} value=\"{1}\"/>", itemTag, item.ToString()));

            xml.AppendFormat("</{0}>", rootTag);

            return xml.ToString();
        }
    }
}