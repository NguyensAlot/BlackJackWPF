using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    /// <summary>
    /// Class to represent player blance and betting
    /// </summary>
    class Player
    {
        #region Fields and Properties
        private int _balance;
        private int _bet;

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

        /// <summary>
        /// default constructor 
        /// </summary>
        public Player()
        {
            // both values just happen to be 1000
            _balance = Constants.SHUFFLE_AMOUNT;
            _bet = 0;
        }

        /// <summary>
        /// puts money on the table to bet and removes from balance
        /// </summary>
        /// <param name="bet">amount to put in</param>
        public void MakeBet(int bet)
        {
            // check if enough money is available
            if (_balance >= bet)
            {
                _balance -= bet;
                _bet += bet;
            }
        }
    }
}
