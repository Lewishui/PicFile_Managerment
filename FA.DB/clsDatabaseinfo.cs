using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace FA.DB
{
    public class clsDatabaseinfo
    {
    }
    public class clszichanfuzaibiaoinfo
    {
        public string Order_id { get; set; }
        public string xiangmu { get; set; }
        public string hangci { get; set; }
        public string qimojine { get; set; }
        public string shangniantongqishu { get; set; }

        public string nianchujine { get; set; }
        public string xiangmuF { get; set; }
        public string hangciG { get; set; }
        public string qimojineH { get; set; }
        public string shangniantongqishuI { get; set; }
        public string nianchujineJ { get; set; }
        public string Input_Date { get; set; }

        public string bianzhidanwei { get; set; }
        public string riqi { get; set; }
        public string danwei { get; set; }


    }

    public class clsFile_Managermentinfo
    {
        public string T_id { get; set; }
        public string wenjianbiaohao { get; set; }
        public string biaoti { get; set; }
        public string wenhao { get; set; }
        public string zhiwendanwei { get; set; }

        public string xingwendanwei { get; set; }
        public string dengjiriqi { get; set; }
        public string miji { get; set; }
        public string wenjianleibie { get; set; }
        public string yeshu { get; set; }
        public string fenshu { get; set; }
        public string accfile_id { get; set; }

        public string beizhu { get; set; }
      

    }
    public class clsAccFileinfo
    {
        public string T_id { get; set; }
        public string File_name { get; set; }
        public string accfile_id { get; set; }
        public string mark1 { get; set; }
        public string mark2 { get; set; }
        public string mark3 { get; set; }
        public string mark4 { get; set; }
        public string mark5 { get; set; }
    }
    public class clstress_info
    {
        public string T_id { get; set; }
        public string ModelID { get; set; }
        public string NodeID { get; set; }
        public string ParentNodeID { get; set; }
        public string Description { get; set; }
        public string ForeColor { get; set; }
        public string BackColor { get; set; }
        public string ImageIndex { get; set; }
        public string SelectedImageIndex { get; set; }
        public string ObjectID { get; set; }
        public string ObjectTypeID { get; set; }
        public string NamedRange { get; set; }
        public string NodeValue { get; set; }
        public string ActiveID { get; set; }
        public string LastUpdateTime { get; set; }
        public string SortOrder { get; set; }
      
    }
}
