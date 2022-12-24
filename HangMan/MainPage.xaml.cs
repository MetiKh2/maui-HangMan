using System.ComponentModel;

namespace HangMan;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    private List<char> letters=new List<char>();

    public List<char> Letters
    {
        get { return letters; }
        set { letters = value; OnPropertyChanged(); }
    }
    private string spotLight;

    public string SpotLight
    {
        get { return spotLight; }
        set { spotLight = value; OnPropertyChanged(); }
    }
    private string message;

    public string Message
    {
        get { return message; }
        set { message = value; OnPropertyChanged(); }
    }

    private string errorStatus;

    public string ErrorStatus
    {
        get { return errorStatus; }
        set
        {
            errorStatus = value;
            OnPropertyChanged();
        }
    }

    private string imageName = "img0.jpg";

    public string ImageName
    {
        get { return imageName; }
        set
        {
            imageName = value;
            OnPropertyChanged();
        }
    }
    public int mistakes = 0;
    private int maxMistakes = 6;
    List<string> words = new List<string> {
      "python",
        "javascript",
        "maui",
        "csharp",
        "mongodb",
        "sql",
        "xaml",
        "word",
        "excel",
        "powerpoint",
        "code",
        "hotreload",
        "snippets" 
    };
    public string answer = "";
    public MainPage()
    {
        InitializeComponent();
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
        BindingContext = this;
        PickWord();
        updateSpotLight(answer,guessList);
    }
    public void PickWord()
    {
        answer = words[new Random().Next(0,words.Count)];
    }
    public void EnableLetters()
    {
        foreach (var children in ContainerBtn.Children)
        {
            var btn = children as Button;

            if (btn != null)
            {
                btn.IsEnabled = true;
                btn.BackgroundColor = new Color(153,50,204);
                btn.TextColor= new Color(255,255,255);
            }
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        btn.IsEnabled = false;
        btn.BackgroundColor = new Color(128, 128, 128);
        btn.TextColor = new Color(0, 0, 0);
        var selectedLetter = btn.Text;
        handleGuess(selectedLetter[0]);
    }
    List<char> guessList = new List<char>();
    private void handleGuess(char letter)
    {
        if (guessList.IndexOf(letter)==-1)  
        {
            guessList.Add(letter);
        }
        if (answer.IndexOf(letter)>=0)
        {
            updateSpotLight(answer,guessList);
            checkWon();
        }
        else
        {
            mistakes += 1;
            ImageName = $"img{mistakes}.jpg";
            updateErrorStatus();
            checkLost();
        }
    }

    private void checkLost()
    {
        if (mistakes==maxMistakes+1)
        {
            Message = "You Lost !";
            disableLetters();
        }
    }

    private void updateErrorStatus()
    {
        ErrorStatus = $"Errors : {mistakes} of {maxMistakes}";
    }

    private void checkWon()
    {
        if (SpotLight.Replace(" ","")==answer)
        {
            Message = "You Win";
            disableLetters();
        }
    }

    private void disableLetters()
    {
        foreach (var item in ContainerBtn.Children)
        {
            var btn = item as Button;
            if (btn!=null)
            {
                btn.IsEnabled = false;
                btn.BackgroundColor = new Color(128, 128, 128);
                btn.TextColor = new Color(0,0,0);
            }
        }
    }

    private void updateSpotLight(string answer, List<char> guessList)
    {
        var temp=answer.Select(p => (guessList.IndexOf(p) >= 0) ? p : '_').ToArray();
        SpotLight = string.Join(' ',temp);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        mistakes = 0;
        guessList = new List<char>();
        ImageName = "img0.jpg";
        PickWord();
        updateSpotLight(answer, guessList);
        Message = "";
        updateErrorStatus();
        EnableLetters();
    }
}

