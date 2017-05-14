using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tsp_sa
{
    public partial class GraphGUI : Form
    {
        public GraphGUI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point[] pts = { new Point(100, 100), new Point(200, 230), new Point(120, 110), new Point(340, 203), new Point(120, 222), new Point(123, 203) };
            for (int i = 0; i < pts.Length; i++)
            {

                if (i != 0)
                {

                    this.CreateGraphics().DrawLine(new Pen(Brushes.Black, 4), pts[i - 1], pts[i]);
                }

                else
                {
                    this.CreateGraphics().DrawLine(new Pen(Brushes.Black, 4), pts[i], pts[i]);

                }
            }
        }
    }
}
