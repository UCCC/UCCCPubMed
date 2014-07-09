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

public partial class l_focus_group : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sqlStatement = "";
        if (!IsPostBack)
        {
            sqlStatement =
                "select lfg.l_focus_group_id," +
                " lfg.l_program_id," +
                " right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name as program," +
                " lfg.description," +
                " lfg.group_number" +
                " from l_focus_group lfg" +
                " inner join l_program lp" +
                " on lfg.l_program_id = lp.l_program_id" +
                " order by right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name";

            Helper.BindGridview(sqlStatement, grdFocusGroup);
            //grdFocusGroup.ShowHeader = true;
            EmptyGridFix(grdFocusGroup);

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
    protected void grdFocusGroup_RowDataBound(object sender, GridViewRowEventArgs e)
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
                LoadLookup.LoadProgram(ddlProgramTemp, currProgramStr, false);
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
            TextBox txtGroupNumberTemp = null;
            txtGroupNumberTemp = (TextBox)e.Row.FindControl("txtNewGroupNumber");
            if (txtGroupNumberTemp == null)
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

        }
    }
    protected void grdFocusGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdFocusGroup.EditIndex = -1;
        FillFocusGroupGrid();
    }
    protected void grdFocusGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);

        Label lblIdTemp = null;
        lblIdTemp = (Label)grdFocusGroup.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        string oldProgramStr = GetOrigLabelValue("lblOrigProgram", idStr);
        string newProgramStr = "";

        DropDownList ddlProgramTemp = null;
        ddlProgramTemp = (DropDownList)grdFocusGroup.Rows[e.RowIndex].FindControl("ddlProgram");
        if (ddlProgramTemp == null)
        {
            return;
        }
        newProgramStr = ddlProgramTemp.SelectedItem.ToString();

        string oldGroupNumberStr = GetOrigLabelValue("lblOrigGroupNumber", idStr);
        string newGroupNumberStr = "";

        TextBox txtGroupNumberTemp = null;
        txtGroupNumberTemp = (TextBox)grdFocusGroup.Rows[e.RowIndex].FindControl("txtGroupNumber");
        if (txtGroupNumberTemp == null)
        {
            return;
        }
        newGroupNumberStr = txtGroupNumberTemp.Text;

        TextBox txtDescriptionTemp = null;
        txtDescriptionTemp = (TextBox)grdFocusGroup.Rows[e.RowIndex].FindControl("txtDescription");
        if (txtDescriptionTemp == null)
        {
            return;
        }
        string sqlStatement = "";

        sqlStatement =
            "select count(*) from l_focus_group" +
            " where l_program_id = " +
            ddlProgramTemp.SelectedValue.ToString() +
            " and group_number = " +
            txtGroupNumberTemp.Text;

        SqlCommand commandCnt2 = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        int existingCnt2 = (int)commandCnt2.ExecuteScalar();
        myConnection.Close();

        if (existingCnt2 > 0)
        {
            string msg = "There is an existing combination of program and focus group number.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            return;
        }

        sqlStatement =
            "Update l_focus_group" +
            " SET description=@description," +
            " group_number=@group_number," +
            " l_program_id=@l_program_id" +
            " WHERE (l_focus_group_id = @l_focus_group_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
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

        SqlParameter group_numberParameter = new SqlParameter();
        group_numberParameter.ParameterName = "@group_number";
        group_numberParameter.SqlDbType = SqlDbType.Int;
        if (txtGroupNumberTemp.Text != "")
        {
            group_numberParameter.Value = txtGroupNumberTemp.Text;
        }
        else
        {
            group_numberParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(group_numberParameter);

        SqlParameter l_program_idParameter = new SqlParameter();
        l_program_idParameter.ParameterName = "@l_program_id";
        l_program_idParameter.SqlDbType = SqlDbType.Int;
        l_program_idParameter.Value = ddlProgramTemp.SelectedValue;
        command.Parameters.Add(l_program_idParameter);

        SqlParameter l_focus_group_idParameter = new SqlParameter();
        l_focus_group_idParameter.ParameterName = "@l_focus_group_id";
        l_focus_group_idParameter.SqlDbType = SqlDbType.Int;
        l_focus_group_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(l_focus_group_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        grdFocusGroup.EditIndex = -1;

        FillFocusGroupGrid();

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
    protected void grdFocusGroup_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
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
    protected void grdFocusGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdFocusGroup.EditIndex = e.NewEditIndex;
        FillFocusGroupGrid();
    }
    protected void grdFocusGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    public void FillFocusGroupGrid()
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select lfg.l_focus_group_id," +
            " lfg.l_program_id," +
            " right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name as program," +
            " lfg.description," +
            " lfg.group_number" +
            " from l_focus_group lfg" +
            " inner join l_program lp" +
            " on lfg.l_program_id = lp.l_program_id" +
            " order by right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name";

        SqlDataSource dsFocusGroup = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFocusGroup;
        grdFocusGroup.DataSource = dsFocusGroup;
        grdFocusGroup.DataBind();

        DataView dv = (DataView)dsFocusGroup.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            //btnAddImage.Visible = true;
            //txtNewDescription.Visible = true;
            grdFocusGroup.EditIndex = dv.Count - 1;
        }
        else
        {
            //btnAddImage.Visible = false;
            //txtNewDescription.Visible = false;
        }
        EmptyGridFix(grdFocusGroup);
    }
    protected void grdFocusGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            TextBox txtNewDescriptionTemp = (TextBox)grdFocusGroup.FooterRow.FindControl("txtNewDescription");
            string descriptionStr = txtNewDescriptionTemp.Text;
            TextBox txtNewGroupNumberTemp = (TextBox)grdFocusGroup.FooterRow.FindControl("txtNewGroupNumber");
            string GroupNumberStr = txtNewGroupNumberTemp.Text;

            DropDownList ddlNewProgramTemp = null;
            ddlNewProgramTemp = (DropDownList)grdFocusGroup.FooterRow.FindControl("ddlNewProgram");
            if (ddlNewProgramTemp == null)
            {
                return;
            }
            if (descriptionStr == "" || GroupNumberStr == "" || ddlNewProgramTemp.SelectedIndex == -1 || ddlNewProgramTemp.SelectedIndex == 0)
            {
                string msg = "Please give program name/focus group name/group number.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            sqlStatement =
                "select count(*) from l_focus_group" +
                " where description = '" +
                descriptionStr +
                "'";

            SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            myConnection.Close();

            if (existingCnt > 0)
            {
                string msg = "There is an existing focus group in focus group list. Please give another focus group name.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                txtNewDescriptionTemp.Text = "";
                return;
            }

            sqlStatement =
                "select count(*) from l_focus_group" +
                " where l_program_id = " +
                ddlNewProgramTemp.SelectedValue.ToString() +
                " and group_number = " +
                GroupNumberStr;

            SqlCommand commandCnt2 = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            int existingCnt2 = (int)commandCnt2.ExecuteScalar();
            myConnection.Close();

            if (existingCnt2 > 0)
            {
                string msg = "There is an existing combination of program and focus group number.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                txtNewDescriptionTemp.Text = "";
                return;
            }

            sqlStatement =
                " insert into l_focus_group" +
                " (description, group_number,l_program_id) values ('" +
                descriptionStr +
                "','" +
                GroupNumberStr +
                "'," +
                ddlNewProgramTemp.SelectedValue.ToString() +
                ")";

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            grdFocusGroup.EditIndex = -1;
            FillFocusGroupGrid();
        }

    }
    protected string GetOrigLabelValue(string labelId, string idStr)
    {
        string itemTemp = "";
        string junk = "";
        for (int i = 0; i < grdFocusGroup.Rows.Count; i++)
        {
            Label lblProgramTemp = null;
            lblProgramTemp = (Label)grdFocusGroup.Rows[i].Cells[1].FindControl(labelId);
            if (lblProgramTemp != null)
            {
                itemTemp = lblProgramTemp.Text;
            }

            Label lblIdTemp = null;
            string currIdStr = "";
            lblIdTemp = (Label)grdFocusGroup.Rows[i].FindControl("lblId");
            if (lblIdTemp != null)
            {
                currIdStr = lblIdTemp.Text;
            }

            if (currIdStr == idStr)
            {
                junk =  itemTemp;
            }
        }
        return "";
    }
}