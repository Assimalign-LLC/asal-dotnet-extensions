using System;
using System.Collections.Generic;
using Xunit;

namespace Assimalign.Extensions.Scheduling.Tests;

using Assimalign.Extensions.Scheduling;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        
        var test = Crontab.Parse("1-59/2 * * * *");

        var schedule = CrontabSchedule.Parse("1-59/2 * * * *");
    }
}


