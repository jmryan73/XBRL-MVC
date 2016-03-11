using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
//using System.Web.UI.WebControls;
using XBRLApp.Common;

namespace XBRLApp.DAL
{
    public class AdditionalDAL
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        SqlConnection conn;
        string strConn = ConfigurationManager.ConnectionStrings["XbrlDbCsEntities"].ConnectionString;

        private void OpenConn()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        private void CloseConn()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public void MappingFormBindGridMapping(string xbrlMapFormTbl, int currentPageSize, int currentPageIndex, string currentSortExpression, string whereClause, GridView grvMapFormTaxo)
        {
            string sql = "EXEC GetPageDate_MappingForm \n"
                       + "	@WhereClause = '" + whereClause + "', \n"
                       + "	@OrderBy = '" + currentSortExpression + "', \n"
                //set to 0, agar data di load semua
                       + "	@PageIndex = 0 , \n"
                       + "	@PageSize = " + currentPageSize + ", \n"
                       + "	@TableName = '" + xbrlMapFormTbl + "'";
            DataSet dsData = Common.ExecuteDataSet(sql);

            grvMapFormTaxo.DataSource = dsData.Tables[0];
            grvMapFormTaxo.PageSize = currentPageSize;
            grvMapFormTaxo.PageIndex = currentPageIndex;
            grvMapFormTaxo.DataBind();
        }

        public void UploadXmlRunPagingData(GridView grvUploadXml, int currentPageSize, int currentPageIndex, string whereClause, string orderBy)
        {
            string sql = "EXEC sp_GetUploadXml ";
            sql += "@WhereClause = '" + whereClause + "', ";
            sql += "@OrderBy = '" + orderBy + "', ";
            //set to 0 to load all data
            sql += "@PageIndex = 0, ";
            sql += "@PageSize = " + currentPageSize;

            DataSet dsData = Common.ExecuteDataSet(sql);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                grvUploadXml.DataSource = dsData.Tables[0];
                grvUploadXml.PageIndex = currentPageIndex;
                grvUploadXml.PageSize = currentPageSize;
                grvUploadXml.DataBind();
            }
            else
            {
            }
        }

        public void TechnicalValidationRunPaging(GridView grvValTech, string formSelected, int CurrentPageSize
            , int CurrentPageIndex, string BankCode, string ColumnName, string Operator
            , string ValueSearch, string SortOrder, string SortExpr)
        {
            try
            {
                OpenConn();

                SqlCommand Cmd = new SqlCommand("dbo.GetPagedData_TechnicalValidation");
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("@TableName", SqlDbType.VarChar, 500).Value = formSelected;
                Cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = CurrentPageSize;
                Cmd.Parameters.Add("@CurrentPage", SqlDbType.Int, 4).Value = CurrentPageIndex;


                Cmd.Parameters.Add("@ColumnName", SqlDbType.VarChar, 200).Value = ColumnName;
                Cmd.Parameters.Add("@Operator", SqlDbType.VarChar, 10).Value = Operator;
                Cmd.Parameters.Add("@ValueSearch", SqlDbType.VarChar, 200).Value = ValueSearch;
                Cmd.Parameters.Add("@ItemCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (BankCode != "")
                {
                    Cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar, 100).Value = BankCode;
                }

                if (SortOrder != "")
                {
                    Cmd.Parameters.Add("@SortOrder", SqlDbType.VarChar, 100).Value = SortOrder;
                }

                if (SortExpr != "")
                {
                    Cmd.Parameters.Add("@SortExpr", SqlDbType.VarChar, 100).Value = SortExpr;
                }

                //DataSet dsData = new DataSet();

                //SqlDataAdapter da = new SqlDataAdapter(Cmd);
                //da.Fill(dsData);

                DataSet ds = Common.ExecuteDataSet(Cmd);

                grvValTech.DataSource = ds.Tables[0];
                grvValTech.DataBind();

                CloseConn();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }

        }
        

        public void DataStagingChangerRunnerPaging(GridView grvStaggingForm, string ColumnName, string Operator, string ValueSearch, string tblName, int CurrentPageSize, int CurrentPageIndex, string SortOrder, string SortExpr)
        {
            try
            {
                //OpenConn();

                //SqlCommand Cmd = new SqlCommand("dbo.sp_ManualCorrection_GetPaged", conn);
                //Cmd.CommandType = CommandType.StoredProcedure;

                //Cmd.Parameters.Add("@TableName", SqlDbType.VarChar, 500).Value = tblName;
                //Cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = CurrentPageSize;
                //Cmd.Parameters.Add("@CurrentPage", SqlDbType.Int, 4).Value = CurrentPageIndex;
                //Cmd.Parameters.Add("@ItemCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                //Cmd.Parameters.Add("@ColumnName", SqlDbType.VarChar, 200).Value = ColumnName;
                //Cmd.Parameters.Add("@Operator", SqlDbType.VarChar, 10).Value = Operator;
                //Cmd.Parameters.Add("@ValueSearch", SqlDbType.VarChar, 200).Value = ValueSearch;

                //if (SortOrder != "")
                //{
                //    Cmd.Parameters.Add("@SortOrder", SqlDbType.VarChar, 100).Value = SortOrder;
                //}

                //if (SortExpr != "")
                //{
                //    Cmd.Parameters.Add("@SortExpr", SqlDbType.VarChar, 100).Value = SortExpr;
                //}

                //int totalRows = Convert.ToInt32(Cmd.Parameters["@ItemCount"].Value);

                //CloseConn();

                string sql = "DECLARE @ItemCount int \n"
                       + "EXEC dbo.sp_ManualCorrection_GetPaged \n"
                    //set to 0, agar data di load semua
                       + "	@TableName = '" + tblName + "', \n"
                       + "	@PageSize = 1000 , \n"
                       + "	@CurrentPage = 1, \n"
                       + "	@ItemCount = @ItemCount " + ParameterDirection.Output + ", \n"
                       + "	@ColumnName = '" + ColumnName + "', \n"
                       + "	@Operator = '" + Operator + "', \n"
                       + "	@ValueSearch = '" + ValueSearch + "'";
                DataSet dsData = Common.ExecuteDataSet(sql);

                grvStaggingForm.PageSize = CurrentPageSize;
                grvStaggingForm.PageIndex = CurrentPageIndex;
                grvStaggingForm.DataSource = dsData.Tables[0];
                grvStaggingForm.DataBind();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }


        public void UploadTechValidationRunPaging(GridView grvValTech, string formSelected, int CurrentPageSize
            , int CurrentPageIndex, string WhereClause)
        {
            try
            {
                SqlCommand Cmd = new SqlCommand("dbo.GetPagedData");
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("@TableName", SqlDbType.VarChar, 500).Value = formSelected;
                Cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = 1000;
                Cmd.Parameters.Add("@CurrentPage", SqlDbType.Int, 4).Value = 1;
                Cmd.Parameters.Add("@ItemCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                Cmd.Parameters.Add("@WhereClause", SqlDbType.VarChar, 1000).Value = WhereClause;

                DataSet ds = Common.ExecuteDataSet(Cmd);

                grvValTech.DataSource = ds.Tables[0];
                grvValTech.PageSize = CurrentPageSize;
                grvValTech.PageIndex = CurrentPageIndex;
                grvValTech.DataBind();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
