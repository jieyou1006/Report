using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLibrary
{
    public class DBTestClass
    {
        /// <summary>
        /// 驗證用戶名密碼
        /// 返回值含義：
        /// “0”帳號錯誤
        /// “1”密碼錯誤
        /// “2”帳號密碼都正確
        /// </summary>
        private void UserCheck()
        {
            string strUserName = "";
            string strPassword = "";

            string strConn = "DSPG_WEB_CD";
            string strInfoSP = "CHECK_USER_PASSWORD_PK.CHECK_USER_PASSWORD_SP";
            OracleParameter[] opInfo = new OracleParameter[4];
            opInfo[0] = new OracleParameter("V_USER_NAME", OracleDbType.Varchar2, 50);
            opInfo[0].Value = strUserName.ToString().Trim();

            opInfo[1] = new OracleParameter("V_PASSWORD", OracleDbType.Varchar2, 50);
            opInfo[1].Value = strPassword.ToString().Trim();

            opInfo[2] = new OracleParameter("RES", OracleDbType.Varchar2, 50);
            opInfo[2].Direction = ParameterDirection.Output;
            opInfo[3] = new OracleParameter("P_CURSOR", OracleDbType.RefCursor);
            opInfo[3].Direction = ParameterDirection.Output;

            OraDBHelper.ExecuteReader(strInfoSP, CommandType.StoredProcedure, opInfo);

            //DBConection dbInfo = new DBConection(strConn);
            //DataTable dtInfo = dbInfo.GetDataByProcedure(strInfoSP, opInfo).Tables[0];

        }
    }
}
