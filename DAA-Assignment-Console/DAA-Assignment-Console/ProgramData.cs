using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAA_Assignment_Console
{
    class ProgramData
    {
        public int progNo;
        public int startTime;
        public int endTime;
        public int region;

        public static implicit operator ProgramData(List<ProgramData> v)
        {
            throw new NotImplementedException();
        }
    }
}
