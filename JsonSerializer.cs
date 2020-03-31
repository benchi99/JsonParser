using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JsonParser
{
    public static class JSONSerializer<T>
    {

        static PropertyInfo[] properties;

        /// <summary>
        /// Serializes a given object that matches the type specified.
        /// </summary>
        /// <param name="instance">Instance of the object</param>
        /// <returns>Serialized JSON object</returns>
        public static string Serialize(T instance)
        {
            string result = "{";

            properties = ObtainProperties();

            foreach (PropertyInfo property in properties)
            {
                result += "\"" + property.Name.ToLower() + "\": " + property.GetValue(instance);
                if (properties.GetValue(properties.Length - 1).Equals(property))
                {
                    break;
                }
                result += ",";
            }

            return result + "}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        public static string Serialize(params T[] instances)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes a given JSON into the corresponding object
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static T Deserialize(string json)
        {
            string[] strProperties = json.Split(',');

            if (ContainsObjectProperties(strProperties))
            {
                Console.WriteLine("las propiedades que hay coinciden con las del objeto, se puede construir.");
                Dictionary<string, string> properties = ParseStrProperties(strProperties);
            }
            else throw new Exception("The given JSON does not contain the matching properties");

            return default;
        }

        // public static T[] Deserialize(string json)
        // {
        //     throw new NotImplementedException();
        // }

        private static Dictionary<string, string> ParseStrProperties(string[] strProperties)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string strProperty in strProperties)
            {
                string[] cleanProperty = CleanUpProperty(strProperty);
                result.Add(cleanProperty[0], cleanProperty[1]);
            }

            return result;
        }

        private static bool ContainsObjectProperties(string[] strProperties)
        {
            properties = ObtainProperties();
            bool flag = false;

            foreach (string strProperty in strProperties)
            {
                string cleanProperty = CleanUpProperty(strProperty)[0];
                foreach (PropertyInfo property in properties)
                {
                    if (cleanProperty == property.Name.ToLower())
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }

            return flag;
        }

        private static string[] CleanUpProperty(string property)
        {
            char[] separators = {':', ' '};

            var regex = new Regex("[{}\",]");
            return regex.Replace(property, "").Split(separators);
        }

        private static PropertyInfo[] ObtainProperties()
        {
            return typeof(T).GetProperties();
        }
    }
}
