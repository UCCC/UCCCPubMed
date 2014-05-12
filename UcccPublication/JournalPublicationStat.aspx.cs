using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class JournalPublicationStat : System.Web.UI.Page
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
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))";

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
        lblTotal.Text = total.ToString();
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

        string leastStr = "1";
        if (txtLeast.Text != "")
        {
            leastStr = txtLeast.Text;
        }

        sqlStatement =
            " select journal + '&startDate=' + '" +
            startDate +
            "' + '&endDate=' + '" +
            endDate +
            "' as journal_link," +
            " journal as journal_name," +
            " cnt as publications from" +
            " (select p.ISOAbbreviation as journal, COUNT(p.publication_id) as cnt" +
            " from PUBLICATION p" +
            " inner join publication_processing pd" +
            " on p.publication_id = pd.publication_id" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " group by ISOAbbreviation" +
            ") x where cnt >= " +
            leastStr +
            " order by cnt desc";

        /*
        sqlStatement =
            " select journal + '&startDate=' + '" +
            startDate +
            "' + '&endDate=' + '" +
            endDate +
            "' as journal_link," +
            " journal as journal_name," +
            " cnt as publications from" +
            " (select p.ISOAbbreviation as journal, COUNT(p.publication_id) as cnt" +
            " from PUBLICATION p" +
            " inner join publication_processing pd" +
            " on p.publication_id = pd.publication_id" +
            " and pd.publication_date >= '" +
            startDate +
            "' and publication_date <= '" +
            endDate +
            "' group by ISOAbbreviation" +
            ") x where cnt >= " +
            leastStr +
            " order by cnt desc";
        */
        SqlDataSource dsPublication = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFishAlkResult;
        gvPublication.DataSource = dsPublication;
        gvPublication.DataBind();

        int numberOfRows = gvPublication.Rows.Count;

        if (numberOfRows <= 20)
        {
            onePubDiv.Visible = true;
            chartPublication.DataSource = dsPublication;
        }
        else
        {
            onePubDiv.Visible = false;
       }

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

        GetPublicationStat(txtStartDate.Text, txtEndDate.Text);

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

        btnPubsSelectedJournals.Visible = true;
    }
    protected void btnPubsSelectedJournals_Click(object sender, EventArgs e)
    {
        List<string> journalList = new List<string>();
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chxSelectTemp = (CheckBox)gvPublication.Rows[i].FindControl("chxSelect");
            if (chxSelectTemp == null)
            {
                return;
            }
            string journalName; ;
            Label lblJournalNameTemp = null;
            lblJournalNameTemp = (Label)gvPublication.Rows[i].FindControl("lblJournalName");
            if (lblJournalNameTemp != null)
            {
                if (chxSelectTemp.Checked)
                {
                    journalName = lblJournalNameTemp.Text;
                    journalList.Add(journalName);
                }
            }
            else
            {
                return;
            }
        }
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
        Session["JOURNALLIST"] = journalList;
        Response.Redirect("~/PublicationOfOneJournal.aspx?startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text);
    }
    /*
    private void gvPublication_AutoGeneratingColumn(object sender, GridViewAutoGeneratingColumnEventArgs e)
    {
        GridViewDataColumn column = e.Column as GridViewDataColumn;
        if (column.DataType == typeof(int)
           || column.DataType == typeof(decimal)
           || column.DataType == typeof(float)
                )
        {
            column.TextAlignment = TextAlignment.Right;
        }
    }
     * */
}