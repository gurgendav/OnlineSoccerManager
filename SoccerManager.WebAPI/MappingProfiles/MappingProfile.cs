using AutoMapper;
using SoccerManager.Application.Entities;
using SoccerManager.WebAPI.Models;

namespace SoccerManager.WebAPI.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SoccerTeam, SoccerTeamResponse>();
        CreateMap<SoccerPlayer, SoccerPlayerResponse>();
        CreateMap<TransferItem, TransferItemResponse>();
    }
}