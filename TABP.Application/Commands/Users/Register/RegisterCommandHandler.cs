using AutoMapper;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Users.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(IUserRepository userRepository,
        ICredentialRepository credentialRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _credentialRepository = credentialRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordService = passwordService;
    }


    public async Task Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(user => user.Email == request.Email))
        {
            throw new UniqueConstraintViolationException(
                "An user with the same email already exists.");
        }

        if (await _userRepository.ExistsAsync(user => user.PhoneNumber == request.PhoneNumber))
        {
            throw new UniqueConstraintViolationException(
                "Phone number already used.");
        }

        if (await _credentialRepository.UsernameExistsAsync(request.Username))
        {
            throw new UniqueConstraintViolationException(
                "An user with the same username already exists.");
        }

        var user = _mapper.Map<User>(request);
        var userCredential = _mapper.Map<Credential>(request);

        var id = new Guid();

        user.Id = id;
        user.Role = Role.Customer;


        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _userRepository.CreateAsync(user);

            await _unitOfWork.SaveChangesAsync();

            userCredential.UserId = user.Id;
            userCredential.HashedPassword = _passwordService.HashPassword(request.Password);

            await _credentialRepository.CreateAsync(userCredential);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}