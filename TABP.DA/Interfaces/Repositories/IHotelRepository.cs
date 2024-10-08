﻿using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models;
using TABP.Shared.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<PagedList<Hotel>> GetAsync(Filters<Hotel> filters, bool includeCity = false,
        bool includeRooms = false);

    Task<PagedList<Hotel>> GetFilteredHotelsAsync(Filters<Hotel> filters, bool includeCity = false,
        bool includeRooms = false);

    Task<IEnumerable<FeaturedHotel>> GetHotelsWithDealsAsync(int count = 5);

    Task<Hotel?> GetByIdAsync(Guid id, bool includeCity = false, bool includeRooms = false);

    Task<double> GetHotelRateAsync(Guid id);
    Task<string?> GetThumbnailPathAsync(Guid id);
    Task<Hotel> CreateAsync(Hotel hotel);
    void Delete(Hotel hotel);
    void Update(Hotel hotel);
    Task UpdateRateAsync(Guid id, double rate);
    Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> predicate);
}