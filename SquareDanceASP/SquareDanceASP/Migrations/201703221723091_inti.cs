namespace SquareDanceASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pet",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Breed = c.String(),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Years = c.Int(nullable: false),
                        Months = c.Int(nullable: false),
                        Sex = c.String(),
                        Spayed = c.Boolean(nullable: false),
                        Microchipped = c.Boolean(nullable: false),
                        WellDogs = c.Boolean(nullable: false),
                        WellDogDetail = c.String(),
                        WellCats = c.Boolean(nullable: false),
                        WellCatDetail = c.String(),
                        WellChild = c.Boolean(nullable: false),
                        WellChildDetail = c.String(),
                        HouseTrained = c.Boolean(nullable: false),
                        HouseTrainedDetail = c.String(),
                        SpecialRequirement = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PetImage",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PetId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pet", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BirthDay = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        Name = c.String(),
                        ProfileImagePath = c.String(),
                        Address = c.String(),
                        ConnectEmail = c.String(),
                        WeChat = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Location = c.Geography(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ServiceAndRate",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DogBoarding = c.Boolean(nullable: false),
                        DogBoardingFee = c.Double(nullable: false),
                        HouseSitting = c.Boolean(nullable: false),
                        HouseSittingFee = c.Double(nullable: false),
                        DropInVisits = c.Boolean(nullable: false),
                        DropInVisitsFee = c.Double(nullable: false),
                        DogWalking = c.Boolean(nullable: false),
                        DogWalkingFee = c.Double(nullable: false),
                        DoggyDayCare = c.Boolean(nullable: false),
                        DoggyDayCareFee = c.Double(nullable: false),
                        DayCareDogs = c.Int(nullable: false),
                        FullTimeWeek = c.Boolean(nullable: false),
                        PottyBreaks = c.String(),
                        Flexible = c.Boolean(nullable: false),
                        DogWalkingTime = c.String(),
                        BoardingSmallDog = c.Boolean(nullable: false),
                        BoardingMediumDog = c.Boolean(nullable: false),
                        BoardingLargeDog = c.Boolean(nullable: false),
                        BoardingGiantDog = c.Boolean(nullable: false),
                        BoardingUnderOne = c.Boolean(nullable: false),
                        HostDifferentFamily = c.Boolean(nullable: false),
                        HostMaleNotNeutered = c.Boolean(nullable: false),
                        HostFemaleNotSpayed = c.Boolean(nullable: false),
                        HostNeedCrateTrained = c.Boolean(nullable: false),
                        HouseSmallDog = c.Boolean(nullable: false),
                        HouseMediumDog = c.Boolean(nullable: false),
                        HouseLargeDog = c.Boolean(nullable: false),
                        HouseGiantDog = c.Boolean(nullable: false),
                        HouseUnderOne = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Sitter", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Sitter",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        EmergencyContactName = c.String(),
                        EmergencyContactPhoneNumber = c.String(),
                        Years = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Sittermage",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sitter", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SitterProfile",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LiveCondition = c.String(),
                        AnySmoker = c.Boolean(nullable: false),
                        HaveChildren = c.Boolean(nullable: false),
                        HaveCats = c.Boolean(nullable: false),
                        CagedPets = c.Boolean(nullable: false),
                        SittingFurniture = c.Boolean(nullable: false),
                        SittingBed = c.Boolean(nullable: false),
                        DogExperience = c.Int(nullable: false),
                        Describe = c.String(),
                        DogCPR = c.Boolean(nullable: false),
                        OralMedication = c.Boolean(nullable: false),
                        InjectedMedication = c.Boolean(nullable: false),
                        SeniorDogExperience = c.Boolean(nullable: false),
                        ExericiseForHighEnergyDog = c.Boolean(nullable: false),
                        Sitter_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Sitter", t => t.Sitter_UserId)
                .Index(t => t.Sitter_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SitterProfile", "Sitter_UserId", "dbo.Sitter");
            DropForeignKey("dbo.Sittermage", "UserId", "dbo.Sitter");
            DropForeignKey("dbo.ServiceAndRate", "UserId", "dbo.Sitter");
            DropForeignKey("dbo.Sitter", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pet", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PetImage", "PetId", "dbo.Pet");
            DropIndex("dbo.SitterProfile", new[] { "Sitter_UserId" });
            DropIndex("dbo.Sittermage", new[] { "UserId" });
            DropIndex("dbo.Sitter", new[] { "UserId" });
            DropIndex("dbo.ServiceAndRate", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.PetImage", new[] { "PetId" });
            DropIndex("dbo.Pet", new[] { "UserId" });
            DropTable("dbo.SitterProfile");
            DropTable("dbo.Sittermage");
            DropTable("dbo.Sitter");
            DropTable("dbo.ServiceAndRate");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PetImage");
            DropTable("dbo.Pet");
        }
    }
}
