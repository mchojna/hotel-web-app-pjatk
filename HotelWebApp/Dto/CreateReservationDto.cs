using System.ComponentModel.DataAnnotations;

namespace HotelWebApp.Dto;

public class CreateReservationDto : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "RoomId musi być dodatnie.")]
    public int RoomId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [Required(AllowEmptyStrings = false)]
    [AllowedValues("planned", "confirmed", "cancelled")]
    public string Status { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
            yield return new ValidationResult(
                "EndTime musi być późniejsze niż StartTime.",
                [nameof(EndTime)]);
    }
}