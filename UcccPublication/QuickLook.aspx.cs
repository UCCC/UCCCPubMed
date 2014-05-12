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

public partial class QuickLook : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnPubId.Value = "0";
    }
    protected void Clear()
    {
        SqlDataSource dsPublication = new SqlDataSource();
        gvPublication.DataSource = dsPublication;
        gvPublication.DataBind();
        dvPublication.DataSource = dsPublication;
        dvPublication.DataBind();
        gvProgram.DataSource = dsPublication;
        gvProgram.DataBind();
        gvResource.DataSource = dsPublication;
        gvResource.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Clear();
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement = "";
        string sqlStatement2 = "";
        string sqlStatement3 = "";
        if (txtPmid.Text != "" || txtPmcid.Text != "")
        {
            if (txtPmid.Text != "")
            {
                sqlStatement =
                    "select p.*,pp.authorlist, lyn.description as final_confirm, pp.review_editorial" +
                    " from publication p" +
                    " inner join publication_processing pp on p.publication_id = pp.publication_id" +
                    " left outer join l_yes_no lyn" +
                    " on pp.final_confirm_id = lyn.l_yes_no_id" +
                    " where p.pmid = " + txtPmid.Text;

                sqlStatement2 =
                    "select pp.publication_program_id," +
                    " lp.abbreviation as program," +
                    " case when lfg.group_number >= 0 then" +
                    " 'FG ' + convert(varchar,lfg.group_number) + '-' + lfg.description" +
                    " else lfg.description end" +
                    " as focus_group" +
                    " from publication_program pp" +
                    " inner join publication p" +
                    " on pp.publication_id = p.publication_id" +
                    " inner join l_program lp" +
                    " on pp.l_program_id = lp.l_program_id" +
                    " left outer join l_focus_group lfg" +
                    " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                    " where p.pmid = " +
                    txtPmid.Text;

                sqlStatement3 =
                    "select" +
                    " lr.description as resource" +
                    " from publication_resource pr" +
                    " inner join publication p" +
                    " on pr.publication_id = p.publication_id" +
                    " inner join l_resource lr" +
                    " on pr.l_resource_id = lr.l_resource_id" +
                    " where p.pmid = " +
                    txtPmid.Text;
            }
            else if (txtPmcid.Text != "")
            {
                sqlStatement =
                    "select p.*,pp.authorlist, lyn.description as final_confirm, pp.review_editorial" +
                    " from publication p" +
                    " inner join publication_processing pp on p.publication_id = pp.publication_id" +
                    " left outer join l_yes_no lyn" +
                    " on pp.final_confirm_id = lyn.l_yes_no_id" +
                    " where p.pmcid = '" + txtPmcid.Text + "'";

                sqlStatement2 =
                    "select pp.publication_program_id," +
                    " lp.abbreviation as program," +
                    " case when lfg.group_number >= 0 then" +
                    " 'FG ' + convert(varchar,lfg.group_number) + '-' + lfg.description" +
                    " else lfg.description end" +
                    " as focus_group" +
                    " from publication_program pp" +
                    " inner join publication p" +
                    " on pp.publication_id = p.publication_id" +
                    " inner join l_program lp" +
                    " on pp.l_program_id = lp.l_program_id" +
                    " left outer join l_focus_group lfg" +
                    " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                    " where p.pmcid = '" +
                    txtPmcid.Text + "'";

                sqlStatement3 =
                    "select" +
                    " lr.description as resource" +
                    " from publication_resource pr" +
                    " inner join publication p" +
                    " on pr.publication_id = p.publication_id" +
                    " inner join l_resource lr" +
                    " on pr.l_resource_id = lr.l_resource_id" +
                    " where p.pmcid = '" +
                    txtPmcid.Text + "'";
            }
            SqlDataSource ds = new SqlDataSource(connectionStr, sqlStatement);
            dvPublication.DataSource = ds;

            DataSourceSelectArguments dssa = new DataSourceSelectArguments();
            dssa.AddSupportedCapabilities(DataSourceCapabilities.RetrieveTotalRowCount);
            dssa.RetrieveTotalRowCount = true;
            DataView dv = (DataView)ds.Select(dssa);
            if (dv.Table.Rows.Count > 0)
            {
                dvPublication.DataBind();
                btnEdit.Visible = true;
            }
            else
            {
                ErrorMessage.Text = "No pub in database. You can import it.";
                btnImport.Visible = true;
            }

            SqlDataSource ds2 = new SqlDataSource(connectionStr, sqlStatement2);
            gvProgram.DataSource = ds2;
            gvProgram.DataBind();

            SqlDataSource ds3 = new SqlDataSource(connectionStr, sqlStatement3);
            gvResource.DataSource = ds3;
            gvResource.DataBind();
        }
        else if (txtTitle.Text != "")
        {
            sqlStatement =
                "select p.publication_id," +
                " p.pmid," +
                " p.article_title" +
                " from publication p" +
                " where p.article_title like '%" +
                txtTitle.Text +
                "%'";

            Helper.BindGridview(sqlStatement, gvPublication);

        }
        else if (txtLastName.Text != "")
        {
            sqlStatement =
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

            sqlStatement =
                "select p.*,pp.authorlist, lyn.description as final_confirm, pp.review_editorial" +
                " from publication p" +
                " inner join publication_processing pp on p.publication_id = pp.publication_id" +
                " left outer join l_yes_no lyn" +
                " on pp.final_confirm_id = lyn.l_yes_no_id" +
                " where p.publication_id = " + 
                idStr;

            SqlDataSource ds = new SqlDataSource(connectionStr, sqlStatement);
            dvPublication.DataSource = ds;
            dvPublication.DataBind();

            sqlStatement =
                "select pp.publication_program_id," +
                " lp.abbreviation as program," +
                " convert(varchar,lfg.group_number) + '-' + lfg.description as focus_group" +
                " from publication_program pp" +
                " inner join publication p" +
                " on pp.publication_id = p.publication_id" +
                " inner join l_program lp" +
                " on pp.l_program_id = lp.l_program_id" +
                " left outer join l_focus_group lfg" +
                " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                " where p.publication_id = " +
                idStr;

            SqlDataSource ds2 = new SqlDataSource(connectionStr, sqlStatement);
            gvProgram.DataSource = ds2;
            gvProgram.DataBind();


        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "select publication_id from publication" +
            " where pmid = " +
            txtPmid.Text; ;

        SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        object pubIdObjObj = commandCnt.ExecuteScalar();
        string pubId = "0";
        if (pubIdObjObj != DBNull.Value)
        {
            pubId = pubIdObjObj.ToString();
        }
        myConnection.Close();

        Response.Redirect("EditPub.aspx?pubId=" + pubId);
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string pmid = txtPmid.Text;
        Response.Redirect("RetrievePub.aspx?pmid=" + pmid);

    }
}