using ArzonOL.Entities;
namespace ArzonOL.Models;

public class ProductVoterModel
{
    public Guid Id {get; set;}   
    public int? Vote { get; set; }
    public string? Comment { get; set; }
    public Guid? ProductId { get; set; }
    public virtual BaseProductEntity? Product { get; set; }
    public Guid? UserId { get; set; }
    public virtual PublicUserModel? User { get; set; }

}