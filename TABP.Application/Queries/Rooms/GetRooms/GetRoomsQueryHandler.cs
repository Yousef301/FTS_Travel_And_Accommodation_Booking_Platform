﻿using AutoMapper;
using MediatR;
using TABP.Application.Queries.Amenities;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Rooms.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, IEnumerable<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomResponse>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetByHotelAsync(request.HotelId);

        return _mapper.Map<IEnumerable<RoomResponse>>(rooms);
    }
}