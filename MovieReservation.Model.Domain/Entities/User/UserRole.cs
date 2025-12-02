namespace MovieReservation.Model.Domain;

public class UserRole
{

    public long UserRole_User_FK { get; set; }
    public long UserRole_Role_FK { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.Now;
    public long AssignedBy { get; set; }
    
    public User? User { get; set; }
    public Role? Role { get; set; }
}