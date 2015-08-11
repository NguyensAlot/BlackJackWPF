using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    class Hand
    {
        /// <summary>
        /// The protected list of cards
        /// </summary>
        protected List<Card> _cards = new List<Card>();

        /// <summary>
        /// The total number of cards in the hand. This is read only.
        /// </summary>
        public int NumCards { get { return _cards.Count; } }

        /// <summary>
        /// A List type containing all of the cards. This is read-only. 
        /// </summary>
        public List<Card> Cards { get { return _cards; } }

        /// <summary>
        /// Checks to see if the hand contains a card of a certain face value
        /// </summary>
        /// <param name="item">the card to look for</param>
        /// <returns>true if the card exists</returns>
        public bool ContainsCard(Enums.FaceValue id)
        {
            foreach (Card c in _cards)
            {
                if (c.FaceValue == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the highest possible hand without busting. If a player has an Ace, it will treat the ace as high (11) if the high value is not greater than 21. Otherwise it will return Ace low (1). 
        /// </summary>
        /// <returns>The total of the best hand from the cards in the Hand</returns>
        public int GetBestHand()
        {
            int[] handValues = GetSumOfHand();

            if (handValues[1] <= 21)
            {
                //send back ace high value that isn't busted
                return handValues[1];
            }
            else
            {
                //send back ace low value (may be same as above or a bust too)
                return handValues[0];
            }
        }

        /// <summary>
        /// Gets the total value of a hand from BlackJack values
        /// </summary>
        /// <returns>int[] array. First value is low value with Ace(s), second value is high value with an Ace. With no aces, the values are identical</returns>
        public int[] GetSumOfHand()
        {
            int[] returnValue = new int[2];
            int highValue = 0;
            int lowValue = 0;

            foreach (Card c in _cards)
            {
                lowValue += ConvertFaceValue(c);
            }

            // check if ace is present in hand
            if (ContainsCard(Enums.FaceValue.ace))
                highValue = lowValue + 10;
            else
                highValue = lowValue;

            // set return values
            returnValue[0] = lowValue;
            returnValue[1] = highValue;

            return returnValue;
        }

        /// <summary>
        /// Checks if a player has blackjack (card total of 21 with two cards). 
        /// </summary>
        /// <returns>True if 21, false if not</returns>
        public bool HasBlackJack()
        {
            if (this.Cards.Count == 2 && GetBestHand() == 21)
                return true;
            return false;
        }

        /// <summary>
        /// Add card to hand
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public int ConvertFaceValue(Card card)
        {
            int cardValue = 0;

            switch (card.FaceValue)
            {
                case Enums.FaceValue.ten:
                case Enums.FaceValue.jack:
                case Enums.FaceValue.queen:
                case Enums.FaceValue.king:
                    cardValue = 10;
                    break;
                case Enums.FaceValue.ace:
                    cardValue = 1;
                    break;
                default:
                    cardValue = (int)card.FaceValue;
                    break;
            }

            return cardValue;
        }

        
    }
}
