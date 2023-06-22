﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RecipeBook_Backend_DotNet.DTOs.CommentDTOs;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;

        public CommentService(DataContext context)
        {
            _context = context;
        }
        public async Task<CommentMinimalDTO?> AddComment(CommentCreateDTO request)
        {
            var recipe = await _context.Recipes.FindAsync(request.RecipeId);
            if (recipe is null)
                return null;

            var user = await _context.Users.FindAsync(request.UserId);
            if (user is null)
                return null;

            var newComment = new Comment { 
                Text = request.Text,
                Username = user.Username,
                User = user,
                Recipe = recipe
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var fComment = await _context.Comments.FindAsync(newComment.Id);
            if (fComment is null)
                return null;

            var userDTO = new UserMinimalDTO
            {
                Id = user.Id,
                Username = user.Username
            };

            var commentDTO = new CommentMinimalDTO
            {
                Id = fComment.Id,
                Text = fComment.Text,
                User = userDTO
            };
            return commentDTO;
        }

        public async Task<CommentMinimalDTO?> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment is null)
                return null;
            else
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            var user = await _context.Users.FindAsync(comment.UserId);
            if (user is null)
                return null;

            var userDTO = new UserMinimalDTO
            {
                Id = user.Id,
                Username = user.Username
            };

            var commentDTO = new CommentMinimalDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                User = userDTO
            };
            return commentDTO;
        }

        public async Task<List<CommentPackedDTO>?> GetAllCommentsByRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
                return null;

            var user = await _context.Users.FindAsync(recipe.UserId);
            if (user is null)
                return null;

            RecipeMinimalDTO recipeDTO = new() { Id = id };
            UserMinimalDTO userDTO = new(){ 
                Id = user.Id, 
                Username = user.Username
            };

            var comments = await _context.Comments.
                Where(comment => comment.RecipeId == id).
                ToListAsync();
            if (comments is null)
                return null;

            List<CommentPackedDTO> commentsDTO = comments.Select(comment => new CommentPackedDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                User = userDTO,
                Recipe = recipeDTO
            }).ToList();

            return commentsDTO;
        }
    }
}