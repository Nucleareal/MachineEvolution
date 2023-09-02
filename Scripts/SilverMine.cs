using System;
using System.Collections.Generic;
using System.Text;

namespace StellaMachineNS
{
    internal class SilverMine : CardData
    {
        protected override bool CanHaveCard(CardData otherCard)
        {
            return otherCard.MyCardType == CardType.Humans;
        }
    }
}
