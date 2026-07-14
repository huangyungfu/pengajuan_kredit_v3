using System.ComponentModel.DataAnnotations;

namespace KreditService.DTOs;

public record OverrideCreateRequest(
    [Required] decimal Plafon, 
    [Required] decimal Bunga, 
    [Required] int Tenor, 
    [Required] decimal Angsuran
);

public record ProperCreateRequest(
    [Required] decimal Plafon, 
    [Required] decimal Bunga, 
    [Required] int Tenor
);

// Id is placed first to align exactly with how the controller parses the object model payload
public record CalculationRequest(
    Guid? Id, 
    [Required] decimal Plafon, 
    [Required] decimal Bunga, 
    [Required] int Tenor
);