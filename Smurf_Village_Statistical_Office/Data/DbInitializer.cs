using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;
using System.Drawing;
using System.Xml.Linq;

namespace Smurf_Village_Statistical_Office.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SmurfVillageContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.LeisureVenues.Any())
                return;

            var houses = new List<MushroomHouse>
            {
                new() { Id = 1, Capacity = 10, Color = Color.Red,    Motto = "Red's Finest",    AcceptedFoods = new List<Food> { Food.Apple, Food.Bread } },
                new() { Id = 2, Capacity = 12, Color = Color.Blue,   Motto = "Blue Lagoon",    AcceptedFoods = new List<Food> { Food.Cheese, Food.Wine } },
                new() { Id = 3, Capacity = 8,  Color = Color.Green,  Motto = "Green Haven",    AcceptedFoods = new List<Food> { Food.Salad, Food.Soup } },
                new() { Id = 4, Capacity = 15, Color = Color.Yellow, Motto = "Sunny Side Up",  AcceptedFoods = new List<Food> { Food.Egg, Food.Bacon } },
                new() { Id = 5, Capacity = 5,  Color = Color.Purple, Motto = "Purple Palace",  AcceptedFoods = new List<Food> { Food.Grape, Food.Cake } },
                new() { Id = 6,  Capacity = 11, Color = Color.Red,    Motto = "Scarlet Sanctuary",   AcceptedFoods = new List<Food> { Food.Chocolate,      Food.Cupcake      } },
                new() { Id = 7,  Capacity = 9,  Color = Color.Blue,   Motto = "Blue Bell Bungalow",  AcceptedFoods = new List<Food> { Food.Bagel,          Food.Yogurt       } },
                new() { Id = 8,  Capacity = 13, Color = Color.Green,  Motto = "Emerald Escape",      AcceptedFoods = new List<Food> { Food.Salad,          Food.Soup         } },
                new() { Id = 9,  Capacity = 14, Color = Color.Yellow, Motto = "Golden Grove",        AcceptedFoods = new List<Food> { Food.Egg,            Food.Bacon        } },
                new() { Id = 10, Capacity = 6,  Color = Color.Purple, Motto = "Violet View",         AcceptedFoods = new List<Food> { Food.Grape,          Food.Pie          } },
                new() { Id = 11, Capacity = 10, Color = Color.Red,    Motto = "Crimson Corner",      AcceptedFoods = new List<Food> { Food.Popcorn,        Food.Chips        } },
                new() { Id = 12, Capacity = 8,  Color = Color.Blue,   Motto = "Azure Abode",         AcceptedFoods = new List<Food> { Food.Salmon,         Food.Shrimp       } },
                new() { Id = 13, Capacity = 12, Color = Color.Green,  Motto = "Olive Oasis",         AcceptedFoods = new List<Food> { Food.OliveOil,       Food.Vinegar      } },
                new() { Id = 14, Capacity = 7,  Color = Color.Yellow, Motto = "Sunbeam Suite",       AcceptedFoods = new List<Food> { Food.Smoothie,       Food.Milkshake    } },
                new() { Id = 15, Capacity = 9,  Color = Color.Purple, Motto = "Royal Retreat",       AcceptedFoods = new List<Food> { Food.Cheesecake,     Food.Macaron      } }
            };
            context.MushroomHouses.AddRange(houses);

            var workplaces = new List<WorkingPlace>
            {
            new() {
                                Id = 1,
                Name = "Health Center",
                AcceptedJobs = new List<Job> {
                    Job.Dentist,
                    Job.Biologist,
                    Job.ChemicalEngineer,
                    Job.Surgeon,
                    Job.Cleaner
                }
            },
            new() {
                Id = 2,
                Name = "Tech Hub",
                AcceptedJobs = new List<Job> {
                    Job.FrontendDeveloper,
                    Job.BackendDeveloper,
                    Job.FullStackDeveloper,
                    Job.DevOpsEngineer,
                    Job.DatabaseAdministrator,
                    Job.SystemAdministrator,
                    Job.NetworkEngineer,
                    Job.DataScientist,
                    Job.AIResearcher
                }
            },
            new() {
                Id = 3,
                Name = "Engineering Works",
                AcceptedJobs = new List<Job> {
                    Job.MechanicalEngineer,
                    Job.CivilEngineer,
                    Job.ElectricalEngineer,
                    Job.ChemicalEngineer,
                    Job.EnvironmentalEngineer,
                    Job.ProcessEngineer,
                    Job.QualityAssuranceSpecialist,
                    Job.MaintenanceTechnician
                }
            },
            new() {
                Id = 4,
                Name = "University",
                AcceptedJobs = new List<Job> {
                    Job.EarlyChildhoodTeacher,
                    Job.PrimarySchoolTeacher,
                    Job.SecondarySchoolTeacher,
                    Job.UniversityLecturer,
                    Job.UniversityProfessor,
                    Job.Researcher,
                    Job.EducationCoordinator,
                    Job.TeachingAssistant
                }
            },
            new() {
                Id = 5,
                Name = "Hospitality Plaza",
                AcceptedJobs = new List<Job> {
                    Job.Chef,
                    Job.Waiter,
                    Job.Barista,
                    Job.Sommelier,
                    Job.HotelReceptionist
                }
            },
            new() {
                Id = 6,
                Name = "Retail Outlet",
                AcceptedJobs = new List<Job> {
                    Job.SalesRepresentative,
                    Job.StoreManager,
                    Job.CustomerServiceRepresentative,
                    Job.LogisticsCoordinator,
                    Job.WarehouseWorker
                }
            },
            new() {
                Id = 7,
                Name = "Consulting Firm",
                AcceptedJobs = new List<Job> {
                    Job.Consultant,
                    Job.ProjectManager,
                    Job.Recruiter,
                    Job.HRSpecialist,
                    Job.MarketingSpecialist
                }
            },
            new() {
                Id = 8,
                Name = "Banking Branch",
                AcceptedJobs = new List<Job> {
                    Job.BankClerk,
                    Job.FinancialAnalyst,
                    Job.TaxAdvisor,
                    Job.Accountant,
                    Job.InsuranceAgent
                }
            },
            new() {
                Id = 9,
                Name = "Law Office",
                AcceptedJobs = new List<Job> {
                    Job.Lawyer,
                    Job.LegalAssistant,
                    Job.Judge,                     
                    Job.Consultant
                }
            },
            new() {
                Id = 10,
                Name = "Pharmacy",
                AcceptedJobs = new List<Job> {
                    Job.Pharmacist,
                    Job.LaboratoryTechnician,
                    Job.Dietitian,
                    Job.Nurse,
                    Job.ChemicalEngineer
                }
            },
            new() {
                Id = 11,
                Name = "Innovation Lab",
                AcceptedJobs = new List<Job> {
                    Job.AIResearcher,
                    Job.DataScientist,
                    Job.CybersecuritySpecialist,
                    Job.Researcher,
                    Job.FrontendDeveloper
                }
            },
            new() {
                Id = 12,
                Name = "Construction Site",
                AcceptedJobs = new List<Job> {
                    Job.CivilEngineer,
                    Job.MechanicalEngineer,
                    Job.ElectricalEngineer,
                    Job.QualityAssuranceSpecialist,
                    Job.MaintenanceTechnician
                }
            },
            new() {
                Id = 13,
                Name = "General Hospital",
                AcceptedJobs = new List<Job> {
                    Job.GeneralPractitioner,
                    Job.Surgeon,
                    Job.Pediatrician,
                    Job.Radiologist,
                    Job.Paramedic
                }
            },
            new() {
                Id = 14,
                Name = "Beauty Salon",
                AcceptedJobs = new List<Job> {
                    Job.Hairdresser,
                    Job.Beautician,
                    Job.Masseur,
                    Job.WellnessTherapist,

                }
            },
            new() {
                Id = 15,
                Name = "Sports Center",
                AcceptedJobs = new List<Job> {
                    Job.FitnessTrainer,
                    Job.Physiotherapist,
                    Job.Dietitian,
                    Job.YogaInstructor,
                }
            }

            };
            context.WorkingPlaces.AddRange(workplaces);

            var venues = new List<LeisureVenue>
            {
                new LeisureVenue { Id = 1, Name = "Riverbank Retreat",    Capacity = 20, AcceptedBrand = Brand.Nike         },
                new LeisureVenue { Id = 2, Name = "Mushroom Meadow",      Capacity = 15, AcceptedBrand = Brand.Adidas       },
                new LeisureVenue { Id = 3, Name = "Blue Lake Pavilion",   Capacity = 25, AcceptedBrand = Brand.Puma          },
                new LeisureVenue { Id = 4, Name = "Golden Grove",         Capacity = 18, AcceptedBrand = Brand.Gucci         },
                new LeisureVenue { Id = 5, Name = "Whispering Woods",     Capacity = 30, AcceptedBrand = Brand.Zara          },
                new LeisureVenue { Id = 6, Name = "Rainbow Falls",        Capacity = 12, AcceptedBrand = Brand.LouisVuitton },
                new LeisureVenue { Id = 7, Name = "Crystal Cavern",       Capacity = 22, AcceptedBrand = Brand.Prada         },
                new LeisureVenue { Id = 8, Name = "Sunny Glade",          Capacity = 16, AcceptedBrand = Brand.Chanel        },
                new LeisureVenue { Id = 9, Name = "Moonlit Clearing",     Capacity = 28, AcceptedBrand = Brand.Burberry      },
                new LeisureVenue { Id = 10,Name = "Stardust Pavilion",    Capacity = 24, AcceptedBrand = Brand.RayBan        },
                new LeisureVenue { Id = 11, Name = "Emerald Springs",    Capacity = 18, AcceptedBrand = Brand.UnderArmour   },
                new LeisureVenue { Id = 12, Name = "Twilight Terrace",   Capacity = 20, AcceptedBrand = Brand.Reebok        },
                new LeisureVenue { Id = 13, Name = "Sunset Promenade",   Capacity = 14, AcceptedBrand = Brand.NewBalance    },
                new LeisureVenue { Id = 14, Name = "Whispering Canyon",  Capacity = 26, AcceptedBrand = Brand.Versace       },
                new LeisureVenue { Id = 15, Name = "Enchanted Orchard",  Capacity = 16, AcceptedBrand = Brand.Fendi         },
                new LeisureVenue { Id = 16, Name = "Sapphire Sanctuary", Capacity = 22, AcceptedBrand = Brand.Balenciaga    },
                new LeisureVenue { Id = 17, Name = "Gilded Grotto",      Capacity = 19, AcceptedBrand = Brand.SaintLaurent },
                new LeisureVenue { Id = 18, Name = "Mystic Falls",       Capacity = 24, AcceptedBrand = Brand.Supreme       },
                new LeisureVenue { Id = 19, Name = "Hidden Hollow",      Capacity = 12, AcceptedBrand = Brand.OffWhite      },
                new LeisureVenue { Id = 20, Name = "Velvet Cove",        Capacity = 21, AcceptedBrand = Brand.Coach         }
            };
            context.LeisureVenues.AddRange(venues);

            var smurfs = new List<Smurf>
            {
                new Smurf { Id = 1,  Name = "Smurfette",       Age = 17, Job = Job.FlowerPicker,          FavouriteFood = Food.Pasta,       FavouriteBrand = Brand.Chanel,           FavouriteColor = Color.Pink    },
                new Smurf { Id = 2,  Name = "Marigold",        Age = 31, Job = Job.Pediatrician,             FavouriteFood = Food.Salad,       FavouriteBrand = Brand.Adidas,         FavouriteColor = Color.Yellow  },
                new Smurf { Id = 3,  Name = "Quartz",          Age = 28, Job = Job.ChemicalEngineer,         FavouriteFood = Food.Orange,      FavouriteBrand = Brand.Puma,           FavouriteColor = Color.Green   },
                new Smurf { Id = 4,  Name = "Byte",            Age = 22, Job = Job.FullStackDeveloper,        FavouriteFood = Food.Burger,      FavouriteBrand = Brand.UnderArmour,    FavouriteColor = Color.Blue    },
                new Smurf { Id = 5,  Name = "Thistle",         Age = 35, Job = Job.Dentist,                   FavouriteFood = Food.Cheese,      FavouriteBrand = Brand.Gucci,          FavouriteColor = Color.Purple  },
                new Smurf { Id = 6,  Name = "Ember",           Age = 29, Job = Job.Barista,              FavouriteFood = Food.Chicken,     FavouriteBrand = Brand.Reebok,         FavouriteColor = Color.Orange   },
                new Smurf { Id = 7,  Name = "Tinker",          Age = 40, Job = Job.MechanicalEngineer,       FavouriteFood = Food.Soup,        FavouriteBrand = Brand.Zara,           FavouriteColor = Color.Gray     },
                new Smurf { Id = 8,  Name = "Blossom",         Age = 27, Job = Job.Researcher,               FavouriteFood = Food.Brownie,     FavouriteBrand = Brand.Chanel,         FavouriteColor = Color.Pink     },
                new Smurf { Id = 9,  Name = "Harbor",          Age = 33, Job = Job.Psychologist,             FavouriteFood = Food.Falafel,     FavouriteBrand = Brand.Prada,          FavouriteColor = Color.Blue    },
                new Smurf { Id = 10, Name = "Bramble",         Age = 26, Job = Job.Forester,                 FavouriteFood = Food.Nuts,        FavouriteBrand = Brand.Balenciaga,     FavouriteColor = Color.Green   },
                new Smurf { Id = 11, Name = "Cobalt",          Age = 38, Job = Job.DataScientist,            FavouriteFood = Food.Smoothie,    FavouriteBrand = Brand.NewBalance,     FavouriteColor = Color.Blue    },
                new Smurf { Id = 12, Name = "Saffron",         Age = 30, Job = Job.Chef,                     FavouriteFood = Food.Pie,         FavouriteBrand = Brand.Dior,           FavouriteColor = Color.Yellow  },
                new Smurf { Id = 13, Name = "Gale",            Age = 36, Job = Job.Pilot,                    FavouriteFood = Food.Sandwich,    FavouriteBrand = Brand.LouisVuitton,  FavouriteColor = Color.White   },
                new Smurf { Id = 14, Name = "Willow",          Age = 42, Job = Job.GeneralPractitioner,           FavouriteFood = Food.Yogurt,      FavouriteBrand = Brand.Hermes,         FavouriteColor = Color.Green   },
                new Smurf { Id = 15, Name = "Nimbus",          Age = 25, Job = Job.Meteorologist,            FavouriteFood = Food.IceCream,    FavouriteBrand = Brand.RayBan,         FavouriteColor = Color.Cyan    },
                new Smurf { Id = 16, Name = "Azure Shroud",     Age = 29, Job = Job.Internist,                   FavouriteFood = Food.Carrot,     FavouriteBrand = Brand.Zara,          FavouriteColor = Color.Orange  },
                new Smurf { Id = 17, Name = "Maple Rambler",     Age = 34, Job = Job.Pharmacist,                 FavouriteFood = Food.Sushi,      FavouriteBrand = Brand.Gucci,         FavouriteColor = Color.Purple  },
                new Smurf { Id = 18, Name = "Twilight Bard",     Age = 27, Job = Job.Psychologist,               FavouriteFood = Food.Chocolate,  FavouriteBrand = Brand.Adidas,        FavouriteColor = Color.Blue    },
                new Smurf { Id = 19, Name = "Crimson Scribe",    Age = 41, Job = Job.Radiologist,                FavouriteFood = Food.Quinoa,     FavouriteBrand = Brand.Coach,         FavouriteColor = Color.Red     },
                new Smurf { Id = 20, Name = "Olive Grove",       Age = 38, Job = Job.LaboratoryTechnician,       FavouriteFood = Food.Cheese,     FavouriteBrand = Brand.LouisVuitton,FavouriteColor = Color.Green   },
                new Smurf { Id = 21, Name = "Sienna Loom",       Age = 24, Job = Job.DataScientist,              FavouriteFood = Food.Salad,      FavouriteBrand = Brand.Uniqlo,       FavouriteColor = Color.Gray    },
                new Smurf { Id = 22, Name = "Marble Quill",      Age = 52, Job = Job.CybersecuritySpecialist,    FavouriteFood = Food.Beer,       FavouriteBrand = Brand.Supreme,      FavouriteColor = Color.Black   },
                new Smurf { Id = 23, Name = "Jade Wanderer",     Age = 31, Job = Job.UIUXDesigner,               FavouriteFood = Food.Taco,       FavouriteBrand = Brand.RayBan,       FavouriteColor = Color.Green   },
                new Smurf { Id = 24, Name = "Pearl Song",        Age = 44, Job = Job.ITSupportSpecialist,        FavouriteFood = Food.Gelato,     FavouriteBrand = Brand.Prada,        FavouriteColor = Color.White   },
                new Smurf { Id = 25, Name = "Slate Voyager",     Age = 36, Job = Job.Accountant,                 FavouriteFood = Food.Juice,      FavouriteBrand = Brand.Hermes,        FavouriteColor = Color.Blue    },
                new Smurf { Id = 26, Name = "Coral Artisan",     Age = 29, Job = Job.TaxAdvisor,                 FavouriteFood = Food.Nuts,       FavouriteBrand = Brand.Mango,         FavouriteColor = Color.Pink    },
                new Smurf { Id = 27, Name = "Topaz Scout",       Age = 48, Job = Job.FinancialAnalyst,           FavouriteFood = Food.Curry,      FavouriteBrand = Brand.Patagonia,     FavouriteColor = Color.Yellow  },
                new Smurf { Id = 28, Name = "Aurora Weaver",     Age = 26, Job = Job.Translator,                 FavouriteFood = Food.Pita,       FavouriteBrand = Brand.Zara,          FavouriteColor = Color.Cyan    },
                new Smurf { Id = 29, Name = "Nova Dreamer",      Age = 39, Job = Job.Interpreter,                FavouriteFood = Food.Milkshake,  FavouriteBrand = Brand.Dior,          FavouriteColor = Color.Magenta },
                new Smurf { Id = 30, Name = "Echo Whisperer",    Age = 33, Job = Job.NGOAdvocate,                FavouriteFood = Food.Butter,     FavouriteBrand = Brand.Burberry,      FavouriteColor = Color.Brown   },
                new Smurf { Id = 31, Name = "Obsidian Echo",    Age = 37, Job = Job.EmergencyMedicalTechnician, FavouriteFood = Food.Juice,          FavouriteBrand = Brand.Puma,       FavouriteColor = Color.Black  },
                new Smurf { Id = 32, Name = "Citrine Melody",   Age = 22, Job = Job.MusicProducer,              FavouriteFood = Food.Cake,           FavouriteBrand = Brand.Supreme,    FavouriteColor = Color.Yellow },
                new Smurf { Id = 33, Name = "Sapphire Blaze",   Age = 45, Job = Job.Surgeon,                    FavouriteFood = Food.Soup,           FavouriteBrand = Brand.Prada,      FavouriteColor = Color.Blue   },
                new Smurf { Id = 34, Name = "Meadow Whisper",   Age = 28, Job = Job.Forester,                   FavouriteFood = Food.Salad,          FavouriteBrand = Brand.Patagonia,   FavouriteColor = Color.Green  },
                new Smurf { Id = 35, Name = "Coral Nexus",      Age = 33, Job = Job.NetworkEngineer,            FavouriteFood = Food.Taco,           FavouriteBrand = Brand.RayBan,     FavouriteColor = Color.Orange },
                new Smurf { Id = 36, Name = "Auric Spark",      Age = 40, Job = Job.ChemicalEngineer,           FavouriteFood = Food.Oats,           FavouriteBrand = Brand.Hermes,      FavouriteColor = Color.Yellow },
                new Smurf { Id = 37, Name = "Glacial Tide",     Age = 29, Job = Job.Biologist,                  FavouriteFood = Food.Salmon,         FavouriteBrand = Brand.Nike,        FavouriteColor = Color.Cyan   },
                new Smurf { Id = 38, Name = "Vermilion Forge",  Age = 52, Job = Job.MaintenanceTechnician,      FavouriteFood = Food.Bacon,          FavouriteBrand = Brand.Coach,       FavouriteColor = Color.Red    },
                new Smurf { Id = 39, Name = "Twilight Scholar", Age = 31, Job = Job.UniversityProfessor,         FavouriteFood = Food.Pie,            FavouriteBrand = Brand.Chanel,      FavouriteColor = Color.Purple },
                new Smurf { Id = 40, Name = "Zephyr Drift",     Age = 26, Job = Job.Pilot,                      FavouriteFood = Food.Bread,          FavouriteBrand = Brand.Uniqlo,      FavouriteColor = Color.Blue   },
                new Smurf { Id = 41, Name = "Maroon Matrix",    Age = 48, Job = Job.DataScientist,             FavouriteFood = Food.Gelato,         FavouriteBrand = Brand.NewBalance,  FavouriteColor = Color.Maroon },
                new Smurf { Id = 42, Name = "Ivory Nexus",      Age = 38, Job = Job.CADTechnician,             FavouriteFood = Food.CottageCheese,  FavouriteBrand = Brand.Diesel,      FavouriteColor = Color.White  },
                new Smurf { Id = 43, Name = "Obsidian Pulse",   Age = 44, Job = Job.CybersecuritySpecialist,   FavouriteFood = Food.Chips,          FavouriteBrand = Brand.Timberland,  FavouriteColor = Color.Black  },
                new Smurf { Id = 44, Name = "Emerald Gale",     Age = 27, Job = Job.EnvironmentalEngineer,     FavouriteFood = Food.OliveOil,       FavouriteBrand = Brand.Patagonia,   FavouriteColor = Color.Green  },
                new Smurf { Id = 45, Name = "Crimson Tales",    Age = 36, Job = Job.Journalist,                 FavouriteFood = Food.Croissant,      FavouriteBrand = Brand.Burberry,    FavouriteColor = Color.Red    },
                new Smurf { Id = 46, Name = "Ochre Oracle",        Age = 43, Job = Job.GeneralPractitioner,         FavouriteFood = Food.Quinoa,        FavouriteBrand = Brand.Puma,          FavouriteColor = Color.Orange  },
                new Smurf { Id = 47, Name = "Cerulean Coder",      Age = 29, Job = Job.MobileAppDeveloper,           FavouriteFood = Food.Tortilla,      FavouriteBrand = Brand.Uniqlo,        FavouriteColor = Color.Blue    },
                new Smurf { Id = 48, Name = "Vermilion Vanguard",   Age = 51, Job = Job.CivilEngineer,                FavouriteFood = Food.Pasta,         FavouriteBrand = Brand.Versace,       FavouriteColor = Color.Red     },
                new Smurf { Id = 49, Name = "Amethyst Analyst",     Age = 37, Job = Job.FinancialAnalyst,             FavouriteFood = Food.Smoothie,      FavouriteBrand = Brand.RayBan,        FavouriteColor = Color.Purple  },
                new Smurf { Id = 50, Name = "Jade Juggler",         Age = 26, Job = Job.EventPlanner,                 FavouriteFood = Food.Popcorn,       FavouriteBrand = Brand.Adidas,        FavouriteColor = Color.Green   },
                new Smurf { Id = 51, Name = "Bronze Botanist",      Age = 33, Job = Job.Biologist,                    FavouriteFood = Food.Salad,         FavouriteBrand = Brand.Patagonia,     FavouriteColor = Color.Brown   },
                new Smurf { Id = 52, Name = "Platinum Pilot",       Age = 42, Job = Job.Pilot,                        FavouriteFood = Food.Sandwich,      FavouriteBrand = Brand.Nike,          FavouriteColor = Color.White   },
                new Smurf { Id = 53, Name = "Graphite Guardian",    Age = 48, Job = Job.CybersecuritySpecialist,      FavouriteFood = Food.Chips,         FavouriteBrand = Brand.Supreme,       FavouriteColor = Color.Black   },
                new Smurf { Id = 54, Name = "Ivory Illustrator",    Age = 31, Job = Job.Illustrator,                  FavouriteFood = Food.Cupcake,       FavouriteBrand = Brand.Chanel,        FavouriteColor = Color.Gray    },
                new Smurf { Id = 55, Name = "Coral Chemist",        Age = 39, Job = Job.ChemicalEngineer,             FavouriteFood = Food.Cheese,        FavouriteBrand = Brand.Gucci,         FavouriteColor = Color.Pink    },
                new Smurf { Id = 56, Name = "Sable Steward",        Age = 46, Job = Job.HotelReceptionist,            FavouriteFood = Food.Coffee,        FavouriteBrand = Brand.Burberry,      FavouriteColor = Color.Black   },
                new Smurf { Id = 57, Name = "Lilac Luminary",       Age = 28, Job = Job.TeachingAssistant,            FavouriteFood = Food.IceCream,      FavouriteBrand = Brand.Zara,          FavouriteColor = Color.Lime    },
                new Smurf { Id = 58, Name = "Marigold Mechanic",    Age = 35, Job = Job.MaintenanceTechnician,         FavouriteFood = Food.Bacon,         FavouriteBrand = Brand.Reebok,        FavouriteColor = Color.Yellow  },
                new Smurf { Id = 59, Name = "Azure Archivist",      Age = 41, Job = Job.Actor,                   FavouriteFood = Food.Banana,       FavouriteBrand = Brand.RalphLauren,   FavouriteColor = Color.Blue    },
                new Smurf { Id = 60, Name = "Pearl Protector",      Age = 30, Job = Job.SecurityGuard,                FavouriteFood = Food.Nuts,          FavouriteBrand = Brand.Timberland,    FavouriteColor = Color.Cyan    }
            };
            context.Smurfs.AddRange(smurfs);
            context.SaveChanges();

            var rnd = new Random();

       

            // Helper dictionary-ek az aktuális foglaltság (occupancy) nyomon követésére
            var houseOccupancy = houses.ToDictionary(h => h, h => h.Residents.Count);
            var workOccupancy = workplaces.ToDictionary(w => w, w => w.Employees.Count);
            var venueOccupancy = venues.ToDictionary(v => v, v => v.Members.Count);

            // 1) HÁZAK: csak olyan színű ház, ami nem egyezik a törp kedvenc színével, és még van szabad hely
            var houseOptions = smurfs.ToDictionary(
                s => s,
                s => houses
                      .Where(h => h.Color != s.FavouriteColor && houseOccupancy[h] < h.Capacity)
                      .ToList()
            );

            foreach (var smurf in houseOptions
                                .OrderBy(x => x.Value.Count)    // kevesebb opciójú törpök először
                                .Select(x => x.Key))
            {
                var opts = houseOptions[smurf]
                           .Where(h => houseOccupancy[h] < h.Capacity)
                           .OrderBy(h => houseOccupancy[h])       // legkisebb occupancy előnyben
                           .ThenBy(_ => rnd.Next())               // ha tie, véletlen
                           .ToList();
                if (opts.Any())
                {
                    var chosen = opts.First();
                    chosen.Residents.Add(smurf);
                    houseOccupancy[chosen]++;
                }
            }

            // 2) MUNKAHELYEK: csak ahol elfogadják a törp szakmáját
            var workOptions = smurfs.ToDictionary(
                s => s,
                s => workplaces
                      .Where(w => w.AcceptedJobs.Contains(s.Job))
                      .ToList()
            );

            foreach (var smurf in workOptions
                                .OrderBy(x => x.Value.Count)
                                .Select(x => x.Key))
            {
                var opts = workOptions[smurf]
                           .OrderBy(w => workOccupancy[w])
                           .ThenBy(_ => rnd.Next())
                           .ToList();
                if (opts.Any())
                {
                    var chosen = opts.First();
                    chosen.Employees.Add(smurf);
                    workOccupancy[chosen]++;
                }
            }

            // 3) SZÓRAKOZÓHELYEK: csak ahol a kedvenc márka megegyezik az elfogadottal, és van még kapacitás
            var venueOptions = smurfs.ToDictionary(
                s => s,
                s => venues
                      .Where(v => v.AcceptedBrand == s.FavouriteBrand && venueOccupancy[v] < v.Capacity)
                      .ToList()
            );

            foreach (var smurf in venueOptions
                                .OrderBy(x => x.Value.Count)
                                .Select(x => x.Key))
            {
                var opts = venueOptions[smurf]
                           .Where(v => venueOccupancy[v] < v.Capacity)
                           .OrderBy(v => venueOccupancy[v])
                           .ThenBy(_ => rnd.Next())
                           .ToList();
                if (opts.Any())
                {
                    var chosen = opts.First();
                    chosen.Members.Add(smurf);
                    venueOccupancy[chosen]++;
                }
            }

            // Végül mentjük a változásokat
            context.SaveChanges();
        }
    }
}
