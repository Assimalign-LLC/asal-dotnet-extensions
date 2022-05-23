using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling.Crontab
{
    public readonly struct CrontabValue
    {
        private readonly string second;
        private readonly string minute;
        private readonly string hour;
        private readonly string month;
        private readonly string weekday;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">The cron expreesion to be parsed.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CrontabValue(string expression)
        {
            var segments = expression.Split(' ');

            if (segments.Length == 6 || segments.Length == 5)
            {
                second = "";
                minute = "";
                hour = "";
                month = "";
                weekday = "";
            }
            else
            {
                throw new ArgumentOutOfRangeException(expression, $"The Cron Expression has only {segments} segments and must only have exactly 6 or 5 segments.");
            }
        }


        public DateTime Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
