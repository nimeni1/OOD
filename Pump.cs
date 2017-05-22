using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OOD2_App
{
    [Serializable]
    public class Pump : Element
    {
        public Pump(double cap, double flow)
        {
            Capacity = cap;
            OutputFlow = flow;
            Type = "pump";
            Elements_connected = new List<Item>() { null };
        }

        public double OutputFlow
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
            if (OutputFlow > Capacity)
                gr.DrawRectangle(new Pen(Color.Red, 2), new Rectangle(X - 22, Y - 22, 44, 44));
            Image img = new Bitmap(Properties.Resources.pump);
            gr.DrawImage(img, new Rectangle(X - 20, Y - 20, 40, 40));
        }

        public override void SetFlow(double flow)
        {
            OutputFlow = flow;
            if (Elements_connected[0] != null)
                Elements_connected[0].SetFlow(flow);
        }
    }
}