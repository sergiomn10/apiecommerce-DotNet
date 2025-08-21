using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using Mapster;

namespace ApiEcommerce.Mapping;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Category, CategoryDto>.NewConfig().TwoWays();
        TypeAdapterConfig<Category, CreateCategoryDto>.NewConfig().TwoWays();

        TypeAdapterConfig<Product, ProductDto>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null)
            .TwoWays();
        TypeAdapterConfig<Product, CreateProductDto>.NewConfig().TwoWays();
        TypeAdapterConfig<Product, UpdateProductDto>.NewConfig().TwoWays();

        TypeAdapterConfig<User, UserDto>.NewConfig().TwoWays();
        TypeAdapterConfig<User, CreateUserDto>.NewConfig().TwoWays();
        TypeAdapterConfig<User, UserLoginDto>.NewConfig().TwoWays();
        TypeAdapterConfig<User, UserLoginResponseDto>.NewConfig().TwoWays();
        TypeAdapterConfig<ApplicationUser, UserDataDto>.NewConfig().TwoWays();
        TypeAdapterConfig<ApplicationUser, UserDto>.NewConfig().TwoWays();
    }
}
