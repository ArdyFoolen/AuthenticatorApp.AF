using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorApp.AF
{
    public interface IDateService
    {
        DateTime GetUtcNow();
    }

    public class DateService : IDateService
    {
        public DateTime GetUtcNow()
            => DateTime.UtcNow;
    }
}
