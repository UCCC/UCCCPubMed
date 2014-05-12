using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ForEndNote : System.Web.UI.Page
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

        }
    }
    protected void btnGetPublication_Click(object sender, EventArgs e)
    {
        ErrorMessage.Text = "";
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text == "")
        {
            ErrorMessage.Text = "Please give start date.";
            return;
        }
        else
        {
            startDate = txtStartDate.Text;
        }
        if (txtEndDate.Text == "")
        {
            ErrorMessage.Text = "Please give end date.";
            return;
        }
        else
        {
            endDate = txtEndDate.Text;
        }
        FillPublicationGrid(startDate, endDate);
        btnExportToExcel.Visible = true;

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string pmidStr = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            Label lblPmidTemp = (Label)e.Row.FindControl("lblPmid");
            if (lblPmidTemp != null)
            {
                pmidStr = lblPmidTemp.Text;
            }
            else
            {
                return;
            }
            //Label lblAddressMatchTemp = (Label)e.Row.FindControl("lblAddressMatch");
            if (lblPmidTemp == null)
            {
                return;
            }

            string pubidStr = "";
            Label lblIdTemp = (Label)e.Row.FindControl("lblId");
            if (lblIdTemp != null)
            {
                pubidStr = lblIdTemp.Text;
            }
            else
            {
                return;
            }
            int pubId = Convert.ToInt32(pubidStr);

            string[] authorArray = GetAuthorList(pubId).ToArray();
            string authorlist = string.Join("//", authorArray);

            Label lblAuthorlistTemp = (Label)e.Row.FindControl("lblAuthorlist");
            if (lblAuthorlistTemp != null)
            {
                lblAuthorlistTemp.Text = authorlist;
            }
            else
            {
                return;
            }

            string[] memberArray = GetMemberList(pubId).ToArray();
            string memberlist = string.Join(", ", memberArray);
            Label lblMemberlistTemp = (Label)e.Row.FindControl("lblMemberlist");
            if (lblMemberlistTemp != null)
            {
                lblMemberlistTemp.Text = memberlist;
            }
            else
            {
                return;
            }

            string[] programArray = GetProgramList(pubId).ToArray();
            string programlist = string.Join(";", programArray);

            Label lblProgramTemp = (Label)e.Row.FindControl("lblProgram");
            if (lblProgramTemp != null)
            {
                lblProgramTemp.Text = programlist;
            }
            else
            {
                return;
            }
            string[] focusGroupArray = GetFocusGroupList(pubId).ToArray();
            string focusGrouplist = string.Join(";", focusGroupArray);

            Label lblFocusGroupTemp = (Label)e.Row.FindControl("lblFocusGroup");
            if (lblFocusGroupTemp != null)
            {
                lblFocusGroupTemp.Text = focusGrouplist;
            }
            else
            {
                return;
            }
            string[] resourceArray = GetResourceList(pubId).ToArray();
            string resourcelist = string.Join("/", resourceArray);

            Label lblResourceTemp = (Label)e.Row.FindControl("lblResource");
            if (lblResourceTemp != null)
            {
                lblResourceTemp.Text = resourcelist;
            }
            else
            {
                return;
            }
        }
    }
    public IEnumerable<string> GetAuthorList(int pubId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select a.LastName + ' ' + a.Initials as name" +
                " from author a" +
                " inner join publication_author pa" +
                " on a.author_id = pa.author_id" +
                " where pa.publication_id = " + pubId.ToString();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetString(reader.GetOrdinal("name"));
                }
            }
        }
    }
    public IEnumerable<string> GetMemberList(int pubId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select a.LastName + '-' + convert(varchar, a.client_id) as name_clientid" +
                " from author a" +
                " inner join publication_author pa" +
                " on a.author_id = pa.author_id" +
                " and a.client_id is not null" +
                " where pa.publication_id = " + pubId.ToString();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetString(reader.GetOrdinal("name_clientid"));
                }
            }
        }
    }
    public IEnumerable<string> GetProgramList(int pubId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select lp.abbreviation as program" +
                " from l_program lp" +
                " inner join publication_program pp" +
                " on lp.l_program_id = pp.l_program_id" +
                " where pp.publication_id = " + pubId.ToString();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetString(reader.GetOrdinal("program"));
                }
            }
        }
    }
    public IEnumerable<string> GetFocusGroupList(int pubId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select lp.abbreviation + '-' + convert(varchar,lfg.group_number) + '-' + lfg.description as focusGroup" +
                " from publication_program pp" +
                " inner join l_program lp" +
                " on pp.l_program_id = lp.l_program_id" +
                " inner join l_focus_group lfg" +
                " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                " where pp.publication_id = " + pubId.ToString();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetString(reader.GetOrdinal("focusGroup"));
                }
            }
        }
    }
    public IEnumerable<string> GetResourceList(int pubId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select lr.description as resource" +
                " from publication_resource ps" +
                " left outer join l_resource lr" +
                " on ps.l_resource_id = lr.l_resource_id" +
                " where ps.publication_id = " + pubId.ToString();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetString(reader.GetOrdinal("resource"));
                }
            }
        }
    }
    protected void FillPublicationGrid(string startDate, string endDate)
    {
        string sqlStatement = "";

        sqlStatement =
            " select distinct p.publication_id," +
            " p.pub_year," +
            " p.pub_month," +
            " convert(varchar,process.epub_date,101) as epub_date," +
            " p.article_title as article," +
            " p.ISOAbbreviation as journal," +
            " process.authorlist_no_inst," +
            " process.authorlist," +
            " p.article_title," +
            " p.Volume," +
            " p.issue," +
            " p.MedlinePgn," +
            " p.pmcid," +
            " p.pmid," +
            " process.full_text_url" +
            " from publication p" +
            " inner join publication_processing process" +
            " on p.publication_id = process.publication_id" +
            " and process.publication_date >= '" +
            startDate +
            "' and process.publication_date <= '" +
            endDate +
            "'";
        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        //SqlDataSource dsInventory = new SqlDataSource();
        //dsInventory = (SqlDataSource)Cache["INVENTORYDATASET"];
        //gvPublication.DataSource = dsInventory;
        //gvPublication.DataBind();
        //  pass the grid that for exporting ...
        Helper.Export("report.xls", this.gvPublication);

    }
}