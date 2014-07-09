using System;
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

public partial class _Default : System.Web.UI.Page
{
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
            string username = Page.User.Identity.Name;
            string startDateStr = txtStartDate.Text;
            string endDateStr = txtEndDate.Text;
            //Helper.GetStartEndDate(out startDateStr, out endDateStr);
            int total = GetTotal(startDateStr, endDateStr);

            //lblTotal.Text = "Total Publications: " + total.ToString();
            GetPublicationStat(startDateStr, endDateStr);
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
                " where pd.review_editorial is null" +
                 " and ((pd.publication_date >= '" +
                 startDate +
                 "' and pd.publication_date <= '" +
                 endDate +
                 "'))";

            /*
            " where pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'";
            */
        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        object totlaObj = commandCnt.ExecuteScalar();
        int total = 0;
        if (totlaObj != null)
        {
            total = Convert.ToInt32(totlaObj);
        }
        conn.Close();
        //lblTotal.Text = total.ToString();
        return total;
    }
    protected void GetPublicationStat(string startDate, string endDate)
    {
        //DataSet ds = GetDataSet();
        //gvPublication.DataSource = ds;
        //gvPublication.DataSource = FlipDataSet(ds);

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            " select program, sum(cnt) as publications from" +
            " (select program_name as program, 0 as cnt from L_program" +
            " where abbreviation is not null and abbreviation <> ''" +
            " and l_program_id not in (2,7,12)" +
            " union" +
            " select lp.program_name as program, COUNT(pp.publication_id) as cnt" +
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
            " group by lp.program_name" +
            ") x" +
            " group by program" +
            " order by program";
        /*
        sqlStatement =
            " select program, sum(cnt) as publications from" +
            " (select program_name as program, 0 as cnt from L_program" +
            " where abbreviation is not null and abbreviation <> ''" +
            " and l_program_id not in (2,7)" +
            " union" +
            " select lp.program_name as program, COUNT(pp.publication_id) as cnt" +
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
            "' group by lp.program_name" +
            ") x" +
            " group by program" +
            " order by program";
         * 
         * */
        SqlDataSource dsPublication = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFishAlkResult;
        //gvPublication.DataSource = dsPublication;
        //gvPublication.DataBind();

        chartPublication.DataSource = dsPublication;

        //gvPublication.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
    }

    protected void btnShowGraph_Click(object sender, EventArgs e)
    {
        string startDateStr = txtStartDate.Text;
        string endDateStr = txtEndDate.Text;

        GetPublicationStat(startDateStr, endDateStr);

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);

        Response.Cookies.Add(_dateCookies);

        //Page_Load(sender, e);
    }
}
