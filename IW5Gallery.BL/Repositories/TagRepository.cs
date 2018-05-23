using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using IW5Gallery.BL.Models;
using IW5Gallery.DAL;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.BL.Repositories
{
    public class TagRepository
    {
        private readonly Mapper _mapper = new Mapper();

        public PersonDetailModel GetPersonById(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var personEntity = context.Persons.Include(p => p.Tags.Select(t => t.Image))
                    .FirstOrDefault(p => p.Id == id);

                return personEntity == null ? null : _mapper.MapPersonEntityToPersonDetailModel(personEntity);
            }
        }

        public bool ContainsPerson(string name, string surname)
        {
            using (var context = new GalleryContext())
            {
                var result = context.Persons.FirstOrDefault(x => x.Surname.Equals(surname) && x.Name.Equals(name));
                return result != null;
            }
        }

        public bool ContainsThing(string name)
        {
            using (var context = new GalleryContext())
            {
                var result = context.Things.FirstOrDefault(x => x.Name.Equals(name));
                var resultToReturn = result != null;
                return resultToReturn;
            }
        }

        public ThingDetailModel GetThingById(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var thingEntity = context.Things.Include(t => t.Tags.Select(tag => tag.Image))
                    .FirstOrDefault(t => t.Id == id);

                return thingEntity == null ? null : _mapper.MapThingEntityToThingDetailModel(thingEntity);
            }
        }

        public List<MiniatureModel> GetAllPersons()
        {
            using (var context = new GalleryContext())
            {
                return context.Persons.Select(_mapper.MapPersonEntityToMiniatureModel)
                    .OrderBy(p => p.Name).ToList();
            }
        }

        public List<MiniatureModel> GetAllThings()
        {
            using (var context = new GalleryContext())
            {
                return context.Things.Select(_mapper.MapThingEntityToMiniatureModel)
                    .OrderBy(t => t.Name).ToList();
            }
        }

        public List<MiniatureModel> GetPersonsByName(string name)
        {
            using (var context = new GalleryContext())
            {
                return context.Persons.Select(_mapper.MapPersonEntityToMiniatureModel)
                    .Where(p => p.Name.Contains(name)).ToList();
            }
        }

        public List<MiniatureModel> GetThingsByName(string name)
        {
            using (var context = new GalleryContext())
            {
                return context.Things.Select(_mapper.MapThingEntityToMiniatureModel)
                    .Where(p => p.Name.Contains(name)).ToList();
            }
        }

        public PersonDetailModel InsertPerson(PersonDetailModel detail)
        {
            using (var context = new GalleryContext())
            {
                var entity = _mapper.MapPersonDetailModelToPersonEntity(detail);
                entity.Id = Guid.NewGuid();

                context.Persons.Add(entity);
                context.SaveChanges();

                return _mapper.MapPersonEntityToPersonDetailModel(entity);
            }
        }

        public ThingDetailModel InsertThing(ThingDetailModel detail)
        {
            using (var context = new GalleryContext())
            {
                var entity = _mapper.MapThingDetailModelToThingEntity(detail);
                entity.Id = Guid.NewGuid();

                context.Things.Add(entity);
                context.SaveChanges();

                return _mapper.MapThingEntityToThingDetailModel(entity);
            }
        }

        public void RemovePerson(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var person = context.Persons.Include(p => p.Tags.Select(t => t.Location))
                    .FirstOrDefault(p => p.Id == id);
                if (person == null)
                    return;

                var locations = person.Tags.ToList().Select(t => t.Location).ToList();
                person.Tags.ToList().ForEach(t => context.Tags.Remove(t));
                locations.ForEach(l => context.Locations.Remove(l));

                context.Persons.Remove(person);
                context.SaveChanges();
            }
        }

        public void RemoveThing(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var thing = context.Things.Include(t => t.Tags.Select(tag => tag.Location))
                    .FirstOrDefault(t => t.Id == id);
                if (thing == null)
                    return;

                var locations = thing.Tags.ToList().Select(t => t.Location).ToList();
                thing.Tags.ToList().ForEach(t => context.Tags.Remove(t));
                locations.ForEach(l => context.Locations.Remove(l));

                context.Things.Remove(thing);
                context.SaveChanges();
            }
        }
    }
}