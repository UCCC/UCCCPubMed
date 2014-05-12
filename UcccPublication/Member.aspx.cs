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

public partial class Member : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillMemberGrid();
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
    protected void gvMember_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvMember_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvMember.EditIndex = -1;
        FillMemberGrid();
    }
    protected void gvMember_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvMember.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        TextBox txtLastNameTemp = null;
        txtLastNameTemp = (TextBox)gvMember.Rows[e.RowIndex].FindControl("txtLastName");
        if (txtLastNameTemp == null)
        {
            return;
        }
        TextBox txtFirstNameTemp = null;
        txtFirstNameTemp = (TextBox)gvMember.Rows[e.RowIndex].FindControl("txtFirstName");
        if (txtFirstNameTemp == null)
        {
            return;
        }
        TextBox txtMiTemp = null;
        txtMiTemp = (TextBox)gvMember.Rows[e.RowIndex].FindControl("txtMi");
        if (txtMiTemp == null)
        {
            return;
        }
        string sqlStatement =
            "Update client" +
            " SET last_name=@last_name," +
            " first_name=@first_name," +
            " mi=@mi" +
            " WHERE (client_id = @client_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter last_nameParameter = new SqlParameter();
        last_nameParameter.ParameterName = "@last_name";
        last_nameParameter.SqlDbType = SqlDbType.VarChar;
        if (txtLastNameTemp.Text != "")
        {
            last_nameParameter.Value = txtLastNameTemp.Text;
        }
        else
        {
            last_nameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(last_nameParameter);

        SqlParameter first_nameParameter = new SqlParameter();
        first_nameParameter.ParameterName = "@first_name";
        first_nameParameter.SqlDbType = SqlDbType.VarChar;
        if (txtFirstNameTemp.Text != "")
        {
            first_nameParameter.Value = txtFirstNameTemp.Text;
        }
        else
        {
            first_nameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(first_nameParameter);

        SqlParameter miParameter = new SqlParameter();
        miParameter.ParameterName = "@mi";
        miParameter.SqlDbType = SqlDbType.VarChar;
        if (txtMiTemp.Text != "")
        {
            miParameter.Value = txtMiTemp.Text;
        }
        else
        {
            miParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(miParameter);

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        client_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(client_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvMember.EditIndex = -1;

        FillMemberGrid();

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
    protected void gvMember_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
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
    protected void gvMember_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvMember.EditIndex = e.NewEditIndex;
        FillMemberGrid();
    }
    public void FillMemberGrid()
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select client_id," +
            " last_name," +
            " first_name," +
            " mi" +
            " from client" +
            " where last_name is not null and last_name <> ''" +
            " order by last_name";

        SqlDataSource dsMember = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsMember;
        gvMember.DataSource = dsMember;
        gvMember.DataBind();
        /*
        DataView dv = (DataView)dsMember.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            //btnAddImage.Visible = true;
            //txtNewDescription.Visible = true;
            gvMember.EditIndex = dv.Count - 1;
        }
        else
        {
            //btnAddImage.Visible = false;
            //txtNewDescription.Visible = false;
        }
         * */
        //EmptyGridFix(gvMember);
    }
    protected void gvMember_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            TextBox txtNewLastNameTemp = (TextBox)gvMember.FooterRow.FindControl("txtNewLastName");
            string last_nameStr = txtNewLastNameTemp.Text;
            TextBox txtNewFirstNameTemp = (TextBox)gvMember.FooterRow.FindControl("txtNewFirstName");
            string first_nameStr = txtNewFirstNameTemp.Text;
            TextBox txtNewMiTemp = (TextBox)gvMember.FooterRow.FindControl("txtNewMi");
            string miStr = txtNewMiTemp.Text;

            sqlStatement =
                "select count(*) from client" +
                " where last_name = '" +
                last_nameStr +
                "' and first_name = '" +
                first_nameStr +
                "' and mi = '" +
                miStr +
                "'";
                

            SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            myConnection.Close();

            if (existingCnt > 0)
            {
                string msg = "There is an existing member in member list. Please give another member name.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                txtNewLastNameTemp.Text = "";
                return;
            }

            sqlStatement =
                " insert into client" +
                " (last_name, first_name, mi) values " +
                "(@last_name, @first_name, @mi)";


            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter last_nameParameter = new SqlParameter();
            last_nameParameter.ParameterName = "@last_name";
            last_nameParameter.SqlDbType = SqlDbType.VarChar;
            last_nameParameter.Value = last_nameStr;
            command.Parameters.Add(last_nameParameter);

            SqlParameter first_nameParameter = new SqlParameter();
            first_nameParameter.ParameterName = "@first_name";
            first_nameParameter.SqlDbType = SqlDbType.VarChar;
            first_nameParameter.Value = first_nameStr;
            command.Parameters.Add(first_nameParameter);

            SqlParameter MIParameter = new SqlParameter();
            MIParameter.ParameterName = "@mi";
            MIParameter.SqlDbType = SqlDbType.VarChar;
            if (miStr != "")
            {
                MIParameter.Value = miStr;
            }
            else
            {
                MIParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(MIParameter);

            SqlParameter client_idParameter = new SqlParameter();
            client_idParameter.ParameterName = "@client_id";
            client_idParameter.SqlDbType = SqlDbType.Int;
            client_idParameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(client_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();

            gvMember.EditIndex = -1;
            FillMemberGrid();
        }

    }
}