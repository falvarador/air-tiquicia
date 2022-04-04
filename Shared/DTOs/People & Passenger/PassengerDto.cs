using System.ComponentModel.DataAnnotations;

public class PassengerDto
{
    public int PassengerId { get; set; }
    [RequiredInteger]
    public int PersonId { get; set; }
    [Required]
    public Int16 QuantityBaggage { get; set; }
}
