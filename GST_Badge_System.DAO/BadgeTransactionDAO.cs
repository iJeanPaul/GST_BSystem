using GST_Badge_System.Model;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GST_Badge_System.DAO
{
	public class BadgeTransactionDAO
	{
		private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=gst_badge_system;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";



		// add badge transaction: More like send a badge
		public int addBadgeTransaction(string sender, string receiver, string badge, string comment)
		{
			if( !String.IsNullOrEmpty(sender) && !String.IsNullOrEmpty(receiver) && 
				!String.IsNullOrEmpty(badge) && !String.IsNullOrEmpty(comment))
			{
				var badgesender = new UserDAO()[sender].User_Id;
				var badgeReceiver = new UserDAO()[receiver].User_Id;
				var sentbadge = new BadgeDAO()[badge].Badge_Id;
				var sendcomment = comment;
				var datetime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

				using(IDbConnection conn = new SqlConnection(connectionString))
				{
					string sql = @"INSERT INTO BadgeTransaction (Sender, Reciever, Badge_Id, BTrans_Date, Badge_Comment) 
									VALUES ( @badgesender, @badgeReceiver, @sentbadge, @datetime, @sendcomment)";
					return conn.Execute(sql, new { badgesender, badgeReceiver , sentbadge, datetime, sendcomment});
				}
			}
			else
			{
				throw new Exception("Failed to send the badge. One or more parameters are wrong.");
			}
		}

		public List<BadgeTransaction> retrieveBadgeTransactions()
		{
			using(var conn = new SqlConnection(connectionString))
			{
				string sql = @"SELECT DISTINCT Users.*, A.*, B.*, BadgeTransaction.BTrans_Date, BadgeTransaction.Badge_Comment 
								FROM (SELECT Users.* FROM Users, BadgeTransaction WHERE Users.User_Id = BadgeTransaction.Reciever) AS A,
								(SELECT Badge.Badge_Image, Badge.Badge_RetireDate, Badge.Badge_ActivateDate, Badge.Badge_Id, 
								Badge_Name, Badge.Badge_Descript, Badge.Badge_Notes,BadgeType.*,BadgeGiveType.*, BadgeStatus.* 
								FROM Badge, BadgeGiveType, BadgeStatus, BadgeType
								WHERE Badge.BadgeGiveType = BadgeGiveType.BGT_Id AND Badge.BadgeStatus = BadgeStatus.BS_Id 
								AND Badge.BadgeStatus = BadgeType.BT_Id) AS B, 
								Users, BadgeTransaction, Badge 
								WHERE Users.User_Id = BadgeTransaction.Sender AND B.Badge_Id = BadgeTransaction.Badge_Id";

				var result = conn.Query<User, User, Badge, BadgeType, BadgeGiveType, BadgeStatus , BadgeTransaction, BadgeTransaction>(sql, 
					(usend, urec, b, bt, bgt, bs, btrans) => {

						b.BadgeType_Object = bt;
						b.BadgeGiveType_Object = bgt;
						b.Badge_Status = bs;

						// btrans.Sender = usend;
						// btrans.Receiver = urec;
                        btrans.Sender_Object = usend;
                        btrans.Receiver_Object = urec;
                        btrans.Badge = b;

						return btrans;
					}, splitOn: "User_Id, Badge_Image, BT_Name, BGT_Name, BS_Name, BTrans_Date"
					).AsList();
				return result;
			}
		}

        public List<BadgeTransaction> getAllBadgeTransactions()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"select * from BadgeTransaction";
                var result = conn.Query<BadgeTransaction>(sql).AsList();

                foreach (var bt in result)
                {
                    UserDAO userdao = new UserDAO();
                    BadgeDAO badgedao = new BadgeDAO();
                    bt.Sender_Object = userdao.findPersonGivenId(bt.Sender);
                    bt.Receiver_Object = userdao.findPersonGivenId(bt.Reciever);
                    bt.Badge = badgedao.findBadgeGivenId(bt.Badge_Id);
                }

                return result;
            }
        }

        // this funciton gets an badge transaciton when given an id
        public BadgeTransaction getBTGivenId(int Id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"select * from BadgeTransaction where bt_id = @BT_Id";
                return conn.Query<BadgeTransaction>(sql, new { BT_Id = Id }).FirstOrDefault();
            }
        }

        public void updateBadgeTransaction(int Id, string receiver, string badge, string comment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var badgeReceiver = new UserDAO()[receiver].User_Id;
                var sentbadge = new BadgeDAO()[badge].Badge_Id;
                string sql = @"update BadgeTransaction set Reciever = @badgeReceiver, Badge_Id = @sentbadge, Badge_Comment = @comment where bt_id = @Id";
                conn.Query(sql, new { badgeReceiver  = badgeReceiver, sentbadge= sentbadge, comment = comment, Id = Id });
            }
        }

        public void deleteBadgeTransaction(int Id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"delete from BadgeTransaction where bt_id = @Id";
                conn.Query(sql, new { Id = Id });
            }
        }
    }
}
