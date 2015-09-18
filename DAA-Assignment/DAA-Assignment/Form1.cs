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
            String outText = "";
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
                        //Console.WriteLine(timeSpaceTradeOffList[0]);
                        for (int k = 0; k < Int32.Parse(timeSpaceTradeOffList[0]); k++)
                        {
                            tsto = new TimeSpaceTradeOff();
                            tsto.Space = Int32.Parse(timeSpaceTradeOffList[(k*2) + 1]);
                            tsto.Time = Int32.Parse(timeSpaceTradeOffList[(k*2) + 2]);
                            //Console.WriteLine("Space="+tsto.Space + ", Time=" + tsto.Time);
                            program.timeSpaceTradeOffs.Add(tsto);
                            
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
            //Console.WriteLine(testCases[0].Programs[0].timeSpaceTradeOffs[0].Space);

            //Printing
            outText = "";

            int avg_turnAround=0;

            for (int i = 0; i < testCases.Count; i++)
            {
                //Console.WriteLine("Case "+i);
                List<TimeSpaceTradeOff> tmpTSTOList = new List<TimeSpaceTradeOff>();
                List<Int32> memoryRegions = testCases[i].memoryRegions;

                outText += "Case "+(i+1)+"\n";
                      
                for (int j = 0; j < testCases[i].Programs.Count; j++)
                {
                    List<TimeSpaceTradeOff> _tmpTSTOList = new List<TimeSpaceTradeOff>();
                    for (int k = 0; k < testCases[i].Programs[j].timeSpaceTradeOffs.Count; k++)
                    {
                        TimeSpaceTradeOff _tmpTSTO = new TimeSpaceTradeOff();
                        _tmpTSTO.Space = testCases[i].Programs[j].timeSpaceTradeOffs[k].Space;
                        _tmpTSTO.Time = testCases[i].Programs[j].timeSpaceTradeOffs[k].Time;
                        _tmpTSTOList.Add(_tmpTSTO);
                    }
                    //Console.WriteLine("Program " + j + ", Size " + _tmpTSTOList.Count);
                    List<TimeSpaceTradeOff> _tmpSortedTSTOList = _tmpTSTOList.OrderBy(o => o.Time).ToList<TimeSpaceTradeOff>();
                    TimeSpaceTradeOff _tmpMinTSTO = _tmpSortedTSTOList[0];
                    tmpTSTOList.Add(_tmpMinTSTO);
                }
                List<TimeSpaceTradeOff> sortedList = tmpTSTOList.OrderBy(o => o.Time).ToList<TimeSpaceTradeOff>();

                outText += "Average turnaround time = " + "\n";
                //sortedList.RemoveAt(0);
                List<Int32> memoryRegionUsage = new List<Int32>();
                for (int z = 0; z < testCases[i].noOfMemoryRegions; z++)
                {
                    memoryRegionUsage.Add(0);
                }
                
                for (int z=0; z<testCases[i].noOfPrograms; z++)
                {
                    int tmpMinTime=0;
                    for(int y=0; y<memoryRegionUsage.Count-1; y++)
                    {
                        if(memoryRegionUsage[y]<memoryRegionUsage[y+1])
                        {
                            tmpMinTime = y;
                        } else
                        {
                            tmpMinTime = y + 1;
                        }
                        Console.WriteLine("Y : " + y);
                    }
                    Console.WriteLine("Min : " + tmpMinTime);
                    memoryRegionUsage[tmpMinTime]+= sortedList[0].Time;
                    
                    sortedList.RemoveAt(0);
                    Console.WriteLine("Z : "+z);
                }
                
                //TODO remove
                for(int z=0; z<memoryRegionUsage.Count; z++)
                {
                    Console.WriteLine("Cur Time "+z+" : "+memoryRegionUsage[z]);
                }
                //TODO remove ^^
                /*
                for (int z=0; z< sortedList.Count; z++)
                {
                    Console.WriteLine(sortedList[z].Time);
                    
                }*/
                Console.WriteLine("Count : " + sortedList.Count);
                //tmpTSTOList.Sort();
                //Console.WriteLine("Test Case " +i+", "+ tmpTSTOList.Count);
                outText += "\n";

                lblOut.Text = outText;
            }
        }
    }
}
