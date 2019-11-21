using System.ComponentModel;

namespace EventManager.API.Domain.Enums
{
    public enum ECenterType
    {
        [Description ("Classroom")]
        Classroom = 1,

        [Description ("Hall")]
        Hall = 2,

        [Description ("Theatre")]
        Theatre = 3,

        [Description ("Banquest")]
        Banquest = 4
    }
}
