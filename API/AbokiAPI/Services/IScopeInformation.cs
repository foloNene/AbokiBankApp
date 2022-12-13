using System.Collections.Generic;

namespace AbokiAPI.Services
{
    public interface IScopeInformation
    {
        Dictionary<string, string> HostScopeInfo { get; }
    }
}