﻿using DCTS.CustomComponents;
using FA.Buiness;
using FA.Common;
using FA.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace PicFile_Managerment
{
    public partial class frmMain : DockContent
    {

        // 后台执行控件
        private BackgroundWorker bgWorker;
        // 消息显示窗体
        private frmMessageShow frmMessageShow;
        // 后台操作是否正常完成
        private bool blnBackGroundWorkIsOK = false;
        //后加的后台属性显
        private bool backGroundRunResult;
        List<clsFile_Managermentinfo> xianjinliuJINGE_Result;

        private SortableBindingList<clsFile_Managermentinfo> sortablezichanfuzaibiaoList;
        string strFileName;
        int rowcount = 0;
        string user_or_admin;
        DateTime startAt;
        DateTime endAt;
        delegate void Mydelegate(string name, string id);
        Mydelegate md = null;
        clsFile_Managermentinfo selcetitem;
        int Rowindex;
        int Cloumn_index;
        string ioState;
        string nametext;
        #region tree
        private string DBConStr = "";
        private string AppPath = "";
        private ContextMenu tvSample1Menu = new ContextMenu();
        private ContextMenu tvSample2Menu = new ContextMenu();
        private DataRow tree_Current_row;

        DataTable pDataTable = new DataTable();

        //private System.Windows.Forms.Label label1;
        //private System.Windows.Forms.TreeView tvSample;
        //private System.Windows.Forms.Button button1;
        //private System.Windows.Forms.Button button2;
        //private System.Windows.Forms.TreeView tvSample2;
        //private System.Windows.Forms.Label label2;
        //private System.Windows.Forms.Button button3;
        //private System.Windows.Forms.Button button4;
        //private System.Windows.Forms.Button button5;
        //private System.Windows.Forms.Button button6;
        //private System.Windows.Forms.ImageList imageList1;


        #endregion
        public frmMain(string logintype)
        {
            InitializeComponent();
            user_or_admin = logintype;
            tree_Current_row = pDataTable.NewRow();

        }

        private void InitialBackGroundWorker()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged +=
                new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                blnBackGroundWorkIsOK = false;
            }
            else if (e.Cancelled)
            {
                blnBackGroundWorkIsOK = true;
            }
            else
            {
                blnBackGroundWorkIsOK = true;
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (frmMessageShow != null && frmMessageShow.Visible == true)
            {
                //设置显示的消息
                frmMessageShow.setMessage(e.UserState.ToString());
                //设置显示的按钮文字
                if (e.ProgressPercentage == clsConstant.Thread_Progress_OK)
                {
                    frmMessageShow.setStatus(clsConstant.Dialog_Status_Enable);
                }
            }
        }

        private void 读取_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources";
            clsAllnew BusinessHelp = new clsAllnew();

            List<string> Alist = BusinessHelp.GetBy_CategoryReportFileName(path);

            if (Alist.Count > 1 && user_or_admin != "admin")
            {
                MessageBox.Show("权限不足 ,不能批量作业", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {

                InitialBackGroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(ReadclaimreportfromServer);

                bgWorker.RunWorkerAsync();

                // 启动消息显示画面
                frmMessageShow = new frmMessageShow(clsShowMessage.MSG_001,
                                                    clsShowMessage.MSG_007,
                                                    clsConstant.Dialog_Status_Disable);
                frmMessageShow.ShowDialog();

                // 数据读取成功后在画面显示
                if (blnBackGroundWorkIsOK)
                {

                }
            }
            catch (Exception ex)
            {
                return;
                throw ex;
            }
        }
        private void ReadclaimreportfromServer(object sender, DoWorkEventArgs e)
        {

            //初始化信息
            clsAllnew BusinessHelp = new clsAllnew();
            //导入程序集
            DateTime oldDate = DateTime.Now;

            BusinessHelp.ReadDatasources(ref this.bgWorker, "");
            DateTime FinishTime = DateTime.Now;
            TimeSpan s = DateTime.Now - oldDate;
            string timei = s.Minutes.ToString() + ":" + s.Seconds.ToString();
            string Showtime = clsShowMessage.MSG_029 + timei.ToString();
            bgWorker.ReportProgress(clsConstant.Thread_Progress_OK, clsShowMessage.MSG_009 + "\r\n" + Showtime);


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string aa = tree_Current_row["NodeID"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("选择下方类目才可以添加信息");
                return;
                throw;
            }
            var form = new frmNewCreate("", tree_Current_row, null);

            if (form.ShowDialog() == DialogResult.OK)
            {
                InitializeDataSource();
            }

        }

        #region 树

        /// <summary>
        /// 创建树形菜单
        /// </summary>
        public void AddTree(int ParentID, TreeNode pNode)
        {
            // 数据库名字字段
            string strName = "Name";
            // 数据库ID字段
            string strID = "ID";
            // 数据库父级ID字段
            string strParentID = "ParentID";
            //DataTable dt = typeManager.GetAllList();
            DataTable dt = new DataTable();


            DataView dvTree = new DataView(dt);
            dvTree.RowFilter = strParentID + " = " + ParentID;
            foreach (DataRowView Row in dvTree)
            {
                TreeNode Node = new TreeNode();
                if (pNode == null)
                {
                    Node.Text = Row[strName].ToString();
                    Node.Name = Row[strName].ToString();
                    Node.Tag = Row[strID].ToString();
                    Node.ImageIndex = 1;
                    this.tvSample.Nodes.Add(Node);
                    AddTree(Int32.Parse(Row[strID].ToString()), Node); //再次递归 
                }
                else
                {
                    Node.Text = Row[strName].ToString();
                    Node.Name = Row[strName].ToString();
                    Node.Tag = Row[strID].ToString();
                    Node.ImageIndex = 1;
                    pNode.Nodes.Add(Node);
                    AddTree(Int32.Parse(Row[strID].ToString()), Node); //再次递归 
                }
            }
        }

        /// <summary>
        /// 主窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTree_Load(object sender, EventArgs e)
        {
            // 根节点ID值
            int i = 0;
            this.tvSample.Nodes.Clear();
            AddTree(i, (TreeNode)null);
            tvSample.HideSelection = true;
            tvSample.ShowLines = true;
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ImageChange(e);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ImageChange(e);
        }
        /// <summary>
        /// 变换文件夹图标
        /// </summary>
        /// <param name="e"></param>
        public void ImageChange(TreeNodeMouseClickEventArgs e)
        {
            if (null == e.Node.FirstNode)
            {
                e.Node.ImageIndex = 0;
                e.Node.SelectedImageIndex = 0;
            }
            else
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.ImageIndex = 0;
                    e.Node.SelectedImageIndex = 0;
                }
                else
                {
                    e.Node.ImageIndex = 1;
                    e.Node.SelectedImageIndex = 1;
                }
            }
        }


        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 设置显示图标的变换
            if (null == e.Node.FirstNode)
            {
                e.Node.ImageIndex = 1;
                e.Node.SelectedImageIndex = 0;
            }
            // 打开新的窗口，每一级对应一类窗口
            if (e.Node != null && null == e.Node.FirstNode)
            {
                string tag = e.Node.Tag.ToString();
                string name = e.Node.Text.ToString();
                switch (e.Node.Level)
                {
                    //case 0:
                    //    this.md = new Mydelegate(OpenfrmMain);
                    //    break;
                    //case 1:
                    //    this.md = new Mydelegate(OpenForm2);
                    //    break;
                    //case 2:
                    //    this.md = new Mydelegate(OpenForm3);
                    //    break;
                }
                DataRow row = (DataRow)e.Node.Tag;
                tree_Current_row = pDataTable.NewRow();
                tree_Current_row = row;

                if (row == null) { return; }

                string saadsd = row["NodeID"].ToString();
                InitializeDataSource();
                //   md(name, tag);
            }
            else
            {

                DataRow row = (DataRow)e.Node.Tag;
                tree_Current_row = pDataTable.NewRow();
                tree_Current_row = row;

                if (row == null) { return; }

                string saadsd = row["NodeID"].ToString();
                InitializeDataSource();

            }
        }
        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="name">传递参数</param>
        /// <param name="id">传递参数</param>
        //public static void OpenfrmMain(string name, string id)
        //{
        //    Form newForm = new frmMain();
        //    newForm.ShowDialog();
        //}
        //public static void OpenForm2(string name, string id)
        //{
        //    Form newForm = new Form2();
        //    newForm.ShowDialog();
        //}
        //public static void OpenForm3(string name, string id)
        //{
        //    Form newForm = new Form3();
        //    newForm.ShowDialog();
        //}

        #endregion

        #region 树2

        private void frmMain_Load(object sender, System.EventArgs e)
        {

            UI.Hourglass(true);


            try
            {

                AppPath = UI.GetAppPath();

                DBConStr = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + AppPath + "sample.mdb";
                DBConStr = ConfigurationManager.ConnectionStrings["GODDbContext"].ConnectionString;
                //"Provider=SQLOLEDB;server=bds28428944.my3w.com,1433;user=bds28428944;password=Lyh079101;database=bds28428944_db"
                //  string ConStr_sql = @"Provider=SQLOLEDB;server=bds28428944.my3w.com,1433;uid=bds28428944;pwd=Lyh079101;database=bds28428944_db"; //本地自己的数据库

                tvSample1Menu.MenuItems.Add("插入",
                                            new EventHandler(tvSample1RightClickInsert));

                tvSample1Menu.MenuItems.Add("编辑",
                                            new EventHandler(tvSample1RightClickEdit));

                tvSample1Menu.MenuItems.Add("上移",
                                            new EventHandler(tvSample1RightClickNudgeUp));

                tvSample1Menu.MenuItems.Add("下移",
                                            new EventHandler(tvSample1RightClickNudgeDown));

                tvSample1Menu.MenuItems.Add("删除",
                                            new EventHandler(tvSample1RightClickDelete));


                tvSample1Menu.MenuItems.Add("保存",
                                      new EventHandler(button3_Click));


                LoadAllTrees();

                tvSample.AllowDrop = true;


            }
            catch (Exception err) { UI.Hourglass(false); UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }



        #region Load All Trees
        private void LoadAllTrees()
        {

            try
            {
                LoadTree(tvSample, Rules.GetHierarchy(DBConStr, 1));

            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Load Tree
        private void LoadTree(TreeView tv, DataSet ds)
        {

            UI.Hourglass(true);

            try
            {

                TreeViewUtil.LoadFromDataSet(tv, ds, "Description");

                if (tv.Nodes.Count > 0)
                {
                    tv.Nodes[0].Expand();
                }

            }
            catch (Exception) { throw; }
            finally
            {
                UI.Hourglass(false);
            }
        }
        #endregion

        #region tvSample1 Right Click Delete
        private void tvSample1RightClickDelete(object sender, System.EventArgs e)
        {

            UI.Hourglass(true);

            try
            {
                TreeViewUtil.DeleteNode(tvSample, true);

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion



        #region tvSample1 Right Click Edit
        private void tvSample1RightClickEdit(object sender, System.EventArgs e)
        {

            UI.Hourglass(true);

            try
            {

                TreeNode node = tvSample.SelectedNode;

                if (node == null) { return; }

                node.TreeView.LabelEdit = true;

                node.BeginEdit();

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion



        #region tvSample1 Right Click Nudge Up
        private void tvSample1RightClickNudgeUp(object sender, System.EventArgs e)
        {

            UI.Hourglass(true);

            try
            {
                TreeViewUtil.NudgeUp(tvSample.SelectedNode);

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion

        #region tvSample1 Right Click Nudge Down
        private void tvSample1RightClickNudgeDown(object sender, System.EventArgs e)
        {

            UI.Hourglass(true);

            try
            {
                TreeViewUtil.NudgeDown(tvSample.SelectedNode);

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion




        #region tvSample1 Right Click Insert
        private void tvSample1RightClickInsert(object sender, System.EventArgs e)
        {

            UI.Hourglass(true);

            try
            {

                TreeNode node = tvSample.SelectedNode;

                if (node == null) { return; }

                InsertNewNode(node);

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion



        #region Insert New Node
        private void InsertNewNode(TreeNode node)
        {

            DataRow row = null;
            DataRow ParentRow = null;
            DataTable dt = null;
            int newindex = 0;

            try
            {

                ParentRow = (DataRow)node.Tag;

                if (ParentRow == null) { return; }

                newindex = int.Parse(ParentRow["SortOrder"].ToString()) + 1;

                dt = ParentRow.Table;

                row = dt.NewRow();

                row["ObjectID"] = Guid.NewGuid().ToString();
                row["ObjectTypeID"] = 1;
                row["ModelID"] = int.Parse(ParentRow["ModelID"].ToString());
                row["NodeID"] = Guid.NewGuid().ToString();
                row["ParentNodeID"] = ParentRow[dt.PrimaryKey[0].ColumnName].ToString();
                row["Description"] = "New Node";
                row["ForeColor"] = "#000000";
                row["BackColor"] = "#FFFFFF";
                row["ImageIndex"] = 0;
                row["SelectedImageIndex"] = 1;
                row["Checked"] = true;
                row["ActiveID"] = 1;
                row["NamedRange"] = "";
                row["NodeValue"] = "";
                row["LastUpdateTime"] = DateTime.Now;
                row["SortOrder"] = newindex;

                dt.Rows.Add(row);

                node.Nodes.Add(TreeViewUtil.GetTreeNodeFromDataRow(row, "Description"));

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Edit Node
        private void EditNode(TreeNode node, string newText)
        {
            DataRow row = null;

            try
            {

                if (node == null) { return; }

                row = (DataRow)node.Tag;

                if (row == null) { return; }

                row["Description"] = newText;

            }
            catch (Exception) { throw; }

        }
        #endregion

        #region Button Reload Test Data
        private void button1_Click(object sender, System.EventArgs e)
        {
            LoadAllTrees();
        }
        #endregion


        #region tvSample Mouse Down
        private void tvSample_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            TreeViewUtil.SetSelectedNodeByPosition(tvSample, e.X, e.Y);

            if (tvSample.SelectedNode == null) { return; }

            if (e.Button == MouseButtons.Right) { return; }

        }
        #endregion

        #region tvSample MouseUp
        private void tvSample_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            switch (e.Button)
            {
                case MouseButtons.Right:

                    tvSample1Menu.Show(tvSample, new Point(e.X, e.Y));
                    return;

                default:
                    break;
            }

        }
        #endregion


        #region tvSample1 Accept Changes
        private void button3_Click(object sender, System.EventArgs e)
        {

            savenode();

        }

        private void savenode()
        {
            DataRow row = null;
            UI.Hourglass(true);

            try
            {

                if (tvSample.Nodes.Count == 0) { return; }

                row = (DataRow)tvSample.Nodes[0].Tag;

                Rules.CommitHierarchy(DBConStr, row.Table.DataSet);

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion

        #region tvSample1 Reject Changes
        private void button4_Click(object sender, System.EventArgs e)
        {

            DataRow row = null;
            UI.Hourglass(true);

            try
            {

                if (tvSample.Nodes.Count < 1) { return; }

                row = (DataRow)tvSample.Nodes[0].Tag;

                row.Table.DataSet.RejectChanges();

                LoadTree(tvSample, row.Table.DataSet);

            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion



        #region Form Closed
        private void frmMain_Closed(object sender, System.EventArgs e)
        {

        }
        #endregion

        #region Exit
        private void cmdExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        #endregion

        #region tvSample Drag And Drop Events
        private void tvSample_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvSample_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeViewUtil.DragEnter((TreeView)sender, e);
        }

        private void tvSample_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeViewUtil.DragOver((TreeView)sender, e);
        }

        private void tvSample_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            DataRow row;
            bool dropOnNewControl = false;

            try
            {

                UI.Hourglass(true);

                TreeViewUtil.DragDrop((TreeView)sender, e, ref dropOnNewControl);

                if (dropOnNewControl)
                {
                    //row = (DataRow)tvSample2.Nodes[0].Tag;
                    //Rules.CommitHierarchy(DBConStr, row.Table.DataSet);
                    row = (DataRow)tvSample.Nodes[0].Tag;
                    Rules.CommitHierarchy(DBConStr, row.Table.DataSet);
                }

                //   this.LoadAllTrees();  

                UI.Hourglass(false);
            }
            catch (Exception err) { UI.ShowError(err.Message); }
            finally { UI.Hourglass(false); }
        }
        #endregion

        private void tvSample_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (e.Label.Trim().Length < 1) { e.CancelEdit = true; }
                EditNode(tvSample.SelectedNode, e.Label);
                tvSample.SelectedNode.EndEdit(false);
                tvSample.LabelEdit = false;
            }
            catch (Exception err) { UI.ShowError(err.Message); }
        }
        #endregion

        private void InitializeDataSource()
        {
            if (tree_Current_row != null)
            {
                clsAllnew BusinessHelp = new clsAllnew();
                xianjinliuJINGE_Result = new List<clsFile_Managermentinfo>();

                string conditions = "";
                conditions = "select * from File_Managerment where NodeID  like '%" + tree_Current_row["NodeID"].ToString() + "%'";//成功
                xianjinliuJINGE_Result = BusinessHelp.find_File_Managerment(ref this.bgWorker, conditions);

                ShowDav();

            }


        }

        private void ShowDav()
        {
            this.toolStripLabel2.Text = "";

            if (xianjinliuJINGE_Result != null && xianjinliuJINGE_Result.Count > 0)
            {
                sortablezichanfuzaibiaoList = new SortableBindingList<clsFile_Managermentinfo>(xianjinliuJINGE_Result);
                this.bindingSource1.DataSource = this.sortablezichanfuzaibiaoList;
                dataGridView.AutoGenerateColumns = false;
                dataGridView.DataSource = this.bindingSource1;

                int Tfenshu = (from s in sortablezichanfuzaibiaoList
                               where Convert.ToInt64(s.T_id) > 0
                               select Convert.ToInt32(s.fenshu)).Sum();

                int Tyeshu = (from s in sortablezichanfuzaibiaoList
                              where Convert.ToInt64(s.T_id) > 0
                              select Convert.ToInt32(s.yeshu)).Sum();


                this.toolStripLabel2.Text = "总计：" + sortablezichanfuzaibiaoList.Count + " 条" + "  份数:" + Tfenshu.ToString() + "  页数:" + Tyeshu.ToString();

            }
            else
            {
                this.toolStripLabel2.Text = "未找到";
                dataGridView.DataSource = null;
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            var row = dataGridView.Rows[Rowindex];
            selcetitem = new clsFile_Managermentinfo();

            var model = row.DataBoundItem as clsFile_Managermentinfo;
            selcetitem = model;

            var oids = GetOrderIdsBySelectedGridCell();
            if (oids.Count > 0 && selcetitem != null && oids[0]!=null)
            {

                var form = new frmPicEdit(oids[0].ToString(), selcetitem);

                if (form.ShowDialog() == DialogResult.OK)
                {

                }
            }
            else
            {
                MessageBox.Show("请选择类目再次尝试！");

            }
        }

        private void 编辑图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var oids = GetOrderIdsBySelectedGridCell();
            var row = dataGridView.Rows[Rowindex];
            selcetitem = new clsFile_Managermentinfo();

            var model = row.DataBoundItem as clsFile_Managermentinfo;
            selcetitem = model;

            if (selcetitem != null)
            {
                if (oids.Count < 1)
                    oids.Add("");

                var form = new frmPicEdit(oids[0], selcetitem);

                if (form.ShowDialog() == DialogResult.OK)
                {

                }
            }
            else
                MessageBox.Show("请选择条目");


        }
        private List<string> GetOrderIdsBySelectedGridCell()
        {

            List<string> order_ids = new List<string>();
            var rows = GetSelectedRowsBySelectedCells(dataGridView);
            foreach (DataGridViewRow row in rows)
            {
                var Diningorder = row.DataBoundItem as clsFile_Managermentinfo;
                order_ids.Add(Diningorder.accfile_id);
            }

            return order_ids;
        }
        private List<string> GetOrderIdsBySelectedGridCell_T_id()
        {

            List<string> order_ids = new List<string>();
            var rows = GetSelectedRowsBySelectedCells(dataGridView);
            foreach (DataGridViewRow row in rows)
            {
                var Diningorder = row.DataBoundItem as clsFile_Managermentinfo;
                order_ids.Add(Diningorder.T_id);
            }

            return order_ids;
        }
        private IEnumerable<DataGridViewRow> GetSelectedRowsBySelectedCells(DataGridView dgv)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                rows.Add(cell.OwningRow);
                clsAllnew BusinessHelp = new clsAllnew();


            }
            rowcount = dgv.SelectedCells.Count;

            return rows.Distinct();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var oids = GetOrderIdsBySelectedGridCell_T_id();
            if (oids != null && oids.Count > 0 && MessageBox.Show(" 确定删除本条?", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                clsAllnew BusinessHelp = new clsAllnew();

                for (int i = 0; i < oids.Count; i++)
                {
                    bool istur = BusinessHelp.deleteFile_Managerment(oids[0].ToString());

                    if (istur == false)
                    {
                        MessageBox.Show("删除失败，请重新尝试！" + oids[0].ToString());
                    }

                }
                InitializeDataSource();
            }
            else
            {
                return;
            }

        }

        private void 编辑属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewColumn column = dataGridView.Columns[Cloumn_index];
            var row = dataGridView.Rows[Rowindex];
            selcetitem = new clsFile_Managermentinfo();

            var model = row.DataBoundItem as clsFile_Managermentinfo;
            selcetitem = model;

            if (selcetitem != null)
            {
                var form = new frmNewCreate("", tree_Current_row, selcetitem);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    InitializeDataSource();
                }
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn column = dataGridView.Columns[Cloumn_index];
            var row = dataGridView.Rows[Rowindex];
            selcetitem = new clsFile_Managermentinfo();

            var model = row.DataBoundItem as clsFile_Managermentinfo;

            selcetitem = model;

        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            Rowindex = e.RowIndex;
            Cloumn_index = e.ColumnIndex;


        }
        private void ReadOtherinfo()
        {
            startAt = this.dateTimePicker1.Value.AddDays(0).Date;
            endAt = this.dateTimePicker2.Value.AddDays(0).Date;
            ioState = this.textBox1.Text;
            nametext = this.textBox2.Text;

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            ReadOtherinfo();

            if (textBox1.Text != "")
            {
                clsAllnew BusinessHelp = new clsAllnew();

                string conditions = "";
                //conditions = "select * from File_Managerment where wenjianbiaohao  like '%" + this.textBox1.Text.ToString() + "%'";//成功

                conditions = "select * from File_Managerment where dengjiriqi >= '" + startAt.ToString("MM/dd/yyyy") + "' AND dengjiriqi<='" + endAt.ToString("MM/dd/yyyy") + "'";
                if (ioState.Length > 0)
                {
                    conditions += " And wenjianbiaohao like '%" + ioState + "%'";
                }
                if (nametext.Length > 0)
                {
                    conditions += " And biaoti like '%" + nametext + "%'";
                }


                xianjinliuJINGE_Result = BusinessHelp.find_File_Managerment(ref this.bgWorker, conditions);
                ShowDav();
            }
            else
            {

                MessageBox.Show("请输入相关查找信息", "查找", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void toolStripDropDownButton5_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {


        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.downcsv(dataGridView);
        }

        private void 编辑图片ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //var oids = GetOrderIdsBySelectedGridCell();
            //if (oids.Count > 0 && selcetitem!=null)
            //{

            //    var form = new frmPicEdit(oids[0].ToString(), selcetitem);

            //    if (form.ShowDialog() == DialogResult.OK)
            //    {

            //    }
            //}
            编辑图片ToolStripMenuItem_Click(null, EventArgs.Empty);

        }

        private void 编辑属性ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            编辑属性ToolStripMenuItem_Click(null, EventArgs.Empty);
        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {

        }

        private void 扫描ToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }
    }
}
