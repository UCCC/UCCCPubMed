using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class MemberCitation : System.Web.UI.Page
{
    public struct PubIdTitle
    {
        public int publcationId;
        public string articleTitle;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadLookup.LoadMember(ddlMember, "xxx");
            LoadLookup.LoadProgram(ddlProgram, "xxx",true);
            ddlProgram.SelectedValue = "0";

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
            LoadLookup.LoadMemberOnProgram(0, ddlMember);

        }
    }
    protected void GetCitationStat()
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
        int clientId;
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            clientId = Convert.ToInt32(ddlMember.SelectedValue);
        }
        else
        {
            ErrorMessage.Text = "Please select a member.";
            return;
        }

        string sqlStatement = "";
        sqlStatement =
            " select p.publication_id,p.article_title as title, pd.authorlist, pd.citation as citation, p.pmid" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientId.ToString() +
            " order by citation desc";

        /*
        sqlStatement =
            " select p.publication_id,p.article_title as title, pd.authorlist, pd.citation as citation, p.pmid" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " and pd.publication_date >= '" +
            startDate +
            "' and publication_date <= '" +
            endDate +
            "' inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientId.ToString() +
            " order by citation desc";
         * */
        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void btnPublicationStat_Click(object sender, EventArgs e)
    {

        GetCitationStat();

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idStr;
        string titleStr;
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        if (e.CommandName.Equals("Link"))
        {
            Label lblIdTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblIdTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                idStr = lblIdTemp.Text;
                hdnPublicationId.Value = idStr;
            }
            else
            {
                return;
            }
            sqlStatement = "select" +
                " pmid from publication where publication_id = " + idStr;

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int pmid = (int)commandCnt.ExecuteScalar();
            conn.Close();

            Response.Redirect("http://www.ncbi.nlm.nih.gov/pubmed/" + pmid.ToString());



        }
        else if (e.CommandName.Equals("Google"))
        {
            Label lblTitleTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblTitleTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblTitle");
            if (lblTitleTemp != null)
            {
                titleStr = lblTitleTemp.Text;
            }
            else
            {
                return;
            }
            titleStr = titleStr.Replace(' ', '+');
            string strUrl = @"http://scholar.google.com/scholar?q=%22" + titleStr + @"%22&btnG=&hl=en&as_sdt=0%2C6";

            Response.Redirect(strUrl);
        }
        //gvPublication.Visible = false;
        //onePubDiv.Visible = true;
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbEditTemp = null;
            lbEditTemp = (LinkButton)e.Row.FindControl("lnkEdit");

            DataRowView drv = (DataRowView)e.Row.DataItem;

        }
    }
    protected void gvPublication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPublication.EditIndex = -1;
        GetCitationStat();
    }
    protected void gvPublication_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        ErrorMessage.Text = "";
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

        TextBox txtCitedByTemp = null;
        txtCitedByTemp = (TextBox)gvPublication.Rows[e.RowIndex].FindControl("txtCitedBy");
        if (txtCitedByTemp == null)
        {
            return;
        }

        string sqlStatement =
            "Update publication_processing" +
            " SET citation=@citation" +
            " WHERE (publication_id = @publication_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter citationParameter = new SqlParameter();
        citationParameter.ParameterName = "@citation";
        citationParameter.SqlDbType = SqlDbType.Int;
        citationParameter.Value = Convert.ToInt32(txtCitedByTemp.Text);
        command.Parameters.Add(citationParameter);

        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(publication_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvPublication.EditIndex = -1;

        GetCitationStat();
    }
    protected void gvPublication_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPublication.EditIndex = e.NewEditIndex;
        GetCitationStat();
    }
    protected void btnCitation_Click(object sender, EventArgs e)
    {
        ErrorMessage.Text = "";
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text != "")
        {
            startDate = txtStartDate.Text;
        }
        else
        {
            ErrorMessage.Text = "Please give start date.";
            return;
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            ErrorMessage.Text = "Please give end date.";
            return;
        }
        string clientIdStr = "";
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            clientIdStr = ddlMember.SelectedValue.ToString();
        }
        else
        {
            ErrorMessage.Text = "Please select member.";
            return;
        }
        sqlStatement =
            "select pd.publication_id," +
            " p.article_title" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientIdStr +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "')) order by p.article_title";
        /*
        sqlStatement =
            "select pd.publication_id," +
            " p.article_title" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientIdStr +
            " where pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'";
        */
        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        List<PubIdTitle> idTitleList = new List<PubIdTitle>();
        try
        {
            while (myReader.Read())
            {
                PubIdTitle pc = new PubIdTitle();
                pc.publcationId = Convert.ToInt32(myReader["publication_id"]);
                pc.articleTitle = myReader["article_title"].ToString();
                idTitleList.Add(pc);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

        int citationGot = 0;
        foreach (PubIdTitle pc in idTitleList)
        {
            int citation = UpdateCitation(pc);
            if (citation != -1)
            {
                citationGot++;
            }
        }
        int totalPub = idTitleList.Count;
        //string msg = "Among " + totalPub.ToString() + ", successfully got " + citationGot + " citation counts.";
        //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

        GetCitationStat();
    }
    protected int UpdateCitation(PubIdTitle pc)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        int citation = Helper.ReadCitation(pc.articleTitle);
        if (citation == -1)
        {
            return -1;
        }
        DateTime today = DateTime.Now;
        sqlStatement = "update publication_processing" +
            " set citation = @citation" +
            ", citation_date = @citation_date" +
            " where publication_id = @publication_id";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Value = pc.publcationId;
        command.Parameters.Add(publication_idParameter);

        SqlParameter citationParameter = new SqlParameter();
        citationParameter.ParameterName = "@citation";
        citationParameter.SqlDbType = SqlDbType.Int;
        citationParameter.Value = citation;
        command.Parameters.Add(citationParameter);

        SqlParameter citation_dateParameter = new SqlParameter();
        citation_dateParameter.ParameterName = "@citation_date";
        citation_dateParameter.SqlDbType = SqlDbType.DateTime;
        citation_dateParameter.Value = today;
        command.Parameters.Add(citation_dateParameter);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
        return citation;
    }
    protected void SelectProgram(object sender, EventArgs e)
    {
        int programId = Convert.ToInt32(ddlProgram.SelectedValue);
        LoadLookup.LoadMemberOnProgram(programId, ddlMember);
    }
}