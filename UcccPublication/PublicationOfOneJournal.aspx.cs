using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

public partial class PublicationOfOneJournal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<string> journalList = new List<string>();
            string journalArray = ""; 
            object journalObj = Request["journal"];
            string journal = "";
            if (journalObj != null)
            {
                journal = journalObj.ToString();
                journalList.Add(journal);
            }
            else 
            {
                journalList = (List<string>)Session["JOURNALLIST"];
            }
            journalArray = ListToArray(journalList, ",");
            lblJournal.Text = journalArray;
            journalArray = "'" + journalArray + "'";
            journalArray = journalArray.Replace(",", "','");
            string startDate = Request["startDate"].ToString();
            string endDate = Request["endDate"].ToString();
            lblStartDate.Text = startDate;
            lblEndDate.Text = endDate;

            GetPublicationStat(journalArray, startDate, endDate);
        }
    }
    protected void GetPublicationStat(string journalArray, string startDate, string endDate)
    {
        string sqlStatement = "";

        sqlStatement =
            " select p.ISOAbbreviation as journal," +
            " p.article_title as article," +
            " pd.authorlist," +
            " p.pmid" +
            " from PUBLICATION p" +
            " inner join publication_processing pd" +
            " on p.publication_id = pd.publication_id" +
            " and ((pd.publication_date >= '" +
            startDate +
            "' and pd.publication_date <= '" +
            endDate +
            "'))" +
            " and p.ISOAbbreviation in (" +
            journalArray +
            ") order by journal";

        Helper.BindGridview(sqlStatement, gvPublication);
    }
    protected string ListToArray(List<string> theList, string delimiter) 
    {   
      StringBuilder sb = new StringBuilder();   
      foreach (string item in theList) {
          if (sb.Length > 0)
          {
              sb.Append(delimiter);
          }
          sb.Append(item);   
      }   
      return sb.ToString();   
    }  
}