using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace textAnalyzer;

public partial class MainWindow : Window
{
    public bool IsFieldOpened = true;
    public string? TextFromFile;
    public readonly TextAnalyzer Analyzer;

    public MainWindow()
    {
        InitializeComponent();
        Analyzer = new TextAnalyzer();
    }

    private void CanAnalyzeText()
    {
        AnalyzeText.IsEnabled = IsFieldOpened ? !string.IsNullOrEmpty(Field.Text) : TextFromFile != null;
    }

    private async void OpenFile_OnClick(object sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var file = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File", AllowMultiple = false,
            FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
        });
        TextFromFile = await File.ReadAllTextAsync(file[0].Path.AbsolutePath);
        FileName.Text = Path.GetFileName(file[0].Name);
        AnalyzeText.IsEnabled = true;
        AnalyzeText.IsVisible = true;
        Results.IsVisible = false;
    }

    private async void SaveFile(string text)
    {
        await File.WriteAllTextAsync(OutputFileName.Text + ".txt", text);
    }

    private void AnalyzeText_OnClick(object sender, RoutedEventArgs e)
    {
        Analyzer.StartAnalysis(IsFieldOpened ? Field.Text : TextFromFile);
        var results = Analyzer.GetResults();
        WordsCount.Text = results.WordsCount;
        LettersCount.Text = results.LettersCount;
        foreach (var word in results.MostCommonWords)
        {
            var w = new TextBlock();
            w.Text = word;
            MostCommonWords.Children.Add(w);
        }
        foreach (var word in results.LongestWords)
        {
            var w = new TextBlock();
            w.Text = word;
            LongestWords.Children.Add(w);
        }
        foreach (var letters in results.LettersDistribution)
        {
            var w = new TextBlock();
            w.Text = letters;
            LettersDistribution.Children.Add(w);
        }
        Results.IsVisible = true;
        AnalyzeText.IsVisible = false;
        string joinedText = string.Join("\n", results);
        SaveFile(joinedText);
    }

    private void BoolSwitcher(bool isField, bool isFile, bool isFieldOpened)
    {
        FieldPanel.IsVisible = isField;
        FilePanel.IsVisible = isFile;
        IsFieldOpened = isFieldOpened;
    }

    private void SwitchPanel_OnClick(object sender, RoutedEventArgs e)
    {
        var whichButton = ((RadioButton)sender).Name;
        if (whichButton == "FieldButton") BoolSwitcher(true, false, true);
        else BoolSwitcher(false, true, false);
        CanAnalyzeText();
    }

    private void Field_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        AnalyzeText.IsEnabled = !string.IsNullOrEmpty(Field.Text);
        AnalyzeText.IsVisible = true;
        Results.IsVisible = false;
    }
}