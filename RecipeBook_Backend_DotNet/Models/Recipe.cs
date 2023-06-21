﻿namespace RecipeBook_Backend_DotNet.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        //public int RecipeCategoryId { get; set; }
        public string RecipeDescription { get; set; } = String.Empty;
        public string PrepareTime { get; set; } = String.Empty;
        public string CookTime { get; set; } = String.Empty;
        public double Rating { get; set; }
        public string PublishingStatus { get; set; } = String.Empty;
        public string Visibility { get; set; } = String.Empty;

        public Category Category { get; set; }

        public User User { get; set; }

        public List<Ingredient> Ingredients { get; set;}
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
    }
}