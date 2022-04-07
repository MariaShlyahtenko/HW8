using Avalonia.Media;
using Avalonia;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using HW8.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace HW8.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        ObservableCollection<Notes> ItemsPlanned { get; set; }
        ObservableCollection<Notes> ItemsInProgress { get; set; }
        ObservableCollection<Notes> ItemsCompleted { get; set; }
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public MainWindowViewModel()
        {
            ItemsPlanned = new ObservableCollection<Notes>();
            ItemsInProgress = new ObservableCollection<Notes>();
            ItemsCompleted = new ObservableCollection<Notes>();
            Content = new KanbanViewModel();
        }

        public void AddCompleted() => ItemsCompleted.Insert(0, new Notes("New Note"));
        public void AddInProgress() => ItemsInProgress.Insert(0, new Notes("New Note"));
        public void AddPlanned() => ItemsPlanned.Insert(0, new Notes("New Note"));

        public void DeleteCompleted(Notes item) => ItemsCompleted.Remove(item);
        public void DeleteInProgress(Notes item) => ItemsInProgress.Remove(item);
        public void DeletePlanned(Notes item) => ItemsPlanned.Remove(item);
        public void New()
        {
            ItemsPlanned.Clear();
            ItemsInProgress.Clear();
            ItemsCompleted.Clear();
        }

        public void WriteToBinaryFile(string filePath)
        {
            List<ObservableCollection<Notes>> notes = new List<ObservableCollection<Notes>>();
            notes.Add(ItemsPlanned);
            notes.Add(ItemsInProgress);
            notes.Add(ItemsCompleted);


            using (StreamWriter wr = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<ObservableCollection<Notes>>));
                xs.Serialize(wr, notes);
            }
        }
        public void OpenWindowView() => Content = new KanbanViewModel();
        public void ReadFromBinaryFile(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<ObservableCollection<Notes>>));
            using (StreamReader sr = new StreamReader(filePath))
            {
                ItemsCompleted.Clear();
                ItemsInProgress.Clear();
                ItemsPlanned.Clear();
                List<ObservableCollection<Notes>> notes = (List<ObservableCollection<Notes>>)xs.Deserialize(sr);
                ItemsCompleted = notes[2];
                ItemsInProgress = notes[1];
                ItemsPlanned = notes[0];
            }
        }
    }
}