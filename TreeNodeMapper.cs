using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Zeroconf;

namespace bonjour_broswer
{
    internal abstract class TreeNodeMapper
    {
        private static IDictionary<string, Type> nodeMapper = getDerivedMappers();

        public static IDictionary<string, Type> getDerivedMappers()
        {
            var baseType = typeof(TreeNodeMapper);
            return Assembly.GetAssembly(baseType).GetTypes().Where(t =>
                    t != baseType && baseType.IsAssignableFrom(t) && t.GetProperty("Name") != null
                    ).ToDictionary(t => t.GetProperty("Name").GetValue(null).ToString());
        }

        public static TreeNodeMapper create(string mapperName)
        {
            return Activator.CreateInstance(nodeMapper[mapperName]) as TreeNodeMapper;
        }

        public static IList<string> getMapperNameList()
        {
            return nodeMapper.Keys.ToList();
        }

        public abstract IList<TreeNode> map(IReadOnlyList<IZeroconfHost> results);

        protected static TreeNode createNode(string name, string value)
        {
            return new TreeNode(string.Format("{0}:{1}", name, value));
        }
    }
}