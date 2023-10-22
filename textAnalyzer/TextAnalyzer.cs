using System;
using System.Collections.Generic;
using System.Linq;

namespace textAnalyzer;

public class TextAnalyzer
{
    private string? _text = "";
    private string[] _words = Array.Empty<string>();
    private string[] _longestWords = Array.Empty<string>();
    private string[] _mostCommonWords = Array.Empty<string>();
    private int _wordsCount = 0;
    private int _lettersCount = 0;
    private Dictionary<char, int>? _lettersDistribution;
    private readonly char[] _alphabet = new[]
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z',
        'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
        'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т',
        'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь',
        'э', 'ю', 'я'
    };
    
    
    public (string LongestWords, string MostCommonWords, string WordsCount, string LettersCount, string LettersDistribution) GetResults()
    {
        return (
            string.Join("\n",_longestWords), 
            string.Join("\n",_mostCommonWords),
            _wordsCount.ToString(), 
            _lettersCount.ToString(),
            string.Join("\n", _lettersDistribution!.Select(x => $"{x.Key}: {x.Value}")))!;
    }

    public void StartAnalysis(string? text)
    {
        _text = text;
        _words = _text!.Split(' ');
        _wordsCount = _words.Length;
        _lettersCount = _text.Length;
        _longestWords = GetLongestWords();
        _mostCommonWords = GetMostCommonWords();
        _lettersDistribution = GetLettersDistribution();
    }

    private Dictionary<char, int> GetLettersDistribution()
    {
        var lettersCountDict = _alphabet.ToDictionary(x => x, x => 0);
        var lettersDistribution = new Dictionary<char, int>();
        foreach (var letter in _text!.ToLower())
        {
            lettersCountDict[letter] = lettersCountDict.TryGetValue(letter, out var value) ? value + 1 : 1;
        }
        foreach (var letter in _alphabet)
        {
            lettersDistribution[letter] = 100 * lettersCountDict[letter] / _lettersCount;
        } 
        return lettersDistribution;
    }
    
    private string[] GetMostCommonWords()
    {
        var wordsCount = new Dictionary<string, int>();
        foreach (var word in _words!)
        {
            wordsCount[word] = wordsCount.TryGetValue(word, out var value) ? value + 1 : 1;
        }

        return wordsCount.OrderByDescending(x => x.Value).Take(10).Select(x => x.Key).ToArray();
    }

    private string[] GetLongestWords()
    {
        var wordsLength = new Dictionary<string, int>();
        foreach (var word in _words!.Distinct().ToArray())
        {
            wordsLength[word] = word.Length;
        }

        return wordsLength.OrderByDescending(x => x.Value).Take(10).Select(x => x.Key).ToArray();
    }
}