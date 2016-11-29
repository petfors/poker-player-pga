using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple.Model
{
    public enum Hand
    {
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        FourOfAKind,
        Straight,
        Flush,
        FullHouse,
        StraightFlush
    }
}
