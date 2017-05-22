using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace OOD2_App
{
    public partial class Form1 : Form
    {
        //item to be drawn, path for Save()
        private string item, savePath;

        //starting and ending point of a pipe
        private List<Point> points;

        //selected item
        public Item i;

        //the network
        private Network n;

        //graphics for drawing, graphics for selecting items
        private Graphics drawingGr, selectGr;

        //pipe in progress
        private Pipe curPipe;

        //pipe is not done drawing until this field is false
        private bool isPipeFree;

        public Form1()
        {
            InitializeComponent();
            drawingGr = panel2.CreateGraphics();
            savePath = string.Empty;
            selectGr = panel1.CreateGraphics();
            KeyPreview = true;
            n = new Network();
            points = new List<Point>();
            item = "";
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.GreenYellow, 5), pictureBox1.Location.X - 3, pictureBox1.Location.Y - 3, 57, 54);
            item = "pump";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.GreenYellow, 5), pictureBox2.Location.X - 3, pictureBox2.Location.Y - 3, 57, 54);
            item = "sink";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.GreenYellow, 5), pictureBox3.Location.X - 3, pictureBox3.Location.Y - 3, 57, 54);
            item = "splitter";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.GreenYellow, 5), pictureBox4.Location.X - 3, pictureBox4.Location.Y - 3, 57, 54);
            item = "splitteradjustable";
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.GreenYellow, 5), pictureBox5.Location.X - 3, pictureBox5.Location.Y - 3, 57, 54);
            item = "merger";
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.GreenYellow, 5), pictureBox6.Location.X - 3, pictureBox6.Location.Y - 3, 57, 54);
            item = "pipe";
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.Red, 5), pictureBox7.Location.X - 3, pictureBox7.Location.Y - 3, 57, 54);
            item = "delete";
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            selectGr.DrawRectangle(new Pen(Color.Blue, 5), pictureBox8.Location.X - 3, pictureBox8.Location.Y - 3, 57, 54);
            item = "";
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (ModifierKeys == (Keys.Shift | Keys.Control))
            {
                if (e.KeyCode == Keys.S)
                    SaveAs();
            }
            if (ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        pictureBox1_Click(sender, e);
                        break;

                    case Keys.D2:
                        pictureBox2_Click(sender, e);
                        break;

                    case Keys.D3:
                        pictureBox3_Click(sender, e);
                        break;

                    case Keys.D4:
                        pictureBox4_Click(sender, e);
                        break;

                    case Keys.Q:
                        pictureBox5_Click(sender, e);
                        break;

                    case Keys.W:
                        pictureBox6_Click(sender, e);
                        break;

                    case Keys.E:
                        pictureBox7_Click(sender, e);
                        break;

                    case Keys.R:
                        pictureBox8_Click(sender, e);
                        break;

                    case Keys.S:
                        Save();
                        break;

                    case Keys.N:
                        n = new Network();
                        panel2.Refresh();
                        break;

                    case Keys.L:
                        LoadFromFile();
                        break;

                    case Keys.Z:
                        if (n.Items.Count > 0)
                        {
                            n.RemoveItem(n.Items[n.Items.Count - 1]);
                            panel2.Refresh();
                            n.DrawAllItems(drawingGr);
                        }
                        break;
                }
            }
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            Item elAtPosition = n.CheckPosition(e.X, e.Y);
            if (item == "delete")
            {
                if (elAtPosition == null)
                {
                    MessageBox.Show(
                        "If you are trying to delete a pipe you should do it from the properties of one of the attached items!");
                }
                else
                {
                    n.RemoveItem(elAtPosition);
                    panel2.Refresh();
                    n.DrawAllItems(drawingGr);
                    i = null;
                    DisplayPanels();
                }
                return;
            }
            if (elAtPosition == null)
            {
                //create new item
                switch (item)
                {
                    case "pump":
                        elAtPosition = new Pump(0, 0);
                        break;

                    case "sink":
                        elAtPosition = new Sink(0, 0);
                        break;

                    case "merger":
                        elAtPosition = new Merger(0, 0);
                        break;

                    case "splitter":
                        elAtPosition = new Splitter(0);

                        break;

                    case "splitteradjustable":
                        elAtPosition = new SplitterAdjustable(30, 0);
                        break;

                    case "pipe":
                        if (isPipeFree)
                        {
                            points.Add(new Point(e.X, e.Y));
                            DrawPipe();
                        }
                        return;

                    default:
                        return;
                }
                ((Element)elAtPosition).X = e.X;
                ((Element)elAtPosition).Y = e.Y;
                elAtPosition.DrawYourself(drawingGr);
                n.AddItem(elAtPosition);
                i = elAtPosition;
            }
            else
            {
                //select item or draw pipe
                i = elAtPosition;
                DisplayPanels();
                if (item == "pipe")
                {
                    //if pipe is still in progress CLOSE IT
                    if (isPipeFree)
                    {
                        switch (i.Type)
                        {
                            case "splitter":
                            case "splitteradjustable":
                            case "sink":
                                if (i.Elements_connected[0] == null)
                                {
                                    points.Add(new Point(((Element)i).X - 20, ((Element)i).Y));
                                    i.Elements_connected[0] = curPipe;
                                }
                                else
                                {
                                    MessageBox.Show("Input is already taken! Choose another element.");
                                    return;
                                }
                                break;

                            case "merger":
                                if (UpOrDown(e.Y))
                                {
                                    if (i.Elements_connected[0] == null)
                                    {
                                        points.Add(new Point(((Element)i).X - 20, ((Element)i).Y - 10));
                                        i.Elements_connected[0] = curPipe;
                                        ((Merger)i).SetFlow(curPipe.Flow, 0);
                                    }
                                    else if (i.Elements_connected[1] == null)
                                    {
                                        points.Add(new Point(((Element)i).X - 10, ((Element)i).Y + 5));
                                        i.Elements_connected[1] = curPipe;
                                        ((Merger)i).SetFlow(curPipe.Flow, 1);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Inputs is already taken! Choose another element.");
                                        return;
                                    }
                                }
                                else
                                {
                                    if (i.Elements_connected[1] == null)
                                    {
                                        points.Add(new Point(((Element)i).X - 10, ((Element)i).Y + 5));
                                        i.Elements_connected[1] = curPipe;
                                        ((Merger)i).SetFlow(curPipe.Flow, 1);
                                    }
                                    else if (i.Elements_connected[0] == null)
                                    {
                                        points.Add(new Point(((Element)i).X - 20, ((Element)i).Y - 10));
                                        i.Elements_connected[0] = curPipe;
                                        ((Merger)i).SetFlow(curPipe.Flow, 0);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Inputs is already taken! Choose another element.");
                                        return;
                                    }
                                }
                                break;

                            default:
                                MessageBox.Show("You can't attach the pipe there!");
                                return;
                                break;
                        }
                        //Close pipe
                        curPipe.Elements_connected[1] = i;
                        /*if (curPipe.Elements_connected[0] is Merger)
                            curPipe.Elements_connected[0].SetFlow(0);
                        else
                        {
                        }*/
                        if (!(curPipe.Elements_connected[1] is Merger))
                            curPipe.SetFlow(curPipe.Flow);
                        isPipeFree = false;
                        n.AddItem(curPipe);
                        DrawPipe();
                    }
                    //else start new pipe
                    else
                    {
                        double pipeInputFlow = 0;
                        points = new List<Point>();
                        Pipe tempPipe = new Pipe(0, 0, points, 0);
                        if (!isPipeFree)
                        {
                            switch (i.Type)
                            {
                                case "pump":
                                    if (i.Elements_connected[0] == null)
                                    {
                                        tempPipe.Flow = ((Pump)i).OutputFlow;
                                        ((Pump)i).Elements_connected[0] = tempPipe;
                                        points.Add(new Point(((Element)i).X + 20, ((Element)i).Y));
                                    }
                                    else
                                    {
                                        MessageBox.Show("Output is already taken!");
                                        return;
                                    }
                                    break;

                                case "splitter":
                                    //Select which one
                                    if (UpOrDown(e.Y))
                                    {
                                        if (i.Elements_connected[1] == null)
                                        {
                                            tempPipe.Flow = ((Splitter)i).OutputFlow1;
                                            i.Elements_connected[1] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 10, ((Element)i).Y - 10));
                                        }
                                        else if (i.Elements_connected[2] == null)
                                        {
                                            tempPipe.Flow = ((Splitter)i).OutputFlow2;
                                            i.Elements_connected[2] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 20, ((Element)i).Y + 10));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Outputs are already taken!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (i.Elements_connected[2] == null)
                                        {
                                            tempPipe.Flow = ((Splitter)i).OutputFlow2;
                                            i.Elements_connected[2] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 20, ((Element)i).Y + 10));
                                        }
                                        else if (i.Elements_connected[1] == null)
                                        {
                                            tempPipe.Flow = ((Splitter)i).OutputFlow1;
                                            i.Elements_connected[1] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 10, ((Element)i).Y - 10));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Outputs are already taken!");
                                            return;
                                        }
                                    }
                                    break;

                                case "splitteradjustable":
                                    //Select which one
                                    if (UpOrDown(e.Y))
                                    {
                                        if (i.Elements_connected[1] == null)
                                        {
                                            tempPipe.Flow = ((SplitterAdjustable)i).OutputFlow1;
                                            i.Elements_connected[1] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 10, ((Element)i).Y - 10));
                                        }
                                        else if (i.Elements_connected[2] == null)
                                        {
                                            tempPipe.Flow = ((SplitterAdjustable)i).OutputFlow2;
                                            i.Elements_connected[2] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 20, ((Element)i).Y + 10));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Outputs are already taken!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (i.Elements_connected[2] == null)
                                        {
                                            tempPipe.Flow = ((SplitterAdjustable)i).OutputFlow2;
                                            i.Elements_connected[2] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 20, ((Element)i).Y + 10));
                                        }
                                        else if (i.Elements_connected[1] == null)
                                        {
                                            tempPipe.Flow = ((SplitterAdjustable)i).OutputFlow1;
                                            i.Elements_connected[1] = tempPipe;
                                            points.Add(new Point(((Element)i).X + 10, ((Element)i).Y - 10));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Outputs are already taken!");
                                            return;
                                        }
                                    }
                                    break;

                                case "merger":
                                    tempPipe.Flow = ((Merger)i).InputFlow1 + ((Merger)i).InputFlow2;
                                    i.Elements_connected[2] = tempPipe;
                                    points.Add(new Point(((Element)i).X + 20, ((Element)i).Y));
                                    break;

                                default:
                                    return;
                            }
                            curPipe = tempPipe;
                            curPipe.Elements_connected[0] = i;
                            //points.Add(new Point(e.X, e.Y));
                            isPipeFree = true;
                        }
                    }
                }
            }
            DisplayPanels();
        }

        private void DrawPipe()
        {
            if (points.Count > 1)
            {
                drawingGr.DrawLines(new Pen(Color.Black, 5), points.ToArray());
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void loadFromFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadFromFile();
        }

        private void Save()
        {
            if (savePath != string.Empty)
            {
                IFormatter bf = null;
                FileStream fs = null;

                try
                {
                    bf = new BinaryFormatter();
                    fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);

                    bf.Serialize(fs, n);

                    MessageBox.Show("Save Successful!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
            else
                SaveAs();
        }

        private void SaveAs()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IFormatter bf = null;
                FileStream fs = null;
                savePath = saveFileDialog1.FileName;

                try
                {
                    bf = new BinaryFormatter();
                    fs = new FileStream(savePath + ".netw", FileMode.CreateNew, FileAccess.Write);

                    bf.Serialize(fs, n);

                    MessageBox.Show("Save Successful!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
        }

        private void LoadFromFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                n = null;
                panel2.Refresh();

                IFormatter bf = null;
                FileStream fs = null;
                savePath = openFileDialog1.FileName;

                try
                {
                    bf = new BinaryFormatter();
                    fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                    n = (Network)(bf.Deserialize(fs));
                    n.DrawAllItems(drawingGr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
        }

        private void DisplayPanels()
        {
            n.DrawAllItems(drawingGr);
            pPump.Visible = false;
            pPipe.Visible = false;
            pSplitterAdj.Visible = false;
            pSplitter.Visible = false;
            pSink.Visible = false;
            pMerger.Visible = false;

            if (i == null)
                return;
            switch (i.Type)
            {
                case "pump":
                    pPump.Visible = true;
                    tbPumpCap.Text = ((Pump)i).Capacity.ToString();
                    tbPumpFlow.Text = ((Pump)i).OutputFlow.ToString();
                    if (i.Elements_connected[0] != null)
                    {
                        btPumpPipe1.Tag = i.Elements_connected[0];
                        btPumpPipe1.Text = "1";
                        btPumpPipe1.Enabled = true;
                    }
                    else
                    {
                        btPumpPipe1.Tag = null;
                        btPumpPipe1.Text = "x";
                        btPumpPipe1.Enabled = false;
                    }
                    break;

                case "splitteradjustable":
                    pSplitterAdj.Visible = true;
                    tbSplitterRatio.Text = ((SplitterAdjustable)i).Percentage.ToString();
                    tbSplitterAdjIn1.Text = ((SplitterAdjustable)i).InputFlow.ToString();
                    tbSplitterAdjOut1.Text = ((SplitterAdjustable)i).OutputFlow1.ToString();
                    tbSplitterAdjOut2.Text = ((SplitterAdjustable)i).OutputFlow2.ToString();
                    if (i.Elements_connected[0] != null)
                    {
                        btSplitterAdjPipe1.Tag = i.Elements_connected[0];
                        btSplitterAdjPipe1.Text = "1";
                        btSplitterAdjPipe1.Enabled = true;
                    }
                    else
                    {
                        btSplitterAdjPipe1.Tag = null;
                        btSplitterAdjPipe1.Text = "x";
                        btSplitterAdjPipe1.Enabled = false;
                    }
                    if (i.Elements_connected[1] != null)
                    {
                        btSplitterAdjPipe2.Tag = i.Elements_connected[1];
                        btSplitterAdjPipe2.Text = "2";
                        btSplitterAdjPipe2.Enabled = true;
                    }
                    else
                    {
                        btSplitterAdjPipe2.Tag = null;
                        btSplitterAdjPipe2.Text = "x";
                        btSplitterAdjPipe2.Enabled = false;
                    }
                    if (i.Elements_connected[2] != null)
                    {
                        btSplitterAdjPipe3.Tag = i.Elements_connected[2];
                        btSplitterAdjPipe3.Text = "3";
                        btSplitterAdjPipe3.Enabled = true;
                    }
                    else
                    {
                        btSplitterAdjPipe3.Tag = null;
                        btSplitterAdjPipe3.Text = "x";
                        btSplitterAdjPipe3.Enabled = false;
                    }
                    break;

                case "splitter":
                    pSplitter.Visible = true;
                    tbSplitterIn.Text = ((Splitter)i).InputFlow.ToString();
                    tbSplitterOut1.Text = ((Splitter)i).OutputFlow1.ToString();
                    tbSplitterOut2.Text = ((Splitter)i).OutputFlow2.ToString();
                    if (i.Elements_connected[0] != null)
                    {
                        btSplitterPipe1.Tag = i.Elements_connected[0];
                        btSplitterPipe1.Text = "1";
                        btSplitterPipe1.Enabled = true;
                    }
                    else
                    {
                        btSplitterPipe1.Tag = null;
                        btSplitterPipe1.Text = "x";
                        btSplitterPipe1.Enabled = false;
                    }
                    if (i.Elements_connected[1] != null)
                    {
                        btSplitterPipe2.Tag = i.Elements_connected[1];
                        btSplitterPipe2.Text = "2";
                        btSplitterPipe2.Enabled = true;
                    }
                    else
                    {
                        btSplitterPipe2.Tag = null;
                        btSplitterPipe2.Text = "x";
                        btSplitterPipe2.Enabled = false;
                    }
                    if (i.Elements_connected[2] != null)
                    {
                        btSplitterPipe3.Tag = i.Elements_connected[2];
                        btSplitterPipe3.Text = "3";
                        btSplitterPipe3.Enabled = true;
                    }
                    else
                    {
                        btSplitterPipe3.Tag = null;
                        btSplitterPipe3.Text = "x";
                        btSplitterPipe3.Enabled = false;
                    }
                    break;

                case "sink":
                    pSink.Visible = true;
                    tbSinkIn.Text = ((Sink)i).InputFlow.ToString();
                    if (i.Elements_connected[0] != null)
                    {
                        btSinkPipe1.Tag = i.Elements_connected[0];
                        btSinkPipe1.Text = "1";
                        btSinkPipe1.Enabled = true;
                    }
                    else
                    {
                        btSinkPipe1.Tag = null;
                        btSinkPipe1.Text = "x";
                        btSinkPipe1.Enabled = false;
                    }
                    break;

                case "merger":
                    pMerger.Visible = true;
                    tbMergerIn1.Text = ((Merger)i).InputFlow1.ToString();
                    tbMergerIn2.Text = ((Merger)i).InputFlow2.ToString();
                    tbMergerOut.Text = (((Merger)i).InputFlow1 + ((Merger)i).InputFlow2).ToString();
                    if (i.Elements_connected[0] != null)
                    {
                        btMergerPipe1.Tag = i.Elements_connected[0];
                        btMergerPipe1.Text = "1";
                        btMergerPipe1.Enabled = true;
                    }
                    else
                    {
                        btMergerPipe1.Tag = null;
                        btMergerPipe1.Text = "x";
                        btMergerPipe1.Enabled = false;
                    }
                    if (i.Elements_connected[1] != null)
                    {
                        btMergerPipe2.Tag = i.Elements_connected[1];
                        btMergerPipe2.Text = "2";
                        btMergerPipe2.Enabled = true;
                    }
                    else
                    {
                        btMergerPipe2.Tag = null;
                        btMergerPipe2.Text = "x";
                        btMergerPipe2.Enabled = false;
                    }
                    if (i.Elements_connected[2] != null)
                    {
                        btMergerPipe3.Tag = i.Elements_connected[2];
                        btMergerPipe3.Text = "3";
                        btMergerPipe3.Enabled = true;
                    }
                    else
                    {
                        btMergerPipe3.Tag = null;
                        btMergerPipe3.Text = "x";
                        btMergerPipe3.Enabled = false;
                    }
                    break;

                case "pipe":
                    pPipe.Visible = true;
                    tbPipeSafety.Text = ((Pipe)i).SafetyLimit.ToString();
                    tbPipeCap.Text = ((Pipe)i).Capacity.ToString();
                    tbPipeFlow.Text = ((Pipe)i).Flow.ToString();
                    ((Pipe)i).DrawYourself(drawingGr, Color.Blue);
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Refresh();
            switch (i.Type)
            {
                case "pump":
                    ((Pump)i).Capacity = Convert.ToDouble(tbPumpCap.Text);
                    ((Pump)i).SetFlow(Convert.ToDouble(tbPumpFlow.Text));
                    break;

                case "splitteradjustable":
                    ((SplitterAdjustable)i).Percentage = Convert.ToDouble(tbSplitterRatio.Text);
                    ((SplitterAdjustable)i).SetFlow(((SplitterAdjustable)i).InputFlow);
                    break;

                case "pipe":
                    ((Pipe)i).SafetyLimit = Convert.ToDouble(tbPipeSafety.Text);
                    ((Pipe)i).Capacity = Convert.ToDouble(tbPipeCap.Text);
                    ((Pipe)i).SetFlow(Convert.ToDouble(tbPipeFlow.Text));
                    break;
            }
            n.DrawAllItems(drawingGr);
            DisplayPanels();
        }

        //checks which output to use, upper or lower one (returns true for upper and false for lower)
        private bool UpOrDown(int y)
        {
            if (((Element)i).Y > y)
                return true;
            return false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //n.DrawAllItems(drawingGr);
        }

        private void newNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            n = new Network();
            panel2.Refresh();
        }

        private void btMergerPipe1_Click(object sender, EventArgs e)
        {
            if (((Item)((Button)sender).Tag) != null)
            {
                i = ((Item)((Button)sender).Tag);
                DisplayPanels();
            }
        }

        private void btPipeDel_Click(object sender, EventArgs e)
        {
            n.RemoveItem(i);
            panel2.Refresh();
            n.DrawAllItems(drawingGr);
            i = null;
            DisplayPanels();
        }

        //drawing won't dissapear when focusing and unfocusing, also pressing Tab
        protected override void WndProc(ref Message m)
        {
            // Suppress the WM_UPDATEUISTATE message
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }
    }
}