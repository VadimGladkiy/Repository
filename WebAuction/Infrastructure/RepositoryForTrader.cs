using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using WebAuction.Models;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WebAuction.Infrastructure
{
    public enum ChoiseTraderColumn
    {
        ID,
        Name,
        Email,
        EMailAndPassword,
        Present,
        Money
    }
    public enum ChoiseItemColumn
    {
        Name,
        Descript,
        Price,
        Image
    }
    public class RepositoryForTrader 
        : ICommonDBRelation<ChoiseTraderColumn, Models.Trader>
    {   
        private SqlConnection itemConnection;
        
        public RepositoryForTrader()
        {
            
        }
        // initialisation of connection
        public void initialDBConnect(string connectionSTR)
        {
            itemConnection = new SqlConnection();
            itemConnection.ConnectionString = connectionSTR;
            itemConnection.Open();
        }
        // close connection
        public void closeDBconnect()
        {
            itemConnection.Close();
        }
        // add item to DB
        public void addItemToDB(WebAuction.Models.Trader objToDB)
        { 
            string str =
            "INSERT INTO Traders Values('"+ // type of SQL query
            objToDB.trader_fullName+"','"+  // for a new name
            objToDB.trader_eMail+"','"+     // for a new mail
            objToDB.trader_password+"',"+   // for a his password
            "'0','0');";                    // a start balance & on-line/off-line

            using (SqlCommand insertRow = new SqlCommand(str, itemConnection))
            {
                insertRow.ExecuteNonQuery();
            }
            
        }
        // check a new line on equal with a line in DB
        public void UpdateItemsColumnInDB(string findDefinedColumn,
            ChoiseTraderColumn changeСolumn, string value)
        {
                string getItemExpression = null;
                switch (changeСolumn)
                {
                    case ChoiseTraderColumn.Present:
                        getItemExpression = string.Format(
                        "Update Traders set trader_IsReady ='{1}' " +
                        "where trader_IsReady = '{0}' ",
                        findDefinedColumn, value);
                        break;
                    case ChoiseTraderColumn.Money:
                        getItemExpression = string.Format(
                        "Update Traders set trader_score ='{1}' " +
                        "where trader_score = '{0}' ",
                        findDefinedColumn, value);
                        break;
                    default:
                        ;
                        break;
                }
            // create SQL command

            using (SqlCommand updateComm =
                new SqlCommand(getItemExpression, itemConnection))
            {
                updateComm.ExecuteNonQuery();
            }
        }
        public void deleteItemFromDB(int _item_id)
        {
            string delExpression =
            "delete drom Traders WHERE trader_ID = " +_item_id+";";
            // create SQL command
            using (SqlCommand delComm =
            new SqlCommand(delExpression, itemConnection))
            {
                // Execute SQL query
                delComm.ExecuteNonQuery();
            }
        }

        public List<Trader> getItemFromDB_byParams(ChoiseTraderColumn column,
            params string[] values)
        {
            List<Trader> outputTraders = new List<Trader>();
            Trader _referenceForTrader;
        string getItemExpression=null;
            switch (column)
            {
                case ChoiseTraderColumn.ID :
                    getItemExpression=
                    string.Format("select* from Traders WHERE trader_ID = {0} "
                    , Int32.Parse(values[0]));
                    break;
                case ChoiseTraderColumn.Name :
                    getItemExpression =
                    string.Format("select* from Traders WHERE trader_fullName = '{0}' "
                    , values[0]);
                    break;
                case ChoiseTraderColumn.Email :
                    getItemExpression =
                    string.Format("select* from Traders WHERE trader_eMail = '{0}' "
                    , values[0]);
                    break;
                case ChoiseTraderColumn.EMailAndPassword :
                    getItemExpression =
                    string.Format("select* from Traders WHERE trader_eMail = '{0}' "+
                    "And trader_password = '{1}' ",values[0],values[1] );
                    break;
                default : 
                    break;
            }
            string[] TraderInfo = new string[6];

            // create SQL command
            using (SqlCommand selectComm =
            new SqlCommand(getItemExpression, itemConnection)) { 
                // Execute SQL query
                SqlDataReader myDataReader = selectComm.ExecuteReader();
                    // organize a cicle by results
                    while (myDataReader.Read())
                    {
                        TraderInfo[0] = myDataReader["trader_ID"].ToString();
                        TraderInfo[1] = myDataReader["trader_fullName"].ToString();
                        TraderInfo[2] = myDataReader["trader_eMail"].ToString();
                        TraderInfo[3] = myDataReader["trader_password"].ToString();
                        TraderInfo[4] = myDataReader["trader_score"].ToString();
                        TraderInfo[5] = myDataReader["trader_IsReady"].ToString();
                    
                    // create a trader
                    _referenceForTrader = new Trader
                    {
                        trader_ID = Int32.Parse(TraderInfo[0]),
                        trader_fullName = TraderInfo[1],
                        trader_eMail = TraderInfo[2],
                        trader_password = TraderInfo[3],
                        trader_score = double.Parse(TraderInfo[4]),
                        trader_isReady = bool.Parse(TraderInfo[5])
                    };
                    // add a founding trader to list
                    outputTraders.Add(_referenceForTrader);
                }
                myDataReader.Close();
                } 
                 
            // return
            return outputTraders;
        }
        public List<Models.Item> getAllItemsFromDB(string usr_id = null,
            string category = null)
        {
            string and = "";
            string base_expression = "select * from Items ";
            string id_expression = null;
            string categ_expression = null;
            List<Models.Item> itemsList = new List<Item>();
            // chose a case of output

            if (usr_id != null && category != null) and += " AND ";
            if (usr_id == null)
            {
                id_expression = string.Format("");
            } else
            { id_expression = string.Format("where item_lord = {0}", usr_id); }

            //
            if (category == null)
            {
                categ_expression = string.Format("");
            } else
            if(category == "sport")
            {
                categ_expression = "where item_category = 'sport' ";    
            }else
            if (category == "study")
            {
                categ_expression = "where item_category = 'study' ";
            }
            else
            if (category == "cuisine")
            {
                categ_expression = "where item_category = 'cuisine' ";
            }
            else
            if (category == "house")
            {
                categ_expression = "where item_category = 'house' ";
            }

            base_expression +=( id_expression + and + categ_expression);
                // 
                using (SqlCommand getAllItems =
            new SqlCommand(base_expression, itemConnection))
            {
                SqlDataReader myDataReader = getAllItems.ExecuteReader();
                //
                while (myDataReader.Read())
                {
                    itemsList.Add(new Item
                    {
                        item_ID = Int32.Parse(myDataReader["item_ID"].ToString()),
                        item_name = myDataReader["item_name"].ToString(),
                        item_description = myDataReader["item_description"].ToString(),
                        item_price = double.Parse(myDataReader["item_price"].ToString()),
                        item_offtime = myDataReader["item_offtime"].ToString(),
                        item_category = myDataReader["item_category"].ToString(),
                        item_pathToPict = myDataReader["item_pathToPict"].ToString(),
                        item_lord = Int32.Parse(myDataReader["item_lord"].ToString())
                    });
                }
                myDataReader.Close();
                // next
            }
            return itemsList;    
        }
        public void pushItemtoDb(Models.Item item, int lord_id)
        {
            string sqlExpression;
            //
            sqlExpression = string.Format("INSERT INTO Items VALUES"+
                "('{0}','{1}',{2},'{3}','{4}','{5}',{6})",
                item.item_name,item.item_description,item.item_price,item.item_offtime,
                item.item_category,item.item_pathToPict, lord_id);

            using (SqlCommand pushComm =
            new SqlCommand(sqlExpression, itemConnection))
            {
                try
                {
                    pushComm.ExecuteNonQuery();
                }
                catch(Exception e) { MessageBox.Show(e.Message); }
            }
        }
        public void changeItemsName(string changedName, int Id)
        {
                string sqlExpress = null;
                sqlExpress = string.Format("update Items set item_name = '{0}' " +
                                           "where item_ID = '{1}' ", changedName, Id);
                using (SqlCommand updateItNComm =
                new SqlCommand(sqlExpress, itemConnection))
                {
                    try
                    {
                        updateItNComm.ExecuteNonQuery();
                    }
                    catch (Exception e) { MessageBox.Show(e.Message); }
                }
           
        }
    }
}