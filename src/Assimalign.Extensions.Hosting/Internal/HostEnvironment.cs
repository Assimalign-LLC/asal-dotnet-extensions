namespace Assimalign.Extensions.Hosting.Internal
{
    using Assimalign.Extensions.Hosting.Abstractions;
    using Assimalign.Extensions.FileProviders.Abstractions;

    /// <summary>
    /// This API supports infrastructure and is not intended to be used
    /// directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class HostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; }

        public string ContentRootPath { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
