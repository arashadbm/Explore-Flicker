using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ExploreFLicker.DataServices.QueryParameters
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class QueryArrayAttribute : Attribute
    {
        public QueryArrayAttribute ( [CallerLineNumber]int order = 0 )
        {
            _order = order;
        }

        public QueryArrayAttribute ( String name, [CallerLineNumber]int order = 0 )
        {
            Name = name;
            _order = order;
        }

        private readonly int _order;

        /// <summary>
        /// Gets or sets the name of the property. If this is null, Property Name will be used.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Format used will calling ToString() on related value.
        /// </summary>
        public String Format { get; set; }

        /// <summary>
        /// Used with formating,
        /// Default is InvariantCulture
        /// </summary>
        public CultureInfo Culture { get; set; }

        ///// <summary>
        ///// A value of true will include the key= even if the value is null or empty
        ///// </summary>
        //public bool AllowEmpty { get; set; }

        /// <summary>
        /// This how the enumerable will be represented in the output query parameters.
        /// Default value is DuplicateKeyWithBrackets,ex: "cars[]=Saab&cars[]=Audi"
        /// </summary>
        public ArrayFormat ArrayFormat { set; get; }

        /// <summary>
        /// Actually YOU shouldn't use this property
        /// It's there for some comptability issue with already existing backend
        /// For example Filter[CategoryIds] as key, we need [] to not be escaped
        /// </summary>
        public bool DontEscapeKey { get; set; }

        public int Order
        {
            get { return _order; }
        }
    }

    /// <summary>
    /// Check this link http://leekelleher.com/2008/06/06/how-to-convert-namevaluecollection-to-a-query-string/
    ///
    /// </summary>

    public enum ArrayFormat
    {
        /// <summary>
        /// Example output: "cars[]=Saab&cars[]=Audi"
        /// </summary>
        DuplicateKeyWithBrackets,
        /// <summary>
        /// Example output "cars=Saab,Audi"
        /// </summary>
        SingleKey

    }
}
