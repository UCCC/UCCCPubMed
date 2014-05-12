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

public partial class CollectiveNameClient : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement = "";
        if (!IsPostBack)
        {
            sqlStatement =
                "select pl.collective_name_client_id," +
                " pl.l_collective_name_id," +
                " lp.collective_name," +
                " pl.client_id," +
                " c.last_name + ', ' + c.first_name as client" +
                " from collective_name_client pl" +
                " inner join l_collective_name lp" +
                " on pl.l_collective_name_id = lp.l_collective_name_id" +
                " inner join client c" +
                " on pl.client_id = c.client_id";

            Helper.BindGridview(sqlStatement, gvCollectiveNameClient);
            EmptyGridFix(gvCollectiveNameClient);

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
    protected void gvCollectiveNameClient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbEditTemp = null;
            lbEditTemp = (LinkButton)e.Row.FindControl("lnkEdit");
            LinkButton lbDeleteTemp = null;
            lbDeleteTemp = (LinkButton)e.Row.FindControl("lnkDelete");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            string currCollectiveNameStr = drv["collective_name"].ToString();
            DropDownList ddlCollectiveNameTemp = null;
            ddlCollectiveNameTemp = (DropDownList)e.Row.FindControl("ddlCollectiveName");
            if (ddlCollectiveNameTemp != null)
            {
                LoadLookup.LoadCollectiveName(ddlCollectiveNameTemp, currCollectiveNameStr);
            }
            string currClientStr = drv["client"].ToString();
            DropDownList ddlMemberTemp = null;
            ddlMemberTemp = (DropDownList)e.Row.FindControl("ddlMember");
            if (ddlMemberTemp != null)
            {
                LoadLookup.LoadMember(ddlMemberTemp, currClientStr, "01/01/2000", "01/01/2015");
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
            DropDownList ddlCollectiveNameTemp = null;
            ddlCollectiveNameTemp = (DropDownList)e.Row.FindControl("ddlNewCollectiveName");
            if (ddlCollectiveNameTemp == null)
            {
                return;
            }
            else
            {
                LoadLookup.LoadCollectiveName(ddlCollectiveNameTemp, "xxx");
            }
            DropDownList ddlMemberTemp = null;
            ddlMemberTemp = (DropDownList)e.Row.FindControl("ddlNewMember");
            if (ddlMemberTemp == null)
            {
                return;
            }
            else
            {
                LoadLookup.LoadMember(ddlMemberTemp, "xxx", "01/01/2000", "01/01/2015");
            }

        }
    }
    protected void gvCollectiveNameClient_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCollectiveNameClient.EditIndex = -1;
        FillCollectiveNameClientGrid();
    }
    protected void gvCollectiveNameClient_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvCollectiveNameClient.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        DropDownList ddlCollectiveNameTemp = null;
        ddlCollectiveNameTemp = (DropDownList)gvCollectiveNameClient.Rows[e.RowIndex].FindControl("ddlCollectiveName");
        if (ddlCollectiveNameTemp == null)
        {
            return;
        }
        DropDownList ddlMemberTemp = null;
        ddlMemberTemp = (DropDownList)gvCollectiveNameClient.Rows[e.RowIndex].FindControl("ddlMember");
        if (ddlMemberTemp == null)
        {
            return;
        }

        string sqlStatement =
            "Update collective_name_client" +
            " SET l_collective_name_id=@l_collective_name_id," +
            " client_id=@client_id" +
            " WHERE (collective_name_client_id = @collective_name_client_id)";

        //idStr;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter l_collective_name_idParameter = new SqlParameter();
        l_collective_name_idParameter.ParameterName = "@l_collective_name_id";
        l_collective_name_idParameter.SqlDbType = SqlDbType.Int;
        l_collective_name_idParameter.Value = ddlCollectiveNameTemp.SelectedValue;
        command.Parameters.Add(l_collective_name_idParameter);

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        client_idParameter.Value = ddlCollectiveNameTemp.SelectedValue;
        command.Parameters.Add(client_idParameter);

        SqlParameter collective_name_client_idParameter = new SqlParameter();
        collective_name_client_idParameter.ParameterName = "@collective_name_client_id";
        collective_name_client_idParameter.SqlDbType = SqlDbType.Int;
        collective_name_client_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(collective_name_client_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvCollectiveNameClient.EditIndex = -1;

        FillCollectiveNameClientGrid();

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
    protected void gvCollectiveNameClient_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
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
    protected void gvCollectiveNameClient_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCollectiveNameClient.EditIndex = e.NewEditIndex;
        FillCollectiveNameClientGrid();
    }
    protected void gvCollectiveNameClient_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvCollectiveNameClient.Rows[e.RowIndex].FindControl("lblId");
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
            " delete from pub_user where client_id = (select client_id from collective_name_client where collective_name_client_id = " + idStr + ")"; ;

        SqlCommand myCommandPubUser = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        myCommandPubUser.ExecuteNonQuery();
        myConnection.Close();

        sqlStatement =
            " delete from collective_name_client where collective_name_client_id = " + idStr;

        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        FillCollectiveNameClientGrid();

    }
    public void FillCollectiveNameClientGrid()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select pl.collective_name_client_id," +
            " pl.l_collective_name_id," +
            " lp.collective_name," +
            " pl.client_id," +
            " c.last_name + ', ' + c.first_name as client" +
            " from collective_name_client pl" +
            " inner join l_collective_name lp" +
            " on pl.l_collective_name_id = lp.l_collective_name_id" +
            " inner join client c" +
            " on pl.client_id = c.client_id";

        SqlDataSource dsCollectiveNameClient = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsCollectiveNameClient;
        gvCollectiveNameClient.DataSource = dsCollectiveNameClient;
        gvCollectiveNameClient.DataBind();

        DataView dv = (DataView)dsCollectiveNameClient.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            gvCollectiveNameClient.EditIndex = dv.Count - 1;
        }
        else
        {
        }
        EmptyGridFix(gvCollectiveNameClient);
    }
    protected void gvCollectiveNameClient_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            DropDownList ddlNewCollectiveNameTemp = null;
            ddlNewCollectiveNameTemp = (DropDownList)gvCollectiveNameClient.FooterRow.FindControl("ddlNewCollectiveName");
            if (ddlNewCollectiveNameTemp == null)
            {
                return;
            }
            DropDownList ddlNewMemberTemp = null;
            ddlNewMemberTemp = (DropDownList)gvCollectiveNameClient.FooterRow.FindControl("ddlNewMember");
            if (ddlNewMemberTemp == null)
            {
                return;
            }

            sqlStatement =
                "select count(*) from collective_name_client" +
                " where l_collective_name_id = " +
                ddlNewCollectiveNameTemp.SelectedValue.ToString() +
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
                " insert into collective_name_client" +
                " (l_collective_name_id, client_id) values (" +
                ddlNewCollectiveNameTemp.SelectedValue.ToString() +
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

            gvCollectiveNameClient.EditIndex = -1;
            FillCollectiveNameClientGrid();
        }

    }
}