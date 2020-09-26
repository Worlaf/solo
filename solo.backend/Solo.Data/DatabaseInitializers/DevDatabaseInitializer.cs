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
                Name = "Парк культуры и отдыха им. Кирова",
                ImageUrl = "https://i.imgur.com/D0U9aFG.png",
                RegionJson = new Region
                {
                    Points = new[]
                    {
                        new Point {Latitude = 56.248128M, Longitude = 93.514754M},
                        new Point {Latitude = 56.238501M, Longitude = 93.524274M},
                        new Point {Latitude = 56.240517M, Longitude = 93.539638M},
                        new Point {Latitude = 56.241828M, Longitude = 93.540003M},
                    }
                }.ToJson()
            };
            _parkRepository.Save(testPark);

            _unitOfWork.Commit();

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name = "Веселое путешествие",
                Type = ObjectType.Attraction,
                AdministrationDescriptionMarkdown = @"Название аттракциона ""Веселое путешествие""
Модель аттракциона ""РЖД-2020""
Дата ввода в экплуатацию ""12.08.1987""
Период проведения регламента ""6 месяцев""
Дата проведния регламента "" 13.06.2020""
Отвественный за эксплуатацию ""Главный инженер Петров В.П.""
Состояние обекта ""В рабочем сотоянии""",
                PublicDescriptionMarkdown = "Описание объекта\r\nПокрутись на цепях %))\r\n\r\nОграничения\r\nДетям до 5 лет\r\nЛицам в состоянии алкашки\r\nБез животных\r\n\r\nПравила безопасности на атракционах.\r\n1.\r\n2.\r\n3.\r\n\r\nВремя работы.\r\nВ выходные и праздничные дни\r\n10.00-17.00\r\n\r\nЦена\r\nДети -100\r\nВзрослый -130\r\n\r\nЦена со скидкой\r\n\r\n\r\nЛьготы\r\nМногодетные семьи\r\nДети детского дома, дети-инвалиды\r\nГерои и полные кавалеры ордена славы",
                Location = Point.FromStrangeCoord(56.244447M, 93.524815M)
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Колесо обзора",
                AdministrationDescriptionMarkdown = "Название аттракциона \"Колесо обзора\"\r\nМодель аттракциона \"Круговерть-ГТ2000\"\r\nДата ввода в экплуатацию \"12.08.1987\"\r\nПериод проведения регламента \"6 месяцев\"\r\nДата проведния регламента \" 13.06.2020\"\r\nОтвественный за эксплуатацию \"Главный инженер Петров В.П.\"\r\nСостояние обекта \"В рабочем сотоянии\"",
                ImageUrl = "https://i.imgur.com/x1A9IiO.jpg",
                Location = new Point(56.244223M, 93.524257M),
                Type = ObjectType.Attraction,
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Беседка над озером",
                PublicDescriptionMarkdown = "Если вы хотите посидеть в тишине с прекрасным видом на пропалывающие в дали парусники",
                Location = new Point(56.244048M, 93.522333M),
                Type = ObjectType.Sight,
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Домик для белки",
                PublicDescriptionMarkdown = "Не обижайте наших белочек. Они очень довречевые.\r\nВы можете покормить их. Просто положите еду в их домики.\r\n\r\nЧем кормить белок.\r\nБелый хлеб.\r\nГрецкий орех.\r\nСухие грибы.\r\nПеченьки.\r\nСемки.\r\nСухарики к пиву.\r\nСухофрукты \r\nКедровый орех.\r\n\r\nВнимание!\r\nНе берите белок в руки.",
                Location = new Point(56.243929M, 93.527559M),
                Type = ObjectType.Sight,
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Домик для белки",
                PublicDescriptionMarkdown = "Не обижайте наших белочек. Они очень довречевые.\r\nВы можете покормить их. Просто положите еду в их домики.\r\n\r\nЧем кормить белок.\r\nБелый хлеб.\r\nГрецкий орех.\r\nСухие грибы.\r\nПеченьки.\r\nСемки.\r\nСухарики к пиву.\r\nСухофрукты \r\nКедровый орех.\r\n\r\nВнимание!\r\nНе берите белок в руки.",
                Location = new Point(56.241359M, 93.527079M),
                Type = ObjectType.Sight,
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Домик для белки",
                PublicDescriptionMarkdown = "Не обижайте наших белочек. Они очень довречевые.\r\nВы можете покормить их. Просто положите еду в их домики.\r\n\r\nЧем кормить белок.\r\nБелый хлеб.\r\nГрецкий орех.\r\nСухие грибы.\r\nПеченьки.\r\nСемки.\r\nСухарики к пиву.\r\nСухофрукты \r\nКедровый орех.\r\n\r\nВнимание!\r\nНе берите белок в руки.",
                Location = new Point(56.242846M, 93.526488M),
                Type = ObjectType.Sight,
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Домик для белки",
                PublicDescriptionMarkdown = "Не обижайте наших белочек. Они очень довречевые.\r\nВы можете покормить их. Просто положите еду в их домики.\r\n\r\nЧем кормить белок.\r\nБелый хлеб.\r\nГрецкий орех.\r\nСухие грибы.\r\nПеченьки.\r\nСемки.\r\nСухарики к пиву.\r\nСухофрукты \r\nКедровый орех.\r\n\r\nВнимание!\r\nНе берите белок в руки.",
                Location = new Point(56.243666M, 93.525560M),
                Type = ObjectType.Sight,
            });

            _parkObjectRepository.Save(new ParkObject
            {
                ParkId = testPark.Id,
                Name="Coffe",
                Location = new Point(56.243806M, 93.527684M),
                Type = ObjectType.Food,
            });

            _unitOfWork.Commit();
        }
    }
}