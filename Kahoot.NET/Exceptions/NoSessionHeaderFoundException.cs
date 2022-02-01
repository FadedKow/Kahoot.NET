﻿namespace Kahoot.NET.Exceptions;


[Serializable]
public class NoSessionHeaderFoundException : Exception
{
    public NoSessionHeaderFoundException() { }
    public NoSessionHeaderFoundException(string message) : base(message) { }    
    public NoSessionHeaderFoundException(string message, Exception inner) : base(message, inner) { }
    protected NoSessionHeaderFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}