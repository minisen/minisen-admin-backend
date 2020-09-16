using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_SQL.SystemBaseManage
{
    public static class PermissionService_SQL
    {
        public static string SearchAccountHasApiPerms = @"

            select 
                distinct A.* 
            from 
                B_PERMISSION A,
                R_ADMIN_USER_ROLE B,
                R_ROLE_PERMISSION C 
            where 
                A.PERMISSIONTYPE = 1 and 
                A.ID = C.PERMISSIONID and 
                B.ROLEID = C.ROLEID and 
                B.ADMINUSERID = {0} and
                A.DELFLAG = 0 and B.DELFLAG = 0 and C.DELFLAG = 0
        ";
    }
}
