using AutoMapper;

using Pharmacies.Domain.Providers;

using data = Pharmacies.Models;

namespace Pharmacies.App.Models
{
    public sealed partial class User
    {
        public class Map: Profile
        {
            public Map()
            {
                // To VM
                this.CreateMap<data.User, User>()
                    .ForMember(destination => destination.Pharmacy,
                        x => x.MapFrom(src => src.Pharmacy))

                    .ForMember(destination => destination.NewPassword,
                        x => x.Ignore())

                    .ForMember(destination => destination.FirstName,
                        x => x.MapFrom(src => src.FirstName))

                    .ForMember(destination => destination.LastName,
                        x => x.MapFrom(src => src.LastName))

                    .ForMember(destination => destination.UserName,
                        x => x.MapFrom(src => src.UserName))

                    .ForMember(destination => destination.IsSuper,
                        x => x.MapFrom(src => src.IsSuperUser))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));

                // To Data Moodel
                this.CreateMap<User, data.User>()
                    .ForMember(destination => destination.PharmacyId,
                        x => x.MapFrom(src => src.Pharmacy.Id == "-1" ? null : src.Pharmacy.Id))

                    .ForMember(destination => destination.Pharmacy,
                        x => x.Ignore())

                    .ForMember(destination => destination.FirstName,
                        x => x.MapFrom(src => src.FirstName))

                    .ForMember(destination => destination.LastName,
                        x => x.MapFrom(src => src.LastName))

                    .ForMember(destination => destination.UserName,
                        x => x.MapFrom(src => src.UserName))

                    .ForMember(destination => destination.IsSuperUser,
                        x => x.MapFrom(src => src.IsSuper))

                    .ForMember(destination => destination.Password,
                        x => x.Ignore())

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id))

                    .AfterMap((src, destination) =>
                     {
                         if(!string.IsNullOrEmpty(src.NewPassword))
                             destination.Password = PasswordProvider.Encrypt(src.NewPassword);

                     });
            }
        }
    }
}
