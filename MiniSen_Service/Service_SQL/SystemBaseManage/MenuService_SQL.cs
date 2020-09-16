using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_SQL.SystemBaseManage
{
    public static class MenuService_SQL
    {
        public static string SearchAccountHasNavMenus = @"

            select 
				distinct * 
			from 
				B_MENU T3 
			where 
				Find_IN_SET(T3.ID, (

					select group_concat(T1._IDS) from (

						select 
							@r as _IDS,
							(select @r := group_concat(b.PARENTID) from B_MENU b where Find_IN_SET(b.ID,_ids)) 
						from B_MENU,
							(
								select 
									@r := group_concat(A.MENUID)
								from 
									B_PERMISSION A,
									R_ADMIN_USER_ROLE B,
									R_ROLE_PERMISSION C 
								where 
									A.ID = C.PERMISSIONID and 
									B.ROLEID = C.ROLEID and 
									A.PERMISSIONTYPE = 2 and 
									B.ADMINUSERID = {0} and 
									A.DELFLAG = 0 and B.DELFLAG = 0 and C.DELFLAG = 0
							) T0 
						where @r is not null
					) T1
				))
        ";

    }
}
