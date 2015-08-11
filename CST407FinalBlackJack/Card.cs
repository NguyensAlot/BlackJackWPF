using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CST407FinalBlackJack
{
    /// <summary>
    /// Card class to represent 1 card in a deck or hand
    /// </summary>
    class Card
    {
        #region Fields and Properties
        // card suit
        private Enums.Suit _suit;
        // card ID
        private Enums.FaceValue _faceValue;
        
        public Enums.Suit Suit { get { return _suit; } }
        public Enums.FaceValue FaceValue { get { return _faceValue; } }
        #endregion

        /// <summary>
        /// Constructor to initialize a new card
        /// </summary>
        /// <param name="suit">card suit</param>
        /// <param name="faceValue">card ID</param>
        public Card(Enums.Suit suit, Enums.FaceValue faceValue)
        {
            _suit = suit;
            _faceValue = faceValue;
        }

        /// <summary>
        /// Override ToString() to display face value and suit (ace of spades).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _faceValue + "of" + _suit;
        }
    }
}
