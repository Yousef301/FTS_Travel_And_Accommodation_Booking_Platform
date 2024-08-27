using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using TABP.Application.Commands.Bookings.CreateBooking;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Tests.Bookings;

public class CreateBookingCommandHandlerTests
{
    private readonly Mock<IBookingDetailRepository> _bookingDetailRepository;
    private readonly Mock<IBookingRepository> _bookingRepository;
    private readonly Mock<IHotelRepository> _hotelRepository;
    private readonly Mock<IRoomRepository> _roomRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CreateBookingCommandHandler _handler;

    public CreateBookingCommandHandlerTests()
    {
        _bookingDetailRepository = new();
        _bookingRepository = new();
        _hotelRepository = new();
        _roomRepository = new();
        _unitOfWork = new();

        _handler = new CreateBookingCommandHandler(
            _bookingDetailRepository.Object,
            _bookingRepository.Object,
            _unitOfWork.Object,
            _roomRepository.Object,
            _hotelRepository.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_Hotel_Not_Exists()
    {
        // Arrange
        var command = CreateCommand(hotelId: Guid.NewGuid());
        SetupHotelRepository(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowBookingOverlapException_When_Booking_Overlaps()
    {
        // Arrange
        var command = CreateCommand(
            roomIds: new List<Guid>(),
            checkInDate: DateOnly.FromDateTime(DateTime.Now),
            checkOutDate: DateOnly.FromDateTime(DateTime.Now).AddDays(1)
        );
        SetupHotelRepository(true);
        SetupBookingRepository(overlap: true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BookingOverlapException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidPaymentMethodException_When_PaymentMethod_Is_Invalid()
    {
        // Arrange
        var command = CreateCommand(
            paymentMethod: "Invalid"
        );
        SetupHotelRepository(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidPaymentMethodException>();
    }

    [Fact]
    public async Task Handle_Should_Create_Booking()
    {
        // Arrange
        var command = CreateCommand(
            paymentMethod: "Visa",
            userId: Guid.NewGuid()
        );
        SetupHotelRepository(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _unitOfWork.Verify(x => x.CommitTransactionAsync(), Times.Once);
    }

    private void SetupHotelRepository(bool exists)
    {
        _hotelRepository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
            .ReturnsAsync(exists);
    }

    private void SetupBookingRepository(bool overlap)
    {
        _bookingRepository.Setup(x =>
                x.IsBookingOverlapsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(),
                    It.IsAny<DateOnly>()))
            .ReturnsAsync(overlap);
    }

    private CreateBookingCommand CreateCommand(
        Guid? hotelId = null,
        IEnumerable<Guid>? roomIds = null,
        DateOnly? checkInDate = null,
        DateOnly? checkOutDate = null,
        string? paymentMethod = null,
        Guid? userId = null)
    {
        return new CreateBookingCommand
        {
            HotelId = hotelId ?? Guid.NewGuid(),
            RoomIds = roomIds ?? new List<Guid>(),
            CheckInDate = checkInDate ?? DateOnly.FromDateTime(DateTime.Now),
            CheckOutDate = checkOutDate ?? DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            PaymentMethod = paymentMethod ?? "Visa",
            UserId = userId ?? Guid.NewGuid()
        };
    }
}