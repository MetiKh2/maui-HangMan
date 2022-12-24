using Android.Views.Animations;
using System.ComponentModel;

namespace Hangman;

public partial class MainPage : ContentPage,INotifyPropertyChanged
{

	private List<char> letters = new List<char>();

    public List<char> Letters
    {
        get { return letters; }
        set
        {
            letters = value;
            OnPropertyChanged();
        }
    }

    private string spotLight;

    public string SpotLight
    {
        get { return spotLight; }
        set { spotLight = value;
            OnPropertyChanged();
        }
    }

    private string message;

    public string Message
    {
        get { return message; }
        set { message = value;
            OnPropertyChanged();
        }
    }

    private string errorStatus;

    public string ErrorStatus
    {
        get { return errorStatus; }
        set { errorStatus = value;
            OnPropertyChanged();
        }
    }

    private string imageName = "img0.jpg";

    public string ImageName
    {
        get { return imageName; }
        set { imageName = value;
            OnPropertyChanged();
        }
    }





    List<string> words = new List<string>()
    {
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

    public int mistake = 0;
    public int maxMistake = 6;
	public MainPage()
	{
		InitializeComponent();
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
        BindingContext = this;

        PickWord();
        UpdateSpotlight(answer, guessList);

    }

    public void PickWord()
    {
        answer = words[new Random().Next(0, words.Count)];
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;

        var selectedLetter = btn.Text;
        btn.IsEnabled = false;
        HandleGeuss(selectedLetter[0]);
    }


    List<char> guessList = new List<char>();
    public void HandleGeuss(char letter)
    {
        if(guessList.IndexOf(letter) == -1)
        {
            guessList.Add(letter);
        }
        
        if (answer.IndexOf(letter) > 0)
        {
            UpdateSpotlight(answer, guessList);
            CheckIfWon();
        }
        else if(answer.IndexOf(letter) == -1)
        {
            mistake++;
            updateErrorStatus();
            CheckIfLost();
            ImageName = $"img{mistake}.jpg";
        }
    }

    public void UpdateSpotlight(string answer, List<char> guess)
    {
        var temp = answer.Select(x => (guess.IndexOf(x) >= 0) ? x : '_').ToArray();

        SpotLight = string.Join(' ', temp);
    }

    public void CheckIfWon()
    {
        if(SpotLight.Replace(" ","") == answer)
        {
            Message = "You Win!";
            DisableLetters();
        }
    }

    public void CheckIfLost()
    {
        if(mistake == maxMistake)
        {
            Message = "You Lost!!";
            DisableLetters();
        }
    }

    public void DisableLetters()
    {
        foreach (var children in ContainerBtn.Children)
        {
            var btn = children as Button;

            if (btn != null)
            {
                btn.IsEnabled = false;
            }
        }
    }

    public void EnableLetters()
    {
        foreach (var children in ContainerBtn.Children)
        {
            var btn = children as Button;

            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }

    public void updateErrorStatus()
    {
        ErrorStatus = $"Errors: {mistake} of {maxMistake}";
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        mistake = 0;
        guessList = new List<char>();
        ImageName = "img0.jpg";
        PickWord();
        UpdateSpotlight(answer, guessList);
        Message = "";
        updateErrorStatus();
        EnableLetters();
    }
}

