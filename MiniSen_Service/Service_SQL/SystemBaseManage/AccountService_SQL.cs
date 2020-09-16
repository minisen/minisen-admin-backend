using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_SQL.SystemBaseManage
{
    /// <summary>
    /// 存放AccountService中的所有sql
    /// </summary>
    public static class AccountService_SQL
    {
        public static string GetLoginUserInfo = @"

            select 
                A.ID,
                A.ACCOUNT,
                B.NAME,
                (select group_concat(NAME) from B_ROLE where ID in (select ROLEID from R_ADMIN_USER_ROLE where ADMINUSERID = {0} and DELFLAG = 0)) as ROLESNAME
            from 
                B_ADMIN_USER A,
                B_ADMIN_USER_INFO B 
            where 
                A.ID = B.ADMINUSERID and 
                A.ID = {0} and 
                A.DELFLAG = 0 and B.DELFLAG = 0 
        ";

        public static string SearchAllAccountInfo = @"

            select 
                A.*,
                B.NAME,
                (select group_concat(ROLEID) from R_ADMIN_USER_ROLE where ADMINUSERID = A.ID and DELFLAG = 0) as HASROLEIDS,
                (select group_concat(NAME) from B_ROLE where ID in (select ROLEID from R_ADMIN_USER_ROLE where ADMINUSERID = A.ID and DELFLAG = 0)) as ROLESNAME
            from 
                B_ADMIN_USER A,
                B_ADMIN_USER_INFO B 
            where 
                A.ID = B.ADMINUSERID and 
                A.DELFLAG = 0 and B.DELFLAG = 0 
        ";

        public static string JudgeIfAccountHasApiPerm = @"
            
            select 
                1 
            from 
                R_ADMIN_USER_ROLE A,
                R_ROLE_PERMISSION B,
                B_PERMISSION C 
            where 
                C.APIURL = {0} and 
                C.ID = B.PERMISSIONID and 
                A.ROLEID = B.ROLEID and 
                A.ADMINUSERID = {1} and 
                A.DELFLAG = 0 and B.DELFLAG = 0 and C.DELFLAG = 0 
        ";
    }
}
