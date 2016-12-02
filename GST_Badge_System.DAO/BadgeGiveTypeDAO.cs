using System;
using System.Collections.Generic;
using Dapper;
using GST_Badge_System.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GST_Badge_System.DAO
{
    public class BadgeGiveTypeDAO
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=gst_badge_system;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // add a list of badges to be used as an indexer
        private List<BadgeGiveType> badgegivetypelist;

        // add an indexer
        public BadgeGiveType this[string badgegivetypename]
        {
            get
            {
                badgegivetypelist = retrieveBadgeGiveTypes();
                return badgegivetypelist.FirstOrDefault((bt) => bt.BGT_Name == badgegivetypename);
            }
        }

        // Retrieve badge give types
        public List<BadgeGiveType> retrieveBadgeGiveTypes()
        {
            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                return conn.Query<BadgeGiveType>("Select * From BadgeGiveType").AsList();
            }
        }

        // add a badge give type
        public int addBadgeGiveType(BadgeGiveType bgt)
        {
            if ( bgt == null)
            {
                throw new Exception("Failed to add the badge give type. Passed wrong badge give type.");
            }

            string name = bgt.BGT_Name
                 , descript = bgt.BGT_Descript;

            using( var conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO BadgeGiveType (BGT_Name, BGT_Descript) VALUES ( @name , @descript )";
                return conn.Execute(sql, new { name, descript });
            }

        }

        // delete badge give type
        public int deleteBadgeGiveType(string bgtName)
        {
            if (String.IsNullOrEmpty(bgtName))
            {
                throw new Exception("Failed to delete the badge give type. Passed wrong badge name.");
            }

            using( var conn = new SqlConnection(connectionString))
            {
                string sql = @"DELETE FROM BadgeGiveType WHERE BGT_Name = @bgtName";
                return conn.Execute(sql, new { bgtName});
            }
        }

        // Update badge give type
        public int updateBadgeGiveType(BadgeGiveType bgt, string oldBGTName)
        {
            if( bgt == null || String.IsNullOrEmpty(oldBGTName))
            {
                throw new Exception("Failed to update the badge give type. Passed in wrong badge type/");
            }

            string name = bgt.BGT_Name
                 , descript = bgt.BGT_Descript;

            using ( var conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE BadgeGiveType SET BGT_Name = @name, BGT_Descript = @descript WHERE BGT_Id = 
                                (select BadgeGiveType.BGT_Id from BadgeGiveType where BGT_Name = @oldBGTName)";
                return conn.Execute(sql, new { name, descript, oldBGTName});
            }
        }
    }
}