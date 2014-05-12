using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class MemberStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLookup.LoadMember(ddlMember, "xxx");

            //FillUpGrid();
        }

    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
    }
    #endregion

    protected bool ValidDdlValue(string labelId, string compareStr)
    {
        string itemTemp;
        for (int i = 0; i < myDatagrid.Items.Count; i++)
        {
            Label lb = (Label)myDatagrid.Items[i].FindControl(labelId);
            if (lb != null)
            {
                itemTemp = lb.Text;
                if (itemTemp == compareStr)
                {
                    return false;

                }
            }

        }
        return true;
    }
    protected bool ValidPrimary(string labelId, string labelEndDate)
    {
        string itemTemp;
        string tempEndDate;
        for (int i = 0; i < myDatagrid.Items.Count; i++)
        {
            Label lb = (Label)myDatagrid.Items[i].FindControl(labelId);
            Label endDate = (Label)myDatagrid.Items[i].FindControl(labelEndDate);
            if (lb != null)
            {
                itemTemp = lb.Text;
                tempEndDate = endDate.Text;
                if (itemTemp == "Yes" && tempEndDate == "")
                {
                    return false;

                }
            }

        }
        return true;
    }
    protected void LoadStatus(DropDownList ddl, string selected)
    {
        string passedClientIdStr = ddlMember.SelectedValue.ToString();
        int clientIdInt = System.Convert.ToInt32(passedClientIdStr);

        string sqlStatement =
            " SELECT" +
            " l_client_status_id," +
            " rtrim(description) as status" +
            " FROM l_client_status";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

        myConnection.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            ddl.Items.Clear();
            while (myReader.Read())
            {
                string nameStr = myReader["status"].ToString();
                string idStr = myReader["l_client_status_id"].ToString();
                ListItem li = new ListItem(nameStr, idStr);
                string nameFromDB = myReader["status"].ToString();
                if (selected == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--Select--");
        }
        finally
        {
            myReader.Close();
            myConnection.Close();
        }
    } 

    protected void LoadYesNo(DropDownList ddl, string selected)
    {
        string sqlStatement =
            " SELECT" +
            " l_yes_no_id," +
            " rtrim(description) as yes_no" +
            " FROM l_yes_no";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            ddl.Items.Clear();
            while (myReader.Read())
            {
                string nameStr = myReader["yes_no"].ToString();
                string idStr = myReader["l_yes_no_id"].ToString();
                ListItem li = new ListItem(nameStr, idStr);
                string nameFromDB = myReader["yes_no"].ToString();
                if (selected == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--Select--");
        }
        finally
        {
            myReader.Close();
            myConnection.Close();
        }
    }

    protected void myDatagrid_OnEditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        myDatagrid.EditItemIndex = e.Item.ItemIndex;
        FillUpGrid();
        myDatagrid.ShowFooter = false;
    }
    protected void myDatagrid_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        myDatagrid.EditItemIndex = -1;
        FillUpGrid();
        myDatagrid.ShowFooter = true;
    }
    protected void myDatagrid_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        //int clientProgramId = (int)myDatagrid.DataKeys[(int)e.Item.ItemIndex];
        string controlIdStr = e.Item.Cells[0].Text;
        string sqlStatement =
            " delete from client_status" +
            " where client_status_id = " +
            controlIdStr;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        try
        {
            myCommand.ExecuteNonQuery();
        }
        finally
        {
            myConnection.Close();
        }

        myDatagrid.EditItemIndex = -1;
        FillUpGrid();
        myDatagrid.ShowFooter = true;
    }
    protected void myDatagrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.EditItem)
        {
            DropDownList ddlNameTemp = null;
            ddlNameTemp = (DropDownList)e.Item.FindControl("ddlStatus");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            string currNameStr = drv["status"].ToString();

            LoadStatus(ddlNameTemp, currNameStr);
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DropDownList ddlNameTemp = null;
            ddlNameTemp = (DropDownList)e.Item.FindControl("ddlAddNewStatus");
            LoadStatus(ddlNameTemp, "dummy");
        }
        if (e.Item.FindControl("btnDel") != null)
        {
            ((LinkButton)e.Item.FindControl("btnDel")).Attributes.Add("onClick", "return confirm('Are you sure you wish to delete this item?');");

        }
    }
    protected void myDatagrid_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        string controlIdStr = e.Item.Cells[0].Text;
        DropDownList ddl = (DropDownList)(e.Item.FindControl("ddlStatus"));
        string dllIdStr = ddl.SelectedValue;
        //string votedDateStr = ((TextBox)(e.Item.FindControl("txtVotedDate"))).Text;
        TextBox txtTempStartDate = (TextBox)(e.Item.FindControl("txtStartDate"));
        string startDateStr = txtTempStartDate.Text;
        /*
        if (startDateStr == "")
        {
            startDateStr = "null";
        }
        else
        {
            startDateStr = "'" + startDateStr + "'";
        }
         * */
        TextBox txtTempEndDate = (TextBox)(e.Item.FindControl("txtEndDate"));
        string endDateStr = txtTempEndDate.Text;
        /*
        if (endDateStr == "")
        {
            endDateStr = "null";
        }
        else
        {
            endDateStr = "'" + endDateStr + "'";
        }
         * */
        TextBox txtTempNote = (TextBox)(e.Item.FindControl("txtNote"));
        string noteStr = txtTempNote.Text;

        UpdateClientTypeVote(controlIdStr, dllIdStr, startDateStr, endDateStr, noteStr);
        myDatagrid.EditItemIndex = -1;
        FillUpGrid();
        myDatagrid.ShowFooter = true;
    }

    protected void FillUpGrid()
    {
        string passedClientIdStr = ddlMember.SelectedValue.ToString();
        int clientIdInt = System.Convert.ToInt32(passedClientIdStr);

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;


        sqlStatement =
            " select cd.client_status_id," +
            " cd.client_id," +
            " rtrim(ld.description) as status," +
            " convert(varchar,cd.start_date,101) as start_date," +
            " convert(varchar,cd.end_date,101) as end_date," +
            " cd.note," +
            " c2.last_name + ', ' + c2.first_name as full_name" +
            " from client_status cd" +
            " inner join l_client_status ld" +
            " on cd.l_client_status_id = ld.l_client_status_id" +
            " inner join client c2" +
            " on cd.client_id = c2.client_id" +
            " where cd.client_id = @CLIENTID" +
            " order by cd.client_status_id";

        SqlCommand command = new SqlCommand(sqlStatement, conn);
        SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(command);
        DataSet dataSet1 = new DataSet(); ;
        SqlParameter clientIdParameter = new SqlParameter();

        clientIdParameter.ParameterName = "@CLIENTID";
        clientIdParameter.SqlDbType = SqlDbType.Int;
        clientIdParameter.Value = clientIdInt;
        command.Parameters.Add(clientIdParameter);

        sqlDataAdapter1.Fill(dataSet1);
        if (dataSet1.Tables["Table"].Rows.Count == 0)
        {
            lblName.Text = "No status information available.";
        }
        else
        {
            lblNameLbl.Text = "Member: ";
            lblName.Text = dataSet1.Tables["Table"].Rows[0]["full_name"].ToString();
        }
        myDatagrid.DataSource = dataSet1.Tables["Table"].DefaultView;
        myDatagrid.DataBind();
    } 

    protected void UpdateClientTypeVote(string controlIdStr, string dllIdStr, string dllIdStr2, string startDateStr, string endDateStr, string noteStr)
    {
        string sqlStatement =
            " update client_program" +
            " set l_program_id = " +
            dllIdStr +
            ", primary_program = " +
            dllIdStr2 +
            ", start_date = " +
            startDateStr +
            ", end_date = " +
            endDateStr +
            ", note = '" +
            noteStr +
            "'  where client_program_id = " +
            controlIdStr;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        try
        {
            myCommand.ExecuteNonQuery();
        }
        finally
        {
            myConnection.Close();
        }
        FillUpGrid();

    }

    protected void DoInsert(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        string passedClientId = ddlMember.SelectedValue.ToString();
        //int clientIdInt = System.Convert.ToInt32(passedClientIdStr);
        if (e.CommandName == "Insert")
        {
            string startDateStr;
            TextBox startDateTextBox;
            startDateTextBox = (TextBox)e.Item.FindControl("txtAddNewStartDate");
            startDateStr = startDateTextBox.Text;
            if (startDateStr == "")
            {
                startDateStr = "null";
            }
            else
            {
                startDateStr = "'" + startDateStr + "'";
            }

            string endDateStr;
            TextBox endDateTextBox;
            endDateTextBox = (TextBox)e.Item.FindControl("txtAddNewEndDate");
            endDateStr = endDateTextBox.Text;
            if (endDateStr == "")
            {
                endDateStr = "null";
            }
            else
            {
                endDateStr = "'" + endDateStr + "'";
            }

            string noteStr;
            TextBox noteTextBox;
            noteTextBox = (TextBox)e.Item.FindControl("txtAddNewNote");
            noteStr = noteTextBox.Text;

            DropDownList ddl = (DropDownList)(e.Item.FindControl("ddlAddNewStatus"));
            if (ddl.SelectedIndex == 0)
            {
                string msg = "Please select a status.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }
            string ddlIdStr = ddl.SelectedValue.ToString();

            string connectionStr2 = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection2 = new SqlConnection(connectionStr2);

            string sqlStatement =
                "update client_status" +
                " set end_date = " +
                startDateStr +
                " where client_id = " +
                passedClientId;

            SqlCommand myCommandUpdate = new SqlCommand(sqlStatement, myConnection2);
            myConnection2.Open();
            try
            {
                myCommandUpdate.ExecuteNonQuery();
            }
            finally
            {
                myConnection2.Close();
            }

            sqlStatement =
                " update client_status" +
                " set end_date = " +
                startDateStr +
                " where client_status_id = " +
                " (select max(client_status_id)" +
                " from client_status where client_id = " +
                passedClientId +
                ")";

            SqlCommand cmdUpdate = new SqlCommand(sqlStatement, myConnection2);

            myConnection2.Open();
            cmdUpdate.ExecuteNonQuery();
            myConnection2.Close();

            sqlStatement =
                " insert into client_status" +
                " (client_id, l_client_status_id, start_date, end_date, note)" +
                " values(" +
                passedClientId +
                ", " +
                ddlIdStr +
                ", " +
                startDateStr +
                ", " +
                endDateStr +
                ", '" +
                noteStr +
                "')";

            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);
            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }
            myDatagrid.EditItemIndex = -1;
            FillUpGrid();
            myDatagrid.ShowFooter = true;

        }
    }
    protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUpGrid();
    }
    protected void UpdateClientTypeVote(string controlIdStr, string dllIdStr, string startDateStr, string endDateStr, string noteStr)
    {
        string sqlStatement =
            " update client_status" +
            " set l_client_status_id = @l_client_status_id" +
            //dllIdStr +
            ", start_date = @start_date" +
            //startDateStr +
            ", end_date = @end_date" +
            //endDateStr +
            ", note = @note" +
            //noteStr +
            "  where client_status_id = @client_status_id";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

        SqlParameter l_client_status_idParameter = new SqlParameter();
        l_client_status_idParameter.ParameterName = "@l_client_status_id";
        l_client_status_idParameter.SqlDbType = SqlDbType.Int;
        l_client_status_idParameter.Value = Convert.ToInt32(dllIdStr);
        myCommand.Parameters.Add(l_client_status_idParameter);

        SqlParameter start_dateParameter = new SqlParameter();
        start_dateParameter.ParameterName = "@start_date";
        start_dateParameter.SqlDbType = SqlDbType.SmallDateTime;
        if (startDateStr == "")
        {
            start_dateParameter.Value = DBNull.Value;
        }
        else
        {
            start_dateParameter.Value = Convert.ToDateTime(startDateStr);
        }
        myCommand.Parameters.Add(start_dateParameter);

        SqlParameter end_dateParameter = new SqlParameter();
        end_dateParameter.ParameterName = "@end_date";
        end_dateParameter.SqlDbType = SqlDbType.SmallDateTime;
        if (endDateStr == "")
        {
            end_dateParameter.Value = DBNull.Value;
        }
        else
        {
            end_dateParameter.Value = Convert.ToDateTime(endDateStr);
        }
        myCommand.Parameters.Add(end_dateParameter);

        SqlParameter noteParameter = new SqlParameter();
        noteParameter.ParameterName = "@note";
        noteParameter.SqlDbType = SqlDbType.VarChar;
        noteParameter.Value = noteStr;
        myCommand.Parameters.Add(noteParameter);

        SqlParameter client_status_idParameter = new SqlParameter();
        client_status_idParameter.ParameterName = "@client_status_id";
        client_status_idParameter.SqlDbType = SqlDbType.Int;
        client_status_idParameter.Value = Convert.ToInt32(controlIdStr);
        myCommand.Parameters.Add(client_status_idParameter);

        myConnection.Open();
        try
        {
            myCommand.ExecuteNonQuery();
        }
        finally
        {
            myConnection.Close();
        }
        FillUpGrid();

    }
}