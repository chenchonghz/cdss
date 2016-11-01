using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace CDSS
{
    public partial class PersonalHealthRecord : System.Web.UI.Page
    {
        //SqlConnection CurSqlConnection; //公用数据库连接
        public static string CDSSConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CDSSConnectionString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                //CurSqlConnection = new SqlConnection(CDSSConnectionString);
                if (Session["type"].ToString() == "Commonuser")//判断登陆者是否为普通用户
                {
                    if (ExistCurrentCommonUserPhr())
                    {
                        DisplayUserBasicalInfo();
                    }
                    else
                    {
                        string MsgInfo=UserCompletedBasicalInfo();
                        if (MsgInfo == "")
                        {
                            ImportCommonUsersInfo();
                        }
                        else
                        {
                            Response.Write("<script>window.alert('" + MsgInfo + "');</script>");
                            Response.Redirect("~/CommonUsers/CommonUsersInfo.aspx");//若不是普通用户，跳转至主页
                        }

                    }                    
                }
                else
                {
                    Response.Write("<script>alert('请先以普通用户身份登录！')</script>");
                    Response.Redirect("Default.aspx");//若不是普通用户，跳转至主页
                }
            }
        }

        private void DisplayUserBasicalInfo()
        {
            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "Select [PhrUID],[CommonUserUID],[LoginName],[Name],[Sex],[DOB],"+
                "[Occupation],[PhrNumber] From CommonUsersPhr where (LoginName=@UserName)";
            SqlCommand command = new SqlCommand(strSQL, CurSqlConnection);
            command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
            SqlDataReader DataReader = command.ExecuteReader();
            if (DataReader.Read())
            {
                LiteralLoginName.Text = DataReader["LoginName"].ToString().Trim();
                LiteralPhrUID.Text = DataReader["PhrUID"].ToString().Trim();
                LiteralUserUID.Text = DataReader["CommonUserUID"].ToString().Trim();
                LiteralPhrNumber.Text = DataReader["PhrNumber"].ToString().Trim();

                TextBoxName.Text = DataReader["Name"].ToString().Trim();
                TextBoxSex.Text = DataReader["Sex"].ToString().Trim();

                string StrDOB;
                DateTime DateDOB;
                StrDOB = DataReader["DOB"].ToString().Trim();
                DateDOB = Convert.ToDateTime(StrDOB);
                TextBoxDOB.Text = DateDOB.ToString("yyyy-MM-dd");
                TextBoxOccupation.Text = DataReader["Occupation"].ToString().Trim();
            }
            else
            {
                //There must be some errors here.
            }
            CurSqlConnection.Close();
        }

        private void ImportCommonUsersInfo()
        {
            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "Select [CommonUserUID],[LoginName],[Name],[Sex],[DOB],[Occupation] " +
                " From CommonUserInfo where (LoginName=@UserName)";
            SqlCommand command = new SqlCommand(strSQL, CurSqlConnection);
            command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
            SqlDataReader DataReader = command.ExecuteReader();
            if (DataReader.Read())
            {
                LiteralPhrUID.Text = "";

                LiteralLoginName.Text = DataReader["LoginName"].ToString().Trim();
                LiteralUserUID.Text = DataReader["CommonUserUID"].ToString().Trim();

                TextBoxName.Text = DataReader["Name"].ToString().Trim();
                TextBoxSex.Text = DataReader["Sex"].ToString().Trim();

                string StrDOB;
                DateTime DateDOB;
                StrDOB = DataReader["DOB"].ToString().Trim();
                DateDOB = Convert.ToDateTime(StrDOB);
                TextBoxDOB.Text = DateDOB.ToString("yyyy-MM-dd");
                TextBoxOccupation.Text = DataReader["Occupation"].ToString().Trim();

                LiteralPhrNumber.Text = CreateNewPhrNumber(StrDOB);

            }
            else
            {
                //There must be some errors here.
            }
            CurSqlConnection.Close();
        }

        private string CreateNewPhrNumber(string UsersDOB)
        {
            string NewPhrNumber;
            DateTime BirthDate = Convert.ToDateTime(UsersDOB);
            string BirthDateStr = BirthDate.ToString("yyyyMMdd");
            string SuffixNumber = GetSuffixNumber(BirthDateStr);
            NewPhrNumber = BirthDateStr + SuffixNumber;
            return NewPhrNumber;
        }

        private string GetSuffixNumber(string BirthDateStr)
        {
            string SuffixNumber = "0001";
            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "Select top 1 PhrNumber From CommonUsersPhr where (PhrNumber like '%" +
                BirthDateStr + "%') order by PhrNumber desc";
            SqlCommand command = new SqlCommand(strSQL, CurSqlConnection);
            SqlDataReader DataReader = command.ExecuteReader();
            string LastSuffix;
            if (DataReader.Read())
            {
                LastSuffix = DataReader["PhrNumber"].ToString().Trim();
                SuffixNumber = IncreaseSuffixNumber(LastSuffix);
            }
            //SuffixNumber = IncreaseSuffixNumber(SuffixNumber);
            CurSqlConnection.Close();
            return SuffixNumber;
        }

        private string IncreaseSuffixNumber(string LastSuffix)
        {
            string NumberSerial="0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string NewSuffix=LastSuffix;
            string tmpStr;
            int p1, p2, p3, p4;
            p1 = NumberSerial.IndexOf(LastSuffix[0]);
            p2 = NumberSerial.IndexOf(LastSuffix[1]);
            p3 = NumberSerial.IndexOf(LastSuffix[2]);
            p4 = NumberSerial.IndexOf(LastSuffix[3]);
            if (p4==35) //等于最后一个字母Z
            {
                //lcf tobecontinued 201412211146
            }
            else
            {
                NewSuffix = NewSuffix.Remove(3, 1);
                tmpStr = NumberSerial.Substring(p4 + 1, 1);
                NewSuffix = NewSuffix.Insert(3, tmpStr);
            }
            return NewSuffix;
        }
        private string UserCompletedBasicalInfo()
        {
            string CompletedInfo = "";
            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "Select [CommonUserUID],[Name],[Sex],[DOB],[Occupation] "+
                " From CommonUserInfo where (LoginName=@UserName)";
            SqlCommand command = new SqlCommand(strSQL, CurSqlConnection);
            command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
            SqlDataReader DataReader = command.ExecuteReader();
            if (DataReader.Read())
            {
                string CommonUserUID = DataReader["CommonUserUID"].ToString().Trim();
                string Name = DataReader["Name"].ToString().Trim();
                string Sex = DataReader["Sex"].ToString().Trim();
                string DOB = DataReader["DOB"].ToString().Trim();
                string Occupation = DataReader["Occupation"].ToString().Trim();
                if (CommonUserUID=="")
                {
                    CompletedInfo = "系统错误，用户唯一编号为空，请联系系统管理员！";
                }
                if ((Name == "") || (Sex == "") || (DOB == "") || (Occupation == ""))
                {
                     CompletedInfo = "建立个人健康档案之前，必须提供个人姓名、性别、出生日期和职业等疾病信息！";
                }
            }
            else
            {
                CompletedInfo="系统错误，请联系系统管理员！";
            }
            CurSqlConnection.Close();
            return CompletedInfo;
        }
        private bool ExistCurrentCommonUserPhr()
        {
            bool PhrExist = false;
            //SqlConnection CurConn = CurSqlConnection;
            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString); 
            CurSqlConnection.Open();
            string strSQL = "Select top 1 * From CommonUsersPhr where (LoginName=@UserName)";
            SqlCommand command = new SqlCommand(strSQL, CurSqlConnection);
            command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
            SqlDataReader DataReader = command.ExecuteReader();
            if (DataReader.Read())
            {
                PhrExist = true;
            }
            CurSqlConnection.Close();
            return PhrExist;
        }

        protected void ButtonSaveBasicalInfo_Click(object sender, EventArgs e)
        {
            string CurPhrUID = LiteralPhrUID.Text.Trim();
            if (CurPhrUID == "")
            {
                InsertNewPersonalRecord();
                LiteralPhrUID.Text = GetCurrentUserPhrUID();
            }
            else
            {
                UpdateOldPersonalRecord();
            }
        }

        private string GetCurrentUserPhrUID()
        {
            string CurrentUserPhrUID = "";
            string LoginName = LiteralLoginName.Text.Trim();
            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "SELECT [PhrUID] FROM [CommonUsersPhr] where ([LoginName]=@LoginName)";
            SqlCommand CurCommand = new SqlCommand(strSQL, CurSqlConnection);
            CurCommand.Parameters.AddWithValue("@LoginName", LoginName);
            SqlDataReader DataReader = CurCommand.ExecuteReader();
            if (DataReader.Read())
            {
                CurrentUserPhrUID = DataReader["PhrUID"].ToString().Trim();
            }
            CurSqlConnection.Close();
            return CurrentUserPhrUID;
        }
        private void InsertNewPersonalRecord()
        {
            string CommonUserUID = LiteralUserUID.Text.Trim();
            string LoginName = LiteralLoginName.Text.Trim();
            string Name = TextBoxName.Text.Trim();
            string Sex = TextBoxSex.Text.Trim();
            string DOB = TextBoxDOB.Text.Trim();
            string Occupation = TextBoxOccupation.Text.Trim();
            string PhrNumber = LiteralPhrNumber.Text.Trim();

            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "INSERT INTO [CommonUsersPhr]([CommonUserUID],[LoginName],[Name] ,[Sex],[DOB],[Occupation],[PhrNumber])" +
                "VALUES(@CommonUserUID,@LoginName,@Name ,@Sex,@DOB,@Occupation,@PhrNumber)";
            SqlCommand CurCommand = new SqlCommand(strSQL, CurSqlConnection);
            CurCommand.Parameters.AddWithValue("@CommonUserUID", CommonUserUID);
            CurCommand.Parameters.AddWithValue("@LoginName", LoginName);
            CurCommand.Parameters.AddWithValue("@Name", Name);
            CurCommand.Parameters.AddWithValue("@Sex", Sex);
            CurCommand.Parameters.AddWithValue("@DOB", DOB);
            CurCommand.Parameters.AddWithValue("@Occupation", Occupation);
            CurCommand.Parameters.AddWithValue("@PhrNumber", PhrNumber);
            CurCommand.ExecuteNonQuery();
            CurSqlConnection.Close();
        }
        private void UpdateOldPersonalRecord()
        {
            string PhrUID = LiteralPhrUID.Text.Trim();
            string Name = TextBoxName.Text.Trim();
            string Sex = TextBoxSex.Text.Trim();
            string DOB = TextBoxDOB.Text.Trim();
            string Occupation = TextBoxOccupation.Text.Trim();

            SqlConnection CurSqlConnection = new SqlConnection(CDSSConnectionString);
            CurSqlConnection.Open();
            string strSQL = "UPDATE [CommonUsersPhr] SET [Name] = @Name,[Sex] = @Sex,[DOB] = @DOB,"+
                "[Occupation] = @Occupation  WHERE ([PhrUID]=@PhrUID)";
            SqlCommand CurCommand = new SqlCommand(strSQL, CurSqlConnection);
            CurCommand.Parameters.AddWithValue("@PhrUID", PhrUID);
            CurCommand.Parameters.AddWithValue("@Name", Name);
            CurCommand.Parameters.AddWithValue("@Sex", Sex);
            CurCommand.Parameters.AddWithValue("@DOB", DOB);
            CurCommand.Parameters.AddWithValue("@Occupation", Occupation);
            CurCommand.ExecuteNonQuery();
            CurSqlConnection.Close();
        }
    }
}