using System.ComponentModel.DataAnnotations;

public class BaggageWeightDto
{
    public int Id { get; set; }
    [Required, Range(2, 250)]
    public Int16 Weight { get; set; }
    [Required, Range(25, 999)]
    public decimal Price { get; set; }
}
