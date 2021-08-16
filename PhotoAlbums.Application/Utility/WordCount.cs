using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utility
{
    public class WordCount
    {
        public static int getWordCount(string words)
        {
            int count = 0;
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            count = words.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            return count;
        }
    }
}
