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
    protected void LoadStatus(DropDownList ddl, string selected)
    {
        string sqlStatement =
            " SELECT" +
            " l_client_status_id," +
            " description as client_status" +
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
                string nameStr = myReader["client_status"].ToString();
                string idStr = myReader["l_client_status_id"].ToString();
                ListItem li = new ListItem(nameStr, idStr);
                string nameFromDB = myReader["client_status"].ToString();
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
            string currNameStr = drv["client_status"].ToString();

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

        UpdateClientTypeVote(controlIdStr, dllIdStr, startDateStr, endDateStr, noteStr);
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
            " select cp.client_status_id," +
            " cp.client_id," +
            " ld.description as client_status," +
            " convert(varchar,cp.start_date,101) as start_date," +
            " convert(varchar,cp.end_date,101) as end_date," +
            " cp.note," +
            " c2.last_name + ', ' + c2.first_name as full_name" +
            " from client_status cp" +
            " inner join l_client_status ld" +
            " on cp.l_client_status_id = ld.l_client_status_id" +
            " inner join client c2" +
            " on cp.client_id = c2.client_id" +
            " where cp.client_id = @CLIENTID" +
            " order by cp.client_status_id";

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
    protected void UpdateClientTypeVote(string controlIdStr, string dllIdStr, string startDateStr, string endDateStr, string noteStr)
    {
        string sqlStatement =
            " update client_status" +
            " set l_client_status_id = " +
            dllIdStr +
            ", start_date = " +
            startDateStr +
            ", end_date = " +
            endDateStr +
            ", note = '" +
            noteStr +
            "'  where client_status_id = " +
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
            string startDateStrNoQuote;
            TextBox startDateTextBox;
            startDateTextBox = (TextBox)e.Item.FindControl("txtAddNewStartDate");
            startDateStr = startDateTextBox.Text;
            startDateStrNoQuote = startDateStr;
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

            string passedClientIdStr = ddlMember.SelectedValue.ToString();
            string sqlStatement = "";
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            sqlStatement =
                " select start_date" +
                " from client_status" +
                " where client_status_id = " +
                " (select max(client_status_id)" +
                " from client_status where client_id = " +
                passedClientIdStr +
                ")";
            SqlCommand cmdDate = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            DateTime lastStartDate = Convert.ToDateTime(cmdDate.ExecuteScalar());
            myConnection.Close();

            DateTime newStartDate = Convert.ToDateTime(startDateStrNoQuote);
            //DateTime newStartDate = DateTime.Parse(startDateStr,"MM/dd/yyyy", System.Globalization.CultureInfo.CurrentUICulture);
            //DateTime newStartDate = ConvertToDateTime(startDateStrNoQuote);
            if (lastStartDate > newStartDate)
            {
                string msg = "The start date is not valid.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            sqlStatement =
                " update client_status" +
                " set end_date = " +
                startDateStr +
                " where client_status_id = " +
                " (select max(client_status_id)" +
                " from client_status where client_id = " +
                passedClientIdStr +
                ")";

            SqlCommand cmdUpdate = new SqlCommand(sqlStatement, myConnection);

            myConnection.Open();
            cmdUpdate.ExecuteNonQuery();
            myConnection.Close();


            string ddlIdStr = ddl.SelectedValue.ToString();

            sqlStatement =
                " insert into client_status" +
                " (client_id, l_client_status_id, start_date, end_date, note)" +
                " values(" +
                ddlMember.SelectedValue.ToString() +
                ", " +
                ddlIdStr +
                ", " +
                startDateStr +
                ", " +
                endDateStr +
                ", '" +
                noteStr +
                "')";

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
    private DateTime ConvertToDateTime(string strDateTime)
    {
        DateTime dtFinaldate; string sDateTime;
        try { dtFinaldate = Convert.ToDateTime(strDateTime); }
        catch (Exception e)
        {
            string[] sDate = strDateTime.Split('/');
            sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
            dtFinaldate = Convert.ToDateTime(sDateTime);
        }
        return dtFinaldate;
    }
    protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUpGrid();
    }
}