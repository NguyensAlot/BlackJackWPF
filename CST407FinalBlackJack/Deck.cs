using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    /// <summary>
    /// Class for deck represenation
    /// </summary>
    class Deck
    {
        #region Fields
        // list of cards
        private List<Card> _cards = new List<Card>();
        #endregion

        /// <summary>
        /// Default constructor creates a shoe of cards 
        /// </summary>
        public Deck()
        {
            CreateDeck(2);
        }

        /// <summary>
        /// creates the decks according to the number of decks desired
        /// </summary>
        /// <param name="shoe">number of decks</param>
        private void CreateDeck(int shoe)
        {
            // for each deck in the shoe
            for (int i = 0; i < shoe; i++)
            {
                // for reach suit
                foreach (Enums.Suit suit in Enum.GetValues(typeof(Enums.Suit)))
                {
                    // for each card ID
                    foreach (Enums.FaceValue fv in Enum.GetValues(typeof(Enums.FaceValue)))
                    {
                        _cards.Add(new Card(suit, fv));
                    }
                }
            }
            //shuffle after creating
            this.Shuffle();
        }

        /// <summary>
        /// Draw a card from the deck and remove it
        /// </summary>
        /// <returns>a card off front of the list</returns>
        public Card Draw()
        {
            if (_cards.Count < 1)
            {
                CreateDeck(2);
            }

            Card card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        public void Shuffle()
        {
            Random random = new Random();
            // repeat swapping a certain amount of times
            for (int i = 0; i < Constants.SHUFFLE_AMOUNT; i++)
            {
                int index1 = random.Next(_cards.Count);
                int index2 = random.Next(_cards.Count);
                SwapCard(index1, index2);
            }
        }

        /// <summary>
        /// Swap the position of 2 cards in the list
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private void SwapCard(int index1, int index2)
        {
            // set card1 equal to temp
            Card tempCard = _cards[index1];
            // set card2 to card1 position
            _cards[index1] = _cards[index2];
            // set card1 temp to card2 position
            _cards[index2] = tempCard;
        }
    }
}
