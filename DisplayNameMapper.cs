using System.Collections.Generic;
using System.Windows.Forms;
using Zeroconf;

namespace bonjour_browser
{
    internal class DisplayNameMapper : TreeNodeMapper
    {
        public static string Name { get { return "Display Name"; } }

        public DisplayNameMapper()
        {
        }

        public override IList<TreeNode> map(IReadOnlyList<IZeroconfHost> results)
        {
            var treeNodes = new List<TreeNode>();
            foreach (var result in results)
            {
                treeNodes.Add(createRootNode(result));
            }
            return treeNodes;
        }

        private static TreeNode createRootNode(IZeroconfHost result)
        {
            var node = new TreeNode(string.Format("{0} - {1}", result.DisplayName, result.IPAddress),
                new TreeNode[]
                {
                    createNode("DisplayName", result.DisplayName),
                    createNode("IP", result.IPAddress),
                    createNode("Id", result.Id),
                    createServicesNode(result.Services)
                });

            return node;
        }

        private static TreeNode createServicesNode(IReadOnlyDictionary<string, IService> services)
        {
            var node = new TreeNode();
            foreach (KeyValuePair<string, IService> service in services)
            {
                node.Nodes.Add(createServiceNode(service.Key, service.Value));
            }
            node.Text = string.Format("Services - {0}", node.Nodes.Count);
            return node;
        }

        private static TreeNode createServiceNode(string name, IService service)
        {
            return new TreeNode(name, new TreeNode[]
            {
                createNode("ServiceName", service.Name),
                createNode("Port", service.Port.ToString()),
                createPropertiesNode("Properties", service.Properties)
            });
        }

        private static TreeNode createPropertiesNode(string name, IReadOnlyList<IReadOnlyDictionary<string, string>> properties)
        {
            var node = new TreeNode();
            foreach (IReadOnlyDictionary<string, string> propertiesDict in properties)
            {
                foreach (var property in propertiesDict)
                {
                    node.Nodes.Add(createNode(property.Key, property.Value));
                }
            }
            node.Text = string.Format("Properties - {0}", node.Nodes.Count);

            return node;
        }
    }
}