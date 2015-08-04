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
        Deck deck = new Deck();
        Hand playerHand;
        Hand dealerHand;
        Hand botHand;
        Image[] imgPlayerHand = new Image[6];
        Image[] imgBotHand = new Image[6];
        Image[] imgDealerHand = new Image[6];
        public MainWindow()
        {
            InitializeComponent();
            lblDeal.Text = "Please make your bet and press R to deal";
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

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    break;
                case Key.A:
                    PlayerHit();
                    break;
                case Key.S:
                    PlayerStand();
                    break;
                case Key.D:
                    break;
                case Key.F:
                    break;
            }


            if (e.Key == Key.R)
            {
                DealHands();
            }
        }

        /// <summary>
        /// TODO: add to betting balance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BetChip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label chip = sender as Label;

            switch (chip.Name)
            {
                case "BetChip1":
                    MessageBox.Show("You bet 1");
                    break;
                case "BetChip5":
                    MessageBox.Show("You bet 5");
                    break;
                case "BetChip25":
                    MessageBox.Show("You bet 25");
                    break;
                case "BetChip100":
                    MessageBox.Show("You bet 100");
                    break;
                case "BetChip500":
                    MessageBox.Show("You bet 500");
                    break;
            }
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
            if (playerHand != null)
            {
                playerHand.AddCard(deck.Draw());
                lblHand.Content = PlayerDisplay();
                ConvertCards(playerHand, Enums.Participant.Player);
            }
        }
        private void PlayerStand()
        {
            BotTurn();
        }
        private void PlayerDouble()
        {

        }

        // general methods
        /// <summary>
        /// TODO: update cards on table
        /// </summary>
        private void DealHands()
        {
            lblDeal.Text = "Press R to deal";
            playerHand = new Hand();
            dealerHand = new Hand();
            botHand = new Hand();

            for (int i = 0; i < 2; i++)
            {
                botHand.AddCard(deck.Draw());
                playerHand.AddCard(deck.Draw());
                dealerHand.AddCard(deck.Draw());

                lblHand.Content = PlayerDisplay();
                lblHandBot.Content = CountHand(botHand).ToString();
                lblHandDealer.Content = CountHand(dealerHand).ToString();
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
        }
        private int CountHand(Hand hand)
        {
            return hand.GetBestHand();
        }

        /// <summary>
        /// Dealers must hit until hand count is at least 17
        /// </summary>
        private void DealerTurn()
        {
            while (CountHand(dealerHand) < 17)
            {
                dealerHand.AddCard(deck.Draw());
                lblHandDealer.Content = CountHand(dealerHand).ToString();
                ConvertCards(dealerHand, Enums.Participant.Dealer);
            }
        }

        /// <summary>
        /// TODO: currently follows dealer rules
        /// </summary>
        private void BotTurn()
        {
            while (CountHand(botHand) < 17)
            {
                botHand.AddCard(deck.Draw());
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
    }
}
