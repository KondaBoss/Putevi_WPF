using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Services
{
    public static class NumberValidationRule
    {
        public static bool Validate(object value)
        {
            int result = 0;
            return int.TryParse(value.ToString(), out result);
        }
    }
}
