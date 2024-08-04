namespace Alman.SharedModels;

public interface IPrecontractBase
{
    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }
    
    public int? PMonth { get; set; }
}
