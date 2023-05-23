using NgSoftware.Specular.Context;

namespace NgSoftware.Specular.Api.Infrastructures;

public sealed partial class SpecularConfiguration : ISpecularContextConfiguration
{
    string ISpecularContextConfiguration.ConnectionString
        => configuration.GetConnectionString("SpecularConnection");
}
