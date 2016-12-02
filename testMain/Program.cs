using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GST_Badge_System.DAO;
using GST_Badge_System.Model;
using System.Reflection;

namespace testMain
{
    class Program
    {
        static void Main(string[] args)
        {
            //MailHelper.SendBadgeNotification("olix.tech@gmail.com", "oliviertyishime@gmail.com");
            //var assembly = Assembly.GetExecutingAssembly().CodeBase;
            UserDAO userdao = new UserDAO();
            //Console.WriteLine("Assembly: " + userdao.importUsers().Count);

            // UserDAO userdao = new UserDAO();
            // List<BadgeTransaction> a = userdao.getUserReceivedBadges(12);
            // List<BadgeTransaction> b = userdao.getUserSentBadges(31);
            // Console.WriteLine("Num: {0}", b.Count());
            //BadgeTransactionDAO bd = new BadgeTransactionDAO();
            //bd.addBadgeTransaction("andy.harbert@oc.edu", "olivier.tuyishime@eagles.oc.edu", "Deep Thinker", "This badges is given to Oliver by Andy Harbert");
            //List<BadgeTransaction> b = userdao.getUserSentBadges(31);
            //Console.WriteLine("Num: {0}", b.Count());

            BadgeTransactionDAO bd = new BadgeTransactionDAO();
            var r = bd.getAllBadgeTransactions();
            Console.WriteLine();
        }
    }
}
