using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM201c
{
    internal class AnswerModel
    {
        public string Question { get; set; }
        public int Type { get; set; }
        public List<string> Answers { get; set; }
    }

}
