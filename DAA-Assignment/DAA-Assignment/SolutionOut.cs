using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAA_Assignment
{
    class SolutionOut
    {
        public List<Programs> programList;
        
        public class Programs
        {
            public int progNo;
            public int startTime;
            public int endTime;
            public int region;
        }
    }
}
