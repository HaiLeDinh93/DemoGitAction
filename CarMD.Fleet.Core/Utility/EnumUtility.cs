using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using CarMD.Fleet.Core.Common;
using System.Linq;
using CarMD.Fleet.Core.Utility.Extensions;

namespace CarMD.Fleet.Core.Utility
{
    public static class EnumUtility
    {
        /// <summary>
        /// Get enum description
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(TEnum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = new DescriptionAttribute[] { };
            if (field != null)
                attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Get enum description
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            var attributes =
                (DescriptionAttribute[])
                enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute),
                                                                                       false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Get list of enum description/index
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<KeyValue> GetDescriptionList(Type enumType)
        {
            var list = new List<KeyValue>();
            foreach (string memberName in Enum.GetNames(enumType))
            {
                list.Add(new KeyValue(
                             GetDescriptionOfEnumItem(enumType, memberName),
                             (int)Enum.Parse(enumType, memberName)));
            }
            return list;
        }

        /// <summary>
        /// Get list of enum description/value
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<KeyValue> GetDescriptionValueList(Type enumType)
        {
            var list = new List<KeyValue>();
            foreach (int memberValue in Enum.GetValues(enumType))
            {
                list.Add(new KeyValue(
                             GetDescriptionOfEnumItem(enumType, memberValue),
                             memberValue));
            }
            return list;
        }
        /// <summary>
        /// Get list of enum description/name
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<KeyValue> GetDescriptionNameList(Type enumType)
        {
            var list = new List<KeyValue>();
            foreach (string memberName in Enum.GetNames(enumType))
            {
                list.Add(new KeyValue(
                             GetDescriptionOfEnumItem(enumType, memberName),
                             memberName));
            }
            return list;
        }

        /// <summary>
        /// Get enum by enum name
        /// </summary>
        /// <typeparam name="EnumType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumType ToEnum<EnumType>(string value)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), value);
        }

        /// <summary>
        /// Get description of an enum by enum name.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static string GetDescriptionOfEnumItem(Type enumType, string itemName)
        {
            return GetDescriptionOfEnumItem(enumType, itemName, string.Empty);
        }

        /// <summary>
        /// Get description of an enum by enum name.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static string GetDescriptionOfEnumItem(Type enumType, string itemName, string defaultDesc)
        {
            if (string.IsNullOrEmpty(itemName))
                return defaultDesc;

            MemberInfo[] memInfo = enumType.GetMember(itemName);

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return defaultDesc;
        }

        /// <summary>
        /// Get description of an enum by enum Index.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public static string GetDescriptionOfEnumItem(Type enumType, int itemIndex)
        {
            var itemName = Enum.GetName(enumType, itemIndex);
            return GetDescriptionOfEnumItem(enumType, itemName);
        }

        /// <summary>
        /// Generic method that can convert a string to any type of enum
        /// </summary>
        /// 
        /// <typeparupgam name="T">Enum type to convert to</typeparam>
        /// <param name="name">Enum item as string</param>
        /// <returns>The enum item that match with the string</returns>
        /// <example>MyEnumType d = CastToEnum&gt;MyEnumType&lt;("StrinValue");</example>
        public static T CastToEnum<T>(string name)
        {
            if (Enum.IsDefined(typeof(T), name))
            {
                return (T)Enum.Parse(typeof(T), name);
            }
            //No enum for string name
            throw new
                ArgumentOutOfRangeException(
                string.Format("The string {0} can't be bound with an item of the \"{1}\" enum",
                              name, typeof(T)));
        }

        /// <summary>
        /// Generic method that can convert a int to any type of enum
        /// </summary>
        /// <typeparam name="T">Enum type to convert to</typeparam>
        /// <param name="number">The number.</param>
        /// <returns>The enum item that match with the int</returns>
        /// <example>MyEnumType d = CastToEnum&gt;MyEnumType&lt;(2);</example>
        public static T CastToEnum<T>(int number)
        {
            if (Enum.IsDefined(typeof(T), number))
            {
                return (T)Enum.ToObject(typeof(T), number);
            }
            //No enum for number number
            throw new
                ArgumentOutOfRangeException(
                string.Format("The value {0} can't be bound with an item of the \"{1}\" enum",
                              number, typeof(T)));
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static List<T> GetCustomAttribute<T>(Type type)
        {
            List<T> attributes = null;
            foreach (
                FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                foreach (var attrib in field.GetCustomAttributes(true))
                {
                    if (attrib.GetType() == typeof(T))
                    {
                        if (attributes == null)
                            attributes = new List<T>();
                        attributes.Add((T)attrib);
                    }
                }
            }
            return attributes;
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(Type type, int value)
        {
            foreach (
                FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                if (field.Name.Equals(System.Enum.GetName(type, value)))
                {
                    foreach (var attrib in field.GetCustomAttributes(true))
                    {
                        if (attrib.GetType() == typeof(T))
                            return (T)attrib;
                    }
                }
            }
            return default(T);
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}