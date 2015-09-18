using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAA_Assignment_Console
{
    class Application
    {
        static void Main(string[] args)
        {
            String Line = File.ReadAllText("text.txt");
            /*
            String Line = "2 4\n"
                + "40 60\n"
                + "1 35 4\n"
                + "1 20 3\n"
                + "1 40 10\n"
                + "1 60 7\n"
                + "3 5\n"
                + "10 20 30\n"
                + "2 10 50 12 30\n"
                + "2 10 100 20 25\n"
                + "1 25 19\n"
                + "1 19 41\n"
                + "2 10 18 30 42\n"
                + "0 0";
            */
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
                    testCase.Programs = new List<Program>();

                    testCase.noOfPrograms = Int32.Parse(items[1]);
                    testCase.noOfMemoryRegions = Int32.Parse(items[0]);

                    String[] memoryRegionList = LineList[i + 1].Split(' ');
                    for (int j = 0; j < testCase.noOfMemoryRegions; j++)
                    {
                        testCase.memoryRegions.Add(Int32.Parse(memoryRegionList[j]));
                    }

                    for (int j = i + 2; j < i + 2 + testCase.noOfPrograms; j++)
                    {
                        Program program = new Program();

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

            for (int i = 0; i < testCases.Count; i++)
            {
                double avg_turnAround = 0.0f;

                List<TimeSpaceTradeOff> tmpTSTOList = new List<TimeSpaceTradeOff>();
                List<Int32> memoryRegionTimeUsage = new List<Int32>();

                for (int y = 0; y < testCases[i].noOfMemoryRegions; y++)
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

                List<ProgramData> programList = new List<ProgramData>();

                int offSet = 0;
                for (int z = 0; z < testCases[i].noOfPrograms; z++)
                {
                    ProgramData prog = new ProgramData();

                    int tmpMinTimeIndex = 0;

                    if ((offSet == 1) && (memoryRegionTimeUsage.Count > 2))
                    {
                        tmpMinTimeIndex = memoryRegionTimeUsage.IndexOf(memoryRegionTimeUsage.OrderBy(num => num).ElementAt(1));
                    }
                    else
                    {
                        tmpMinTimeIndex = memoryRegionTimeUsage.IndexOf(memoryRegionTimeUsage.Min()) + offSet;
                    }

                    if (tmpMinTimeIndex > memoryRegionTimeUsage.Count - 1)
                    {
                        tmpMinTimeIndex = memoryRegionTimeUsage.Count - 1;
                    }

                    if (tmpSortedTSTOList[0].Space > testCases[i].memoryRegions[tmpMinTimeIndex])
                    {
                        offSet++;
                        z--;
                        continue;
                    }

                    prog.progNo = tmpSortedTSTOList[0].Prog;
                    prog.region = tmpMinTimeIndex;
                    prog.startTime = memoryRegionTimeUsage[tmpMinTimeIndex];
                    prog.endTime = memoryRegionTimeUsage[tmpMinTimeIndex] + tmpTSTOList[prog.progNo].Time;

                    programList.Add(prog);

                    memoryRegionTimeUsage[tmpMinTimeIndex] += tmpTSTOList[tmpSortedTSTOList[0].Prog].Time;

                    tmpSortedTSTOList.RemoveAt(0);

                    offSet = 0;
                }

                Console.WriteLine("Case " + (i + 1));
                String outProgramText = "";

                List<ProgramData> tmpSortedSolNProgs = programList.OrderBy(o => o.progNo).ToList<ProgramData>();
                foreach (ProgramData prog in tmpSortedSolNProgs)
                {
                    outProgramText += "Program " + (prog.progNo + 1) + " runs in region " + (prog.region + 1) + " from " +
                        prog.startTime + " to " + prog.endTime + "\n";
                    avg_turnAround += prog.endTime;
                }

                Console.WriteLine("Average turnaround time = " + (avg_turnAround / testCases[i].noOfPrograms));
                Console.Write(outProgramText);
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}