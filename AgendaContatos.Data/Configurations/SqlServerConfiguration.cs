using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Configurations
{
    public class SqlServerConfiguration
    {
     public static string GetConnectionString()
        {
            return @"Data Source=SQL8004.site4now.net;Initial Catalog=db_a8c15b_bdagendacontatos;User Id=db_a8c15b_bdagendacontatos_admin;Password=YOUR_DB_PASSWORD";
        }
    }
}
