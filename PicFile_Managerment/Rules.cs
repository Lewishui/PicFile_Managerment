using System;
using System.Data;
using System.Diagnostics;


namespace PicFile_Managerment
{
	/// <summary>
	/// Summary description for Rules.
	/// </summary>
	public class Rules
	{
		public Rules()
		{
			//
			// TODO: Add constructor logic here
			//
		}


        #region Get Hierarchy
        public static DataSet GetHierarchy(string connectionString,int modelID)
        {
             string sql = "";
             DataSet ds = null;
             
             try
             {

               sql = "select * from hierarchy ";
               sql += "  where modelid=" + modelID.ToString();
               sql += "  order by parentnodeid,sortorder asc";

               ds = DataAccess.GetDataSet(connectionString,sql);

               SetHierarchyRelationships(ds);

             }
             catch (Exception) { throw; }
             return ds;
        }
        #endregion

        #region Set Hierarchy Relationships
        public static void SetHierarchyRelationships(DataSet ds)
        {
 
            DataColumn fk;
            DataColumn[] pk = new DataColumn[1];
            ForeignKeyConstraint fkcdelete;
            DataRelation relation;
            string datarelationname = "ParentChild";

             try
             {

                ds.Tables[0].TableName = "Hierarchy";

                pk[0] = ds.Tables[0].Columns["NodeID"];
            
                fk = ds.Tables[0].Columns["ParentNodeID"];

                ds.Tables[0].PrimaryKey = pk;

           	    fkcdelete = new ForeignKeyConstraint(pk[0],fk);
 
			    fkcdelete.DeleteRule = Rule.Cascade; 

		        relation = new DataRelation(datarelationname,pk[0],fk,false);

              ds.Tables[0].Constraints.Add(fkcdelete);
              ds.Relations.Add("ParentChild", ds.Tables[0].Columns["NodeID"], ds.Tables[0].Columns["ParentNodeID"], false);  
			    ds.Tables[0].AcceptChanges();

			    ds.Relations.Add(relation);  

             }
             catch (Exception) { throw; }
             return;
        }
        #endregion

        #region Commit Hierarchy
        public static bool CommitHierarchy(string connectionString,DataSet ds)
        {

           string sql = "";
           bool ret = false;
           int ModelID = 0;
           DataTable dt;

           try
           {

             if (ds.Tables.Count < 1) { return ret; }
             
             dt = ds.Tables[0];

             ModelID = int.Parse(dt.Rows[0]["ModelID"].ToString());

             sql = "select  * from Hierarchy where ModelID = " + ModelID.ToString();

             for(int i=0;i<dt.Rows.Count;i++)
             {

               if (dt.Rows[i].RowState.ToString() == "Deleted") { continue; }

               if (int.Parse(dt.Rows[i]["ModelID"].ToString()) != ModelID)
               {
                 dt.Rows[i]["ModelID"] = ModelID;
               }

             }

             DataAccess.CommitToDataBase(connectionString,ds,sql);

             ret = true;

           }
           catch (Exception e) 
           {
               Debug.WriteLine(e.Message);
               throw;
           }
           return ret;
        }
        #endregion

    }
}
