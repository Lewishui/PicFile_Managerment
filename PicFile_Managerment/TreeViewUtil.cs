using System;
using System.Drawing;
using System.Xml;
using System.Diagnostics; 
using System.Windows.Forms;
using System.Data;

namespace PicFile_Managerment
{
 
	public class TreeViewUtil
	{
		 
		private const string NodeObjectName = "System.Windows.Forms.TreeNode";
		private const string CheckedColumnName = "Checked";
		private const string ForeColorColumnName = "ForeColor";
		private const string BackColorColumnName = "BackColor";
		private const string ImageIndexColumnName = "ImageIndex";
		private const string SelectedImageIndexColumnName = "SelectedImageIndex";
        private const string SortOrderColumnName = "SortOrder";

        #region Check All Child Nodes
        public static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
			
           foreach(TreeNode node in treeNode.Nodes)
           {

              node.Checked = nodeChecked;

              if(node.Nodes.Count > 0)
              {
                 CheckAllChildNodes(node, nodeChecked);
              }
              if (!node.Checked)
              {
                  node.Collapse();
              }
              else
              {
                  node.ExpandAll();
              }
           }
        }
        #endregion

        #region Collapse Tree Branch
        public static void CollapseTreeBranch(TreeNode parentNode)
        {
			 
           try
           {
			 
                if (parentNode.Nodes.Count < 1) { return; }  
								
                for(int i=0;i<parentNode.Nodes.Count;i++)
                {
                    parentNode.Nodes[i].Collapse();
                }
				 
	       }
           catch (Exception) { throw; }
 
        }
        #endregion

        #region Show Context Menu
        public static void ShowContextMenu(TreeView tv,ContextMenu menu)
        {
            try
            {
              Point pt = new Point(tv.SelectedNode.Bounds.Left,
                                   tv.SelectedNode.Bounds.Bottom);

               menu.Show(tv,pt);
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Load From Data Set
        public static void LoadFromDataSet(TreeView tv,DataSet ds,string textColumnName)
        {
 
          TreeNode node;
          DataRow row;

          try
          {
			 
            tv.Nodes.Clear();
			 	
			if (ds.Tables.Count < 1) { return; }
            
			if (ds.Tables[0].Rows.Count < 1) { return; } 

            string fkcolumnname = FindForeignKeyColumnName(ds.Tables[0]);

            row = ds.Tables[0].Rows[0]; 

			tv.BeginUpdate();  

		    node = GetTreeNodeFromDataRow(row,textColumnName);

            tv.Nodes.Add(node);

            AddNodeFromDataRow(node,row,textColumnName,FindForeignKeyRelationName(row.Table));

          }
          catch (Exception) { throw; }
          finally
          {
             tv.EndUpdate(); 
             ds.AcceptChanges(); 
          } 
        }
        #endregion

        #region Add Node From Data Row
        public static void AddNodeFromDataRow(TreeNode parentNode,DataRow row,string textColumnName,string dataRelationName)
        {
           try
           {
        
               foreach(DataRow childrow in row.GetChildRows(dataRelationName))
               {

					   parentNode.Nodes.Add(GetTreeNodeFromDataRow(childrow,textColumnName));
				 
					   if (parentNode.TreeView.CheckBoxes)
					   {
						   if (row.Table.Columns.Contains(CheckedColumnName))
						   {
							   parentNode.LastNode.Checked = (bool)row[CheckedColumnName];
						   }
					   }

					   AddNodeFromDataRow(parentNode.LastNode,childrow,textColumnName,dataRelationName);
				    
               }

           }
           catch (Exception) { throw; }
        }
        #endregion

        #region Get Tree Node From Data Row
        public static TreeNode GetTreeNodeFromDataRow(DataRow row,string textColumnName)
        {

            TreeNode child = null;
            string fcolor="";
			string bcolor="";
			string imageidx="";
			string selimageidx="";
			 

            try
            {
                              
				if (row.Table.Columns.Contains(ForeColorColumnName))
				{
                    fcolor = row[ForeColorColumnName].ToString();
				}
				if (row.Table.Columns.Contains(BackColorColumnName))
				{
					bcolor = row[BackColorColumnName].ToString();
				}
				if (row.Table.Columns.Contains(ImageIndexColumnName))
				{
					imageidx = row[ImageIndexColumnName].ToString();
				}
				if (row.Table.Columns.Contains(SelectedImageIndexColumnName))
				{
					selimageidx = row[SelectedImageIndexColumnName].ToString();
				}
 
				child = new TreeNode();
				child.Text = row[textColumnName].ToString().Trim();

				if (imageidx.Length > 0)
				{
					child.ImageIndex = Convert.ToInt32(imageidx);
				}
				if (selimageidx.Length > 0)
				{
					child.SelectedImageIndex = Convert.ToInt32(selimageidx);
				}
				if (fcolor.Length > 0) 
				{
					child.ForeColor = ColorTranslator.FromHtml(fcolor);
				}
				if (bcolor.Length > 0) 
				{
					child.BackColor = ColorTranslator.FromHtml(bcolor);
				}

				child.Tag = row;
				
			}
            catch (Exception) { throw; }
			return child;
        }
        #endregion

		#region Set Selected Node By Position
		public static void SetSelectedNodeByPosition(TreeView tv,int mouseX,int mouseY)
		{
			TreeNode node = null;

			try
			{

				Point pt = new Point(mouseX,mouseY);
          
				tv.PointToClient(pt);
          
				node = tv.GetNodeAt(pt);

				tv.SelectedNode = node;

				if (node == null) return;
          
				if (!node.Bounds.Contains(pt)) { return; }

			}
			catch { }
			return;
		}
		#endregion

		#region Delete Node
		public static void DeleteNode(TreeView tv,bool permitRootNodeDeletion)
		{
			 
            DataRow row;

			try
			{

				if (tv.SelectedNode == null) { return; }

				if (tv.SelectedNode == tv.Nodes[0]) 
				{
					if (!permitRootNodeDeletion)
					{
						return;
					}
					else
					{
                        row = (DataRow)tv.SelectedNode.Tag; 
                        row.Delete(); 
						tv.Nodes.Clear();
						return;
					}
				}
	   
                row = (DataRow)tv.SelectedNode.Tag; 
                row.Delete(); 
				tv.SelectedNode.Remove(); 

			}
			catch (Exception) { throw; }
			return;
		}
		#endregion

		#region Drag Enter
		public static void DragEnter(TreeView tv,System.Windows.Forms.DragEventArgs e)
		{

			try
			{
				if (e.Data.GetDataPresent(NodeObjectName, true))
				{
					e.Effect = DragDropEffects.Move;
					return;
				}
 
			    e.Effect = DragDropEffects.None;
			 
			}
			catch (Exception) { throw; }

		}
		#endregion

		#region Drag Over
		public static void DragOver(TreeView tv,System.Windows.Forms.DragEventArgs e)
		{

			try
			{

				if (!e.Data.GetDataPresent(NodeObjectName, true)) { return; }
 
				Point pt = tv.PointToClient(new Point(e.X,e.Y));
 
				TreeNode tgtnode = tv.GetNodeAt(pt); 
       
				if (tv.SelectedNode != tgtnode)
				{
					tv.SelectedNode = tgtnode;
          
					TreeNode drop = (TreeNode)e.Data.GetData(NodeObjectName);

					while (tgtnode != null)
					{

						if (tgtnode == drop) 
						{
							e.Effect = DragDropEffects.None;
							return;
						}

						tgtnode = tgtnode.Parent;
					}
        
				}

				e.Effect = DragDropEffects.Move;
			}
			catch (Exception) { throw; }

		}
		#endregion

		#region Drag Drop
		public static void DragDrop(TreeView tv,System.Windows.Forms.DragEventArgs e,ref bool dropOnNewControl)
		{
    
            DataRow row;
            DataRow NewParentRow;
            DataTable dt;

			try
			{

              dropOnNewControl = false;

			  if (!e.Data.GetDataPresent(NodeObjectName,true)) { return; }
        
			  TreeNode drop = (TreeNode)e.Data.GetData(NodeObjectName);

			  TreeNode tgtnode = tv.SelectedNode;

              if (drop == drop.TreeView.Nodes[0]) 
		      {
                  return;
              }

				if (drop == tgtnode)
				{
					return;
				}

              NewParentRow = (DataRow)tgtnode.Tag;
              
              dt = NewParentRow.Table;

			  row = (DataRow)drop.Tag;
               
              // Is this the same control?

              if (tgtnode.TreeView == drop.TreeView)
              {
                // If same control, just change the parent key;
                row[FindForeignKeyColumnName(dt)] = NewParentRow[dt.PrimaryKey[0].ColumnName];
              }
              else
              {
                dropOnNewControl = true;
                // If different control, we must copy the branch of DataRows
                // to the target node's DataRow DataTable.
                SetNewDataRowReferencesForNode(drop,row,NewParentRow,true);
                row.Delete(); 
              }
 
              // This removes the TreeNodes from the TreeView but
              // doesn't destroy our drop DataRows.

              drop.Remove();

			  if (tgtnode == null)
			  {
				tv.Nodes.Add(drop);
			  }
			  else
			  { 
				tgtnode.Nodes.Add(drop);
			  }
        
              ReOrderSiblings(drop);

			  drop.EnsureVisible();

			  tv.SelectedNode = drop;
              
		   } 
		   catch (Exception err) 
           {
                Debug.WriteLine(err.Message);
                throw;
           }
		}
		#endregion

        #region Set New Data Row References For Node
        private static void SetNewDataRowReferencesForNode(TreeNode node,DataRow oldRow,DataRow newParentRow,bool resetThisParentID)
        {

           DataRow NewRow = null;
           DataTable dt;
          
           try
           {

             dt = newParentRow.Table;

             NewRow = CopyRowToNewTable(oldRow,dt);
             
             if (resetThisParentID)
             {
               NewRow[FindForeignKeyColumnName(dt)] = newParentRow[dt.PrimaryKey[0].ColumnName];
             }

             dt.Rows.Add(NewRow);

             node.Tag =  NewRow;

             foreach(TreeNode childnode in node.Nodes)
             {
               SetNewDataRowReferencesForNode(childnode,(DataRow)childnode.Tag,newParentRow,false);
             }

           } 
		   catch (Exception) { throw; }

        }
        #endregion

        #region Copy Row To New Table
		private static DataRow CopyRowToNewTable(DataRow oldRow,DataTable newParentTable)
		{
    
           DataRow NewRow = null;
 
		   try
		   {
              
               NewRow = newParentTable.NewRow(); 
               NewRow.ItemArray = oldRow.ItemArray; 
           
		   } 
		   catch (Exception e) { Debug.WriteLine(e.Message); throw; }
           return NewRow;
		}
		#endregion

        #region Find Foreign Key Column Name
        public static string FindForeignKeyColumnName(DataTable dt)
        {
           string Ret = "";

           try
           {

              DataSet ds = dt.DataSet;

              DataRelation rel = ds.Relations[0];

              Ret = rel.ChildColumns[0].ColumnName; 

           }
           catch (Exception) { throw; }
           return Ret;
        }
        #endregion

        #region Find Foreign Key Relation Name
        public static string FindForeignKeyRelationName(DataTable dt)
        {
           string Ret = "";

           try
           {

              DataSet ds = dt.DataSet;

              DataRelation rel = ds.Relations[0];

              Ret = rel.RelationName; 

           }
           catch (Exception) { throw; }
           return Ret;
        }
        #endregion

        #region Nudge Up
        public static void NudgeUp(TreeNode node)
        {
           int NewIndex = 0;
           TreeNode NodeClone = null;

           try
           {
  
              if (node == null) { return; }

              if (node.Index == 0) { return; }

              NewIndex = node.Index - 1;
               
              NodeClone = (TreeNode)node.Clone();

              node.Parent.Nodes.Insert(NewIndex,NodeClone); 

              node.Parent.Nodes.Remove(node);

              ReOrderSiblings(NodeClone);

              NodeClone.TreeView.SelectedNode = NodeClone;
             
           }
           catch (Exception) { throw; }
           return;
        }
        #endregion

        #region Nudge Down
        public static void NudgeDown(TreeNode node)
        {
           int NewIndex = 0;
           TreeNode NodeClone = null;

           try
           {
  
              if (node == null) { return; }

              NewIndex = node.Index + 2;
               
              if (NewIndex > node.Parent.Nodes.Count) { return; }

              NodeClone = (TreeNode)node.Clone();

              node.Parent.Nodes.Insert(NewIndex,NodeClone); 

              node.Parent.Nodes.Remove(node);

              ReOrderSiblings(NodeClone);

              NodeClone.TreeView.SelectedNode = NodeClone;
             
           }
           catch (Exception) { throw; }
           return;
        }
        #endregion

        #region Set Row Sort Order
        private static void SetRowSortOrder(TreeNode node,int newIndex)
        {

           try
           {
 
               DataRow row = (DataRow)node.Tag; 

               row[SortOrderColumnName] = newIndex;
             
           }
           catch (Exception) { throw; }
           return;
        }
        #endregion

        #region Re Order Siblings
        private static void ReOrderSiblings(TreeNode node)
        {

           TreeNode child = null;
          
           try
           {
 
               for(int i=0;i<node.Parent.Nodes.Count;i++)
               {
                  child = node.Parent.Nodes[i]; 
                  SetRowSortOrder(child,i);
               }
              
           }
           catch (Exception) { throw; }
           return;
        }
        #endregion




	}
}
