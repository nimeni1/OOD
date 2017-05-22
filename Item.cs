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
    public class Item
    {
        public delegate void critical(Item item, string message);

        public string Type { get; set; }

        public List<Item> Elements_connected
        {
            get;
            set;
        }

        public virtual void SetFlow(double flow)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetFlow(double flow, int position)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DrawYourself(Graphics gr)
        {
            throw new System.NotImplementedException();
        }
    }
}