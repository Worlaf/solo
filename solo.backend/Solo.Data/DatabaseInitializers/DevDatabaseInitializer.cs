using System;
using System.Collections.Generic;
using System.Text;
using Solo.Common.Extensions;
using Solo.Data.Infrastructure;
using Solo.Data.Repositories;
using Solo.Domain;
using Solo.Domain.Entities;
using Solo.Domain.Map;

namespace Solo.Data.DatabaseInitializers
{
    public class DevDatabaseInitializer
    {
        private ParkRepository _parkRepository;
        private ParkObjectRepository _parkObjectRepository;
        private IUnitOfWork _unitOfWork;
        private SoloDbContext _soloDbContext;
        private UserRepository _userRepository;

        public DevDatabaseInitializer(ParkRepository parkRepository, ParkObjectRepository parkObjectRepository, IUnitOfWork unitOfWork, SoloDbContext soloDbContext, UserRepository userRepository)
        {
            _parkRepository = parkRepository;
            _parkObjectRepository = parkObjectRepository;
            _unitOfWork = unitOfWork;
            _soloDbContext = soloDbContext;
            _userRepository = userRepository;
        }

        public void Initialize()
        {
            _soloDbContext.Database.EnsureCreated();

            _userRepository.Save(new User
            {
                AuthId = "Admin",
                HasPrivileges = true,
                Permissions = (int)(Permissions.ParkObjectManagement | Permissions.Communication)
            });

            var testPark = new Park
            {
                Name = "Липецкий нижний парк",
                ImageUrl = "https://i.imgur.com/D0U9aFG.png",
                RegionJson = new Region
                {
                    Points = new[]
                    {
                        new Point {Latitude = 52.605316M, Longitude = 39.597577M},
                        new Point {Latitude = 52.606938M, Longitude = 39.598677M},
                        new Point {Latitude = 52.608401M, Longitude = 39.605705M},
                        new Point {Latitude = 52.606242M, Longitude = 39.611984M},
                        new Point {Latitude = 52.604160M, Longitude = 39.614474M},
                        new Point {Latitude = 52.602648M, Longitude = 39.611930M}
                    }
                }.ToJson()
            };
            _parkRepository.Save(testPark);

            _unitOfWork.Commit();

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                AdministrationDescriptionMarkdown = "Test Description",
                Name = "Автодром",
                PublicDescriptionMarkdown = "Прокатись с ветерком!",
                Location = Point.FromStrangeCoord(56.244280M, 93.523656M)
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Колесо обзора",
                ImageUrl = "https://i.imgur.com/x1A9IiO.jpg",
                Location = new Point(52.60456049456933M, 39.60562761347504M),
                Type = ObjectType.Attraction,
            });

            _unitOfWork.Commit();
        }
    }
}