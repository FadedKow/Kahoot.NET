﻿namespace Kahoot.NET.Question;

public class Question : IQuestion
{
    public async Task AnswerAsync(int id)
    {
    }
    public int Position { get; }
    public TimeSpan TimeLeft { get; }
    public QuestionType QuestionType { get; }
    public bool Ended { get; }
    public int Number { get; }
    public GameMode GameMode { get; }
}