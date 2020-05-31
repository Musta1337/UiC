using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiC.Commands.Commands.Matching
{
    public abstract class BaseCriteria<T>
    {
        public BaseCriteria(BaseMatching<T> matching, string pattern)
        {
            Matching = matching;
            Pattern = pattern;
        }

        public BaseMatching<T> Matching
        {
            get;
            protected set;
        }

        public string Pattern
        {
            get;
            protected set;
        }

        public abstract T[] GetMatchings();
    }
}
