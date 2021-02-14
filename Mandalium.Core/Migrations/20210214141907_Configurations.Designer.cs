﻿// <auto-generated />
using System;
using Mandalium.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mandalium.Core.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210214141907_Configurations")]
    partial class Configurations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Mandalium.Core.Models.BlogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("Headline")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("InnerTextHtml")
                        .IsRequired()
                        .HasColumnType("varchar(MAX)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("SubHeadline")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<int>("TimesRead")
                        .HasColumnType("int");

                    b.Property<int?>("TopicId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("WriterEntry")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.HasIndex("UserId");

                    b.ToTable("BlogEntries");
                });

            modelBuilder.Entity("Mandalium.Core.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BlogEntryId")
                        .HasColumnType("int");

                    b.Property<string>("CommentString")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<string>("CommenterName")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlogEntryId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Mandalium.Core.Models.MostReadEntries", b =>
                {
                    b.Property<int>("BlogEntryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsWriterEntry")
                        .HasColumnType("bit");

                    b.ToTable("MostReadEntries");
                });

            modelBuilder.Entity("Mandalium.Core.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("varchar(300)");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Mandalium.Core.Models.SystemSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("SystemSettings");
                });

            modelBuilder.Entity("Mandalium.Core.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Mandalium.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Background")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("varchar(300)");

                    b.Property<bool>("Role")
                        .HasColumnType("bit");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Mandalium.Core.Models.BlogEntry", b =>
                {
                    b.HasOne("Mandalium.Core.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId");

                    b.HasOne("Mandalium.Core.Models.User", "User")
                        .WithMany("BlogEntries")
                        .HasForeignKey("UserId");

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mandalium.Core.Models.Comment", b =>
                {
                    b.HasOne("Mandalium.Core.Models.BlogEntry", "BlogEntry")
                        .WithMany("Comments")
                        .HasForeignKey("BlogEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mandalium.Core.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");

                    b.Navigation("BlogEntry");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mandalium.Core.Models.Photo", b =>
                {
                    b.HasOne("Mandalium.Core.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mandalium.Core.Models.BlogEntry", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Mandalium.Core.Models.User", b =>
                {
                    b.Navigation("BlogEntries");

                    b.Navigation("Comments");

                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
