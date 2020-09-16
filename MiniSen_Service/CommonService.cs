using CoffeeSql.Core.QueryEngine;
using MiniSen_Entity;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service
{
    internal class CommonService<TEntity> where TEntity : EntityCommon, new()
    {
        private MiniSenDbContext ctx { get; set; }

        public CommonService(MiniSenDbContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="one"></param>
        /// <returns>插入数据的id</returns>
        public string Insert(TEntity one)
        {
            ctx.Add(one);
            return one.Id;
        }

        /// <summary>
        /// 硬删除数据
        /// </summary>
        /// <param name="one"></param>
        /// <returns>受影响的行数</returns>
        public int RealDelete(TEntity one)
        {
            return ctx.Delete(one);
        }

        /// <summary>
        /// 根据id硬删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>受影响的行数</returns>
        public int RealDeleteById(string id)
        {
            return ctx.Delete<TEntity>(t => t.Id.Equals(id));
        }

        /// <summary>
        /// 根据id软删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>受影响的行数</returns>
        public int MarkDeleteById(string id)
        {
            return ctx.Update<TEntity>(t => new { t.DelFlag }, new TEntity { DelFlag = 1 }).Where(t => t.Id.Equals(id)).Done();
        }

        /// <summary>
        /// 查询所有未被软删除的数据
        /// </summary>
        /// <returns></returns>
        public SqlQueryable<TEntity> GetAllNoMarkDeleted()
        {
            return ctx.Queryable<TEntity>().Select().Where(t => t.DelFlag == 0);
        }

        /// <summary>
        /// 查询指定Id未被软删除的数据
        /// </summary>
        /// <returns></returns>
        public TEntity GetByIdNoMarkDeleted(string id)
        {
            return ctx.Queryable<TEntity>().Select().Where(t => t.Id.Equals(id) && t.DelFlag == 0).ToOne();
        }

        /// <summary>
        /// 判断是否已存在指定Id的未被软删除的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AnyByIdNoMarkDeleted(string id)
        {
            return ctx.Queryable<TEntity>().Where(t => t.Id.Equals(id) && t.DelFlag == 0).Any();
        }

        /// <summary>
        /// 设置where条件为未被软删除
        /// </summary>
        /// <returns></returns>
        public SqlQueryable<TEntity> WhereNoMarkDeleted()
        {
            return ctx.Queryable<TEntity>().Where(t => t.DelFlag == 0);
        }
    }
}
