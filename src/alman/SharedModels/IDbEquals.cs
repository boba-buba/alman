namespace Alman.SharedModels;

/// <summary>
/// Provides functionality to compare if two rows of the same table are the same.
/// </summary>
public interface IDbEquals
{
    /// <summary>
    /// Compares if two roows of the table are the same.
    /// </summary>
    /// <param name="other">The row we compare with.</param>
    /// <returns>True if rows are the same, false otherwise.</returns>
    public static abstract bool DbEquals(IDbEquals other);
}
