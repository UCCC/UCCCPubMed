using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string memberIdStr = (string)Session["memberId"];

        string roleIdStr = (string)Session["roleId"];

        if (roleIdStr == "1" || roleIdStr == "2" || roleIdStr == "3" || roleIdStr == "4")
        {
            ErrorMessage.Text = "";
            LoginBanner1.Visible = false;
        }
        else
        {
            ErrorMessage.Text = "To see the contents in this page, please login.";
            //string msg = "To see the contents in this page, please login.";
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        }


    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        string userNameStr = txtUserName.Text;
        string oldPwdStr = txtOldPwd.Text;
        string newPwdStr = txtNewPwd.Text;
        string confirmPwdStr = txtConfirmPwd.Text;

        if (newPwdStr == confirmPwdStr)
        {
            //ConfigurationManager.AppSettings.Get("ConnectionString");
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection conn = new SqlConnection(connectionStr);
            string sqlStatement;

            sqlStatement = "select count(*) as cnt from member where" +
                " user_name = '" +
                userNameStr +
                "' and password = '" +
                newPwdStr +
                "'";
            SqlCommand commandScalar = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int cnt = Convert.ToInt32(commandScalar.ExecuteScalar());
            conn.Close();
            if (cnt > 0)
            {
                Response.Write("<script language=JavaScript> alert('The combination of user name and password was used.'); </script>");
                return;
            }

            sqlStatement =
                "update member set password = '" +
                newPwdStr +
                "' where user_name = '" +
                userNameStr +
                "' and password = '" +
                oldPwdStr +
                "'";

            SqlCommand command = new SqlCommand(sqlStatement, conn);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
            Response.Write("<script language=JavaScript> alert('Password is changed.'); </script>");
        }
        else
        {
            Response.Write("<script language=JavaScript> alert('Confirmation failed.'); </script>");
        }

    }
}