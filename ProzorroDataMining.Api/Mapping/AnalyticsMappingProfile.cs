using AutoMapper;
using ProzorroDataMining.Application.DTOs;
using ProzorroDataMining.Domain.Results;

namespace ProzorroDataMining.Api.Mapping
{
    public class AnalyticsMappingProfile : Profile
    {
        public AnalyticsMappingProfile()
        {
            // Map AnalyticsResult to ProcurerStatsDto
            CreateMap<AnalyticsResult, ProcurerStatsDto>()
                .ForMember(dest => dest.ProcuringEntity, opt => opt.MapFrom(src => src.ProcuringEntity))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            // Map AnalyticsResult to SupplierStatsDto
            CreateMap<AnalyticsResult, SupplierStatsDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            // Map AnalyticsResult to SavingsDto
            CreateMap<AnalyticsResult, SavingsDto>()
                .ForMember(dest => dest.TotalSavings, opt => opt.MapFrom(src => src.Savings));
        }
    }
}
