/******************************************
 * Author: Anthony Nguyen
 * Program: Blackjack with an AI
 * Date Created: July 28, 2015
 * Last Modified: August 12, 2015
 * 
 * Description: This is the casino game Blackjack. 
 *      It includes 3 players: human player, dealer, and AI
 *****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Data;

namespace CST407FinalBlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // initialize a dekc for play
        Deck mainDeck = new Deck();
        // initialiaze 2 players for betting, human and AI
        Player playerHuman = new Player();
        Player playerAI = new Player();
        // initialize the AI 
        KnowledgeBaseAI kbAI = new KnowledgeBaseAI();
        // create 3 hands for playing
        Hand playerHand;
        Hand dealerHand;
        Hand botHand;
        // create an array of Image for graphical hand representaion
        Image[] imgPlayerHand = new Image[Constants.HAND_SIZE];
        Image[] imgBotHand = new Image[Constants.HAND_SIZE];
        Image[] imgDealerHand = new Image[Constants.HAND_SIZE];

        public MainWindow()
        {
            InitializeComponent();
            // on startup, show bet and deal instruction
            lblDeal.Text = "Please make your bet and press R to deal";
            // display participant balance
            lblPlayerBalance.Content = playerHuman.Balance.ToString();
            lblBotBalance.Content = playerAI.Balance.ToString();

            // set Image array values
            InitializeImages();

            // hide hand value display
            lblHand.Visibility = Visibility.Hidden;
            lblHandBot.Visibility = Visibility.Hidden;
            lblHandDealer.Visibility = Visibility.Hidden;
        }

        #region Event Handlers
        /// <summary>
        /// handles click event for hit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            PlayerHit();
        }

        /// <summary>
        /// handles click event for stand button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            PlayerStand();
        }

        /// <summary>
        /// handles click event for double button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {
            PlayerDouble();
        }

        /// <summary>
        /// handles key up presses for hit, stand, double, and deal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // deal
                case Key.R:
                    DealHands();
                    break;
                // hit
                case Key.A:
                    PlayerHit();
                    break;
                // stand
                case Key.S:
                    PlayerStand();
                    break;
                // double
                case Key.D:
                    PlayerDouble();
                    break;
            }
        }

        /// <summary>
        /// handles click event for betting
        /// AI bets the same amount each round to show the difference in play by evaluating the current balance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BetChip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label chip = sender as Label;
            int betAmount = 0;

            switch (chip.Name)
            {
                case "BetChip1":
                    betAmount += 1;
                    break;
                case "BetChip5":
                    betAmount += 5;
                    break;
                case "BetChip25":
                    betAmount += 25;
                    break;
                case "BetChip100":
                    betAmount += 100;
                    break;
                case "BetChip500":
                    betAmount += 500;
                    break;
            }
            // human and AI bet the same amount each round
            playerHuman.MakeBet(betAmount);
            playerAI.MakeBet(betAmount);

            // update bet and balance display for both players
            lblPlayerBet.Content = playerHuman.Bet.ToString();
            lblPlayerBalance.Content = playerHuman.Balance.ToString();
            lblBotBet.Content = playerAI.Bet.ToString();
            lblBotBalance.Content = playerAI.Balance.ToString();
        }
        #endregion

        #region Player Methods
        /// <summary>
        /// check if an ace is present and displays both possible hand values
        /// can't be used for dealer because it calculates whole hand value
        /// </summary>
        /// <returns>a string with appropriate values</returns>
        private string PlayerDisplay()
        {
            // check for ace
            if (playerHand.ContainsCard(Enums.FaceValue.ace))
            {
                // GetSumOfHand returns an array of int. 
                int lowAce = playerHand.GetSumOfHand()[0];
                int highAce = playerHand.GetSumOfHand()[1];

                // check for bust and return
                if (highAce < 21) return lowAce.ToString() + "/" + highAce.ToString();
            }
            // return if no ace
            return playerHand.GetBestHand().ToString();
        }

        /// <summary>
        /// adds another card to the players hand and display hand value
        /// </summary>
        private void PlayerHit()
        {
            // check for hand and bet
            if (playerHand != null && playerHuman.Bet != 0)
            {
                // add card
                playerHand.AddCard(mainDeck.Draw());
                // adjust display and value
                lblHand.Content = PlayerDisplay();
                ConvertCards(playerHand, Enums.Participant.Player);
                // check if bust and stand if true
                if (playerHand.GetBestHand() > 21) PlayerStand();
            }
        }

        /// <summary>
        /// player will end turn and go to AI's turn
        /// </summary>
        private void PlayerStand()
        {
            // check for hand and bet
            if (playerHand != null && playerHuman.Bet != 0) AITurn();
        }

        /// <summary>
        /// this will add a card to the player's hand, double the bet value and end turn
        /// </summary>
        private void PlayerDouble()
        {
            // check for hand and bet
            if (playerHand != null && playerHuman.Bet != 0)
            {
                int bet = playerHuman.Bet;

                // add a card
                PlayerHit();
                // double the bet
                playerHuman.MakeBet(bet);
                // end turn
                PlayerStand();
            }
        }
        #endregion

        #region General Methods
        /// <summary>
        /// deal cards to each hand in play
        /// </summary>
        private void DealHands()
        {
            // check for bet
            if (playerHuman.Bet > 0)
            {
                // update instruction text
                lblDeal.Text = "Press R to deal";
                // initialize hands
                playerHand = new Hand();
                dealerHand = new Hand();
                botHand = new Hand();

                // show hand value display
                if (lblHand.Visibility == Visibility.Hidden ||
                    lblHandBot.Visibility == Visibility.Hidden ||
                    lblHandDealer.Visibility == Visibility.Hidden)
                {
                    lblHand.Visibility = Visibility.Visible;
                    lblHandBot.Visibility = Visibility.Visible;
                    lblHandDealer.Visibility = Visibility.Visible;
                }

                // add 2 cards to each hand
                for (int i = 0; i < 2; i++)
                {
                    // add card
                    botHand.AddCard(mainDeck.Draw());
                    playerHand.AddCard(mainDeck.Draw());
                    dealerHand.AddCard(mainDeck.Draw());
                }

                // update hand value display
                lblHand.Content = PlayerDisplay();
                lblHandBot.Content = botHand.GetBestHand().ToString();
                // only give hand value of 1 card
                lblHandDealer.Content = dealerHand.ConvertFaceValue(dealerHand.Cards[0]);

                // check the first card for ace, if true update possible hand values
                if (dealerHand.Cards[0].FaceValue == Enums.FaceValue.ace)
                    lblHandDealer.Content += "/11";

                // hide cards from last round
                HideCards();

                // update card displays
                ConvertCards(playerHand, Enums.Participant.Player);
                ConvertCards(botHand, Enums.Participant.AI);
                ConvertCards(dealerHand, Enums.Participant.Dealer);
                // reveal only 1 card
                ShowCardBack(imgDealerHand, 1);

                // check for win condition
                if (playerHand.HasBlackJack())
                {
                    AITurn();
                    CollectChips(CheckWin(playerHand), playerHuman);
                }
            }
            else MessageBox.Show("Please place your bet");
        }

        /// <summary>
        /// dealer must hit until hand value is at least 17
        /// not accounting for soft-17 rule
        /// </summary>
        private void DealerTurn()
        {
            while (dealerHand.GetBestHand() < 17)
            {
                dealerHand.AddCard(mainDeck.Draw());
            }
            // update hand value and card display
            lblHandDealer.Content = dealerHand.GetBestHand().ToString();
            ConvertCards(dealerHand, Enums.Participant.Dealer);
            // check for win conditions
            CollectChips(CheckWin(playerHand), playerHuman);
            CollectChips(CheckWin(botHand), playerAI);
        }

        /// <summary>
        /// AI's turn to play. plays according to chart 
        /// </summary>
        private void AITurn()
        {
            // check for blackjack, if true, reward chips
            if (botHand.HasBlackJack())
            {
                DealerTurn();
                CollectChips(CheckWin(botHand), playerAI);
            }

            // AI reads in dealer's revealed card
            kbAI.DealerCard = dealerHand.Cards[0].FaceValue;
            // action that AI will take
            string action;
            // flag if bot chose to stand
            bool playing = true;

            while (playing)
            {
                // set action
                action = kbAI.AIPlay(botHand);
                switch (action)
                {
                    // hit
                    case "h":
                        botHand.AddCard(mainDeck.Draw());
                        break;
                    // double
                    case "d":
                        if (botHand != null && playerAI.Bet != 0)
                        {
                            int bet = playerAI.Bet;

                            botHand.AddCard(mainDeck.Draw());
                            playerAI.MakeBet(bet);
                            playing = false;
                            DealerTurn();
                        }
                        break;
                    // stand
                    default:
                        playing = false;
                        break;
                }
            }
            // update hand value and display
            lblHandBot.Content = botHand.GetBestHand().ToString();
            ConvertCards(botHand, Enums.Participant.AI);
            // end turn
            DealerTurn();
        }

        /// <summary>
        /// displays the cards in hand
        /// </summary>
        /// <param name="hand">hand to display</param>
        /// <param name="turn">participant identifier</param>
        private void ConvertCards(Hand hand, Enums.Participant turn)
        {
            List<string> images = new List<string>();
            string name;

            foreach (Card card in hand.Cards)
            {
                // get the name of the card in hand
                name = card.Suit.ToString().Substring(0, 2) + (int)card.FaceValue;
                // use URI pack string to get image location
                images.Add("pack://application:,,,/CST407FinalBlackJack;component/Resources/deck/" + name.ToLower() + ".png");
            }

            switch (turn)
            {
                // player
                case Enums.Participant.Player:
                    for (int i = 0; i < images.Count; i++)
                    {
                        // check for hand count
                        if (images.Count < Constants.HAND_SIZE)
                            // set the images
                            imgPlayerHand[i].Source = new ImageSourceConverter().ConvertFromString(images[i]) as ImageSource;
                    }
                    break;
                // AI
                case Enums.Participant.AI:
                    for (int i = 0; i < images.Count; i++)
                    {
                        if (images.Count < Constants.HAND_SIZE)
                            imgBotHand[i].Source = new ImageSourceConverter().ConvertFromString(images[i]) as ImageSource;
                    }
                    break;
                // dealer
                case Enums.Participant.Dealer:
                    for (int i = 0; i < images.Count; i++)
                    {
                        if (images.Count < Constants.HAND_SIZE)
                            imgDealerHand[i].Source = new ImageSourceConverter().ConvertFromString(images[i]) as ImageSource;
                    }
                    break;
            }
        }
        /// <summary>
        /// shows the card back for the dealer
        /// </summary>
        /// <param name="cardImages">Image array to modify</param>
        /// <param name="index">index of card list to hide</param>
        private void ShowCardBack(Image[] cardImages, int index)
        {
            string uriPack = "pack://application:,,,/CST407FinalBlackJack;component/Resources/deck/cardback.png";
            if (index < Constants.HAND_SIZE)
                cardImages[index].Source = new ImageSourceConverter().ConvertFromString(uriPack) as ImageSource;
        }

        /// <summary>
        /// calculates the end result state for input hand against dealer
        /// </summary>
        /// <param name="hand">hand to calculate against dealer</param>
        /// <returns></returns>
        private Enums.EndResult CheckWin(Hand hand)
        {
            // return value
            Enums.EndResult result = Enums.EndResult.Waiting;
            // hand values
            int playerHandValue = hand.GetBestHand();
            int dealerHandValue = dealerHand.GetBestHand();

            // win or push, player has blackjack
            if (hand.HasBlackJack())
            {
                // dealer has blackjack aswell
                if (dealerHand.HasBlackJack())
                    result = Enums.EndResult.Push;
                else
                    result = Enums.EndResult.PlayerBlackJack;
            }

            // dealer blackjack
            else if (dealerHand.HasBlackJack()) result = Enums.EndResult.DealerBlackJack;

            // push, equal hand value
            else if (playerHandValue == dealerHandValue) result = Enums.EndResult.Push;

            // loss, dealer has greater hand value w/o bust
            else if (dealerHandValue <= 21 && playerHandValue < dealerHandValue) result = Enums.EndResult.DealerWin;

            // loss, player bust
            else if (playerHandValue > 21) result = Enums.EndResult.PlayerBust;

            // win, player has greater hand value w/o bust
            else if (playerHandValue <= 21 && playerHandValue > dealerHandValue) result = Enums.EndResult.PlayerWin;

            // win, dealer bust
            else if (dealerHandValue > 21) result = Enums.EndResult.DealerBust;

            return result;
        }

        /// <summary>
        /// reward or take chips from player depending on end result
        /// </summary>
        /// <param name="er">end result for player</param>
        /// <param name="player">player to have balance adjusted</param>
        private void CollectChips(Enums.EndResult er, Player player)
        {
            switch (er)
            {
                // tie
                case Enums.EndResult.Push:
                    player.Balance += player.Bet;
                    player.Bet = 0;
                    break;
                // loss
                case Enums.EndResult.DealerBlackJack:
                case Enums.EndResult.DealerWin:
                case Enums.EndResult.PlayerBust:
                    player.Bet = 0;
                    break;
                // win
                case Enums.EndResult.DealerBust:
                case Enums.EndResult.PlayerWin:
                    player.Balance += player.Bet * 2;
                    player.Bet = 0;
                    break;
                // blackjack, 3:2 win
                case Enums.EndResult.PlayerBlackJack:
                    player.Balance += player.Bet * 2 + (int)Math.Ceiling(player.Bet * .5);
                    player.Bet = 0;
                    break;
                // in the case no result is found, ie Waiting
                default:
                    break;
            }
            // update bet and balance value display for both players
            lblPlayerBet.Content = playerHuman.Bet.ToString();
            lblPlayerBalance.Content = playerHuman.Balance.ToString();
            lblBotBet.Content = playerAI.Bet.ToString();
            lblBotBalance.Content = playerAI.Balance.ToString();

            // no more chips available for human player
            if (player.Balance == 0) MessageBox.Show("GAME OVER");
        }
        #endregion

        #region Other Methods
        /// <summary>
        /// set Image source to null to hide cards
        /// </summary>
        private void HideCards()
        {
            imgPlayerCard3.Source = null;
            imgPlayerCard4.Source = null;
            imgPlayerCard5.Source = null;
            imgPlayerCard6.Source = null;

            imgBotCard3.Source = null;
            imgBotCard4.Source = null;
            imgBotCard5.Source = null;
            imgBotCard6.Source = null;

            imgDealerCard3.Source = null;
            imgDealerCard4.Source = null;
            imgDealerCard5.Source = null;
            imgDealerCard6.Source = null;
        }

        /// <summary>
        /// set array values
        /// </summary>
        private void InitializeImages()
        {
            // initialize Images
            imgPlayerHand[0] = imgPlayerCard1;
            imgPlayerHand[1] = imgPlayerCard2;
            imgPlayerHand[2] = imgPlayerCard3;
            imgPlayerHand[3] = imgPlayerCard4;
            imgPlayerHand[4] = imgPlayerCard5;
            imgPlayerHand[5] = imgPlayerCard6;

            imgBotHand[0] = imgBotCard1;
            imgBotHand[1] = imgBotCard2;
            imgBotHand[2] = imgBotCard3;
            imgBotHand[3] = imgBotCard4;
            imgBotHand[4] = imgBotCard5;
            imgBotHand[5] = imgBotCard6;

            imgDealerHand[0] = imgDealerCard1;
            imgDealerHand[1] = imgDealerCard2;
            imgDealerHand[2] = imgDealerCard3;
            imgDealerHand[3] = imgDealerCard4;
            imgDealerHand[4] = imgDealerCard5;
            imgDealerHand[5] = imgDealerCard6;
        }
        #endregion
    }
}

