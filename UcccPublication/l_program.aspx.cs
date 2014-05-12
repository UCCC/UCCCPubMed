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

public partial class l_program : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillProgramGrid();
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
    protected void gvProgram_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvProgram_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvProgram.EditIndex = -1;
        FillProgramGrid();
    }
    protected void gvProgram_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvProgram.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        TextBox txtProgramNameTemp = null;
        txtProgramNameTemp = (TextBox)gvProgram.Rows[e.RowIndex].FindControl("txtProgramName");
        if (txtProgramNameTemp == null)
        {
            return;
        }
        TextBox txtAbbreviationTemp = null;
        txtAbbreviationTemp = (TextBox)gvProgram.Rows[e.RowIndex].FindControl("txtAbbreviation");
        if (txtAbbreviationTemp == null)
        {
            return;
        }
        string sqlStatement =
            "Update l_program" +
            " SET program_name=@program_name," +
            " abbreviation=@abbreviation" +
            " WHERE (l_program_id = @l_program_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter program_nameParameter = new SqlParameter();
        program_nameParameter.ParameterName = "@program_name";
        program_nameParameter.SqlDbType = SqlDbType.VarChar;
        if (txtProgramNameTemp.Text != "")
        {
            program_nameParameter.Value = txtProgramNameTemp.Text;
        }
        else
        {
            program_nameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(program_nameParameter);

        SqlParameter abbreviationParameter = new SqlParameter();
        abbreviationParameter.ParameterName = "@abbreviation";
        abbreviationParameter.SqlDbType = SqlDbType.VarChar;
        if (txtAbbreviationTemp.Text != "")
        {
            abbreviationParameter.Value = txtAbbreviationTemp.Text;
        }
        else
        {
            abbreviationParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(abbreviationParameter);

        SqlParameter l_program_idParameter = new SqlParameter();
        l_program_idParameter.ParameterName = "@l_program_id";
        l_program_idParameter.SqlDbType = SqlDbType.Int;
        l_program_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(l_program_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvProgram.EditIndex = -1;

        FillProgramGrid();

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
    protected void gvProgram_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
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
    protected void gvProgram_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvProgram.EditIndex = e.NewEditIndex;
        FillProgramGrid();
    }
    public void FillProgramGrid()
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select l_program_id," +
            " program_name," +
            " abbreviation" +
            " from l_program" +
            " where program_name is not null and program_name <> ''" +
            " order by program_name";

        SqlDataSource dsProgram = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsProgram;
        gvProgram.DataSource = dsProgram;
        gvProgram.DataBind();
    }
    protected void gvProgram_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            TextBox txtNewProgramNameTemp = (TextBox)gvProgram.FooterRow.FindControl("txtNewProgramName");
            string program_nameStr = txtNewProgramNameTemp.Text;
            TextBox txtNewAbbreviationTemp = (TextBox)gvProgram.FooterRow.FindControl("txtNewAbbreviation");
            string abbreviationStr = txtNewAbbreviationTemp.Text;

            sqlStatement =
                "select count(*) from l_program" +
                " where program_name = '" +
                program_nameStr +
                "'";


            SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            myConnection.Close();

            if (existingCnt > 0)
            {
                string msg = "There is an existing program in program list. Please give another program name.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                txtNewProgramNameTemp.Text = "";
                return;
            }

            sqlStatement =
                " insert into l_program" +
                " (program_name, abbreviation) values " +
                "(@program_name, @abbreviation)";


            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter program_nameParameter = new SqlParameter();
            program_nameParameter.ParameterName = "@program_name";
            program_nameParameter.SqlDbType = SqlDbType.VarChar;
            program_nameParameter.Value = program_nameStr;
            command.Parameters.Add(program_nameParameter);

            SqlParameter abbreviationParameter = new SqlParameter();
            abbreviationParameter.ParameterName = "@abbreviation";
            abbreviationParameter.SqlDbType = SqlDbType.VarChar;
            abbreviationParameter.Value = abbreviationStr;
            command.Parameters.Add(abbreviationParameter);

            SqlParameter l_program_idParameter = new SqlParameter();
            l_program_idParameter.ParameterName = "@l_program_id";
            l_program_idParameter.SqlDbType = SqlDbType.Int;
            l_program_idParameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(l_program_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();

            gvProgram.EditIndex = -1;
            FillProgramGrid();
        }

    }
}