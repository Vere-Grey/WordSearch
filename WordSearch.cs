using System;
using System.Collections.Generic;
using System.Linq;

public class WordSearch
{
    private readonly char[][] gridData;
    private readonly int gridWidth;
    private readonly int gridHeight;

    private class WordLocator
    {
        private readonly string word;
        private readonly (int, int) wordStart;
        public (int, int)? wordEnd;
        public bool isWordFound = false;
        
        public WordLocator(string word, (int, int) wordStart)
        {
            this.word = word;
            this.wordStart = wordStart;
        }

        public (int, int)? FindEndOfWordInGrid()
        {
            CheckRight();
            CheckLeft();
            CheckUp();
            CheckDown();
            CheckDiagLeftUp();
            CheckDiagRightUp();
            CheckDiagRightDown();
            CheckDiagLeftDown();
            return wordEnd;
        }
        

    }

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

    public Dictionary<string, ((int, int), (int, int))?> Search(string[] wordsToSearchFor)
    {
        var results = new Dictionary<string, ((int, int), (int, int))?>{};
        foreach (var word in wordsToSearchFor)
        {
            var startCoordinates = FindLetterInGrid(word.First());
            foreach (var wordStart in startCoordinates)
            {
                var wordLocator = new WordLocator(word, wordStart);
                wordLocator.FindEndOfWordInGrid();
                if (wordLocator.isWordFound)
                {
                    results.Add(word, ToHumanReadable((wordStart, wordLocator.wordEnd)));
                    break;
                }
            }

            if (!results.ContainsKey(word))
            {
                results.Add(word, null);
            }
            
        }

        return results;
    }

    private ((int, int), (int, int))? ToHumanReadable(((int, int), (int, int)?) result) 
        => result.Item2 == null ? null : ((result.Item1.Item2 + 1, result.Item1.Item1 + 1), (result.Item2.Value.Item2 + 1, result.Item2.Value.Item1 + 1));
}