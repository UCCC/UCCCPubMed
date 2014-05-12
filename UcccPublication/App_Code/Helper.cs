using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Helper
/// </summary>
public class Helper
{
	public Helper()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static void BindGridview(string sqlStatement, GridView gv)
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        SqlDataSource ds = new SqlDataSource(connectionStr, sqlStatement);
        gv.DataSource = ds;
        gv.DataBind();
    }
    public static int ReadCitation(string titleStr)
    {
        int citationNum = 0;
        string wholeStream = "";
        string strUrl;
        string title;

        if (titleStr != "")
        {
            //try
            //{
            title = titleStr;
            title = title.Replace(' ', '+');
            strUrl = @"http://scholar.google.com/scholar?q=%22" + title + @"%22&btnG=&hl=en&as_sdt=0%2C6";

            Uri theUrl = new Uri(strUrl);
            //Create the request object
            WebRequest req = WebRequest.Create(theUrl);
            //req.Proxy = null;
            /*
            req.Credentials = new NetworkCredential(Username, Password);
            req.KeepAlive = false;
            req.Proxy = null;
            */

            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            wholeStream = sr.ReadToEnd();
            //}
            //catch (WebException ex)
            //{
            /*
            using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                html = sr.ReadToEnd();
             * */
            //}


        }
        else
        {
            return -1;
        }
        wholeStream = wholeStream.Replace(',', ' ');

        //read File          
        //instantiate with this pattern          
        Regex CitationRegex = new Regex(@"\bCited by\s*[0-9]*\b", RegexOptions.IgnoreCase);

        //find items that matches with our pattern         
        MatchCollection citationMatches = CitationRegex.Matches(wholeStream);
        //if (citationMatches.Count == 0)
        //{
        //    return citationNum;
        //}
        if (citationMatches.Count == 0)
        {
            return -1;
        }
        foreach (Match citationMatche in citationMatches)
        {
            string citationStr = citationMatche.Value.Substring(9);
            citationNum = Convert.ToInt32(citationStr);
            break;
        }
        return citationNum;

    }
    public static int GetAddress(string pmidStr)
    {
        int AddressNum = 0;
        string wholeStream = "";
        string strUrl;

        if (pmidStr != "")
        {
            //try
            //{
            strUrl = @"http://www.ncbi.nlm.nih.gov/pubmed/" + pmidStr;

            Uri theUrl = new Uri(strUrl);
            //Create the request object
            WebRequest req = WebRequest.Create(theUrl);
            //req.Proxy = null;

            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            wholeStream = sr.ReadToEnd();
        }
        else
        {
            return 0;
        }
        wholeStream = wholeStream.Replace(',', ' ');


        Regex stateRegex = new Regex(@"\bColorado\b", RegexOptions.IgnoreCase);

        //find items that matches with our pattern         
        MatchCollection stateMatches = stateRegex.Matches(wholeStream);
        AddressNum = stateMatches.Count;
        if (AddressNum > 0)
        {
            return AddressNum;
        }
        //instantiate with this pattern          
        //Regex AddressRegex = new Regex(@"\bColorado|Aurora|Denver|ucdenver.edu\b", RegexOptions.IgnoreCase);
        Regex AddressRegex = new Regex(@"\bAurora|Denver\b", RegexOptions.IgnoreCase);

        //find items that matches with our pattern         
        MatchCollection AddressMatches = AddressRegex.Matches(wholeStream);

        AddressNum = AddressMatches.Count;

        /*
        foreach (Match AddressMatche in AddressMatches)
        {
            string AddressStr = AddressMatche.Value;
        }
         * */
        return AddressNum;

    }
    public static void GetStartEndDate(out string startDateStr, out string endDateStr)
    {
        DateTime today = DateTime.Now;
        int thisYear = today.Year;
        int startYear;
        int endYear;
        string flipDateStr = "06/30/" + thisYear.ToString();
        DateTime flipDate = DateTime.Parse(flipDateStr);
        if (today < flipDate)
        {
            startYear = thisYear - 1;
            endYear = thisYear;
        }
        else
        {
            startYear = thisYear;
            endYear = thisYear + 1;
        }
        startDateStr = "07/01/" + startYear.ToString();
        endDateStr = "06/30/" + endYear.ToString();

    }
    public static string ListToArray(List<string> theList, string delimiter)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string item in theList)
        {
            if (sb.Length > 0)
            {
                sb.Append(delimiter);
            }
            sb.Append(item);
        }
        return sb.ToString();
    }
    public static void Export(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    Helper.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    Helper.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    Helper.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if (current.HasControls())
            {
                Helper.PrepareControlForExport(current);
            }
        }
    }
}