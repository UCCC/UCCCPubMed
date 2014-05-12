using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public partial class SetFocusGroup : System.Web.UI.Page
{
    public struct FocusGroup
    {
        public int pubProgId;
        public int focusGroupId;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userIdStr = Session["userId"].ToString();
            string roleIdStr = Session["roleId"].ToString();
            if (roleIdStr == "1")
            {
                LoadLookup.LoadProgram(ddlProgram, "xxx", false);
            }
            if (roleIdStr == "2")
            {
                string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionStr);
                string sqlStatement;
                sqlStatement = "select" +
                    " right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name as program" +
                    " from pub_user pu" +
                    " inner join program_leader pl" +
                    " on pu.client_id = pl.client_id" +
                    " inner join l_program lp" +
                    " on pl.l_program_id = lp.l_program_id" +
                    " where pu.pub_user_id = " +
                    userIdStr;

                SqlCommand command = new SqlCommand(sqlStatement, conn);
                conn.Open();
                string program = "";
                object programObj = (object)command.ExecuteScalar();
                if (programObj != DBNull.Value)
                {
                    program = programObj.ToString();
                }
                conn.Close();
                LoadLookup.LoadProgram(ddlProgram, program, false);
                ddlProgram.Enabled = false;
                //int programId = Convert.ToInt32(ddlProgram.SelectedValue);
                //LoadLookup.LoadMemberOnProgram(programId, ddlMember);
            }

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }

            Menu myMenu = null;
            myMenu = (Menu)Master.FindControl("Menu1");
            if (myMenu != null)
            {
                if (roleIdStr != "1" && roleIdStr == "2")
                {
                    MenuItem mi = (MenuItem)myMenu.FindItem("Edit Pubs");
                    //mi.v
                }
            }
        }

    }
    protected void btnGetPublicationlist_Click(object sender, EventArgs e)
    {
        if (rbtnRemoved.Checked)
        {
            FillRemovedGrid();
            btnSave.Visible = false;
        }
        else
        {
            FillPublicationGrid();
            btnSave.Visible = true;
        }

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        btnSave.Visible = false;
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            string currGroupStr = drv["focus_group"].ToString();
            string currGroup = drv["l_focus_group_id"].ToString();
            string reviewEditorial = drv["review_editorial"].ToString();
            RadioButtonList rbl = null;
            rbl = (RadioButtonList)e.Row.FindControl("rblFocusGroup");
            if (rbl != null)
            {
                sqlStatement =
                    "select l_focus_group_id," +
                    " case when group_number >= 0 then" +
                    " 'FG ' + convert(varchar,group_number) + '-' + description" +
                    " else description end" +
                    " as focus_group" +
                    " from l_focus_group where l_program_id = " +
                    ddlProgram.SelectedValue.ToString();

                SqlDataSource dsFocusGroup = new SqlDataSource(connectionStr, sqlStatement);

                rbl.DataSource = dsFocusGroup;
                rbl.DataTextField = "focus_group";
                rbl.DataValueField = "l_focus_group_id";
                rbl.DataBind();
                /*
                ListItem li2 = new ListItem("Not Program-related", "-2");
                rbl.Items.Add(li2);
                 * */
                ListItem li = new ListItem("Review/Editorial – Do not report", "-1");
                rbl.Items.Add(li);
                rbl.SelectedValue = currGroup;
                if (reviewEditorial == "True")
                {
                    rbl.SelectedValue = (-1).ToString(); ;
                }
                ListItem li3 = new ListItem("Admin Office Use Only", "-2");
                rbl.Items.Add(li3);
            }

        }
    }
    protected void gvPublication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPublication.EditIndex = -1;
        FillPublicationGrid();
    }
    protected void gvPublication_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        ErrorMessage.Text = "";
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

        RadioButtonList rbl = null;
        rbl = (RadioButtonList)gvPublication.Rows[e.RowIndex].FindControl("rblFocusGroup");
        if (rbl == null)
        {
            return;
        }

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement =
            "Update publication_program" +
            " SET l_focus_group_id=@l_focus_group_id" +
            " WHERE publication_program_id = @publication_program_id";

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter l_focus_group_idParameter = new SqlParameter();
        l_focus_group_idParameter.ParameterName = "@l_focus_group_id";
        l_focus_group_idParameter.SqlDbType = SqlDbType.Int;
        if (rbl.SelectedIndex == -1)
        {
            l_focus_group_idParameter.Value = DBNull.Value;
        }
        else
        {
            l_focus_group_idParameter.Value = rbl.SelectedValue;
        }
        command.Parameters.Add(l_focus_group_idParameter);

        SqlParameter publication_program_idParameter = new SqlParameter();
        publication_program_idParameter.ParameterName = "@publication_program_id";
        publication_program_idParameter.SqlDbType = SqlDbType.Int;
        publication_program_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(publication_program_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();


        if (rbl.SelectedIndex != -1)
        {
            sqlStatement =
                "Update publication_processing set final_conform_id = 1 where publication_id =" +
                " (select publication_id from publication_program where publication_program_id = " +
                lblIdTemp.Text + ")";
            SqlCommand command2 = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            command2.ExecuteNonQuery();
            myConnection.Close();
        }

        gvPublication.EditIndex = -1;
        FillPublicationGrid();
    }
    protected void gvPublication_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPublication.EditIndex = e.NewEditIndex;
        FillPublicationGrid();
    }
    public void FillPublicationGrid()
    {
        string sqlStatement = "";
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text != "")
        {
            startDate = txtStartDate.Text;
        }
        else
        {
            startDate = "01/01/1990";
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            DateTime now = DateTime.Now;
            endDate = now.ToString();
        }
        string programIdStr = "";
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programIdStr = ddlProgram.SelectedValue.ToString();
        }
        else
        {
            ErrorMessage.Text = "Please select program.";
            return;
        }

        string groupNumberClause = "";
        if (rbtnFGSet.Checked)
        {
            groupNumberClause = " and group_number > 0";
        }
        else if (rbtnFG0.Checked)
        {
            groupNumberClause = " and group_number = 0";
        }
        else if (rbtnMultiMember.Checked)
        {
            groupNumberClause = " and group_number = -1";
        }
        else if (rbtnNotProgramRelated.Checked)
        {
            groupNumberClause = " and group_number = -2";
        }
        if (rbtnAll.Checked) //all
        {
            sqlStatement =
                "select pp.publication_program_id," +
                " p.pmid," +
                " pd.review_editorial," +
                " pd.PUBLICATION_id," +
                " pd.authorlist_no_inst," +
                " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
                " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication," +
                " pd.full_text_url," +
                " pp.l_focus_group_id," +
                " lfg.description as focus_group" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " and pd.final_confirm_id = 1" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pp.l_program_id = " +
                programIdStr +
                " left outer join l_focus_group lfg" +
                " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                " and lfg.group_number >= 0" +
                " where ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "')) order by" +
                " p.pub_year desc," +
                " replace(pd.authorlist_no_inst,'<b>','')";
        }
        else if (rbtnReview.Checked) // review/editorial
        {
            sqlStatement =
                "select pp.publication_program_id," +
                " p.pmid," +
                " pd.review_editorial," +
                " pd.PUBLICATION_id," +
                " pd.authorlist_no_inst," +
                " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
                " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication," +
                " pd.full_text_url," +
                //" pp.l_focus_group_id," +
                " 0 as l_focus_group_id," +
                " ' ' as focus_group" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " and pd.final_confirm_id = 1" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pp.l_program_id = " +
                programIdStr +
                //" and pp.l_focus_group_id is null" +
                " where" +
                " pd.review_editorial = 1" +
                " and" +
                " ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "')) order by" +
                " p.pub_year desc," +
                " replace(pd.authorlist_no_inst,'<b>','')";
        }
        else if (rbtnNoFg.Checked) // no focus group set
        {
            sqlStatement =
                "select pp.publication_program_id," +
                " p.pmid," +
                " pd.review_editorial," +
                " pd.PUBLICATION_id," +
                " pd.authorlist_no_inst," +
                " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
                " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication," +
                " pd.full_text_url," +
                //" pp.l_focus_group_id," +
                " 0 as l_focus_group_id," +
                //" lfg.description as focus_group" +
                " ' ' as focus_group" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " and pd.final_confirm_id = 1" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pp.l_program_id = " +
                programIdStr +
                " and pp.l_focus_group_id is null" +
                //" left outer join l_focus_group lfg" +
                //" on pp.l_focus_group_id = lfg.l_focus_group_id" +
                " where" +
                " pd.review_editorial is null" +
                " and" +
                " ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "')) order by" +
                " p.pub_year desc," +
                " replace(pd.authorlist_no_inst,'<b>','')";
        }
        else
        {
            sqlStatement =
                "select pp.publication_program_id," +
                " p.pmid," +
                " pd.review_editorial," +
                " pd.PUBLICATION_id," +
                " pd.authorlist_no_inst," +
                " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
                " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication," +
                " pd.full_text_url," +
                " pp.l_focus_group_id," +
                " lfg.description as focus_group" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " and pd.final_confirm_id = 1" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pp.l_program_id = " +
                programIdStr +
                " and pp.l_focus_group_id is not null" +
                " inner join l_focus_group lfg" +
                " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                groupNumberClause +
                //" and group_number > 0" +
                " where" +
                " pd.review_editorial is null" +
                " and" +
                " ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "')) order by" +
                " p.pub_year desc," +
                " replace(pd.authorlist_no_inst,'<b>','')";
        }

        Helper.BindGridview("", gvRemoved);
        Helper.BindGridview(sqlStatement, gvPublication);
    }
    public void FillRemovedGrid()
    {
        string sqlStatement = "";
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text != "")
        {
            startDate = txtStartDate.Text;
        }
        else
        {
            startDate = "01/01/1990";
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            DateTime now = DateTime.Now;
            endDate = now.ToString();
        }
        string programIdStr = "";
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programIdStr = ddlProgram.SelectedValue.ToString();
        }
        else
        {
            ErrorMessage.Text = "Please select program.";
            return;
        }

        sqlStatement =
            "select p.publication_id," +
            " p.pmid," +
            " pd.authorlist_no_inst," +
            " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
            " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " inner join publication_author pa" +
            " on p.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id is not null" +
            " inner join client_program cp" +
            " on a.client_id = cp.client_id" +
            " and cp.end_date is null" +
            " and cp.l_program_id = " +
            programIdStr +
            " where" +
            " ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "')) order by" +
            " p.pub_year desc," +
            " replace(pd.authorlist_no_inst,'<b>','')";

        Helper.BindGridview("", gvPublication);
        Helper.BindGridview(sqlStatement, gvRemoved);
        btnSelectAllToRestore.Visible = true;
        btnRestoreSelected.Visible = true;
        btnSave.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        List<FocusGroup> focusGroupList = new List<FocusGroup>();
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            FocusGroup fc = new FocusGroup();
            RadioButtonList rbl = (RadioButtonList)gvPublication.Rows[i].FindControl("rblFocusGroup");
            if (rbl == null)
            {
                return;
            }
            if (rbl.SelectedIndex != -1)
            {
                fc.focusGroupId = Convert.ToInt32(rbl.SelectedValue.ToString());

                ErrorMessage.Text = "";
                int id;

                Label lblIdTemp = null;
                lblIdTemp = (Label)gvPublication.Rows[i].FindControl("lblId");
                if (lblIdTemp != null)
                {
                    id = Convert.ToInt32(lblIdTemp.Text);
                }
                else
                {
                    return;
                }
                fc.pubProgId = id;
                focusGroupList.Add(fc);
            }
        }
        foreach (FocusGroup fc in focusGroupList)
        {
            SaveFocusGroupSelection(fc);
        }
        FillPublicationGrid();
        string msg = "Saved.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);

    }
    protected void SaveFocusGroupSelection(FocusGroup fc)
    {
        int publicationProgramId = fc.pubProgId;
        int focusGroupId = fc.focusGroupId;

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        if (focusGroupId == -1) //review/editorial
        {
            /*
            sqlStatement =
                "IF NOT EXISTS(SELECT pmid FROM reject_publication WHERE pmid = (select pmid from publication p inner join publication_program pp on p.publication_id = pp.publication_id" +
                " and pp.publication_program_id = @publication_program_id))" +
                " insert into reject_publication (pmid) values (" +
                "(select pmid from publication p inner join publication_program pp on p.publication_id = pp.publication_id" +
                " and pp.publication_program_id = @publication_program_id))";
             * */
            sqlStatement =
                "update publication_processing set review_editorial = 1" +
                " where publication_id =" +
                " (select publication_id from publication_program where publication_program_id =  @publication_program_id)";
            SqlCommand commandInsert = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_program_idParameterInsert = new SqlParameter();
            publication_program_idParameterInsert.ParameterName = "@publication_program_id";
            publication_program_idParameterInsert.SqlDbType = SqlDbType.Int;
            publication_program_idParameterInsert.Value = publicationProgramId;
            commandInsert.Parameters.Add(publication_program_idParameterInsert);

            myConnection.Open();
            commandInsert.ExecuteNonQuery();
            myConnection.Close();

            sqlStatement =
                "update publication_program set l_focus_group_id = null" +
                " where publication_program_id = @publication_program_id";
            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_program_idParameter = new SqlParameter();
            publication_program_idParameter.ParameterName = "@publication_program_id";
            publication_program_idParameter.SqlDbType = SqlDbType.Int;
            publication_program_idParameter.Value = publicationProgramId;
            command.Parameters.Add(publication_program_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();

        }
            /*
        else if (focusGroupId == -2) //not in the program, to be removed
        {
            sqlStatement =
                "delete from publication_program" +
                " WHERE (publication_program_id = @publication_program_id)";

            SqlCommand command = new SqlCommand(sqlStatement, myConnection);
            SqlParameter publication_program_idParameter = new SqlParameter();
            publication_program_idParameter.ParameterName = "@publication_program_id";
            publication_program_idParameter.SqlDbType = SqlDbType.Int;
            publication_program_idParameter.Value = publicationProgramId;
            command.Parameters.Add(publication_program_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();
        }
             * */
        else if (focusGroupId == -2) //unknown, clear
        {
            sqlStatement =
                "update publication_processing set review_editorial = null" +
                " where publication_id =" +
                " (select publication_id from publication_program where publication_program_id =  @publication_program_id)";
            SqlCommand commandInsert = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_program_idParameterInsert = new SqlParameter();
            publication_program_idParameterInsert.ParameterName = "@publication_program_id";
            publication_program_idParameterInsert.SqlDbType = SqlDbType.Int;
            publication_program_idParameterInsert.Value = publicationProgramId;
            commandInsert.Parameters.Add(publication_program_idParameterInsert);

            myConnection.Open();
            commandInsert.ExecuteNonQuery();
            myConnection.Close();

            sqlStatement =
                "update publication_program set l_focus_group_id = null" +
                " where publication_program_id = @publication_program_id";
            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_program_idParameter = new SqlParameter();
            publication_program_idParameter.ParameterName = "@publication_program_id";
            publication_program_idParameter.SqlDbType = SqlDbType.Int;
            publication_program_idParameter.Value = publicationProgramId;
            command.Parameters.Add(publication_program_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();
        }
        else //some focus group is selected
        {
            sqlStatement =
                "Update publication_program" +
                " SET l_focus_group_id=@l_focus_group_id" +
                " WHERE (publication_program_id = @publication_program_id)";

            SqlCommand command = new SqlCommand(sqlStatement, myConnection);
            SqlParameter l_focus_group_idParameter = new SqlParameter();
            l_focus_group_idParameter.ParameterName = "@l_focus_group_id";
            l_focus_group_idParameter.SqlDbType = SqlDbType.Int;
            if (focusGroupId != 0)
            {
                l_focus_group_idParameter.Value = focusGroupId;
            }
            else
            {
                l_focus_group_idParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(l_focus_group_idParameter);

            SqlParameter publication_program_idParameter = new SqlParameter();
            publication_program_idParameter.ParameterName = "@publication_program_id";
            publication_program_idParameter.SqlDbType = SqlDbType.Int;
            publication_program_idParameter.Value = publicationProgramId;
            command.Parameters.Add(publication_program_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();

            sqlStatement =
                "update publication_processing set review_editorial = null" +
                " where publication_id =" +
                " (select publication_id from publication_program where publication_program_id =  @publication_program_id)";
            SqlCommand command2 = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_program_idParameter2 = new SqlParameter();
            publication_program_idParameter2.ParameterName = "@publication_program_id";
            publication_program_idParameter2.SqlDbType = SqlDbType.Int;
            publication_program_idParameter2.Value = publicationProgramId;
            command2.Parameters.Add(publication_program_idParameter2);

            myConnection.Open();
            command2.ExecuteNonQuery();
            myConnection.Close();

        }
        gvPublication.EditIndex = -1;
        //FillPublicationGrid();
    }
    protected void gvRemoved_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ErrorMessage.Text = "";
        //btnSave.Visible = false;
    }
    protected void gvRemoved_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            string pubIdStr = drv["publication_id"].ToString();
            sqlStatement =
                "select count(*)" +
                " from publication_program where l_program_id = " +
                ddlProgram.SelectedValue.ToString() +
                " and publication_id = " +
                pubIdStr;

            SqlCommand command = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int cnt = (int)command.ExecuteScalar();
            conn.Close();

            if (cnt > 0)
            {
                e.Row.Visible = false;
            }
        }
    }
    protected void gvRemoved_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //gvRemoved.EditIndex = -1;
        //FillPublicationGrid();
    }
    protected void gvRemoved_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        /*
        ErrorMessage.Text = "";
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvRemoved.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        RadioButtonList rbl = null;
        rbl = (RadioButtonList)gvRemoved.Rows[e.RowIndex].FindControl("rblFocusGroup");
        if (rbl == null)
        {
            return;
        }

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement =
            "Update publication_program" +
            " SET l_focus_group_id=@l_focus_group_id" +
            " WHERE publication_program_id = @publication_program_id";

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter l_focus_group_idParameter = new SqlParameter();
        l_focus_group_idParameter.ParameterName = "@l_focus_group_id";
        l_focus_group_idParameter.SqlDbType = SqlDbType.Int;
        if (rbl.SelectedIndex == -1)
        {
            l_focus_group_idParameter.Value = DBNull.Value;
        }
        else
        {
            l_focus_group_idParameter.Value = rbl.SelectedValue;
        }
        command.Parameters.Add(l_focus_group_idParameter);

        SqlParameter publication_program_idParameter = new SqlParameter();
        publication_program_idParameter.ParameterName = "@publication_program_id";
        publication_program_idParameter.SqlDbType = SqlDbType.Int;
        publication_program_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(publication_program_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();


        if (rbl.SelectedIndex != -1)
        {
            sqlStatement =
                "Update publication_processing set final_conform_id = 1 where publication_id =" +
                " (select publication_id from publication_program where publication_program_id = " +
                lblIdTemp.Text + ")";
            SqlCommand command2 = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            command2.ExecuteNonQuery();
            myConnection.Close();
        }

        gvRemoved.EditIndex = -1;
        FillPublicationGrid();
         * */
    }
    protected void gvRemoved_RowEditing(object sender, GridViewEditEventArgs e)
    {
        /*
        gvRemoved.EditIndex = e.NewEditIndex;
        FillPublicationGrid();
         * */
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        List<int> pubIdList = new List<int>();
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chxSelectToRestoreTemp = (CheckBox)gvPublication.Rows[i].FindControl("chxSelectToRestore");
            if (chxSelectToRestoreTemp == null)
            {
                return;
            }
            int id;
            Label lblIdTemp = null;
            lblIdTemp = (Label)gvPublication.Rows[i].FindControl("lblId");
            if (lblIdTemp != null)
            {
                if (chxSelectToRestoreTemp.Checked)
                {
                    id = Convert.ToInt32(lblIdTemp.Text);
                    pubIdList.Add(id);
                }
            }
            else
            {
                return;
            }
        }
        //RestoreProgram(pubIdList);
    }
    protected void RestoreProgram(List<int> pubIdList)
    {
        string pubIdListStr = string.Join(",", pubIdList.Select(n => n.ToString()).ToArray());
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        foreach (int pubId in pubIdList)
        {
            sqlStatement =
                "insert into publication_program (publication_id, l_program_id)" +
                " values (" +
                pubId.ToString() +
                "," +
                ddlProgram.SelectedValue.ToString() +
                ")";

            SqlCommand command = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();

        }


        string authorIdStr = Request["authorId"].ToString();
        //FillPublicationGrid(authorIdStr);
    }
    protected void btnSelectAllToRestore_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gvRemoved.Rows.Count; i++)
        {
            CheckBox chx = gvRemoved.Rows[i].FindControl("chxSelectToRestore") as CheckBox;
            chx.Checked = true;
        }

    }
}