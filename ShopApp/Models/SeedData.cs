using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ShopApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShopAppContext(
                serviceProvider.GetRequiredService<DbContextOptions<ShopAppContext>>()))
            {
                // Look for any movies.
                if (context.Notebook.Any())
                {
                    return;   // DB has been seeded
                }

                context.Notebook.AddRange(
                  /* new Notebook
                   {
                       Name = "Asus",
                       GPU = "AMD",
                       RAM = 123,
                       Processor = "test",
                       ScreenSizeInch = 13,
                       //ImageData = new byte[0xD35D35D35D35],
                       Length = 1,
                       Width = 1,
                       Height = 1
                   },

                    new Notebook
                    {
                        Name = "Lenovo",
                        GPU = "AMD",
                        RAM = 123,
                        Processor = "test",
                        ScreenSizeInch = 13,
                        //ImageData = new byte[0xD35D35D35D35],
                        Length = 1,
                        Width = 1,
                        Height = 1
                    },

                    new Notebook
                    {
                        Name = "Acer",
                        GPU = "NVidia",
                        RAM = 123,
                        Processor = "test",
                        ScreenSizeInch = 15,
                        //ImageData = new byte[0xD35D35D35D35],
                        Length = 1,
                        Width = 1,
                        Height = 1
                    },*/

                    new Notebook
                    {
                        Name = "MSI",
                        GPU = "NVidia",
                        RAM = 1233,
                        Processor = "test2",
                        ScreenSizeInch = 16,
                        //ImageData = new byte[0xD35D35D35D353],
                        //Length = 2,
                        //Width = 2,
                        //Height = 2
                    }

                );
                context.SaveChanges();
            }
        }
    }
}