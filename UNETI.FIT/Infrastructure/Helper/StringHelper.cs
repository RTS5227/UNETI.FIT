using System;
using System.Collections.Generic;
using System.Linq;

namespace UNETI.FIT.Infrastructure.Helper
{
    public static class StringHelper
    {
        public static IEnumerable<string> SplitToLines(this string raw, int maxLength)
        {
            IEnumerable<string> str = raw.Split(' ').Concat(new[] { " " });
            return str.Skip(1).Aggregate(str.Take(1).ToList(), (seed, w) =>
            {
                string last = seed.Last();
                while (last.Length > maxLength)
                {
                    seed[seed.Count - 1] = last.Substring(0, maxLength);//0 to maxLength
                    last = last.Substring(maxLength);//maxLength to rest
                    seed.Add(last);
                }
                string temp = last + " " + w;
                if (temp.Length > maxLength) seed.Add(w);
                else seed[seed.Count - 1] = temp;
                return seed;
            });
        }

        public static string[] SplitToArray(this string input)
        {
            input = input.Replace(' ', new char());
            return input.Split(',');
        }
    }
}