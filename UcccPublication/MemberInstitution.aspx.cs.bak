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

public partial class MemberInstitution : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLookup.LoadMemberOnProgram(0, ddlMember);
        }

    }
    protected bool ValidDdlValue(string labelId, string compareStr)
    {
        string itemTemp;
        for (int i = 0; i < dgPrimaryInstitution.Items.Count; i++)
        {
            Label lb = (Label)dgPrimaryInstitution.Items[i].FindControl(labelId);
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
    protected void LoadInstitution(DropDownList ddl, string selected)
    {
        string sqlStatement =
            " SELECT" +
            " l_institution_id," +
            " rtrim(description) as institution" +
            " FROM l_institution" +
            " order by description";

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
                string nameStr = myReader["institution"].ToString();
                string idStr = myReader["l_institution_id"].ToString();
                ListItem li = new ListItem(nameStr, idStr);
                string nameFromDB = myReader["institution"].ToString();
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


    protected void dgPrimaryInstitution_OnEditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        dgPrimaryInstitution.EditItemIndex = e.Item.ItemIndex;
        FillUpDgPrimaryInstitution();
        dgPrimaryInstitution.ShowFooter = false;
    }
    protected void dgPrimaryInstitution_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        dgPrimaryInstitution.EditItemIndex = -1;
        FillUpDgPrimaryInstitution();
        dgPrimaryInstitution.ShowFooter = true;
    }
    protected void dgPrimaryInstitution_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        //int clientProgramId = (int)dgPrimaryInstitution.DataKeys[(int)e.Item.ItemIndex];
        string controlIdStr = e.Item.Cells[0].Text;
        string sqlStatement =
            " delete from client_institution" +
            " where client_institution_id = " +
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

        dgPrimaryInstitution.EditItemIndex = -1;
        FillUpDgPrimaryInstitution();
        dgPrimaryInstitution.ShowFooter = true;
    }
    protected void dgPrimaryInstitution_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.EditItem)
        {
            DropDownList ddlNameTemp = null;
            ddlNameTemp = (DropDownList)e.Item.FindControl("ddlInstitution");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            string currNameStr = drv["institution"].ToString();

            LoadInstitution(ddlNameTemp, currNameStr);

            DropDownList ddlYesNoTemp = null;
            ddlYesNoTemp = (DropDownList)e.Item.FindControl("ddlYesNo");

            string currYesNoStr = drv["yes_no"].ToString();

            LoadYesNo(ddlYesNoTemp, currYesNoStr);
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DropDownList ddlNameTemp = null;
            ddlNameTemp = (DropDownList)e.Item.FindControl("ddlAddNewInstitution");
            LoadInstitution(ddlNameTemp, "dummy");

            DropDownList ddlYesNoTemp = null;
            ddlYesNoTemp = (DropDownList)e.Item.FindControl("ddlAddNewYesNo");
            LoadYesNo(ddlYesNoTemp, "dummy");
        }
        if (e.Item.FindControl("btnDel") != null)
        {
            ((LinkButton)e.Item.FindControl("btnDel")).Attributes.Add("onClick", "return confirm('Are you sure you wish to delete this item?');");

        }
    }
    protected void dgPrimaryInstitution_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        string controlIdStr = e.Item.Cells[0].Text;

        DropDownList ddl = (DropDownList)(e.Item.FindControl("ddlInstitution"));
        string dllIdStr = ddl.SelectedValue;
        string ddlNameStr = ddl.SelectedItem.ToString();
        bool isValidDdlValue = ValidDdlValue("lblOrigInstitution", ddlNameStr);
        if (!isValidDdlValue)
        {
            string msg = "Cannot duplicate institution.";
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

        UpdateClientPrimaryInstitution(controlIdStr, dllIdStr, startDateStr, endDateStr, noteStr);
        dgPrimaryInstitution.EditItemIndex = -1;
        FillUpDgPrimaryInstitution();
        dgPrimaryInstitution.ShowFooter = true;
    }

    protected void FillUpDgPrimaryInstitution()
    {
        string passedClientIdStr = ddlMember.SelectedValue.ToString();
        int clientIdInt = System.Convert.ToInt32(passedClientIdStr);

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;


        sqlStatement =
            " select cp.client_institution_id," +
            " cp.client_id," +
            " rtrim(ld.description) as institution," +
            " lynu.description as yes_no," +
            " convert(varchar,cp.start_date,101) as start_date," +
            " convert(varchar,cp.end_date,101) as end_date," +
            " cp.note," +
            " c2.last_name + ', ' + c2.first_name as full_name" +
            " from client_institution cp" +
            " inner join l_institution ld" +
            " on cp.l_institution_id = ld.l_institution_id" +
            " inner join client c2" +
            " on cp.client_id = c2.client_id" +
            " inner join l_yes_no lynu" +
            " on cp.primary_institution = lynu.l_yes_no_id" +
            " where cp.client_id = @CLIENTID" +
            " and cp.primary_institution = 1" +
            " order by cp.client_institution_id";

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
            lblName.Text = "No institution information available.";
        }
        else
        {
            lblNameLbl.Text = "Member: ";
            lblName.Text = dataSet1.Tables["Table"].Rows[0]["full_name"].ToString();
        }
        dgPrimaryInstitution.DataSource = dataSet1.Tables["Table"].DefaultView;
        dgPrimaryInstitution.DataBind();
    }

    protected void UpdateClientPrimaryInstitution(string controlIdStr, string dllIdStr, string startDateStr, string endDateStr, string noteStr)
    {
        string sqlStatement =
            " update client_institution" +
            " set l_institution_id = " +
            dllIdStr +
            //", primary_institution = " +
            //dllIdStr2 +
            ", start_date = " +
            startDateStr +
            ", end_date = " +
            endDateStr +
            ", note = '" +
            noteStr +
            "'  where client_institution_id = " +
            controlIdStr +
            " and primary_institution = 1";

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
        FillUpDgPrimaryInstitution();

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

            DropDownList ddl = (DropDownList)(e.Item.FindControl("ddlAddNewInstitution"));
            DropDownList ddl2 = (DropDownList)(e.Item.FindControl("ddlAddNewYesNo"));
            if (ddl.SelectedIndex == 0 || ddl2.SelectedIndex == 0)
            {
                string msg = "Please select an institution and specify primary.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            string passedClientIdStr = ddlMember.SelectedValue.ToString();

            string ddlIdStr = ddl.SelectedValue.ToString();
            string ddlIdStr2 = ddl2.SelectedValue.ToString();

            string ddlNameStr = ddl.SelectedItem.ToString();
            bool isValidDdlValue = ValidDdlValue("lblOrigInstitution", ddlNameStr);
            if (!isValidDdlValue)
            {
                string msg = "Cannot duplicate institution.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }
            string ddlNameStr2 = ddl2.SelectedItem.ToString();
            bool isValidDdlValue2 = ValidDdlValue("lblOrigYesNoUnknown", ddlNameStr2);
            if (!isValidDdlValue2 && ddlNameStr2 == "Yes")
            {
                string msg = "Cannot duplicate primary institution.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            string sqlStatement =
                " insert into client_institution" +
                " (client_id, l_institution_id, primary_institution, start_date, end_date, note)" +
                " values(" +
                passedClientIdStr +
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
            dgPrimaryInstitution.EditItemIndex = -1;
            FillUpDgPrimaryInstitution();
            dgPrimaryInstitution.ShowFooter = true;

        }
    }
    protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUpDgPrimaryInstitution();
    }
}