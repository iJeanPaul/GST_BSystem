using GST_Badge_System.Model;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using CsvHelper;
using System.Data;
using System.Linq;

namespace GST_Badge_System.DAO
{
	/*
	 This class will be used to read/write all the badge data from the data database
	 */
	public class BadgeDAO //: IcrudOperations<Badge>
	{
		private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=gst_badge_system;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

		// add a list of users to be used as an indexer
		private List<Badge> badgelist;

		// add an indexer
		public Badge this[string badgename]
		{
			get
			{
				badgelist = list();
				return badgelist.FirstOrDefault((b) => b.Badge_Name == badgename);
			}
		}

		public void create(Badge badge)
		{

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Badge
									VALUES ( @Badge_Image, @Badge_RetireDate, @Badge_ActivateDate, @Badge_Name, @Badge_Descript, @Badge_Notes, @BadgeType, @BadgeGiveType, @BadgeStatus)";
                dbConnection.Query(sql, badge);
            }
		}

        public Badge findBadgeGivenId(int Id)
        {
            using (IDbConnection dbConnection=new SqlConnection(connectionString))
            {
                String sql = "SELECT * FROM BADGE WHERE Badge_Id=@Badge_Id";
                var badge = dbConnection.Query<Model.Badge>(sql, new { Badge_Id = Id }).FirstOrDefault();
                return badge;
            }

        }
		public void delete(int Id, Badge badge)
		{
            using (IDbConnection connection=new SqlConnection(connectionString))
            {
                connection.Query<Badge>("DELETE FROM BADGE WHERE Badge_Id=@Badge_Id",new{Badge_Id=Id});

            }
		}

		public List<Badge> list()
		{
			using (IDbConnection conn = new SqlConnection(connectionString))
			{
				string sql = @"SELECT DISTINCT Badge.Badge_Image, Badge.Badge_RetireDate, Badge.Badge_ActivateDate, Badge.Badge_Id, 
										Badge_Name, Badge.Badge_Descript, Badge.Badge_Notes,BadgeType.*,BadgeGiveType.*, BadgeStatus.* 
								FROM Badge, BadgeGiveType, BadgeStatus, BadgeType
								WHERE Badge.BadgeGiveType = BadgeGiveType.BGT_Id AND Badge.BadgeStatus = BadgeStatus.BS_Id 
								AND Badge.BadgeStatus = BadgeType.BT_Id";

				var result = conn.Query<Badge, BadgeType, BadgeGiveType, BadgeStatus, Badge>(sql,
					(b,bt,bgt,bs) => {
						b.BadgeType_Object = bt;
						b.BadgeGiveType_Object = bgt;
						b.Badge_Status = bs;

						return b;
					},
					splitOn:"BT_Name, BGT_Name, BS_Name"
					).AsList();
				return result;
			}
		}

		public Badge retrieve(string id)
		{
			throw new NotImplementedException();
		}

		public Badge update(Badge badge)
		{
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Query<Badge>("UPDATE BADGE SET " +
                                              "Badge_Name=@Badge_Name," +
                                              "Badge_Descript=@Badge_Descript," +
                                              "Badge_Notes=@Badge_Notes,"+
                                              "Badge_Image=@Badge_Image,"+
                                              "Badge_ActivateDate=@Badge_ActivateDate,"+
                                              "Badge_RetireDate=@Badge_RetireDate,"+
                                              "BadgeType=@BadgeType" + 
                              " WHERE Badge_Id= @Badge_Id", badge);
                return null;
            }
		}

		public List<Badge> ImportBadges(string filePath)
		{
			List<Badge> badges = new List<Badge>();

			// Read the file and display it line by line.
			using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
			{
				var csv = new CsvReader(file);

				while (csv.Read())
				{
					Badge temp_badge = new Badge();

					var number = csv.GetField<int>("Badge#");
					var name = csv.GetField<string>("Badge Name");
					var descript = csv.GetField<string>("Badge Summary");
					var dateActive = csv.GetField<string>("Date Activated");
					var dateRetire = csv.GetField<string>("Date Retired");
					var notes = csv.GetField<string>("Notes");
					var imageURL = csv.GetField<string>("Image website address");

					temp_badge.Badge_Id = number;
					temp_badge.Badge_Name = name;
					temp_badge.Badge_Descript = descript;
					temp_badge.Badge_Image = imageURL;

					if (!String.IsNullOrEmpty(dateActive))
					{
						temp_badge.Badge_ActivateDate = Convert.ToDateTime(dateActive);
					}
					
					if (dateRetire != "")
					{
						temp_badge.Badge_RetireDate = Convert.ToDateTime(dateRetire);
					}

					if (!String.IsNullOrEmpty(notes))
					{
						temp_badge.Badge_Notes = notes;
					}

					// add the badge to the list
					badges.Add(temp_badge);
				}
			}

			return badges;
		}

		// push badges to database
		private void pushBadgeHelper(IDbConnection conn, string badgetypename, string filepath2)
		{
			foreach (Badge badge in ImportBadges(filepath2))

			{
				string image, name, descript, notes, activedate, retiredate;
				int number, givetypeid, statusid;

				number = badge.Badge_Id;
				name = badge.Badge_Name;
				descript = badge.Badge_Descript;
				notes = badge.Badge_Notes;
				image = badge.Badge_Image;
				activedate = badge.Badge_ActivateDate.ToShortDateString();
				givetypeid = new BadgeGiveTypeDAO()[badgetypename].BGT_Id;
				statusid = new BadgeStatusDAO()["Active"].BS_Id;
				retiredate = null;

				DateTime flag = new DateTime();
				if (badge.Badge_RetireDate != flag)
				{
					retiredate = badge.Badge_RetireDate.ToShortDateString();
					statusid = new BadgeStatusDAO()["DeActivated"].BS_Id;
				}

				string sql = @"INSERT INTO Badge (Badge_Name, Badge_Descript, Badge_ActivateDate, Badge_RetireDate, 
									Badge_Notes, Badge_Image, BadgeGiveType, BadgeStatus) 
									VALUES ( @name, @descript, @activedate, @retiredate, @notes, @image, @givetypeid,
												@statusid);";
				conn.Execute(sql, new {name, descript, activedate, retiredate, notes, image, givetypeid, statusid });
			}
		}

		// upload badges to the database
		public int uploadBadges()
		{
			using (var conn = new SqlConnection(connectionString))
			{
				// get the badge give type
				// Student to peer
				string path = GetDirectory.getFilePath() + @"\GST_Badge_System.DAO\Data\Student-Peer.csv";
				pushBadgeHelper(conn, "Student to peer", path);

				// Student to self
				path = GetDirectory.getFilePath() + @"\GST_Badge_System.DAO\Data\Student-Self.csv";
				pushBadgeHelper(conn, "Student to self", path);

				// Faculty to student
				path = GetDirectory.getFilePath() + @"\GST_Badge_System.DAO\Data\Faculty-Student.csv";
				pushBadgeHelper(conn, "Faculty to student", path);

				// Staff to student
				path = GetDirectory.getFilePath() + @"\GST_Badge_System.DAO\Data\Staff-Student.csv";
				pushBadgeHelper(conn, "Staff to student", path);

			}
			return 1;
		}
	}
}
