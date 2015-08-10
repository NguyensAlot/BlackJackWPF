using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    class Player
    {
        #region Fields
        private int _balance;
        private int _bet;
        #endregion

        #region Properties
        public int Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
        public int Bet
        {
            get { return _bet; }
            set { _bet = value; }
        } 
        #endregion



        public Player()
        {
            _balance = 1000;
            _bet = 0;
        }

        public void MakeBet(int bet)
        {
            if (_balance >= bet)
            {
                _balance -= bet;
                _bet += bet;
            }
        }
    }
}
