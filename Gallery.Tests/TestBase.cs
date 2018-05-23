using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL;
using IW5Gallery.DAL.Entities;

namespace Gallery.Tests
{
    public class TestBase : IDisposable
    {
        public TestBase()
        {
            using (var context = new GalleryContext())
            {
                // ********** Images ************
                var images = new List<Image>
                {
                    new Image() {Id = new Guid("1abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(1)jpg", Path = "../Images/(1).jpg", ThumbnailPath = "../Images/(1).jpg"},
                    new Image() {Id = new Guid("2abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(1)png", Path = "../Images/(1).png", ThumbnailPath = "../Images/(1).png"},
                    new Image() {Id = new Guid("3abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(2)jpg", Path = "../Images/(2).jpg", ThumbnailPath = "../Images/(2).jpg"},
                    new Image() {Id = new Guid("4abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(2)png", Path = "../Images/(2).png", ThumbnailPath = "../Images/(2).png"},
                    new Image() {Id = new Guid("5abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(3)jpg", Path = "../Images/(3).jpg", ThumbnailPath = "../Images/(3).jpg"},
                    new Image() {Id = new Guid("6abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(3)png", Path = "../Images/(3).png", ThumbnailPath = "../Images/(3).png"},
                    new Image() {Id = new Guid("7abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(4)jpg", Path = "../Images/(4).jpg", ThumbnailPath = "../Images/(4).jpg"},
                    new Image() {Id = new Guid("8abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(4)png", Path = "../Images/(4).png", ThumbnailPath = "../Images/(4).png"},
                    new Image() {Id = new Guid("9abdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(5)jpg", Path = "../Images/(5).jpg", ThumbnailPath = "../Images/(5).jpg"},
                    new Image() {Id = new Guid("aabdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(5)png", Path = "../Images/(5).png", ThumbnailPath = "../Images/(5).png"},
                    new Image() {Id = new Guid("babdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(6)jpg", Path = "../Images/(6).jpg", ThumbnailPath = "../Images/(6).jpg"},
                    new Image() {Id = new Guid("cabdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(6)png", Path = "../Images/(6).png", ThumbnailPath = "../Images/(6).png"},
                    new Image() {Id = new Guid("dabdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(7)jpg", Path = "../Images/(7).jpg", ThumbnailPath = "../Images/(7).jpg"},
                    new Image() {Id = new Guid("eabdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-7), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(7)png", Path = "../Images/(7).png", ThumbnailPath = "../Images/(7).png"},
                    new Image() {Id = new Guid("fabdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(8)jpg", Path = "../Images/(8).jpg", ThumbnailPath = "../Images/(8).jpg"},
                    new Image() {Id = new Guid("11bdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(8)png", Path = "../Images/(8).png", ThumbnailPath = "../Images/(8).png"},
                    new Image() {Id = new Guid("12bdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.jpg, Height = 1920, Width = 1080, Name = "(9)jpg", Path = "../Images/(9).jpg", ThumbnailPath = "../Images/(9).jpg"},
                    new Image() {Id = new Guid("13bdfee1-c970-4afd-aff8-aa3cfef8b1ac"), DateAdded = DateTime.Now.AddDays(-1), DateTaken = DateTime.Now.AddDays(-2), Format = Format.png, Height = 1920, Width = 1080, Name = "(9)png", Path = "../Images/(9).png", ThumbnailPath = "../Images/(9).png"}
                };

                images.ForEach(s => context.Images.AddOrUpdate(s));

                // ********** ALBUMS ************

                var oneAlbumId = new Guid("ab8db9b3-799c-4ef2-9d85-ce32a9ffa843");
                var secondAlbumId = new Guid("bb8db9b3-799c-4ef2-9d85-ce32a9ffa843");
                var thirdAlbumId = new Guid("cb8db9b3-799c-4ef2-9d85-ce32a9ffa843");
                var forthAlbumId = new Guid("db8db9b3-799c-4ef2-9d85-ce32a9ffa843");
                var fifthAlbumId = new Guid("eb8db9b3-799c-4ef2-9d85-ce32a9ffa843");
                context.Albums.AddOrUpdate(
                    r => r.Id,
                    new Album()
                    {
                        Id = oneAlbumId,
                        Name = "Top Kek Album",
                        Images = new List<AlbumImage>()
                        {
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[0].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[2].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[4].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[6].Id
                        },
                        }
                    },
                    new Album()
                    {
                        Id = secondAlbumId,
                        Name = "The Kekiest Album",
                        Images = new List<AlbumImage>()
                        {
                        new AlbumImage()
                        {
                            AlbumId = secondAlbumId,
                            ImageId = images[0].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = secondAlbumId,
                            ImageId = images[1].Id
                        }
                        }
                    },
                    new Album()
                    {
                        Id = thirdAlbumId,
                        Name = "Best of Pepe 1997",
                        Images = new List<AlbumImage>()
                        {
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[2].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[3].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[5].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[6].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[9].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[7].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[1].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[8].Id
                        },
                        }
                    },
                    new Album()
                    {
                        Id = forthAlbumId,
                        Name = "Average Pepe",
                        Images = new List<AlbumImage>()
                        {
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[3].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[4].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[6].Id
                        },
                        new AlbumImage()
                        {
                            AlbumId = oneAlbumId,
                            ImageId = images[8].Id
                        },
                        },
                        CoverPhotoId = images[0].Id
                    },
                    new Album()
                    {
                        Name = "Album to be",
                        Id = fifthAlbumId,

                    }
                    );

                // ********** Tags ************
                var person1Id = new Guid("7a3aaaaa-799c-4ef2-9d85-ce32a9ffa843");
                var person2Id = new Guid("7a3baaaa-799c-4ef2-9d85-ce32a9ffa843");

                context.Persons.AddOrUpdate(new Person()
                {
                    Name = "Pepe",
                    Surname = "Master",
                    Id = person1Id,
                    Tags = new List<Tag>()
                {
                    new Tag()
                    {
                        ImageId = images[0].Id,
                        Location = new Location()
                        {
                            Height = 200,
                            Width = 200,
                            XCoordinate = 500,
                            YCoordinate = 500,
                            Id = new Guid("10caaaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = person1Id,
                        TagType = TagType.Person
                    },
                    new Tag()
                    {
                        ImageId = images[1].Id,
                        Location = new Location()
                        {
                            Height = 40,
                            Width = 230,
                            XCoordinate = 200,
                            YCoordinate = 200,
                            Id = new Guid("10cbaaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = person1Id,
                        TagType = TagType.Person
                    },
                    new Tag()
                    {
                        ImageId = images[2].Id,
                        Location = new Location()
                        {
                            Height = 20,
                            Width = 20,
                            XCoordinate = 750,
                            YCoordinate = 600,
                            Id = new Guid("10ccaaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = person1Id,
                        TagType = TagType.Person
                    },
                }
                });

                context.Persons.AddOrUpdate(new Person()
                {
                    Name = "Alfonz",
                    Surname = "Muricz",
                    Id = person2Id,
                    Tags = new List<Tag>()
                {
                    new Tag()
                    {
                        ImageId = images[0].Id,
                        Location = new Location()
                        {
                            Height = 20,
                            Width = 20,
                            XCoordinate = 750,
                            YCoordinate = 600,
                            Id = new Guid("10cdaaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = person2Id,
                        TagType = TagType.Person
                    },
                    new Tag()
                    {
                        ImageId = images[3].Id,
                        Location = new Location()
                        {
                            Height = 80,
                            Width = 80,
                            XCoordinate = 200,
                            YCoordinate = 200,
                            Id = new Guid("10ceaaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = person2Id,
                        TagType = TagType.Person
                    },
                }
                });

                var thing1Id = new Guid("7a3caaaa-799c-4ef2-9d85-ce32a9ffa843");
                var thing2Id = new Guid("7a3daaaa-799c-4ef2-9d85-ce32a9ffa843");
                var thing3Id = new Guid("7a3eaaaa-799c-4ef2-9d85-ce32a9ffa843");
                context.Things.AddOrUpdate(new Thing()
                {
                    Name = "Zabak",
                    Id = thing1Id,
                    Tags = new List<Tag>()
                {
                    new Tag()
                    {
                        ImageId = images[0].Id,
                        Location = new Location()
                        {
                            Height = 25,
                            Width = 20,
                            XCoordinate = 450,
                            YCoordinate = 600,
                            Id = new Guid("10cfaaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing1Id,
                        TagType = TagType.Thing
                    },
                    new Tag()
                    {
                        ImageId = images[1].Id,
                        Location = new Location()
                        {
                            Height = 50,
                            Width = 50,
                            XCoordinate = 250,
                            YCoordinate = 200,
                            Id = new Guid("10cabaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing1Id,
                        TagType = TagType.Thing
                    },
                    new Tag()
                    {
                        ImageId = images[1].Id,
                        Location = new Location()
                        {
                            Height = 50,
                            Width = 20,
                            XCoordinate = 350,
                            YCoordinate = 300,
                            Id = new Guid("10cacaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing1Id,
                        TagType = TagType.Thing
                    },
                    new Tag()
                    {
                        ImageId = images[3].Id,
                        Location = new Location()
                        {
                            Height = 80,
                            Width = 80,
                            XCoordinate = 200,
                            YCoordinate = 200,
                            Id = new Guid("10cadaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing1Id,
                        TagType = TagType.Thing
                    },
                }
                });

                context.Things.AddOrUpdate(new Thing()
                {
                    Name = "Swag tricko",
                    Id = thing2Id,
                    Tags = new List<Tag>()
                {
                    new Tag()
                    {
                        ImageId = images[5].Id,
                        Location = new Location()
                        {
                            Height = 80,
                            Width = 70,
                            XCoordinate = 450,
                            YCoordinate = 600,
                            Id = new Guid("10caeaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing2Id,
                        TagType = TagType.Thing
                    },
                    new Tag()
                    {
                        ImageId = images[6].Id,
                        Location = new Location()
                        {
                            Height = 50,
                            Width = 50,
                            XCoordinate = 250,
                            YCoordinate = 200,
                            Id = new Guid("10cafaaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing2Id,
                        TagType = TagType.Thing
                    },
                }
                });

                context.Things.AddOrUpdate(new Thing()
                {
                    Name = "Siltovka",
                    Id = thing3Id,
                    Tags = new List<Tag>()
                {
                    new Tag()
                    {
                        ImageId = images[5].Id,
                        Location = new Location()
                        {
                            Height = 25,
                            Width = 20,
                            XCoordinate = 550,
                            YCoordinate = 610,
                            Id = new Guid("10caabaa-799c-4ef2-9d85-ce32a9ffa843")
                        },
                        TaggableId = thing3Id,
                        TagType = TagType.Thing
                    },
                }
                });

                context.SaveChanges();
            }
        }
        public void Dispose()
        {
            using (var context = new GalleryContext())
            {
                // ********* CleanUp ************
                context.AlbumImages.ToList().ForEach(a => context.AlbumImages.Remove(a));
                context.Albums.ToList().ForEach(a => context.Albums.Remove(a));
                context.Tags.ToList().ForEach(t => context.Tags.Remove(t));
                context.Locations.ToList().ForEach(l => context.Locations.Remove(l));
                context.Persons.ToList().ForEach(p => context.Persons.Remove(p));
                context.Things.ToList().ForEach(t => context.Things.Remove(t));
                context.Images.ToList().ForEach(i => context.Images.Remove(i));

                context.SaveChanges();
            }
        }
    }
}
