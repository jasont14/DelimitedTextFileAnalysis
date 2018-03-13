using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextFileMeta
{
    class TextFileAnalysis
    {
        string[] fieldNames;
        int[] maxLength;
        bool[] isNull;
        string[] recommendedType;
        bool[] nullPresent;
        string fileName;
        string directoryInfo;
        char delimiter;
        int recordCount;
        int fieldCount;

        public string FileName { get => fileName; set => fileName = value; }
        public string DirectoryInfo { get => directoryInfo; set => directoryInfo = value; }
        public char Delimiter { get => delimiter; set => delimiter = value; }

        public TextFileAnalysis(string fileName, string directory, char delimeter)
        {
            FileName = fileName;
            DirectoryInfo = directory;
            Delimiter = delimeter;            
            InitializeTextFileAnalysisFields();
        }

        public void RunAnalysis()
        {
            string s = null;
            using (StreamReader sr = new StreamReader(directoryInfo + fileName))
            {
                try
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (recordCount > 0)
                        {
                            EvalRecord(s+Delimiter);
                        }                        
                        recordCount++;
                    }
                }
                catch (Exception exc)
                {
                    string error = exc.Message;
                }
            }
        }

        private void InitializeTextFileAnalysisFields()
        {            
            fieldCount = 0;
            recordCount = 0;
            SetFieldNamesFromFile();
            maxLength = new int[fieldCount];
            isNull = new bool[fieldCount];
            recommendedType = new string[fieldCount];
            nullPresent = new bool[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                maxLength[i] = 0;
                isNull[i] = true;
                recommendedType[i] = "Not Implemented";
                nullPresent[i] = false;
            }         
        }

        //open file and read header to get field count
        private int SetFieldNamesFromFile()
        {
            int result = -1;
            string s;
            using (StreamReader sr = new StreamReader(directoryInfo + fileName))
            {
                try
                {
                    s = sr.ReadLine();
                    GetHeaderInfo(s);
                    sr.Close();
                }
                catch(Exception exc)
                {
                    string error = exc.Message;
                }
            }
            return result;
        }

        private void GetHeaderInfo(string s)
        {
            int count = 1;
            int sLength = s.Length;
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                count++;
                sb.Append(c);

                //Read to delimiter
                if (c == Delimiter)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(",");
                    fieldCount++;
                }
                else if(count-1 == sLength)
                {
                    sb.Append(",");
                    fieldCount++;
                }
            }

            SetFieldInfo(sb.ToString());
        }

        private void SetFieldInfo(string s)
        {
            int count = 0;
            int iterationCount = 1;
            fieldNames = new string[fieldCount];

            StringBuilder sb = new StringBuilder();

            foreach(char c in s)
            {
                sb.Append(c);

                if (c == ',' || iterationCount == s.Length)
                {
                    sb.Remove(sb.Length - 1, 1);
                    fieldNames[count] = sb.ToString();
                    sb.Clear();
                    count++;
                }

                iterationCount++;
            }
        }          

        private void EvalRecord(string s)
        {
            int recordLength = 0;
            int recordFieldCount = 0;
            int iterCount = 1;            
            
            foreach (char c in s)
            {
                if (c == Delimiter || iterCount == s.Length)
                {
                    if (recordLength > maxLength[recordFieldCount])
                    {
                        maxLength[recordFieldCount] = recordLength;
                    }
                    if (recordLength > 0)
                    {
                        isNull[recordFieldCount] = false;
                    }
                    if (recordLength == 0)
                    {
                        nullPresent[recordFieldCount] = true;
                    }

                    recordLength = 0;
                    recordFieldCount++;
                }              
                iterCount++;

                if (c != delimiter)
                { 
                recordLength++;
                }

            }
        }

        public void PrintAnalysisDetails()
        {
            Console.WriteLine("Field Count: {0}", fieldCount);
            Console.Write("Field Name \t");
            Console.Write("Max Length \t");
            Console.Write("isNull \t");
            Console.Write("null Present \t");
            Console.Write("Recommended");
            Console.WriteLine();

            int i = 0;
            while (i<fieldCount)
            {
                Console.Write(fieldNames[i] + "\t");
                Console.Write(maxLength[i].ToString() + "\t\t");
                Console.Write(isNull[i].ToString() + "\t");
                Console.Write(nullPresent[i].ToString() + "\t\t");
                Console.Write(recommendedType[i] + "\t");
                Console.WriteLine();
                i++;
            }

            Console.WriteLine("Total Record Count(Excluding 1st Line): {0}", recordCount);
        }
    }
}
