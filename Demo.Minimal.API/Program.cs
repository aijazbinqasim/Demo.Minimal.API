using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Demo.Minimal.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minimal API",
                    Version = "v1",
                    Description = "Showing how you can build minimal api with .net"
                });
            });


            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapPost("/api/book", ([FromBody] Book b) =>
            {
                var book = new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author
                };

                return Results.Created($"/api/book/{book.Id}", book);
            })
                .WithDisplayName("Create Books")
                .Accepts<object>("application/json");


            app.MapGet("/api/book/{id}", ([FromRoute] int id) =>
            {
                var book = new Book
                {
                    Id = 100,
                    Title = "C#",
                    Author = "John"
                };

                return book.Id == id ? Results.Ok(book) : Results.NotFound();
            })
                .WithDisplayName("Find Book By ID")
                 .Produces(404);


            app.MapDelete("/api/book/{id}", ([FromRoute] int id) =>
            {
                return Results.NoContent();
            }).WithDisplayName("Delete Book By ID");


            app.Run();
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
    }   
}
