using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class EditPub : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLookup.LoadMember(ddlMember, "xxx", "01/01/2000", "01/01/2015");
            LoadMonth(ddlMonth, "xxx");
            LoadSeason(ddlSeason, "xxx");
            hdnPubId.Value = "0";

            object pubIdObj = Request["pubId"];
            string pubId = "";
            if (pubIdObj != null)
            {
                pubId = pubIdObj.ToString();
                FillTextBoxes(pubId);
            }

        }
    }
    protected void LoadSeason(DropDownList ddl, string currentSeason)
    {
        List<string> seasonList = new List<string>();
        seasonList.Add("Winter");
        seasonList.Add("Spring");
        seasonList.Add("Summer");
        seasonList.Add("Fall");

        foreach (string s in seasonList)
        {
            ListItem li = new ListItem(s);
            if (currentSeason == s)
            {
                //li.Selected = true;
            }
            ddl.Items.Add(li);
        }
        ddl.Items.Insert(0, "--season--");
        /*
        string listItemName = "Winter";
        ListItem li = new ListItem(listItemName);
        if (currentSeason == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Spring";
        li = new ListItem(listItemName);
        if (currentSeason == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Summer";
        li = new ListItem(listItemName);
        if (currentSeason == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Fall";
        li = new ListItem(listItemName);
        if (currentSeason == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        ddl.Items.Insert(0, "--season--");
        */
    }
    protected void LoadMonth(DropDownList ddl, string currentMonth)
    {


        List<string> monthList = new List<string>();
        monthList.Add("Jan");
        monthList.Add("Feb");
        monthList.Add("Mar");
        monthList.Add("Apr");
        monthList.Add("May");
        monthList.Add("Jun");
        monthList.Add("Jul");
        monthList.Add("Aug");
        monthList.Add("Sep");
        monthList.Add("Oct");
        monthList.Add("Nov");
        monthList.Add("Dec");

        ListItem li2 = new ListItem("--month--");
        ddl.Items.Add(li2);
        foreach (string s in monthList)
        {
            ListItem li = new ListItem(s);
            if (currentMonth == s)
            {
                //li.Selected = true;
            }
            ddl.Items.Add(li);
        }

        /*
        string listItemName = "Jan";
        ListItem li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Feb";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Mar";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Apr";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "May";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Jun";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Jul";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Aug";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Sep";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Oct";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Nov";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        listItemName = "Dec";
        li = new ListItem(listItemName);
        if (currentMonth == listItemName)
        {
            li.Selected = true;
        }
        ddl.Items.Add(li);
        ddl.Items.Insert(0, "--month--");
         * */
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string publicationIdStr = hdnPubId.Value;
        UpdatePublication(publicationIdStr);
        string msg = "Publication is updated.";
        //ErrorMessage.Text = msg;
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
    }
    protected void UpdatePublication(string publicationIdStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;
        sqlStatement =
            " update publication" +
            " set journal_title=@journal_title,volume=@volume,issue=@issue,pub_year=@pub_year,pub_season=@pub_season," +
            " pub_month=@pub_month,pub_day=@pub_day,article_title=@article_title,ISOAbbreviation=@ISOAbbreviation," +
            " abstract_text=@abstract_text,MedlinePgn=@MedlinePgn,pmcid=@pmcid,pmid=@pmid,CollectiveName=@CollectiveName" +
            " where publication_id=" +
            publicationIdStr;


        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

        SqlParameter journal_titleParameter = new SqlParameter();
        journal_titleParameter.ParameterName = "@journal_title";
        journal_titleParameter.SqlDbType = SqlDbType.VarChar;
        if (txtJournalTitle.Text != "")
        {
            journal_titleParameter.Value = txtJournalTitle.Text;
        }
        else
        {
            journal_titleParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(journal_titleParameter);

        SqlParameter volumeParameter = new SqlParameter();
        volumeParameter.ParameterName = "@volume";
        volumeParameter.SqlDbType = SqlDbType.VarChar;
        if (txtVolume.Text != "")
        {
            volumeParameter.Value = txtVolume.Text;
        }
        else
        {
            volumeParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(volumeParameter);

        SqlParameter issueParameter = new SqlParameter();
        issueParameter.ParameterName = "@issue";
        issueParameter.SqlDbType = SqlDbType.VarChar;
        if (txtIssue.Text != "")
        {
            issueParameter.Value = txtIssue.Text;
        }
        else
        {
            issueParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(issueParameter);

        string SeasonStr = "";
        if (ddlSeason.SelectedIndex != 0 && ddlSeason.SelectedIndex != -1)
        {
            SeasonStr = ddlSeason.SelectedItem.ToString();
        }
        string MonthStr = "";
        if (ddlMonth.SelectedIndex != 0 && ddlMonth.SelectedIndex != -1)
        {
            MonthStr = ddlMonth.SelectedItem.ToString();
        }

        SqlParameter pub_yearParameter = new SqlParameter();
        pub_yearParameter.ParameterName = "@pub_year";
        pub_yearParameter.SqlDbType = SqlDbType.Int;
        if (txtYear.Text != "")
        {
            pub_yearParameter.Value = Convert.ToInt32(txtYear.Text);
        }
        else
        {
            pub_yearParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pub_yearParameter);

        SqlParameter pub_seasonParameter = new SqlParameter();
        pub_seasonParameter.ParameterName = "@pub_season";
        pub_seasonParameter.SqlDbType = SqlDbType.VarChar;
        if (SeasonStr != "")
        {
            pub_seasonParameter.Value = SeasonStr;
        }
        else
        {
            pub_seasonParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pub_seasonParameter);

        SqlParameter pub_monthParameter = new SqlParameter();
        pub_monthParameter.ParameterName = "@pub_month";
        pub_monthParameter.SqlDbType = SqlDbType.VarChar;
        if (MonthStr != "")
        {
            pub_monthParameter.Value = MonthStr;
        }
        else
        {
            pub_monthParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pub_monthParameter);

        SqlParameter pub_dayParameter = new SqlParameter();
        pub_dayParameter.ParameterName = "@pub_day";
        pub_dayParameter.SqlDbType = SqlDbType.Int;
        if (txtDay.Text != "")
        {
            pub_dayParameter.Value = Convert.ToInt32(txtDay.Text);
        }
        else
        {
            pub_dayParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pub_dayParameter);

        SqlParameter article_titleParameter = new SqlParameter();
        article_titleParameter.ParameterName = "@article_title";
        article_titleParameter.SqlDbType = SqlDbType.VarChar;
        if (txtArticleTitle.Text != "")
        {
            article_titleParameter.Value = txtArticleTitle.Text;
        }
        else
        {
            article_titleParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(article_titleParameter);

        SqlParameter ISOAbbreviationParameter = new SqlParameter();
        ISOAbbreviationParameter.ParameterName = "@ISOAbbreviation";
        ISOAbbreviationParameter.SqlDbType = SqlDbType.VarChar;
        if (txtISOAbbreviation.Text != "")
        {
            ISOAbbreviationParameter.Value = txtISOAbbreviation.Text;
        }
        else
        {
            ISOAbbreviationParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(ISOAbbreviationParameter);

        SqlParameter abstract_textParameter = new SqlParameter();
        abstract_textParameter.ParameterName = "@abstract_text";
        abstract_textParameter.SqlDbType = SqlDbType.VarChar;
        if (txtAbstract.Text != "")
        {
            abstract_textParameter.Value = txtAbstract.Text;
        }
        else
        {
            abstract_textParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(abstract_textParameter);

        SqlParameter MedlinePgnParameter = new SqlParameter();
        MedlinePgnParameter.ParameterName = "@MedlinePgn";
        MedlinePgnParameter.SqlDbType = SqlDbType.VarChar;
        if (txtMedlinePgn.Text != "")
        {
            MedlinePgnParameter.Value = txtMedlinePgn.Text;
        }
        else
        {
            MedlinePgnParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(MedlinePgnParameter);

        SqlParameter pmcidParameter = new SqlParameter();
        pmcidParameter.ParameterName = "@pmcid";
        pmcidParameter.SqlDbType = SqlDbType.VarChar;
        if (txtPmcid.Text != "")
        {
            pmcidParameter.Value = txtPmcid.Text;
        }
        else
        {
            pmcidParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pmcidParameter);

        SqlParameter pmidParameter = new SqlParameter();
        pmidParameter.ParameterName = "@pmid";
        pmidParameter.SqlDbType = SqlDbType.Int;
        if (txtPmid.Text != "")
        {
            pmidParameter.Value = Convert.ToInt32(txtPmid.Text);
        }
        else
        {
            pmidParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pmidParameter);

        SqlParameter CollectiveNameParameter = new SqlParameter();
        CollectiveNameParameter.ParameterName = "@CollectiveName";
        CollectiveNameParameter.SqlDbType = SqlDbType.VarChar;
        if (txtCollectiveName.Text != "")
        {
            CollectiveNameParameter.Value = txtCollectiveName.Text;
        }
        else
        {
            CollectiveNameParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(CollectiveNameParameter);


        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Value = Convert.ToInt32(publicationIdStr);
        myCommand.Parameters.Add(publication_idParameter);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        //string pubDate = MakePubDate();
        if (txtYear.Text == "")
        {
            ErrorMessage.Text = "No pub year.";
            return;
        }

        string pubDate = MakePubDate(txtYear.Text, SeasonStr, MonthStr, txtDay.Text);

        sqlStatement =
            " update publication_processing" +
            " set publication_date=@publication_date,full_text_url=@full_text_url" +
            " where publication_id=" +
            publicationIdStr;

        SqlCommand myCommand2 = new SqlCommand(sqlStatement, myConnection);

        SqlParameter publication_dateParameter = new SqlParameter();
        publication_dateParameter.ParameterName = "@publication_date";
        publication_dateParameter.SqlDbType = SqlDbType.DateTime;
        if (pubDate != "")
        {
            publication_dateParameter.Value = Convert.ToDateTime(pubDate);
        }
        else
        {
            publication_dateParameter.Value = DBNull.Value;
        }
        myCommand2.Parameters.Add(publication_dateParameter);

        SqlParameter full_text_urlParameter = new SqlParameter();
        full_text_urlParameter.ParameterName = "@full_text_url";
        full_text_urlParameter.SqlDbType = SqlDbType.VarChar;
        if (txtPmcid.Text != "")
        {
            string url = url = @"http://www.ncbi.nlm.nih.gov/pmc/articles/" + txtPmcid.Text + "/";
            full_text_urlParameter.Value = url;
        }
        else
        {
            full_text_urlParameter.Value = DBNull.Value;
        }
        myCommand2.Parameters.Add(full_text_urlParameter);

        SqlParameter publication_idParameter2 = new SqlParameter();
        publication_idParameter2.ParameterName = "@publication_id";
        publication_idParameter2.SqlDbType = SqlDbType.Int;
        publication_idParameter2.Value = Convert.ToInt32(publicationIdStr);
        myCommand2.Parameters.Add(publication_idParameter2);

        myConnection.Open();
        myCommand2.ExecuteNonQuery();
        myConnection.Close();
    }
    protected string MakePubDate(string YearStr, string SeasonStr, string MonthStr, string DayStr)
    {
        //string YearStr = txtYear.Text;
        //string SeasonStr = ddlSeason.SelectedItem.ToString();
        //string MonthStr = ddlMonth.SelectedItem.ToString();
        //string DayStr = txtDay.Text;
        string publicationDate = "";
        string newMonthStr = "";
        string newDayStr = "";
        if (SeasonStr == "")
        {
            if (MonthStr != "")
            {
                if (DayStr != "")
                {
                    //DayStr = "0" + DayStr;
                    //DayStr = DayStr.Substring(DayStr.Length - 2);
                }
                else
                {
                    newDayStr = "01";
                }
                ProcessPub.ConvertMonthStr(MonthStr, DayStr, out newMonthStr, out newDayStr);
                publicationDate = newMonthStr + "/" + newDayStr + "/" + YearStr;
            }
            else
            {
                publicationDate = "01/01/" + YearStr;
            }

        }
        else //if (SeasonStr != "")
        {
            switch (SeasonStr)
            {
                case "Winter":
                    newMonthStr = "01";
                    newDayStr = "01";
                    break;
                case "Spring":
                    newMonthStr = "04";
                    newDayStr = "01";
                    break;
                case "Summer":
                    newMonthStr = "07";
                    newDayStr = "01";
                    break;
                case "Fall":
                case "Autumn":
                    newMonthStr = "10";
                    newDayStr = "01";
                    break;
                default:
                    break;
            }
            publicationDate = newMonthStr + "/" + newDayStr + "/" + YearStr;
        }
        return publicationDate;

    }
    protected void gvAuthor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbEditTemp = null;
            lbEditTemp = (LinkButton)e.Row.FindControl("lnkEdit");
            LinkButton lbDeleteTemp = null;
            lbDeleteTemp = (LinkButton)e.Row.FindControl("lnkDelete");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            string currClientStr = drv["client"].ToString();

            DropDownList ddlMemberTemp = null;
            ddlMemberTemp = (DropDownList)e.Row.FindControl("ddlMember");
            if (ddlMemberTemp != null)
            {
                LoadLookup.LoadMember(ddlMemberTemp, currClientStr, "01/01/2000", "01/01/2015");
            }
            TextBox txtLastNameTemp = null;
            txtLastNameTemp = (TextBox)e.Row.FindControl("txtLastName");
            if (txtLastNameTemp == null)
            {
                return;
            }
            else
            {
                txtLastNameTemp.Text = drv["LastName"].ToString();
            }
            string ForeName = drv["ForeName"].ToString();
            string Initials = drv["Initials"].ToString();
            string firstName;
            string initial;
            FromForeNameToFirstName(ForeName, Initials, out firstName, out initial);
            TextBox txtFirstNameTemp = null;
            txtFirstNameTemp = (TextBox)e.Row.FindControl("txtFirstName");
            if (txtFirstNameTemp == null)
            {
                return;
            }
            else
            {
                txtFirstNameTemp.Text = firstName;
            }
            TextBox txtInitialTemp = null;
            txtInitialTemp = (TextBox)e.Row.FindControl("txtInitial");
            if (txtInitialTemp == null)
            {
                return;
            }
            else
            {
                txtInitialTemp.Text = initial;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            LinkButton lbAddTemp = null;
            lbAddTemp = (LinkButton)e.Row.FindControl("lnkAdd");
            if (lbAddTemp == null)
            {
                return;
            }
            TextBox txtLastNameTemp = null;
            txtLastNameTemp = (TextBox)e.Row.FindControl("txtNewLastName");
            if (txtLastNameTemp == null)
            {
                return;
            }
            TextBox txtFirstNameTemp = null;
            txtFirstNameTemp = (TextBox)e.Row.FindControl("txtNewFirstName");
            if (txtFirstNameTemp == null)
            {
                return;
            }
            TextBox txtInitialTemp = null;
            txtInitialTemp = (TextBox)e.Row.FindControl("txtNewInitial");
            if (txtInitialTemp == null)
            {
                return;
            }
            DropDownList ddlMemberTemp = null;
            ddlMemberTemp = (DropDownList)e.Row.FindControl("ddlNewMember");
            if (ddlMemberTemp == null)
            {
                return;
            }
            else
            {
                LoadLookup.LoadMember(ddlMemberTemp, "xxx", "01/01/2000", "01/01/2015");
            }

        }
    }
    protected void gvAuthor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvAuthor.EditIndex = -1;
        string pubIdStr = hdnPubId.Value;
        int pubId = Convert.ToInt32(pubIdStr);
        FillAuthorGrid(pubId);
    }
    protected void gvAuthor_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvAuthor.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        TextBox txtLastNameTemp = null;
        txtLastNameTemp = (TextBox)gvAuthor.Rows[e.RowIndex].FindControl("txtLastName");
        if (txtLastNameTemp == null)
        {
            return;
        }
        TextBox txtFirstNameTemp = null;
        txtFirstNameTemp = (TextBox)gvAuthor.Rows[e.RowIndex].FindControl("txtFirstName");
        if (txtFirstNameTemp == null)
        {
            return;
        }
        TextBox txtInitialTemp = null;
        txtInitialTemp = (TextBox)gvAuthor.Rows[e.RowIndex].FindControl("txtInitial");
        if (txtInitialTemp == null)
        {
            return;
        }
        DropDownList ddlMemberTemp = null;
        ddlMemberTemp = (DropDownList)gvAuthor.Rows[e.RowIndex].FindControl("ddlMember");
        if (ddlMemberTemp == null)
        {
            return;
        }

        string sqlStatement =
            "Update publication_author" +
            " SET description=@description," +
            " group_number=@group_number," +
            " client_id=@client_id" +
            " WHERE (author_id = @author_id)";

        //idStr;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter LastNameParameter = new SqlParameter();
        LastNameParameter.ParameterName = "@LastName";
        LastNameParameter.SqlDbType = SqlDbType.Int;
        if (txtLastNameTemp.Text != "")
        {
            LastNameParameter.Value = txtLastNameTemp.Text;
        }
        else
        {
            LastNameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(LastNameParameter);

        SqlParameter FirstNameParameter = new SqlParameter();
        FirstNameParameter.ParameterName = "@FirstName";
        FirstNameParameter.SqlDbType = SqlDbType.Int;
        if (txtFirstNameTemp.Text != "")
        {
            FirstNameParameter.Value = txtFirstNameTemp.Text;
        }
        else
        {
            FirstNameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(FirstNameParameter);

        SqlParameter InitialParameter = new SqlParameter();
        InitialParameter.ParameterName = "@Initial";
        InitialParameter.SqlDbType = SqlDbType.Int;
        if (txtInitialTemp.Text != "")
        {
            InitialParameter.Value = txtInitialTemp.Text;
        }
        else
        {
            InitialParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(InitialParameter);

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        client_idParameter.Value = ddlMemberTemp.SelectedValue;
        command.Parameters.Add(client_idParameter);

        SqlParameter author_idParameter = new SqlParameter();
        author_idParameter.ParameterName = "@author_id";
        author_idParameter.SqlDbType = SqlDbType.Int;
        author_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(author_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvAuthor.EditIndex = -1;

        string pubIdStr = hdnPubId.Value;
        int pubId = Convert.ToInt32(pubIdStr);
        FillAuthorGrid(pubId);

        if (e.RowIndex > 0)
        {
            return; // RowIndex=0 is the row we want to insert
        }
        System.Collections.Hashtable h =
                    new System.Collections.Hashtable();

        foreach (System.Collections.DictionaryEntry x in e.NewValues)
        {
            h[x.Key] = x.Value;
        }

    }
    protected void gvAuthor_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
    {
        // Append the original field values to the log text.
        /*
        foreach (DictionaryEntry valueEntry in e.OldValues)
        {
            tempStr += valueEntry.Key + "=" + valueEntry.Value + ";";
        }

        if (e.Exception == null)
        {
            // The update operation succeeded. Clear the message label.
            //ErrorMessage.Text = "";
        }
        else
        {
            // The update operation failed. Display an error message.
            //ErrorMessage.Text = e.AffectedRows.ToString() + " rows updated. " + e.Exception.Message;
            e.ExceptionHandled = true;
        }
        */
    }
    protected void gvAuthor_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAuthor.EditIndex = e.NewEditIndex;
        string pubIdStr = hdnPubId.Value;
        int pubId = Convert.ToInt32(pubIdStr);
        FillAuthorGrid(pubId);
    }
    protected void gvAuthor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvAuthor.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }
        string authorIdStr;
        Label lblAuthorIdTemp = null;
        lblAuthorIdTemp = (Label)gvAuthor.Rows[e.RowIndex].FindControl("lblAuthorId");
        if (lblAuthorIdTemp != null)
        {
            authorIdStr = lblAuthorIdTemp.Text;
        }
        else
        {
            return;
        }
        string sqlStatement =
            " delete from publication_author where publication_author_id = " + idStr;

        //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        int authorUsedCnt = 0;
        sqlStatement =
            "select count(*) as cnt from publication_author" +
            " where author_id = " +
            authorIdStr;

        SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        object authorCntObj = commandCnt.ExecuteScalar();
        if (authorCntObj != DBNull.Value)
        {
            authorUsedCnt = Convert.ToInt32(authorCntObj);
        }
        myConnection.Close();

        if (authorUsedCnt == 0)
        {
            sqlStatement =
                "delete from author" +
                " where author_id = " +
                authorIdStr;

            SqlCommand commandDelete = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            commandDelete.ExecuteNonQuery();
            myConnection.Close();
        }
        string pubIdStr = hdnPubId.Value;
        int pubId = Convert.ToInt32(pubIdStr);
        FillAuthorGrid(pubId);

    }
    public void FillAuthorGrid(int publicationId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        string sqlStatement =
            "select pa.publication_author_id, c.last_name + ', ' + c.first_name as client," +
            " a.client_id," +
            " a.author_id," +
            " a.LastName as last_name," +
            " case when charindex(' ', a.ForeName) = 0 then a.ForeName" +
            " else substring(a.ForeName,1,charindex(' ', a.ForeName)-1)" +
            " end as first_name," +
            " case when len(a.Initials) = 2 then substring(a.Initials,2,1)" +
            " else ''" +
            " end as mi" +
            " from author a" +
            " inner join publication_author pa" +
            " on a.author_id = pa.author_id" +
            " and pa.publication_id = " +
            publicationId.ToString() +
            " left outer join client c" +
            " on a.client_id = c.client_id";

        SqlDataSource dsAuthor = new SqlDataSource(connectionStr, sqlStatement);
        //Cache["FISHALKDATASOURCE"] = dsAuthor;
        gvAuthor.DataSource = dsAuthor;
        gvAuthor.DataBind();

        DataView dv = (DataView)dsAuthor.Select(DataSourceSelectArguments.Empty);
        if (dv.Count == 0)
        {
            //btnAddImage.Visible = true;
            //txtNewDescription.Visible = true;
            gvAuthor.EditIndex = dv.Count - 1;
        }
        else
        {
            //btnAddImage.Visible = false;
            //txtNewDescription.Visible = false;
        }
    }
    protected void gvAuthor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int authorId = 0;
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("Insert"))
        {
            //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);

            string sqlStatement = "";

            TextBox txtNewLastNameTemp = (TextBox)gvAuthor.FooterRow.FindControl("txtNewLastName");
            if (txtNewLastNameTemp == null)
            {
                ErrorMessage.Text = "No txtNewLastName";
                return;
            }
            string LastNameStr = txtNewLastNameTemp.Text;
            TextBox txtNewFirstNameTemp = (TextBox)gvAuthor.FooterRow.FindControl("txtNewFirstName");
            string firstName = txtNewFirstNameTemp.Text;
            if (txtNewFirstNameTemp == null)
            {
                ErrorMessage.Text = "No txtNewFirstNameTemp";
                return;
            }
            TextBox txtNewInitialTemp = (TextBox)gvAuthor.FooterRow.FindControl("txtNewInitial");
            if (txtNewInitialTemp == null)
            {
                ErrorMessage.Text = "No txtNewInitialTemp";
                return;
            }
            string ForeName;
            string Initials;
            string initial = txtNewInitialTemp.Text;
            FromFirstNameToForeName(firstName, initial, out ForeName, out Initials);

            DropDownList ddlNewMemberTemp = null;
            ddlNewMemberTemp = (DropDownList)gvAuthor.FooterRow.FindControl("ddlNewMember");
            int clientId;
            if (ddlNewMemberTemp == null)
            {
                return;
            }
            else
            {
                if (ddlNewMemberTemp.SelectedIndex != 0 && ddlNewMemberTemp.SelectedIndex != -1)
                {
                    clientId = Convert.ToInt32(ddlNewMemberTemp.SelectedValue);
                }
                else
                {
                    clientId = 0; ;
                }
            }


            authorId = InsertAuthor(LastNameStr, ForeName, Initials, clientId);

            sqlStatement =
                " insert into publication_author" +
                " (publication_id, author_id) values (" +
                " @publication_id, @author_id)";

            SqlCommand command2 = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            string pubIdStr = hdnPubId.Value;
            if (pubIdStr != "0")
            {
                publication_idParameter.Value = Convert.ToInt32(pubIdStr);
            }
            else //never happen
            {
                publication_idParameter.Value = DBNull.Value;
                return;
            }
            command2.Parameters.Add(publication_idParameter);

            SqlParameter author_idParameter2 = new SqlParameter();
            author_idParameter2.ParameterName = "@author_id";
            author_idParameter2.SqlDbType = SqlDbType.Int;
            if (authorId != 0)
            {
                author_idParameter2.Value = authorId;
            }
            else //never happen
            {
                ErrorMessage.Text = "No authorId";
                return;
            }
            command2.Parameters.Add(author_idParameter2);

            myConnection.Open();
            command2.ExecuteNonQuery();
            myConnection.Close();

            gvAuthor.EditIndex = -1;
            int pubId = Convert.ToInt32(pubIdStr);
            FillAuthorGrid(pubId);
        }

    }
    protected void FromFirstNameToForeName(string fistName, string init, out string ForeName, out string Initials)
    {
        if (init.Length > 0)
        {
            ForeName = fistName + " " + init.Substring(0, 1);
            Initials = fistName.Substring(0, 1) + init.Substring(0, 1);
        }
        else
        {
            ForeName = fistName;
            Initials = fistName.Substring(0, 1);
        }
    }
    protected void FromForeNameToFirstName(string ForeName, string Initials, out string fistName, out string init)
    {
        if (ForeName.IndexOf(' ') == -1)
        {
            fistName = ForeName;
            init = ForeName.Substring(0, 1);
        }
        else
        {
            fistName = ForeName.Substring(0, ForeName.IndexOf(' '));
            string temp = ForeName.Substring(ForeName.IndexOf(' ') + 1);
            init = temp.Substring(0, 1);
        }
    }
    protected int InsertAuthor(string LastNameStr, string ForeNameStr, string InitialsStr, int clientId)
    {
        //string connectionStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);

        string sqlStatement = "";
        int authorId = 0;
        sqlStatement =
            "select author_id from author" +
            " where LastName = '" +
            LastNameStr +
            "' and ForeName = '" +
            ForeNameStr +
            "' and Initials = '" +
            InitialsStr +
            "'";

        SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        object authorIdObj = commandCnt.ExecuteScalar();
        if (authorIdObj != null)
        {
            authorId = Convert.ToInt32(authorIdObj);
        }
        myConnection.Close();
        if (authorId != 0)
        {
            return authorId;
        }

        sqlStatement =
            " insert into author" +
            " (LastName, ForeName,Initials,client_id) values (" +
            " @LastName, @ForeName, @Initials, @client_id)" +
            " SET @author_id = SCOPE_IDENTITY()";

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter LastNameParameter = new SqlParameter();
        LastNameParameter.ParameterName = "@LastName";
        LastNameParameter.SqlDbType = SqlDbType.VarChar;
        if (LastNameStr != "")
        {
            LastNameParameter.Value = LastNameStr;
        }
        else
        {
            LastNameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(LastNameParameter);

        SqlParameter ForeNameParameter = new SqlParameter();
        ForeNameParameter.ParameterName = "@ForeName";
        ForeNameParameter.SqlDbType = SqlDbType.VarChar;
        if (ForeNameStr != "")
        {
            ForeNameParameter.Value = ForeNameStr;
        }
        else
        {
            ForeNameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(ForeNameParameter);

        SqlParameter InitialsParameter = new SqlParameter();
        InitialsParameter.ParameterName = "@Initials";
        InitialsParameter.SqlDbType = SqlDbType.VarChar;
        if (InitialsStr != "")
        {
            InitialsParameter.Value = InitialsStr;
        }
        else
        {
            InitialsParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(InitialsParameter);

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        if (clientId != 0)
        {
            client_idParameter.Value = clientId;
        }
        else
        {
            client_idParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(client_idParameter);

        SqlParameter author_idParameter = new SqlParameter();
        author_idParameter.ParameterName = "@author_id";
        author_idParameter.SqlDbType = SqlDbType.Int;
        author_idParameter.Direction = ParameterDirection.Output;
        command.Parameters.Add(author_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        authorId = (int)author_idParameter.Value;
        myConnection.Close();

        return authorId;
    }
    protected void ddlNewMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlNewMemberTemp = (DropDownList)sender;
        GridViewRow rowTemp = (GridViewRow)ddlNewMemberTemp.NamingContainer;
        TextBox txtLastNameTemp = (TextBox)rowTemp.FindControl("txtNewLastName");
        TextBox txtFirstNameTemp = (TextBox)rowTemp.FindControl("txtNewFirstName");
        TextBox txtInitialTemp = (TextBox)rowTemp.FindControl("txtNewInitial");

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement =
            "select last_name, first_name, MI from client where client_id = " + ddlNewMemberTemp.SelectedValue.ToString();
        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                txtLastNameTemp.Text = myReader["last_name"].ToString();
                txtFirstNameTemp.Text = myReader["first_name"].ToString();
                txtInitialTemp.Text = myReader["MI"].ToString();
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idStr;
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        if (e.CommandName.Equals("Select"))
        {
            SqlDataSource dsPublication = new SqlDataSource();
            gvAuthor.DataSource = dsPublication;
            gvAuthor.DataBind();

            Label lblIdTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblIdTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                idStr = lblIdTemp.Text;
                hdnPubId.Value = idStr;
            }
            else
            {
                return;
            }
            FillTextBoxes(idStr);
            /*
            sqlStatement = "select" +
                " journal_title," +
                " volume," +
                " issue," +
                " MedlinePgn," +
                " pub_year," +
                " pub_season," +
                " pub_month," +
                " pub_day," +
                " article_title," +
                " ISOAbbreviation," +
                " abstract_text," +
                " pmid," +
                " pmcid," +
                " CollectiveName" +
                " from publication" +
                " where publication_id = " +
                idStr;

            SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
            conn.Open();
            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    txtJournalTitle.Text = myReader["journal_title"].ToString();
                    txtVolume.Text = myReader["volume"].ToString();
                    txtIssue.Text = myReader["issue"].ToString();
                    txtMedlinePgn.Text = myReader["MedlinePgn"].ToString();
                    txtYear.Text = myReader["pub_year"].ToString();
                    txtDay.Text = myReader["pub_day"].ToString();
                    txtArticleTitle.Text = myReader["article_title"].ToString();
                    txtISOAbbreviation.Text = myReader["ISOAbbreviation"].ToString();
                    txtAbstract.Text = myReader["abstract_text"].ToString();
                    txtPmid.Text = myReader["pmid"].ToString();
                    txtPmcid.Text = myReader["pmcid"].ToString();
                    string seasonStr = myReader["pub_season"].ToString();
                    if (ddlSeason.Items.FindByValue(seasonStr) != null)
                    {
                        ddlSeason.SelectedIndex = ddlSeason.Items.IndexOf(ddlSeason.Items.FindByValue(seasonStr));
                    }
                    string monthStr = myReader["pub_month"].ToString();
                    if (ddlMonth.Items.FindByValue(monthStr) != null)
                    {
                        ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthStr));
                    }
                    txtCollectiveName.Text = myReader["CollectiveName"].ToString();
                }
            }
            finally
            {
                myReader.Close();
                conn.Close();
            }
            */
        }
        else if (e.CommandName.Equals("Link"))
        {
            Label lblIdTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblIdTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                idStr = lblIdTemp.Text;
            }
            else
            {
                return;
            }
            sqlStatement = "select" +
                " journal_title," +
                " issn," +
                " issn_type," +
                " volume," +
                " issue," +
                " MedlinePgn," +
                " pub_year," +
                " pub_season," +
                " pub_month," +
                " pub_day," +
                " MedlineDate," +
                " article_title," +
                " ISOAbbreviation," +
                " abstract_text," +
                " pmid," +
                " pmcid" +
                " from publication" +
                " where publication_id = " +
                idStr;

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int pmid = (int)commandCnt.ExecuteScalar();
            conn.Close();

        }
        //gvPublication.Visible = false;
    }
    protected void FillTextBoxes(string idStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        sqlStatement = "select" +
            " journal_title," +
            " volume," +
            " issue," +
            " MedlinePgn," +
            " pub_year," +
            " pub_season," +
            " pub_month," +
            " pub_day," +
            " article_title," +
            " ISOAbbreviation," +
            " abstract_text," +
            " pmid," +
            " pmcid," +
            " CollectiveName" +
            " from publication" +
            " where publication_id = " +
            idStr;

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                txtJournalTitle.Text = myReader["journal_title"].ToString();
                txtVolume.Text = myReader["volume"].ToString();
                txtIssue.Text = myReader["issue"].ToString();
                txtMedlinePgn.Text = myReader["MedlinePgn"].ToString();
                txtYear.Text = myReader["pub_year"].ToString();
                txtDay.Text = myReader["pub_day"].ToString();
                txtArticleTitle.Text = myReader["article_title"].ToString();
                txtISOAbbreviation.Text = myReader["ISOAbbreviation"].ToString();
                txtAbstract.Text = myReader["abstract_text"].ToString();
                txtPmid.Text = myReader["pmid"].ToString();
                txtPmcid.Text = myReader["pmcid"].ToString();
                string seasonStr = myReader["pub_season"].ToString();
                if (ddlSeason.Items.FindByValue(seasonStr) != null)
                {
                    ddlSeason.SelectedIndex = ddlSeason.Items.IndexOf(ddlSeason.Items.FindByValue(seasonStr));
                }
                string monthStr = myReader["pub_month"].ToString();
                if (ddlMonth.Items.FindByValue(monthStr) != null)
                {
                    ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthStr));
                }
                txtCollectiveName.Text = myReader["CollectiveName"].ToString();
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Clear();
        string sqlStatement =
            "select p.publication_id," +
            " p.pmid," +
            " p.article_title" +
            " from publication p" +
            " where p.article_title like '%" +
            txtSearchTitle.Text +
            "%'";

        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void btnAuthor_Click(object sender, EventArgs e)
    {
        int publicationId = Convert.ToInt32(hdnPubId.Value);
        FillAuthorGrid(publicationId);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
        txtSearchTitle.Text = "";
    }
    protected void Clear()
    {
        txtJournalTitle.Text = "";
        txtISOAbbreviation.Text = "";
        txtVolume.Text = "";
        txtIssue.Text = "";
        txtMedlinePgn.Text = "";
        txtYear.Text = "";
        ddlSeason.SelectedIndex = -1;
        ddlMonth.SelectedIndex = -1;
        txtDay.Text = "";
        txtArticleTitle.Text = "";
        txtAbstract.Text = "";
        txtPmid.Text = "";
        txtPmcid.Text = "";

        SqlDataSource dsPublication = new SqlDataSource();
        gvPublication.DataSource = dsPublication;
        gvPublication.DataBind();
        gvAuthor.DataSource = dsPublication;
        gvAuthor.DataBind();

        hdnPubId.Value = "0";
    }
    protected void btnEPub_Click(object sender, EventArgs e)
    {
        Clear();

        string clientIdStr = "";
        string sqlStatement;
        if (ddlMember.SelectedIndex != 0 && ddlMember.SelectedIndex != -1)
        {
            clientIdStr = ddlMember.SelectedValue.ToString();
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " p.article_title" +
                " from publication p" +
                " inner join publication_author pa" +
                " on p.publication_id = pa.publication_id" +
                " inner join author a" +
                " on pa.author_id = a.author_id" +
                " and a.client_id = " +
                clientIdStr +
                " where p.volume is null" +
                " order by p.article_title";
        }
        else
        {
            sqlStatement =
                "select publication_id," +
                " pmid," +
                " article_title" +
                " from publication" +
                " where MedlinePgn is null" +
                " order by article_title";
        }

        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void btnPmidSearch_Click(object sender, EventArgs e)
    {
        Clear();
        string sqlStatement =
            "select p.publication_id," +
            " p.pmid," +
            " p.article_title" +
            " from publication p" +
            " where p.pmid in (" +
            txtPmidSearch.Text +
            ")";

        Helper.BindGridview(sqlStatement, gvPublication);

    }
    protected void btnProcessPub_Click(object sender, EventArgs e)
    {
        string publicationIdStr = hdnPubId.Value;
        int publicationId = Convert.ToInt32(publicationIdStr);
        ProcessPub.UpdateAllProcessInfoForOnePubId(publicationId);

    }
    protected void btnSearchForLastName_Click(object sender, EventArgs e)
    {
        Clear();
        string sqlStatement =
            "select p.publication_id," +
            " p.pmid," +
            " p.article_title" +
            " from publication p" +
            " inner join publication_processing pp" +
            " on p.publication_id = pp.publication_id" +
            " where pp.authorlist like '%" +
            txtLastName.Text +
            "%'";

        Helper.BindGridview(sqlStatement, gvPublication);
    }
}