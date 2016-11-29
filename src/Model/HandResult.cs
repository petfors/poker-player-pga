using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple.Model
{
    public class HandResult
    {
        public Hand Hand { get; set; }
        public IEnumerable<EvaluatedCard> Cards
        {
            get;
            set;
        }
    }
}
