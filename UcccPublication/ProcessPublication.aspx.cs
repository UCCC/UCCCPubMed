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
using System.Net;
using System.IO;

public partial class ProcessPublication : System.Web.UI.Page
{
    public enum AllOrNoProgram
    {
        all = 1,
        noProgram
    };
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            hdnPubCnt.Value = "0";
            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
            LoadLookup.LoadMember(ddlMember, "xxx",txtStartDate.Text,txtEndDate.Text);
        }
    }
    protected void btnProcessPubByPmid_Click(object sender, EventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "select publication_id from publication where pmid = " + txtPmid.Text;
        SqlCommand commandFromPmid = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        object pubIdObj = commandFromPmid.ExecuteScalar();
        int publicationId;
        //if (pubIdObj != DBNull.Value)
        if (pubIdObj != null)
        {
            publicationId = Convert.ToInt32(pubIdObj);
        }
        else
        {
            myConnection.Close();
            string msg = "Not found publication in database.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            return;
        }
        myConnection.Close();

        TreatInfoForOnePubId(publicationId);
        Response.Redirect("DisplayOnePubByPmid.aspx?pmid=" + txtPmid.Text);
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        //List<int> pubIdList = GetPubIdList(lastPubId).ToList();
        List<int> pubIdList = GetNotProcessedPubIdList().ToList();
        //ProcessPubList(pubIdList);
        //ConfirmByName(pubIdList);

        int totalConfirmedCnt = 0;
        int totalConflictCnt = 0;
        int totalUnknownCnt = 0;
        int confirmId = 0;
        foreach (int publicationId in pubIdList)
        {
           
            ProcessPub.ProcessAllInfoForOnePubId(publicationId, false);
            confirmId = ProcessPub.ConfirmOnePubByAuthorInitial(publicationId);
            ProcessPub.UpdateNameConfirm(publicationId, confirmId);
            if (confirmId == 1)
            {
                totalConfirmedCnt++;
            }
            else if (confirmId == 2)
            {
                totalConflictCnt++;
            }
            else
            {
                totalUnknownCnt++;
            }
        }
        int pubCnt = pubIdList.Count;
        string msg = "Among " + pubCnt.ToString() + " pubs, " + totalConfirmedCnt +
            " are name (initial) confirmed, " +
            totalConflictCnt.ToString() +
            " are name (initial) conflict, and " +
            totalUnknownCnt.ToString() +
            " are unknown.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
    }
    public IEnumerable<int> GetNotProcessedPubIdList()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            //cmd.CommandText = "select publication_id from publication where publication_id > " + lastPubId.ToString();
            cmd.CommandText = "select publication_id from publication where publication_id not in (select publication_id from publication_processing)";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("publication_id"));
                }
            }
        }
    }
    public IEnumerable<int> GetAllPubIdList()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            //cmd.CommandText = "select publication_id from publication where publication_id > " + lastPubId.ToString();
            //cmd.CommandText = "select publication_id from publication_processing where authorlist_no_inst is null";
            cmd.CommandText = "select publication_id from publication";
            //cmd.CommandText = "select publication_id from publication_processing where formated_article_title is null";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("publication_id"));
                }
            }
        }
    }
    public IEnumerable<int> GetNoEPubDatePubIdList()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            //cmd.CommandText = "select publication_id from publication where publication_id > " + lastPubId.ToString();
            cmd.CommandText = "select publication_id from publication_processing where epub_date is null";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("publication_id"));
                }
            }
        }
    }
    protected void TreatInfoForOnePubId(int publicationId)
    {
        //string pmidStr = pmid.ToString();

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "select count(*) as cnt from publication_processing where publication_id = " + publicationId.ToString();
        SqlCommand commandFromProcessing = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        int cnt = (int)commandFromProcessing.ExecuteScalar();
        myConnection.Close();

        if (cnt == 0)
        {
            ProcessPub.ProcessAllInfoForOnePubId(publicationId, false);
            int confirmId = ProcessPub.ConfirmOnePubByAuthorInitial(publicationId);
            ProcessPub.UpdateNameConfirm(publicationId, confirmId);
        }
        else if (cnt == 1)
        {
            ProcessPub.UpdateAllProcessInfoForOnePubId(publicationId);
            int confirmId = ProcessPub.ConfirmOnePubByAuthorInitial(publicationId);
            ProcessPub.UpdateNameConfirm(publicationId, confirmId);
        }
        else
        {
            //error
        }
    }
    protected void SavePublog(string value, DateTime theDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "insert into publog (value,log_date)" +
            " values(@value,@log_date)";

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter valueParameter = new SqlParameter();
        valueParameter.ParameterName = "@value";
        valueParameter.SqlDbType = SqlDbType.VarChar;
        valueParameter.Value = value;
        command.Parameters.Add(valueParameter);

        SqlParameter log_dateParameter = new SqlParameter();
        log_dateParameter.ParameterName = "@log_date";
        log_dateParameter.SqlDbType = SqlDbType.Date;
        log_dateParameter.Value = theDate;
        command.Parameters.Add(log_dateParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

    }
    protected void btnProcessSelectedPubs_Click(object sender, EventArgs e)
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
        foreach (int id in pubIdList)
        {
            //bool isRE = ProcessPub.IsReviewEditorial(id);
            ProcessPub.UpdateAllProcessInfoForOnePubId(id);
            int confirmId = ProcessPub.ConfirmOnePubByAuthorInitial(id);
            ProcessPub.UpdateNameConfirm(id, confirmId);
        }
        string msg = pubIdList.Count.ToString() + " publication are processed.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);

    }
    public void FillPublicationGrid(int an)
    {
        string sqlStatement;
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text != "")
        {
            startDate = txtStartDate.Text;
        }
        else
        {
            string msg =  "please enter start date.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "please enter start date.";
            return;
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            string msg = "please enter end date.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "please enter end date.";
            return;
        }

        if (an == (int)AllOrNoProgram.all)
        {
            string clientIdStr = "";
            if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
            {
                clientIdStr = ddlMember.SelectedValue.ToString();
            }
            else
            {
                string msg = "Please select member.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                //ErrorMessage.Text = "Please select member.";
                return;
            }
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.publication_processing_id," +
                " pd.authorlist," +
                " isnull(p.article_title,'') publication," + 
                " isnull(p.ISOAbbreviation,'') journal" +
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
                "'))" +
                //" order by pd.authorlist";
                " order by p.pub_year desc," +
                " replace(pd.authorlist,'<b>','')";

            /*
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.publication_processing_id," +
                " pd.authorlist," +
                " isnull(p.article_title,'') publication," + 
                " isnull(p.ISOAbbreviation,'') journal" +
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
             * */
        }
        else
        {
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.publication_processing_id," +
                " pd.authorlist," +
                " isnull(p.article_title,'') publication," +
                " isnull(p.ISOAbbreviation,'') journal" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " where ((pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'))" +
                " and p.publication_id not in" +
                " (select publication_id from publication_program)" +
                //" order by pd.authorlist";
                " order by p.pub_year desc," +
                " replace(pd.authorlist,'<b>','')";


            /*
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " pd.publication_processing_id," +
                " pd.authorlist," +
                " isnull(p.article_title,'') publication," +
                " isnull(p.ISOAbbreviation,'') journal" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " where pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'" +
                " and p.publication_id not in" +
                " (select publication_id from publication_program)";
             * */
        }

        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void btnGetPublication_Click(object sender, EventArgs e)
    {
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text == "")
        {
            string msg = "Please give start date.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "Please give start date.";
            return;
        }
        else
        {
            startDate = txtStartDate.Text;
        }
        if (txtEndDate.Text == "")
        {
            string msg = "Please give end date.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "Please give end date.";
            return;
        }
        else
        {
            endDate = txtEndDate.Text;
        }
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
        }
        else
        {
            string msg = "Please select a member.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "Please select program.";
            return;
        }

        FillPublicationGrid((int)AllOrNoProgram.all);
        btnProcessSelectedPubs.Visible = true;
        btnSelectAll.Visible = true;
        btnDelete.Visible = false;

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            CheckBox chx = gvPublication.Rows[i].FindControl("chxSelect") as CheckBox;
            chx.Checked = true;
        }

    }
    protected void btnAllNoProgram_Click(object sender, EventArgs e)
    {
        string startDate = "";
        string endDate = "";
        if (txtStartDate.Text == "")
        {
            string msg = "Please give start date.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "Please give start date.";
            return;
        }
        else
        {
            startDate = txtStartDate.Text;
        }
        if (txtEndDate.Text == "")
        {
            string msg = "Please give end date.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            //ErrorMessage.Text = "Please give end date.";
            return;
        }
        else
        {
            endDate = txtEndDate.Text;
        }
        FillPublicationGrid((int)AllOrNoProgram.noProgram);
        btnDelete.Visible = true;
        btnSelectAll.Visible = true;
        btnProcessSelectedPubs.Visible = false;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
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

        string authorIdStr = Request["authorId"].ToString();
        FillPublicationGrid((int)AllOrNoProgram.noProgram);
    }
    protected void btnReprocess_Click(object sender, EventArgs e)
    {
        List<int> pubIdList = GetAllPubIdList().ToList();

        int totalConfirmedCnt = 0;
        int totalConflictCnt = 0;
        int totalUnknownCnt = 0;
        int confirmId = 0;
        foreach (int publicationId in pubIdList)
        {
            ProcessPub.UpdateAllProcessInfoForOnePubId(publicationId);
            confirmId = ProcessPub.ConfirmOnePubByAuthorInitial(publicationId);
            ProcessPub.UpdateNameConfirm(publicationId, confirmId);
        }
        int pubCnt = pubIdList.Count;
        string msg = "Among " + pubCnt.ToString() + " pubs, " + totalConfirmedCnt +
            " are name (initial) confirmed, " +
            totalConflictCnt.ToString() +
            " are name (initial) conflict, and " +
            totalUnknownCnt.ToString() +
            " are unknown.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);

    }
    protected void btnReprocess_ClickTitle(object sender, EventArgs e)
    {
        List<int> pubIdList = GetAllPubIdList().ToList();

        foreach (int publicationId in pubIdList)
        {
            ProcessPub.UpdateTitleForOnePubId(publicationId);
        }
    }
    protected void btnPutEpubDate_Click(object sender, EventArgs e)
    {
        List<int> pubIdList = GetNoEPubDatePubIdList().ToList();

        foreach (int publicationId in pubIdList)
        {
            ProcessPub.UpdateEPubDateForOnePubId(publicationId);
        }
    }
}
