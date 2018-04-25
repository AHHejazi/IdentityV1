using Framework.Core.Data;
using Framework.Core.Model;
using System.Threading.Tasks;

namespace Framework.Core.LogProvider
{
    public class LogHelper
    {
       
        public async Task InsertLog(EventLog log)
        {
            using (var context = new CommonsDbEntities())
            {
               context.EventLog.Add(log);
              await context.SaveChangesAsync();
            }
            
        }


        public void InsertLog(string log)
        {
            using (var context = new CommonsDbEntities())
            {
                
            }

        }
    }
}
