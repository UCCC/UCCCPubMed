using System;
using System.Drawing;
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

public partial class programLeader : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement = "";
        if (!IsPostBack)
        {
            sqlStatement =
                "select pl.program_leader_id," +
                " pl.l_program_id," +
                " right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name as program," +
                " pl.client_id," +
                " c.last_name + ', ' + c.first_name as client" +
                " from program_leader pl" +
                " inner join l_program lp" +
                " on pl.l_program_id = lp.l_program_id" +
                " inner join client c" +
                " on pl.client_id = c.client_id";

            Helper.BindGridview(sqlStatement, gvProgramLeader);
            EmptyGridFix(gvProgramLeader);

        }
        string memberIdStr = (string)Session["memberId"];

        string roleIdStr = (string)Session["roleId"];

        if (roleIdStr == "1" || roleIdStr == "2" || roleIdStr == "3")
        {
            ErrorMessage.Text = "";
        }
        else
        {
            //ErrorMessage.Text = "To see the contents in this page, please login.";
            //string msg = "To see the contents in this page, please login.";
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        }
    }
    /// ==========================================================================
    /// <summary>
    /// Show grid even if datasource is empty
    /// <param name="grdView">GridView</param>
    /// ==========================================================================
    protected void EmptyGridFix(GridView grdView)
    {
        // normally executes after a grid load method
        if (grdView.Rows.Count == 0 &&
            grdView.DataSource != null)
        {
            DataTable dt = null;

            // need to clone sources otherwise it will be indirectly adding to 
            // the original source

            if (grdView.DataSource is DataSet)
            {
                dt = ((DataSet)grdView.DataSource).Tables[0].Clone();
            }
            else if (grdView.DataSource is DataTable)
            {
                dt = ((DataTable)grdView.DataSource).Clone();
            }

            else if (grdView.DataSource is SqlDataSource)
            {
                SqlDataSource sds = new SqlDataSource();
                DataView dv = new DataView();
                sds = (SqlDataSource)grdView.DataSource;
                dv = (DataView)sds.Select(DataSourceSelectArguments.Empty);
                dt = dv.ToTable().Clone();
                /*
                DataView dv = new DataView(); 
                DataTable dt = new DataTable(); 
                dv = mySQLDataSource.Select(DataSourceSelectArguments.Empty); 
                dt = dv.ToTable();
                 * */
            }
            if (dt == null)
            {
                return;
            }

            dt.Rows.Add(dt.NewRow()); // add empty row
            grdView.DataSource = dt;
            grdView.DataBind();

            // hide row
            grdView.Rows[0].Visible = false;
            grdView.Rows[0].Controls.Clear();
        }

        // normally executes at all postbacks
        if (grdView.Rows.Count == 1 &&
            grdView.DataSource == null)
        {
            bool bIsGridEmpty = true;

            // check first row that all cells empty
            for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
            {
                if (grdView.Rows[0].Cells[i].Text != string.Empty)
                {
                    bIsGridEmpty = false;
                }
            }
            // hide row
            if (bIsGridEmpty)
            {
                grdView.Rows[0].Visible = false;
                grdView.Rows[0].Controls.Clear();
            }
        }
    }
    protected void gvProgramLeader_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbEditTemp = null;
            lbEditTemp = (LinkButton)e.Row.FindControl("lnkEdit");
            LinkButton lbDeleteTemp = null;
            lbDeleteTemp = (LinkButton)e.Row.FindControl("lnkDelete");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            string currProgramStr = drv["program"].ToString();
            DropDownList ddlProgramTemp = null;
            ddlProgramTemp = (DropDownList)e.Row.FindControl("ddlProgram");
            if (ddlProgramTemp != null)
            {
                LoadLookup.LoadProgram(ddlProgramTemp, currProgramStr,false);
            }
            string currClientStr = drv["client"].ToString();
            DropDownList ddlMemberTemp = null;
            ddlMemberTemp = (DropDownList)e.Row.FindControl("ddlMember");
            if (ddlMemberTemp != null)
            {
                LoadLookup.LoadMember(ddlMemberTemp, currClientStr,"01/01/2000","01/01/2015");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            LinkButton lbAddTemp = null;
            lbAddTemp = (LinkButton)e.Row.FindControl("lnkAdd");
            if (lbAddTemp == null)
            {
                return;
            }
            DropDownList ddlProgramTemp = null;
            ddlProgramTemp = (DropDownList)e.Row.FindControl("ddlNewProgram");
            if (ddlProgramTemp == null)
            {
                return;
            }
            else
            {
                LoadLookup.LoadProgram(ddlProgramTemp, "xxx", false);
            }
            DropDownList ddlMemberTemp = null;
            ddlMemberTemp = (DropDownList)e.Row.FindControl("ddlNewMember");
            if (ddlMemberTemp == null)
            {
                return;
            }
            else
            {
                LoadLookup.LoadMember(ddlMemberTemp, "xxx","01/01/2000","01/01/2015");
            }

        }
    }
    protected void gvProgramLeader_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvProgramLeader.EditIndex = -1;
        FillProgramLeaderGrid();
    }
    protected void gvProgramLeader_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvProgramLeader.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        DropDownList ddlProgramTemp = null;
        ddlProgramTemp = (DropDownList)gvProgramLeader.Rows[e.RowIndex].FindControl("ddlProgram");
        if (ddlProgramTemp == null)
        {
            return;
        }
        DropDownList ddlMemberTemp = null;
        ddlMemberTemp = (DropDownList)gvProgramLeader.Rows[e.RowIndex].FindControl("ddlMember");
        if (ddlMemberTemp == null)
        {
            return;
        }

        string sqlStatement =
            "Update program_leader" +
            " SET l_program_id=@l_program_id," +
            " client_id=@client_id" +
            " WHERE (program_leader_id = @program_leader_id)";

        //idStr;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter l_program_idParameter = new SqlParameter();
        l_program_idParameter.ParameterName = "@l_program_id";
        l_program_idParameter.SqlDbType = SqlDbType.Int;
        l_program_idParameter.Value = ddlProgramTemp.SelectedValue;
        command.Parameters.Add(l_program_idParameter);

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        client_idParameter.Value = ddlProgramTemp.SelectedValue;
        command.Parameters.Add(client_idParameter);

        SqlParameter program_leader_idParameter = new SqlParameter();
        program_leader_idParameter.ParameterName = "@program_leader_id";
        program_leader_idParameter.SqlDbType = SqlDbType.Int;
        program_leader_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(program_leader_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvProgramLeader.EditIndex = -1;

        FillProgramLeaderGrid();

        if (e.RowIndex > 0)
        {
            return; // RowIndex=0 is the row we want to insert
        }
        System.Collections.Hashtable h =
                    new System.Collections.Hashtable();

        foreach (System.Collections.DictionaryEntry x in e.NewValues)
        {
            h[x.Key] = x.Value;
        }

    }
    protected void gvProgramLeader_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
    {
        String tempStr = "";

        // Append the original field values to the log text.
        foreach (DictionaryEntry valueEntry in e.OldValues)
        {
            tempStr += valueEntry.Key + "=" + valueEntry.Value + ";";
        }

        if (e.Exception == null)
        {
            // The update operation succeeded. Clear the message label.
            //ErrorMessage.Text = "";
        }
        else
        {
            // The update operation failed. Display an error message.
            //ErrorMessage.Text = e.AffectedRows.ToString() + " rows updated. " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

    }
    protected void gvProgramLeader_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvProgramLeader.EditIndex = e.NewEditIndex;
        FillProgramLeaderGrid();
    }
    protected void gvProgramLeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvProgramLeader.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }
        //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement =
            " delete from pub_user where client_id = (select client_id from program_leader where program_leader_id = " + idStr + ")"; ;

        SqlCommand myCommandPubUser = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        myCommandPubUser.ExecuteNonQuery();
        myConnection.Close();

        sqlStatement =
            " delete from program_leader where program_leader_id = " + idStr;

        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        FillProgramLeaderGrid();

    }
    public void FillProgramLeaderGrid()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select pl.program_leader_id," +
            " pl.l_program_id," +
            " right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name as program," +
            " pl.client_id," +
            " c.last_name + ', ' + c.first_name as client" +
            " from program_leader pl" +
            " inner join l_program lp" +
            " on pl.l_program_id = lp.l_program_id" +
            " inner join client c" +
            " on pl.client_id = c.client_id";

        SqlDataSource dsProgramLeader = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsProgramLeader;
        gvProgramLeader.DataSource = dsProgramLeader;
        gvProgramLeader.DataBind();

        DataView dv = (DataView)dsProgramLeader.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            gvProgramLeader.EditIndex = dv.Count - 1;
        }
        else
        {
        }
        EmptyGridFix(gvProgramLeader);
    }
    protected void gvProgramLeader_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            DropDownList ddlNewProgramTemp = null;
            ddlNewProgramTemp = (DropDownList)gvProgramLeader.FooterRow.FindControl("ddlNewProgram");
            if (ddlNewProgramTemp == null)
            {
                return;
            }
            DropDownList ddlNewMemberTemp = null;
            ddlNewMemberTemp = (DropDownList)gvProgramLeader.FooterRow.FindControl("ddlNewMember");
            if (ddlNewMemberTemp == null)
            {
                return;
            }

            sqlStatement =
                "select count(*) from program_leader" +
                " where l_program_id = " +
                ddlNewProgramTemp.SelectedValue.ToString() +
                " and client_id = " +
                ddlNewMemberTemp.SelectedValue.ToString();


            SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            myConnection.Close();

            if (existingCnt > 0)
            {
                //string msg = "There is an existing focus group in focus group list. Please give another focus group name.";
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                //txtNewDescriptionTemp.Text = "";
                return;
            }

            sqlStatement =
                " insert into program_leader" +
                " (l_program_id, client_id) values (" +
                ddlNewProgramTemp.SelectedValue.ToString() +
                "," +
                ddlNewMemberTemp.SelectedValue.ToString() +
                ")";

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            string name = ddlNewMemberTemp.SelectedItem.ToString();
            string lastName = name.Substring(0, name.IndexOf(','));
            string firstName = name.Substring(name.IndexOf(',') + 2);
            sqlStatement =
                " insert into pub_user" +
                " (last_name, first_name, l_role_id, user_name, password,client_id) values ('" +
                lastName +
                "','" +
                firstName +
                "', 2, '" +
                lastName +
                "','" +
                lastName +
                "_1'," +
                ddlNewMemberTemp.SelectedValue.ToString() +
                ")";

            SqlCommand myCommandPubUser = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            myCommandPubUser.ExecuteNonQuery();
            myConnection.Close();

            gvProgramLeader.EditIndex = -1;
            FillProgramLeaderGrid();
        }

    }
}