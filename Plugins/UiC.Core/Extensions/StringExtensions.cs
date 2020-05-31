using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Core.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitAdvanced(this string expression, string delimiter)
        {
            return SplitAdvanced(expression, delimiter, "", false);
        }

        public static string[] SplitAdvanced(this string expression, string delimiter,
                                     string qualifier)
        {
            return SplitAdvanced(expression, delimiter, qualifier, false);
        }


        public static string[] SplitAdvanced(this string expression, string delimiter,
            string qualifier, bool ignoreCase)
        {
            return SplitAdvanced(expression, new[] { delimiter }, qualifier, false);
        }

        public static string[] SplitAdvanced(this string expression, string[] delimiters,
                                     string qualifier, bool ignoreCase)
        {
            bool qualifierState = false;
            var startIndex = 0;
            var values = new ArrayList();

            for (int charIndex = 0; charIndex < expression.Length - 1; charIndex++)
            {
                if (qualifier != null)
                    if (string.Compare(expression.Substring
                                           (charIndex, qualifier.Length), qualifier, ignoreCase) == 0)
                    {
                        qualifierState = !(qualifierState);
                    }
                    else if (!(qualifierState)
                             && (delimiters.Any(x => string.Compare(expression.Substring
                                                  (charIndex, x.Length), x, ignoreCase) == 0)))
                    {
                        values.Add(expression.Substring
                                       (startIndex, charIndex - startIndex));
                        startIndex = charIndex + 1;
                    }
            }

            if (startIndex < expression.Length)
                values.Add(expression.Substring
                               (startIndex, expression.Length - startIndex));

            var returnValues = new string[values.Count];
            values.CopyTo(returnValues);
            return returnValues;
        }
    }
}
