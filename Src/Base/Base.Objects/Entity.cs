using System.Diagnostics;

namespace Base.Objects
{
    /// <summary>
    /// Базовый класс для всех объектов системы
    /// </summary>
    [DebuggerDisplay("Id={Id} Deleted={IsDeleted}")]
    public abstract class Entity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Id пользователя, создавшего объект
        /// </summary>
        public string CreatedByUserID { get; set; }
        /// <summary>
        /// Дата создания сущности
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Id пользователя, сделавшего последнее обновление
        /// </summary>
        public string UpdatedByUserId { get; set; }
        /// <summary>
        /// Дата изменения сущности
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Флаг удалено
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}