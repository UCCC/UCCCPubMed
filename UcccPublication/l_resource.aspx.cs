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

public partial class l_resource : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sqlStatement = "";
        if (!IsPostBack)
        {
            sqlStatement =
                "select l_resource_id," +
                " description" +
                " from l_resource" +
                " order by description";
           
            Helper.BindGridview(sqlStatement, grdResource);
            EmptyGridFix(grdResource);

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
    protected void grdResource_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void grdResource_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdResource.EditIndex = -1;
        FillResourceGrid();
    }
    protected void grdResource_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)grdResource.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        TextBox txtDescriptionTemp = null;
        txtDescriptionTemp = (TextBox)grdResource.Rows[e.RowIndex].FindControl("txtDescription");
        if (txtDescriptionTemp == null)
        {
            return;
        }
        string sqlStatement =
            "Update l_resource" +
            " SET description=@description" +
            " WHERE (l_resource_id = @l_resource_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter descriptionParameter = new SqlParameter();
        descriptionParameter.ParameterName = "@description";
        descriptionParameter.SqlDbType = SqlDbType.VarChar;
        if (txtDescriptionTemp.Text != "")
        {
            descriptionParameter.Value = txtDescriptionTemp.Text;
        }
        else
        {
            descriptionParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(descriptionParameter);

        SqlParameter l_resource_idParameter = new SqlParameter();
        l_resource_idParameter.ParameterName = "@l_resource_id";
        l_resource_idParameter.SqlDbType = SqlDbType.Int;
        l_resource_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(l_resource_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        grdResource.EditIndex = -1;

        FillResourceGrid();

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
    protected void grdResource_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
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
    protected void grdResource_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdResource.EditIndex = e.NewEditIndex;
        FillResourceGrid();
    }
    public void FillResourceGrid()
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select l_resource_id," +
            " description" +
            " from l_resource" +
            " order by description";

        SqlDataSource dsResource = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsResource;
        grdResource.DataSource = dsResource;
        grdResource.DataBind();

        DataView dv = (DataView)dsResource.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            //btnAddImage.Visible = true;
            //txtNewDescription.Visible = true;
            grdResource.EditIndex = dv.Count - 1;
        }
        else
        {
            //btnAddImage.Visible = false;
            //txtNewDescription.Visible = false;
        }
        EmptyGridFix(grdResource);
    }
    protected void grdResource_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            TextBox txtNewDescriptionTemp = (TextBox)grdResource.FooterRow.FindControl("txtNewDescription");
            string descriptionStr = txtNewDescriptionTemp.Text;

            sqlStatement =
                "select count(*) from l_resource" +
                " where description = '" +
                descriptionStr +
                "'";

            SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            myConnection.Close();

            if (existingCnt > 0)
            {
                string msg = "There is an existing resource in resource list. Please give another resource name.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                txtNewDescriptionTemp.Text = "";
                return;
            }

            sqlStatement =
                " insert into l_resource" +
                " (description) values ('" +
                descriptionStr +
                "')";

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            grdResource.EditIndex = -1;
            FillResourceGrid();
        }

    }
}