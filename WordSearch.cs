using System;
using System.Collections.Generic;
using System.Linq;

public class WordSearch
{
    public readonly char[][] gridData;
    private readonly int gridHeight;
    private readonly int gridWidth;

    public WordSearch(string grid)
    {
        gridHeight = grid.Split("\n").Length;
        gridData = new char[gridHeight][];
        var index = 0;
        foreach (var line in grid.Split("\n"))
        {
            gridData[index] = line.ToCharArray();
            index++;
        }

        gridWidth = gridData[0].Length;
    }

    private List<(int, int)> FindLetterInGrid(char letter)
    {
        var results = new List<(int, int)>();

        for (var x = 0; x < gridHeight; x++)
        {
            for (var y = 0; y < gridWidth; y++)
            {
                if (gridData[x][y].Equals(letter))
                {
                    results.Add((x, y));
                }
            }
        }

        return results;
    }

    public Dictionary<string, ((int, int), (int, int))?> Search(string[] wordsToSearchFor)
    {
        var results = new Dictionary<string, ((int, int), (int, int))?>();
        foreach (var word in wordsToSearchFor)
        {
            var startCoordinates = FindLetterInGrid(word.First());
            foreach (var wordStart in startCoordinates)
            {
                var wordLocator = new WordLocator(word, wordStart, gridData);
                wordLocator.FindEndOfWordInGrid();
                if (wordLocator.IsWordFound())
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
        => ((result.Item1.Item2 + 1, result.Item1.Item1 + 1),
            (result.Item2.Value.Item2 + 1, result.Item2.Value.Item1 + 1));

    private class WordLocator
    {
        private readonly int gridHight;
        private readonly int gridWight;
        private readonly string word;
        private readonly int wordRange;
        private readonly int wordStartX;
        private readonly int wordStartY;
        public readonly char[][] gridData;
        public (int, int)? wordEnd;

        public WordLocator(string word, (int cordX, int cordY) wordStart, char[][] gridData)
        {
            this.word = word;
            wordRange = word.Length - 1;
            wordStartX = wordStart.cordX;
            wordStartY = wordStart.cordY;
            this.gridData = gridData;
            gridWight = gridData[0].Length;
            gridHight = gridData.Length;
        }

        public bool IsWordFound() => wordEnd != null;

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

        private void CheckDiagLeftDown()
        {
            var canWordFit = wordStartX + wordRange <= gridHight && wordStartY - wordRange >= 0;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX + index][wordStartY - index] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX + wordRange, wordStartY - wordRange);
        }

        private void CheckDiagRightDown()
        {
            var canWordFit = wordStartX + wordRange <= gridHight && wordStartY + wordRange <= gridWight;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX + index][wordStartY + index] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX + wordRange, wordStartY + wordRange);
        }

        private void CheckDiagRightUp()
        {
            var canWordFit = wordStartX - wordRange >= 0 && wordStartY + wordRange <= gridWight;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX - index][wordStartY + index] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX - wordRange, wordStartY + wordRange);
        }

        private void CheckDiagLeftUp()
        {
            var canWordFit = wordStartX - wordRange >= 0 && wordStartY - wordRange >= 0;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX - index][wordStartY - index] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX - wordRange, wordStartY - wordRange);
        }

        private void CheckDown()
        {
            var canWordFit = wordStartX + wordRange <= gridHight;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX + index][wordStartY] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX + wordRange, wordStartY);
        }

        private void CheckUp()
        {
            var canWordFit = wordStartX - wordRange >= 0;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX - index][wordStartY] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX - wordRange, wordStartY);
        }

        private void CheckLeft()
        {
            var canWordFit = wordStartY - wordRange >= 0;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX][wordStartY - index] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX, wordStartY - wordRange);
        }

        private void CheckRight()
        {
            var canWordFit = wordStartY + wordRange <= gridWight;
            if (IsWordFound() || !canWordFit)
            {
                return;
            }

            for (var index = 1; index <= wordRange; index++)
            {
                if (gridData[wordStartX][wordStartY + index] != word[index])
                {
                    return;
                }
            }

            wordEnd = (wordStartX, wordStartY + wordRange);
        }
    }
}