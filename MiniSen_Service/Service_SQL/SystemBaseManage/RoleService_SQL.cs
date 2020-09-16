using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_SQL.SystemBaseManage
{
    /// <summary>
    /// 存放RoleService中的所有sql
    /// </summary>
    public static class RoleService_SQL
    {
        public static string SearchAllRoleInfo = @"

            select 
                A.*,
                (select 
                    group_concat(M.ID)
                from 
                    B_PERMISSION M,
                    R_ROLE_PERMISSION N 
                where 
                    M.ID = N.PERMISSIONID and 
                    N.ROLEID = A.ID and 
                    M.PERMISSIONTYPE = 1 and 
                    M.DELFLAG = 0 and N.DELFLAG = 0) as HAS_API_PERMIDS,
                (select 
                    group_concat(M.MENUID)
                from 
                    B_PERMISSION M,
                    R_ROLE_PERMISSION N 
                where 
                    M.ID = N.PERMISSIONID and 
                    N.ROLEID = A.ID and 
                    M.PERMISSIONTYPE = 2 and 
                    M.DELFLAG = 0 and N.DELFLAG = 0) as HAS_MENUIDS 
            from 
                B_ROLE A 
            where 
                A.DELFLAG = 0
        ";

        public static string MarkDeleteAllApiPermsOfRole = @"
        
            update 
                R_ROLE_PERMISSION 
            set 
                DELFLAG = 1 
            where 
                ID in (
                    select A._ID from (
                        select 
                            A.ID as _ID 
                        from 
                            R_ROLE_PERMISSION A,
                            B_PERMISSION B 
                        where 
                            A.PERMISSIONID = B.ID and 
                            A.ROLEID = {0} and 
                            B.PERMISSIONTYPE = 1 and 
                            A.DELFLAG = 0 and B.DELFLAG = 0
                    ) A 
                )
        ";

        public static string MarkDeleteAllMenuPermsOfRole = @"
        
            update 
                R_ROLE_PERMISSION 
            set 
                DELFLAG = 1 
            where 
                ID in (
                    select A._ID from (
                        select 
                            A.ID as _ID 
                        from 
                            R_ROLE_PERMISSION A,
                            B_PERMISSION B 
                        where 
                            A.PERMISSIONID = B.ID and 
                            A.ROLEID = {0} and 
                            B.PERMISSIONTYPE = 2 and 
                            A.DELFLAG = 0 and B.DELFLAG = 0
                    ) A 
                )
        ";

    }
}
