using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    abstract class AbstractBot
    {
        protected int wins;

        public virtual bool BotPlay(Hand hand)
        {
            throw new NotImplementedException();
        }
    }
}
