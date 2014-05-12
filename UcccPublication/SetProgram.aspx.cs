using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class SetProgram : System.Web.UI.Page
{
    public struct myPaper
    {
        public int pubId;
        public int yesNoId;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
            LoadLookup.LoadMember(ddlMember, "xxx",txtStartDate.Text,txtEndDate.Text);
        }

    }
    protected void btnGetPublicationlist_Click(object sender, EventArgs e)
    {
        FillPublicationGrid();

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

        //btnRemoveSelected.Visible = true;
    }
    protected void LoadPmid(DropDownList ddl, string pmidStr)
    {
        string startDate = "";
        string endDate = "";
        ErrorMessage.Text = "";
        if (txtStartDate.Text == "")
        {
            ErrorMessage.Text = "Please give start date.";
            return;
        }
        else
        {
            startDate = txtStartDate.Text;
        }
        if (txtEndDate.Text == "")
        {
            ErrorMessage.Text = "Please give end date.";
            return;
        }
        else
        {
            endDate = txtEndDate.Text;
        }
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement =
            "select " +
            " p.pmid" +
            " from publication p" +
            " inner join publication_author pa" +
            " on p.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            ddlMember.SelectedValue.ToString() +
            " inner join publication_processing pd" +
            " on pd.publication_id = pa.publication_id" +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " order by p.pmid";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string idStr = myReader["pmid"].ToString();
                ListItem li = new ListItem(idStr, idStr);
                if (pmidStr == idStr)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--pmid--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            //string currYesNoStr = drv["yesno"].ToString();
            
            string currProgramStr = drv["program"].ToString();
            DropDownList ddlProgramTemp = null;
            ddlProgramTemp = (DropDownList)e.Row.FindControl("ddlProgram");
            int programId = Convert.ToInt32(ddlMember.SelectedValue);
            if (ddlProgramTemp != null)
            {
                LoadProgramAbbreviation(ddlProgramTemp, currProgramStr);
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlPmidTemp = null;
            ddlPmidTemp = (DropDownList)e.Row.FindControl("ddlNewPmid");
            if (ddlPmidTemp == null)
            {
                return;
            }
            else
            {
                LoadPmid(ddlPmidTemp, "xxx");
            }

            DropDownList ddlProgramTemp = null;
            ddlProgramTemp = (DropDownList)e.Row.FindControl("ddlNewProgram");
            if (ddlProgramTemp == null)
            {
                return;
            }
            else
            {
                LoadProgramAbbreviation(ddlProgramTemp, "xxx");
            }
        }
    }
    protected void LoadProgramAbbreviation(DropDownList ddl, string program)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " l_program_id," +
            " ABBREVIATION as program" +
            " from l_program" +
            " where ABBREVIATION is not null and ABBREVIATION <> ''" +
            " order by ABBREVIATION";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["program"].ToString();
                string idStr = myReader["l_program_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (program == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--program--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
    }
    protected void gvPublication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPublication.EditIndex = -1;
        FillPublicationGrid();
    }
    protected void gvPublication_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvPublication.EditIndex = -1;

        FillPublicationGrid();
    }
    protected void gvPublication_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPublication.EditIndex = e.NewEditIndex;
        FillPublicationGrid();
    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Insert"))
        {
            DropDownList ddlNewPmidTemp = null;
            ddlNewPmidTemp = (DropDownList)gvPublication.FooterRow.FindControl("ddlNewPmid");
            if (ddlNewPmidTemp == null)
            {
                return;
            }
            if (ddlNewPmidTemp.SelectedIndex == 0 || ddlNewPmidTemp.SelectedIndex == -1)
            {
                //ErrorMessage.Text = "Please select block.";
                string msg = "Please select pmid.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            DropDownList ddlNewProgramTemp = null;
            ddlNewProgramTemp = (DropDownList)gvPublication.FooterRow.FindControl("ddlNewProgram");
            if (ddlNewProgramTemp == null)
            {
                return;
            }
            if (ddlNewProgramTemp.SelectedIndex == 0 || ddlNewProgramTemp.SelectedIndex == -1)
            {
                //ErrorMessage.Text = "Please select block.";
                string msg = "Please select program.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionStr);
            string sqlStatement;

            sqlStatement =
                "select count(*) from publication_program" +
                " where publication_id = " +
                " (select publication_id from publication where pmid = " +
                ddlNewPmidTemp.Text +
                ") and l_program_id = " +
                ddlNewProgramTemp.Text;

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int existingCnt = (int)commandCnt.ExecuteScalar();
            conn.Close();

            if (existingCnt > 0)
            {
                //ErrorMessage.Text = "There is an existing block for this subject and date. Please give another name for this block.";
                string msg = "This publication is already in this program. Try again.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                return;
            }

            sqlStatement =
                " insert into publication_program" +
                " (publication_id, l_program_id) values (" +
                " (select publication_id from publication where pmid = " +
                ddlNewPmidTemp.Text +
                ")," +
                ddlNewProgramTemp.Text  +
                ")";

            SqlCommand myCommandInsert = new SqlCommand(sqlStatement, conn);

            conn.Open();
            myCommandInsert.ExecuteNonQuery();
            conn.Close();

            gvPublication.EditIndex = -1;
            FillPublicationGrid();
        }
    }
    protected void gvPublication_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvPublication.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        string sqlStatement =
            "delete from publication_program" +
            " WHERE (publication_program_id = @publication_program_id)";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter publication_program_idParameter = new SqlParameter();
        publication_program_idParameter.ParameterName = "@publication_program_id";
        publication_program_idParameter.SqlDbType = SqlDbType.Int;
        publication_program_idParameter.Value = Convert.ToInt32(idStr);
        command.Parameters.Add(publication_program_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvPublication.EditIndex = -1;

        FillPublicationGrid();

    }
    protected void DeleteFromPubProgram(int pubProgId)
    {
        string sqlStatement =
            "delete from publication_program" +
            " WHERE (publication_program_id = @publication_program_id)";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter publication_program_idParameter = new SqlParameter();
        publication_program_idParameter.ParameterName = "@publication_program_id";
        publication_program_idParameter.SqlDbType = SqlDbType.Int;
        publication_program_idParameter.Value = pubProgId;
        command.Parameters.Add(publication_program_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();


    }
    public void FillPublicationGrid()
    {
        string sqlStatement;
        string startDate = "";
        string endDate = "";
        ErrorMessage.Text = "";
        if (txtStartDate.Text == "")
        {
            ErrorMessage.Text = "Please give start date.";
            return;
        }
        else
        {
            startDate = txtStartDate.Text;
        }
        if (txtEndDate.Text == "")
        {
            ErrorMessage.Text = "Please give end date.";
            return;
        }
        else
        {
            endDate = txtEndDate.Text;
        }
        string clientIdStr = "";
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            clientIdStr = ddlMember.SelectedValue.ToString();
        }
        else
        {
            ErrorMessage.Text = "Please select member.";
            return;
        }

        sqlStatement =
            "select pp.publication_program_id, p.pmid," +
            " pa.PUBLICATION_id," +
            " p.article_title as publication," +
            " pd.authorlist_no_inst," +
            " pp.l_program_id," +
            " lp.abbreviation as program" +
            " from publication p" +
            " inner join publication_author pa" +
            " on p.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientIdStr +
            " inner join publication_processing pd" +
            " on pd.publication_id = pa.publication_id" +
            " left outer join publication_program pp" +
            " on p.publication_id = pp.publication_id" +
            " left outer join l_program lp" +
            " on pp.l_program_id = lp.l_program_id" +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            //" order by pd.authorlist";
            " order by p.pub_year desc," +
            " replace(pd.authorlist,'<b>','')";


        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void btnAllYes_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            RadioButtonList ralist = gvPublication.Rows[i].FindControl("rblYesNo") as RadioButtonList;
            ralist.SelectedValue = "1";
            //Response.Write("line_" + Convert.ToString(i + 1) + "selectedindex_" + ralist.SelectedIndex.ToString());
        }

    }
    protected void btnRemoveSelected_Click(object sender, EventArgs e)
    {
        List<int> pubProgIdList = new List<int>();
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chxSelectTemp = (CheckBox)gvPublication.Rows[i].FindControl("chxSelect");
            if (chxSelectTemp == null)
            {
                return;
            }
            int id;
            Label lblIdTemp = null;
            lblIdTemp = (Label)gvPublication.Rows[i].FindControl("lblId");
            if (lblIdTemp != null)
            {
                if (chxSelectTemp.Checked)
                {
                    id = Convert.ToInt32(lblIdTemp.Text);
                    pubProgIdList.Add(id);
                }
            }
            else
            {
                return;
            }
        }
        //DelectPubs(pubProgIdList);
        foreach (int pubProgId in pubProgIdList)
        {
            DeleteFromPubProgram(pubProgId);
        }
        gvPublication.EditIndex = -1;

        FillPublicationGrid();

    }
}