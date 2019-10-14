using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom
{
    public string correct;
    public string stageName;
    public string stageQuestion;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;
    public List<string> details = new List<string>();

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

    
    

    public string StageName { get; set; }
    public string StageQuestion { get; set; }
    public string Answer1 { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Answer4 { get; set; }
    public string Correct{ get; set; }
}
