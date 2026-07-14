using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KreditService.Models;

[Table("pengajuan_kredit")]
public class PengajuanKredit
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("plafon", TypeName = "numeric")]
    public decimal Plafon { get; set; }

    [Column("bunga", TypeName = "decimal(5,2)")]
    public decimal Bunga { get; set; }

    [Column("tenor")]
    public int Tenor { get; set; }

    [Column("angsuran", TypeName = "numeric")]
    public decimal Angsuran { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } 
}