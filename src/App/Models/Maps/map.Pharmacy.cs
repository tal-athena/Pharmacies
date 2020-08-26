using AutoMapper;

using data = Pharmacies.Models;

namespace Pharmacies.App.Models
{
    public sealed partial class Pharmacy
    {
        public class Map: Profile
        {
            public Map()
            {
                // To VM
                this.CreateMap<data.Pharmacy, Pharmacy>()
                    .ForMember(destination => destination.Name,
                        x => x.MapFrom(src => src.Name))

                    .ForMember(destination => destination.Address,
                        x => x.MapFrom(src => src.Address))

                    .ForMember(destination => destination.Zip,
                        x => x.MapFrom(src => src.Zip))

                    .ForMember(destination => destination.City,
                        x => x.MapFrom(src => src.City))

                    .ForMember(destination => destination.Phone,
                        x => x.MapFrom(src => src.Phone))

                    .ForMember(destination => destination.Country,
                        x => x.MapFrom(src => src.Country))

                    .ForMember(destination => destination.Email,
                        x => x.MapFrom(src => src.Email))

                    .ForMember(destination => destination.Contact,
                        x => x.MapFrom(src => src.Contact))

                    .ForMember(destination => destination.Logo,
                        x => x.MapFrom(src => src.Logo))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));

                // To Data Moodel
                this.CreateMap<Pharmacy, data.Pharmacy>()
                    .ForMember(destination => destination.Name,
                        x => x.MapFrom(src => src.Name))

                    .ForMember(destination => destination.Address,
                        x => x.MapFrom(src => src.Address))

                    .ForMember(destination => destination.Zip,
                        x => x.MapFrom(src => src.Zip))

                    .ForMember(destination => destination.City,
                        x => x.MapFrom(src => src.City))

                    .ForMember(destination => destination.Phone,
                        x => x.MapFrom(src => src.Phone))

                    .ForMember(destination => destination.Country,
                        x => x.MapFrom(src => src.Country))

                    .ForMember(destination => destination.Email,
                        x => x.MapFrom(src => src.Email))

                    .ForMember(destination => destination.Contact,
                        x => x.MapFrom(src => src.Contact))

                    .ForMember(destination => destination.Logo,
                        x => x.MapFrom(src => src.Logo))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));
            }
        }
    }
}
