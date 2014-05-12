using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class PublicationOfOneMember : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string clientIdStr = Request["clientId"].ToString();
            string startDate = Request["startDate"].ToString();
            string endDate = Request["endDate"].ToString();
            int clientId = Convert.ToInt32(clientIdStr);

            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionStr);
            string sqlStatement = "";

            sqlStatement =
                "select last_name + ', ' + first_name as name from client" +
                " where client_id = " +
                clientIdStr;

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            lblMember.Text = (string)commandCnt.ExecuteScalar();
            conn.Close();

            lblStartDate.Text = startDate;
            lblEndDate.Text = endDate;


            int total = GetTotal(clientId, startDate, endDate);
            lblTotal.Text = "Total Publications: " + total.ToString();

            GetPublicationStat(clientId, startDate, endDate);
        }
    }
    protected int GetTotal(int clientId, string startDate, string endDate)
    {
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
            clientId.ToString() +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))";

        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)commandCnt.ExecuteScalar();
        conn.Close();
        return total;
    }
    protected void GetPublicationStat(int clientId, string startDate, string endDate)
    {
        string sqlStatement = "";

        sqlStatement =
            "select" +
            " count(pd.publication_id) as publications," +
            " CONVERT(CHAR(4), pd.publication_date, 120) as the_year" +
            " from publication_processing pd" +
            " inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " and pd.review_editorial is null" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientId.ToString() +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " group by   CONVERT(CHAR(4), pd.publication_date, 120)";

        Helper.BindGridview(sqlStatement, gvPublication);
    }
}