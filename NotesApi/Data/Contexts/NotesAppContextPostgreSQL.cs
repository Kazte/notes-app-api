﻿using Microsoft.EntityFrameworkCore;
using NotesApi.Data.Interfaces;
using NotesApi.Shared.Models;

namespace NotesApi.Data.Contexts;

public class NotesAppContextPostgreSQL : DbContext, INotesAppContext
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }

    public NotesAppContextPostgreSQL(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>().Property(o => o.NoteCreated).HasDefaultValueSql("now()");
        modelBuilder.Entity<Note>().Property(o => o.NoteModified).HasDefaultValueSql("now()");
    }

    public async Task<IEnumerable<Note>> GetNotes()
    {
        return await Notes.ToListAsync();
    }

    public async Task<Note?> GetNoteById(int noteId)
    {
        return await Notes.FirstOrDefaultAsync(n => n.Id == noteId);
    }

    public async Task<IEnumerable<Note>> GetNotesByUserId(int userId)
    {
        return await Notes.Where(n => n.userId == userId).ToListAsync();
    }

    public async Task PostNote(Note note)
    {
        await Notes.AddAsync(note);
        await SaveChangesAsync();
    }

    public async Task PutNote(Note note)
    {
        Notes.Update(note);
        await SaveChangesAsync();
    }

    public async Task<bool> DeleteNote(Note note)
    {
        var n = await Notes.FirstOrDefaultAsync(x => x.Id == note.Id);

        if (n is null)
            return false;

        Notes.Remove(n);
        await SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await Users.Include(u => u.Roles).ToListAsync();
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await Users.Include(u => u.Roles).FirstOrDefaultAsync(n => n.Id == userId);
    }

    public async Task PostUser(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
    }

    public Task PutUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUser(User user)
    {
        throw new NotImplementedException();
    }
}