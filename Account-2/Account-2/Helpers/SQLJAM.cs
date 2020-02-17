using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
namespace Account_2.Helpers
{
    public class SQLJAM
    {
        public static SqlConnection myDB = new SqlConnection();
        public static string dbEndPoint= "Data Source=bassistdb.c36wb1cgcmyu.us-east-1.rds.amazonaws.com;MultipleActiveResultSets=true;Initial Catalog=GG;Persist Security Info=True;User ID=Winston;Password=Oipoliceench00";

        public static void Initialise()
        {
            if(myDB.State!=ConnectionState.Open)
            {
                myDB.ConnectionString = dbEndPoint;
                myDB.Open();
            }
        }

        public static bool InsertIntoTable(string tableName,string id,string obj)
        {
            string sql = String.Format("INSERT INTO {0} (id,object) VALUES ('{1}','{2}')",tableName,id, obj);
            SqlCommand cmd = new SqlCommand(sql,myDB);
            try
            {
                int c = cmd.ExecuteNonQuery();
                if (c>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string SelectFromTable(string tableName,string id)
        {
            string sql = String.Format("SELECT object FROM {0} WHERE id ='{1}'",tableName,id);
            SqlCommand cmd = new SqlCommand(sql,myDB);

            SqlDataReader reader = cmd.ExecuteReader();
            int c = reader.FieldCount;

            if(c>0)
            {
                reader.Read();
                return reader[0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static bool UpdateTable(string tableName,string id,string obj)
        {
            string sql = String.Format("Update {0} SET object = '{1}' WHERE id = '{2}'",tableName,obj,id);
            SqlCommand cmd = new SqlCommand(sql,myDB);
            int c = cmd.ExecuteNonQuery();
            try
            {
                if(c>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}