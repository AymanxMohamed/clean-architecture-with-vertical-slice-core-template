using Core.Application.Authentication.Commands.Register;
using Core.Application.Authentication.Common;
using Core.Application.Authentication.Queries.Login;
using Core.Presentation.Authentication.Dtos;

using Mapster;

namespace Core.Presentation.Authentication.Mappings;

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