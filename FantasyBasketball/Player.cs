using System.Collections.Generic;

public class Player
{
    string m_name;
    List<string> m_eligiblePositions;

    public Player(string name, List<string> eligiblePositions)
    {
        m_name = name;
        m_eligiblePositions = eligiblePositions;
    }

    public string GetName()
    {
        return m_name;
    }

    public List<string> GetEligiblePositions()
    {
        return m_eligiblePositions;
    }
}