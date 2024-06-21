using ProjectName.Application.Authentication.Commands.Register;
using ProjectName.Application.Authentication.Dtos;
using ProjectName.Application.Authentication.Queries.Login;
using ProjectName.Presentation.Authentication.Dtos;

using Mapster;

namespace ProjectName.Presentation.Authentication.Mappings;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LoginRequest, LoginQuery>();
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest.Id, src => src.User.Id.Value)
            .Map(dest => dest, src => src.User);
    }
}