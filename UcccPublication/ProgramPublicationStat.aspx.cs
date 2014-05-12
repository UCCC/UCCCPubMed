using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ProgramPublicationStat : System.Web.UI.Page
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
            hdnTotal.Value = "0";
        }
    }
    protected int GetTotal(string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
            " where pd.review_editorial is null" +
            " and (pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "')";
        /*
        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
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
        //lblTotal.Text = total.ToString();
        return total;
    }
    protected void GetPublicationStat(string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement =
            " select convert(varchar,l_program_id) + '&startDate=' + '" +
            startDate +
            "' + '&endDate=' + '" +
            endDate +
            "' as l_program_id, program, sum(cnt) as publications" +
            " from" +
            " (select l_program_id, program_name as program, 0 as cnt from L_program" +
            " where abbreviation is not null and abbreviation <> ''" +
            " and l_program_id not in (2,7,12)" +
            " union" +
            " select lp.l_program_id as l_program_id, lp.program_name as program, COUNT(pp.publication_id) as cnt" +
            " from PUBLICATION_program pp" +
            " inner join l_focus_group lfg" +
            " on pp.l_focus_group_id = lfg.l_focus_group_id" +
            " and lfg.group_number > 0" +
            " inner join L_program lp" +
            " on pp.L_program_id = lp.L_program_id" +
            " and pp.l_program_id not in (2,7,12)" +
            " inner join publication_processing pd" +
            " on pp.publication_id = pd.publication_id" +
            " and pd.review_editorial is null" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " group by lp.l_program_id, lp.program_name" +
            ") x" +
            " group by l_program_id, program" +
            " order by program";
        /*
        sqlStatement =
            " select convert(varchar,l_program_id) + '&startDate=' + '" +
            startDate +
            "' + '&endDate=' + '" +
            endDate +
            "' as l_program_id, program, sum(cnt) as publications" +
            " from" +
            " (select l_program_id, program_name as program, 0 as cnt from L_program" +
            " where abbreviation is not null and abbreviation <> ''" +
            " and l_program_id not in (2,7)" +
            " union" +
            " select lp.l_program_id as l_program_id, lp.program_name as program, COUNT(pp.publication_id) as cnt" +
            " from PUBLICATION_program pp" +
            " inner join L_program lp" +
            " on pp.L_program_id = lp.L_program_id" +
            " and pp.l_program_id not in (2,7)" +
            " inner join publication_processing pd" +
            " on pp.publication_id = pd.publication_id" +
            " and pd.publication_date >= '" +
            startDate +
            "' and publication_date <= '" +
            endDate +
            "' group by lp.l_program_id, lp.program_name" +
            ") x" +
            " group by l_program_id, program" +
            " order by program";
         * */
        SqlDataSource dsPublication = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFishAlkResult;
        gvPublication.DataSource = dsPublication;
        gvPublication.DataBind();

        chartPublication.DataSource = dsPublication;
        lblTotal.Text = "Total Pubs: " + hdnTotal.Value;
        //gvPublication.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Label lblImageTypeTemp = (Label)e.Row.FindControl("lblPublications");
            int cnt = Convert.ToInt32(lblImageTypeTemp.Text);
            int hdnValue = Convert.ToInt32(hdnTotal.Value);
            hdnValue = hdnValue + cnt;
            hdnTotal.Value = hdnValue.ToString();
        }
    }
}