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
    public class Splitter : Element
    {
        public Splitter(double inp)
        {
            InputFlow = inp;
            Type = "splitter";
            Elements_connected = new List<Item>() { null, null, null };
        }

        public double InputFlow
        {
            get;

            set;
        }

        public double OutputFlow2
        {
            get;
            set;
        }

        public double OutputFlow1
        {
            get;
            set;
        }

        public override void DrawYourself(Graphics gr)
        {
            Image img = new Bitmap(Properties.Resources.splitter);
            gr.DrawImage(img, new Rectangle(X - 20, Y - 20, 40, 40));
        }

        public override void SetFlow(double flow)
        {
            OutputFlow1 = flow / 2;
            OutputFlow2 = OutputFlow1;

            if (Elements_connected[1] != null)
                Elements_connected[1].SetFlow(OutputFlow1);
            if (Elements_connected[2] != null)
                Elements_connected[2].SetFlow(OutputFlow2);

            /*InputFlow = flow;
            foreach (Item i in Elements_connected) i.SetFlow(flow / 2, OutputFlow1);
            OutputFlow1 = InputFlow / 2;
            OutputFlow2 = InputFlow / 2;*/
        }
    }
}