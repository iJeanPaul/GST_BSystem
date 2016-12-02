using System;
using System.Collections.Generic;
using Dapper;
using GST_Badge_System.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GST_Badge_System.DAO
{
    public class BadgeStatusDAO
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=gst_badge_system;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // add a list of badge statuses to be used as an indexer
        private List<BadgeStatus> badgestatuslist;

        // add an indexer
        public BadgeStatus this[string badgestatusname]
        {
            get
            {
                badgestatuslist = retrieveBadgeTypes();
                return badgestatuslist.FirstOrDefault((bt) => bt.BS_Name == badgestatusname);
            }
        }

        // Retrieve badge statuses
        public List<BadgeStatus> retrieveBadgeTypes()
        {
            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                return conn.Query<BadgeStatus>("Select * From BadgeStatus").AsList();
            }
        }

        // add a badge status
        public int addBadgeStatus(BadgeStatus bs)
        {
            if (bs == null)
            {
                throw new Exception("Failed to add the badge status. Passed wrong badge status.");
            }

            string name = bs.BS_Name
                 , descript = bs.BS_Descript;

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO BadgeStatus (BS_Name, BS_Descript) VALUES ( @name , @descript )";
                return conn.Execute(sql, new { name, descript });
            }

        }

        // delete badge status
        public int deleteBadgeStatus(string bsName)
        {
            if (String.IsNullOrEmpty(bsName))
            {
                throw new Exception("Failed to delete the badge status. Passed wrong badge status name.");
            }

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"DELETE FROM BadgeStatus WHERE BS_Name = @bsName";
                return conn.Execute(sql, new { bsName });
            }
        }

        // Update badge status
        public int updateBadgeStatus(BadgeStatus bs, string oldBSName)
        {
            if (bs == null || String.IsNullOrEmpty(oldBSName))
            {
                throw new Exception("Failed to update the badge status. Passed in wrong badge status");
            }

            string name = bs.BS_Name
                 , descript = bs.BS_Descript;

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE BadgeStatus SET BS_Name = @name, BS_Descript = @descript WHERE BS_Id = 
                                (select BadgeStatus.BS_Id from BadgeStatus where BS_Name = @oldBSName)";
                return conn.Execute(sql, new { name, descript, oldBSName });
            }
        }
    }
}
