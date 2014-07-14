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

public partial class RetrievePub : System.Web.UI.Page
{
    public struct author
    {
        public string LastName;
        public string ForeName;
        public string Initials;
        public string Suffix;
    }

    public struct getResult
    {
        public string Program;
        public string Member;
        public int NumberPublications;
        public bool Import;
        public int NumImported;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLookup.LoadMember(ddlMember, "xxx","01/01/2000","01/01/2015");
            LoadLookup.LoadProgram(ddlProgram, "xxx", false);
            LoadAlphabetaDdl();
            hdnPubCnt.Value = "0";
            hdnMemberCnt.Value = "0";

            ddlPubMax.Items.Add("1");
            ddlPubMax.Items.Add("2");
            ddlPubMax.Items.Add("3");
            ddlPubMax.Items.Add("10");
            ddlPubMax.Items.Add("20");
            ddlPubMax.Items.Add("All");
            ddlPubMax.SelectedIndex = 5;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("PMID", typeof(string)));
            dt.Columns.Add(new DataColumn("Publication", typeof(string)));
            dt.Columns.Add(new DataColumn("Authors", typeof(string)));
            dt.Columns.Add(new DataColumn("Existing", typeof(string)));

            Session["PREVIEWTABLE"] = dt;

            object pmidObj = Request["pmid"];
            if (pmidObj != null)
            {
                int pmid = Convert.ToInt32(pmidObj);
                GetAndInsertAllInfoForOnePmid(pmid, true);
            }

            DataTable dtResults = new DataTable();
            dtResults.Columns.Add(new DataColumn("Program", typeof(string)));
            dtResults.Columns.Add(new DataColumn("Member", typeof(string)));
            dtResults.Columns.Add(new DataColumn("NoOfPub", typeof(int)));
            dtResults.Columns.Add(new DataColumn("NoImport", typeof(int)));

            Session["RESULTTABLE"] = dtResults;

            btnGetPubAll.Attributes.Add("onclick", "hourglass();");
            btnGetPubByProgram.Attributes.Add("onclick", "hourglass();");
            btnGetPubByMember.Attributes.Add("onclick", "hourglass();");
            btnGetPubByStartLetter.Attributes.Add("onclick", "hourglass();");

            btnImportArticleEDate.Attributes.Add("onclick", "hourglass();");
            btnImportBySpecifiedName.Attributes.Add("onclick", "hourglass();");
            btnImportCollectiveName.Attributes.Add("onclick", "hourglass();");
            btnImportEntrezDate.Attributes.Add("onclick", "hourglass();");
            btnReImportTitle.Attributes.Add("onclick", "hourglass();");
            btnSearchOnTitle.Attributes.Add("onclick", "hourglass();");
            btnSearchPmid.Attributes.Add("onclick", "hourglass();");
            btnSpecialImport.Attributes.Add("onclick", "hourglass();");

        }
    }
    protected void LoadAlphabetaDdl()
    {
        ddlStartWith.Items.Add("--");
        ddlStartWith.Items.Add("A");
        ddlStartWith.Items.Add("B");
        ddlStartWith.Items.Add("C");
        ddlStartWith.Items.Add("D");
        ddlStartWith.Items.Add("E");
        ddlStartWith.Items.Add("F");
        ddlStartWith.Items.Add("G");
        ddlStartWith.Items.Add("H");
        ddlStartWith.Items.Add("I");
        ddlStartWith.Items.Add("J");
        ddlStartWith.Items.Add("K");
        ddlStartWith.Items.Add("L");
        ddlStartWith.Items.Add("M");
        ddlStartWith.Items.Add("N");
        ddlStartWith.Items.Add("O");
        ddlStartWith.Items.Add("P");
        ddlStartWith.Items.Add("Q");
        ddlStartWith.Items.Add("R");
        ddlStartWith.Items.Add("S");
        ddlStartWith.Items.Add("T");
        ddlStartWith.Items.Add("U");
        ddlStartWith.Items.Add("V");
        ddlStartWith.Items.Add("W");
        ddlStartWith.Items.Add("X");
        ddlStartWith.Items.Add("Y");
        ddlStartWith.Items.Add("Z");
    }
    protected void btnGetPubByMember_Click(object sender, EventArgs e)
    {
        string client = ddlMember.SelectedItem.ToString();
        string firstName = client.Substring(client.IndexOf(',') + 2);
        string LastName = client.Substring(0, client.IndexOf(','));
        string clientPlusSign = firstName + "+" + LastName;
        hdnClientWithPlusSign.Value = clientPlusSign;

        SavePublog("Getting Publications for " + hdnClientWithPlusSign.Value.ToString() + " member", DateTime.Now, "btnGetPubByMember");

        GetAndInsertPubForOneMember(clientPlusSign);

        string msg = hdnPubCnt.Value + " publications imported.";
        //ErrorMessage.Text = msg;
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        hdnPubCnt.Value = "0";

        DataTable dt = (DataTable)Session["RESULTTABLE"];
        gvResults.Visible = true;
        gvResults.DataSource = dt;
        gvResults.DataBind();

        dt.Clear();
        Session["RESULTTABLE"] = dt;
    }
    protected void GetAndInsertPubForOneMember(string clientPlusSign, int programid = -1)
    {
        List<int> pmidListFromPubMed = GetPmidListForOneMemberFromPubMed(clientPlusSign);

        List<int> pmidListFromDb = GetPmidListForOneMemberInDb(clientPlusSign);
        List<int> pmidRejectFromDb = GetPmidListFromReject().ToList();

        // logs and add record to result table
        //SavePublog(clientPlusSign, DateTime.Now, "GetAndInsertPubForOneMember");
        SavePublog("Getting " + pmidListFromPubMed.Count.ToString() + " publications for " + clientPlusSign, DateTime.Now, "InsertPubForOneProgram");

        int no_imported = 0;
        foreach (int pmid in pmidListFromPubMed)
        {
            if (!pmidListFromDb.Contains(pmid) && !pmidRejectFromDb.Contains(pmid))
            {
                if (GetAndInsertAllInfoForOnePmid(pmid, false) > 0)
                {
                    no_imported++;
                }
            }
        }

        // hdnPubCnt

        DataTable dtResults = (DataTable)Session["RESULTTABLE"];
        DataRow dr = dtResults.NewRow();
        dr["Program"] = programid.ToString();
        dr["Member"] = clientPlusSign;
        dr["NoOfPub"] = pmidListFromPubMed.Count.ToString();
        dr["NoImport"] = no_imported;
        dtResults.Rows.Add(dr);
        Session["RESULTTABLE"] = dtResults;

    }
    protected List<int> GetPmidListForOneMemberInDb(string clientWithPlusSign)
    {
        List<int> pmidIdList = new List<int>();
        string firstName = clientWithPlusSign.Substring(0, clientWithPlusSign.IndexOf('+'));
        string lastName = clientWithPlusSign.Substring(clientWithPlusSign.IndexOf('+') + 1);
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement =
            "select p.pmid" +
            " from publication p" +
            " inner join publication_author pa" +
            " on p.publication_id = pa.publication_id" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.LastName = @last_name" +
            " and substring(a.ForeName, 0, charindex(' ', a.ForeName)) = @first_name";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);

        SqlParameter last_nameParameter = new SqlParameter();
        last_nameParameter.ParameterName = "@last_name";
        last_nameParameter.SqlDbType = SqlDbType.VarChar;
        last_nameParameter.Value = lastName;
        myCommand.Parameters.Add(last_nameParameter);

        SqlParameter first_nameParameter = new SqlParameter();
        first_nameParameter.ParameterName = "@first_name";
        first_nameParameter.SqlDbType = SqlDbType.VarChar;
        first_nameParameter.Value = firstName;
        myCommand.Parameters.Add(first_nameParameter);

        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();

        try 
        {
            while (myReader.Read())
            {
                if(myReader["pmid"].ToString() != "")
                {
                    pmidIdList.Add((int) myReader["pmid"]);
                }
            }

            //pmidIdList = (from IDataRecord record in myReader select (int)record["pmid"]).ToList();
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        return pmidIdList;
    }
    protected List<int> GetPmidListForOneMemberFromPubMed(string clientWithPlusSign)
    {
        string pubMax = ddlPubMax.SelectedItem.ToString();
        string url;
        if (pubMax == "20")
        {
            url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + clientWithPlusSign + @"[au]";
        }
        else if (pubMax == "All")
        {
            url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + clientWithPlusSign + @"[au]" + "&retmax=10000&usehistory=y";
        }
        else
        {
            url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + clientWithPlusSign + @"[au]" + "&retmax=" + pubMax;
        }
        XmlTextReader xmlReader = new XmlTextReader(url);
        string xmlNodeText = "";
        int pmid = 0;
        List<int> pmidList = new List<int>();
        //int pubCnt = Convert.ToInt32(hdnPubCnt.Value);

        while (xmlReader.Read())
        {
            switch (xmlReader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    xmlNodeText = xmlReader.Name;
                    break;

                case XmlNodeType.Text: //Display the text in each element.
                    if (xmlNodeText == "Id")
                    {
                        pmid = Convert.ToInt32(xmlReader.Value);
                        pmidList.Add(pmid);
                    }
                    break;

                case XmlNodeType.EndElement: //Display the end of the element.
                    break;
            }
        }
        //hdnPubCnt.Value = pubCnt.ToString();
        return pmidList;
    }
    protected IEnumerable<int> GetPmidListFromReject()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select pmid from reject_publication";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("pmid"));
                }
            }
        }
    }
    protected void btnGetPubByProgram_Click(object sender, EventArgs e)
    {

        int programId = Convert.ToInt32(ddlProgram.SelectedValue);
        SavePublog(ddlProgram.SelectedItem.ToString(), DateTime.Now, "GetPubByProgram");

        InsertPubForOneProgram(programId);

        string msg = hdnMemberCnt.Value + " members with " + hdnPubCnt.Value + " publications imported.";
        //ErrorMessage.Text = msg;
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        hdnPubCnt.Value = "0";
    }
    protected void btnGetPubByStartLetter_Click(object sender, EventArgs e)
    {
        string startLetter = ddlStartWith.SelectedItem.ToString();
        if (startLetter == "--")
        {
            string msg2 = "Please select a letter.";
            //ErrorMessage.Text = msg;
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg2 + "');", true);
            return;
        }
        InsertPubForStartLetter(startLetter);

        string msg = hdnMemberCnt.Value + " members with " + hdnPubCnt.Value + " publications imported.";
        //ErrorMessage.Text = msg;
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        hdnPubCnt.Value = "0";
    }
    protected void InsertPubForOneProgram(int programId)
    {
        List<string> nameList = GetNamePlusSignListForOneProgram(programId);

        SavePublog("Getting Publications for " + nameList.Count.ToString() + " members", DateTime.Now, "InsertPubForOneProgram");

        foreach (string clientWithPlusSign in nameList)
        {
            SavePublog("Getting Publications for " + clientWithPlusSign, DateTime.Now, "InsertPubForOneProgram");

            hdnClientWithPlusSign.Value = clientWithPlusSign;
            GetAndInsertPubForOneMember(clientWithPlusSign, programId);
        }
    }
    protected void InsertPubForStartLetter(string startLetter)
    {
        List<string> nameList = GetNamePlusSignListForStartLetter(startLetter);
        foreach (string clientWithPlusSign in nameList)
        {
            SavePublog(clientWithPlusSign, DateTime.Now, "InsertPubForStartLetter");
            hdnClientWithPlusSign.Value = clientWithPlusSign;
            GetAndInsertPubForOneMember(clientWithPlusSign);
        }
    }
    protected void btnGetPubAll_Click(object sender, EventArgs e)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement = "select convert(int,l_program_id) as l_program_id" +
            " from l_program" +
            " where abbreviation is not null" +
            " and abbreviation <> ''" +
            " and l_program_id not in (2,7)";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            List<int> programIdList = (from IDataRecord record in myReader
                                       select (int)record["l_program_id"]).ToList();

            // log getting programs 
            SavePublog("Getting Publications for " + programIdList.Count.ToString() + " program", DateTime.Now, "btnGetPubAll");

            // Testing of one program
            // InsertPubForOneProgram(programIdList[5]);
            foreach (int programId in programIdList)
            {
                InsertPubForOneProgram(programId);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        string msg = hdnMemberCnt.Value + " members with " + hdnPubCnt.Value + " publications imported.";
        //ErrorMessage.Text = msg;
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        hdnPubCnt.Value = "0";

        DataTable dtResults = (DataTable)Session["RESULTTABLE"];
        gvResults.Visible = true;
        gvResults.DataSource = dtResults;
        gvResults.DataBind();

        dtResults.Clear();
        Session["RESULTTABLE"] = dtResults;
    }
    protected List<string> GetNamePlusSignListForOneProgram(int programId)
    {
        List<string> nameList = new List<string>();

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        int memberCnt = Convert.ToInt32(hdnMemberCnt.Value);
        sqlStatement = "select" +
            " c.first_name + '+' + c.last_name as name" +
            " from client c" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
            " inner join client_program cp" +
            " on c.client_id = cp.client_id" +
            " and cp.l_program_id = " +
            programId.ToString() +
            " and cp.end_date is null";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {

            nameList = (from IDataRecord record in myReader
                                     select (string)record["name"]).ToList();
            /*
            foreach (string clientWithPlusSign in nameList)
            {
                SavePublog(clientWithPlusSign, DateTime.Now);
                hdnClientWithPlusSign.Value = clientWithPlusSign;
                GetPmidListForOneMemberFromPubMed(clientWithPlusSign);
                memberCnt++;
            }
             * */
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        hdnMemberCnt.Value = nameList.Count.ToString();
        //string msg = hdnMemberCnt.Value + " members with " + hdnPubCnt.Value + " publications imported.";
        //ErrorMessage.Text = msg;
        //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        return nameList;
    }
    protected List<string> GetNamePlusSignListForStartLetter(string startLetter)
    {
        List<string> nameList = new List<string>();

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        int memberCnt = Convert.ToInt32(hdnMemberCnt.Value);
        sqlStatement = "select" +
            " c.first_name + '+' + c.last_name as name" +
            " from client c" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
            " and left(c.last_name,1) = '" +
            startLetter +
            "'";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {

            nameList = (from IDataRecord record in myReader
                        select (string)record["name"]).ToList();
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        hdnMemberCnt.Value = nameList.Count.ToString();
        return nameList;
    }
    protected bool ReadXml(int pmid, out string journalTitleStr, out string issnStr, out string IssnTypeStr,
        out string VolumeStr, out string IssueStr, out string YearStr, out string SeasonStr, out string MonthStr,
        out string DayStr, out string MedlineDateStr, out string ArticleTitleStr, out string ISOAbbreviationStr, out string AbstractTextStr,
        out string MedlinePgnStr, out string pmcidStr, out List<author> authorList, out string CollectiveNameStr, out List<string> pubtypeList,
        out string articleEYearStr, out string articleEMonthStr, out string articleEDayStr,
        out string entrezYearStr, out string entrezMonthStr, out string entrezDayStr, 
        out string pubmedYearStr, out string pubmedMonthStr, out string pubmedDayStr)
    {
        string url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" +
            pmid.ToString() + @"&retmode=xml&rettype=abstract";

        XmlTextReader xmlReader = new XmlTextReader(url);
        /*
        if (pmid == 21320378)
        {
            int junk;
            junk = 0;
        }
         * */
        journalTitleStr = "";
        issnStr = "";
        IssnTypeStr = "";
        VolumeStr = "";
        IssueStr = "";
        YearStr = "";
        SeasonStr = "";
        MonthStr = "";
        DayStr = "";
        MedlineDateStr = "";
        ArticleTitleStr = "";
        ISOAbbreviationStr = "";
        AbstractTextStr = "";
        MedlinePgnStr = "";
        pmcidStr = "";
        authorList = new List<author>();
        pubtypeList = new List<string>();

        articleEYearStr = "";
        articleEMonthStr = "";
        articleEDayStr = "";
        entrezYearStr = "";
        entrezMonthStr = "";
        entrezDayStr = "";
        pubmedYearStr = "";
        pubmedMonthStr = "";
        pubmedDayStr = "";
        CollectiveNameStr = "";

        bool isBook = false;

        try
        {
            xmlReader.Read();
            while (!xmlReader.EOF) //load loop       
            {
                while (xmlReader.Name != "Article" || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                    if (xmlReader.Name == "PubmedBookArticle")
                    {
                        isBook = true;
                        break;
                    }
                }
                if (isBook)
                {
                    break;
                }
                while (xmlReader.Name != "Journal" || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                }
                while ((xmlReader.Name != "ISSN" && xmlReader.Name != "Title"
                    && xmlReader.Name != "ISOAbbreviation" && xmlReader.Name != "JournalIssue") || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                    if (xmlReader.Name == "ISSN")
                    {
                        IssnTypeStr = xmlReader.GetAttribute("IssnType");
                        issnStr = xmlReader.ReadElementString("ISSN");
                        continue;
                    }
                    if (xmlReader.Name == "JournalIssue")
                    {
                        xmlReader.Read();
                        while ((xmlReader.Name != "Volume" && xmlReader.Name != "Issue"
                            && xmlReader.Name != "PubDate") || !xmlReader.IsStartElement())
                        {
                            xmlReader.Read();
                            if (xmlReader.Name == "PubDate" && xmlReader.IsStartElement())
                            {
                                xmlReader.Read();
                                while ((xmlReader.Name != "Year" && xmlReader.Name != "Season" && xmlReader.Name != "Month"
                                    && xmlReader.Name != "Day" && xmlReader.Name != "MedlineDate") || !xmlReader.IsStartElement())
                                {
                                    xmlReader.Read();
                                    if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                                    {
                                        YearStr = xmlReader.ReadElementString("Year");
                                        continue;
                                    }
                                    if (xmlReader.Name == "Season" && xmlReader.IsStartElement())
                                    {
                                        SeasonStr = xmlReader.ReadElementString("Season");
                                        continue;
                                    }
                                    if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                                    {
                                        MonthStr = xmlReader.ReadElementString("Month");
                                        continue;
                                    }
                                    if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                                    {
                                        DayStr = xmlReader.ReadElementString("Day");
                                        continue;
                                    }
                                    if (xmlReader.Name == "MedlineDate" && xmlReader.IsStartElement())
                                    {
                                        MedlineDateStr = xmlReader.ReadElementString("MedlineDate");
                                        continue;
                                    }
                                    if (xmlReader.Name == "PubDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                                    {
                                        xmlReader.Read();
                                        break;
                                    }
                                }
                            }
                            if (xmlReader.Name == "Volume")
                            {
                                VolumeStr = xmlReader.ReadElementString("Volume");
                                continue;
                            }
                            if (xmlReader.Name == "Issue")
                            {
                                IssueStr = xmlReader.ReadElementString("Issue");
                                continue;
                            }
                            if (xmlReader.Name == "JournalIssue" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                xmlReader.Read();
                                break;
                            }
                        }
                        continue;
                    }
                    if (xmlReader.Name == "Title")
                    {
                        journalTitleStr = xmlReader.ReadElementString("Title");
                        continue;
                    }
                    if (xmlReader.Name == "ISOAbbreviation")
                    {
                        ISOAbbreviationStr = xmlReader.ReadElementString("ISOAbbreviation");
                        continue;
                    }
                    if (xmlReader.Name == "Journal" && xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        xmlReader.Read();
                        break;
                    }
                }
                while ((xmlReader.Name != "ArticleTitle" && xmlReader.Name != "MedlinePgn" &&
                    xmlReader.Name != "AbstractText" && xmlReader.Name != "AuthorList" &&
                    xmlReader.Name != "PublicationTypeList" &&
                    xmlReader.Name != "ArticleDate") || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                    if (xmlReader.Name == "ArticleTitle" && xmlReader.IsStartElement())
                    {
                        ArticleTitleStr = xmlReader.ReadElementString("ArticleTitle");
                        continue;
                    }
                    if (xmlReader.Name == "MedlinePgn" && xmlReader.IsStartElement())
                    {
                        MedlinePgnStr = xmlReader.ReadElementString("MedlinePgn");
                        continue;
                    }
                    if (xmlReader.Name == "AbstractText" && xmlReader.IsStartElement())
                    {
                        AbstractTextStr = xmlReader.ReadElementString("AbstractText");
                        continue;
                    }
                    if (xmlReader.Name == "Article" && xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        xmlReader.Read();
                        break;
                    }
                    if (xmlReader.Name == "AuthorList" && xmlReader.IsStartElement())
                    {
                        author at = new author();
                        while (xmlReader.Read())
                        {
                            if (xmlReader.IsStartElement())
                            {
                                switch (xmlReader.Name)
                                {
                                    case "Author":
                                        break;
                                    case "LastName":
                                        at.LastName = xmlReader.ReadString();
                                        break;
                                    case "ForeName":
                                        at.ForeName = xmlReader.ReadString();
                                        break;
                                    case "Initials":
                                        at.Initials = xmlReader.ReadString();
                                        authorList.Add(at);
                                        break;
                                    case "Suffix":
                                        string Suffix = xmlReader.ReadString();
                                        author a = authorList[authorList.Count - 1];
                                        a.Suffix = Suffix;
                                        authorList[authorList.Count - 1] = a;
                                        break;
                                    case "CollectiveName":
                                        CollectiveNameStr = xmlReader.ReadString();
                                        break;
                                }
                            }
                            if (xmlReader.Name == "AuthorList" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                        continue;
                    }
                    if (xmlReader.Name == "PublicationTypeList" && xmlReader.IsStartElement())
                    {
                        string pubtype;
                        while (xmlReader.Read())
                        {
                            if (xmlReader.IsStartElement())
                            {
                                switch (xmlReader.Name)
                                {
                                    case "PublicationType":
                                        pubtype = xmlReader.ReadString();
                                        pubtypeList.Add(pubtype);
                                        break;
                                }
                            }
                            if (xmlReader.Name == "PublicationTypeList" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                        continue;
                    }
                    if (xmlReader.Name == "ArticleDate" && xmlReader.IsStartElement())
                    {
                        string articleEStr = xmlReader.GetAttribute("DateType");
                        if (articleEStr == "Electronic")
                        {
                            xmlReader.Read();
                            while ((xmlReader.Name != "Year" && xmlReader.Name != "Month"
                                && xmlReader.Name != "Day") || !xmlReader.IsStartElement())
                            {
                                xmlReader.Read();
                                if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                                {
                                    articleEYearStr = xmlReader.ReadElementString("Year");
                                    continue;
                                }
                                if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                                {
                                    articleEMonthStr = xmlReader.ReadElementString("Month");
                                    continue;
                                }
                                if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                                {
                                    articleEDayStr = xmlReader.ReadElementString("Day");
                                    continue;
                                }
                                if (xmlReader.Name == "ArticleDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                                {
                                    xmlReader.Read();
                                    break;
                                }
                            }

                        }
                    }
                }
                while (xmlReader.Name != "History" || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                    if (xmlReader.Name == "History" && xmlReader.IsStartElement())
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader.IsStartElement())
                            {
                                string statusStr = xmlReader.GetAttribute("PubStatus");
                                if (statusStr == "entrez")
                                {
                                    xmlReader.Read();
                                    while ((xmlReader.Name != "Year" && xmlReader.Name != "Month"
                                        && xmlReader.Name != "Day") || !xmlReader.IsStartElement())
                                    {
                                        xmlReader.Read();
                                        if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                                        {
                                            entrezYearStr = xmlReader.ReadElementString("Year");
                                            continue;
                                        }
                                        if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                                        {
                                            entrezMonthStr = xmlReader.ReadElementString("Month");
                                            continue;
                                        }
                                        if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                                        {
                                            entrezDayStr = xmlReader.ReadElementString("Day");
                                            continue;
                                        }
                                        if ((xmlReader.Name == "Hour" || xmlReader.Name == "Minute") && xmlReader.IsStartElement())
                                        {
                                            break;
                                        }
                                        if (xmlReader.Name == "PubMedPubDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                                        {
                                            xmlReader.Read();
                                            break;
                                        }
                                    }
                                }
                                else if (xmlReader.GetAttribute("PubStatus") == "pubmed")
                                {
                                    xmlReader.Read();
                                    while ((xmlReader.Name != "Year" && xmlReader.Name != "Month"
                                        && xmlReader.Name != "Day") || !xmlReader.IsStartElement())
                                    {
                                        xmlReader.Read();
                                        if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                                        {
                                            pubmedYearStr = xmlReader.ReadElementString("Year");
                                            continue;
                                        }
                                        if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                                        {
                                            pubmedMonthStr = xmlReader.ReadElementString("Month");
                                            continue;
                                        }
                                        if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                                        {
                                            pubmedDayStr = xmlReader.ReadElementString("Day");
                                            continue;
                                        }
                                        if ((xmlReader.Name == "Hour" || xmlReader.Name == "Minute") && xmlReader.IsStartElement())
                                        {
                                            break;
                                        }
                                        if (xmlReader.Name == "PubMedPubDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                                        {
                                            xmlReader.Read();
                                            break;
                                        }
                                    }
                                    if ((xmlReader.Name == "PubmedData" || xmlReader.Name == "History") && xmlReader.NodeType == XmlNodeType.EndElement)
                                    {
                                        // xmlReader.Read();
                                        break;
                                    }

                                }
                                else
                                {
                                    continue;
                                }

                            }
                            if (xmlReader.Name == "History" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
                while (xmlReader.Name != "ArticleIdList" || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                    if (xmlReader.Name == "ArticleIdList" && xmlReader.IsStartElement())
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader.IsStartElement())
                            {
                                string IdTypeStr = xmlReader.GetAttribute("IdType");
                                if (IdTypeStr == "pmc")
                                {
                                    pmcidStr = xmlReader.ReadString();
                                    break;
                                }
                            }
                            if (xmlReader.Name == "ArticleIdList" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
                break;
            } // load loop         
        }
        catch (Exception ex)
        {
            // most likely a time out has happened
            ;
        }

        xmlReader.Close();
        return isBook;
    }
    protected bool MemberConflict(List<author> authorList)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        string clientWithPlusSign = hdnClientWithPlusSign.Value;
        if (clientWithPlusSign == "")
        {
            return false;
        }
        string clientLastName = clientWithPlusSign.Substring(clientWithPlusSign.IndexOf('+') + 1);
        string clientFirstName = clientWithPlusSign.Substring(0, clientWithPlusSign.IndexOf('+'));
        string authorLastName;
        string authorFirstName;
        string authorInitial;
        foreach (author a in authorList)
        {
            authorLastName = a.LastName;
            if (authorLastName == clientLastName)
            {
                if (a.ForeName.IndexOf(' ') == -1)
                {
                    authorFirstName = a.ForeName;
                }
                else
                {
                    authorFirstName = a.ForeName.Substring(0, a.ForeName.IndexOf(' '));
                }
                if (authorFirstName == clientFirstName)
                {
                    if (a.Initials.Length == 2)
                    {
                        authorInitial = a.Initials.Substring(1, 1);
                    }
                    else
                    {
                        authorInitial = "";
                        return false;
                    }
                    if (authorFirstName == clientFirstName)
                    {
                        sqlStatement =
                            "select" +
                            " case when (c.MI IS not null and c.MI <> @MI" +
                            ") then 1" +
                            " else 0" +
                            " end as conflict" +
                            " from CLIENT c" +
                            " inner join client_status cs" +
                            " on c.client_id = cs.client_id" +
                            " and cs.l_client_status_id = 3" +
                            " and (cs.end_date is null or" +
                            " dateadd(year,1,cs.end_date) > getdate())" +
                            " where c.last_name = @last_name" +
                            " and c.first_name = @first_name";

                        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);

                        SqlParameter last_nameParameter = new SqlParameter();
                        last_nameParameter.ParameterName = "@last_name";
                        last_nameParameter.SqlDbType = SqlDbType.VarChar;
                        last_nameParameter.Value = authorLastName;
                        myCommand.Parameters.Add(last_nameParameter);

                        SqlParameter first_nameParameter = new SqlParameter();
                        first_nameParameter.ParameterName = "@first_name";
                        first_nameParameter.SqlDbType = SqlDbType.VarChar;
                        first_nameParameter.Value = authorFirstName;
                        myCommand.Parameters.Add(first_nameParameter);

                        SqlParameter MIParameter = new SqlParameter();
                        MIParameter.ParameterName = "@MI";
                        MIParameter.SqlDbType = SqlDbType.VarChar;
                        MIParameter.Value = authorInitial;
                        myCommand.Parameters.Add(MIParameter);

                        conn.Open();
                        int conflictInt = 0;
                        object conflictObj = (object)myCommand.ExecuteScalar();
                        conn.Close();
                        if (conflictObj == null)
                        {
                            return false;
                        }
                        else
                        {
                            conflictInt = Convert.ToInt32(conflictObj);
                        }
                        if (conflictInt == 1)
                        {
                            hdnClientWithPlusSign.Value = "";
                            return true;
                        }
                        return false;

                    }
                }
            }
        }
        hdnClientWithPlusSign.Value = "";
        return false;
    }
    protected int GetAndInsertAllInfoForOnePmid(int pmid, bool forceImport)
    {
        string journalTitleStr = "";
        string issnStr = "";
        string IssnTypeStr = "";
        string VolumeStr = "";
        string IssueStr = "";
        string YearStr = "";
        string SeasonStr = "";
        string MonthStr = "";
        string DayStr = "";
        string MedlineDateStr = "";
        string ArticleTitleStr = "";
        string ISOAbbreviationStr = "";
        string AbstractTextStr = "";
        string MedlinePgnStr = "";
        string pmcidStr = "";
        List<author> authorList = new List<author>();
        string CollectiveNameStr = "";
        List<string> pubtypeList = new List<string>();
        string articleEYearStr;
        string articleEMonthStr;
        string articleEDayStr;
        string entrezYearStr;
        string entrezMonthStr;
        string entrezDayStr;
        string pubmedYearStr;
        string pubmedMonthStr;
        string pubmedDayStr;

        bool isBook = ReadXml(pmid, out journalTitleStr, out issnStr,
            out IssnTypeStr, out VolumeStr, out IssueStr,
            out YearStr, out SeasonStr, out MonthStr,
            out DayStr, out MedlineDateStr, out ArticleTitleStr, out ISOAbbreviationStr,
            out AbstractTextStr, out MedlinePgnStr, out pmcidStr,
            out authorList, out CollectiveNameStr, out pubtypeList, 
            out articleEYearStr, out articleEMonthStr, out articleEDayStr, 
            out entrezYearStr, out entrezMonthStr, out entrezDayStr, 
            out pubmedYearStr, out pubmedMonthStr, out pubmedDayStr);

        if (isBook)
        {
            return 0;
        }
        List<ProcessPub.authorInfo> authorInfoList = new List<ProcessPub.authorInfo>();
        foreach (author a in authorList)
        {
            ProcessPub.authorInfo ai = new ProcessPub.authorInfo();
            ai.LastName = a.LastName;
            ai.ForeName = a.ForeName;
            ai.Initials = a.Initials;
            ai.Suffix = a.Suffix;
            authorInfoList.Add(ai);
        }

        bool confirm1 = false;
        bool confirm4 = false;
        foreach (ProcessPub.authorInfo ai in authorInfoList)
        {
            ProcessPub.ClientConfirm cc = new ProcessPub.ClientConfirm();
            cc = ProcessPub.ConfirmOneAuthorOnAuthorInfo(ai);
            if (cc.confirmId == 1)
            {
                confirm1 = true;
                break;
            }
            else if (cc.confirmId == 4)
            {
                confirm4 = true;
                break;
            }
        }
        if (!confirm1 && !confirm4)
        //if (MemberConflict(authorList))
        {
            DateTime today = DateTime.Now;
            SavePublog(pmid.ToString() + " name conflict", today, "GetAndInsertAllInfoForOnePmid");
            if (!forceImport)
            {
                SaveReject(pmid, "name conflict"); //found reject name
                return 0;
            }
        }
        int publicationId =
            InsertOnePublication(pmid, journalTitleStr, issnStr,
                IssnTypeStr, VolumeStr, IssueStr,
                YearStr, SeasonStr, MonthStr,
                DayStr, MedlineDateStr, ArticleTitleStr, ISOAbbreviationStr,
                AbstractTextStr, MedlinePgnStr, pmcidStr, CollectiveNameStr, 
                articleEYearStr, articleEMonthStr, articleEDayStr, 
                entrezYearStr, entrezMonthStr, entrezDayStr, 
                pubmedYearStr, pubmedMonthStr, pubmedDayStr);

        if (publicationId == 0)
        {
            return publicationId;
        }
        InsertPublicationAuthors(publicationId, authorList);
        InsertPublicationPubtypes(publicationId, pubtypeList);
        int importedPubs = Convert.ToInt32(hdnPubCnt.Value);
        importedPubs++;
        hdnPubCnt.Value = importedPubs.ToString();
        return publicationId;
    }
    protected void ReadAllInfoForOnePmid(int pmid)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;
        string pmidStr = pmid.ToString();

        sqlStatement = "select count(*) from publication where pmid = " + pmidStr;
        SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        int existingPmidCnt = (int)commandCnt.ExecuteScalar();
        myConnection.Close();

        string journalTitleStr = "";
        string issnStr = "";
        string IssnTypeStr = "";
        string VolumeStr = "";
        string IssueStr = "";
        string YearStr = "";
        string SeasonStr = "";
        string MonthStr = "";
        string DayStr = "";
        string MedlineDateStr = "";
        string ArticleTitleStr = "";
        string ISOAbbreviationStr = "";
        string AbstractTextStr = "";
        string MedlinePgnStr = "";
        string pmcidStr = "";
        List<author> authorList = new List<author>();
        string CollectiveNameStr = "";
        List<string> pubtypeList = new List<string>();
        string articleEYearStr;
        string articleEMonthStr;
        string articleEDayStr;
        string entrezYearStr;
        string entrezMonthStr;
        string entrezDayStr;
        string pubmedYearStr;
        string pubmedMonthStr;
        string pubmedDayStr;

        bool isBook = ReadXml(pmid, out journalTitleStr, out issnStr,
            out IssnTypeStr, out VolumeStr, out IssueStr,
            out YearStr, out SeasonStr, out MonthStr,
            out DayStr, out MedlineDateStr, out ArticleTitleStr, out ISOAbbreviationStr,
            out AbstractTextStr, out MedlinePgnStr, out pmcidStr,
            out authorList, out CollectiveNameStr, out pubtypeList, 
            out articleEYearStr, out articleEMonthStr, out articleEDayStr, 
            out entrezYearStr, out entrezMonthStr, out entrezDayStr, 
            out pubmedYearStr, out pubmedMonthStr, out pubmedDayStr);

        if (isBook)
        {
            return;
        }
        List<ProcessPub.authorInfo> authorInfoList = new List<ProcessPub.authorInfo>();
        foreach (author a in authorList)
        {
            ProcessPub.authorInfo ai = new ProcessPub.authorInfo();
            ai.LastName = a.LastName;
            ai.ForeName = a.ForeName;
            ai.Initials = a.Initials;
            ai.Suffix = a.Suffix;
            authorInfoList.Add(ai);
        }

        bool confirm1 = false;
        bool confirm4 = false;
        foreach (ProcessPub.authorInfo ai in authorInfoList)
        {
            ProcessPub.ClientConfirm cc = new ProcessPub.ClientConfirm();
            cc = ProcessPub.ConfirmOneAuthorOnAuthorInfo(ai);
            if (cc.confirmId == 1)
            {
                confirm1 = true;
                break;
            }
            else if (cc.confirmId == 4)
            {
                confirm4 = true;
                break;
            }
        }
        //if (MemberConflict(authorList))
        if (!confirm1 && !confirm4)
        {
            //DateTime today = DateTime.Now;
            //SavePublog(pmid.ToString() + " conflict", today);
            //SaveReject(pmid, "conflict");
            //return;
        }
        string existing;
        if (existingPmidCnt > 0)
        {
            existing = "Yes";
        }
        else
        {
            existing = "No";
        }
        PreviewOnePub(ArticleTitleStr, ISOAbbreviationStr, VolumeStr, MedlinePgnStr,
            YearStr, pmid.ToString(), pmcidStr, authorList, existing);
    }
    protected void PreviewOnePub(string article_title, string ISOAbbreviation, string volume, string MedlinePgn, 
        string pub_year, string pmidStr, string pmcid, List<author> authorlist, string existing)
    {
        DataTable dt = (DataTable)Session["PREVIEWTABLE"];

        string publication = article_title + " <i>" + ISOAbbreviation + " " +
            "</i> " + volume + ":" + MedlinePgn + ", " +
            pub_year + ". " + pmcid;
        //string authorlistFormated = string.Join(",", authorlist.ToArray());
        StringBuilder sBuilder = new StringBuilder("");

        string authorlistFormated = "";
        foreach (author a in authorlist)
        {
            sBuilder.Append(a.LastName);
            sBuilder.Append(" ");
            sBuilder.Append(a.ForeName);
            sBuilder.Append(", ");
        }
        if (sBuilder.Length >= 2)
        {
            sBuilder.Remove(sBuilder.Length - 2, 2);
        }
        authorlistFormated = sBuilder.ToString();

        DataRow dr = dt.NewRow();

        dr["PMID"] = pmidStr;
        dr["Publication"] = publication;
        dr["Authors"] = authorlistFormated;
        dr["Existing"] = existing;
        dt.Rows.Add(dr);

        Session["PREVIEWTABLE"] = dt;
    }
    protected int InsertOnePublication(int pmid, string journalTitleStr, string issnStr,
            string IssnTypeStr, string VolumeStr, string IssueStr,
            string YearStr, string SeasonStr, string MonthStr,
            string DayStr, string MedlineDateStr, string ArticleTitleStr, string ISOAbbreviationStr,
            string AbstractTextStr, string MedlinePgnStr, string pmcidStr, string CollectiveNameStr, 
            string articleEYearStr, string articleEMonthStr, string articleEDayStr, 
            string entrezYearStr, string entrezMonthStr, string entrezDayStr, 
            string pubmedYearStr, string pubmedMonthStr, string pubmedDayStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;
        int publicationId = 0;
        DateTime today = DateTime.Now;
        string sqlStatementFinal = "";

        try
        {
            sqlStatement =
                "IF NOT EXISTS(SELECT pmid FROM publication WHERE pmid = @pmid)" +
                " insert into publication" +
                " (journal_title,issn,issn_type,volume,issue,pub_year,pub_season," +
                " pub_month,pub_day,MedlineDate,article_title,ISOAbbreviation,abstract_text,MedlinePgn,pmcid,pmid,collectiveName," +
                " article_e_year,article_e_month,article_e_day," +
                " entrez_year,entrez_month,entrez_day," +
                " pubmed_year,pubmed_month, pubmed_day) values " +
                " (@journal_title,@issn,@issn_type,@volume,@issue,@pub_year,@pub_season," +
                " @pub_month,@pub_day,@MedlineDate,@article_title,@ISOAbbreviation,@abstract_text,@MedlinePgn,@pmcid,@pmid,@CollectiveName," +
                " @article_e_year,@article_e_month,@article_e_day," +
                " @entrez_year,@entrez_month,@entrez_day," +
                " @pubmed_year,@pubmed_month, @pubmed_day)" +
                " SET @publication_id = SCOPE_IDENTITY()";


            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            try
            {
                SqlParameter journal_titleParameter = new SqlParameter();
                journal_titleParameter.ParameterName = "@journal_title";
                journal_titleParameter.SqlDbType = SqlDbType.VarChar;
                if (journalTitleStr != "")
                {
                    journal_titleParameter.Value = journalTitleStr;
                }
                else
                {
                    journal_titleParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(journal_titleParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (journal_title)");
            }


            try
            {
                SqlParameter issnParameter = new SqlParameter();
                issnParameter.ParameterName = "@issn";
                issnParameter.SqlDbType = SqlDbType.VarChar;
                if (issnStr != "")
                {
                    issnParameter.Value = issnStr;
                }
                else
                {
                    issnParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(issnParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (issn)");
            }

            try
            {
                SqlParameter issn_typeParameter = new SqlParameter();
                issn_typeParameter.ParameterName = "@issn_type";
                issn_typeParameter.SqlDbType = SqlDbType.VarChar;
                if (IssnTypeStr != "")
                {
                    issn_typeParameter.Value = IssnTypeStr;
                }
                else
                {
                    issn_typeParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(issn_typeParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (issn_type)");
            }

            try
            {
                SqlParameter volumeParameter = new SqlParameter();
                volumeParameter.ParameterName = "@volume";
                volumeParameter.SqlDbType = SqlDbType.VarChar;
                if (VolumeStr != "")
                {
                    volumeParameter.Value = VolumeStr;
                }
                else
                {
                    volumeParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(volumeParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (volume)");
            }

            try
            {
                SqlParameter issueParameter = new SqlParameter();
                issueParameter.ParameterName = "@issue";
                issueParameter.SqlDbType = SqlDbType.VarChar;
                if (IssueStr != "")
                {
                    issueParameter.Value = IssueStr;
                }
                else
                {
                    issueParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(issueParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (isse)");
            }

            try
            {
                SqlParameter pub_yearParameter = new SqlParameter();
                pub_yearParameter.ParameterName = "@pub_year";
                pub_yearParameter.SqlDbType = SqlDbType.Int;
                if (YearStr != "")
                {
                    pub_yearParameter.Value = Convert.ToInt32(YearStr);
                }
                else
                {
                    pub_yearParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(pub_yearParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pub_year)");
            }

            try
            {
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
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pub_season)");
            }

            try
            {
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
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pub_month)");
            }

            try
            {
                SqlParameter pub_dayParameter = new SqlParameter();
                pub_dayParameter.ParameterName = "@pub_day";
                pub_dayParameter.SqlDbType = SqlDbType.Int;
                if (DayStr != "")
                {
                    pub_dayParameter.Value = Convert.ToInt32(DayStr);
                }
                else
                {
                    pub_dayParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(pub_dayParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pub_day)");
            }

            try
            {
                SqlParameter MedlineDateParameter = new SqlParameter();
                MedlineDateParameter.ParameterName = "@MedlineDate";
                MedlineDateParameter.SqlDbType = SqlDbType.VarChar;
                if (MedlineDateStr != "")
                {
                    MedlineDateParameter.Value = MedlineDateStr;
                }
                else
                {
                    MedlineDateParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(MedlineDateParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (MedlineDate)");
            }

            try
            {
                SqlParameter article_titleParameter = new SqlParameter();
                article_titleParameter.ParameterName = "@article_title";
                article_titleParameter.SqlDbType = SqlDbType.NVarChar;
                if (ArticleTitleStr != "")
                {
                    article_titleParameter.Value = ArticleTitleStr;
                }
                else
                {
                    article_titleParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(article_titleParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (article_title)");
            }

            try
            {
                SqlParameter ISOAbbreviationParameter = new SqlParameter();
                ISOAbbreviationParameter.ParameterName = "@ISOAbbreviation";
                ISOAbbreviationParameter.SqlDbType = SqlDbType.VarChar;
                if (ISOAbbreviationStr != "")
                {
                    ISOAbbreviationParameter.Value = ISOAbbreviationStr.Replace(".", "");
                }
                else
                {
                    ISOAbbreviationParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(ISOAbbreviationParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (ISOAbbreviation)");
            }

            try
            {
                SqlParameter abstract_textParameter = new SqlParameter();
                abstract_textParameter.ParameterName = "@abstract_text";
                abstract_textParameter.SqlDbType = SqlDbType.NVarChar;
                if (AbstractTextStr != "")
                {
                    abstract_textParameter.Value = AbstractTextStr;
                }
                else
                {
                    abstract_textParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(abstract_textParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (abstract_text)");
            }

            try
            {
                SqlParameter MedlinePgnParameter = new SqlParameter();
                MedlinePgnParameter.ParameterName = "@MedlinePgn";
                MedlinePgnParameter.SqlDbType = SqlDbType.VarChar;
                if (MedlinePgnStr != "")
                {
                    MedlinePgnParameter.Value = MedlinePgnStr;
                }
                else
                {
                    MedlinePgnParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(MedlinePgnParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (MedlinePgn)");
            }

            try
            {
                SqlParameter pmcidParameter = new SqlParameter();
                pmcidParameter.ParameterName = "@pmcid";
                pmcidParameter.SqlDbType = SqlDbType.VarChar;
                if (pmcidStr != "")
                {
                    pmcidParameter.Value = pmcidStr;
                }
                else
                {
                    pmcidParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(pmcidParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pmcid)");
            }

            try
            {
                SqlParameter pmidParameter = new SqlParameter();
                pmidParameter.ParameterName = "@pmid";
                pmidParameter.SqlDbType = SqlDbType.Int;
                pmidParameter.Value = pmid;
                myCommand.Parameters.Add(pmidParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pmid)");
            }

            try
            {
                SqlParameter CollectiveNameParameter = new SqlParameter();
                CollectiveNameParameter.ParameterName = "@CollectiveName";
                CollectiveNameParameter.SqlDbType = SqlDbType.VarChar;
                if (CollectiveNameStr != "")
                {
                    CollectiveNameParameter.Value = CollectiveNameStr;
                }
                else
                {
                    CollectiveNameParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(CollectiveNameParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (CollectiveName)");
            }

            try
            {
                SqlParameter article_e_yearParameter = new SqlParameter();
                article_e_yearParameter.ParameterName = "@article_e_year";
                article_e_yearParameter.SqlDbType = SqlDbType.VarChar;
                if (articleEYearStr != "")
                {
                    article_e_yearParameter.Value = articleEYearStr;
                }
                else
                {
                    article_e_yearParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(article_e_yearParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (article_year)");
            }

            try
            {
                SqlParameter article_e_monthParameter = new SqlParameter();
                article_e_monthParameter.ParameterName = "@article_e_month";
                article_e_monthParameter.SqlDbType = SqlDbType.VarChar;
                if (articleEMonthStr != "")
                {
                    article_e_monthParameter.Value = articleEMonthStr;
                }
                else
                {
                    article_e_monthParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(article_e_monthParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (article_e_month)");
            }

            try
            {
                SqlParameter article_e_dayParameter = new SqlParameter();
                article_e_dayParameter.ParameterName = "@article_e_day";
                article_e_dayParameter.SqlDbType = SqlDbType.VarChar;
                if (articleEDayStr != "")
                {
                    article_e_dayParameter.Value = articleEDayStr;
                }
                else
                {
                    article_e_dayParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(article_e_dayParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (article_e_day)");
            }

            try
            {
                SqlParameter entrez_yearParameter = new SqlParameter();
                entrez_yearParameter.ParameterName = "@entrez_year";
                entrez_yearParameter.SqlDbType = SqlDbType.Int;
                if (entrezYearStr != "")
                {
                    entrez_yearParameter.Value = Convert.ToInt32(entrezYearStr);
                }
                else
                {
                    entrez_yearParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(entrez_yearParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (entrez_year)");
            }

            try
            {
                SqlParameter entrez_monthParameter = new SqlParameter();
                entrez_monthParameter.ParameterName = "@entrez_month";
                entrez_monthParameter.SqlDbType = SqlDbType.Int;
                if (entrezMonthStr != "")
                {
                    entrez_monthParameter.Value = Convert.ToInt32(entrezMonthStr);
                }
                else
                {
                    entrez_monthParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(entrez_monthParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (entrez_month)");
            }

            try
            {
                SqlParameter entrez_dayParameter = new SqlParameter();
                entrez_dayParameter.ParameterName = "@entrez_day";
                entrez_dayParameter.SqlDbType = SqlDbType.Int;
                if (entrezDayStr != "")
                {
                    entrez_dayParameter.Value = Convert.ToInt32(entrezDayStr);
                }
                else
                {
                    entrez_dayParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(entrez_dayParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (entrez_day)");
            }

            try
            {
                SqlParameter pubmed_yearParameter = new SqlParameter();
                pubmed_yearParameter.ParameterName = "@pubmed_year";
                pubmed_yearParameter.SqlDbType = SqlDbType.Int;
                if (pubmedYearStr != "")
                {
                    pubmed_yearParameter.Value = Convert.ToInt32(pubmedYearStr);
                }
                else
                {
                    pubmed_yearParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(pubmed_yearParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pubmed_year)");
            }

            try
            {
                SqlParameter pubmed_monthParameter = new SqlParameter();
                pubmed_monthParameter.ParameterName = "@pubmed_month";
                pubmed_monthParameter.SqlDbType = SqlDbType.Int;
                if (pubmedMonthStr != "")
                {
                    pubmed_monthParameter.Value = Convert.ToInt32(pubmedMonthStr);
                }
                else
                {
                    pubmed_monthParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(pubmed_monthParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pubmed_month)");
            }

            try
            {
                SqlParameter pubmed_dayParameter = new SqlParameter();
                pubmed_dayParameter.ParameterName = "@pubmed_day";
                pubmed_dayParameter.SqlDbType = SqlDbType.Int;
                if (pubmedDayStr != "")
                {
                    pubmed_dayParameter.Value = Convert.ToInt32(pubmedDayStr);
                }
                else
                {
                    pubmed_dayParameter.Value = DBNull.Value;
                }
                myCommand.Parameters.Add(pubmed_dayParameter);
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (pubmed_day)");
            }

            try
            {
                SqlParameter publication_idParameter = new SqlParameter();
                publication_idParameter.ParameterName = "@publication_id";
                publication_idParameter.SqlDbType = SqlDbType.Int;
                publication_idParameter.Direction = ParameterDirection.Output;
                myCommand.Parameters.Add(publication_idParameter);
                publicationId = (int)publication_idParameter.Value;
            }
            catch (Exception ex)
            {
                SavePublog(pmid.ToString() + ", " + ex.Message.ToString(), today, "Error - InsertOnePublication - Parameter (publication_id)");
            }

            sqlStatementFinal = myCommand.CommandText;

            myConnection.Open();
            myCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SavePublog(pmid.ToString() + ", " + ex.Message.ToString() , today, "Error - InsertOnePublication", sqlStatementFinal);
        }

        finally
        {
            myConnection.Close();
        }

        SavePublog(pmid.ToString(), today, "InsertOnePublication");

        return publicationId;
    }
    protected void SavePublog(string value, DateTime theDate, string tag, string query = "")
    {
        // Check this 
        //string pmuser = System.Web.HttpContext.Current.User.Identity.Name;
        //string pmuser = Session["userId"].ToString();
        string pmuser = "";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "insert into publog (value,log_date,time_stamp,pmuser,tag, query)" +
            " values(@value,@log_date,@time_stamp,@pmuser,@tag,@query)";

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

        SqlParameter time_stampParameter = new SqlParameter();
        time_stampParameter.ParameterName = "@time_stamp";
        time_stampParameter.SqlDbType = SqlDbType.DateTime;
        time_stampParameter.Value = theDate;
        command.Parameters.Add(time_stampParameter);

        SqlParameter pmuserParameter = new SqlParameter();
        pmuserParameter.ParameterName = "@pmuser";
        pmuserParameter.SqlDbType = SqlDbType.VarChar;
        pmuserParameter.Value = pmuser;
        command.Parameters.Add(pmuserParameter);

        SqlParameter tagParameter = new SqlParameter();
        tagParameter.ParameterName = "@tag";
        tagParameter.SqlDbType = SqlDbType.VarChar;
        tagParameter.Value = tag;
        command.Parameters.Add(tagParameter);

        SqlParameter queryParameter = new SqlParameter();
        queryParameter.ParameterName = "@query";
        queryParameter.SqlDbType = SqlDbType.VarChar;
        queryParameter.Value = query;
        command.Parameters.Add(queryParameter);

        try
        {
            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            // ignore this as this is the error log so no where for the message to land
            if(myConnection.State == ConnectionState.Open) myConnection.Close();
        }

    }
    protected void SaveReject(int pmid, string note)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement =
            "select count(reject_publication_id) as cnt" +
            " from reject_publication" +
            " where pmid = @pmid";

        SqlCommand commandReject = new SqlCommand(sqlStatement, myConnection);

        SqlParameter thepmidParameter = new SqlParameter();
        thepmidParameter.ParameterName = "@pmid";
        thepmidParameter.SqlDbType = SqlDbType.Int;
        thepmidParameter.Value = pmid;
        commandReject.Parameters.Add(thepmidParameter);

        myConnection.Open();
        int pmidCnt = (int)commandReject.ExecuteScalar();
        myConnection.Close();

        if (pmidCnt == 0)
        {
            sqlStatement =
                "IF NOT EXISTS(SELECT pmid FROM reject_publication WHERE pmid = @pmid)" +
                " insert into reject_publication (pmid, note)" +
                " values(@pmid, @note)";

            SqlCommand command = new SqlCommand(sqlStatement, myConnection);

            SqlParameter pmidParameter = new SqlParameter();
            pmidParameter.ParameterName = "@pmid";
            pmidParameter.SqlDbType = SqlDbType.Int;
            pmidParameter.Value = pmid;
            command.Parameters.Add(pmidParameter);

            SqlParameter noteParameter = new SqlParameter();
            noteParameter.ParameterName = "@note";
            noteParameter.SqlDbType = SqlDbType.VarChar;
            noteParameter.Value = note;
            command.Parameters.Add(noteParameter);

            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();
        }

    }
    protected int InsertAuthor(author a)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        int authorId = 0;
        //try
        //{
        sqlStatement =
            "select author_id" +
            " from author" +
            " where LastName = @LastName" +
            " and ForeName = @ForeName" +
            " and Initials = @Initials" +
            " and Suffix = @Suffix";


        SqlCommand commandAuthor = new SqlCommand(sqlStatement, myConnection);

        SqlParameter LastNameParameter = new SqlParameter();
        LastNameParameter.ParameterName = "@LastName";
        LastNameParameter.SqlDbType = SqlDbType.VarChar;
        LastNameParameter.Value = a.LastName;
        commandAuthor.Parameters.Add(LastNameParameter);

        SqlParameter ForeNameParameter = new SqlParameter();
        ForeNameParameter.ParameterName = "@ForeName";
        ForeNameParameter.SqlDbType = SqlDbType.VarChar;
        ForeNameParameter.Value = a.ForeName;
        commandAuthor.Parameters.Add(ForeNameParameter);

        SqlParameter InitialsParameter = new SqlParameter();
        InitialsParameter.ParameterName = "@Initials";
        InitialsParameter.SqlDbType = SqlDbType.VarChar;
        InitialsParameter.Value = a.Initials;
        commandAuthor.Parameters.Add(InitialsParameter);

        SqlParameter SuffixParameter = new SqlParameter();
        SuffixParameter.ParameterName = "@Suffix";
        SuffixParameter.SqlDbType = SqlDbType.VarChar;
        if (a.Suffix == null || a.Suffix == "")
        {
            SuffixParameter.Value = DBNull.Value; 
        }
        else
        {
            SuffixParameter.Value = a.Suffix;
        }
        commandAuthor.Parameters.Add(SuffixParameter);

        myConnection.Open();

        object authorObj = commandAuthor.ExecuteScalar();
        if (authorObj != null)
        {
            authorId = Convert.ToInt32(authorObj);
        }
        myConnection.Close();
        if (authorId != 0)
        {
            return authorId;
        }

        sqlStatement =
                " insert into author" +
                " (LastName, ForeName,Initials,Suffix) values (" +
                " @LastName, @ForeName, @Initials, @Suffix)" +
                " SET @author_id = SCOPE_IDENTITY()";

        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

        SqlParameter LastNameParameter3 = new SqlParameter();
        LastNameParameter3.ParameterName = "@LastName";
        LastNameParameter3.SqlDbType = SqlDbType.NVarChar;
        if (a.LastName != "")
        {
            LastNameParameter3.Value = a.LastName;
        }
        else
        {
            LastNameParameter3.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(LastNameParameter3);

        SqlParameter ForeNameParameter2 = new SqlParameter();
        ForeNameParameter2.ParameterName = "@ForeName";
        ForeNameParameter2.SqlDbType = SqlDbType.NVarChar;
        if (a.ForeName != "")
        {
            ForeNameParameter2.Value = a.ForeName;
        }
        else
        {
            ForeNameParameter2.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(ForeNameParameter2);

        SqlParameter InitialsParameter2 = new SqlParameter();
        InitialsParameter2.ParameterName = "@Initials";
        InitialsParameter2.SqlDbType = SqlDbType.NVarChar;
        if (a.Initials != "")
        {
            InitialsParameter2.Value = a.Initials;
        }
        else
        {
            InitialsParameter2.Value = DBNull.Value;
        }
        myCommand.Parameters.Add(InitialsParameter2);

        SqlParameter SuffixParameter2 = new SqlParameter();
        SuffixParameter2.ParameterName = "@Suffix";
        SuffixParameter2.SqlDbType = SqlDbType.NVarChar;
        if (a.Suffix == null || a.Suffix == "")
        {
            SuffixParameter2.Value = DBNull.Value;
        }
        else
        {
            SuffixParameter2.Value = a.Suffix;
        }
        myCommand.Parameters.Add(SuffixParameter2);

        SqlParameter author_idParameter = new SqlParameter();
        author_idParameter.ParameterName = "@author_id";
        author_idParameter.SqlDbType = SqlDbType.Int;
        author_idParameter.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(author_idParameter);


        myConnection.Open();
        myCommand.ExecuteNonQuery();
        authorId = (int)author_idParameter.Value;
        myConnection.Close();

        return authorId;
    }
    protected int InsertPubtype(string pubtype)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        int pubtypeId = 0;
        try
        {
            sqlStatement =
                "select pubtype_id" +
                " from pubtype" +
                " where description = @description";

            SqlCommand commandPubtype = new SqlCommand(sqlStatement, myConnection);

            SqlParameter descriptionParameter = new SqlParameter();
            descriptionParameter.ParameterName = "@description";
            descriptionParameter.SqlDbType = SqlDbType.VarChar;
            descriptionParameter.Value = pubtype;
            commandPubtype.Parameters.Add(descriptionParameter);

            myConnection.Open();

            object pubtypeObj = commandPubtype.ExecuteScalar();
            if (pubtypeObj != null)
            {
                pubtypeId = Convert.ToInt32(pubtypeObj);
            }
            myConnection.Close();
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = "ERROR: " + ex.Message.ToString() + "pubtype=" + pubtype;
        }
        if (pubtypeId != 0)
        {
            return pubtypeId;
        }

        //try
        //{
        sqlStatement =
                " insert into pubtype" +
                " (description) values (" +
                " @description)" +
                " SET @pubtype_id = SCOPE_IDENTITY()";

        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

        SqlParameter descriptionParameter2 = new SqlParameter();
        descriptionParameter2.ParameterName = "@description";
        descriptionParameter2.SqlDbType = SqlDbType.VarChar;
        if (pubtype != "")
        {
            descriptionParameter2.Value = pubtype;
        }
        else
        {
            ErrorMessage.Text = "Pubtype is empty";
            return 0;
        }
        myCommand.Parameters.Add(descriptionParameter2);

        SqlParameter pubtype_idParameter = new SqlParameter();
        pubtype_idParameter.ParameterName = "@pubtype_id";
        pubtype_idParameter.SqlDbType = SqlDbType.Int;
        pubtype_idParameter.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(pubtype_idParameter);


        myConnection.Open();
        myCommand.ExecuteNonQuery();
        pubtypeId = (int)pubtype_idParameter.Value;
        //}
        //catch (Exception ex)
        //{
        //    ErrorMessage.Text = "ERROR: " + ex.Message.ToString() + "type=" + type;
        //}
        //finally
        //{
        myConnection.Close();
        //}

        return pubtypeId;
    }
    protected void InsertPublicationAuthors(int publicationId, List<author> authorList)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        foreach (author ar in authorList) // Loop through List with foreach
        {

            int authorId = InsertAuthor(ar);
            sqlStatement =
                " insert into publication_author" +
                " (publication_id,author_id) values " +
                " (@publication_id,@author_id)";

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            myCommand.Parameters.Add(publication_idParameter);

            SqlParameter author_idParameter = new SqlParameter();
            author_idParameter.ParameterName = "@author_id";
            author_idParameter.SqlDbType = SqlDbType.Int;
            author_idParameter.Value = authorId;
            myCommand.Parameters.Add(author_idParameter);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

    }
    protected void InsertPublicationPubtypes(int publicationId, List<string> pubtypeList)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        foreach (string pubtype in pubtypeList) // Loop through List with foreach
        {

            int pubtypeId = InsertPubtype(pubtype);
            sqlStatement =
                " insert into publication_pubtype" +
                " (publication_id,pubtype_id) values " +
                " (@publication_id,@pubtype_id)";

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            myCommand.Parameters.Add(publication_idParameter);

            SqlParameter pubtype_idParameter = new SqlParameter();
            pubtype_idParameter.ParameterName = "@pubtype_id";
            pubtype_idParameter.SqlDbType = SqlDbType.Int;
            pubtype_idParameter.Value = pubtypeId;
            myCommand.Parameters.Add(pubtype_idParameter);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

    }
    protected void btnSearchOnTitle_Click(object sender, EventArgs e)
    {
        string url;
        string title = txtTitle.Text;
        title = title.Replace(' ', '+');

        //http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=Pubmed&retmax=3&term=Neprilysin+Regulates+Pulmonary+Artery&field=title
        url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=Pubmed&retmax=20&term=" + title + @"&field=title";

        SearchPub(url);
    }
    protected void SearchPub(string url)
    {
        XmlTextReader xmlReader = new XmlTextReader(url);
        string xmlNodeText = "";
        int pmid = 0;
        List<int> pmidList = new List<int>();
        //int pubCnt = Convert.ToInt32(hdnPubCnt.Value);

        while (xmlReader.Read())
        {
            switch (xmlReader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    xmlNodeText = xmlReader.Name;
                    break;

                case XmlNodeType.Text: //Display the text in each element.
                    if (xmlNodeText == "Id")
                    {
                        pmid = Convert.ToInt32(xmlReader.Value);
                        pmidList.Add(pmid);
                    }
                    break;

                case XmlNodeType.EndElement: //Display the end of the element.
                    break;
            }
        }
        foreach (int id in pmidList)
        {
            ReadAllInfoForOnePmid(id);
        }

        DataTable dt = (DataTable)Session["PREVIEWTABLE"];
        gvPublication.Visible = true;
        gvPublication.DataSource = dt;
        gvPublication.DataBind();

        dt.Clear();
        Session["PREVIEWTABLE"] = dt;
    }
    protected void gvPublication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string pmidStr = "";

        if (e.CommandName.Equals("Link"))
        {
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            LinkButton lnkImportTemp = null;
            int rowindex = rowSelect.RowIndex;
            lnkImportTemp = (LinkButton)gvPublication.Rows[rowindex].FindControl("lnkImport");
            if (lnkImportTemp == null)
            {
                return;
            }
            Label lblPmidTemp = null;
            lblPmidTemp = (Label)gvPublication.Rows[rowindex].FindControl("lblPmid");
            if (lblPmidTemp != null)
            {
                pmidStr = lblPmidTemp.Text;
            }
            else
            {
                return;
            }

            int pmid = Convert.ToInt32(pmidStr);
            hdnClientWithPlusSign.Value = "";
            int publicationId = GetAndInsertAllInfoForOnePmid(pmid,true);
            if (publicationId != 0)
            {
                ProcessUnconfirmedAuthorList();
                ProcessPub.ProcessAllInfoForOnePubId(publicationId, false);
                ProcessPub.UpdateFinalConfirm(publicationId, 1);
            }
            //Response.Redirect("DisplayOnePubByPmid.aspx?pmid=" + pmid.ToString());
            string msg = hdnPubCnt.Value + " publications imported and processed.";
            //ErrorMessage.Text = msg;
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            hdnPubCnt.Value = "0";
        }
    }
    protected void gvPublication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkImportTemp = null;
            lnkImportTemp = (LinkButton)e.Row.FindControl("lnkImport");
            if (lnkImportTemp == null)
            {
                return;
            }
            string imported = "";
            Label lblExistingTemp = null;
            lblExistingTemp = (Label)e.Row.FindControl("lblExisting");
            if (lblExistingTemp != null)
            {
                imported = lblExistingTemp.Text;
            }
            else
            {
                return;
            }
            if (imported == "Yes")
            {
                lnkImportTemp.Visible = false;
            }
        }
    }
    public IEnumerable<int> GetAllPmidList()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select pmid from publication";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("pmid"));
                }
            }
        }
    }
    public IEnumerable<int> GetAllPmidListNoEntrezDate()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select pmid from publication where entrez_year is null and pubmed_year is null and pmid is not null";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("pmid"));
                }
            }
        }
    }
    public IEnumerable<int> GetAllPmidListNoArticleEDate()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select pmid from publication where article_e_year is null and pmid is not null";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("pmid"));
                }
            }
        }
    }
    protected void btnReImportTitle_Click(object sender, EventArgs e)
    {
        //GetAndInsertAllInfoForOnePmid(22894553, true);
        //return;
        List<int> pmidList = GetAllPmidList().ToList();

        foreach (int pmid in pmidList)
        {
            string ArticleTitleStr = "";
            string AbstractTextStr = "";
            ReadTitleFromXml(pmid, out ArticleTitleStr, out AbstractTextStr);
            UpdatePublicationTitleByPmid(pmid, ArticleTitleStr, AbstractTextStr);
        }
        /*
        int pmid = 22086675;
        string ArticleTitleStr = "";
        string AbstractTextStr = "";
        ReadTitleFromXml(pmid, out ArticleTitleStr, out AbstractTextStr);
        UpdatePublicationTitleByPmid(pmid, ArticleTitleStr, AbstractTextStr);
         * */
    }
    protected bool ReadTitleFromXml(int pmid, out string ArticleTitleStr, out string AbstractTextStr)
    {
        string url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" +
            pmid.ToString() + @"&retmode=xml&rettype=abstract";

        XmlTextReader xmlReader = new XmlTextReader(url);
        ArticleTitleStr = "";
        AbstractTextStr = "";

        xmlReader.Read();
        bool isBook = false;
        while (!xmlReader.EOF) //load loop       
        {
            while (xmlReader.Name != "Article" || !xmlReader.IsStartElement())
            {
                xmlReader.Read();
                if (xmlReader.Name == "PubmedBookArticle")
                {
                    isBook = true;
                    break;
                }
            }
            if (isBook)
            {
                break;
            }
            while ((xmlReader.Name != "ArticleTitle" && xmlReader.Name != "AbstractText") || !xmlReader.IsStartElement())
            {
                xmlReader.Read();
                if (xmlReader.Name == "ArticleTitle" && xmlReader.IsStartElement())
                {
                    ArticleTitleStr = xmlReader.ReadElementString("ArticleTitle");
                    continue;
                }
                if (xmlReader.Name == "AbstractText" && xmlReader.IsStartElement())
                {
                    AbstractTextStr = xmlReader.ReadElementString("AbstractText");
                    continue;
                }
                if (xmlReader.Name == "Article" && xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    xmlReader.Read();
                    break;
                }
            }
            break;
        } // load loop         
        xmlReader.Close();
        return isBook;
    }
    protected void UpdatePublicationTitleByPmid(int pmid, string newTitle, string newAbstract)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "update publication" +
            " set article_title=@article_title," +
            " abstract_text=@abstract_text" +
            " where pmid=@pmid";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter pmidParameter = new SqlParameter();
        pmidParameter.ParameterName = "@pmid";
        pmidParameter.SqlDbType = SqlDbType.Int;
        pmidParameter.Value = pmid;
        command.Parameters.Add(pmidParameter);

        SqlParameter article_titleParameter = new SqlParameter();
        article_titleParameter.ParameterName = "@article_title";
        article_titleParameter.SqlDbType = SqlDbType.NVarChar;
        if (newTitle != "")
        {
            article_titleParameter.Value = newTitle;
        }
        else
        {
            article_titleParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(article_titleParameter);

        SqlParameter abstract_textParameter = new SqlParameter();
        abstract_textParameter.ParameterName = "@abstract_text";
        abstract_textParameter.SqlDbType = SqlDbType.NVarChar;
        if (newAbstract != "")
        {
            abstract_textParameter.Value = newAbstract;
        }
        else
        {
            abstract_textParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(abstract_textParameter);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected void UpdateCollectiveNameByPmid(int pmid, string CollectiveNameStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "update publication" +
            " set CollectiveName=@CollectiveName" +
            " where pmid=@pmid";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter pmidParameter = new SqlParameter();
        pmidParameter.ParameterName = "@pmid";
        pmidParameter.SqlDbType = SqlDbType.Int;
        pmidParameter.Value = pmid;
        command.Parameters.Add(pmidParameter);

        SqlParameter CollectiveNameParameter = new SqlParameter();
        CollectiveNameParameter.ParameterName = "@CollectiveName";
        CollectiveNameParameter.SqlDbType = SqlDbType.NVarChar;
        if (CollectiveNameStr != "")
        {
            CollectiveNameParameter.Value = CollectiveNameStr;
        }
        else
        {
            CollectiveNameParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(CollectiveNameParameter);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected void ReadEntrezDateFromXml(int pmid, out string entrezYearStr, out string entrezMonthStr,
        out string entrezDayStr, out string pubmedYearStr, out string pubmedMonthStr,
        out string pubmedDayStr)
    {
        string url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" +
            pmid.ToString() + @"&retmode=xml&rettype=abstract";

        entrezYearStr = "";
        entrezMonthStr = "";
        entrezDayStr = "";
        pubmedYearStr = "";
        pubmedMonthStr = "";
        pubmedDayStr = "";

        XmlTextReader xmlReader = new XmlTextReader(url);
        xmlReader.ReadToFollowing("PubmedData");
        if (xmlReader.Name == "PubmedData" && xmlReader.IsStartElement())
        {
            while (xmlReader.Read())
            {
                if (xmlReader.Name == "PubMedPubDate" && xmlReader.IsStartElement())
                {
                    string junkstr = xmlReader.GetAttribute("PubStatus");
                    if (xmlReader.GetAttribute("PubStatus") == "entrez")
                    {
                        xmlReader.Read();
                        while ((xmlReader.Name != "Year" && xmlReader.Name != "Month"
                            && xmlReader.Name != "Day") || !xmlReader.IsStartElement())
                        {
                            xmlReader.Read();
                            if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                            {
                                entrezYearStr = xmlReader.ReadElementString("Year");
                                continue;
                            }
                            if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                            {
                                entrezMonthStr = xmlReader.ReadElementString("Month");
                                continue;
                            }
                            if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                            {
                                entrezDayStr = xmlReader.ReadElementString("Day");
                                continue;
                            }
                            if ((xmlReader.Name == "Hour" || xmlReader.Name == "Minute") && xmlReader.IsStartElement())
                            {
                                break;
                            }
                            if (xmlReader.Name == "PubMedPubDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                xmlReader.Read();
                                break;
                            }
                        }
                    }
                    else if (xmlReader.GetAttribute("PubStatus") == "pubmed")
                    {
                        xmlReader.Read();
                        while ((xmlReader.Name != "Year" && xmlReader.Name != "Month"
                            && xmlReader.Name != "Day") || !xmlReader.IsStartElement())
                        {
                            xmlReader.Read();
                            if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                            {
                                pubmedYearStr = xmlReader.ReadElementString("Year");
                                continue;
                            }
                            if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                            {
                                pubmedMonthStr = xmlReader.ReadElementString("Month");
                                continue;
                            }
                            if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                            {
                                pubmedDayStr = xmlReader.ReadElementString("Day");
                                continue;
                            }
                            if ((xmlReader.Name == "Hour" || xmlReader.Name == "Minute") && xmlReader.IsStartElement())
                            {
                                break;
                            }
                            if (xmlReader.Name == "PubMedPubDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                            {
                                xmlReader.Read();
                                break;
                            }
                        }
                        if ((xmlReader.Name == "PubmedData" || xmlReader.Name == "History") && xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            // xmlReader.Read();
                            break;
                        }

                    }
                    else 
                    {
                        continue;
                    }
                }
                if ((xmlReader.Name == "PubmedData" || xmlReader.Name == "History") && xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
            }
        }
        xmlReader.Close();
    }
    protected void ReadArticleEDateFromXml(int pmid, out string articleEYearStr, out string articleEMonthStr,
        out string articleEDayStr)
    {
        string url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" +
            pmid.ToString() + @"&retmode=xml&rettype=abstract";

        articleEYearStr = "";
        articleEMonthStr = "";
        articleEDayStr = "";

        XmlTextReader xmlReader = new XmlTextReader(url);
        xmlReader.ReadToFollowing("ArticleDate");
        if (xmlReader.Name == "ArticleDate" && xmlReader.IsStartElement())
        {
            if (xmlReader.GetAttribute("DateType") == "Electronic")
            {
                xmlReader.Read();
                while ((xmlReader.Name != "Year" && xmlReader.Name != "Month"
                    && xmlReader.Name != "Day") || !xmlReader.IsStartElement())
                {
                    xmlReader.Read();
                    if (xmlReader.Name == "Year" && xmlReader.IsStartElement())
                    {
                        articleEYearStr = xmlReader.ReadElementString("Year");
                        continue;
                    }
                    if (xmlReader.Name == "Month" && xmlReader.IsStartElement())
                    {
                        articleEMonthStr = xmlReader.ReadElementString("Month");
                        continue;
                    }
                    if (xmlReader.Name == "Day" && xmlReader.IsStartElement())
                    {
                        articleEDayStr = xmlReader.ReadElementString("Day");
                        continue;
                    }
                    if (xmlReader.Name == "ArticleDate" && xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        xmlReader.Read();
                        break;
                    }
                }
            }
        }
        xmlReader.Close();
    }
    protected void ReadCollectiveNameFromXml(int pmid, out string CollectiveNameStr)
    {
        string url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" +
            pmid.ToString() + @"&retmode=xml&rettype=abstract";

        CollectiveNameStr = "";

        XmlTextReader xmlReader = new XmlTextReader(url);
        //xmlReader.ReadToFollowing("PubmedData");
        xmlReader.ReadToFollowing("CollectiveName");
        if (xmlReader.Name == "CollectiveName" && xmlReader.IsStartElement())
        {
            CollectiveNameStr = xmlReader.ReadElementString("CollectiveName");
        }
        xmlReader.Close();
    }
    protected void UpdatePublicationEntrezDateByPmid(int pmid, string entrezYearStr, string entrezMonthStr,
                string entrezDayStr, string pubmedYearStr, string pubmedMonthStr, string pubmedDayStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "update publication" +
            " set entrez_year=@entrez_year," +
            " entrez_month=@entrez_month," +
            " entrez_day=@entrez_day," +
            " pubmed_year=@pubmed_year," +
            " pubmed_month=@pubmed_month," +
            " pubmed_day=@pubmed_day" +
            " where pmid=@pmid";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter pmidParameter = new SqlParameter();
        pmidParameter.ParameterName = "@pmid";
        pmidParameter.SqlDbType = SqlDbType.Int;
        pmidParameter.Value = pmid;
        command.Parameters.Add(pmidParameter);

        SqlParameter entrez_yearParameter = new SqlParameter();
        entrez_yearParameter.ParameterName = "@entrez_year";
        entrez_yearParameter.SqlDbType = SqlDbType.Int;
        if (entrezYearStr != "")
        {
            entrez_yearParameter.Value = Convert.ToInt32(entrezYearStr);
        }
        else
        {
            entrez_yearParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(entrez_yearParameter);

        SqlParameter entrez_monthParameter = new SqlParameter();
        entrez_monthParameter.ParameterName = "@entrez_month";
        entrez_monthParameter.SqlDbType = SqlDbType.Int;
        if (entrezMonthStr != "")
        {
            entrez_monthParameter.Value = Convert.ToInt32(entrezMonthStr);
        }
        else
        {
            entrez_monthParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(entrez_monthParameter);

        SqlParameter entrez_dayParameter = new SqlParameter();
        entrez_dayParameter.ParameterName = "@entrez_day";
        entrez_dayParameter.SqlDbType = SqlDbType.Int;
        if (entrezDayStr != "")
        {
            entrez_dayParameter.Value = Convert.ToInt32(entrezDayStr);
        }
        else
        {
            entrez_dayParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(entrez_dayParameter);

        SqlParameter pubmed_yearParameter = new SqlParameter();
        pubmed_yearParameter.ParameterName = "@pubmed_year";
        pubmed_yearParameter.SqlDbType = SqlDbType.Int;
        if (pubmedYearStr != "")
        {
            pubmed_yearParameter.Value = Convert.ToInt32(pubmedYearStr);
        }
        else
        {
            pubmed_yearParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(pubmed_yearParameter);

        SqlParameter pubmed_monthParameter = new SqlParameter();
        pubmed_monthParameter.ParameterName = "@pubmed_month";
        pubmed_monthParameter.SqlDbType = SqlDbType.Int;
        if (pubmedMonthStr != "")
        {
            pubmed_monthParameter.Value = Convert.ToInt32(pubmedMonthStr);
        }
        else
        {
            pubmed_monthParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(pubmed_monthParameter);

        SqlParameter pubmed_dayParameter = new SqlParameter();
        pubmed_dayParameter.ParameterName = "@pubmed_day";
        pubmed_dayParameter.SqlDbType = SqlDbType.Int;
        if (pubmedDayStr != "")
        {
            pubmed_dayParameter.Value = Convert.ToInt32(pubmedDayStr);
        }
        else
        {
            pubmed_dayParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(pubmed_dayParameter);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected void UpdatePublicationArticleEDateByPmid(int pmid, string articleEYearStr, string articleEMonthStr,
                string articleEDayStr)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "update publication" +
            " set article_e_year=@article_e_year," +
            " article_e_month=@article_e_month," +
            " article_e_day=@article_e_day" +
            " where pmid=@pmid";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter pmidParameter = new SqlParameter();
        pmidParameter.ParameterName = "@pmid";
        pmidParameter.SqlDbType = SqlDbType.Int;
        pmidParameter.Value = pmid;
        command.Parameters.Add(pmidParameter);

        SqlParameter article_e_yearParameter = new SqlParameter();
        article_e_yearParameter.ParameterName = "@article_e_year";
        article_e_yearParameter.SqlDbType = SqlDbType.VarChar;
        if (articleEYearStr != "")
        {
            article_e_yearParameter.Value = articleEYearStr;
        }
        else
        {
            article_e_yearParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(article_e_yearParameter);

        SqlParameter article_e_monthParameter = new SqlParameter();
        article_e_monthParameter.ParameterName = "@article_e_month";
        article_e_monthParameter.SqlDbType = SqlDbType.VarChar;
        if (articleEMonthStr != "")
        {
            article_e_monthParameter.Value = articleEMonthStr;
        }
        else
        {
            article_e_monthParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(article_e_monthParameter);

        SqlParameter article_e_dayParameter = new SqlParameter();
        article_e_dayParameter.ParameterName = "@article_e_day";
        article_e_dayParameter.SqlDbType = SqlDbType.VarChar;
        if (articleEDayStr != "")
        {
            article_e_dayParameter.Value = articleEDayStr;
        }
        else
        {
            article_e_dayParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(article_e_dayParameter);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected void btnImportEntrezDate_Click(object sender, EventArgs e)
    {
        List<int> pmidList = GetAllPmidListNoEntrezDate().ToList();

        foreach (int pmid in pmidList)
        {
            string entrezYearStr;
            string entrezMonthStr;
            string entrezDayStr;
            string pubmedYearStr;
            string pubmedMonthStr;
            string pubmedDayStr;
            ReadEntrezDateFromXml(pmid, out entrezYearStr, out entrezMonthStr,
                out entrezDayStr, out pubmedYearStr, out pubmedMonthStr,
                out pubmedDayStr);
            UpdatePublicationEntrezDateByPmid(pmid, entrezYearStr, entrezMonthStr,
                entrezDayStr, pubmedYearStr, pubmedMonthStr, pubmedDayStr);
        }
        /*
        int pmid = 21303977;
            string entrezYearStr;
            string entrezMonthStr;
            string entrezDayStr;
            string pubmedYearStr;
            string pubmedMonthStr;
            string pubmedDayStr;
            ReadEntrezDateFromXml(pmid, out entrezYearStr, out entrezMonthStr,
                out entrezDayStr, out pubmedYearStr, out pubmedMonthStr,
                out pubmedDayStr);
            UpdatePublicationEntrezDateByPmid(pmid, entrezYearStr, entrezMonthStr,
                entrezDayStr, pubmedYearStr, pubmedMonthStr, pubmedDayStr);
         * */


    }
    protected void btnImportCollectiveName_Click(object sender, EventArgs e)
    {
        List<int> pmidList = GetAllPmidList().ToList();

        foreach (int pmid in pmidList)
        {
            string CollectiveNameStr = "";
            ReadCollectiveNameFromXml(pmid, out CollectiveNameStr);
            if (CollectiveNameStr != "")
            {
                UpdateCollectiveNameByPmid(pmid, CollectiveNameStr);
            }
        }

    }
    protected void btnImportBySpecifiedName_Click(object sender, EventArgs e)
    {
        string firstName = txtFirstName.Text;
        string LastName = txtLastName.Text;
        string clientPlusSign;
        if (firstName != "")
        {
            clientPlusSign = firstName + "+" + LastName;
        }
        else
        {
            clientPlusSign = LastName;
        }
        hdnClientWithPlusSign.Value = clientPlusSign;

        string url = @"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + clientPlusSign + @"[au]" + "&retmax=10000&usehistory=y";
        SearchPub(url);

    }
    protected void btnSearchPmid_Click(object sender, EventArgs e)
    {
        List<int> pmidList = null;

        if (txtPmidSearch.Text.Contains(' '))
        {
            pmidList = new List<int>(txtPmidSearch.Text.Split(' ').Select(int.Parse));
        }
        else if (txtPmidSearch.Text.Contains(','))
        {
            pmidList = new List<int>(txtPmidSearch.Text.Split(',').Select(int.Parse));
        }
        else
        {
            pmidList = new List<int>(txtPmidSearch.Text.Split(',').Select(int.Parse));
        }
        //List<int> pmidList = new List<int>(txtPmidSearch.Text.Split(',').Select(int.Parse));
        //int pmid = Convert.ToInt32(txtPmidSearch.Text);
        foreach (int pmid in pmidList)
        {
            ReadAllInfoForOnePmid(pmid);
        }

        DataTable dt = (DataTable)Session["PREVIEWTABLE"];
        gvPublication.Visible = true;
        gvPublication.DataSource = dt;
        gvPublication.DataBind();

        dt.Clear();
        Session["PREVIEWTABLE"] = dt;

    }
    protected void ProcessUnconfirmedAuthorList()
    {
        List<int> authorIdList = GetUnconfirmedAuthorList();
        ProcessPub.ProcessAuthorList(authorIdList);
    }
    protected List<int> GetUnconfirmedAuthorList()
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "select author_id from author " +
            " where confirm_id is null";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        List<int> authorList = new List<int>();
        try
        {
            while (myReader.Read())
            {
                int authorId = Convert.ToInt32(myReader["author_id"]);
                authorList.Add(authorId);
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        return authorList;
    }
    protected void btnImportArticleEDate_Click(object sender, EventArgs e)
    {
        List<int> pmidList = GetAllPmidListNoArticleEDate().ToList();

        foreach (int pmid in pmidList)
        {
            string articleEYearStr;
            string articleEMonthStr;
            string articleEDayStr;
            ReadArticleEDateFromXml(pmid, out articleEYearStr, out articleEMonthStr,
                out articleEDayStr);
            UpdatePublicationArticleEDateByPmid(pmid, articleEYearStr, articleEMonthStr, articleEDayStr);
        }

    }
    protected void btnSpecialImport_Click(object sender, EventArgs e)
    {
        if (!specialImportTable.Visible)
        {
            specialImportTable.Visible = true;
        }
        else
        {
            specialImportTable.Visible = false;
        }
    }
}
