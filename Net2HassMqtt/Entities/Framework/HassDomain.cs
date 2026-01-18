using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

public sealed class HassDomain
{
    public HassDomain(string name)
    {
        Name = name;
    }
    public string Name { get; }

    public static implicit operator HassDomain(string hassDomainName)
    {
        return new HassDomain(hassDomainName);
    }
}
