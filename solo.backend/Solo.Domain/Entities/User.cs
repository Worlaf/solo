using System;

namespace Solo.Domain.Entities
{
    public class User : EntityBase
    {
        public string AuthId { get; set; }
        public int Permissions { get; set; }
        public bool HasPrivileges { get; set; } // todo: льготы, возможно заменить на набор флагов, если планируются разные условия для разных групп льготников
    }

    // todo: на данный момент пермишены распространяются на все парки, логично что нужно ввести сущность Permissions-Parks
    [Flags]
    public enum Permissions
    {
        ParkObjectManagement = 0b00000001,
        Communication = 0b00000010,
    }
}
