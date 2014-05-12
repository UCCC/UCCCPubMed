using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ProgramPubType : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLookup.LoadProgram(ddlProgram, "xxx", false);

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
        }
    }
    protected int GetTotal(int programId, string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
            " inner join publication_program pp" +
            " on pd.publication_id = pp.publication_id" +
            " and pp.l_program_id = " +
            programId.ToString() +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))";

        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)commandCnt.ExecuteScalar();
        conn.Close();
        lblTotal.Text = total.ToString();
        return total;
    }
    protected void GetPubtypeStat(int programId, string startDate, string endDate)
    {
        string sqlStatement = "";

        sqlStatement =
            " select pubtype, sum(cnt) as publications from" +
            " (select description as pubtype, 0 as cnt from pubtype" +
            " union" +
            " select lr.description as pubtype, COUNT(pr.pubtype_id) as cnt" +
            " from PUBLICATION_PUBTYPE pr" +
            " inner join pubtype lr" +
            " on pr.pubtype_id = lr.pubtype_id" +
            " inner join publication_processing pd" +
            " on pr.publication_id = pd.publication_id" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " inner join publication_program pp" +
            " on pd.publication_id = pp.publication_id" +
            " and pp.l_program_id = " +
            programId.ToString() +
            " group by lr.description" +
            ") x" +
            " group by pubtype" +
            " having sum(cnt) > 0" +
            " order by sum(cnt) desc";
        Helper.BindGridview(sqlStatement, gvPubtype);
    }
    protected void btnPubtypeStat_Click(object sender, EventArgs e)
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

        GetPubtypeStat(programId, txtStartDate.Text, txtEndDate.Text);

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
}