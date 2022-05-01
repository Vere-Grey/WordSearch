using System;
using System.Collections.Generic;
using System.Linq;

public class WordSearch
{
    private readonly char[][] gridData;
    private readonly int gridWidth;
    private readonly int gridHeight;

    public WordSearch(string grid)
    {
        gridHeight = grid.Split("\n").Length;
        gridData = new char[gridHeight][];
        var index = 0;
        foreach (var line in grid.Split("\n"))
        {
            gridData[index] =  line.ToCharArray();
            index++;
        }
        gridWidth = gridData[0].Length;
    }

    private List<(int, int)> FindLetterInGrid(char letter)
    {
        var results = new List<(int, int)>();

        for (int x = 0; x < gridHeight; x++)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                if (gridData[x][y].Equals(letter))
                    results.Add((x, y));
            }
        }

        return results;
    }
    
    private int? GetDirection(int wordLength, (int startX, int startY) start, (int endX, int endY) end)
    {
        var rangeX = end.endX - start.startX + 1;
        var rangeY = end.endY - start.startY + 1;
        var doesRangeNotEqualWordLength = wordLength != Math.Abs(rangeX) && wordLength != Math.Abs(rangeY);
        if (doesRangeNotEqualWordLength)
        {
            return null;
        }

        return (rangeX, rangeY) switch
        {
            (1, var y) when y == wordLength => 1,
            var (x, y) when x == wordLength && y == wordLength => 2,
            (var x, 1) when x == wordLength => 3,
            var (x, y) when x == wordLength && y == -wordLength => 4,
            (1, var y) when y == -wordLength => 5,
            var (x, y) when x == -wordLength && y == -wordLength => 6,
            (var x, 1) when x == -wordLength => 7,
            var (x, y) when x == -wordLength && y == wordLength => 8,
            _ => null
        };

    }

    private ((int, int), (int, int))? FindWordInGrid(string word, List<(int, int)> startCoordinates, List<(int, int)> endCoordinates)

    {
        foreach (var start in startCoordinates)
        {
            foreach (var end in endCoordinates)
            {
                // int? result = GetDirection(word.Length, start, end) switch
                // {
                //     _ => null
                // };
                if (GetDirection(word.Length, start, end) != null)
                {
                    return (start, end);
                }
            }
        }

        return null;
    }


    public Dictionary<string, ((int, int), (int, int))?> Search(string[] wordsToSearchFor)
    {
        var results = new Dictionary<string, ((int, int), (int, int))?>{};
        foreach (var word in wordsToSearchFor)
        {
            var startCoordinates = FindLetterInGrid(word.First());
            var endCoordinates = FindLetterInGrid(word.Last());
            if(!startCoordinates.Any() | !endCoordinates.Any())
            {
                results.Add(word, null); 
                continue;
            }

            var result = FindWordInGrid(word, startCoordinates, endCoordinates);
            results.Add(word, ToHumanReadable(result));
            
        }

        return results;
    }

    private ((int, int), (int, int))? ToHumanReadable(((int, int), (int, int))? result) 
        => result == null ? null : ((result.Value.Item1.Item2 + 1, result.Value.Item1.Item1 + 1), (result.Value.Item2.Item2 + 1, result.Value.Item2.Item1 + 1));
}