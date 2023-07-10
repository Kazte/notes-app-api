﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.Shared.Models;

[Table("Notes")]
public class Note
{
    [Key]
    public int Id { get; set; }
    public string NoteTitle { get; set; }
    public string NoteBody { get; set; }
    public DateTime NoteCreated { get; set; }
    public DateTime NoteModified { get; set; }
    public int userId { get; set; }
    public User User { get; set; }
}