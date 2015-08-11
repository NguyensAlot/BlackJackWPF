using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace CST407FinalBlackJack
{
    class ConcreteDealerBot : AbstractBot
    {
        public override bool BotPlay(Hand hand)
        {
            while (hand.GetBestHand() < 17)
            {
                return true;
            }
            return false;
        }
    }

    class ConcreteSmarterBot : AbstractBot
    {
        private Enums.FaceValue _dealerCard;
        public Enums.FaceValue DealerCard
        {
            get { return _dealerCard; }
            set { _dealerCard = value; }
        }

        public ConcreteSmarterBot()
        {
            _dealerCard = 0;
        }

        public override bool BotPlay(Hand hand)
        {
            int botHandValue = hand.GetBestHand();
            bool playOn = false;

            if (botHandValue <= 11)
                playOn = true;

            else if (botHandValue >= 12 && botHandValue <= 16)
            {
                switch (_dealerCard)
                {
                    case Enums.FaceValue.two:
                    case Enums.FaceValue.three:
                    case Enums.FaceValue.four:
                    case Enums.FaceValue.five:
                    case Enums.FaceValue.six:
                        playOn = false;
                        break;
                    default:
                        playOn = true;
                        break;
                }
            }
            //else if (botHandValue >= 17 && botHandValue <= 21)
            //    playOn = false;
            return playOn;
        }
    }

    class ConcreteAssistedBot
    {
        private Enums.FaceValue _dealerCard;
        private DataSet _ds;

        public Enums.FaceValue DealerCard
        {
            get { return _dealerCard; }
            set { _dealerCard = value; }
        }

        public ConcreteAssistedBot()
        {
            _dealerCard = 0;
            _ds = RetrieveChart();
        }

        public string BotPlay(Hand hand)
        {
            string botHandAsID = "";
            int aceID = (int)Enums.FaceValue.ace;
            int cardID;

            // check for ace
            if (hand.ContainsCard(Enums.FaceValue.ace) && hand.Cards.Count == Constants.BLACKJACK_CARD_AMT)
            {
                botHandAsID = aceID.ToString();
                foreach (Card card in hand.Cards)
                {
                    switch (card.FaceValue)
                    {
                        case Enums.FaceValue.eight:
                        case Enums.FaceValue.nine:
                        case Enums.FaceValue.ten:
                        case Enums.FaceValue.jack:
                        case Enums.FaceValue.queen:
                        case Enums.FaceValue.king:
                            botHandAsID += "8";
                            break;
                        case Enums.FaceValue.ace:
                            break;
                        default:
                            cardID = (int)card.FaceValue;
                            botHandAsID += cardID.ToString();
                            break;
                    }
                }
                System.Windows.MessageBox.Show(botHandAsID);
            }
            else botHandAsID = hand.GetBestHand().ToString();
            //System.Windows.MessageBox.Show(botHandAsID);
            cardID = (int)_dealerCard;
            // iterate through rows to find matching hand
            for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
            {
                if (botHandAsID == _ds.Tables[0].Rows[i][0].ToString())
                {
                    // iterate through columns to find dealer's revealed card
                    for (int j = 0; j < _ds.Tables[0].Columns.Count; j++)
                    {
                        if (cardID.ToString() == _ds.Tables[0].Rows[0][j].ToString())
                        {
                            return _ds.Tables[0].Rows[i][j].ToString();
                        }
                    }
                }
            }
            return "s";
        }


        public DataSet RetrieveChart()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            string connectionString = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            // adding single quotes sovled ISAM problem 
            props["Extended Properties"] = "\'Excel 12.0 XML;HDR=NO;\'";
            props["Data Source"] = "../../Resources/Blackjack Betting System.xlsx";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }
            connectionString = sb.ToString();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                // retrieve the data using data adapter
                OleDbDataAdapter da = new OleDbDataAdapter("select * from [Sheet1$]", conn);
                // fill the data adapter
                da.Fill(dt);
                // add the table to the data set
                ds.Tables.Add(dt);
                conn.Close();
            }
            return ds;
        }
    }


    class ConcreteCountingBot : AbstractBot
    {
        private int _count;

        public ConcreteCountingBot()
        {
            _count = 0;
        }

        public override bool BotPlay(Hand hand)
        {
            return false;
        }

        public int CountCards(Hand hand, Enums.Participant turn)
        {

            for (int i = 0; i < hand.Cards.Count; i++)
            {
                if (turn == Enums.Participant.Dealer)
                    i = 0;

                switch (hand.Cards[i].FaceValue)
                {
                    case Enums.FaceValue.two:
                    case Enums.FaceValue.three:
                    case Enums.FaceValue.four:
                    case Enums.FaceValue.five:
                    case Enums.FaceValue.six:
                        _count += 1;
                        break;
                    case Enums.FaceValue.seven:
                    case Enums.FaceValue.eight:
                    case Enums.FaceValue.nine:
                        break;
                    case Enums.FaceValue.ten:
                    case Enums.FaceValue.jack:
                    case Enums.FaceValue.queen:
                    case Enums.FaceValue.king:
                    case Enums.FaceValue.ace:
                        _count -= 1;
                        break;
                }
            }

            return _count;
        }
    }


    class ConcreteOscarBot : AbstractBot
    {
        private int _initialBet;
        private int _winCount;
        private double _betMultiplier;
        private Enums.EndResult _result;

        public ConcreteOscarBot(Player bot)
        {
            _initialBet = 100;
            _winCount = 0;
            _betMultiplier = 1;
            _result = Enums.EndResult.Waiting;
        }

        public override bool BotPlay(Hand hand)
        {
            base.BotPlay(hand);
            return false;
        }
    }
}
