using System;
using System.Collections.Generic;
using System.Globalization;

public enum GameResult
{
    None,
    Win,
    Draw,
    Loss
}

public class GameData
{
    private DateTime _date;
    private string _event;
    private int _round;
    private string _site;

    public string White { get; private set; }

    public string Black { get; private set; }

    public GameResult Result { get; private set; }

    public Dictionary<int, List<string>> Moves { get; set; } = new Dictionary<int, List<string>>();

    public string GetResultAsText()
    {
        var text = "";
        switch (Result)
        {
            case GameResult.None:
                text = "";
                break;
            case GameResult.Win:
                text = "1 - 0";
                break;
            case GameResult.Loss:
                text = "0 - 1";
                break;
            case GameResult.Draw:
                text = "1/2";
                break;
        }

        return text;
    }

    /**
     * Parse list of tags from PGN header
     */
    public void SetTags(Dictionary<string, string> tags)
    {
        foreach (var tagPair in tags)
        {
            var key = tagPair.Key.ToLower().Trim();
            var val = tagPair.Value.Trim().Trim('"');

            switch (key)
            {
                case "event":
                    _event = val;
                    break;
                case "site":
                    _site = val;
                    break;
                case "white":
                    White = val;
                    break;
                case "black":
                    Black = val;
                    break;
                case "date":
                    _date = DateTime.ParseExact(val, "yyyy.MM.dd",
                        CultureInfo.InvariantCulture);
                    break;
                case "round":
                    if (!int.TryParse(val, out _round))
                    {
                        _round = -1;
                    }

                    break;
                case "result":
                    switch (tagPair.Value.Trim('"'))
                    {
                        case "1-0":
                            Result = GameResult.Win;
                            break;
                        case "0-1":
                            Result = GameResult.Loss;
                            break;
                        case "1/2-1/2":
                            Result = GameResult.Draw;
                            break;
                        case "*":
                            Result = GameResult.None;
                            break;
                        default:
                            Result = GameResult.None;
                            break;
                    }

                    break;
            }
        }
    }
}