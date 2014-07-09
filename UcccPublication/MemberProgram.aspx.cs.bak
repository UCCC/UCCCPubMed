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

public partial class MemberProgram : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLookup.LoadMemberOnProgram(0, ddlMember);

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
    protected void LoadProgram(DropDownList ddl, string selected)
    {
        string sqlStatement =
            " SELECT" +
            " l_program_id," +
            " rtrim(program_name) as program" +
            " FROM l_program" +
            " where deactivated_date is null";

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
                string nameStr = myReader["program"].ToString();
                string idStr = myReader["l_program_id"].ToString();
                ListItem li = new ListItem(nameStr, idStr);
                string nameFromDB = myReader["program"].ToString();
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
            " delete from client_program" +
            " where client_program_id = " +
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
            ddlNameTemp = (DropDownList)e.Item.FindControl("ddlProgram");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            string currNameStr = drv["program"].ToString();

            LoadProgram(ddlNameTemp, currNameStr);

            DropDownList ddlYesNoTemp = null;
            ddlYesNoTemp = (DropDownList)e.Item.FindControl("ddlYesNo");

            string currYesNoStr = drv["yes_no"].ToString();

            LoadYesNo(ddlYesNoTemp, currYesNoStr);
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DropDownList ddlNameTemp = null;
            ddlNameTemp = (DropDownList)e.Item.FindControl("ddlAddNewProgram");
            LoadProgram(ddlNameTemp, "dummy");

            DropDownList ddlYesNoTemp = null;
            ddlYesNoTemp = (DropDownList)e.Item.FindControl("ddlAddNewYesNo");
            LoadYesNo(ddlYesNoTemp, "dummy");
        }
        if (e.Item.FindControl("btnDel") != null)
        {
            ((LinkButton)e.Item.FindControl("btnDel")).Attributes.Add("onClick", "return confirm('Are you sure you wish to delete this item?');");

        }
    }
    protected void myDatagrid_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        string controlIdStr = e.Item.Cells[0].Text;

        DropDownList ddl = (DropDownList)(e.Item.FindControl("ddlProgram"));
        string dllIdStr = ddl.SelectedValue;
        string ddlNameStr = ddl.SelectedItem.ToString();
        bool isValidDdlValue = ValidDdlValue("lblOrigProgram", ddlNameStr);
        if (!isValidDdlValue)
        {
            string msg = "Cannot duplicate program.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            return;
        }

        DropDownList ddl2 = (DropDownList)(e.Item.FindControl("ddlYesNo"));
        string dllIdStr2 = ddl2.SelectedValue;
        string ddlNameStr2 = ddl2.SelectedItem.ToString();
        bool isValidDdlValue2 = ValidDdlValue("lblOrigYesNo", ddlNameStr2);
        if (!isValidDdlValue2 && ddlNameStr2 == "Yes")
        {
            string msg = "Cannot duplicate primary program.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            return;
        }

        TextBox txtTempStartDate = (TextBox)(e.Item.FindControl("txtStartDate"));
        string startDateStr = txtTempStartDate.Text;
        if (startDateStr == "")
        {
            startDateStr = "null";
        }
        else
        {
            startDateStr = "'" + startDateStr + "'";
        }
        TextBox txtTempEndDate = (TextBox)(e.Item.FindControl("txtEndDate"));
        string endDateStr = txtTempEndDate.Text;
        if (endDateStr == "")
        {
            endDateStr = "null";
        }
        else
        {
            endDateStr = "'" + endDateStr + "'";
        }
        TextBox txtTempNote = (TextBox)(e.Item.FindControl("txtNote"));
        string noteStr = txtTempNote.Text;

        UpdateClientTypeVote(controlIdStr, dllIdStr, dllIdStr2, startDateStr, endDateStr, noteStr);
        myDatagrid.EditItemIndex = -1;
        FillUpGrid();
        myDatagrid.ShowFooter = true;
    }

    protected void FillUpGrid()
    {
        string passedClientIdStr = ddlMember.SelectedValue.ToString();
        int clientIdInt = Convert.ToInt32(ddlMember.SelectedValue);

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;


        sqlStatement =
            " select cp.client_program_id," +
            " cp.client_id," +
            " rtrim(ld.program_name) as program," +
            " lynu.description as yes_no," +
            " convert(varchar,cp.start_date,101) as start_date," +
            " convert(varchar,cp.end_date,101) as end_date," +
            " cp.note," +
            " c2.last_name + ', ' + c2.first_name as full_name" +
            " from client_program cp" +
            " inner join l_program ld" +
            " on cp.l_program_id = ld.l_program_id" +
            " inner join client c2" +
            " on cp.client_id = c2.client_id" +
            " inner join l_yes_no lynu" +
            " on cp.primary_program = lynu.l_yes_no_id" +
            " where cp.client_id = @CLIENTID" +
            " order by cp.client_program_id";

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
            lblName.Text = "No program information available.";
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

            DropDownList ddl = (DropDownList)(e.Item.FindControl("ddlAddNewProgram"));
            DropDownList ddl2 = (DropDownList)(e.Item.FindControl("ddlAddNewYesNo"));
            if (ddl.SelectedIndex == 0 || ddl2.SelectedIndex == 0)
            {
                string msg = "Please select a program and specify primary.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            string passedClientIdStr = ddlMember.SelectedValue.ToString();

            string ddlIdStr = ddl.SelectedValue.ToString();
            string ddlIdStr2 = ddl2.SelectedValue.ToString();
            string ddlNameStr = ddl.SelectedItem.ToString();
            bool isValidDdlValue = ValidDdlValue("lblOrigProgram", ddlNameStr);
            if (!isValidDdlValue)
            {
                string msg = "Cannot duplicate program.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }
            string ddlNameStr2 = ddl2.SelectedItem.ToString();
            if (ddlIdStr2 == "1")
            {
                bool isValidPrimary = ValidPrimary("lblOrigYesNo", "lblOrigEndDate");
                if (!isValidPrimary)
                {
                    string msg = "Cannot duplicate current primary program.";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                    return;
                }
            }
            /*
            bool isValidDdlValue2 = ValidDdlValue("lblOrigYesNo", ddlNameStr2);
            if (!isValidDdlValue2 && ddlNameStr2 == "Yes")
            {
                ClientManagement.CMControl.WebMsgBox.Show("Cannot duplicate primary program.");
                return;
            }
            */

            string sqlStatement =
                " insert into client_program" +
                " (client_id, l_program_id, primary_program, start_date, end_date, note)" +
                " values(" +
                ddlMember.SelectedValue.ToString() +
                ", " +
                ddlIdStr +
                ", " +
                ddlIdStr2 +
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
}