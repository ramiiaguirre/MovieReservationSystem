using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;

public class SignUp : ISignUp
{
    private readonly IUserUnitOfWork _userUnitOfWork = default!;
    public SignUp(IUserUnitOfWork userUnitOfWork)
    {
        _userUnitOfWork = userUnitOfWork;
    }

    public async Task<User> Execute(User user)
    {
        var userCreated = await _userUnitOfWork.Users.Add(user);

        await _userUnitOfWork.Save();

        var role = await _userUnitOfWork.Roles.Get(r => r.Name == "Customer");
        
        // Attachear para que EF sepa que ya existen
        await _userUnitOfWork.Users.Update(userCreated);
        await _userUnitOfWork.Roles.Update(role);


        await _userUnitOfWork.UserRoles.Add(new UserRole
        {
            UserRole_User_FK = userCreated.Id,
            UserRole_Role_FK = role!.Id,
            AssignedAt = DateTime.Now.Date,
            AssignedBy = userCreated.Id,
            User = user,
            Role = role

        });

        await _userUnitOfWork.Save();
        return userCreated;
    }
    
}