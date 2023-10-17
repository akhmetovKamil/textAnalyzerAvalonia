using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace textAnalyzer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    public string Text;
    public bool IsFieldOpened = true;
    public bool IsFilePicked = false;

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
    }
}