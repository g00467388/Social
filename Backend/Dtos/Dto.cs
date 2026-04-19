/*
    This class is used as a genric type constraint in service.cs, 
    with the purpose of preventing usage of non DTO classes
*/
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public abstract record Dto
{
    [BindNever]
    public Guid ID {get; set;}
}
