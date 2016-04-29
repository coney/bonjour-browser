using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Zeroconf;

namespace bonjour_broswer
{
    internal class ServiceNameMapper : TreeNodeMapper
    {
        public static string Name { get { return "Service Name"; } }

        public ServiceNameMapper()
        {
        }

        public override IList<TreeNode> map(IReadOnlyList<IZeroconfHost> results)
        {
            var treeNodes = new List<TreeNode>();
            IDictionary<string, IList<IZeroconfHost>> newResults = mapResultsByService(results);
            foreach (var serviceItem in newResults)
            {
                TreeNode serviceNode = createServiceNode(serviceItem.Key);
                treeNodes.Add(serviceNode);
                foreach (var result in serviceItem.Value)
                {
                    serviceNode.Nodes.Add(createRootNode(serviceItem.Key, result));
                }
            }
            return treeNodes;
        }

        private TreeNode createServiceNode(string key)
        {
            return new TreeNode(key);
        }

        private IDictionary<string, IList<IZeroconfHost>> mapResultsByService(IReadOnlyList<IZeroconfHost> results)
        {
            var newResults = new SortedDictionary<string, IList<IZeroconfHost>>();
            foreach (var result in results)
            {
                if (result.Services.Count == 0)
                {
                    continue;
                }
                foreach (var service in result.Services)
                {
                    Console.WriteLine("add " + service.Key + ":" + result.DisplayName);
                    if (!newResults.ContainsKey(service.Key))
                    {
                        newResults[service.Key] = new List<IZeroconfHost>();
                    }
                    newResults[service.Key].Add(result);
                }
            }
            return newResults;
        }

        private static TreeNode createRootNode(string serviceName, IZeroconfHost result)
        {
            var service = result.Services[serviceName];

            var node = new TreeNode(string.Format("{0} - {1}", result.DisplayName, result.IPAddress),
                new TreeNode[]
                {
                    createNode("DisplayName", result.DisplayName),
                    createNode("IP", result.IPAddress),
                    createNode("Id", result.Id),
                    createNode("Port", service.Port.ToString()),
                    createPropertiesNode("Properties", service.Properties)
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