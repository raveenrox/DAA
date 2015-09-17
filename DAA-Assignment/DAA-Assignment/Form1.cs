using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAA_Assignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {

            //Reading
            String Line = txtdata.Text;

            //Storing
            String[] LineList = Line.Split('\n');

            List<TestCase> testCases;
            testCases = new List<TestCase>();

            for (int i = 0; i < LineList.Length; i++)
            {
                if (!LineList[i].Equals("0 0"))
                {
                    String[] items = LineList[i].Split(' ');

                    TestCase testCase = new TestCase();
                    ProgramS program = new ProgramS();
                    testCase.memoryRegions = new List<int>();
                    testCase.Programs = new List<ProgramS>();

                    testCase.noOfPrograms = Int32.Parse(items[1]);
                    testCase.noOfMemoryRegions = Int32.Parse(items[0]);

                    //Console.WriteLine("Memory = "+ testCase.noOfMemoryRegions + ", Progs = " + testCase.noOfPrograms);
                    String[] memoryRegionList = LineList[i + 1].Split(' ');
                    //Console.WriteLine(testCase.noOfMemoryRegions + ", " + memoryRegionList.Length);
                    for (int j = 0; j < testCase.noOfMemoryRegions; j++)
                    {
                        //Console.WriteLine("i=" + i + ", j=" + j+", memoryRegionList="+ memoryRegionList[j]);
                        testCase.memoryRegions.Add(Int32.Parse(memoryRegionList[j]));
                    }
                    //Console.WriteLine("mR Count = "+testCase.memoryRegions.Count);
                    
                    for (int j = i + 2; j < i + 2 + testCase.noOfPrograms; j++)
                    {
                        //Console.WriteLine("i=" + i + ", j=" + j);
                        String[] timeSpaceTradeOffList = LineList[j].Split(' ');
                        //Console.WriteLine(LineList[j]);
                        program.timeSpaceTradeOffs = new List<TimeSpaceTradeOff>();
                        for (int k = 0; k <= Int32.Parse(timeSpaceTradeOffList[0]); k++)
                        {
                            TimeSpaceTradeOff tsto = new TimeSpaceTradeOff();
                            tsto.Space = Int32.Parse(timeSpaceTradeOffList[k + 1]);
                            tsto.Time = Int32.Parse(timeSpaceTradeOffList[k + 2]);
                            //Console.WriteLine("Space="+tsto.Space + ", Time=" + tsto.Time);
                            program.timeSpaceTradeOffs.Add(tsto);
                            k++;
                        }
                        //Console.WriteLine("i=" + i + ", j=" + j + ", tSTOCount=" + program.timeSpaceTradeOffs.Count);
                        testCase.Programs.Add(program);
                    }
                    testCases.Add(testCase);
                    i = i + 1 + testCase.noOfPrograms;
                } else
                {
                    break;
                }

                //Console.WriteLine(LineList.Length);
            }
            //Console.WriteLine(testCases[1].Programs.Count);

            //Printing
            lblOut.Text = "";
            for (int i = 0; i < testCases.Count; i++)
            {

                lblOut.Text += "Case "+(i+1)+"\n";

                lblOut.Text += "Average turnaround time = "+"\n";

                for (int j = 0; j < testCases[i].Programs.Count; j++)
                {
                    for (int k = 0; k < testCases[i].Programs[j].timeSpaceTradeOffs.Count; k++)
                    {
                        lblOut.Text += "Program " + (j + 1) + " Space "+ testCases[i].Programs[j].timeSpaceTradeOffs[k].Space+  "\n";
                    }
                }

                lblOut.Text += "\n";
            }
        }
    }
}
