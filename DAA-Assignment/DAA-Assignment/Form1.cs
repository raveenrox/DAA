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
                    testCase.memoryRegions = new List<int>();
                    testCase.Programs = new List<ProgramS>();

                    testCase.noOfPrograms = Int32.Parse(items[1]);
                    testCase.noOfMemoryRegions = Int32.Parse(items[0]);

                    String[] memoryRegionList = LineList[i + 1].Split(' ');
                    for (int j = 0; j < testCase.noOfMemoryRegions; j++)
                    {
                        testCase.memoryRegions.Add(Int32.Parse(memoryRegionList[j]));
                    }
                    
                    for (int j = i + 2; j < i + 2 + testCase.noOfPrograms; j++)
                    {
                        ProgramS program = new ProgramS();

                        String[] timeSpaceTradeOffList = LineList[j].Split(' ');
                        program.timeSpaceTradeOffs = new List<TimeSpaceTradeOff>();
                        TimeSpaceTradeOff tsto;
                        for (int k = 0; k <= Int32.Parse(timeSpaceTradeOffList[0]); k++)
                        {
                            tsto = new TimeSpaceTradeOff();
                            tsto.Space = Int32.Parse(timeSpaceTradeOffList[k + 1]);
                            tsto.Time = Int32.Parse(timeSpaceTradeOffList[k + 2]);
                            Console.WriteLine("Space="+tsto.Space + ", Time=" + tsto.Time);
                            program.timeSpaceTradeOffs.Add(tsto);
                            k++;
                        }
                        testCase.Programs.Add(program);
                    }
                    testCases.Add(testCase);
                    i = i + 1 + testCase.noOfPrograms;
                } else
                {
                    break;
                }
            }
            Console.WriteLine(testCases[0].Programs[0].timeSpaceTradeOffs[0].Space);

            //Printing
            lblOut.Text = "";

            int avg_turnAround=0;

            for (int i = 0; i < testCases.Count; i++)
            {
                List<TimeSpaceTradeOff> tmpTSTOList = new List<TimeSpaceTradeOff>();
                
                lblOut.Text += "Case "+(i+1)+"\n";

                lblOut.Text += "Average turnaround time = "+"\n";

                for (int j = 0; j < testCases[i].Programs.Count; j++)
                {
                    for (int k = 0; k < testCases[i].Programs[j].timeSpaceTradeOffs.Count; k++)
                    {
                        TimeSpaceTradeOff tmpTSTO = new TimeSpaceTradeOff();
                        tmpTSTO.Space = testCases[i].Programs[j].timeSpaceTradeOffs[k].Space;
                        tmpTSTO.Time = testCases[i].Programs[j].timeSpaceTradeOffs[k].Time;
                        tmpTSTOList.Add(tmpTSTO);
                    }
                }
                List<TimeSpaceTradeOff> sortedList = tmpTSTOList.OrderBy(o => o.Time).ToList<TimeSpaceTradeOff>();
                sortedList.RemoveAt(0);
                for (int z=0; z< sortedList.Count; z++)
                {
                    Console.WriteLine(sortedList[z].Space);
                   
                }
                //tmpTSTOList.Sort();
                Console.WriteLine(tmpTSTOList.Count);
                lblOut.Text += "\n";
            }
        }
    }
}
