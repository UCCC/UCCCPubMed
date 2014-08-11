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

public partial class SetAuthorship : System.Web.UI.Page
{
    public struct pubid_ours
    {
        public int pubId;
        public int yesNoId;
    }
    int total = 0;
    int yesCnt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadLookup.LoadMember(ddlMember, "xxx");
            LoadLookup.LoadProgram(ddlProgram, "xxx",true);
            ddlProgram.SelectedValue = "0";
            LoadLookup.LoadMemberOnProgram(0, ddlMember);

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
        ListPublications();
    }
    protected void ListPublications()
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
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
        }
        else
        {
            ErrorMessage.Text = "Please select program.";
            return;
        }

        FillPublicationGrid();
        //btnReportSelected.Visible = true;

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rv = (DataRowView)e.Row.DataItem;
            RadioButtonList rblInit = (RadioButtonList)e.Row.FindControl("rblByName");
            if (rblInit != null)
            {
                //rbl.SelectedValue = ((YOURDATAITEM)(e.Row.DataItem).l_yes_no_id.ToString();
                //DataRowView rv = (DataRowView)e.Row.DataItem;
                string selectedValue = rv["name_confirm_id"].ToString();

                rblInit.SelectedValue = selectedValue;
            }
            RadioButtonList rblAddr = (RadioButtonList)e.Row.FindControl("rblByAddress");
            if (rblAddr != null)
            {
                //rbl.SelectedValue = ((YOURDATAITEM)(e.Row.DataItem).l_yes_no_id.ToString();
                //DataRowView rv = (DataRowView)e.Row.DataItem;
                string selectedValue = rv["address_confirm_id"].ToString();

                rblAddr.SelectedValue = selectedValue;
            }
            RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblFinal");
            if (rbl != null)
            {
                //rbl.SelectedValue = ((YOURDATAITEM)(e.Row.DataItem).l_yes_no_id.ToString();
                //DataRowView rv = (DataRowView)e.Row.DataItem;
                string selectedValue = rv["final_confirm_id"].ToString();
                if (selectedValue == "1")
                {
                    yesCnt++;
                    if (chxNotYes.Checked)
                    {
                        e.Row.Visible = false;
                    }
                }
                total++;
                rbl.SelectedValue = selectedValue;
            }
        }
    }
    protected void gvPublication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPublication.EditIndex = -1;
        FillPublicationGrid();
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
            hdnPublicationId.Value = idStr;
        }
        else
        {
            return;
        }

        gvPublication.EditIndex = -1;

        FillPublicationGrid();
    }
    protected void gvPublication_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPublication.EditIndex = e.NewEditIndex;
        FillPublicationGrid();
    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idStr;
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        if (e.CommandName.Equals("Email"))
        {
            /*
            sqlStatement = "select ";

            SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int total = (int)commandCnt.ExecuteScalar();
            conn.Close();
            */
            string toEmailAddress = "";
            string hlink = "";
            ClientScript.RegisterStartupScript(this.GetType(), "mailto",
               "<script type = 'text/javascript'>parent.location='mailto:" +
               toEmailAddress +
               "?subject=Shared Resources&body=Check shared resource by clicking " + hlink + "'</script>");

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
        gvPublication.Visible = false;
        onePubDiv.Visible = true;
    }
    public void FillPublicationGrid()
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
            ErrorMessage.Text = "please enter start date.";
            return;
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            ErrorMessage.Text = "please enter end date.";
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
            "select p.publication_id," +
            " p.pmid," +
            " p.pmcid," +
            " pd.publication_processing_id," +
            " pd.authorlist," +
            " isnull(p.article_title,'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication," +
            " pd.name_confirm_id," +
            " lyn.description as initialMatch," +
            " pd.address_confirm_id," +
            " case when pd.address_confirm_id = 1 then 'Yes' when pd.address_confirm_id = 2 then 'No' else 'Not checked' end as addressMatch," +
            " pd.final_confirm_id" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " inner join publication_author pa" +
            " on pd.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientIdStr +
            " left outer join l_yes_no lyn" +
            " on pd.name_confirm_id = lyn.l_yes_no_id" +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            //" order by p.pub_year desc, pd.authorlist";
            " order by p.pub_year desc," +
            " replace(pd.authorlist,'<b>','')";


        Helper.BindGridview(sqlStatement, gvPublication);
        lblCntStr.Text = "Total Pubs: " + yesCnt.ToString() + " / " + total.ToString();

        btnSaveAuthorship.Visible = true;
        btnReport.Visible = true;
        btnAllYes.Visible = true;
        btnAllNo.Visible = true;
        btnCheckAddress.Visible = true;
    }
    protected void btnSaveAuthorship_Click(object sender, EventArgs e)
    {
        List<pubid_ours> pubid_oursList = new List<pubid_ours>();
        List<int> yesNoIdList = new List<int>();
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            pubid_ours mp = new pubid_ours();
            RadioButtonList ralist = gvPublication.Rows[i].FindControl("rblFinal") as RadioButtonList;
            if (ralist == null)
            {
                return;
            }
            if (ralist.SelectedIndex == -1)
            {
                continue;
            }
            mp.yesNoId = Convert.ToInt32(ralist.SelectedValue.ToString());

            ErrorMessage.Text = "";
            int id;

            Label lblIdTemp = null;
            lblIdTemp = (Label)gvPublication.Rows[i].FindControl("lblId");
            if (lblIdTemp != null)
            {
                id = Convert.ToInt32(lblIdTemp.Text);
            }
            else
            {
                return;
            }
            mp.pubId = id;
            pubid_oursList.Add(mp);
        }
        foreach (pubid_ours mp in pubid_oursList)
        {
            SaveYesNoSelection(mp);
        }
        string msg = "Publication is updated.";
        //ErrorMessage.Text = msg;
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        ErrorMessage.Text = "Final authorship is saved.";
    }
    protected void ReportMemberPubs()
    {
        ErrorMessage.Text = "";
        string startDate = "";
        string endDate = "";
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

        string withAbstract = "0";
        string wInst = "0";
        Response.Redirect("Report.aspx?clientId=" + clientIdStr +
            "&startDate=" + startDate +
            "&endDate=" + endDate +
            "&withAbstract=" + withAbstract +
            "&wInst=" + wInst);
    }
    protected void SaveYesNoSelection(pubid_ours mp)
    {
        int publicationId = mp.pubId;
        int yesNoId = mp.yesNoId;

        string sqlStatement;
        if (yesNoId == 2)
        {
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);
            ProcessPub.SaveRejectOnPubId(publicationId);

            sqlStatement =
                "delete from publication_processing where publication_id = @publication_id" +
                "; delete from publication_author where publication_id = @publication_id" +
                "; delete from publication_program where publication_id = @publication_id" +
                "; delete from publication_programmatic where publication_id = @publication_id" +
                "; delete from publication_pubtype where publication_id = @publication_id" +
                "; delete from publication_resource where publication_id = @publication_id" +
                "; delete from publication where publication_id = @publication_id" +
                "; delete from author where author_id not in (select author_id from publication_author)";

            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            command.Parameters.Add(publication_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();

        }
        else
        {
            sqlStatement =
                "Update publication_processing" +
                " SET final_confirm_id=@final_confirm_id" +
                " WHERE (publication_id = @publication_id)";

            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(connectionStr);
            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter final_confirm_idParameter = new SqlParameter();
            final_confirm_idParameter.ParameterName = "@final_confirm_id";
            final_confirm_idParameter.SqlDbType = SqlDbType.Int;
            final_confirm_idParameter.Value = yesNoId;
            command.Parameters.Add(final_confirm_idParameter);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            command.Parameters.Add(publication_idParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        gvPublication.EditIndex = -1;

        FillPublicationGrid();
    }
    protected void btnAllYes_Click(object sender, EventArgs e)
    {
        SelectAllToYesNo("1");
    }
    protected void btnAllNo_Click(object sender, EventArgs e)
    {
        SelectAllToYesNo("2");
    }
    protected void SelectAllToYesNo(string yesNoIdStr)
    {
        for (int i = 0; i < gvPublication.Rows.Count; i++)
        {
            RadioButtonList ralist = gvPublication.Rows[i].FindControl("rblFinal") as RadioButtonList;
            ralist.SelectedValue = yesNoIdStr;
            //Response.Write("line_" + Convert.ToString(i + 1) + "selectedindex_" + ralist.SelectedIndex.ToString());
        }
    }
    protected void btnCheckAddress_Click(object sender, EventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        string startDate = "";
        string endDate = "";
        ErrorMessage.Text = "";
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
            " p.pmid" +
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
            "'))";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        List<int> pmidList = new List<int>();
        try
        {
            while (myReader.Read())
            {
                int pmid = Convert.ToInt32(myReader["pmid"]);
                pmidList.Add(pmid);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        foreach (int pmid in pmidList)
        {
            UpdateAddressConfirm(pmid.ToString());
        }
        FillPublicationGrid();
    }
    protected bool UpdateAddressConfirm(string pmidStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        int matchedCnt = Helper.GetAddress(pmidStr);
        int confirm_id = 0;
        if (matchedCnt > 0)
        {
            confirm_id = 1;
        }
        else
        {
            confirm_id = 2;
        }
        sqlStatement = "update publication_processing" +
            " set address_confirm_id = " +
            confirm_id.ToString() +
            " where publication_id = " +
            "(select publication_id from publication where pmid = " + pmidStr + ")";
        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
        if (confirm_id == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void SelectProgram(object sender, EventArgs e)
    {
        int programId = Convert.ToInt32(ddlProgram.SelectedValue);
        LoadLookup.LoadMemberOnProgram(programId, ddlMember);
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        string lbl = lblCntStr.Text;
        string num = lbl.Substring(lbl.IndexOf(':') + 2);
        num = num.Substring(0, num.IndexOf('/') -1);
        /*
        if (num == "0")
        {
            ErrorMessage.Text = "No confirmed pubs.";
            return;
        }
         * */
        ReportMemberPubs();

    }
    protected void btnReportSelected_Click(object sender, EventArgs e)
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
        var pubIdArray = string.Join(",", pubIdList); // (.NET 4.0 only)
        //var result = string.Join(";", data.Select(x => x.ToString()).ToArray()); 
        Session["pubIdList"] = pubIdArray.ToString();
        lblSelectedPubIdList.Text = pubIdArray.ToString();
        Response.Redirect("ReportOnePub.aspx");

    }
}