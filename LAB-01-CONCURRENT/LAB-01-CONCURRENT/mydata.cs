using System;
using System.Collections.Generic;

namespace LAB_01_CONCURRENT
{
    class mydata
    {
        public string Text { get; set; }
        public int Number { get; set; }
        public double Value { get; set; }

        public mydata(string text, int number, double value)//constructor eith parameter
        {
            Text = text;
            Number = number;
            Value = value;
        }
        public override string ToString()//override method
        {
            string line;
            line = string.Format("| {0,-20} | {1,-8} | {2,-8} |", Text, Number, Value);
            return line;

        }

        public int ComputeResult()//calculating the result
        {           
            return Number * (int)Value;
        }

        public bool FitsCriterion()
        {
            return Number % 2 == 0; // Criteria to check if it goes to result monitor or not;if even number then goes to result monitor
        }
        public int CompareTo(mydata other)//compare to method used in sorting algorithm
        {
            if (this.Number * (int)Value > other.Number * (int)Value)//sort according to compute result.
                return 1;
            else if (this.Number * (int)Value < other.Number * (int)Value)
                return -1;
            return 0;
        }

    }
}
