using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Our.Umbraco.ezSearch
{
    public static class ExamineEventsHelper
    {
        public static string GetPropertyValue(umbraco.NodeFactory.Node node, string alias)
        {
            return getPropertyValue(node, alias);
        }

        private static string getPropertyValue(umbraco.NodeFactory.Node node, string alias)
        {
            var property = (umbraco.NodeFactory.Property)node.GetProperty(alias);
            return property == null ? string.Empty : property.Value;
        }

        //ExamineEventsHelper
        //{
        //    ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].RebuildIndex();
        //}
    }
}