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
    public class Merger : Element
    {
        public Merger(double inp1, double inp2)
        {
            InputFlow1 = inp1;
            InputFlow2 = inp2;
            Type = "merger";
            Elements_connected = new List<Item>() { null, null, null };
        }

        public double InputFlow1
        {
            get;
            set;
        }

        public double InputFlow2
        {
            get;
            set;
        }

        public override void DrawYourself(Graphics gr)
        {
            Image img = new Bitmap(Properties.Resources.merger);
            gr.DrawImage(img, new Rectangle(X - 20, Y - 20, 40, 40));
        }

        public override void SetFlow(double flow, int position)
        {
            if (position == 0)
                InputFlow1 = flow;
            else
                InputFlow2 = flow;

            if (Elements_connected[2] != null)
                Elements_connected[2].SetFlow(InputFlow1 + InputFlow2);
        }
    }
}