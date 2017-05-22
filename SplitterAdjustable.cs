using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OOD2_App
{
    [Serializable]
    public class SplitterAdjustable : Splitter
    {
        public SplitterAdjustable(double percentage, double inp)
            : base(inp)
        {
            Percentage = percentage;
            InputFlow = inp;
            Type = "splitteradjustable";
            Elements_connected = new List<Item>() { null, null, null };
        }

        public double Percentage { get; set; }

        public override void DrawYourself(Graphics gr)
        {
            Image img = new Bitmap(Properties.Resources.adjSplitter);
            gr.DrawImage(img, new Rectangle(X - 20, Y - 20, 40, 40));
        }

        public override void SetFlow(double flow)
        {
            InputFlow = flow;
            OutputFlow1 = InputFlow * Percentage / 100;
            OutputFlow2 = InputFlow - OutputFlow1;

            if (Elements_connected[1] != null)
                Elements_connected[1].SetFlow(OutputFlow1);
            if (Elements_connected[2] != null)
                Elements_connected[2].SetFlow(OutputFlow2);
        }
    }
}