using System.Collections.Generic;

public interface IPlayer
{
    string GetName();
    List<string> GetEligiblePositions();
}