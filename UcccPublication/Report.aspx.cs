using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public partial class Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Display();
    }
    protected void Display()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        object csvFileObj = Request["csvFile"];
        string csvFile = "";
        if (csvFileObj != null)
        {
            csvFile = csvFileObj.ToString() + ".csv";
        }
        object resIdObj = Request["resId"];
        string resId = "";
        if (resIdObj != null)
        {
            resId = resIdObj.ToString();
        }
        string startDate = Request["startDate"].ToString();
        string endDate = Request["endDate"].ToString();
        string programIdStr = "";
        int programId = 0;
        string clientIdStr = "";
        int clientId = 0;
        string maxGroupStr = "";
        int maxGroupInt = 0;
        if (Request["programId"] != null)
        {
            programIdStr = Request["programId"].ToString();
            programId = Convert.ToInt32(programIdStr);
            DisplayForProgram(programId);
            return;
        }
        if (Request["clientId"] != null)
        {
            //clientIdStr = Request["clientId"].ToString();
            //clientId = Convert.ToInt32(clientIdStr);
            DisplayForMember();
            return;
        }
        if (Request["maxGroup"] != null)
        {
            maxGroupStr = Request["maxGroup"].ToString();
            maxGroupInt = Convert.ToInt32(maxGroupStr);
        }

        string programName = "";
        string programStr = "";
        string member = "";
        if (programIdStr != "")
        {
            sqlStatement = "select" +
                " program_name, abbreviation as program from l_program where l_program_id = " + programIdStr;

            SqlCommand commandProgram = new SqlCommand(sqlStatement, conn);
            conn.Open();
            SqlDataReader readerProgram;
            readerProgram = commandProgram.ExecuteReader();
            try
            {
                while (readerProgram.Read())
                {
                    programName = readerProgram["program_name"].ToString();
                    programStr = readerProgram["program"].ToString();
                }
            }
            finally
            {
                readerProgram.Close();
                conn.Close();
            }
        }
        else if (clientIdStr != "")
        {
            sqlStatement =
                "select first_name + ' ' + last_name as member" +
                " from client where client_id = " +
                clientIdStr;
            SqlCommand myCommandName = new SqlCommand(sqlStatement, conn);
            conn.Open();
            member = (string)myCommandName.ExecuteScalar();
            conn.Close();
        }

        string startYearStr = startDate.Substring(startDate.Length - 4);
        string endYearStr = endDate.Substring(endDate.Length - 4);
        List<int> yearList = new List<int>();
        int endYear = Convert.ToInt32(endYearStr);
        int startYear = Convert.ToInt32(startYearStr);
        int theyear = endYear;
        yearList.Add(theyear);
        theyear--;
        while (theyear >= startYear)
        {
            yearList.Add(theyear);
            theyear--;
        }

        int total = GetTotal(programIdStr, clientIdStr, maxGroupInt, startDate, endDate);

        //programmatic info
        int inter = ProgrammaticCnt(1, programId, startDate, endDate);
        int intra = ProgrammaticCnt(2, programId, startDate, endDate);
        int inter_intra = ProgrammaticCnt(3, programId, startDate, endDate);
        int totalCollaborative = inter + intra + inter_intra;

        double interPercentage = (double)inter / (double)total;
        interPercentage = interPercentage * 1000;
        interPercentage = Math.Round(interPercentage);
        interPercentage = interPercentage / 10;

        double intraPercentage = (double)intra / (double)total;
        intraPercentage = intraPercentage * 1000;
        intraPercentage = Math.Round(intraPercentage);
        intraPercentage = intraPercentage / 10;

        double inter_intraPercentage = (double)inter_intra / (double)total;
        inter_intraPercentage = inter_intraPercentage * 1000;
        inter_intraPercentage = Math.Round(inter_intraPercentage);
        inter_intraPercentage = inter_intraPercentage / 10;

        double totalCPerc = interPercentage + intraPercentage + inter_intraPercentage;

        string datePeriod = startDate + " - " + endDate;
        string totalStr = total.ToString();
        string TotalCollabPubStr = totalCollaborative.ToString() + " (" + totalCPerc + "%)";

        Table tblTopTable1 = new Table();
        TableRow trTopTable1_r1 = new TableRow();
        TableCell tcTopTable1_r1_c1 = new TableCell();
        tcTopTable1_r1_c1.HorizontalAlign = HorizontalAlign.Center;
        Label lblTopTitle1 = new Label();
        //tcSymbol.Width = 40;
        lblTopTitle1.Font.Size = new FontUnit(10);
        lblTopTitle1.Font.Name = "Arial";
        if (programIdStr != "")
        {
            lblTopTitle1.Text = "University of Colorado Cancer Center" +
                "<br />" +
                programName +
                " Program Publications " +
                "<br />" +
                startDate + " - " + endDate;
        }
        else if (clientIdStr != "")
        {
            lblTopTitle1.Text = "University of Colorado Cancer Center" +
                "<br />" +
                " Member Publications " +
                "<br />" +
                startDate + " - " + endDate;

        }
        else if (resId != "")
        {
            sqlStatement =
                "select description as resource" +
                " from l_resource where l_resource_id = " +
                resId.ToString();

            SqlCommand myCommandName = new SqlCommand(sqlStatement, conn);
            conn.Open();
            string resource = (string)myCommandName.ExecuteScalar();
            conn.Close();
            lblTopTitle1.Text = "University of Colorado Cancer Center" +
                "<br />" +
                resource +
                " Publications " +
                "<br />" +
                startDate + " - " + endDate;
        }
        else
        {
            lblTopTitle1.Text = "University of Colorado Cancer Center" +
                "<br />" +
                " Publications " +
                "<br />" +
                startDate + " - " + endDate;
        }

        tcTopTable1_r1_c1.Controls.Add(lblTopTitle1);
        trTopTable1_r1.Cells.Add(tcTopTable1_r1_c1);
        tblTopTable1.Rows.Add(trTopTable1_r1);

        TableRow trTopTable1_r2 = new TableRow();
        TableCell tcTopTable1_r1_c2 = new TableCell();
        Label lblSelectedPublications = new Label();
        if (resId == "")
        {
            lblSelectedPublications.Text = "Selected Publications";
            lblSelectedPublications.Font.Size = new FontUnit(10);
            lblSelectedPublications.Font.Name = "Arial";
            lblSelectedPublications.Font.Underline = true;
            lblSelectedPublications.Font.Bold = true;
        }
        tcTopTable1_r1_c2.Controls.Add(lblSelectedPublications);

        Label lblTopTitle2 = new Label();
        lblTopTitle2.Font.Size = new FontUnit(10);
        lblTopTitle2.Font.Name = "Arial";
        if (programIdStr != "")
        {
            if (maxGroupInt != 0) //not All, not "No focus group only"
            {
                lblTopTitle2.Text =
                    "<br />The " +
                    programName +
                    " program produced a total of " +
                    totalStr +
                    " cancer-related publications during the " +
                    startYearStr + " - " + endYearStr +
                    " reporting period of which " +
                    TotalCollabPubStr +
                    " were either inter- and/or intra--programmatic collaborations." +
                    " The following list is selected to highlight the research conducted by each focus group.";
            }
            else if (maxGroupInt == 0) //all - even no focus group
            {
                lblTopTitle2.Text =
                    "<br />The " +
                    programName +
                    " program produced a total of " +
                    totalStr +
                    " cancer-related publications during the " +
                    startYearStr + " - " + endYearStr +
                    " reporting period of which " +
                    TotalCollabPubStr +
                    " were either inter- and/or intra--programmatic collaborations."+
                    " The following list is selected to highlight the research conducted by focus group 0.";
            }
        }
        else if (clientIdStr != "")
        {
            lblTopTitle2.Text =
                "<br />Cancer Center member " +
                member +
                " published a total of " +
                totalStr +
                " cancer-related publications during the " +
                startYearStr + " - " + endYearStr +
                " reporting period. ";
        }
        else if (csvFile != "")
        {
            lblTopTitle2.Text =
                "<br />Cancer Center member published cancer-related publications during the " +
                startYearStr + " - " + endYearStr +
                " reporting period. ";
        }
        else if (resId != "")
        {
            lblTopTitle2.Text = "";
            /*
                "<br />Shared resource is used by publications during the " +
                startYearStr + " - " + endYearStr +
                " reporting period. ";
             * */
        }
        else
        {
            lblTopTitle2.Text =
                "<br />Cancer Center published a total of " +
                totalStr +
                " cancer-related publications during the " +
                startDate + " - " + endDate +
                " time period. ";
        }

        tcTopTable1_r1_c2.HorizontalAlign = HorizontalAlign.Left;
        tcTopTable1_r1_c2.Controls.Add(lblTopTitle2);
        trTopTable1_r2.Cells.Add(tcTopTable1_r1_c2);
        tblTopTable1.Rows.Add(trTopTable1_r2);

        topPlaceholder.Controls.Add(tblTopTable1);

        if (csvFile == "" && resId == "")
        {
            Table tblTopTable2 = new Table();
            tblTopTable2.BorderWidth = 1;


            TableRow trTopTable2_r1 = new TableRow();
            trTopTable2_r1.BackColor = System.Drawing.Color.Silver;

            TableCell tcTopTable2_r1_c1 = new TableCell();
            tcTopTable2_r1_c1.ColumnSpan = 5;

            tcTopTable2_r1_c1.Text = "Cancer Related Publications (N = " + totalStr + ")";
            tcTopTable2_r1_c1.Font.Size = new FontUnit(10);
            tcTopTable2_r1_c1.Font.Name = "Arial";
            tcTopTable2_r1_c1.HorizontalAlign = HorizontalAlign.Center;
            trTopTable2_r1.Cells.Add(tcTopTable2_r1_c1);

            tblTopTable2.Rows.Add(trTopTable2_r1);

            TableRow trTopTable2_r2 = new TableRow();
            trTopTable2_r2.BackColor = System.Drawing.Color.Silver;

            TableCell tcTopTable2_r2_c1 = new TableCell();
            tcTopTable2_r2_c1.Text = "Reporting Period";
            tcTopTable2_r2_c1.Font.Name = "Arial";
            tcTopTable2_r2_c1.Font.Size = new FontUnit(10);
            tcTopTable2_r2_c1.HorizontalAlign = HorizontalAlign.Center;
            trTopTable2_r2.Cells.Add(tcTopTable2_r2_c1);

            TableCell tcTopTable2_r2_c2 = new TableCell();
            tcTopTable2_r2_c2.Text = "Inter-Programmatic (*)";
            tcTopTable2_r2_c2.Font.Name = "Arial";
            tcTopTable2_r2_c2.Font.Size = new FontUnit(10);
            tcTopTable2_r2_c2.HorizontalAlign = HorizontalAlign.Center;
            trTopTable2_r2.Cells.Add(tcTopTable2_r2_c2);

            TableCell tcTopTable2_r2_c3 = new TableCell();
            tcTopTable2_r2_c3.Text = "Intra-Programmatic (+)";
            tcTopTable2_r2_c3.Font.Name = "Arial";
            tcTopTable2_r2_c3.Font.Size = new FontUnit(10);
            tcTopTable2_r2_c3.HorizontalAlign = HorizontalAlign.Center;
            trTopTable2_r2.Cells.Add(tcTopTable2_r2_c3);

            TableCell tcTopTable2_r2_c4 = new TableCell();
            tcTopTable2_r2_c4.Text = "Inter-/Intra-Programmatic (*+)";
            tcTopTable2_r2_c4.Font.Name = "Arial";
            tcTopTable2_r2_c4.Font.Size = new FontUnit(10);
            tcTopTable2_r2_c4.HorizontalAlign = HorizontalAlign.Center;
            trTopTable2_r2.Cells.Add(tcTopTable2_r2_c4);

            TableCell tcTopTable2_r2_c5 = new TableCell();
            tcTopTable2_r2_c5.Text = "Total Collaborative Publications";
            tcTopTable2_r2_c5.Font.Name = "Arial";
            tcTopTable2_r2_c5.Font.Size = new FontUnit(10);
            tcTopTable2_r2_c5.HorizontalAlign = HorizontalAlign.Center;
            trTopTable2_r2.Cells.Add(tcTopTable2_r2_c5);

            tblTopTable2.Rows.Add(trTopTable2_r2);

            TableRow trTopTable2_r3 = new TableRow();

            TableCell tcTopTable2_r3_c1 = new TableCell();
            Label lblDatePeriod = new Label();
            lblDatePeriod.Text = datePeriod;
            lblDatePeriod.Font.Name = "Arial";
            lblDatePeriod.Font.Size = new FontUnit(10);
            tcTopTable2_r3_c1.Controls.Add(lblDatePeriod);
            trTopTable2_r3.Cells.Add(tcTopTable2_r3_c1);

            TableCell tcTopTable2_r3_c2 = new TableCell();
            Label lblInter2 = new Label();
            lblInter2.Text = inter.ToString() + " (" + interPercentage + "%)";
            lblInter2.Font.Name = "Arial";
            lblInter2.Font.Size = new FontUnit(10);
            tcTopTable2_r3_c2.HorizontalAlign = HorizontalAlign.Center;
            tcTopTable2_r3_c2.Controls.Add(lblInter2);
            trTopTable2_r3.Cells.Add(tcTopTable2_r3_c2);

            TableCell tcTopTable2_r3_c3 = new TableCell();
            Label lblIntra2 = new Label();
            lblIntra2.Text = intra.ToString() + " (" + intraPercentage + "%)";
            lblIntra2.Font.Name = "Arial";
            lblIntra2.Font.Size = new FontUnit(10);
            tcTopTable2_r3_c3.HorizontalAlign = HorizontalAlign.Center;
            tcTopTable2_r3_c3.Controls.Add(lblIntra2);
            trTopTable2_r3.Cells.Add(tcTopTable2_r3_c3);

            TableCell tcTopTable2_r3_c4 = new TableCell();
            Label lblInterIntra2 = new Label();
            lblInterIntra2.Text = inter_intra.ToString() + " (" + inter_intraPercentage + "%)";
            lblInterIntra2.Font.Name = "Arial";
            lblInterIntra2.Font.Size = new FontUnit(10);
            tcTopTable2_r3_c4.HorizontalAlign = HorizontalAlign.Center;
            tcTopTable2_r3_c4.Controls.Add(lblInterIntra2);
            trTopTable2_r3.Cells.Add(tcTopTable2_r3_c4);

            TableCell tcTopTable2_r3_c5 = new TableCell();
            Label lblTotalColl = new Label();
            lblTotalColl.Text = TotalCollabPubStr.ToString();
            lblTotalColl.Font.Name = "Arial";
            lblTotalColl.Font.Size = new FontUnit(10);
            tcTopTable2_r3_c5.HorizontalAlign = HorizontalAlign.Center;
            tcTopTable2_r3_c5.Controls.Add(lblTotalColl);
            trTopTable2_r3.Cells.Add(tcTopTable2_r3_c5);

            tblTopTable2.Rows.Add(trTopTable2_r3);

            topPlaceholder.Controls.Add(tblTopTable2);
            lblExclusiveNote.Font.Name = "Arial";
            lblExclusiveNote.Font.Size = new FontUnit(10);
            lblExclusiveNote.Text = "Note: Programmatic categories are exclusive.";
        }

        int groupCnt = 0;
        if (maxGroupInt != -1) // not "No focus group only"
        {
            groupCnt = maxGroupInt + 1;
        }
        else
        {
            groupCnt = 1;
        }
        string focusGroup = "";
        for (int j = 0; j < groupCnt; j++)
        //for (int j = 1; j < groupCnt; j++)
        {
            int groupNum = (j + 1) % groupCnt;
            if (groupCnt > 1)
            {
                if (groupNum == 0)
                {
                    break;
                }
            }
            //int groupNum = j;
            if (programIdStr != "")
            {
                sqlStatement = "select" +
                    " description as focus_group from l_focus_group" +
                    " where l_program_id = " +
                    programIdStr +
                    " and group_number = " +
                    groupNum.ToString();
                SqlCommand command = new SqlCommand(sqlStatement, conn);
                conn.Open();
                focusGroup = (string)command.ExecuteScalar();
                conn.Close();
            }

            Label lblGroup = new Label();
            lblGroup.Font.Name = "Arial";
            lblGroup.Font.Italic = true;
            lblGroup.Font.Bold = true;
            lblGroup.Font.Underline = true;
            //lblGroup.Text = "<u>Focus Group " + groupNum.ToString() + " - " + focusGroup + "</u>";
            lblGroup.Text = "Focus Group " + groupNum.ToString() + " - " + focusGroup;
            GridViewPlaceHolder.Controls.Add(lblGroup);

            DivCtrl theDiv = new DivCtrl();
            theDiv.Height = 10;
            GridViewPlaceHolder.Controls.Add(theDiv);

            string wAbstract = "0";
            if (Request["wAbstract"] != null)
            {
                wAbstract = Request["wAbstract"].ToString();
            }
            bool hasRecord = false;
            foreach (int thisyear in yearList)
            {
                string insideStartDate = "01/01/" + thisyear.ToString();
                string insideEndDate = "12/31/" + thisyear;
                if (thisyear == startYear)
                {
                    insideStartDate = startDate;
                }
                if (thisyear == endYear)
                {
                    insideEndDate = endDate;
                }

                if (csvFile != "")
                {
                    List<string> pmidList = new List<string>();
                    pmidList = ReadCVS(Server.MapPath("~/upload/"), csvFile);
                    sqlStatement = MakeSqlStatementForPmidList(pmidList, insideStartDate, insideEndDate);
                    footNote.Visible = false;
                }
                else if (resId != "")
                {
                    int resourceId = Convert.ToInt32(resId);
                    sqlStatement = MakeSqlStatementForSharedResource(resourceId, insideStartDate, insideEndDate);
                    footNote.Visible = false;
                }
                else
                {
                    if (programIdStr != "")
                    {
                        sqlStatement = MakeSqlStatementForProgram(programId, groupNum, maxGroupInt, wAbstract, insideStartDate, insideEndDate);
                    }
                    else if (clientIdStr != "")
                    {
                        sqlStatement = MakeSqlStatementForMember(j, wAbstract, insideStartDate, insideEndDate);
                    }
                    else
                    {
                        sqlStatement = MakeSqlStatementForAll(insideStartDate, insideEndDate);
                    }
                }

                SqlCommand myCommand2 = new SqlCommand(sqlStatement, conn);
                conn.Open();
                SqlDataReader myReader;
                myReader = myCommand2.ExecuteReader();
                Label lblThisYear = new Label();
                lblThisYear.Font.Name = "Arial";
                lblThisYear.Font.Italic = true;
                lblThisYear.Font.Bold = true;
                lblThisYear.Font.Underline = true;
                lblThisYear.Text = thisyear.ToString();
                Table tbl = new Table();
                if (myReader.HasRows)
                {
                    GridViewPlaceHolder.Controls.Add(lblThisYear);
                    GridViewPlaceHolder.Controls.Add(tbl);
                    hasRecord = true;
                }
                try
                {
                    while (myReader.Read())
                    {
                        string symbolStr = myReader["symbol"].ToString();
                        if (csvFile != "" || resId != "")
                        {
                            symbolStr = "";
                        }
                        string publicationStr = myReader["publication"].ToString();

                        TableRow tr = new TableRow();
                        tr.VerticalAlign = VerticalAlign.Top;

                        TableCell tcSymbol = new TableCell();
                        tcSymbol.Width = 40;
                        Label lblSymbol = new Label();
                        lblSymbol.Font.Size = new FontUnit(10);
                        lblSymbol.Font.Name = "Arial";
                        lblSymbol.Text = symbolStr;
                        tcSymbol.Controls.Add(lblSymbol);
                        tcSymbol.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(tcSymbol);

                        TableCell tcPublication = new TableCell();
                        Label lblPublication = new Label();
                        lblPublication.Font.Size = new FontUnit(10);
                        lblPublication.Font.Name = "Arial";
                        if (programIdStr != "")
                        {
                            //string newProgramStr = "<u>" + programStr + "</u>";
                            string newProgramStr = programStr;
                            publicationStr = publicationStr.Replace(programStr, newProgramStr);
                        }
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
            if (!hasRecord || csvFile != "" || resId != "")
            {
                GridViewPlaceHolder.Controls.Remove(lblGroup);
            }
            DivCtrl theDiv2 = new DivCtrl();
            theDiv2.Height = 10;
            GridViewPlaceHolder.Controls.Add(theDiv2);

        }
    }
    protected void DisplayForProgram(int programId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        string startDate = Request["startDate"].ToString();
        string endDate = Request["endDate"].ToString();
        string maxGroupStr = "";
        int maxGroupInt = 0;
        if (Request["maxGroup"] != null)
        {
            maxGroupStr = Request["maxGroup"].ToString();
            maxGroupInt = Convert.ToInt32(maxGroupStr);
        }

        string programName = "";
        string programStr = "";
        sqlStatement = "select" +
            " program_name, abbreviation as program from l_program where l_program_id = " + programId.ToString();

        SqlCommand commandProgram = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader readerProgram;
        readerProgram = commandProgram.ExecuteReader();
        try
        {
            while (readerProgram.Read())
            {
                programName = readerProgram["program_name"].ToString();
                programStr = readerProgram["program"].ToString();
            }
        }
        finally
        {
            readerProgram.Close();
            conn.Close();
        }

        string startYearStr = startDate.Substring(startDate.Length - 4);
        string endYearStr = endDate.Substring(endDate.Length - 4);
        List<int> yearList = new List<int>();
        int endYear = Convert.ToInt32(endYearStr);
        int startYear = Convert.ToInt32(startYearStr);
        int theyear = endYear;
        yearList.Add(theyear);
        theyear--;
        while (theyear >= startYear)
        {
            yearList.Add(theyear);
            theyear--;
        }

        int total = GetTotalForProgram(programId, maxGroupInt, startDate, endDate);

        //programmatic info
        int inter = ProgrammaticCnt(1, programId, startDate, endDate);
        int intra = ProgrammaticCnt(2, programId, startDate, endDate);
        int inter_intra = ProgrammaticCnt(3, programId, startDate, endDate);
        int totalCollaborative = inter + intra + inter_intra;

        double interPercentage = (double)inter / (double)total;
        interPercentage = interPercentage * 1000;
        interPercentage = Math.Round(interPercentage);
        interPercentage = interPercentage / 10;

        double intraPercentage = (double)intra / (double)total;
        intraPercentage = intraPercentage * 1000;
        intraPercentage = Math.Round(intraPercentage);
        intraPercentage = intraPercentage / 10;

        double inter_intraPercentage = (double)inter_intra / (double)total;
        inter_intraPercentage = inter_intraPercentage * 1000;
        inter_intraPercentage = Math.Round(inter_intraPercentage);
        inter_intraPercentage = inter_intraPercentage / 10;

        double totalCPerc = interPercentage + intraPercentage + inter_intraPercentage;

        string datePeriod = startDate + " - " + endDate;
        string totalStr = total.ToString();
        string TotalCollabPubStr = totalCollaborative.ToString() + " (" + totalCPerc + "%)";

        //top table
        Table tblTopTable1 = new Table();
        TableRow trTopTable1_r1 = new TableRow();
        TableCell tcTopTable1_r1_c1 = new TableCell();
        tcTopTable1_r1_c1.HorizontalAlign = HorizontalAlign.Center;
        Label lblTopTitle1 = new Label();
        //tcSymbol.Width = 40;
        lblTopTitle1.Font.Size = new FontUnit(10);
        lblTopTitle1.Font.Name = "Arial";
        lblTopTitle1.Text = "University of Colorado Cancer Center" +
            "<br />" +
            programName +
            " Program Publications " +
            "<br />" +
            startDate + " - " + endDate;

        tcTopTable1_r1_c1.Controls.Add(lblTopTitle1);
        trTopTable1_r1.Cells.Add(tcTopTable1_r1_c1);
        tblTopTable1.Rows.Add(trTopTable1_r1);

        TableRow trTopTable1_r2 = new TableRow();
        TableCell tcTopTable1_r1_c2 = new TableCell();
        Label lblSelectedPublications = new Label();
        lblSelectedPublications.Text = "Selected Publications";
        lblSelectedPublications.Font.Size = new FontUnit(10);
        lblSelectedPublications.Font.Name = "Arial";
        lblSelectedPublications.Font.Underline = true;
        lblSelectedPublications.Font.Bold = true;
        tcTopTable1_r1_c2.Controls.Add(lblSelectedPublications);

        Label lblTopTitle2 = new Label();
        lblTopTitle2.Font.Size = new FontUnit(10);
        lblTopTitle2.Font.Name = "Arial";
        if (maxGroupInt != 0) //not All, not "No focus group only"
        {
            lblTopTitle2.Text =
                "<br />The " +
                programName +
                " program produced a total of " +
                totalStr +
                " cancer-related publications during the " +
                startYearStr + " - " + endYearStr +
                " reporting period of which " +
                TotalCollabPubStr +
                " were either inter- and/or intra--programmatic collaborations." +
                " The following list is selected to highlight the research conducted by each focus group.";
        }
        else if (maxGroupInt == 0) //all - even no focus group
        {
            lblTopTitle2.Text =
                "<br />The " +
                programName +
                " program produced a total of " +
                totalStr +
                " cancer-related publications during the " +
                startYearStr + " - " + endYearStr +
                " reporting period of which " +
                TotalCollabPubStr +
                " were either inter- and/or intra--programmatic collaborations." +
                " The following list is selected to highlight the research conducted by focus group 0.";
        }

        tcTopTable1_r1_c2.HorizontalAlign = HorizontalAlign.Left;
        tcTopTable1_r1_c2.Controls.Add(lblTopTitle2);
        trTopTable1_r2.Cells.Add(tcTopTable1_r1_c2);
        tblTopTable1.Rows.Add(trTopTable1_r2);

        topPlaceholder.Controls.Add(tblTopTable1);

        Table tblTopTable2 = new Table();
        tblTopTable2.BorderWidth = 1;


        TableRow trTopTable2_r1 = new TableRow();
        trTopTable2_r1.BackColor = System.Drawing.Color.Silver;

        TableCell tcTopTable2_r1_c1 = new TableCell();
        tcTopTable2_r1_c1.ColumnSpan = 5;

        tcTopTable2_r1_c1.Text = "Cancer Related Publications (N = " + totalStr + ")";
        tcTopTable2_r1_c1.Font.Size = new FontUnit(10);
        tcTopTable2_r1_c1.Font.Name = "Arial";
        tcTopTable2_r1_c1.HorizontalAlign = HorizontalAlign.Center;
        trTopTable2_r1.Cells.Add(tcTopTable2_r1_c1);

        tblTopTable2.Rows.Add(trTopTable2_r1);

        TableRow trTopTable2_r2 = new TableRow();
        trTopTable2_r2.BackColor = System.Drawing.Color.Silver;

        TableCell tcTopTable2_r2_c1 = new TableCell();
        tcTopTable2_r2_c1.Text = "Reporting Period";
        tcTopTable2_r2_c1.Font.Name = "Arial";
        tcTopTable2_r2_c1.Font.Size = new FontUnit(10);
        tcTopTable2_r2_c1.HorizontalAlign = HorizontalAlign.Center;
        trTopTable2_r2.Cells.Add(tcTopTable2_r2_c1);

        TableCell tcTopTable2_r2_c2 = new TableCell();
        tcTopTable2_r2_c2.Text = "Inter-Programmatic (*)";
        tcTopTable2_r2_c2.Font.Name = "Arial";
        tcTopTable2_r2_c2.Font.Size = new FontUnit(10);
        tcTopTable2_r2_c2.HorizontalAlign = HorizontalAlign.Center;
        trTopTable2_r2.Cells.Add(tcTopTable2_r2_c2);

        TableCell tcTopTable2_r2_c3 = new TableCell();
        tcTopTable2_r2_c3.Text = "Intra-Programmatic (+)";
        tcTopTable2_r2_c3.Font.Name = "Arial";
        tcTopTable2_r2_c3.Font.Size = new FontUnit(10);
        tcTopTable2_r2_c3.HorizontalAlign = HorizontalAlign.Center;
        trTopTable2_r2.Cells.Add(tcTopTable2_r2_c3);

        TableCell tcTopTable2_r2_c4 = new TableCell();
        tcTopTable2_r2_c4.Text = "Inter-/Intra-Programmatic (*+)";
        tcTopTable2_r2_c4.Font.Name = "Arial";
        tcTopTable2_r2_c4.Font.Size = new FontUnit(10);
        tcTopTable2_r2_c4.HorizontalAlign = HorizontalAlign.Center;
        trTopTable2_r2.Cells.Add(tcTopTable2_r2_c4);

        TableCell tcTopTable2_r2_c5 = new TableCell();
        tcTopTable2_r2_c5.Text = "Total Collaborative Publications";
        tcTopTable2_r2_c5.Font.Name = "Arial";
        tcTopTable2_r2_c5.Font.Size = new FontUnit(10);
        tcTopTable2_r2_c5.HorizontalAlign = HorizontalAlign.Center;
        trTopTable2_r2.Cells.Add(tcTopTable2_r2_c5);

        tblTopTable2.Rows.Add(trTopTable2_r2);

        TableRow trTopTable2_r3 = new TableRow();

        TableCell tcTopTable2_r3_c1 = new TableCell();
        Label lblDatePeriod = new Label();
        lblDatePeriod.Text = datePeriod;
        lblDatePeriod.Font.Name = "Arial";
        lblDatePeriod.Font.Size = new FontUnit(10);
        tcTopTable2_r3_c1.Controls.Add(lblDatePeriod);
        trTopTable2_r3.Cells.Add(tcTopTable2_r3_c1);

        TableCell tcTopTable2_r3_c2 = new TableCell();
        Label lblInter2 = new Label();
        lblInter2.Text = inter.ToString() + " (" + interPercentage + "%)";
        lblInter2.Font.Name = "Arial";
        lblInter2.Font.Size = new FontUnit(10);
        tcTopTable2_r3_c2.HorizontalAlign = HorizontalAlign.Center;
        tcTopTable2_r3_c2.Controls.Add(lblInter2);
        trTopTable2_r3.Cells.Add(tcTopTable2_r3_c2);

        TableCell tcTopTable2_r3_c3 = new TableCell();
        Label lblIntra2 = new Label();
        lblIntra2.Text = intra.ToString() + " (" + intraPercentage + "%)";
        lblIntra2.Font.Name = "Arial";
        lblIntra2.Font.Size = new FontUnit(10);
        tcTopTable2_r3_c3.HorizontalAlign = HorizontalAlign.Center;
        tcTopTable2_r3_c3.Controls.Add(lblIntra2);
        trTopTable2_r3.Cells.Add(tcTopTable2_r3_c3);

        TableCell tcTopTable2_r3_c4 = new TableCell();
        Label lblInterIntra2 = new Label();
        lblInterIntra2.Text = inter_intra.ToString() + " (" + inter_intraPercentage + "%)";
        lblInterIntra2.Font.Name = "Arial";
        lblInterIntra2.Font.Size = new FontUnit(10);
        tcTopTable2_r3_c4.HorizontalAlign = HorizontalAlign.Center;
        tcTopTable2_r3_c4.Controls.Add(lblInterIntra2);
        trTopTable2_r3.Cells.Add(tcTopTable2_r3_c4);

        TableCell tcTopTable2_r3_c5 = new TableCell();
        Label lblTotalColl = new Label();
        lblTotalColl.Text = TotalCollabPubStr.ToString();
        lblTotalColl.Font.Name = "Arial";
        lblTotalColl.Font.Size = new FontUnit(10);
        tcTopTable2_r3_c5.HorizontalAlign = HorizontalAlign.Center;
        tcTopTable2_r3_c5.Controls.Add(lblTotalColl);
        trTopTable2_r3.Cells.Add(tcTopTable2_r3_c5);

        tblTopTable2.Rows.Add(trTopTable2_r3);

        topPlaceholder.Controls.Add(tblTopTable2);
        lblExclusiveNote.Font.Name = "Arial";
        lblExclusiveNote.Font.Size = new FontUnit(10);
        lblExclusiveNote.Text = "Note: Programmatic categories are exclusive.";

        int groupCnt = 0;
        if (maxGroupInt != -1) // not "No focus group only"
        {
            groupCnt = maxGroupInt + 1;
        }
        else
        {
            groupCnt = 1;
        }
        string focusGroup = "";
        for (int j = 0; j < groupCnt; j++)
        //for (int j = 1; j < groupCnt; j++)
        {
            int groupNum = (j + 1) % groupCnt;
            if (groupCnt > 1)
            {
                if (groupNum == 0)
                {
                    break;
                }
            }
            //int groupNum = j;
            sqlStatement = "select" +
                " description as focus_group from l_focus_group" +
                " where l_program_id = " +
                programId.ToString() +
                " and group_number = " +
                groupNum.ToString();
            SqlCommand command = new SqlCommand(sqlStatement, conn);
            conn.Open();
            focusGroup = (string)command.ExecuteScalar();
            conn.Close();

            Label lblGroup = new Label();
            lblGroup.Font.Name = "Arial";
            lblGroup.Font.Italic = true;
            lblGroup.Font.Bold = true;
            lblGroup.Font.Underline = true;
            //lblGroup.Text = "<u>Focus Group " + groupNum.ToString() + " - " + focusGroup + "</u>";
            lblGroup.Text = "Focus Group " + groupNum.ToString() + " - " + focusGroup;
            GridViewPlaceHolder.Controls.Add(lblGroup);

            DivCtrl theDiv = new DivCtrl();
            theDiv.Height = 10;
            GridViewPlaceHolder.Controls.Add(theDiv);

            string wAbstract = "0";
            if (Request["wAbstract"] != null)
            {
                wAbstract = Request["wAbstract"].ToString();
            }
            bool hasRecord = false;
            foreach (int thisyear in yearList)
            {
                string insideStartDate = "01/01/" + thisyear.ToString();
                string insideEndDate = "12/31/" + thisyear;
                if (thisyear == startYear)
                {
                    insideStartDate = startDate;
                }
                if (thisyear == endYear)
                {
                    insideEndDate = endDate;
                }
                sqlStatement = MakeSqlStatementForProgram(programId, groupNum, maxGroupInt, wAbstract, insideStartDate, insideEndDate);

                SqlCommand myCommand2 = new SqlCommand(sqlStatement, conn);
                conn.Open();
                SqlDataReader myReader;
                myReader = myCommand2.ExecuteReader();
                Label lblThisYear = new Label();
                lblThisYear.Font.Name = "Arial";
                lblThisYear.Font.Italic = true;
                lblThisYear.Font.Bold = true;
                lblThisYear.Font.Underline = true;
                lblThisYear.Text = thisyear.ToString();
                Table tbl = new Table();
                if (myReader.HasRows)
                {
                    GridViewPlaceHolder.Controls.Add(lblThisYear);
                    GridViewPlaceHolder.Controls.Add(tbl);
                    hasRecord = true;
                }
                try
                {
                    while (myReader.Read())
                    {
                        string symbolStr = myReader["symbol"].ToString();
                        string publicationStr = myReader["publication"].ToString();

                        TableRow tr = new TableRow();
                        tr.VerticalAlign = VerticalAlign.Top;

                        TableCell tcSymbol = new TableCell();
                        tcSymbol.Width = 40;
                        Label lblSymbol = new Label();
                        lblSymbol.Font.Size = new FontUnit(10);
                        lblSymbol.Font.Name = "Arial";
                        lblSymbol.Text = symbolStr;
                        tcSymbol.Controls.Add(lblSymbol);
                        tcSymbol.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(tcSymbol);

                        TableCell tcPublication = new TableCell();
                        Label lblPublication = new Label();
                        lblPublication.Font.Size = new FontUnit(10);
                        lblPublication.Font.Name = "Arial";
                        if (programId.ToString() != "")
                        {
                            //string newProgramStr = "<u>" + programStr + "</u>";
                            string newProgramStr = programStr;
                            publicationStr = publicationStr.Replace(programStr, newProgramStr);
                        }
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
            if (!hasRecord)
            {
                GridViewPlaceHolder.Controls.Remove(lblGroup);
            }
            DivCtrl theDiv2 = new DivCtrl();
            theDiv2.Height = 10;
            GridViewPlaceHolder.Controls.Add(theDiv2);

        }
    }
    protected void DisplayForMember()
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        string startDate = Request["startDate"].ToString();
        string endDate = Request["endDate"].ToString();
        string focusGroupStr = "";
        int focusGroup = 0;
        if (Request["maxGroup"] != null)
        {
            focusGroupStr = Request["maxGroup"].ToString();
            focusGroup = Convert.ToInt32(focusGroupStr);
        }
        else
        {
            focusGroup = -1;
        }

        string startYearStr = startDate.Substring(startDate.Length - 4);
        string endYearStr = endDate.Substring(endDate.Length - 4);
        List<int> yearList = new List<int>();
        int endYear = Convert.ToInt32(endYearStr);
        int startYear = Convert.ToInt32(startYearStr);
        int theyear = endYear;
        yearList.Add(theyear);
        theyear--;
        while (theyear >= startYear)
        {
            yearList.Add(theyear);
            theyear--;
        }

        int total = GetTotalForMember(focusGroup, startDate, endDate);

        string datePeriod = startDate + " - " + endDate;
        string totalStr = total.ToString();

        Table tblTopTable1 = new Table();
        TableRow trTopTable1_r1 = new TableRow();
        TableCell tcTopTable1_r1_c1 = new TableCell();
        tcTopTable1_r1_c1.HorizontalAlign = HorizontalAlign.Center;
        Label lblTopTitle1 = new Label();
        //tcSymbol.Width = 40;
        lblTopTitle1.Font.Size = new FontUnit(10);
        lblTopTitle1.Font.Name = "Arial";
        lblTopTitle1.Text = "University of Colorado Cancer Center" +
            "<br />" +
            " Member Publications " +
            "<br />" +
            startDate + " - " + endDate;

        tcTopTable1_r1_c1.Controls.Add(lblTopTitle1);
        trTopTable1_r1.Cells.Add(tcTopTable1_r1_c1);
        tblTopTable1.Rows.Add(trTopTable1_r1);

        TableRow trTopTable1_r2 = new TableRow();
        TableCell tcTopTable1_r1_c2 = new TableCell();
        Label lblSelectedPublications = new Label();
        lblSelectedPublications.Text = "Selected Publications";
        lblSelectedPublications.Font.Size = new FontUnit(10);
        lblSelectedPublications.Font.Name = "Arial";
        lblSelectedPublications.Font.Underline = true;
        lblSelectedPublications.Font.Bold = true;
        tcTopTable1_r1_c2.Controls.Add(lblSelectedPublications);

        ListBox memberList = (ListBox)Session["MEMBERLIST"];
        List<String> lstitems = new List<String>();

        for (int i = 0; i < memberList.Items.Count; i++)
        {
            if (memberList.Items[i].Selected)
                lstitems.Add(memberList.Items[i].Text);
        }

        string memberArray = "";
        memberArray = Helper.ListToArray(lstitems, ": ");
        

        Label lblTopTitle2 = new Label();
        lblTopTitle2.Font.Size = new FontUnit(10);
        lblTopTitle2.Font.Name = "Arial";
        lblTopTitle2.Text =
            "<br />Cancer Center member " +
            //member +
            memberArray +
            " published a total of " +
            totalStr +
            " cancer-related publications during the " +
            startYearStr + " - " + endYearStr +
            " reporting period. ";

        tcTopTable1_r1_c2.HorizontalAlign = HorizontalAlign.Left;
        tcTopTable1_r1_c2.Controls.Add(lblTopTitle2);
        trTopTable1_r2.Cells.Add(tcTopTable1_r1_c2);
        tblTopTable1.Rows.Add(trTopTable1_r2);

        topPlaceholder.Controls.Add(tblTopTable1);

        string wAbstract = "0";
        if (Request["wAbstract"] != null)
        {
            wAbstract = Request["wAbstract"].ToString();
        }
        foreach (int thisyear in yearList)
        {
            string insideStartDate = "01/01/" + thisyear.ToString();
            string insideEndDate = "12/31/" + thisyear;
            if (thisyear == startYear)
            {
                insideStartDate = startDate;
            }
            if (thisyear == endYear)
            {
                insideEndDate = endDate;
            }

            sqlStatement = MakeSqlStatementForMember(focusGroup, wAbstract, insideStartDate, insideEndDate);

            SqlCommand myCommand2 = new SqlCommand(sqlStatement, conn);
            conn.Open();
            SqlDataReader myReader;
            myReader = myCommand2.ExecuteReader();
            Label lblThisYear = new Label();
            lblThisYear.Font.Name = "Arial";
            lblThisYear.Font.Italic = true;
            lblThisYear.Font.Bold = true;
            lblThisYear.Font.Underline = true;
            lblThisYear.Text = thisyear.ToString();
            Table tbl = new Table();
            if (myReader.HasRows)
            {
                GridViewPlaceHolder.Controls.Add(lblThisYear);
                GridViewPlaceHolder.Controls.Add(tbl);
            }
            try
            {
                while (myReader.Read())
                {
                    string symbolStr = "";
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

            DivCtrl theDiv2 = new DivCtrl();
            theDiv2.Height = 10;
            GridViewPlaceHolder.Controls.Add(theDiv2);

        }
        footNote.Visible = false;
    }
    protected int GetTotal(string programIdStr, string clientIdStr, int maxGroupInt, string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        if (programIdStr != "")
        {
            if (maxGroupInt != 0)  //not All, not "No focus group only"
            {
                sqlStatement =
                    "select count(pd.publication_processing_id) from publication_processing pd" +
                    " inner join publication_program pp" +
                    " on pd.publication_id = pp.publication_id" +
                    " and pd.review_editorial is null" +
                    " and pp.l_program_id = " +
                    programIdStr +
                    " where pd.publication_date >= '" +
                    startDate +
                    "' and pd.publication_date <= '" +
                    endDate +
                    "' and pp.l_focus_group_id is not null";
            }
            else if (maxGroupInt == 0) // 0 fg
            {
                sqlStatement =
                    "select count(pd.publication_processing_id) from publication_processing pd" +
                    " inner join publication_program pp" +
                    " on pd.publication_id = pp.publication_id" +
                    " and pd.review_editorial is null" +
                    " and pp.l_program_id = " +
                    programIdStr +
                    " inner join l_focus_group lfg" +
                    " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                    " and lfg.group_number = 0" +
                    " where pd.publication_date >= '" +
                    startDate +
                    "' and pd.publication_date <= '" +
                    endDate +
                    "' and pp.l_focus_group_id is not null";
            }
            else // no fg set
            {
                sqlStatement =
                    "select count(pd.publication_processing_id) from publication_processing pd" +
                    " inner join publication_program pp" +
                    " on pd.publication_id = pp.publication_id" +
                    " and pd.review_editorial is null" +
                    " and pp.l_program_id = " +
                    programIdStr +
                    " where pd.publication_date >= '" +
                    startDate +
                    "' and pd.publication_date <= '" +
                    endDate +
                    "' and pp.l_focus_group_id is null";
            }
        }
        else if (clientIdStr != "")
        {
            sqlStatement =
                "select count(pd.publication_processing_id)" +
                " from publication_author pa" +
                " inner join publication_processing pd" +
                " on pa.publication_id = pd.publication_id" +
                " and pd.review_editorial is null" +
                " inner join author a" +
                " on pa.author_id = a.author_id" +
                " and a.client_id = " +
                clientIdStr +
                " where pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'";
        }
        else
        {
            sqlStatement =
                "select count(pd.publication_processing_id)" +
                " from publication_processing pd" +
                " where pd.review_editorial is null" +
                " and pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'";
        }
        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)myCommand.ExecuteScalar();
        conn.Close();

        return total;
    }
    protected int GetTotalForProgram(int programId, int maxGroupInt, string startDate, string endDate)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;
        if (maxGroupInt != 0)  // set some FG (0 or not 0)
        {
            sqlStatement =
                "select count(pd.publication_processing_id) from publication_processing pd" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pd.review_editorial is null" +
                " and pp.l_program_id = " +
                programId.ToString() +
                " inner join l_focus_group lfg" +
                " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                " and lfg.group_number > 0" +
                " where pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'";
        }
        else if (maxGroupInt == 0) // FG 0
        {
            sqlStatement =
                "select count(pd.publication_processing_id) from publication_processing pd" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pd.review_editorial is null" +
                " and pp.l_program_id = " +
                programId.ToString() +
                " inner join l_focus_group lfg" +
                " on pp.l_focus_group_id = lfg.l_focus_group_id" +
                " and lfg.group_number = 0" +
                " where pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "' and pp.l_focus_group_id is not null";
        }
        else // no FG set
        {
            sqlStatement =
                "select count(pd.publication_processing_id) from publication_processing pd" +
                " inner join publication_program pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pd.review_editorial is null" +
                " and pp.l_program_id = " +
                programId.ToString() +
                " where pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "' and pp.l_focus_group_id is null";
        }
        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)myCommand.ExecuteScalar();
        conn.Close();

        return total;
    }
    protected int GetTotalForMember(int focusGroup, string startDate, string endDate)
    {
        ListBox memberList = (ListBox)Session["MEMBERLIST"];
        List<String> lstitems = new List<String>();

        for (int i = 0; i < memberList.Items.Count; i++)
        {
            if (memberList.Items[i].Selected)
                lstitems.Add(memberList.Items[i].Value);
        }

        string memberArray = "";
        memberArray = Helper.ListToArray(lstitems, ",");
        
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement;

        string focusGroupTerm = "";
        if (focusGroup == 1) //FG non-0
        {
            focusGroupTerm = " and lfg.group_number > 0";
        }
        else if (focusGroup == 0)
        {
            focusGroupTerm = " and lfg.group_number = 0";
        }

        sqlStatement =
            "select count(pd.publication_processing_id)" +
            " from publication_author pa" +
            " inner join publication_processing pd" +
            " on pa.publication_id = pd.publication_id" +
            " and pd.review_editorial is null" +
            " inner join author a" +
            " on pa.author_id = a.author_id" +
            " and a.client_id in (" +
            memberArray +
            ") inner join publication_program pp" +
            " on pd.publication_id = pp.publication_id" +
            " inner join l_focus_group lfg" +
            " on pp.l_focus_group_id = lfg.l_focus_group_id" +
            focusGroupTerm +
            " where pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int total = (int)myCommand.ExecuteScalar();
        conn.Close();

        return total;
    }
    protected string MakeSqlStatementForProgram(int programId, int groupNum, int maxGroupInt, string wAbstract, string insideStartDate, string insideEndDate)
    {
        string sqlStatement = "";

        string wInst = "1";
        //string authorlistName = " process.authorlist +";
        string authorlistName = "authorlist";
        if (Request["wInst"] != null)
        {
            wInst = Request["wInst"].ToString();
            if (wInst == "0")
            {
                //authorlistName = " process.authorlist_no_inst +";
                authorlistName = "authorlist_no_inst";
            }
        }
        string titleField = "p.article_title";
        string wPmid = "0";
        string pmidTerm = "";
        if (Request["wPmid"] != null)
        {
            wPmid = Request["wPmid"].ToString();
            if (wPmid == "1")
            {
                pmidTerm = " 'PMID:' + isnull(convert(varchar,p.pmid),'') + ' ' +";
            }
        }
        string abstractTerm = "";
        if (wAbstract == "1")
        {
            abstractTerm = " + 'Abstract: ' + p.abstract_text + '<br /><br />'";
        }
        string groupTerm = "";
        if (maxGroupInt > 0)
        {
            groupTerm = " and lfg.group_number = " + groupNum.ToString();
        }
        else if (maxGroupInt == 0)
        {
            groupTerm = " and lfg.group_number = 0";
        }
        sqlStatement =
            "select lp.symbol as symbol," +
            " process." +
            authorlistName +
            " +" +
            " '.  ' + isnull(" + titleField + ",'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.MedlinePgn is null then" +
            " 'Epub ahead of print, ' +" +
            " case when p.entrez_year is not null then convert(varchar,p.entrez_year) + '. ' else '' end" +
            " else" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, '') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end" +
            " end +" +
            pmidTerm +
            " isnull(p.pmcid,'') + '<br /><br />'" +
            abstractTerm +
            " as publication" +
            " from publication_processing process" +
            " inner join publication p" +
            " on process.publication_id = p.publication_id" +
            " and process.review_editorial is null" +
            " inner join publication_program pp" +
            " on process.publication_id = pp.publication_id" +
            " and pp.l_program_id = " +
            programId.ToString() +
            " inner join l_focus_group lfg" +
            " on pp.l_focus_group_id = lfg.l_focus_group_id" +
            groupTerm +
            " left outer join publication_programmatic pmm" +
            " on process.publication_id = pmm.publication_id" +
            " and pmm.l_program_id = " +
            programId.ToString() +
            " left outer join l_programmatic lp" +
            " on pmm.l_programmatic_id = lp.l_programmatic_id" +
            " where" +
            " (process.publication_date >= '" +
            insideStartDate +
            "' and process.publication_date <= '" +
            insideEndDate +
            "')" +
            " order by " +
            " replace(" +
            authorlistName +
            ",'<b>','')";

        return sqlStatement;
    }
    protected string MakeSqlStatementForMember(int focusGroup, string wAbstract, string insideStartDate, string insideEndDate)
    {
        string sqlStatement = "";

        string wInst = "1";
        //string authorlistName = " process.authorlist +";
        string authorlistName = "authorlist";
        if (Request["wInst"] != null)
        {
            wInst = Request["wInst"].ToString();
            if (wInst == "0")
            {
                //authorlistName = " process.authorlist_no_inst +";
                authorlistName = "authorlist_no_inst";
            }
        }
        string titleField = "p.article_title";
        string wPmid = "0";
        string pmidTerm = "";
        if (Request["wPmid"] != null)
        {
            wPmid = Request["wPmid"].ToString();
            if (wPmid == "1")
            {
                pmidTerm = " 'PMID:' + isnull(convert(varchar,p.pmid),'') + ' ' +";
            }
        }
        string abstractTerm = "";
        if (wAbstract == "1")
        {
            abstractTerm = " + 'Abstract: ' + p.abstract_text + '<br /><br />'";
        }

        string focusGroupTerm = "";
        if (focusGroup == 1)
        {
            focusGroupTerm = " and lfg.group_number > 0";
        }
        else if (focusGroup == 0)
        {
            focusGroupTerm = " and lfg.group_number = 0";
        }
        ListBox memberList = (ListBox)Session["MEMBERLIST"];
        List<String> lstitems = new List<String>();

        for (int i = 0; i < memberList.Items.Count; i++)
        {
            if (memberList.Items[i].Selected)
                lstitems.Add(memberList.Items[i].Value);
        }

        string memberArray = "";
        memberArray = Helper.ListToArray(lstitems, ",");

        sqlStatement =
            "select distinct" +
            " replace(" +
            authorlistName +
            ",'<b>','') as dummy," +
            " process." +
            authorlistName +
            " +" +
            " '.  ' + isnull(" + titleField + ",'') +" +
            " ' <i>' + REPLACE(isnull(p.ISOAbbreviation,''),'.','') + '</i> ' +" +
            " case when p.MedlinePgn is null then" +
            " 'Epub ahead of print, ' +" +
            " case when p.entrez_year is not null then convert(varchar,p.entrez_year) + '. ' else '' end" +
            " else" +
            " case when p.volume is not null then p.volume + ':' else '' end + " +
            " isnull(p.MedlinePgn, '') + ', ' + " +
            " case when p.pub_year is not null then convert(varchar,p.pub_year) + '. ' else '' end" +
            " end +" +
            pmidTerm +
            " isnull(p.pmcid,'') + '<br /><br />'" +
            abstractTerm +
            " as publication" +
            " from publication_processing process" +
            " inner join publication p" +
            " on process.publication_id = p.publication_id" +
            " and process.review_editorial is null" +
            " inner join publication_author pau" +
            " on process.publication_id = pau.publication_id" +
            " inner join author a" +
            " on pau.author_id = a.author_id" +
            " and a.client_id in (" +
            memberArray +
            //" and a.client_id = " +
            //clientId.ToString() +
            ") inner join publication_program pp" +
            " on p.publication_id = pp.publication_id" +
            " inner join l_focus_group lfg" +
            " on pp.l_focus_group_id = lfg.l_focus_group_id" +
            focusGroupTerm +
            " where" +
            " (process.publication_date >= '" +
            insideStartDate +
            "' and process.publication_date <= '" +
            insideEndDate +
            "')" +
            " order by " +
            " replace(" +
            authorlistName +
            ",'<b>','')";
        return sqlStatement;
    }
    protected string MakeSqlStatementForPmidList(List<string> pmidList, string insideStartDate, string insideEndDate)
    {
        string sqlStatement = "";
        string pmidListStr = string.Join(",", pmidList.Select(n => n.ToString()).ToArray());

        //string authorlistName = " process.authorlist +";
        string authorlistName = "authorlist_no_inst";
        string pmidTerm = "";
        pmidTerm = " 'PMID:' + isnull(convert(varchar,p.pmid),'') + ' ' +";
        sqlStatement =
            "select lp.symbol as symbol," +
            //" process.authorlist +" +
            " process.authorlist_no_inst" +
            " +" +
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
            pmidTerm +
            " isnull(p.pmcid,'') + '<br /><br />' as publication" +
            " from publication_processing process" +
            " inner join publication p" +
            " on process.publication_id = p.publication_id" +
            " and process.review_editorial is null" +
            " left outer join l_programmatic lp" +
            " on process.l_programmatic_id = lp.l_programmatic_id" +
            " where" +
            " p.pmid in (" +
            pmidListStr +
            ") and" +
            " (process.publication_date >= '" +
            insideStartDate +
            "' and process.publication_date <= '" +
            insideEndDate +
            "')" +
            " order by " +
            " replace(" +
            authorlistName +
            ",'<b>','')";
        return sqlStatement;
    }
    protected string MakeSqlStatementForSharedResource(int resourceId, string insideStartDate, string insideEndDate)
    {
        string sqlStatement = "";

        string pmidTerm = "";
        pmidTerm = " 'PMID:' + isnull(convert(varchar,p.pmid),'') + ' ' +";
        sqlStatement =
            "select '' as symbol," +
            " process.authorlist_no_inst" +
            " +" +
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
            pmidTerm +
            " isnull(p.pmcid,'') + '<br /><br />' as publication" +
            " from publication_processing process" +
            " inner join publication p" +
            " on process.publication_id = p.publication_id" +
            " inner join publication_resource pr" +
            " on p.publication_id = pr.publication_id" +
            " where" +
            " pr.l_resource_id = " +
            resourceId.ToString() +
            " and" +
            " (process.publication_date >= '" +
            insideStartDate +
            "' and process.publication_date <= '" +
            insideEndDate +
            "')" +
            " order by " +
            " p.pub_year desc," +
            " replace(process.authorlist_no_inst,'<b>','')";
        return sqlStatement;
    }
    protected string MakeSqlStatementForAll(string insideStartDate, string insideEndDate)
    {
        string sqlStatement = "";

        //string authorlistName = " process.authorlist +";
        string authorlistName = "authorlist_no_inst";
        sqlStatement =
            "select lp.symbol as symbol," +
            //" process.authorlist +" +
            " process.authorlist_no_inst" +
            " +" +
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
            " isnull(p.pmcid,'') + '<br /><br />' as publication" +
            " from publication_processing process" +
            " inner join publication p" +
            " on process.publication_id = p.publication_id" +
            " and process.review_editorial is null" +
            " left outer join l_programmatic lp" +
            " on process.l_programmatic_id = lp.l_programmatic_id" +
            " where" +
            " process.publication_date >= '" +
            insideStartDate +
            "' and process.publication_date <= '" +
            insideEndDate +
            "'" +
            " order by " +
            " replace(" +
            authorlistName +
            ",'<b>','')";
        return sqlStatement;
    }
    protected int ProgrammaticCnt(int programmaticId, int programId, string startDate, string endDate)
    {
        string maxGroupStr = "";
        int maxGroupInt = 0;
        if (Request["maxGroup"] != null)
        {
            maxGroupStr = Request["maxGroup"].ToString();
            maxGroupInt = Convert.ToInt32(maxGroupStr);
        }

        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        if (maxGroupInt != 0)  //not All, not "No focus group only"
        {
            sqlStatement = "select" +
                " count(*) from publication_processing pd" +
                " inner join publication_programmatic pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pd.review_editorial is null" +
                " and pp.l_program_id = " +
                programId.ToString() +
                " inner join publication_program ppg" +
                " on pd.publication_id = ppg.publication_id" +
                " and ppg.l_program_id = " +
                programId.ToString() +
                " where pp.l_programmatic_id = " +
                programmaticId.ToString() +
                " and pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "' and ppg.l_focus_group_id is not null";
        }
        else if (maxGroupInt == 0) //for All within the program
        {
            sqlStatement = "select" +
                " count(*) from publication_processing pd" +
                " inner join publication_programmatic pp" +
                " on pd.publication_id = pp.publication_id" +
                " and pd.review_editorial is null" +
                " and pp.l_program_id = " +
                programId.ToString() +
                " inner join publication_program ppg" +
                " on pd.publication_id = ppg.publication_id" +
                " and ppg.l_program_id = " +
                programId.ToString() +
                " inner join l_focus_group lfg" +
                " on ppg.l_focus_group_id = lfg.l_focus_group_id" +
                " and lfg.group_number = 0" +
                " where pp.l_programmatic_id = " +
                programmaticId.ToString() +
                " and pd.publication_date >= '" +
                startDate +
                "' and pd.publication_date <= '" +
                endDate +
                "'";
        }

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int cnt = (int)myCommand.ExecuteScalar();
        conn.Close();
        return cnt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        /*
        string startDate = Request["startDate"].ToString();
        string endDate = Request["endDate"].ToString();
        string programIdStr = Request["programId"].ToString();
        Response.Redirect("ReportByProgramPrint.aspx?programId=" + programIdStr + "&startDate=" + startDate + "&endDate=" + endDate);
        */
        btnExport.Visible = false;
        //Response.ContentType = "application/msword"; same:
        Response.ContentType = "application/vnd.ms-word";
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        /*
        switch (FileExtension.ToLower())
        {
            case "pdf":
                Response.ContentType = "application/pdf";
                break;
            case "doc":
                Response.ContentType = "application/msword";
                break;
            case "docx":
                Response.ContentType = "application/vnd.ms-word.document.12";
                break;
            case "xls":
                Response.ContentType = "application/vnd.ms-excel";
                break;
            case "xlsx":
                Response.ContentType = "application/vnd.ms-excel.12";
                break;
            default:
                Response.ContentType = "image/jpeg";
                break;
        }
        */
        //Response.ContentEncoding = System.Text.Encoding.Unicode;
        //Response.ContentEncoding = System.Text.UnicodeEncoding.UTF32;
        Response.Charset = "UTF-16";
        //Response.ContentEncoding = System.Text.UnicodeEncoding.UTF8;
        //Response.Charset = "UTF-8";
        //Response.ContentEncoding = System.Text.UnicodeEncoding.Unicode;
        //Response.Charset = "Unicode";
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment; filename = Report.doc");

        /*
        //Response.ContentType = "application/msword";
        Response.ContentType = "application/vnd.ms-word";
        Response.Buffer = true;
        //Response.AddHeader("content-disposition", "attachment; filename = Report.docx");
        Response.AddHeader("content-disposition", "inline; filename = Report2.docx");

        MemoryStream ms = new MemoryStream()
        Report.SaveAs(ms);

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + ".doc\");
        Response.ContentType = "application/msword";

        ms.WriteTo(Response.OutputStream);
        Response.End();
         * 
        Response.ContentType = "application/vnd.ms-word";
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-word";
        Response.AddHeader("content-disposition", "inline; filename = Document.docx");
         * */
        /*
        Response.ClearHeaders();
        Response.AppendHeader("Content-disposition", "attachment;filename=HelloWorld.doc");
        Response.ContentType = "application/msword";
        wordDocument.SaveToStream(Response.OutputStream);
        Response.End();
        */
        /*
        Response.ContentType = "application/msword";
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "inline; filename = report.doc");
        */
        /*
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Output.doc");
        HttpContext.Current.Response.ContentType = "application/msword";
        //HttpContext.Current.Response.Flush();
        //HttpContext.Current.Response.End();     
        */

        /*
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/msword";
        Response.WriteFile(ExportPath);
        Response.Flush();
        Response.Close();
        */


    }
    public List<string> ReadCVS(string dir_path, string filename)
    {
        DataSet ds = new DataSet();
        List<string> pmidList = new List<string>();
        try
        {
            // Creates and opens an ODBC connection
            string ConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + dir_path + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
            string qry_select;
            OdbcConnection conn;
            conn = new OdbcConnection(ConnString.Trim());
            conn.Open();
            qry_select = "select * from [" + filename + "]";
            //Creates the data adapter
            OdbcDataAdapter oledb_da = new OdbcDataAdapter(qry_select, conn);

            //Fills records to the Dataset from CSV file
            oledb_da.Fill(ds, "csv_data");

            //closes the connection
            conn.Close();
            DataTable dt = ds.Tables["csv_data"];
            foreach (DataRow row in dt.Rows)
            {
                pmidList.Add(row[0].ToString());
            }
        }
        catch (Exception e) //Error
        {
        }
        return pmidList;
    }
}