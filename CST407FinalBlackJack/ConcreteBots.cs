using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

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

    class ConcreteAssistedBot : AbstractBot
    {
        private int _dealerCard;
        public int DealerCard
        {
            get { return _dealerCard; }
            set { _dealerCard = value; }
        }

        public ConcreteAssistedBot()
        {
            _dealerCard = 0;
        }

        public override bool BotPlay(Hand hand)
        {
            return false;
        }

        public void RetrieveChart()
        {
            //string connectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = @\"..\\..\\Resources\\Blackjack Betting System.xlsx; Extended Properties = \"Excel 12.0 Xml;HDR=YES\";";

            //DataTable sheetData = new DataTable();
            //using (OleDbConnection conn = new OleDbConnection(connectionString))
            //{
            //    conn.Open();
            //    // retrieve the data using data adapter
            //    OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("select * from [Sheet1]", conn);
            //    sheetAdapter.Fill(sheetData);
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
