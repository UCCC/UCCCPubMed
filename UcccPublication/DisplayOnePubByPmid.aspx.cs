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

public partial class DisplayOnePubByPmid : System.Web.UI.Page
{
    public struct Author
    {
        public int pubId;
        public int authorId;
        public string LastName;
        public string ForeName;
        public string Initials;
    }
    public struct Client
    {
        public int clientId;
        public int confirmId;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string pmidStr = Request["pmid"].ToString();
        int pmid = Convert.ToInt32(pmidStr);
        DisplayOnePub(pmid);
        lblPmid.Text = pmidStr;
        int addressFound = Helper.GetAddress(pmidStr);
        if (addressFound > 0)
        {
            lblAddressConfirmed.Text = "Confirmed";
        }
        else
        {
            lblAddressConfirmed.Text = "Not confirmed";
        }
        ConfirmInitial(pmid);
    }
    protected void ConfirmInitial(int pmid)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        sqlStatement =
            "select " +
            " p.publication_id," +
            " a.author_id," +
            " a.LastName," +
            " a.ForeName," +
            " a.Initials" +
            " from publication p" +
            " inner join publication_author pa" +
            " on p.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id is not null" +
            " where p.pmid = " +
            pmid.ToString();

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        List<Author> authorList = new List<Author>();
        try
        {
            while (myReader.Read())
            {
                Author a = new Author();
                a.pubId = Convert.ToInt32(myReader["publication_id"]);
                a.authorId = Convert.ToInt32(myReader["author_id"]);
                a.LastName = myReader["LastName"].ToString();
                a.ForeName = myReader["ForeName"].ToString();
                a.Initials = myReader["Initials"].ToString();
                authorList.Add(a);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        int total = authorList.Count;
        int overwrittenConfirmId = 0;
        int totalConfirmedCnt = 0;
        int totalConflictCnt = 0;
        int totalUnknownCnt = 0;
        int confirmedCntForOnePub = 0;
        int conflictCntForOnePub = 0;
        int unknownCntForOnePub = 0;
        int pubCnt = 0;
        int prevPubId = 0;
        int lastPubId = 0;

        foreach (Author a in authorList)
        {
            Client c = new Client();
            c = Confirm(a);
            lastPubId = a.pubId;
            if (a.pubId != prevPubId)
            {
                if (prevPubId != 0)
                {
                    if (confirmedCntForOnePub > 0)
                    {
                        overwrittenConfirmId = 1;
                    }
                    else if (conflictCntForOnePub > 0)
                    {
                        overwrittenConfirmId = 2;
                    }
                    else
                    {
                        overwrittenConfirmId = 3;
                    }
                    //UpdateInitConfirm(prevPubId, overwrittenConfirmId);
                    if (overwrittenConfirmId == 1)
                    {
                        totalConfirmedCnt++;
                    }
                    else if (overwrittenConfirmId == 2)
                    {
                        totalConflictCnt++;
                    }
                    else
                    {
                        totalUnknownCnt++;
                    }
                }
                prevPubId = a.pubId;
                pubCnt++;
                confirmedCntForOnePub = 0;
                conflictCntForOnePub = 0;
                unknownCntForOnePub = 0;
            }
            if (c.clientId != 0)
            {
                UpdateAuthor(a.authorId, c.clientId);
                if (c.confirmId == 1)
                {
                    confirmedCntForOnePub++;
                }
                else if (c.confirmId == 2)
                {
                    conflictCntForOnePub++;
                }
                else
                {
                    unknownCntForOnePub++;
                }
            }
        }
        if (confirmedCntForOnePub > 0)
        {
            overwrittenConfirmId = 1;
        }
        else if (conflictCntForOnePub > 0)
        {
            overwrittenConfirmId = 2;
        }
        else
        {
            overwrittenConfirmId = 3;
        }
        //UpdateInitConfirm(lastPubId, overwrittenConfirmId);
        if (overwrittenConfirmId == 1)
        {
            totalConfirmedCnt++;
        }
        else if (overwrittenConfirmId == 2)
        {
            totalConflictCnt++;
        }
        else
        {
            totalUnknownCnt++;
        }
        if (overwrittenConfirmId == 1)
        {
            lblInitConfirmed.Text = "Confirmed";
        }
        else if (overwrittenConfirmId == 2)
        {
            lblInitConfirmed.Text = "Conflict";
        }
        else
        {
            lblInitConfirmed.Text = "Undecided";
        }

        /*
        ErrorMessage.Text = "Among " + pubCnt.ToString() + ", " + totalConfirmedCnt +
            " initial(s) are confirmed, " +
            totalConflictCnt.ToString() +
            " initial(s) are conflict, and " +
            totalUnknownCnt.ToString() +
            " initial(s) are unknown.";
        */
    }
    protected void UpdateAuthor(int authorId, int clientId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "update author" +
            " set client_id = " +
            clientId.ToString() +
            " where author_id = " +
            authorId.ToString();
        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected Client Confirm(Author a)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        string initial;
        string firstName;
        if (a.ForeName.IndexOf(' ') == -1)
        {
            firstName = a.ForeName;
        }
        else
        {
            firstName = a.ForeName.Substring(0, a.ForeName.IndexOf(' '));
        }

        if (a.Initials.Length == 2)
        {
            initial = a.Initials.Substring(1, 1);
        }
        else
        {
            initial = "";
        }

        if (initial == "")
        {
            sqlStatement =
                "select c.client_id," +
                " case when c.MI IS null" +
                " then 1" +
                " else 3" +
                " end as confirm" +
                " from CLIENT c" +
                " inner join client_status cs" +
                " on c.client_id = cs.client_id" +
                " and cs.l_client_status_id = 3" +
                " and (cs.end_date is null or" +
                " dateadd(year,1,cs.end_date) > getdate())" +
                " where c.last_name = @last_name" +
                " and c.first_name = @first_name";
        }
        else
        {
            sqlStatement =
                "select c.client_id," +
                " case when c.MI = @MI" +
                " then 1" +
                " when (c.MI IS not null and c.MI <> @MI" +
                ") then 2" +
                " else 3" +
                " end as confirm" +
                " from CLIENT c" +
                " inner join client_status cs" +
                " on c.client_id = cs.client_id" +
                " and cs.l_client_status_id = 3" +
                " and (cs.end_date is null or" +
                " dateadd(year,1,cs.end_date) > getdate())" +
                " where c.last_name = @last_name" +
                " and c.first_name = @first_name";
        }

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter last_nameParameter = new SqlParameter();
        last_nameParameter.ParameterName = "@last_name";
        last_nameParameter.SqlDbType = SqlDbType.VarChar;
        last_nameParameter.Value = a.LastName;
        command.Parameters.Add(last_nameParameter);

        SqlParameter first_nameParameter = new SqlParameter();
        first_nameParameter.ParameterName = "@first_name";
        first_nameParameter.SqlDbType = SqlDbType.VarChar;
        first_nameParameter.Value = firstName;
        command.Parameters.Add(first_nameParameter);

        if (initial != "")
        {
            SqlParameter MIParameter = new SqlParameter();
            MIParameter.ParameterName = "@MI";
            MIParameter.SqlDbType = SqlDbType.VarChar;
            MIParameter.Value = initial;
            command.Parameters.Add(MIParameter);
        }

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        client_idParameter.Direction = ParameterDirection.Output;
        command.Parameters.Add(client_idParameter);

        SqlParameter confirmParameter = new SqlParameter();
        confirmParameter.ParameterName = "@confirm";
        confirmParameter.SqlDbType = SqlDbType.Int;
        confirmParameter.Direction = ParameterDirection.Output;
        command.Parameters.Add(confirmParameter);

        conn.Open();
        SqlDataReader myReader;
        myReader = command.ExecuteReader();
        Client c = new Client();
        try
        {
            while (myReader.Read())
            {
                c.clientId = Convert.ToInt32(myReader["client_id"]);
                c.confirmId = Convert.ToInt32(myReader["confirm"]);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        return c;
    }

    protected void DisplayOnePub(int pmid)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        sqlStatement =
            "select lp.symbol as symbol," +
            " pd.authorlist + ' ' + " +
            " isnull(p.article_title,'') + ' <i>' + isnull(p.ISOAbbreviation,'') +" +
            " '</i> ' + convert(varchar,isnull(p.volume,'')) + ':' + isnull(p.MedlinePgn,'') + ', ' +" +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end +" +
            " isnull(p.pmcid,'') as publication," +
            " p.article_title" +
            " from publication_processing pd" +
            " inner join publication p" +
            " on pd.publication_id = p.publication_id" +
            " left outer join l_programmatic lp" +
            " on pd.l_programmatic_id = lp.l_programmatic_id" +
            " where p.pmid = " +
            pmid.ToString();

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                lblProgrammatic.Text = myReader["symbol"].ToString();
                //lblReport.Attributes.Add("style", "text-align:left;");
                lblPublication.Style.Add(HtmlTextWriterStyle.TextAlign, "Left");
                lblPublication.Text = myReader["publication"].ToString();
                string title = myReader["article_title"].ToString();
                int citationCnt = Helper.ReadCitation(title);
                if (citationCnt != -1)
                {
                    lblCitation.Text = citationCnt.ToString();
                }
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
    }
}