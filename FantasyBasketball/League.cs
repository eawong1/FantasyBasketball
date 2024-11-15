public class League
{
    private string m_leagueId;
    private string m_leagueYear;
    private string? m_swid;
    private string? m_espn;

    public League(string leagueId, string leagueYear, string? swid, string? espn)
    {
        m_leagueId = leagueId;
        m_leagueYear = leagueYear;
        m_swid = swid;
        m_espn = espn;
    }
}