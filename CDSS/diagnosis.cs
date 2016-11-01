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
        public List<string> optionname;
    }
    public struct newask
    {
        public string propertyname;
        public List<string> optionname;
    }
    class globalvarible
    {
        public static double[] specifity = new double[6] { 0, 0.2, 0.3, 0.5, 0.8, 1 };
        public static double[] frequency = new double[6] { 0.5, 0.45, 0.4, 0.3, 0.2, 0 };
        public static double punish= 0.05;
        public static double none=0.5;
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
                            if (!xn3.HasChildNodes)  //没有子节点，即症候是两层；
                            {
                                temp2 = "NULL";
                                temp3 = "NULL";
                                detail temp = new detail();
                                temp.symptomname = temp1;
                                temp.propertyname = temp2;
                                temp.optionname = temp3;
                                globalvarible.signs.Add(temp);
                            }
                            else          //有节点，三层或不存在该症候;
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
                                    temp.propertyname = temp2;
                                    if (temp3 == "") temp3 = "NULL";
                                    temp.optionname = temp3;
                                    globalvarible.signs.Add(temp);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            getdata();
        }
        static void getdata()
        {
            //依据sign从数据库中读取相关疾病并计算概率；
            connect2sql conn = new connect2sql();
            conn.initialization();
            conn.connection.Open();
            string command;
            double count = 0;
            foreach (detail tempdetail in globalvarible.signs)
            {
                string temp1 = tempdetail.symptomname;
                string temp2 = tempdetail.propertyname;
                string temp3 = tempdetail.optionname;
                if (temp2 == "NULL" && temp3 == "NULL")
                {
                    count++;
                    command = "SELECT DiagnosisDataUID, DiseaseName, Specifity FROM DiagnosisData WHERE (FindingName='" + temp1 + "') AND (PropertyName IS NULL)";
                    add_disease(command,conn.connection,count);
                }
                else if (temp2 == "None" && temp3 == "NULL")
                {
                    add_disease_none(temp1,conn.connection);   
                }
                else
                {
                    count++;
                    command = "SELECT DiagnosisDataUID, DiseaseName, Specifity FROM DiagnosisData WHERE (FindingName='" + temp1 + "') AND (PropertyName='" + temp2 + "') AND (OptionName='" + temp3 + "')";
                    add_disease(command,conn.connection,count);
                }     
            }
          complete_disease(conn.connection);
          conn.connection.Close();
          ask_finding();
        }
        static void add_disease(string command,SqlConnection connection,double count)
        {
            //症候存在时，将症候的ID存入globalvarible.id ；将疾病的计算结果存储在globalvarible.possibility里；
            SqlCommand mycommand = new SqlCommand();
            mycommand.Connection = connection;
            mycommand.CommandText = command;
            SqlDataReader reader;
            reader = mycommand.ExecuteReader();
            
            #region
            while (reader.Read())
            {
                string tempdis;
                tempdis = reader["DiseaseName"].ToString();
                int specifity = reader.GetInt32(2);
                string id = reader["DiagnosisDataUID"].ToString();
                globalvarible.askedid.Add(id);
                if (globalvarible.possibility.ContainsKey(tempdis))
                {
                    //globalvarible.possibility已经存在该疾病;
                    (globalvarible.possibility)[tempdis] = (globalvarible.possibility)[tempdis] * globalvarible.specifity[specifity];
                    for (int i = 0; i < globalvarible.possibility.Keys.Count;i++ )
                    {
                        List<string> keys = new List<string>(globalvarible.possibility.Keys);
                        string key = keys[i];
                        if (key == tempdis)
                            continue;
                        globalvarible.possibility[key] = globalvarible.possibility[key] * globalvarible.punish;
                    }
                }
                else
                {
                    for(int i=0;i< globalvarible.possibility.Keys.Count;i++)
                    {
                        List<string> keys =new List<string>(globalvarible.possibility.Keys);
                        string key = keys[i];
                        globalvarible.possibility[key] = globalvarible.possibility[key] * globalvarible.punish;
                    }
                        
                    double punish = globalvarible.punish;
                    globalvarible.possibility.Add(tempdis, globalvarible.specifity[specifity]*Math.Pow(punish,count-1));
                }

            }
            #endregion
            reader.Close();
        }
        static void add_disease_none(string temp1,SqlConnection connection)
        {
            SqlCommand mycommand = new SqlCommand();
            string command = "SELECT DiagnosisDataUID, DiseaseName,Frequency FROM DiagnosisData WHERE (FindingName='" + temp1 + "') AND (PropertyName IS NOT NULL)";
            mycommand.CommandText = command;
            mycommand.Connection = connection;
            SqlDataReader reader;
            SqlDataReader tempreader=mycommand.ExecuteReader();
            bool indicator = tempreader.HasRows;
            tempreader.Close();
            if(!indicator)
            {
                command= "SELECT DiagnosisDataUID, DiseaseName,Frequency FROM DiagnosisData WHERE (FindingName=" + temp1 + ") AND (PropertyName IS NULL)";
                mycommand.CommandText=command;
            }
            reader = mycommand.ExecuteReader();  
              List<string> nonedis = new List<string>();
              while (reader.Read())
              {
                string id = reader["DiagnosisDataUID"].ToString();
                globalvarible.askedid.Add(id);
                string tempdis = reader["DiseaseName"].ToString();
                int frequency = reader.GetInt32(2);
                if (globalvarible.possibility.ContainsKey(tempdis))
                {
                    globalvarible.possibility[tempdis] = globalvarible.possibility[tempdis] * globalvarible.frequency[frequency];
                    nonedis.Add(tempdis);
                }
               }
              for (int i = 0; i < globalvarible.possibility.Keys.Count; i++)
              {
                  List<string> keys = new List<string>(globalvarible.possibility.Keys);
                  string tempnone = keys[i];
                    if (!nonedis.Contains(tempnone))
                        globalvarible.possibility[tempnone] = globalvarible.possibility[tempnone] * globalvarible.none;
                }
              reader.Close();
              mycommand.Connection.Close();
              connection.Open();
              }
        static void complete_disease(SqlConnection connection)
        {
            //读取相关疾病未曾提到的症候,放入globalvarabile.disease；疾病作为key；
            SqlCommand mycommand = new SqlCommand();
            mycommand.Connection = connection;
            string command;
            SqlDataReader reader;
           for(int i=0;i< globalvarible.possibility.Keys.Count;i++)
           {
               List<string> keys = new List<string>(globalvarible.possibility.Keys);
               string tempdis = keys[i];
               command = "SELECT * FROM DiagnosisData WHERE (DiseaseName='" + tempdis + "') AND (PropertyName IS NOT NULL)";
               mycommand.CommandText=command;
               SqlDataReader tempreader = mycommand.ExecuteReader();
               bool indicator = tempreader.HasRows;
               tempreader.Close();
               if(!indicator)
               {
                   command = "SELECT * FROM DiagnosisData WHERE (DiseaseName='" + tempdis + "') AND (PropertyName IS NULL)";
                   mycommand.CommandText = command;
               }
                   reader = mycommand.ExecuteReader();
                   #region
                   while (reader.Read())
                   {
                       string id = reader["DiagnosisDataUID"].ToString();
                       if (!globalvarible.askedid.Contains(id))
                       {
                           symbol tempsym = new symbol();
                           tempsym.id = reader["DiagnosisDataUID"].ToString();
                           tempsym.findingtype = reader["FindingType"].ToString();
                           tempsym.findingname = reader["FindingName"].ToString();
                           if (reader.IsDBNull(4) || reader.IsDBNull(5))
                           {
                               tempsym.propertyname = "NULL";
                               tempsym.optionname = "NULL";
                           }
                           else
                           {
                               tempsym.propertyname = reader["PropertyName"].ToString();
                               tempsym.optionname = reader["OptionName"].ToString();
                           }
                           tempsym.frequency = reader.GetInt32(6);
                           tempsym.specifity = reader.GetInt32(7);
                           if (globalvarible.disease.ContainsKey(tempdis))
                               (globalvarible.disease)[tempdis].Add(tempsym);
                           else
                           {
                               List<symbol> templist = new List<symbol>();
                               templist.Add(tempsym);
                               globalvarible.disease.Add(tempdis, templist);
                           }
                       }
                   }
                   reader.Close();
                   #endregion
           }
        }
        static void ask_finding()
        {
            //查找需要询问的finding;
            int indicator;
            int num_thres=0;
            double sum_thres = 0;//统计前n个概率之和>0.95,sum_thres=n;
           indicator=normalization();
           if (indicator>0)
               return;
           foreach (string tempdis in globalvarible.possibility.Keys)
           {
               sum_thres+=globalvarible.possibility[tempdis];
               num_thres++;
               if (sum_thres > globalvarible.thres)
                   break;
           }
           if (num_thres > 5)
               disease_remove(num_thres);
           else
               max_first(num_thres);
           complete_asking();
          // output();            
        }
        static void disease_remove(int num)
        {
            //有很多候选疾病，依据排除疾病的方法;
            Dictionary<List<string>, int> total_sign = new Dictionary<List<string>, int>();
            string temp1 = "";//findingname;
            string temp2 = "";//propertyname;
            //total_sign 统计每个<findingname,propertyname>出现的个数;
            #region
            foreach (string key in globalvarible.possibility.Keys)
            {
                if (num <= 0)
                    break;
                num--;
                foreach (symbol sign in globalvarible.disease[key])
                {
                    if (globalvarible.count < 2 && sign.findingtype == "Laboratory")
                        continue;
                    temp1 = sign.findingname;
                    temp2 = sign.propertyname;
                    List<string> temp = new List<string>();
                    temp.Add(temp1);
                    temp.Add(temp2);
                    if (total_sign.ContainsKey(temp))
                        total_sign[temp]++;
                    else
                        total_sign.Add(temp, 1);
                }
            }
            var dissort = from objDic in total_sign orderby objDic.Value descending select objDic;//对value排序
            total_sign.Clear();
            foreach (KeyValuePair<List<string>, int> temp4 in dissort)
            {
                total_sign.Add(temp4.Key, temp4.Value);
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
                    ask temp4 = new ask();
                    temp4.findingname = temp1;
                    temp4.propertyname = temp2;
                    temp4.optionname = new List<string>();
                    globalvarible.question.Add(temp4);
                }
            }
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
                        count = add_max(count, key);
                        indicator1++;
                    }
                else if (indicator2 == 0)
                    while (indicator1 < 2)
                    {
                        count = add_max(count, key);
                        indicator1++;
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
                ask ask_temp1 = new ask();
                ask_temp1.propertyname = temp5.propertyname;
                ask_temp1.findingname = temp5.findingname;
                ask_temp1.findingtype = temp5.findingtype;
                ask_temp1.optionname = new List<string>();
                if (!globalvarible.question.Contains(ask_temp1) && temp5.frequency > max_frequency)
                {
                    max_frequency = temp5.frequency;
                    temp1 = temp5.findingname;
                    temp2 = temp5.propertyname;
                    temp3 = temp5.findingtype;
                }
            }
            if (max_frequency != 0)
            {
                ask ask_temp2 = new ask();
                ask_temp2.propertyname = temp2;
                ask_temp2.findingname = temp1;
                ask_temp2.findingtype = temp3;
                ask_temp2.optionname = new List<string>();
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
                }
                else if(tempkey[0]=="Sign")
                {
                    temp1="Sign";
                    temp2="SignName";
                }
                 else if(tempkey[0]=="Predisposition")
                {
                     temp1="Predisposition";
                     temp2="PredispositionName";
                 }
                else
                {
                     temp1="Laboratory";
                     temp2="LaboratoryName";
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
            connect2sql conn = new connect2sql();
            conn.initialization();
            conn.connection.Open();
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
            connect2sql conn = new connect2sql();
            conn.initialization();
            conn.connection.Open();
            string command = "select Description from FindingsBase where (ChineseName ='"+findingname+"')";
            SqlCommand mycommand = new SqlCommand();
            mycommand.CommandText = command;
            mycommand.Connection = conn.connection;
            XmlReader reader = mycommand.ExecuteXmlReader();
            conn.connection.Close();
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
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
            if (globalvarible.possibility[temp1] > globalvarible.thres_deter && globalvarible.possibility[temp1] > globalvarible.multiple * globalvarible.possibility[temp2])
                indicator = 1;
            else
                indicator = 0;
            return (indicator);
        }
     public static xml Main(string[] args)
        {
            //xmltest();
            XmlDocument xml=new XmlDocument();
            xml.Load("test.xml");
            read_data(xml);    
          XmlDocument outxml=output();
            outxml.Save("output.xml");
            System.Console.ReadLine();
        }
    }
}
