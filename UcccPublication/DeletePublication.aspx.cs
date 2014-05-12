using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public partial class DeletePublication : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillPublicationGrid();
        }
    }
    protected void btnGetPublication_Click(object sender, EventArgs e)
    {
        ErrorMessage.Text = "";

        FillPublicationGrid();

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);
        btnDelete.Visible = true;
        btnSelectAll.Visible = true;
        btnClearAll.Visible = true;
    }
    protected void FillPublicationGrid()
    {
        string sqlStatement = "";
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

        sqlStatement =
            " select distinct p.publication_id," +
            " p.article_title," +
            " isnull(p.article_title,'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as article," +
            " p.ISOAbbreviation as journal," +
            " p.pmid," +
            " pp.authorlist" +
            " from publication p" +
            " inner join publication_processing pp" +
            " on p.publication_id = pp.publication_id" +
            " and ((pp.publication_date >= '" +
            startDate +
            "' and pp.publication_date <= '" +
            endDate +
            "'))" +
            " where pp.final_confirm_id = 2" +
            " or pp.final_confirm_id is null" +
            //" order by pp.authorlist";
            " order by p.pub_year desc," +
            " replace(pp.authorlist,'<b>','')";



        //" left join publication_processing pp" +
        //" on p.publication_id = pa.publication_id";

        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void FillNoProgramGrid()
    {
        string sqlStatement = "";
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

        sqlStatement =
            " select distinct p.publication_id," +
            " p.article_title," +
            " isnull(p.article_title,'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as article," +
            " p.ISOAbbreviation as journal," +
            " p.pmid," +
            " pp.authorlist" +
            " from publication p" +
            " inner join publication_processing pp" +
            " on p.publication_id = pp.publication_id" +
            " and ((pp.publication_date >= '" +
            startDate +
            "' and pp.publication_date <= '" +
            endDate +
            "'))" +
            " where p.publication_id not in" +
            " (select publication_id from publication_program)";


        //" left join publication_processing pp" +
        //" on p.publication_id = pa.publication_id";

        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

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
            string autholist = string.Join(", ", authorArray);

            Label lblAuthorlistTemp = (Label)e.Row.FindControl("lblAuthorlist");
            if (lblAuthorlistTemp != null)
            {
                lblAuthorlistTemp.Text = autholist;
            }
            else
            {
                return;
            }
        }
    }
    protected void gvPublication_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvPublication.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        ProcessPub.SaveRejectOnPubId(Convert.ToInt32(idStr));

        sqlStatement =
                "delete from publication_author where publication_id = " + idStr +
                "; delete from publication_processing where publication_id = " + idStr +
                "; delete from publication_program where publication_id = " + idStr +
                "; delete from publication_programmatic where publication_id = " + idStr +
                "; delete from publication_pubtype where publication_id = " + idStr +
                "; delete from publication_resource where publication_id = " + idStr +
                "; delete from publication where publication_id = " + idStr +
                "; delete from author where author_id not in (select author_id from publication_author)";


        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter publication_program_idParameter = new SqlParameter();
        publication_program_idParameter.ParameterName = "@publication_program_id";
        publication_program_idParameter.SqlDbType = SqlDbType.Int;
        publication_program_idParameter.Value = Convert.ToInt32(idStr);
        command.Parameters.Add(publication_program_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvPublication.EditIndex = -1;

        FillPublicationGrid();
    }
    public IEnumerable<string> GetAuthorList(int pubId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select a.LastName + ' ' + a.ForeName as name" +
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
    protected void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        List<int> pubIdList = new List<int>();
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chxSelectTemp = (CheckBox)gvPublication.Rows[i].FindControl("chxSelect");
            if (chxSelectTemp == null)
            {
                return;
            }
            int id;
            Label lblIdTemp = null;
            lblIdTemp = (Label)gvPublication.Rows[i].FindControl("lblId");
            if (lblIdTemp != null)
            {
                if (chxSelectTemp.Checked)
                {
                    id = Convert.ToInt32(lblIdTemp.Text);
                    pubIdList.Add(id);
                }
            }
            else
            {
                return;
            }
        }
        DelectPubs(pubIdList);
    }
    protected void DelectPubs(List<int> pubIdList)
    {
        string pubIdListStr = string.Join(",", pubIdList.Select(n => n.ToString()).ToArray());
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        foreach (int pubId in pubIdList)
        {
            ProcessPub.SaveRejectOnPubId(pubId);

        }

        sqlStatement =
            "delete from publication_author where publication_id in (" + pubIdListStr + ")" +
            "; delete from publication_processing where publication_id in (" + pubIdListStr + ")" +
            "; delete from publication_program where publication_id in (" + pubIdListStr + ")" +
            "; delete from publication_programmatic where publication_id in (" + pubIdListStr + ")" +
            "; delete from publication_pubtype where publication_id in (" + pubIdListStr + ")" +
            "; delete from publication_resource where publication_id in (" + pubIdListStr + ")" +
            "; delete from publication where publication_id in (" + pubIdListStr + ")" +
            "; delete from author where author_id not in (select author_id from publication_author)";

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        FillPublicationGrid();
        int cnt = pubIdList.Count;
        ErrorMessage.Text = cnt.ToString() + " publications are deleted.";
    }
    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chx = gvPublication.Rows[i].FindControl("chxSelect") as CheckBox;
            chx.Checked = true;
        }

    }
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chx = gvPublication.Rows[i].FindControl("chxSelect") as CheckBox;
            chx.Checked = false;
        }

    }
    protected void btnNoProgram_Click(object sender, EventArgs e)
    {
        ErrorMessage.Text = "";

        FillNoProgramGrid();

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);
        btnDelete.Visible = true;
        btnSelectAll.Visible = true;
        btnClearAll.Visible = true;

    }
    protected void btnDeletePmid_Click(object sender, EventArgs e)
    {
        int pmid = Convert.ToInt32(txtPmid.Text);
        DeletePubOfPmid(pmid);
        string msg = "The publication with PMID " + pmid.ToString() + " is deleted.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
    }
    protected void DeletePubOfPmid(int pmid)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        //ProcessPub.SaveRejectOnPubId(pubId);

        sqlStatement =
            "delete from publication_author where publication_id = (select publication_id from publication where pmid = " + pmid.ToString() + ")" +
            "; delete from publication_processing where publication_id = (select publication_id from publication where pmid = " + pmid.ToString() + ")" +
            "; delete from publication_program where publication_id = (select publication_id from publication where pmid = " + pmid.ToString() + ")" +
            "; delete from publication_programmatic where publication_id = (select publication_id from publication where pmid = " + pmid.ToString() + ")" +
            "; delete from publication_pubtype where publication_id = (select publication_id from publication where pmid = " + pmid.ToString() + ")" +
            "; delete from publication_resource where publication_id = (select publication_id from publication where pmid = " + pmid.ToString() + ")" +
            "; delete from publication where pmid = " + pmid.ToString();

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();
    }
}