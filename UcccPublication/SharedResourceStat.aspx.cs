using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class SharedResourceStat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            onePubDiv.Visible = false;
            LoadLookup.LoadProgram(ddlProgram, "xxx", false);
            LoadSharedResource(ddlSharedResource, "xxx");


            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
        }
    }
    protected int GetTotalForProgram(int programId, string startDate, string endDate)
    {
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

        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)commandCnt.ExecuteScalar();
        conn.Close();
        lblTotal.Text = total.ToString();
        return total;
    }
    protected int GetTotalForSharedResource(int resourceId, string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "select count(pd.publication_processing_id) from publication_processing pd" +
            " inner join publication_resource pp" +
            " on pd.publication_id = pp.publication_id" +
            " and pd.review_editorial is null" +
            " and pp.l_resource_id = " +
            resourceId.ToString() +
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
    protected void GetResourceStatForProgram(int programId, string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            " select res as resource, sum(cnt) as publications from" +
            " (select description as res, 0 as cnt from L_RESOURCE" +
            " union" +
            " select lr.description as res, COUNT(pr.l_resource_id) as cnt" +
            " from PUBLICATION_RESOURCE pr" +
            " inner join L_RESOURCE lr" +
            " on pr.l_resource_id = lr.l_resource_id" +
            " inner join publication_processing pd" +
            " on pr.publication_id = pd.publication_id" +
            " and pd.review_editorial is null" +
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
            " group by res" +
            " order by res";
        SqlDataSource dsPublication = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFishAlkResult;
        gvResource.DataSource = dsPublication;
        gvResource.DataBind();
        chartPublication.Titles[0].Text = ddlProgram.SelectedItem.ToString() + " uses Shared Resources";
        chartPublication.Series[0].XValueMember = "resource";
        chartPublication.DataSource = dsPublication;
    }
    protected void GetProgramStatForResource(int resourceId, string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            " select program, sum(cnt) as publications from" +
            " (select program_name as program, 0 as cnt from L_PROGRAM where abbreviation is not null" +
            " union" +
            " select lp.program_name as program, COUNT(pr.l_program_id) as cnt" +
            " from PUBLICATION_PROGRAM pr" +
            " inner join L_PROGRAM lp" +
            " on pr.l_program_id = lp.l_program_id" +
            " inner join publication_processing pd" +
            " on pr.publication_id = pd.publication_id" +
            " and pd.review_editorial is null" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " inner join publication_resource pp" +
            " on pd.publication_id = pp.publication_id" +
            " and pp.l_resource_id = " +
            resourceId.ToString() +
            " group by lp.program_name" +
            ") x" +
            " group by program" +
            " order by program";
        SqlDataSource dsPublication = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsFishAlkResult;
        gvResource.DataSource = dsPublication;
        gvResource.DataBind();
        chartPublication.Titles[0].Text = "Shared resource " + ddlSharedResource.SelectedItem.ToString() + " used by programs";
        chartPublication.Series[0].XValueMember = "program";
        chartPublication.DataSource = dsPublication;
    }
    protected void btnResourceOnProgramStat_Click(object sender, EventArgs e)
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
        int total = 0;
        //int programId;
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            int programId = Convert.ToInt32(ddlProgram.SelectedValue);
            total = GetTotalForProgram(programId, txtStartDate.Text, txtEndDate.Text);
            GetResourceStatForProgram(programId, txtStartDate.Text, txtEndDate.Text);
            ddlProgram.SelectedIndex = -1;
        }
        else if (ddlSharedResource.SelectedIndex != 0 && ddlSharedResource.SelectedIndex != -1)
        {
            int resourceId = Convert.ToInt32(ddlSharedResource.SelectedValue);
            total = GetTotalForSharedResource(resourceId, txtStartDate.Text, txtEndDate.Text);
            GetProgramStatForResource(resourceId, txtStartDate.Text, txtEndDate.Text);
            ddlSharedResource.SelectedIndex = -1;
        }
        else
        {
            ErrorMessage.Text = "Please select program or shared resource.";
            return;
        }

        //int programId = Convert.ToInt32(ddlProgram.SelectedValue);
        lblTotal.Text = "Total Publications: " + total.ToString();
        onePubDiv.Visible = true;

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void LoadSharedResource(DropDownList ddl, string resource)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement =
            "select l_resource_id," +
            " description resource" +
            " from l_resource";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            ddl.Items.Clear();
            while (myReader.Read())
            {
                string nameFromDB = myReader["resource"].ToString();
                string idStr = myReader["l_resource_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (resource == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--shared resource--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
}