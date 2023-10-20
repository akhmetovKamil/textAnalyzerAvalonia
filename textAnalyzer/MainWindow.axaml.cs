using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace textAnalyzer;

public partial class MainWindow : Window
{
    public string? Text;
    public bool IsFieldOpened = true;
    public bool IsFilePicked = false;
    public TextAnalyzer Analyzer;

    public MainWindow()
    {
        InitializeComponent();
        Analyzer = new TextAnalyzer();
        
    }
    private async void OpenFile_OnClick(object sender, RoutedEventArgs e)
    {
        // TODO Replace with TopLevel.StorageProvider
        OpenFileDialog dialog = new OpenFileDialog(); dialog.AllowMultiple = false;
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
            WordsCount.Text = results.WordsCount;
            LettersCount.Text = results.LettersCount;
            MostCommonWords.Text = results.MostCommonWords;
            LongestWords.Text = results.LongestWords;
            LettersDistribution.Text = results.LettersDistribution;
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