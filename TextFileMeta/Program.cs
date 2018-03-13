using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileMeta
{
    class Program
    {
        static void Main(string[] args)
        {
            TextFileAnalysis myInfo = new TextFileAnalysis("AK_Features_20180201.txt", @"DATA\", '|');     
            myInfo.RunAnalysis();
            myInfo.PrintAnalysisDetails();
            Console.ReadKey();
        }
    }
}
