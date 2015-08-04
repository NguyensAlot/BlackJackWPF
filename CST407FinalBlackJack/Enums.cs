using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    class Enums
    {
        public enum Suit
        {
            hearts = 0,
            diamonds,
            clubs,
            spades
        }
        
        /// <summary>
        /// Unique identifier for cards
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

        public enum Participant
        {
            Player = 0,
            Bot,
            Dealer
        }

        /// <summary>
        /// EndResult maintains the game result state. 
        /// </summary>
        public enum EndResult
        {
            /// <summary>
            /// The dealer got Blackjack. In this case, you will not get a PlayerMove event. This is a loss. 
            /// If a player gets a Blackjack when a Dealer gets a Blackjack the result will be a push (tie) instead. 
            /// </summary>
            DealerBlackJack = 0,

            /// <summary>
            /// The player got Blackjack and will be paid 3:2, so for a bet of $100, you will win $150. This is a win. 
            /// </summary>
            PlayerBlackJack = 1,

            /// <summary>
            /// The player has busted by going over 21. This is a loss. 
            /// </summary>
            PlayerBust = 2,

            /// <summary>
            /// The dealer has busted by going over 21. This is a win. 
            /// </summary>
            DealerBust = 3,

            /// <summary>
            /// The player has tied with the dealer. This is a push (tie)
            /// </summary>
            Push = 4,

            /// <summary>
            /// The player won the game. 
            /// </summary>
            PlayerWin = 5,

            /// <summary>
            /// The dealer won the game. 
            /// </summary>
            DealerWin = 6,
        }
    }

    class Constants
    {
        public const int NUM_OF_CARDS = 52;
        public const int SHUFFLE_AMOUNT = 1000;
        public const int HAND_SIZE = 6;
    }
}
