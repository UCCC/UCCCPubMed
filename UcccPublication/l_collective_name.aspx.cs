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

public partial class l_collective_name : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sqlStatement = "";
        if (!IsPostBack)
        {
            sqlStatement =
                "select l_collective_name_id," +
                " collective_name" +
                " from l_collective_name";

            Helper.BindGridview(sqlStatement, grdCollectiveName);
            EmptyGridFix(grdCollectiveName);

        }
        string memberIdStr = (string)Session["memberId"];

        string roleIdStr = (string)Session["roleId"];

        if (roleIdStr == "1" || roleIdStr == "2" || roleIdStr == "3")
        {
            ErrorMessage.Text = "";
            //LoginBanner1.Visible = false;
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
    protected void grdCollectiveName_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)grdCollectiveName.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }
        string sqlStatement =
            " delete from COLLECTIVE_NAME_CLIENT where l_collective_name_id = " + idStr +
            "; delete from l_collective_name where l_collective_name_id = " + idStr;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        FillCollectiveNameGrid();

    }
    protected void grdCollectiveName_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbEditTemp = null;
            lbEditTemp = (LinkButton)e.Row.FindControl("lnkEdit");
            LinkButton lbDeleteTemp = null;
            lbDeleteTemp = (LinkButton)e.Row.FindControl("lnkDelete");

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            LinkButton lbAddTemp = null;
            lbAddTemp = (LinkButton)e.Row.FindControl("lnkAdd");
            if (lbAddTemp == null)
            {
                return;
            }
        }
    }
    protected void grdCollectiveName_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdCollectiveName.EditIndex = -1;
        FillCollectiveNameGrid();
    }
    protected void grdCollectiveName_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)grdCollectiveName.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        TextBox txtCollectiveNameTemp = null;
        txtCollectiveNameTemp = (TextBox)grdCollectiveName.Rows[e.RowIndex].FindControl("txtCollectiveName");
        if (txtCollectiveNameTemp == null)
        {
            return;
        }
        string sqlStatement =
            "Update l_collective_name" +
            " SET collective_name=@collective_name" +
            " WHERE (l_collective_name_id = @l_collective_name_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter collective_nameParameter = new SqlParameter();
        collective_nameParameter.ParameterName = "@collective_name";
        collective_nameParameter.SqlDbType = SqlDbType.VarChar;
        if (txtCollectiveNameTemp.Text != "")
        {
            collective_nameParameter.Value = txtCollectiveNameTemp.Text;
        }
        else
        {
            collective_nameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(collective_nameParameter);

        SqlParameter l_collective_name_idParameter = new SqlParameter();
        l_collective_name_idParameter.ParameterName = "@l_collective_name_id";
        l_collective_name_idParameter.SqlDbType = SqlDbType.Int;
        l_collective_name_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(l_collective_name_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        grdCollectiveName.EditIndex = -1;

        FillCollectiveNameGrid();

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
    protected void grdCollectiveName_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
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
    protected void grdCollectiveName_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdCollectiveName.EditIndex = e.NewEditIndex;
        FillCollectiveNameGrid();
    }
    public void FillCollectiveNameGrid()
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select l_collective_name_id," +
            " collective_name" +
            " from l_collective_name";

        SqlDataSource dsCollectiveName = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsCollectiveName;
        grdCollectiveName.DataSource = dsCollectiveName;
        grdCollectiveName.DataBind();

        DataView dv = (DataView)dsCollectiveName.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            //btnAddImage.Visible = true;
            //txtNewDescription.Visible = true;
            grdCollectiveName.EditIndex = dv.Count - 1;
        }
        else
        {
            //btnAddImage.Visible = false;
            //txtNewDescription.Visible = false;
        }
        EmptyGridFix(grdCollectiveName);
    }
    protected void grdCollectiveName_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            TextBox txtNewCollectiveNameTemp = (TextBox)grdCollectiveName.FooterRow.FindControl("txtNewCollectiveName");
            string collective_nameStr = txtNewCollectiveNameTemp.Text;

            sqlStatement =
                "select count(*) from l_collective_name" +
                " where collective_name = '" +
                collective_nameStr +
                "'";

            SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            myConnection.Close();

            if (existingCnt > 0)
            {
                string msg = "There is an existing collective name in collective name list. Please give another collective name.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                txtNewCollectiveNameTemp.Text = "";
                return;
            }

            sqlStatement =
                " insert into l_collective_name" +
                " (collective_name) values ('" +
                collective_nameStr +
                "')";

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            grdCollectiveName.EditIndex = -1;
            FillCollectiveNameGrid();
        }

    }
}