namespace Alman.SharedModels;

public interface IPrecontractBase : IIdentifier
{
    //public int Id { get; set; }

    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }
    
    public int? PMonth { get; set; }
}
