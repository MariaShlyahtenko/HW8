using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Drawing;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using HW8.ViewModels;

namespace HW8.Views
{
    public partial class KanbanView : UserControl
    {
        public KanbanView()
        {
            InitializeComponent();
            this.FindControl<MenuItem>("Save").Click += async delegate
            {
                var taskPath = new SaveFileDialog().ShowAsync((Window)this.Parent);

                string path = await taskPath;
                var context = this.Parent.DataContext as MainWindowViewModel;
                if (path is not null)
                {
                    context.WriteToBinaryFile(path);
                }
                context.OpenWindowView();

            };
            this.FindControl<MenuItem>("Load").Click += async delegate
            {
                var taskPath = new OpenFileDialog().ShowAsync((Window)this.Parent);
                string[]? path = await taskPath;
                var context = this.Parent.DataContext as MainWindowViewModel;
                if (path is not null)
                {
                    context.ReadFromBinaryFile(string.Join("/", path));
                }
                context.OpenWindowView();
            };

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Close_App(object sender, RoutedEventArgs e)
        {
            var context = this.Parent as MainWindow;
            context.Close();
        }
        private void ShowAboutWindow(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new AboutView();
            dialogWindow.ShowDialog((Window)this.Parent);
        }

    }
}
