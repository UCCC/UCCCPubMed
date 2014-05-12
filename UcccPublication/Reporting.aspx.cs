using System;
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
using System.IO;

public partial class Reporting : System.Web.UI.Page
{
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userIdStr = Session["userId"].ToString();
            string roleIdStr = Session["roleId"].ToString();
            if (roleIdStr == "1")
            {
                LoadLookup.LoadProgram(ddlProgram, "xxx", true);
                ddlProgram.SelectedValue = "0";
                LoadLookup.LoadMemberOnProgram(0, ddlMember);
                //LoadLookup.LoadMember(ddlMember, "xxx");
            }
            if (roleIdStr == "2")
            {
                string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionStr);
                string sqlStatement;
                sqlStatement = "select" +
                    " right('0' + convert(varchar, lp.l_program_id),2) + '-' + lp.program_name as program" +
                    " from pub_user pu" +
                    " inner join program_leader pl" +
                    " on pu.client_id = pl.client_id" +
                    " inner join l_program lp" +
                    " on pl.l_program_id = lp.l_program_id" +
                    " where pu.pub_user_id = " +
                    userIdStr;

                SqlCommand command = new SqlCommand(sqlStatement, conn);
                conn.Open();
                string program = "";
                object programObj = (object)command.ExecuteScalar();
                if (programObj != DBNull.Value)
                {
                    program = programObj.ToString();
                }
                conn.Close();
                LoadLookup.LoadProgram(ddlProgram, program, true);

                //ddlProgram.SelectedValue = "0";
                //LoadLookup.LoadMemberOnProgram(0, ddlMember);

                //ddlProgram.Enabled = false;
                int programId = Convert.ToInt32(ddlProgram.SelectedValue);
                LoadLookup.LoadMemberOnProgram(programId, ddlMember);
                //SelectProgram(sender, e);
            }
            LoadLookup.LoadResource(ddlResource, "xxx");

            //LoadLookup.LoadProgram(ddlProgram, "xxx", false);
            //LoadLookup.LoadMember(ddlMember, "xxx");

            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
            FillMemberSource(-1);
       }
        //string scriptStr = "function ShowConfirmation(){return confirm('hahaha');}";
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Hello", scriptStr, true);
        //Button1.Attributes.Add("OnClick", "javascript:return ShowConfirmation();");
    }
    protected void FillMemberSource(int programId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        if (programId == -1)
        {
            sqlStatement = "select" +
                " c.client_id," +
                " c.last_name + ', ' + c.first_name as name" +
                " from CLIENT c" +
                " where c.last_name <> ''" +
                " order by c.last_name";
        }
        else
        {
            sqlStatement = "select" +
                " c.client_id," +
                " c.last_name + ', ' + c.first_name as name" +
                " from CLIENT c" +
                " inner join client_program cp" +
                " on c.client_id = cp.client_id" +
                " and cp.l_program_id = " +
                programId.ToString() +
                " where c.last_name <> ''" +
                " order by c.last_name";
        }

        SqlDataSource dsMember = new SqlDataSource(connectionStr, sqlStatement);
        lbSource.DataTextField = "name";
        lbSource.DataValueField = "client_id";

        lbSource.DataSource = dsMember;
        lbSource.DataBind();
    }
    protected void NullFocusCnt(string programIdStr, string startDate, string endDate, out int cntTotal, out int cntForcusGroup)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement =
            " select count(pa.publication_processing_id) from publication_processing pa" +
            " inner join publication_program pp" +
            " on pa.publication_id = pp.publication_id" +
            " and pa.review_editorial is null" +
            " and pp.l_program_id = " +
            programIdStr +
            " where ((pa.publication_date >= '" +
            startDate +
            "' and pa.publication_date <= '" +
            endDate +
            "'))";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        cntTotal = (int)myCommand.ExecuteScalar();
        conn.Close();

        sqlStatement =
            " select count(pa.publication_processing_id) from publication_processing pa" +
            " inner join publication_program pp" +
            " on pa.publication_id = pp.publication_id" +
            " and pa.review_editorial is null" +
            " and pp.l_program_id = " +
            programIdStr +
            " where ((pa.publication_date >= '" +
            startDate +
            "' and pa.publication_date <= '" +
            endDate +
            "'))" +
            " and pp.l_focus_group_id is not null";

        SqlCommand myCommandForcusGroup = new SqlCommand(sqlStatement, conn);
        conn.Open();
        cntForcusGroup = (int)myCommandForcusGroup.ExecuteScalar();
        conn.Close();
    }
    protected void btnReportMemberAll_Click(object sender, EventArgs e)
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
            //ErrorMessage.Text = "Please select member.";
            //return;
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
        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);   

        Response.Cookies.Add(_dateCookies);
        string wAbstract = "0";
        if (chxAbstract.Checked)
        {
            wAbstract = "1";
        }
        string wInst = "0";
        if (chxInst.Checked)
        {
            wInst = "1";
        }
        string wPmid = "0";
        if (chxPmid.Checked)
        {
            wPmid = "1";
        }
        Response.Redirect("Report.aspx?clientId=1" + 
            "&startDate=" + startDate + 
            "&endDate=" + endDate + 
            "&wAbstract=" + wAbstract +
            "&wInst=" + wInst +
            "&wPmid=" + wPmid);

    }
    protected void btnGetAllPublication_Click(object sender, EventArgs e)
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
        string wAbstract = "0";
        if (chxAbstract.Checked)
        {
            wAbstract = "1";
        }
        string wInst = "0";
        if (chxInst.Checked)
        {
            wInst = "1";
        }
        string wPmid = "0";
        if (chxPmid.Checked)
        {
            wPmid = "1";
        }
        Response.Redirect("Report.aspx?startDate=" + startDate + 
            "&endDate=" + endDate + 
            "&wAbstract=" + wAbstract +
            "&wInst=" + wInst +
            "&wPmid=" + wPmid);
        //Response.Redirect("Report.aspx?startDate=" + startDate + "&endDate=" + endDate);

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);
    }
    protected int GetMaxGroup(int programId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select max(group_number) from l_focus_group where l_program_id = " + programId.ToString();
        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int maxGroupNumber = (int)commandCnt.ExecuteScalar();
        conn.Close();
        return maxGroupNumber;
    }
    protected int GetTotalFocusGroup(int programId)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select max(group_number) from l_focus_group where l_program_id = " + programId.ToString();
        SqlCommand commandCnt = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int maxGroupNumber = (int)commandCnt.ExecuteScalar();
        conn.Close();
        return maxGroupNumber;
    }
    protected void btnReportProgram0FG_Click(object sender, EventArgs e)
    {
        string programIdStr;
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programIdStr = ddlProgram.SelectedValue.ToString();
        }
        else
        {
            ErrorMessage.Text = "Please select program.";
            return;
        }
        int programId = Convert.ToInt32(programIdStr);
        int maxGroup = 0;
        ProgramReport0OrNon0FG(programId, maxGroup);

    }
    protected void SelectProgram(object sender, EventArgs e)
    {
        string programIdStr = ddlProgram.SelectedValue.ToString();
        int programId = Convert.ToInt32(programIdStr);
        //if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        LoadLookup.LoadMemberOnProgram(programId, ddlMember);
        if (ddlProgram.SelectedValue.ToString() != "0")
        {
            int totalFocusGroup = GetTotalFocusGroup(programId);
            btnReportProgramNon0FG.Text = "Focus Groups 1 - " + totalFocusGroup.ToString();
        }
        else
        {
            btnReportProgramNon0FG.Text = "Focus Groups Excluding 0";
        }
        FillMemberSource(programId);
    }
    protected void btnReportProgramNon0FG_Click(object sender, EventArgs e)
    {
        string programIdStr;
        if (ddlProgram.SelectedIndex != 0 && ddlProgram.SelectedIndex != -1)
        {
            programIdStr = ddlProgram.SelectedValue.ToString();
        }
        else
        {
            ErrorMessage.Text = "Please select program.";
            return;
        }
        int programId = Convert.ToInt32(programIdStr);
        int maxGroup = GetMaxGroup(programId);
        ProgramReport0OrNon0FG(programId, maxGroup);

    }
    protected void ProgramReport0OrNon0FG(int programId, int maxGroup)
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
        int cntTotal = 0;
        int cntForcusGroup = 0;
        NullFocusCnt(programId.ToString(), startDate, endDate, out cntTotal, out cntForcusGroup);

        string wAbstract = "0";
        if (chxAbstract.Checked)
        {
            wAbstract = "1";
        }
        string wInst = "0";
        if (chxInst.Checked)
        {
            wInst = "1";
        }
        string wPmid = "0";
        if (chxPmid.Checked)
        {
            wPmid = "1";
        }
        Response.Redirect("Report.aspx?programId=" + programId.ToString() +
            "&startDate=" + startDate +
            "&endDate=" + endDate +
            "&maxGroup=" + maxGroup.ToString() +
            "&wAbstract=" + wAbstract +
            "&wInst=" + wInst +
            "&wPmid=" + wPmid);
        //Response.Redirect("Report.aspx?programId=" + programIdStr + "&startDate=" + startDate + "&endDate=" + endDate + "&maxGroup=" + maxGroupNumber.ToString());

        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);

    }
    protected void btnOpenCsv_Click_save(object sender, EventArgs e)
    {
        //FileUpload fu = new FileUpload();
        string filename = "";
        if (fu.HasFile)
        {
            filename = Path.GetFullPath(fu.FileName);
            //filename = fu.PostedFile.FileName;
        }
        List<string> pmidList = new List<string>();

        //CSVReader reader = new CSVReader(FileUpload1.PostedFile.InputStream);   27:        //get the header   28:        string[] headers = reader.GetCSVLine();           29:        DataTable dt = new DataTable();   30:        //add headers   31:        foreach (string strHeader in headers)   32:            dt.Columns.Add(strHeader);   33:     34:        string[] data;   35:        while ((data = reader.GetCSVLine()) != null)   36:            dt.Rows.Add(data);   37:        //bind gridview   38:        gv.DataSource = dt;   39:        gv.DataBind();
        
        using (CsvFileReader reader = new CsvFileReader(filename))
        {
            CsvRow row = new CsvRow();
            while (reader.ReadRow(row))
            {
                foreach (string s in row)
                {
                    pmidList.Add(s);
                }
            }
        }
         

        /*
        FileUpload fu = new FileUpload();
        if (fu.HasFile)
        {
            try
            {
                string filename = Path.GetFileName(fu.FileName);
                string ary;
                StreamReader sr = new StreamReader(filename);
                int placeholder = 0;
              //  while (sr.Peek != -1)
                //{
               // }


                //fu.SaveAs(Server.MapPath("~/") + filename);
                //StatusLabel.Text = "Upload status: File uploaded!";
            }
            catch (Exception ex)
            {
                //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
            }
        }
        */
        /*
        if (fu..ShowDialog() == DialogResult.OK)
        {
            System.IO.StreamReader sr = new
               System.IO.StreamReader(openFileDialog1.FileName);
            MessageBox.Show(sr.ReadToEnd());
            sr.Close();
        }
        */
    }
    public class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream)
            : base(stream)
        {
        }

        public CsvFileReader(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();
            if (String.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }
    }
    protected void btnOpenCsv_Click(object sender, EventArgs e)
    {
        string startDate = txtStartDate.Text;
        string endDate = txtEndDate.Text;

        fu.SaveAs(Server.MapPath("~/upload/") + fu.FileName);
        string csv_path = Server.MapPath("~/upload/") + fu.FileName;
        string csvCoreName = fu.FileName.Substring(0, fu.FileName.Length - 4);
        Response.Redirect("Report.aspx?csvFile=" + csvCoreName +
            "&startDate=" + startDate +
            "&endDate=" + endDate);
        //List<string> pmidList = new List<string>();
        //pmidList = ReadCVS(Server.MapPath("~/upload/"), fu.FileName);
    }
    protected void btnResource_Click(object sender, EventArgs e)
    {
        string startDate = txtStartDate.Text;
        string endDate = txtEndDate.Text;
        string resourceIdStr = ddlResource.SelectedValue.ToString();
        Response.Redirect("Report.aspx?resId=" + resourceIdStr +
            "&startDate=" + startDate +
            "&endDate=" + endDate);
    }
    protected void btnReportMemberNon0FG_Click(object sender, EventArgs e)
    {
        Session["MEMBERLIST"] = lbDistination;
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
            //ErrorMessage.Text = "Please select member.";
            //return;
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
        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);

        Response.Cookies.Add(_dateCookies);
        string wAbstract = "0";
        if (chxAbstract.Checked)
        {
            wAbstract = "1";
        }
        string wInst = "0";
        if (chxInst.Checked)
        {
            wInst = "1";
        }
        string wPmid = "0";
        if (chxPmid.Checked)
        {
            wPmid = "1";
        }
        Response.Redirect("Report.aspx?clientId=1" +
            "&startDate=" + startDate +
            "&endDate=" + endDate +
            "&maxGroup=1" +
            "&wAbstract=" + wAbstract +
            "&wInst=" + wInst +
            "&wPmid=" + wPmid);

    }
    protected void btnReportMember0FG_Click(object sender, EventArgs e)
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
            //ErrorMessage.Text = "Please select member.";
            //return;
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
        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);

        Response.Cookies.Add(_dateCookies);
        string wAbstract = "0";
        if (chxAbstract.Checked)
        {
            wAbstract = "1";
        }
        string wInst = "0";
        if (chxInst.Checked)
        {
            wInst = "1";
        }
        string wPmid = "0";
        if (chxPmid.Checked)
        {
            wPmid = "1";
        }
        Response.Redirect("Report.aspx?clientId=1" +
            "&startDate=" + startDate +
            "&endDate=" + endDate +
            "&maxGroup=0" +
            "&wAbstract=" + wAbstract +
            "&wInst=" + wInst +
            "&wPmid=" + wPmid);

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbSource.Items)
        {
            if (li.Selected == true)
            {
                lbDistination.Items.Add(li);
            }
        }
        foreach (ListItem li in lbDistination.Items)
        {
            lbSource.Items.Remove(li);
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbDistination.Items)
        {
            if (li.Selected == true)
            {
                lbSource.Items.Add(li);
            }
        }
        foreach (ListItem li in lbSource.Items)
        {
            lbDistination.Items.Remove(li);
        }

    }
}