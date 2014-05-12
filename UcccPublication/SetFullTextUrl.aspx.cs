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

public partial class SetFullTextUrl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadMember(ddlMember, "xxx");
            //LoadMemberList.LoadMember(ddlMember, "xxx");
            //LoadLookup.LoadMember(ddlMember, "xxx");
            LoadLookup.LoadProgram(ddlProgram, "xxx", true);
            ddlProgram.SelectedValue = "0";
            LoadLookup.LoadMemberOnProgram(0, ddlMember);

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }

            string userIdStr = Session["userId"].ToString();
            string roleIdStr = Session["roleId"].ToString();

            Menu myMenu = null;
            myMenu = (Menu)Master.FindControl("Menu1");
            if (myMenu != null)
            {
                if (roleIdStr != "1" && roleIdStr == "2")
                {
                    MenuItem mi = (MenuItem)myMenu.FindItem("Edit Pubs");
                    //mi.v
                }
            }
        }

    }
    protected void btnGetPublications_Click(object sender, EventArgs e)
    {
        FillPublicationGrid();

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
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
        }
        else
        {
            return;
        }

        TextBox txtFullTextUrlTemp = null;
        txtFullTextUrlTemp = (TextBox)gvPublication.Rows[e.RowIndex].FindControl("txtFullTextUrl");
        if (txtFullTextUrlTemp == null)
        {
            return;
        }

        string sqlStatement =
            "Update publication_processing" +
            " SET full_text_url=@full_text_url" +
            " WHERE publication_id = @publication_id";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter full_text_urlParameter = new SqlParameter();
        full_text_urlParameter.ParameterName = "@full_text_url";
        full_text_urlParameter.SqlDbType = SqlDbType.VarChar;
        string url = txtFullTextUrlTemp.Text;
        if (!url.Contains("http://"))
        {
            url = "http://" + url;
        }
        if (url == "")
        {
            full_text_urlParameter.Value = DBNull.Value;
        }
        else
        {
            full_text_urlParameter.Value = url;
        }
        command.Parameters.Add(full_text_urlParameter);

        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(publication_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        gvPublication.EditIndex = -1;

        FillPublicationGrid();
    }
    protected void gvPublication_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPublication.EditIndex = e.NewEditIndex;
        FillPublicationGrid();
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
            startDate = "01/01/1990";
        }
        if (txtEndDate.Text != "")
        {
            endDate = txtEndDate.Text;
        }
        else
        {
            DateTime now = DateTime.Now;
            endDate = now.ToString();
        }
        string clientIdStr = "";
        ErrorMessage.Text = "";
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
            "select " +
            " pd.PUBLICATION_id," +
            " p.pmid," +
            " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
            " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication," +
            " pd.full_text_url" +
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
        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void SelectProgram(object sender, EventArgs e)
    {
        int programId = Convert.ToInt32(ddlProgram.SelectedValue);
        LoadLookup.LoadMemberOnProgram(programId, ddlMember);
    }
}