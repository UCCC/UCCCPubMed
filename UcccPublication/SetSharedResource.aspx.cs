using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class SetSharedResource : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        if (!IsPostBack)
        {
            //LoadLookup.LoadMember(ddlMember, "xxx");
            LoadLookup.LoadProgram(ddlProgram, "xxx", true);
            ddlProgram.SelectedValue = "0";
            LoadLookup.LoadMemberOnProgram(0, ddlMember);

            sqlStatement =
                "select l_resource_id, description as resource" +
                " from l_resource order by description";

            SqlDataSource dsResource = new SqlDataSource(connectionStr, sqlStatement);

            chxlResource.DataSource = dsResource;
            chxlResource.DataTextField = "resource";
            chxlResource.DataValueField = "l_resource_id";
            chxlResource.DataBind();

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
        }

    }
    protected void btnGetAuthorlist_Click(object sender, EventArgs e)
    {
        if (txtPmid.Text != "")
        {
            int pmid = Convert.ToInt32(txtPmid.Text);
            FillPublicationGridByPmid(pmid);
        }
        else
        {
            FillPublicationGrid();
        }

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

        }
    }
    protected void gvPublication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPublication.EditIndex = -1;
        FillPublicationGrid();
    }
    protected void gvPublication_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
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
        string idStr = "0"; ;
        string pubIdStr;
        //ErrorMessage.Text = "";
        if (e.CommandName.Equals("resource"))
        {

            chxlResource.ClearSelection();
            Label lblIdTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lblIdTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                pubIdStr = lblIdTemp.Text;
                hdnPublicationId.Value = pubIdStr;
            }
            else
            {
                return;
            }
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionStr);
            string sqlStatement;

            sqlStatement =
                "select p.pmid, p.publication_id," +
                " pd.authorlist," +
                " isnull(p.article_title,'') + ' ' + isnull(p.journal_title,'') +" +
                " ' ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') as publication" +
                " from publication_processing pd" +
                " inner join publication p" +
                " on pd.publication_id = p.publication_id" +
                " where p.publication_id = " +
                pubIdStr;

            SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
            conn.Open();
            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    lblPmid.Text = myReader["pmid"].ToString();
                    lblPublication.Text = myReader["publication"].ToString();
                    lblAuthorlist.Text = myReader["authorlist"].ToString();
                }
            }
            finally
            {
                myReader.Close();
                conn.Close();
            }
            //hdnPublicationId.Value = idStr;

            sqlStatement =
                "select l_resource_id from publication_resource where publication_id = " + pubIdStr;
            SqlCommand commandResource = new SqlCommand(sqlStatement, conn);
            conn.Open();
            SqlDataReader readerResource;
            readerResource = commandResource.ExecuteReader();
            List<int> resourceIdList = new List<int>();
            try
            {
                while (readerResource.Read())
                {
                    ListItem li = new ListItem();
                    string resourceIdStr = readerResource["l_resource_id"].ToString();
                    li = chxlResource.Items.FindByValue(resourceIdStr);
                    li.Selected = true;
                }
            }
            finally
            {
                readerResource.Close();
                conn.Close();
            }
            gvPublication.Visible = false;
            onePubDiv.Visible = true;
        }
    }
    public void FillPublicationGrid()
    {
        string sqlStatement;
        string startDate = "";
        string endDate = "";
        ErrorMessage.Text = "";
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
            "select p.pmid," +
            " pa.PUBLICATION_id," +
            " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
            " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication," +
            " pd.authorlist" +
            " from publication_author pa" +
            " inner join publication p" +
            " on pa.publication_id = p.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id = " +
            clientIdStr +
            " inner join publication_processing pd" +
            " on pd.publication_id = pa.publication_id" +
            " where ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            //" order by p.pub_year desc, pd.authorlist";
            " order by p.pub_year desc," +
            " replace(pd.authorlist,'<b>','')";


        Helper.BindGridview(sqlStatement, gvPublication);
    }
    public void FillPublicationGridByPmid(int pmid)
    {
        string sqlStatement;
        ErrorMessage.Text = "";
        sqlStatement =
            "select p.pmid," +
            " p.PUBLICATION_id," +
            " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
            " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication," +
            " pd.authorlist" +
            " from publication p" +
            " inner join publication_processing pd" +
            " on pd.publication_id = p.publication_id" +
            " where p.pmid = " +
            pmid.ToString();

        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        gvPublication.Visible = true;
        onePubDiv.Visible = false;

    }
    protected void LoopSourceList(int publicationId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement =
            "select l_resource_id" +
            " from publication_resource" +
            " where publication_id = " +
            publicationId.ToString();

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        List<int> resourceIdList = new List<int>();
        try
        {
            while (myReader.Read())
            {
                int theResource = Convert.ToInt32(myReader["l_resource_id"]);
                resourceIdList.Add(theResource);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        for (int i = 1; i <= chxlResource.Items.Count; i++)
        {
            if (resourceIdList.Contains(i))
            {
                ListItem li = chxlResource.Items.FindByValue(i.ToString());
                if (li.Selected)
                {
                    //do nothing
                }
                else
                {
                    RemoveResource(publicationId, i);
                }
            }
            else
            {
                ListItem li = chxlResource.Items.FindByValue(i.ToString());
                if (li.Selected)
                {
                    InsertResource(publicationId, i);
                }
                else
                {
                    //do nothing
                }
            }
        }
        gvPublication.Visible = true;
        onePubDiv.Visible = false;
    }
    protected void InsertResource(int publicationId, int resourceId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement =
            "insert into publication_resource (publication_id, l_resource_id)" +
            " values(" +
            publicationId.ToString() +
            "," +
            resourceId.ToString() +
            ")";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        myCommand.ExecuteNonQuery();
        conn.Close();

    }
    protected void RemoveResource(int publicationId, int resourceId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement =
            "delete from publication_resource where publication_id = " +
            publicationId.ToString() +
            " and l_resource_id = " +
            resourceId.ToString();

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        myCommand.ExecuteNonQuery();
        conn.Close();

    }
    protected void btnSaveResource_Click(object sender, EventArgs e)
    {
        int publicationId = Convert.ToInt32(hdnPublicationId.Value);
        LoopSourceList(publicationId);

    }
    protected void SelectProgram(object sender, EventArgs e)
    {
        int programId = Convert.ToInt32(ddlProgram.SelectedValue);
        LoadLookup.LoadMemberOnProgram(programId, ddlMember);
    }
}