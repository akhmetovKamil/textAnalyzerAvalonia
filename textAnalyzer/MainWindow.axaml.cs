using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace textAnalyzer;

public partial class MainWindow : Window
{
    public string Text;
    public bool IsFieldOpened = true;
    public bool IsFilePicked = false;
    public TextAnalyzer Analyzer;

    public MainWindow()
    {
        InitializeComponent();
        Analyzer = new TextAnalyzer();
    }

    // Create class Analyze
    public class TextAnalyzer
    {
        private string Text;
        private string[] Words;
        private string[] LongestWords;
        private string[] MostCommonWords;
        private string[] Yoparesete; // TODO перевести "союзы и предлоги"
        private int WordsCount;
        private int LettersCount;
        // TODO dict для распределения по алфавитам

         public (string Text, string[] Words, string[] LongestWords, string[] MostCommonWords, string[] Yoparesete,int WordsCount,int LettersCount) GetResults()
         {
             return (Text, Words, LongestWords, MostCommonWords, Yoparesete, WordsCount, LettersCount);
         }
         
        public void StartAnalysis(string text)
        {
            Text = text;
            Words = Text.Split(' ');
            WordsCount = Words.Length;
            LettersCount = Text.Length;
            LongestWords = GetLongestWords();
            MostCommonWords = GetMostCommonWords();
            // other methods
        }
        
        private string[] GetMostCommonWords()
        {
            throw new System.NotImplementedException();
        }

        private string[] GetLongestWords()
        {
            throw new System.NotImplementedException();
        }
    }


    private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO Replace with TopLevel.StorageProvider
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.AllowMultiple = false;
        dialog.Filters.Add(new FileDialogFilter() { Name = "Text files", Extensions = { "txt" } });
        var result = await dialog.ShowAsync(this);
        if (result != null)
        {
            Text = await File.ReadAllTextAsync(result[0]);
            FileName.Text = Path.GetFileName(result[0]);
            IsFilePicked = true;
            AnalyzeText.IsEnabled = true;
        }
        else AnalyzeText.IsEnabled = false;
    }

    private void AnalyzeText_OnClick(object sender, RoutedEventArgs e)
    {
        if (AnalyzeText.IsEnabled)
        {
            Text = IsFieldOpened ? Field.Text : Text;
            Analyzer.StartAnalysis(Text);
            var results = Analyzer.GetResults();
            // TODO Show results in xaml
        }
    }


    private void BoolSwitcher(bool isField, bool isFile, bool isOpened)
    {
        FieldPanel.IsVisible = isField;
        FilePanel.IsVisible = isFile;
        IsFieldOpened = isOpened;
    }

    private void SwitchPanel_OnClick(object sender, RoutedEventArgs e)
    {
        var whichButton = ((RadioButton)sender).Name;
        if (whichButton == "FieldButton") BoolSwitcher(true, false, true);
        else BoolSwitcher(false, true, false);
    }

    private void Field_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        AnalyzeText.IsEnabled = !string.IsNullOrEmpty(Field.Text);
    }
}