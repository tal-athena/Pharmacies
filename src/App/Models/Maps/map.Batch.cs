using System;
using System.Globalization;

using AutoMapper;

using data = Pharmacies.Models;

namespace Pharmacies.App.Models
{
    public sealed partial class Batch
    {
        public class Map: Profile
        {
            public Map()
            {
                // To VM
                this.CreateMap<data.Batch, Batch>()
                    .ForMember(destination => destination.Documents,
                        x => x.MapFrom(src => src.Documents))

                    .ForMember(destination => destination.Pharmacy,
                        x => x.MapFrom(src => src.Pharmacy))

                    .ForMember(destination => destination.Arrival,
                        x => x.MapFrom(src =>
                                       src.Arrival.HasValue
                                          ? src.Arrival.Value.ToString("d.M.yyyy")
                                          : string.Empty))

                    .ForMember(destination => destination.Expiery,
                        x => x.MapFrom(src =>
                                       src.Expiery.HasValue
                                          ? src.Expiery.Value.ToString("d.M.yyyy")
                                          : string.Empty))

                    .ForMember(destination => destination.Created,
                        x => x.MapFrom(src => src.Created.ToString("d.M.yyyy")))

                    .ForMember(destination => destination.ProducerName,
                        x => x.MapFrom(src => src.ProducerName))

                    .ForMember(destination => destination.ProducersBatchID,
                        x => x.MapFrom(src => src.ProducersBatchID))

                    .ForMember(destination => destination.ProductName,
                        x => x.MapFrom(src => src.ProductName))

                    .ForMember(destination => destination.ProductType,
                        x => x.MapFrom(src => src.ProductType))

                    .ForMember(destination => destination.Comments,
                        x => x.MapFrom(src => src.Comments))

                    .ForMember(destination => destination.THC,
                        x => x.MapFrom(src => src.THC))

                    .ForMember(destination => destination.CBD,
                        x => x.MapFrom(src => src.CBD))

                    .ForMember(destination => destination.CBG,
                        x => x.MapFrom(src => src.CBG))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));

                // To Data Moodel
                this.CreateMap<Batch, data.Batch>()
                    .ForMember(destination => destination.Documents,
                        x => x.Ignore())

                    .ForMember(destination => destination.PharmacyId,
                        x => x.MapFrom(src => src.Pharmacy.Id))

                    .ForMember(destination => destination.Pharmacy,
                        x => x.Ignore())

                    .ForMember(destination => destination.Arrival,
                        x => x.MapFrom(src =>
                              !string.IsNullOrWhiteSpace(src.Arrival)
                                     ?  DateTime.ParseExact(src.Arrival, "d.M.yyyy", CultureInfo.InvariantCulture)
                                     : (DateTime?) null))

                    .ForMember(destination => destination.Expiery,
                        x => x.MapFrom(src =>
                              !string.IsNullOrWhiteSpace(src.Expiery)
                                     ?  DateTime.ParseExact(src.Expiery, "d.M.yyyy", CultureInfo.InvariantCulture)
                                     : (DateTime?) null))

                    .ForMember(destination => destination.Created,
                        x => x.Ignore())

                    .ForMember(destination => destination.ProducerName,
                        x => x.MapFrom(src => src.ProducerName))

                    .ForMember(destination => destination.ProducersBatchID,
                        x => x.MapFrom(src => src.ProducersBatchID))

                    .ForMember(destination => destination.ProductName,
                        x => x.MapFrom(src => src.ProductName))

                    .ForMember(destination => destination.ProductType,
                        x => x.MapFrom(src => src.ProductType))

                    .ForMember(destination => destination.Comments,
                        x => x.MapFrom(src => src.Comments))

                    .ForMember(destination => destination.THC,
                        x => x.MapFrom(src => src.THC))

                    .ForMember(destination => destination.CBD,
                        x => x.MapFrom(src => src.CBD))

                    .ForMember(destination => destination.CBG,
                        x => x.MapFrom(src => src.CBG))

                    .ForMember(destination => destination.Id,
                        x => x.MapFrom(src => src.Id));
            }
        }
    }
}
