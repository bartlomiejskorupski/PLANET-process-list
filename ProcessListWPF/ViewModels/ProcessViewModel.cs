namespace ProcessListWPF.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }

    public ProcessViewModel()
    {
        
    }


}
