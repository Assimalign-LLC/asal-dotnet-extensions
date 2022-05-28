using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public sealed class JobScheduleBuilder
{



    public JobScheduleBuilder ConfigureContext(Func<IJobContext> configure)
    {

        return this;
    }

}
