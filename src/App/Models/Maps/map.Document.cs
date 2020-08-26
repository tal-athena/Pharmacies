using AutoMapper;

using data = Pharmacies.Models;

namespace Pharmacies.App.Models
{
    public sealed partial class Document
    {
        public class Map: Profile
        {
            public Map()
            {
                // To VM
                this.CreateMap<data.Document, Document>()
                    .ForMember(destination => destination.Display,
                        x => x.MapFrom(src => src.DisplayName))

                    .ForMember(destination => destination.Actual,
                        x => x.MapFrom(src => src.ActualName))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));

                // To Data Moodel
                this.CreateMap<Document, data.Document>()
                    .ForMember(destination => destination.DisplayName,
                        x => x.MapFrom(src => src.Display))

                    .ForMember(destination => destination.ActualName,
                        x => x.MapFrom(src => src.Actual))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));
            }
        }
    }
}
