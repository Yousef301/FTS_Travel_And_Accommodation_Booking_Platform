﻿using TABP.DAL.Enums;
using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Booking : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    public Invoice? Invoice { get; set; }
    public User User { get; set; }
    public Hotel Hotel { get; set; }
}