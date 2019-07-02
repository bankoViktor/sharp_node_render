using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NodeInterface
{
    public partial class NodeForm : Form
    {
        public NodeForm()
        {
            InitializeComponent();           
        }

        private void nodeRender1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Random random = new Random(); // для рандимазации цветов нодов
                
                nodeRender1.Nodes.Add(new NodeInterface.Node()
                {
                    NodeID = nodeRender1.Nodes.Count,
                    Size = new Size(50, 80),
                    Location = new Point(e.X, e.Y),
                    BackColor = Color.FromArgb(
                        random.Next(0, 255), // R
                        random.Next(0, 255), // G
                        random.Next(0, 255)) // B
                });

                nodeRender1.Refresh();
            }

           
        }

        private void nodeRender1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"cn {nodeRender1.Nodes.Count.ToString()} " +
                   $"hi {nodeRender1.Highlight} " +
                   $"sl {nodeRender1.Selected.Count} " +
                   $"pn {nodeRender1.deb_paint} ";
                   
        }
    }
}
