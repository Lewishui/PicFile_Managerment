using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections;
using clsCommon;
using System.Data.SqlClient;
using FA.DB;

namespace PicFile_Managerment
{

    public class DataAccess
    {
        public string ConStr;
        public DataAccess()
        {
            ConStr = SQLHelper.ConStr_sql;

        }


        #region Commit To Database
        public static void CommitToDataBase(string connectionString, DataSet ds, string sql)
        {

            try
            {

                if (!ds.HasChanges()) { return; }

                using (OleDbConnection conn = new OleDbConnection())
                {

                    conn.ConnectionString = connectionString;
                    conn.Open();

                    using (OleDbDataAdapter da = new OleDbDataAdapter(sql, conn))
                    {
                        OleDbCommandBuilder b = new OleDbCommandBuilder(da);
                        da.Update(ds.Tables[0]);
                        ds.AcceptChanges();
                    }

                }

            }
            catch (Exception) { throw; }

        }
        #endregion

        #region Get Data Set
        //public static DataSet GetDataSet(string connectionString,string sql)
        //{


        //   DataSet ds = new DataSet();

        //   try
        //   {

        //      using (OleDbConnection conn = new OleDbConnection())
        //      {

        //          conn.ConnectionString = connectionString;
        //        conn.Open(); 

        //        using(OleDbDataAdapter da = new OleDbDataAdapter(sql,conn))
        //        {

        //            da.Fill(ds);  

        //        }

        //      }
        //    }
        //    catch (Exception) { throw; }
        //    return ds;
        //}
        public static DataSet GetDataSet(string connectionString, string sql)
        {


            DataSet ds = new DataSet();

            try
            {
                string ConStr_sql = @"Provider=SQLOLEDB;server=bds28428944.my3w.com,1433;uid=bds28428944;pwd=Lyh079101;database=bds28428944_db"; //本地自己的数据库

                using (OleDbConnection conn = new OleDbConnection())
                {

                    conn.ConnectionString = ConStr_sql;
                    conn.Open();

                    using (OleDbDataAdapter da = new OleDbDataAdapter(sql, conn))
                    {

                        da.Fill(ds);

                    }

                    using (SqlDataReader reader = SQLHelper.ExecuteReader(SQLHelper.CONNECTION_STRING_BASE, CommandType.Text, sql))
                    {
                        while (reader.Read())
                        {
                            clstress_info tempnote = new clstress_info();
                            if (reader["ModelID"].ToString() != "")
                                tempnote.ModelID = reader["ModelID"].ToString();

                            if (reader["NodeID"].ToString() != "")
                                tempnote.NodeID = reader["NodeID"].ToString();
                  
                        }
                    }

                }
            }
            catch (Exception) { throw; }
            return ds;
        }
        #endregion

    }
}
