using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace QRHere
{
    public interface IDatabase­Connection
    {
        SQLite.SQLiteConnection DbConnection();
    }
}