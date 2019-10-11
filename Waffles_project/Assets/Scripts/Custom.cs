using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom
{
    private string correct;
    private string stageName;
    private string stageQuestion;
    private string answer1;
    private string answer2;
    private string answer3;
    private string answer4;


    public Custom()
    {

    }

    public Custom(string stageName , string stageQuestion , string answer1 , string answer2 , string answer3 , string answer4,string correct)
    {
        this.StageName = stageName;
        this.StageQuestion = stageQuestion;
        this.Answer1 = answer1;
        this.Answer2 = answer2;
        this.Answer3 = answer3;
        this.Answer4 = answer4;
        this.correct = correct;
    }

    public string StageName { get => stageName; set => stageName = value; }
    public string StageQuestion { get => stageQuestion; set => stageQuestion = value; }
    public string Answer1 { get => answer1; set => answer1 = value; }
    public string Answer2 { get => answer2; set => answer2 = value; }
    public string Answer3 { get => answer3; set => answer3 = value; }
    public string Answer4 { get => answer4; set => answer4 = value; }
    public string Correct { get => correct; set => correct = value; }
}
