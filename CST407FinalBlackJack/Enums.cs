using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    /// <summary>
    /// Class for all enums used
    /// </summary>
    class Enums
    {
        /// <summary>
        /// stardard 4 suits to a deck
        /// </summary>
        public enum Suit
        {
            hearts = 0,
            diamonds,
            clubs,
            spades
        }
        
        /// <summary>
        /// unique identifier for cards
        /// </summary>
        public enum FaceValue
        {
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9,
            ten = 10,
            jack = 11,
            queen = 12,
            king = 13,
            ace = 14
        }

        /// <summary>
        /// name of the players... could be managed as a list if number of players wasn't static
        /// </summary>
        public enum Participant
        {
            Player = 0,
            AI,
            Dealer
        }

        /// <summary>
        /// the end result of each hand
        /// </summary>
        public enum EndResult
        {
            /// <summary>
            /// dealer got blackjack on deal
            /// loss or push
            /// </summary>
            DealerBlackJack = 0,

            /// <summary>
            /// pays 3:2, get 21 off deal
            /// win or push
            /// </summary>
            PlayerBlackJack,

            /// <summary>
            /// hand value is over 21
            /// loss
            /// </summary>
            PlayerBust,

            /// <summary>
            /// dealer hand value is over 21
            /// win
            /// </summary>
            DealerBust,

            /// <summary>
            /// when the player and dealer tie
            /// push (no money lost)
            /// </summary>
            Push,

            /// <summary>
            /// player hand value is greater than dealer's
            /// win
            /// </summary>
            PlayerWin,

            /// <summary>
            /// dealer hand value is greater than player's
            /// loss
            /// </summary>
            DealerWin,

            /// <summary>
            /// result is undecided
            /// </summary>
            Waiting
        }
    }

    /// <summary>
    /// Class that contains constant values
    /// </summary>
    class Constants
    {
        public const int NUM_OF_CARDS = 52;
        public const int SHUFFLE_AMOUNT = 1000;
        // max hand size for graphical content
        public const int HAND_SIZE = 6;
        public const int BLACKJACK_CARD_AMT = 2;
        public const int TWENTY_ONE = 21;
    }
}
