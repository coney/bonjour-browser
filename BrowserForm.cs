using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zeroconf;

namespace bonjour_broswer
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();
        }

        private void updateServiceList(IReadOnlyList<IZeroconfHost> results)
        {
            if (InvokeRequired)
            {
                Invoke(new BonjourService.UpdateServiceListCallback(updateServiceList), results);
            }
            else
            {
                updateServiceTree(TreeNodeMapper.create(cbOrder.SelectedItem.ToString()).map(results));
                btnRefresh.Enabled = true;
            }
        }

        private void updateServiceTree(IList<TreeNode> treeNodes)
        {
            listServices.Nodes.Clear();
            foreach (var treeNode in treeNodes)
            {
                listServices.Nodes.Add(treeNode);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            var bonjourService = new BonjourService();
            bonjourService.ListAll(updateServiceList);
        }

        private void BrowserForm_Load(object sender, EventArgs e)
        {
            
            cbOrder.Items.AddRange(TreeNodeMapper.getMapperNameList().ToArray());
            cbOrder.SelectedItem = cbOrder.Items[0];
        }
    }
}
