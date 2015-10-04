using System;

namespace ExploreFLicker.DataServices.QueryParameters
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class QueryIgnoreAttribute : Attribute
    {
    }
}
