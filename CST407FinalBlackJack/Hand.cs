using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    /// <summary>
    /// Class representation of a blackjack hand
    /// </summary>
    class Hand
    {
        #region Field and Properties
        // the list of cards each hand contains
        private List<Card> _cards = new List<Card>();
        // returns the list of cards
        public List<Card> Cards { get { return _cards; } }
        #endregion

        /// <summary>
        /// traverse hand to find if card exists
        /// </summary>
        /// <param name="id">card to find</param>
        /// <returns>true if card exists</returns>
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
        /// since aces are double valued, this will calculate what ace should be for best hand without busting
        /// </summary>
        /// <returns>hand value</returns>
        public int GetBestHand()
        {
            int[] handValues = GetSumOfHand();

            // try to find higher value first
            if (handValues[1] <= 21) return handValues[1];
            // return hand value with lower ace
            else return handValues[0];
        }

        /// <summary>
        /// calculates hand value with both high and low aces.
        /// </summary>
        /// <returns>int array containing hand value. if no ace is present, both indices carry the same value</returns>
        public int[] GetSumOfHand()
        {
            int[] returnValue = new int[2];
            int highValue = 0;
            int lowValue = 0;

            foreach (Card c in _cards)
            {
                lowValue += ConvertFaceValue(c);
            }

            // adjust hand value is ace present
            if (ContainsCard(Enums.FaceValue.ace)) highValue = lowValue + 10;
            // else array will contain the same value
            else highValue = lowValue;

            returnValue[0] = lowValue;
            returnValue[1] = highValue;
            return returnValue;
        }

        /// <summary>
        /// checks if hand has blackjack
        /// </summary>
        /// <returns>true if hand value is 21 with two cards in hand</returns>
        public bool HasBlackJack()
        {
            if (this.Cards.Count == 2 && GetBestHand() == 21) return true;
            return false;
        }

        /// <summary>
        /// add card to hand
        /// </summary>
        /// <param name="card">card to add</param>
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        /// <summary>
        /// converts card ID to actual card value
        /// </summary>
        /// <param name="card">card to convert</param>
        /// <returns>card value</returns>
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
