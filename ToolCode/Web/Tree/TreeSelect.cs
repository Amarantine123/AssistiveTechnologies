using System.Collections.Generic;
using System.Text;

namespace NFine.Code
{
    /// <summary>
    /// Tree selector
    /// </summary>
    public static class TreeSelect
    {
        /// <summary>
        /// List<TreeSelectModel>Extension Method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string TreeSelectJson(this List<TreeSelectModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeSelectJson(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }
        /// <summary>
        /// Tree structure through recursion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parentId"></param>
        /// <param name="blank"></param>
        /// <returns></returns>
        private static string TreeSelectJson(List<TreeSelectModel> data, string parentId, string blank)
        {
            StringBuilder sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            var tabline = "";
            if (parentId != "0")
            {
                tabline = "　　";
            }
            if (ChildNodeList.Count > 0)
            {
                tabline = tabline + blank;
            }
            foreach (TreeSelectModel entity in ChildNodeList)
            {
                entity.text = tabline + entity.text;
                string strJson = entity.ToJson();
                sb.Append(strJson);
                sb.Append(TreeSelectJson(data, entity.id, tabline));
            }
            return sb.ToString().Replace("}{", "},{");
        }
    }
}