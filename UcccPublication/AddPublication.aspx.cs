using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class AddPublication : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMonth(ddlMonth, "xxx");
            LoadSeason(ddlSeason, "xxx");
            hdnPubId.Value = "0";
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
        string msg;
        if (txtArticleTitle.Text == "" || txtYear.Text == "")
        {
            string msg2 = "Please give article title and pub year.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg2 + "');", true);
            return;
        }
        int publicationId = InsertPublication();
        if (publicationId != 0)
        {
            hdnPubId.Value = publicationId.ToString();
            //FillAuthorGrid(publicationId);
            //ErrorMessage.Text = "New publication is saved.";
            msg = "New publication is saved.";
            //ErrorMessage.Text = msg;
            btnAuthor.Visible = true;
            btnSave.Enabled = false;
        }
        else
        {
            msg = "No publication added.";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
    }
    protected int InsertPublication()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;
        int publicationId = 0;
        sqlStatement =
            " insert into publication" +
            " (journal_title,volume,pub_year,pub_season," +
            " pub_month,pub_day,article_title,ISOAbbreviation,abstract_text,MedlinePgn,pmcid,pmid) values " +
            " (@journal_title,@volume,@pub_year,@pub_season," +
            " @pub_month,@pub_day,@article_title,@ISOAbbreviation,@abstract_text,@MedlinePgn,@pmcid,@pmid)" +
            " SET @publication_id = SCOPE_IDENTITY()";

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
        if (txtVolumn.Text != "")
        {
            volumeParameter.Value = txtVolumn.Text;
        }
        else
        {
            volumeParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(volumeParameter);

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
        if (ddlSeason.SelectedIndex != 0 &&  ddlSeason.SelectedIndex != -1)
        {
            pub_seasonParameter.Value = ddlSeason.SelectedItem.ToString();
        }
        else
        {
            pub_seasonParameter.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(pub_seasonParameter);

        SqlParameter pub_monthParameter = new SqlParameter();
        pub_monthParameter.ParameterName = "@pub_month";
        pub_monthParameter.SqlDbType = SqlDbType.VarChar;
        if (ddlMonth.SelectedIndex != 0 && ddlMonth.SelectedIndex != -1)
        {
            pub_monthParameter.Value = ddlMonth.SelectedItem.ToString(); ;
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

        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(publication_idParameter);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        publicationId = (int)publication_idParameter.Value;
        myConnection.Close();

        return publicationId;
    }
    protected void EmptyGridFix(GridView grdView)
    {
        // normally executes after a grid load method
        if (grdView.Rows.Count == 0 &&
            grdView.DataSource != null)
        {
            DataTable dt = null;

            if (grdView.DataSource is DataSet)
            {
                dt = ((DataSet)grdView.DataSource).Tables[0].Clone();
            }
            else if (grdView.DataSource is DataTable)
            {
                dt = ((DataTable)grdView.DataSource).Clone();
            }

            else if (grdView.DataSource is SqlDataSource)
            {
                SqlDataSource sds = new SqlDataSource();
                DataView dv = new DataView();
                sds = (SqlDataSource)grdView.DataSource;
                dv = (DataView)sds.Select(DataSourceSelectArguments.Empty);
                dt = dv.ToTable().Clone();
                /*
                DataView dv = new DataView(); 
                DataTable dt = new DataTable(); 
                dv = mySQLDataSource.Select(DataSourceSelectArguments.Empty); 
                dt = dv.ToTable();
                 * */
            }
            if (dt == null)
            {
                return;
            }

            dt.Rows.Add(dt.NewRow()); // add empty row
            grdView.DataSource = dt;
            grdView.DataBind();

            // hide row
            grdView.Rows[0].Visible = false;
            grdView.Rows[0].Controls.Clear();
        }

        // normally executes at all postbacks
        if (grdView.Rows.Count == 1 &&
            grdView.DataSource == null)
        {
            bool bIsGridEmpty = true;

            // check first row that all cells empty
            for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
            {
                if (grdView.Rows[0].Cells[i].Text != string.Empty)
                {
                    bIsGridEmpty = false;
                }
            }
            // hide row
            if (bIsGridEmpty)
            {
                grdView.Rows[0].Visible = false;
                grdView.Rows[0].Controls.Clear();
            }
        }
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
                LoadLookup.LoadMember(ddlMemberTemp, currClientStr,"01/01/2000","01/01/2015");
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
            TextBox txtForeNameTemp = null;
            txtForeNameTemp = (TextBox)e.Row.FindControl("txtForeName");
            if (txtForeNameTemp == null)
            {
                return;
            }
            else
            {
                txtForeNameTemp.Text = drv["ForeName"].ToString();
            }
            TextBox txtInitialsTemp = null;
            txtInitialsTemp = (TextBox)e.Row.FindControl("txtInitials");
            if (txtInitialsTemp == null)
            {
                return;
            }
            else
            {
                txtInitialsTemp.Text = drv["Initials"].ToString();
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
            TextBox txtForeNameTemp = null;
            txtForeNameTemp = (TextBox)e.Row.FindControl("txtNewForeName");
            if (txtForeNameTemp == null)
            {
                return;
            }
            TextBox txtInitialsTemp = null;
            txtInitialsTemp = (TextBox)e.Row.FindControl("txtNewInitials");
            if (txtInitialsTemp == null)
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
                LoadLookup.LoadMember(ddlMemberTemp, "xxx","01/01/2000","01/01/2015");
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
        TextBox txtForeNameTemp = null;
        txtForeNameTemp = (TextBox)gvAuthor.Rows[e.RowIndex].FindControl("txtForeName");
        if (txtForeNameTemp == null)
        {
            return;
        }
        TextBox txtInitialsTemp = null;
        txtInitialsTemp = (TextBox)gvAuthor.Rows[e.RowIndex].FindControl("txtInitials");
        if (txtInitialsTemp == null)
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
            "Update author" +
            " SET LastName=@LastName," +
            " ForeName=@ForeName," +
            " Initials=@Initials," +
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

        SqlParameter ForeNameParameter = new SqlParameter();
        ForeNameParameter.ParameterName = "@ForeName";
        ForeNameParameter.SqlDbType = SqlDbType.Int;
        if (txtForeNameTemp.Text != "")
        {
            if (txtInitialsTemp.Text != "")
            {
                string init = txtInitialsTemp.Text;
                ForeNameParameter.Value = txtForeNameTemp.Text + " " + init[0];
            }
            else
            {
                ForeNameParameter.Value = txtForeNameTemp.Text;
            }
        }
        else
        {
            ForeNameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(ForeNameParameter);

        SqlParameter InitialsParameter = new SqlParameter();
        InitialsParameter.ParameterName = "@Initials";
        InitialsParameter.SqlDbType = SqlDbType.Int;
        if (txtInitialsTemp.Text != "")
        {
            InitialsParameter.Value = txtInitialsTemp.Text;
        }
        else
        {
            InitialsParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(InitialsParameter);

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
        /*
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
         * */
        string sqlStatement =
            "select pa.publication_author_id, c.last_name + ', ' + c.first_name as client," +
            " a.client_id," +
            " a.author_id," +
            " a.LastName as LastName," +
            " a.ForeName as ForeName," +
            " a.Initials as Initials" +
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
        EmptyGridFix(gvAuthor);
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

            TextBox txtNewForeNameTemp = (TextBox)gvAuthor.FooterRow.FindControl("txtNewForeName");
            if (txtNewForeNameTemp == null)
            {
                ErrorMessage.Text = "No txtNewForeNameTemp";
                return;
            }
            string ForeNameStr = txtNewForeNameTemp.Text;

            TextBox txtNewInitialsTemp = (TextBox)gvAuthor.FooterRow.FindControl("txtNewInitials");
            if (txtNewInitialsTemp == null)
            {
                ErrorMessage.Text = "No txtNewInitialsTemp";
                return;
            }
            string InitialsStr = txtNewInitialsTemp.Text;

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


            authorId = InsertAuthor(LastNameStr, ForeNameStr, InitialsStr, clientId);

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
            fistName = ForeName.Substring(0,ForeName.IndexOf(' '));
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
        TextBox txtInitialsTemp = (TextBox)rowTemp.FindControl("txtNewInitials");

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        if (ddlNewMemberTemp.SelectedIndex != 0 && ddlNewMemberTemp.SelectedIndex != -1)
        {
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
                    txtInitialsTemp.Text = myReader["MI"].ToString();
                }
            }
            finally
            {
                myReader.Close();
                conn.Close();
            }
        }
    }
    protected void btnAuthor_Click(object sender, EventArgs e)
    {
        int publicationId = Convert.ToInt32(hdnPubId.Value);
        FillAuthorGrid(publicationId);
        btnProcess.Visible = true;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
        //txtSearchTitle.Text = "";
    }
    protected void Clear()
    {
        txtJournalTitle.Text = "";
        txtISOAbbreviation.Text = "";
        txtVolumn.Text = "";
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
        gvAuthor.DataSource = dsPublication;
        gvAuthor.DataBind();

        hdnPubId.Value = "0";
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        string pubIdStr = hdnPubId.Value;
        if (pubIdStr == "0")
        {
            string msg3 = "No publication saved.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg3 + "');", true);
            return;
        }

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "select count(*) as cnt from publication_author where publication_id = " + pubIdStr;
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        int cnt = (int)command.ExecuteScalar();
        myConnection.Close();

        if (cnt == 0)
        {
            string msg2 = "Publication ID " + pubIdStr + " has no any author. Please add authors.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg2 + "');", true);
            return;
        }
        
        int publicationId = Convert.ToInt32(pubIdStr);
        ProcessAllInfoForOnePub(publicationId);
    }
    protected void ProcessAllInfoForOnePub(int publicationId)
    {
        ProcessPub.ProcessAllInfoForOnePubId(publicationId, false);
        int confirmId = ProcessPub.ConfirmOnePubByAuthorInitial(publicationId);
        ProcessPub.UpdateNameConfirm(publicationId, confirmId);
    }
}