using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ExploreFLicker.DataServices.QueryParameters
{

    //Developed by Ahmed Rashad
    //few snippets in this class is from http://ole.michelsen.dk/blog/serialize-object-into-a-query-string-with-reflection.html
    public static class QueryParameterExtensions
    {
        public static String AppendQueryString ( this string url, Object parameters )
        {
            if(String.IsNullOrEmpty(url)) throw new ArgumentNullException("url", "url can't be empty");
            string query = parameters.ToQueryString();
            if(String.IsNullOrEmpty(query)) return url;
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if(url[url.Length - 1] != '?') builder.Append('?');
            builder.Append(query);
            return builder.ToString();
        }

        public static String ToQueryString ( this Object input )
        {
            StringBuilder builder = new StringBuilder();
            if(input == null) throw new ArgumentNullException("input");
            var type = input.GetType();
            var properties = type.GetRuntimeProperties();
            foreach(var property in properties)
            {
                if(!property.CanRead || !property.GetMethod.IsPublic) continue;
                if(property.IsDefined(typeof(QueryIgnoreAttribute))) continue;
                var value = property.GetValue(input, null);
                bool addPrimitive = value == null || value is string || !(value is IEnumerable);
                if(addPrimitive)
                    AppendPrimitive(property, value, builder);
                else
                    AppendEnumerable(property, value, builder);
            }
            return builder.ToString();
        }

        private static void AppendPrimitive ( PropertyInfo property, object obj, StringBuilder builder )
        {
            String name = property.Name;
            String format = null;
            CultureInfo culture = null;
            bool allowEmpty = false;
            var attribute = property.GetCustomAttribute<QueryParameterAttribute>();
            if(attribute != null)
            {
                name = String.IsNullOrWhiteSpace(attribute.Name) ? name : attribute.Name;
                format = attribute.Format;
                allowEmpty = attribute.AllowEmpty;
                culture = attribute.Culture;
            }

            if(obj == null && !allowEmpty) return;//return and don't add key
            AppendKey(builder, name);
            builder.Append('=');
            if(obj == null) return;//return but after key was added
            AppendValue(property.PropertyType, obj, builder, format, culture);
        }

        private static void AppendEnumerable ( PropertyInfo property, object value, StringBuilder builder )
        {

            String name = property.Name;
            String format = null;
            CultureInfo culture = null;
            ArrayFormat arrayFormat = ArrayFormat.DuplicateKeyWithBrackets;
            var enumerable = value as IEnumerable;
            var attribute = property.GetCustomAttribute<QueryArrayAttribute>();
            bool escapeKey = true;
            if(attribute != null)
            {
                name = String.IsNullOrWhiteSpace(attribute.Name) ? name : attribute.Name;
                format = attribute.Format;
                arrayFormat = attribute.ArrayFormat;
                escapeKey = !attribute.DontEscapeKey;
                culture = attribute.Culture;
            }

            var elementType = GetElementType(property);
            if(!IsPrimitive(elementType)) return;//We Don't support array of custom objects
            if(arrayFormat == ArrayFormat.DuplicateKeyWithBrackets)
            {
                if (enumerable != null)
                    foreach(var item in enumerable)
                    {
                        AppendKey(builder, name, escapeKey);
                        builder.Append("[]=");
                        AppendValue(elementType, item, builder, format, culture);
                    }
            }
            else if(arrayFormat == ArrayFormat.SingleKey)
            {
                AppendKey(builder, name, escapeKey);
                builder.Append('=');
                if (enumerable != null)
                    foreach(var item in enumerable)
                    {
                        if(builder[builder.Length - 1] != '=')
                            builder.Append(',');
                        AppendValue(elementType, item, builder, format, culture);
                    }
            }
        }

        private static bool IsPrimitive ( Type elementType )
        {
            if(elementType.GetTypeInfo().IsPrimitive || elementType == typeof(string)) return true;
            return false;
        }

        private static Type GetElementType ( PropertyInfo property )
        {
            var typeInfo = property.PropertyType.GetTypeInfo();
            var elementType = typeInfo.IsGenericType ? typeInfo.GenericTypeArguments[0] : typeInfo.GetElementType();
            return elementType;
        }

        private static void AppendKey ( StringBuilder builder, string name, bool escapeKey = true )
        {
            if(builder.Length != 0) builder.Append('&');
            builder.Append(escapeKey ? Uri.EscapeUriString(name) : name);
        }

        private static void AppendValue ( Type type, object obj, StringBuilder builder, string format, CultureInfo culture )
        {
            if(obj == null) return;
            string val;
            if(!String.IsNullOrWhiteSpace(format))
            {
                if(culture == null) culture = CultureInfo.InvariantCulture;
                val = type == typeof(DateTime)
                    ? ((DateTime)obj).ToString(format, culture)
                    : String.Format(culture, format, obj);
            }
            else
            {
                if(type == typeof(DateTime))
                    val = ((DateTime)obj).ToString(culture);
                else
                    val = obj.ToString();
            }
            builder.Append(Uri.EscapeUriString(val));
        }

    }
}
