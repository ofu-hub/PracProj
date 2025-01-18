using AccessModuleSystem.Contracts.User;
using AccessModuleSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessModuleSystem.Server.Controllers;

/// <summary>
/// Контроллер работы с пользователями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
  /// <summary>
  /// Контекст базы данных
  /// </summary>
  private readonly DatabaseContext _context;

  /// <summary>
  /// Конструктор класса <see cref="UsersController"/>
  /// </summary>
  /// <param name="context"></param>
  public UsersController(DatabaseContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Получить список пользователей
  /// </summary>
  /// <returns></returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
  {
    var users = await _context.Users
        .Select(user => new UserReadDTO
        {
          Id = user.Id,
          Username = user.Username,
          Name = user.Name,
          Surname = user.Surname,
          Patronymic = user.Patronymic,
          Email = user.Email,
          Role = user.Role,
          CreatedAt = user.CreatedAt
        })
        .ToListAsync();

    return Ok(users);
  }

  /// <summary>
  /// Получить пользователя по идентификатору
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<UserReadDTO>> GetUser(Guid id)
  {
    var user = await _context.Users.FindAsync(id);

    if (user == null)
    {
      return NotFound();
    }

    return Ok(new UserReadDTO
    {
      Id = user.Id,
      Username = user.Username,
      Name = user.Name,
      Surname = user.Surname,
      Patronymic = user.Patronymic,
      Email = user.Email,
      Role = user.Role,
      CreatedAt = user.CreatedAt
    });
  }

  /// <summary>
  /// Создать нового пользователя
  /// </summary>
  /// <param name="userCreateDTO"></param>
  /// <returns></returns>
  [HttpPost]
  public async Task<ActionResult<UserReadDTO>> CreateUser(UserCreateDTO userCreateDTO)
  {
    var user = new User
    {
      Id = Guid.NewGuid(),
      Username = userCreateDTO.Username,
      Name = userCreateDTO.Name,
      Surname = userCreateDTO.Surname,
      Patronymic = userCreateDTO.Patronymic,
      Email = userCreateDTO.Email,
      PasswordHash = HashPassword(userCreateDTO.Password), // Предполагается наличие метода хеширования
      Role = userCreateDTO.Role,
      CreatedAt = DateTime.UtcNow
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserReadDTO
    {
      Id = user.Id,
      Username = user.Username,
      Name = user.Name,
      Surname = user.Surname,
      Patronymic = user.Patronymic,
      Email = user.Email,
      Role = user.Role,
      CreatedAt = user.CreatedAt
    });
  }

  /// <summary>
  /// Обновить данные пользователя
  /// </summary>
  /// <param name="id"></param>
  /// <param name="userUpdateDTO"></param>
  /// <returns></returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDTO userUpdateDTO)
  {
    var user = await _context.Users.FindAsync(id);

    if (user == null)
    {
      return NotFound();
    }

    user.Name = userUpdateDTO.Name;
    user.Surname = userUpdateDTO.Surname;
    user.Patronymic = userUpdateDTO.Patronymic;
    user.Email = userUpdateDTO.Email;

    _context.Entry(user).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Удалить пользователя
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteUser(Guid id)
  {
    var user = await _context.Users.FindAsync(id);

    if (user == null)
    {
      return NotFound();
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Хеширование пароля
  /// </summary>
  /// <param name="password"></param>
  /// <returns></returns>
  private string HashPassword(string password)
  {
    // Простая заглушка для хеширования пароля
    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
  }
}