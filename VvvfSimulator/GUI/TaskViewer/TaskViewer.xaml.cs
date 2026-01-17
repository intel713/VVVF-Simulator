using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VvvfSimulator.GUI.Resource.Language;
using VvvfSimulator.GUI.Resource.Theme;

namespace VvvfSimulator.GUI.TaskViewer
{
    public class TaskProgress
    {
        public double Progress = 0;
        public double Total = -1;

        public double RelativeProgress
        {
            get
            {
                if (Total == -1) return 0;
                return Progress / Total * 100;
            }
        }

        public bool Cancel = false;
    }
    public class TaskInfo
    {
        public TaskProgress Data { get; set; }
        public Task Task { get; set; }
        public string Description { get; set; }
        public bool Cancelable
        {
            get
            {
                return (!Data.Cancel && Data.RelativeProgress < 99.9);
            }
        }

        public String Status
        {
            get
            {
                if (Data.Cancel) return LanguageManager.GetString("TaskViewer.Status.Canceled");
                if (Data.RelativeProgress > 99.9) return LanguageManager.GetString("TaskViewer.Status.Complete");
                return LanguageManager.GetString("TaskViewer.Status.Running");
            }
        }

        public Brush StatusColor
        {
            get
            {
                if (Data.Cancel) return ThemeManager.GetBrush("TaskViewer.StatusColor.Canceled");
                if (Data.RelativeProgress > 99.9) return ThemeManager.GetBrush("TaskViewer.StatusColor.Complete");
                return ThemeManager.GetBrush("TaskViewer.StatusColor.Running");
            }
        }

        public TaskInfo(Task Task, TaskProgress progressData, string Description)
        {
            this.Task = Task;
            this.Data = progressData;
            this.Description = Description;
        }
    }

    /// <summary>
    /// TaskViewer_Main.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskViewer : Window
    {
        // Task Progress List
        public static List<TaskInfo> TaskList = [];

        public TaskViewer()
        {
            InitializeComponent();

            DataContext = TaskList;
            TaskView.Items.Refresh();

            RunUpdateTask();
        }

        public bool updateGridTask = true;
        
        public void RunUpdateTask()
        {
            Task task = Task.Run(() =>
            {
                while (updateGridTask)
                {
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            TaskView.Items.Refresh();
                        });
                    }
                    catch
                    {
                        break;
                    }

                    Thread.Sleep(500);
                }
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateGridTask = false;
        }

        private void ClickCancelButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Object tag = btn.Tag;
            if (tag == null) return;

            for (int i = 0; i < TaskList.Count; i++)
            {
                TaskInfo data = TaskList[i];
                if (data.Task.Id.ToString().Equals(tag.ToString()))
                {
                    data.Data.Cancel = true;
                    break;
                }
            }
        }

        private void OnWindowControlButtonClick(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null) return;
            string? tag = btn.Tag.ToString();
            if (tag == null) return;

            if (tag.Equals("Close"))
                Close();
            else if (tag.Equals("Maximize"))
            {
                if (WindowState.Equals(WindowState.Maximized))
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            }
            else if (tag.Equals("Minimize"))
                WindowState = WindowState.Minimized;
        }
    }
}
