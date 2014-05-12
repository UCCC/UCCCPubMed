using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class MemberPublicationStat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie _dateCookies = Request.Cookies["dates"];
        if (_dateCookies != null)
        {
            txtStartDate.Text = _dateCookies["startDate"];
            txtEndDate.Text = _dateCookies["endDate"];
        }

        if (!IsPostBack)
        {
            LoadLookup.LoadProgram(ddlProgram, "xxx", false);
        }
    }
    protected int GetTotal(int programId, string startDate, string endDate)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
            " inner join publication_program pp" +
            " on pd.publication_id = pp.publication_id" +
            " and pd.review_editorial is null" +
            " and pp.l_program_id = " +
            programId.ToString() +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))";
        /*
        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
            " inner join publication_program pp" +
            " on pd.publication_id = pp.publication_id" +
            " and pp.l_program_id = " +
            programId.ToString() +
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
    protected void GetPublicationStat(int programId, string startDate, string endDate)
    {
        string sqlStatement =
            " select" +
            " convert(varchar,client_id) + '&startDate=' + '" +
            startDate +
            "' + '&endDate=' + '" +
            endDate +
            "' as client_id," +
            " client as member, sum(cnt) as publications from" +
            " (select c.client_id," +
            " c.last_name + ', ' + c.first_name as client," +
            " 0 as cnt" +
            " from client c" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
            " inner join client_program cp" +
            " on c.client_id = cp.client_id" +
            " and cp.l_program_id = " +
            programId.ToString() +
            " union" +
            " select c.client_id," +
            " c.last_name + ', ' + c.first_name as client," +
            " COUNT(pa.publication_id) as cnt" +
            " from PUBLICATION_author pa" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " inner join client c" +
            " on a.client_id = c.client_id" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
            " inner join client_program cp" +
            " on c.client_id = cp.client_id" +
            " and cp.l_program_id = " +
            programId.ToString() +
            " inner join publication_processing pd" +
            " on pa.publication_id = pd.publication_id" +
            " and pd.review_editorial is null" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " group by c.client_id, c.last_name + ', ' + c.first_name" +
            ") x" +
            " group by client_id, client" +
            " having sum(cnt) > 0" +
            " order by client";

        /*
        string sqlStatement =
            " select" +
            " convert(varchar,client_id) + '&startDate=' + '" +
            startDate +
            "' + '&endDate=' + '" +
            endDate +
            "' as client_id," +
            " client as member, sum(cnt) as publications from" +
            " (select c.client_id," +
            " c.last_name + ', ' + c.first_name as client," +
            " 0 as cnt" +
            " from client c" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
            " inner join client_program cp" +
            " on c.client_id = cp.client_id" +
            " and cp.l_program_id = " +
            programId.ToString() +
            " union" +
            " select c.client_id," +
            " c.last_name + ', ' + c.first_name as client," +
            " COUNT(pa.publication_id) as cnt" +
            " from PUBLICATION_author pa" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " inner join client c" +
            " on a.client_id = c.client_id" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
            " inner join client_program cp" +
            " on c.client_id = cp.client_id" +
            " and cp.l_program_id = " +
            programId.ToString() +
            " inner join publication_processing pd" +
            " on pa.publication_id = pd.publication_id" +
            " and pd.publication_date >= '" +
            startDate +
            "' and publication_date <= '" +
            endDate +
            "' group by c.client_id, c.last_name + ', ' + c.first_name" +
            ") x" +
            " group by client_id, client" +
            " having sum(cnt) > 0" +
            " order by client";
        */
        Helper.BindGridview(sqlStatement, gvPublication);
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
        int programId;
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programId = Convert.ToInt32(ddlProgram.SelectedValue);
        }
        else
        {
            ErrorMessage.Text = "Please select program.";
            return;
        }

        int total = GetTotal(programId, txtStartDate.Text, txtEndDate.Text);
        lblTotal.Text = "Total Publications: " + total.ToString();

        GetPublicationStat(programId, txtStartDate.Text, txtEndDate.Text);

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
}