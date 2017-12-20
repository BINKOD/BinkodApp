using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinkodApp.Core
{
    public class GetDT
    {
        private SqlConnection Conn;

        public DataTable GetDatatable(string TableName, string WhereClause = "")
        {
            DataTable _dt = new DataTable();
            try
            {
                string SQL = $"Select * from {TableName} {(string.IsNullOrEmpty(WhereClause) ? "" : " Where " + WhereClause)}";
                SqlDataAdapter _adp = new SqlDataAdapter(SQL, Conn);
                _adp.Fill(_dt);
            }
            catch (Exception ex) { }
            return _dt;
        }

        public DataTable GetDatatable(string SQL)
        {
            DataTable _dt = new DataTable();
            try
            {
                SqlDataAdapter _adp = new SqlDataAdapter(SQL, Conn);
                _adp.Fill(_dt);
            }
            catch (Exception ex) { }
            return _dt;
        }
    }
}
