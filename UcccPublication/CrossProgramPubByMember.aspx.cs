using System;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;

public partial class CrossProgramPubByMember : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            onePubDiv.Visible = false;

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
            LoadLookup.LoadMember(ddlMember, "xxx", txtStartDate.Text, txtEndDate.Text);
        }
    }
    protected int GetTotal(string startDate, string endDate)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
            " inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " and pd.review_editorial is null" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            ddlMember.SelectedValue.ToString() +
            " where (pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "')";

        /*
            " where pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'";
        */
        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)commandCnt.ExecuteScalar();
        conn.Close();
        lblTotal.Text = total.ToString();
        return total;
    }
    protected void GetPublicationStat(string startDate, string endDate)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            " select sort, l_program_id, program, sum(publications) as publications from" +
            " (select case l_program_id when " +
            " (select l_program_id from client_program where client_id = " + 
            ddlMember.SelectedValue.ToString() +
            " and primary_program = 1)" +
            " then 0 else l_program_id end as sort," +
            " l_program_id, program_name as program, 0 as publications from L_program" +
            " where abbreviation is not null and abbreviation <> ''" +
            " and l_program_id not in (2,7)" +
            " union" +
            " select" +
            " case lp.l_program_id when " +
            " (select l_program_id from client_program where client_id = " +
            ddlMember.SelectedValue.ToString() +
            " and primary_program = 1)" +
            " then 0 else lp.l_program_id end as sort," +
            " lp.l_program_id as l_program_id," +
                 " lp.program_name as program," +
                 " COUNT(pp.publication_id) as publications" +
             " from PUBLICATION_program pp" +
             " inner join L_program lp" +
                 " on pp.L_program_id = lp.L_program_id" +
             " inner join publication_processing pd" +
                 " on pp.publication_id = pd.publication_id" +
                 " and pd.review_editorial is null" +
                 " and ((pd.publication_date >= '" +
                 startDate +
                 "' and pd.publication_date <= '" +
                 endDate +
                 "'))" +
            " where pp.publication_id in" +
            " (" +
             " select pp.publication_id" +
             " from PUBLICATION_program pp" +
             " inner join publication_processing pd" +
                 " on pp.publication_id = pd.publication_id" +
                 " and ((pd.publication_date >= '" +
                 startDate +
                 "' and pd.publication_date <= '" +
                 endDate +
                 "'))" +
                 " inner join PUBLICATION_AUTHOR pa" +
		                " on pp.publication_id = pa.publication_id" +
	             " inner join AUTHOR a" +
		                " on pa.author_id = a.author_id" +
		                " and a.client_id = " +
                        ddlMember.SelectedValue.ToString() +
             " where pp.l_program_id = " +
            " (select l_program_id from client_program where client_id = " +
            ddlMember.SelectedValue.ToString() +
            " and primary_program = 1)" +
             " )" +
             " group by lp.l_program_id, lp.program_name) x" +
             " group by l_program_id, program, sort" +
             " order by sort";
        SqlDataSource dsPublication = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFishAlkResult;
        gvPublication.DataSource = dsPublication;
        gvPublication.DataBind();

        chartPublication.DataSource = dsPublication;
    }
    protected void btnPublicationStat_Click(object sender, EventArgs e)
    {
        ErrorMessage.Text = "";
        if (txtStartDate.Text == "")
        {
            ErrorMessage.Text = "Please give start date.";
            return;
        }
        if (txtEndDate.Text == "")
        {
            ErrorMessage.Text = "Please give end date.";
            return;
        }
        if (ddlMember.SelectedIndex == 0 || ddlMember.SelectedIndex == -1)
        {
            ErrorMessage.Text = "Please select a member.";
            return;
        }

        int total = GetTotal(txtStartDate.Text, txtEndDate.Text);
        lblTotal.Text = "Total Publications: " + total.ToString();
        onePubDiv.Visible = true;

        GetPublicationStat(txtStartDate.Text, txtEndDate.Text);

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            string currProgramStr = drv["program"].ToString();
            Label lblImageTypeTemp = (Label)e.Row.FindControl("lblProgram");
            lblImageTypeTemp.Text = currProgramStr;
        }
         * */
    }
}