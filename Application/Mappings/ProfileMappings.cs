using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entity;

namespace Application.Mappings;

public class ProfileMappings : Profile
{
    // OBS: Essa classe não pode receber a injeção do AuthService para gerar o token
    public ProfileMappings()
    {
        // CreateMap<ClasseDeOrigemOuRecurso, ClasseDeDestinoASerMapeado>
        // No CommandHandler passamos os dados:
        // CreateUserCommand (que é o request) -> User (sava no banco) -> UserInfoViewModel (DTO de saída)
        // ForMember - usado quando existem campos nulos entre propriedades
        // MapFrom fará uma atribuição ao campo RefreshToken
        CreateMap<CreateUserCommand, User>()
            .ForMember(x => x.PasswordHash, x => x.MapFrom(x => x.Password))
            .ForMember(x => x.RefreshToken, x => x.MapFrom(x => GenerateGuid()))
            .ForMember(x => x.RefreshTokenExpirationTime, x => x.MapFrom(x => AddFiveDays()));

        // Recebe objeto User -> transforma para -> UserInfoViewModel
        CreateMap<User, UserInfoViewModel>()
            .ForMember(x => x.TokenJWT, x => x.AllowNull());
    }

    private DateTime AddFiveDays()
    {
        return DateTime.Now.AddDays(5);
    }

    private string GenerateGuid()
    {
        return Guid.NewGuid().ToString();
    }
}
