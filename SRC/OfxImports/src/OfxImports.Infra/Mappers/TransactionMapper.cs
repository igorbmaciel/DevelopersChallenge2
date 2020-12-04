using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfxImports.Domain.Entities;

namespace OfxImports.Infra.Mappers
{
    public class TransactionMapper : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> mapper)
        {
            mapper.ToTable("Transaction");

            mapper.Property(e => e.Id)
                .HasAnnotation("Relational:ColumnName", "TransactionId")
                .ValueGeneratedOnAdd();

            mapper.HasKey(e => e.Id);

            mapper.Property(e => e.BankAccountId)
                .IsRequired();

            mapper.Property(e => e.Type);

            mapper.Property(e => e.TransactionValue);

            mapper.Property(e => e.Description)
                 .HasMaxLength(4000)
                 .IsUnicode(false)
                 .IsRequired()
                 .HasAnnotation("Relational:ColumnName", "Description");

            mapper.Property(e => e.Date)
                       .HasColumnType("timestamp")
                       .HasDefaultValueSql("(now() at time zone 'utc')")
                       .HasAnnotation("Relational:ColumnName", "Date");

            mapper.HasOne(d => d.BankAccount)
               .WithMany(p => p.TransactionList)
               .HasForeignKey(d => d.BankAccountId)
               .HasConstraintName("FK_Transaction_BankAccount");
          

            mapper.HasIndex(e => e.Id)
                 .IsUnique();
        }
    }
}
