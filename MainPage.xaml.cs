using System.ComponentModel;

namespace Hangman_project4_maui;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    #region UI Properties
    public string Spotlight { 
		get => spotlight;
		set {
            spotlight = value;
			OnPropertyChanged();
        }  
	}
	private List<char> letters = new List<char>();

	public List<char> Letters
	{
		get { return letters; }
		set { 
			letters = value;
            OnPropertyChanged();
        }
	}
	private string message;

	public string Message
	{
		get { return message; }
		set { 
			message = value; 
			OnPropertyChanged();
		}
	}
	private string gameStatus;

	public string GameStatus
	{
		get { return gameStatus; }
		set { 
			gameStatus = value;
            OnPropertyChanged();
        }
	}
	private string currentImage ="img0.jpg";

	public string CurrentImage
	{
		get { return currentImage; }
		set { 
			currentImage = value;
            OnPropertyChanged();
        }
	}

	int mistakes = 0;
	int maxWrong = 6;
	#endregion
	#region Fields
	List<string> words = new List<string>()
	{
		"python",
		"javascript",
		"maui",
		"csharp",
		"issam",
		"mama",
		"rachida",
		"chaimae",
		"mazzouz",
		"boutissante",
		"mohamed"
	};
	string answer = "";
	private string spotlight;
	List<char> guessed = new List<char>();
    #endregion

    public MainPage()
	{
		InitializeComponent();
		Letters.AddRange("azertyuiopqsdfghjklmwxcvbn");		
		BindingContext = this;
		PickWord();
		CalculateWord(answer, guessed);

    }
	#region Game Engine
	private void PickWord()
	{
		answer = words[new Random().Next(0,words.Count)];
	}
	private void CalculateWord(string answer, List<char> gussed)
	{
		var temp = answer.Select(x => (gussed.IndexOf(x) >= 0 ? x : '_')).ToArray();
		Spotlight = string.Join(" ",temp);
	}
    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
		var btn = sender as Button;
		if (btn != null)
		{
			var letter = btn.Text;
			btn.IsEnabled = false;
			HandlGuess(letter[0]);
		}
    }

    private void HandlGuess(char v)
    {
		if(guessed.IndexOf(v) == -1)
		{
			guessed.Add(v);
		}
		if(answer.IndexOf(v) >= 0)
		{
			CalculateWord(answer, guessed);
			CheckIfGameWon();
		}
		else if(answer.IndexOf(v) == -1)
		{
			mistakes++;
			UpdateStatus();
			CheckIfGameLost();
			CurrentImage = $"img{mistakes}.jpg";

        }
    }

    private void CheckIfGameLost()
    {
        if(mistakes == maxWrong)
		{
			Message = "You List!!";
			DisabledLetters();
		}
    }

    private void DisabledLetters()
    {
        foreach (var children in LettersContainer.Children)
        {
			var btn = children as Button;
			if (btn != null)
			{
				btn.IsEnabled = false;
			}
        }
    }

    private void CheckIfGameWon()
    {
        if(Spotlight.Replace(" ", "") == answer)
		{
			Message = "You win!";
            DisabledLetters();

        }
    }

	private void UpdateStatus()
	{
		GameStatus = $"Errors: {mistakes} of {maxWrong }";
	}

    private void Reset_Clicked(object sender, EventArgs e)
    {
		mistakes = 0;
		guessed = new List<char>();
		CurrentImage = "img0.jpg";
		PickWord();
		Message = "";
		UpdateStatus();
		EnabledLetters();
    }
    private void EnabledLetters()
    {
        foreach (var children in LettersContainer.Children)
        {
            var btn = children as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }
}

