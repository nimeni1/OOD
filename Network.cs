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
    public class Network
    {
        public List<Item> Items
        {
            get;
            set;
        }

        public Network()
        {
            Items = new List<Item>();
        }

        //now* this one is good
        public Item CheckPosition(int x, int y)
        {
            Rectangle a = new Rectangle(x, y, 40, 40);
            foreach (Item i in Items)
            {
                if (i is Element)
                {
                    Element e = ((Element)i);
                    Rectangle b = new Rectangle(e.X, e.Y, 40, 40);
                    if (a.IntersectsWith(b))
                    {
                        return i;
                    }
                }
            }
            return null;
        }

        //not sure about this one
        //Graphics gr;
        public void Draw(Item item, Graphics gr)
        {
            item.DrawYourself(gr);
        }

        public bool RemoveItem(Item item)
        {
            if (item is Pipe)
            {
                item.SetFlow(0);
                ((Pipe)item).Detach();
            }
            else
            {
                if (item is Pump)
                    item.SetFlow(0);
                else if (item is Merger)
                {
                    if (item.Elements_connected[0] != null)
                        item.Elements_connected[0].SetFlow(0);
                    if (item.Elements_connected[1] != null)
                        item.Elements_connected[1].SetFlow(0);
                }
                else
                {
                    //setflow from pipe to el_con[0]
                    if (item.Elements_connected[0] != null)
                        item.Elements_connected[0].SetFlow(0);
                    //detach all pipes
                }
                for (int x = 0; x < item.Elements_connected.Count; x++)
                {
                    Item i = item.Elements_connected[x];
                    //redundant?
                    if (i != null)
                    {
                        ((Pipe)i).Detach();
                        for (int j = 0; j < Items.Count; j++)
                        {
                            if (Items[j] == i)
                                Items.RemoveAt(j);
                        }
                    }
                }
            }
            //delete item from list
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == item)
                    Items.RemoveAt(i);
            }
            return true;

            /*//if the item is not a pipe, it is searched through all the items which are pipes and have the current item at output, in order to remove them
            //then all the pipes connected to the output of the current item are removed; finally the item is removed
            //if the item is a pipe, then it simply removes it
            //this happens only if the FindItem method returned sth

            Element e;
            Pipe p;
            Sink k;
            Pump pu;
            Splitter spl;
            SplitterAdjustable spla;
            Merger m;
            if (item != null)
            {
                if (!(item is Pipe))
                {
                    for (int i = 0; i <= Items.Count; i++) if (Items[i] is Pipe) for (int j = 0; j <= Items[i].Elements_connected.Count; j++)
                                if (Items[i].Elements_connected[j] == item) RemoveItem(Items[i]);
                    e = (Element)item;
                    for (int i = 0; i < e.Elements_connected.Count; i++) if (e.Elements_connected[i] is Pipe) RemoveItem(e.Elements_connected[i]);
                }

                Items.Remove(item);

                //sets everything to 0 except pump (if they were not deleted)
                foreach (Item i in Items)
                {
                    if (i is Pipe) { p = (Pipe)i; p.Flow = 0; }
                    if (i is Splitter) { spl = (Splitter)i; spl.InputFlow = 0; spl.OutputFlow1 = 0; spl.OutputFlow2 = 0; }
                    if (i is SplitterAdjustable) { spla = (SplitterAdjustable)i; spla.InputFlow = 0; spla.OutputFlow1 = 0; spla.OutputFlow2 = 0; }
                    if (i is Merger) { m = (Merger)i; m.InputFlow1 = 0; m.InputFlow2 = 0; }
                    if (i is Sink) { k = (Sink)i; k.InputFlow = 0; }
                }

                //calculates for the remaining graph, starting with pump (if existing); the flow is set to the previous value that the pump had as flow
                //foreach (Item i in Items) if (i is Pump) { pu = (Pump)i; pu.SetFlow(pu.OutputFlow, pu.OutputFlow); }
                return true;
            }
            return false;*/
        }

        //implementation suggested by the teacher
        public void DrawAllItems(Graphics gr)
        {
            foreach (Item i in Items)
            {
                i.DrawYourself(gr);
            }
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }
    }
}