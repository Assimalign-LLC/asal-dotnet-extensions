using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling.Crontab
{
    public sealed class CrontabSerialization
    {


        public DateTime Current { get; }

        public long CurrntEpoch { get; }



        /// <summary>
        /// Enumerate the occurrences from current ocurence.
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<DateTime> Occurances(int max)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerate the occurrences from current.
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<DateTime> Occurances(DateTime max)
        {
            throw new NotImplementedException();
        }

    }
}
