using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PmidCSVtoReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie _dateCookies = Request.Cookies["dates"];
        if (_dateCookies != null)
        {
            txtStartDate.Text = _dateCookies["startDate"];
            txtEndDate.Text = _dateCookies["endDate"];
            ErrorMessage.Text = _dateCookies["ErrorMessage"];
        }

    }
    protected void btnOpenCsv_Click(object sender, EventArgs e)
    {
        string startDate = txtStartDate.Text;
        string endDate = txtEndDate.Text;

        // check to see is path exists
        try
        {
            fu.SaveAs(Server.MapPath("~/upload/") + fu.FileName);
            string csv_path = Server.MapPath("~/upload/") + fu.FileName;
            string csvCoreName = fu.FileName.Substring(0, fu.FileName.Length - 4);
            Response.Redirect("Report.aspx?csvFile=" + csvCoreName +
                "&startDate=" + startDate +
                "&endDate=" + endDate);
            //List<string> pmidList = new List<string>();
            //pmidList = ReadCVS(Server.MapPath("~/upload/"), fu.FileName);
        }
        catch (Exception ex)
        {
            String errormsg = ex.Message.ToString();
            ErrorMessage.Text = ex.Message.ToString();
        }

    }
}