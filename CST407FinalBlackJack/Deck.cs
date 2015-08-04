using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST407FinalBlackJack
{
    class Deck
    {
        // Creates a list of cards
        private List<Card> _cards = new List<Card>();

        /// <summary>
        /// Creates classic 52 cards with each suit and face value
        /// </summary>
        /// <param name="numberOfDecks">How many decks to create</param>
        public Deck()
        {
            CreateDeck(2);
        }

        private void CreateDeck(int shoe)
        {
            for (int i = 0; i < shoe; i++)
            {
                foreach (Enums.Suit suit in Enum.GetValues(typeof(Enums.Suit)))
                {
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
        /// Draws one card and removes it from the deck
        /// </summary>
        /// <returns>Returns a single card</returns>
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
        /// Shuffles the cards in the deck
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

        private void SwapCard(int index1, int index2)
        {
            Card tempCard = _cards[index1];
            _cards[index1] = _cards[index2];
            _cards[index2] = tempCard;
        }
    }
}
