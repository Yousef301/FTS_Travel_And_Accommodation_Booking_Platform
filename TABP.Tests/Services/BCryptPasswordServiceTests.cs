using Moq;
using FluentAssertions;
using TABP.Application.Services.Implementations;

namespace TABP.Tests.Services;

public class BCryptPasswordServiceTests
{
    private readonly Mock<BCryptPasswordService> _bCryptPasswordService;

    public BCryptPasswordServiceTests()
    {
        _bCryptPasswordService = new();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var plainPassword = "plainPassword";

        // Act
        var hashedPassword = _bCryptPasswordService.Object.HashPassword(plainPassword);

        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
        hashedPassword.Should().NotBe(plainPassword);
    }

    [Fact]
    public void ValidatePassword_ShouldReturnTrue_WhenPasswordIsValid()
    {
        // Arrange
        var plainPassword = "plainPassword";
        var hashedPassword = _bCryptPasswordService.Object.HashPassword(plainPassword);

        // Act
        var isValid = _bCryptPasswordService.Object.ValidatePassword(plainPassword, hashedPassword);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidatePassword_ShouldReturnFalse_WhenPasswordIsInvalid()
    {
        // Arrange
        var plainPassword = "plainPassword";
        var hashedPassword = _bCryptPasswordService.Object.HashPassword(plainPassword);
        var wrongPassword = "wrongPassword";

        // Act
        var isValid = _bCryptPasswordService.Object.ValidatePassword(wrongPassword, hashedPassword);

        // Assert
        isValid.Should().BeFalse();
    }
}