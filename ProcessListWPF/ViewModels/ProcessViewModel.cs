namespace ProcessListWPF.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Priority { get; set; }
    public double Memory { get; set; }
    public string MemoryFormatted => $"{Memory:0.##} MB";
    public ProcessViewModel()
    {
        
    }

}
