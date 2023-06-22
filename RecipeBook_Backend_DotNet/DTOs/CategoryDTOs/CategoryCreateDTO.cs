namespace RecipeBook_Backend_DotNet.DTOs.CategoryDTOs
{
    public record struct CategoryCreateDTO(
        string Name,
        string PicUrl
        );
}
