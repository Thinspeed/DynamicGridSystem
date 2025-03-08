using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids;

public class GridConfiguration : EntityConfiguration<Grid>;