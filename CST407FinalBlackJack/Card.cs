using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CST407FinalBlackJack
{
    class Card
    {
        // Fields
        private Enums.Suit _suit;
        private Enums.FaceValue _faceValue;
        // Properties
        public Enums.Suit Suit { get { return _suit; } }
        public Enums.FaceValue FaceValue { get { return _faceValue; } }

        /// <summary>
        /// Constructor to initialize a new card
        /// </summary>
        /// <param name="suit"></param>
        /// <param name="faceValue"></param>
        public Card(Enums.Suit suit, Enums.FaceValue faceValue)
        {
            _suit = suit;
            _faceValue = faceValue;
        }

        /// <summary>
        /// Override ToString() to display face value and suit (ace of spades)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _faceValue + "of" + _suit;
        }
    }
}
