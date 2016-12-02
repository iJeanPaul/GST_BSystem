using System;
using System.Collections.Generic;
using Dapper;
using GST_Badge_System.Model;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace GST_Badge_System.DAO
{
    public class BadgeTypeDAO
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=gst_badge_system;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // add a list of badges to be used as an indexer
        private List<BadgeType> badgetypelist;

        // add an indexer
        public BadgeType this[string badgetypename]
        {
            get
            {
                badgetypelist = retrieveBadgeTypes();
                return badgetypelist.FirstOrDefault((bt) => bt.BT_Name == badgetypename);
            }
        }

        // Retrieve badge types
        public List<BadgeType> retrieveBadgeTypes()
        {
            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                return conn.Query<BadgeType>("Select * From BadgeType").AsList();
            }
        }

        // add a badge type
        public int addBadgeType(BadgeType bt)
        {
            if (bt == null)
            {
                throw new Exception("Failed to add the badge type. Passed wrong badge type.");
            }

            string name = bt.BT_Name
                 , descript = bt.BT_Descript;

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO BadgeType (BT_Name, BT_Descript) VALUES ( @name , @descript )";
                return conn.Execute(sql, new { name, descript });
            }

        }

        // delete badge type
        public int deleteBadgeType(string btName)
        {
            if (String.IsNullOrEmpty(btName))
            {
                throw new Exception("Failed to delete the badge type. Passed wrong badge name.");
            }

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"DELETE FROM BadgeType WHERE BT_Name = @btName";
                return conn.Execute(sql, new { btName });
            }
        }

        // Update badge type
        public int updateBadgeType(BadgeType bt, string oldBTName)
        {
            if (bt == null || String.IsNullOrEmpty(oldBTName))
            {
                throw new Exception("Failed to update the badge give type. Passed in wrong badge type/");
            }

            string name = bt.BT_Name
                 , descript = bt.BT_Descript;

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE BadgeType SET BT_Name = @name, BT_Descript = @descript WHERE BT_Id = 
                                (select BadgeType.BT_Id from BadgeType where BT_Name = @oldBTName)";
                return conn.Execute(sql, new { name, descript, oldBTName });
            }
        }
    }
}
