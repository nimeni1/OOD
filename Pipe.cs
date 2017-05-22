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
    public class Pipe : Item
    {
        public Pipe(double cap, double flow, List<Point> points, double safety)
        {
            TurningPoints = points;
            Capacity = cap;
            Flow = flow;
            SafetyLimit = safety;
            Type = "pipe";
            Elements_connected = new List<Item>() { null, null };
        }

        public List<Point> TurningPoints
        {
            get;
            set;
        }

        public double Flow
        {
            get;
            set;
        }

        public double Capacity
        {
            get;
            set;
        }

        public double SafetyLimit { get; set; }

        public override void DrawYourself(Graphics gr)
        {
            Color cl = Color.Chartreuse;
            if (Capacity <= Flow)
                cl = Color.Red;
            if (SafetyLimit <= Flow && Flow < Capacity)
                cl = Color.Yellow;
            if (TurningPoints.Count > 1)
            {
                gr.DrawLines(new Pen(cl, 6), TurningPoints.ToArray());
            }
        }

        public void DrawYourself(Graphics gr, Color color)
        {
            if (TurningPoints.Count > 1)
                gr.DrawLines(new Pen(color, 6), TurningPoints.ToArray());
        }

        public override void SetFlow(double flow)
        {
            if (Elements_connected[1] != null && Elements_connected[1] is Merger)
            {
                if (Elements_connected[1].Elements_connected[0] == this)
                    Elements_connected[1].SetFlow(flow, 0);
                else if (Elements_connected[1].Elements_connected[1] == this)
                    Elements_connected[1].SetFlow(flow, 1);
            }
            else if (Elements_connected[1] != null)
                Elements_connected[1].SetFlow(flow);
            Flow = flow;
        }

        public void Detach()
        {
            //detach Elements_connected[0]
            if (Elements_connected[0] is Splitter || Elements_connected[0] is SplitterAdjustable)
            {
                if (Elements_connected[0].Elements_connected[1] == this)
                    Elements_connected[0].Elements_connected[1] = null;
                else if (Elements_connected[0].Elements_connected[2] == this)
                    Elements_connected[0].Elements_connected[2] = null;
            }
            else if (Elements_connected[0] is Pump)
                Elements_connected[0].Elements_connected[0] = null;
            else if (Elements_connected[0] is Merger)
                Elements_connected[0].Elements_connected[2] = null;

            //detach Elements_connected[1]
            if (Elements_connected[1] is Splitter || Elements_connected[1] is SplitterAdjustable ||
                Elements_connected[1] is Sink)
                Elements_connected[1].Elements_connected[0] = null;
            else if (Elements_connected[1] is Merger)
            {
                if (Elements_connected[1].Elements_connected[0] == this)
                    Elements_connected[1].Elements_connected[0] = null;
                else if (Elements_connected[1].Elements_connected[1] == this)
                    Elements_connected[1].Elements_connected[1] = null;
            }
        }
    }
}