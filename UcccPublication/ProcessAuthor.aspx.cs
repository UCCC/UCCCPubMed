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

public partial class ProcessAuthor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hdnPubCnt.Value = "0";
            HttpCookie _dateCookies = Request.Cookies["dates"];
            if (_dateCookies != null)
            {
                txtStartDate.Text = _dateCookies["startDate"];
                txtEndDate.Text = _dateCookies["endDate"];
            }
        }
    }
    protected void btnProcessAuthorList_Click(object sender, EventArgs e)
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
    protected void SavePublog(string value, DateTime theDate)
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
    protected void btnListAuthors_Click(object sender, EventArgs e)
    {
        string lastName = "";
        string firstName = "";
        if (txtLastName.Text != "")
        {
            lastName = txtLastName.Text;
        }
        else
        {
            ErrorMessage.Text = "please enter last name.";
            return;
        }
        firstName = txtFirstName.Text;

        FillAuthorGrid(lastName, firstName);
        FillClientGrid(lastName, firstName);
        //ProcessSelectedAuthors.Visible = true;
    }
    public void FillAuthorGrid(string lastName, string firstName)
    {
        string sqlStatement;
        string shortLastName;
        string escapeLastName;

        int singleQuotePos = lastName.IndexOf("'");
        if (singleQuotePos > 0)
        {
            shortLastName = lastName.Substring(singleQuotePos + 1);
            escapeLastName = lastName.Replace("'", "''");
        }
        else
        {
            shortLastName = lastName;
            escapeLastName = lastName;
        }

        if (firstName == "")
        {
            sqlStatement =
                "select a.author_id," +
                " a.LastName," +
                " a.ForeName," +
                " a.Initials," +
                " a.client_id," +
                " a.confirm_id," +
                " lyn.description as confirm" +
                " from author a" +
                " left outer join l_yes_no lyn" +
                " on a.confirm_id = lyn.l_yes_no_id" +
                " where CHARINDEX('" +
                shortLastName +
                "', a.LastName) >0 and ('" +
                escapeLastName +
                "' = a.LastName or CHARINDEX(' ',a.LastName)>0)" +
                " order by a.LastName, a.ForeName";

        }
        else
        {
            sqlStatement =
                "select a.author_id," +
                " a.LastName," +
                " a.ForeName," +
                " a.Initials," +
                " a.client_id," +
                " a.confirm_id," +
                " lyn.description as confirm" +
                " from author a" +
                " left outer join l_yes_no lyn" +
                " on a.confirm_id = lyn.l_yes_no_id" +
                " where CHARINDEX('" +
                shortLastName +
                "', a.LastName) >0 and ('" +
                escapeLastName +
                "' = a.LastName or CHARINDEX(' ',a.LastName)>0)" +
                " and  (CHARINDEX('" +
                firstName +
                "', a.ForeName) >0 or" +
                " CHARINDEX(a.ForeName,'" +
                firstName
                + "') > 0)" +
                " order by a.LastName, a.ForeName";
        }

        Helper.BindGridview(sqlStatement, gvAuthor);

        //ProcessSelectedAuthors.Visible = true;
        lblDescription.Visible = true;

        /*
        HttpCookie _dateCookies = new HttpCookie("dates");
        _dateCookies["startDate"] = txtStartDate.Text;
        _dateCookies["endDate"] = txtEndDate.Text;
        _dateCookies.Expires = DateTime.Now.AddDays(5);
        Response.Cookies.Add(_dateCookies);
         * */
    }
    public void FillClientGrid(string lastName, string firstName)
    {
        string sqlStatement;
        string shortLastName;
        string escapeLastName;

        int singleQuotePos = lastName.IndexOf("'");
        if (singleQuotePos > 0)
        {
            shortLastName = lastName.Substring(singleQuotePos + 1);
            escapeLastName = lastName.Replace("'", "''");
        }
        else
        {
            shortLastName = lastName;
            escapeLastName = lastName;
        }

        if (firstName == "")
        {
            sqlStatement =
                "select c.client_id," +
                " c.last_name," +
                " c.first_name," +
                " c.MI," +
                " convert(varchar, cs.start_date,101) as start_date," +
                " case when cs.end_date is null then '' else convert(varchar,cs.end_date,101) end as end_date" +
                " from client c" +
                " inner join client_status cs" +
                " on c.client_id = cs.client_id" +
                " and cs.l_client_status_id = 3" + 
                " where c.last_name like '%" +
                shortLastName +
                "%'" +
                " order by c.last_name,first_name";
        }
        else
        {
            sqlStatement =
                "select c.client_id," +
                " c.last_name," +
                " c.first_name," +
                " c.MI," +
                " convert(varchar, cs.start_date,101) as start_date," +
                " case when cs.end_date is null then '' else convert(varchar,cs.end_date,101) end as end_date" +
                " from client c" +
                " inner join client_status cs" +
                " on c.client_id = cs.client_id" +
                " and cs.l_client_status_id = 3" +
                " where c.last_name like '%" +
                shortLastName +
                "%'" +
                " and c.first_name like '%" +
                firstName +
                "%'" +
                " order by c.last_name,c.first_name";
        }

        Helper.BindGridview(sqlStatement, gvClient);
    }
    protected void ProcessSelectedAuthors_Click(object sender, EventArgs e)
    {
        List<int> authorIdList = new List<int>();
        for (int i = 0; i < gvAuthor.Rows.Count; i++)
        {
            CheckBox chxSelectTemp = (CheckBox)gvAuthor.Rows[i].FindControl("chxSelect");
            if (chxSelectTemp == null)
            {
                return;
            }
            int id;
            Label lblIdTemp = null;
            lblIdTemp = (Label)gvAuthor.Rows[i].FindControl("lblId");
            if (lblIdTemp != null)
            {
                if (chxSelectTemp.Checked)
                {
                    id = Convert.ToInt32(lblIdTemp.Text);
                    authorIdList.Add(id);
                }
            }
            else
            {
                return;
            }
        }
        ProcessPub.ProcessAuthorList(authorIdList);

    }
    protected void gvAuthor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idStr;

        if (e.CommandName.Equals("Link"))
        {
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            Label lblIdTemp = null;
            lblIdTemp = (Label)gvAuthor.Rows[rowindex].FindControl("lblId");
            if (lblIdTemp != null)
            {
                idStr = lblIdTemp.Text;
            }
            else
            {
                return;
            }

            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            if (chxAllDate.Checked)
            {
                startDate = "01/01/1970";
                DateTime today = DateTime.Now;
                endDate = today.ToShortDateString();
            }
            //Response.Redirect("~/PublicationOfOneAuthor.aspx?authorId=" + idStr + "&startDate=" + startDate + "&endDate=" + endDate);
            Response.Redirect("~/AuthorPublication.aspx?authorId=" + idStr + "&startDate=" + startDate + "&endDate=" + endDate);

        }
    }
    protected void gvAuthor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Label lblIdTemp = (Label)e.Row.FindControl("lblId");
            string authorIdStr = lblIdTemp.Text;
            //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
            string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionStr);
            string sqlStatement = "";
            LinkButton lnkDeleteTemp = (LinkButton)e.Row.FindControl("lnkDelete");
            if (lnkDeleteTemp == null)
            {
                return;
            }

            if (chxAllDate.Checked)
            {
                lnkDeleteTemp.Visible = true;
                sqlStatement =
                    "select count(distinct pa.publication_id) as cnt" +
                    " from publication_author pa" +
                    " where pa.author_id = " + authorIdStr;
            }
            else
            {
                lnkDeleteTemp.Visible = false;
                sqlStatement =
                    "select count(distinct pa.publication_id) as cnt" +
                    " from publication_author pa" +
                    " inner join publication_processing pp" +
                    " on pa.publication_id = pp.publication_id" +
                    " and pp.review_editorial is null" +
                    " and ((pp.publication_date >= '" +
                    txtStartDate.Text +
                    "' and pp.publication_date <= '" +
                    txtEndDate.Text +
                    "'))" +
                    " where pa.author_id = " + authorIdStr;

                /*
                sqlStatement =
                    "select count(distinct pa.publication_id) as cnt" +
                    " from publication_author pa" +
                    " inner join publication_processing pp" +
                    " on pa.publication_id = pp.publication_id" +
                    " and pp.publication_date >= '" +
                    txtStartDate.Text +
                    "' and pp.publication_date <= '" +
                    txtEndDate.Text +
                    "' where pa.author_id = " + authorIdStr;
                 * */
            }

            SqlCommand command = new SqlCommand(sqlStatement, conn);
            conn.Open();
            int pubCnt = 0;
            object maxClientAuthorObj = (object)command.ExecuteScalar();
            if (maxClientAuthorObj != DBNull.Value)
            {
                pubCnt = Convert.ToInt32(maxClientAuthorObj);
            }
            conn.Close();

            LinkButton lnkLinkTemp = (LinkButton)e.Row.FindControl("lnkLink");
            lnkLinkTemp.Text = pubCnt.ToString();
            //Label lblIdTemp = (Label)e.Row.FindControl("lblId");

            /*
            string currConfirmStr = drv["confirm"].ToString();
            DropDownList ddlConfirmTemp = null;
            ddlConfirmTemp = (DropDownList)e.Row.FindControl("ddlConfirm");
            if (ddlConfirmTemp != null)
            {
                LoadLookup.LoadYesNo(ddlConfirmTemp, currConfirmStr);
            }
             * */
        }
    }
    protected void gvAuthor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvAuthor.EditIndex = -1;
        FillAuthorGrid(txtLastName.Text, txtFirstName.Text);
    }
    protected void gvAuthor_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        ErrorMessage.Text = "";
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvAuthor.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        TextBox txtClientIdTemp = null;
        txtClientIdTemp = (TextBox)gvAuthor.Rows[e.RowIndex].FindControl("txtClientId");
        if (txtClientIdTemp == null)
        {
            return;
        }

        string sqlStatement;
        if (txtClientIdTemp.Text != "")
        {
            sqlStatement =
                "Update author" +
                " SET client_id=@client_id" +
                ", confirm_id = 1" +
                " WHERE (author_id = @author_id)";
        }
        else
        {
            sqlStatement =
                "Update author" +
                " SET client_id=@client_id" +
                ", confirm_id = 2" +
                " WHERE (author_id = @author_id)";
        }

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        SqlCommand command = new SqlCommand(sqlStatement, myConnection);

        SqlParameter client_idParameter = new SqlParameter();
        client_idParameter.ParameterName = "@client_id";
        client_idParameter.SqlDbType = SqlDbType.Int;
        if (txtClientIdTemp.Text == "")
        {
            client_idParameter.Value = DBNull.Value; ;
        }
        else
        {
            client_idParameter.Value = Convert.ToInt32(txtClientIdTemp.Text);
        }
        command.Parameters.Add(client_idParameter);

        SqlParameter author_idParameter = new SqlParameter();
        author_idParameter.ParameterName = "@author_id";
        author_idParameter.SqlDbType = SqlDbType.Int;
        author_idParameter.Value = System.Convert.ToInt32(lblIdTemp.Text);
        command.Parameters.Add(author_idParameter);

        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        List<int> pubIdList = GetPubIdListByAuthorId(idStr).ToList();
        foreach (int pubId in pubIdList)
        {
            ProcessPub.UpdateAllProcessInfoForOnePubId(pubId);
        }

        gvAuthor.EditIndex = -1;
        FillAuthorGrid(txtLastName.Text,txtFirstName.Text);
    }
    protected void gvAuthor_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAuthor.EditIndex = e.NewEditIndex;
        FillAuthorGrid(txtLastName.Text, txtFirstName.Text);
    }
    protected void gvAuthor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string idStr;

        Label lblIdTemp = null;
        lblIdTemp = (Label)gvAuthor.Rows[e.RowIndex].FindControl("lblId");
        if (lblIdTemp != null)
        {
            idStr = lblIdTemp.Text;
        }
        else
        {
            return;
        }

        List<int> pubIdList = GetPubIdListByAuthorId(idStr).ToList();
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection myConnection = new SqlConnection(connectionStr);
        string sqlStatement;
        foreach (int pubId in pubIdList)
        {
            ProcessPub.SaveRejectOnPubId(pubId);
            sqlStatement =
                "delete from publication_author where publication_id = " + pubId +
                "; delete from publication_processing where publication_id = " + pubId +
                "; delete from publication_program where publication_id = " + pubId +
                "; delete from publication_programmatic where publication_id = " + pubId +
                "; delete from publication_pubtype where publication_id = " + pubId +
                "; delete from publication_resource where publication_id = " + pubId +
                "; delete from publication where publication_id = " + pubId;

            SqlCommand myCommand = new SqlCommand(sqlStatement, myConnection);
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        sqlStatement =
            "delete from author where author_id = " + idStr;

        SqlCommand command = new SqlCommand(sqlStatement, myConnection);
        myConnection.Open();
        command.ExecuteNonQuery();
        myConnection.Close();

        FillAuthorGrid(txtLastName.Text, txtFirstName.Text);

    }
    public IEnumerable<int> GetPubIdListByAuthorId(string authorIdStr)
    {
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        using (var connection = new SqlConnection(connectionStr))
        using (var cmd = connection.CreateCommand())
        {
            connection.Open();
            cmd.CommandText = "select publication_id from publication_author where author_id = " + authorIdStr;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return reader.GetInt32(reader.GetOrdinal("publication_id"));
                }
            }
        }
    }
    protected void FillNameGrid(string startWith)
    {
        string sqlStatement = "";

        sqlStatement =
             "select c.last_name + ', ' + c.first_name as name," +
             " COUNT(distinct a.author_id) as cnt" +
             " from author a" +
             " inner join client c" +
             " on CHARINDEX(c.last_name, a.LastName) >0" +
             " and (c.last_name = a.LastName or CHARINDEX(' ',a.LastName)>0)" +
             " and (CHARINDEX(c.FIRST_NAME, a.ForeName) >0 or CHARINDEX(a.ForeName, c.FIRST_NAME) >0)" +
             " inner join client_status cs" +
             " on c.client_id = cs.client_id" +
             " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate())" +
             " where left(c.last_name,1) = '" +
             startWith +
             "' group by c.last_name, c.first_name" +
             " order by c.last_name";

        Helper.BindGridview(sqlStatement, gvName);
    }
    protected void gvName_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string nameStr;

        if (e.CommandName.Equals("Link"))
        {
            LinkButton lnkNameTemp = null;
            GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
            lnkNameTemp = (LinkButton)gvName.Rows[rowindex].FindControl("lnkName");
            if (lnkNameTemp != null)
            {
                nameStr = lnkNameTemp.Text;
            }
            else
            {
                return;
            }
            string lastName = nameStr.Substring(0, nameStr.IndexOf(','));
            string firstName = nameStr.Substring(nameStr.IndexOf(',') + 2);
            txtLastName.Text = lastName;
            txtFirstName.Text = firstName;
            FillAuthorGrid(lastName, firstName);
            FillClientGrid(lastName, firstName);
            //ProcessSelectedAuthors.Visible = true;
        }
    }
    protected void ClickStartWithLink(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        FillNameGrid(lb.Text);
    }
    protected void CheckAllDates(object sender, EventArgs e)
    {
        if (chxAllDate.Checked)
        {
            txtStartDate.Text = "";
            txtEndDate.Text = "";
        }
    }
    protected void btnProcessPmid_Click(object sender, EventArgs e)
    {
        string msg = "";
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "select publication_id from publication where pmid = " + txtPmid.Text;
        SqlCommand command = new SqlCommand(sqlStatement, conn);
        conn.Open();
        int pubId = 0;
        object pubIdObj = (object)command.ExecuteScalar();
        if (pubIdObj != null)
        {
            pubId = Convert.ToInt32(pubIdObj);
        }
        else
        {
            conn.Close();
            msg = "Not found publication in database.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
            return;
        }
        conn.Close();

        ProcessPub.ProcessAllInfoForOnePubId(pubId, true);
        msg = "processed.";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
    }
}