﻿using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class BookingDetail : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public int? NumberOfChildren { get; set; }
    public int? NumberOfAdults { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Booking Booking { get; set; }
    public Room Room { get; set; }
}