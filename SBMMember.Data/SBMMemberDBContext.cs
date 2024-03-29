﻿using Microsoft.EntityFrameworkCore;
using System;
using SBMMember.Models;
namespace SBMMember.Data
{
    public class SBMMemberDBContext : DbContext
    {

        public SBMMemberDBContext(DbContextOptions<SBMMemberDBContext> options) : base(options)
        {

        }

        public DbSet<Members> Members { get; set; }
        public DbSet<Member_PersonalDetails> Member_PersonalDetails { get; set; }
        public DbSet<Member_BusinessDetails> Member_BusinessDetails { get; set; }
        public DbSet<Member_ContactDetails> Member_ContactDetails { get; set; }
        public DbSet<Member_EducationEmploymentDetails> Member_EducationEmploymentDetails { get; set; }
        public DbSet<Member_FamilyDetails> Member_FamilyDetails { get; set; }
        public DbSet<Member_FormStatus> Member_FormStatuses { get; set; }
        public DbSet<Member_PaymentsAndReciepts> Member_PaymentsAndReciepts { get; set; }
        public DbSet<JobPostings> JobPostings { get; set; }
        public DbSet<EventInfo> EventInfos { get; set; }
        public DbSet<EventGallery> EventGalleries { get; set; }
        public DbSet<EventAds> EventAds { get; set; }
        public DbSet<AdminUsers> AdminUsers { get; set; }
        public DbSet<MarqueeText> MarqueeTexts { get; set; }
        public DbSet<UpcomingEvent> UpcomingEvents { get; set; }
        public DbSet<EventTitles> EventTitles { get; set; }
        public DbSet<MemberMeetings> MemberMeetings { get; set; }
        public DbSet<BannerAds> BannerAdvts { get; set; }
        public DbSet<SBMSubscriptionCharges> SubscriptionCharges { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Members>().ToTable("Members");
            modelBuilder.Entity<Member_PersonalDetails>().ToTable("Member_PersonalDetails");
            modelBuilder.Entity<Member_BusinessDetails>().ToTable("Member_BusinessDetails");
            modelBuilder.Entity<Member_ContactDetails>().ToTable("Member_ContactDetails");
            modelBuilder.Entity<Member_EducationEmploymentDetails>().ToTable("Member_EducationEmploymentDetails");
            modelBuilder.Entity<Member_FamilyDetails>().ToTable("Member_FamilyDetails");
            modelBuilder.Entity<Member_FormStatus>().ToTable("Member_FormStatus");
            modelBuilder.Entity<Member_PaymentsAndReciepts>().ToTable("Member_PaymentsAndReciepts");
            modelBuilder.Entity<JobPostings>().ToTable("JobPostings");
            modelBuilder.Entity<EventInfo>().ToTable("EventInfo");
            modelBuilder.Entity<EventGallery>().ToTable("EventGallery");
            modelBuilder.Entity<EventAds>().ToTable("EventAds");
            modelBuilder.Entity<AdminUsers>().ToTable("AdminUsers");
            modelBuilder.Entity<MarqueeText>().ToTable("MarqueeText");
            modelBuilder.Entity<UpcomingEvent>().ToTable("UpcomingEvents");
            modelBuilder.Entity<EventTitles>().ToTable("EventTitles");
            modelBuilder.Entity<MemberMeetings>().ToTable("MemberMeetings");
            modelBuilder.Entity<BannerAds>().ToTable("BannerAds");
            modelBuilder.Entity<SBMSubscriptionCharges>().ToTable("SubscriptionCharges");
        }
    }
}
