using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ZedGraph;
using System.IO;

namespace CompressV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitGraph();
        }

        private void InitGraph()
        {
            GraphPane pane = zedGraph.GraphPane;
            pane.XAxis.Title.Text = "Samples";
            pane.YAxis.Title.Text = "Volts";
            pane.Title.Text = "EMG";
        }

        private void btDraw_Click(object sender, EventArgs e)
        {
            DrawGraph("delta.bin", "smartOffset.bin");
        }

        private void DrawGraph(string pathGraph, string pathAxis)
        {
            BinaryReader rdrGraph = new BinaryReader(File.Open(pathGraph, FileMode.Open), Encoding.ASCII);
            BinaryReader rdrAxis = new BinaryReader(File.Open(pathAxis, FileMode.Open), Encoding.ASCII);
            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();

            PointPairList points = new PointPairList();
            PointPairList axis = new PointPairList();
            PointPairList area = new PointPairList();
            byte areaOffset = (byte)(Math.Pow(2, 4) - 1);

            int length = rdrGraph.ReadInt32(); //reand length
            byte miss = rdrGraph.ReadByte();
            byte beginWith = rdrGraph.ReadByte();
            double min = rdrGraph.ReadDouble(); //read min
            double max = rdrGraph.ReadDouble(); //read max

            int x = 0;
            do
            {
                short v = rdrGraph.ReadInt16();
                points.Add(x, v);
                x++;
            } while (rdrGraph.PeekChar() > -1);

            x = 0;

            length = rdrAxis.ReadInt32();
            miss = rdrAxis.ReadByte();
            beginWith = rdrAxis.ReadByte();
            min = rdrAxis.ReadDouble();
            max = rdrAxis.ReadDouble();

            short offset = rdrAxis.ReadInt16();
            int totalOffset = offset;
            byte serviceByte = rdrAxis.ReadByte();
            byte last = serviceByte;
            byte isEven = 0;
            do
            {
                if (last == serviceByte)
                {
                    if (isEven == 1)
                    {
                        x--;
                        totalOffset -= offset;
                    }
                    axis.Add(x, totalOffset);
                    area.Add(x, totalOffset + areaOffset);
                    offset = rdrAxis.ReadInt16();
                    isEven = rdrAxis.ReadByte();
                    totalOffset += offset;
                    axis.Add(x, totalOffset);
                    area.Add(x, totalOffset + areaOffset);
                    totalOffset -= 3 * offset;
                    x -= 2;
                }
                last = rdrAxis.ReadByte();
                x += 2;
                totalOffset += 2 * offset;
            } while (rdrAxis.PeekChar() > -1);
            axis.Add(x, offset);
            area.Add(x, offset + areaOffset);

            LineItem lineGraph = pane.AddCurve("Values", points, Color.Blue, SymbolType.None);
            LineItem lineAxis = pane.AddCurve("Axis", axis, Color.Red, SymbolType.Circle);
            LineItem lineArea = pane.AddCurve("Area", area, Color.Yellow, SymbolType.None);

            zedGraph.AxisChange();
            zedGraph.Invalidate();

            rdrGraph.Close();
            rdrAxis.Close();
        }

        private void btCompress_Click(object sender, EventArgs e)
        {
            Compress();
        }

        private void CompressMultiple()
        {
            for (int i = 20; i < 40; i++)
            {
                string name = "differencesPercentage" + Convert.ToString(i) + ".txt";
                StreamWriter strmWrtr = new StreamWriter(name);
                for (int j = 0; j < i + 1; j++)
                {
                    Compression.TxtToBinDouble("txt1.txt", "txt.bin");
                    Compression.Sampling("txt.bin", "samples.bin", (byte)i, (byte)j);
                    Compression.DoubleToShort("samples.bin", "short.bin", 0, 1024);
                    Compression.DelShort("short.bin", "delta.bin");
                    Compression.DelSmartOffset("delta.bin", "smartOffset.bin");

                    Compression.DecDelSmartOffset("smartOffset.bin", "decSmartOffset.bin");
                    Compression.DecDelShort("decSmartOffset.bin", "decDelta.bin");
                    Compression.ShortToDouble("decDelta.bin", "decShort.bin");
                    Compression.Interpolation("decShort.bin", "interpolation.bin");

                    double dif = Compression.CalcPercentDiff("txt.bin", "interpolation.bin");
                    strmWrtr.Write(Convert.ToString(dif) + "\r\n");
                }
                strmWrtr.Close();
            }
        }

        private void Compress()
        {
            Compression.TxtToBinDouble("txt2.txt", "txt.bin");
            Compression.Sampling("txt.bin", "samples.bin", 4);
            Compression.DoubleToShort("samples.bin", "short.bin", 0, 1024);
            Compression.DelShort("short.bin", "delta.bin");
            Compression.DelSmartOffset("delta.bin", "smartOffset.bin");

            Compression.DecDelSmartOffset("smartOffset.bin", "decSmartOffset.bin");
            Compression.DecDelShort("decSmartOffset.bin", "decDelta.bin");
            Compression.ShortToDouble("decDelta.bin", "decShort.bin");
            Compression.Interpolation("decShort.bin", "interpolation.bin");

            double dif = Compression.CalcPercentDiff("txt.bin", "interpolation.bin");
            StreamWriter strmWrtr = new StreamWriter("differencesPercentage0.txt");
            strmWrtr.Write(Convert.ToString(dif) + "\r\n");
            strmWrtr.Close();
        }

        private void btScale_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            GraphPane pane = zedGraph.GraphPane;

            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void btDrawDifference_Click(object sender, EventArgs e)
        {
            Differnce("txt.bin", "interpolation.bin");
        }

        private void Differnce(string pathCurve1, string pathCurve2)
        {
            BinaryReader rdrOrigin = new BinaryReader(File.Open(pathCurve1, FileMode.Open), Encoding.ASCII);
            BinaryReader rdrRestored = new BinaryReader(File.Open(pathCurve2, FileMode.Open), Encoding.ASCII);

            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();

            PointPairList origin = new PointPairList();
            PointPairList restored = new PointPairList();

            int x = 0;
            do
            {
                double cur = rdrOrigin.ReadDouble();
                origin.Add(x, cur);
                cur = rdrRestored.ReadDouble();
                restored.Add(x, cur);
                x++;
            } while (rdrRestored.PeekChar() > -1);

            LineItem lineCurve1 = pane.AddCurve("Origin", origin, Color.Blue, SymbolType.None);
            LineItem lineCurve2 = pane.AddCurve("Restored", restored, Color.Red, SymbolType.None);

            zedGraph.AxisChange();
            zedGraph.Invalidate();

            rdrOrigin.Close();
            rdrRestored.Close();
        }
    }
}