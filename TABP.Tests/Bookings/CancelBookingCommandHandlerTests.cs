using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using TABP.Application.Commands.Bookings.CancelBooking;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;

namespace TABP.Tests.Bookings;

public class CancelBookingCommandHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IMapper> _mapper;
    private readonly CancelBookingCommandHandler _handler;

    public CancelBookingCommandHandlerTests()
    {
        _bookingRepository = new();
        _unitOfWork = new();
        _mapper = new();

        _handler = new CancelBookingCommandHandler(
            _bookingRepository.Object,
            _unitOfWork.Object,
            _mapper.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_Booking_Not_Exists()
    {
        // Arrange
        var command = new CancelBookingCommand
        {
            BookingId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
        SetupBookingRepository(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowUnauthorizedAccessException_When_User_Not_Owner_Of_Booking()
    {
        // Arrange
        var command = new CancelBookingCommand
        {
            BookingId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
        SetupBookingRepositoryForUser(new Guid());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowBookingStatusException_When_Booking_Status_Not_Confirmed()
    {
        // Arrange
        var command = new CancelBookingCommand
        {
            BookingId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
        SetupBookingRepositoryForStatus(BookingStatus.Cancelled, command.UserId);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BookingStatusException>();
    }

    private void SetupBookingRepository(bool exists)
    {
        _bookingRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Booking, bool>>>()))
            .ReturnsAsync(exists);
    }

    private void SetupBookingRepositoryForUser(Guid userId)
    {
        _bookingRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), true))
            .ReturnsAsync(new Booking { UserId = userId });
    }

    private void SetupBookingRepositoryForStatus(BookingStatus status, Guid userId)
    {
        _bookingRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), true))
            .ReturnsAsync(new Booking
            {
                BookingStatus = status,
                UserId = userId
            });
    }
}