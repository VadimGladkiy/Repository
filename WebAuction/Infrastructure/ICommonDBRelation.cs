using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAuction.Infrastructure
{
    interface ICommonDBRelation<U,T>
    {
         // check a string line in a DataBase 
         void UpdateItemsColumnInDB(string findDefinedColumn,
             U changeColumn, string value);
         // add item to DB
         void addItemToDB(T _newItem);
        // delete item from DB
        void deleteItemFromDB(int _item_id);
        // get item from DB by item.ID
        List<T> getItemFromDB_byParams(U column, params string[] values);
    }
}
