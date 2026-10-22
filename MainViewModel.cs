using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using OneThreeFive.Models;

namespace OneThreeFive.ViewModels
{
    public class MainViewModel
    {
        const int BIG_TASKS = 1;
        const int MEDIUM_TASKS = 3;
        const int SMALL_TASKS = 5;

        private static readonly string SavePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

        public ObservableCollection<TaskItem> BigTask { get; set; } = new();

        public ObservableCollection<TaskItem> MediumTasks { get; set; } = new();

        public ObservableCollection<TaskItem> SmallTasks { get; set; } = new();

        public ICommand ResetDayCommand { get; }

        public MainViewModel()
        {
            for (int i = 0; i < BIG_TASKS; i++)
                BigTask.Add(new TaskItem());

            for (int i = 0; i < MEDIUM_TASKS; i++)
                MediumTasks.Add(new TaskItem());

            for (int i = 0; i < SMALL_TASKS; i++)
                SmallTasks.Add(new TaskItem());

            ResetDayCommand = new Command(ExecuteResetDay);

            Load();
        }

        private void ExecuteResetDay()
        {
            foreach (var task in BigTask)
            {
                task.Title = string.Empty;
                task.IsCompleted = false;
            }

            foreach (var task in MediumTasks)
            {
                task.Title = string.Empty;
                task.IsCompleted = false;
            }

            foreach (var task in SmallTasks)
            {
                task.Title = string.Empty;
                task.IsCompleted = false;
            }
        }

        private record TaskState(string Title, bool IsCompleted);
        private record SaveState(List<TaskState> Big, List<TaskState> Medium, List<TaskState> Small);

        private static List<TaskState> ToState(IEnumerable<TaskItem> items) =>
        items.Select(t => new TaskState(t.Title ?? string.Empty, t.IsCompleted)).ToList();

        private static void Apply(IList<TaskItem> target, List<TaskState>? saved)
        {
            if (saved is null) return;

            for (int i = 0; i < target.Count && i < saved.Count; i++)
            {
                target[i].Title = saved[i].Title;
                target[i].IsCompleted = saved[i].IsCompleted;
            }
        }

        public void Save()
        {
            try
            {
                var state = new SaveState(ToState(BigTask), ToState(MediumTasks), ToState(SmallTasks));
                File.WriteAllText(SavePath, JsonSerializer.Serialize(state));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save Failed: {ex}");
            }
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(SavePath)) return;
                var state = JsonSerializer.Deserialize<SaveState>(File.ReadAllText(SavePath));
                if (state is null) return;

                Apply(BigTask, state.Big);
                Apply(MediumTasks, state.Medium);
                Apply(SmallTasks, state.Small);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Load failed: {ex}");
            }
        }
    }
}