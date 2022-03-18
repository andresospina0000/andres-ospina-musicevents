using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicEvents.Entities;

public class Sale : EntityBase
{
    public DateTime SaleDate { get; set; }
    public Concert Concert { get; set; }
    
    [Column("EventId")]
    public int ConcertId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalSale { get; set; }
    
    [Required]
    [StringLength(36)]
    public string UserId { get; set; }

    [StringLength(8)]
    public string OperationNumber { get; set; }
}