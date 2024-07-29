﻿using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class City : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Country { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    public ICollection<CityImage> Images { get; set; } = new List<CityImage>();
}