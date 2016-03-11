using System;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data;
//using System.Web.UI.WebControls;

public class Common
{
    public static string StrConn =
        System.Configuration.ConfigurationManager.ConnectionStrings["XbrlDbCsEntities"].ToString();

    public static int SQLCommandTimeout =
        Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("SqlCommandTimeOut"));

    public static void ExecuteNonQuery(string sql)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = SQLCommandTimeout;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static DataSet ExecuteDataSet(string sql)
    {
        DataSet dsData = new DataSet();
        try
        {
            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = SQLCommandTimeout;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dsData);
                }
            }
            return dsData;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static DataSet ExecuteDataSet(SqlCommand sqlCmd)
    {

        DataSet dsData = new DataSet();
        try
        {
            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                using (sqlCmd)
                {
                    sqlCmd.CommandTimeout = SQLCommandTimeout;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Connection = conn;

                    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                    da.Fill(dsData);
                }
            }
            return dsData;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static DataSet ExecuteDataSet(string sql, string dataSetName)
    {
        DataSet dsData = new DataSet();
        try
        {
            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = SQLCommandTimeout;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dsData, dataSetName);
                }
            }
            return dsData;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static int ExecuteScalar(string sql)
    {
        int intData = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = SQLCommandTimeout;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    intData = (int)cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return intData;
    }

    public static string RemoveHtmlTag(string text)
    {
        return System.Text.RegularExpressions.Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
    }

    public static string getStringFieldValue(object strValue)
    {
        return Convert.ToString((((strValue == null)) ? "" : strValue));
    }

    public static string ConvertFileNameToModuleName(string ASPXFileName)
    {
        if (ASPXFileName == null || ASPXFileName == "")
        {
            return "";
        }
        string OriFilename = ASPXFileName;
        string RemoveExt = OriFilename;
        if (OriFilename.IndexOf(".") > 0)
        {
            RemoveExt = OriFilename.Substring(0, OriFilename.LastIndexOf("."));
        }
        Regex r = new Regex("[A-Z]");
        MatchCollection GroupColl = r.Matches(RemoveExt);
        int last = 0;
        int curr = 0;
        curr = 0;
        last = 0;
        string jadi = "";
        foreach (Match i in GroupColl)
        {
            curr = i.Index;
            if (curr != last)
            {
                jadi = (jadi + RemoveExt.Substring(last, curr - last)) + " ";
            }
            last = curr;
        }
        jadi += RemoveExt.Substring(last, RemoveExt.Length - last);
        return jadi;

    }

    public static string ConvertModuleNameToFileName(string strModuleName)
    {
        if (strModuleName == null || strModuleName == "")
        {
            return "";
        }
        if (strModuleName.IndexOf(".") > 0)
        {
            return strModuleName;
        }
        strModuleName = strModuleName.Replace(" ", "");
        return strModuleName + ".aspx";
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// get minute login expire
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[Ery MN]	28/01/2013	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public static Int32 GetLoginExpire()
    {
        return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LoginExpire"]);
    }


    public static bool isValidDatasetRow(DataSet ds)
    {
        bool tempisValidDatasetRow = false;
        try
        {
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tempisValidDatasetRow = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return tempisValidDatasetRow;
    }

    private static bool ColumnEqual(object A, object B)
    {
        //
        // Compares two values to determine if they are equal. Also compares DBNULL.Value.
        //
        // NOTE: If your DataTable contains object fields, you must extend this
        // function to handle the fields in a meaningful way if you intend to group on them.
        //
        if (A == DBNull.Value & B == DBNull.Value) // Both are DBNull.Value.
        {
            return true;
        }
        if (A == DBNull.Value | B == DBNull.Value) // Only one is DBNull.Value.
        {
            return false;
        }
        return A == B; // Value type standard comparison
    }

    public static bool isValidDataTableRow(DataTable dt)
    {
        bool tempisValidDataTableRow = false;
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    tempisValidDataTableRow = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return tempisValidDataTableRow;
    }

    public static DataTable getSelectDistinct(string TableName, DataTable dt, string FieldName, Type FieldType)
    {
        object LastValue = null;
        //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
        //		DataRow dr = null;
        DataTable ReturnDT = new DataTable(TableName);
        ReturnDT.Columns.Add(FieldName, FieldType);
        LastValue = null;
        if (isValidDataTableRow(dt))
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    ReturnDT.Rows.Add(new object[] { LastValue });
                }
            }
        }
        return ReturnDT;
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// get user id session
    /// </summary>
    /// <value></value>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[Ery MN]	28/01/2013	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    /* public static string SessionUserID
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {

                return Convert.ToString(((ObjWebPage.Session["UserId"] == null) ? "" : ObjWebPage.Session["UserId"]));
            }
            catch
            {
                return "";
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["UserId"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    } */

    /* public static string SessionUserName
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {

                return
                    Convert.ToString(((ObjWebPage.Session["UserName"] == null) ? "" : ObjWebPage.Session["UserName"]));
            }
            catch
            {
                return "";
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["UserName"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    } */

    /// <summary>
    /// Session Pk User Login Id 
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
   /* public static int? SessionPkUserId
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToInt32(((ObjWebPage.Session["PK_MsUser_ID"] == null)
                                         ? 0
                                         : ObjWebPage.Session["PK_MsUser_ID"]));
            }
            catch (Exception ex)
            {
                return 2;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["PK_MsUser_ID"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    } */

   /* public static DateTime? SessionLoginDate
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToDateTime(((ObjWebPage.Session["LoginDate"] == null)
                                            ? null
                                            : ObjWebPage.Session["LoginDate"]));
            }
            catch (Exception ex)
            {
                return Convert.ToDateTime("");
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["LoginDate"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    } */

    /* public static int? SessionFKMsGroupMenuId
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToInt32(((ObjWebPage.Session["FK_MsGroupMenu_Id"] == null)
                                         ? 0
                                         : ObjWebPage.Session["FK_MsGroupMenu_Id"]));
            }
            catch (Exception ex)
            {
                return 2;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["FK_MsGroupMenu_Id"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    }  */

    /* public static string SessionRoleName
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToString(((ObjWebPage.Session["RoleName"] == null) ? "" : ObjWebPage.Session["RoleName"]));
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["RoleName"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    }  */

    /* public static int? SessionCheckerMakerLevel
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToInt32(((ObjWebPage.Session["CheckerMaker"] == null)
                                         ? 0
                                         : ObjWebPage.Session["CheckerMaker"]));
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["CheckerMaker"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    }  */

    /* public static int? SessionPKGroupApproval
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToInt32(((ObjWebPage.Session["PK_MsGroupApproval_Id"] == null)
                                         ? 1
                                         : ObjWebPage.Session["PK_MsGroupApproval_Id"]));
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["PK_MsGroupApproval_Id"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    }  */

    /* public static string SessionBranchCode
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToString(((ObjWebPage.Session["BranchCode"] == null) ? "" : ObjWebPage.Session["BranchCode"]));
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["BranchCode"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    } */

    /* public static string SessionIntendedPage
    {
        get
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                return
                    Convert.ToString(((ObjWebPage.Session["SessionIntendedPage"] == null) ? "Default.aspx" : ObjWebPage.Session["SessionIntendedPage"]));
            }
            catch (Exception ex)
            {
                return "Default.aspx";
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
        set
        {
            System.Web.UI.Page ObjWebPage = new System.Web.UI.Page();
            try
            {
                ObjWebPage.Session["SessionIntendedPage"] = value;
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjWebPage.Dispose();
            }
        }
    }  */

    public static bool IsDateValid(string strFormat, string strTanggal)
    {

        try
        {
            DateTime ddate = ConvertToDate(strFormat, strTanggal);

            if (ddate.CompareTo(DateTime.MinValue) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static DateTime ConvertToDate(string strFormat, string strDate)
    {
        string[] formats = new string[1];
        formats[0] = strFormat;
        try
        {
            return DateTime.ParseExact(strDate, formats, CultureInfo.InvariantCulture,
                                       DateTimeStyles.AdjustToUniversal);
        }
        catch (Exception ex)
        {
            return DateTime.MinValue;
        }
    }

    public static string EncodeJavascript(string strText)
    {
        if (strText == null)
        {
            return "";
        }
        if (strText.Length == 0)
        {
            return "";
        }
        //        System.Text.RegularExpressions.Regex.Replace(strText, "\|[|'|""", "\$1")

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        char ch = '\0';
        for (int i = 0; i < strText.Length; i++)
        {
            ch = strText[i];
            if (ch == '\\' || ch == '\"' || ch == '\'' || ch == '>')
            {
                sb.Append("\\");
                sb.Append(ch);
            }
            else if (ch == Convert.ToChar("\r") || Equals(ch, Environment.NewLine))
            {
                sb.Append("\\n");
            }
            else if (ch == Convert.ToChar("\t"))
            {
                sb.Append("\\t");
            }
            else
            {
                sb.Append(ch);
            }
        }
        return sb.ToString();
    }

    //buat change sign pas sorting
    /* public static void ChangeHeader(GridViewRow headerRow, string CurrentSortExpression, SortDirection GridViewSortDirection)
    {
        for (int i = 0; i < headerRow.Cells.Count; i++)
        {
            if (headerRow.Cells[i] is DataControlFieldCell)
            {
                DataControlField field =
                  ((DataControlFieldCell)headerRow.Cells[i]).ContainingField;

                //remove all previous sorting marks if they exist
                Regex r = new Regex(@"\s(\u25bc|\u25b2)");
                field.HeaderText = r.Replace(field.HeaderText, "");

                if (field.SortExpression != null && field.SortExpression ==
                    CurrentSortExpression)
                {
                    //add current sorting state mark
                    if (GridViewSortDirection == SortDirection.Ascending)
                        field.HeaderText += " \u25b2";
                    else
                        field.HeaderText += " \u25bc";
                }
            }
        }
    } */


    /// <summary>
    /// return default datetime null
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public static DateTime GetDefaultDate
    {
        get { return new DateTime(1900, 1, 1); }
    }

    #region  Grid SortCommand

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// change sort command
    /// </summary>
    /// <param name="sSort"></param>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[Ery MN]	05/06/2006	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public static string ChangeSortCommand(string sSort)
    {
        string sTemp = sSort;
        if (sSort.Substring(sSort.Length - 4, 4).Trim() == "asc")
        {
            sTemp = sSort.Substring(0, sSort.Length - 5) + " desc";
        }
        else
        {
            sTemp = sSort.Substring(0, sSort.Length - 5) + "  asc";
        }
        return sTemp;
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// find sort index
    /// </summary>
    /// <param name="dgVictim"></param>
    /// <param name="sSort"></param>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[Ery MN]	05/06/2006	Created
    /// </history>
    /// -----------------------------------------------------------------------------
   /* public static Int16 IndexSort(System.Web.UI.WebControls.DataGrid dgVictim, string sSort)
    {
        for (Int16 i = 0; i <= Convert.ToInt16(dgVictim.Columns.Count - 1); i++)
        {
            if (dgVictim.Columns[i].SortExpression == sSort)
            {
                return i;
            }
        }
        return -1;
    } */

    /* public static Int16 IndexSort(System.Web.UI.WebControls.GridView dgVictim, string sSort)
    {
        for (Int16 i = 0; i <= Convert.ToInt16(dgVictim.Columns.Count - 1); i++)
        {
            if (dgVictim.Columns[i].SortExpression == sSort)
            {
                return i;
            }
        }
        return -1;
    } */

    #endregion

    #region  Post Back Go Button Check

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// add js script
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[Ery MN]	28/01/2013	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public static string AddJSS()
    {
        StringBuilder sBuild = new StringBuilder();
        try
        {
            sBuild.Append("<script language=\"JavaScript\">");
            sBuild.Append("function IsnotEmpty(cContol,iCurr,iMax)");
            sBuild.Append("{");
            sBuild.Append("var bTemp = false;");
            sBuild.Append(
                "if((cContol.value != \"\")&&(cContol.value > 0.5)&&(cContol.value != iCurr)&&(cContol.value <= iMax))");
            sBuild.Append("bTemp = true;");
            sBuild.Append("return bTemp;");
            sBuild.Append("};");
            sBuild.Append("</script>");
            return sBuild.ToString();
        }
        catch
        {
            throw;
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// add check for button goview
    /// </summary>
    /// <param name="TargetPage">web form</param>
    /// <param name="cVictim">button</param>
    /// <param name="cMustHave">textbox</param>
    /// <param name="iCurr">current page index</param>
    /// <param name="iMax">max page</param>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[Ery MN]	28/01/2013	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    /* public static void AddPostBackGoBut(System.Web.UI.Page TargetPage, System.Web.UI.WebControls.WebControl cVictim,
                                        System.Web.UI.WebControls.WebControl cMustHave, int iCurr, Int64 iMax)
    {
        try
        {
            cVictim.Attributes.Add("onclick",
                                   "return IsnotEmpty(" + cMustHave.ID + "," + Convert.ToString(iCurr) + "," +
                                   Convert.ToString(iMax) + ");");
            //TargetPage.RegisterClientScriptBlock("genericblock", AddJSS)
            TargetPage.ClientScript.RegisterClientScriptBlock(typeof(string), "genericblock", AddJSS());
        }
        catch (Exception ex)
        {
            throw;
        }
    } */

    #endregion
}

public class CustomException : Exception
{

    public CustomException(string strMessage)
        : base(strMessage)
    {

    }


}




