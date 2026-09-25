using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Examine;
using Umbraco.Core;
using umbraco.NodeFactory;
using UmbracoExamine;
using System.Collections.Generic;

namespace Our.Umbraco.ezSearch
{
    public class ezSearchBoostrapper : IApplicationEventHandler
    {
        //ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].RebuildIndex();

        #region Application Event Handlers

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {
            //ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].RebuildIndex();
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {
            //ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"]
                  //.GatheringNodeData += OnGatheringNodeData;
            //ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].RebuildIndex();
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {
            ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].RebuildIndex();

            ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"]
                .GatheringNodeData += OnGatheringNodeData;
        }

        #endregion

        private void OnGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            // Create searchable path
            if (e.Fields.ContainsKey("path"))
            {
                e.Fields["searchPath"] = e.Fields["path"].Replace(',', ' ');
            }

            // Extract the filename from media items
            if (e.Fields.ContainsKey("umbracoFile"))
            {
                e.Fields["umbracoFileName"] = Path.GetFileName(e.Fields["umbracoFile"]);
            }

            //check if this is 'Content' (as opposed to media, etc...)
            if (e.IndexType == IndexTypes.Content)
            {
                Node theNode = new Node(e.NodeId);
                if (theNode.NodeTypeAlias == "umbPropertyDetails")
                {
                    var node = new Node(e.NodeId);

                    var propertyValue = ExamineEventsHelper.GetPropertyValue(theNode, "price");

                    if (propertyValue != string.Empty)
                    {
                        //e.Fields.Add("__" + "price", GetFieldValue(e, propertyValue, "price"));
                        e.Fields.Add("paddedPrice", GetFieldValue(e, propertyValue, "price"));
                    }
                    else //(propertyValue != string.Empty)
                    {
                        var propertyRentValue = ExamineEventsHelper.GetPropertyValue(theNode, "rent");

                        e.Fields.Add("paddedRent", GetFieldValue(e, propertyRentValue, "rent"));
                    }
                }

                AddToContentsField(e);

            }

            // Lowercase all the fields for case insensitive searching
            //var keys = e.Fields.Keys.ToList();
            //foreach (var key in keys)
            //{
            //    e.Fields[key] = HttpUtility.HtmlDecode(e.Fields[key].ToLower(CultureInfo.InvariantCulture));
            //}

            //// Extract the filename from media items
            //if (e.Fields.ContainsKey("umbracoFile"))
            //{
            //    e.Fields["umbracoFileName"] = Path.GetFileName(e.Fields["umbracoFile"]);
            //}

            // Stuff all the fields into a single field for easier searching
            //var combinedFields = new StringBuilder();
            //foreach (var keyValuePair in e.Fields)
            //{
            //    combinedFields.AppendLine(keyValuePair.Value);
            //}
            //AddToContentsField(e);
            //e.Fields.Add("contents", combinedFields.ToString());
        }

        private void AddToContentsField(IndexingNodeDataEventArgs e)
        {
            Dictionary<string, string> fields = e.Fields;

            var combinedFields = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in fields)
            {
                combinedFields.AppendLine(keyValuePair.Value);
            }
            e.Fields.Add("contents", combinedFields.ToString());

        }

        /// <summary>
        /// get ultimate picker field acutal value not the id of target 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="propertyValue"></param>
        /// <param name="luceneFieldAlias"></param>
        /// <returns></returns>
        private string GetFieldValue(IndexingNodeDataEventArgs e, string propertyValue, string luceneFieldAlias)
        {

            int nodeId = 0;

            int.TryParse(propertyValue, out nodeId);

            var n = new Node(nodeId);

            //node does not exist but we have numeric value
            if (n.Id != 0)
            {
                if (n.GetProperty(luceneFieldAlias) != null)
                {
                    //have to pad out to get lucene range queries to work
                    int i = 0;

                    int.TryParse(n.Name, out i);

                    return i.ToString("D6");
                }
                return n.Name;
            }

            return nodeId.ToString("D6");

        }
    }
}
