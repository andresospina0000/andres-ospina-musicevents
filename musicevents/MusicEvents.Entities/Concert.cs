using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicEvents.Entities;

[Table("Events")]
public class Concert : EntityBase
{
    [StringLength(100)]
    [Required]
    public string Title { get; set; }

    [StringLength(500)]
    [Required]
    public string Description { get; set; }

    public DateTime DateEvent { get; set; }

    public int TicketsQuantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string ImageUrl { get; set; }

    public string Place { get; set; }

    public int GenreId { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public Genre Genre { get; set; }

    public bool Finalized { get; set; }

}