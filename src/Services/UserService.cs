namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;
using WebApi.Repositories;

public interface IUserService
{
    Task<IEnumerable<User>?> GetAll();
    Task<User?> GetById(int id);
    Task<User> Create(UserCreateRequest model);
    Task Update(int id, UserUpdateRequest model);
    Task Delete(int id);
}

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<User>?> GetAll()
        => await _userRepository.GetAll();

    public async Task<User?> GetById(int id)
    {
        var user = await _userRepository.GetById(id);

        if (user == null)
            throw new KeyNotFoundException($"User {id} not found");

        return user;
    }

    public async Task<User> Create(UserCreateRequest model)
    {
        // validate
        if (await _userRepository.GetByEmail(model.Email!) != null)
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // save user
        return await _userRepository.Create(user);
    }

    public async Task Update(int id, UserUpdateRequest model)
    {
        var user = await _userRepository.GetById(id);

        if (user == null)
            throw new KeyNotFoundException($"User {id} not found");

        // validate
        var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
        if (emailChanged && await _userRepository.GetByEmail(model.Email!) != null)
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        // copy model props to user
        _mapper.Map(model, user);

        // save user
        await _userRepository.Update(user);
    }

    public async Task Delete(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
            throw new KeyNotFoundException($"User {id} not found");

        await _userRepository.Delete(id);
    }
}