using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;

public partial class Inventory : System.Web.UI.Page
{
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }
    public enum inventoryType
    {
        noPmcid = 1,
        reviewEditorial,
        FGNotSet,
        noPubYearMonth,
        noFinalAuthorship
    };
    bool notCancerRelated = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userIdStr = Session["userId"].ToString();
            string roleIdStr = Session["roleId"].ToString();
            if (roleIdStr == "1")
            {
                LoadLookup.LoadProgram(ddlProgram, "xxx", true);
                ddlProgram.SelectedValue = "0";
                LoadLookup.LoadMemberOnProgram(0, ddlMember);
                LoadLookup.LoadResource(ddlResource, "xxx");
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
                LoadLookup.LoadProgram(ddlProgram, program, true);

                //ddlProgram.SelectedValue = "0";
                //LoadLookup.LoadMemberOnProgram(0, ddlMember);

                //ddlProgram.Enabled = false;
                int programId = Convert.ToInt32(ddlProgram.SelectedValue);
                LoadLookup.LoadMemberOnProgram(programId, ddlMember);

                //SelectProgram(sender, e);
            }

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
        }
        //string scriptStr = "function ShowConfirmation(){return confirm('hahaha');}";
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Hello", scriptStr, true);
        //Button1.Attributes.Add("OnClick", "javascript:return ShowConfirmation();");
    }
    protected void NullFocusCnt(string programIdStr, string startDate, string endDate, out int cntTotal, out int cntForcusGroup)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement =
            " select count(pa.publication_processing_id) from publication_processing pa" +
            " inner join publication_program pp" +
            " on pa.publication_id = pp.publication_id" +
            " and pa.review_editorial is null" +
            " and pp.l_program_id = " +
            programIdStr +
            " where ((pa.publication_date >= '" +
            startDate +
            "' and pa.publication_date <= '" +
            endDate +
            "'))";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        cntTotal = (int)myCommand.ExecuteScalar();
        conn.Close();

        sqlStatement =
            " select count(pa.publication_processing_id) from publication_processing pa" +
            " inner join publication_program pp" +
            " on pa.publication_id = pp.publication_id" +
            " and pa.review_editorial is null" +
            " and pp.l_program_id = " +
            programIdStr +
            " where ((pa.publication_date >= '" +
            startDate +
            "' and pa.publication_date <= '" +
            endDate +
            "'))" +
            " and pp.l_focus_group_id is not null";

        SqlCommand myCommandForcusGroup = new SqlCommand(sqlStatement, conn);
        conn.Open();
        cntForcusGroup = (int)myCommandForcusGroup.ExecuteScalar();
        conn.Close();
    }
    protected void SelectProgram(object sender, EventArgs e)
    {
        string programIdStr = ddlProgram.SelectedValue.ToString();
        int programId = Convert.ToInt32(programIdStr);
        //if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        LoadLookup.LoadMemberOnProgram(programId, ddlMember);
        if (ddlProgram.SelectedValue.ToString() != "0")
        {
        }
        else
        {
            //btnReportFocusNon0.Text = "Focus Groups Excluding 0";
        }
    }
    protected void btnProgramRemoved_Click(object sender, EventArgs e)
    {
        notCancerRelated = true;
        FillRemovedGrid();
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
            " pd.authorlist," +
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

        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (notCancerRelated)
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
    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idStr;
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        if (e.CommandName.Equals("Link"))
        {
            Label lblIdTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblIdTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                idStr = lblIdTemp.Text;
            }
            else
            {
                return;
            }
            sqlStatement = "select" +
                " pmid from publication where publication_id = " + idStr;

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int pmid = (int)commandCnt.ExecuteScalar();
            conn.Close();

            Response.Redirect("http://www.ncbi.nlm.nih.gov/pubmed/" + pmid.ToString());
        }
        //gvPublication.Visible = false;
    }
    protected string MakeSqlStatement(inventoryType type)
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
            ErrorMessage.Text = "please enter start date.";
            return "";
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            ErrorMessage.Text = "please enter end date.";
            return "";
        }
        string programIdStr = "";
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programIdStr = ddlProgram.SelectedValue.ToString();
        }
        string clientIdStr = "";
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            clientIdStr = ddlMember.SelectedValue.ToString();
        }
        if (programIdStr == "" && clientIdStr == "")
        {
            ErrorMessage.Text = "Please select a program or a member.";
            return "";
        }
        string conditionClause = "";
        if (type == inventoryType.noPmcid)
        {
            conditionClause = " and (p.pmcid is null or p.pmcid = '')"; 
        }
        else if (type == inventoryType.reviewEditorial)
        {
            conditionClause = " and pd.review_editorial = 1"; 
        }
        else if (type == inventoryType.FGNotSet)
        {
            conditionClause = " and pd.review_editorial is null  and pp.l_focus_group_id is null"; 
        }
        else if (type == inventoryType.noPubYearMonth)
        {
            conditionClause = " and (p.pub_year is null or p.pub_month is null)"; 
        }
        else if (type == inventoryType.noFinalAuthorship)
        {
            conditionClause = " and (pd.final_confirm_id is null or pd.final_confirm_id <> 1)"; 
        }
        if (clientIdStr != "")
        {
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.authorlist," +
                " isnull(p.article_title,'') +" +
                " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
                " case when p.volume is not null then p.volume + ':' else '' end + " +
                " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + ', ' else '' end +" +
                " case when p.pub_month is not null then p.pub_month + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " inner join publication_author pa" +
                " on pd.publication_id = pa.publication_id" +
                " inner join author a" +
                " on pa.author_id = a.author_id" +
                " and a.client_id = " +
                clientIdStr +
                " where ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'))" +
                conditionClause +
                " order by" +
                " p.pub_year desc," +
                " replace(pd.authorlist,'<b>','')";
        }
        else if (programIdStr != "0")
        {
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.authorlist," +
                " isnull(p.article_title,'') +" +
                " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
                " case when p.volume is not null then p.volume + ':' else '' end + " +
                " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + ', ' else '' end +" +
                " case when p.pub_month is not null then p.pub_month + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " inner join publication_program pp" +
                " on p.publication_id = pp.publication_id" +
                " and pp.l_program_id = " +
                programIdStr +
                " where ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'))" +
                conditionClause +
                " order by" +
                " p.pub_year desc," +
                " replace(pd.authorlist,'<b>','')";
        }
        else
        {
            return "";
        }
        return sqlStatement;
    }
    protected string MakeResourceSqlStatement()
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
            ErrorMessage.Text = "please enter start date.";
            return "";
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            ErrorMessage.Text = "please enter end date.";
            return "";
        }
        string programIdStr = "";
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programIdStr = ddlProgram.SelectedValue.ToString();
        }
        string clientIdStr = "";
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            clientIdStr = ddlMember.SelectedValue.ToString();
        }
        if (programIdStr == "" && clientIdStr == "")
        {
            ErrorMessage.Text = "Please select a program or a member.";
            return "";
        }
        if (clientIdStr != "")
        {
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.authorlist," +
                " isnull(p.article_title,'') +" +
                " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
                " case when p.volume is not null then p.volume + ':' else '' end + " +
                " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + ', ' else '' end +" +
                " case when p.pub_month is not null then p.pub_month + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication," +
                " lr.description as resource" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " inner join publication_resource pr" +
                " on p.publication_id = pr.publication_id" +
                " inner join l_resource lr" +
                " on pr.l_resource_id = lr.l_resource_id" +
                " inner join publication_author pa" +
                " on pd.publication_id = pa.publication_id" +
                " inner join author a" +
                " on pa.author_id = a.author_id" +
                " and a.client_id = " +
                clientIdStr +
                " where ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'))" +
                " order by" +
                " resource," +
                " p.pub_year desc," +
                " replace(pd.authorlist,'<b>','')";
        }
        else if (programIdStr != "0")
        {
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.authorlist," +
                " isnull(p.article_title,'') +" +
                " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
                " case when p.volume is not null then p.volume + ':' else '' end + " +
                " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + ', ' else '' end +" +
                " case when p.pub_month is not null then p.pub_month + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication," +
                " lr.description as resource" +
                " from publication_processing pd" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " inner join publication p" +
                " on pp.publication_id = p.publication_id" +
                " and pp.l_program_id = " +
                programIdStr +
                " inner join publication_resource pr" +
                " on pp.publication_id = pr.publication_id" +
                " inner join l_resource lr" +
                " on pr.l_resource_id = lr.l_resource_id" +
                " where ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'))" +
                " order by" +
                " resource," +
                " p.pub_year desc," +
                " replace(pd.authorlist,'<b>','')";
        }
        else
        {
            return "";
        }
        return sqlStatement;
    }
    protected string MakeProgPub4ResourceSqlStatement()
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
            ErrorMessage.Text = "please enter start date.";
            return "";
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            ErrorMessage.Text = "please enter end date.";
            return "";
        }
        string resourceIdStr = "";
        if (ddlResource.SelectedIndex != 0 && ddlResource.SelectedIndex != -1)
        {
            resourceIdStr = ddlResource.SelectedValue.ToString();
        }
        else 
        {
            ErrorMessage.Text = "Please select a shared resource";
            return "";
        }
        sqlStatement =
            "select p.publication_id," +
            " p.pmid," +
            " pd.authorlist," +
            " isnull(p.article_title,'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + ', ' else '' end +" +
            " case when p.pub_month is not null then p.pub_month + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication," +
            " lp.program_name as program" +
            " from publication_processing pd" +
            " inner join publication_program pp" +
            " on pd.publication_id = pp.publication_id" +
            " inner join l_program lp" +
            " on pp.l_program_id = lp.l_program_id" +
            " inner join publication p" +
            " on pp.publication_id = p.publication_id" +
            " inner join publication_resource pr" +
            " on pp.publication_id = pr.publication_id" +
            " and pr.l_resource_id = " +
            resourceIdStr +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " order by" +
            " program," +
            " p.pub_year desc," +
            " replace(pd.authorlist,'<b>','')";
        return sqlStatement;
    }
    protected void btnNoPmcid_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = true;
        gvResource.Visible = false;
        string sqlStatement = MakeSqlStatement(inventoryType.noPmcid);
        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void btnReviewEditorial_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = true;
        gvResource.Visible = false;

        string sqlStatement = MakeSqlStatement(inventoryType.reviewEditorial);
        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void btnFGNotSet_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = true;
        gvResource.Visible = false;
        string sqlStatement = MakeSqlStatement(inventoryType.FGNotSet);
        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void btnNoPubYearMonth_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = true;
        gvResource.Visible = false;
        string sqlStatement = MakeSqlStatement(inventoryType.noPubYearMonth);
        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void btnNotFinalAuthorship_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = true;
        gvResource.Visible = false;
        string sqlStatement = MakeSqlStatement(inventoryType.noFinalAuthorship);
        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void gvResource_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*
            DataRowView drv = (DataRowView)e.Row.DataItem;
            DataRow drRow = drv.Row;
            hdnCurrResource.Value = drv["resource"].ToString();
            if (hdnPrevResource.Value != hdnCurrResource.Value)
            {
                hdnPrevResource.Value = hdnCurrResource.Value;
            }
            else
            {
                Label lblTempResource = (Label)e.Row.Cells[1].FindControl("lblResource");
                lblTempResource.Text = "";
            }
            */
        }
    }
    protected void gvResource_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idStr;
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        if (e.CommandName.Equals("Link"))
        {
            Label lblIdTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblIdTemp = (Label)gvResource.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                idStr = lblIdTemp.Text;
            }
            else
            {
                return;
            }
            sqlStatement = "select" +
                " pmid from publication where publication_id = " + idStr;

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int pmid = (int)commandCnt.ExecuteScalar();
            conn.Close();

            Response.Redirect("http://www.ncbi.nlm.nih.gov/pubmed/" + pmid.ToString());
        }
        //gvPublication.Visible = false;
    }
    protected void btnResource_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = false;
        gvProgPub4Resource.Visible = false;
        gvResource.Visible = true;
        string sqlStatement = MakeResourceSqlStatement();
        Helper.BindGridview(sqlStatement, gvResource);
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            gvResource.Caption = "Member: " + ddlMember.SelectedItem.ToString();
        }
        else
        {
            gvResource.Caption = "Program: " + ddlProgram.SelectedItem.ToString();
        }
        
    }
    protected void btnProgPub4Resource_Click(object sender, EventArgs e)
    {

        gvPublication.Visible = false;
        gvResource.Visible = false;
        gvProgPub4Resource.Visible = true;
        string sqlStatement = MakeProgPub4ResourceSqlStatement();
        Helper.BindGridview(sqlStatement, gvProgPub4Resource);
        gvProgPub4Resource.Caption = "Shared resource: " + ddlResource.SelectedItem.ToString();
    }
}