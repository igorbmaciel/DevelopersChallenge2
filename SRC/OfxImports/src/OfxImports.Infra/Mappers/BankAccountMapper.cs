using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfxImports.Domain.Entities;

namespace OfxImports.Infra.Mappers
{
    public class BankAccountMapper : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> mapper)
        {
            mapper.ToTable("BankAccount");

            mapper.Property(e => e.Id)
                .HasAnnotation("Relational:ColumnName", "BankAccountId")
                .ValueGeneratedOnAdd();

            mapper.HasKey(e => e.Id);

            mapper.Property(e => e.Type);

            mapper.Property(e => e.AgencyCode);

            mapper.Property(e => e.Code);

            mapper.Property(e => e.AccountCode);

            mapper.HasIndex(e => e.Id)
                 .IsUnique();
        }
    }
}
