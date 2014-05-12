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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserName.Page.SetFocus(UserName);
        Menu myMenu = null;
        myMenu = (Menu)Master.FindControl("Menu1");
        if (myMenu != null)
        {
            myMenu.Visible = true;
        }

    }
    protected void CommandBtn_Click(Object sender, CommandEventArgs e)
    {
        LoginProcess();
    }
    protected void LoginProcess()
    {
        try
        {
            //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionStr);
            string sqlStatement;

            sqlStatement =
            sqlStatement =
                "select pub_user_id, l_role_id, last_name, first_name" +
                " from pub_user" +
                " where user_name = '" +
                UserName.Text +
                "' and password = '" +
                Password.Text + "'";

            SqlCommand command = new SqlCommand(sqlStatement, conn);

            SqlDataReader myReader;

            conn.Open();
            myReader = command.ExecuteReader();
            string lnameStr = "";
            string fnameStr = "";
            string userIdStr = "";
            string roleIdStr = "";
            try
            {
                while (myReader.Read())
                {
                    lnameStr = myReader["last_name"].ToString();
                    fnameStr = myReader["first_name"].ToString();
                    userIdStr = myReader["pub_user_id"].ToString();
                    roleIdStr = myReader["l_role_id"].ToString();
                }
            }
            finally
            {
                myReader.Close();
                conn.Close();
            }

            if (userIdStr != "")
            {
                Session["userId"] = userIdStr;
                ErrorMessage.Text = fnameStr + " " + lnameStr + ", you have logged in";
                UserName.Text = "";
                Session["roleId"] = roleIdStr;
                Response.Redirect("Default.aspx");
            }
            else
            {
                ErrorMessage.Text = "Login failed, please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = ex.Message;
        }

    }
    protected void btnGuest_Click(object sender, EventArgs e)
    {
        Session["userId"] = "";
        Session["roleId"] = "";
        Response.Redirect("Default.aspx");

    }
}