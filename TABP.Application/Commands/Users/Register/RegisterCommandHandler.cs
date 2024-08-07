using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;

namespace TABP.Application.Commands.Users.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(IUserRepository userRepository, ICredentialRepository credentialRepository,
        IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _credentialRepository = credentialRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordService = passwordService;
    }


    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExist(request.Email))
        {
            throw new EmailAlreadyExist();
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