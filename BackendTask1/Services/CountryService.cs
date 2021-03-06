﻿namespace BackendTask1
{
    using System.Collections.Generic;
    using System.Linq;
    using BackendTask1.Dtos;
    using BackendTask1.EntityModels;
    using BackendTask1.Interfaces.Country;

    public class CountryService : IService
    {
        public List<T> ParsingResult<T, T2>(List<T2> data) where T : Entity where T2 : class
        {
            var parsedData = data as List<CountryDto>;

            if (parsedData == null)
            {
                throw new System.Exception("Can't parse data");
            }

            var countries = new List<CountryEntity>();

            using (var context = new CountryContext())
            {
                var regions = parsedData.Select(x => x.Region.Value);
                var incomeLevels = parsedData.Select(x => x.IncomeLevel.Value);
                var adminregions = parsedData.Select(x => x.Adminregion.Value);
                var lendingTypes = parsedData.Select(x => x.LendingType.Value);

                var values = regions.Concat(incomeLevels.Concat(adminregions.Concat(lendingTypes))).Distinct().ToList();

                var allRegions = context.Regions
                    .Where(x => values.Contains(x.Value))
                    .ToList();

                foreach (var country in countries)
                {
                    var adminregion = allRegions
                        .FirstOrDefault(x => x.Value == country.Adminregion.Value);

                    var incomeLevel = allRegions
                        .FirstOrDefault(x => x.Value == country.IncomeLevel.Value);

                    var region = allRegions
                        .FirstOrDefault(x => x.Value == country.Region.Value);

                    var lendingType = allRegions
                        .FirstOrDefault(x => x.Value == country.LendingType.Value);

                    var newCountry = new CountryEntity
                    {
                        Adminregion = adminregion,
                        IncomeLevel = incomeLevel,
                        Region = region,
                        LendingType = lendingType,
                        CapitalCity = country.CapitalCity,
                        Iso2Code = country.Iso2Code,
                        Latitude = country.Latitude,
                        Longitude = country.Longitude,
                        Name = country.Name
                    };

                    countries.Add(newCountry);
                }
            }

            return countries as List<T>;
        }

        public void Save<T>(List<T> entities) where T : Entity
        {
            var saveEntityList = entities as List<CountryEntity>;
            if (saveEntityList == null)
            {
                throw new System.Exception("Can't save empty entity");
            }

            using (var context = new CountryContext())
            {
                foreach (var saveEntity in saveEntityList)
                {
                    context.Countries.Add(saveEntity);
                }

                context.SaveChanges();
            }
        }

        public void Save<T>(T entity) where T : Entity
        {
            var saveEntity = entity as CountryEntity;
            if (saveEntity == null)
            {
                throw new System.Exception("Can't save empty entity");
            }

            using (var context = new CountryContext())
            {
                context.Countries.Add(saveEntity);
                context.SaveChanges();
            }
        }
    }
}
