global using RecipeBook_Backend_DotNet.Models;
global using Microsoft.EntityFrameworkCore;
global using RecipeBook_Backend_DotNet.Data;

using RecipeBook_Backend_DotNet.Services.RecipeServices;
using RecipeBook_Backend_DotNet.Services.IngredientServices;
using RecipeBook_Backend_DotNet.Services.CategoryServices;
using RecipeBook_Backend_DotNet.Services.LikeServices;
using RecipeBook_Backend_DotNet.Services.CommentServices;
using RecipeBook_Backend_DotNet.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
