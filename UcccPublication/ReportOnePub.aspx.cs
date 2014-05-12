using System;
using System.Drawing;
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

public partial class ReportOnePub : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        string pubIdList = (string)Session["pubIdList"];
        if (pubIdList != null)
        {
            FillPublicationGridByIdList(pubIdList);
            return;
        }
        */
        Display();
    }
    protected void FillPublicationGridByIdList(string pubIdList)
    {
        string sqlStatement = "";

        sqlStatement =
            "select p.publication_id,p.pmid,lp.symbol as symbol," +
            " process.authorlist +" +
            " '.  ' + isnull(p.article_title,'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.MedlinePgn is null then" +
            " 'Epub ahead of print, ' +" +
            " case when p.entrez_year is not null then convert(varchar,p.entrez_year) + '. ' else '' end" +
            " else" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, '') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end" +
            " end +" +
            " isnull(p.pmcid,'') + '<br /><br />' as article," +
            " p.ISOAbbreviation as journal" +
            " from publication_processing process" +
            " inner join publication p" +
            " on process.publication_id = p.publication_id" +
            " and process.review_editorial is null" +
            " left outer join l_programmatic lp" +
            " on process.l_programmatic_id = lp.l_programmatic_id" +
            " where" +
            " p.publication_id in (" +
            pubIdList +
            ")" +
            " order by " +
            " replace(process.authorlist,'<b>','')";

        //Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected void Display()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        string withAbstract = "0";
        if (Request["withAbstract"] != null)
        {
            withAbstract = Request["withAbstract"].ToString();
        }
        string wInst = "1";
        if (Request["wInst"] != null)
        {
            wInst = Request["wInst"].ToString();
        }
        string pmidList = "";
        if (Request["pmidList"] != null)
        {
            pmidList = Request["pmidList"].ToString();
        }
        pmidList = pmidList.Replace("-", ",");

        sqlStatement = GetSqlStatementForOnePub(pmidList, withAbstract, wInst);

        SqlCommand myCommand2 = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand2.ExecuteReader();
        Table tbl = new Table();
        if (myReader.HasRows)
        {
            GridViewPlaceHolder.Controls.Add(tbl);
        }
        try
        {
            while (myReader.Read())
            {
                string symbolStr = myReader["symbol"].ToString();
                string publicationStr = myReader["publication"].ToString();

                TableRow tr = new TableRow();

                TableCell tcSymbol = new TableCell();
                tcSymbol.Width = 40;
                Label lblSymbol = new Label();
                lblSymbol.Font.Size = new FontUnit(10);
                lblSymbol.Font.Name = "Arial";
                lblSymbol.Text = symbolStr;
                tcSymbol.Controls.Add(lblSymbol);
                tr.Cells.Add(tcSymbol);

                TableCell tcPublication = new TableCell();
                Label lblPublication = new Label();
                lblPublication.Font.Size = new FontUnit(10);
                lblPublication.Font.Name = "Arial";
                lblPublication.Text = publicationStr;
                tcPublication.Controls.Add(lblPublication);
                tr.Cells.Add(tcPublication);

                tbl.Rows.Add(tr);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
    protected string GetSqlStatementForOnePub(string pmidList, string withAbstract, string wInst)
    {
        string sqlStatement = "";
        string authorlistName = " pp.authorlist +";
        if (wInst == "0")
        {
            authorlistName = " pp.authorlist_no_inst +";
        }
        string cap = "0";
        string titleField = "p.article_title";
        if (Request["cap"] != null)
        {
            cap = Request["cap"].ToString();
            if (cap == "1")
            {
                titleField = "pp.formated_article_title";
            }
        }
        if (withAbstract == "1")
        {
            sqlStatement =
                "select lp.symbol as symbol," +
                //" pa.authorlist +" +
                authorlistName +
                " '.  ' + isnull(" + titleField + ",'') +" +
                " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
                " case when p.volume is not null then p.volume + ':' else '' end + " +
                " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') + '<br />' +" +
                " 'Abstract: ' + p.abstract_text + '<br /><br />' as publication" +
                " from publication_processing pp" +
                " inner join publication p" +
                " on pp.publication_id = p.publication_id" +
                " left outer join l_programmatic lp" +
                " on pp.l_programmatic_id = lp.l_programmatic_id" +
                " where p.pmid in (" + pmidList + ")";

        }
        else
        {
            sqlStatement =
                "select lp.symbol as symbol," +
                authorlistName +
                " '.  ' + isnull(" + titleField + ",'') +" +
                " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
                " case when p.volume is not null then p.volume + ':' else '' end + " +
                " isnull(p.MedlinePgn, 'Epub ahead of print') + ', ' + " +
                " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
                " isnull(p.pmcid,'') + '<br /><br />' as publication" +
                " from publication_processing pp" +
                " inner join publication p" +
                " on pp.publication_id = p.publication_id" +
                " left outer join l_programmatic lp" +
                " on pp.l_programmatic_id = lp.l_programmatic_id" +
                " where p.pmid in (" + pmidList + ")";
        }

        return sqlStatement;
    }
}