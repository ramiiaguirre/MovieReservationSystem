namespace MovieReservation.Model.Domain;
public class RolePermission
{
    public long RolePermission_Permission_FK { get; set; }
    public long RolePermission_Role_FK { get; set; }

    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
}