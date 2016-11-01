using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
namespace ConsoleApplication2
{
    public struct symbol
    {
       public string id;
       public string findingtype;
       public string findingname;
       public string propertyname;
       public string optionname;
       public int frequency;
       public int specifity;
    }
    public struct detail
    {
        public string symptomname;
        public string propertyname;
        public string optionname;
    }
    public struct ask
    {
        public string findingtype;
        public string findingname;
        public string propertyname;
      //  public List<string> optionname;
    }
    public struct newask
    {
        public string propertyname;
        public List<string> optionname;
    }
    class globalvarible
    {
        public static double[] specifity = new double[6] { 0.5,2, 3, 5, 8, 10 };
        public static double[] frequency = new double[6] { 5, 4.5, 4, 3, 2, 0.5 };
        public static double punish= 0.1;
        public static double none=5;
        public static double thres = 0.95;
        public static double thres_deter = 0.5;
        public static double multiple = 5;
        public static List<detail> signs = new List<detail> ();
        public static Dictionary<string,List<symbol>> disease=new Dictionary<string,List<symbol>>();
        public static Dictionary<string, double> possibility = new Dictionary<string, double>();
        public static List<ask> question=new List<ask>();
        public static List<string> askedid=new List<string>();
        public static int count = 0;
        public static Dictionary<List<string>,List<newask>> newquestion=new Dictionary<List<string>,List<newask>>();
    }
    class connect2sql
    {
        public SqlConnection connection;
        private string connect_string = "Data Source=202.38.78.73;User ID=cdssuser;Password=cdssuser;Database=CDSS;MultipleActiveresultsets=TRUE";
        public void initialization()
        {
            connection = new SqlConnection(connect_string);
        }

    }

    class Program
    {
        static void read_data(XmlDocument doc)
        {
            //将input存入signs里，
            XmlNodeList xl1 = doc.SelectSingleNode("XmlInfoInput").ChildNodes;
            foreach (XmlNode xn1 in xl1)
            {
                if (!xn1.HasChildNodes)
                    continue;
                XmlNodeList xl2 = xn1.ChildNodes;
                foreach (XmlNode xn2 in xl2)
                {
                    XmlNodeList xl3 = xn2.ChildNodes;
                    string temp1 = "NULL";
                    string temp2 = "NULL";
                    string temp3 = "NULL";
                    #region 
                    foreach (XmlNode xn3 in xl3)
                    {
                        
                        if (xn3.Name!="Properties")
                            temp1 = xn3.InnerText;
                        if (xn3.Name == "Properties")
                        {
                            
                                XmlNodeList xl4 = xn3.ChildNodes;
                                foreach (XmlNode xn4 in xl4)
                                {
                                    temp2 = "NULL";
                                    temp3 = "NULL";
                                    foreach (XmlNode xn5 in xn4.ChildNodes)
                                        if (xn5.Name == "PropertyName")
                                            temp2 = xn5.InnerText;
                                        else if (xn5.Name == "OptionName")
                                            temp3 = xn5.InnerText;
                                    detail temp = new detail();
                                    temp.symptomname = temp1;
                                    if (temp2 == "") temp2 = "NULL";
                                    temp.propertyname = temp2;
                                    if (temp3 == "") temp3 = "NULL";
                                    temp.optionname = temp3;
                                    globalvarible.signs.Add(temp);
                                }
                        }
                    }
                    #endregion
                }
            }
        }
        static void getdata()
        {
            //依据sign从数据库中读取相关疾病并计算概率；
            connect2sql conn = new connect2sql();
            conn.initialization();
            conn.connection.Open();
            List<string> nofinding_collection = new List<string>();//记录患者没有的症候;
            string command;
            double count = 0;//count用来记录globalvarible.signs里症候的个数；
            foreach (detail tempdetail in globalvarible.signs)
            {
                string temp1 = tempdetail.symptomname;
                string temp2 = tempdetail.propertyname;
                string temp3 = tempdetail.optionname;
                if (temp2 == "NULL" && temp3 == "NULL") //两层，说明该症候没有property；
                {
                    count++;
                    command = "SELECT DiagnosisDataUID, DiseaseName, Specifity FROM DiagnosisData WHERE (FindingName='" + temp1 + "') AND (PropertyName IS NULL)";
                    add_disease(command,conn.connection,count);//取出相关疾病，计算概率，并把记录已经询问过的ID；
                }
                else if (temp2 == "None" && temp3 == "NULL")
                {
                    //症候不存在;
                    nofinding_collection.Add(temp1);
                    
                }
                else                                           //三层；
                {
                    count++;
                    command = "SELECT DiagnosisDataUID, DiseaseName, Specifity FROM DiagnosisData WHERE (FindingName='" + temp1 + "') AND (PropertyName='" + temp2 + "') AND (OptionName='" + temp3 + "')";
                    add_disease(command,conn.connection,count);
                }     
            }
            if(nofinding_collection.Count>0)
             add_disease_none(nofinding_collection, conn.connection);//利用不存在的症候进行筛选;
            if (normalization() > 0)  //已确定诊断疾病;
                return;
            else
              complete_disease(conn.connection);//不能确定疾病，将相关疾病的症候提取到globalvarible.disease里，以便询问新的症候；
          conn.connection.Close();
          //ask_finding();
        }
        static void add_disease(string command,SqlConnection connection,double count)
        {
            //症候存在时，将症候的ID存入globalvarible.id ；将疾病的计算结果存储在globalvarible.possibility里；
            SqlCommand mycommand = new SqlCommand();
            mycommand.Connection = connection;
            mycommand.CommandText = command;
            SqlDataReader reader;
            reader = mycommand.ExecuteReader();
            List<string> dis_collection = new List<string>();//用来记录此症候相关的疾病;
            List<string> share_collection = new List<string>();//用来记录症候相关疾病与globalvarible.possibility共有的症候；
            #region
            while (reader.Read())
            {
                string tempdis;
                tempdis = reader["DiseaseName"].ToString();
                if (!dis_collection.Contains(tempdis))
                    dis_collection.Add(tempdis);
                int specifity = reader.GetInt32(2);
                string id = reader["DiagnosisDataUID"].ToString();
                globalvarible.askedid.Add(id);  //globalvarible.askedid记录已经添加的症候;
                if (globalvarible.possibility.ContainsKey(tempdis))
                //globalvarible.possibility已经存在该疾病;
                {
                    share_collection.Add(tempdis);
                    (globalvarible.possibility)[tempdis] = (globalvarible.possibility)[tempdis] * globalvarible.specifity[specifity];
                }
            }
            #endregion
            reader.Close();
            if (dis_collection.Count == 0)  //该症候在数据库里不存在,所有疾病都不做处理;
                return;
            List<string> tempdis_co = new List<string>(globalvarible.possibility.Keys);
            for (int i = 0; i < tempdis_co.Count; i++)
            {
                string tempdis = tempdis_co[i];
                if (!dis_collection.Contains(tempdis))   //globalvarible.possibility不含该症候疾病需要添加惩罚;
                    globalvarible.possibility[tempdis] = globalvarible.possibility[tempdis] * globalvarible.punish;
            }
            foreach (string tempdis in dis_collection)
                if (!share_collection.Contains(tempdis))//向globalvarible.possibility添加新的疾病;
                    globalvarible.possibility.Add(tempdis, Math.Pow(globalvarible.punish, count - 1));
        }
        static void add_disease_none(List<string> nofinding_collection,SqlConnection connection)
        {
            SqlCommand mycommand = new SqlCommand();
            mycommand.Connection = connection;
            SqlDataReader reader;
            foreach (string nofinding in nofinding_collection)
            {
                string command = "SELECT DiagnosisDataUID, DiseaseName,Frequency FROM DiagnosisData WHERE (FindingName='" + nofinding + "') AND (PropertyName IS NOT NULL)";
                mycommand.CommandText = command;
                SqlDataReader tempreader = mycommand.ExecuteReader();
                bool indicator = tempreader.HasRows;
                tempreader.Close();
                if (!indicator)
                {
                    //只有两层或者在DiagnosisData表里未曾提到的症候;当这个症候未出现时，所有疾病都不作处理;
                    command = "SELECT DiagnosisDataUID, DiseaseName,Frequency FROM DiagnosisData WHERE (FindingName=" + nofinding + ") AND (PropertyName IS NULL)";
                    mycommand.CommandText = command;
                }
                reader = mycommand.ExecuteReader();
                List<string> nonedis = new List<string>();
                int indicator1 = 0;
                while (reader.Read())
                {
                    indicator1++;
                    string id = reader["DiagnosisDataUID"].ToString();
                    globalvarible.askedid.Add(id);
                    string tempdis = reader["DiseaseName"].ToString();
                    int frequency = reader.GetInt32(2);
                    if (globalvarible.possibility.ContainsKey(tempdis))
                    {
                        //疾病与未出现的症候有关；
                        globalvarible.possibility[tempdis] = globalvarible.possibility[tempdis] * globalvarible.frequency[frequency];
                        nonedis.Add(tempdis);
                    }
                }
                reader.Close();
                if (indicator1 == 0)  //该症候在数据库里不存在;
                    continue;
                for (int i = 0; i < globalvarible.possibility.Keys.Count; i++)
                {
                    //疾病与未出现的症候无关;
                    List<string> keys = new List<string>(globalvarible.possibility.Keys);
                    string tempnone = keys[i];
                    if (!nonedis.Contains(tempnone))
                        globalvarible.possibility[tempnone] = globalvarible.possibility[tempnone] * globalvarible.none;
                }     
            }
          }
        static void complete_disease(SqlConnection connection)
        {
            //读取相关疾病未曾提到的症候,放入globalvarabile.disease；疾病作为key；
            SqlCommand mycommand = new SqlCommand();
            mycommand.Connection = connection;
            string command;
            SqlDataReader reader;
          
            //上述list用来暂存症候信息;
            foreach(string tempdis in globalvarible.possibility.Keys)
            {
                List<string> tempid = new List<string>();
                List<string> tempfindingtype = new List<string>();
                List<string> tempfindingname = new List<string>();
                List<string> temppropertyname = new List<string>();
                List<string> tempoptionname = new List<string>();
                List<int> tempfrequency = new List<int>();
                List<int> tempspecifity = new List<int>();
               command = "SELECT * FROM DiagnosisData WHERE (DiseaseName='" + tempdis + "')";//取出所有跟疾病相关的症候;
               mycommand.CommandText=command;
               reader = mycommand.ExecuteReader();
                   #region
                   while (reader.Read())
                   {
                       string id = reader["DiagnosisDataUID"].ToString();
                       if (!globalvarible.askedid.Contains(id))
                       {
                          tempid.Add(reader["DiagnosisDataUID"].ToString());
                          tempfindingtype.Add(reader["FindingType"].ToString());
                          tempfindingname.Add(reader["FindingName"].ToString());
                           if (reader.IsDBNull(4) || reader.IsDBNull(5))
                           {
                               temppropertyname.Add("NULL");
                                tempoptionname.Add("NULL");
                           }
                           else
                           {
                              temppropertyname.Add(reader["PropertyName"].ToString());
                              tempoptionname.Add(reader["OptionName"].ToString());
                           }
                           tempfrequency.Add(reader.GetInt32(6));
                           tempspecifity.Add(reader.GetInt32(7));
                       }
                   }
                   reader.Close();
                   #endregion
                   #region   将症候向globalvaribel.disease里存储;
                   List<string> level3_dis=new List<string>();//存储三层症候用来判断症候是两层还是三层;
                for (int i = 0; i < tempid.Count; i++)
                    if (temppropertyname[i] != "NULL")
                        level3_dis.Add(tempfindingname[i]);
                for (int i = 0; i < tempid.Count; i++)
                {
                    if (level3_dis.Contains(tempfindingname[i]) && temppropertyname[i] == "NULL")  //该症候是三层的
                            continue;//症候是三层，数据库对应的是两层，此情况不应该记录;
                    symbol sym = new symbol();
                    sym.id = tempid[i];
                    sym.findingtype = tempfindingtype[i];
                    sym.findingname = tempfindingname[i];
                    sym.propertyname = temppropertyname[i];
                    sym.optionname = tempoptionname[i];
                    sym.frequency = tempfrequency[i];
                    sym.specifity = tempspecifity[i];
                    List<symbol> tempsym=new List<symbol>();
                    tempsym.Add(sym);
                    if (globalvarible.disease.ContainsKey(tempdis))
                        globalvarible.disease[tempdis].Add(sym);
                    else
                        globalvarible.disease.Add(tempdis, tempsym);
                }
                   #endregion
            }
            Dictionary<string, List<symbol>> temp55 = globalvarible.disease;
        }
        static void ask_finding()
        {
            //查找需要询问的finding;
            int num_thres=0;
            double sum_thres = 0;//统计前n个概率之和,用sum_thres记录和，用num_thres记录数目;
           foreach (string tempdis in globalvarible.possibility.Keys)
           {
               sum_thres+=globalvarible.possibility[tempdis];
               num_thres++;
               if (sum_thres > globalvarible.thres)
                   break;
           }
           if (num_thres > 5)
               disease_remove(num_thres);  //去除疾病原则;
           else
               max_first(num_thres);       //最大化首选疾病原则;
          
          // output();            
        }
        static void disease_remove(int num)
        {
            //有很多候选疾病，依据排除疾病的方法;
            Dictionary<List<string>, int> total_sign = new Dictionary<List<string>, int>();
            string temp1 = "";//findingname;
            string temp2 = "";//propertyname;
            string temp3 = "";//findingtype;
            //total_sign 统计每个<findingname,propertyname>出现的个数,方便查找出现次数最多的症候组合;
            #region
            foreach (string key in globalvarible.possibility.Keys)
            {
                if (num <= 0)
                    break;
                num--;
                foreach (symbol sign in globalvarible.disease[key])
                {
                  //  if (globalvarible.count < 2 && sign.findingtype == "Laboratory")
                    //    continue;//当询问次数在两次以内的时候不考虑Labratory;
                    if (sign.findingtype != "Symptom")
                        continue;
                    temp1 = sign.findingname;
                    temp2 = sign.propertyname;
                    temp3 = sign.findingtype;
                    List<string> temp = new List<string>();
                    temp.Add(temp1);
                    temp.Add(temp2);
                    temp.Add(temp3);
                    if (total_sign.ContainsKey(temp))
                        total_sign[temp]++;
                    else
                        total_sign.Add(temp, 1);
                }
            }
            if (total_sign.Keys.Count > 1)
            {
                var dissort = from objDic in total_sign orderby objDic.Value descending select objDic;//对value排序
                Dictionary<List<string>, int> temp_total = new Dictionary<List<string>, int>();
                foreach (KeyValuePair<List<string>, int> temp4 in dissort)
                {
                    temp_total.Add(temp4.Key, temp4.Value);
                }
                total_sign.Clear();
                total_sign = temp_total;
            }
            #endregion
            int count = 0;
            foreach (List<string> temp in total_sign.Keys)
            {
                if (total_sign[temp] > 2)
                {
                    if (count > 8)
                        break;
                    count++;
                    temp1 = temp[0];
                    temp2 = temp[1];
                    temp3 = temp[2];
                    ask temp4 = new ask();
                    temp4.findingname = temp1;
                    temp4.propertyname = temp2;
                    temp4.findingtype = temp3;
                  //  temp4.optionname = new List<string>();
                    globalvarible.question.Add(temp4);
                }
            }
            //当共同症候总数少于8时，询问每种候选疾病的最大frequency对应症候；
            #region
            if (count < 8)
            {
                foreach (string tempdis in globalvarible.possibility.Keys)
                {
                    count = add_max(count, tempdis);
                    if (count >= 8)
                        break;
                }
            }
            #endregion
        }
        static void max_first(int num)
        {
            //候选疾病数目不多，依据最大化首选疾病的原则;
            int count = 0;
            int indicator1 = 0;
            int indicator2 = 0;
            foreach (string key in globalvarible.possibility.Keys)
            {
                if (count > 5)
                    break;
                if (indicator1 == 0)
                    while (indicator1 < 3)
                    {
                        //询问第一个候选疾病前三高frequency
                        count = add_max(count, key);
                        indicator1++;
                    }
                else if (indicator2 == 0)
                    while (indicator2 < 2)
                    {
                        count = add_max(count, key);
                        indicator2++;
                    }
                else
                    count = add_max(count, key);
               }
        }
        static int add_max(int count,string tempdis)
        {
            //将疾病中frequency最大的finding找出并加入到globalvarible.question里;
          string  temp1 = "NULL";
          string temp2 = "NULL";
          string temp3 = "NULL";
          int max_frequency = 0;
          foreach (symbol temp5 in globalvarible.disease[tempdis])
            {
                if (temp5.findingtype != "Symptom")
                    continue;
                ask ask_temp1 = new ask();
                ask_temp1.propertyname = temp5.propertyname;
                ask_temp1.findingname = temp5.findingname;
                ask_temp1.findingtype = temp5.findingtype;
             //   ask_temp1.optionname = new List<string>();
                if (!globalvarible.question.Contains(ask_temp1) && temp5.frequency > max_frequency)
                {
                    max_frequency = temp5.frequency;
                    temp1 = temp5.findingname;
                    temp2 = temp5.propertyname;
                    temp3 = temp5.findingtype;
                }
            }
          if (max_frequency > 0)
          {
              //找到了最大的frequency；
              ask ask_temp2 = new ask();
              ask_temp2.propertyname = temp2;
              ask_temp2.findingname = temp1;
              ask_temp2.findingtype = temp3;
           //   ask_temp2.optionname = new List<string>();
              globalvarible.question.Add(ask_temp2);
              count++;
          }
            return (count);
        }
        static XmlDocument output()
        {
            #region
            XmlDocument xml=new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xml.CreateXmlDeclaration("1.0", "gb2312", null);
            xml.AppendChild(xmldecl);
            XmlElement root = xml.CreateElement("", "XmlInfoOutput", "");
            xml.AppendChild(root);
            XmlElement xn1_symptoms=xml.CreateElement("","Symptoms","");
                root.AppendChild(xn1_symptoms);
                XmlElement xn1_signs=xml.CreateElement("","Signs","");
                root.AppendChild(xn1_signs);
                XmlElement xn1_lab=xml.CreateElement("","Laboratorys","");
                root.AppendChild(xn1_lab);
                XmlElement xn1_pre=xml.CreateElement("","Predisposition","");
                root.AppendChild(xn1_pre);
                XmlElement xn1_rad=xml.CreateElement("","Radiologys","");
                root.AppendChild(xn1_rad);
                XmlElement xn1_other=xml.CreateElement("","otherExams","");
                root.AppendChild(xn1_other);
                XmlElement xn1_diagnosis=xml.CreateElement("","Diagnoses","");
                root.AppendChild(xn1_diagnosis);
              #endregion
                List<string> newtemp = new List<string>();
            foreach(KeyValuePair<List<string>,List<newask>> key in globalvarible.newquestion)
            {
                List<string> tempkey=key.Key;
                List<newask> option=key.Value;
                #region
                string temp1="";
                string temp2="";
                if(tempkey[0]=="Symptom")
                {
                    temp1="Symptom";
                    temp2="SymptomName";
                    newtemp.Add(temp1);
                }
                else if(tempkey[0]=="Sign")
                {
                    temp1="Sign";
                    temp2="SignName";
                    newtemp.Add(temp1);
                }
                 else if(tempkey[0]=="Predisposition")
                {
                     temp1="Predisposition";
                     temp2="PredispositionName";
                     newtemp.Add(temp1);
                 }
                else
                {
                     temp1="Laboratory";
                     temp2="LaboratoryName";
                     newtemp.Add(temp1);
                 }
               #endregion
                #region 
                XmlElement xn1=xml.CreateElement("",temp1,"");
                XmlElement xn2=xml.CreateElement("",temp2,"");
                xn2.InnerText=tempkey[1];
                xn1.AppendChild(xn2);
                XmlElement xn22=xml.CreateElement("","Properties","");
                xn1.AppendChild(xn22);
                foreach(newask tempask in option)
                {
                    XmlElement xn3=xml.CreateElement("","Property","");
                    xn22.AppendChild(xn3);
                    XmlElement xn4=xml.CreateElement("","PropertyName","");
                    if(tempask.propertyname=="NULL")
                    {
                        xn4.InnerText="";
                        xn3.AppendChild(xn4);
                        XmlElement xn5=xml.CreateElement("","OptionName","");
                        xn5.InnerText="";
                        xn3.AppendChild(xn5);
                        continue;
                     }
                    xn4.InnerText=tempask.propertyname;
                    xn3.AppendChild(xn4);
                    List<string> tempoption=tempask.optionname;
                    for(int i=0;i<tempoption.Count;i++)
                    {
                        XmlElement xn42=xml.CreateElement("","OptionName","");
                        xn42.InnerText=tempoption[i];
                        xn3.AppendChild(xn42);
                    }
                }
                XmlElement xn31=xml.CreateElement("","Property","");
                xn22.AppendChild(xn31);
                XmlElement xn41=xml.CreateElement("","PropertyName","");
                xn41.InnerText="None";
                xn31.AppendChild(xn41);
                XmlElement xn45=xml.CreateElement("","OptionName","");
                xn45.InnerText="";
                xn31.AppendChild(xn45);
                if(tempkey[0]=="Symptom")
                  xn1_symptoms.AppendChild(xn1);
                else if(tempkey[0]=="Sign")
                  xn1_signs.AppendChild(xn1);
                else if(tempkey[0]=="Predisposition")
                  xn1_pre.AppendChild(xn1);
                else
                 xn1_lab.AppendChild(xn1);
             #endregion
            }
            if (!newtemp.Contains("Symptom")) xn1_symptoms.InnerText = "";
            if (!newtemp.Contains("Sign")) xn1_signs.InnerText = "";
            if (!newtemp.Contains("Predisposition")) xn1_pre.InnerText = "";
            if (!newtemp.Contains("Laboratory")) xn1_lab.InnerText = "";
            xn1_other.InnerText = ""; xn1_rad.InnerText = "";
            foreach(KeyValuePair<string,double> temp in globalvarible.possibility)
            {
                XmlElement xn5=xml.CreateElement("","DiseaseName","");
                xn5.InnerText=temp.Key;
                XmlElement xn51=xml.CreateElement("","ProbalityValue","");
                xn51.InnerText=temp.Value.ToString();
                xn1_diagnosis.AppendChild(xn5);
                xn1_diagnosis.AppendChild(xn51);
             }
            return(xml);
        }
        static void complete_asking()
        {
            foreach (ask temp in globalvarible.question)
            {
                if (temp.propertyname != "NULL")
                {
                    read_option(temp.findingtype,temp.findingname, temp.propertyname);
                }
            }

        }
        static void read_option(string findingtype,string findingname,string propertyname)
        {
            //从findingbase里提取optionname，并存储在globalvarible.newquestion里，为输出做准备;
            connect2sql conn = new connect2sql();
            conn.initialization();
            conn.connection.Open();
            string command = "select Description from FindingsBase where (ChineseName ='"+findingname+"')";
            SqlCommand mycommand = new SqlCommand();
            mycommand.CommandText = command;
            mycommand.Connection = conn.connection;
            XmlReader reader = mycommand.ExecuteXmlReader();
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();
            conn.connection.Close();
            XmlElement root = xml.DocumentElement;
            XmlNodeList xl1 = root.ChildNodes;
            string name = "";
            string tempop = "";
            List<string> option = new List<string>();
            #region
            foreach (XmlNode xn1 in xl1)
            {
                if (xn1.Name == "Properties")
                {
                    XmlNodeList xl2 = xn1.ChildNodes;
                    foreach (XmlNode xn2 in xl2)
                    {
                        XmlNodeList xl3 = xn2.ChildNodes;
                        foreach (XmlNode xn3 in xl3)
                        {
                            if (xn3.Name == "Name")
                            {
                                name = xn3.InnerText;
                                name = name.Trim();
                            }
                            if (xn3.Name == "Options" && name == propertyname)
                                foreach (XmlNode xn4 in xn3.ChildNodes)
                                {
                                    tempop = xn4.InnerText;
                                    //tempop.Replace(" ", "");
                                    tempop = tempop.Trim();
                                    option.Add(tempop);
                                }
                        }
                    }
                }

            }
            #endregion
            #region
            List<string> key=new List<string>();
            key.Add(findingtype);
            key.Add(findingname);
            newask tempnewask=new newask();
            tempnewask.propertyname=propertyname;
            tempnewask.optionname=option;
            if(globalvarible.newquestion.ContainsKey(key))
                globalvarible.newquestion[key].Add(tempnewask);
            else
            {
                List<newask> tempq=new List<newask>();
                tempq.Add(tempnewask);
                globalvarible.newquestion.Add(key,tempq);
            }
          #endregion
        }
        static int normalization()
        { 
           double sum_pos=0;
           int indicator=0;
           string temp1="";
           string temp2="";
           Dictionary<string, double> t =new Dictionary<string,double>();
           var dissort = from d in globalvarible.possibility orderby d.Value descending select d;
          // globalvarible.possibility.Clear();
           foreach (KeyValuePair<string, double> temp in dissort)
               t.Add(temp.Key, temp.Value);
           globalvarible.possibility = t;    
            foreach(string tempdis in globalvarible.possibility.Keys)
                sum_pos = sum_pos + globalvarible.possibility[tempdis];
            int count=0;
            for (int i = 0; i < globalvarible.possibility.Keys.Count;i++ )
            {
                List<string> keys = new List<string>(globalvarible.possibility.Keys);
                string tempdis = keys[i];
                count++;
                globalvarible.possibility[tempdis] = globalvarible.possibility[tempdis] / sum_pos;
                if (count == 1)
                    temp1 = tempdis;
                if (count == 2)
                    temp2 = tempdis;
            }
            if (count < 2) indicator = 1;
            else  if (globalvarible.possibility[temp1] > globalvarible.thres_deter && globalvarible.possibility[temp1] > globalvarible.multiple * globalvarible.possibility[temp2])
                indicator = 1;
            else
                indicator = 0;
            return (indicator);
        }
        public static XmlDocument diagnosis(XmlDocument xml)
        {
            //xmltest();
            //XmlDocument xml=new XmlDocument();
            //xml.Load("XmlInfoInput.xml");
            read_data(xml);    //将xml里的症候读入到globalvarible.signs里，方便后续处理;
            getdata();//依据globalvarible.signs里的症候，从数据库里提取相关疾病，并计算这些疾病的概率，用globalvarible.id记录已经被提取的症候;
            ask_finding();//依据globalvarible.disease里的信息进行询问;
            complete_asking();//从fingdingsbase表里提取property的option；
            XmlDocument outxml=output();
             return(outxml);
            //outxml.Save("output.xml");
            //System.Console.ReadLine();
        }
    }
}
