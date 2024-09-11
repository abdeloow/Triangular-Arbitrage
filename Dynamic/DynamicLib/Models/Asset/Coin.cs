namespace DynamicLib;

/// <summary>
/// Represents a coin in a trading pair, including its name and previous value.
/// </summary>
public class Coin
{
    /// <summary>
    /// Gets the name of the coin.
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// Initializes a new instance of the <see cref="Coin"/> class.
    /// </summary>
    /// <param name="name">The name of the coin.</param>
    public Coin(string name)
    {
        Name = name;
    }
    public override string ToString()
    {
        return $"{Name}";
    }
    /// <summary>
    /// Overrides the default equality comparison to compare Coin objects based on their properties.
    /// This is necessary because the Coin object is used as a key in the adjacency list of the Graph 
    /// that takes all coins within the tickers. The default equality comparison uses reference equality, 
    /// which would not work correctly for this purpose.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Coin coin)
        {
            return Name == coin.Name;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}   