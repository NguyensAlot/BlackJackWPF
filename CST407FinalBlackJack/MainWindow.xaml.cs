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
        Deck mainDeck = new Deck();
        Player playerHuman = new Player();
        Player playerBot = new Player();
        ConcreteAssistedBot concreteBot = new ConcreteAssistedBot();
        Hand playerHand;
        Hand dealerHand;
        Hand botHand;
        Image[] imgPlayerHand = new Image[Constants.HAND_SIZE];
        Image[] imgBotHand = new Image[Constants.HAND_SIZE];
        Image[] imgDealerHand = new Image[Constants.HAND_SIZE];
        public MainWindow()
        {
            InitializeComponent();
            lblDeal.Text = "Please make your bet and press R to deal";
            lblPlayerBalance.Content = playerHuman.Balance.ToString();
            lblBotBalance.Content = playerBot.Balance.ToString();

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

            lblHand.Visibility = Visibility.Hidden;
            lblHandBot.Visibility = Visibility.Hidden;
            lblHandDealer.Visibility = Visibility.Hidden;
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            PlayerHit();
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            PlayerStand();
        }

        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {
            PlayerDouble();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    DealHands();
                    break;
                case Key.A:
                    PlayerHit();
                    break;
                case Key.S:
                    PlayerStand();
                    break;
                case Key.D:
                    PlayerDouble();
                    break;
                case Key.F:
                    break;
            }
        }

        /// <summary>
        /// 
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
                    //playerHuman.MakeBet(1);
                    break;
                case "BetChip5":
                    betAmount += 5;
                    //playerHuman.MakeBet(5);
                    break;
                case "BetChip25":
                    betAmount += 25;
                    //playerHuman.MakeBet(25);
                    break;
                case "BetChip100":
                    betAmount += 100;
                    //playerHuman.MakeBet(100);
                    break;
                case "BetChip500":
                    betAmount += 500;
                    //playerHuman.MakeBet(500);
                    break;
            }
            playerHuman.MakeBet(betAmount);
            playerBot.MakeBet(betAmount);

            lblPlayerBet.Content = playerHuman.Bet.ToString();
            lblPlayerBalance.Content = playerHuman.Balance.ToString();
            lblBotBet.Content = playerBot.Bet.ToString();
            lblBotBalance.Content = playerBot.Balance.ToString();
        }
        // OTHER METHODS
        // player methods
        private string PlayerDisplay()
        {
            if (playerHand.ContainsCard(Enums.FaceValue.ace))
            {
                int lowAce = playerHand.GetSumOfHand()[0];
                int highAce = playerHand.GetSumOfHand()[1];

                if (highAce < 21)
                    return lowAce.ToString() + "/" + highAce.ToString();
            }
            return CountHand(playerHand).ToString();
        }
        private void PlayerHit()
        {
            if (playerHand != null && playerHuman.Bet != 0)
            {
                playerHand.AddCard(mainDeck.Draw());
                lblHand.Content = PlayerDisplay();
                ConvertCards(playerHand, Enums.Participant.Player);
                if (CountHand(playerHand) > 21)
                {
                    PlayerStand();
                }
            }
        }
        private void PlayerStand()
        {
            if (playerHand != null && playerHuman.Bet != 0)
            {
                //MessageBox.Show(concreteBot.CountCards(botHand, Enums.Participant.Player).ToString());
                BotTurn();
                //CollectChips(CheckWin(playerHand), playerHuman);
            }
        }
        private void PlayerDouble()
        {
            if (playerHand != null && playerHuman.Bet != 0)
            {
                int bet = playerHuman.Bet;

                PlayerHit();
                playerHuman.MakeBet(bet);
                PlayerStand();
            }
        }

        // general methods
        /// <summary>
        /// 
        /// </summary>
        private void DealHands()
        {
            if (playerHuman.Bet > 0)
            {
                lblDeal.Text = "Press R to deal";
                playerHand = new Hand();
                dealerHand = new Hand();
                botHand = new Hand();

                if (lblHand.Visibility == Visibility.Hidden ||
                    lblHandBot.Visibility == Visibility.Hidden ||
                    lblHandDealer.Visibility == Visibility.Hidden)
                {
                    lblHand.Visibility = Visibility.Visible;
                    lblHandBot.Visibility = Visibility.Visible;
                    lblHandDealer.Visibility = Visibility.Visible;
                }

                for (int i = 0; i < 2; i++)
                {
                    botHand.AddCard(mainDeck.Draw());
                    playerHand.AddCard(mainDeck.Draw());
                    dealerHand.AddCard(mainDeck.Draw());

                    lblHand.Content = PlayerDisplay();
                    lblHandBot.Content = CountHand(botHand).ToString();
                    lblHandDealer.Content = dealerHand.ConvertFaceValue(dealerHand.Cards[0]);

                    if (dealerHand.Cards[0].FaceValue == Enums.FaceValue.ace)
                        lblHandDealer.Content += "/11";
                }

                imgPlayerCard3.Source = null;
                imgPlayerCard4.Source = null;
                imgPlayerCard5.Source = null;
                imgPlayerCard6.Source = null;

                imgBotCard3.Source = null;
                imgBotCard4.Source = null;
                imgBotCard5.Source = null;

                imgDealerCard3.Source = null;
                imgDealerCard4.Source = null;
                imgDealerCard5.Source = null;

                ConvertCards(playerHand, Enums.Participant.Player);
                ConvertCards(botHand, Enums.Participant.Bot);
                ConvertCards(dealerHand, Enums.Participant.Dealer);
                ShowCardBack(imgDealerHand, 1);

                if (playerHand.HasBlackJack())
                {
                    BotTurn();
                    CollectChips(CheckWin(playerHand), playerHuman);
                }
            }
            else
                MessageBox.Show("Please place your bet");
        }

        private int CountHand(Hand hand)
        {
            if (hand != null)
                return hand.GetBestHand();
            return 0;
        }

        /// <summary>
        /// Dealers must hit until hand count is at least 17
        /// </summary>
        private void DealerTurn()
        {
            while (CountHand(dealerHand) < 17)
            {
                dealerHand.AddCard(mainDeck.Draw());
            }
            lblHandDealer.Content = CountHand(dealerHand).ToString();
            ConvertCards(dealerHand, Enums.Participant.Dealer);
            CollectChips(CheckWin(playerHand), playerHuman);
            CollectChips(CheckWin(botHand), playerBot);
        }

        /// <summary>
        /// TODO: currently follows dealer rules
        /// </summary>
        private void BotTurn()
        {
            if (botHand.HasBlackJack())
            {
                DealerTurn();
                CollectChips(CheckWin(botHand), playerBot);
            }

            concreteBot.DealerCard = dealerHand.Cards[0].FaceValue;
            string action;
            bool playing = true;

            while (playing)
            {
                action = concreteBot.BotPlay(botHand);
                switch (action)
                {
                    case "h":
                        botHand.AddCard(mainDeck.Draw());
                        break;
                    case "d":
                        if (botHand != null && playerBot.Bet != 0)
                        {
                            int bet = playerBot.Bet;

                            botHand.AddCard(mainDeck.Draw());
                            playerBot.MakeBet(bet);
                            playing = false;
                            DealerTurn();
                        }
                        break;
                    default:
                        playing = false;
                        //CollectChips(CheckWin(botHand), playerBot);
                        break;
                }
                lblHandBot.Content = CountHand(botHand).ToString();
                ConvertCards(botHand, Enums.Participant.Bot);
            }
            DealerTurn();
        }

        private List<string> ConvertCards(Hand hand, Enums.Participant turn)
        {
            List<string> images = new List<string>();
            string name;
            foreach (Card card in hand.Cards)
            {
                name = card.Suit.ToString().Substring(0, 2) + (int)card.FaceValue;
                images.Add("pack://application:,,,/CST407FinalBlackJack;component/Resources/deck/" + name.ToLower() + ".png");
            }

            switch (turn)
            {
                case Enums.Participant.Player:
                    for (int i = 0; i < images.Count; i++)
                    {
                        if (images.Count < Constants.HAND_SIZE)
                            imgPlayerHand[i].Source = new ImageSourceConverter().ConvertFromString(images[i]) as ImageSource;
                    }
                    break;
                case Enums.Participant.Bot:
                    for (int i = 0; i < images.Count; i++)
                    {
                        if (images.Count < Constants.HAND_SIZE)
                            imgBotHand[i].Source = new ImageSourceConverter().ConvertFromString(images[i]) as ImageSource;
                    }
                    break;
                case Enums.Participant.Dealer:
                    for (int i = 0; i < images.Count; i++)
                    {
                        if (images.Count < Constants.HAND_SIZE)
                            imgDealerHand[i].Source = new ImageSourceConverter().ConvertFromString(images[i]) as ImageSource;
                    }
                    break;
            }

            return images;
        }
        /// <summary>
        /// Shows the card back for the dealer
        /// </summary>
        /// <param name="cardImages"></param>
        /// <param name="index"></param>
        private void ShowCardBack(Image[] cardImages, int index)
        {
            string uriPack = "pack://application:,,,/CST407FinalBlackJack;component/Resources/deck/cardback.png";
            if (index < Constants.HAND_SIZE)
                cardImages[index].Source = new ImageSourceConverter().ConvertFromString(uriPack) as ImageSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hand">player hand to be calculated</param>
        /// <returns></returns>
        private Enums.EndResult CheckWin(Hand hand)
        {
            Enums.EndResult result = Enums.EndResult.Waiting;
            int playerHandValue = CountHand(hand);
            int dealerHandValue = CountHand(dealerHand);

            if (hand.HasBlackJack())
            {
                if (dealerHand.HasBlackJack())
                    result = Enums.EndResult.Push;
                else
                    result = Enums.EndResult.PlayerBlackJack;
            }

            else if (dealerHand.HasBlackJack())
            {
                result = Enums.EndResult.DealerBlackJack;
            }
            else if (playerHandValue == dealerHandValue)
            {
                result = Enums.EndResult.Push;
            }

            else if (dealerHandValue <= 21 && playerHandValue < dealerHandValue)
            {
                result = Enums.EndResult.DealerWin;
            }

            else if (playerHandValue > 21)
            {
                result = Enums.EndResult.PlayerBust;
            }

            else if (playerHandValue <= 21 && playerHandValue > dealerHandValue)
            {
                result = Enums.EndResult.PlayerWin;
            }

            else if (dealerHandValue > 21)
            {
                if (playerHandValue > 21)
                    result = Enums.EndResult.Push;
                else
                    result = Enums.EndResult.DealerBust;
            }

            return result;
        }

        private void CollectChips(Enums.EndResult er, Player player)
        {
            switch (er)
            {
                case Enums.EndResult.Push:
                    player.Balance += player.Bet;
                    player.Bet = 0;
                    break;
                case Enums.EndResult.DealerBlackJack:
                case Enums.EndResult.DealerWin:
                case Enums.EndResult.PlayerBust:
                    player.Bet = 0;
                    break;
                case Enums.EndResult.DealerBust:
                case Enums.EndResult.PlayerWin:
                    player.Balance += player.Bet * 2;
                    player.Bet = 0;
                    break;
                case Enums.EndResult.PlayerBlackJack:
                    player.Balance += player.Bet * 2 + (int)Math.Ceiling(player.Bet * .5);
                    player.Bet = 0;
                    break;
                default:
                    break;
            }
            lblPlayerBet.Content = playerHuman.Bet.ToString();
            lblPlayerBalance.Content = playerHuman.Balance.ToString();

            lblBotBet.Content = playerBot.Bet.ToString();
            lblBotBalance.Content = playerBot.Balance.ToString();

            if (player.Balance == 0)
                MessageBox.Show("GAME OVER");
        }
    }
}
