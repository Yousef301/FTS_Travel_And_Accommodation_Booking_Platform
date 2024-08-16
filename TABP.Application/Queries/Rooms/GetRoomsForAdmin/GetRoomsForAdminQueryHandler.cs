using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Rooms.GetRoomsForAdmin;

public class GetRoomsForAdminQueryHandler : IRequestHandler<GetRoomsForAdminQuery, PagedList<RoomAdminResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsForAdminQueryHandler(IRoomRepository roomRepository,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<RoomAdminResponse>> Handle(GetRoomsForAdminQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetByHotelIdPagedAsync(request.HotelId,
            new Filters<Room>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                FilterExpression = GetRoomsBasedOnNumberOrTypeExpression(request.SearchString),
                SortExpression = GetSortExpression(request.SortBy),
                SortOrder = request.SortOrder
            });

        return _mapper.Map<PagedList<RoomAdminResponse>>(rooms);
    }

    private Expression<Func<Room, bool>> GetRoomsBasedOnNumberOrTypeExpression(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return r => true;
        }

        return r => r.RoomNumber.Contains(searchString);
    }

    private Expression<Func<Room, object>> GetSortExpression(string? sortBy)
    {
        return sortBy?.ToLower() switch
        {
            "roomtype" => c => c.RoomType,
            _ => c => c.RoomNumber
        };
    }
}