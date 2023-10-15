using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace textAnalyzer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // AnalyzeText.IsEnabled = false;
        InitializeComponent();
    }

    public string Text;
    public bool IsFieldOpened = true;
    public bool isFilePicked = false;
    public bool CanAnalyzeText = false;
    
    private void AnalyzeText_OnClick(object? sender, RoutedEventArgs e)
    {
        СheckIsTextNotNull();
        if (AnalyzeText.IsEnabled)
        {
            Text = IsFieldOpened ? Field.Text : "Run function which open file";
        }
        
    }
    
    private void СheckIsTextNotNull()
    {
        if (IsFieldOpened) AnalyzeText.IsEnabled = string.IsNullOrEmpty(Field.Text);
        else
        {
            // Проверять открыт ли файл
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
        CanAnalyzeText = !string.IsNullOrEmpty(Field.Text); // Delete variable
    }
}