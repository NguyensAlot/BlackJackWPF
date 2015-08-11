using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace CST407FinalBlackJack
{
    /// <summary>
    /// class for knowledge base AI that bets according to chart
    /// </summary>
    class KnowledgeBaseAI
    {
        #region Fields and Properties
        // card ID for revealed dealer card
        private Enums.FaceValue _dealerCard;
        // data set for betting chart
        private DataSet _ds;

        public Enums.FaceValue DealerCard
        {
            get { return _dealerCard; }
            set { _dealerCard = value; }
        }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public KnowledgeBaseAI()
        {
            _dealerCard = 0;
            _ds = RetrieveChart();
        }

        /// <summary>
        /// Evaluate the table and plays accordingly to hand, dealer card, and betting chart
        /// </summary>
        /// <param name="hand">hand to be played</param>
        /// <returns>best action in form of string</returns>
        public string AIPlay(Hand hand)
        {
            // used for finding correct row in chart
            string botHandAsID = "";
            // used to convert enum to int value (ie. ace to 14) then to string
            int cardID;

            // check for ace only if 2 cards in hand
            if (hand.ContainsCard(Enums.FaceValue.ace) && hand.Cards.Count == Constants.BLACKJACK_CARD_AMT)
            {
                // set card ID
                cardID = (int)Enums.FaceValue.ace;
                botHandAsID = cardID.ToString();

                // get the 2nd card ID and append to string
                foreach (Card card in hand.Cards)
                {
                    switch (card.FaceValue)
                    {
                        // 8 or higher will result in the same action
                        case Enums.FaceValue.eight:
                        case Enums.FaceValue.nine:
                        case Enums.FaceValue.ten:
                        case Enums.FaceValue.jack:
                        case Enums.FaceValue.queen:
                        case Enums.FaceValue.king:
                            botHandAsID += "8";
                            break;
                        // do nothing
                        case Enums.FaceValue.ace:
                            break;
                        // for low card numbers, give the card value
                        default:
                            cardID = (int)card.FaceValue;
                            botHandAsID += cardID.ToString();
                            break;
                    }
                }
            }
            // if no ace, get the best hand
            else botHandAsID = hand.GetBestHand().ToString();

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
                            // return best action according to chart
                            return _ds.Tables[0].Rows[i][j].ToString();
                        }
                    }
                }
            }
            return "s";
        }

        /// <summary>
        /// Open the betting chart excel file through OleDB and store the values into a data set and return it
        /// </summary>
        /// <returns>data set of betting chart</returns>
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
            // create the connection string
            connectionString = sb.ToString();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                // retrieve the data using data adapter with select all command
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
}
