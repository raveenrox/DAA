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

            String Line = txtdata.Text;

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
                        for (int k = 0; k < Int32.Parse(timeSpaceTradeOffList[0]); k++)
                        {
                            tsto = new TimeSpaceTradeOff();
                            tsto.Space = Int32.Parse(timeSpaceTradeOffList[(k * 2) + 1]);
                            tsto.Time = Int32.Parse(timeSpaceTradeOffList[(k * 2) + 2]);
                            program.timeSpaceTradeOffs.Add(tsto);
                        }
                        testCase.Programs.Add(program);
                    }
                    testCases.Add(testCase);
                    i = i + 1 + testCase.noOfPrograms;
                }
                else
                {
                    break;
                }
            }

            outText = "";

            for(int i=0; i<testCases.Count; i++)
            {
                double avg_turnAround = 0.0f;

                List<TimeSpaceTradeOff> tmpTSTOList = new List<TimeSpaceTradeOff>();
                List<Int32> memoryRegionTimeUsage = new List<Int32>();

                for(int y=0; y<testCases[i].noOfMemoryRegions; y++)
                {
                    memoryRegionTimeUsage.Add(0);
                }
                
                for (int j = 0; j < testCases[i].noOfPrograms; j++)
                {
                    List<TimeSpaceTradeOff> _tmpTSTOList = new List<TimeSpaceTradeOff>();
                    for (int k = 0; k < testCases[i].Programs[j].timeSpaceTradeOffs.Count; k++)
                    {
                        TimeSpaceTradeOff _tmpTSTO = new TimeSpaceTradeOff();
                        _tmpTSTO.Space = testCases[i].Programs[j].timeSpaceTradeOffs[k].Space;
                        _tmpTSTO.Time = testCases[i].Programs[j].timeSpaceTradeOffs[k].Time;
                        _tmpTSTO.Prog = j;
                        _tmpTSTOList.Add(_tmpTSTO);
                    }
                    List<TimeSpaceTradeOff> _tmpSortedTSTOList = _tmpTSTOList.OrderBy(o => o.Time).ToList<TimeSpaceTradeOff>();
                    TimeSpaceTradeOff _tmpMinTSTO = _tmpSortedTSTOList[0];
                    tmpTSTOList.Add(_tmpMinTSTO);
                }
                List<TimeSpaceTradeOff> tmpSortedTSTOList = tmpTSTOList.OrderBy(o => o.Time).ToList<TimeSpaceTradeOff>();

                SolutionOut solN = new SolutionOut();
                solN.programList = new List<SolutionOut.Programs>();

                int offSet = 0;
                for (int z = 0; z < testCases[i].noOfPrograms; z++)
                {
                    SolutionOut.Programs prog = new SolutionOut.Programs();

                    int tmpMinTimeIndex = 0;

                    if ((offSet == 1) && (memoryRegionTimeUsage.Count>2))
                    {
                        tmpMinTimeIndex = memoryRegionTimeUsage.IndexOf(memoryRegionTimeUsage.OrderBy(num=>num).ElementAt(1));
                    } else
                    {
                        tmpMinTimeIndex = memoryRegionTimeUsage.IndexOf(memoryRegionTimeUsage.Min()) + offSet;
                    }

                    if(tmpMinTimeIndex>memoryRegionTimeUsage.Count-1)
                    {
                        tmpMinTimeIndex = memoryRegionTimeUsage.Count - 1;
                    }

                    if (tmpSortedTSTOList[0].Space>testCases[i].memoryRegions[tmpMinTimeIndex])
                    {
                        offSet++;
                        z--;
                        continue;
                    }

                    prog.progNo = tmpSortedTSTOList[0].Prog;
                    prog.region = tmpMinTimeIndex;
                    prog.startTime = memoryRegionTimeUsage[tmpMinTimeIndex];
                    prog.endTime = memoryRegionTimeUsage[tmpMinTimeIndex] + tmpTSTOList[prog.progNo].Time;
                    
                    solN.programList.Add(prog);

                    memoryRegionTimeUsage[tmpMinTimeIndex] += tmpTSTOList[tmpSortedTSTOList[0].Prog].Time;

                    tmpSortedTSTOList.RemoveAt(0);

                    offSet = 0;
                }

                Console.WriteLine("Case " + (i + 1));
                String outProgramText = "";

                List<SolutionOut.Programs> tmpSortedSolNProgs = solN.programList.OrderBy(o => o.progNo).ToList<SolutionOut.Programs>();
                foreach(SolutionOut.Programs prog in tmpSortedSolNProgs)
                {
                    outProgramText += "Program " + (prog.progNo+1) + " runs in region " + (prog.region+1) + " from " +
                        prog.startTime + " to " + prog.endTime + "\n";
                    avg_turnAround += prog.endTime;
                }

                Console.WriteLine("Average turnaround time = " + (avg_turnAround / testCases[i].noOfPrograms));
                Console.Write(outProgramText);
                Console.WriteLine();
            }
        }
    }
}
