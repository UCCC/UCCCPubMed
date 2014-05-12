using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for LoadLookup
/// </summary>
public class LoadLookup
{
	public LoadLookup()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static void LoadMember(DropDownList ddl, string name, string startDate, string endDate)
    {
        ddl.Items.Clear();
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " c.client_id," +
            " c.last_name + ', ' + c.first_name as name" +
            " from CLIENT c" +
            " inner join client_status cs" +
            " on c.client_id = cs.client_id" +
            " and cs.l_client_status_id = 3" +
            " and (cs.end_date is null or" +
            " dateadd(year,1,cs.end_date) > getdate()" +
            " or (dateadd(year,1,cs.end_date) > '" +
            endDate +
            "' and cs.end_date > '" +
            startDate +
            "'))" +
            " where c.last_name <> ''" +
            " order by c.last_name";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["name"].ToString();
                string idStr = myReader["client_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (name == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--member--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
    public static void LoadMember(DropDownList ddl, string name)
    {
        ddl.Items.Clear();
        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " c.client_id," +
            " c.last_name + ', ' + c.first_name as name" +
            " from CLIENT c" +
            " where c.last_name <> ''" +
            " order by c.last_name";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["name"].ToString();
                string idStr = myReader["client_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (name == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--member--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
    public static void LoadProgram(DropDownList ddl, string program, bool followByMember)
    {
        ddl.Items.Clear();

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " l_program_id," +
            " right('0' + convert(varchar, l_program_id),2) + '-' + program_name as program" +
            " from l_program" +
            " where ABBREVIATION is not null and ABBREVIATION <> ''" +
            " and l_program_id not in (2,7)" +
            //" order by ABBREVIATION";
            " order by program";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["program"].ToString();
                string idStr = myReader["l_program_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (program == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            //ddl.Items.Insert(0, "All programs");

            //ddl.Items.Insert(0, new ListItem("All programs", "0"));
            //ddl.Items.Insert(-1, "--programs--");
            if (followByMember)
            {
                ddl.Items.Add(new ListItem("All programs", "0"));
            }
            ddl.Items.Insert(0, new ListItem("--program--", "-1"));
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
    public static void LoadMemberOnProgram(int programId, DropDownList ddlMember)
    {
        ddlMember.Items.Clear();

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        if (programId == 0)
        {
            sqlStatement = "select" +
                " c.client_id," +
                " c.last_name + ', ' + c.first_name as name" +
                " from CLIENT c" +
                " inner join client_status cs" +
                " on c.client_id = cs.client_id" +
                " and cs.l_client_status_id = 3" +
                " and (cs.end_date is null or" +
                " dateadd(year,1,cs.end_date) > getdate())" +
                " where c.last_name <> ''" +
                " order by c.last_name";
        }
        else
        {
            sqlStatement = "select" +
                " c.client_id," +
                " c.last_name + ', ' + c.first_name as name" +
                " from CLIENT c" +
                " inner join client_status cs" +
                " on c.client_id = cs.client_id" +
                " and cs.l_client_status_id = 3" +
                " and (cs.end_date is null or" +
                " dateadd(year,1,cs.end_date) > getdate())" +
                " inner join client_program cp" +
                " on c.client_id = cp.client_id" +
                " and cp.l_program_id = " +
                programId.ToString() +
                " where c.last_name <> ''" +
                " order by c.last_name";
        }

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["name"].ToString();
                string idStr = myReader["client_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                ddlMember.Items.Add(li);
            }
            ddlMember.Items.Insert(0, "--member--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }
    }
    public static void LoadYesNo(DropDownList ddl, string yesNo)
    {
        ddl.Items.Clear();

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " l_yes_no_id," +
            " description as yesNo" +
            " from l_yes_no";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["yesNo"].ToString();
                string idStr = myReader["l_yes_no_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (yesNo == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "----");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
    public static void LoadCollectiveName(DropDownList ddl, string name)
    {
        ddl.Items.Clear();

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " l_collective_name_id," +
            " collective_name" +
            " from l_collective_name";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["collective_name"].ToString();
                string idStr = myReader["l_collective_name_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (name == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "----");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
    public static void LoadResource(DropDownList ddl, string name)
    {
        ddl.Items.Clear();

        //string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");
        string connectionStr = ConfigurationManager.ConnectionStrings["UcccPubMedDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionStr);
        string sqlStatement = "";
        sqlStatement = "select" +
            " l_resource_id," +
            " description as resource" +
            " from l_resource" +
            " order by description";

        SqlCommand myCommand = new SqlCommand(sqlStatement, conn);
        conn.Open();
        SqlDataReader myReader;
        myReader = myCommand.ExecuteReader();
        ddl.Items.Clear();
        try
        {
            while (myReader.Read())
            {
                string nameFromDB = myReader["resource"].ToString();
                string idStr = myReader["l_resource_id"].ToString();
                ListItem li = new ListItem(nameFromDB, idStr);
                if (name == nameFromDB)
                {
                    li.Selected = true;
                }
                ddl.Items.Add(li);
            }
            ddl.Items.Insert(0, "--Shared Resource--");
        }
        finally
        {
            myReader.Close();
            conn.Close();
        }

    }
}