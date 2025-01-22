using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kutse_App_Vsevolod.Models
{
    public class GuestDBInitializer : CreateDatabaseIfNotExists<GuestContext>
        //DropCreateDatabaseAlways<GuestContext>
    {
        protected override void Seed(GuestContext db)
        {
            base.Seed(db);
        }
    }
}