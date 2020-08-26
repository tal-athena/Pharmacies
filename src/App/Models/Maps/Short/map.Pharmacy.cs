using AutoMapper;

using Pharmacies.App.Models.Common;

using data = Pharmacies.Models;

namespace Pharmacies.App.Models
{
    public sealed partial class Pharmacy
    {
        public class ShortMap: Profile
        {
            public ShortMap()
            {
                // To VM
                this.CreateMap<data.Pharmacy, NameWithId>()
                    .ForMember(destination => destination.Name,
                        x => x.MapFrom(src => src.Name))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));

                // To Data Moodel
                this.CreateMap<NameWithId, data.Pharmacy>()
                    .ForMember(destination => destination.Name,
                        x => x.MapFrom(src => src.Name))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));

            }
        }
    }
}
