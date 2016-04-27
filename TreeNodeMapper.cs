using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Zeroconf;

namespace bonjour_broswer
{
    internal abstract class TreeNodeMapper
    {
        private const string ORDER_BY_NAME = "Display Name";
        private const string ORDER_BY_SERVICE = "Services Name";
        private static Dictionary<string, Type> nodeMapper = new Dictionary<string, Type>() {
                { "Display Name", typeof(DisplayNameMapper)}
            };

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