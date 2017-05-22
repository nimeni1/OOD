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
    public class Sink : Element
    {
        public Sink(double cap, double inp)
        {
            Capacity = cap;
            InputFlow = inp;
            Type = "sink";
            Elements_connected = new List<Item>() { null };
        }

        public double InputFlow
        {
            get;
            set;
        }

        public double Capacity
        {
            get;
            set;
        }

        public override void DrawYourself(Graphics gr)
        {
            Image img = new Bitmap(Properties.Resources.sink);
            gr.DrawImage(img, new Rectangle(X - 20, Y - 20, 40, 40));
        }

        public override void SetFlow(double flow)
        {
            InputFlow = flow;
        }
    }
}