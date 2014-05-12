using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

/// <summary>
/// Summary description for ProcessPub
/// </summary>
public class ProcessPub
{
    public struct authorInfo
    {
        public string LastName;
        public string ForeName;
        public string Initials;
        public string Suffix;
    }
    public struct ClientConfirm
    {
        public int clientId;
        public int confirmId;
    }
    public struct ProgramProgrammatic
    {
        public int programId;
        public int programmaticId;
    }
    public ProcessPub()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static void ProcessAllInfoForOnePubId(int publicationId, bool debug)
    {
        //declare
        string formatedAuthorlist;
        string formatedAuthorlistNoInst;
        int programmaticId;
        List<int> programList = new List<int>();
        List<ProgramProgrammatic> progProgList = new List<ProgramProgrammatic>();

        //process pub to get all results for publication_processing/publication_program/publication_programmatic tables.
        string publicationDate = MakePubDate(publicationId);
        MakeAuthorlistProgProg(publicationId, publicationDate, out formatedAuthorlist, out formatedAuthorlistNoInst, out programmaticId, out progProgList, out programList);
        string fullTextUrl = GetFullTextUrl(publicationId);
        string ePubDate = MakeEPubDate(publicationId);
        string formatedTitle = FormatTitle(publicationId);

        if (!debug)
        {
            //actually insert above results into the tables.
            InsertPublicationProcessing(publicationId, formatedAuthorlist, formatedAuthorlistNoInst, programmaticId, publicationDate, fullTextUrl, formatedTitle, ePubDate);
            InsertProgramList(publicationId, programList);
            InsertProgrammaticList(publicationId, progProgList);
        }
    }
    public static void UpdateAllProcessInfoForOnePubId(int publicationId)
    {
        //declare
        string formatedAuthorlist;
        string formatedAuthorlistNoInst;
        int programmaticId;
        List<int> programList = new List<int>();
        List<ProgramProgrammatic> progProgList = new List<ProgramProgrammatic>();

        //process pub to get all results for publication_processing/publication_program/publication_programmatic tables.
        string publicationDate = MakePubDate(publicationId);
        MakeAuthorlistProgProg(publicationId, publicationDate, out formatedAuthorlist, out formatedAuthorlistNoInst, out programmaticId, out progProgList, out programList);
        string fullTextUrl = GetFullTextUrl(publicationId);
        string ePubDate = MakeEPubDate(publicationId);
        string formatedTitle = FormatTitle(publicationId);

        //actually update above results into the tables.
        UpdatePublicationProcessing(publicationId, formatedAuthorlist, formatedAuthorlistNoInst, programmaticId, publicationDate, fullTextUrl, formatedTitle, ePubDate);
        UpdatePublicationProgram(publicationId, programList);
        UpdatePublicationProgrammatic(publicationId, progProgList);
    }
    public static void UpdateTitleForOnePubId(int publicationId)
    {
        string formatedTitle = FormatTitle(publicationId);
        UpdatePublicationProcessingWithTitle(publicationId, formatedTitle);
    }
    public static void UpdateEPubDateForOnePubId(int publicationId)
    {
        string ePubDate = MakeEPubDate(publicationId);
        UpdatePublicationProcessingWithEPubDate(publicationId, ePubDate);
    }
    public static void MakeAuthorlistProgProg(int publicationId, string pubDate, out string authorlist, out string authorlistNoInst, out int overallProgrammaticId, out List<ProgramProgrammatic> progProgList, out List<int> programIdList)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " a.LastName + ' ' + a.Initials as name," +
            " cp.l_program_id," +
            " lp.abbreviation as program," +
            " ci.l_institution_id," +
            " li.abbreviation as institution," +
            " p.CollectiveName," +
            " case when ((cs.end_date is null" +
            " and cs.start_date < '" +
            pubDate +
            "') or (cs.start_date <= '" +
            pubDate +
            "' and dateadd(year,1,cs.end_date) > '" +
            pubDate +
            "'))" +
	        " and cs.l_client_status_id = 3 then 1 else 0 end as is_client" +
            " from author a" +
            " inner join publication_author pa" +
            " on a.author_id = pa.author_id" +
            " inner join publication p" +
            " on pa.publication_id = p.publication_id" +
            " and p.publication_id = " +
            publicationId.ToString() +
            " left outer join client_program cp" +
            " on a.client_id = cp.client_id" +
            " and cp.end_date is null" +
            " and cp.start_date <= '" +
            pubDate +
            "' left outer join client_status cs" +
	        " on a.client_id = cs.client_id" + 
	        //" and (cs.end_date is null or (cs.start_date <= '" +
            //pubDate +
            //"' and dateadd(year,1,cs.end_date) > '" +
            //pubDate +
            //"'))" +
	        " and cs.l_client_status_id = 3" +
            " left outer join l_program lp" +
            " on cp.l_program_id = lp.l_program_id" +
            " left outer join client_institution ci" +
            " on a.client_id = ci.client_id" +
            " and ci.PRIMARY_INSTITUTION = 1" +
            " and ci.end_date is null" +
            " and ci.start_date < '" +
            pubDate +
            "'" +
            " left outer join l_institution li" +
            " on ci.l_institution_id = li.l_institution_id" +
            " order by publication_author_id";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        StringBuilder authorlistBuilder = new StringBuilder("");
        StringBuilder authorlistNoInstBuilder = new StringBuilder("");

        myReader = myCommand.ExecuteReader();
        string CollectiveNameStr = "";
        string name = "";
        string prevName = "";
        string programName = "";
        List<string> memberProgramList = new List<string>();
        string memberProgramArray = "";
        List<string> memberInstitutionList = new List<string>();
        string memberInstitutionArray = "";
        
        string institutionName = "";
        List<int> programIdListTemp = new List<int>();
        List<ProgramProgrammatic> progProgListTemp = new List<ProgramProgrammatic>();
        StringBuilder programmaticBuilder = new StringBuilder("");
        int[] programIdArray = new int[6] { 3, 4, 5, 6, 8, 10 };
        bool[] repeatProgramArray = new bool[6]{false, false, false, false, false, false};
        bool[] addedProgramArray = new bool[6]{false, false, false, false, false, false};
        List<int> repeatedProgramIdList = new List<int>();
        List<int> singleProgramIdList = new List<int>();



        int clientCnt = 0;
        int is_client = 0;
        int prev_is_client = 0;
        int programId = 0;
        try
        {
            while (myReader.Read())
            {
                name = myReader["name"].ToString();
                programName = myReader["program"].ToString();
                institutionName = myReader["institution"].ToString();
                CollectiveNameStr = myReader["CollectiveName"].ToString();
                is_client = Convert.ToInt32(myReader["is_client"]);
                object programIdObj = myReader["l_program_id"];
                if (programIdObj != DBNull.Value)
                {
                    programId = Convert.ToInt32(programIdObj);
                }
                if (is_client == 0)
                {
                    programName = "";
                    programId = 0;
                    institutionName = "";
                }
                if (prevName != name) //start process a member
                {
                    if (is_client == 1)
                    {
                        clientCnt++;
                    }
                    if (prev_is_client == 1)
                    {
                        if (memberProgramList.Count != 0)
                        {
                            memberProgramArray = Helper.ListToArray(memberProgramList, "/");
                            memberInstitutionArray = Helper.ListToArray(memberInstitutionList, "/");

                            authorlistNoInstBuilder.Append("<b>" + prevName);
                            authorlistNoInstBuilder.Append(" (");
                            authorlistNoInstBuilder.Append(memberProgramArray);
                            authorlistNoInstBuilder.Append(")</b>, ");

                            authorlistBuilder.Append("<b>" + prevName);
                            authorlistBuilder.Append(" (");
                            authorlistBuilder.Append(memberProgramArray);
                            authorlistBuilder.Append("-");
                            authorlistBuilder.Append(memberInstitutionArray);
                            authorlistBuilder.Append(")</b>, ");

                            memberProgramList.Clear();
                            memberInstitutionList.Clear();
                        }
                        else
                        {
                            authorlistNoInstBuilder.Append("<b>" + prevName);
                            authorlistNoInstBuilder.Append("</b>, ");

                            authorlistBuilder.Append("<b>" + prevName);
                            authorlistBuilder.Append("</b>, ");
                        }
                    }
                    else if (prevName != "")
                    {
                        authorlistBuilder.Append(prevName);
                        authorlistBuilder.Append(", ");
                        authorlistNoInstBuilder.Append(prevName);
                        authorlistNoInstBuilder.Append(", ");
                    }
                    prevName = name;
                    prev_is_client = is_client;
                }
                else
                {
                    prev_is_client = Math.Max(prev_is_client, is_client);
                }
                if (programName != "" && !memberProgramList.Contains(programName))
                {
                    memberProgramList.Add(programName);
                }
                if (institutionName != "" && !memberInstitutionList.Contains(institutionName))
                {
                    memberInstitutionList.Add(institutionName);
                }

                //calculate programmatic
                if (programIdListTemp.Contains(programId))
                {
                    if (!repeatedProgramIdList.Contains(programId))
                    {
                        repeatedProgramIdList.Add(programId);
                    }
                    if (singleProgramIdList.Contains(programId))
                    {
                        singleProgramIdList.Remove(programId);
                    }
                }
                else if (programId != 0)
                {
                    programIdListTemp.Add(programId);
                    singleProgramIdList.Add(programId);
                }
            }
            if (prev_is_client == 1)
            {
                if (memberProgramList.Count != 0)
                {
                    memberProgramArray = Helper.ListToArray(memberProgramList, "/");
                    memberInstitutionArray = Helper.ListToArray(memberInstitutionList, ",");

                    authorlistNoInstBuilder.Append("<b>" + prevName);
                    authorlistNoInstBuilder.Append(" (");
                    authorlistNoInstBuilder.Append(memberProgramArray);
                    authorlistNoInstBuilder.Append(")</b>");

                    authorlistBuilder.Append("<b>" + prevName);
                    authorlistBuilder.Append(" (");
                    authorlistBuilder.Append(memberProgramArray);
                    authorlistBuilder.Append("-");
                    authorlistBuilder.Append(memberInstitutionArray);
                    authorlistBuilder.Append(")</b>");

                    memberProgramList.Clear();
                    memberInstitutionList.Clear();
                }
                else
                {
                    authorlistNoInstBuilder.Append("<b>" + prevName);
                    authorlistNoInstBuilder.Append("</b>");

                    authorlistBuilder.Append("<b>" + prevName);
                    authorlistBuilder.Append("</b>");
                }
            }
            else if (is_client == 0)
            {
                authorlistBuilder.Append(name);
                authorlistNoInstBuilder.Append(name);
            }

            ProgramProgrammatic pp = new ProgramProgrammatic();
            overallProgrammaticId = 4;
            if (clientCnt >= 2)
            {
                if (programIdListTemp.Count > 1)
                {
                    if (clientCnt == 2)
                    {
                        overallProgrammaticId = 1;//??
                    }
                    else
                    {
                        if (repeatedProgramIdList.Count > 0)
                        {
                            overallProgrammaticId = 3;
                        }
                        else
                        {
                            overallProgrammaticId = 2;
                        }
                    }
                    foreach (int id in repeatedProgramIdList)
                    {
                        if (!singleProgramIdList.Contains(id))
                        {
                            pp.programId = id;
                            if (clientCnt == 2)
                            {
                                pp.programmaticId = 2;
                            }
                            else
                            {
                                pp.programmaticId = 3;
                            }
                            progProgListTemp.Add(pp);
                        }
                    }
                    foreach (int id in singleProgramIdList)
                    {
                        pp.programId = id;
                        pp.programmaticId = 1;
                        progProgListTemp.Add(pp);
                    }

                }
                else if (programIdListTemp.Count == 1)
                {
                    overallProgrammaticId = 2;
                    pp.programId = repeatedProgramIdList[0];
                    pp.programmaticId = 2;
                    progProgListTemp.Add(pp);
                }
                
            }

        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
        if (CollectiveNameStr != "")
        {
            authorlistBuilder.Append(", ");
            authorlistBuilder.Append(CollectiveNameStr);
            authorlistNoInstBuilder.Append(", ");
            authorlistNoInstBuilder.Append(CollectiveNameStr);
        }
        authorlist = authorlistBuilder.ToString();
        authorlistNoInst = authorlistNoInstBuilder.ToString();
        programIdList = programIdListTemp;
        progProgList = progProgListTemp;
    }
    protected static string GetFullTextUrl(int publicationId)
    {
        string url = "";

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "select pmcid from publication where publication_id = " + publicationId.ToString(); ;
        SqlCommand commandCnt = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        string pmcid = "";
        object pmcidObj = (object)commandCnt.ExecuteScalar();
        if (pmcidObj != null)
        {
            pmcid = pmcidObj.ToString();
        }
        myConnection.Close();

        if (pmcid != "")
        {
            url = @"http://www.ncbi.nlm.nih.gov/pmc/articles/" + pmcid + "/";
        }
        else
        {
        }

        return url;
    }
    public static bool IsReviewEditorial(int publicationId)
    {

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "select pubtype_id from publication_pubtype where publication_id = " + publicationId.ToString(); ;
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                string pubtype = myReader["pubtype_id"].ToString();
                if (pubtype == "2" || pubtype == "3" || pubtype == "6" || pubtype == "15")
                {
                    return true;
                }
            }
        }
        finally
        {
            myReader.Close();
            myConnection.Close();
        }
        return false;
    }
    public static string MakePubDate(int publicationId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        string YearStr = "";
        string SeasonStr = "";
        string MonthStr = "";
        string DayStr = "";
        string MedlineDateStr = "";
        sqlStatement = "select MedlineDate, pub_year, pub_season, pub_month, pub_day from publication where publication_id = " + publicationId.ToString();
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                MedlineDateStr = myReader["MedlineDate"].ToString();
                YearStr = myReader["pub_year"].ToString();
                SeasonStr = myReader["pub_season"].ToString();
                MonthStr = myReader["pub_month"].ToString();
                DayStr = myReader["pub_day"].ToString();
            }
        }
        finally
        {
            myReader.Close();
            myConnection.Close();
        }
        string publicationDate = "";
        string newMonthStr = "";
        string newDayStr = "";
        if (MedlineDateStr == "")
        {
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
                    ConvertMonthStr(MonthStr, DayStr, out newMonthStr, out newDayStr);
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
        }
        else //MedlineDateStr != ""
        {
            string MedlineDateStrTemp = MedlineDateStr;
            if (MedlineDateStrTemp.IndexOf(' ') != -1)
            {
                string yearStr = MedlineDateStrTemp.Substring(0, MedlineDateStrTemp.IndexOf(' '));
                //string yearStr = MedlineDateStrTemp.Substring(0, MedlineDateStrTemp.IndexOfAny(new char[] { ' ', '-' }));
                string monthDayStr = MedlineDateStrTemp.Substring(MedlineDateStrTemp.IndexOf(' ') + 1);
                //string monthDayStr = MedlineDateStrTemp.Substring(MedlineDateStrTemp.IndexOfAny(new char[] { ' ', '-' }) + 1);
                int dashPos = monthDayStr.IndexOf('-');
                int spacePos = monthDayStr.IndexOf(' ');
                string monthStr;
                string dayStr;
                if (spacePos == -1)
                {
                    if (dashPos == -1)
                    {
                        monthStr = monthDayStr;
                    }
                    else
                    {
                        monthStr = monthDayStr.Substring(0, dashPos);
                    }
                    dayStr = "01";
                }
                else
                {
                    monthStr = monthDayStr.Substring(0, spacePos);
                    dayStr = monthDayStr.Substring(spacePos + 1, dashPos - spacePos - 1);

                }

                string newMonth = "";
                string newDay = "";
                ConvertMonthStr(monthStr, dayStr, out newMonth, out newDay);
                publicationDate = yearStr + "-" + newMonth + "-" + newDay;
            }
            else if (MedlineDateStrTemp.IndexOf('-') != -1)
            {
                string yearStr = MedlineDateStrTemp.Substring(0, MedlineDateStrTemp.IndexOf('-'));
                //string monthDayStr = MedlineDateStrTemp.Substring(MedlineDateStrTemp.IndexOfAny(new char[] { ' ', '-' }) + 1);
                publicationDate = yearStr + "-01-01";
            }
            else
            {
                DateTime today = DateTime.Now;
                SavePublog("Bad MedlineDate: " + MedlineDateStrTemp + ", pubid=" + publicationId.ToString(), today);
            }
        }
        return publicationDate;

    }
    protected static string MakeEPubDate(int publicationId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;

        string entrezYearStr = "";
        string entrezMonthStr = "";
        string entrezDayStr = "";
        string pubmedYearStr = "";
        string pubmedMonthStr = "";
        string pubmedDayStr = "";

        sqlStatement = "select entrez_year, entrez_month, entrez_day, pubmed_year, pubmed_month, pubmed_day from publication where publication_id = " + publicationId.ToString();
        SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                entrezYearStr = myReader["entrez_year"].ToString();
                entrezMonthStr = myReader["entrez_month"].ToString();
                entrezDayStr = myReader["entrez_day"].ToString();
                pubmedYearStr = myReader["pubmed_year"].ToString();
                pubmedMonthStr = myReader["pubmed_month"].ToString();
                pubmedDayStr = myReader["pubmed_day"].ToString();
            }
        }
        finally
        {
            myReader.Close();
            myConnection.Close();
        }
        string ePubDate = "";
        string entrezPubDate = "";
        string pubmedPubDate = "";
        if (entrezYearStr == "" && pubmedYearStr == "")
        {
            return "";
        }
        if (entrezYearStr != "")
        {
            if (entrezMonthStr != "")
            {
                if (entrezDayStr != "")
                {
                    entrezPubDate = entrezMonthStr + "/" + entrezDayStr + "/" + entrezYearStr;
                }
                else
                {
                    entrezPubDate = entrezMonthStr + "/01/" + entrezYearStr;

                }
            }
            else
            {
                entrezPubDate = "01/01/" + entrezYearStr;
            }
        }
        if (pubmedYearStr != "")
        {
            if (pubmedMonthStr != "")
            {
                if (pubmedDayStr != "")
                {
                    pubmedPubDate = pubmedMonthStr + "/" + pubmedDayStr + "/" + pubmedYearStr;
                }
                else
                {
                    pubmedPubDate = pubmedMonthStr + "/01/" + pubmedYearStr;

                }
            }
            else
            {
                pubmedPubDate = "01/01/" + pubmedYearStr;
            }
        }
        DateTime entrezDateTime = DateTime.Parse(entrezPubDate);
        DateTime pubmedDateTime = DateTime.Parse(pubmedPubDate);
        if (entrezDateTime < pubmedDateTime)
        {
            ePubDate = entrezPubDate;
        }
        else
        {
            ePubDate = pubmedPubDate;
        }

        return ePubDate;

    }
    protected static string FormatTitle(int publicationId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        sqlStatement = "select article_title from publication where publication_id = " + publicationId.ToString();
        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        string articleTitle;
        object articleTitleObj = (object)myCommand.ExecuteScalar();
        if (articleTitleObj == null)
        {
            articleTitle = "";
        }
        else
        {
            articleTitle = articleTitleObj.ToString();
        }
        conn.Close();

        string formatedTitle = TitleCaseString(articleTitle);
        //string asTitleCase =
        //    System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(articleTitle.ToLower());
        return formatedTitle;
    }
    public static string TitleCaseString(string s)
    {
        string formatedTitle = "";
        if (s == null)
        {
            return s;
        }
        String[] words = s.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length == 0)
            {
                continue;
            }
            bool allUpper = true;
            for (int j = 0; j < words[i].Length; j++)
            {
                if (char.IsLower(words[i][j]))
                {
                    allUpper = false;
                    break;
                }
            }
            if (!allUpper)
            {
                words[i] = words[i].ToLower();
            }
            /*
            Char firstChar = Char.ToUpper(words[i][0]);
            String rest = "";
            if (words[i].Length > 1)
            {
                rest = words[i].Substring(1).ToLower();
            }
            words[i] = firstChar + rest;
             * */
        }
        formatedTitle = string.Join(" ", words);
        if (formatedTitle.Length > 0)
        {
            char firstChar = Char.ToUpper(formatedTitle[0]);
            string rest = formatedTitle.Substring(1);
            formatedTitle = firstChar + rest;
        }
        return formatedTitle;
    }
    protected static void InsertPublicationProcessing(int publicationId, string authorlist, string authorlistNoInst, int programmaticId,
        string publicationDate, string fullTextUrl, string formatedTitle, string ePubDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement = "select count(*) as cnt from publication_processing where publication_id = " + publicationId.ToString();

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int processed;
        object processedObj = (object)myCommand.ExecuteScalar();
        //if (processedObj == DBNull.Value)
        if (processedObj == null)
        {
            processed = 0;
        }
        else
        {
            processed = Convert.ToInt32(processedObj);
        }
        conn.Close();

        if (processed == 0)
        {
            sqlStatement =
                "insert into publication_processing" +
                " (publication_id, authorlist, authorlist_no_inst, l_programmatic_id,publication_date,full_text_url,formated_article_title, epub_date)" +
                " values(@publication_id,@authorlist,@authorlist_no_inst,@l_programmatic_id,@publication_date,@full_text_url,@formated_article_title,@epub_date" +
                ")";

            SqlCommand command = new SqlCommand(sqlStatement, conn);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            command.Parameters.Add(publication_idParameter);

            SqlParameter authorlistParameter = new SqlParameter();
            authorlistParameter.ParameterName = "@authorlist";
            authorlistParameter.SqlDbType = SqlDbType.VarChar;
            authorlistParameter.Value = authorlist;
            command.Parameters.Add(authorlistParameter);

            SqlParameter authorlist_no_instParameter = new SqlParameter();
            authorlist_no_instParameter.ParameterName = "@authorlist_no_inst";
            authorlist_no_instParameter.SqlDbType = SqlDbType.VarChar;
            authorlist_no_instParameter.Value = authorlistNoInst;
            command.Parameters.Add(authorlist_no_instParameter);

            SqlParameter l_programmatic_idParameter = new SqlParameter();
            l_programmatic_idParameter.ParameterName = "@l_programmatic_id";
            l_programmatic_idParameter.SqlDbType = SqlDbType.Int;
            l_programmatic_idParameter.Value = programmaticId;
            command.Parameters.Add(l_programmatic_idParameter);

            SqlParameter publication_dateParameter = new SqlParameter();
            publication_dateParameter.ParameterName = "@publication_date";
            publication_dateParameter.SqlDbType = SqlDbType.DateTime;
            if (publicationDate != "")
            {
                publication_dateParameter.Value = Convert.ToDateTime(publicationDate);
            }
            else
            {
                publication_dateParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(publication_dateParameter);

            SqlParameter full_text_urlParameter = new SqlParameter();
            full_text_urlParameter.ParameterName = "@full_text_url";
            full_text_urlParameter.SqlDbType = SqlDbType.VarChar;
            if (fullTextUrl != "")
            {
                full_text_urlParameter.Value = fullTextUrl;
            }
            else
            {
                full_text_urlParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(full_text_urlParameter);

            SqlParameter formated_article_titleParameter = new SqlParameter();
            formated_article_titleParameter.ParameterName = "@formated_article_title";
            formated_article_titleParameter.SqlDbType = SqlDbType.NVarChar;
            if (formatedTitle != "")
            {
                formated_article_titleParameter.Value = formatedTitle;
            }
            else
            {
                formated_article_titleParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(formated_article_titleParameter);

            SqlParameter epub_dateParameter = new SqlParameter();
            epub_dateParameter.ParameterName = "@epub_date";
            epub_dateParameter.SqlDbType = SqlDbType.DateTime;
            if (ePubDate != "")
            {
                epub_dateParameter.Value = Convert.ToDateTime(ePubDate);
            }
            else
            {
                epub_dateParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(epub_dateParameter);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }
        else
        {
            DateTime today = DateTime.Now;
            SavePublog("duplicate pubid=" + publicationId.ToString(), today);
        }
    }
    protected static void InsertProgramList(int publicationId, List<int> programList)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        foreach (int programId in programList)
        {
            sqlStatement = "select count(*) from publication_program where publication_id = " +
                publicationId.ToString() +
                " and l_program_id = " +
                programId.ToString();

            SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int cnt;
            object cntObj = (object)myCommand.ExecuteScalar();
            //if (cntObj == DBNull.Value)
            if (cntObj == null)
            {
                cnt = 0;
            }
            else
            {
                cnt = Convert.ToInt32(cntObj);
            }
            conn.Close();

            if (cnt == 0)
            {
                sqlStatement =
                    "insert into publication_program" +
                    " (publication_id, l_program_id)" +
                    " values(@publication_id,@l_program_id" +
                    ")";

                SqlCommand command = new SqlCommand(sqlStatement, conn);

                SqlParameter publication_idParameter = new SqlParameter();
                publication_idParameter.ParameterName = "@publication_id";
                publication_idParameter.SqlDbType = SqlDbType.Int;
                publication_idParameter.Value = publicationId;
                command.Parameters.Add(publication_idParameter);

                SqlParameter l_program_idParameter = new SqlParameter();
                l_program_idParameter.ParameterName = "@l_program_id";
                l_program_idParameter.SqlDbType = SqlDbType.Int;
                l_program_idParameter.Value = programId;
                command.Parameters.Add(l_program_idParameter);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
    protected static void InsertProgrammaticList(int publicationId, List<ProgramProgrammatic> programmaticList)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        foreach (ProgramProgrammatic pp in programmaticList)
        {
            sqlStatement = "select count(*) from publication_programmatic where publication_id = " +
                publicationId.ToString() +
                " and l_program_id = " +
                pp.programId.ToString() +
                " and l_programmatic_id = " +
                pp.programmaticId.ToString();

            SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int cnt;
            object cntObj = (object)myCommand.ExecuteScalar();
            //if (cntObj == DBNull.Value)
            if (cntObj == null)
            {
                cnt = 0;
            }
            else
            {
                cnt = Convert.ToInt32(cntObj);
            }
            conn.Close();

            if (cnt == 0)
            {
                sqlStatement =
                    "insert into publication_programmatic" +
                    " (publication_id, l_program_id, l_programmatic_id)" +
                    " values(@publication_id,@l_program_id, @l_programmatic_id" +
                    ")";

                SqlCommand command = new SqlCommand(sqlStatement, conn);

                SqlParameter publication_idParameter = new SqlParameter();
                publication_idParameter.ParameterName = "@publication_id";
                publication_idParameter.SqlDbType = SqlDbType.Int;
                publication_idParameter.Value = publicationId;
                command.Parameters.Add(publication_idParameter);

                SqlParameter l_program_idParameter = new SqlParameter();
                l_program_idParameter.ParameterName = "@l_program_id";
                l_program_idParameter.SqlDbType = SqlDbType.Int;
                l_program_idParameter.Value = pp.programId;
                command.Parameters.Add(l_program_idParameter);

                SqlParameter l_programmatic_idParameter = new SqlParameter();
                l_programmatic_idParameter.ParameterName = "@l_programmatic_id";
                l_programmatic_idParameter.SqlDbType = SqlDbType.Int;
                l_programmatic_idParameter.Value = pp.programmaticId;
                command.Parameters.Add(l_programmatic_idParameter);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
    public static void ConvertMonthStr(string MonthStr, string DayStr, out string newMonthStr, out string newDayStr)
    {
        newMonthStr = "";
        newDayStr = DayStr;
        switch (MonthStr)
        {
            case "Jan":
            case "01":
            case "1":
                newMonthStr = "01";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Feb":
            case "02":
            case "2":
                newMonthStr = "02";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Mar":
            case "03":
            case "3":
                newMonthStr = "03";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Apr":
            case "04":
            case "4":
                newMonthStr = "04";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "May":
            case "05":
            case "5":
                newMonthStr = "05";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Jun":
            case "06":
            case "6":
                newMonthStr = "06";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Jul":
            case "07":
            case "7":
                newMonthStr = "07";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Aug":
            case "08":
            case "8":
                newMonthStr = "08";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Sep":
            case "09":
            case "9":
                newMonthStr = "09";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Oct":
            case "10":
                newMonthStr = "10";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Nov":
            case "11":
                newMonthStr = "11";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
            case "Dec":
            case "12":
                newMonthStr = "12";
                if (DayStr == "")
                {
                    newDayStr = "01";
                }
                break;
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
    }
    protected static void SavePublog(string value, DateTime theDate)
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
    protected static void UpdatePublicationProcessing(int publicationId, string authorlist, string authorlistNoInst, int programmaticId,
        string publicationDate, string fullTextUrl, string formatedTitle, string ePubDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement = "select count(*) as cnt from publication_processing where publication_id = " + publicationId.ToString();

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int processed;
        object processedObj = (object)myCommand.ExecuteScalar();
        //if (processedObj == DBNull.Value)
        if (processedObj == null)
        {
            processed = 0;
        }
        else
        {
            processed = Convert.ToInt32(processedObj);
        }
        conn.Close();

        if (processed == 1)
        {
            sqlStatement =
                "update publication_processing" +
                " set authorlist=@authorlist, authorlist_no_inst=@authorlist_no_inst,l_programmatic_id=@l_programmatic_id," +
                " publication_date=@publication_date,full_text_url=@full_text_url,formated_article_title=@formated_article_title,epub_date = @epub_date" +
                " where publication_id=@publication_id";

            SqlCommand command = new SqlCommand(sqlStatement, conn);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            command.Parameters.Add(publication_idParameter);

            SqlParameter authorlistParameter = new SqlParameter();
            authorlistParameter.ParameterName = "@authorlist";
            authorlistParameter.SqlDbType = SqlDbType.VarChar;
            authorlistParameter.Value = authorlist;
            command.Parameters.Add(authorlistParameter);

            SqlParameter authorlist_no_instParameter = new SqlParameter();
            authorlist_no_instParameter.ParameterName = "@authorlist_no_inst";
            authorlist_no_instParameter.SqlDbType = SqlDbType.VarChar;
            authorlist_no_instParameter.Value = authorlistNoInst;
            command.Parameters.Add(authorlist_no_instParameter);

            SqlParameter l_programmatic_idParameter = new SqlParameter();
            l_programmatic_idParameter.ParameterName = "@l_programmatic_id";
            l_programmatic_idParameter.SqlDbType = SqlDbType.Int;
            l_programmatic_idParameter.Value = programmaticId;
            command.Parameters.Add(l_programmatic_idParameter);

            SqlParameter publication_dateParameter = new SqlParameter();
            publication_dateParameter.ParameterName = "@publication_date";
            publication_dateParameter.SqlDbType = SqlDbType.DateTime;
            if (publicationDate != "")
            {
                publication_dateParameter.Value = Convert.ToDateTime(publicationDate);
            }
            else
            {
                publication_dateParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(publication_dateParameter);

            SqlParameter full_text_urlParameter = new SqlParameter();
            full_text_urlParameter.ParameterName = "@full_text_url";
            full_text_urlParameter.SqlDbType = SqlDbType.VarChar;
            if (fullTextUrl != "")
            {
                full_text_urlParameter.Value = fullTextUrl;
            }
            else
            {
                full_text_urlParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(full_text_urlParameter);

            SqlParameter formated_article_titleParameter = new SqlParameter();
            formated_article_titleParameter.ParameterName = "@formated_article_title";
            formated_article_titleParameter.SqlDbType = SqlDbType.NVarChar;
            if (formatedTitle != "")
            {
                formated_article_titleParameter.Value = formatedTitle;
            }
            else
            {
                formated_article_titleParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(formated_article_titleParameter);

            SqlParameter epub_dateParameter = new SqlParameter();
            epub_dateParameter.ParameterName = "@epub_date";
            epub_dateParameter.SqlDbType = SqlDbType.DateTime;
            if (ePubDate != "")
            {
                epub_dateParameter.Value = Convert.ToDateTime(ePubDate);
            }
            else
            {
                epub_dateParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(epub_dateParameter);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }
        else
        {
            DateTime today = DateTime.Now;
            SavePublog("not processed pubid=" + publicationId.ToString(), today);
        }
    }
    protected static void UpdatePublicationProcessingWithTitle(int publicationId, string formatedTitle)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "update publication_processing" +
            " set formated_article_title=@formated_article_title" +
            " where publication_id=@publication_id";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Value = publicationId;
        command.Parameters.Add(publication_idParameter);

        SqlParameter formated_article_titleParameter = new SqlParameter();
        formated_article_titleParameter.ParameterName = "@formated_article_title";
        formated_article_titleParameter.SqlDbType = SqlDbType.NVarChar;
        if (formatedTitle != "")
        {
            formated_article_titleParameter.Value = formatedTitle;
        }
        else
        {
            formated_article_titleParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(formated_article_titleParameter);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected static void UpdatePublicationProcessingWithEPubDate(int publicationId, string ePubDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "update publication_processing" +
            " set epub_date=@epub_date" +
            " where publication_id=@publication_id";

        SqlCommand command = new SqlCommand(sqlStatement, conn);

        SqlParameter publication_idParameter = new SqlParameter();
        publication_idParameter.ParameterName = "@publication_id";
        publication_idParameter.SqlDbType = SqlDbType.Int;
        publication_idParameter.Value = publicationId;
        command.Parameters.Add(publication_idParameter);

        SqlParameter epub_dateParameter = new SqlParameter();
        epub_dateParameter.ParameterName = "@epub_date";
        epub_dateParameter.SqlDbType = SqlDbType.DateTime;
        if (ePubDate != "")
        {
            epub_dateParameter.Value = Convert.ToDateTime(ePubDate);
        }
        else
        {
            epub_dateParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(epub_dateParameter);


        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected static void UpdatePublicationProgram(int publicationId, List<int> programList)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "select l_program_id from publication_program where publication_id = " + publicationId.ToString();

        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader readerProgram;
        int program_id;
        readerProgram = command.ExecuteReader();
        try
        {
            while (readerProgram.Read())
            {
                program_id = Convert.ToInt32(readerProgram["l_program_id"]);
                if (!programList.Contains(program_id))
                {
                    RemovePubProg(publicationId, program_id);
                }
                else
                {
                    programList.Remove(program_id);
                }
            }
        }
        finally
        {
            readerProgram.Close();
            conn.Close();
        }

        foreach (int programId in programList)
        {
            sqlStatement =
                "insert into publication_program" +
                " (publication_id, l_program_id)" +
                " values(@publication_id,@l_program_id" +
                ")";

            SqlCommand commandInsert = new SqlCommand(sqlStatement, conn);

            SqlParameter publication_idParameter = new SqlParameter();
            publication_idParameter.ParameterName = "@publication_id";
            publication_idParameter.SqlDbType = SqlDbType.Int;
            publication_idParameter.Value = publicationId;
            commandInsert.Parameters.Add(publication_idParameter);

            SqlParameter l_program_idParameter = new SqlParameter();
            l_program_idParameter.ParameterName = "@l_program_id";
            l_program_idParameter.SqlDbType = SqlDbType.Int;
            l_program_idParameter.Value = programId;
            commandInsert.Parameters.Add(l_program_idParameter);

            conn.Open();
            commandInsert.ExecuteNonQuery();
            conn.Close();
        }
    }
    protected static void RemovePubProg(int publicationId, int programId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "delete from publication_program where publication_id = " + publicationId.ToString() + " and l_program_id = " + programId.ToString();

        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
    protected static void UpdatePublicationProgrammatic(int publicationId, List<ProgramProgrammatic> progProgList)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "delete from publication_Programmatic where publication_id = " +
            publicationId.ToString();

        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command.ExecuteScalar();
        conn.Close();

        foreach (ProgramProgrammatic pp in progProgList)
        {
            sqlStatement = "select count(*) from publication_programmatic where publication_id = " +
                publicationId.ToString() +
                " and l_program_id = " +
                pp.programId.ToString() +
                " and l_programmatic_id = " +
                pp.programmaticId.ToString();

            SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int cnt;
            object cntObj = (object)myCommand.ExecuteScalar();
            //if (cntObj == DBNull.Value)
            if (cntObj == null)
            {
                cnt = 0;
            }
            else
            {
                cnt = Convert.ToInt32(cntObj);
            }
            conn.Close();

            if (cnt == 0)
            {
                sqlStatement =
                    "insert into publication_programmatic" +
                    " (publication_id, l_program_id, l_programmatic_id)" +
                    " values(@publication_id,@l_program_id,@l_programmatic_id" +
                    ")";

                SqlCommand commandInsert = new SqlCommand(sqlStatement, conn);

                SqlParameter publication_idParameter = new SqlParameter();
                publication_idParameter.ParameterName = "@publication_id";
                publication_idParameter.SqlDbType = SqlDbType.Int;
                publication_idParameter.Value = publicationId;
                commandInsert.Parameters.Add(publication_idParameter);

                SqlParameter l_program_idParameter = new SqlParameter();
                l_program_idParameter.ParameterName = "@l_program_id";
                l_program_idParameter.SqlDbType = SqlDbType.Int;
                l_program_idParameter.Value = pp.programId;
                commandInsert.Parameters.Add(l_program_idParameter);

                SqlParameter l_programmatic_idParameter = new SqlParameter();
                l_programmatic_idParameter.ParameterName = "@l_programmatic_id";
                l_programmatic_idParameter.SqlDbType = SqlDbType.Int;
                l_programmatic_idParameter.Value = pp.programmaticId;
                commandInsert.Parameters.Add(l_programmatic_idParameter);

                conn.Open();
                commandInsert.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
    public static IEnumerable<int> GetAuthorIdListOfOnePub(int pubId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText =
                "select pa.author_id from publication_author pa" +
                " where pa.publication_id = " + pubId.ToString();
            /*
            cmd.CommandText = 
                "select a.author_id from publication_author pa" +
                " inner join author a" +
                " on pa.author_id = a.author_id" +
                " and a.client_id is not null" +
                " where pa.publication_id = " + pubId.ToString();
             * */
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("author_id"));
                }
            }
        }
    }
    public static int ConfirmOnePubByAuthorInitial(int pubId)
    {
        List<int> authorIdList = GetAuthorIdListOfOnePub(pubId).ToList();

        //int total = authorList.Count;
        int overwrittenConfirmId = 0;

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";

        sqlStatement =
            "select a.confirm_id, count(a.author_id) as cnt" +
            " from publication_author pa" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and pa.publication_id = " +
            pubId.ToString() +
            " group by a.confirm_id";

        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader readerAuthor;
        int confirm_id = 0;
        int cnt = 0;
        bool confirm1 = false;
        bool confirm4 = false;
        readerAuthor = command.ExecuteReader();
        try
        {
            while (readerAuthor.Read())
            {
                if (readerAuthor["confirm_id"] != DBNull.Value)
                {
                    confirm_id = Convert.ToInt32(readerAuthor["confirm_id"]);
                    cnt = Convert.ToInt32(readerAuthor["cnt"]);
                    if (confirm_id == 1 && cnt > 0)
                    {
                        confirm1 = true;
                        break;
                    }
                    else if (confirm_id == 4 && cnt > 0)
                    {
                        confirm4 = true;
                    }
                }
            }
        }
        finally
        {
            readerAuthor.Close();
            conn.Close();
        }

        if (confirm1)
        {
            overwrittenConfirmId = 1;
        }
        else if (confirm4)
        {
            overwrittenConfirmId = 4;
        }
        else 
        {
            overwrittenConfirmId = 2;
        }
        
        return overwrittenConfirmId;
    }
    public static ClientConfirm ConfirmOneAuthor(int authorId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        sqlStatement = "select LastName, ForeName, Initials from author where author_id = " + authorId.ToString();
        SqlCommand command = new SqlCommand(sqlStatement, conn);

        string LastName = "";
        string ForeName = "";
        string Initials = "";
        conn.Open();
        SqlDataReader readerAuthor;
        readerAuthor = command.ExecuteReader();
        try
        {
            while (readerAuthor.Read())
            {
                LastName = readerAuthor["LastName"].ToString();
                ForeName = readerAuthor["ForeName"].ToString();
                Initials = readerAuthor["Initials"].ToString();
            }
        }
        finally
        {
            readerAuthor.Close();
            conn.Close();
        }

        string initial;
        string firstName;
        if (ForeName.IndexOf(' ') == -1)
        {
            firstName = ForeName;
        }
        else
        {
            firstName = ForeName.Substring(0, ForeName.IndexOf(' '));
        }

        if (Initials.Length == 2)
        {
            initial = Initials.Substring(1, 1);
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
                " else 4" +
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
                " case" +
                " when (c.MI is not null and c.MI <> '' and c.MI = @MI)" +
                " then 1" +
                " when (c.MI IS not null and c.MI <> '' and c.MI <> @MI)" +
                " then 2" +
                " when (c.MI is null or c.MI = '')" +
                " then 4" +
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

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        if (initial == "")
        {
            SqlParameter last_nameParameter = new SqlParameter();
            last_nameParameter.ParameterName = "@last_name";
            last_nameParameter.SqlDbType = SqlDbType.VarChar;
            last_nameParameter.Value = LastName;
            myCommand.Parameters.Add(last_nameParameter);

            SqlParameter first_nameParameter = new SqlParameter();
            first_nameParameter.ParameterName = "@first_name";
            first_nameParameter.SqlDbType = SqlDbType.VarChar;
            first_nameParameter.Value = firstName;
            myCommand.Parameters.Add(first_nameParameter);

        }
        else
        {
            SqlParameter miParameter = new SqlParameter();
            miParameter.ParameterName = "@mi";
            miParameter.SqlDbType = SqlDbType.VarChar;
            miParameter.Value = initial;
            myCommand.Parameters.Add(miParameter);

            SqlParameter last_nameParameter = new SqlParameter();
            last_nameParameter.ParameterName = "@last_name";
            last_nameParameter.SqlDbType = SqlDbType.VarChar;
            last_nameParameter.Value = LastName;
            myCommand.Parameters.Add(last_nameParameter);

            SqlParameter first_nameParameter = new SqlParameter();
            first_nameParameter.ParameterName = "@first_name";
            first_nameParameter.SqlDbType = SqlDbType.VarChar;
            first_nameParameter.Value = firstName;
            myCommand.Parameters.Add(first_nameParameter);

        }

        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ClientConfirm c = new ClientConfirm();
        c.clientId = 0;
        c.confirmId = 0;
        try
        {
            while (myReader.Read())
            {
                object clientIdObj = myReader["client_id"];
                if (clientIdObj != DBNull.Value)
                {
                    c.clientId = Convert.ToInt32(clientIdObj);
                }
                else
                {
                    break;
                }
                object confirmIdObj = myReader["confirm"];
                if (confirmIdObj != DBNull.Value)
                {
                    c.confirmId = Convert.ToInt32(confirmIdObj);
                }
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

        return c;
    }
    public static ClientConfirm ConfirmOneAuthorOnAuthorInfo(authorInfo a)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        string LastName = a.LastName;
        string ForeName = a.ForeName;
        string Initials = a.Initials;
        string initial;
        string firstName;
        if (ForeName.IndexOf(' ') == -1)
        {
            firstName = ForeName;
        }
        else
        {
            firstName = ForeName.Substring(0, ForeName.IndexOf(' '));
        }

        if (Initials.Length == 2)
        {
            initial = Initials.Substring(1, 1);
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
                " else 4" +
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
                " case" +
                " when (c.MI is not null and c.MI <> '' and c.MI = @MI)" +
                " then 1" +
                " when (c.MI IS not null and c.MI <> '' and c.MI <> @MI)" +
                " then 2" +
                " when (c.MI is null or c.MI = '')" +
                " then 4" +
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

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        if (initial == "")
        {
            SqlParameter last_nameParameter = new SqlParameter();
            last_nameParameter.ParameterName = "@last_name";
            last_nameParameter.SqlDbType = SqlDbType.VarChar;
            last_nameParameter.Value = LastName;
            myCommand.Parameters.Add(last_nameParameter);

            SqlParameter first_nameParameter = new SqlParameter();
            first_nameParameter.ParameterName = "@first_name";
            first_nameParameter.SqlDbType = SqlDbType.VarChar;
            first_nameParameter.Value = firstName;
            myCommand.Parameters.Add(first_nameParameter);

        }
        else
        {
            SqlParameter miParameter = new SqlParameter();
            miParameter.ParameterName = "@mi";
            miParameter.SqlDbType = SqlDbType.VarChar;
            miParameter.Value = initial;
            myCommand.Parameters.Add(miParameter);

            SqlParameter last_nameParameter = new SqlParameter();
            last_nameParameter.ParameterName = "@last_name";
            last_nameParameter.SqlDbType = SqlDbType.VarChar;
            last_nameParameter.Value = LastName;
            myCommand.Parameters.Add(last_nameParameter);

            SqlParameter first_nameParameter = new SqlParameter();
            first_nameParameter.ParameterName = "@first_name";
            first_nameParameter.SqlDbType = SqlDbType.VarChar;
            first_nameParameter.Value = firstName;
            myCommand.Parameters.Add(first_nameParameter);

        }

        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ClientConfirm c = new ClientConfirm();
        c.clientId = 0;
        c.confirmId = 0;
        try
        {
            while (myReader.Read())
            {
                object clientIdObj = myReader["client_id"];
                if (clientIdObj != DBNull.Value)
                {
                    c.clientId = Convert.ToInt32(clientIdObj);
                }
                else
                {
                    break;
                }
                object confirmIdObj = myReader["confirm"];
                if (confirmIdObj != DBNull.Value)
                {
                    c.confirmId = Convert.ToInt32(confirmIdObj);
                }
            }
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

        return c;
    }
    public static void UpdateNameConfirm(int pubId, int confirmId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "update publication_processing" +
            " set name_confirm_id = " +
            confirmId.ToString() +
            " where publication_id = " +
            pubId.ToString();

        SqlCommand command2 = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command2.ExecuteNonQuery();
        conn.Close();
    }
    public static void SaveRejectOnPubId(int pubId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement =
            "select count(*) from reject_publication where pmid = (select pmid from publication where publication_id = @publication_id)";

        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);

        SqlParameter publication_idParameterInsert2 = new SqlParameter();
        publication_idParameterInsert2.ParameterName = "@publication_id";
        publication_idParameterInsert2.SqlDbType = SqlDbType.Int;
        publication_idParameterInsert2.Value = Convert.ToInt32(pubId);
        commandCnt.Parameters.Add(publication_idParameterInsert2);

        conn.Open();
        int cnt = (int)commandCnt.ExecuteScalar();
        conn.Close();
        if (cnt > 0)
        {
            return;
        }

        sqlStatement =
            "IF NOT EXISTS(SELECT pmid FROM reject_publication WHERE pmid = (select pmid from publication where publication_id = @publication_id))" +
            " insert into reject_publication (pmid) values ((select pmid from publication where publication_id = @publication_id))";

        SqlCommand commandInsert = new SqlCommand(sqlStatement, conn);

        SqlParameter publication_idParameterInsert = new SqlParameter();
        publication_idParameterInsert.ParameterName = "@publication_id";
        publication_idParameterInsert.SqlDbType = SqlDbType.Int;
        publication_idParameterInsert.Value = Convert.ToInt32(pubId);
        commandInsert.Parameters.Add(publication_idParameterInsert);

        conn.Open();
        commandInsert.ExecuteNonQuery();
        conn.Close();
    }
    public static void UpdateFinalConfirm(int pubId, int confirmId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        sqlStatement = "update publication_processing" +
            " set final_confirm_id = " +
            confirmId.ToString() +
            " where publication_id = " +
            pubId.ToString();

        SqlCommand command2 = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command2.ExecuteNonQuery();
        conn.Close();
    }
    public static void ProcessAuthorList(List<int> authorList)
    {
        int total = authorList.Count;
        int confirmedCnt = 0;
        int tempConfirmedCnt = 0;
        int conflictCnt = 0;
        foreach (int authorId in authorList)
        {
            ProcessPub.ClientConfirm c = new ProcessPub.ClientConfirm();
            c = ProcessPub.ConfirmOneAuthor(authorId);

            if (c.clientId == 0)
            {
                GiveClientIdConfirmIdToAuthor(authorId, 0, 2);
                conflictCnt++;
            }
            else if (c.confirmId == 1)
            {
                GiveClientIdConfirmIdToAuthor(authorId, c.clientId, 1);
                confirmedCnt++;
            }
            else if (c.confirmId == 2)
            {
                GiveClientIdConfirmIdToAuthor(authorId, 0, 2);
                conflictCnt++;
            }
            else if (c.confirmId == 4)
            {
                GiveClientIdConfirmIdToAuthor(authorId, c.clientId, 4);
                tempConfirmedCnt++;
            }
            else
            {
                GiveClientIdConfirmIdToAuthor(authorId, 0, 3);
            }
        }

        /*
        string msg = "Among " + total.ToString() + " authors, " +
            confirmedCnt +
            " initial(s) are confirmed, " +
            tempConfirmedCnt +
            " initial(s) are temp confirmed, " +
            conflictCnt.ToString() +
            " initial(s) are conflict.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
         * */
    }
    public static void GiveClientIdConfirmIdToAuthor(int authorId, int clientId, int confirmId)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        if (clientId == 0)
        {
            sqlStatement = "update author" +
                " set confirm_id = " +
                confirmId.ToString() +
                " where author_id = " +
                authorId.ToString();
        }
        else
        {
            sqlStatement = "update author" +
                " set client_id = " +
                clientId.ToString() +
                ", confirm_id = " +
                confirmId.ToString() +
                " where author_id = " +
                authorId.ToString();
        }
        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();
    }
}